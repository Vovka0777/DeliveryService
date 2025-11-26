document.addEventListener('DOMContentLoaded', function () {
    
    const priceRange = document.getElementById('priceRange');
    const priceValue = document.getElementById('priceValue');
    const applyFiltersBtn = document.getElementById('applyFilters');
    const sortOrderSelect = document.getElementById('sortOrder');
    const searchInput = document.getElementById('elasticSearch'); // Наше поле поиска
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
        sortOrderSelect.addEventListener('change', function () {
            filterItems();
        });
    }

    // === НОВАЯ ЛОГИКА ПОИСКА ===
    let debounceTimer;
    if (searchInput) {
        searchInput.addEventListener('input', function () {
            // Показываем/скрываем крестик
            if (this.value.trim() !== '') {
                clearBtn.style.display = 'block';
            } else {
                clearBtn.style.display = 'none';
            }

            // Задержка (debounce), чтобы не бомбить сервер запросами при каждой букве
            clearTimeout(debounceTimer);
            debounceTimer = setTimeout(() => {
                filterItems();
            }, 500); // Запрос уйдет через 0.5 сек после окончания ввода
        });
    }

    // Очистка поиска
    if (clearBtn) {
        clearBtn.addEventListener('click', function () {
            searchInput.value = '';
            this.style.display = 'none';
            filterItems(); // Сбрасываем фильтр
        });
    }

    // Основная функция фильтрации (ТЕПЕРЬ ВКЛЮЧАЕТ ПОИСК)
    function filterItems() {
        const maxPrice = document.getElementById('priceRange').value;
        const sortOrder = document.getElementById('sortOrder').value;
        const searchText = document.getElementById('elasticSearch').value; // Берем текст поиска
        
        let selectedCategories = [];
        const checkboxes = document.querySelectorAll('.form-check-input:checked');
        checkboxes.forEach((checkbox) => {
            selectedCategories.push(parseInt(checkbox.value));
        });

        const filterData = {
            maxPrice: parseFloat(maxPrice),
            categories: selectedCategories,
            ordering: sortOrder,
            name: searchText // Отправляем имя на сервер
        };

        fetch('/Catalog/GetItemsByFilter', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(filterData)
        })
        .then(response => response.json())
        .then(result => {
            if (result.statusCode === 200) {
                renderCatalog(result.data);
            } else {
                console.error('Ошибка фильтрации:', result.description);
            }
        })
        .catch(error => console.error('Ошибка сети:', error));
    }

    function renderCatalog(items) {
        const container = document.getElementById('productsList');
        container.innerHTML = '';

        if (!items || items.length === 0) {
            container.innerHTML = '<p class="text-center">Товары не найдены.</p>';
            return;
        }

        items.forEach(item => {
            const cardHtml = `
                <div class="product-card">
                    <div class="card-img-container">
                        <img src="${item.pathImg}" alt="${item.name}" class="product-img" />
                    </div>
                    <div class="card-body">
                        <h3 class="card-title">${item.name}</h3>
                        <span class="badge bg-secondary mb-2">Категория: ${item.category}</span>
                        <p class="card-text">${item.description}</p>
                        <div class="card-footer-custom">
                            <span class="price-tag">${item.price} ₽</span>
                            <a href="/Catalog/GetItem/${item.id}" class="btn btn-primary btn-sm">
                                Купить
                            </a>
                        </div>
                    </div>
                </div>
            `;
            const tempDiv = document.createElement('div');
            tempDiv.innerHTML = cardHtml;
            container.appendChild(tempDiv.firstElementChild);
        });
    }
});