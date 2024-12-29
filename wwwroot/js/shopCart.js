

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


$(document).ready(function () {
    $(".takeItemsToShopCart").on('click', function () {
        var productsIds = JSON.parse(localStorage.getItem("itemsShopCart"));
        console.log(productsIds);

        fillShopCartPage(productsIds);
    });
})

async function fillShopCartPage(productsIds) {
    if (!productsIds || productsIds.length === 0) {
        console.log("Os items não podem ser nulos!");
        return;
    }

    const query = productsIds.map(id => `productsIds=${encodeURIComponent(id)}`).join('&');
    const url = `/Product/ShopCart?${query}`

    window.location.href = url;
}