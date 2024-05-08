var currentDate = new Date();

var formattedDate = currentDate.toISOString().split('T')[0];
document.getElementById('ngayf').value = formattedDate;

function exportToPDF() {
    var ngay = document.getElementById('ngayf').value; // Lấy giá trị ngày từ trường nhập liệu
    $.ajax({
        url: '/Admin/ExportToPDF', // Thay đổi URL để gọi hàm ExportToPDF trong Controller
        type: 'POST',
        data: { ngay: ngay }, // Truyền ngày qua dữ liệu của yêu cầu POST
        xhrFields: {
            responseType: 'blob' // Đảm bảo phản hồi là dạng blob
        },
        success: function (data) {
            var blob = new Blob([data], { type: 'application/pdf' }); // Thay đổi kiểu dữ liệu của blob thành 'application/pdf'
            var link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = 'QuanSo.pdf'; // Thay đổi tên file xuất ra thành 'QuanSo.pdf'
            link.click();
        },
        error: function (xhr, status, error) {
            // Xử lý lỗi
        }
    });
}
