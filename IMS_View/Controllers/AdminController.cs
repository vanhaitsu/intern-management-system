using AutoMapper;
using IMS.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IMS.Controllers
{
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
