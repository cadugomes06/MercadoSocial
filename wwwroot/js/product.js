
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
document.getElementById("sectionSelect").addEventListener("change", updateProducts);






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

document.getElementById("filterSection").addEventListener("change", filterListBySection);


