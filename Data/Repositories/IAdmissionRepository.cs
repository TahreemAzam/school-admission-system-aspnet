using SchoolWebsite1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolWebsite1.Data.Repositories
{
    public interface IAdmissionRepository
    {
        Task<IEnumerable<Admission>> GetAllAsync();
        Task<Admission> GetByIdAsync(int id);
        Task<int> AddAsync(Admission admission);
        Task<int> UpdateAsync(Admission admission);
        Task<int> UpdateStatusAsync(int id, string status);

        Task<int> DeleteAsync(int id);
    }
}
