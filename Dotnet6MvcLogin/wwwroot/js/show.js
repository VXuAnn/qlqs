document.addEventListener("DOMContentLoaded", function () {
    // Lấy ngày hiện tại
    var currentDate = new Date();

    // Lấy ngày, tháng, năm hiện tại
    var day = currentDate.getDate();
    var month = currentDate.getMonth() + 1; // Tháng trong JavaScript bắt đầu từ 0
    var year = currentDate.getFullYear();

    // Định dạng ngày tháng năm
    var formattedDate = (day < 10 ? '0' : '') + day + '/' + (month < 10 ? '0' : '') + month + '/' + year;

    // Gán giá trị mặc định cho input
    document.getElementById('ngay').value = formattedDate;