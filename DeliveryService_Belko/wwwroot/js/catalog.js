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

        items.forEach((item, index) => {
            // Создаем элемент
            const wrapper = document.createElement('div');
            // Если используете Bootstrap grid в контейнере, классы могут быть не нужны на обертке, 
            // так как grid задан в родительском .catalog-container css.
            // Но для анимации нам нужен элемент.

            // Генерация HTML, соответствующего catalog.css
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
                            <a href="/Catalog/GetItem/${item.id}" class="btn btn-primary btn-sm">
                                Подробнее
                            </a>
                        </div>
                    </div>
                </div>
            `;

            wrapper.innerHTML = cardHtml;
            container.appendChild(wrapper.firstElementChild);
        });
    }
});