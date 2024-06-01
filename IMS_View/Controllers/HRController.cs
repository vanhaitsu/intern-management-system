using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IMS_View.Controllers
{
    [Authorize]
    public class HRController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
