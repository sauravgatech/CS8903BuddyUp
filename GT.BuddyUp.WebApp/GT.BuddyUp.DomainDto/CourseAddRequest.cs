using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GT.BuddyUp.DomainDto
{
    /// <summary>
    /// Course Add Request
    /// </summary>
    public class CourseAddRequest
    {
        /// <summary>
        /// Course Code
        /// </summary>
        [Required(ErrorMessage = "Course Code is required"), StringLength(24, ErrorMessage = "Course Code can not be greater than 24 characters.")]
        public string CourseCode { get; set; }

        /// <summary>
        /// Name of Course
        /// </summary>
        [Required(ErrorMessage = "CourseName is required"), StringLength(128, ErrorMessage = "CourseName can not be greater than 128 characters.")]
        public string CourseName { get; set; }
        /// <summary>
        /// Course Description
        /// </summary>
        [Required(ErrorMessage = "CourseDescription is required"), StringLength(512, ErrorMessage = "CourseDescription can not be greater than 512 characters.")]
        public string CourseDescription { get; set; }
        /// <summary>
        /// Group Type
        /// </summary>
        public string GroupType { get; set; }
        /// <summary>
        /// Group Size
        /// </summary>
        public int? GroupSize { get; set; }

        public string Term { get; set; }
        /// <summary>
        /// Comma Separated Skill Sets
        /// </summary>
        public string DesiredSkillSets { get; set; }
        /// <summary>
        /// For group formation if similar skill sets should group together
        /// </summary>
        public bool? PreferSimiliarSkillSet { get; set; }
        /// <summary>
        /// Initial Set of User; List of email id
        /// </summary>
        public List<CourseNewUser> userList { get; set; }
    }

    /// <summary>
    /// Users for the new Course
    /// </summary>
    public class CourseNewUser
    {
        /// <summary>
        /// Users Email Id
        /// </summary>
        public string emailId { get; set; }

        /// <summary>
        /// User Role in the course
        /// </summary>
        public string roleCode { get; set; }
    }
}
