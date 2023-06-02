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
        }
        else if (commandActionType == CommandActionType.HttpPost) {
            table = GenerateSimpleTable(result[i].id, "Url");
        }
        else if (commandActionType == CommandActionType.InlineKeyboard) {
            table = GenerateButtonTable(result[i].id, true);
        }
        else if (commandActionType == CommandActionType.ReplyKeyboard) {
            table = GenerateButtonTable(result[i].id, false);
        }

        presentComandActions.appendChild(table);

        let content = JSON.parse(result[i].content);
        let inputTitle = document.getElementById(`inputTitle-${table.id}`);
        inputTitle.placeholder = "title";
        inputTitle.value = content.title;

        let tbody = document.getElementById(`tbody-${table.id}`);

        if (commandActionType == CommandActionType.InlineKeyboard || commandActionType == CommandActionType.ReplyKeyboard) {
            for (let j = 0; j < content.content.length; j++) {
                table.setAttribute("rowCount", `${Number(table.getAttribute("rowCount")) + 1}`);
                let row = document.createElement('tr');
                let rowCount = table.getAttribute("rowCount");
                row.id = `row-${table.id}-${rowCount}`;
                row.setAttribute("cellCount", "0");
                let cellWithButton = document.createElement('td');
                let addCellButton = document.createElement('button');
                addCellButton.innerHTML = "add cell";
                addCellButton.onclick = function () {
                    let cellCount = Number(row.getAttribute("cellCount")) + 1;
                    row.setAttribute("cellCount", `${cellCount}`);
                    let cell = document.createElement('td');
                    //cell.id = `cell-${table.id}-${rowCount}-${cellCount}`;
                    row.appendChild(cell);
                    let inputTable = document.createElement('table');
                    let inputTbody = document.createElement('tbody');
                    let inputRow = document.createElement('tr');
                    let inputCell = document.createElement('td');
                    inputCell.innerHTML = "text";
                    inputRow.appendChild(inputCell);
                    inputCell = document.createElement('td');
                    let inputText = document.createElement('input');
                    inputText.id = `inputText-${table.id}-${rowCount}-${cellCount}`;
                    inputCell.appendChild(inputText);
                    inputRow.appendChild(inputCell);
                    inputTbody.appendChild(inputRow);
                    inputRow = document.createElement('tr');
                    inputCell = document.createElement('td');
                    inputCell.innerHTML = "url";
                    inputRow.appendChild(inputCell);
                    inputCell = document.createElement('td');
                    let inputUrl = document.createElement('input');
                    inputUrl.id = `inputUrl-${table.id}-${rowCount}-${cellCount}`;
                    inputCell.appendChild(inputUrl);
                    inputRow.appendChild(inputCell);
                    inputTbody.appendChild(inputRow);
                    inputTable.appendChild(inputTbody);
                    cell.appendChild(inputTable);
                };

                cellWithButton.appendChild(addCellButton);
                row.appendChild(cellWithButton);
                for (let k = 0; k < content.content[j].length; k++) {
                    let cellCount = Number(row.getAttribute("cellCount")) + 1;
                    row.setAttribute("cellCount", cellCount);
                    let cell = document.createElement('td');
                    //cell.id = `cell-${table.id}-${rowCount}-${cellCount}`;
                    let inputTable = document.createElement('table');
                    let inputTbody = document.createElement('tbody');
                    let inputRow = document.createElement('tr');
                    let inputCell = document.createElement('td');
                    inputCell.innerHTML = "text";
                    inputRow.appendChild(inputCell);
                    inputCell = document.createElement('td');
                    let inputText = document.createElement('input');
                    inputText.id = `inputText-${table.id}-${rowCount}-${cellCount}`;
                    inputText.value = content.content[j][k].text;
                    inputCell.appendChild(inputText);
                    inputRow.appendChild(inputCell);
                    inputTbody.appendChild(inputRow);
                    inputRow = document.createElement('tr');
                    inputCell = document.createElement('td');
                    inputCell.innerHTML = "url";
                    inputRow.appendChild(inputCell);
                    inputCell = document.createElement('td');
                    let inputUrl = document.createElement('input');
                    inputUrl.id = `inputUrl-${table.id}-${rowCount}-${cellCount}`;
                    inputUrl.value = content.content[j][k].url;
                    inputCell.appendChild(inputUrl);
                    inputRow.appendChild(inputCell);
                    inputTbody.appendChild(inputRow);
                    inputTable.appendChild(inputTbody);
                    cell.appendChild(inputTable);
                    row.appendChild(cell);
                }

                tbody.appendChild(row);
            }
        }
        else {

            var inputText = document.getElementById(`inputValue-${table.id}`);
            inputText.value = content.content;
            //let row = document.createElement('tr');
            //row.id = `row-${table.id}`;
            //let inputCell = document.createElement('td');

            //if(commandActionType == CommandActionType.Text)
            //    inputCell.innerHTML = "text";
            //else if(commandActionType == CommandActionType.HttpPost)
            //    inputCell.innerHTML = "url";

            //row.appendChild(inputCell);
            //inputCell = document.createElement('td');
            //let inputText = document.createElement('input');
            //inputText.id = `inputValue-${table.id}`;
            //inputCell.appendChild(inputText);
            //row.appendChild(inputCell);
            //tbody.appendChild(row);
        }

        let tfoot = document.getElementById(`tfoot-${table.id}`);
        var updateButton = document.createElement('button');
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
        tfoot.appendChild(updateButton);
        let deleteButton = document.createElement('button');
        deleteButton.innerHTML = "Delete";
        deleteButton.onclick = async () => {
            await DeleteCommandAction(result[i].id);
            document.getElementById(buttonId).click();
        };
        tfoot.appendChild(deleteButton);
    }

    //var table = GenerateButtonTable(1, true);
    //presentComandActions.appendChild(table);

    //Setup newComandActions
    newComandActions.setAttribute("tableCount", 0);
    var addTableButton = document.createElement('button');
    addTableButton.innerHTML = "add table";
    addTableButton.onclick = function () {
        let tableCount = Number(newComandActions.getAttribute("tableCount")) + 1;
        newComandActions.setAttribute("tableCount", `${tableCount}`);
        let table;

        if (commandActionType == CommandActionType.Text) {
            table = GenerateSimpleTable(tableCount, "Text");
        }
        else if (commandActionType == CommandActionType.HttpPost) {
            table = GenerateSimpleTable(tableCount, "Url");
        }
        else if (commandActionType == CommandActionType.InlineKeyboard) {
            table = GenerateButtonTable(tableCount, true);
        }
        else if (commandActionType == CommandActionType.ReplyKeyboard) {
            table = GenerateButtonTable(tableCount, false);
        }

        newComandActions.appendChild(table);
        let tfid = `tfoot-table-${tableCount}`;
        let tfoot = document.getElementById(tfid);
        let saveButton = document.createElement('button');
        saveButton.innerHTML = "Save";
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
        tfoot.appendChild(saveButton);
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
        .then(response => {
            if (response.ok) {
                showMessageBox("Success", "CommandAction created successfully");
            }
        })
        .catch(ex => { showMessageBox(`Error when save CommandAction ${ex.json}`, ex, '#d70040'); });
};

