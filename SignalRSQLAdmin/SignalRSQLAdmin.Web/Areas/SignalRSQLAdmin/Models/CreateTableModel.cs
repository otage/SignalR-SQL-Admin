using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRSQLAdmin.Web.Areas.SignalRSQLAdmin.Models
{
    public class CreateTableModel : Model
    {
        public string Name { get; set; }
        public CreateFieldModel[] Fields { get; set; }

        // TODO : real validation
        public override bool Validate()
        {
            return true;
        }
    }
}