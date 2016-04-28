using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GT.BuddyUp.DomainDto
{
    /// <summary>
    /// Course Get Response
    /// </summary>
    public class CourseGetResponse
    {
        /// <summary>
        /// Course Code
        /// </summary>
        public string CourseCode { get; set; }

        /// <summary>
        /// Course Name
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// Course Description
        /// </summary>
        public string CourseDescription { get; set; }

        /// <summary>
        /// Group Type
        /// </summary>
        public string GroupType { get; set; }
        /// <summary>
        /// Group Size
        /// </summary>
        public int? GroupSize { get; set; }
        /// <summary>
        /// Comma Separated Skill Sets
        /// </summary>
        public string DesiredSkillSets { get; set; }
        /// <summary>
        /// For group formation if similar skill sets should group together
        /// </summary>
        public bool? PreferSimiliarSkillSet { get; set; }

        /// <summary>
        /// Questionnaire
        /// </summary>
       public string QuestionnaireCode { get; set; }
        /// <summary>
        /// Class Roaster
        /// </summary>
       public string Roaster { get; set; }

       public string Term { get; set; }
        /// <summary>
        /// List of users
        /// </summary>
       public List<CourseUser> UserList { get; set; }

       public List<CourseGroups> CourseGroups { get; set; }
       
    }

    public class CourseUser
    {
        public string EmailID { get; set; }
        public string Name { get; set; }
        public string RoleCode { get; set; }
    }

    public class CourseForUser
    {
        public string CourseCode {get; set;}
        public string CourseNane {get; set;}
        public string Term {get; set;}
    }
    public class UserCourseGetReponse
    {
        public List<CourseForUser> RegisteredCourses { get; set; }
        public List<CourseForUser> EligibleToRegisterCourses { get; set; }
    }


    public class CourseGroups
    {
        public string GroupCode { get; set; }

        public string GroupName { get; set; }

        public string TimeZone { get; set; }

        public string Objective { get; set; }

        public List<CourseUser> GroupUsers { get; set; }
    }
}
