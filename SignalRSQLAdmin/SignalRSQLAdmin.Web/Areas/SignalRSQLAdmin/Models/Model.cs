using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRSQLAdmin.Web.Areas.SignalRSQLAdmin.Models
{
    public abstract class Model
    {
        public virtual bool Validate() 
        {
            throw new NotImplementedException();
        }
    }
}