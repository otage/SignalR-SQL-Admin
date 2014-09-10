using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRSQLAdmin.Web.Areas.SignalRSQLAdmin.Models
{
    public class CreateTableModel
    {
        string Name { get; set; }
        Dictionary<string, string> Fields { get; set; }
    }
}