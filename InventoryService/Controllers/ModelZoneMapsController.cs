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
    public class ModelZoneMapsController : ApiController
    {
        private FGInventoryEntities db = new FGInventoryEntities();

        /*// GET: api/ModelZoneMaps
        public IQueryable<ModelZoneMap> GetModelZoneMaps()
        {
            return db.ModelZoneMaps;
        }*/

        // GET api/ModelZoneMaps
        [Route("api/ModelZoneMaps")]
        public HttpResponseMessage Get()
        {
            var models = ModelZone2MinQuantityRepository.GetAllModels();
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, models);
            return response;
        }


        // GET api/ModelZoneMaps
        [Route("api/ModelZoneMaps/Qty")]
        public HttpResponseMessage GetQty()
        {
            var models = ModelZone2MinQuantityRepository.GetAllModelsQty();
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, models);
            return response;
        }

        // GET api/ModelZoneMaps/DailyReport/date/
        [Route("api/ModelZoneMaps/DailyReport/date/{date:datetime}")]
        public HttpResponseMessage GetDailyReport(DateTime date)
        {
            var models = ModelZone2MinQuantityRepository.GetDailyReportByModels(date);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, models);
            return response;
        }

        // GET: api/ModelZoneMaps/5
        [ResponseType(typeof(ModelZoneMap))]
        public IHttpActionResult GetModelZoneMap(int id)
        {
            ModelZoneMap modelZoneMap = db.ModelZoneMaps.Find(id);
            if (modelZoneMap == null)
            {
                return NotFound();
            }

            return Ok(modelZoneMap);
        }

       /* // PUT: api/ModelZoneMaps/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutModelZoneMap(int id, ModelZoneMap modelZoneMap)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != modelZoneMap.Seq)
            {
                return BadRequest();
            }

            db.Entry(modelZoneMap).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModelZoneMapExists(id))
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

        // POST: api/ModelZoneMaps
        [ResponseType(typeof(ModelZoneMap))]
        public IHttpActionResult PostModelZoneMap(ModelZoneMap modelZoneMap)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ModelZoneMaps.Add(modelZoneMap);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = modelZoneMap.Seq }, modelZoneMap);
        }

        // DELETE: api/ModelZoneMaps/5
        [ResponseType(typeof(ModelZoneMap))]
        public IHttpActionResult DeleteModelZoneMap(int id)
        {
            ModelZoneMap modelZoneMap = db.ModelZoneMaps.Find(id);
            if (modelZoneMap == null)
            {
                return NotFound();
            }

            db.ModelZoneMaps.Remove(modelZoneMap);
            db.SaveChanges();

            return Ok(modelZoneMap);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ModelZoneMapExists(int id)
        {
            return db.ModelZoneMaps.Count(e => e.Seq == id) > 0;
        }
    }
}