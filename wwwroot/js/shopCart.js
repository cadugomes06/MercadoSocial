

var NumShopCart;
var productsOnShopCart = [];

$(document).ready(function () {
    NumShopCart = parseInt(localStorage.getItem("NumShopCart")) || 0;
    productsOnShopCart = JSON.parse(localStorage.getItem("itemsShopCart")) || [];
    console.log("teste: ", productsOnShopCart);

    $("#countShopCart").text(NumShopCart);

    $(".addItemOnShopCart").on('click', function () {
        var productID = $(this).data('id');
        addItemOnShopCart(productID);
    });
})


function addItemOnShopCart(productID) {
    if (productsOnShopCart.includes(productID)) {
        alert("Ops... O item já foi adicionado");
        return;
    }

    productsOnShopCart.push(productID);
    localStorage.setItem("itemsShopCart", JSON.stringify(productsOnShopCart));
    console.log("Array: ", productsOnShopCart);
    countShopCart();
}


function countShopCart() {
    NumShopCart = NumShopCart + 1;
    $("#countShopCart").text(NumShopCart);
    localStorage.setItem("NumShopCart", NumShopCart);
}


$(document).ready(function () {
    $(".takeItemsToShopCart").on('click', function () {
        var productsIds = JSON.parse(localStorage.getItem("itemsShopCart"));
        console.log(productsIds);

        fillShopCartPage(productsIds);
    });
})

async function fillShopCartPage(productsIds) {
    if (!productsIds || productsIds.length === 0) {
        alert("Adicione um produto primeiro");
        return;
    }

    const query = productsIds.map(id => `productsIds=${encodeURIComponent(id)}`).join('&');
    const url = `/Product/ShopCart?${query}`

    window.location.href = url;
}


$(".addQuantity").on('click', function () {
    var input = $(this).closest("#containerQuantityCart").find("#inputQuantityItem");

    var inputValue = parseInt(input.val()) || 1;

    if (inputValue < 10) {
        inputValue++;
        input.val(inputValue);
    }
});

$(".removeQuantity").on('click', function () {
    var input = $(this).closest("#containerQuantityCart").find("#inputQuantityItem");

    var inputValue = parseInt(input.val()) || 1;

    if (inputValue > 1 ) {
        inputValue--;
        input.val(inputValue);
    }
});

$(".btnRemoveItemCart").on('click', function () {
    console.log("deletar item...");
})