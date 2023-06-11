document.getElementById("CommandActionText").onclick = async () => {
    await SetUpCommandActionView("CommandActionText", CommandActionType.Text);
}

document.getElementById("CommandActionPost").onclick = async () => {
    await SetUpCommandActionView("CommandActionPost", CommandActionType.HttpPost);
}

document.getElementById("CommandActionInline").onclick = async () => {
    await SetUpCommandActionView("CommandActionInline", CommandActionType.InlineKeyboard);
}

document.getElementById("CommandActionReply").onclick = async () => {
    await SetUpCommandActionView("CommandActionReply", CommandActionType.ReplyKeyboard);
}

const SetUpCommandActionView = async (buttonId, commandActionType) => {
    var presentComandActions = document.getElementById("presentComandActions");
    var newComandActions = document.getElementById("newComandActions");

    presentComandActions.innerHTML = "";
    newComandActions.innerHTML = "";

    presentComandActions.hidden = true;
    newComandActions.hidden = true;

    let commandActionTypeEnum;
    
    if (commandActionType == CommandActionType.Text) {
        commandActionTypeEnum = 0;
    }
    else if (commandActionType == CommandActionType.HttpPost) {
        commandActionTypeEnum = 1;
    }
    else if (commandActionType == CommandActionType.InlineKeyboard)
    {
        commandActionTypeEnum = 2;
    }
    else if (commandActionType == CommandActionType.ReplyKeyboard) {
        commandActionTypeEnum = 3;
    }

    //Setup presentComandActions
    let result = await SendRequest(`api/CommandAction/${telegramBotId}/${commandActionTypeEnum}/byTelegramBotId`, RequestType.GET)
        .then(response => { return response.json(); })
        .catch(ex => showMessageBox("Error", ex, '#d70040'));

    for (let i = 0; i < result.length; i++) {
        let table;

        if (commandActionType == CommandActionType.Text) {
            table = GenerateSimpleTable(result[i].id, "Text");
            presentComandActions.appendChild(table);
            document.getElementById(`inputGroupText-${table.id}`).innerHTML = "Text - ";
        }
        else if (commandActionType == CommandActionType.HttpPost) {
            table = GenerateSimpleTable(result[i].id, "Url");
            presentComandActions.appendChild(table);
            document.getElementById(`inputGroupText-${table.id}`).innerHTML = "POST - ";
        }
        else if (commandActionType == CommandActionType.InlineKeyboard) {
            table = GenerateButtonTable(result[i].id, true);
            presentComandActions.appendChild(table);
            document.getElementById(`inputGroupText-${table.id}`).innerHTML = "Inline - ";
        }
        else if (commandActionType == CommandActionType.ReplyKeyboard) {
            table = GenerateButtonTable(result[i].id, false);
            presentComandActions.appendChild(table);
            document.getElementById(`inputGroupText-${table.id}`).innerHTML = "Reply - ";
        }

        let content = JSON.parse(result[i].content);
        let inputTitle = document.getElementById(`inputTitle-${table.id}`);
        inputTitle.placeholder = "title";
        inputTitle.value = content.title;

        let tbody = document.getElementById(`tbody-${table.id}`);

        if (commandActionType == CommandActionType.InlineKeyboard || commandActionType == CommandActionType.ReplyKeyboard) {
            for (let j = 0; j < content.content.length; j++) {
                table.setAttribute("rowCount", `${Number(table.getAttribute("rowCount")) + 1}`);
                let row = document.createElement('div');
                row.setAttribute("class", "row");
                let rowCount = table.getAttribute("rowCount");
                row.id = `row-${table.id}-${rowCount}`;
                row.setAttribute("cellCount", "0");
                let cellWithButton = document.createElement('div');
                cellWithButton.setAttribute("class", "col p-2 border");
                let addCellButton = document.createElement('button');
                addCellButton.innerHTML = "Add cell";
                addCellButton.setAttribute("class", "btn btn-primary");
                addCellButton.onclick = function () {
                    let cellCount = Number(row.getAttribute("cellCount")) + 1;
                    row.setAttribute("cellCount", `${cellCount}`);
                    let cell = document.createElement('div');
                    cell.setAttribute("class", "col p-2 border");
                    //cell.id = `cell-${table.id}-${rowCount}-${cellCount}`;
                    row.appendChild(cell);
                    let inputTable = document.createElement('div');
                    inputTable.setAttribute("class", "container");
                    let inputRow = document.createElement('div');
                    inputRow.setAttribute("class", "row");
                    let inputCell = document.createElement('div');
                    inputCell.setAttribute("class", "col-3 p-2");
                    inputCell.innerHTML = "text";
                    inputRow.appendChild(inputCell);
                    inputCell = inputCell = document.createElement('div');
                    inputCell.setAttribute("class", "col p-2");
                    let inputText = document.createElement('input');
                    inputText.setAttribute("class", "form-control");
                    inputText.id = `inputText-${table.id}-${rowCount}-${cellCount}`;
                    inputCell.appendChild(inputText);
                    inputRow.appendChild(inputCell);
                    inputTable.appendChild(inputRow);
                    if (commandActionType == CommandActionType.InlineKeyboard) {
                        inputRow.setAttribute("class", "row border-bottom");
                        inputRow = document.createElement('div');
                        inputRow.setAttribute("class", "row border-top");
                        inputCell = inputCell = document.createElement('div');
                        inputCell.setAttribute("class", "col-3 p-2");
                        inputCell.innerHTML = "url";
                        inputRow.appendChild(inputCell);
                        inputCell = inputCell = document.createElement('div');
                        inputCell.setAttribute("class", "col p-2");
                        let inputUrl = document.createElement('input');
                        inputUrl.setAttribute("class", "form-control");
                        inputUrl.id = `inputUrl-${table.id}-${rowCount}-${cellCount}`;
                        inputCell.appendChild(inputUrl);
                        inputRow.appendChild(inputCell);
                        inputTable.appendChild(inputRow);
                    }
                    cell.appendChild(inputTable);
                };

                cellWithButton.appendChild(addCellButton);
                row.appendChild(cellWithButton);
                for (let k = 0; k < content.content[j].length; k++) {
                    cellWithButton.setAttribute("class", "col-3 p-2 border");
                    let cellCount = Number(row.getAttribute("cellCount")) + 1;
                    row.setAttribute("cellCount", cellCount);
                    let cell = document.createElement('div');
                    cell.setAttribute("class", "col p-2 border");
                    //cell.id = `cell-${table.id}-${rowCount}-${cellCount}`;
                    let inputTable = document.createElement('div');
                    inputTable.setAttribute("class", "container");
                    let inputRow = document.createElement('div');
                    inputRow.setAttribute("class", "row");
                    let inputCell = document.createElement('div');
                    inputCell.setAttribute("class", "col-3 p-2");
                    inputCell.innerHTML = "text";
                    inputRow.appendChild(inputCell);
                    inputCell = document.createElement('div');
                    inputCell.setAttribute("class", "col p-2");
                    let inputText = document.createElement('input');
                    inputText.setAttribute("class", "form-control");
                    inputText.id = `inputText-${table.id}-${rowCount}-${cellCount}`;
                    inputText.value = content.content[j][k].text;
                    inputCell.appendChild(inputText);
                    inputRow.appendChild(inputCell);
                    inputTable.appendChild(inputRow);
                    if (commandActionType == CommandActionType.InlineKeyboard) {
                        inputRow.setAttribute("class", "row border-bottom");
                        inputRow = document.createElement('div');
                        inputRow.setAttribute("class", "row border-top");
                        inputCell = inputCell = document.createElement('div');
                        inputCell.setAttribute("class", "col-3 p-2");
                        inputCell.innerHTML = "url";
                        inputRow.appendChild(inputCell);
                        inputCell = inputCell = document.createElement('div');
                        inputCell.setAttribute("class", "col p-2");
                        let inputUrl = document.createElement('input');
                        inputUrl.setAttribute("class", "form-control");
                        inputUrl.id = `inputUrl-${table.id}-${rowCount}-${cellCount}`;
                        inputUrl.value = content.content[j][k].url;
                        inputCell.appendChild(inputUrl);
                        inputRow.appendChild(inputCell);
                        inputTable.appendChild(inputRow);
                    }
                    cell.appendChild(inputTable);
                    row.appendChild(cell);
                }

                tbody.appendChild(row);
            }
        }
        else {

            var inputText = document.getElementById(`inputValue-${table.id}`);
            inputText.value = content.content;
        }

        let tfoot = document.getElementById(`tfoot-${table.id}`);
        let cell = document.createElement('div');
        cell.setAttribute("class", "col p-2 border-start p-2 border-bottom");
        var updateButton = document.createElement('button');
        updateButton.setAttribute("class", "btn btn-primary");
        updateButton.innerHTML = "Update";
        updateButton.onclick = async () => {
            let tableJson;
            if (commandActionType == CommandActionType.InlineKeyboard)
                tableJson = await GetButtonTableJson(table.id, CommandActionType.InlineKeyboard);
            else if (commandActionType == CommandActionType.ReplyKeyboard)
                tableJson = await GetButtonTableJson(table.id, CommandActionType.ReplyKeyboard);
            else
                tableJson = await GetTableJson(table.id);

            await UpdateCommandAction(result[i].id, new CommandAction(tableJson, commandActionTypeEnum, telegramBotId));
            document.getElementById(buttonId).click();
        };
        cell.appendChild(updateButton);
        tfoot.appendChild(cell);
        cell = document.createElement('div');
        cell.setAttribute("class", "col p-2 border-end p-2 border-bottom");
        let deleteButton = document.createElement('button');
        deleteButton.innerHTML = "Delete";
        deleteButton.setAttribute("class", "btn btn-danger");
        deleteButton.onclick = async () => {
            await DeleteCommandAction(result[i].id);
            document.getElementById(buttonId).click();
        };
        cell.appendChild(deleteButton);
        tfoot.appendChild(cell);
    }

    //Setup newComandActions
    newComandActions.setAttribute("tableCount", 0);
    var addTableButton = document.createElement('button');
    addTableButton.innerHTML = "Add new";
    addTableButton.setAttribute("class", "btn btn-primary");
    addTableButton.onclick = function () {
        let tableCount = Number(newComandActions.getAttribute("tableCount")) + 1;
        newComandActions.setAttribute("tableCount", `${tableCount}`);
        let table;

        if (commandActionType == CommandActionType.Text) {
            table = GenerateSimpleTable(tableCount, "Text");
            presentComandActions.appendChild(table);
            document.getElementById(`inputGroupText-${table.id}`).innerHTML = "Text - ";
        }
        else if (commandActionType == CommandActionType.HttpPost) {
            table = GenerateSimpleTable(tableCount, "Url");
            presentComandActions.appendChild(table);
            document.getElementById(`inputGroupText-${table.id}`).innerHTML = "POST - ";
        }
        else if (commandActionType == CommandActionType.InlineKeyboard) {
            table = GenerateButtonTable(tableCount, true);
            presentComandActions.appendChild(table);
            document.getElementById(`inputGroupText-${table.id}`).innerHTML = "Inline - ";
        }
        else if (commandActionType == CommandActionType.ReplyKeyboard) {
            table = GenerateButtonTable(tableCount, false);
            presentComandActions.appendChild(table);
            document.getElementById(`inputGroupText-${table.id}`).innerHTML = "Reply - ";
        }

        newComandActions.appendChild(table);
        let tfid = `tfoot-table-${tableCount}`;
        let tfoot = document.getElementById(tfid);
        let saveButton = document.createElement('button');
        let cell = document.createElement('div');
        cell.setAttribute("class", "col p-2 border");
        saveButton.innerHTML = "Save";
        saveButton.setAttribute("class", "btn btn-primary");
        saveButton.onclick = async () => {
            let tableJson;
            if (commandActionType == CommandActionType.InlineKeyboard)
                tableJson = await GetButtonTableJson(table.id, CommandActionType.InlineKeyboard);
            else if (commandActionType == CommandActionType.ReplyKeyboard)
                tableJson = await GetButtonTableJson(table.id, CommandActionType.ReplyKeyboard);
            else
                tableJson = await GetTableJson(table.id);

            await CreateCommandAction(new CommandAction(tableJson, commandActionTypeEnum, telegramBotId));
            document.getElementById(buttonId).click();
        };
        cell.appendChild(saveButton);
        tfoot.appendChild(cell);
    };
    newComandActions.appendChild(addTableButton);

    presentComandActions.hidden = false;
    newComandActions.hidden = false;
}

