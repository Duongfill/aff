$(document).ready(function () {
    var itemsPerPage = 6;
    var totalItems = $('.deal-item').length;
    var totalPages = Math.ceil(totalItems / itemsPerPage);
    var currentPage = 1;

    function showPage(page) {
        $('.deal-item').removeClass('active');
        var start = (page - 1) * itemsPerPage;
        var end = start + itemsPerPage;
        $('.deal-item').slice(start, end).addClass('active');

        // Thêm các ô trống nếu không đủ dữ liệu
        //var visibleItems = $('.deal-item.active').length;
        //for (var i = visibleItems; i < itemsPerPage; i++) {
        //    $('#dealCategoriesContainer').append('<div class="col-md-2 col-sm-6 col-xs-12 deal-item empty-item active" style="text-align: center;"><a href="#"><img style="width: 70px;height:70px" src="/image/Asset 8.png" alt="SMO MEDIA"></a><h6>SMO MEDIA</h6></div>');
        //}

        $('#prevSlide').prop('disabled', currentPage === 1);
        $('#nextSlide').prop('disabled', currentPage === totalPages);
    }

    $('#prevSlide').click(function () {
        if (currentPage > 1) {
            currentPage--;
            showPage(currentPage);
        }
    });

    $('#nextSlide').click(function () {
        if (currentPage < totalPages) {
            currentPage++;
            showPage(currentPage);
        }
    });

    showPage(currentPage);
});
