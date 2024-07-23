
$(document).ready(function () {
    $('#cancelButton').click(function () {
        window.location.href = "Index"
    
});
    $('#filterButton').click(function () {
        var selectedStatus = $('#statusDropdown').val();
        var selectedBank = $('#bankDropdown').val();
        var selectedCreatedAt = $('#createdAtInput').val();
        var selectedCreatedAtTo = $('#createdAtInputTo').val();
        var textsearch = $('#textsearch').val();//$('#statusDropdown').data('pageNumber')
        var currentPage = 1;
        if (selectedStatus === "All" && selectedBank !== "All" ) {
            $.ajax({
                url: 'Index',
                type: 'GET',
                data: { page: currentPage, textsearch: textsearch, createdat: selectedCreatedAt, bankname: selectedBank, todate: selectedCreatedAtTo },
                success: function (data) {
                    window.location.href = "/Seller/Orders/Index?page=" + currentPage + "&textsearch=" + textsearch + "&createdat=" + selectedCreatedAt + "&todate=" + selectedCreatedAtTo + "&bankname=" + selectedBank;
                },
                error: function () {
                    console.log('Đã xảy ra lỗi khi gửi yêu cầu lọc.');
                }
            });
        }
        else if (selectedBank === "All" && selectedStatus !== "All" ) {
            $.ajax({
                url: 'Index',
                type: 'GET',
                data: { page: currentPage, status: selectedStatus, textsearch: textsearch, createdat: selectedCreatedAt, todate: selectedCreatedAtTo },
                success: function (data) {
                    window.location.href = "/Seller/Orders/Index?page=" + currentPage + "&textsearch=" + textsearch + "&status=" + selectedStatus + "&createdat=" + selectedCreatedAt + "&todate=" + selectedCreatedAtTo;
                },
                error: function () {
                    console.log('Đã xảy ra lỗi khi gửi yêu cầu lọc.');
                }
            });
        }
        else if (selectedBank === "All" && selectedStatus === "All") {
            $.ajax({
                url: 'Index',
                type: 'GET',
                data: { page: currentPage, textsearch: textsearch, createdat: selectedCreatedAt, todate: selectedCreatedAtTo },
                success: function (data) {
                    window.location.href = "/Seller/Orders/Index?page=" + currentPage + "&textsearch=" + textsearch + "&createdat=" + selectedCreatedAt + "&todate=" + selectedCreatedAtTo;
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
                data: { page: currentPage, textsearch: textsearch, status: selectedStatus, createdat: selectedCreatedAt, todate: selectedCreatedAtTo, bankname: selectedBank },
                success: function (data) {
                    window.location.href = "/Seller/Orders/Index?page=" + currentPage + "&textsearch=" + textsearch + "&status=" + selectedStatus + "&createdat=" + selectedCreatedAt + "&todate=" + selectedCreatedAtTo +"&bankname=" + selectedBank;
                },
                error: function () {
                    console.log('Đã xảy ra lỗi khi gửi yêu cầu lọc.');
                }
            });
        }
    });
    $('.status-button').click(function () {
        var status = $(this).data('status');
        $('#Status').val(status); // Cập nhật giá trị của hidden input
        if (status == 'Từ chối đánh giá') {
            $('#feedbackReason').show(); // Hiển thị phần nhập lý do
        } else {
            $('#feedbackReason').hide(); // Ẩn phần nhập lý do
            $('#SellerFeedback').val(''); // Xóa nội dung nhập lý do nếu có
        }
    });

    // Lắng nghe sự kiện input để xóa nội dung của hộp văn bản nếu chọn trạng thái khác khi đã nhập lý do
    $('#SellerFeedback').on('input', function () {
        if ($('#Status').val() != 'Từ chối đánh giá') {
            // Xóa nội dung của hộp văn bản nếu trạng thái không phải là "Từ chối đánh giá"
            $(this).val('');
        }
    });

    // Lắng nghe sự kiện input để xóa nội dung của hộp văn bản nếu chọn trạng thái khác khi đã nhập lý do

    $('.status-button').click(function () {
        var status = $(this).data('status');
        var backgroundColor = ''; // Khởi tạo màu nền mới

        // Xác định màu nền mới dựa trên giá trị của status
        switch (status) {
            case 'Chờ hoàn tiền':
                backgroundColor = '#2A2A86';
                break;
            case 'Đã hoàn tiền':
                backgroundColor = '#028E02';
                break;
            case 'Từ chối đánh giá':
                backgroundColor = '#660000';
                break;
            default:
                backgroundColor = '#3897E9';
                break;
        }
        $('#status-div').html('<div style="border-radius:10px;color:white;padding:5px;text-align:center;background-color:' + backgroundColor + ';width:160px;margin:10px 10px 10px 0px;">' + status + '</div>');
        $('#Status').val(status);
    });

    var initialStatus = $('#Status').val();

    if (initialStatus === 'Đã hoàn tiền') {
        $('.status-button[data-status="Từ chối đánh giá"]').hide();
        $('.status-button[data-status="Chờ hoàn tiền"]').hide();
    }
    if (initialStatus === 'Từ chối đánh giá') {
        $('.status-button[data-status="Đã hoàn tiền"]').hide();
        $('.status-button[data-status="Chờ hoàn tiền"]').hide();
    }
    if (initialStatus === 'Chờ hoàn tiền') {
        $('.status-button[data-status="Từ chối đánh giá"]').hide();
    }

    var form = document.getElementById('editform');
    var statusInput = document.getElementById('Status');
    var rejectButton = document.querySelector('button[data-status="Từ chối đánh giá"]');
    var refundButton = document.querySelector('button[data-status="Chờ hoàn tiền"]');

    form.addEventListener('submit', function (event) {
        var status = statusInput.value;
        
        if (status === 'Đã hoàn tiền') {
            if (!confirm('Bạn có chắc chắn muốn thay đổi trạng thái này?')) {
                event.preventDefault();
            } else {
                rejectButton.style.display = 'none';
                refundButton.style.display = 'none';
            }
        }
    });
    var form = document.getElementById('editform');
    var statusButtons = document.querySelectorAll('.status-button');
    var statusInput = document.getElementById('Status');
    var feedbackReason = document.getElementById('feedbackReason');
    var feedbackTextarea = document.querySelector('textarea[name="SellerFeedback"]');
    var submitButton = document.getElementById('submitButton');

    statusButtons.forEach(function (button) {
        button.addEventListener('click', function () {
            var status = this.getAttribute('data-status');
            statusInput.value = status;

            if (status === 'Từ chối đánh giá') {
                feedbackReason.style.display = 'block';
            } else {
                feedbackReason.style.display = 'none';
                feedbackTextarea.value = ''; 
            }
        });
    });

    form.addEventListener('submit', function (event) {
        var status = statusInput.value;

        if (status === 'Từ chối đánh giá' && feedbackTextarea.value.trim() === '') {
            event.preventDefault();
            alert('Vui lòng nhập lý do từ chối đánh giá.');
        }
    });
    
});
