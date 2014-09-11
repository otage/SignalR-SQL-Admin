using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRSQLAdmin.Web.Areas.SignalRSQLAdmin.Services
{
    public abstract partial class Manager
    {
        string _server = @".\SQLEXPRESS";
        string _serverUserId = "sa";
        string _serverPassword = "vii2s8di";

        protected bool IsTrustedConnection = true;

        private string GetConnectionString()
        {
            return String.Format(@"Server={0};Database={1};dTrusted_Connection=True;", _server, _dbName);
        }
    }
}