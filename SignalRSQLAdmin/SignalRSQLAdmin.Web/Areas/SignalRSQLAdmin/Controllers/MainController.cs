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
            string dbName = "TestSignalR";
            string tableName = "dbo.MSreplication_options";

            ITableReader tableReader = new TablesManager();

            List<TableModel> tables = tableReader.GetTablesFromDb(dbName);
            TableModel table = tableReader.GetTableInfoFromDb(tableName, dbName);


            ViewData["dbName"] = dbName;
            ViewData["tablesList"] = tables;
            return View(table);
        }

        public ActionResult DisplayLeftSideBar()
        {
            string dbName = "TestSignalR";

            ITableReader tableReader = new TablesManager();

            List<TableModel> tables = tableReader.GetTablesFromDb(dbName);


            ViewData["dbName"] = dbName;
            return PartialView("_LeftSideBar", tables);
        }

        public void CreateTable()
        {
            // Call Notify and give Returned id
            
        }
    }
}