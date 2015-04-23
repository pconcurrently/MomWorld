namespace MomWorld.DataContexts.Migrations.SubscriberMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixTask : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserTasks", "DueDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.UserTasks", "CreatedDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.UserTasks", "Date");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserTasks", "Date", c => c.DateTime(nullable: false));
            DropColumn("dbo.UserTasks", "CreatedDate");
            DropColumn("dbo.UserTasks", "DueDate");
        }
    }
}
