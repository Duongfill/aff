$(document).ready(function () {
    $('#cancelButton').click(function () {
        window.location.href = "Index"

    });
    $('#filterButton').click(function () {
        var selectedStatus = $('#statusDropdown').val();
        var datecreate = $('#createdAtInput').val();
        var textsearch = $('#textsearch').val();
        var currentPage = 1;
        if (selectedStatus === "All") {
            $.ajax({
                url: 'Index',
                type: 'GET',
                data: { page: currentPage, textsearch: textsearch, },
                success: function (data) {
                    window.location.href = "/Admin/Deals/Index?page=" + currentPage + "&textsearch=" + textsearch ;
                },
                error: function () {
                    console.log('Đã xảy ra lỗi khi gửi yêu cầu lọc.');
                }
            });
        }
        else {
            $.ajax({
                url: 'Index',
                type: 'GET',
                data: { page: currentPage, textsearch: textsearch, status: selectedStatus, createdat: datecreate },
                success: function (data) {
                    window.location.href = "/Admin/Deals/Index?page=" + currentPage + "&textsearch=" + textsearch + "&status=" + selectedStatus + "&createdat=" + datecreate;
                },
                error: function () {
                    console.log('Đã xảy ra lỗi khi gửi yêu cầu lọc.');
                }
            });
        }
    });

});
