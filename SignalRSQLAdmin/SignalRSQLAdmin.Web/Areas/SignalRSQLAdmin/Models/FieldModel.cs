using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRSQLAdmin.Web.Areas.SignalRSQLAdmin.Models
{
    public class FieldModel
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public int MaxLength { get; set; }

        public bool isNullable { get; set; }

        public bool isPrimaryKey { get; set; }

    }
}