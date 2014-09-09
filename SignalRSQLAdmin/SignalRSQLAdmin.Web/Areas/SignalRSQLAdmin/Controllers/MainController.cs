using SignalRSQLAdmin.Web.Areas.SignalRSQLAdmin.Models;
using SignalRSQLAdmin.Web.Areas.SignalRSQLAdmin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace SignalRSQLAdmin.Web.Areas.SignalRSQLAdmin.Controllers
{
    public class MainController : Controller
    {
        public ActionResult Index()
        {
            string dbName = "master";
            ITableReader tableReader = new Tables();
            List<TableModel> tables = tableReader.GetTablesFromDb(dbName);

            ViewData["dbName"] = dbName;
            return View(tables);
        }
 

        public void CreateTable()
        {
            // Call Notify and give Returned id
            
        }
    }
}