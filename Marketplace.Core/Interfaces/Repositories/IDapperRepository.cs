using Dapper;
using System.Data;

namespace Marketplace.Core.Interfaces.Repositories
{
    public interface IDapperRepository
    {
        Task<T> BulkInsert<T>(DataTable dataTable, object parameter, string connectionString, string procedurename, bool userTransactionScope = true);
        Task<T> Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        Task<List<T>> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        Task<T> Insert<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
    }
}
