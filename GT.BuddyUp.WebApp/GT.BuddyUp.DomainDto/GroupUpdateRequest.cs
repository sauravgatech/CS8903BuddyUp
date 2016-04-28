using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GT.BuddyUp.DomainDto
{
    public class GroupUpdateRequest
    {
        /// <summary>
        /// Group Code
        /// </summary>
        [Required, StringLength(24, ErrorMessage = "GroupCode can not be greater than 24 characters")]
        public string GroupCode { get; set; }

        /// <summary>
        /// Name of Group
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Group Type Code
        /// </summary>
        public string GroupTypeCode { get; set; }

        /// <summary>
        /// Group Objective
        /// </summary>
        public string Objective { get; set; }

        /// <summary>
        /// Timezone in which group would be most active
        /// </summary>
        public string TimeZone { get; set; }

        /// <summary>
        /// List of new users of group
        /// </summary>
        public List<string> newUserList { get; set; }
        /// <summary>
        /// List of users to be removed from group
        /// </summary>
        public List<string> removeUserList { get; set; }
    }
}