const GetTableJson = async (id) => {
    let text = document.getElementById(`inputValue-${id}`).value;
    let title = document.getElementById(`inputTitle-${id}`).value;
    let keyboardMarkup = new KeyboardMarkup(title, text);
    return await JSON.stringify(keyboardMarkup);
}

const GetButtonTableJson = async (id, commandActionType) => {
    let table = document.getElementById(id);
    let list = new Array();
    for (let i = 1; i <= Number(table.getAttribute("rowCount")); i++) {
        let row = document.getElementById(`row-${id}-${i}`);
        let rowArray = new Array();
        for (let j = 1; j <= Number(row.getAttribute("cellCount")); j++) {
            let text = document.getElementById(`inputText-${id}-${i}-${j}`).value;

            if (commandActionType == CommandActionType.InlineKeyboard) {
                let url = document.getElementById(`inputUrl-${id}-${i}-${j}`).value;

                var inlineKeyboardButton = new InlineKeyboardButton(url, text);
                rowArray.push(inlineKeyboardButton);
            }
            else if (commandActionType == CommandActionType.ReplyKeyboard) {
                var replyKeyboardButton = new ReplyKeyboardButton(text);
                rowArray.push(replyKeyboardButton);
            }
        }
        list.push(rowArray);
    }
    let title = document.getElementById(`inputTitle-${id}`).value;
    let keyboardMarkup = new KeyboardMarkup(title, list);
    return await JSON.stringify(keyboardMarkup);
}

