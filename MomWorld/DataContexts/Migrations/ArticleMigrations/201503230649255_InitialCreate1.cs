namespace MomWorld.DataContexts.Migrations.ArticleMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Articles", "Description");
        }
    }
}
