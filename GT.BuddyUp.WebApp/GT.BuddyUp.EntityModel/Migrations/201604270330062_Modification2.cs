namespace GT.BuddyUp.EntityModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Modification2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "Term", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "Term");
        }
    }
}
