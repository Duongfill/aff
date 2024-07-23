$(document).ready(function () {
    $('#searchInput').on('input', function () {
        search();
    });

    $('#searchInput').on('click', function (e) {
        e.stopPropagation();
        $('#searchResults').toggle();
        $('#searchterm').toggle();
        $('#errorMessage').hide();
    });
    $('#searchInput').on('focus', function () {
        // Hiển thị lịch sử tìm kiếm
        showSearchHistory();
    });

    // Xử lý sự kiện khi input mất focus (không chọn)
    $('#searchInput').on('blur', function () {
        hideSearchHistory();
    });
    $(document).on('click', function (e) {
        if (!$(e.target).closest('#searchterm').length) {
            $('#searchResults').empty();
            $('#searchResults').hide();
            $('#errorMessage').hide();
        }
    });

    function search() {
        var query = $('#searchInput').val().trim();
        if (query.length > 2) {
            $.ajax({
                url: '/Home/Search',
                method: 'GET',
                data: { query: query },
                dataType: 'json',
                success: function (response) {
                    displaySearchResults(response);
                },
                error: function (xhr, status, error) {
                    console.error('Error:', error);
                }
            });
        }
    }
    function displaySearchResults(results) {
        var $resultsContainer = $('#searchResults');
        $resultsContainer.empty();
        if (results.length === 0) {
            $resultsContainer.append('<li class="no-results">Không có kết quả tìm kiếm</li>');
        } else {
            var maxResults = Math.min(results.length, 5);
            for (var i = 0; i < maxResults; i++) {
                $resultsContainer.append('<li class="result"><a href="/Deals/Details/?id=' + results[i].id + '">' + results[i].name + '</a></li>');
            }
        }
    }
    function showSearchHistory() {
        $('#searchResults').empty();
        var searchHistory = JSON.parse(localStorage.getItem("searchHistory"));
        if (searchHistory && searchHistory.length > 0) {
            searchHistory.forEach(function (keyword) {
                $('#searchResults').append('<li class="result"><a href="/Home/SearchAll/?query='+keyword+'">' + keyword + '</a></li>');
        });
            $('#searchResults').show();
            $('#searchterm').show();
        }
    }
    $('#searchInput').on('keydown', function (e) {
        var key = e.which || e.keyCode;
        if (key === 40) { // Kiểm tra nếu phím nhấn là mũi tên xuống
            e.preventDefault();
            var $current = $('.result.focus');
            if ($current.length == 0) {
                $('.result').eq(0).addClass('focus');
            } else {
                $current.removeClass('focus');
                var $next = $current.next('.result');
                if ($next.length === 0) {
                    $next = $('.result').eq(0);
                }
                $next.addClass('focus');
            }
        }
    });
    $('#searchForm').submit(function (event) {
        event.preventDefault();
        var query = $('#searchInput').val().trim();
        $('#searchQuery').val(query);
        if (!query) {
            window.location.href = "/Home/Index"
        } else {
            saveSearchKeyword(query);
            var currentPage = getUrlParameter('page') || 1; // Lấy giá trị trang hiện tại từ URL
            window.location.href = "/Home/SearchAll/?query=" + encodeURIComponent(query) + "&page=" + currentPage;
        }
    });

    function getUrlParameter(name) {
        name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
        var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
        var results = regex.exec(location.search);
        return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
    };
    $('#searchInput').on('input', function () {
        $('#errorMessage').hide();
    });
    function saveSearchKeyword(keyword) {
        var searchHistory = localStorage.getItem("searchHistory");
        if (!searchHistory) {
            localStorage.setItem("searchHistory", JSON.stringify([keyword]));
        } else {
            var historyArray = JSON.parse(searchHistory);
            historyArray.unshift(keyword);
            if (historyArray.length > 5) {
                historyArray.pop();
                
            }localStorage.setItem("searchHistory", JSON.stringify(historyArray));
        }
    }
});
