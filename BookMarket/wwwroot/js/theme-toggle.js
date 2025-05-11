const themeToggleBtn = document.getElementById('themeToggle');
const themeIcon = document.getElementById('themeIcon');
const htmlElement = document.documentElement;
const navbar = document.getElementById('mainNavbar');

const applyTheme = theme => {
    htmlElement.setAttribute('data-bs-theme', theme);
    themeIcon.className = theme === 'dark' ? 'bi bi-moon-stars-fill' : 'bi bi-brightness-high-fill';

    // Switch navbar theme classes
    if (theme === 'dark') {
        navbar.classList.remove('navbar-light', 'bg-light');
        navbar.classList.add('navbar-dark', 'bg-dark');
    } else {
        navbar.classList.remove('navbar-dark', 'bg-dark');
        navbar.classList.add('navbar-light', 'bg-light');
    }
};

const savedTheme = localStorage.getItem('theme') || (window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light');
applyTheme(savedTheme);

themeToggleBtn.addEventListener('click', () => {
    const newTheme = htmlElement.getAttribute('data-bs-theme') === 'dark' ? 'light' : 'dark';
    localStorage.setItem('theme', newTheme);
    applyTheme(newTheme);
});
