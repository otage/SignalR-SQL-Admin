using SignalRSQLAdmin.Web.Areas.SignalRSQLAdmin.Models;
using SignalRSQLAdmin.Web.Areas.SignalRSQLAdmin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRSQLAdmin.Web.Areas.SignalRSQLAdmin.Hubs
{
	public partial class MainHub
	{
        public CreateTableResult CreateTable(CreateTableModel model)
        {
            CreateTableResult result;
            if (!model.Validate()) throw new InvalidOperationException("you modafucka");
            using (ITableActions _tableActions = new TablesManager(DbName))
            {   
               result = _tableActions.CreateTable(model);
               NotifyCreateTableResult( result );
            }
            return result;
        }

        public void DeleteTable(DeleteTableModel model)
        {
            if (!model.Validate()) throw new InvalidOperationException("you modafucka");
            using (ITableActions _tableActions = new TablesManager(DbName))
            {
                NotifyDeleteTableResult(_tableActions.DeleteTable(model));
            }
        }
	}
}