const url = "https://hapan9-telegram.azurewebsites.net/index.html";
var telegramBotId;
var jwtToken;

class KeyboardMarkup {
    constructor(title, content) {
        this.title = title;
        this.content = content;
    }
}
class ReplyKeyboardButton {
    constructor(text) {
        this.text = text;
    }
}
class InlineKeyboardButton {
    constructor(url, text) {
        this.url = url;
        this.text = text;
    }
}
class CommandAction {
    constructor(content, commandActionType, telegramBotId) {
        this.content = content;
        this.commandActionType = commandActionType;
        this.telegramBotId = telegramBotId;
    }
}
var CommandActionType;
(function (CommandActionType) {
    CommandActionType[CommandActionType["Text"] = 0] = "Text";
    CommandActionType[CommandActionType["HttpPost"] = 1] = "HttpPost";
    CommandActionType[CommandActionType["InlineKeyboard"] = 2] = "InlineKeyboard";
    CommandActionType[CommandActionType["ReplyKeyboard"] = 3] = "ReplyKeyboard";
})(CommandActionType || (CommandActionType = {}));
var RequestType;
(function (RequestType) {
    RequestType["GET"] = "GET";
    RequestType["POST"] = "POST";
    RequestType["PUT"] = "PUT";
    RequestType["DELETE"] = "DELETE";
})(RequestType || (RequestType = {}));
class Command {
    constructor(command, description, isActive, telegramBotId, commandActionId) {
        this.command = command;
        this.description = description;
        this.isActive = isActive;
        this.telegramBotId = telegramBotId;
        this.commandActionId = commandActionId;
    }
}
class TelegramBot {
    constructor(token, isActive, userId) {
        this.token = token;
        this.isActive = isActive;
        this.userId = userId;
    }
}
class Value {
    constructor(value) {
        this.value = value;
    }
}