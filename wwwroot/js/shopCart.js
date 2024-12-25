

var shopCart;
var productsOnShopCart = [];

$(document).ready(function () {
    shopCart = parseInt(localStorage.getItem("shopCart")) || 0;
    productsOnShopCart = JSON.parse(localStorage.getItem("itemsShopCart")) || [];
    console.log("teste: ", productsOnShopCart);

    $("#countShopCart").text(shopCart);

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
    shopCart = shopCart + 1;
    $("#countShopCart").text(shopCart);
    localStorage.setItem("shopCart", shopCart);
}

