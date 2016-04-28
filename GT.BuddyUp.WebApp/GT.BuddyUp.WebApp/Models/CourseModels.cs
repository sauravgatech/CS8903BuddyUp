using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GT.BuddyUp.WebApp.Models
{
    
       
    public class CreateCourseModel
    {
        [Required]
        [Display(Name = "Course Code")]
        [StringLength(24, ErrorMessage = "The Course Code must be less than 24 characters long.")]
        public string CourseCode { get; set; }

        [Display(Name = "Course Name")]
        [Required]
        [StringLength(128, ErrorMessage = "The Course Name must be less than 128 characters long.")]
        public string CourseName { get; set; }

        [Display(Name = "Course Description")]
        [Required]
        [StringLength(2048, ErrorMessage = "The Course Description must be less than 2048 characters long.")]
        public string CourseDescription { get; set; }

        [Display(Name = "Group Type")]
        public string GroupType { get; set; }

        [Display(Name = "Preferred Group Size")]
        public int GroupSize { get; set; }

        public List<string> AvailableTerms {get; set;}

        [Display(Name = "Term of Course")]
        public string Term { get; set; }

        //[Display(Name = "Desired Skill Sets")]
        //public List<Skill> DesiredSkillSets { get; set; }

        [Display(Name = "Comma Separated List of Skill set (This would be used for group suggestions)", Description = "Comma Separated list of skill sets")]
        public string DesiredSkillSets { get; set; }

        [Display(Name = "Create Group with Similar Skill Sets")]
        public bool PreferSimiliarSkillSet { get; set; }

        [Display(Name = "Generate Intelligent Questionnaire for Group Suggestions")]
        public bool GenerateIntelligentQuestionnaire { get; set; }

        [Display(Name = "List of users. (Teacher is added by default and is required)")]
        public List<CourseUser> Users { get; set; }

        [Display(Name = "List of additional Questions (Optional)")]
        public List<Question> Questions { get; set; }
    }

    public class CourseUser
    {
        [Display(Name="Email ID")]
        public string emailId { get; set; }

        [Display(Name = "User Role")]
        public Role role { get; set; }
    }

    public enum SkillLevel
    {
        Beginner,
        Intermediate,
        Expert
    }

    public class Skill
    {
        [Display(Name = "Skill")]
        public string skill { get; set; }

        [Display(Name="Preferred Skill Level")]
        public SkillLevel requiredSkillLevel { get; set; }
    }

    public enum Priority
    {
        High,
        Medium,
        Low
    }

    public class Question
    {
        [Display(Name = "Question")]
        public string QuestionText {get; set;}

        [Display(Name = "Type")]
        public string QuestionType{get;set;}

        [Display(Name = "Answer Choices")]
        public string AnswerChoices {get; set;}

        [Display(Name= "Priority")]
        public Priority Priority { get; set; }
    }

    public class DisplayCourseModel
    {
        [Display(Name = "Course Code")]
        public string CourseCode { get; set; }

        [Display(Name = "Course Name")]
        public string CourseName { get; set; }

        [Display(Name = "Course Description")]
        public string CourseDescription { get; set; }

        [Display(Name = "Group Type")]
        public string GroupType { get; set; }

        [Display(Name = "Preferred Group Size")]
        public int GroupSize { get; set; }

        [Display(Name = "Desired Skill Sets")]
        public string DesiredSkillSets { get; set; }

        [Display(Name = "Create Group with Similar Skill Sets?")]
        public bool PreferSimiliarSkillSet { get; set; }

        [Display(Name = "Current List of users.")]
        public List<DisplayCourseUser> Users { get; set; }

        [Display(Name="Class Roaster")]
        public List<string> Roaster { get; set; }

        [Display(Name = "List of Questions")]
        public List<DsiplayQuestion> Questions { get; set; }

        public List<DisplayGroup> Groups { get; set; }
    }


    public class DisplayGroup
    {
        [Display(Name = "Group Code")]
        [Key, Required]
        public string GroupCode { get; set; }

        [Display(Name = "Group Name")]
        public string GroupName { get; set; }

        [Display(Name = "Timezone")]
        public string TimeZone { get; set; }

        [Display(Name = "Objective / Summary")]
        public string Objective { get; set; }

        [Display(Name="Group Members")]
        public List<DisplayCourseUser> GroupMembers { get; set; }
    }

    public class DisplayCourseUser
    {
        [Display(Name = "Email ID")]
        public string emailId { get; set; }

        [Display(Name = "Name")]
        public string name { get; set; }

        [Display(Name = "User Role")]
        public string role { get; set; }
    }

    public class DsiplayQuestion
    {
        [Display(Name = "Question")]
        public string QuestionText { get; set; }

        [Display(Name = "Question Type")]
        public string QuestionType { get; set; }

        [Display(Name = "Answer Choices")]
        public List<string> AnswerChoices { get; set; }

        public string selectedAnswer { get; set; }
    }

    public class StudentDashboardModel
    {
        public List<string> AllCourseDropDown { get; set; }
        public List<string> RegisteredCourseDropDown { get; set; }
        public string selectedCourse { get; set; }
        public string selectedRegisteredCourse { get; set; }
    }


    public class ModifyGroupsModel
    {
        public string courseCode { get; set; }
        public string courseName { get; set; }
        public string courseDescription { get; set; }
        public List<GroupModifiableModel> UserAndGroups { get; set; }
    }

    public class ModifyRoasterModel
    {
        public string message { get; set; }
        public string courseCode { get; set; }
        public string courseName { get; set; }
        public List<RoasterID> roasterIds { get; set; }
        [Display(Name="New GTIDs to Add")]
        public string newRoasterIds { get; set; }
    }

    public class RoasterID
    {
        public string gtId { get; set; }

        public bool isMarkedForDeletion { get; set; }
    }

    public class GroupModifiableModel
    {
        public string GroupName { get; set; }
        public string Objective { get; set; }
        public string MemberName { get; set; }
        public string EmailId { get; set; }
        //public string FitPercentage { get; set; }
        public string SuggestedGroups { get; set; }
        public string selectedNewGroup { get; set; }
        public List<string> NewGroups { get; set; }
    }
}