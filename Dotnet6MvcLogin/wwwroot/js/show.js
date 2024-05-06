
    // Hàm để định dạng ngày từ "yyyy-MM-dd" thành "dd/MM/yy"
    function formatDate(dateString) {
        var date = new Date(dateString);
    var day = date.getDate();
    var month = date.getMonth() + 1;
    var year = date.getFullYear().toString().slice(-2); // Lấy 2 chữ số cuối của năm
    return (day < 10 ? '0' : '') + day + '/' + (month < 10 ? '0' : '') + month + '/' + year;
    }

    // Lặp qua tất cả các ô chứa ngày và định dạng lại ngày
    document.addEventListener("DOMContentLoaded", function () {
        var dateCells = document.querySelectorAll('.date-format');
    dateCells.forEach(function (cell) {
        cell.textContent = formatDate(cell.textContent);
        });
    });

