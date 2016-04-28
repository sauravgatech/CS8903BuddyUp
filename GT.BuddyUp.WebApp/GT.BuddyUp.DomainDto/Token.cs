using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;

namespace GT.BuddyUp.DomainDto
{
    /// <summary>
    /// Token Response after authentication
    /// </summary>
    //public class UserPrincipal : IPrincipal
    //{
    //    /// <summary>
    //    /// Token with the prefix "GTToken "
    //    /// </summary>
    //    public string token { get; set; }
    //    /// <summary>
    //    /// Maximum Session Time in minutes
    //    /// </summary>
    //    public int maxSessionTime { get; set; }
    //    /// <summary>
    //    /// Remaining time in the session
    //    /// </summary>
    //    public int remainingTime { get; set; }
    //    /// <summary>
    //    /// Has Password expired
    //    /// </summary>
    //    public bool passwordExpired { get; set; }
    //    /// <summary>
    //    /// Message to the user
    //    /// </summary>
    //    public string message { get; set; }

    //    /// <summary>
    //    /// User Details 
    //    /// </summary>
    //    public UserGetResponse user { get; set; }

    //    /// <summary>
    //    /// User Identity
    //    /// </summary>
    //    //public IIdentity Identity
    //    //{
    //    //    get { return new UserIdentity() { Name = user.firstName + " " + user.lastName, IsAuthenticated = true }; }
    //    //}

    //    //public bool IsInRole(string role)
    //    //{
    //    //    if (this.user == null || this.user.UserCourseDetails == null || this.user.UserCourseDetails.Count == 0)
    //    //        return false;
    //    //    return this.user.UserCourseDetails.Any(x => x.RoleCode.Equals(role, StringComparison.OrdinalIgnoreCase));
    //    //}

    //    //public class UserIdentity : IIdentity
    //    //{
    //    //    public string AuthenticationType { get { return "GTAuthentication"; } }
    //    //    public string Name { get; protected set; }
    //    //    public bool IsAuthenticated { get; protected set; }
    //    //}
    //}
}
