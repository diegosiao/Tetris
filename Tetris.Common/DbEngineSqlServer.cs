using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace Tetris.Common
{
    public class DbEngineSqlServer : DbEngine
    {
        public DbEngineSqlServer(string ConnectionString) : base(SupportedDatabases.SqlServer, ConnectionString)
        {
            DefaultConnectionString = ConnectionString;
        }

        internal override string ProcedureDeclareVar
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        internal override string ProcedureSetVar
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        internal override DbConnection GetConnection()
        {
            return new SqlConnection(DefaultConnectionString);
        }

        internal override DbParameter GetParameter(string Name, object Value)
        {
            return new SqlParameter(Name, Value);
        }
    }
}
