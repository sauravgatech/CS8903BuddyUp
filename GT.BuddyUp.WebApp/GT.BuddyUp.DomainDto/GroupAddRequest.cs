using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GT.BuddyUp.DomainDto
{
    public class GroupAddRequest
    {
        /// <summary>
        /// Group Code
        /// </summary>
        [Required, StringLength(24, ErrorMessage= "GroupCode can not be greater than 24 characters")]
        public string GroupCode { get; set; }

        /// <summary>
        /// Name of Group
        /// </summary>
        [Required, StringLength(128, ErrorMessage = "GroupName can not be greater than 128 characters")]
        public string GroupName { get; set; }

        /// <summary>
        /// Group Type Code
        /// </summary>
        [Required, StringLength(24, ErrorMessage = "GroupTypeCode can not be greater than 24 characters")]
        public string GroupTypeCode { get; set; }

        /// <summary>
        /// Group Objective
        /// </summary>
        [Required, StringLength(2048, ErrorMessage = "Objective can not be greater than 2048 characters")]
        public string Objective { get; set; }

        /// <summary>
        /// Timezone in which group would be most active
        /// </summary>
        [Required, StringLength(128, ErrorMessage = "TimeZone can not be greater than 24 characters")]
        public string TimeZone { get; set; }

        /// <summary>
        /// Course in which group exist
        /// </summary>
        [Required, StringLength(24, ErrorMessage = "CourseCode can not be greater than 24 characters")]
        public string CourseCode { get; set; }

        /// <summary>
        /// List of users of group
        /// </summary>
        public List<string> userList { get; set; }
    }
}
