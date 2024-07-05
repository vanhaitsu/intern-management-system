using AutoMapper;
using IMS.Repositories.Entities;
using IMS.Repositories.Interfaces;
using IMS.Repositories.Models.TrainingProgramModel;
using IMS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
