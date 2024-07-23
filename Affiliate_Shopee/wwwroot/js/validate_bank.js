
    document.addEventListener("DOMContentLoaded", function() {
        document.getElementById("Name").addEventListener("input", function () {
            validateNameInput();
        });
        document.getElementById("Code").addEventListener("input", function () {
            validateCodeInput();
        });
    });

function validateNameInput() {
    var nameInput = document.getElementById("Name").value;
    // Kiểm tra nếu trường nhập liệu có chứa số hoặc là khoảng trống
    if (nameInput.match(/[0-9]/) || nameInput.trim() === "") {
        document.getElementById("codeError1").innerText = "Tên ngân hàng không được chứa số hoặc để trống!";
    } else {
        document.getElementById("codeError1").innerText = ""; // Xóa thông báo lỗi nếu không có lỗi
    }
}
function validateCodeInput() {
        
        var codeInput = document.getElementById("Code").value;
    // Kiểm tra nếu trường nhập liệu có chứa số hoặc là khoảng trống
    
    if (codeInput.match(/[0-9]/) || codeInput.trim() === "") {
        document.getElementById("codeError").innerText = "Tên viết tắt không được chứa số hoặc để trống!";
        } else {
        document.getElementById("codeError").innerText = ""; // Xóa thông báo lỗi nếu không có lỗi
        }
    }

