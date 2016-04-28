using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GT.BuddyUp.EntityModel
{
    public partial class Group : AuditBase
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GroupId { get; set; }

        [StringLength(24)]
        public string GroupCode { get; set; }

        [StringLength(128)]
        public string GroupName { get; set; }

        [StringLength(2048)]
        public string Objective { get; set; }

        [StringLength(128)]
        public string TimeZone { get; set; }

        [ForeignKey("GroupType")]
        public int GroupTypeId { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; }

        public virtual GroupType GroupType { get; set; }

        public virtual Course Course { get; set; }

        public virtual ICollection<CourseUser> CourseUserRoles { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}
