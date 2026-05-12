document.addEventListener("DOMContentLoaded", function () {
    // Automatically highlight active sidebar links based on URL
    const currentPath = window.location.pathname;
    const navLinks = document.querySelectorAll('.nav-link');

    navLinks.forEach(link => {
        if (link.getAttribute('href') === currentPath) {
            link.classList.add('active');
        }
    });

    // Optional: Add a subtle flicker effect to neon titles on load
    const brand = document.querySelector('h4');
    if (brand) {
        brand.style.opacity = '0.8';
        setTimeout(() => { brand.style.opacity = '1'; }, 100);
    }
});