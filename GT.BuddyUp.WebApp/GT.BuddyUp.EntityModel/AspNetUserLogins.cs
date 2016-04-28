using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GT.BuddyUp.EntityModel
{
    public partial class AspNetUserLogins
    {
        [Key, StringLength(128), Column(Order=1)]
        public string LoginProvider {get; set;}

        [Key, StringLength(128), Column(Order = 2)]
        public string ProviderKey {get; set;}

        [Key, StringLength(128), Index, ForeignKey("AspNetUsers"), Column(Order = 3)]
        public string UserId {get; set;}

        public virtual AspNetUsers AspNetUsers { get; set; }
    }
}
