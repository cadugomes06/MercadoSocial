
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
    } else {
        console.log("Nenhuma seção selecionada.");
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
                //console.log("dados recebidos", data)

                const containerCards = document.getElementById('containerProducts')
                containerCards.innerHTML = '';
                if (data && data.length > 0) {
                    console.log("criando cards...")

                    data.forEach(product => {
                        const productElement = document.createElement('div');
                        productElement.className = 'card position-relative';
                        productElement.style.width = '18rem'
                        productElement.innerHTML = `                         
                            <img src="data:image/png;base64,${product.imageBase64}"
                             class="card-img-top"
                             alt="imagem-produto"
                             style="height: 14rem;"
                             />
                             <a href="Product/Edit/${product.id}" class="position-absolute top-0 text-white icon-link icon-link-hover"
                                style="right: 2px; top: 0px; --bs-icon-link-transform: translate3d(0, -.125rem, 0);">
                                  <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="#808080" class="bi bi-pencil-fill" viewBox="0 0 16 16">
                                    <path d="M12.854.146a.5.5 0 0 0-.707 0L10.5 1.793 14.207 5.5l1.647-1.646a.5.5 0 0 0 0-.708zm.646 6.061L9.793 2.5 3.293 9H3.5a.5.5 0 0 1 .5.5v.5h.5a.5.5 0 0 1 .5.5v.5h.5a.5.5 0 0 1 .5.5v.5h.5a.5.5 0 0 1 .5.5v.207zm-7.468 7.468A.5.5 0 0 1 6 13.5V13h-.5a.5.5 0 0 1-.5-.5V12h-.5a.5.5 0 0 1-.5-.5V11h-.5a.5.5 0 0 1-.5-.5V10h-.5a.5.5 0 0 1-.175-.032l-.179.178a.5.5 0 0 0-.11.168l-2 5a.5.5 0 0 0 .65.65l5-2a.5.5 0 0 0 .168-.11z" />
                                 </svg>
                             </a>

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

    try {
        var product = await getProductById(productId);

        if (product != null) {
            showOnScreen(product);
        }

    } catch (error) {
        console.log("Erro ao buscar o Produto. " + error);
    }

    $(".containerModalProduct").removeClass("hide");
});


//Fechar Modal
$(document).on('click', '.btnCloseModal', function () {
    console.log("btnClose");

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
                    console.log("sucesso na requisição AJAX");
                    resolve(product);
                } else {
                    console.log("Não deu sucesso!");
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


function showOnScreen(product) {
    var modalProduct = $(".modalProduct");

    var element = document.createElement('div');
    modalProduct.empty();

    element.innerHTML = `
              <div style="padding: 10px;">
                 <div><h2 style="text-align: center; margin-bottom: 1rem; margin-top: 1rem;">${product.name}</h2></div>

                 <div style="display: flex; justify-content: center; margin-bottom: 1rem;">
                    <img src="data:image/png;base64,${product.imageBase64}" style="width: 300px;
                       height: 280px; object-fit: cover; box-shadow: 1px 1px 6px 1px rgba(0, 0, 0, 0.2); border-radius: 6px;" />
                 </div>

                 <div>
                     <p><strong>Descrição:</strong> ${product.description}.</p>
                     <p><strong>Seção:</strong> ${product.section}.</p>
                     <p><strong>Quantidade:</strong> ${product.quantity} unidades.</p>
                     <p><strong>Criado por:</strong> ${product.userId}.</p>
                 </div>

                 <div style="width: 100%; height: 50px; display:flex; justify-content: end; align-items: center; gap: 16px;
                 padding-right: 1rem;">
                    <button class="btn btn-danger">Excluir</button>
                     <a href="/Product/Edit/${product.id}" class="btn btn-secondary">Editar</a>
                 </div>
             </div>
             <button class="btn btn-close btnCloseModal"></button>
             `;

    modalProduct.append(element);
}