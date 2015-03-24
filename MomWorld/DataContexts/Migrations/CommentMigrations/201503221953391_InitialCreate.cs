namespace MomWorld.DataContexts.Migrations.CommentMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ArticleId = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(nullable: false),
                        Date = c.DateTime(),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Articles", t => t.ArticleId, cascadeDelete: true)
                .Index(t => t.ArticleId);            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "ArticleId", "dbo.Articles");
            DropIndex("dbo.Comments", new[] { "ArticleId" });
            DropTable("dbo.Articles");
            DropTable("dbo.Comments");
        }
    }
}
