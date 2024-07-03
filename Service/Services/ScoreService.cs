using AutoMapper;
using IMS.Models.Entities;
using IMS.Models.Interfaces;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class ScoreService : IScoreService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ScoreService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task<bool> CreateScoreAsync(Score score)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteScoreByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Score> GetByTraineeIdAsync(Guid trineeId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Score>> GetByTrainingProgramCodeAsync(string code)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateScoreAsync(Score score)
        {
            throw new NotImplementedException();
        }
    }
}

