namespace MomWorld.DataContexts.Migrations.ArticleMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _11th : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "Phase", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Categories", "Phase");
        }
    }
}
