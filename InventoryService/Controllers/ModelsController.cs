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
    public class ModelsController : ApiController
    {
        private FGInventoryEntities db = new FGInventoryEntities();

        // GET: api/Models
        /*public IQueryable<MODEL> GetMODELs()
        {
            return db.MODELs;
        }*/

        // GET api/Models
        [Route("api/Models")]
        public HttpResponseMessage Get()
        {
            var models = ModelsRepository.GetAllModelDto();
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, models);
            return response;
        }

        // GET: api/Models/5
        /*[ResponseType(typeof(MODEL))]
        public IHttpActionResult GetMODEL(string id)
        {
            MODEL mODEL = db.MODELs.Find(id);
            if (mODEL == null)
            {
                return NotFound();
            }

            return Ok(mODEL);
        }*/

        [Route("~/api/Models/{modelNo?}")]
        public HttpResponseMessage GetModelByNo(int modelNo)
        {
            var model = ModelsRepository.GetModelDtoByModeNo(modelNo.ToString());
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, model);
            return response;
        }

        // PUT: api/Models/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMODEL(string id, MODEL mODEL)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != mODEL.MODELNO)
            {
                return BadRequest();
            }

            db.Entry(mODEL).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MODELExists(id))
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

        // POST: api/Models
        [ResponseType(typeof(MODEL))]
        public IHttpActionResult PostMODEL(MODEL mODEL)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.MODELs.Add(mODEL);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (MODELExists(mODEL.MODELNO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = mODEL.MODELNO }, mODEL);
        }

        // DELETE: api/Models/5
        [ResponseType(typeof(MODEL))]
        public IHttpActionResult DeleteMODEL(string id)
        {
            MODEL mODEL = db.MODELs.Find(id);
            if (mODEL == null)
            {
                return NotFound();
            }

            db.MODELs.Remove(mODEL);
            db.SaveChanges();

            return Ok(mODEL);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MODELExists(string id)
        {
            return db.MODELs.Count(e => e.MODELNO == id) > 0;
        }
    }
}