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
    public class CustOrdersController : ApiController
    {
        private FGInventoryEntities db = new FGInventoryEntities();

        // GET: api/CustOrders
        /*public IQueryable<CustOrder> GetCustOrders()
        {
            return db.CustOrders;
        }*/

        /*[Route("api/CustOrders")]
        public HttpResponseMessage Get()
        {
            var employees = CustOrderRepository.GetAllOrder();
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, employees);
            return response;
        }*/

        // GET: api/CustOrders/5
        /*[ResponseType(typeof(CustOrder))]
        public IHttpActionResult GetCustOrder(int id)
        {
            CustOrder custOrder = db.CustOrders.Find(id);
            if (custOrder == null)
            {
                return NotFound();
            }

            return Ok(custOrder);
        }*/

        [Route("~/api/CustOrders/salesOrder/{salesorder}")]
        public HttpResponseMessage GetItemsBySalesOrder(string salesorder)
        {
            var order = ReachTreeRepository.getSalesOrderInfo(salesorder);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, order);
            return response;
        }


        // PUT: api/CustOrders/5
        /* [ResponseType(typeof(void))]
         public IHttpActionResult PutCustOrder(int id, CustOrder custOrder)
         {
             if (!ModelState.IsValid)
             {
                 return BadRequest(ModelState);
             }

             if (id != custOrder.Seq)
             {
                 return BadRequest();
             }

             db.Entry(custOrder).State = EntityState.Modified;

             try
             {
                 db.SaveChanges();
             }
             catch (DbUpdateConcurrencyException)
             {
                 if (!CustOrderExists(id))
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

       /* [Route("api/CustOrders")]
        public HttpResponseMessage Put(List<CustOrder> e)
        {
            var item = CustOrderRepository.UpdateOrder(e);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, item);
            return response;
        }*/

        /*// POST: api/CustOrders
        [ResponseType(typeof(CustOrder))]
        public IHttpActionResult PostCustOrder(CustOrder custOrder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CustOrders.Add(custOrder);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = custOrder.Seq }, custOrder);
        }*/
      /*  [Route("api/CustOrders")]
        public HttpResponseMessage Post(List<CustOrder> e)
        {
            HttpResponseMessage response = null;
            try
            {
                var inventory = CustOrderRepository.InsertOrder(e);
                response = Request.CreateResponse(HttpStatusCode.OK, inventory);
            }
            catch (Exception x)
            {
                string error = x.ToString();
                if (error.Equals("An error occurred while updating the entries. See the inner exception for details."))
                    return new HttpResponseMessage(HttpStatusCode.NotModified);
                else
                    return new HttpResponseMessage(HttpStatusCode.Forbidden);
            }


            return response;
        }*/

        /*// DELETE: api/CustOrders/5
        [ResponseType(typeof(CustOrder))]
        public IHttpActionResult DeleteCustOrder(int id)
        {
            CustOrder custOrder = db.CustOrders.Find(id);
            if (custOrder == null)
            {
                return NotFound();
            }

            db.CustOrders.Remove(custOrder);
            db.SaveChanges();

            return Ok(custOrder);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CustOrderExists(int id)
        {
            return db.CustOrders.Count(e => e.Seq == id) > 0;
        }*/
    }
}