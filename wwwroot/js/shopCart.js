

var NumShopCart;
var productsOnShopCart = [];

$(document).ready(function () {
    NumShopCart = parseInt(localStorage.getItem("NumShopCart")) || 0;
    productsOnShopCart = JSON.parse(localStorage.getItem("itemsShopCart")) || [];

    console.log("productsOnShopCart: ", productsOnShopCart);

    $("#countShopCart").text(NumShopCart);

    $(".addItemOnShopCart").on('click', function () {
        var productID = $(this).data('id');
        addItemOnShopCart(productID);
    });
})

$(document).on(function () {
    const productsStoraged = JSON.parse(localStorage.getItem("itemsShopCart")) || [];

    console.log("atualizou...");
    updateShopCartItems();
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

function removeTotalItemsShopCart() {
    if (NumShopCart > 0) {
        NumShopCart = NumShopCart - 1;
        $("#countShopCart").text(NumShopCart);
        localStorage.setItem("NumShopCart", NumShopCart);
    }
}

$(document).ready(function () {
    $(".takeItemsToShopCart").on('click', function () {
        var productsIds = JSON.parse(localStorage.getItem("itemsShopCart"));
        console.log("takeItemsToShopCart ", productsIds);

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


$(document).on('click', ".btnRemoveItemCart", function() {

    const productId = $(this).closest("div").find(".productId").val();
    removeItemShopCart(productId);
})

function removeItemShopCart(productId) {
    const arrProducts = JSON.parse(localStorage.getItem("itemsShopCart"));

    productId = parseInt(productId, 10);

    const index = arrProducts.indexOf(productId);

    if (index === -1) {
        alert("Não foi possível localizar o produto para remove-lo")
    }

    arrProducts.splice(index, 1);
    localStorage.setItem("itemsShopCart", JSON.stringify(arrProducts));

    removeTotalItemsShopCart();
    updateShopCartItems(arrProducts);
}

function updateShopCartItems(arrProducts) {

    console.log("Tipo... ", typeof arrProducts);
    console.log("Tipo... ", arrProducts);

    $.ajax({
        url: '/Product/UpdateListShopCart',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(arrProducts),
        success: function (response) {
            $('.pv_wrapperProducts').html(response);
            console.log("Response OK");
        },
        error: function (er) {
            console.log("Erro...", er.responseText);
        }
    });
}