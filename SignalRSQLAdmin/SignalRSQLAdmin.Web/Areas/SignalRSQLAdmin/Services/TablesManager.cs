using Microsoft.SqlServer.Management.Smo;
using SignalRSQLAdmin.Web.Areas.SignalRSQLAdmin.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace SignalRSQLAdmin.Web.Areas.SignalRSQLAdmin.Services
{
    public class TablesManager : ITableReader, ITableActions
    {
        private static string _server = @"ASUS-SANTI";
        //private static string _serverUserId = "sa";
        //private static string _serverPassword = "vii2s8di";

        private static string GetConnectionString(string dbName)
        {
            // Will return the correct ConnectionString with the correct DB given
            // TODO : May be check dbname before made the concat..

            return @"Server=" + _server + ";Database="
                + dbName + "; Trusted_Connection=True;";
        }

        public List<string> GetListOfDbType( string dbName )
        {
            string sqlQuery = "SELECT name FROM sys.types";
            List<string> result = new List<string>(); 

            using (SqlConnection connection = new SqlConnection(GetConnectionString(dbName)))
            {
                try
                {
                    connection.Open();
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
               finally 
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
            return result;
        }

        public List<TableModel> GetTablesFromDb(string dbName)
        {
            List<TableModel> TableModels = new List<TableModel>();
            CreateTableResult result = new CreateTableResult();
            using ( SqlConnection connection = new SqlConnection( GetConnectionString( dbName ) ) )
            {
                try
                {
                    connection.Open();
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
                catch 
                {
                    return TableModels;
                }
                finally 
                {
                    if ( connection.State == ConnectionState.Open )
                    {
                        connection.Close();
                    }
                }
            }
            return TableModels;
        }

        public List<Array> GetTable(string tableName, string dbName)
        { 
            List<Array> result = new List<Array>();
            string sqlQuery = "SELECT * FROM " + tableName;

            using (SqlConnection connection = new SqlConnection(GetConnectionString(dbName)))
            {
                try
                {
                    connection.Open();
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
                                result.Add( tmp );
                            }
                        }
                    }
                }
                catch
                {
                    return result;
                }

                finally 
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
            return result;

        }

        public TableModel GetTableInfoFromDb(string tableName, string dbName)
        {
            TableModel tm = new TableModel();
            string sqlQuery = "SELECT c.name 'Column Name', t.Name 'Data type', c.max_length 'Max Length', c.is_nullable, ISNULL(i.is_primary_key, 0) 'Primary Key' "
                + " FROM sys.columns c INNER JOIN sys.types t ON c.user_type_id = t.user_type_id"
                + " LEFT OUTER JOIN sys.index_columns ic ON ic.object_id = c.object_id AND ic.column_id = c.column_id"
                + " LEFT OUTER JOIN sys.indexes i ON ic.object_id = i.object_id AND ic.index_id = i.index_id"
                + " WHERE c.object_id = OBJECT_ID(@tableName)";

            using ( SqlConnection connection = new SqlConnection( GetConnectionString( dbName ) ) )
            {
                try
                {
                    connection.Open();
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
                            tm.FirstRows = GetTable(tableName, dbName);
                        }
                    }
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
            return tm;
        }

        public CreateTableResult CreateTable( CreateTableModel model )
        {
            string dbName = "TestSignalR";
            CreateTableResult result  = new CreateTableResult();

            Server myServer = new Server(_server);
            myServer.ConnectionContext.LoginSecure = true;
            //myServer.ConnectionContext.Login = "sa";
            //myServer.ConnectionContext.Password = "vii2s8di";

            // If using a Secure Connection
            // myServer.ConnectionContext.LoginSecure = true;

            try
            {
                myServer.ConnectionContext.Connect();
                Database myDatabase;
                myDatabase = myServer.Databases[dbName]; 
                Table myEmpTable = new Table( myDatabase, model.Name );

                result.TableModel.Name = model.Name;
                foreach (var f in model.Fields)
                {
                    var field = new FieldModel();
                    field.Name = f.Name;
                    result.TableModel.Fields.Add(field);
                    //Need to select the Type
                    var dataType = DataType.Int;
                    Column tmpField = new Column(myEmpTable, f.Name, dataType);
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

            finally
            {
                if (myServer.ConnectionContext.IsOpen)
                    myServer.ConnectionContext.Disconnect();
            }
            return result;
        }

        public DeleteTableResult DeleteTable(DeleteTableModel model)
        {
            string dbName = "TestSignalR";
            DeleteTableResult result = new DeleteTableResult();

            Server myServer = new Server(_server);
            myServer.ConnectionContext.LoginSecure = true;
            //myServer.ConnectionContext.Login = "sa";
            //myServer.ConnectionContext.Password = "vii2s8di";

            // If using a Secure Connection
            // myServer.ConnectionContext.LoginSecure = true;

            try
            {
                myServer.ConnectionContext.Connect();
                Database db = myServer.Databases[dbName];
                db.Tables[model.Name].Drop();

                result.Name = model.Name;
            }

            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
            }

            finally
            {
                if (myServer.ConnectionContext.IsOpen)
                    myServer.ConnectionContext.Disconnect();
            }
            return result;
        }
    }
}