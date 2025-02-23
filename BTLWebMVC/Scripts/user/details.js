document.addEventListener("DOMContentLoaded", function () {
    const minusBtn = document.querySelector(".minus");
    const plusBtn = document.querySelector(".plus");
    const quantityValue = document.getElementById("quantity_value");
    const addToCartButton = document.querySelector(".add_to_cart_button");

    let quantity = 1;
    let interval;

    function updateQuantityDisplay() {
        quantityValue.textContent = quantity;
    }

    function updateButtonVisibility() {
        addToCartButton.style.display = quantity > 0 ? "inline-block" : "none";
    }

    function startIncrement() {
        interval = setInterval(function () {
            quantity++;
            updateQuantityDisplay();
            updateButtonVisibility();
        }, 150);
    }

    function startDecrement() {
        interval = setInterval(function () {
            if (quantity > 1) {
                quantity--;
                updateQuantityDisplay();
                updateButtonVisibility();
            }
        }, 150);
    }

    plusBtn.addEventListener("click", function () {
        quantity++;
        updateQuantityDisplay();
        updateButtonVisibility();
    });

    minusBtn.addEventListener("click", function () {
        if (quantity > 1) {
            quantity--;
            updateQuantityDisplay();
            updateButtonVisibility();
        }
    });

    plusBtn.addEventListener("mousedown", startIncrement);
    minusBtn.addEventListener("mousedown", startDecrement);

    document.addEventListener("mouseup", function () {
        clearInterval(interval);
    });

    plusBtn.addEventListener("mouseleave", function () {
        clearInterval(interval);
    });

    minusBtn.addEventListener("mouseleave", function () {
        clearInterval(interval);
    });

    updateQuantityDisplay();
    updateButtonVisibility();
});
document.addEventListener("DOMContentLoaded", function () {
    const thumbnails = document.querySelectorAll("#thumbnail-list .thumbnail-item");
    const mainImage = document.getElementById("main-image");

    thumbnails.forEach(thumbnail => {
        thumbnail.addEventListener("click", function () {
            thumbnails.forEach(item => item.classList.remove("active"));

            thumbnail.classList.add("active");

            const imageUrl = thumbnail.getAttribute("data-image");
            mainImage.style.backgroundImage = "url('" + imageUrl + "')";
        });
    });
});