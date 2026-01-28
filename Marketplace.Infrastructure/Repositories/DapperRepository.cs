using Dapper;
using Marketplace.Core.Interfaces.Repositories;
using Marketplace.Infrastructure.Data.Context;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace Marketplace.Infrastructure.Repositories
{
    public class DapperRepository : IDapperRepository
    {
        protected readonly DapperContext _context;

        public DapperRepository(DapperContext context)
        {
            _context = context;
        }


        public async Task<T> Insert<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using var connection = _context.CreateConnection();

            // Cast to DbConnection to access OpenAsync
            if (connection is DbConnection dbConnection)
            {
                if (dbConnection.State != System.Data.ConnectionState.Open)
                    await dbConnection.OpenAsync();
            }
            else
            {
                // Fallback if the underlying connection doesn't support async
                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();
            }

            using var tran = connection.BeginTransaction();

            // Execute the query
            var result = (await connection.QueryAsync<T>(sp, parms, commandType: commandType, transaction: tran))
                         .FirstOrDefault();

            // Commit only if successful. 
            // If an error occurs before this line, 'tran' is disposed automatically, 
            // which triggers a rollback in almost all ADO.NET providers.
            tran.Commit();

            return result;
        }

        public async Task<List<T>> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            List<T> result;
            using var connection = _context.CreateConnection();

            if (connection is DbConnection dbConnection)
            {
                if (dbConnection.State != System.Data.ConnectionState.Open)
                    await dbConnection.OpenAsync();
            }
            else
            {
                // Fallback if the underlying connection doesn't support async
                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();
            }

            using var tran = connection.BeginTransaction();

            result = (await connection.QueryAsync<T>(sp, parms, commandType: commandType, transaction: tran)).ToList();

            tran.Commit();


            return result;
        }

        public async Task<T> Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            using var connection = _context.CreateConnection();

            if (connection is DbConnection dbConnection)
            {
                if (dbConnection.State != System.Data.ConnectionState.Open)
                    await dbConnection.OpenAsync();
            }
            else
            {
                // Fallback if the underlying connection doesn't support async
                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();
            }

            using var tran = connection.BeginTransaction();

            result = (await connection.QueryAsync<T>(sp, parms, commandType: commandType, transaction: tran)).FirstOrDefault();

            tran.Commit();


            return result;
        }

        public async Task<T> BulkInsert<T>(DataTable dataTable, object parameter, string connectionString, string procedurename, bool userTransactionScope = true)
        {
            T result;
            using IDbConnection db = new SqlConnection(connectionString);
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                if (userTransactionScope)
                {
                    using var tran = db.BeginTransaction();
                    try
                    {
                        result = await db.ExecuteScalarAsync<T>(procedurename, parameter, commandType: CommandType.StoredProcedure, transaction: tran);
                        tran.Commit();
                    }
                    catch (Exception)
                    {
                        tran.Rollback();
                        throw;
                    }
                }
                else
                {
                    try
                    {
                        result = await db.ExecuteScalarAsync<T>(procedurename, parameter, commandType: CommandType.StoredProcedure);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }
            return result;
        }
    }
}
