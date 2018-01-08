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
    public class FGInventoryController : ApiController
    {
        private FGInventoryEntities db = new FGInventoryEntities();



       /* // GET: api/FGInventory
        public IQueryable<InventoryIn> GetInventoryIns()
        {
            return db.InventoryIns;
        }*/


      

        // GET api/FGInventory
        [Route("api/FGInventory")]
        public HttpResponseMessage Get()
        {
            var employees = InventoryRepository.GetAllInventory();
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, employees);
            return response;
        }


        // GET: api/FGInventory/5
        [ResponseType(typeof(InventoryIn))]
        public IHttpActionResult GetInventoryIn(int id)
        {
            InventoryIn inventoryIn = db.InventoryIns.Find(id);
            if (inventoryIn == null)
            {
                return NotFound();
            }

            return Ok(inventoryIn);
        }

        [Route("~/api/FGInventory/location/{location:int}")]
        public HttpResponseMessage GetItemsByLocation(int location)
        {
            var employees = InventoryRepository.SearchInventoryByLocation(location.ToString());
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, employees);
            return response;
        }

        [Route("~/api/FGInventory/model/{modelNo:int}")]
        public HttpResponseMessage GetItemsByModelNo(int modelNo)
        {
            var employees = InventoryRepository.SearchInventoryByModel(modelNo.ToString());
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, employees);
            return response;
        }



        /*// PUT: api/FGInventory/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutInventoryIn(int id, InventoryIn inventoryIn)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != inventoryIn.Seq)
            {
                return BadRequest();
            }

            db.Entry(inventoryIn).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InventoryInExists(id))
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

        // POST: api/FGInventory
        /*[ResponseType(typeof(InventoryIn))]
        public IHttpActionResult PostInventoryIn(InventoryIn inventoryIn)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.InventoryIns.Add(inventoryIn);

            try
            {
                db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }

                if (InventoryInExists(inventoryIn.Seq))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = inventoryIn.Seq }, inventoryIn);
        }*/

        // POST: api/FGInventory
        /*[ResponseType(typeof(List<InventoryIn>))]
        public IHttpActionResult PostInventoryIn(List<InventoryIn> inventoryIn)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.InventoryIns.AddRange(inventoryIn);

            try
            {
                db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }

                
            }

            return Ok(inventoryIn);
        }*/

        [Route("api/FGInventory")]
        public HttpResponseMessage Post(List<InventoryIn> e)
        {
            var employees = InventoryRepository.InsertInventory(e);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, employees);
            return response;
        }

        /*[Route("api/FGInventory")]
         public HttpResponseMessage Post(InventoryIn e)
         {
             var employees = InventoryRepository.InsertInventory(e);
             HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, employees);
             return response;
         }*/

       /* [Route("api/FGInventory")]
        public HttpResponseMessage Put(InventoryIn e)
        {
            var item = InventoryRepository.UpdateInventory(e);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, item);
            return response;
        }*/

        [Route("api/FGInventory")]
        public HttpResponseMessage Put(List<InventoryIn> e)
        {
            var item = InventoryRepository.UpdateInventory(e);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, item);
            return response;
        }



        // DELETE: api/FGInventory/5
        /*[ResponseType(typeof(InventoryIn))]
        public IHttpActionResult DeleteInventoryIn(int id)
        {
            InventoryIn inventoryIn = db.InventoryIns.Find(id);
            if (inventoryIn == null)
            {
                return NotFound();
            }

            db.InventoryIns.Remove(inventoryIn);
            db.SaveChanges();

            return Ok(inventoryIn);
        }*/

        [Route("api/FGInventory")]
        public HttpResponseMessage Delete(InventoryIn e)
        {
            var employees = InventoryRepository.DeleteInventory(e);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, employees);
            return response;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool InventoryInExists(int id)
        {
            return db.InventoryIns.Count(e => e.Seq == id) > 0;
        }
    }
}