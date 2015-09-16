// Copyright (c) 2012,2013 Peter Coles - http://mrcoles.com/ - All rights reserved.
// Use of this source code is governed by the MIT License found in LICENSE
//
// console object for debugging
//
var E={};
var log = (function() {
    var parElt = document.getElementById('wrap'),
        logElt = document.createElement('div');
    logElt.id = 'log';
    logElt.style.display = 'block';
    parElt.appendChild(logElt);

    return function() {
        var a, p, results = [];
        for (var i=0, len=arguments.length; i<len; i++) {
            a = arguments[i];
            try {
                a = JSON.stringify(a, null, 2);
            } catch(e) {}
            results.push(a);
        }
        p = document.createElement('p');
        p.innerText = results.join(' ');
        p.innerHTML = p.innerHTML.replace(/ /g, '&nbsp;');
        logElt.appendChild(p);
    };
})();

//
// utility methods
//
function $(id) { return document.getElementById(id); }
function show(id) { $(id).style.display = 'block'; }
function hide(id) { $(id).style.display = 'none'; }

//
// URL Matching test - to verify we can talk to this URL
//
var matches = ['http://*/*', 'https://*/*', 'ftp://*/*', 'file://*/*'],
    noMatches = [/^https?:\/\/chrome.google.com\/.*$/];
function testURLMatches(url) {
    // couldn't find a better way to tell if executeScript
    // wouldn't work -- so just testing against known urls
    // for now...
    var r, i;
    for (i=noMatches.length-1; i>=0; i--) {
        if (noMatches[i].test(url)) {
            return false;
        }
    }
    for (i=matches.length-1; i>=0; i--) {
        r = new RegExp('^' + matches[i].replace(/\*/g, '.*') + '$');
        if (r.test(url)) {
            return true;
        }
    }
    return false;
}

//
// Events
//
var screenshot, contentURL = '';

function sendScrollMessage(tab) {
    contentURL = tab.url;
    screenshot = {};
    chrome.tabs.sendRequest(tab.id, {msg: 'scrollPage'}, function() {
        // We're done taking snapshots of all parts of the window. Display
        // the resulting full screenshot image in a new browser tab.
        openPage();
    });
}

/* A function creator for callbacks */
function doStuffWithDOM(domContent) {
    console.log("I received the following DOM content:\n" + domContent);
}

/* When the browser-action button is clicked... */
chrome.browserAction.onClicked.addListener(function(tab) {
    /*...check the URL of the active tab against our pattern and... */
   // if (urlRegex.test(tab.url)) {
        /* ...if it matches, send a message specifying a callback too */
        chrome.tabs.sendMessage(tab.id, { text: "report_back" },
                                doStuffWithDOM);
    //}
});

function sendLogMessage(data) {
    chrome.tabs.getSelected(null, function(tab) {
        chrome.tabs.sendRequest(tab.id, {msg: 'logMessage', data: data}, function() {});
    });
}

chrome.extension.onRequest.addListener(function(request, sender, callback) {
    if (request.msg === 'capturePage') {
        capturePage(request, sender, callback);
    } else {
        console.error('Unknown message received from content script: ' + request.msg);
    }
});


function capturePage(data, sender, callback) {
    var canvas;
  //  $('bar').style.width = parseInt(data.complete * 100, 10) + '%';
    E.DOM=data.html;
	
    // Get window.devicePixelRatio from the page, not the popup
    var scale = data.devicePixelRatio && data.devicePixelRatio !== 1 ?
        1 / data.devicePixelRatio : 1;

    // if the canvas is scaled, then x- and y-positions have to make
    // up for it
    if (scale !== 1) {
        data.x = data.x / scale;
        data.y = data.y / scale;
        data.totalWidth = data.totalWidth / scale;
        data.totalHeight = data.totalHeight / scale;
    }


    if (!screenshot.canvas) {
       

        // sendLogMessage('TOTALDIMENSIONS: ' + data.totalWidth + ', ' + data.totalHeight);

        // // Scale to account for device pixel ratios greater than one. (On a
        // // MacBook Pro with Retina display, window.devicePixelRatio = 2.)
        // if (scale !== 1) {
        //     // TODO - create option to not scale? It's not clear if it's
        //     // better to scale down the image or to just draw it twice
        //     // as large.
        //     screenshot.ctx.scale(scale, scale);
        // }
    }

    // sendLogMessage(data);

    chrome.tabs.captureVisibleTab(
        null, {format: 'png', quality: 100}, function(dataURI) {
            if (dataURI) {
                var image = new Image();
                image.onload = function() {
                    // sendLogMessage('img dims: ' + image.width + ', ' + image.height);
                    canvas = document.createElement('canvas');
                    canvas.width = image.width;
                    canvas.height =image.height;
                    screenshot.canvas = canvas;
                    screenshot.ctx = canvas.getContext('2d');
                    screenshot.ctx.drawImage(image,0, 0);
                    callback(true);
                };
                image.src = dataURI;
            }
        });
}

