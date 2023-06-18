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
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Services;

public sealed class TelegramService : BaseService<TelegramService>, ITelegramService
{
    private const string TelegramProcessingApiUrl = "https://hapan9-telegram.azurewebsites.net:443/api/Telegram/";
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
        await bot.SendChatActionAsync(update.Message.Chat.Id, ChatAction.Typing).ConfigureAwait(false);

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

                try
                {
                    var content = JsonConvert.SerializeObject(update.Message);
                    var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
                    await _httpClient.PostAsync(actionContent.Content, httpContent).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, ex.Message);
                }
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

    public Task<bool> IsBotAvailableAsync(string botId)
    {
        var bot = new TelegramBotClient(botId, _httpClient);

        return bot.TestApiAsync();
    }

    public async Task SetWebHookAsync(string botId)
    {
        var bot = new TelegramBotClient(botId, _httpClient);
        await bot.DeleteWebhookAsync().ConfigureAwait(false);

        await bot.SetWebhookAsync($"{TelegramProcessingApiUrl}{botId}")
            .ConfigureAwait(false);
    }

    public async Task DeleteWebHookAsync(string botId)
    {
        var bot = new TelegramBotClient(botId, _httpClient);
        var webHook = await bot.GetWebhookInfoAsync().ConfigureAwait(false);
        if (webHook.Url.StartsWith(TelegramProcessingApiUrl)) await bot.DeleteWebhookAsync().ConfigureAwait(false);
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

    public Task ChangeNameAsync(string botId, string? value)
    {
        var bot = new TelegramBotClient(botId, _httpClient);
        return bot.SetMyNameAsync(value);
    }

    public Task ChangeDescriptionAsync(string botId, string? value)
    {
        var bot = new TelegramBotClient(botId, _httpClient);
        return bot.SetMyDescriptionAsync(value);
    }

    public Task ChangeShortDescriptionAsync(string botId, string? value)
    {
        var bot = new TelegramBotClient(botId, _httpClient);
        return bot.SetMyShortDescriptionAsync(value);
    }

    public async Task<TelegramBotDescriptionDto> GetDescriptionAsync(string botId)
    {
        var bot = new TelegramBotClient(botId, _httpClient);
        var info = await bot.GetMeAsync().ConfigureAwait(false);
        var name = await bot.GetMyNameAsync().ConfigureAwait(false);
        var description = await bot.GetMyDescriptionAsync().ConfigureAwait(false);
        var shortDescription = await bot.GetMyShortDescriptionAsync().ConfigureAwait(false);

        return Mapper.Map<TelegramBotDescriptionDto>(shortDescription.ShortDescription, opt =>
        {
            opt.AfterMap((_, dest) =>
            {
                dest.Name = name.Name;
                dest.Description = description.Description;
                dest.UserName = info.Username;
            });
        });
    }

    public Task ChangeCommandsAsync(string botId, IEnumerable<BotCommand> commands)
    {
        var bot = new TelegramBotClient(botId, _httpClient);
        return bot.SetMyCommandsAsync(commands);
    }
}