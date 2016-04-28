using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GT.BuddyUp.EntityModel
{
    public partial class Course : AuditBase
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CourseId { get; set; }

        [StringLength(24)]
        public string CourseCode { get; set; }

        [StringLength(128)]
        public string CourseName { get; set; }

        [StringLength(2048)]
        public string CourseDescription { get; set; }

        [ForeignKey("Questionnaire")]
        public int? QuestionnaireId { get; set; }

        [ForeignKey("GroupType")]
        public int? PrefGroupTypeId { get; set; }

        public int? PrefGroupSize { get; set; }

        public bool? SimilarSkillSetPreffered { get; set; }

        public string DesiredSkillSets { get; set; }

        public string Roaster { get; set; }

        public string Term { get; set; }

        public virtual GroupType GroupType { get; set; }

        public virtual Questionnaire Questionnaire { get; set; }

        public virtual ICollection<CourseUser> CourseUserRoles { get; set; }

        public virtual ICollection<Group> Groups { get; set; }
    }
}
