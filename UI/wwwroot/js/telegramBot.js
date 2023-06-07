var userId;

const GetTelegramBots = async () => {
    var presentBots = document.getElementById("presentTelegramBots");
    presentBots.innerHTML = "";

    let result = await SendRequest(`api/TelegramBot`, RequestType.GET)
        .then(response => { return response.json(); })
        .catch(ex => showMessageBox("Error", ex, '#d70040'));

    for (let i = 0; i < result.length; i++) {
        let table = document.createElement('table');
        table.id = `table-${result[i].id}`;
        table.border = "1";
        let thead = document.createElement('thead');
        thead.innerHTML = `@${result[i].username}`;
        thead.setAttribute("colspan", "100%");
        table.appendChild(thead);
        let tbody = document.createElement('tbody');
        tbody.id = `tbody-${table.id}`;
        table.appendChild(tbody);
        let row = document.createElement('tr');
        let cell = document.createElement('td');
        let img = document.createElement('img');
        img.setAttribute("width", "50");
        img.setAttribute("height", "50");
        img.src = result[i].avatar;
        cell.appendChild(img);
        row.appendChild(cell);
        cell = document.createElement('td');
        var name = `${result[i].firstName}`;
        if (result[i].lastName != null) {
            name = `${name} ${result[i].lastName}`;
        }
        cell.innerHTML = name;
        row.appendChild(cell);
        cell = document.createElement('td');

        let isActive = document.createElement('input');
        isActive.type = "checkbox";
        isActive.id = `isActive-${result[i].id}`;
        isActive.checked = result[i].isActive;
        cell.appendChild(isActive);
        row.appendChild(cell);
        cell = document.createElement('td');
        let token = document.createElement('input');
        token.id = `token-${result[i].id}`;
        token.value = result[i].token;
        token.size = "50";
        cell.appendChild(token);
        row.appendChild(cell);
        cell = document.createElement('td');
        let btn = document.createElement('button');
        btn.innerHTML = "Manage";
        btn.onclick = async () => {
            telegramBotId = result[i].id;
            await document.getElementById("Commands").click();
            document.getElementById("botsListFrame").hidden = true;
            document.getElementById("commandsListFrame").hidden = false;
        };
        cell.appendChild(btn);
        row.appendChild(cell);
        tbody.appendChild(row);
        table.appendChild(tbody);
        let tfoot = document.createElement('tfoot');
        let saveButton = document.createElement('button');
        saveButton.innerHTML = "Update";
        saveButton.onclick = async () => {
            let telegramBot = GetTelegramBotTable(result[i].id);
            await UpdateTelegramBot(result[i].id, telegramBot)
                .then(async () => {
                    await GetTelegramBots();
                });
        };
        tfoot.appendChild(saveButton);
        let deleteButton = document.createElement('button');
        deleteButton.innerHTML = "Delete";
        deleteButton.onclick = async () => {
            await DeleteTelegramBot(result[i].id)
                .then(async () => {
                    await GetTelegramBots();
                });
        }
        tfoot.appendChild(deleteButton);
        table.appendChild(tfoot);
        presentBots.appendChild(table);
    }


    var newTelegramBot = document.getElementById("newTelegramBot");
    newTelegramBot.innerHTML = "";
    let createButton = document.createElement('button');
    createButton.innerHTML = "Create";
    createButton.onclick = function () {
        newTelegramBot.innerHTML = "";
        table = document.createElement('table');
        table.id = `table-new`;
        table.border = "1";
        thead = document.createElement('thead');
        thead.innerHTML = `@New`;
        thead.setAttribute("colspan", "100%");

        table.appendChild(thead);
        tbody = document.createElement('tbody');
        row = document.createElement('tr');
        cell = document.createElement('td');
        isActive = document.createElement('input');
        isActive.type = "checkbox";
        isActive.id = `isActive-new`;
        cell.appendChild(isActive);
        row.appendChild(cell);
        cell = document.createElement('td');
        token = document.createElement('input');
        token.id = `token-new`;
        token.size = "50";
        cell.appendChild(token);
        row.appendChild(cell);
        tbody.appendChild(row);
        table.appendChild(tbody);
        tfoot = document.createElement('tfoot');
        saveButton = document.createElement('button');
        saveButton.innerHTML = "Create";
        saveButton.onclick = async () => {
            let telegramBot = GetTelegramBotTable("new");
            await CreateTelegramBot(telegramBot)
            .then(async () => {
                await GetTelegramBots();
            });
        };
        tfoot.appendChild(saveButton);
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
        .then(response => {
            if (response.ok) {
                showMessageBox("Success", "TelrgramBot created successfully");
            }
        })
        .catch(ex => { showMessageBox(`Error while creating TelrgramBot ${ex.json}`, ex, '#d70040'); });
}

const UpdateTelegramBot = async (id, content) => {
    await SendRequest(`api/TelegramBot/${id}`, RequestType.PUT, content)
        .then(response => {
            if (response.ok) {
                showMessageBox("Success", "TelrgramBot updated successfully");
            }
        })
        .catch(ex => { showMessageBox(`Error while updating TelrgramBot ${ex.json}`, ex, '#d70040'); });
}

const DeleteTelegramBot = async (id) => {
    await SendRequest(`api/TelegramBot/${id}`, RequestType.DELETE)
        .then(response => {
            if (response.ok) {
                showMessageBox("Success", "TelrgramBot deleted successfully");
            }
        })
        .catch(ex => { showMessageBox(`Error while deleting TelrgramBot ${ex.json}`, ex, '#d70040'); });
}