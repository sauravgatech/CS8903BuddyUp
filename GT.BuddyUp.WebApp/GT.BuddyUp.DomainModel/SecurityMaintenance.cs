using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using System.Security.Cryptography;
using System.Configuration;
using System.Text;
using GT.BuddyUp.Platform.Common;
using GT.BuddyUp.Platform.Repository;
using GT.BuddyUp.EntityModel;
using GT.BuddyUp.DomainDto;

namespace GT.BuddyUp.DomainModel
{
    public class SecurityMaintenance : ISecurity
    {
        private IUnitOfWork _uow;
        private IRepository<UserProfile> _repUser;
        private IRepository<SessionToken> _repSessionToken;

        private const string _tokenPrefix = "GTToken ";

        private DomainModelResponse _domainModelResponse = new DomainModelResponse();

        public SecurityMaintenance([Dependency("buddyup")] IUnitOfWork uow)
        {
            _uow = uow;
        }

        [InjectionMethod]
        public void Initialize(IRepository<UserProfile> repUser,
            IRepository<SessionToken> repSessionToken)
        {
            _repUser = repUser.Use(_uow);
            _repSessionToken = repSessionToken.Use(_uow);
        }

        public UserPrincipal Login(AuthenticationRequest AuthenticationRequest)
        {

            UserProfile up = _repUser.Get(filter: u => u.EmailId == AuthenticationRequest.email).FirstOrDefault();

            if (null == up)
            {
                _domainModelResponse.addResponse("Login", MessageCodes.ErrDoesnotExist, "User : " + AuthenticationRequest.email);
                throw _domainModelResponse;
            }

            if (up.LastPasswordChangeDate < DateTime.UtcNow.AddDays(-90)) //Password Expired, send a password expired token
            {
                return InternalCreateToken(up, true);
            }

            if (!IsCorrectPassword(up.HashedPassword, AuthenticationRequest.password))
            {
                _domainModelResponse.addResponse("Login", MessageCodes.ErrValidationFailed, "Password mismatch.");
                throw _domainModelResponse;
            }

            return InternalCreateToken(up);
        }


        private UserPrincipal InternalCreateToken(UserProfile up, bool isPasswordExpire = false)
        {
            DateTime dtNow = DateTime.UtcNow;
            string sToken = String.Format("{0}:{1}", Guid.NewGuid().ToString(), dtNow.AddHours(3.0).ToString("yyyyMMddHHmmss"));
            sToken = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(sToken));
            UserPrincipal t = new UserPrincipal()
            {
                token = String.Concat(_tokenPrefix, sToken),
                maxSessionTime = isPasswordExpire ? 30 : 120,
                remainingTime = isPasswordExpire ? 30 : 120,
                passwordExpired = isPasswordExpire,
                message = isPasswordExpire ? "Password expired. Use this token to reset password." : null,
                user = new UserGetResponse()
                {
                    emailId = up.EmailId,
                    firstName = up.FirstName,
                    lastName = up.LastName,
                    isAdmin = (up.isAdmin == null) ? false : (bool)up.isAdmin,
                    UserCourseDetails = mapCourseDetails(up.CourseUserRoles)
                }
            };

            _repSessionToken.Add(new SessionToken
            {
                Token = t.token,
                CreationTimeUtc = DateTime.UtcNow,
                HasPasswordExpired = isPasswordExpire,
                LastActivityTimeUtc = DateTime.UtcNow,
                UserId = up.UserId,
                User = up,
                UserName = up.EmailId
            });
            _uow.Commit();
            return t;
        }

        private List<UserCourseDetail> mapCourseDetails(ICollection<CourseUserRole> courseUserRoles)
        {
            List<UserCourseDetail> userCourseDetails = new List<UserCourseDetail>();
            foreach (CourseUserRole courseUserRole in courseUserRoles)
            {
                UserCourseDetail ucd = new UserCourseDetail()
                {
                    courseCode = courseUserRole.Course.CourseCode,
                    CourseName = courseUserRole.Course.CourseName,
                    CourseDescription = courseUserRole.Course.CourseDescription,
                    RoleCode = courseUserRole.Role.RoleCode,
                    GroupCode = courseUserRole.Group != null ? courseUserRole.Group.GroupCode : null,
                    GroupName = courseUserRole.Group != null ? courseUserRole.Group.GroupName : null,
                    Objective = courseUserRole.Group != null ? courseUserRole.Group.Objective : null
                };
                userCourseDetails.Add(ucd);
            }
            return userCourseDetails;
        }

        private bool IsCorrectPassword(string userHashedPassword, string enteredPlainTextPassword)
        {
            if (string.Equals(userHashedPassword, CreateHash(enteredPlainTextPassword)))
                return true;
            return false;
        }

        private string CreateHash(string plainTextPassword)
        {
            SHA256 mySHA256 = SHA256Managed.Create();
            byte[] b = UnicodeEncoding.Unicode.GetBytes(plainTextPassword);
            byte[] h = mySHA256.ComputeHash(b, 0, b.Length);
            return Convert.ToBase64String(h);
        }

        public UserPrincipal ValidateToken(string token)
        {
            return null;
        }

        public DomainModelResponse Logout(string token)
        {
            return _domainModelResponse;
        }

        public DomainModelResponse ResetPassword(AuthenticationRequest AuthenticationRequest, string newPassword)
        {
            return _domainModelResponse;
        }

        public DomainModelResponse ResetPassword(AuthenticationRequest AuthenticationRequest, string newPassword, bool resetByAdmin)
        {
            return _domainModelResponse;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_uow != null)
            {
                _uow.Dispose();
                _uow = null;
            }
        }
    }
}
