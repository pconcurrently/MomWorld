namespace MomWorld.DataContexts.Migrations.SubscriberMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserTasks", "UserName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserTasks", "UserName");
        }
    }
}
