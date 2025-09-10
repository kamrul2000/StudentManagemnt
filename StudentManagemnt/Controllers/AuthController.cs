using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Services;
using StudentManagemnt.Interface;
using StudentManagemnt.Models;

namespace StudentManagement.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(string username, string email, string password)
        {
            var user = new User { Username = username, Email = email };
            await _authService.Register(user, password);
            TempData["Message"] = "Registration successful! Please login.";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var token = await _authService.Login(username, password);
            if (token == null)
            {
                ViewBag.Error = "Invalid username or password";
                return View();
            }

            // For demo: store token in TempData (or cookie in real apps)
            TempData["Token"] = token;
            return RedirectToAction("Index", "Student");
        }
    }
}
