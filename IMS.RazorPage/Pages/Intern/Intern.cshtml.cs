using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IMS.RazorPage.Pages.Intern
{
    [Authorize]
    public class InternModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
