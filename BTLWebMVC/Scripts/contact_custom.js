/* JS Document */

/******************************

[Table of Contents]

1. Vars and Inits

******************************/

jQuery(document).ready(function ($) {
    "use strict";

    var header = $('.header');
    var topNav = $('.top_nav');
    var hamburger = $('.hamburger_container');
    var menu = $('.hamburger_menu');
    var menuActive = false;
    var hamburgerClose = $('.hamburger_close');
    var fsOverlay = $('.fs_menu_overlay');

    setHeader();

    $(window).on('resize', function () {
        setHeader();
    });

    $(document).on('scroll', function () {
        setHeader();
    });

    initMenu();
    initMap();

    function setHeader() {
        if (window.innerWidth < 992) {
            if ($(window).scrollTop() > 100) {
                header.css({ 'top': "0" });
            } else {
                header.css({ 'top': "0" });
            }
        } else {
            if ($(window).scrollTop() > 100) {
                header.css({ 'top': "-50px" });
            } else {
                header.css({ 'top': "0" });
            }
        }
        if (window.innerWidth > 991 && menuActive) {
            closeMenu();
        }
    }

    function initMenu() {
        if (hamburger.length) {
            hamburger.on('click', function () {
                if (!menuActive) {
                    openMenu();
                }
            });
        }

        if (fsOverlay.length) {
            fsOverlay.on('click', function () {
                if (menuActive) {
                    closeMenu();
                }
            });
        }

        if (hamburgerClose.length) {
            hamburgerClose.on('click', function () {
                if (menuActive) {
                    closeMenu();
                }
            });
        }

        if ($('.menu_item').length) {
            var items = document.getElementsByClassName('menu_item');
            var i;

            for (i = 0; i < items.length; i++) {
                if (items[i].classList.contains("has-children")) {
                    items[i].onclick = function () {
                        this.classList.toggle("active");
                        var panel = this.children[1];
                        if (panel.style.maxHeight) {
                            panel.style.maxHeight = null;
                        } else {
                            panel.style.maxHeight = panel.scrollHeight + "px";
                        }
                    }
                }
            }
        }
    }

    function openMenu() {
        menu.addClass('active');
        fsOverlay.css('pointer-events', "auto");
        menuActive = true;
    }

    function closeMenu() {
        menu.removeClass('active');
        fsOverlay.css('pointer-events', "none");
        menuActive = false;
    }

    function initMap() {
        var latitude = mapData.latitude;
        var longitude = mapData.longitude;
        var locationName = mapData.locationName;

        var map = L.map('map').setView([latitude, longitude], 16);

        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '© <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
            maxZoom: 18,
        }).addTo(map);

        var marker = L.marker([latitude, longitude]).addTo(map);
        marker.bindPopup(locationName).openPopup();

        window.addEventListener('resize', function () {
            setTimeout(function () {
                map.invalidateSize();
                map.setView([latitude, longitude], 16);
            }, 1400);
        });
    }
});