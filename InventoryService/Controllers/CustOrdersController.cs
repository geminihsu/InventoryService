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

       
        [Route("~/api/CustOrders/salesOrder/{salesorder}")]
        public HttpResponseMessage GetItemsBySalesOrder(string salesorder)
        {
            var order = ReachTreeRepository.getSalesOrderInfo(salesorder);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, order);
            return response;
        }


        
    }
}