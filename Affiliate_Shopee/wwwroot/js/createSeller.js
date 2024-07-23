
function getFileName(event) {
    var fileName = event.target.files[0].name;
    document.getElementById("Nameimg").value = fileName;
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
        };
    })(img);
    reader.readAsDataURL(file);
}
function validateNameShop() {
    var nameShopInput = document.getElementById('NameShop');
    var nameShopValue = nameShopInput.value.trim();
    var nameShopError = document.getElementById('nameShopError');
    if (nameShopValue === '') {
        nameShopError.textContent = 'Tên shop không được để trống!';
        return false;
    } else if (nameShopValue.length < 4 || nameShopValue.length > 50) {
        nameShopError.textContent = 'Tên shop phải có 4 đến 50 ký tự!';
        return false;
    }
    else {
        nameShopInput.style.borderColor = '';
        nameShopError.textContent = '';
        return true;
    }
}
function validateImgFile() {
    var imgInput = document.getElementById('ImgFB');
    var imgError = document.getElementById('imgError');
    if (imgInput.files.length === 0) {
        imgError.textContent = 'Vui lòng chọn một tập tin ảnh!';
        return false;
    } else {
        imgError.textContent = '';
        return true;
    }
}
function validateLinkInput() {
    var linkInput = document.getElementById('Linkfb');
    var linkValue = linkInput.value.trim();
    var errorSpan = document.getElementById('linkFbError');
    if (linkValue.includes('https://www.facebook.com/')) {
        linkInput.style.borderColor = '';
        errorSpan.textContent = '';
        return true;
    }
    else if (linkValue === '') {
        errorSpan.innerHTML = 'Link Facebook không được để trống!<br/>';
        return false;
    }

    else {
        errorSpan.innerHTML = 'Link FaceBook không hợp lệ!<br/>';
        return false;
    }
}

var nameShopInput = document.getElementById('NameShop');
var imgInput = document.getElementById('ImgFB');
var linkInput = document.getElementById('Linkfb');



nameShopInput.addEventListener('blur', validateNameShop);
imgInput.addEventListener('change', validateImgFile);
linkInput.addEventListener('blur', validateLinkInput);


function dangky() {
    var nameShopInput = document.getElementById('NameShop');
    var nameShop = nameShopInput.value.trim();
    var linkfbInput = document.getElementById('Linkfb');
    var linkfb = linkfbInput.value.trim();
    var imgInput = document.getElementById('ImgFB');

    var nameShopError = document.getElementById('nameShopError');
    var linkFbError = document.getElementById('linkFbError');
    var imgError = document.getElementById('imgError');

    var isValid = true;

    if (nameShop === '') {
        nameShopInput.style.borderColor = 'red';
        nameShopError.textContent = 'Vui lòng nhập Tên Shop!';
        isValid = false;
    } else if (nameShop.length < 4 || nameShop.length > 50) {
        nameShopInput.style.borderColor = 'red';
        nameShopInput.classList.add('input-error');
        nameShopError.textContent = 'Tên shop phải có 4 đến 50 ký tự!';
        isValid = false;
    } else {
        nameShopInput.style.borderColor = '';
        nameShopError.textContent = '';
    }

    if (linkfb === '' || !/^https:\/\/(www\.)?facebook\.com\//i.test(linkfb)) {
        linkfbInput.style.borderColor = 'red';
        linkFbError.innerHTML = 'Vui lòng nhập Link Facebook hợp lệ! <br/>';
        isValid = false;
    } else {
        linkfbInput.style.borderColor = '';
        linkFbError.textContent = '';
    }

    if (imgInput.files.length === 0) {
        imgInput.style.borderColor = 'red';
        imgError.textContent = 'Vui lòng chọn Ảnh Avatar Facebook!';
        isValid = false;
    } else {
        imgInput.style.borderColor = '';
        imgError.textContent = '';
    }

    return isValid;
}




document.addEventListener('DOMContentLoaded', function () {
    document.getElementById('instructionLink').addEventListener('click', function (event) {
        event.preventDefault(); // Prevent default link behavior
        var instructionImage = document.getElementById('instructionImage');
        if (instructionImage.style.display === 'none') {
            instructionImage.style.display = 'block'; // Hiển thị ảnh nếu đang ẩn
        } else {
            instructionImage.style.display = 'none'; // Ẩn ảnh nếu đang hiển thị
        }
    });
});
$(document).ready(function () {
    $("#customer_register").on("submit", function (event) {
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
                    toastr.success('Đăng ký thành công!');
                    window.location.href = response.redirectUrl;
                } else {
                    toastr.error(response.message);
                }
            },
            error: function () {
                toastr.error('Có lỗi xảy ra, vui lòng thử lại.');
            }
        });
    });
});