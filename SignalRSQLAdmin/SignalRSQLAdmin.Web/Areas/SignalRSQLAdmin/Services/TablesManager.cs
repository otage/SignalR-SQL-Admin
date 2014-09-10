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
            return @"Server=.\SQLEXPRESS;Database="
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

        public TableModel GetTableFromDb(string tableName, string dbName)
        {
            TableModel tm = new TableModel();
            string sqlQuery = "SELECT c.name 'Column Name', t.Name 'Data type', c.max_length 'Max Length', c.is_nullable, ISNULL(i.is_primary_key, 0) 'Primary Key'"
                + "FROM sys.columns c INNER JOIN sys.types t ON c.user_type_id = t.user_type_id"
                + "LEFT OUTER JOIN sys.index_columns ic ON ic.object_id = c.object_id AND ic.column_id = c.column_id"
                + "LEFT OUTER JOIN sys.indexes i ON ic.object_id = i.object_id AND ic.index_id = i.index_id"
                + "WHERE c.object_id = OBJECT_ID('@tableName')";

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
                                fm.MaxLength = reader.GetInt32(2);
                                fm.isNullable = reader.GetInt32(3);
                                fm.isPrimaryKey = reader.GetInt32(4);
                                tm.Fields.Add(fm);
                            }
                        }
                    }
                }
                catch 
                {
                    return tm;
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
    }
}