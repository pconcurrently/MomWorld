using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MomWorld.DataContexts;
using MomWorld.Entities;

namespace MomWorld.Controllers
{
    public class QuizsController : ApiController
    {
        private QuizDb db = new QuizDb();

        // GET: api/Quizs
        public IQueryable<Quiz> GetQuizzes()
        {
            return db.Quizzes;
        }

        // GET: api/Quizs/5
        [ResponseType(typeof(Quiz))]
        public async Task<IHttpActionResult> GetQuiz(string id)
        {
            Quiz quiz = await db.Quizzes.FindAsync(id);
            if (quiz == null)
            {
                return NotFound();
            }

            return Ok(quiz);
        }

        // PUT: api/Quizs/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutQuiz(string id, Quiz quiz)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != quiz.Id)
            {
                return BadRequest();
            }

            db.Entry(quiz).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuizExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Quizs
        [ResponseType(typeof(Quiz))]
        public async Task<IHttpActionResult> PostQuiz(Quiz quiz)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Quizzes.Add(quiz);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (QuizExists(quiz.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = quiz.Id }, quiz);
        }

        // DELETE: api/Quizs/5
        [ResponseType(typeof(Quiz))]
        public async Task<IHttpActionResult> DeleteQuiz(string id)
        {
            Quiz quiz = await db.Quizzes.FindAsync(id);
            if (quiz == null)
            {
                return NotFound();
            }

            db.Quizzes.Remove(quiz);
            await db.SaveChangesAsync();

            return Ok(quiz);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool QuizExists(string id)
        {
            return db.Quizzes.Count(e => e.Id == id) > 0;
        }
    }
}