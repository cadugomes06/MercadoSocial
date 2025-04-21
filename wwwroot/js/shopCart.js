

var NumShopCart;
var productsOnShopCart = [];

$(function() {
    NumShopCart = parseInt(localStorage.getItem("NumShopCart")) || 0;
    productsOnShopCart = JSON.parse(localStorage.getItem("itemsShopCart")) || [];

    $("#countShopCart").text(NumShopCart);

    $(".addItemOnShopCart").on('click', function () {
        var productID = $(this).data('id');
        addItemOnShopCart(productID);
    });
})

$(function () {
    const itemsStoraged = JSON.parse(localStorage.getItem("itemsShopCart")) || [];

    var productsIds = itemsStoraged.map(item => item.id);

    updateShopCartItems(productsIds);
})


function addItemOnShopCart(productID) {
    console.log(productsOnShopCart);
    var teste = productsOnShopCart.filter(item => item.id === productID);
    console.log(teste);

    if (teste.length != 0) {
        alert("Ops... O item já foi adicionado");
        return;
    }

    productsOnShopCart.push( {id: productID, quantity: 1, user: null } );

    localStorage.setItem("itemsShopCart", JSON.stringify(productsOnShopCart));
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

$(function () {
    $(".takeItemsToShopCart").on('click', function () {
        var itemsShopCart = JSON.parse(localStorage.getItem("itemsShopCart"));

        fillShopCartPage(itemsShopCart);
    });
})

async function fillShopCartPage(itemsShopCart) {
    if (!itemsShopCart || itemsShopCart.length === 0) {
        alert("Adicione um produto primeiro");
        return;
    }

    var productsIds = itemsShopCart.map(item => item.id);

    const query = productsIds.map(id => `productsIds=${encodeURIComponent(id)}`).join('&');
    const url = `/ShopCart/Index?${query}`;

    window.location.href = url;
}


$(document).on('click', ".addQuantity", function () {
    var input = $(this).closest("#containerQuantityCart").find("#inputQuantityItem");
    var inputValue = parseInt(input.val()) || 1;

    if (inputValue < 10) {
        inputValue++;
        input.val(inputValue);
    }
});

$(document).on('click', ".removeQuantity", function () {
    var input = $(this).closest("#containerQuantityCart").find("#inputQuantityItem");

    var inputValue = parseInt(input.val()) || 1;

    if (inputValue > 1) {
        inputValue--;
        input.val(inputValue);
    }
});


$(document).on('click', ".btnRemoveItemCart", function () {

    const productId = $(this).closest("div").find(".productId").val();
    removeItemShopCart(productId);
})

function removeItemShopCart(productId) {
    const itemsStoraged = JSON.parse(localStorage.getItem("itemsShopCart"));

    productId = parseInt(productId, 10);

    const index = itemsStoraged.findIndex(item => item.id === productId);

    if (index === -1) {
        alert("Não foi possível localizar o produto para remove-lo");
        return;
    }

    itemsStoraged.splice(index, 1);
    localStorage.setItem("itemsShopCart", JSON.stringify(itemsStoraged));

    removeTotalItemsShopCart();

    productsIds = itemsStoraged.map(item => item.id);
    updateShopCartItems(productsIds);
}

function updateShopCartItems(productsIds) {

    $.ajax({
        url: '/ShopCart/UpdateListShopCart',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(productsIds),
        success: function (response) {
            $('.pv_wrapperProducts').html(response);
        },
        error: function (er) {
            console.log("Erro...", er.responseText);
        }
    });
}

$("#btnSaveBasket").on('click', function() {
    const itemsStoraged = JSON.parse(localStorage.getItem("itemsShopCart"));

    if (itemsStoraged === null || itemsStoraged.length === 0) {
        alert("Houve um erro ao localizar os itens no carrinho.");
        return;
    }

    const itemsUpdated = UpdateQuantity();

    console.log("salvar cesta - itemsUpdated", itemsUpdated);
});

function UpdateQuantity() {
    const itemsStoraged = JSON.parse(localStorage.getItem("itemsShopCart"));

    itemsStoraged.forEach(function (item) {
        var input = $(` input[name="${item.id}"] `);
        item.quantity = parseInt(input.val(), 10);
    });

    return itemsStoraged;
}