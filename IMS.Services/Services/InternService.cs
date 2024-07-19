using AutoMapper;
using IMS.Services.Interfaces;
using IMS.Repositories.Entities;
using Microsoft.EntityFrameworkCore;
using IMS.Repositories.Interfaces;
using IMS.Repositories.Models.CommonModel;
using IMS.Repositories.Models.InternModel;
using IMS.Repositories.AccountModel;
using IMS.Repositories.Models.AccountModel;

namespace IMS_View.Services.Services
{
    public class InternService : IInternService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public InternService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<LoginResult> CheckLogin(string email, string password)
        {
            if (string.IsNullOrEmpty(email))
            {
                return new LoginResult { ErrorMessage = "Email is required." };
            }

            var intern = await _unitOfWork.InternRepository.GetInternByMail(email);
            if (intern == null)
            {
                return new LoginResult { ErrorMessage = "Email or password is not correct." };
            }
            if (!password.Equals(intern.Password))
            {
                return new LoginResult { ErrorMessage = "Email or password is not correct." };
            }
            if (intern.IsDeleted)
            {
                return new LoginResult { ErrorMessage = "Your account was blocked." };
            }

            var loginModel = new LoginModel
            {
                Id = intern.Id,
                Email = intern.Email,
                Password = intern.Password,
                FullName = intern.FullName,
            };
            return new LoginResult { LoginModel = loginModel };
        }

        public async Task<bool> CheckExistedIntern(string email)
        {
            Intern intern = new Intern();
            intern = await _unitOfWork.InternRepository.GetInternByMail(email);
            if (intern == null)
            {
                return false;
            }
            return true;
        }


        public async Task<bool> SignUp(InternRegisterModel internRegisterModel)
        {
            Intern user = _mapper.Map<Intern>(internRegisterModel);
            _unitOfWork.InternRepository.AddAsync(user);
            if (await _unitOfWork.SaveChangeAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<List<string>> GetAllInternEmails()
        {
            var interns = _unitOfWork.InternRepository.GetAll();

            if (interns == null)
            {
                return new List<string>();
            }

            var emails = interns.Select(interns => interns.Email).ToList();
            return emails;
        }

        public async Task<bool> Create(InternRegisterModel internRegisterModel)
        {
            Intern intern = _mapper.Map<Intern>(internRegisterModel);
            intern.IsDeleted = false;
            _unitOfWork.InternRepository.AddAsync(intern);
            if (await _unitOfWork.SaveChangeAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> CreateRange(List<InternRegisterModel> internRegisterModels)
        {
            List<Intern> interns = _mapper.Map<List<Intern>>(internRegisterModels);
            foreach (var intern in interns)
            {
                intern.IsDeleted = false;
                intern.Password = "123456789";
            }
            _unitOfWork.InternRepository.AddRangeAsync(interns);
            if (await _unitOfWork.SaveChangeAsync() > 0)
            {
                return true;
            }
            return false;
        }


        public async Task<Intern> GetInternAsync(Guid id)
        {
            var intern = await _unitOfWork.InternRepository.GetAsync(id);
            if (intern != null)
            {
                return intern;
            }
            return null;
        }

        public async Task<bool> Update(Guid id, InternUpdateModel internUpdateModel)
        {
            var existedIntern = await _unitOfWork.InternRepository.GetAsync(id);
            if (existedIntern != null)
            {
                _mapper.Map(internUpdateModel, existedIntern);
                _unitOfWork.InternRepository.Update(existedIntern);
                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> Delete(Guid id)
        {
            var existedIntern = await _unitOfWork.InternRepository.GetAsync(id);
            if (existedIntern != null)
            {
                _unitOfWork.InternRepository.SoftDelete(existedIntern);
            }
            if (await _unitOfWork.SaveChangeAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> Restore(Guid id)
        {
            var existedIntern = await _unitOfWork.InternRepository.GetAsync(id);
            if (existedIntern != null)
            {
                existedIntern.IsDeleted = false;
                _unitOfWork.InternRepository.Update(existedIntern);
                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<List<InternGetModel>> GetAllInterns(InternFilterModel filterModel)
        {
            var internList = await _unitOfWork.InternRepository.GetAllAsync(
                filter: x =>
                    x.IsDeleted == filterModel.IsDeleted &&
                    (filterModel.MentorId == null || x.Mentorships.SingleOrDefault().AccountId == filterModel.MentorId) &&
                    (string.IsNullOrEmpty(filterModel.Search) || x.FullName.ToLower().Contains(filterModel.Search.ToLower()) ||
                     x.Email.ToLower().Contains(filterModel.Search.ToLower())),
                orderBy: x => filterModel.OrderByDescending
                    ? x.OrderByDescending(x => x.CreationDate)
                    : x.OrderBy(x => x.CreationDate),
                pageIndex: filterModel.PageNumber,
                pageSize: filterModel.PageSize,
                includeProperties: "Mentorships,Feedbacks,Assignments,Applications"
            );

            List<InternGetModel> internDetailList = null;

            if (internList != null)
            {
                internDetailList = internList.Data.Select(x => new InternGetModel
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    Email = x.Email,
                    DOB = x.DOB,
                    Gender = x.Gender,
                    PhoneNumber = x.PhoneNumber,
                    Address = x.Address,
                    IsDelete = x.IsDeleted,
                    Skill = x.Skill,
                    Education = x.Education,
                    WorkHistory = x.WorkHistory,
                    feedbacks = x.Feedbacks.Where(f => f.InternId == x.Id).Select(f => f.Comment).ToList(),
                }).ToList();
            }

            return internDetailList ?? new List<InternGetModel>();
        }

        public async Task<int> GetTotalInternsCount(InternFilterModel filterModel)
        {
            IQueryable<Intern> query = _unitOfWork.InternRepository.GetAll().AsQueryable();


            if (!string.IsNullOrEmpty(filterModel.Search))
            {
                var search = filterModel.Search.ToLower();
                query = query.Where(a =>
                    a.FullName.ToLower().Contains(search) ||
                    a.Email.ToLower().Contains(search)
                );
            }
            if (filterModel.MentorId != null)
            {
                query = query.Where(x => x.Mentorships.SingleOrDefault().Id == filterModel.MentorId);
            }

            return await query.CountAsync();
        }

        public async Task<Intern> GetByEmail(string email)
        {
            return await _unitOfWork.InternRepository.GetInternByMail(email);
        }

        public async Task<List<Intern>> GetRegisterCustomer()
        {
            return await _unitOfWork.InternRepository.GetRegisterCustomer();
        }

        public async Task<bool> RegisterIntern(Intern intern)
        {
            await _unitOfWork.InternRepository.AddAsync(intern);
            if (await _unitOfWork.SaveChangeAsync() > 0)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> Edit(Intern intern)
        {
            _unitOfWork.InternRepository.Update(intern);
            if (await _unitOfWork.SaveChangeAsync() > 0)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> HardDelete(Guid id)
        {
            var existedIntern = await _unitOfWork.InternRepository.GetAsync(id);
            if (existedIntern != null)
            {
                _unitOfWork.InternRepository.HardDelete(existedIntern);
            }
            if (await _unitOfWork.SaveChangeAsync() > 0)
            {
                return true;
            }
            return false;
        }
    }
}
