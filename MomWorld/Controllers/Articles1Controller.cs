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
    public class Articles1Controller : ApiController
    {
        private ArticleDb db = new ArticleDb();

        // GET: api/Articles1
        public IQueryable<Article> GetArticles()
        {
            return db.Articles;
        }

        // GET: api/Articles1/5
        [ResponseType(typeof(Article))]
        public async Task<IHttpActionResult> GetArticle(string id)
        {
            Article article = await db.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            return Ok(article);
        }

        // PUT: api/Articles1/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutArticle(string id, Article article)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != article.Id)
            {
                return BadRequest();
            }

            db.Entry(article).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(id))
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

        // POST: api/Articles1
        [ResponseType(typeof(Article))]
        public async Task<IHttpActionResult> PostArticle(Article article)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Articles.Add(article);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ArticleExists(article.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = article.Id }, article);
        }

        // DELETE: api/Articles1/5
        [ResponseType(typeof(Article))]
        public async Task<IHttpActionResult> DeleteArticle(string id)
        {
            Article article = await db.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            db.Articles.Remove(article);
            await db.SaveChangesAsync();

            return Ok(article);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ArticleExists(string id)
        {
            return db.Articles.Count(e => e.Id == id) > 0;
        }
    }
}