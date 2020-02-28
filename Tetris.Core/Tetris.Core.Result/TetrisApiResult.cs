using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Tetris.Core.Extensions;

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

        public ExpandoObject Outputs { get; set; }

        public dynamic Result
        {
            get;
            set;
        }

        public TetrisApiResult()
        {
            Outputs = new ExpandoObject();
        }

        public TetrisApiResult(ModelStateDictionary modelState)
        {
            Outputs = new ExpandoObject();

            foreach (var item in modelState)
            {
                var msg = item.Value.Errors.FirstOrDefault()?.ErrorMessage;

                if (!string.IsNullOrWhiteSpace(msg))
                    Outputs.TryAdd(item.Key.Camelize(), msg);
            }
        }

        public void LoadResultOutputs(string outputs)
        {
            if (string.IsNullOrEmpty(outputs))
                return;

            //FORMATO ESPERADO:
            //name=This name is a little weird&email=[i,C548]Google emails are preferred(gmail)&[warning]This account will be manually analysed'
            var messages = outputs.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
            
            var _outputs = messages.Select(x => new TetrisApiResultOutput(x));

            if (_outputs.Any(x => x.Type == TetrisApiResultOutput.TetrisOutputType.Exception))
                Outputs.TryAdd("exceptions", _outputs.Where(x => x.Type == TetrisApiResultOutput.TetrisOutputType.Exception).Select(x => new { x.Key, x.Code, x.Message }));

            if (_outputs.Any(x => x.Type == TetrisApiResultOutput.TetrisOutputType.Error))
                Outputs.TryAdd("errors", _outputs.Where(x => x.Type == TetrisApiResultOutput.TetrisOutputType.Error).Select(x => new { x.Key, x.Code, x.Message }));

            if (_outputs.Any(x => x.Type == TetrisApiResultOutput.TetrisOutputType.Warning))
                Outputs.TryAdd("warnings", _outputs.Where(x => x.Type == TetrisApiResultOutput.TetrisOutputType.Warning).Select(x => new { x.Key, x.Code, x.Message }));

            if (_outputs.Any(x => x.Type == TetrisApiResultOutput.TetrisOutputType.Info))
                Outputs.TryAdd("info", _outputs.Where(x => x.Type == TetrisApiResultOutput.TetrisOutputType.Info).Select(x => new { x.Key, x.Code, x.Message }));

            Succeded = !_outputs.Any(x => x.Type == TetrisApiResultOutput.TetrisOutputType.Error || x.Type == TetrisApiResultOutput.TetrisOutputType.Exception);
        }
    }
}
