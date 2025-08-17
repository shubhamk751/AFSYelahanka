const gallery = document.querySelector('.dock-gallery');
let isDown = false;
let startX;
let scrollLeft;

//Drag scroll for desktop
if (window.innerWidth >= 768) {
    gallery.addEventListener('mousedown', (e) => {
        isDown = true;
        gallery.classList.add('dragging');
        startX = e.pageX - gallery.offsetLeft;
        scrollLeft = gallery.scrollLeft;
    });

    gallery.addEventListener('mouseleave', () => {
        isDown = false;
        gallery.classList.remove('dragging');
    });

    gallery.addEventListener('mouseup', () => {
        isDown = false;
        gallery.classList.remove('dragging');
    });

    gallery.addEventListener('mousemove', (e) => {
        if (!isDown) return;
        e.preventDefault();
        const x = e.pageX - gallery.offsetLeft;
        const walk = (x - startX) * 2; // Scroll speed
        gallery.scrollLeft = scrollLeft - walk;
    });
}

// Scroll buttons
const leftBtn = document.querySelector('.scroll-btn.left');
const rightBtn = document.querySelector('.scroll-btn.right');

leftBtn.addEventListener('click', () => {
    gallery.scrollBy({ left: -200, behavior: 'smooth' });
});

rightBtn.addEventListener('click', () => {
    gallery.scrollBy({ left: 200, behavior: 'smooth' });
});