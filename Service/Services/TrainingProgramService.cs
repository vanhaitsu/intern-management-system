﻿using AutoMapper;
using IMS.Models.Entities;
using IMS.Models.Interfaces;
using IMS_View.Services.Interfaces;
using Model.Enums;
using Model.ViewModels.TraineeModel;
using Model.ViewModels.TrainingProgramModel;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_VIew.Services.Services
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

        public async Task<TrainingProgram> GetByName(string name)
        public async Task<List<TrainingProgram>> GetByAccount(Guid accountId)
        {
            TrainingProgram program = await _unitOfWork.TrainingProgramRepository.GetByName(name);
            if (program != null)
            {
                return program;
            return await _unitOfWork.TrainingProgramRepository.GetByAccount(accountId);
        }
            return null;
        }

        public async Task<TrainingProgram> GetByCode(string code)
        public async Task<List<TrainingProgram>> GetAllPrograms(int pageSize, int pageNumber)
        {
            if (pageSize == 0)
        {
                pageSize = int.Parse(Pagination.defaultPageSize.ToString());
            return await _unitOfWork.TrainingProgramRepository.GetByCode(code);
        }

        public async Task<bool> Create(TrainingProgramCreateModel trainingProgramCreateModel)
        {
            TrainingProgram trainingProgram = _mapper.Map<TrainingProgram>(trainingProgramCreateModel);
            trainingProgram.IsDeleted = false;
            trainingProgram.Status = TrainingPrgramStatus.Processing.ToString();
            _unitOfWork.TrainingProgramRepository.AddAsync(trainingProgram);
            if (await _unitOfWork.SaveChangeAsync() > 0)
            if (pageNumber == 0)
            {
                pageNumber = int.Parse(Pagination.defaultPageNumber.ToString());
                return true;
            }
            List<TrainingProgram> programs = await _unitOfWork.TrainingProgramRepository.GetAllAsync();
            return programs;
            return false;
        }
    }
}
