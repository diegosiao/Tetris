using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Dynamic;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace Movi.Api.Core
{
    public class MoviQuery : MoviExecutableBase, IValidatableObject
    {
        private readonly string procedure;

        public MoviQuery(string procedure = null) 
        {
            this.procedure = procedure;
        }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield return null;
        }

        public async Task<MoviApiResult> Execute()
        {
            var result = new MoviApiResult();

            try
            {
                if (Controller?.ModelState != null && !Controller.ModelState.IsValid)
                    return await Task.FromResult(new MoviApiResult(Controller.ModelState));

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

                var connectionString = MoviSettings.ConnectionStrings_Queries;

                if (!string.IsNullOrWhiteSpace(procedureAttr.ConnectionStringKey))
                    connectionString = MoviStartup.Configuration.GetConnectionString(procedureAttr.ConnectionStringKey);

                using (IDbConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    switch (procedureAttr.ResultType)
                    {
                        case MoviQueryResultType.MultipleCollections:
                            result.Result = await conn.QueryMultipleAsync(procedureAttr?.Procedure ?? procedure, parameters, commandType: CommandType.StoredProcedure);
                            await PrepareMultipleCollectionAsync(procedureAttr, result);
                            break;
                        case MoviQueryResultType.Collection:
                            result.Result = await conn.QueryAsync(procedureAttr?.Procedure ?? procedure, parameters, commandType: CommandType.StoredProcedure);
                            PrepareCollection(procedureAttr, result);
                            break;
                        case MoviQueryResultType.Single:
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

        private void PrepareCollection(MoviProcedureAttribute proceureAttribute, MoviApiResult result)
        {
            if (result.Result == null || 
                proceureAttribute == null || 
                proceureAttribute.ResultNames.Length == 0)
                return;

            var finalResult = new ExpandoObject();
            finalResult.TryAdd<string, dynamic>(proceureAttribute.ResultNames[0], (object)result.Result);

            result.Result = finalResult;
        }

        private async Task PrepareMultipleCollectionAsync(MoviProcedureAttribute proceureAttribute, MoviApiResult result)
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
