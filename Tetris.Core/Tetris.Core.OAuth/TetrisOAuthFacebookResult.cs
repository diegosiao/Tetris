using System;
using Newtonsoft.Json;

namespace Tetris.Core.OAuth
{
    public class TetrisOAuthFacebookResult
    {
        /*
                 "app_id": "977201776114211",
        "type": "USER",
        "application": "ZipContas",
        "data_access_expires_at": 1610667672,
        "expires_at": 1602896400,
        "is_valid": true,
        "scopes": [
            "email",
            "public_profile"
        ],
        "user_id": "1269562713381547"
         */
        public bool AccessGranted => Data?.IsValid ?? false;

        public TetrisOAuthFacebookDataResult Data { get; set; }

    }

    public class TetrisOAuthFacebookDataResult
    {
        [JsonProperty("app_id")]
        public string AppId { get; set; }

        public string Type { get; set; }

        public string Application { get; set; }

        [JsonProperty("data_access_expires_at")]
        public long DataAccessExpiresAt { get; set; }

        [JsonProperty("expires_at")]
        public long ExpiresAt { get; set; }

        [JsonProperty("is_valid")]
        public bool IsValid { get; set; }

        public string[] Scopes { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("error")]
        public TetrisOAuthFacebookErrorResult Error { get; set; }
    }

    public class TetrisOAuthFacebookErrorResult
    {
        public string Message { get; set; }

        public string Code { get; set; }
    }
}
