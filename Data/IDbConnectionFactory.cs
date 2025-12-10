using System.Data;
namespace SchoolWebsite1.Data
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
