using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GT.BuddyUp.DomainDto
{
    /// <summary>
    /// Request DTO to add user
    /// </summary>
    public class UserAddRequest
    {
        /// <summary>
        /// Email Id of the user
        /// </summary>
        [Required(ErrorMessage = "Email Id can not be null or empty"), StringLength(128, ErrorMessage="Email ID can not be greater than 128 characters")]
        public string emailId { get; set; }

        /// <summary>
        /// Password of the user
        /// </summary>
        [Required(ErrorMessage = "Password can not be null or empty"), StringLength(32, ErrorMessage = "Password can not be greater than 32 characters")]
        public string password { get; set; }

        /// <summary>
        /// Security Question
        /// </summary>
        [Required(ErrorMessage = "SecurityQuestion can not be null or empty"), StringLength(128, ErrorMessage = "SecurityQuestion can not be greater than 128 characters")]
        public string securityQuestion { get; set; }

        /// <summary>
        /// Answer to security question
        /// </summary>
        [Required(ErrorMessage = "Answer can not be null or empty"), StringLength(128, ErrorMessage = "Answer can not be greater than 128 characters")]
        public string answer { get; set; }

        /// <summary>
        /// First Name of user
        /// </summary>
        [Required(ErrorMessage = "firstName can not be null or empty"), StringLength(64, ErrorMessage = "firstName can not be greater than 64 characters")]
        public string firstName { get; set; }

        /// <summary>
        /// Last name of user
        /// </summary>
        [Required(ErrorMessage = "lastName can not be null or empty"), StringLength(128, ErrorMessage = "lastName can not be greater than 64 characters")]
        public string lastName { get; set; }

        /// <summary>
        /// Is user an admin
        /// </summary>
        public bool? isAdmin { get; set; }

        /// <summary>
        /// Role Code of User
        /// </summary>
        public string RoleCode { get; set; }
    }
}
