using AutoMapper;
using IMS.Models.Entities;
using IMS.Models.Interfaces;
using Model.Enums;
using Model.ViewModels.TraineeModel;
using Model.ViewModels.TrainingProgramModel;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
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

        public async Task<TrainingProgram> GetByCode(string code)
        {
            return await _unitOfWork.TrainingProgramRepository.GetByCode(code);
        }

        public async Task<bool> Create(TrainingProgramCreateModel trainingProgramCreateModel)
        {
            TrainingProgram trainingProgram = _mapper.Map<TrainingProgram>(trainingProgramCreateModel);
            trainingProgram.IsDeleted = false;
            trainingProgram.Status = TrainingPrgramStatus.Processing.ToString();
            _unitOfWork.TrainingProgramRepository.AddAsync(trainingProgram);
            if (await _unitOfWork.SaveChangeAsync() > 0)
            {
                return true;
            }
            return false;
        }
    }
}
