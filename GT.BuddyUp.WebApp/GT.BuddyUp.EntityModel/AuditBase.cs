using System;
using System.ComponentModel.DataAnnotations;

namespace GT.BuddyUp.EntityModel
{
    public abstract class AuditBase
    {
        [StringLength(32)]
        public string LastChangedBy { get; set; }
        public Nullable<DateTime> LastChangedTime { get; set; }
    }
}
