$(document).ready(function () {
    $("#myForm").on("submit", function (event) {
        event.preventDefault();

        var formData = new FormData(this);

        $.ajax({
            url: $(this).attr("action"),
            type: $(this).attr("method"),
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                if (response.success) {
                    toastr.success(response.message);
                    // Optional: Redirect to a different page if needed
                     window.location.href = "/Seller/Deals/Index";
                } else {
                    toastr.error(response.message);
                }
            },
            error: function (xhr, status, error) {
                toastr.error('Bạn phải nhập đủ thông tin vào các trường có dấu *.');
                console.error(xhr.responseText);
            }
        });
    });
});