using SignalRSQLAdmin.Web.Areas.SignalRSQLAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRSQLAdmin.Web.Areas.SignalRSQLAdmin.Services
{
    interface ITableActions : IDisposable
    {
        CreateTableResult CreateTable( CreateTableModel model );
        DeleteTableResult DeleteTable( DeleteTableModel model );
    }
}
