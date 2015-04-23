namespace MomWorld.DataContexts.Migrations.SubscriberMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTaskTitle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserTasks", "Title", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserTasks", "Title");
        }
    }
}
