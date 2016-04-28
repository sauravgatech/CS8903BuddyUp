using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GT.BuddyUp.DomainDto;
using GT.BuddyUp.Platform.Common;
using GT.BuddyUp.Platform.Repository;
using GT.BuddyUp.EntityModel;
using Microsoft.Practices.Unity;
using System.Security.Cryptography;

namespace GT.BuddyUp.DomainModel
{
    public class UserMaintenance : IUser
    {
        private IUnitOfWork _uow;
        private IRepository<AspNetUsers> _repUser;
        private IRepository<AspNetRoles> _repRole;
        private IRepository<AspNetUserRoles> _repUserRoles;
        private IRepository<EntityModel.CourseUser> _repCourseUserRole;
        private IRepository<Course> _repCourse;
        IRepository<Group> _repGroup;

        private DomainModelResponse _securityResponse = new DomainModelResponse();

        public UserMaintenance([Dependency("buddyup")] IUnitOfWork uow)
        {
            _uow = uow;
        }

        [InjectionMethod]
        public void Initialize(IRepository<AspNetUsers> repUser,
                                IRepository<AspNetRoles> repRole,
                                IRepository<EntityModel.CourseUser> repCourseUserRole,
                                IRepository<Course> repCourse,
                                IRepository<Group> repGroup,
                                IRepository<AspNetUserRoles> repUserRoles)
        {
            _repUser = repUser.Use(_uow);
            _repRole = repRole.Use(_uow);
            _repCourseUserRole = repCourseUserRole.Use(_uow);
            _repCourse = repCourse.Use(_uow);
            _repGroup = repGroup.Use(_uow);
            _repUserRoles = repUserRoles.Use(_uow);
        }

        public IEnumerable<UserGetResponse> Get(string emailId = "")
        {
            IEnumerable<AspNetUsers> userProfiles = _repUser.Get(filter: u => (u.Email == emailId || emailId == ""), includes: "CourseUsers");
            List<UserGetResponse> userResponses = new List<UserGetResponse>();
            foreach (AspNetUsers up in userProfiles)
            {
                UserGetResponse ur = new UserGetResponse()
                {
                    firstName = up.FirstName,
                    lastName = up.LastName,
                    emailId = up.Email,
                    GTID = up.GTID
                };
                if (up.CourseUsers != null)
                {
                    ur.UserCourseDetails = new List<UserCourseDetail>();
                    foreach (var cur in up.CourseUsers)
                    {
                        UserCourseDetail ucd = new UserCourseDetail()
                        {
                            courseCode = cur.Course.CourseCode,
                            CourseName = cur.Course.CourseName,
                        };
                        ur.UserCourseDetails.Add(ucd);
                    }
                }
                userResponses.Add(ur);
            }
            return userResponses;
        }

        //public DomainModelResponse Add(UserAddRequest request)
        //{
        //    EntityModel.Role role = _repRole.Get(filter: f => f.RoleCode == request.RoleCode).FirstOrDefault();

        //    UserProfile up = new UserProfile()
        //    {
        //        EmailId = request.emailId,
        //        FirstName = request.firstName,
        //        LastName = request.lastName,
        //        HashedPassword = createHash(request.password),
        //        SecurityQuestion = request.securityQuestion,
        //        HashedAnswer = createHash(request.answer),
        //        isAdmin = request.isAdmin,
        //        LastChangedTime = DateTime.UtcNow,
        //        LastPasswordChangeDate = DateTime.UtcNow,
        //        PasswordExpired = false,
        //        CourseUserRoles = null
        //    };
        //    _repUser.Add(up);
        //    _uow.Commit();
        //    AddUserToCourse(new UpdateUserCourse() { courseCode = "Default", emailId = request.emailId, RoleCode = request.RoleCode });
        //    _securityResponse.addResponse("Add", MessageCodes.InfoCreatedSuccessfully, "User");
        //    return _securityResponse;
        //}

        private string createHash(string plainTextPassword)
        {
            SHA256 mySHA256 = SHA256Managed.Create();
            byte[] b = UnicodeEncoding.Unicode.GetBytes(plainTextPassword);
            byte[] h = mySHA256.ComputeHash(b, 0, b.Length);
            return Convert.ToBase64String(h);
        }

        public DomainModelResponse Update(UserUpdateRequest request)
        {
            AspNetUsers up = _repUser.Get(filter: f => f.Email == request.emailId).FirstOrDefault();
            if(up == null)
            {
                _securityResponse.addResponse("Update", MessageCodes.ErrDoesnotExist, "User : " + request.emailId);
                throw _securityResponse;
            }

            if (!String.IsNullOrWhiteSpace(request.firstName))
                up.FirstName = request.firstName;
            if (!String.IsNullOrWhiteSpace(request.lastName))
                up.LastName = request.lastName;
            if (!String.IsNullOrWhiteSpace(request.GTID))
                up.GTID = request.GTID;
            _repUser.Update(up);
            _uow.Commit();
            _securityResponse.addResponse("Update", MessageCodes.InfoSavedSuccessfully, "User");
            return _securityResponse;
        }


