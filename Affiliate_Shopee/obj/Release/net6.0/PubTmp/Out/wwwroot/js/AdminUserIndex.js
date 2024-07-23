$(document).ready(function () {
    $('#cancelButton').click(function () {
        window.location.href = "Index"

    });
    $('#filterButton').click(function () {
        var selectedStatus = $('#statusDropdown').val();
        var selectedStatus1 = $('#statusDropdown1').val();
        var textsearch = $('#phonenumber').val();
        var currentPage = 1;
        if (selectedStatus === "All") {
            $.ajax({
                url: 'Index',
                type: 'GET',
                data: { page: currentPage, textsearch: textsearch, },
                success: function (data) {
                    window.location.href = "/Admin/AspNetUsers/Index?page=" + currentPage + "&phonenumber=" + textsearch;
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
                data: { page: currentPage, textsearch: textsearch, status: selectedStatus, statusnew: selectedStatus1 },
                success: function (data) {
                    window.location.href = "/Admin/AspNetUsers/Index?page=" + currentPage + "&phonenumber=" + textsearch + "&status=" + selectedStatus + "&discriminator=" + selectedStatus1;
                },
                error: function () {
                    console.log('Đã xảy ra lỗi khi gửi yêu cầu lọc.');
                }
            });
        }
    });

});
function updateStatusReason(element) {
    var id = $(element).data('id');
    var status = $(element).prop('checked') ? 'Đang hoạt động' : 'Ngừng hoạt động';

    $.ajax({
        url: '/Admin/AspNetUsers/UpdateStatus',
        type: 'POST',
        data: { id: id, status: status },
        success: function (data) {
            showSuccessToast('Cập nhật trạng thái thành công!');
        },
        error: function (xhr, status, error) {
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