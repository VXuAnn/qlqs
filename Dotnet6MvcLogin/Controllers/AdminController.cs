using Dotnet6MvcLogin.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcLogin.Models;
using MvcLogin.Repositories;
using OfficeOpenXml;
using System.Data;
using System.IO;
using System;
/*using iText.Kernel.Pdf;*/
using iTextSharp.text;
using iTextSharp.text.pdf;
using Org.BouncyCastle.Utilities;




namespace Dotnet6MvcLogin.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        #region giới hạn thời gian
        public TimeSpan startHour;
        public TimeSpan endHour;
        private readonly Time _workingHoursService;

        public AdminController(Time workingHoursService)
        {
            _workingHoursService = workingHoursService;
        }
        public ActionResult SetHours(TimeSpan start, TimeSpan end)
        {



            _workingHoursService.startHour = start;
            _workingHoursService.endHour = end;


            return RedirectToAction("Index", "Admin"); // Chuyển hướng đến trang chủ hoặc trang khác
        }

        public TimeSpan GetStartHour()
        {
            return startHour;
        }

        public TimeSpan GetEndHour()
        {
            return endHour;
        }


        #endregion

        #region Xem người dùng
        public IActionResult Display(string id_dv)
        {
            List<Users> users = new List<Users>();
            try
            {

                RepoAdmin quanlyUser = new RepoAdmin();
                if (id_dv != null)
                {
                    users = quanlyUser.SearchUser(id_dv);
                }
                else
                {
                    users = quanlyUser.GetUsers();
                }
            }
            catch
            {

            }
            return View(users);
        }
        #endregion

        #region  Xem Quân Số
        public IActionResult Index(DateTime? ngay)
        {
            List<QuanSo> quanSos = new List<QuanSo>();

            try
            {
                RepoAdmin admin = new RepoAdmin(); // Tạo mới một đối tượng QuanLyQuanSo

                if (ngay.HasValue)
                {
                    quanSos = admin.SearchQuanSo(ngay.Value);
                }
                else
                {
                    // Sử dụng DateTime.MinValue để đại diện cho giá trị null
                    quanSos = admin.SearchQuanSo(DateTime.Now);
                }
            }
            catch
            {
                // Xử lý nếu có lỗi xảy ra khi tạo đối tượng QuanLyQuanSo hoặc khi gọi phương thức SearchQuanSoByDate hoặc GetBaoCaoQuanSo
            }

            return View(quanSos);
        }
        #endregion

        #region Xem đơn vị
        public IActionResult DisplayDonVi()
        {
            List<DonVi> users = new List<DonVi>();


            RepoAdmin quanlyUser = new RepoAdmin();

            users = quanlyUser.GetDonVi();




            return View(users);
        }
        #endregion

        #region Xem THDV
        public IActionResult IndexTHDV(DateTime? ngay)
        {
            List<THDV> quanSos = new List<THDV>();

            try
            {
                RepoAdmin admin = new RepoAdmin(); // Tạo mới một đối tượng QuanLyQuanSo

                if (ngay.HasValue)
                {
                    quanSos = admin.GetTHDV(ngay.Value);
                }
                else
                {
                    // Sử dụng DateTime.MinValue để đại diện cho giá trị null
                    quanSos = admin.GetTHDV(DateTime.Now);
                }
            }
            catch
            {
                // Xử lý nếu có lỗi xảy ra khi tạo đối tượng QuanLyQuanSo hoặc khi gọi phương thức SearchQuanSoByDate hoặc GetBaoCaoQuanSo
            }

            return View(quanSos);
        }
        #endregion

        #region Xem lịch sử đăng nhập
        public IActionResult History(DateTime? ngay, string? name)
        {
            List<LoginHistory> users = new List<LoginHistory>();
            try
            {
                RepoAdmin quanlyUser = new RepoAdmin();
                if (ngay.HasValue)
                {
                    users = quanlyUser.SearchHisoryDate(ngay.Value.Date);
                }
                else if (name != null)
                {
                    users = quanlyUser.SearchHisoryName(name);
                }
                else
                {
                    users = quanlyUser.SearchHisoryDate(DateTime.Now.Date);
                }
            }
            catch { }


            return View(users);
        }
        #endregion

        #region Xem lịch sử hành động của người dùng
        public IActionResult HistoryAction(DateTime? ngay, string? id_dv)
        {
            List<Combinecs> users = new List<Combinecs>();
            try
            {

                RepoAdmin quanlyUser = new RepoAdmin();
                if (ngay.HasValue)
                {
                    users = quanlyUser.SearchHisoryActDate(ngay.Value);
                }
                else if (id_dv != null)
                {
                    users = quanlyUser.SearchHisoryActID(id_dv);
                }
                else
                {
                    users = quanlyUser.SearchHisoryActDate(DateTime.Now);
                }
            }
            catch { }

            return View(users);
        }
        #endregion

        #region Tìm Kiếm
        public IActionResult Search(DateTime ngay)
        {
            return RedirectToAction("Index", new { ngay });
        }
        #endregion


        #region Sửa thông tin quân số
        public ActionResult Edit(string id)
        {
            QuanSo users = new QuanSo();

            RepoAdmin quanlyUser = new RepoAdmin();

            users = quanlyUser.GetBaoCaoQuanSoById(id);
            return View(users);
        }
        [HttpPost]

        public IActionResult EditNew([FromBody] Dictionary<string, string> updatedValues)
        {
            try
            {
                Console.WriteLine("Dữ liệu nhận được: ");
                foreach (var key in updatedValues.Keys)
                {
                    Console.WriteLine($"{key}: {updatedValues[key]}");
                }

                if (updatedValues.ContainsKey("IdDv"))
                {
                    string id = updatedValues["IdDv"];
                    //string ngay = updatedValues["Ngay"];

                    int tongqs = int.Parse(updatedValues["TongQs"]);
                    int qsVang = int.Parse(updatedValues["QsVang"]);
                    int idbcqs = int.Parse(updatedValues["IdBcqs"]);
                    //DateTime ngay = DateTime.Parse(updatedValues["Ngay"]);
                    QuanSo qs = new QuanSo
                    {
                        IdDv = id,
                        Ngay = ParseNullableDateTime(updatedValues, "Ngay"),
                        TongQs = tongqs,
                        QsVang = qsVang,
                        DaoNgu = ParseNullableInt(updatedValues, "DaoNgu"),
                        DiVien = ParseNullableInt(updatedValues, "DiVien"),
                        BenhXa = ParseNullableInt(updatedValues, "BenhXa"),
                        DiHoc = ParseNullableInt(updatedValues, "DiHoc"),
                        DiThucTe = ParseNullableInt(updatedValues, "DiThucTe"),
                        DiThucTap = ParseNullableInt(updatedValues, "DiThucTap"),
                        DiTt = ParseNullableInt(updatedValues, "DiTt"),
                        DiCtac = ParseNullableInt(updatedValues, "DiCtac"),
                        ThaiSan = ParseNullableInt(updatedValues, "ThaiSan"),
                        LyDoKhac = ParseNullableInt(updatedValues, "LyDoKhac"),
                        ChuThich = updatedValues.ContainsKey("ChuThich") && !string.IsNullOrEmpty(updatedValues["ChuThich"]) ? updatedValues["ChuThich"] : null,
                        IdBcqs = idbcqs
                    };


                    bool isValidQsVang = qs.QsVang == qs.DaoNgu + qs.DiVien + qs.BenhXa +
                             qs.DiHoc + qs.DiThucTe + qs.DiThucTap +
                             qs.DiTt + qs.DiCtac + qs.ThaiSan +
                             qs.LyDoKhac;

                    if (ModelState.IsValid && isValidQsVang)
                    {
                        RepoAdmin _DbQs = new RepoAdmin();
                        if (_DbQs.EditQuanSo(qs))
                        {
                            return Json(new { success = true, message = "Cập nhật thành công", redirectUrl = Url.Action("Index") });
                        }
                        else
                        {
                            return Json(new { success = false, message = "Cập nhật không thành công" });
                        }
                    }
                    else
                    {
                        if (!isValidQsVang)
                        {
                            return Json(new { success = false, message = "Giá trị của quân số vắng phải bằng tổng các  trường khác.", redirectUrl = Url.Action("Index") });
                          
                        }
                        else
                        {
                            return Json(new { success = false, message = "Dữ liệu không hợp lệ." });
                        }
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Thiếu khóa chính 'IdDv'." });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        private int? ParseNullableInt(Dictionary<string, string> values, string key)
        {
            if (values.ContainsKey(key) && int.TryParse(values[key], out int result))
            {
                return result;
            }
            return null;
        }


        private DateTime? ParseNullableDateTime(Dictionary<string, string> values, string key)
        {
            if (values.ContainsKey(key) && DateTime.TryParse(values[key], out DateTime result))
            {
                return result;
            }
            return null;
        }


        #endregion

        #region Sửa thông tin Tình hình đơn vị
        public ActionResult EditTHDV(string id)
        {
            THDV users = new THDV();

            RepoAdmin quanlyUser = new RepoAdmin();

            users = quanlyUser.GetTHDVById(id);
            return View(users);
        }
        public IActionResult EditNewTHDV([FromBody] Dictionary<string, string> updatedValues)
        {
            try
            {

                if (updatedValues.ContainsKey("IdDv"))
                {
                    int id = int.Parse(updatedValues["Id"]);
                    string iddv = updatedValues["IdDv"];
                    string tentb = updatedValues["TenTB"];
                    string nvvs = updatedValues["Nvvs"];
                    string canhgac = updatedValues["CanhGac"];
                    string ghichu = updatedValues["GhiChu"];


                    
                    //DateTime ngay = DateTime.Parse(updatedValues["Ngay"]);
                    THDV qs = new THDV
                    {
                        Id = id,
                        Ngay = ParseNullableDateTime(updatedValues, "Ngay"),
                        IdDv = iddv,
                        TenTB = tentb,
                        Nvvs = nvvs,
                        CanhGac = canhgac,
                        GhiChu = ghichu,
                        
                    };


                    // Kiểm tra xem đã tồn tại một bản ghi cho ngày này chưa
                    RepoAdmin _DbQs = new RepoAdmin();


                    if (ModelState.IsValid)
                    {
                        if (_DbQs.EditTHDV(qs))
                        {
                            TempData["success"] = "sửa thành công!";
                            return Json(new { success = true, message = "Cập nhật thành công" });
                        }
                    }
                    else
                    {


                        return View("EditTHDV");
                    }
                }
            }
            catch
            {
                return View("EditTHDV");
            }

            return View("EditTHDV"); // T
        } 
    
        #endregion

        #region Thêm đơn vị
        public ActionResult AddDonVi()
        {
            return View();
        }

        public ActionResult AddDonViNew(DonVi dv)
        {
            try
            {


                RepoAdmin _DbQs = new RepoAdmin();

                if (ModelState.IsValid)
                {
                    // Thêm thông tin hành động vào cơ sở dữ liệu
                    if (_DbQs.addDonVi(dv))
                    {




                        TempData["success"] = "thêm thành công!";
                        return RedirectToAction("AddDonVi");

                    }
                }
                else
                {


                    return View("AddDonVi");
                }
            }
            catch
            {
                return View("AddDonVi");
            }

            return View("AddDonVi"); // Thêm này nếu có lỗi xảy ra và không thể xử lý được
        }
        #endregion

        #region Xuất pdf
        [HttpPost]
        public IActionResult ExportToPDF(DateTime? ngay)
        {
            DateTime dateToExport = ngay ?? DateTime.Now; // Sử dụng ngày hiện tại nếu không có ngày được chọn từ người dùng

            RepoAdmin repoAdmin = new RepoAdmin();
            DataTable dataTable = repoAdmin.GetBaoCaoQuanSoData(dateToExport);
            List<string> columnOrder = new List<string>
    {
        "Đơn vị",
        "Ngày",
        "Tổng quân số",
        "Quân số vắng",
        "Đảo ngũ",
        "Đi viện",
        "Bệnh xá",
        "Đi học",
        "Đi thực tế",
        "Đi thực tập",
        "Tranh thủ",
        "Đi công tác",
        "Thai sản",
        "Lý do khác",
        "Chú thích"
       /* "Đơn vị ",
        "Ngày",
        "Tổng quân số",
        "Quân số vắng",
        "Đào ngũ",
        "Đi viện",
        "Bệnh xá",
        "Đi học",
        "Đi thực tế",
        "Đi thực tập",
        "Tranh thủ",
        "Đi công tác",
        "Thai sản",
        "Lý do khác",
        "Chú thích"*/
    };

            
            Document document = new Document(PageSize.A4.Rotate());
            MemoryStream memoryStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
            document.SetMargins(20, 20, 20, 20);
           

            document.Open();

            

            PdfPTable table = new PdfPTable(columnOrder.Count);
            float[] columnWidths = new float[columnOrder.Count];

            // Thiết lập kích thước cho từng cột
            for (int i = 0; i < columnOrder.Count; i++)
            {
                // Đặt kích thước mặc định cho các cột
                columnWidths[i] = 200f; // Đổi giá trị này tùy thuộc vào kích thước mong muốn của từng cột
                BaseFont baseFont = BaseFont.CreateFont("E:\\BTL\\qlqs\\Dotnet6MvcLogin\\wwwroot\\arial.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                Font font = new Font(baseFont, 12, Font.NORMAL);
                PdfPCell cell = new PdfPCell(new Phrase(columnOrder[i], font));


                /*PdfPCell cell = new PdfPCell(new Phrase(columnOrder[i]));*/
                //PdfPCell cell = new PdfPCell(new Phrase(columnOrder[i], FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, BaseColor.BLACK)));
                cell.Rowspan = 3; // Cho phép nội dung của ô xuống dòng ở dòng tiếp theo
                table.AddCell(cell);
            }


            // Thiết lập kích thước cho từng cột
            table.SetWidths(columnWidths);

            // Thêm dữ liệu từ DataTable vào bảng PDF
            foreach (DataRow row in dataTable.Rows)
            {
                for (int i = 1; i < dataTable.Columns.Count; i++)
                {
                    object value = row[i]; // Lấy giá trị từ cột thứ i
                    PdfPCell cell = new PdfPCell(new Phrase(value != null ? value.ToString() : ""));

                    // Kiểm tra nếu giá trị là ngày thì định dạng lại
                    if (value is DateTime)
                    {
                        cell.Phrase = new Phrase(((DateTime)value).ToString("dd/MM/yyyy"));
                    }

                    table.AddCell(cell);
                }
            }

            document.Add(table);
            document.Close();

            byte[] fileContents = memoryStream.ToArray();

            return File(fileContents, "application/pdf", $"QuanSo_{dateToExport.ToString("yyyyMMdd")}.pdf");


        }
        #endregion


    }
}

