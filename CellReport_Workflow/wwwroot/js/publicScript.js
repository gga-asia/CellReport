window.openWarningDialog = function (message, timeout) {

    if (!timeout) {
        timeout = 0;
    }

    toastr.error("", message, {
        timeOut: timeout * 1000,
        progressBar: true,
        extendedTimeOut: 0
    });
}

window.openSuccessDialog = function (message, timeout) {


    toastr.success("", message, {
        timeOut: (timeout || 5) * 1000,
        progressBar: true,
        extendedTimeOut: 0
    });

}

window.SmartMessageBox = function (option) {
    var title = option.title || "";
    var message = option.message || "";
    var callback = option.callback;

    bootbox.confirm({
        title: title,
        message: "<span><strong>" + message + "</strong></span>",
        centerVertical: true,
        swapButtonOrder: true,
        buttons: {
            confirm: {
                label: "是",
                className: 'btn-danger shadow-0'
            },
            cancel: {
                label: "否",
                className: 'btn-default'
            }
        },
        className: "modal-alert",
        closeButton: false,
        callback: callback
    });
}

function TriggerFullScreen() {
    if (!document.fullscreenElement && !document.mozFullScreenElement && !document.webkitFullscreenElement && !document.msFullscreenElement) {

        if (document.documentElement.requestFullscreen) {
            /* Standard browsers */
            document.documentElement.requestFullscreen();
        } else if (document.documentElement.msRequestFullscreen) {
            /* Internet Explorer */
            document.documentElement.msRequestFullscreen();
        } else if (document.documentElement.mozRequestFullScreen) {
            /* Firefox */
            document.documentElement.mozRequestFullScreen();
        } else if (document.documentElement.webkitRequestFullscreen) {
            /* Chrome */
            document.documentElement.webkitRequestFullscreen(Element.ALLOW_KEYBOARD_INPUT);
        }
    }
}

function CloseFullScreen() {
    if (!document.fullscreenElement && !document.mozFullScreenElement && !document.webkitFullscreenElement && !document.msFullscreenElement) {
    } else {

        if (document.exitFullscreen) {
            document.exitFullscreen();
        } else if (document.msExitFullscreen) {
            document.msExitFullscreen();
        } else if (document.mozCancelFullScreen) {
            document.mozCancelFullScreen();
        } else if (document.webkitExitFullscreen) {
            document.webkitExitFullscreen();
        }
    }
}

function SetCookie(name, value, expire) {
    var now = new Date();

    if (!!expire) {
        try {
            now = new Date(expire);
        }
        catch (e) {
            console.warn(e);
        }
    }
    //沒設定就訂後一年
    if (!expire) {
        var time = now.getTime();
        var expireTime = time + 365 * 24 * 60 * 60 * 1000;
        now.setTime(expireTime);
    }

    var argv = SetCookie.arguments;
    var argc = SetCookie.arguments.length;
    var path = master.RootUrl;
    var domain = (argc > 4) ? argv[4] : null;
    var secure = (argc > 5) ? argv[5] : false;
    document.cookie = name + "=" + escape(value) + ";  expires=" + now.toGMTString()
        + ((path == null) ? "" : (";  path=" + path)) + ((domain == null) ? "" : (";  domain=" + domain))
        + ((secure == true) ? ";  secure" : "");
}

function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}