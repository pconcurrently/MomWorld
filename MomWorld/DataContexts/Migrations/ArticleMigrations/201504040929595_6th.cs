namespace MomWorld.DataContexts.Migrations.ArticleMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _6th : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reports", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reports", "Date");
        }
    }
}
