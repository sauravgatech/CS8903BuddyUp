using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GT.BuddyUp.DomainDto
{
    /// <summary>
    /// Course User Add Request
    /// </summary>
    public class CourseUserUpdateRequest
    {
        /// <summary>
        /// Course Code
        /// </summary>
        [Required]
        public string courseCode { get; set; }

        /// <summary>
        /// Email Id
        /// </summary>
        [Required]
        public string email { get; set; }
        /// <summary>
        /// Role Code
        /// </summary>
        [Required]
        public string RoleCode { get; set; }

        /// <summary>
        /// Answer Set
        /// </summary>
        public string answerSet { get; set; }

        /// <summary>
        /// Group Code
        /// </summary>
        public string GroupCode { get; set; }
    }
}
