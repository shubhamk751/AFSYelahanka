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

        .alumni - cta {
    background: #f0f8ff; /* light blue background */
    padding: 40px 20px;
    border - radius: 10px;
    box - shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
}

.alumni - cta h2 {
    margin - bottom: 15px;
}

.alumni - cta p {
    margin - bottom: 25px;
    color: #555;
}