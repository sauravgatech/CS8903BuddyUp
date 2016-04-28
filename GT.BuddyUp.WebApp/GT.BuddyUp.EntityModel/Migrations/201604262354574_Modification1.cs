namespace GT.BuddyUp.EntityModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Modification1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "Roaster", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "Roaster");
        }
    }
}
