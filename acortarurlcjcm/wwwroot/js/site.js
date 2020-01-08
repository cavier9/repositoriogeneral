var urlInput = document.querySelector("#urlshort");
var submitBtn = document.querySelector("#submit"); 
var respArea = document.querySelector("#resp-area");

urlInput.addEventListener('input', function (ev) {
    //TODO: add on change event logic
});

submitBtn.onclick = function(ev){
    if(!this.classList.contains("copiar")){
        let url = urlInput.value;
        if(!validateURL(url)){
            // TODO: let the user know what was wrong
            RespArea("No parece ser una URL válida");
            return null;
        }
        fetch("/",{
            method:"POST",
            body: JSON.stringify(url), 
            headers:{
                'Content-Type': 'application/json'
            }
        }).then(res => res.json())
        .then(response => {
            if (response.status === "URL ya existe") {
				urlInput.value = new URL(window.location) + response.token;
				submitBtn.innerHTML = "Copiar";
				submitBtn.classList.add("copiar");
			}
			else if (response.status === "acortado") {
				urlInput.value = "";
                RespArea("Ese enlace ya ha sido acortado");
            }else{
                console.log(response);
                urlInput.value = new URL(window.location)+response;
                submitBtn.innerHTML = "Copiar";
                submitBtn.classList.add("copiar");
            }
            
        }).catch(error=> console.log(error));
    }else{
        urlInput.select();
        document.execCommand("copy");
    }
}

function validateURL(url){
    var pattern = new RegExp('^(https?:\\/\\/)?'+ 
    '((([a-z\\d]([a-z\\d-]*[a-z\\d])*)\\.)+[a-z]{2,}|'+ 
    '((\\d{1,3}\\.){3}\\d{1,3}))'+ 
    '(\\:\\d+)?(\\/[-a-z\\d%_.~+]*)*'+
    '(\\?[;&a-z\\d%_.~+=-]*)?'+ 
    '(\\#[-a-z\\d_]*)?$','i'); 
  return !!pattern.test(url);
}

respArea.addEventListener("mouseout", function(ev){
    setTimeout(() => {
        respArea.classList.remove("active");
        respArea.classList.add("inactive");
    }, 1000);
    
})

respArea.onclick = function(ev){
    respArea.classList.remove("active");
    respArea.classList.add("inactive");
}

function RespArea(text){
    respArea.classList.remove("inactive");
    respArea.classList.add("active");
    respArea.innerHTML = text;
}