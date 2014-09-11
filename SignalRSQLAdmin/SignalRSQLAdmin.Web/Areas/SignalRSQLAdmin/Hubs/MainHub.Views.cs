using SignalRSQLAdmin.Web.Areas.SignalRSQLAdmin.Models;
using SignalRSQLAdmin.Web.Areas.SignalRSQLAdmin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRSQLAdmin.Web.Areas.SignalRSQLAdmin.Hubs
{
    public partial class MainHub
    {
        public void NotifyCreateTableResult( CreateTableResult result )
        {
            if (String.IsNullOrEmpty(result.ErrorMessage))
            {
                Clients.All.notifyCreateTableResult( result );
                return;
            }
            Clients.Caller.notifyCreateTableResult( result );
        }

        public void NotifyDeleteTableResult(DeleteTableResult result)
        {
            Clients.All.notifyDeleteTableResult( result );
        }

        public void DisplayTableResult( string tableName )
        {
            ITableReader tableReader = new TablesManager();
            TableModel table = tableReader.GetTableInfoFromDb(tableName, "TestSignalR");
            Clients.Caller.displaySelectedTable( table );
        }
    }
}