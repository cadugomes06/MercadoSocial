
const btnClose = document.querySelector('.close-alert');
const popUp = document.querySelector('.alert');
console.log(popUp);

if (btnClose != null && popUp != null) {
    btnClose.addEventListener('click',  closePopupScreen)
}

function closePopupScreen() {
    console.log("function desativar pop-up")
    popUp.classList.add("hide")
}





$(".close-alert").on('click', () => {
    $('.alert').attr({ "style": "display: none;" });
    })



