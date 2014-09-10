using Microsoft.SqlServer.Management.Smo;
using SignalRSQLAdmin.Web.Areas.SignalRSQLAdmin.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SignalRSQLAdmin.Web.Areas.SignalRSQLAdmin.Services
{
    public class TablesManager : ITableReader, ITableActions
    {


        private static string GetConnectionString(string dbName)
        {
            // Will return the correct ConnectionString with the correct DB given
            // TODO : May be check dbname before made the concat..
<<<<<<< Updated upstream
            return @"Server=.\SQLEXPRESS;Database="
=======
            return @"Server=ASUS-SANTI;Database="
>>>>>>> Stashed changes
                + dbName
                + ";User Id=sa;Password=vii2s8di;";
        }

        public List<TableModel> GetTablesFromDb(string dbName)
        {
            List<TableModel> TableModels = new List<TableModel>();
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
                                fm.isNullable = reader.GetBoolean(3);
                                fm.isPrimaryKey = reader.GetBoolean(4);
                                tm.Fields.Add(fm);
                            }
                            tm.FirstRows = GetTable( tableName, dbName );
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
            string dbName = "master";
            CreateTableResult result  = new CreateTableResult();

            Server myServer = new Server( @".\SQLEXPRESS" );
            myServer.ConnectionContext.LoginSecure = false;
            myServer.ConnectionContext.Login = "sa";
            myServer.ConnectionContext.Password = "vii2s8di";

            // If using a Secure Connection
            // myServer.ConnectionContext.LoginSecure = true;

            try
            {
                myServer.ConnectionContext.Connect();
                Database myDatabase = new Database( myServer, dbName );
                Table myEmpTable = new Table( myDatabase, model.Name );

                foreach ( var f in model.Fields )
                {
                    var dataType =  DataType.Int;
                    Column tmpField = new Column( myEmpTable, f.Name , dataType );
                    if ( f.IsPrimaryKey )
                        tmpField.Identity = true;
                    if ( f.IsNullable )
                        tmpField.Nullable = true;
                }
            }
           
            catch 
            {
                result.ErrorMessage = "Something went wrong...Noob.";
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