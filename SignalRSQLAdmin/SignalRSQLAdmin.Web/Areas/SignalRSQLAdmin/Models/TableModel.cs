using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRSQLAdmin.Web.Areas.SignalRSQLAdmin.Models
{
    public class TableModel
    {
        public TableModel()
        {
            this.Fields = new List<FieldModel>();
            this.FirstRows = new List<Array>();

        }
        public string Name { get; set; }

        public string Type { get; set; }

        public List<FieldModel> Fields { get; set; }

        public  List<Array> FirstRows { get; set; }
    }
}