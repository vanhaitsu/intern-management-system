using AutoMapper;
using IMS.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        //[AuthorFilter]
        public IActionResult Index()
        {
            //Guid? userId = _claimsService.GetCurrentUserId();
            return View();
        }
    }
}
