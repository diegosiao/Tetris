﻿using Dapper;
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
using MySql.Data.MySqlClient;
using System.Dynamic;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

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
            var database = connectionString?.Split(":");

            string assembly, type;

            switch (database[0].ToLower())
            {
                case "mysql":
                    assembly = "MySql.Data";
                    type = "MySql.Data.MySqlClient.MySqlConnection";
                    break;
                default:
                    throw new TetrisConfigurationException("Tetris connectionStrings should start with the database vendor name ':' then the connection string itself. Go to github.com/diegosiao/Tetris for supported database vendors. ");
            }

            IDbConnection conn = Activator.CreateInstance(assembly, type).Unwrap() as IDbConnection;
            conn.ConnectionString = database[1];

            return conn;
        }


        protected void PrepareCollection(TetrisProcedureAttribute proceureAttribute, TetrisApiResult result)
        {
            if (result.Result == null ||
                proceureAttribute == null ||
                proceureAttribute.ResultNames.Length == 0)
                return;

            var finalResult = new ExpandoObject();
            finalResult.TryAdd<string, dynamic>(proceureAttribute.ResultNames[0], (object)result.Result);

            result.Result = finalResult;
        }

        protected async Task PrepareMultipleCollectionAsync(TetrisProcedureAttribute proceureAttribute, TetrisApiResult result)
        {
            if (result.Result == null ||
                proceureAttribute == null)
                return;

            var finalResult = new ExpandoObject();

            var gridReader = (GridReader)result.Result;

            int i = 0;
            while (!gridReader.IsConsumed)
            {
                var collection = await gridReader.ReadAsync();
                var name = proceureAttribute.ResultNames.Length - 1 >= i ? proceureAttribute.ResultNames[i] : $"collection{i + 1}";
                finalResult.TryAdd<string, dynamic>(name, collection);
                i++;
            }

            result.Result = finalResult;
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
            return expression ? null : new ValidationResult(errorMessage, memberNames);
        }
    }
}
