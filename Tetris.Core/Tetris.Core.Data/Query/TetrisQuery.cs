using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Dynamic;
using System.Threading.Tasks;
using Tetris.Core.Domain.Attributes;
using static Dapper.SqlMapper;

namespace Tetris.Core.Data.Query
{
    public abstract class TetrisQuery : TetrisExecutableBase, IValidatableObject
    {
        private readonly string procedure;

        public TetrisQuery(string procedure = null) 
        {
            this.procedure = procedure;
        }

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

                var outputs = GetOutputsParameterNames();

                var inputoutputs = GetInOutParameterNames();

                foreach (var output in outputs)
                    parameters.Add(output, direction: ParameterDirection.Output);

                foreach (var inout in inputoutputs)
                    parameters.Add(inout, direction: ParameterDirection.InputOutput);

                if (procedureAttr.AddSessionIdParam)
                    parameters.Add("sessionid", Controller?.User?.IdSessao);

                if (procedureAttr.AddOutputsParam)
                    parameters.Add("outputs", null, null, ParameterDirection.Output);

                var connectionString = TetrisSettings.ForQueries;

                if (!string.IsNullOrWhiteSpace(procedureAttr.ConnectionStringKey))
                    connectionString = TetrisStartup.Configuration.GetConnectionString(procedureAttr.ConnectionStringKey);

                using (IDbConnection conn = GetDatabaseConnection(connectionString))
                {
                    conn.Open();

                    switch (procedureAttr.ResultType)
                    {
                        case TetrisQueryResultType.MultipleCollections:
                            result.Result = await conn.QueryMultipleAsync(procedureAttr?.Procedure ?? procedure, parameters, commandType: CommandType.StoredProcedure);
                            await PrepareMultipleCollectionAsync(procedureAttr, result);
                            break;
                        case TetrisQueryResultType.Collection:
                            result.Result = await conn.QueryAsync(procedureAttr?.Procedure ?? procedure, parameters, commandType: CommandType.StoredProcedure);
                            PrepareCollection(procedureAttr, result);
                            break;
                        case TetrisQueryResultType.Single:
                            result.Result = await conn.QuerySingleAsync(procedureAttr?.Procedure ?? procedure, parameters, commandType: CommandType.StoredProcedure);
                            PrepareCollection(procedureAttr, result);
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

        private void PrepareCollection(TetrisProcedureAttribute proceureAttribute, TetrisApiResult result)
        {
            if (result.Result == null || 
                proceureAttribute == null || 
                proceureAttribute.ResultNames.Length == 0)
                return;

            var finalResult = new ExpandoObject();
            finalResult.TryAdd<string, dynamic>(proceureAttribute.ResultNames[0], (object)result.Result);

            result.Result = finalResult;
        }

        private async Task PrepareMultipleCollectionAsync(TetrisProcedureAttribute proceureAttribute, TetrisApiResult result)
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
    }
}
