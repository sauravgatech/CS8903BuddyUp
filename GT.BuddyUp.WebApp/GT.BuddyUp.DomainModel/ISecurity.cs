using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GT.BuddyUp.DomainDto;
using GT.BuddyUp.Platform.Common;

namespace GT.BuddyUp.DomainModel
{
    public interface ISecurity : IDisposable
    {
        UserPrincipal Login(AuthenticationRequest AuthenticationRequest);

        UserPrincipal ValidateToken(string token);

        DomainModelResponse Logout(string token);

        DomainModelResponse ResetPassword(AuthenticationRequest AuthenticationRequest, string newPassword);

        DomainModelResponse ResetPassword(AuthenticationRequest AuthenticationRequest, string newPassword, bool resetByAdmin);
    }
}
