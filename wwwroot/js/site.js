
const btnClose = document.querySelector('.close-alert');
const popUp = document.querySelector('.alert');
console.log(popUp);

if (btnClose != null && popUp != null) {
    btnClose.addEventListener('click',  closePopupScreen)
}

function closePopupScreen() {
    popUp.classList.add("hide")
}



