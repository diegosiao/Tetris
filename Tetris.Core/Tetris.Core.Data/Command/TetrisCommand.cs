using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Dynamic;
using System.Threading.Tasks;
using Tetris.Core.Domain.Attributes;
using Tetris.Core.Result;

namespace Tetris.Core.Data.Command
{
    public abstract class TetrisCommand : TetrisExecutableBase, IValidatableObject
    {
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield return null;
        }

        public async Task<TetrisApiResult> Execute()
        {
            var result = new TetrisApiResult();

            try
            {
                if (Controller?.ModelState != null && !Controller.ModelState.IsValid)
                    return await Task.FromResult(new TetrisApiResult(Controller.ModelState));

                var procedureAttr = GetProcedureAttribute(this);
                var parameters = new DynamicParameters(this);

                if (InternalParameters != null)
                    parameters.AddDynamicParams(InternalParameters);

                var outputs = GetOutputsParameterNames();

                var inputoutputs = GetInOutParameterNames();

                foreach (var output in outputs)
                    parameters.Add(output, direction: ParameterDirection.Output);

                foreach (var inout in inputoutputs)
                    parameters.Add(inout, GetValue(inout), direction: ParameterDirection.InputOutput);

                if (procedureAttr.AddSessionIdParam)
                    parameters.Add("sessionid", Controller?.User?.IdSessao);

                if (procedureAttr.AddOutputsParam)
                    parameters.Add("outputs", null, null, ParameterDirection.Output);

                var connectionString = TetrisSettings.ConnectionStrings_Commands;

                if (!string.IsNullOrWhiteSpace(procedureAttr.ConnectionStringKey))
                    connectionString = TetrisStartup.Configuration.GetConnectionString(procedureAttr.ConnectionStringKey);

                using (IDbConnection conn = GetDatabaseConnection(connectionString))
                {
                    conn.Open();

                    switch (procedureAttr.ResultType)
                    {
                        case TetrisQueryResultType.MultipleCollections:
                            result.Result = await conn.QueryMultipleAsync(procedureAttr?.Procedure, parameters, commandType: CommandType.StoredProcedure);
                            await PrepareMultipleCollectionAsync(procedureAttr, result);
                            break;
                        case TetrisQueryResultType.Collection:
                            result.Result = await conn.QueryAsync(procedureAttr?.Procedure, parameters, commandType: CommandType.StoredProcedure);
                            PrepareCollection(procedureAttr, result);
                            break;
                        case TetrisQueryResultType.Single:
                            var dbSingleResult = await conn.QuerySingleOrDefaultAsync(procedureAttr?.Procedure, parameters, commandType: CommandType.StoredProcedure);
                            
                            if (dbSingleResult != null)
                            {
                                PrepareCollection(procedureAttr, result);
                                result.Result = dbSingleResult;
                            }
                                
                            break;
                    }

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
                result.Outputs.TryAdd("exception", new { Message = $"Desculpe, ocorreu um erro durante o processamento de sua requisição. {ex.Message}" });
            }

            return result;
        }

    }
}
