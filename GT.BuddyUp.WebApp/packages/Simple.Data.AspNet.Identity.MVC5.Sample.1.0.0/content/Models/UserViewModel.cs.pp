using System;
using System.ComponentModel;
using Simple.Data.AspNet.Identity;

namespace $rootnamespace$.Models {
    public class UserViewModel {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool Confirmed { get; set; }
        public bool Locked { get; set; }
        public IdentityRole Role { get; set; }

        [DisplayName("Role")]
        public string SelectedRole { get; set; }

        public UserViewModel() {
            
        }

        public UserViewModel(IdentityUser user) {
            Id = user.Id;
            Email = user.Email;
            UserName = user.UserName;
            Confirmed = user.EmailConfirmed;
            Locked = user.LockoutEndDateUtc > DateTime.UtcNow;

        }
    }
}
