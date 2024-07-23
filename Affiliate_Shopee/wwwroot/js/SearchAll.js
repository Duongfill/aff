// Hàm xử lý phản hồi từ server và hiển thị kết quả tìm kiếm


document.getElementById("searchForm").addEventListener("submit", function (event) {
    event.preventDefault(); // Ngăn chặn hành động mặc định của form
    var query = document.getElementById("searchInput").value.trim();
    if (query !== "") {
        window.location.href = "/Home/SearchAll/?query=" + encodeURIComponent(query);
    }
});
function displaySearchResults(results) {
    console.log(results);
    var productsContainer = document.querySelector('.products-view-grid.products');
    productsContainer.innerHTML = '';
    results.forEach(function (result) {
        var productElement = document.createElement('div');
        productElement.classList.add('col-xs-6', 'col-sm-4', 'col-md-3', 'col-lg-15');
        productElement.innerHTML = `
            <div class="product-box">
                <div class="product-thumbnail">
                    <a href="/smart-tivi-samsung-4k-55-inch-ua55tu8500" title="Smart Tivi Samsung 4K 55 inch UA55TU8500">
                        <img src="//bizweb.dktcdn.net/100/270/860/themes/606449/assets/loaders.svg?1705895589400"
                             data-lazyload="//bizweb.dktcdn.net/thumb/large/100/270/860/products/ua55tu8500-org.jpg?v=1601347058923"
                             alt="Smart Tivi Samsung 4K 55 inch UA55TU8500" class="img-responsive center-block" />
                    </a>
                    
                </div>
                <div class="product-info a-center">
                    <a class="product-name" href="/smart-tivi-samsung-4k-55-inch-ua55tu8500"
                       title="Smart Tivi Samsung 4K 55 inch UA55TU8500">${result.name}</a>
                    <div class="price-box clearfix">
                        
                    </div>
                </div>
            </div>
        `;
        productsContainer.appendChild(productElement);
    });
}
