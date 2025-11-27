document.addEventListener('DOMContentLoaded', function () {
    // -------------------------------------------------------------------
    // 1. Плавное изменение фона шапки при скролле
    // -------------------------------------------------------------------
    window.addEventListener('scroll', function () {
        var header = document.getElementById('header-top');
        var scrollY = window.scrollY;
        var maxScroll = 250;
        var opacity = Math.min(scrollY / maxScroll, 1);
        
        // 🌟 ИЗМЕНЕНИЕ: Используем наш новый синий цвет (RGB: 37, 99, 235)
        header.style.backgroundColor = `rgba(37, 99, 235, ${opacity * 0.85})`; // Чуть повысил непрозрачность для читаемости
    });

    // -------------------------------------------------------------------
    // 2. Логика для слайдера отзывов (если он существует)
    // -------------------------------------------------------------------
    const reviews = document.querySelector('.reviews-slider');
    const prevButton = document.getElementById('prevReview');
    const nextButton = document.getElementById('nextReview');
    const reviewsWrapper = document.querySelector('.reviews-wrapper');

    if (reviews && prevButton && nextButton) {
        let currentIndex = 0;
        const reviewsCount = document.querySelectorAll('.review-item').length;

        function showReview(index) {
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

    // Пауза анимации отзывов при наведении
    const reviewsContainer = document.getElementById('reviews-container');
    if (reviewsContainer && reviewsWrapper) {
        reviewsContainer.addEventListener('mouseenter', function () {
            reviewsWrapper.style.animationPlayState = 'paused';
        });

        reviewsContainer.addEventListener('mouseleave', function () {
            reviewsWrapper.style.animationPlayState = 'running';
        });
    }

    // -------------------------------------------------------------------
    // 3. Логика для футера и подписки
    // -------------------------------------------------------------------
    var yearEl = document.getElementById('currentYear');
    if (yearEl) yearEl.textContent = new Date().getFullYear();

    var form = document.getElementById('footer-subscribe-form');
    if (form) {
        form.addEventListener('submit', function (e) {
            e.preventDefault();
            var emailInput = form.querySelector('input[name="email"]');
            var email = emailInput && emailInput.value.trim();
            if (!email) {
                emailInput.focus();
                return;
            }
            var originalBtnText = form.querySelector('.btn-subscribe').textContent;
            var btn = form.querySelector('.btn-subscribe');
            btn.disabled = true;
            btn.textContent = 'Спасибо!';
            setTimeout(function () {
                btn.disabled = false;
                btn.textContent = originalBtnText;
                form.reset();
            }, 1800);
        });
    }

    // -------------------------------------------------------------------
    // 4. ЛОГИКА ГАМБУРГЕРА
    // -------------------------------------------------------------------

    function toggleMenu() {
        const sideMenu = document.getElementById('side-menu');
        if (sideMenu) {
            sideMenu.classList.toggle('active');
        }
    }

    const hamburgerButton = document.getElementById('hamburger');

    if (hamburgerButton) {
        hamburgerButton.addEventListener('click', toggleMenu);
    }
});