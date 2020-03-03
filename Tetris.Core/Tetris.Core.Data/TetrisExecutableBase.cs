using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Tetris.Core.Domain.Attributes;
using Tetris.Core.Exceptions;
using Tetris.Core.Domain;
using System.Data;
using Tetris.Core.Data.Query;
using Tetris.Core.Data.Command;
using Tetris.Core.Tetris.Core.Application.Exceptions;

namespace Tetris.Core.Data
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class TetrisExecutableBase
    {
        internal TetrisApiController Controller { get; set; }

        /// <summary>
        /// The user associated with the request that called this operation
        /// </summary>
        protected TetrisUser User => Controller?.User;

        /// <summary>
        /// Additional parameters not exposed as properties
        /// </summary>
        public object InternalParameters { get; set; }

        /// <summary>
        /// Override this method to access and/or modify the result of the operation
        /// </summary>
        /// <param name="result"></param>
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

        internal IDbConnection GetDatabaseConnection(string connectionString)
        {
            if (this is TetrisQuery)
                return Activator.CreateInstance(TetrisSettings.DatabaseConnectionForQueries, connectionString) as IDbConnection;

            if (this is TetrisCommand)
                return Activator.CreateInstance(TetrisSettings.DatabaseConnectionForCommands, connectionString) as IDbConnection;

            throw new TetrisConfigurationException("It is necessary to determine the Type of the Database Connection for Queries and/or Commands before executing database operations. Ex.: Tetris.Settings.SetTetrisDatabaseConnectionTypeForCommands(typeof(MySqlConnection)). ");
        }

        internal TetrisProcedureAttribute GetProcedureAttribute(object command)
        {
            var attributes = command.GetType().GetCustomAttributes(typeof(TetrisProcedureAttribute), true);

            if (attributes?.Length != 1)
                throw new TetrisDbException($"The class '{command.GetType().FullName}' does not specify the attribute [TetrisProcedure]. ");

            var attribute = attributes[0] as TetrisProcedureAttribute;

            if (string.IsNullOrWhiteSpace(attribute.Procedure))
                throw new TetrisDbException($"The attribute [TetrisProcedure] in class '{command.GetType().FullName}' does not provide the required procedure name. ");

            return attribute;
        }

        internal void SetController(TetrisApiController controller)
        {
            Controller = controller;
        }

        /// <summary>
        /// Prepared assertion to help validating data when overriding 'Validate()'
        /// </summary>
        /// <param name="value"></param>
        /// <param name="errorMessage"></param>
        /// <param name="memberNames"></param>
        /// <returns></returns>
        protected ValidationResult AssertNotEmpty(string value, string errorMessage, params string[] memberNames)
        {
            return string.IsNullOrWhiteSpace(value) ? new ValidationResult(errorMessage, memberNames) : null;
        }

        /// <summary>
        /// Prepared assertion to help validating data when overriding 'Validate()'
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="errorMessage"></param>
        /// <param name="memberNames"></param>
        /// <returns></returns>
        protected ValidationResult AssertTrue(bool expression, string errorMessage, params string[] memberNames)
        {
            return expression ? new ValidationResult(errorMessage, memberNames) : null;
        }
    }
}
