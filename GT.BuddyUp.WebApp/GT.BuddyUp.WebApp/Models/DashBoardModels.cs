using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GT.BuddyUp.WebApp.Models
{
    public enum GroupType
    {
        StudyGroup,
        ProjectGroup
    }
    public class GroupDetailModel
    {
        [Display(Name="Group Code")]
        public string GroupCode { get; set; }

        [Display(Name = "Group Name")]
        public string GroupName { get; set; }

        [Display(Name = "Group Type Code")]
        public string GroupTypeCode { get; set; }

        [Display(Name = "Group Objective / Summary")]
        public string Objective { get; set; }

        [Display(Name = "Most Active Timezone")]
        public string TimeZone { get; set; }

        [Display(Name = "Course Code")]
        public string CourseCode { get; set; }

        [Display(Name = "List of Users")]
        public List<GroupUserModel> UserList { get; set; }
        
        [Display(Name = "Group Posts")]
        public List<PostModel> GroupPosts { get; set; }
    }

    public class TeacherDashboardModel
    {
        public Dictionary<string, string> Courses { get; set; }
        public Dictionary<string, string> CourseDescription { get; set; }
    }

    public class GroupUserModel
    {
        [Display(Name = "Email Id")]
        public string emailId { get; set; }
        [Display(Name = "Name")]
        public string name { get; set; }
    }

    public class PostModel
    {
        public string PostText { get; set; }

        public string UserName { get; set; }

        public DateTime TimePosted { get; set; }

        public List<PostModel> ChildPosts { get; set; }
    }

    public class GroupSummary
    {
        [Key, Required]
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
        public string TimeZone { get; set; }
        public string Objective { get; set; }

    }
    public class GroupListModel
    {
        public string CourseCode { get; set; }
        public GroupSummary registeredGroup { get; set; }
        public List<GroupSummary> SuggestedGroups { get; set; }
        [Key, Required]
        public List<GroupSummary> AllGroups { get; set; }
    }


    public class GroupCreateModel
    {
        [Display(Name = "Group Name")]
        [Required, StringLength(128, ErrorMessage = "GroupName can not be greater than 128 characters")]
        public string GroupName { get; set; }

        [Display(Name = "Group Type Code")]
        [Required, StringLength(24, ErrorMessage = "GroupTypeCode can not be greater than 24 characters")]
        public string GroupTypeCode { get; set; }

        [Display(Name = "Group Objective /Summary")]
        [Required, StringLength(2048, ErrorMessage = "Objective can not be greater than 2048 characters")]
        public string Objective { get; set; }

        public List<string> timeZones { get; set; }

        [Display(Name = "Most Active Timezone of Group")]
        [Required, StringLength(128, ErrorMessage = "TimeZone can not be greater than 24 characters")]
        public string TimeZone { get; set; }

        [Display(Name="Max number of Group Members")]
        public int? MaxNumberOfUsers { get; set; }

        [Display(Name = "Course Code")]
        [Required, StringLength(24, ErrorMessage = "CourseCode can not be greater than 24 characters")]
        public string CourseCode { get; set; }
    }
}