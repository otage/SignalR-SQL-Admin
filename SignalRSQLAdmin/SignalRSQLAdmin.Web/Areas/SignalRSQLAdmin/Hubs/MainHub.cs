using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using SignalRSQLAdmin.Web.Areas.SignalRSQLAdmin.Models;
using SignalRSQLAdmin.Web.Areas.SignalRSQLAdmin.Services;

namespace SignalRSQLAdmin.Web.Areas.SignalRSQLAdmin.Hubs
{
    public partial class MainHub : Hub
    {
        ITableActions _tableActions = new TablesManager();


    }
}