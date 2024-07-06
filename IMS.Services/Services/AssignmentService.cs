using AutoMapper;
using IMS.Repositories.Entities;
using IMS.Repositories.Interfaces;
using IMS.Repositories.Models.AssignmentModels;
using IMS.Repositories.QueryModels;
using IMS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Services.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AssignmentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> Create(AssignmentCreateModel assignmentCreateModel)
        {
            var assignment = _mapper.Map<Assignment>(assignmentCreateModel);
            assignment.CreationDate = DateTime.UtcNow;
            assignment.EndDate = assignment.StartDate.Value.AddDays(assignmentCreateModel.Duration);
            _unitOfWork.AssignmentRepository.AddAsync(assignment);
            if (await _unitOfWork.SaveChangeAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<QueryResultModel<List<Assignment>>> GetAssignments(AssignmentFilterModel assignmentFilterModel)
        {
            var assignmentList = await _unitOfWork.AssignmentRepository.GetAllAsync(
                filter: x =>
                    (string.IsNullOrEmpty(assignmentFilterModel.Search)
                    || x.Name.ToLower().Contains(assignmentFilterModel.Search.ToLower())
                    || x.Material.ToLower().Contains(assignmentFilterModel.Search.ToLower())
                    || x.Comment.ToLower().Contains(assignmentFilterModel.Search.ToLower())
                    || x.Intern.FullName.ToLower().Contains(assignmentFilterModel.Search.ToLower())
                    || x.Description.ToLower().Contains(assignmentFilterModel.Search.ToLower())),
                orderBy: x => assignmentFilterModel.OrderByDescending
                    ? x.OrderByDescending(x => x.CreationDate)
                    : x.OrderBy(x => x.CreationDate),
                pageIndex: assignmentFilterModel.PageNumber,
                pageSize: assignmentFilterModel.PageSize,
                includeProperties: "Intern"
            );

            return assignmentList;
        }
    }
}
