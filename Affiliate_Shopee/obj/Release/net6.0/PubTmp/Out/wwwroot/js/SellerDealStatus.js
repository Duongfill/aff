//document.addEventListener('DOMContentLoaded', function () {
//    const toggle = document.querySelector('.toggle-input');
//    toggle.addEventListener('change', function () {
//        // Gửi trạng thái mới xuống server nếu cần
//        const newState = this.checked;
        
//    });
//});
function updateStatusReason(element) {
    var id = $(element).data('id');
    var status = $(element).prop('checked') ? 1 : 0;

    $.ajax({
        url: '/Seller/Deals/UpdateStatusReason',
        type: 'POST',
        data: { id: id, status: status },
        success: function (data) {
            showSuccessToast('Cập nhật trạng thái thành công!');
        },
        error: function (xhr, status, error) {
           /* showSuccessToast('Đã xảy ra lỗi khi cập nhật trạng thái!');*/
        }
    });
}
function showSuccessToast(message) {
    var toastContainer = document.getElementById('toast-container');
    var toast = document.createElement('div');
    toast.className = 'toast toast-success';
    toast.innerHTML = '<div class="toast-body" style="color:#333; font-weight: bold;">' + message + '</div>';
    toastContainer.appendChild(toast);
    $(toast).toast('show');
}
$(document).ready(function () {
    $('#cancelButton').click(function () {
        window.location.href = "Index"

    });
    $('#filterButton').click(function () {
        var selectedStatus = $('#statusDropdown').val();

        var selectedCreatedAt = $('#createdAtInput').val();
        var textsearch = $('#textsearch').val();
        var currentPage = 1;
        if (selectedStatus === "All") {
            $.ajax({
                url: 'Index',
                type: 'GET',
                data: { page: currentPage, textsearch: textsearch, createdat: selectedCreatedAt },
                success: function (data) {
                    window.location.href = "/Seller/Deals/Index?page=" + currentPage + "&textsearch=" + textsearch + "&createdat=" + selectedCreatedAt;
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
                data: { page: currentPage, textsearch: textsearch, status: selectedStatus, createdat: selectedCreatedAt },
                success: function (data) {
                    window.location.href = "/Seller/Deals/Index?page=" + currentPage + "&textsearch=" + textsearch + "&status=" + selectedStatus + "&createdat=" + selectedCreatedAt;
                },
                error: function () {
                    console.log('Đã xảy ra lỗi khi gửi yêu cầu lọc.');
                }
            });
        }
    });

});