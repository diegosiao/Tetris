using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Tetris.OAuth
{
    /// <summary>
    /// Just a lab thing
    /// </summary>
    public static class TetrisOAuthGoogle
    {
        public static async Task<bool> CheckTokenAsync(string token)
        {
            try
            {
                using var client = new HttpClient();
                var url = TetrisSettings.GoogleCheckTokenUrl
                                        .Replace("{client_token}", token);

                var response = await client.GetAsync(url);

                var body = JsonConvert.DeserializeObject<JObject>(await response.Content.ReadAsStringAsync());
                
                return body.TryGetValue("email", out JToken value);
            }
            catch (Exception ex)
            {
                TetrisLog.ExceptionAsync(ex);
                return false;
            }
        }
    }
}
