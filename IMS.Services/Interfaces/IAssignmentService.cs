using IMS.Repositories.Entities;
using IMS.Repositories.Models.AssignmentModels;
using IMS.Repositories.QueryModels;
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
        Task<QueryResultModel<List<Assignment>>> GetAssignments(AssignmentFilterModel assignmentFilterModel);
    }
}
