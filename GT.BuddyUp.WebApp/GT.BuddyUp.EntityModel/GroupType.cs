using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GT.BuddyUp.EntityModel
{
    public partial class GroupType : AuditBase
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GroupTypeId { get; set; }

        [StringLength(24)]
        public string GroupTypeCode { get; set; }

        [StringLength(128)]
        public string Description { get; set; }

        public virtual ICollection<Group> Groups { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}
