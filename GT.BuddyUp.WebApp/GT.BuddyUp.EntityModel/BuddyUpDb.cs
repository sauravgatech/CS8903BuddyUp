using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace GT.BuddyUp.EntityModel
{
    public partial class BuddyUpDb : DbContext
    {
        public BuddyUpDb()
            : base("BuddyUpDb")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<BuddyUpDb, Migrations.Configuration>());
        }

        public BuddyUpDb(System.Data.Common.DbConnection dbConnection)
            : base(dbConnection, true)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<BuddyUpDb, Migrations.Configuration>());
        }

        public DbSet<AnswerChoice> AnswerChoices { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<CourseUser> CourseUserRoles { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<GroupType> GroupTypes { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<Questionnaire> Questionnaires { get; set; }

        public DbSet<QuestionType> QuestionTypes { get; set; }

        public DbSet<AspNetRoles> AspNetRoles { get; set; }

        public DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }

        public DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }

        public DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }

        public DbSet<AspNetUsers> AspNetUsers { get; set; }

        public DbSet<Post> Posts { get; set; }
    }
}
