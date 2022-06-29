using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Movies.Models;

namespace Movies.Controllers
{
    public class moviesController : ApiController
    {
        private VueCRUDEntities db = new VueCRUDEntities();

        // GET: api/movies
        public IQueryable<movie> Getmovies()
        {
            return db.movies;
        }

        // GET: api/movies/5
        [ResponseType(typeof(movie))]
        public IHttpActionResult Getmovie(int id)
        {
            movie movie = db.movies.Find(id);
            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }

        // PUT: api/movies/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putmovie(int id, movie movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != movie.idMovie)
            {
                return BadRequest();
            }

            db.Entry(movie).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!movieExists(id))
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

        // POST: api/movies
        [ResponseType(typeof(movie))]
        public IHttpActionResult Postmovie(movie movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.movies.Add(movie);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = movie.idMovie, year = movie.year, genre=movie.genre,
                movieType = movie.movieType,
                description = movie.description, title=movie.title, watched= movie.watched }, movie);
        }

        // DELETE: api/movies/5
        [ResponseType(typeof(movie))]
        public IHttpActionResult Deletemovie(int id)
        {
            movie movie = db.movies.Find(id);
            if (movie == null)
            {
                return NotFound();
            }

            db.movies.Remove(movie);
            db.SaveChanges();

            return Ok(movie);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool movieExists(int id)
        {
            return db.movies.Count(e => e.idMovie == id) > 0;
        }
    }
}