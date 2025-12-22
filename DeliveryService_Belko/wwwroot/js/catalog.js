document.addEventListener('DOMContentLoaded', function () {

    const priceRange = document.getElementById('priceRange');
    const priceValue = document.getElementById('priceValue');
    const applyFiltersBtn = document.getElementById('applyFilters');
    const sortOrderSelect = document.getElementById('sortOrder');
    const searchInput = document.getElementById('elasticSearch');
    const clearBtn = document.getElementById('clearSearch');

    // Обновление цифры цены
    if (priceRange) {
        priceRange.addEventListener('input', function () {
            priceValue.textContent = this.value;
        });
    }

    // Обработчики событий
    if (applyFiltersBtn) {
        applyFiltersBtn.addEventListener('click', function (e) {
            e.preventDefault();
            filterItems();
        });
    }

    if (sortOrderSelect) {
        sortOrderSelect.addEventListener('change', () => filterItems());
    }

    // Живой поиск с debounce
    let debounceTimer;
    if (searchInput) {
        searchInput.addEventListener('input', function () {
            const hasText = this.value.trim() !== '';
            if (clearBtn) clearBtn.style.display = hasText ? 'block' : 'none';

            clearTimeout(debounceTimer);
            debounceTimer = setTimeout(() => {
                filterItems();
            }, 400);
        });
    }

    if (clearBtn) {
        clearBtn.addEventListener('click', function () {
            if (searchInput) searchInput.value = '';
            this.style.display = 'none';
            filterItems();
        });
    }

    function filterItems() {
        const productsContainer = document.getElementById('productsList');
        // Добавляем эффект загрузки (прозрачность)
        productsContainer.style.opacity = '0.5';

        const maxPrice = priceRange ? priceRange.value : 100000;
        const sortOrder = sortOrderSelect ? sortOrderSelect.value : "name_asc";
        const searchText = searchInput ? searchInput.value : "";

        let selectedCategories = [];
        const checkboxes = document.querySelectorAll('.form-check-input:checked');
        checkboxes.forEach((checkbox) => {
            selectedCategories.push(parseInt(checkbox.value));
        });

        const filterData = {
            maxPrice: parseFloat(maxPrice),
            categories: selectedCategories,
            ordering: sortOrder,
            name: searchText
        };

        fetch('/Catalog/GetItemsByFilter', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(filterData)
        })
            .then(response => response.json())
            .then(result => {
                if (result.statusCode === 200) {
                    renderCatalog(result.data);
                } else {
                    console.error('Ошибка:', result.description);
                    productsContainer.innerHTML = `<p class="text-danger text-center">Ошибка: ${result.description}</p>`;
                }
            })
            .catch(error => {
                console.error('Ошибка сети:', error);
                productsContainer.innerHTML = '<p class="text-center">Ошибка соединения.</p>';
            })
            .finally(() => {
                // Возвращаем непрозрачность
                productsContainer.style.opacity = '1';
            });
    }

    function renderCatalog(items) {
        const container = document.getElementById('productsList');
        container.innerHTML = '';

        if (!items || items.length === 0) {
            container.innerHTML = `
                <div class="col-12 text-center py-5">
                    <h4 class="text-muted">Ничего не найдено 😔</h4>
                    <p>Попробуйте изменить параметры поиска</p>
                </div>`;
            return;
        }

        // Считываем статус авторизации из атрибута data-is-auth
        // Убедись, что в Index.cshtml у div#productsList есть атрибут data-is-auth="@User.Identity.IsAuthenticated..."
        const isAuth = container.getAttribute('data-is-auth') === 'true';

        items.forEach((item, index) => {
            const wrapper = document.createElement('div');

            // Формируем HTML кнопки корзины (только если юзер авторизован)
            let cartButtonHtml = '';
            if (isAuth) {
                cartButtonHtml = `
                    <a href="/Cart/Add/${item.id}" 
                       class="btn btn-outline-success btn-sm rounded-pill px-3" 
                       title="Добавить в корзину">
                        <i class="bi bi-cart-plus"></i>
                    </a>`;
            }

            // Генерируем карточку
            const cardHtml = `
            <div class="product-card" style="animation: fadeInUp 0.5s ease forwards; animation-delay: ${index * 0.05}s; opacity: 0; transform: translateY(20px);">
                <div class="card-img-container">
                    <img src="${item.pathImg}" alt="${item.name}" class="product-img" loading="lazy" />
                </div>
                <div class="card-body">
                    <h3 class="card-title">${item.name}</h3>
                    <p class="card-text">${item.description}</p>
                    
                    <div class="card-footer-custom d-flex justify-content-between align-items-center">
                        <span class="price-tag">${item.price} ₽</span>
                        
                        <div class="d-flex align-items-center gap-2">
                            <a href="/Catalog/GetItem/${item.id}" class="btn btn-primary btn-sm rounded-pill px-3">
                                Подробнее
                            </a>
                            ${cartButtonHtml}
                        </div>
                    </div>
                </div>
            </div>
        `;

            wrapper.innerHTML = cardHtml;
            container.appendChild(wrapper.firstElementChild);
        });
    }
});