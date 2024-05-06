﻿using Dotnet6MvcLogin.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcLogin.Models;
using MvcLogin.Repositories;
using OfficeOpenXml;
using System.Data;
using System.IO;
using System;

namespace Dotnet6MvcLogin.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
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
            System.Console.WriteLine(start);
            System.Console.WriteLine(end);

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
        [HttpPost]
        public IActionResult ExportToExcel(DateTime? ngay)
        {
            DateTime dateToExport = ngay ?? DateTime.Now; // Sử dụng ngày hiện tại nếu không có ngày được chọn từ người dùng

            RepoAdmin repoAdmin = new RepoAdmin();
            DataTable dataTable = repoAdmin.GetBaoCaoQuanSoData(dateToExport);

            // Danh sách các tên cột bạn muốn hiển thị
            string[] columnNames = {
        "Đơn vị ",
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
        "Chú thích"
    };

            byte[] fileContents;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                // Thêm các tên cột vào file Excel
                for (int i = 1; i < columnNames.Length+1; i++) // Bắt đầu từ i = 1
                {
                    worksheet.Cells[1, i].Value = columnNames[i - 1];
                }

                // Thêm dữ liệu từ DataTable vào bảng Excel
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    for (int j = 1; j < dataTable.Columns.Count; j++) // Bắt đầu từ j = 1
                    {
                        var value = dataTable.Rows[i][j];
                        if (dataTable.Columns[j].DataType == typeof(DateTime))
                        {
                            // Nếu ô chứa ngày, định dạng lại định dạng ngày
                            worksheet.Cells[i + 2, j].Value = value;
                            worksheet.Cells[i + 2, j].Style.Numberformat.Format = "dd/MM/yyyy";
                        }
                        else
                        {
                            worksheet.Cells[i + 2, j].Value = value;
                        }
                    }
                }

                fileContents = package.GetAsByteArray();
            }

            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"QuanSo_{dateToExport.ToString("yyyyMMdd")}.xlsx");
        }


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

        public IActionResult Search(DateTime ngay)
        {
            return RedirectToAction("Index", new { ngay });
        }

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

        public ActionResult Edit(string id)
        {
            QuanSo users = new QuanSo();

            RepoAdmin quanlyUser = new RepoAdmin();

            users = quanlyUser.GetBaoCaoQuanSoById(id);
            return View(users);
        }
        public IActionResult EditNew(QuanSo qs)
        {
            try
            {
                // Kiểm tra xem giá trị của qs_vang có phù hợp không
                bool isValidQsVang = qs.QsVang == qs.DaoNgu || qs.QsVang == qs.DiVien || qs.QsVang == qs.BenhXa ||
                                      qs.QsVang == qs.DiHoc || qs.QsVang == qs.DiThucTe || qs.QsVang == qs.DiThucTap ||
                                      qs.QsVang == qs.DiTt || qs.QsVang == qs.DiCtac || qs.QsVang == qs.ThaiSan ||
                                      qs.QsVang == qs.LyDoKhac;

                // Kiểm tra xem đã tồn tại một bản ghi cho ngày này chưa
                RepoAdmin _DbQs = new RepoAdmin();


                if (ModelState.IsValid && isValidQsVang)
                {
                    if (_DbQs.EditQuanSo(qs))
                    {
                        TempData["success"] = "sửa thành công!";
                        return View("Edit");
                    }
                }
                else
                {
                    if (!isValidQsVang)
                    {
                        TempData["error"] = "Giá trị của 'qs_vang' phải bằng một trong các trường khác.";
                    }


                    return View("Edit");
                }
            }
            catch
            {
                return View("Edit");
            }

            return View("Edit"); // T
        }

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

        public ActionResult EditTHDV(string id)
        {
            THDV users = new THDV();

            RepoAdmin quanlyUser = new RepoAdmin();

            users = quanlyUser.GetTHDVById(id);
            return View(users);
        }
        public IActionResult EditNewTHDV(THDV qs)
        {
            try
            {

                // Kiểm tra xem đã tồn tại một bản ghi cho ngày này chưa
                RepoAdmin _DbQs = new RepoAdmin();


                if (ModelState.IsValid)
                {
                    if (_DbQs.EditTHDV(qs))
                    {
                        TempData["success"] = "sửa thành công!";
                        return View("EditTHDV");
                    }
                }
                else
                {


                    return View("EditTHDV");
                }
            }
            catch
            {
                return View("EditTHDV");
            }

            return View("EditTHDV"); // T
        }

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
        public IActionResult DisplayDonVi()
        {
            List<DonVi> users = new List<DonVi>();


            RepoAdmin quanlyUser = new RepoAdmin();

            users = quanlyUser.GetDonVi();




            return View(users);
        }
    }
}

