using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GT.BuddyUp.EntityModel
{
    public partial class Questionnaire : AuditBase
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuestionnaireId { get; set; }

        public string QuestionnaireCode { get; set; }

        public string QuestionSet { get; set; }

        public bool? IsATemplate { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}
