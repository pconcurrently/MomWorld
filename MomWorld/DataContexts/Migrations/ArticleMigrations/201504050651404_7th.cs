namespace MomWorld.DataContexts.Migrations.ArticleMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _7th : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ArticleLikes", "Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Articles", "DescriptionImage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ArticleLikes", "Date");
            DropColumn("dbo.Articles", "DescriptionImage");
        }
    }
}
