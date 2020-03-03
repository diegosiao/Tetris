using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Threading.Tasks;

namespace Tetris
{
    /// <summary>
    /// Represents a database read or write operation. Your commands should inherit from this class.
    /// <para>For more information go to <see href="https://github.com/diegosiao/Tetris">https://github.com/diegosiao/Tetris</see></para>
    /// </summary>
    public abstract class TetrisCommand : TetrisExecutableBase, IValidatableObject
    {
        /// <summary>
        /// Overwrite this method to implement custom validation before database executions
        /// </summary>
        /// <param name="validationContext"></param>
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield return null;
        }

        /// <summary>
        /// Call this method in your controller. The properties of the class are treated as parameters of the procedure or SQL associated with this command.
        /// </summary>
        /// <returns></returns>
        public async Task<TetrisApiResult> Execute()
        {
            var result = new TetrisApiResult();

            if (Controller?.ModelState != null && !Controller.ModelState.IsValid)
                return await Task.FromResult(new TetrisApiResult(Controller.ModelState));

            try
            {
                var procedureAttr = GetProcedureAttribute(this);
                var parameters = new DynamicParameters(this);

                if (InternalParameters != null)
                    parameters.AddDynamicParams(InternalParameters);

                var outputs = GetOutputsParameterNames();

                var inputoutputs = GetInOutParameterNames();

                // To override values setting the correct direction
                foreach (var output in outputs)
                    parameters.Add(output, direction: ParameterDirection.Output);

                foreach (var inout in inputoutputs)
                    parameters.Add(inout, GetValue(inout), direction: ParameterDirection.InputOutput);

                if (procedureAttr.AddSessionIdParam)
                    parameters.Add("sessionid", Controller?.User?.IdSessao);

                if (procedureAttr.AddOutputsParam)
                    parameters.Add("outputs", direction: ParameterDirection.Output);

                var connectionString = TetrisSettings.ForCommands;

                if (!string.IsNullOrWhiteSpace(procedureAttr.ConnectionStringKey))
                    connectionString = TetrisStartup.Configuration.GetConnectionString(procedureAttr.ConnectionStringKey);

                using (IDbConnection conn = GetDatabaseConnection(connectionString))
                {
                    conn.Open();

                    var rowsAffected = await conn.ExecuteAsync(new CommandDefinition(procedureAttr.Procedure, parameters, commandType: CommandType.StoredProcedure));
                    result.Succeded = true;
                }

                if (procedureAttr.AddOutputsParam)
                    result.LoadResultOutputs(parameters.Get<string>("outputs"));
                
                LoadOutputParamters(outputs, parameters);
                LoadOutputParamters(inputoutputs, parameters);

                OnExecuted(result);
            }
            catch (Exception ex)
            {
                result.Succeded = false;
                result.Outputs.TryAdd("exception", new { Message = $"Oops... Something bad happened: {ex.Message}" });
            }

            return result;
        }

    }
}
