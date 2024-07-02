using AutoMapper;
using IMS.Models.Entities;
using IMS.Models.Interfaces;
using IMS_View.Services.Interfaces;
using Model.Enums;
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
        {
            TrainingProgram program = await _unitOfWork.TrainingProgramRepository.GetByName(name);
            if (program != null)
            {
                return program;
            }
            return null;
        }

        public async Task<List<TrainingProgram>> GetAllPrograms(int pageSize, int pageNumber)
        {
            if (pageSize == 0)
            {
                pageSize = int.Parse(Pagination.defaultPageSize.ToString());
            }
            if (pageNumber == 0)
            {
                pageNumber = int.Parse(Pagination.defaultPageNumber.ToString());
            }
            List<TrainingProgram> programs = await _unitOfWork.TrainingProgramRepository.GetAllAsync();
            return programs;
        }
    }
}
