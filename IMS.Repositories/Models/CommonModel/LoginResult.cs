using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Repositories.Models.CommonModel
{
    public class LoginResult
    {
        public LoginModel LoginModel { get; set; }
        public string ErrorMessage { get; set; }
    }
}
