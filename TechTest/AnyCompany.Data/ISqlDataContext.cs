using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyCompany.Data
{
    public interface ISqlDataContext : IDisposable
    {
        IDbCommand CreateCommand();
        SqlParameter CreateParameter(string paramName, object paramValue);
        SqlParameter CreateOutputParameter(string paramName, SqlDbType type);
        void SaveChanges();


    }
}
