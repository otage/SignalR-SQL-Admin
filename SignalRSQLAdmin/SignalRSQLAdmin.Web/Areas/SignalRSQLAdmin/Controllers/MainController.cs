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

            ITableReader tableReader = new TablesManager(dbName);

            List<TableModel> tables = tableReader.GetTables();

            TableModel table = tableReader.GetTableInfo(tableName);

            
            List<string> listOfTypes = tableReader.GetListOfDbType();

            ViewData["dbName"] = dbName;
            ViewData["tablesList"] = tables;
            ViewData["listOfTypes"] = listOfTypes;
            return View("~/Areas/SignalRSQLAdmin/Views/Main/Index.cshtml");
        }

        public ActionResult DisplayLeftSideBar()
        {
            string dbName = "TestSignalR";

            ITableReader tableReader = new TablesManager(dbName);

            List<TableModel> tables = tableReader.GetTables();


            ViewData["dbName"] = dbName;
            return PartialView("_LeftSideBar", tables);
        }

        public ActionResult DisplaySelectedTable(string id)
        {   
            string dbName = "TestSignalR";

            ITableReader tableReader = new TablesManager(dbName);

            TableModel table = tableReader.GetTableInfo(id);
            if (table.Name == null)
                table = null;
            ViewData["dbName"] = dbName;
            return PartialView("Index", table);
        }

        public void CreateTable()
        {
            // Call Notify and give Returned id
            
        }
    }
}