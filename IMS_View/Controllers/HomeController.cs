using IMS.Models;
using IMS.Models.Entities;
using IMS.Models.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using AutoMapper;
using IMS.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using IMS_View.Services.Interfaces;

namespace IMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAccountService _accountService;
        public HomeController(ILogger<HomeController> logger, IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            Account account = await _accountService.CheckLogin(email, password);
            if (account == null)
            {
                ViewBag.Error = "Email or password is wrong.\nPlease check again!";
                return View();
            }
            return RedirectToAction("Index", "Admin");
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(AccountRegisterModel accountRegisterModel)
        {
            if (ModelState.IsValid)
            {
                if (await _accountService.SignUp(accountRegisterModel))
                    ViewBag.Message = "Register successfully!";
                    return View(accountRegisterModel);
                }
            else
            {
                ViewBag.Message = "Please validate the inputed value!";
                return View(accountRegisterModel);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new IMS_View.Models.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
