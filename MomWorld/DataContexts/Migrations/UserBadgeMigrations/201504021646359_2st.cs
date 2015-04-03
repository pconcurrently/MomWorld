namespace MomWorld.DataContexts.Migrations.UserBadgeMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2st : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.UserBadges");
            AddColumn("dbo.UserBadges", "Name", c => c.String());
            AddColumn("dbo.UserBadges", "Image", c => c.String());
            AddColumn("dbo.UserBadges", "Status", c => c.String());
            AlterColumn("dbo.UserBadges", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.UserBadges", "Id");
            DropColumn("dbo.UserBadges", "UserId");
            DropColumn("dbo.UserBadges", "Badge");
            DropColumn("dbo.UserBadges", "Link");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserBadges", "Link", c => c.String(nullable: false));
            AddColumn("dbo.UserBadges", "Badge", c => c.String(nullable: false));
            AddColumn("dbo.UserBadges", "UserId", c => c.String(nullable: false));
            DropPrimaryKey("dbo.UserBadges");
            AlterColumn("dbo.UserBadges", "Id", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.UserBadges", "Status");
            DropColumn("dbo.UserBadges", "Image");
            DropColumn("dbo.UserBadges", "Name");
            AddPrimaryKey("dbo.UserBadges", "Id");
        }
    }
}
