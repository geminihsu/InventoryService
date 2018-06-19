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
    public class PalletsController : ApiController
    {
        private FGInventoryEntities db = new FGInventoryEntities();

        [Route("api/Pallets")]
        public HttpResponseMessage Get()
        {
            var employees = PalletRepository.GetAllPallets();
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, employees);
            return response;
        }


        [Route("~/api/Pallets/{salesOrder}")]
        public HttpResponseMessage GetItemsBySalesOrder(String salesOrder)
        {
            var employees = PalletRepository.GetPalletBySalesOrder(salesOrder);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, employees);
            return response;
        }

        [Route("~/api/Pallets/date/{date:datetime}")]
        public HttpResponseMessage GetItemsByDate(DateTime date)
        {
            var employees = PalletRepository.GetPalletByDate(date);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, employees);
            return response;
        }

        [Route("api/Pallets")]
        public HttpResponseMessage Put(Pallet e)
        {
            var item = PalletRepository.UpdatePallet(e);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, item);
            return response;
        }


        [Route("api/Pallets")]
        public HttpResponseMessage Post(List<Pallet> e)
        {
            HttpResponseMessage response = null;
            try
            {
                var inventory = new List<Pallet>();


                inventory = PalletRepository.InsertPallet(e);
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

        // DELETE: api/Pallets/5
        [ResponseType(typeof(Pallet))]
        public IHttpActionResult DeletePallet(int id)
        {
            Pallet pallet = db.Pallets.Find(id);
            if (pallet == null)
            {
                return NotFound();
            }

            db.Pallets.Remove(pallet);
            db.SaveChanges();

            return Ok(pallet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PalletExists(int id)
        {
            return db.Pallets.Count(e => e.Seq == id) > 0;
        }
    }
}