using System.Net.Sockets;
using System.Text;
using System.Transactions;
using AutoMapper;
using Contracts.Dto;
using Contracts.Dto.TelegramBot;
using Contracts.Entities;
using DAL.UnitOfWork.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Services.Base;
using Services.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Services;

public class TelegramService : BaseService<TelegramService>, ITelegramService
{
    private readonly HttpClient _httpClient;

    public TelegramService(IMapper mapper, IUnitOfWorkFactory unitOfWorkFactory, ILogger<TelegramService> logger,
        HttpClient httpClient) :
        base(mapper, unitOfWorkFactory, logger)
    {
        _httpClient = httpClient;
    }

    public async Task ProcessMessageAsync(string botId, Update update)
    {
        if (update.Message is null)
            return;

        var unitOfWork = UnitOfWorkFactory.CreateSqlUnitOfWork(IsolationLevel.ReadUncommitted);
        var telegramBot = await unitOfWork.TelegramBotRepository
            .GetFirstOrDefaultAsync(b => b.Token != null && b.Token.Equals(botId)).ConfigureAwait(false);

        if (telegramBot == null)
            return;

        var command = await unitOfWork.CommandRepository
            .GetFirstOrDefaultAsync(c => c.Command == update.Message.Text && c.TelegramBotId == telegramBot.Id)
            .ConfigureAwait(false);

        if (command == null) return;

        var commandAction = await unitOfWork.CommandActionRepository
            .GetFirstOrDefaultAsync(ca => ca.Id == command.CommandActionId && ca.TelegramBotId == telegramBot.Id)
            .ConfigureAwait(false);

        if (commandAction == null) return;

        var bot = new TelegramBotClient(botId, _httpClient);

        switch (commandAction.CommandActionType)
        {
            case CommandActionType.Text:
            {
                if (commandAction.Content == null) break;
                var actionContent = JsonConvert.DeserializeObject<KeyboardMarkupDto<string>>(commandAction.Content);
                if (actionContent == null) break;

                await bot.SendTextMessageAsync(update.Message.Chat.Id, $"{actionContent.Content}")
                    .ConfigureAwait(false);
                break;
            }
            case CommandActionType.HttpPost:
            {
                if (commandAction.Content == null) break;
                var actionContent = JsonConvert.DeserializeObject<KeyboardMarkupDto<string>>(commandAction.Content);
                if (actionContent == null) break;

                var content = JsonConvert.SerializeObject(update);
                var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
                await _httpClient.PostAsync(actionContent.Content, httpContent).ConfigureAwait(false);
                break;
            }
            case CommandActionType.InlineKeyboard:
            case CommandActionType.ReplyKeyboard:
            default:
            {
                switch (commandAction)
                {
                    case { CommandActionType: CommandActionType.InlineKeyboard, Content: { } }:
                    {
                        var keyboardMarkup =
                            JsonConvert
                                .DeserializeObject<KeyboardMarkupDto<IEnumerable<IEnumerable<InlineKeyboardButton>>>>(
                                    commandAction.Content);

                        if (keyboardMarkup != null)
                        {
                            var inlineKeyboardMarkup = new InlineKeyboardMarkup(keyboardMarkup.Content);

                            await bot.SendTextMessageAsync(update.Message.Chat.Id, keyboardMarkup.Title,
                                replyMarkup: inlineKeyboardMarkup).ConfigureAwait(false);
                        }

                        break;
                    }
                    case { CommandActionType: CommandActionType.ReplyKeyboard, Content: { } }:
                    {
                        var keyboardMarkup =
                            JsonConvert.DeserializeObject<KeyboardMarkupDto<IEnumerable<IEnumerable<KeyboardButton>>>>(
                                commandAction.Content);

                        if (keyboardMarkup != null)
                        {
                            var replyKeyboardMarkup = new ReplyKeyboardMarkup(keyboardMarkup.Content);

                            await bot.SendTextMessageAsync(update.Message.Chat.Id, keyboardMarkup.Title,
                                replyMarkup: replyKeyboardMarkup).ConfigureAwait(false);
                        }

                        break;
                    }
                }

                break;
            }
        }
    }

    public async Task SetUpBotAsync(string botId)
    {
        var bot = new TelegramBotClient(botId, _httpClient);
        await bot.DeleteWebhookAsync().ConfigureAwait(false);

        var isBotAvailable = await bot.TestApiAsync().ConfigureAwait(false);

        if (!isBotAvailable) throw new ApiRequestException("Bot is unavailable");

        await bot.SetWebhookAsync($"https://hapan9-telegram.azurewebsites.net:443/api/Telegram/{botId}")
            .ConfigureAwait(false);
    }

    public async Task<List<TelegramBotInfoDto>> GetBotsInfoAsync(List<TelegramBot> telegramBots)
    {
        List<TelegramBotInfoDto> results = new();
        foreach (var telegramBot in telegramBots)
        {
            var bot = new TelegramBotClient(telegramBot.Token, _httpClient);
            var botInfo = await bot.GetMeAsync().ConfigureAwait(false);
            var photos = await bot.GetUserProfilePhotosAsync(botInfo.Id).ConfigureAwait(false);

            string? avatar;
            if (photos.TotalCount == 0)
            {
                avatar = null;
            }
            else
            {
                var file = await bot.GetFileAsync(photos.Photos.Last().Last().FileId).ConfigureAwait(false);
                avatar = $"https://api.telegram.org/file/bot{telegramBot.Token}/{file.FilePath}";
            }

            var result = Mapper.Map<TelegramBotInfoDto>(botInfo, opt =>
            {
                opt.AfterMap((_, dest) =>
                {
                    dest.Id = telegramBot.Id;
                    dest.Token = telegramBot.Token;
                    dest.IsActive = telegramBot.IsActive;
                    dest.UserId = telegramBot.UserId;
                    dest.Avatar = avatar;
                });
            });

            results.Add(result);
        }

        return results;
    }
}