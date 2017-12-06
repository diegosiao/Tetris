using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using Tetris.Common.Mapping;

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

        public Dictionary<string, SqlStatement> CachedStatements { get; set; }

        internal abstract string ProcedureDeclareVar { get; }

        internal abstract string ProcedureSetVar { get; }

        internal abstract DbConnection GetConnection();

        internal abstract DbParameter GetParameter(string Name, object Value);

        internal virtual SqlStatement GetInsertStatement(Type ObjectType, Dictionary<string, object> MappedValues, string primaryKeyName)
        {
            var statement = new SqlStatement();
            statement.CommandType = CommandType.Text;

            var insertColumns = new StringBuilder($"INSERT INTO { MappingUtils.GetTable(ObjectType) } (");
            var insertValues = new StringBuilder("VALUES(");

            foreach (var item in MappedValues)
            {
                if (item.Key.Equals(primaryKeyName))
                    continue;

                insertColumns.Append(item.Key).Append(",");
                insertValues.Append("@" + item.Key).Append(",");

                statement.Parameters.Add(GetParameter("@" + item.Key, item.Value));
            }
            
            statement.Sentence = 
                insertColumns.ToString().Substring(0, insertColumns.Length - 1) + ") " + 
                insertValues.ToString().Substring(0, insertValues.Length - 1) + ") ";

            return statement;
        }

        internal virtual SqlStatement GetUpdateStatement(Type ObjectType, Dictionary<string, object> MappedValues, string primaryKeyName)
        {
            var statement = new SqlStatement();
            statement.CommandType = CommandType.Text;

            var updateStatement = new StringBuilder($"UPDATE { MappingUtils.GetTable(ObjectType) } SET ");
            
            foreach (var item in MappedValues)
            {
                if (item.Key.Equals(primaryKeyName))
                    continue;

                updateStatement.Append($"{ item.Key } = @{ item.Key }, ");

                statement.Parameters.Add(GetParameter("@" + item.Key, item.Value));
            }
            
            statement.Parameters.Add(GetParameter($"@{ primaryKeyName }", MappedValues[primaryKeyName]));

            statement.Sentence = updateStatement.ToString().Substring(0, updateStatement.Length - 2);
            statement.Sentence += $" WHERE { primaryKeyName } = @{ primaryKeyName }"; 
            return statement;
        }

        internal virtual SqlStatement GetDeleteStatement(Type ObjectType, string primaryKeyName, object primaryKeyValue)
        {
            var statement = new SqlStatement();
            statement.CommandType = CommandType.Text;

            statement.Sentence = $"DELETE FROM { MappingUtils.GetTable(ObjectType) } WHERE { primaryKeyName } = @{ primaryKeyName }";
            statement.Parameters.Add(GetParameter($"@{ primaryKeyName }", primaryKeyValue));

            return statement;
        }

        internal virtual SqlStatement GetByIdStatement(Type ObjectType, string primaryKeyName, object primaryKeyValue)
        {
            var statement = new SqlStatement();
            statement.CommandType = CommandType.Text;

            statement.Sentence = $"SELECT * FROM { MappingUtils.GetTable(ObjectType) } WHERE { primaryKeyName } = @{ primaryKeyName }";
            statement.Parameters.Add(GetParameter($"@{ primaryKeyName }", primaryKeyValue));

            return statement;
        }

        internal virtual void ExecuteStatement(SqlStatement statement)
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

        internal virtual DbDataReader ExecuteReadingStatement(SqlStatement statement)
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
