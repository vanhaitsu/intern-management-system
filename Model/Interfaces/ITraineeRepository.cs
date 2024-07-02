using IMS.Models.Entities;
using Model.ViewModels.TraineeModel;

namespace IMS.Models.Interfaces
{
    public interface ITraineeRepository : IGenericRepository<Trainee>
    {
        Task<List<TraineeGetModel>> GetAllTrainees(int pageSize, int pageNumber, string searchTerm);
        Task<Trainee> GetTraineeByMail(string email);
        IQueryable<Trainee> GetAll();
    }
}
