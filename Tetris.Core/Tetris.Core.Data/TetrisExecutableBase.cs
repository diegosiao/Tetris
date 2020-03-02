using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Tetris.Core.Domain.Attributes;
using Tetris.Core.Exceptions;
using Tetris.Core.Result;
using Tetris.Core.Domain;
using System.Data;
using Tetris.Core.Data.Query;
using Tetris.Core.Data.Command;
using Tetris.Core.Tetris.Core.Application.Exceptions;

namespace Tetris.Core.Data
{
    public abstract class TetrisExecutableBase
    {
        internal TetrisApiController Controller { get; set; }

        protected TetrisUser User => Controller?.User;

        public object InternalParameters { get; set; }

        public virtual void OnExecuted(TetrisApiResult result) {  }

        internal IEnumerable<string> GetOutputsParameterNames()
        {
            foreach (var prop in GetType().GetRuntimeProperties())
            {
                if (prop.GetCustomAttributes(typeof(TetrisOutputParameterAttribute), false).FirstOrDefault() is TetrisOutputParameterAttribute output)
                {
                    yield return prop.Name;
                }
            }
        }

        internal IEnumerable<string> GetInOutParameterNames()
        {
            foreach (var prop in GetType().GetRuntimeProperties())
            {
                if (prop.GetCustomAttributes(typeof(TetrisInputOutputParameterAttribute), false).FirstOrDefault() is TetrisInputOutputParameterAttribute output)
                {
                    yield return prop.Name;
                }
            }
        }

        internal object GetValue(string property)
        {
            try 
            {
                return GetType().GetProperty(property).GetValue(this);
            }
            catch
            {
                return null;
            }
        }

        internal void LoadOutputParamters(IEnumerable<string> outputs, DynamicParameters parameters)
        {
            if (outputs == null || !outputs.Any())
                return;

            foreach (var output in outputs)
            {
                try
                {
                    if (parameters.ParameterNames.Contains(output) && parameters.Get<dynamic>(output) != null)
                    {
                        var prop = GetType().GetProperty(output, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                        if(prop.PropertyType == typeof(bool))
                            prop.SetValue(this, parameters.Get<dynamic>(output).ToString().Equals("1"));
                        else
                            prop.SetValue(this, parameters.Get<dynamic>(output));
                    }
                }
                catch (Exception ex)
                {
                    Debugger.Log(1, "DB", $"Erro carregando os outputs: {ex.Message}");
                }
            }
        }

        protected IDbConnection GetDatabaseConnection(string connectionString)
        {
            if (this is TetrisQuery)
                return Activator.CreateInstance(TetrisSettings.DatabaseConnectionForQueries, connectionString) as IDbConnection;

            if (this is TetrisCommand)
                return Activator.CreateInstance(TetrisSettings.DatabaseConnectionForCommands, connectionString) as IDbConnection;

            throw new TetrisConfigurationException("It is necessary to determine the Type of the Database Connection for Queries and/or Commands before executing database operations. Ex.: Tetris.Settings.SetTetrisDatabaseConnectionTypeForCommands(typeof(MySqlConnection))");
        }

        internal TetrisProcedureAttribute GetProcedureAttribute(object command)
        {
            var attributes = command.GetType().GetCustomAttributes(typeof(TetrisProcedureAttribute), true);

            if (attributes?.Length != 1)
                throw new TetrisDbException($"A procedure do comando não foi especificada com o atributo [MoviProcedure] em '{command.GetType().FullName}'");

            var attribute = attributes[0] as TetrisProcedureAttribute;

            if (string.IsNullOrWhiteSpace(attribute.Procedure))
                throw new TetrisDbException($"O nome da procedure do comando não foi especificada com o atributo [MoviProcedure] em '{command.GetType().FullName}'");

            return attribute;
        }

        internal void SetController(TetrisApiController controller)
        {
            Controller = controller;
        }

        protected ValidationResult AssertNotEmpty(string value, string errorMessage, params string[] memberNames)
        {
            return string.IsNullOrWhiteSpace(value) ? new ValidationResult(errorMessage, memberNames) : null;
        }

        protected ValidationResult AssertTrue(bool expression, string errorMessage, params string[] memberNames)
        {
            return expression ? new ValidationResult(errorMessage, memberNames) : null;
        }
    }
}
