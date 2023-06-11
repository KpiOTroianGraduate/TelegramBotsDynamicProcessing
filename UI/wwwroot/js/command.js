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
        let cell = document.createElement('div');
        cell.setAttribute("class", "col p-2 border-start border-bottom");
        let saveButton = document.createElement('button');
        saveButton.setAttribute("class", "btn btn-primary");
        saveButton.innerHTML = "Update";
        saveButton.onclick = async () => {
            let command = await GetCommandTable(table.id);
            await UpdateCommand(result[i].id, command);
            document.getElementById("Commands").click();
        };
        cell.appendChild(saveButton);
        tfoot.appendChild(cell);
        cell = document.createElement('div');
        cell.setAttribute("class", "col p-2 border-end border-bottom");
        let deleteButton = document.createElement('button');
        deleteButton.innerHTML = "Delete";
        deleteButton.setAttribute("class", "btn btn-primary");
        deleteButton.onclick = async () => {
            await DeleteCommand(result[i].id);
            document.getElementById("Commands").click();
        }
        cell.appendChild(deleteButton);
        tfoot.appendChild(cell);
    }

    newComandActions.setAttribute("tableCount", 0);
    var addTableButton = document.createElement('button');
    addTableButton.innerHTML = "Add new";
    addTableButton.setAttribute("class", "btn btn-primary");
    addTableButton.onclick = function () {
        let tableCount = Number(newComandActions.getAttribute("tableCount")) + 1;
        newComandActions.setAttribute("tableCount", `${tableCount}`);
        let table = GenerateCommandTable(tableCount);
        newComandActions.appendChild(table);
        let tfoot = document.getElementById(`tfoot-${table.id}`);
        let cell = document.createElement('div');
        cell.setAttribute("class", "col p-2 border"); 
        let saveButton = document.createElement('button');
        saveButton.innerHTML = "Save";
        saveButton.setAttribute("class", "btn btn-primary");
        saveButton.onclick = async () => {
            let command = await GetCommandTable(table.id);
            await CreateCommand(command);
            document.getElementById("Commands").click();
        };
        cell.appendChild(saveButton);
        tfoot.appendChild(cell);
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
        .then(async response => {
            if (response.ok) {
                showMessageBox("Success", "Command created successfully");
            }
            else {
                var result = await response.json();
                showMessageBox(`Error while saving Command`, result, '#d70040');
            }
        })
        .catch(ex => { showMessageBox(`Error while saving Command`, ex, '#d70040'); });
}

const UpdateCommand = async (id, content) => {
    await SendRequest(`api/Command/${id}`, RequestType.PUT, content)
        .then(async response => {
            if (response.ok) {
                showMessageBox("Success", "Command updated successfully");
            }
            else {
                var result = await response.json();
                showMessageBox(`Error while updating Command`, result, '#d70040');
            }
        })
        .catch(ex => { showMessageBox(`Error while updating Command`, ex, '#d70040'); });
}

const DeleteCommand = async (id) => {
    await SendRequest(`api/Command/${id}`, RequestType.DELETE)
        .then(async response => {
            if (response.ok) {
                showMessageBox("Success", "Command deleted successfully");
            }
            else {
                var result = await response.json();
                showMessageBox(`Error while deleting Command`, result, '#d70040');
            }
        })
        .catch(ex => { showMessageBox(`Error while deleting Command`, ex, '#d70040'); });
}

function GenerateCommandTable(commandId, commandActionId = null) {
    let table = document.createElement('div');
    table.setAttribute("class", "container p-4");
    table.id = `table-${commandId}`;
    let row = document.createElement('div');
    row.setAttribute("class", "row");
    let cell = document.createElement('div');
    cell.setAttribute("class", "col-3 p-2 border");
    cell.innerHTML = "Command";
    row.appendChild(cell);
    cell = document.createElement('div');
    cell.setAttribute("class", "col p-2 border");
    let inputText = document.createElement('input');
    inputText.id = `command-${table.id}`;
    inputText.setAttribute("class", "form-control");
    inputText.placeholder = "Command";
    cell.appendChild(inputText);
    row.appendChild(cell);
    table.appendChild(row);
    row = document.createElement('div');
    row.setAttribute("class", "row");
    cell = document.createElement('div');
    cell.setAttribute("class", "col-3 p-2 border");
    cell.innerHTML = "Description";
    row.appendChild(cell);
    cell = document.createElement('div');
    cell.setAttribute("class", "col p-2 border");
    let description = document.createElement('input');
    description.id = `description-${table.id}`;
    description.setAttribute("class", "form-control");
    description.placeholder = "Description";
    cell.appendChild(description);
    row.appendChild(cell);
    table.appendChild(row);
    row = document.createElement('div');
    row.setAttribute("class", "row");
    cell = document.createElement('div');
    cell.setAttribute("class", "col-3 p-2 border");
    cell.innerHTML = "Action for command";
    row.appendChild(cell);
    cell = document.createElement('div');
    cell.setAttribute("class", "col p-2 border");
    inputText = document.createElement('select');
    inputText.setAttribute("class", "form-select");
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
    table.appendChild(row);
    row = document.createElement('div');
    row.setAttribute("class", "row");
    cell = document.createElement('div');
    cell.setAttribute("class", "col-3 p-2 border");
    cell.innerHTML = "Active";
    row.appendChild(cell);
    cell = document.createElement('div');
    cell.setAttribute("class", "col p-2 border");
    let isActive = document.createElement('input');
    isActive.type = "checkbox";
    isActive.id = `isActive-${table.id}`;
    isActive.setAttribute("class", "form-check-input");
    isActive.checked = true;
    cell.appendChild(isActive);
    row.appendChild(cell);
    table.appendChild(row);
    let tfoot = document.createElement('div');
    tfoot.setAttribute("class", "row");
    tfoot.id = `tfoot-${table.id}`;
    table.appendChild(tfoot);

    return table;
}