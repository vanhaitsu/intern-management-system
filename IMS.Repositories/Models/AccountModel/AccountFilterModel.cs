using IMS.Repositories.Entities;
using IMS.Repositories.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Repositories.Models.AccountModel
{
    public class AccountFilterModel
    {
        public string Order { get; set; } = "creationDate";
        public bool OrderByDescending { get; set; } = true;
        public string? Role { get; set; }
        public string? Search { get; set; }
        public  int PageSize { get; set; } =  10;
        public int PageNumber { get; set; } = 1;
    }
}