const CreateCommandAction = async (content) => {

    await SendRequest(`api/CommandAction`, RequestType.POST, content)
        .then(async response => {
            if (response.ok) {
                showMessageBox("Success", "CommandAction created successfully");
            }
            else {
                var result = await response.json();
                showMessageBox(`Error while saving CommandAction`, result, '#d70040');
            }
        })
        .catch(ex => { showMessageBox(`Error while saving CommandAction`, ex, '#d70040'); });
};

const UpdateCommandAction = async (id, content) => {
    await SendRequest(`api/CommandAction/${id}`, RequestType.PUT, content)
        .then(async response => {
            if (response.ok) {
                showMessageBox("Success", "CommandAction updated successfully");
            }
            else {
                var result = await response.json();
                showMessageBox(`Error while updating CommandAction`, result, '#d70040');
            }
        })
        .catch(ex => { showMessageBox(`Error while updating CommandAction`, ex, '#d70040'); });
}

const DeleteCommandAction = async (id) => {
    await SendRequest(`api/CommandAction/${id}`, RequestType.DELETE)
        .then(async response => {
            if (response.ok) {
                showMessageBox("Success", "CommandAction deleted successfully");
            }
            else {
                var result = await response.json();
                showMessageBox(`Error while deleting CommandAction`, result, '#d70040');
            }
        })
        .catch(ex => { showMessageBox(`Error while deleting CommandAction`, ex, '#d70040'); });
}

