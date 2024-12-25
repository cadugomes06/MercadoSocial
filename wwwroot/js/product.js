
//Filtrar 
function updateProducts() {
    const sectionSelect = document.getElementById("sectionSelect");
    const productSelect = document.getElementById("productSelect");
    const selectedSection = sectionSelect.value;


    if (selectedSection) {
        selectedSection.disabled = true

        $.ajax({
            type: 'GET',
            url: 'GetProductsBySection',
            contentType: 'application/json; charset=utf-8',
            data: { section: selectedSection },
            success: function (data) {

                productSelect.innerHTML = '<option value="" selected disabled>Escolha o produto</option>';
                if (data && data.length > 0) {
                    data.forEach(product => {
                        const option = document.createElement("option");
                        option.value = product.id; // Certifique-se de que 'id' e 'name' correspondem aos atributos dos seus produtos
                        option.textContent = product.name;
                        productSelect.appendChild(option);
                    });
                } else {
                    const option = document.createElement("option");
                    option.value = "";
                    option.textContent = "Nenhum produto encontrado";
                    option.disabled = true;
                    selectedSection.disabled = false
                    productSelect.appendChild(option);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.error('Erro ao buscar produtos:', textStatus, errorThrown);
            }
        });
    }
}

$(function () {
    $(document).on('change', '#sectionSelect', updateProducts);
});





//filtrar index de produtos por sessão.
function filterListBySection() {
    const filterSection = document.getElementById('filterSection');
    const filterValue = filterSection.value

    if (filterValue) {

        $.ajax({
            type: 'GET',
            url: '/Product/GetProductsBySection',
            contentType: 'application/json; charset=utf-8',
            data: { section: filterValue },
            success: function (data) {

                const containerCards = document.getElementById('containerProducts')
                containerCards.innerHTML = '';
                if (data && data.length > 0) {

                    data.forEach(product => {
                        const productElement = document.createElement('div');
                        productElement.className = 'card position-relative';
                        productElement.style.width = '18rem'
                        productElement.innerHTML = `                         
                             <img src="data:image/png;base64,${product.imageBase64}"
                             class="card-img-top activeModalProduct"
                             alt="imagem-produto"
                             style="height: 14rem; cursor: pointer;"
                             id=${product.id}
                             >

                             <div class="card-body">
                                <div class="d-flex align-content-center justify-content-between">
                                  <h5 class="card-title">${product.name}</h5>
                                  <p class="text-dark fs-6 text-center">
                                    Total
                                    <span class="text-info">${product.quantity}</span>
                                  </p>
                             </div>

                         <p class="card-text">${product.description}</p>
                        <a href="#" class="btn btn-primary">Adicionar a Cesta</a>                   
                        `;
                        containerCards.appendChild(productElement);
                    });

                } else {
                    const productNotFound = document.createElement('div')
                    productNotFound.innerHTML = `<h2>Nenhum produto encontrado nessa sessão!</h2>`;
                    containerCards.appendChild(productNotFound);
                }

            },
            error: function (xhr, status, error) {
                console.log("Erro na requisição AJAX:", error);
            }
        });
    }
}

$(function () {
    $(document).on('change', '#filterSection', filterListBySection)
});



//Ativar modal ao clicar no produto
$(document).on('click', '.activeModalProduct', async function () {
    var productId = $(this).attr('id');
    console.log("abrir modal ?!");

    try {
        var product = await getProductById(productId);

        if (product != null) {
            var user = await getUserById(product.userId);
        }

        if (product != null && user != null) {
            showOnScreen(product, user);
        }

    } catch (error) {
        console.log("Erro ao buscar dados do Produto. " + error);
    }

    $(".containerModalProduct").removeClass("hide");
});


//Fechar Modal
$(document).on('click', '.btnCloseModal', function () {
    $(".containerModalProduct").addClass("hide");
});


//Buscar um produto pelo ID
async function getProductById(productId) {

    return new Promise((resolve, reject) => {
        $.ajax({
            url: `/Product/GetProductById/${productId}`,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            success: function (product) {

                if (product) {
                    resolve(product);
                } else {
                    return "Produto não encontrato";
                }
            },
            erro: function (error) {
                console.log("Houve um erro na requisição AJAX" + error);
                reject(error);
            }
        });
    });
}


async function getUserById(id) {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: '/User/GetUserById/' + id,
            type: 'GET',
            ContentType: 'application/json',
            success: function (data) {
                if (data) {
                    resolve(data);
                }
            },
            erro: function (er) {
                console.log("Houve um erro na requisição ", + er);
                reject(er);
            }
        });
    });
}

function getTypeSection(section) {

    return new Promise((resolve, reject) => {
        $.ajax({
            url: '/Product/TypeSectionEnum?section=' + section,
            type: 'GET',
            contentType: 'application/json; chatset=utf-8',
            success: function (sectionEnum) {
                resolve(sectionEnum);
            },
            error: function (er) {
                reject("Erro ao processar a solicitação..." + er.responseText);
            }
        });

    });
}


async function showOnScreen(product, user) {
    var modalProduct = $(".modalProduct");
    var element = document.createElement('div');
    modalProduct.empty();

    var numSection = product.section
    var enumName = await getTypeSection(numSection);

    element.innerHTML = `
              <div style="padding: 10px;">
                 <div><h2 style="text-align: center; margin-bottom: 1rem; margin-top: 1rem;">${product.name}</h2></div>

                 <div style="display: flex; justify-content: center; margin-bottom: 1rem;">
                    <img src="data:image/png;base64,${product.imageBase64}" style="width: 300px;
                       height: 280px; object-fit: cover; box-shadow: 1px 1px 6px 1px rgba(0, 0, 0, 0.2); border-radius: 6px;" />
                 </div>

                 <div>
                     <p><strong>Descrição:</strong> ${product.description}.</p>
                     <p><strong>Seção:</strong> ${enumName} </p>
                     <p><strong>Quantidade:</strong> ${product.quantity} unidades.</p>
                     <p><strong>Criado por:</strong> ${user.name}.</p>
                 </div>

                 <div style="width: 100%; height: 50px; display:flex; justify-content: end; align-items: center; gap: 16px;
                 padding-right: 1rem;">
                     <a data-id="${product.id}" class="btn btn-danger btn-remove-product">Excluir</a>
                     <a href="/Product/Edit/${product.id}" class="btn btn-secondary">Editar</a>
                 </div>
             </div>
             <button class="btn btn-close btnCloseModal"></button>
             `;

    modalProduct.append(element);
}


$(document).on('click', '.btn-remove-product', function () {
    var productId = $(this).data('id');

    if (confirm("Tem certeza que deseja excluir esse produto?")) {
        $.ajax({
            url: '/Product/RemoveProduct/',
            type: 'POST',
            data: { id: productId },
            success: function (res) {

                if (res) {
                    alert("Produto excluído com sucesso");
                    $(this).closest('div').remove();
                } else {
                    alert("Falha ao excluir produto!");
                }
            },
            erro: function (er) {
                alert("Erro ao processar a requisição " + er.responseText);
            }
        });
    }
})