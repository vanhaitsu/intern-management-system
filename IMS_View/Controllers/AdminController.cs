using AutoMapper;
using IMS.Models.Interfaces;
using IMS_View.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.ViewModels.AccountModel;
using System.Security.Claims;

namespace IMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IAccountService _accountService;
        public AdminController(ILogger<AdminController> logger, IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        //[AuthorFilter]
        public IActionResult Index()
        {
            //Guid? userId = _claimsService.GetCurrentUserId();
            return View();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAccount(Guid id, AccountUpdateModel accountUpdateModel)
        {
            if (ModelState.IsValid)
            {
                if (await _accountService.Update(id, accountUpdateModel))
                {
                    ViewBag.Message = "Update successfully!";
                    return View(accountUpdateModel);
                }
                else
                {
                    ViewBag.Message = "Failed to update!";
                    return View(accountUpdateModel);
                }
            }
            else
            {
                ViewBag.Message = "Please validate the inputed value!";
                return View(accountUpdateModel);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAccount(Guid Id)
        {
            if (await _accountService.Delete(Id))
            {
                ViewBag.Message = "Delete successfully!";
            }
            else
            {
                ViewBag.Message = "Failed to delete!";
            }

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAccounts()
        {
            var accounts = await _accountService.GetAll();

            if (accounts != null)
            {
                if (accounts.Any())
                {
                    return View(accounts);
                }
                else
                {
                    ViewBag.Message = "No Account found.";
                    return View();
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Failed to retrieve accounts.";
                return View("Error");
            }
        }

    }
}
