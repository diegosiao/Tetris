using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

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

        internal override string SqlProcedureCreation(DbProcedure procedure)
        {
            var sql = new StringBuilder()
            .AppendLine($"CREATE PROCEDURE { procedure.GetSchema() }.{ procedure.GetName() }")
            .AppendLine(" --  #PARAMETERS# @param1 INT;")
            .AppendLine("AS")
            .AppendLine("BEGIN")
            .AppendLine(procedure.ToString())
            .AppendLine("END")
            .AppendLine("-- GO");

            return sql.ToString();
        }

        private string BuildedProcedureBody(DbProcedure procedure)
        {
            var body = new StringBuilder();

            

            return "";
        }
    }
}
