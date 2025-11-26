document.addEventListener('DOMContentLoaded', function () {

    const priceRange = document.getElementById('priceRange');
    const priceValue = document.getElementById('priceValue');
    const applyFiltersBtn = document.getElementById('applyFilters');
    const sortOrderSelect = document.getElementById('sortOrder');

    // 1. Обновление цифры цены при движении ползунка
    if (priceRange) {
        priceRange.addEventListener('input', function () {
            priceValue.textContent = this.value;
        });
    }

    // 2. Обработка кнопки "Применить" (Фильтры)
    if (applyFiltersBtn) {
        applyFiltersBtn.addEventListener('click', function (e) {
            e.preventDefault();
            filterItems();
        });
    }

    // 3. Обработка изменения сортировки
    if (sortOrderSelect) {
        sortOrderSelect.addEventListener('change', function () {
            filterItems();
        });
    }

    // Основная функция фильтрации
    function filterItems() {
        // Собираем данные с элементов управления
        const maxPrice = document.getElementById('priceRange').value;
        const sortOrder = document.getElementById('sortOrder').value;

        // Собираем ID выбранных категорий
        let selectedCategories = [];
        const checkboxes = document.querySelectorAll('.form-check-input:checked');
        checkboxes.forEach((checkbox) => {
            selectedCategories.push(parseInt(checkbox.value));
        });

        // Создаем объект фильтра (соответствует C# классу ItemFilter)
        const filterData = {
            maxPrice: parseFloat(maxPrice),
            categories: selectedCategories,
            ordering: sortOrder
        };

        // Отправляем запрос на сервер
        fetch('/Catalog/GetItemsByFilter', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(filterData)
        })
            .then(response => response.json())
            .then(result => {
                // Если статус OK (код 200), перерисовываем каталог
                if (result.statusCode === 200) {
                    renderCatalog(result.data);
                } else {
                    console.error('Ошибка фильтрации:', result.description);
                }
            })
            .catch(error => console.error('Ошибка сети:', error));
    }

    // Функция перерисовки HTML карточек
    function renderCatalog(items) {
        const container = document.getElementById('productsList');
        container.innerHTML = ''; // Очищаем текущие товары

        if (!items || items.length === 0) {
            container.innerHTML = '<p class="text-center">Товары не найдены.</p>';
            return;
        }

        items.forEach(item => {
            // Генерируем HTML для одной карточки
            // Важно: поля объекта item приходят в camelCase (name, price, pathImg)
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

            // Вставляем карточку в контейнер
            const tempDiv = document.createElement('div');
            tempDiv.innerHTML = cardHtml;
            container.appendChild(tempDiv.firstElementChild);
        });
    }
});