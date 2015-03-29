namespace MomWorld.DataContexts.Migrations.StatusMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StatusComments",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        CreatorName = c.String(),
                        CreatorAvatar = c.String(),
                        StatusId = c.String(maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Status", t => t.StatusId)
                .Index(t => t.StatusId);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        CreatorName = c.String(),
                        CreatorAvatar = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        Content = c.String(),
                        like = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StatusComments", "StatusId", "dbo.Status");
            DropIndex("dbo.StatusComments", new[] { "StatusId" });
            DropTable("dbo.Status");
            DropTable("dbo.StatusComments");
        }
    }
}
