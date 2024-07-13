using AutoMapper;
using IMS.Repositories.Entities;
using IMS.Repositories.Interfaces;
using IMS.Repositories.Models.AccountModel;
using IMS.Repositories.Models.InterviewModel;
using IMS.Repositories.Models.NewFolder;
using IMS.Repositories.Models.TrainingProgramModel;
using IMS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace IMS.Services.Services
{
    public class InterviewService : IInterviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public InterviewService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> Create(InterviewCreateModel interviewCreateModel)
        {
            Interview interview = _mapper.Map<Interview>(interviewCreateModel);
            interview.IsDeleted = false;
            interview.Status = Repositories.Enums.InterviewStatus.Scheduled;
            interview.InternId = interviewCreateModel.InternId;
            _unitOfWork.InterviewRepository.AddAsync(interview);
            if (await _unitOfWork.SaveChangeAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<List<InterviewGetModel>> GetAllInterview(InterviewFilterModel filterModel)
        {
            var interviewList = await _unitOfWork.InterviewRepository.GetAllAsync(
                filter: x =>
                    (x.IsDeleted == false) &&
                    (string.IsNullOrEmpty(filterModel.Search) || x.InternName.ToLower().Contains(filterModel.Search.ToLower()) ||
                    x.InternEmail.ToLower().Contains(filterModel.Search.ToLower())),
                orderBy: x => filterModel.OrderByAscending
                ? x.OrderBy(x => x.Date)
                : x.OrderByDescending(x => x.Date),
                pageIndex: filterModel.PageNumber,
                pageSize: filterModel.PageSize
            );

            List<InterviewGetModel> interviewDetailList = null;

            if (interviewList != null)
            {
                interviewDetailList = interviewList.Data.Select(x => new InterviewGetModel
                {
                    Id = x.Id,
                    InternName = x.InternName,
                    InternMail = x.InternEmail,
                    Date = x.Date,
                    Time = x.Time,
                    Location = x.Location,
                    Status = x.Status
                }).ToList();
            }

            return interviewDetailList ?? new List<InterviewGetModel>();
        }

        public async Task<int> GetTotalInterviewsCount(InterviewFilterModel filterModel)
        {
            IQueryable<Interview> query = _unitOfWork.InterviewRepository.GetAll().AsQueryable();


            if (!string.IsNullOrEmpty(filterModel.Search))
            {
                var search = filterModel.Search.ToLower();
                query = query.Where(a =>
                    a.InternName.ToLower().Contains(search) ||
                    a.InternEmail.ToLower().Contains(search)
                );
            }
            return await query.CountAsync();
        }

        public async Task<Interview> Get(Guid id)
        {
            return await _unitOfWork.InterviewRepository.GetAsync(id);
        }

        public async Task<bool> Update(Interview interview)
        {

            _unitOfWork.InterviewRepository.Update(interview);
            if (await _unitOfWork.SaveChangeAsync() > 0)
            {
                return true;
            }
            return false;
        }
    }
}
