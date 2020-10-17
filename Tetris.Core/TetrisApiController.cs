using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Tetris.Core.Result;
using Tetris.Core.Data;
using Tetris.Core.Domain;

namespace Tetris.Core
{
    public class TetrisApiController : Controller
    {
        new public TetrisUser User => new TetrisUser(base.User);

        protected async Task<TetrisApiResult> Ping()
        {
            var result = new TetrisApiResult();
            result.Outputs.Add(new TetrisApiResultOutput($"pong=[i]{GetType().Name}"));
            
            return await Task.FromResult(result);
        }

        new protected async Task<TetrisApiResult> Unauthorized()
        {
            var result = new TetrisApiResult();
            result.Outputs.Add(new TetrisApiResultOutput("Desculpe, acesso negado"));
            
            return await Task.FromResult(result);
        }


        protected async Task<HttpResponseMessage> HttpNotFound()
        {
            var response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotFound
            };

            return await Task.FromResult(response);
        }

        protected async Task<HttpResponseMessage> JsonResponse(TetrisApiResult result)
        {
            var response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(result))
            };

            return await Task.FromResult(response);
        }

        public async Task<HttpResponseMessage> CreateImageResponse(byte[] content, string mimeType)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(new MemoryStream(content))
            };
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(mimeType);

            return await Task.FromResult(response);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            foreach(var arg in context.ActionArguments)
            {
                if(arg.Value is TetrisExecutableBase executable) 
                    executable.Controller = this;
            }
        }
    }
}
