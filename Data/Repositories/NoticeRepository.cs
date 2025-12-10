using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SchoolWebsite1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolWebsite1.Data.Repositories
{
    public class NoticeRepository : INoticeRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        public NoticeRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<IEnumerable<Notice>> GetAllAsync()
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            string sql = "SELECT * FROM Notices";
            return await connection.QueryAsync<Notice>(sql);
        }

        public async Task<Notice> GetByIdAsync(int id)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            string sql = "SELECT * FROM Notices WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<Notice>(sql, new { Id = id });
        }

        public async Task<int> AddAsync(Notice notice)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            string sql = @"INSERT INTO Notices 
                           (Title, Message, CreatedAt) 
                           VALUES (@Title, @Message, @CreatedAt)";
            return await connection.ExecuteAsync(sql, notice);
        }

        public async Task<int> UpdateAsync(Notice notice)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            string sql = @"UPDATE Notices SET 
                           Title = @Title,
                           Message = @Message
                           WHERE Id = @Id";
            return await connection.ExecuteAsync(sql, notice);
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            string sql = "DELETE FROM Notices WHERE Id = @Id";
            return await connection.ExecuteAsync(sql, new { Id = id });
        }

    }
}
