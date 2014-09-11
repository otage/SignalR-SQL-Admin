using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRSQLAdmin.Web.Areas.SignalRSQLAdmin.Models
{
    public class DeleteTableModel : Model
    {
        public string Database { get; set; }
        public string Name { get; set; }

        //TO DO : IMPLEMENT REAL VALIDATION
        public override bool Validate()
        {
            return true;
        }
    }
}