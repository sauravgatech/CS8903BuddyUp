using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GT.BuddyUp.EntityModel
{
    public partial class Post : AuditBase
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostId { get; set; }

        public string PostText { get; set; }
        
        [StringLength(128)]
        public string UserName { get; set; }

        public DateTime TimePosted { get; set; }

        public int? ParentId { get; set; }

        [ForeignKey("Group")]
        public int GroupId { get; set; }

        public virtual Group Group { get; set; }
    }
}
