using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Tetris.Core.OAuth
{
    /// <summary>
    /// Just a lab thing
    /// </summary>
    public static class TetrisOAuthFacebook
    {
        public static async Task<bool> CheckTokenAsync(string token)
        {
            try
            {
                using var client = new HttpClient();
                var url = TetrisSettings.FacebookCheckTokenUrl
                                        .Replace("{client_token}", token)
                                        .Replace("{access_token}", $"{TetrisSettings.FacebookAppId}|{TetrisSettings.FacebookAppSecret}");

                var response = await client.GetAsync(url);

                var body = JsonConvert.DeserializeObject<JObject>(await response.Content.ReadAsStringAsync());

                return body["error"] == null && body["data"].Value<bool>("is_valid");
            }
            catch (Exception ex)
            {
                TetrisLog.ExceptionAsync(ex);
                return false;
            }
        }
    }
}
