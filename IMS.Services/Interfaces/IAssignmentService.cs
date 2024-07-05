using IMS.Repositories.Models.AssignmentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Services.Interfaces
{
    public interface IAssignmentService
    {
        Task<bool> Create(AssignmentCreateModel assignmentCreateModel);
    }
}
