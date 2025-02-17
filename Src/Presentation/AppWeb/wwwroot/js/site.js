// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$('.carousel_in').owlCarousel({
    center: true,
    items: 1,
    loop: true,
    autoplay: true,
    navText: ['', ''],
    addClassActive: true,
    margin: 5,
    responsive: {
        600: {
            items: 1
        },
        1000: {
            items: 2,
            nav: true,
        }
    }
});
