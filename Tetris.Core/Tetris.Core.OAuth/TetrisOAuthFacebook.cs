using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Tetris.Core.Tetris.Core.Application.Exceptions;

namespace Tetris.Core.OAuth
{
    public static class TetrisOAuthFacebook
    {
        public static async Task<TetrisOAuthFacebookResult> CheckTokenAsync(string token)
        {
            if (string.IsNullOrEmpty(TetrisSettings.FacebookCheckTokenUrl))
                throw new TetrisConfigurationException("Informe a configuração 'AppSettings:FacebookCheckTokenUrl' para usar esse recurso. ");

            if (string.IsNullOrEmpty(TetrisSettings.FacebookClientToken))
                throw new TetrisConfigurationException("Informe a configuração 'AppSettings:FacebookClientToken' para usar esse recurso. ");

            try
            {
                using var client = new HttpClient();
                var url = TetrisSettings.FacebookCheckTokenUrl
                                        .Replace("{user_token}", token)
                                        .Replace("{app_id}", TetrisSettings.FacebookAppId)
                                        .Replace("{client_token}", TetrisSettings.FacebookClientToken);

                var response = await client.GetAsync(url);

                var json = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<TetrisOAuthFacebookResult>(json);

                return result;
            }
            catch (Exception ex)
            {
                TetrisLog.ExceptionAsync(ex);
                return new TetrisOAuthFacebookResult();
            }
        }
    }
}
