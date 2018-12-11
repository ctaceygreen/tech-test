using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyCompany.Data
{
    public class AdoContext : ISqlDataContext
    {
        private IDbConnection _connection;
        private IDbTransaction _transaction;

        public AdoContext(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }
        public IDbCommand CreateCommand()
        {
            var command = _connection.CreateCommand();
            if (_transaction == null)
            {
                //Create new transaction
                _transaction = _connection.BeginTransaction();
            }
            command.Transaction = _transaction;
            return command;
        }
        public SqlParameter CreateParameter(string paramName, object paramValue)
        {
            return new SqlParameter(paramName, paramValue);
        }
        public SqlParameter CreateOutputParameter(string paramName, SqlDbType type)
        {
            SqlParameter IDParameter = new SqlParameter(paramName, type);
            IDParameter.Direction = ParameterDirection.Output;
            return IDParameter;
        }
        public void SaveChanges()
        {
            _transaction.Commit();
            _transaction = null;
        }
        public void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
                _transaction = null;
            }
            if (_connection != null)
            {
                _connection.Close();
                _connection = null;
            }
        }
    }
}
