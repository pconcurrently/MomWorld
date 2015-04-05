namespace MomWorld.DataContexts.Migrations.ArticleMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _9th : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Articles", "Tags2");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Articles", "Tags2", c => c.String());
        }
    }
}
