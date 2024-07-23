
$(document).ready(function () {
    $('#createOrderForm').submit(function (e) {
        e.preventDefault(); // Ngăn chặn form gửi đi mặc định

        $.ajax({
            url: $(this).attr('action'), 
            type: $(this).attr('method'), 
            data: $(this).serialize(), 
            success: function (response) {
                if (response.success) {
                    window.location.href = response.shopeeLink;
                } else {
                    toastr.error('Deal đã hết hạn, không thể mua được .');
                }
            },
            error: function (xhr, status, error) {
                toastr.error('Đã xảy ra lỗi: ' + error);
            }
        });
    });
});

function checkInput() {
    var text = document.getElementById("text").value;
    var submitButton = document.getElementById("submitComplain");

    if (text.trim() !== "") {
        submitButton.style.display = "block";
    } else {
        submitButton.style.display = "none";
    }
}
$('#complainButton').click(function () {
    $('#complainForm').toggle();
    $('#complainButton1').hide();
});
function getFileName(event) {
    var fileName = event.target.files[0].name;
    document.getElementById("LinkName").value = fileName;
    var fileInput = event.target;
    var file = fileInput.files[0];

    var uploadLabel = document.getElementById("uploadLabel");
    var img = document.createElement("img");
    img.file = file;
    uploadLabel.innerHTML = '';
    uploadLabel.appendChild(img);

    var reader = new FileReader();
    reader.onload = (function (aImg) {
        return function (e) {
            aImg.onload = function () {
                var imgWidth = aImg.width;
                var imgHeight = aImg.height;
                var ratio = 1;
                if (imgWidth > 150 || imgHeight > 150) {
                    if (imgWidth > imgHeight) {
                        ratio = 150 / imgWidth;
                    } else {
                        ratio = 150 / imgHeight;
                    }
                }
                aImg.width = imgWidth * ratio;
                aImg.height = imgHeight * ratio;
                uploadLabel.style.width = (imgWidth * ratio) + "px";
                uploadLabel.style.height = (imgHeight * ratio) + "px";
                uploadLabel.style.border = "none";
            };
            aImg.src = e.target.result;
            var submitButton = document.getElementById("noti2");
            if (event.target.files.length > 0) {
                submitButton.style.display = "block";

            } else {
                submitButton.style.display = "none";
            }
        };
    })(img);
    reader.readAsDataURL(file);
}
function getFileName2(event) {
    var fileName = event.target.files[0].name;
    document.getElementById("LinkName").value = fileName;
    var fileInput = event.target;
    var file = fileInput.files[0];

    var uploadLabel = document.getElementById("uploadLabel");
    var img = document.createElement("img");
    img.file = file;
    uploadLabel.innerHTML = '';
    uploadLabel.appendChild(img);

    var reader = new FileReader();
    reader.onload = (function (aImg) {
        return function (e) {
            aImg.onload = function () {
                var imgWidth = aImg.width;
                var imgHeight = aImg.height;
                var ratio = 1;
                if (imgWidth > 150 || imgHeight > 150) {
                    if (imgWidth > imgHeight) {
                        ratio = 150 / imgWidth;
                    } else {
                        ratio = 150 / imgHeight;
                    }
                }
                aImg.width = imgWidth * ratio;
                aImg.height = imgHeight * ratio;
                uploadLabel.style.width = (imgWidth * ratio) + "px";
                uploadLabel.style.height = (imgHeight * ratio) + "px";
                uploadLabel.style.border = "none";

                // Hiển thị phần payment và nút xác nhận khi có tệp được chọn và một trong hai radio button được chọn
                var paymentDiv = document.getElementById("payment");
                var refundButton = document.getElementById("refund");
                var bankRadio = document.getElementById("bank");
                var momoRadio = document.getElementById("momo");

                if (event.target.files.length > 0) {
                    paymentDiv.style.display = "block";

                }

                bankRadio.addEventListener('click', function () {
                    if (event.target.files.length > 0 && bankRadio.checked) {
                        refundButton.style.display = "block";
                    }
                });

                momoRadio.addEventListener('click', function () {
                    if (event.target.files.length > 0 && momoRadio.checked) {
                        refundButton.style.display = "block";
                    }
                });
            };
            aImg.src = e.target.result;
        };
    })(img);
    reader.readAsDataURL(file);
}
