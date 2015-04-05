namespace MomWorld.DataContexts.Migrations.ArticleMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _8th : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "Tags2", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Articles", "Tags2");
        }
    }
}
