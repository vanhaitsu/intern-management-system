using AutoMapper;
using IMS_View.Services.Interfaces;
using IMS.Models.Interfaces;
using IMS.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Model.ViewModels.TraineeModel;
using Model.ViewModels.AccountModel;

namespace IMS_View.Services.Services
{
    public class TraineeService: ITraineeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TraineeService(IUnitOfWork unitOfWork, IMapper mapper)
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

        public async Task<bool> CheckExistedTrainee(string email)
        {
            Trainee trainee = new Trainee();
            trainee = await _unitOfWork.TraineeRepository.GetTraineeByMail(email);
            if (trainee == null)
            {
                return false;
            }
            return true;
        }

        public async Task<List<string>> GetAllTraineeEmails()
        {
            var trainees = _unitOfWork.TraineeRepository.GetAll(); 

            if (trainees == null)
            {
                return new List<string>(); 
            }

            var emails = trainees.Select(trainee => trainee.Email).ToList();
            return emails;
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

        public async Task<bool> CreateRange(List<TraineeRegisterModel> traineeRegisterModels)
        {
            List<Trainee> trainees = _mapper.Map<List<Trainee>>(traineeRegisterModels);
            foreach(var trainee in trainees)
            {
                trainee.IsDeleted = false;
                trainee.Password = "123456789";
            }
            _unitOfWork.TraineeRepository.AddRangeAsync(trainees);
            if (await _unitOfWork.SaveChangeAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateRange(List<TraineeRegisterModel> traineeRegisterModels)
        {
            List<Trainee> traineesToUpdate = await _unitOfWork.TraineeRepository.GetAllAsync();

            foreach (var model in traineeRegisterModels)
            {
                var existingTrainee = traineesToUpdate.FirstOrDefault(t => t.Email == model.Email);

                if (existingTrainee != null)
                {
                    _mapper.Map(model, existingTrainee); 

                    existingTrainee.IsDeleted = false;
                    existingTrainee.Password = "123456789";
                }
            }
            _unitOfWork.TraineeRepository.UpdateRange(traineesToUpdate);

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
                TrainingProgram programExists = null;
                if (!string.IsNullOrEmpty(traineeUpdateModel.ProgramName))
                {
                    programExists = await _unitOfWork.TrainingProgramRepository.GetAsync(traineeUpdateModel.ProgramId);
                    if (programExists == null)
                    {
                        return false;
                    }

                    if (existedTrainee.TrainingProgram != null)
                    {
                        if (existedTrainee.TrainingProgram.Name == traineeUpdateModel.ProgramName)
                        {
                            return false;
                        }
                    }
                    else
                    {  
                        existedTrainee.TrainingProgram = programExists;
                    }
                }
                _mapper.Map(traineeUpdateModel, existedTrainee);
                if (!string.IsNullOrEmpty(traineeUpdateModel.ProgramName) && existedTrainee.TrainingProgram == null)
                {
                    existedTrainee.TrainingProgram = programExists;
                }

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

        public async Task<List<TraineeGetModel>> GetTraineesByMentor(int pageSize, int pageNumber, string searchTerm, Guid accountId)
        {
            return await _unitOfWork.TraineeRepository.GetTraineesByMentor(pageSize, pageNumber, searchTerm, accountId);
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

        public async Task<int> GetTotal(string searchTerm, Guid accountId)
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
            if(accountId != null)
            {
                query = query.Where(a => a.TrainingProgram != null && a.TrainingProgram.AccountId == accountId);
            }

            return await query.CountAsync();
        }
    }
}
