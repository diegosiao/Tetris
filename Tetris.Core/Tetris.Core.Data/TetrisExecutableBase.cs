using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Tetris.Core.Data
{
    public abstract class TetrisExecutableBase
    {
        internal TetrisApiController Controller { get; set; }

        protected TetrisUser User => Controller?.User;

        public object InternalParameters { get; set; }

        public virtual void OnExecuted(MoviApiResult result) {  }

        internal IEnumerable<string> GetOutputsParameterNames()
        {
            foreach (var prop in GetType().GetRuntimeProperties())
            {
                if (prop.GetCustomAttributes(typeof(MoviOutputParameterAttribute), false).FirstOrDefault() is MoviOutputParameterAttribute output)
                {
                    yield return prop.Name;
                }
            }
        }

        internal IEnumerable<string> GetInOutParameterNames()
        {
            foreach (var prop in GetType().GetRuntimeProperties())
            {
                if (prop.GetCustomAttributes(typeof(MoviInputOutputParameterAttribute), false).FirstOrDefault() is MoviInputOutputParameterAttribute output)
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

        internal MoviProcedureAttribute GetProcedureAttribute(object command)
        {
            var attributes = command.GetType().GetCustomAttributes(typeof(MoviProcedureAttribute), true);

            if (attributes?.Length != 1)
                throw new MoviDbException($"A procedure do comando não foi especificada com o atributo [MoviProcedure] em '{command.GetType().FullName}'");

            var attribute = attributes[0] as MoviProcedureAttribute;

            if (string.IsNullOrWhiteSpace(attribute.Procedure))
                throw new MoviDbException($"O nome da procedure do comando não foi especificada com o atributo [MoviProcedure] em '{command.GetType().FullName}'");

            return attribute;
        }

        internal void SetController(MoviApiController controller)
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
