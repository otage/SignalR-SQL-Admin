using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRSQLAdmin.Web.Areas.SignalRSQLAdmin.Services
{
    public abstract partial class Manager
    {
        const string _server = @".\SQLEXPRESS";
        const string _serverUserId = "sa";
        const string _serverPassword = "vii2s8di";
        bool IsTrustedConnection = true;
    }
}