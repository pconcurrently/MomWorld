namespace MomWorld.DataContexts.Migrations.ArticleMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3rd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "Tags", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Articles", "Tags");
        }
    }
}
