
    document.addEventListener("DOMContentLoaded", function () {
        var currentDate = new Date();
    
        var formattedDate = currentDate.toISOString().split('T')[0];
        document.getElementById('ngayf').value = formattedDate;
    });
    function exportToExcel() {
        var ngay = document.getElementById('ngayf').value; // Lấy giá trị ngày từ trường nhập liệu
        $.ajax({
            url: '/Admin/ExportToExcel', // Không truyền ngày vào URL
            type: 'POST',
            data: { ngay: ngay }, // Truyền ngày qua dữ liệu của yêu cầu POST
            xhrFields: {
                responseType: 'blob' // Đảm bảo phản hồi là dạng blob
            },
            success: function (data) {
                var blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
                var link = document.createElement('a');
                link.href = window.URL.createObjectURL(blob);
                link.download = 'QuanSo.xlsx';
                link.click();
            },
            error: function (xhr, status, error) {
                // Xử lý lỗi
            }
        });
    }

   