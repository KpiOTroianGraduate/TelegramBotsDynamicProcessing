const GetTelegramBots = async () => {
    let result = await SendRequest(`api/TelegramBot`, RequestType.GET)
        .then(response => { return response.json(); })
        .catch(ex => showMessageBox("Error", ex, '#d70040'));

    console.log(result);
    var presentBots = document.getElementById("presentTelegramBots");

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
        presentBots.appendChild(table);
    }
}