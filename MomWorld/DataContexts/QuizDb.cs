using MomWorld.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MomWorld.DataContexts
{
    public class QuizDb : DbContext
    {
        public QuizDb()
            : base("MomWorldConnection")
        {
        }

        public DbSet<Quiz> Quizzes { get; set; }

        public DbSet<Option> Options { get; set; }

        public DbSet<QuizQuestion> QuizQuestions { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<Answer> Anwsers { get; set; }
    }
}