namespace GT.BuddyUp.EntityModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AnswerChoices",
                c => new
                    {
                        AnswerId = c.Int(nullable: false, identity: true),
                        Answer = c.String(maxLength: 128),
                        QuestionId = c.Int(nullable: false),
                        LastChangedBy = c.String(maxLength: 32),
                        LastChangedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.AnswerId)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .Index(t => t.QuestionId);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        QuestionId = c.Int(nullable: false, identity: true),
                        QuestionText = c.String(maxLength: 128),
                        QuestionTypeId = c.Int(nullable: false),
                        Priority = c.String(maxLength: 16),
                        LastChangedBy = c.String(maxLength: 32),
                        LastChangedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.QuestionId)
                .ForeignKey("dbo.QuestionTypes", t => t.QuestionTypeId, cascadeDelete: true)
                .Index(t => t.QuestionTypeId);
            
            CreateTable(
                "dbo.QuestionTypes",
                c => new
                    {
                        QuestionTypeId = c.Int(nullable: false, identity: true),
                        Type = c.String(maxLength: 24),
                        LastChangedBy = c.String(maxLength: 32),
                        LastChangedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.QuestionTypeId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(maxLength: 256),
                        Description = c.String(),
                        Discriminator = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        UserName = c.String(maxLength: 256),
                        FirstName = c.String(maxLength: 128),
                        LastName = c.String(maxLength: 128),
                        GTID = c.String(maxLength: 32),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.GTID, unique: true, name: "GTIDIndex");
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        CourseId = c.Int(nullable: false, identity: true),
                        CourseCode = c.String(maxLength: 24),
                        CourseName = c.String(maxLength: 128),
                        CourseDescription = c.String(maxLength: 2048),
                        QuestionnaireId = c.Int(),
                        PrefGroupTypeId = c.Int(),
                        PrefGroupSize = c.Int(),
                        SimilarSkillSetPreffered = c.Boolean(),
                        DesiredSkillSets = c.String(),
                        LastChangedBy = c.String(maxLength: 32),
                        LastChangedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.CourseId)
                .ForeignKey("dbo.GroupTypes", t => t.PrefGroupTypeId)
                .ForeignKey("dbo.Questionnaires", t => t.QuestionnaireId)
                .Index(t => t.QuestionnaireId)
                .Index(t => t.PrefGroupTypeId);
            
            CreateTable(
                "dbo.CourseUsers",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        CourseId = c.Int(nullable: false),
                        GroupId = c.Int(),
                        AnswerSet = c.String(),
                        LastChangedBy = c.String(maxLength: 32),
                        LastChangedTime = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.UserId, t.CourseId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.GroupId)
                .Index(t => t.UserId)
                .Index(t => t.CourseId)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        GroupId = c.Int(nullable: false, identity: true),
                        GroupCode = c.String(maxLength: 24),
                        GroupName = c.String(maxLength: 128),
                        Objective = c.String(maxLength: 2048),
                        TimeZone = c.String(maxLength: 128),
                        GroupTypeId = c.Int(nullable: false),
                        CourseId = c.Int(nullable: false),
                        LastChangedBy = c.String(maxLength: 32),
                        LastChangedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.GroupId)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.GroupTypes", t => t.GroupTypeId, cascadeDelete: true)
                .Index(t => t.GroupTypeId)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.GroupTypes",
                c => new
                    {
                        GroupTypeId = c.Int(nullable: false, identity: true),
                        GroupTypeCode = c.String(maxLength: 24),
                        Description = c.String(maxLength: 128),
                        LastChangedBy = c.String(maxLength: 32),
                        LastChangedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.GroupTypeId);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        PostId = c.Int(nullable: false, identity: true),
                        PostText = c.String(),
                        UserName = c.String(maxLength: 128),
                        TimePosted = c.DateTime(nullable: false),
                        ParentId = c.Int(),
                        GroupId = c.Int(nullable: false),
                        LastChangedBy = c.String(maxLength: 32),
                        LastChangedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.PostId)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.Questionnaires",
                c => new
                    {
                        QuestionnaireId = c.Int(nullable: false, identity: true),
                        QuestionnaireCode = c.String(),
                        QuestionSet = c.String(),
                        IsATemplate = c.Boolean(),
                        LastChangedBy = c.String(maxLength: 32),
                        LastChangedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.QuestionnaireId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Courses", "QuestionnaireId", "dbo.Questionnaires");
            DropForeignKey("dbo.Courses", "PrefGroupTypeId", "dbo.GroupTypes");
            DropForeignKey("dbo.CourseUsers", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.Posts", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.Groups", "GroupTypeId", "dbo.GroupTypes");
            DropForeignKey("dbo.Groups", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.CourseUsers", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.CourseUsers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AnswerChoices", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.Questions", "QuestionTypeId", "dbo.QuestionTypes");
            DropIndex("dbo.Posts", new[] { "GroupId" });
            DropIndex("dbo.Groups", new[] { "CourseId" });
            DropIndex("dbo.Groups", new[] { "GroupTypeId" });
            DropIndex("dbo.CourseUsers", new[] { "GroupId" });
            DropIndex("dbo.CourseUsers", new[] { "CourseId" });
            DropIndex("dbo.CourseUsers", new[] { "UserId" });
            DropIndex("dbo.Courses", new[] { "PrefGroupTypeId" });
            DropIndex("dbo.Courses", new[] { "QuestionnaireId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "GTIDIndex");
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Questions", new[] { "QuestionTypeId" });
            DropIndex("dbo.AnswerChoices", new[] { "QuestionId" });
            DropTable("dbo.Questionnaires");
            DropTable("dbo.Posts");
            DropTable("dbo.GroupTypes");
            DropTable("dbo.Groups");
            DropTable("dbo.CourseUsers");
            DropTable("dbo.Courses");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.QuestionTypes");
            DropTable("dbo.Questions");
            DropTable("dbo.AnswerChoices");
        }
    }
}
