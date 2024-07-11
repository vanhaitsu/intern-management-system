using AutoMapper;
using IMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace IMS.RazorPage.Pages.Common
{
    public class Verify
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string VerifyCode { get; set; }
    }
    public class VerifyMailModel : PageModel
    {
        private readonly IInternService _internService;
        private readonly IMapper _mapper;
        [BindProperty]
        public Verify Verify { get; set; }
        public VerifyMailModel(IInternService internService, IMapper mapper)
        {
            _internService = internService;
            _mapper = mapper;
        }

        //public async Task<IActionResult> OnGetAsync()
        //{
        //    //ViewData["Email"] = TempData["Email"] as string;
        //    return Page();
        //}

        public void OnGet()
        {
            if (TempData.ContainsKey("Email"))
            {
                Verify = new Verify
                {
                    Email = TempData["Email"].ToString()
                };
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var customer = await _internService.GetByEmail(Verify.Email);
                if (customer == null)
                {
                    TempData["ErrorMessage"] = "Intern not found";
                    return Page();
                }

                if (!customer.EmailVerifyCode.ToString().Equals(Verify.VerifyCode))
                {
                    TempData["ErrorMessage"] = "Verify code is wrong!";
                    return Page();
                }
                //Repositories.Entities.Intern intern = _mapper.Map<Intern>(customer);
                customer.Status = 1;
                //customer.EmailVerifyCode = null;
                //customer.ExpiredCode = null;
                _internService.Edit(customer);
                TempData["SuccessMessage"] = "Successfully, login now.";
            }
            return Page();
        }
    }
}

