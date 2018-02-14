using InventoryService.Controllers.DbUtil;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace InventoryService.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            //ReachTreeRepository.getSalesOrderInfo("M73229");
            return View();
        }

      
    }


}