function GenerateSimpleTable(tableId, valueName){
    let table = document.createElement('div');
    table.setAttribute("class", "container p-4");
    table.id = `table-${tableId}`;
    let thead = document.createElement('div');
    thead.setAttribute("class", "row");
    let cell = document.createElement('div');
    cell.setAttribute("class", "col p-2 border");
    let titleGroup = document.createElement('div');
    titleGroup.setAttribute("class", "input-group");
    let inputGroupText = document.createElement('span');
    inputGroupText.setAttribute("class", "input-group-text");
    inputGroupText.id = `inputGroupText-${table.id}`;
    titleGroup.appendChild(inputGroupText);
    let inputTitle = document.createElement('input');
    inputTitle.setAttribute("class", "form-control");
    inputTitle.id = `inputTitle-${table.id}`;
    inputTitle.placeholder = "Title";
    titleGroup.appendChild(inputTitle);
    cell.appendChild(titleGroup);
    thead.appendChild(cell);
    table.appendChild(thead);
    let row = document.createElement('div');
    row.setAttribute("class", "row");
    cell = document.createElement('div');
    cell.setAttribute("class", "col-3 p-2 border");
    cell.innerHTML = valueName;
    row.appendChild(cell);
    inputCell = document.createElement('div');
    inputCell.setAttribute("class", "col p-2 border");
    let inputText = document.createElement('input');
    inputText.setAttribute("class", "form-control");
    inputText.id = `inputValue-${table.id}`;
    inputCell.appendChild(inputText);
    row.appendChild(inputCell);
    table.appendChild(row);
    let tfoot = document.createElement('div');
    tfoot.setAttribute("class", "row");
    tfoot.id = `tfoot-${table.id}`;
    table.appendChild(tfoot);

    return table;
}

