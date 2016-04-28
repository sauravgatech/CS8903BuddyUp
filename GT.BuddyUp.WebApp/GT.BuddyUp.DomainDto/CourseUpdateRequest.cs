using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GT.BuddyUp.DomainDto
{
    /// <summary>
    /// Course update request
    /// </summary>
    public class CourseUpdateRequest
    {
        /// <summary>
        /// Course Code
        /// </summary>
        [Required(ErrorMessage = "Course Code is required"), StringLength(24, ErrorMessage = "Course Code can not be greater than 24 characters.")]
        public string CourseCode { get; set; }

        /// <summary>
        /// Course Name
        /// </summary>
        [StringLength(128, ErrorMessage = "CourseName can not be greater than 128 characters.")]
        public string CourseName { get; set; }
        
        /// <summary>
        /// Course Description
        /// </summary>
        [StringLength(512, ErrorMessage = "CourseDescription can not be greater than 512 characters.")]
        public string CourseDescription { get; set; }
        /// <summary>
        /// Questionnaire Code
        /// </summary>
        public string QuestionnaireCode { get; set; }
        
        /// <summary>
        /// Group Type
        /// </summary>
        public string GroupType { get; set; }
        /// <summary>
        /// Updated Roaster
        /// </summary>
        public string Roaster { get; set; }
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
        /// List of new users to be added
        /// </summary>
        public List<CourseUpdateUser> CourseNewUsers { get; set; }

        /// <summary>
        /// List of users to be removed
        /// </summary>
        public List<CourseUpdateUser> CourseDeleteUsers { get; set; }
    }

    public class CourseUpdateUser
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
