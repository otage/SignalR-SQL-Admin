using SignalRSQLAdmin.Web.Areas.SignalRSQLAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRSQLAdmin.Web.Areas.SignalRSQLAdmin.Services
{
    public class CreateTableResult : Result
    {
        public CreateTableResult()
        {
            this.TableModel = new TableModel();
        }
        public TableModel TableModel { get; set; }
    }
}