using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomWorld.Entities
{
    public class Quiz
    {
        [Key]
        [MaxLength(128)]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Intro { get; set; }

        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public string QuizQuestionId { get; set; }

        public QuizQuestion QuizQuestion { get; set; }

        public Quiz()
        {
            Id = Guid.NewGuid().ToString();
        }
    }

    public class QuizQuestion
    {
        [Key]
        [MaxLength(128)]
        public string Id { get; set; }

        public string QuestionId { get; set; }

        public virtual Question Question { get; set; }

        public QuizQuestion()
        {
            Id = Guid.NewGuid().ToString();
        }
    }

    public class Question
    {
        [Key]
        [MaxLength(128)]
        public string Id { get; set; }

        public string Content { get; set; }

        public string AnswerId { get; set; }

        public virtual Answer Anwser { get; set; }

        public Question()
        {
            Id = Guid.NewGuid().ToString();
        }

    }

    public class Option
    {
        [Key]
        [MaxLength(128)]
        public string Id { get; set; }

        public string QuestionId { get; set; }

        public virtual Question Question { get; set; }

        public string Content { get; set; }

        public Option()
        {
            Id = Guid.NewGuid().ToString();
        }
    }

    public class Answer
    {
        [Key]
        [MaxLength(128)]
        public string Id { get; set; }

        public string OptionId { get; set; }
        public virtual Option Option { get; set; }

        public Answer()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
