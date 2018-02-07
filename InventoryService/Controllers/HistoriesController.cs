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
using InventoryService.Models;

namespace InventoryService.Controllers
{
    public class HistoriesController : ApiController
    {
        private FGInventoryEntities db = new FGInventoryEntities();

        // GET: api/Histories
        public IQueryable<History> GetHistories()
        {
            return db.Histories;
        }

        // GET: api/Histories/5
        [ResponseType(typeof(History))]
        public IHttpActionResult GetHistory(int id)
        {
            History history = db.Histories.Find(id);
            if (history == null)
            {
                return NotFound();
            }

            return Ok(history);
        }

        // PUT: api/Histories/5
       /* [ResponseType(typeof(void))]
        public IHttpActionResult PutHistory(int id, History history)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != history.Seq)
            {
                return BadRequest();
            }

            db.Entry(history).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HistoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }*/

        // POST: api/Histories
        [ResponseType(typeof(History))]
        public IHttpActionResult PostHistory(History history)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Histories.Add(history);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = history.Seq }, history);
        }

        // DELETE: api/Histories/5
        [ResponseType(typeof(History))]
        public IHttpActionResult DeleteHistory(int id)
        {
            History history = db.Histories.Find(id);
            if (history == null)
            {
                return NotFound();
            }

            db.Histories.Remove(history);
            db.SaveChanges();

            return Ok(history);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool HistoryExists(int id)
        {
            return db.Histories.Count(e => e.Seq == id) > 0;
        }
    }
}