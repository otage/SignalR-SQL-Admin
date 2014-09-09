using SignalRSQLAdmin.Web.Areas.SignalRSQLAdmin.Models;
using SignalRSQLAdmin.Web.Areas.SignalRSQLAdmin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SignalRSQLAdmin.Web.Areas.SignalRSQLAdmin.Controllers
{
    public class MainController : Controller
    {
        public ActionResult Index()
        {
            List<TableModel> tables = ITableReader.GetTablesFromDb( "master" );
            ViewData["tables"] = tables;
            return View();
        }
 

        public void CreateTable()
        {
            // Call Notify and give Returned id
            
        }
    }
}