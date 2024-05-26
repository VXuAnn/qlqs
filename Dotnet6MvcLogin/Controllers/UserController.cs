using Dotnet6MvcLogin.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MvcLogin.Models;
using MvcLogin.Repositories;
using System.Runtime.ConstrainedExecution;
namespace MvcLogin.Controllers
{
    public class UserController : Controller
    {
        private readonly Time _workingHoursService;

        public UserController(Time workingHoursService)
        {
            _workingHoursService = workingHoursService;
        }

        #region xem quân số đơn vị
        public IActionResult Index()
        {
            List<QuanSo> quanSos = new List<QuanSo>();
            RepoUser user = new RepoUser();
            quanSos = user.GetBaoCaoQuanSoByIddv(User.Identity.Name);
            return View(quanSos);
        }
        #endregion
        #region Thêm quân số
        public ActionResult AddQuanSo()
        {
            var startTime = _workingHoursService.startHour;
            var endTime = _workingHoursService.endHour;
            var time = DateTime.Now.TimeOfDay;

            if (time >= startTime && time < endTime)
            {
                return View();

            }
            else
            {
                return RedirectToAction("Ban");
            }
        }
        public ActionResult AddNewQuanSo(QuanSo qs, THDV thdv)
        {
            try

            {
                if (thdv.CanhGac == null || thdv.Nvvs == null)
                {
                    thdv.CanhGac = thdv.CanhGac ?? "Bảo đảm";
                    thdv.Nvvs = thdv.Nvvs ?? "Bảo đảm";

                }



                if (qs.DaoNgu == null || qs.DiVien == null || qs.BenhXa == null ||
    qs.DiHoc == null || qs.DiThucTe == null || qs.DiThucTap == null ||
    qs.DiTt == null || qs.DiCtac == null || qs.ThaiSan == null ||
    qs.LyDoKhac == null)
                {
                    qs.DaoNgu = qs.DaoNgu ?? 0;
                    qs.DiVien = qs.DiVien ?? 0;
                    qs.BenhXa = qs.BenhXa ?? 0;
                    qs.DiHoc = qs.DiHoc ?? 0;
                    qs.DiThucTe = qs.DiThucTe ?? 0;
                    qs.DiThucTap = qs.DiThucTap ?? 0;
                    qs.DiTt = qs.DiTt ?? 0;
                    qs.DiCtac = qs.DiCtac ?? 0;
                    qs.ThaiSan = qs.ThaiSan ?? 0;
                    qs.LyDoKhac = qs.LyDoKhac ?? 0;
                }
                // Kiểm tra xem giá trị của qs_vang có phù hợp không
                bool isValidQsVang = qs.QsVang == qs.DaoNgu + qs.DiVien + qs.BenhXa +
                             qs.DiHoc + qs.DiThucTe + qs.DiThucTap +
                             qs.DiTt + qs.DiCtac + qs.ThaiSan +
                             qs.LyDoKhac;
                bool CheckTB = !string.IsNullOrEmpty(thdv.TenTB);
                // Kiểm tra xem đã tồn tại một bản ghi cho ngày này chưa
                RepoUser _DbQs = new RepoUser();
                bool isExistingRecordForToday = _DbQs.isExistingRecord(DateTime.Today, User.Identity.Name);

                if (ModelState.IsValid && isValidQsVang && !isExistingRecordForToday)
                {
                    // Thêm thông tin hành động vào cơ sở dữ liệu
                    if (_DbQs.AddQuanSo(qs, thdv))
                    {
                        // Tạo đối tượng Combinecs để lưu vào lịch sử hành động
                        Combinecs combine = new Combinecs
                        {
                            Uname = qs.IdDv,
                            LastActionTime = DateTime.Now,
                            LastAction = "Add"
                        };

                        // Gọi hàm để thêm vào lịch sử hành động
                        if (_DbQs.addCombinces(combine))
                        {
                            TempData["success"] = "Nhập thành công!";
                            return RedirectToAction("AddQuanSo");
                        }
                    }
                }
                else
                {
                    if (!isValidQsVang)
                    {
                        TempData["error"] = "Giá trị của 'qs_vang' phải bằng một trong các trường khác.";
                    }
                    else if (!CheckTB)
                    {
                        TempData["errorTB"] = "Nhập tên trực ban";
                    }
                    else if (isExistingRecordForToday)
                    {
                        TempData["error"] = "Chỉ được phép nhập một lần trong ngày. Nếu có sai xót hãy sửa bên xem lại";
                    }

                    return View("AddQuanSo");
                }
            }
            catch
            {
                return View("AddQuanSo");
            }

            return View("AddQuanSo"); // Thêm này nếu có lỗi xảy ra và không thể xử lý được
        }

