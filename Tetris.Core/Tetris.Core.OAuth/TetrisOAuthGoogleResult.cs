using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Tetris.Core.Tetris.Core.OAuth
{

    /*
     SUCCESS
     {
        "azp": "93614072716-i51l4ccdvohro1qvm7otncfgmn7pooik.apps.googleusercontent.com",
        "aud": "93614072716-i51l4ccdvohro1qvm7otncfgmn7pooik.apps.googleusercontent.com",
        "sub": "115554328711656778802",
        "scope": "https://www.googleapis.com/auth/userinfo.email https://www.googleapis.com/auth/userinfo.profile openid",
        "exp": "1602352007",
        "expires_in": "2590",
        "email": "diegosiao@gmail.com",
        "email_verified": "true",
        "access_type": "online"
     }


     ERROR
    {
        "error_description": "Invalid Value"
    }
     */

    public class TetrisOAuthGoogleResult
    {
        public bool AccessGranted => string.IsNullOrEmpty(ErrorDescription);

        public string Azp { get; set; }

        public string Aud { get; set; }

        public string Sub { get; set; }

        public string Scope { get; set; }

        public long Exp { get; set; }

        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }

        public string Email { get; set; }

        [JsonProperty("email_verified")]
        public bool EmailVerified { get; set; }

        [JsonProperty("access_type")]
        public string Acess_Type { get; set; }

        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; }
    }
}
