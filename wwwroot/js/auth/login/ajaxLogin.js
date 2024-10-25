var xmlhttp = false;
try {
    xmlhttp = new ActiveXObject("Msxml2.XMLHTTP");
    console.log("You are using MS IE.");
} catch (e) {
    xmlhttp = false;
}

if (!xmlhttp && typeof XMLHttpRequest != "undefined") {
    xmlhttp = new XMLHttpRequest();
    console.log("You are not using MS IE");
}

function makeRequest(method, serverPage, objID) {
    const obj = document.querySelector(objID);
    const formData = new FormData(obj);
    const json = JSON.stringify({
        EmailOrUsername: formData.get('EmailOrUsername'),
        Password: formData.get('Password')
    });
    xmlhttp.open(method, serverPage, true);
    xmlhttp.setRequestHeader(
        'Content-type', 'application/json'
        );
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4 ) {
            if (xmlhttp.status == 200) {
                window.location.href = `${window.location.origin}/Home`
            }
            else {
                document.documentElement.innerHTML = xmlhttp.responseText;
            }
        }
    };
    xmlhttp.send(json);
}
