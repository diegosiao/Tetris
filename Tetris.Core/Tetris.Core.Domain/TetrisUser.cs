using System.Security.Claims;
using System.Linq;

namespace Tetris.Core
{
    public class TetrisUser
    {
        public string IdUsuario { get; private set; }

        public string IdSessao { get; private set; }

        public string IdInquilino { get; set; }

        public string Name { get; private set; }

        public string Email { get; private set; }

        public string Phone { get; private set; }

        public bool EmailChecked { get; private set; }

        public bool PhoneChecked { get; private set; }
        
        public TetrisUser() {  }

        public TetrisUser(ClaimsPrincipal user)
        {
            IdUsuario = user?.Claims.FirstOrDefault(x => x.Type == TetrisClaims.UserId)?.Value;
            IdSessao = user?.Claims.FirstOrDefault(x => x.Type == TetrisClaims.SessionId)?.Value;
            IdInquilino = user?.Claims.FirstOrDefault(x => x.Type == TetrisClaims.TenantId)?.Value;

            Name = user?.Claims.FirstOrDefault(x => x.Type == TetrisClaims.UserName)?.Value;
            Email = user?.Claims.FirstOrDefault(x => x.Type == TetrisClaims.UserEmail)?.Value;
            Phone = user?.Claims.FirstOrDefault(x => x.Type == TetrisClaims.UserPhone)?.Value;

            EmailChecked = user?.Claims.FirstOrDefault(x => x.Type == TetrisClaims.UserEmailChecked)?.Value == "true";
            PhoneChecked = user?.Claims.FirstOrDefault(x => x.Type == TetrisClaims.UserPhoneChecked)?.Value == "true";
        }
    }
}
