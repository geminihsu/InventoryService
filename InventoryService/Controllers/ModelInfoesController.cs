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
    public class ModelInfoesController : ApiController
    {
        private FGInventoryEntities db = new FGInventoryEntities();

        /* // GET: api/ModelInfoes
         public IQueryable<ModelInfo> GetModelInfoes()
         {
             return db.ModelInfoes;
         }*/

        // GET api/ModelInfoes
        [Route("api/ModelInfoes")]
        public HttpResponseMessage Get()
        {
            var models = ReachTreeRepository.getModelInfo();

            //var models = ModelInfoRepository.GetAllModel();
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, models);
            return response;
        }

        /* // GET: api/ModelInfoes/5
         [ResponseType(typeof(ModelInfo))]
         public IHttpActionResult GetModelInfo(int id)
         {
             ModelInfo modelInfo = db.ModelInfoes.Find(id);
             if (modelInfo == null)
             {
                 return NotFound();
             }

             return Ok(modelInfo);
         }*/

        [Route("~/api/ModelInfoes/{modelNo?}")]
        public HttpResponseMessage GetModelByNo(string modelNo)
        {
            var model = ModelInfoRepository.GetModelByNo(modelNo.ToString());
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, model);
            return response;
        }


        
        // POST: api/ModelInfoes
        [ResponseType(typeof(ModelInfo))]
        public IHttpActionResult PostModelInfo(ModelInfo modelInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ModelInfoes.Add(modelInfo);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = modelInfo.Seq }, modelInfo);
        }

        // DELETE: api/ModelInfoes/5
        [ResponseType(typeof(ModelInfo))]
        public IHttpActionResult DeleteModelInfo(int id)
        {
            ModelInfo modelInfo = db.ModelInfoes.Find(id);
            if (modelInfo == null)
            {
                return NotFound();
            }

            db.ModelInfoes.Remove(modelInfo);
            db.SaveChanges();

            return Ok(modelInfo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ModelInfoExists(int id)
        {
            return db.ModelInfoes.Count(e => e.Seq == id) > 0;
        }
    }
}