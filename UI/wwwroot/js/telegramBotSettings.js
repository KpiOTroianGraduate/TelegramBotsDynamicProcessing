document.getElementById("TelegramBotSettings").onclick = async () => {
    await GetBotDescription();
}

const GetBotDescription = async () => {
    let presentComandActions = document.getElementById("presentComandActions");
    presentComandActions.innerHTML = "";
    let newComandActions = document.getElementById("newComandActions");
    newComandActions.innerHTML = "";

    let result = await SendRequest(`api/TelegramBot/${telegramBotId}/description`, RequestType.GET)
        .then(response => { return response.json(); })
        .catch(ex => { showMessageBox(`Error while getting TelrgramBot description`, ex, '#d70040'); });
    console.log(result);

    let table = document.createElement('div');
    table.setAttribute("class", "container p-4");
    let thead = document.createElement('div');
    thead.setAttribute("class", "row");
    let cell = document.createElement('div');
    cell.setAttribute("class", "col p-2 border text-center");
    cell.innerHTML = `@${result.userName}`;
    thead.appendChild(cell);
    table.appendChild(thead);
    let row = document.createElement('div');
    row.setAttribute("class", "row");
    cell = document.createElement('div');
    cell.setAttribute("class", "col-3 p-2 border text-center");
    cell.innerHTML = "Name";
    row.appendChild(cell);
    cell = document.createElement('div');
    cell.setAttribute("class", "col p-2 border text-center");
    input = document.createElement('input');
    input.id = `${telegramBotId}-name`;
    input.setAttribute("class", "form-control");
    input.value = result.name;
    input.placeholder = "Telegram Bot Name";
    cell.appendChild(input);
    row.appendChild(cell);
    cell = document.createElement('div');
    cell.setAttribute("class", "col-3 p-2 border text-center");
    let updateButton = document.createElement('button');
    updateButton.innerHTML = "Update";
    updateButton.setAttribute("class", "btn btn-primary");
    updateButton.onclick = async () => {
        let value = document.getElementById(`${telegramBotId}-name`).value;
        await SendRequest(`api/TelegramBot/${telegramBotId}/name`, RequestType.PUT, new Value(value))
            .then(async response => {
                showMessageBox("Success", "TelrgramBot name updated successfully");
                await GetBotDescription();
            })
            .catch(ex => { showMessageBox(`Error while updating TelrgramBot name`, ex, '#d70040'); });

    };
    cell.appendChild(updateButton);
    row.appendChild(cell);
    table.appendChild(row);
    row = document.createElement('div');
    row.setAttribute("class", "row");
    cell = document.createElement('div');
    cell.setAttribute("class", "col-3 p-2 border text-center");
    cell.innerHTML = "Short description";
    row.appendChild(cell);
    cell = document.createElement('div');
    cell.setAttribute("class", "col p-2 border text-center");
    input = document.createElement('input');
    input.id = `${telegramBotId}-short-description`;
    input.setAttribute("class", "form-control");
    input.placeholder = "Telegram Bot Short Description";
    input.value = result.shortDescription;
    cell.appendChild(input);
    row.appendChild(cell);
    cell = document.createElement('div');
    cell.setAttribute("class", "col-3 p-2 border text-center");
    updateButton = document.createElement('button');
    updateButton.innerHTML = "Update";
    updateButton.setAttribute("class", "btn btn-primary");
    updateButton.onclick = async () => {
        let value = document.getElementById(`${telegramBotId}-short-description`).value;
        await SendRequest(`api/TelegramBot/${telegramBotId}/shortDescription`, RequestType.PUT, new Value(value))
            .then(async response => {
                showMessageBox("Success", "TelrgramBot short description updated successfully");
                await GetBotDescription();
            })
            .catch(ex => { showMessageBox(`Error while updating TelrgramBot short description`, ex, '#d70040'); });

    };
    cell.appendChild(updateButton);
    row.appendChild(cell);
    table.appendChild(row);
    row = document.createElement('div');
    row.setAttribute("class", "row");
    cell = document.createElement('div');
    cell.setAttribute("class", "col-3 p-2 border text-center");
    cell.innerHTML = "Description";
    row.appendChild(cell);
    cell = document.createElement('div');
    cell.setAttribute("class", "col p-2 border text-center");
    input = document.createElement('input');
    input.id = `${telegramBotId}-description`;
    input.setAttribute("class", "form-control");
    input.placeholder = "Telegram Bot Description";
    input.value = result.description;
    cell.appendChild(input);
    row.appendChild(cell);
    cell = document.createElement('div');
    cell.setAttribute("class", "col-3 p-2 border text-center");
    updateButton = document.createElement('button');
    updateButton.innerHTML = "Update";
    updateButton.setAttribute("class", "btn btn-primary");
    updateButton.onclick = async () => {
        let value = document.getElementById(`${telegramBotId}-description`).value;
        await SendRequest(`api/TelegramBot/${telegramBotId}/description`, RequestType.PUT, new Value(value))
            .then(async response => {
                showMessageBox("Success", "TelrgramBot description updated successfully");
                await GetBotDescription();
            })
            .catch(ex => { showMessageBox(`Error while updating TelrgramBot description`, ex, '#d70040'); });

    };
    cell.appendChild(updateButton);
    row.appendChild(cell);
    table.appendChild(row);
    presentComandActions.appendChild(table);
}