        #endregion

        #region Sửa thông tin nhập
        public ActionResult Return(string id)
        {
            Combinecs users = new Combinecs();

            RepoUser quanlyUser = new RepoUser();
            id = User.Identity.Name;
            users = quanlyUser.GetBaoCaoQuanSoById(id);
            return View(users);
        }
        public IActionResult ReturnNew(QuanSo qs, THDV thdv)
        {
            try
            {
                if (qs.DaoNgu == null || qs.DiVien == null || qs.BenhXa == null ||
                     qs.DiHoc == null || qs.DiThucTe == null || qs.DiThucTap == null ||
                     qs.DiTt == null || qs.DiCtac == null || qs.ThaiSan == null ||
                     qs.LyDoKhac == null)
                {
                    qs.DaoNgu = qs.DaoNgu ?? 0;
                    qs.DiVien = qs.DiVien ?? 0;
                    qs.BenhXa = qs.BenhXa ?? 0;
                    qs.DiHoc = qs.DiHoc ?? 0;
                    qs.DiThucTe = qs.DiThucTe ?? 0;
                    qs.DiThucTap = qs.DiThucTap ?? 0;
                    qs.DiTt = qs.DiTt ?? 0;
                    qs.DiCtac = qs.DiCtac ?? 0;
                    qs.ThaiSan = qs.ThaiSan ?? 0;
                    qs.LyDoKhac = qs.LyDoKhac ?? 0;
                }
                /// Kiểm tra xem giá trị của qs_vang có phù hợp không
                bool isValidQsVang = qs.QsVang == qs.DaoNgu + qs.DiVien + qs.BenhXa +
                             qs.DiHoc + qs.DiThucTe + qs.DiThucTap +
                             qs.DiTt + qs.DiCtac + qs.ThaiSan +
                             qs.LyDoKhac;
                bool CheckTB = !string.IsNullOrEmpty(thdv.TenTB);
                // Kiểm tra xem đã tồn tại một bản ghi cho ngày này chưa
                RepoUser _DbQs = new RepoUser();

                if (ModelState.IsValid && isValidQsVang)
                {
                    // Thêm thông tin hành động vào cơ sở dữ liệu
                    if (_DbQs.EditQuanSo(qs, thdv))
                    {
                        // Tạo đối tượng Combinecs để lưu vào lịch sử hành động
                        Combinecs combine = new Combinecs
                        {
                            Uname = qs.IdDv,
                            LastActionTime = DateTime.Now,
                            LastAction = "Edit"
                        };

                        // Gọi hàm để thêm vào lịch sử hành động
                        if (_DbQs.addCombinces(combine))
                        {
                            TempData["success"] = "sửa thành công!";
                            return RedirectToAction("AddQuanSo");
                        }
                    }
                }
                else
                {
                    if (!isValidQsVang)
                    {
                        TempData["error"] = "Giá trị của 'qs_vang' phải bằng một trong các trường khác.";
                    }
                    else if (!CheckTB)
                    {
                        TempData["errorTB"] = "Nhập tên trực ban";
                    }
                    else
                    {
                        // Ghi lại các lỗi ModelState
                        var errors = ModelState.Values.SelectMany(v => v.Errors);
                        foreach (var error in errors)
                        {
                            // Ghi lại thông báo lỗi
                            Console.WriteLine(error.ErrorMessage);
                            if (error.Exception != null)
                            {
                                Console.WriteLine(error.Exception.Message);
                            }
                        }
                    }

                    return View("AddQuanSo");
                }
            }
            catch
            {
                return View("AddQuanSo");
            }

            return View("AddQuanSo"); // Thêm này nếu có lỗi xảy ra và không thể xử lý được
        }
        #endregion

        public ActionResult Ban()
        {
            return View();
        }




    }
}
