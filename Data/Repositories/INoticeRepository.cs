using SchoolWebsite1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolWebsite1.Data.Repositories
{
    public interface INoticeRepository
    {
        Task<IEnumerable<Notice>> GetAllAsync();
        Task<Notice> GetByIdAsync(int id);
        Task<int> AddAsync(Notice notice);
        Task<int> UpdateAsync(Notice notice);
        Task<int> DeleteAsync(int id);
    }
}
