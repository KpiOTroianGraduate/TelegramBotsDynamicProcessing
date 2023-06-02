let commandActions;

document.getElementById("Commands").onclick = async () => {
    var presentComandActions = document.getElementById("presentComandActions");
    var newComandActions = document.getElementById("newComandActions");

    presentComandActions.innerHTML = "";
    newComandActions.innerHTML = "";

    presentComandActions.hidden = true;
    newComandActions.hidden = true;

    commandActions = await SendRequest(`api/CommandAction/${telegramBotId}/byTelegramBotId`, RequestType.GET)
        .then(response => { return response.json(); })
        .catch(ex => showMessageBox("Error", ex, '#d70040'));

    let result = await SendRequest(`api/Command/${telegramBotId}/byTelegramBotId`, RequestType.GET)
        .then(response => { return response.json(); })
        .catch(ex => showMessageBox("Error", ex, '#d70040'));

    for (let i = 0; i < result.length; i++) {
        var table = GenerateCommandTable(result[i].id, result[i].commandActionId);
        presentComandActions.appendChild(table);

        let description = document.getElementById(`description-${table.id}`);
        description.value = result[i].description;
        let isActive = document.getElementById(`isActive-${table.id}`);
        if (result[i].isActive == false)
            isActive.checked = false;

        let command = document.getElementById(`command-${table.id}`);
        command.value = result[i].command;

        let tfoot = document.getElementById(`tfoot-${table.id}`);
        let saveButton = document.createElement('button');
        saveButton.innerHTML = "Update";
        saveButton.onclick = async () => {
            let command = await GetCommandTable(table.id);
            await UpdateCommand(result[i].id, command);
            document.getElementById("Commands").click();
        };
        tfoot.appendChild(saveButton);
        let deleteButton = document.createElement('button');
        deleteButton.innerHTML = "Delete";
        deleteButton.onclick = async () => {
            await DeleteCommand(result[i].id);
            document.getElementById("Commands").click();
        }
        tfoot.appendChild(deleteButton);
    }

    newComandActions.setAttribute("tableCount", 0);
    var addTableButton = document.createElement('button');
    addTableButton.innerHTML = "add table";
    addTableButton.onclick = function () {
        let tableCount = Number(newComandActions.getAttribute("tableCount")) + 1;
        newComandActions.setAttribute("tableCount", `${tableCount}`);
        let table = GenerateCommandTable(tableCount);
        newComandActions.appendChild(table);
        let tfoot = document.getElementById(`tfoot-${table.id}`);
        let saveButton = document.createElement('button');
        saveButton.innerHTML = "Save";
        saveButton.onclick = async () => {
            let command = await GetCommandTable(table.id);
            await CreateCommand(command);
            document.getElementById("Commands").click();
        };
        tfoot.appendChild(saveButton);
    }
    newComandActions.appendChild(addTableButton);
    presentComandActions.hidden = false;
    newComandActions.hidden = false;
}

function GetCommandTable(tableId) {
    let command = document.getElementById(`command-${tableId}`);
    let description = document.getElementById(`description-${tableId}`);
    let optionValue = document.getElementById(`optionValue-${tableId}`);
    let isActive = document.getElementById(`isActive-${tableId}`);

    let commandActionId;
    if (optionValue.value == "null") {
        commandActionId = null;
    }
    else {
        commandActionId = optionValue.value;
    }
    return new Command(command.value, description.value, isActive.checked, telegramBotId, commandActionId);
}

const CreateCommand = async (content) => {
    await SendRequest(`api/Command`, RequestType.POST, content)
        .then(response => {
            if (response.ok) {
                showMessageBox("Success", "Command created successfully");
            }
        })
        .catch(ex => { showMessageBox(`Error when save Command ${ex.json}`, ex, '#d70040'); });
}

const UpdateCommand = async (id, content) => {
    await SendRequest(`api/Command/${id}`, RequestType.PUT, content)
        .then(response => {
            if (response.ok) {
                showMessageBox("Success", "Command updated successfully");
            }
        })
        .catch(ex => { showMessageBox(`Error when update Command ${ex.json}`, ex, '#d70040'); });
}

const DeleteCommand = async (id) => {
    await SendRequest(`api/Command/${id}`, RequestType.DELETE)
        .then(response => {
            if (response.ok) {
                showMessageBox("Success", "Command deleted successfully");
            }
        })
        .catch(ex => { showMessageBox(`Error when update Command ${ex.json}`, ex, '#d70040'); });
}

function GenerateCommandTable(commandId, commandActionId = null) {
    let table = document.createElement('table');
    table.id = `table-${commandId}`;
    table.border = "1";
    let tbody = document.createElement('tbody');
    tbody.id = `tbody-${table.id}`;
    table.appendChild(tbody);
    let row = document.createElement('tr');
    let cell = document.createElement('td');
    cell.innerHTML = "Command";
    row.appendChild(cell);
    cell = document.createElement('td');
    let inputText = document.createElement('input');
    inputText.id = `command-${table.id}`;
    cell.appendChild(inputText);
    row.appendChild(cell);
    tbody.appendChild(row);
    row = document.createElement('tr');
    cell = document.createElement('td');
    cell.innerHTML = "Description";
    row.appendChild(cell);
    cell = document.createElement('td');
    let description = document.createElement('input');
    description.id = `description-${table.id}`;
    cell.appendChild(description);
    row.appendChild(cell);
    tbody.appendChild(row);
    row = document.createElement('tr');
    cell = document.createElement('td');
    cell.innerHTML = "CommandAction";
    row.appendChild(cell);
    cell = document.createElement('td');
    inputText = document.createElement('select');
    let option = document.createElement('option');
    option.value = null;
    option.text = "None";
    inputText.appendChild(option);
    for (let i = 0; i < commandActions.length; i++) {
        option = document.createElement('option');
        option.value = commandActions[i].id;
        if (commandActions[i].id == commandActionId)
            option.selected = true;
        let startText;
        if (commandActions[i].commandActionType == 0) {
            startText = "Text";
        }
        else if (commandActions[i].commandActionType == 1) {
            startText = "POST";
        }
        else if (commandActions[i].commandActionType == 2) {
            startText = "Inline";
        }
        else if (commandActions[i].commandActionType == 3) {
            startText = "Reply";
        }
        option.text = `${startText} - ${commandActions[i].title}`;
        inputText.appendChild(option);
    }
    inputText.id = `optionValue-${table.id}`;
    cell.appendChild(inputText);
    row.appendChild(cell);
    tbody.appendChild(row);
    row = document.createElement('tr');
    cell = document.createElement('td');
    cell.innerHTML = "Is active";
    row.appendChild(cell);
    cell = document.createElement('td');
    let isActive = document.createElement('input');
    isActive.type = "checkbox";
    isActive.id = `isActive-${table.id}`;
    isActive.checked = true;
    cell.appendChild(isActive);
    row.appendChild(cell);
    tbody.appendChild(row);
    let tfoot = document.createElement('tfoot');
    tfoot.id = `tfoot-${table.id}`;
    tfoot.setAttribute("colspan", "100%");
    table.appendChild(tfoot);

    return table;
}