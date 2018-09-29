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
    public class DailyTotalsController : ApiController
    {
        private FGInventoryEntities db = new FGInventoryEntities();

        // GET: api/DailyTotals
        public IQueryable<DailyTotal> GetDailyTotals()
        {
            return db.DailyTotals;
        }

        // GET: api/DailyTotals/5
        [ResponseType(typeof(DailyTotal))]
        public IHttpActionResult GetDailyTotal(int id)
        {
            DailyTotal dailyTotal = db.DailyTotals.Find(id);
            if (dailyTotal == null)
            {
                return NotFound();
            }

            return Ok(dailyTotal);
        }

     
        // POST: api/DailyTotals
        [ResponseType(typeof(DailyTotal))]
        public IHttpActionResult PostDailyTotal(DailyTotal dailyTotal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.DailyTotals.Add(dailyTotal);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (DailyTotalExists(dailyTotal.Total))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = dailyTotal.Total }, dailyTotal);
        }

        // DELETE: api/DailyTotals/5
        [ResponseType(typeof(DailyTotal))]
        public IHttpActionResult DeleteDailyTotal(int id)
        {
            DailyTotal dailyTotal = db.DailyTotals.Find(id);
            if (dailyTotal == null)
            {
                return NotFound();
            }

            db.DailyTotals.Remove(dailyTotal);
            db.SaveChanges();

            return Ok(dailyTotal);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DailyTotalExists(int id)
        {
            return db.DailyTotals.Count(e => e.Total == id) > 0;
        }
    }
}