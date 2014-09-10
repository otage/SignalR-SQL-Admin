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
            Clients.All.notifyCreateTableResult( result );
        }
    }
}