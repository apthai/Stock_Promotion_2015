
function OpenColorBox(_url) {
    $.colorbox({
        href: _url,
        iframe: true,
        width: "90%",
        height: "90%",
        transition: "none",
        opacity: 0.6
    });
}


function OpenColorBox(_url,w,h) {
    $.colorbox({
        href: _url,
        iframe: true,
        width: w,
        height: h,
        transition: "none",
        opacity: 0.6
    });
}

function Popup60(url) {
    var scr_H = screen.height;
    var scr_W = screen.width;
    var h = (scr_H / 100 * 80);
    var w = (scr_W / 100 * 60);
    var t = (scr_H / 100 * 5);
    var l = (scr_W / 100 * 25);
    window.open(url, '', 'height=' + h + ',width=' + w + ',top=' + t + ',left=' + l + ',status=yes,scrollbars=no,resizable=yes,menubar=no;toolbar=no,titlebar=no');
}

function Popup80(url) {
    var scr_H = screen.height;
    var scr_W = screen.width;
    var h = (scr_H / 100 * 80);
    var w = (scr_W / 100 * 90);
    var t = (scr_H / 100 * 5);
    var l = (scr_W / 100 * 5);
    window.open(url, '', 'height=' + h + ',width=' + w + ',top=' + t + ',left=' + l + ',status=yes,scrollbars=no,resizable=yes,menubar=no;toolbar=no,titlebar=no');
}

function PopupFullScr(url) {
    if (mypopUp != undefined) {
        mypopUp.close();
    }
    var scr_H = screen.height;
    var scr_W = screen.width;
    var h = (scr_H);
    var w = (scr_W);
    var t = 0;
    var l = 0;
    myWindow = window.open(url, '', 'height=' + h + ',width=' + w + ',top=' + t + ',left=' + l + ',status=yes,scrollbars=no,resizable=yes,menubar=no;toolbar=no,titlebar=no');
}