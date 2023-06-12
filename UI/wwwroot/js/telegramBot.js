var userId;

const GetTelegramBots = async () => {
    var presentBots = document.getElementById("presentTelegramBots");
    presentBots.innerHTML = "";

    let result = await SendRequest(`api/TelegramBot`, RequestType.GET)
        .then(response => { return response.json(); })
        .catch(ex => showMessageBox("Error while reading telegramBots list", ex, '#d70040'));

    for (let i = 0; i < result.length; i++) {
        console.log(result[i]);
        let table = document.createElement('div');
        table.setAttribute("class", "container p-4");
        let thead = document.createElement('div');
        thead.setAttribute("class", "row");
        let cell = document.createElement('div');
        cell.setAttribute("class", "col p-2 border text-center");
        cell.innerHTML = `@${result[i].username}`;
        thead.appendChild(cell);
        table.appendChild(thead);
        let inlineMenu = document.createElement('div');
        inlineMenu.setAttribute("class", "row");
        cell = document.createElement('div');
        cell.setAttribute("class", "col-2 p-2 border text-center");
        cell.innerHTML = "Photo";
        inlineMenu.appendChild(cell);
        cell = document.createElement('div');
        cell.setAttribute("class", "col-2 p-2 border text-center");
        cell.innerHTML = "Name";
        inlineMenu.appendChild(cell);
        cell = document.createElement('div');
        cell.setAttribute("class", "col-1 p-2 border text-center");
        cell.innerHTML = "Active";
        inlineMenu.appendChild(cell);
        cell = document.createElement('div');
        cell.setAttribute("class", "col p-2 border text-center");
        cell.innerHTML = "Token";
        inlineMenu.appendChild(cell);
        cell = document.createElement('div');
        cell.setAttribute("class", "col-2 p-2 border text-center");
        cell.innerHTML = "Actions";
        inlineMenu.appendChild(cell);
        table.appendChild(inlineMenu);
        let row = document.createElement('div');
        row.setAttribute("class", "row");
        cell = document.createElement('div');
        cell.setAttribute("class", "col-2 p-2 border text-center");
        let img = document.createElement('img');
        img.setAttribute("width", "50");
        img.setAttribute("height", "50");
        img.setAttribute("class", "rounded float-left")
        img.src = result[i].avatar;
        cell.appendChild(img);
        row.appendChild(cell);
        cell = document.createElement('div');
        cell.setAttribute("class", "col-2 p-2 border text-center");
        let name = `${result[i].firstName}`;
        if (result[i].lastName != null) {
            name = `${name} ${result[i].lastName}`;
        }
        cell.innerHTML = name;
        row.appendChild(cell);
        cell = document.createElement('div');
        cell.setAttribute("class", "col-1 p-2 border text-center");
        let isActive = document.createElement('input');
        isActive.type = "checkbox";
        isActive.id = `isActive-${result[i].id}`;
        isActive.checked = result[i].isActive;
        isActive.setAttribute("class", "form-check-input");
        cell.appendChild(isActive);
        row.appendChild(cell);
        cell = document.createElement('div');
        cell.setAttribute("class", "col p-2 border text-center");
        let token = document.createElement('input');
        token.id = `token-${result[i].id}`;
        token.value = result[i].token;
        token.size = "50";
        token.placeholder = "TelegramBot token";
        token.setAttribute("class", "form-control");
        cell.appendChild(token);
        row.appendChild(cell);
        cell = document.createElement('div');
        cell.setAttribute("class", "col-2 p-2 border text-center");
        let btn = document.createElement('button');
        btn.setAttribute("class", "btn btn-primary");
        btn.innerHTML = "Manage";
        btn.onclick = async () => {
            telegramBotId = result[i].id;
            await document.getElementById("Commands").click();
            document.getElementById("botsListFrame").hidden = true;
            document.getElementById("commandActionsFrame").hidden = false;
        };
        cell.appendChild(btn);
        row.appendChild(cell);
        table.appendChild(row);
        let tfoot = document.createElement('div');
        tfoot.setAttribute("class", "row text-center");
        cell = document.createElement('div');
        cell.setAttribute("class", "col text-center p-2 border-start border-bottom");
        let saveButton = document.createElement('button');
        saveButton.innerHTML = "Update";
        saveButton.setAttribute("class", "btn btn-primary");
        saveButton.onclick = async () => {
            let telegramBot = GetTelegramBotTable(result[i].id);
            await UpdateTelegramBot(result[i].id, telegramBot)
                .then(async () => {
                    await GetTelegramBots();
                });
        };
        cell.appendChild(saveButton);
        tfoot.appendChild(cell);
        cell = document.createElement('div');
        cell.setAttribute("class", "col text-center p-2 border-end border-bottom");
        let deleteButton = document.createElement('button');
        deleteButton.innerHTML = "Delete";
        deleteButton.setAttribute("class", "btn btn-danger");
        deleteButton.onclick = async () => {
            await DeleteTelegramBot(result[i].id)
                .then(async () => {
                    await GetTelegramBots();
                });
        }
        cell.appendChild(deleteButton);
        tfoot.appendChild(cell);
        table.appendChild(tfoot);
        presentBots.appendChild(table);
    }


    var newTelegramBot = document.getElementById("newTelegramBot");
    newTelegramBot.innerHTML = "";
    let createButton = document.createElement('button');
    createButton.innerHTML = "Create";
    createButton.setAttribute("class", "btn btn-primary");
    createButton.onclick = function () {
        newTelegramBot.innerHTML = "";
        table = document.createElement('div');
        table.setAttribute("class", "container p-4");
        thead = document.createElement('div');
        thead.setAttribute("class", "row");
        cell = document.createElement('div');
        cell.setAttribute("class", "col p-2 border");
        cell.innerHTML = `New telegram bot`;
        thead.appendChild(cell);
        table.appendChild(thead);
        inlineMenu = document.createElement('div');
        inlineMenu.setAttribute("class", "row");
        cell = document.createElement('div');
        cell.setAttribute("class", "col-2 p-2 border text-center");
        cell.innerHTML = "Active";
        inlineMenu.appendChild(cell);
        cell = document.createElement('div');
        cell.setAttribute("class", "col p-2 border text-center");
        cell.innerHTML = "TelegramBot Token";
        inlineMenu.appendChild(cell);
        table.appendChild(inlineMenu);
        row = document.createElement('div');
        row.setAttribute("class", "row");
        cell = document.createElement('div');
        cell.setAttribute("class", "col-2 p-2 border");
        isActive = document.createElement('input');
        isActive.type = "checkbox";
        isActive.id = `isActive-new`;
        isActive.setAttribute("class", "form-check-input");
        isActive.checked = true;
        cell.appendChild(isActive);
        row.appendChild(cell);
        cell = document.createElement('div');
        cell.setAttribute("class", "col p-2 border");
        token = document.createElement('input');
        token.id = `token-new`;
        token.size = "50";
        token.setAttribute("class", "form-control");
        token.placeholder = "TelegramBot token";
        cell.appendChild(token);
        row.appendChild(cell);
        table.appendChild(row);
        tfoot = document.createElement('div');
        tfoot.setAttribute("class", "row");
        cell = document.createElement('div');
        cell.setAttribute("class", "col p-2 border text-center");
        saveButton = document.createElement('button');
        saveButton.innerHTML = "Create";
        saveButton.setAttribute("class", "btn btn-primary");
        saveButton.onclick = async () => {
            let telegramBot = GetTelegramBotTable("new");
            await CreateTelegramBot(telegramBot)
            .then(async () => {
                await GetTelegramBots();
            });
        };
        cell.appendChild(saveButton);
        tfoot.appendChild(cell);
        table.appendChild(tfoot);
        newTelegramBot.appendChild(table);
    }
    newTelegramBot.appendChild(createButton);

}

