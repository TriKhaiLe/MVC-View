using MVC_View.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace MVC_View.Controllers
{
    public class UsersController : Controller
    {
        private readonly QuanLySanPhamContext _context;

        public UsersController()
        {
            _context = new QuanLySanPhamContext();
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                HttpContext.Session.SetString("Username", user.Username);

                if (user.Username == "admin")
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
                else
                {
                    return RedirectToAction("Index", "Products");
                }
            }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("Username");
            return RedirectToAction("Login", "Users");
        }


        public IActionResult ConfirmLogin()
        {
            string userName = Request.Form["Username"];
            string password = Request.Form["Password"];

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName)
            };

            // Tạo claims identity
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Đăng nhập bằng cookie
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true, // Giữ trạng thái đăng nhập ngay cả khi đóng trình duyệt
                ExpiresUtc = DateTime.UtcNow.AddMinutes(30) // Thời gian hết hạn cookie
            };

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties).Wait();

            return RedirectToAction("Index", "Product");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Handle registration data
        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            if (ModelState.IsValid)
            {
                // Check if user already exists (you might want to add this check in a database)
                var existingUser = _context.Users.FirstOrDefault(u => u.Username == user.Username);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Username", "Username is already taken.");
                    return View(user);
                }

                // Add the new user to your database
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // Optionally, automatically log in the user after registration
                HttpContext.Session.SetString("Username", user.Username);

                return RedirectToAction("Index", "Products");
            }

            // If model state is invalid, return the form with validation errors
            return View(user);
        }

    }
}
