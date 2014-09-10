using SignalRSQLAdmin.Web.Areas.SignalRSQLAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRSQLAdmin.Web.Areas.SignalRSQLAdmin.Hubs
{
	public partial class MainHub
	{
        public void CreateTable(CreateTableModel model)
        {
            if ( !model.IsValid ) throw new InvalidOperationException( "you modafucka" );
			NotifyCreateTableResult( _tableActions.CreateTable( model ) );
            return;
        }
	}
}