const UpdateCommandAction = async (id, content) => {
    await SendRequest(`api/CommandAction/${id}`, RequestType.PUT, content)
        .then(response => {
            if (response.ok) {
                showMessageBox("Success", "CommandAction updated successfully");
            }
        })
        .catch(ex => { showMessageBox(`Error when update CommandAction ${ex.json}`, ex, '#d70040'); });
}

const DeleteCommandAction = async (id) => {
    await SendRequest(`api/CommandAction/${id}`, RequestType.DELETE)
        .then(response => {
            if (response.ok) {
                showMessageBox("Success", "CommandAction deleted successfully");
            }
        })
        .catch(ex => { showMessageBox(`Error when delete CommandAction ${ex.json}`, ex, '#d70040'); });
}

function GenerateSimpleTable(tableId, valueName){
    let table = document.createElement('table');
    table.id = `table-${tableId}`;
    table.border = "1";
    let thead = document.createElement('thead');
    let inputTitle = document.createElement('input');
    inputTitle.id = `inputTitle-${table.id}`;
    inputTitle.placeholder = "title";
    thead.appendChild(inputTitle);
    thead.setAttribute("colspan", "100%");
    table.appendChild(thead);
    let tbody = document.createElement('tbody');
    tbody.id = `tbody-${table.id}`;
    table.appendChild(tbody);
    let row = document.createElement('tr');
    let cell = document.createElement('td');
    cell.innerHTML = valueName;
    row.appendChild(cell);
    inputCell = document.createElement('td');
    let inputText = document.createElement('input');
    inputText.id = `inputValue-${table.id}`;
    cell.appendChild(inputText);
    row.appendChild(cell);
    tbody.appendChild(row);
    let tfoot = document.createElement('tfoot');
    tfoot.id = `tfoot-${table.id}`;
    tfoot.setAttribute("colspan", "100%");
    table.appendChild(tfoot);

    return table;
}

