document.addEventListener('DOMContentLoaded', function () {
    window.addEventListener('scroll', function () {
        var header = document.getElementById('header-top');
        var scrollY = window.scrollY;
        var maxScroll = 250;

        var opacity = Math.min(scrollY / maxScroll, 1);
        header.style.backgroundColor = `rgba(255, 165, 0, ${opacity})`;
    });
});
document.addEventListener('DOMContentLoaded', function () {
    const reviews = document.querySelector('.reviews-slider');
    const prevButton = document.getElementById('prevReview');
    const nextButton = document.getElementById('nextReview');
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
});
document.getElementById('reviews-container').addEventListener('mouseenter', function () {
    const reviewsWrapper = document.querySelector('.reviews-wrapper');
    reviewsWrapper.style.animationPlayState = 'paused';
});

document.getElementById('reviews-container').addEventListener('mouseleave', function () {
    const reviewsWrapper = document.querySelector('.reviews-wrapper');
    reviewsWrapper.style.animationPlayState = 'running';
});


document.addEventListener('DOMContentLoaded', function () {

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

    function toggleMenu() {
        const sideMenu = document.getElementById('side-menu')

        sideMenu.classList.toggle('active');
    }

    document.getElementById('hamburger').addEventListener('click',toggleMenu)
});
