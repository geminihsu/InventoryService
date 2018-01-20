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
using InventoryService.Controllers.DbUtil;
using InventoryService.Models;

namespace InventoryService.Controllers
{
    public class ReportsController : ApiController
    {
        private FGInventoryEntities db = new FGInventoryEntities();

        // GET: api/Reports
        /*public IQueryable<Report> GetReports()
        {
            return db.Reports;
        }*/

        // GET api/Models
        [Route("api/Reports")]
        public HttpResponseMessage Get()
        {
            var models = ReportsRepository.GetAllModels();
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, models);
            return response;
        }

        // GET: api/Reports/5
        [ResponseType(typeof(Report))]
        public IHttpActionResult GetReport(string id)
        {
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return NotFound();
            }

            return Ok(report);
        }

        /*// PUT: api/Reports/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutReport(string id, Report report)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != report.Model)
            {
                return BadRequest();
            }

            db.Entry(report).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReportExists(id))
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

        [Route("api/Reports")]
        public HttpResponseMessage Put(List<Report> e)
        {
            var item = ReportsRepository.UpdateReportRecord(e);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, item);
            return response;
        }

        // POST: api/Reports
        [ResponseType(typeof(Report))]
        public IHttpActionResult PostReport(Report report)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Reports.Add(report);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ReportExists(report.Model))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = report.Model }, report);
        }

        // DELETE: api/Reports/5
        [ResponseType(typeof(Report))]
        public IHttpActionResult DeleteReport(string id)
        {
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return NotFound();
            }

            db.Reports.Remove(report);
            db.SaveChanges();

            return Ok(report);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ReportExists(string id)
        {
            return db.Reports.Count(e => e.Model == id) > 0;
        }
    }
}