using IMS.Models.Entities;
using IMS.Models.Interfaces;
using Model.ViewModels.TraineeModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Interfaces
{
    public interface ITraineeRepository : IGenericRepository<Trainee>
    {
        Task<List<TraineeGetModel>> GetAllTrainees(int pageSize, int pageNumber, string searchTerm);
        Task<Trainee> GetTraineeByMail(string email);
        IQueryable<Trainee> GetAll();
    }
}
