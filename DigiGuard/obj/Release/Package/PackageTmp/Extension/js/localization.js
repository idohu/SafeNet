var defaultLanguage = "eng";
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
var strings = (function() {
        var json = null;	 
		 var xhr = new XMLHttpRequest();
xhr.onreadystatechange = function()
		{
			if (xhr.readyState === XMLHttpRequest.DONE) {
				if (xhr.status === 200) {
						json = JSON.parse(xhr.responseText);
						document.getElementById("fname").placeholder = json.firstName;
						document.getElementById("lname").placeholder = json.lastName;
						document.getElementById("phone").placeholder = json.phone;
						document.getElementById("email").placeholder = json.Email;
						document.getElementById("location").placeholder = json.Location;
						document.getElementById("desc").placeholder = json.Description;
						document.getElementById("cc").text = json.Category;
						document.getElementById("abuse").text = json.Abuse;
						document.getElementById("shaming").text = json.Shaming;
						document.getElementById("incitment").text = json.Incitment;
						document.getElementById("suicide").text = json.Suicide;
						document.getElementById("pedophile").text = json.Pedophile;
						document.getElementById("terror").text = json.Terror;
						document.getElementById("invalid").innerHTML = json.Invalid;
						document.getElementById("uh-oh").innerHTML = json.uhoh;
					}

				}
		 }; 
xhr.open("GET", chrome.extension.getURL('js/languages/'+lang+'.json'), true);
xhr.send();
		 
         // $.ajax({
             // 'async': false,
             // 'global': false,
             // 'url': "languages/"+lang+".json",
             // 'dataType': "json",
             // 'success': function (data) {
                 // json = data;
             // },
			 // 'error':function (xhr,textStatus,errorThrown){
				 
			 // }
         // });
        return json;
    })();
/* document.getElementById("fname").placeholder = strings.firstName;
document.getElementById("lname").placeholder = strings.lastName;
document.getElementById("phone").placeholder = strings.phone;
document.getElementById("email").placeholder = strings.Email;
document.getElementById("location").placeholder = strings.Location;
document.getElementById("desc").placeholder = strings.Description;
document.getElementById("cc").placeholder = strings.Category;
document.getElementById("abuse").placeholder = strings.Abuse;
document.getElementById("shaming").placeholder = strings.Shaming;
document.getElementById("incitment").placeholder = strings.Incitment;
document.getElementById("suicide").placeholder = strings.Suicide;
document.getElementById("pedophile").placeholder = strings.Pedophile;
document.getElementById("terror").placeholder = strings.Terror;
document.getElementById("invalid").placeholder = strings.Invalid;
document.getElementById("uh-oh").placeholder = strings.uh-oh; */

if (localStorage['side']!=undefined)
    document.getElementById("loginForm").style.direction = localStorage['side'];