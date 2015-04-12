namespace MomWorld.DataContexts.Migrations.ArticleMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _10th : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "Phase", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Articles", "Phase");
        }
    }
}
