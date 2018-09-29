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

      

        // GET api/ModelZoneMaps
        [Route("api/ModelZoneMaps")]
        public HttpResponseMessage Get()
        {
            var models = ModelZone2MinQuantityRepository.GetAllModels();
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, models);
            return response;
        }

        [Route("api/ModelZoneMaps/{modelNo?}")]
        public HttpResponseMessage GetByModel(int modelNo)
        {
            var models = ModelZone2MinQuantityRepository.GetAllInfoByModel(modelNo.ToString());
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

        // GET api/ModelZoneMaps
        [Route("api/ModelZoneMaps/zone/{zoneCode:int}")]
        public HttpResponseMessage GetZoneQty(int zoneCode)
        {
            InventoryRepository.GetAllInventory();
            var models = ModelZone2MinQuantityRepository.GetAllModelsZoneQty(zoneCode);
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