function openPage() {
    // standard dataURI can be too big, let's blob instead
    // http://code.google.com/p/chromium/issues/detail?id=69227#c27

    var dataURI = screenshot.canvas.toDataURL();

    // convert base64 to raw binary data held in a string
    // doesn't handle URLEncoded DataURIs
    var byteString = atob(dataURI.split(',')[1]);

    // separate out the mime component
    var mimeString = dataURI.split(',')[0].split(':')[1].split(';')[0];

    // write the bytes of the string to an ArrayBuffer
    var ab = new ArrayBuffer(byteString.length);
    var ia = new Uint8Array(ab);
    for (var i = 0; i < byteString.length; i++) {
        ia[i] = byteString.charCodeAt(i);
    }
	SendData(dataURI.split(',')[1]);
    // create a blob for writing to a file
    var blob = new Blob([ab], {type: mimeString});

    // come up with file-system size with a little buffer
    var size = blob.size + (1024/2);

    // come up with a filename
    var name = contentURL.split('?')[0].split('#')[0];
    if (name) {
        name = name
            .replace(/^https?:\/\//, '')
            .replace(/[^A-z0-9]+/g, '-')
            .replace(/-+/g, '-')
            .replace(/^[_\-]+/, '')
            .replace(/[_\-]+$/, '');
        name = '-' + name;
    } else {
        name = '';
    }
    name = 'screencapture' + name + '-' + Date.now() + '.png';

    function onwriteend() {
        // open the file that now contains the blob
     //   window.open('filesystem:chrome-extension://' + chrome.i18n.getMessage('@@extension_id') + '/temporary/' + name);
    }

    function errorHandler() {
        show('uh-oh');
    }

    // create a blob for writing to a file
    window.webkitRequestFileSystem(window.TEMPORARY, size, function(fs){
        fs.root.getFile(name, {create: true}, function(fileEntry) {
            fileEntry.createWriter(function(fileWriter) {
                fileWriter.onwriteend = onwriteend;
                fileWriter.write(blob);
            }, errorHandler);
        }, errorHandler);
    }, errorHandler);
}

//
// start doing stuff immediately! - including error cases
//
document.getElementById("btn_report").addEventListener("click", function(){
    hide("main");
    show("loader");
chrome.tabs.getSelected(null, function(tab) {

    if (testURLMatches(tab.url)) {
        var loaded = false;
        E.url=tab.url;
        chrome.tabs.executeScript(tab.id, {file: 'page.js'}, function() {
            loaded = true;
       //     show('loading');
            sendScrollMessage(tab);
        });
		chrome.tabs.sendMessage(tab.id, { text: "report_back" },
                                doStuffWithDOM);
        window.setTimeout(function() {
            if (!loaded) {
                show('uh-oh');
            }
        }, 1000);
    } else {
        show('invalid');
    }
});
});

function SendData(imgURL) {
    
    document.getElementById('btn_report').disabled = true;

    E.category = document.getElementById('category').value == "" ? -1 : document.getElementById('category').value;
    E.fname = document.getElementById('fname').value;
    E.lname = document.getElementById('lname').value;
    E.phone = document.getElementById('phone').value;
    E.email=document.getElementById('email').value;
    E.desc=document.getElementById('desc').value;
    E.location=document.getElementById('location').value;
    var data=JSON.stringify({"url": E.url,"img":imgURL,"dom": E.DOM,"category": E.category,"name": E.fname,"lName": E.lname,
    "phone": E.phone,"email": E.email,"description": E.desc,"location": E.location});
    //var url = "http://localhost:2314/ClientService.asmx";
    var url = "http://watcher-g4536865.cloudapp.net/watcher/ClientService.asmx";
    var flag = false;
    var xhr = new XMLHttpRequest();
	xhr.onreadystatechange = function(data) {
			console.log(data)
        if(flag){
            document.getElementById('btn_report').disabled = false;
            hide("loader");
            show("main");
            window.close();
        }
        flag=true;
    };
	xhr.open("POST", url + "/PostReport", true);
	xhr.setRequestHeader("Content-Type","application/json; charset=utf-8");
	xhr.send(data);
}

document.getElementById("settingsImg").addEventListener("click",openOptionsPage);
function openOptionsPage() {
    chrome.tabs.create({'url': "/options.html"});
}


