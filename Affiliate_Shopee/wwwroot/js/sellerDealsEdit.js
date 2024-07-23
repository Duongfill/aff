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
function validateNameDeal() {
    var nameDealInput = document.getElementById('Name');
    var nameDealValue = nameDealInput.value.trim();
    nameDealValue = nameDealValue.replace(/\s{2,}/g, ' ');
    var nameDealError = document.getElementById('nameDealError');

    if (nameDealValue === '') {
        nameDealError.textContent = 'Tên Deal không được để trống';
        return false;
    }
    else if (nameDealValue.length < 4 || nameDealValue.length > 50) {
        nameDealError.textContent = 'Tên sản phẩm phải có 4 đến 50 ký tự';
        return false;
    }
    else {
        nameDealInput.style.borderColor = '';
        nameDealError.textContent = '';
        return true;
    }
}
//function validateSlot() {
//    var slot = document.getElementById('Slot');
//    var slotError = document.getElementById('slotError');
//    if (slot.value < 10) {
//        slotError.textContent = 'Slot tối thiểu là 10';
//        return false;
//    } else {
//        slot.style.borderColor = '';
//        slotError.textContent = '';
//        return true;
//    }
//}
function validatePrice() {
    var shopeeprice = document.getElementById('ShopeePrice');
    var ShopeePriceError = document.getElementById('ShopeePriceError');

    if (shopeeprice.value < 1000 || shopeeprice === '') {
        ShopeePriceError.textContent = 'Giá bán tối thiểu là 1.000 đ';
        return false;
    } else {
        shopeeprice.style.borderColor = '';
        ShopeePriceError.textContent = '';
        return true;
    }
}
function validateRefundPrice() {
    var refundprice = parseFloat(document.getElementById('RefundPrice').value);
    var shopeeprice = parseFloat(document.getElementById('ShopeePrice').value);
    //var refundpricest = document.getElementById('RefundPrice';
    var RefundPriceError = document.getElementById('RefundPriceError');

    if (isNaN(refundprice) || refundprice === '') {
        RefundPriceError.textContent = 'Số tiền không được để trống và phải là số';
        return false;
    }
    else if (refundprice < shopeeprice * 0.25) {
        RefundPriceError.textContent = 'Số tiền tối thiểu phải bằng 25% giá sản phẩm';
        return false;
    }
    else if (refundprice > shopeeprice) {
        RefundPriceError.textContent = 'Số tiền hoàn không được lớn hơn giá sản phẩm';
        return false;
    }
    else {
        RefundPriceError.textContent = '';
        return true;
    }
}
function validateLinkShopee() {
    var linkInput = document.getElementById('ShopeeLink');
    var linkValue = linkInput.value.trim();
    var errorSpan = document.getElementById('linkShopeeError');


    if (linkValue === '') {
        errorSpan.innerHTML = 'Vui lòng nhập link sản phẩm!<br/>';
        return false;
    } else if (!linkValue.includes('https://shopee.vn/')) {
        errorSpan.textContent = 'Link không hợp lệ';
        return false;
    } else {
        errorSpan.innerHTML = '';
        return true;
    }
}


var nameShopeeInput = document.getElementById('Name');
var ShopeelinkInput = document.getElementById('ShopeeLink');
var slotInput = document.getElementById('Slot');
/*var slotError = document.getElementById('slotError');*/
var ShopeePrice = document.getElementById('ShopeePrice');
var RefundPrice = document.getElementById('RefundPrice');
var dealexpiredat = document.getElementById('dealexpiredat');


nameShopeeInput.addEventListener('blur', validateNameDeal);
ShopeelinkInput.addEventListener('blur', validateLinkShopee);
/*slotInput.addEventListener('blur', validateSlot);*/
ShopeePrice.addEventListener('blur', validatePrice);
RefundPrice.addEventListener('blur', validateRefundPrice);
dealexpiredat.addEventListener('blur', checkdate);

//function checkdate() {
//    var selectedDate = new Date(document.getElementById('dealexpiredat').value);
//    var currentDate = new Date();

//    if (selectedDate <= currentDate) {
//        document.getElementById('dealExpiredatError').innerText = 'Ngày hết hạn phải lớn hơn ngày hiện tại';
//        return false;
//    }
//    else if (selectedDate == null) {
//        return true;
//    }
//    else {
//        document.getElementById('dealExpiredatError').innerText = '';
//        return true;
//    }


