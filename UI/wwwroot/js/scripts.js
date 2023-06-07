async function SendRequest(urlEnd, requestType, data = null) {
    let request = {
        method: requestType,
        headers: {
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + jwtToken,
        }
    };
    if (requestType == RequestType.POST || requestType == RequestType.PUT) {
        request.body = JSON.stringify(data);
    }
    return await fetch(`${url}${urlEnd}`, request)
        .then(response => {
            if (response.status == 401) {
                window.location.replace(`login`);
            }
            return response;
        });
}

function showMessageBox(header, text, color = "#007aff") {

    let currentdate = new Date();
    let hours = currentdate.getHours();
    let minutes = currentdate.getMinutes();
    if (hours < 10) {
        hours = `0${hours}`;
    }
    if (minutes < 10) {
        minutes = `0${minutes}`;
    }
    let div = `<div class="toast fade show" role="alert" aria-live="assertive" aria-atomic="true">
                <div class="toast-header">
                    <svg class="bd-placeholder-img rounded me-2" width="20" height="20" xmlns="http://www.w3.org/2000/svg" aria-hidden="true" preserveAspectRatio="xMidYMid slice" focusable="false"><rect width="100%" height="100%" fill="${color}"></rect></svg>

                    <strong class="me-auto">${header}</strong>
                    <small class="text-muted">${hours}:${minutes}</small>
                    <button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Close"></button>
                </div>
                <div class="toast-body">
                    ${text}
                </div>
            </div>`;
    document.getElementById("toastMessages").innerHTML += div;
}