document.addEventListener('DOMContentLoaded', function () {
    // -------------------------------------------------------------------
    // 1. Плавное изменение фона шапки при скролле
    // -------------------------------------------------------------------
    const header = document.getElementById('header-top');

    function updateHeader() {
        if (!header) return;
        const scrollY = window.scrollY;
        const maxScroll = 200;
        const opacity = Math.min(scrollY / maxScroll, 1);

        // RGB: 255, 255, 255 (белый) с прозрачностью, либо легкий синий оттенок
        // Для Glassmorphism лучше использовать белый полупрозрачный фон
        if (scrollY > 10) {
            header.style.background = `rgba(255, 255, 255, ${0.8 + (opacity * 0.15)})`;
            header.style.boxShadow = `0 4px 20px rgba(0,0,0, ${opacity * 0.05})`;
            header.style.backdropFilter = "blur(15px)";
        } else {
            header.style.background = "rgba(255, 255, 255, 0.6)";
            header.style.boxShadow = "none";
            header.style.backdropFilter = "blur(5px)";
        }
    }

    window.addEventListener('scroll', updateHeader);
    updateHeader(); // Init check

    // -------------------------------------------------------------------
    // 2. Анимация появления элементов при скролле (Scroll Reveal)
    // -------------------------------------------------------------------
    const observerOptions = {
        threshold: 0.15, // Срабатывает, когда 15% элемента видно
        rootMargin: "0px 0px -50px 0px"
    };

    const observer = new IntersectionObserver((entries, observer) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.style.opacity = "1";
                entry.target.style.transform = "translateY(0)";
                observer.unobserve(entry.target);
            }
        });
    }, observerOptions);

    // Применяем к элементам, которые хотим анимировать
    const elementsToAnimate = document.querySelectorAll('.row-item, .about-item, .product-card, .info-sections, .map');

    elementsToAnimate.forEach(el => {
        // Начальное состояние (скрыто)
        el.style.opacity = "0";
        el.style.transform = "translateY(30px)";
        el.style.transition = "opacity 0.6s ease-out, transform 0.6s ease-out";
        observer.observe(el);
    });

    // -------------------------------------------------------------------
    // 3. Логика для слайдера отзывов
    // -------------------------------------------------------------------
    const reviews = document.querySelector('.reviews-slider');
    const prevButton = document.getElementById('prevReview');
    const nextButton = document.getElementById('nextReview');
    const reviewsWrapper = document.querySelector('.reviews-wrapper');

    // Если есть кнопки (ручной слайдер)
    if (reviews && prevButton && nextButton) {
        let currentIndex = 0;
        const items = document.querySelectorAll('.review-item');
        const reviewsCount = items.length;

        function showReview(index) {
            // Для простого слайдера сдвигаем
            reviews.style.transform = `translateX(-${index * 100}%)`;
        }

        prevButton.addEventListener('click', function () {
            currentIndex = (currentIndex - 1 + reviewsCount) % reviewsCount;
            showReview(currentIndex);
        });

        nextButton.addEventListener('click', function () {
            currentIndex = (currentIndex + 1) % reviewsCount;
            showReview(currentIndex);
        });
    }

    // Пауза бесконечной анимации при наведении
    const reviewsContainer = document.querySelector('.reviews-container');
    if (reviewsContainer && reviewsWrapper) {
        reviewsContainer.addEventListener('mouseenter', function () {
            reviewsWrapper.style.animationPlayState = 'paused';
        });

        reviewsContainer.addEventListener('mouseleave', function () {
            reviewsWrapper.style.animationPlayState = 'running';
        });
    }

    // -------------------------------------------------------------------
    // 4. Футер и подписка
    // -------------------------------------------------------------------
    const yearEl = document.getElementById('currentYear');
    if (yearEl) yearEl.textContent = new Date().getFullYear();

    const subForm = document.getElementById('footer-subscribe-form');
    if (subForm) {
        subForm.addEventListener('submit', function (e) {
            e.preventDefault();
            const emailInput = subForm.querySelector('input[name="email"]');
            const btn = subForm.querySelector('.btn-subscribe');

            if (!emailInput || !emailInput.value.trim()) {
                emailInput?.focus();
                return;
            }

            const originalText = btn.textContent;
            btn.style.width = btn.offsetWidth + 'px'; // Фиксируем ширину
            btn.disabled = true;
            btn.textContent = '✓';
            btn.style.background = '#10b981'; // Зеленый

            setTimeout(function () {
                btn.disabled = false;
                btn.textContent = originalText;
                btn.style.background = ''; // Сброс цвета
                btn.style.width = '';
                subForm.reset();
            }, 2000);
        });
    }

    // -------------------------------------------------------------------
    // 5. Мобильное меню (Гамбургер)
    // -------------------------------------------------------------------
    const hamburgerButton = document.getElementById('hamburger');
    const sideMenu = document.getElementById('side-menu');
    const closeMenuBtn = document.getElementById('side-menu-close'); // Если добавишь крестик
    const body = document.body;

    function toggleMenu() {
        if (!sideMenu) return;

        const isActive = sideMenu.classList.toggle('active');

        // Блокируем скролл фона при открытом меню
        if (isActive) {
            body.style.overflow = 'hidden';
        } else {
            body.style.overflow = '';
        }
    }

    if (hamburgerButton) {
        hamburgerButton.addEventListener('click', (e) => {
            e.stopPropagation();
            toggleMenu();
        });
    }

    // Закрытие при клике вне меню
    document.addEventListener('click', (e) => {
        if (sideMenu && sideMenu.classList.contains('active')) {
            if (!sideMenu.contains(e.target) && !hamburgerButton.contains(e.target)) {
                toggleMenu();
            }
        }
    });
}); 