//}
function checkdate() {
    var dealExpired = document.querySelector('input[name="DealExpired"]:checked').value;

    if (dealExpired === "0") {
        // Nếu người dùng chọn "Đến ngày"
        var selectedDate = new Date(document.getElementById('dealexpiredat').value);
        var currentDate = new Date();

        if (!selectedDate) {
            // Nếu trường nhập ngày trống
            document.getElementById('dealExpiredatError').innerText = 'Vui lòng chọn ngày hết hạn.';
            return false;
        } else if (selectedDate <= currentDate) {
            // Nếu ngày hết hạn nhỏ hơn hoặc bằng ngày hiện tại
            document.getElementById('dealExpiredatError').innerText = 'Ngày hết hạn phải lớn hơn ngày hiện tại.';
            return false;
        } else {
            // Ngày hợp lệ
            document.getElementById('dealExpiredatError').innerText = '';
            return true;
        }
    } else {
        // Nếu người dùng chọn "Hết slot"
        document.getElementById('dealExpiredatError').innerText = '';
        return true;
    }
}



//function validateForm() {
//    var isNameValid = document.getElementById('Name').value !== '' ? validateNameDeal() : false;
//    /*var isSlotValid = document.getElementById('Slot').value !== '' ? validateSlot() : false;*/
//    var isPriceValid = document.getElementById('ShopeePrice').value !== '' ? validatePrice() : false;
//    var isRefundPriceValid = document.getElementById('RefundPrice').value !== '' ? validateRefundPrice() : false;
//    var isLinkValid = document.getElementById('ShopeeLink').value !== '' ? validateLinkShopee() : false;

//    var isDateValid = checkdate();



//    // Kiểm tra nếu tất cả các trường đều không có lỗi
//    if (isNameValid && isPriceValid && isRefundPriceValid && isLinkValid && isDateValid) {
//        // Hiển thị nút "Đăng ký"
//        document.getElementById('submitbutton').style.display = 'block';
//    } else {
//        // Ẩn nút "Đăng ký"
//        document.getElementById('submitbutton').style.display = 'none';
//    }
//}

//document.getElementById('Name').addEventListener('input', validateForm);
//document.getElementById('ShopeeLink').addEventListener('input', validateForm);
//document.getElementById('Slot').addEventListener('input', validateForm);
//document.getElementById('ShopeePrice').addEventListener('input', validateForm);
//document.getElementById('RefundPrice').addEventListener('input', validateForm);
//document.getElementById('dealexpiredat').addEventListener('input', validateForm);


//document.getElementById('submitbutton').style.display = 'none';

