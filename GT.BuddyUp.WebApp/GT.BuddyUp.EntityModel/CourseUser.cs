using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GT.BuddyUp.EntityModel
{
    public partial class CourseUser : AuditBase
    {

        [Key, ForeignKey("AspNetUsers"), Column(Order = 1)]
        public string UserId { get; set; }

        [Key, ForeignKey("Course"), Column(Order = 2)]
        public int CourseId { get; set; }

        [ForeignKey("Group")]
        public int? GroupId { get; set; }

        public string AnswerSet { get; set; }

        public virtual AspNetUsers AspNetUsers { get; set; }

        public virtual Course Course { get; set; }

        public virtual Group Group { get; set; }
    }
}
