document.addEventListener("DOMContentLoaded", function () {
    const currentPath = window.location.pathname.toLowerCase();
    const currentSegments = currentPath.split('/').filter(Boolean);
    const navLinks = document.querySelectorAll('.sidebar .nav-link');

    navLinks.forEach(link => {
        const href = link.getAttribute('href');
        if (!href || href === '#') return;

        const linkSegments = href.toLowerCase().split('/').filter(Boolean);
        let isMatch = false;

        if (linkSegments.length >= 2) {
            const linkArea = linkSegments[0];       // 'admin'
            const linkController = linkSegments[1]; // 'dashboard', 'products', etc.

            if (currentSegments.length >= 2) {
                // Normal case: /Admin/Products, /Admin/Dashboard, etc.
                isMatch = linkArea === currentSegments[0] &&
                    linkController === currentSegments[1];

            } else if (currentSegments.length === 1) {
                // URL is just /Admin — only match Dashboard if it's the Dashboard link
                // but DON'T auto-activate Dashboard for other pages
                isMatch = linkArea === currentSegments[0] &&
                    linkController === 'dashboard' &&
                    currentSegments[0] === 'admin';

            } else {
                // Root URL / — only match Dashboard link explicitly
                isMatch = linkController === 'dashboard' && linkArea === 'admin';
            }

            // 🔒 Key fix: Dashboard link must ONLY be active on the dashboard page
            if (linkController === 'dashboard' && currentSegments.length >= 2) {
                isMatch = linkArea === currentSegments[0] &&
                    currentSegments[1] === 'dashboard';
            }

        } else if (linkSegments.length === 1) {
            isMatch = currentSegments[currentSegments.length - 2] === linkSegments[0] ||
                currentSegments[0] === linkSegments[0];
        }

        if (isMatch) link.classList.add('active');
    });

    // Neon flicker
    const brand = document.querySelector('.sidebar h4');
    if (brand) {
        brand.style.opacity = '0.8';
        setTimeout(() => { brand.style.opacity = '1'; }, 100);
    }
});