using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GT.BuddyUp.DomainDto
{
    public class GroupGetResponse
    {
        /// <summary>
        /// Group Code
        /// </summary>
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
        /// Course in which group exist
        /// </summary>
        public string CourseCode { get; set; }

        /// <summary>
        /// List of users of group
        /// </summary>
        public List<GroupUser> UserList { get; set; }
        /// <summary>
        /// Posts in the Group
        /// </summary>
        public List<Post> GroupPosts { get; set; }
    }

    public class GroupUser
    {
        public string emailId {get; set;}
        public string name { get; set; }
    }

    public class Post
    {
        public string PostText { get; set; }

        public string UserName { get; set; }

        public DateTime TimePosted { get; set; }

        public List<Post> ChildPosts { get; set; }
    }
}
