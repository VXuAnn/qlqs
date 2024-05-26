using Dotnet6MvcLogin.Models.DTO;
using Dotnet6MvcLogin.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MvcLogin.Models;
using MvcLogin.Repositories;
using MvcLogin.Models;
using Microsoft.EntityFrameworkCore;

namespace Dotnet6MvcLogin.Controllers
{
    public class UserAuthenticationController : Controller
    {

        private readonly IUserAuthenticationService _authService;
        public UserAuthenticationController(IUserAuthenticationService authService)
        {
            this._authService = authService;
        }


        public IActionResult Login()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _authService.LoginAsync(model);

            if (result.StatusCode == 1)
            {
                // Tạo một thể hiện của RepoAdmin
                var repoAdmin = new RepoAdmin();
                var users = new Users();
                var history = new LoginHistory();
                
                    var historyRecord = new LoginHistory
                    {

                        FirstName = model.Username,

                        LoginTime = DateTime.Now,
                       
                        
                    };
                   
                

                // Lưu bản ghi lịch sử vào cơ sở dữ liệu
               
                

                // Gọi phương thức SaveLoginHistoryToDatabase từ thể hiện của RepoAdmin
                bool saveResult = repoAdmin.SaveLoginHistoryToDatabase(historyRecord);

                if (saveResult)
                {
                    if (User.IsInRole("admin"))
                    {
                        return RedirectToAction("Index", "Admin");
                        /* return RedirectToAction("Index", "TrangChu");*/
                    }
                    else if (!string.IsNullOrEmpty(User.Identity.Name))
                    {
                        /* return RedirectToAction("AddQuanSo", "User");*/
                        return RedirectToAction("Index", "TrangChu");
                    }
                    else
                    {
                        return RedirectToAction(nameof(Login));
                    }
                }
                
            }
            else
            {
                TempData["msg"] = result.Message;
                return RedirectToAction(nameof(Login));
            }
            return RedirectToAction(nameof(Login));
        }

        // Thêm một hành động mặc định trong trường hợp không có trường hợp nào phù hợp.
        public IActionResult DefaultAction()
        {
            // Có thể chuyển hướng đến một trang lỗi hoặc trang nào đó khác tùy thuộc vào yêu cầu của ứng dụng.
            return RedirectToAction("Index", "Home");
        }


        

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationModel model)
        {
            if (!ModelState.IsValid) { return View(model); }


            model.Role = "user";
            var result = await this._authService.RegisterAsync(model);
            TempData["msg"] = result.Message;
            return RedirectToAction(nameof(Registration));
        }



        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await this._authService.LogoutAsync();  
            return RedirectToAction(nameof(Login));
        }
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAdmin()
        {
            RegistrationModel model = new RegistrationModel
            {
                Username = "admin",
                Password = "Admin@12345#"
            };
            model.Role = "admin";
            var result = await this._authService.RegisterAsync(model);
            return Ok(result);
        }

        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult>ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
              return View(model);
            var result = await _authService.ChangePasswordAsync(model, User.Identity.Name);
            TempData["msg"] = result.Message;
            return RedirectToAction(nameof(ChangePassword));
        }

    }
}
