using IMS.Repositories.Entities;
using IMS.Repositories.Models.CommonModel;
using IMS.Repositories.Models.InternModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Services.Interfaces
{
    public interface IInternService
    {
        Task<LoginModel> CheckLogin(string email, string password);
        Task<bool> CheckExistedIntern(string email);
        Task<List<string>> GetAllInternEmails();
        Task<bool> Create(InternRegisterModel internRegisterModel);
        Task<bool> CreateRange(List<InternRegisterModel> internRegisterModels);
        Task<Intern> GetInternAsync(Guid id);
        Task<List<InternGetModel>> GetAllInterns(InternFilterModel filterModel);
        Task<int> GetTotalInternsCount(InternFilterModel filterModel);
        Task<bool> Update(Guid id, InternUpdateModel internUpdateModel);
        Task<bool> Delete(Guid id);
        Task<bool> Restore(Guid id);
    }
}
