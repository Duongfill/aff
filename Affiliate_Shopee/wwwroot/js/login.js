function checkPhoneNumber() {
    var phoneNumberInput = document.getElementById('phoneNumberInput');
    var phoneNumber = phoneNumberInput.value.trim();
    var phoneNumberError = document.getElementById('phonenumber');
    /*!phoneNumber.startsWith('0') || phoneNumber.length > 10*/
    if (phoneNumber == '') {
        phoneNumberError.textContent = 'Số điện thoại phải bắt đầu bằng số 0.'
        return false;
    } else {
        phoneNumberError.textContent = '';
        return true;
    }
}
function checkPassword() {
    var phoneNumberInput = document.getElementById('password');
    var phoneNumber = phoneNumberInput.value.trim();
    var phoneNumberError = document.getElementById('passwordError');

    if (phoneNumber == '') {
        //phoneNumberInput.style.borderColor = 'red'
        //phoneNumberError.textContent = 'Nhập mật khẩu.';
        return false;
    } else {
        phoneNumberError.textContent = '';
        return true;
    }
}
var passwordInput = document.getElementById('password');
passwordInput.addEventListener('blur', checkPassword);
var phoneNumberInput = document.getElementById('phoneNumberInput');
phoneNumberInput.addEventListener('blur', checkPhoneNumber);






function limitDigits(input) {
    input.value = input.value.replace(/\D/g, '');
    if (input.value.length > 10) {
        input.value = input.value.slice(0, 10);
    }
    
}
document.addEventListener("DOMContentLoaded", function () {
    var phoneNumberInput = document.getElementById("phoneNumberInput");
    var passwordInput = document.getElementById("password");

    phoneNumberInput.addEventListener("focus", function () {
        phoneNumberInput.classList.remove("border-danger");
        document.getElementById("phonenumber").style.display = "none";
    });

    passwordInput.addEventListener("focus", function () {
        passwordInput.classList.remove("border-danger");
        document.getElementById("passwordError").style.display = "none";
    });
});
function togglePasswordVisibility() {
    var passwordInput = document.getElementById('password');
    var togglePassword = document.getElementById('togglePassword');
    if (passwordInput.type === 'password') {
        passwordInput.type = 'text';
        togglePassword.classList.remove('fa-eye');
        togglePassword.classList.add('fa-eye-slash');
    } else {
        passwordInput.type = 'password';
        togglePassword.classList.remove('fa-eye-slash');
        togglePassword.classList.add('fa-eye');
    }
}