function huy() {
    location.reload();
}
function dangky() {
    var nameDealInput = document.getElementById('Name');
    var nameDeal = nameDealInput.value.trim();
    nameDeal = nameDeal.replace(/\s{2,}/g, ' ');
    var linkshopeeInput = document.getElementById('ShopeeLink');
    var linkshopee = linkshopeeInput.value.trim();
    var imgInput = document.getElementById('ImgDeal');
    var imgInputError = document.getElementById('ImageDealError');
    var slot = document.getElementById('Slot');
    var refundprice = parseFloat(document.getElementById('RefundPrice').value);
    var shopeeprice = parseFloat(document.getElementById('ShopeePrice').value);
    var refundpricest = document.getElementById('RefundPrice');
    var shopeepricest = document.getElementById('ShopeePrice');

    var slotError = document.getElementById('slotError');
    var nameDealError = document.getElementById('nameDealError');
    var linkShopeeError = document.getElementById('linkShopeeError');
    var ShopeePriceError = document.getElementById('ShopeePriceError');
    var RefundPriceError = document.getElementById('RefundPriceError');
    var selectedDate = new Date(document.getElementById('dealexpiredat').value);
    var currentDate = new Date();
    var isValid = true;



    if (nameDeal === '') {
        nameDealInput.style.borderColor = 'red';
        nameDealError.textContent = 'Tên deal không được để trống!';
        isValid = false;
    } else if (nameDeal.length < 4 || nameDeal.length > 50) {
        nameDealInput.style.borderColor = 'red';
        nameDealError.textContent = 'Tên sản phẩm phải có 4 đến 50 ký tự';
        isValid = false;
    } else {
        nameDealInput.style.borderColor = '';
        nameDealError.textContent = '';

    }

    if (!/^https:\/\/shopee\.vn\//i.test(linkshopee)) {
        linkshopeeInput.style.borderColor = 'red';
        linkShopeeError.innerHTML = 'Link không hợp lệ! <br/>';
        isValid = false;
    }
    else if (linkshopee === '') {
        linkshopeeInput.style.borderColor = 'red';
        linkShopeeError.innerHTML = 'Vui lòng nhập link mua sản phẩm <br/>';
        isValid = false;
    }
    else {
        linkshopeeInput.style.borderColor = '';
        linkShopeeError.textContent = '';

    }
    if (slot.value < 10 || slot.value == '') {
        slot.style.borderColor = 'red';
        slotError.textContent = 'Slot tối thiểu là 10';
        isValid = false;
    }
    else {
        slot.style.borderColor = '';
        slotError.textContent = '';

    }

    if (shopeeprice < 1000 || shopeeprice == '' || isNaN(shopeeprice)) {
        shopeepricest.style.borderColor = 'red';
        ShopeePriceError.textContent = 'Giá bán tối thiểu là 1.000 đ';
        isValid = false;
    }
    else {
        shopeepricest.style.borderColor = '';
        ShopeePriceError.textContent = '';

    }

    if (isNaN(refundprice) || refundprice === '') {
        refundpricest.style.borderColor = 'red';
        RefundPriceError.textContent = 'Số tiền không được để trống và phải là số';
        isValid = false;
    }
    else if (refundprice < shopeeprice * 0.25) {
        refundpricest.style.borderColor = 'red';
        RefundPriceError.textContent = 'Số tiền tối thiểu phải bằng 25% giá sản phẩm';
        isValid = false;
    }
    else if (refundprice > shopeeprice) {
        refundpricest.style.borderColor = 'red';
        RefundPriceError.textContent = 'Số tiền hoàn không được lớn hơn giá sản phẩm';
        isValid = false;
    }
    else {
        refundpricest.style.borderColor = '';
        RefundPriceError.textContent = '';
    }


    if (imgInput.files.length === 0) {

        imgInputError.textContent = 'Vui lòng chọn ảnh sản phẩm';
        isValid = false;
    } else {

        imgInputError.textContent = '';

    }

    if (selectedDate <= currentDate) {
        document.getElementById('dealExpiredatError').innerText = 'Ngày hết hạn phải lớn hơn ngày hiện tại';
        isValid = false;
    } else {
        document.getElementById('dealExpiredatError').innerText = '';

    }


    return isValid;

}

function handleResponse(message) {
    alert(message); // Hiển thị thông báo
    window.location.href = '/Seller/Deals/Index'; // Chuyển hướng trang
}

document.querySelectorAll('input[type="radio"][name="freeship"]').forEach(function (radio) {
    radio.addEventListener('change', function () {
        if (radio.value === '1') {
            document.querySelector('input[type="radio"][value="0"]').checked = false;
        } else {
            document.querySelector('input[type="radio"][value="1"]').checked = false;
        }
    });
});
function toggleDateInput(dealExpired) {
    var dateInput = document.getElementById("expirationDateInput");
    var dateInputField = document.getElementById("dealexpiredat");
    if (dealExpired == 0) {
        dateInput.style.display = "block";
        dateInputField.disabled = false;

    } else {
        dateInput.style.display = "none";
        dateInputField.value = null; // Đặt giá trị của trường nhập liệu "Đến ngày" về null
        dateInputField.disabled = true;

    }
}
document.addEventListener("DOMContentLoaded", function () {
    // Lấy giá trị của DealExpired hiện tại và gọi hàm toggleDateInput
    var dealExpiredValue = document.querySelector('input[name="DealExpired"]:checked').value;
    toggleDateInput(dealExpiredValue);
});
document.querySelectorAll('input[name="DealExpired"]').forEach(function (radio) {
    radio.addEventListener('change', function () {
        // Lấy giá trị của radio button được chọn
        var dealExpiredValue = this.value;
        // Gọi hàm toggleDateInput để cập nhật hiển thị của phần nhập ngày
        toggleDateInput(dealExpiredValue);
    });
});
