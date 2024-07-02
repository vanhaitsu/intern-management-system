using IMS.Models.Entities;
using Model.ViewModels.AccountModel;
using Model.ViewModels.TraineeModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_View.Services.Interfaces
{
    public interface ITraineeService
    {
        Task<TraineeLoginModel> CheckLogin(string email, string password);
        Task<Trainee> GetTraineeAsync(Guid id);
        Task<bool> Update(Guid id, TraineeUpdateModel traineeUpdateModel);
        Task<bool> Delete(Guid id);
        Task<List<TraineeGetModel>> GetAllTrainees(int PageSize, int PageNumber, string SearchTerm);
        Task<bool> Create(TraineeRegisterModel traineeRegisterModel);
        Task<int> GetTotalTraineesCount(string searchTerm);
        Task<bool> Restore(Guid id);
        Task<bool> CreateRange(List<TraineeRegisterModel> traineeRegisterModels);
    }
}