function GenerateButtonTable(tableId, addValue) {
    let table = document.createElement('div');
    table.id = `table-${tableId}`;
    table.setAttribute("rowCount", "0");
    table.setAttribute("class", "container p-4");
    let thead = document.createElement('div');
    thead.setAttribute("class", "row");
    let cell = document.createElement('div');
    cell.setAttribute("class", "col p-2 border");
    let titleGroup = document.createElement('div');
    titleGroup.setAttribute("class", "input-group");
    let inputGroupText = document.createElement('span');
    inputGroupText.setAttribute("class", "input-group-text");
    inputGroupText.id = `inputGroupText-${table.id}`;
    titleGroup.appendChild(inputGroupText);
    let inputTitle = document.createElement('input');
    inputTitle.setAttribute("class", "form-control");
    inputTitle.id = `inputTitle-${table.id}`;
    inputTitle.placeholder = "Title";
    titleGroup.appendChild(inputTitle);
    cell.appendChild(titleGroup);
    thead.appendChild(cell);
    table.appendChild(thead);
    let tbody = document.createElement('div');
    tbody.id = `tbody-${table.id}`;
    table.appendChild(tbody);
    let addRowButton = document.createElement('button');
    addRowButton.innerHTML = "Add row";
    addRowButton.setAttribute("class", "btn btn-primary");
    addRowButton.onclick = function () {
        table.setAttribute("rowCount", `${Number(table.getAttribute("rowCount")) + 1}`);
        let row = document.createElement('div');
        row.setAttribute("class", "row");
        let rowCount = table.getAttribute("rowCount");
        row.id = `row-${table.id}-${rowCount}`;
        row.setAttribute("cellCount", "0");
        let cellWithButton = document.createElement('div');
        cellWithButton.setAttribute("class", "col p-2 border");
        let addCellButton = document.createElement('button');
        addCellButton.innerHTML = "Add cell";
        addCellButton.setAttribute("class", "btn btn-primary");
        addCellButton.onclick = function () {
            let cellCount = Number(row.getAttribute("cellCount")) + 1;
            row.setAttribute("cellCount", `${cellCount}`);
            cell = document.createElement('div');
            cell.setAttribute("class", "col p-2 border");
            row.appendChild(cell);
            let inputTable = document.createElement('div');
            inputTable.setAttribute("class", "container");
            let inputRow = document.createElement('div');
            inputRow.setAttribute("class", "row");
            let inputCell = document.createElement('div');
            inputCell.setAttribute("class", "col-3");
            inputCell.innerHTML = "text";
            inputRow.appendChild(inputCell);
            inputCell = document.createElement('div');
            inputCell.setAttribute("class", "col");
            let inputText = document.createElement('input');
            inputText.setAttribute("class", "form-control");
            inputText.id = `inputText-${table.id}-${rowCount}-${cellCount}`;
            inputCell.appendChild(inputText);
            inputRow.appendChild(inputCell);
            inputTable.appendChild(inputRow);
            if (addValue == true) {
                inputRow.setAttribute("class", "row p-2 border-bottom");
                inputRow = document.createElement('div');
                inputRow.setAttribute("class", "row p-2 border-top");
                inputCell = document.createElement('div');
                inputCell.setAttribute("class", "col-3");
                inputCell.innerHTML = "url";
                inputRow.appendChild(inputCell);
                inputCell = document.createElement('div');
                inputCell.setAttribute("class", "col");
                let inputUrl = document.createElement('input');
                inputUrl.setAttribute("class", "form-control");
                inputUrl.id = `inputUrl-${table.id}-${rowCount}-${cellCount}`;
                inputCell.appendChild(inputUrl);
                inputRow.appendChild(inputCell);
                inputTable.appendChild(inputRow);
            }
            cell.appendChild(inputTable);
            cellWithButton.setAttribute("class", "col-3 p-2 border");
        };
        cellWithButton.appendChild(addCellButton);
        row.appendChild(cellWithButton);
        tbody.appendChild(row);
    };
    let addRowRow = document.createElement('div');
    addRowRow.setAttribute("class", "row");
    cell = document.createElement('div');
    cell.setAttribute("class", "col p-2 border");
    cell.appendChild(addRowButton);
    addRowRow.appendChild(cell);
    table.appendChild(addRowRow);
    let tfoot = document.createElement('div');
    tfoot.setAttribute("class", "row");
    tfoot.id = `tfoot-${table.id}`;
    table.appendChild(tfoot);

    return table;
}