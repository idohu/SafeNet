var defaultLanguage = "eng";
function loadOptions() {
	var lang = localStorage["language"];

	// valid colors are red, blue, green and yellow
	if (lang == undefined || (lang != "eng" && lang != "heb")) {
		lang = defaultLanguage;
	}

	var select = document.getElementById("language");
	for (var i = 0; i < select.children.length; i++) {
		var child = select.children[i];
			if (child.value == lang) {
			child.selected = "true";
			break;
		}
	}
	var auto = localStorage["AutoCheck"];
	if (auto=="true" || auto=="false")
	document.getElementById("auto").checked = JSON.parse(auto);
}

function saveOptions() {
	var select = document.getElementById("language");
	var lang = select.children[select.selectedIndex].value;
	localStorage["language"] = lang;
	localStorage["AutoCheck"] = document.getElementById("auto").checked;
}

function eraseOptions() {
	localStorage.removeItem("language");
	location.reload();
}

window.addEventListener('load', loadOptions);
document.getElementById("save").addEventListener("click",saveOptions);
document.getElementById("reset").addEventListener("click",eraseOptions);