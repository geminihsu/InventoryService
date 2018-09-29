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


     
        [Route("~/api/Histories/serialNo/{serialNo}")]
        public HttpResponseMessage GetItemsBySN(String serialNo)
        {
            var employees = HistoryRepository.SearchSNHistory(serialNo);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, employees);
            return response;
        }

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


        [Route("~/api/Histories/dailyship/{date:datetime}")]
        public HttpResponseMessage GetDateShip(DateTime date)
        {
            var employees = HistoryRepository.SearchDailyShipping(date);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, employees);
            return response;
        }

      

        [Route("api/Histories")]
        public HttpResponseMessage Post(List<History> e)
        {
            HttpResponseMessage response = null;
            try
            {

                var SO = InventoryRepository.DeleteInventory(e);
                var shipping = HistoryRepository.SearchShippingBySalesOrder(SO);
                response = Request.CreateResponse(HttpStatusCode.OK, shipping);
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