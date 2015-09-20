document.onreadystatechange = function () {
    //When document is fully loaded
    try{

        if (document.readyState == "complete") {
            var auto = localStorage["AutoCheck"];
            if (auto=="true"){
                //Add the json word file
                var words = strWords;
                var count = 0;
                //Search each word in the document
                for (var i=0;i<words.length;i++){
                    if (window.find(words[i])){
                        count++;
                    }
                }
                if (count >3){
                    //Add Ajax call here
                    //Send document.documentElement.innerHTML
                }
                //	});
            }
        }
    }
    catch(err) {
        alert(err.message);
    }
}