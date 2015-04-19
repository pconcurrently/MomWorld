namespace MomWorld.DataContexts.Migrations.IdentityMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserRoutine : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserRoutines",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(),
                        Phase = c.String(),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserRoutines");
        }
    }
}
