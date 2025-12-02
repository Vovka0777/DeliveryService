document.addEventListener('DOMContentLoaded', function () {
    // -------------------------------------------------------------------
    // 0. Dark Mode Logic
    // -------------------------------------------------------------------
    const themeToggleBtn = document.getElementById('theme-toggle');
    const themeToggleMobileBtn = document.getElementById('theme-toggle-mobile');
    const htmlElement = document.documentElement;

    function updateThemeIcon(isDark) {
        [themeToggleBtn, themeToggleMobileBtn].forEach(btn => {
            if (!btn) return;
            const icon = btn.querySelector('i');
            if (isDark) {
                icon.classList.remove('bi-moon');
                icon.classList.add('bi-sun');
            } else {
                icon.classList.remove('bi-sun');
                icon.classList.add('bi-moon');
            }
        });
    }

    const savedTheme = localStorage.getItem('theme') || 'light';
    if (savedTheme === 'dark') {
        htmlElement.setAttribute('data-theme', 'dark');
        updateThemeIcon(true);
    }

    function toggleTheme() {
        const currentTheme = htmlElement.getAttribute('data-theme');
        const newTheme = currentTheme === 'dark' ? 'light' : 'dark';
        htmlElement.setAttribute('data-theme', newTheme);
        localStorage.setItem('theme', newTheme);
        updateThemeIcon(newTheme === 'dark');
    }

    if (themeToggleBtn) themeToggleBtn.addEventListener('click', toggleTheme);
    if (themeToggleMobileBtn) themeToggleMobileBtn.addEventListener('click', toggleTheme);

    // -------------------------------------------------------------------
    // 1. Хедер при скролле
    // -------------------------------------------------------------------
    const header = document.getElementById('header-top');
    function updateHeader() {
        if (!header) return;
        const scrollY = window.scrollY;
        if (scrollY > 10) {
            header.style.boxShadow = `0 4px 20px rgba(0,0,0, 0.1)`;
            header.style.backdropFilter = "blur(15px)";
        } else {
            header.style.boxShadow = "none";
            header.style.backdropFilter = "blur(5px)";
        }
    }
    window.addEventListener('scroll', updateHeader);
    updateHeader();

    // -------------------------------------------------------------------
    // 2. Анимация появления
    // -------------------------------------------------------------------
    const observerOptions = { threshold: 0.15, rootMargin: "0px 0px -50px 0px" };
    const observer = new IntersectionObserver((entries, observer) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.style.opacity = "1";
                entry.target.style.transform = "translateY(0)";
                observer.unobserve(entry.target);
            }
        });
    }, observerOptions);

    const elementsToAnimate = document.querySelectorAll('.row-item, .about-item, .product-card, .info-sections, .map');
    elementsToAnimate.forEach(el => {
        el.style.opacity = "0";
        el.style.transform = "translateY(30px)";
        el.style.transition = "opacity 0.6s ease-out, transform 0.6s ease-out";
        observer.observe(el);
    });

    // -------------------------------------------------------------------
    // 3. Отзывы (оставляем старый код)
    // -------------------------------------------------------------------
    const reviews = document.querySelector('.reviews-slider');
    const prevButton = document.getElementById('prevReview');
    const nextButton = document.getElementById('nextReview');
    if (reviews && prevButton && nextButton) {
        let currentIndex = 0;
        const items = document.querySelectorAll('.review-item');
        const reviewsCount = items.length;
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

    // -------------------------------------------------------------------
    // 4. Футер
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
            btn.style.width = btn.offsetWidth + 'px';
            btn.disabled = true;
            btn.textContent = '✓';
            btn.style.background = '#10b981';
            setTimeout(function () {
                btn.disabled = false;
                btn.textContent = originalText;
                btn.style.background = '';
                btn.style.width = '';
                subForm.reset();
            }, 2000);
        });
    }

    // -------------------------------------------------------------------
    // 5. МОБИЛЬНОЕ МЕНЮ (ОБНОВЛЕННАЯ ЛОГИКА)
    // -------------------------------------------------------------------
    const hamburgerButton = document.getElementById('hamburger');
    const closeMenuButton = document.getElementById('close-menu-btn');
    const sideMenu = document.getElementById('side-menu');
    const menuOverlay = document.getElementById('menu-overlay');
    const menuLinks = document.querySelectorAll('.nav-list a');
    const body = document.body;

    function openMenu() {
        if (sideMenu) sideMenu.classList.add('active');
        if (menuOverlay) menuOverlay.classList.add('active');
        body.style.overflow = 'hidden';
    }

    function closeMenu() {
        if (sideMenu) sideMenu.classList.remove('active');
        if (menuOverlay) menuOverlay.classList.remove('active');
        body.style.overflow = '';
    }

    if (hamburgerButton) {
        hamburgerButton.addEventListener('click', (e) => {
            e.stopPropagation();
            openMenu();
        });
    }

    if (closeMenuButton) {
        closeMenuButton.addEventListener('click', closeMenu);
    }

    if (menuOverlay) {
        menuOverlay.addEventListener('click', closeMenu);
    }

    // Закрываем при клике на ссылку
    menuLinks.forEach(link => {
        link.addEventListener('click', closeMenu);
    });
});