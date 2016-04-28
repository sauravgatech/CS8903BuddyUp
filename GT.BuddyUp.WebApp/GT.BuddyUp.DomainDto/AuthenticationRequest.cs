using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GT.BuddyUp.DomainDto
{
    /// <summary>
    /// Authrntication Request
    /// </summary>
    public class AuthenticationRequest
    {
        /// <summary>
        /// Email Id of the user
        /// </summary>
        [Required(ErrorMessage = "Email can not be null or empty")]
        public string email { get; set; }
        /// <summary>
        /// Plain text password of the user
        /// </summary>
        [Required(ErrorMessage = "Password can not be null or empty")]
        public string password { get; set; }
    }
}
