using IMS.Models;
using IMS.Models.Entities;
using IMS.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Model.ViewModels.AccountModel;
using Model.ViewModels.TraineeModel;

namespace IMS.Models.Repositories
{
    public class TraineeRepository : GenericRepository<Trainee>, ITraineeRepository
    {
        private readonly AppDbContext _dbContext;
        public TraineeRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Trainee> GetTraineeByMail(string email)
        {
            return await _dbContext.Trainees.SingleOrDefaultAsync(a => a.Email.Equals(email));
        }
        public IQueryable<Trainee> GetAll()
        {
            return _dbContext.Trainees.AsQueryable();
        }

        public async Task<List<TraineeGetModel>> GetAllTrainees(int pageSize, int pageNumber, string searchTerm)
        {
            IQueryable<Trainee> query = _dbContext.Trainees.Include(a => a.Scores).Include(a => a.TrainingProgram);
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(a =>
                    a.FullName.Contains(searchTerm.ToLower()) ||
                    a.Email.Contains(searchTerm.ToLower()) ||
                    a.PhoneNumber.Contains(searchTerm.ToLower())
                );
            }

            var traineeModels = await query
                .Select(a => new TraineeGetModel
                {
                    Id = a.Id,
                    FullName = a.FullName,
                    Address = a.Address,
                    DOB = a.DOB,
                    Email = a.Email,
                    Gender = a.Gender,
                    PhoneNumber = a.PhoneNumber,
                    Code = a.Code,
                    University = a.University,
                    Status = a.Status,
                    ProgramId = a.ProgramId,
                    ProgramName = a.TrainingProgram.Name,
                    IsDelete = a.IsDeleted
                })
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return traineeModels;
        }

        public async Task<List<TraineeGetModel>> GetTraineesByMentor(int pageSize, int pageNumber, string searchTerm, Guid accountId)
        {
            IQueryable<Trainee> query = _dbContext.Trainees.Include(a => a.Scores).Include(a => a.TrainingProgram);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(a =>
                    a.FullName.Contains(searchTerm.ToLower()) ||
                    a.Email.Contains(searchTerm.ToLower()) ||
                    a.PhoneNumber.Contains(searchTerm.ToLower())
                );
            }

            var traineeModels = await query
                .Where(a => a.TrainingProgram != null && a.TrainingProgram.AccountId == accountId)
                .Select(a => new TraineeGetModel
                {
                    Id = a.Id,
                    FullName = a.FullName,
                    Address = a.Address,
                    DOB = a.DOB,
                    Email = a.Email,
                    Gender = a.Gender,
                    PhoneNumber = a.PhoneNumber,
                    Code = a.Code,
                    University = a.University,
                    Status = a.Status,
                    ProgramId = a.ProgramId,
                    ProgramName = a.TrainingProgram.Name,
                    IsDelete = a.IsDeleted
                })
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return traineeModels;
        }

        public async Task<List<TraineeGetModel>> GetTraineesByProgram(int pageSize, int pageNumber, 
            Guid trainingProgramId)
        {
            IQueryable<Trainee> query = _dbContext.Trainees.Include(a => a.TrainingProgram);
            if (trainingProgramId != null)
            {
                query = query.Where(a =>
                    a.ProgramId == trainingProgramId
                );
            }

            var traineeModels = await query
                .Select(a => new TraineeGetModel
                {
                    Id = a.Id,
                    FullName = a.FullName,
                    Address = a.Address,
                    DOB = a.DOB,
                    Email = a.Email,
                    Gender = a.Gender,
                    PhoneNumber = a.PhoneNumber,
                    Code = a.Code,
                    University = a.University,
                    Status = a.Status,
                    ProgramId = a.ProgramId,
                    ProgramName = a.TrainingProgram.Name,
                    IsDelete = a.IsDeleted
                })
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return traineeModels;
        }
    }
}
