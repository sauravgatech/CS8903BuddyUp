using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace GT.BuddyUp.EntityModel
{
    public partial class AspNetUsers
    {
        [Key, StringLength(128)]
        public string Id {get; set;}

        [StringLength(256)]
        public string Email {get; set;}

        [StringLength(256), Index("UserNameIndex", IsUnique = true)]
        public string UserName { get; set; }

        [StringLength(128)]
        public string FirstName { get; set; }

        [StringLength(128)]
        public string LastName { get; set; }

        [StringLength(32), Index("GTIDIndex", IsUnique = true)]
        public string GTID { get; set; }

        public bool EmailConfirmed {get; set;}
        
        public string PasswordHash {get; set;}
        
        public string SecurityStamp {get; set;}
        
        public string PhoneNumber {get; set;}
        
        public bool PhoneNumberConfirmed {get; set;}
        public bool TwoFactorEnabled {get; set;}
        
        public DateTime? LockoutEndDateUtc {get; set;}
        
        public bool LockoutEnabled {get; set;}
        
        public int AccessFailedCount {get; set;}

        public virtual ICollection<CourseUser> CourseUsers { get; set; }
        
    }
}
