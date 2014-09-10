using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using SignalRSQLAdmin.Web.Areas.SignalRSQLAdmin.Models;
using SignalRSQLAdmin.Web.Areas.SignalRSQLAdmin.Services;

namespace SignalRSQLAdmin.Web.Areas.SignalRSQLAdmin.Hubs
{
    public class MainHub : Hub
    {
        ITableActions _tableActions = new TablesManager();

        public void Hello()
        {
            Clients.All.fuckALLClients("Migi et Antoine");
        }

        public void Notify( string message )
        {
            Clients.All.displayMessage( message );
        }

        public TableActionResult CreateTable(CreateTableModel model)
        {
            if (model.IsValid)
            {
                return _tableActions.CreateTable(model);
            }
            Notify("Model not valid");
        }
    }
}