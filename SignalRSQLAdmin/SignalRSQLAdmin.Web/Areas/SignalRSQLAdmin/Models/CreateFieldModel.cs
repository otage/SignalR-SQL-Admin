using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRSQLAdmin.Web.Areas.SignalRSQLAdmin.Models
{
    public class CreateFieldModel : Model
    {
        public string Name { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsNullable { get; set; }
        public string Type { get; set; }
        public Int16 MaxLength { get; set; }
    }
}