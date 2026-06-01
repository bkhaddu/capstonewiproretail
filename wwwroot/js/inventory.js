function checkStock(productId) {
    fetch(`/api/inventory/check/${productId}`)
        .then(response => response.json())
        .then(data => {
            alert(
                `Product: ${data.productName}\nStock: ${data.stockQuantity}\nAvailable: ${data.isAvailable}`
            );
        })
        .catch(error => {
            console.error("Error checking stock:", error);
        });
}