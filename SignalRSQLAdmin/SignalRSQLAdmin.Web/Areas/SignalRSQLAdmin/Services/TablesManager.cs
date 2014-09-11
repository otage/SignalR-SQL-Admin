using Microsoft.SqlServer.Management.Smo;
using SignalRSQLAdmin.Web.Areas.SignalRSQLAdmin.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SignalRSQLAdmin.Web.Areas.SignalRSQLAdmin.Services
{
    public class TablesManager : Manager, ITableReader, ITableActions
    {

        public TablesManager( string dbName )
            : base( dbName )
        {
                 
        }

        public List<string> GetListOfDbType()
        {
            string sqlQuery = "SELECT name FROM sys.types";
            List<string> result = new List<string>();
            SqlConnection connection = GetOpenConnection();

            try
            {
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            result.Add(reader.GetString(0));
                        }
                    }
                }
            }
            catch { }
            return result;
        }

        public List<TableModel> GetTables()
        {
            List<TableModel> TableModels = new List<TableModel>();
            CreateTableResult result = new CreateTableResult();
            SqlConnection connection = GetOpenConnection();
            try
            {
                string sqlQuery = "SELECT * FROM information_schema.tables";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))

                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        TableModel tm = new TableModel();
                        tm.Name = reader.GetString(2);
                        tm.Type = reader.GetString(3);
                        TableModels.Add(tm);
                    }
                }
            }
            catch { }          
            return TableModels;
        }

        public List<Array> GetTable(string tableName)
        { 
            List<Array> result = new List<Array>();
            string sqlQuery = "SELECT * FROM " + tableName;
            SqlConnection connection = GetOpenConnection();
            try
            {
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            Object[] tmp = new Object[reader.FieldCount];
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                tmp[i] = reader[i];
                            }
                            result.Add(tmp);
                        }
                    }
                }
            }
            catch { }
            return result;

        }

        public TableModel GetTableInfo(string tableName)
        {
            TableModel tm = new TableModel();
            string sqlQuery = "SELECT c.name 'Column Name', t.Name 'Data type', c.max_length 'Max Length', c.is_nullable, ISNULL(i.is_primary_key, 0) 'Primary Key' "
                + " FROM sys.columns c INNER JOIN sys.types t ON c.user_type_id = t.user_type_id"
                + " LEFT OUTER JOIN sys.index_columns ic ON ic.object_id = c.object_id AND ic.column_id = c.column_id"
                + " LEFT OUTER JOIN sys.indexes i ON ic.object_id = i.object_id AND ic.index_id = i.index_id"
                + " WHERE c.object_id = OBJECT_ID(@tableName)";
            SqlConnection connection = GetOpenConnection();

            try
            {
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@tableName";
                    param.Value = tableName;
                    command.Parameters.Add(param);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        tm.Name = tableName;
                        while (reader.Read())
                        {
                            FieldModel fm = new FieldModel();
                            fm.Name = reader.GetString(0);
                            fm.Type = reader.GetString(1);
                            fm.MaxLength = reader.GetInt16(2);
                            fm.IsNullable = reader.GetBoolean(3);
                            fm.IsPrimaryKey = reader.GetBoolean(4);
                            tm.Fields.Add(fm);
                        }
                        tm.FirstRows = GetTable(tableName);
                    }
                }
            }
            catch { }
            return tm;
        }

        public CreateTableResult CreateTable( CreateTableModel model )
        {
            CreateTableResult result  = new CreateTableResult();

            try
            {
                Database myDatabase = GetDatabase();
                Table myEmpTable = new Table( myDatabase, model.Name );
                
                result.TableModel.Name = model.Name;

                result.TableModel.Name = model.Name;
                foreach (var f in model.Fields)
                {
                    var field = new FieldModel();
                    field.Name = f.Name;
                    result.TableModel.Fields.Add(field);
                    


                    // check Attr Datatype
                    Type t = typeof(DataType);
                    PropertyInfo p = t.GetProperty(f.Type, BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public);
                    var dt = (DataType)p.GetGetMethod().Invoke( null, new object[0]);

                    
                    Column tmpField = new Column(myEmpTable, f.Name, dt);
                    if (f.IsPrimaryKey)
                        tmpField.Identity = true;
                    if (f.IsNullable)
                        tmpField.Nullable = true;
                    myEmpTable.Columns.Add(tmpField);
                }

                myEmpTable.Create();
                // TODO : add other informations to tablemodel's result.
                result.TableModel = new TableModel();
                result.TableModel.Name = model.Name;
                result.TableModel.Type = "Table";

                foreach (var field in model.Fields)
                {
                    var f = new FieldModel();
                    f.Name = field.Name;
                    f.Type = field.Type;
                    f.IsNullable = field.IsNullable;
                    f.IsPrimaryKey = field.IsPrimaryKey;
                    f.MaxLength = field.MaxLength;
                    result.TableModel.Fields.Add(f);
                }
            }
           

            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
            }
            return result;
        }

        public DeleteTableResult DeleteTable(DeleteTableModel model)
        {
            DeleteTableResult result = new DeleteTableResult();

            try
            {
                Database db = GetDatabase();
                db.Tables[model.Name].Drop();

                result.Name = model.Name;
            }

            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
            }
            return result;
        }
    }
}