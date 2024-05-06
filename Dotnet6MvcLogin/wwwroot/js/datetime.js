function formatDate(dateString) {
    var date = new Date(dateString);
    var day = date.getDate();
    var month = date.getMonth() + 1;
    var year = date.getFullYear();
    var hours = date.getHours();
    var minutes = date.getMinutes();
    var seconds = date.getSeconds();

    var formattedDate = (day < 10 ? '0' : '') + day + '/' + (month < 10 ? '0' : '') + month + '/' + year;

    // Kiểm tra xem có thông tin về giờ, phút, giây không
    if (hours !== 0 || minutes !== 0 || seconds !== 0) {
        formattedDate += ' ' + (hours < 10 ? '0' : '') + hours + ':' + (minutes < 10 ? '0' : '') + minutes + ':' + (seconds < 10 ? '0' : '') + seconds;
    }

    return formattedDate;
}

// Lặp qua tất cả các ô chứa ngày và định dạng lại ngày
document.addEventListener("DOMContentLoaded", function () {
    var dateCells = document.querySelectorAll('.date-formattt');
    dateCells.forEach(function (cell) {
        cell.textContent = formatDate(cell.textContent);
    });
});
