using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Services.Interfaces
{
    public interface IMentorshipService
    {

        Task<bool> LinkMentorforIntern(Guid internId, Guid MentorId, Guid CreatedBy);
    }
}
