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

       
    }
}