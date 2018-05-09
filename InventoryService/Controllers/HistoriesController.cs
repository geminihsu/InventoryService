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
    public class HistoriesController : ApiController
    {
        private FGInventoryEntities db = new FGInventoryEntities();

        // GET api/FGInventory
        [Route("api/Histories")]
        public HttpResponseMessage Get()
        {
            var employees = HistoryRepository.GetAllShippingHistory();
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, employees);
            return response;
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


       /* [Route("~/api/Histories/model/{modelNo:int}")]
        public HttpResponseMessage GetItemsByModelNo(int modelNo)
        {
            var employees = HistoryRepository.SearchShippingByModel(modelNo.ToString());
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, employees);
            return response;
        }*/

        [Route("~/api/Histories/salesOrder/{salesOrder}")]
        public HttpResponseMessage GetItemsBySalesOrder(String salesOrder)
        {
            var employees = HistoryRepository.SearchShippingBySalesOrder(salesOrder);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, employees);
            return response;
        }

        [Route("~/api/Histories/date/{date:datetime}")]
        public HttpResponseMessage GetItemsByDate(DateTime date)
        {
            var employees = HistoryRepository.SearchShippingByDate(date);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, employees);
            return response;
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

        /*// POST: api/Histories
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
        }*/

        [Route("api/Histories")]
        public HttpResponseMessage Post(List<History> e)
        {
            HttpResponseMessage response = null;
            try
            {

                var inventory = InventoryRepository.DeleteInventory(e);
                //var inventory = HistoryRepository.InsertInventory(e);
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