        //public DomainModelResponse AddUserToCourse(UpdateUserCourse request)
        //{
        //    UserProfile up = _repUser.Get(filter: f => f.EmailId == request.emailId).FirstOrDefault();
        //    if(up == null)
        //    {
        //        _securityResponse.addResponse("AddUserToCourse", MessageCodes.ErrDoesnotExist, "User : " + request.emailId);
        //        throw _securityResponse;
        //    }
        //    CourseUserRole cur = new CourseUserRole();
        //    cur.UserId = up.UserId;
        //    cur.UserProfile = up;
        //    GT.CS6460.BuddyUp.EntityModel.Role role = _repRole.Get(filter: f => f.RoleCode == request.RoleCode).FirstOrDefault();
        //    if (role == null)
        //    {
        //        _securityResponse.addResponse("AddUserToCourse", MessageCodes.ErrDoesnotExist, "Role : " + request.RoleCode);
        //        throw _securityResponse;
        //    }
            
        //    cur.RoleId = role.RoleId;
        //    cur.Role = role;
        //    Course course = _repCourse.Get(filter: f => f.CourseCode == request.courseCode).FirstOrDefault();
        //    if(course == null)
        //    {
        //        _securityResponse.addResponse("AddUserToCourse", MessageCodes.ErrDoesnotExist, "Course : " + request.courseCode);
        //        throw _securityResponse;
        //    }
        //    cur.CourseId = course.CourseId;
        //    cur.Course = course;
        //    _repCourseUserRole.Add(cur);
        //    _uow.Commit();
        //    _securityResponse.addResponse("AddUserToCourse", MessageCodes.InfoCreatedSuccessfully, "CourseUserRole for user : " + request.emailId);
        //    return _securityResponse;
        //}

        public DomainModelResponse AddUserToGroup(UpdateUserGroup request)
        {
            AspNetUsers up = _repUser.Get(filter: f => f.Email == request.emailId).FirstOrDefault();
            if (up == null)
            {
                _securityResponse.addResponse("AddUserToGroup", MessageCodes.ErrDoesnotExist, "User : " + request.emailId);
                throw _securityResponse;
            }

            Course course = _repCourse.Get(filter: f => f.CourseCode == request.courseCode).FirstOrDefault();
            if (course == null)
            {
                _securityResponse.addResponse("AddUserToGroup", MessageCodes.ErrDoesnotExist, "Course : " + request.courseCode);
                throw _securityResponse;
            }

            Group group = _repGroup.Get(filter: f => f.GroupCode == request.GroupCode).FirstOrDefault();
            if (group == null)
            {
                _securityResponse.addResponse("AddUserToGroup", MessageCodes.ErrDoesnotExist, "Group : " + request.GroupCode);
                throw _securityResponse;
            }

            EntityModel.CourseUser cur = _repCourseUserRole.Get(filter: f => f.UserId == up.Id && f.CourseId == course.CourseId).FirstOrDefault();
            if (cur == null)
            {
                _securityResponse.addResponse("AddUserToGroup", MessageCodes.ErrDoesnotExist, "CourseUserRole for User : " + request.emailId);
                throw _securityResponse;
            }

            cur.GroupId = group.GroupId;
            cur.Group = group;
            _repCourseUserRole.Update(cur);
            _uow.Commit();
            _securityResponse.addResponse("AddUserToGroup", MessageCodes.InfoSavedSuccessfully, "CourseUserRole for user : " + request.emailId);
            return _securityResponse;
        }


        public DomainModelResponse RemoveUserFromGroup(UpdateUserGroup request)
        {
            AspNetUsers up = _repUser.Get(filter: f => f.Email == request.emailId).FirstOrDefault();
            if (up == null)
            {
                _securityResponse.addResponse("RemoveUserFromGroup", MessageCodes.ErrDoesnotExist, "User : " + request.emailId);
                throw _securityResponse;
            }

            Course course = _repCourse.Get(filter: f => f.CourseCode == request.courseCode).FirstOrDefault();
            if (course == null)
            {
                _securityResponse.addResponse("RemoveUserFromGroup", MessageCodes.ErrDoesnotExist, "Course : " + request.courseCode);
                throw _securityResponse;
            }

            EntityModel.CourseUser cur = _repCourseUserRole.Get(filter: f => f.UserId == up.Id && f.CourseId == course.CourseId).FirstOrDefault();
            if (cur == null)
            {
                _securityResponse.addResponse("RemoveUserFromGroup", MessageCodes.ErrDoesnotExist, "CourseUserRole for User : " + request.emailId);
                throw _securityResponse;
            }

            cur.GroupId = null;
            cur.Group = null;
            _repCourseUserRole.Update(cur);
            _uow.Commit();
            _securityResponse.addResponse("RemoveUserFromGroup", MessageCodes.InfoSavedSuccessfully, "CourseUserRole for user : " + request.emailId);
            return _securityResponse;
        }

        //public DomainModelResponse Delete(string emailId)
        //{
        //    return new DomainModelResponse();
        //}


        public string GetUserRole(string email)
        {
            AspNetUsers user = _repUser.Get(filter: x => x.Email == email).FirstOrDefault();
            if(user!=null)
            {
                AspNetUserRoles role = _repUserRoles.Get(filter: x => x.UserId == user.Id).FirstOrDefault();
                if (role != null)
                    return role.RoleId;
            }
            return null;
        }
    }
}
