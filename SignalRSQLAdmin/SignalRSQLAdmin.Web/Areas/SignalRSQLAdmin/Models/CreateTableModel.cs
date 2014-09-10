using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRSQLAdmin.Web.Areas.SignalRSQLAdmin.Models
{
    public class CreateTableModel
    {
        public string Name { get; set; }
        public string[][] Fields { get; set; }

        //TODO !!
        public bool IsValid = true;
    }
}