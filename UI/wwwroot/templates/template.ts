namespace Tempate {
    const url: string = "";
    var jwtToken: string;

    class KeyboardMarkup {
        title: string;
        content: Array<Array<ReplyKeyboardButton>> | Array<Array<InlineKeyboardButton>> | string;

        constructor(title: string, content: Array<Array<ReplyKeyboardButton>> | Array<Array<InlineKeyboardButton>> | string) {
            this.title = title;
            this.content = content;
        }
    }

    class ReplyKeyboardButton {
        text: string;

        constructor(text: string) {
            this.text = text;
        }
    }

    class InlineKeyboardButton {
        url: string;
        text: string;

        constructor(url: string, text: string) {
            this.url = url;
            this.text = text;
        }
    }

    class CommandAction {
        content: string | null;
        commandActionType: CommandActionType;
        telegramBotId: string;

        constructor(content: string | null, commandActionType: CommandActionType, telegramBotId: string) {
            this.content = content;
            this.commandActionType = commandActionType;
            this.telegramBotId = telegramBotId;
        }
    }

    enum CommandActionType {
        Text,
        HttpPost,
        InlineKeyboard,
        ReplyKeyboard
    }

    enum RequestType {
        GET = "GET",
        POST = "POST",
        PUT = "PUT",
        DELETE = "DELETE"
    }

    class Command {
        command: string;
        description: string | null;
        isActive: boolean;
        telegramBotId: string;
        commandActionId: string | null;

        constructor(command: string, description: string | null, isActive: boolean, telegramBotId: string, commandActionId: string | null) {
            this.command = command;
            this.description = description;
            this.isActive = isActive;
            this.telegramBotId = telegramBotId;
            this.commandActionId = commandActionId;
        }
    }

    async function SendRequest(urlEnd: string, requestType: string, data: any) {
        return await fetch(`${url}${urlEnd}`, {
            method: requestType,
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + jwtToken,
            },
            body: JSON.stringify(data)
        })
            .then(response => {
                if (response.status == 401) {
                    window.location.replace(`${url}login`);
                }
            });
    }
}