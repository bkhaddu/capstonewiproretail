document.addEventListener("DOMContentLoaded", function () {
    const reveals = document.querySelectorAll(".reveal");

    if (!reveals.length) return;

    const observer = new IntersectionObserver(
        (entries) => {
            entries.forEach((entry) => {
                if (!entry.isIntersecting) return;
                entry.target.classList.add("visible");
                observer.unobserve(entry.target);
            });
        },
        { threshold: 0.1 }
    );

    reveals.forEach((el, index) => {
        el.style.transitionDelay = `${Math.min(index * 50, 300)}ms`;
        observer.observe(el);
    });
});
