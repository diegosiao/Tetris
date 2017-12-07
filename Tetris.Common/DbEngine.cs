using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using Tetris.Mapping;

namespace Tetris.Common
{
    public enum SupportedDatabases { SqlServer }

    public abstract class DbEngine
    {
        public readonly SupportedDatabases Database;

        public string DefaultConnectionString { get; set; }

        public DbEngine(SupportedDatabases Database, string ConnectionString)
        {
            this.Database = Database;
        }

        public Dictionary<string, StatementSql> CachedStatements { get; set; }

        internal abstract string SqlProcedureCreation(DbProcedure procedure);

        internal abstract string ProcedureDeclareVar { get; }

        internal abstract string ProcedureSetVar { get; }
                
        internal abstract DbConnection GetConnection();

        internal abstract DbParameter GetParameter(string Name, object Value);

        internal virtual StatementSql GetInsertStatement(object Object)
        {
            var statement = new StatementSql();
            statement.CommandType = CommandType.Text;

            var insertColumns = new StringBuilder($"INSERT INTO { MappingUtils.GetTable(Object) } (");
            var insertValues = new StringBuilder("VALUES(");

            foreach (var item in MappingUtils.GetWriteValues(Object))
            {
                if (item.Key.Equals(MappingUtils.GetPrimaryKeyName(Object)))
                    continue;

                insertColumns.Append(item.Key).Append(",");
                insertValues.Append("@" + item.Key).Append(",");

                statement.Parameters.Add(GetParameter("@" + item.Key, item.Value));
            }
            
            statement.Sentence = 
                insertColumns.ToString().Substring(0, insertColumns.Length - 1) + ") " + 
                insertValues.ToString().Substring(0, insertValues.Length - 1) + "); ";

            return statement;
        }

        internal virtual StatementSql GetUpdateStatement(object Object)
        {
            var statement = new StatementSql();
            statement.CommandType = CommandType.Text;

            var updateStatement = new StringBuilder($"UPDATE { MappingUtils.GetTable(Object) } SET ");

            var primaryKeyName = MappingUtils.GetPrimaryKeyName(Object);
            var mappedValues = MappingUtils.GetWriteValues(Object);
            foreach (var item in mappedValues)
            {
                if (item.Key.Equals(primaryKeyName))
                    continue;

                updateStatement.Append($"{ item.Key } = @{ item.Key }, ");

                statement.Parameters.Add(GetParameter("@" + item.Key, item.Value));
            }
            
            statement.Parameters.Add(GetParameter($"@{ primaryKeyName }", mappedValues[primaryKeyName]));

            statement.Sentence = updateStatement.ToString().Substring(0, updateStatement.Length - 2);
            statement.Sentence += $" WHERE { primaryKeyName } = @{ primaryKeyName }"; 
            return statement;
        }

        internal virtual StatementSql GetDeleteStatement(object Object, string primaryKeyName, object primaryKeyValue)
        {
            var statement = new StatementSql();
            statement.CommandType = CommandType.Text;

            statement.Sentence = $"DELETE FROM { MappingUtils.GetTable(Object) } WHERE { primaryKeyName } = @{ primaryKeyName }";
            statement.Parameters.Add(GetParameter($"@{ primaryKeyName }", primaryKeyValue));

            return statement;
        }

        internal virtual StatementSql GetByIdStatement(object Object, string primaryKeyName, object primaryKeyValue)
        {
            var statement = new StatementSql();
            statement.CommandType = CommandType.Text;

            statement.Sentence = $"SELECT * FROM { MappingUtils.GetTable(Object) } WHERE { primaryKeyName } = @{ primaryKeyName }";
            statement.Parameters.Add(GetParameter($"@{ primaryKeyName }", primaryKeyValue));

            return statement;
        }

        internal virtual void ExecuteStatement(StatementSql statement)
        {
            using(var connection = GetConnection())
            {
                var command = connection.CreateCommand();
                command.CommandType = statement.CommandType;
                command.CommandText = statement.Sentence;
                command.Parameters.AddRange(statement.Parameters.ToArray());

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        internal virtual DbDataReader ExecuteReadingStatement(StatementSql statement)
        {
            var command = GetConnection().CreateCommand();
            command.CommandType = statement.CommandType;
            command.CommandText = statement.Sentence;
            command.Parameters.AddRange(statement.Parameters.ToArray());
                
            command.Connection.Open();
            return command.ExecuteReader();
        }
    }
}
