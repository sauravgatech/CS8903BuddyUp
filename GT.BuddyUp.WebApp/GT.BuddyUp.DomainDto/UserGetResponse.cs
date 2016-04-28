using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace GT.BuddyUp.DomainDto
{
    /// <summary>
    /// Response when Get User API is called
    /// </summary>
    public class UserGetResponse : UserIdentity
    {
        /// <summary>
        /// Email Id of user
        /// </summary>
        public string emailId { get; set; }

        /// <summary>
        /// First Name of User
        /// </summary>
        public string firstName { get; set; }

        /// <summary>
        /// Last Name of User
        /// </summary>
        public string lastName { get; set; }

        /// <summary>
        /// Indicates if user is admin
        /// </summary>
        public string GTID { get; set; }

        /// <summary>
        /// Course details of user
        /// </summary>
        public List<UserCourseDetail> UserCourseDetails { get; set; }

    }

    /// <summary>
    /// Course Details of User
    /// </summary>
    public class UserCourseDetail
    {
        /// <summary>
        /// Code of course
        /// </summary>
        public string courseCode { get; set; }

        /// <summary>
        /// Name of Course
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// Course Description
        /// </summary>
        public string CourseDescription { get; set; }

        /// <summary>
        /// Role of user in the course
        /// </summary>
        //public string RoleCode { get; set; }

        /// <summary>
        /// Description of the role
        /// </summary>
        //public string RoleDescription { get; set; }

        /// <summary>
        /// Group Code
        /// </summary>
        public string GroupCode { get; set; }

        /// <summary>
        /// Group Name
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Group Objective
        /// </summary>
        public string Objective { get; set; }
    }


    public class UserIdentity : IIdentity
    {
        public string AuthenticationType { get { return "GTAuthentication"; } }
        public string Name { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}
