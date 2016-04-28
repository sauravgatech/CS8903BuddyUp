using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Collections.Generic;

namespace GT.BuddyUp.EntityModel.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<GT.BuddyUp.EntityModel.BuddyUpDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "GT.CS6460.BuddyUp.EntityModel.BuddyUpDb";
        }

        protected override void Seed(GT.BuddyUp.EntityModel.BuddyUpDb db)
        {
            db.AspNetRoles.AddOrUpdate(x => x.Id, new AspNetRoles() { Id = "Admin", Name = "Admin", Description = "Administrator", Discriminator = "Admin" });
            db.AspNetRoles.AddOrUpdate(x => x.Id, new AspNetRoles() { Id = "Teacher", Name = "Teacher", Description = "Teacher", Discriminator = "Teacher" });
            db.AspNetRoles.AddOrUpdate(x => x.Id, new AspNetRoles() { Id = "Student", Name = "Student", Description = "Student", Discriminator = "Student" });
            db.AspNetRoles.AddOrUpdate(x => x.Id, new AspNetRoles() { Id = "TA", Name = "TA", Description = "TA", Discriminator = "TA" });
            //db.AspNetUsers.AddOrUpdate(x => x.Id, new AspNetUsers() { Id = "17ecd6d1-50a0-4ce8-82be-7cc4161e928e", Email = "buddyup.gatech@gmail.com", EmailConfirmed = true, FirstName = "BuddyUp", LastName = "GATech", GTID = "Admin", AccessFailedCount = 0, LockoutEnabled = false, LockoutEndDateUtc = null, PasswordHash = "ANib0tOq1WyR3XqSWbHHjyVwyFcGLUq8eJ7iAUidhgf1p0sQkDnkBTIUKF8sW6AxEw==", PhoneNumber = "", PhoneNumberConfirmed = false, SecurityStamp = "44f0d82f-1072-408c-9eb9-03c7b46b7053", TwoFactorEnabled = false, UserName = "buddyup.gatech@gmail.com" });
            db.AspNetUsers.AddOrUpdate(x => x.Id, new AspNetUsers() { Id = "17ecd6d1-50a0-4ce8-82be-7cc4161e928e", Email = "buddyup.gatech@gmail.com", EmailConfirmed = true, FirstName = "BuddyUp", LastName = "GATech", GTID = "Admin", AccessFailedCount = 0, LockoutEnabled = false, LockoutEndDateUtc = null, PasswordHash = "ANib0tOq1WyR3XqSWbHHjyVwyFcGLUq8eJ7iAUidhgf1p0sQkDnkBTIUKF8sW6AxEw==", PhoneNumber = "", PhoneNumberConfirmed = false, SecurityStamp = "", TwoFactorEnabled = false, UserName = "buddyup.gatech@gmail.com" });
            db.AspNetUserRoles.AddOrUpdate(x => new { x.RoleId, x.UserId }, new AspNetUserRoles() { UserId = "17ecd6d1-50a0-4ce8-82be-7cc4161e928e", RoleId = "Admin" });
            db.GroupTypes.AddOrUpdate(x => x.GroupTypeId, new GroupType() { GroupTypeId = 1, GroupTypeCode = "Study", Description = "Study Groups" });
            db.GroupTypes.AddOrUpdate(x => x.GroupTypeId, new GroupType() { GroupTypeId = 2, GroupTypeCode = "OpenProject", Description = "Open Project Group" });
            db.GroupTypes.AddOrUpdate(x => x.GroupTypeId, new GroupType() { GroupTypeId = 3, GroupTypeCode = "ClosedProject", Description = "Closed Project Group" });
            db.QuestionTypes.AddOrUpdate(x => x.QuestionTypeId, new QuestionType() { QuestionTypeId = 1, Type = "MultipleChoice" });
            db.QuestionTypes.AddOrUpdate(x => x.QuestionTypeId, new QuestionType() { QuestionTypeId = 2, Type = "FillUpTheBlank" });


        }
    }
}
