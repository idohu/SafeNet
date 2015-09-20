var defaultLanguage = "eng";
function loadOptions() {
	var lang = localStorage["language"];

    if (lang == undefined || (lang != "eng" && lang != "heb" && lang!='rus' && lang !='arb')) {
        var lng=window.navigator.userLanguage || window.navigator.language;
        switch (lng)
        {
            case 'en‐US':
            case 'en‐AU':
            case 'en-BZ':
            case 'en‐CA':
            case 'en‐CB':
            case 'en‐IE':
            case 'en‐JM':
            case 'en‐NZ':
            case 'en‐PH':
            case 'en‐ZA':
            case 'en‐TT':
            case 'en‐GB':
            case 'en‐ZW':
                lang = 'eng';
                break;
            case 'he-IL':
                lang = 'heb';
                break;
            case 'ru‐RU':
                lang='rus';
                break;
            case 'ar-DZ':
            case 'ar-BH':
            case 'ar-EG':
            case 'ar-IQ':
            case 'ar-JO':
            case 'ar-KW':
            case 'ar-LB':
            case 'ar-LY':
            case 'ar-MA':
            case 'ar-OM':
            case 'ar-QA':
            case 'ar-SA':
            case 'ar-SY':
            case 'ar-TN':
            case 'ar-AE':
            case 'ar-YE':
                lang='arb';
                break;
            default:
                lang=defaultLanguage;
                break;


        }

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
    localStorage['side']='ltr';
    if (lang == 'arb' || lang == 'heb')
    localStorage['side']='rtl';
	localStorage["AutoCheck"] = document.getElementById("auto").checked;
}

function eraseOptions() {
	localStorage.removeItem("language");
	location.reload();
}

window.addEventListener('load', loadOptions);
document.getElementById("save").addEventListener("click",saveOptions);
document.getElementById("reset").addEventListener("click",eraseOptions);