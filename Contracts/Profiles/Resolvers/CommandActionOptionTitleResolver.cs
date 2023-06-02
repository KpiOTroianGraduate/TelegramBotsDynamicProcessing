using AutoMapper;
using Contracts.Dto;
using Contracts.Entities;
using Newtonsoft.Json;
using Telegram.Bot.Types.ReplyMarkups;

namespace Contracts.Profiles.Resolvers;

public class CommandActionOptionTitleResolver : IValueResolver<CommandAction, CommandActionOptionDto, string?>
{
    public string? Resolve(CommandAction source, CommandActionOptionDto destination, string? destMember,
        ResolutionContext context)
    {
        if (source.Content == null)
            return null;

        return source.CommandActionType switch
        {
            CommandActionType.Text or CommandActionType.HttpPost => JsonConvert
                .DeserializeObject<KeyboardMarkupDto<string>>(source.Content)!.Title,
            CommandActionType.InlineKeyboard => JsonConvert
                .DeserializeObject<KeyboardMarkupDto<IEnumerable<IEnumerable<InlineKeyboardButton>>>>(
                    source.Content)!.Title,
            CommandActionType.ReplyKeyboard => JsonConvert
                .DeserializeObject<KeyboardMarkupDto<IEnumerable<IEnumerable<KeyboardButton>>>>(source.Content)!
                .Title,
            _ => null
        };
    }
}