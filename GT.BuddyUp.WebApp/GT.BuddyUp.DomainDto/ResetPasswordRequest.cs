using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GT.BuddyUp.DomainDto
{
    /// <summary>
    /// Request to reset the password
    /// </summary>
    public class ResetPasswordRequest
    {
        /// <summary>
        /// Email Id of the user
        /// </summary>
        [Required(ErrorMessage = "Email can not be null or empty")]
        public string email { get; set; }
        /// <summary>
        /// Current Plain text password of the user
        /// </summary>
        [Required(ErrorMessage = "Current Password can not be null or empty")]
        public string currentPassword { get; set; }

        /// <summary>
        /// New Plain text password of the user
        /// </summary>
        [Required(ErrorMessage = "New Password can not be null or empty")]
        public string newPassword { get; set; }
    }
}
