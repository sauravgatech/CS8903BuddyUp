using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GT.BuddyUp.EntityModel
{
    public partial class AspNetUserRoles
    {
        [StringLength(128), Index]
        [Key, ForeignKey("AspNetUsers"), Column(Order = 1)]
        public string UserId {get; set;}

        [StringLength(128), Index]
        [Key, ForeignKey("AspNetRoles"), Column(Order = 2)]
        public string RoleId {get; set;}

        public virtual AspNetUsers AspNetUsers { get; set; }

        public virtual AspNetRoles AspNetRoles { get; set; }
    }
}
