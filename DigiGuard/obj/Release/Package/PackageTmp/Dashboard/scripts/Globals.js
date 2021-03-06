﻿// VARIABLES
var prefix = "http://";
var server = "localhost:2314/";//"watcher-m852y043.cloudapp.net/";//"watcher-g4536865.cloudapp.net/";//"localhost:2314/";//

// Functions
if (readCookie("WatcherUser") == null) {
    if (window.location.pathname.indexOf("/login.html")==-1)
        window.location = "login.html";
}

function LogOut() {
    createCookie("WatcherUser", "", -1);
    window.location = "login.html";
}

function readCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}

function createCookie(name, value, days) {
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        var expires = "; expires=" + date.toGMTString();
    }
    else var expires = "";
    document.cookie = name + "=" + value + expires + "; path=/";
}