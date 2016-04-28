using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GT.BuddyUp.EntityModel
{
    public partial class AspNetRoles
    {
        [Key,StringLength(128)]
        public string Id { get; set; }

        [StringLength(256), Index("RoleNameIndex")]
        public string Name { get; set; }

        public string Description { get; set; }

        [StringLength(128)]
        public string Discriminator { get; set; }
    }
}
