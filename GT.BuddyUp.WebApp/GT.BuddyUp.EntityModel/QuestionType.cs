using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GT.BuddyUp.EntityModel
{
    public partial class QuestionType : AuditBase
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuestionTypeId { get; set; }
        
        [StringLength(24)]
        public string Type { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}
