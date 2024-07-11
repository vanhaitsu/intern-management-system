using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Repositories.Models.MentorshipModel
{
    public class MentorshipCreateModel
    {
      //public Guid Id { get; set; } = new Guid();
       public Guid InternId { get; set; }
       public Guid AcountId { get; set; }
       public DateTime CreatedDate { get; set; } = DateTime.Now;
        //public bool isDeleted { get; set; } = false;
    }
}
