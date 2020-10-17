using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Tetris.Core.Result
{
    public class TetrisApiResult
    {
        public bool Succeded
        {
            get;
            set;
        }

        public bool Failed => !Succeded;

        public List<TetrisApiResultOutput> Outputs { get; set; }

        public dynamic Result
        {
            get;
            set;
        }

        public TetrisApiResult() { }

        public TetrisApiResult(ModelStateDictionary modelState)
        {
            Outputs = modelState.Select(x => new TetrisApiResultOutput(x)).ToList();
        }

        public void LoadResultOutputs(string outputs)
        {
            if (string.IsNullOrEmpty(outputs))
                return;

            //FORMATO ESPERADO:
            //name=This name is a little weird&email=[i,C548]Google emails are preferred(gmail.com)&[warning]This account will be manually analysed'
            var messages = outputs.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
            
            Outputs = messages.Select(x => new TetrisApiResultOutput(x)).ToList();

            /*
            if (_outputs.Any(x => x.Type == TetrisApiResultOutput.TetrisOutputType.Exception))
                Outputs.TryAdd("exceptions", _outputs.Where(x => x.Type == TetrisApiResultOutput.TetrisOutputType.Exception).Select(x => new { x.Key, x.Code, x.Message }));

            if (_outputs.Any(x => x.Type == TetrisApiResultOutput.TetrisOutputType.Error))
                Outputs.TryAdd("errors", _outputs.Where(x => x.Type == TetrisApiResultOutput.TetrisOutputType.Error).Select(x => new { x.Key, x.Code, x.Message }));

            if (_outputs.Any(x => x.Type == TetrisApiResultOutput.TetrisOutputType.Warning))
                Outputs.TryAdd("warnings", _outputs.Where(x => x.Type == TetrisApiResultOutput.TetrisOutputType.Warning).Select(x => new { x.Key, x.Code, x.Message }));

            if (_outputs.Any(x => x.Type == TetrisApiResultOutput.TetrisOutputType.Info))
                Outputs.TryAdd("info", _outputs.Where(x => x.Type == TetrisApiResultOutput.TetrisOutputType.Info).Select(x => new { x.Key, x.Code, x.Message }));
            */

            Succeded = !Outputs.Any(x => x.Type == TetrisApiResultOutput.TetrisOutputType.Error || x.Type == TetrisApiResultOutput.TetrisOutputType.Exception);
        }

        public static async Task<TetrisApiResult> Fail(string message = null)
        {
            var result = new TetrisApiResult();
            result.LoadResultOutputs(message);

            return await Task.FromResult(result);
        }

        public static async Task<TetrisApiResult> Success(object result)
        {
            var apiResult = new TetrisApiResult();
            apiResult.Succeded = true;
            apiResult.Result = result;

            return await Task.FromResult(apiResult);
        }
    }
}