function GenerateButtonTable(tableId, addValue) {
    let table = document.createElement('table');
    table.id = `table-${tableId}`;
    table.setAttribute("rowCount", "0");
    table.border = "1";
    let thead = document.createElement('thead');
    let inputTitle = document.createElement('input');
    inputTitle.id = `inputTitle-${table.id}`;
    inputTitle.placeholder = "title";
    thead.appendChild(inputTitle);
    thead.setAttribute("colspan", "100%");
    table.appendChild(thead);
    let tbody = document.createElement('tbody');
    tbody.id = `tbody-${table.id}`;
    table.appendChild(tbody);
    let addRowButton = document.createElement('button');
    addRowButton.innerHTML = "add row";
    addRowButton.onclick = function () {
        table.setAttribute("rowCount", `${Number(table.getAttribute("rowCount")) + 1}`);
        let row = document.createElement('tr');
        let rowCount = table.getAttribute("rowCount");
        row.id = `row-${table.id}-${rowCount}`;
        row.setAttribute("cellCount", "0");
        let cellWithButton = document.createElement('td');
        let addCellButton = document.createElement('button');
        addCellButton.innerHTML = "add cell";
        addCellButton.onclick = function () {
            let cellCount = Number(row.getAttribute("cellCount")) + 1;
            row.setAttribute("cellCount", `${cellCount}`);
            let cell = document.createElement('td');
            //cell.id = `cell-${table.id}-${rowCount}-${cellCount}`;
            row.appendChild(cell);
            let inputTable = document.createElement('table');
            let inputTbody = document.createElement('tbody');
            let inputRow = document.createElement('tr');
            let inputCell = document.createElement('td');
            inputCell.innerHTML = "text";
            inputRow.appendChild(inputCell);
            inputCell = document.createElement('td');
            let inputText = document.createElement('input');
            inputText.id = `inputText-${table.id}-${rowCount}-${cellCount}`;
            inputCell.appendChild(inputText);
            inputRow.appendChild(inputCell);
            inputTbody.appendChild(inputRow);
            if (addValue == true) {
                inputRow = document.createElement('tr');
                inputCell = document.createElement('td');
                inputCell.innerHTML = "url";
                inputRow.appendChild(inputCell);
                inputCell = document.createElement('td');
                let inputUrl = document.createElement('input');
                inputUrl.id = `inputUrl-${table.id}-${rowCount}-${cellCount}`;
                inputCell.appendChild(inputUrl);
                inputRow.appendChild(inputCell);
                inputTbody.appendChild(inputRow);
            }
            inputTable.appendChild(inputTbody);
            cell.appendChild(inputTable);
        };
        cellWithButton.appendChild(addCellButton);
        row.appendChild(cellWithButton);
        tbody.appendChild(row);
    };
    let tfoot = document.createElement('tfoot');
    tfoot.appendChild(addRowButton);
    tfoot.id = `tfoot-${table.id}`;
    tfoot.setAttribute("colspan", "100%");
    table.appendChild(tfoot);

    return table;
}