function GetTelegramBotTable(id) {
    let token = document.getElementById(`token-${id}`).value;
    let isActive = document.getElementById(`isActive-${id}`).checked;

    return new TelegramBot(token, isActive, userId);
}

const CreateTelegramBot = async (content) => {
    await SendRequest(`api/TelegramBot/`, RequestType.POST, content)
        .then(async response => {
            if (response.ok) {
                showMessageBox("Success", "TelrgramBot created successfully");
            }
            else {
                var result = await response.json();
                showMessageBox(`Error while creating TelrgramBot`, result, '#d70040');
            }
        })
        .catch(ex => { showMessageBox(`Error while creating TelrgramBot`, ex, '#d70040'); });
}

const UpdateTelegramBot = async (id, content) => {
    await SendRequest(`api/TelegramBot/${id}`, RequestType.PUT, content)
        .then(async response => {
            if (response.ok) {
                showMessageBox("Success", "TelrgramBot updated successfully");
            }
            else {
                var result = await response.json();
                showMessageBox(`Error while updating TelrgramBot`, result, '#d70040');
            }
        })
        .catch(ex => { showMessageBox(`Error while updating TelrgramBot`, ex, '#d70040'); });
}

const DeleteTelegramBot = async (id) => {
    await SendRequest(`api/TelegramBot/${id}`, RequestType.DELETE)
        .then(async response => {
            if (response.ok) {
                showMessageBox("Success", "TelrgramBot deleted successfully");
            }
            else {
                var result = await response.json();
                showMessageBox(`Error while deleting TelrgramBot`, result, '#d70040');
            }
        })
        .catch(ex => { showMessageBox(`Error while deleting TelrgramBot`, ex, '#d70040'); });
}