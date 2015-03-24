namespace MomWorld.DataContexts.Migrations.CategoryMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(),
                        CategoryId = c.String(nullable: false, maxLength: 128),
                        Title = c.String(nullable: false),
                        Content = c.String(nullable: false),
                        PostedDate = c.DateTime(),
                        LastModifiedDate = c.DateTime(),
                        ViewNumber = c.Int(),
                        LastSeenUserId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Articles", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Articles", new[] { "CategoryId" });
            DropTable("dbo.Articles");
            DropTable("dbo.Categories");
        }
    }
}
