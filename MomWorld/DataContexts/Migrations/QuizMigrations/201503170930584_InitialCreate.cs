namespace MomWorld.DataContexts.Migrations.QuizMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        OptionId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Options", t => t.OptionId)
                .Index(t => t.OptionId);
            
            CreateTable(
                "dbo.Options",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        QuestionId = c.String(maxLength: 128),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.QuestionId)
                .Index(t => t.QuestionId);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Content = c.String(),
                        AnswerId = c.String(),
                        Anwser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Answers", t => t.Anwser_Id)
                .Index(t => t.Anwser_Id);
            
            CreateTable(
                "dbo.QuizQuestions",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        QuestionId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.QuestionId)
                .Index(t => t.QuestionId);
            
            CreateTable(
                "dbo.Quizs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Intro = c.String(),
                        Description = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(nullable: false),
                        QuizQuestionId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.QuizQuestions", t => t.QuizQuestionId)
                .Index(t => t.QuizQuestionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Quizs", "QuizQuestionId", "dbo.QuizQuestions");
            DropForeignKey("dbo.QuizQuestions", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.Answers", "OptionId", "dbo.Options");
            DropForeignKey("dbo.Options", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.Questions", "Anwser_Id", "dbo.Answers");
            DropIndex("dbo.Quizs", new[] { "QuizQuestionId" });
            DropIndex("dbo.QuizQuestions", new[] { "QuestionId" });
            DropIndex("dbo.Questions", new[] { "Anwser_Id" });
            DropIndex("dbo.Options", new[] { "QuestionId" });
            DropIndex("dbo.Answers", new[] { "OptionId" });
            DropTable("dbo.Quizs");
            DropTable("dbo.QuizQuestions");
            DropTable("dbo.NineMonthArticles");
            DropTable("dbo.Questions");
            DropTable("dbo.Options");
            DropTable("dbo.Answers");
        }
    }
}
