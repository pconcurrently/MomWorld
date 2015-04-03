namespace MomWorld.DataContexts.Migrations.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2nd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Status");
        }
    }
}
