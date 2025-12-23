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

    // Кнопка "Применить"
    if (applyFiltersBtn) {
        applyFiltersBtn.addEventListener('click', function (e) {
            e.preventDefault();
            filterItems();
        });
    }

    // Сортировка
    if (sortOrderSelect) {
        sortOrderSelect.addEventListener('change', () => filterItems());
    }

    // Поиск
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

    // Очистка поиска
    if (clearBtn) {
        clearBtn.addEventListener('click', function () {
            if (searchInput) searchInput.value = '';
            this.style.display = 'none';
            filterItems();
        });
    }

    // Фильтрация
    function filterItems() {
        const productsContainer = document.getElementById('productsList');
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
                productsContainer.style.opacity = '1';
            });
    }

    // Отрисовка
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

        // Права доступа
        const userIsAdmin = (typeof isAdmin !== 'undefined') ? isAdmin : false;
        const userIsAuth = (typeof isAuthenticated !== 'undefined') ? isAuthenticated : false;

        items.forEach((item, index) => {
            const wrapper = document.createElement('div');

            let cartButton = '';
            if (userIsAuth) {
                cartButton = `
                    <a href="/Cart/Add?id=${item.id}" 
                       class="btn btn-outline-success btn-sm rounded-pill" 
                       title="В корзину">
                        <i class="bi bi-cart-plus"></i>
                    </a>`;
            }

            let adminButtons = '';
            if (userIsAdmin) {
                adminButtons = `
                    <a href="/Catalog/Save?id=${item.id}" class="btn btn-warning btn-sm rounded-pill" title="Редактировать">
                        <i class="bi bi-pencil"></i>
                    </a>
                    <a href="/Catalog/Delete?id=${item.id}" class="btn btn-danger btn-sm rounded-pill" onclick="return confirm('Удалить?')" title="Удалить">
                        <i class="bi bi-trash"></i>
                    </a>
                `;
            }

            // ЧИСТАЯ СТРУКТУРА HTML (без лишних d-flex классов Bootstrap)
            const cardHtml = `
            <div class="product-card" style="animation: fadeInUp 0.5s ease forwards; animation-delay: ${index * 0.05}s; opacity: 0; transform: translateY(20px);">
                <div class="card-img-container">
                    <img src="${item.pathImg}" alt="${item.name}" class="product-img" loading="lazy" />
                </div>
                <div class="card-body">
                    <h3 class="card-title">${item.name}</h3>
                    <p class="card-text">${item.description}</p>
                    
                    <div class="card-footer-custom">
                        <span class="price-tag">${item.price} ₽</span>
                        
                        <div class="footer-buttons">
                            <a href="/Catalog/GetItem?id=${item.id}" class="btn btn-primary btn-sm rounded-pill">
                                Подробнее
                            </a>
                            ${cartButton}
                            ${adminButtons}
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