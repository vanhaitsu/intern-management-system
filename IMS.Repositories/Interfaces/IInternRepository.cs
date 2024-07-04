
using IMS.Repositories.Entities;
using IMS.Repositories.Interfaces;


namespace IMS.Models.Interfaces
{
    public interface IInternRepository : IGenericRepository<Intern>
    {
        //Task<List<TraineeGetModel>> GetAllTrainees(int pageSize, int pageNumber, string searchTerm);
        //Task<List<TraineeGetModel>> GetTraineesByMentor(int pageSize, int pageNumber, string searchTerm, Guid accountId);
        Task<Intern> GetInternByMail(string email);
        IQueryable<Intern> GetAll();
    }
}
