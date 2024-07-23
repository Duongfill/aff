﻿
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
