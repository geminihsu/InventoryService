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
    public class ShippingsController : ApiController
    {
        private FGInventoryEntities db = new FGInventoryEntities();

        // GET: api/Shippings
        /*public IQueryable<Shipping> GetShippings()
        {
            return db.Shippings;
        }*/

        // GET api/Shippings
        [Route("api/Shippings")]
        public HttpResponseMessage Get()
        {
            var employees = ShippingRepository.GetAllShippingHistory();
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, employees);
            return response;
        }

        // GET: api/Shippings/5
        [ResponseType(typeof(Shipping))]
        public IHttpActionResult GetShipping(int id)
        {
            Shipping shipping = db.Shippings.Find(id);
            if (shipping == null)
            {
                return NotFound();
            }

            return Ok(shipping);
        }


        [Route("~/api/Shippings/date/{date:regex(\\d{4}-\\d{2}-\\d{2})}")]
        public HttpResponseMessage GetItemsByDate(DateTime date)
        {
            var employees = ShippingRepository.SearchShippingByDate(date);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, employees);
            return response;
        }

        [Route("~/api/Shippings/model/{modelNo:int}")]
        public HttpResponseMessage GetItemsByModel(int modelNo)
        {
            var employees = ShippingRepository.SearchhShippingByModel(modelNo.ToString());
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, employees);
            return response;
        }

        // PUT: api/Shippings/5
        /*[ResponseType(typeof(void))]
        public IHttpActionResult PutShipping(int id, Shipping shipping)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != shipping.Seq)
            {
                return BadRequest();
            }

            db.Entry(shipping).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShippingExists(id))
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

        // POST: api/Shippings
        /*[ResponseType(typeof(Shipping))]
        public IHttpActionResult PostShipping(Shipping shipping)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Shippings.Add(shipping);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ShippingExists(shipping.Seq))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = shipping.Seq }, shipping);
        }*/


        [Route("api/Shippings")]
        public HttpResponseMessage Post(List<Shipping> e)
        {
            //delete inventory items and then insert into shipping table
            var inventory = InventoryRepository.DeleteInventory(e);
            var shipping = ShippingRepository.InsertInventory(e);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, shipping);
            return response;
        }

        // DELETE: api/Shippings/5
        [ResponseType(typeof(Shipping))]
        public IHttpActionResult DeleteShipping(int id)
        {
            Shipping shipping = db.Shippings.Find(id);
            if (shipping == null)
            {
                return NotFound();
            }

            db.Shippings.Remove(shipping);
            db.SaveChanges();

            return Ok(shipping);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ShippingExists(int id)
        {
            return db.Shippings.Count(e => e.Seq == id) > 0;
        }
    }
}