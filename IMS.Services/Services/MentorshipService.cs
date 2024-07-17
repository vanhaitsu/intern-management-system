using AutoMapper;
using IMS.Repositories.Entities;
using IMS.Repositories.Interfaces;
using IMS.Repositories.Models.MentorshipModel;
using IMS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Services.Services
{
    public class MentorshipService : IMentorshipService
    {
        private readonly IUnitOfWork _unitofWork;
        private readonly IMapper _mapper;
        public MentorshipService(IUnitOfWork unitofWork, IMapper mapper)
        {
            _unitofWork = unitofWork;
            _mapper = mapper;
        }
        public async Task<bool> LinkMentorforIntern(Guid internId, Guid MentorId, Guid CreatedBy)
        {
            var mentorToLink = _unitofWork.AccountRepository.GetAsync(MentorId);
            var internToLink = _unitofWork.InternRepository.GetAsync(internId);
            if(mentorToLink != null && internToLink != null)
            {
               
                var result = new Mentorship()
                {
                    InternId = internId,
                    AccountId = MentorId,
                    CreatedBy = CreatedBy
                };
                await _unitofWork.MentorshipRepository.AddAsync(result);
                await _unitofWork.SaveChangeAsync();
                return true;
            }
            return false;
        }
    }
}
