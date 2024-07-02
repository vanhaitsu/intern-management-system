using AutoMapper;
using IMS_View.Services.Interfaces;
using IMS.Models.Interfaces;
using IMS.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Model.ViewModels.TraineeModel;

namespace IMS_View.Services.Services
{
    public class TraneeService: ITraineeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TraneeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TraineeLoginModel> CheckLogin(string email, string password)
        {
            Trainee trainee = new Trainee();
            TraineeLoginModel loginModel = new TraineeLoginModel();
            if (!string.IsNullOrEmpty(email))
            {
                trainee = await _unitOfWork.TraineeRepository.GetTraineeByMail(email);
                if (trainee == null || !password.Equals(trainee.Password))
                {
                    return null;
                }
                else
                {
                    loginModel.Id = trainee.Id;
                    loginModel.Email = trainee.Email;
                    loginModel.Password = trainee.Password;
                    loginModel.FullName = trainee.FullName;
                    loginModel.PhoneNumber = trainee.PhoneNumber;
                    loginModel.DOB = trainee.DOB;
                    loginModel.Gender = trainee.Gender;
                    loginModel.University = trainee.University;
                }
            }
            return loginModel;
        }

        public async Task<bool> SignUp(TraineeRegisterModel traineeRegisterModel)
        {
            Trainee trainee = _mapper.Map<Trainee>(traineeRegisterModel);
            _unitOfWork.TraineeRepository.AddAsync(trainee);
            if (await _unitOfWork.SaveChangeAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> Create(TraineeRegisterModel traineeRegisterModel)
        {
            Trainee trainee = _mapper.Map<Trainee>(traineeRegisterModel);
            trainee.IsDeleted = false;
            _unitOfWork.TraineeRepository.AddAsync(trainee);
            if (await _unitOfWork.SaveChangeAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<Trainee> GetTraineeAsync(Guid id)
        {
            var trainees = await _unitOfWork.TraineeRepository.GetAsync(id);
            if (trainees != null)
            {
                return trainees;
            }
            return null;
        }

        public async Task<bool> Update(Guid id, TraineeUpdateModel traineeUpdateModel)
        {
            var existedTrainee = await _unitOfWork.TraineeRepository.GetAsync(id);
            if (existedTrainee != null)
            {
                _mapper.Map(traineeUpdateModel, existedTrainee);
                _unitOfWork.TraineeRepository.Update(existedTrainee);
                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    return true;
                }
            }
            return false;
        }


        public async Task<bool> Delete(Guid id)
        {
            var existedTrainee = await _unitOfWork.TraineeRepository.GetAsync(id);
            if (existedTrainee != null)
            {
                _unitOfWork.TraineeRepository.SoftDelete(existedTrainee);
            }
            if (await _unitOfWork.SaveChangeAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> Restore(Guid id)
        {
            var existedTrainee = await _unitOfWork.TraineeRepository.GetAsync(id);
            if (existedTrainee != null)
            {
                existedTrainee.IsDeleted = false;
                _unitOfWork.TraineeRepository.Update(existedTrainee);
                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<List<TraineeGetModel>> GetAllTrainees(int pageSize, int pageNumber, string searchTerm)
        {
            return await _unitOfWork.TraineeRepository.GetAllTrainees(pageSize, pageNumber, searchTerm);
        }

        public async Task<int> GetTotalTraineesCount(string searchTerm)
        {
            IQueryable<Trainee> query = _unitOfWork.TraineeRepository.GetAll().AsQueryable();


            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(a =>
                    a.FullName.Contains(searchTerm.ToLower()) ||
                    a.Email.Contains(searchTerm.ToLower()) ||
                    a.PhoneNumber.Contains(searchTerm.ToLower())
                );
            }

            return await query.CountAsync();
        }
    }
}
