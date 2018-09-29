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
    public class ContainersController : ApiController
    {
        private FGInventoryEntities db = new FGInventoryEntities();

        [Route("api/Containers")]
        public HttpResponseMessage Get()
        {
            var employees = ContainerRepository.GetAllContainers();
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, employees);
            return response;
        }

        [Route("~/api/Containers/{containerNo}")]
        public HttpResponseMessage GetItemsByContainerNo(String containerNo)
        {
            var employees = ContainerRepository.GetContainer(containerNo);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, employees);
            return response;
        }

        [Route("~/api/Containers/date/{date:datetime}")]
        public HttpResponseMessage GetItemsByDate(DateTime date)
        {
            var employees = ContainerRepository.GetContainerByDate(date);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, employees);
            return response;
        }

        [Route("api/Containers")]
        public HttpResponseMessage Put(List<Container> e)
        {
            var item = ContainerRepository.UpdateContainer(e);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, item);
            return response;
        }

      

        [Route("api/Containers")]
        public HttpResponseMessage Post(List<Container> e)
        {
            HttpResponseMessage response = null;
            try
            {
                var inventory = new List<Container>();

                if (!ContainerRepository.isSNexsit(e))
                {
                    inventory = ContainerRepository.InsertContainer(e);
                    response = Request.CreateResponse(HttpStatusCode.OK, inventory);
                }
                else
                {
                    response = Request.CreateResponse(HttpStatusCode.Conflict, inventory);
                }

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

        // DELETE: api/Containers/5
       [Route("api/Containers/{Seq:int}")]
       public HttpResponseMessage Delete(int Seq)
       {
           var employees = ContainerRepository.DeleteContainer(Seq);
           HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, employees);
           return response;
       }


        // DELETE: api/Containers/5
        [Route("api/Containers/")]
        public HttpResponseMessage Delete(List<Container> e)
        {
            var employees = ContainerRepository.DeleteContainer(e);
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

        private bool ContainerExists(int id)
        {
            return db.Containers.Count(e => e.Seq == id) > 0;
        }
    }
}