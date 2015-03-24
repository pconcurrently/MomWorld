namespace MomWorld.DataContexts.Migrations.NineMonthArticleMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NineMonthArticles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Date = c.Int(nullable: false),
                        Content = c.String(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.NineMonthArticles");
        }
    }
}
