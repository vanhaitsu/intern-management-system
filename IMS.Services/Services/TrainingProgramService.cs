using AutoMapper;
using IMS.Repositories.Entities;
using IMS.Repositories.Interfaces;
using IMS.Repositories.Models.AccountModel;
using IMS.Repositories.Models.TrainingProgramModel;
using IMS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Services.Services
{
    public class TrainingProgramService : ITrainingProgramService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TrainingProgramService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<TrainingProgram>> GetByAccount(Guid accountId)
        {
            return await _unitOfWork.TrainingProgramRepository.GetByAccount(accountId);
        }

        public async Task<TrainingProgram> Get (Guid id)
        {
            return await _unitOfWork.TrainingProgramRepository.GetAsync(id);
        }

        public async Task<bool> SoftDelete(TrainingProgram trainingProgram)
        {
            _unitOfWork.TrainingProgramRepository.SoftDelete(trainingProgram);
            if (await _unitOfWork.SaveChangeAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> Update(TrainingProgramUpdateModel trainingProgramUpdateModel)
        {
            TrainingProgram trainingProgram = await _unitOfWork.TrainingProgramRepository
                .GetAsync((Guid)trainingProgramUpdateModel.Id);
            if (trainingProgramUpdateModel.StartDate != trainingProgram.StartDate || 
                trainingProgramUpdateModel.Duration != trainingProgram.Duration)
            {
                trainingProgramUpdateModel.EndDate = trainingProgramUpdateModel.StartDate.Value
                .AddDays(trainingProgramUpdateModel.Duration.Value);
            }
            else
            {
                trainingProgramUpdateModel.EndDate = trainingProgram.EndDate;
            }
            _mapper.Map(trainingProgramUpdateModel, trainingProgram);
            _unitOfWork.TrainingProgramRepository.Update(trainingProgram);
            if (await _unitOfWork.SaveChangeAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> Create(TrainingProgramCreateModel trainingProgramCreateModel)
        {
            TrainingProgram trainingProgram = _mapper.Map<TrainingProgram>(trainingProgramCreateModel);
            trainingProgram.IsDeleted = false;
            if (trainingProgramCreateModel.StartDate != null && trainingProgramCreateModel.Duration != null)
            {
                trainingProgram.EndDate = trainingProgramCreateModel.StartDate.Value
                .AddDays(trainingProgramCreateModel.Duration.Value);
            }
            _unitOfWork.TrainingProgramRepository.AddAsync(trainingProgram);
            if (await _unitOfWork.SaveChangeAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<List<TrainingProgramGetModel>> GetAllTrainingPrograms(AccountFilterModel filterModel, Guid accountId)
        {
            var trainingProgramList = await _unitOfWork.TrainingProgramRepository.GetAllAsync(
                filter: x =>
                    (x.CreatedBy == accountId) &&
                    (x.IsDeleted == false) &&
                    (string.IsNullOrEmpty(filterModel.Search) || x.Name.ToLower().Contains(filterModel.Search.ToLower())),
                orderBy: x => filterModel.OrderByDescending
                    ? x.OrderByDescending(x => x.CreationDate)
                    : x.OrderBy(x => x.CreationDate),
                pageIndex: filterModel.PageNumber,
                pageSize: filterModel.PageSize
            );

            List<TrainingProgramGetModel> trainingProgramDetailList = null;

            if (trainingProgramList != null)
            {
                trainingProgramDetailList = trainingProgramList.Data.Select(x => new TrainingProgramGetModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Duration = x.Duration,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate
                }).ToList();
            }

            return trainingProgramDetailList ?? new List<TrainingProgramGetModel>();
        }


        public async Task<int> GetTotalTrainingProgramsCount(AccountFilterModel filterModel, Guid accountId)
        {
            IQueryable<TrainingProgram> query = _unitOfWork.TrainingProgramRepository.GetAll().AsQueryable();


            if (!string.IsNullOrEmpty(filterModel.Search))
            {
                var search = filterModel.Search.ToLower();
                query = query.Where(a =>
                    a.Name.ToLower().Contains(search)
                );
            }
            if (!string.IsNullOrEmpty(filterModel.Role))
            {
                query = query.Where(a => a.CreatedBy == accountId);
            }

            return await query.CountAsync();
        }
    }
}
