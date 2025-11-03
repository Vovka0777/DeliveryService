document.addEventListener('DOMContentLoaded', function () {
    window.addEventListener('scroll', function () {
        var header = document.getElementById('header-top');
        var scrollY = window.scrollY;
        var maxScroll = 250;

        var opacity = Math.min(scrollY / maxScroll, 1);
        header.style.backgroundColor = `rgba(255, 165, 0, ${opacity})`;
    });
});