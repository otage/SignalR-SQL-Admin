using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRSQLAdmin.Web.Areas.SignalRSQLAdmin.Models
{
    public class TableModel
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public virtual  List<FieldModel> Fields { get; set; }
    }
}