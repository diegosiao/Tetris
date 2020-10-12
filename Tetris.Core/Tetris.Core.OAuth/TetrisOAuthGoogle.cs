using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Tetris.Core.Tetris.Core.Application.Exceptions;
using Tetris.Core.Tetris.Core.OAuth;

namespace Tetris.Core.OAuth
{
    public static class TetrisOAuthGoogle
    {
        public static async Task<TetrisOAuthGoogleResult> CheckTokenAsync(string token)
        {
            if (string.IsNullOrEmpty(TetrisSettings.GoogleCheckTokenUrl))
                throw new TetrisConfigurationException("Informe a configuração 'AppSettings:GoogleCheckTokenUrl' para usar esse recurso. ");

            try
            {
                using var client = new HttpClient();
                var url = TetrisSettings.GoogleCheckTokenUrl
                                        .Replace("{client_token}", token);

                var response = await client.GetAsync(url);

                var json = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<TetrisOAuthGoogleResult>(json);

                return result;
            }
            catch (Exception ex)
            {
                TetrisLog.ExceptionAsync(ex);
                return new TetrisOAuthGoogleResult { ErrorDescription = ex.Message };
            }
        }
    }
}
