document.addEventListener("DOMContentLoaded", function () {
    var sendotpButton = document.getElementById("sendotp");
    if (sendotpButton) {
        sendotpButton.addEventListener("click", function () {
            var otpInput = document.querySelector('input[name="Input.ConfirmPassword"]');
            var passwordInput = document.querySelector('input[name="Input.Password"]');
            var checkbox = document.querySelector('.checkbox');

            if (otpInput && passwordInput && checkbox) {
                otpInput.closest('.form-floating').style.display = "block"; // Hiển thị ô nhập mã OTP
                passwordInput.closest('.form-floating').style.display = "block"; // Hiển thị ô nhập mật khẩu
                checkbox.style.display = "block"; // Hiển thị checkbox
                document.getElementById("registerSubmit").style.display = "block"; // Hiển thị nút Đăng ký
            }
        });
    }
});
