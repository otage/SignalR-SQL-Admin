using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SignalRSQLAdmin.Web.Areas.SignalRSQLAdmin.Services
{
    public abstract class Manager : IDisposable
    {
        private static string _server = @".\SQLEXPRESS";
        private static string _serverUserId = "sa";
        private static string _serverPassword = "vii2s8di";       

        private readonly string _dbName;
        Server _smoServer;
        Database _smoDatabase;
        SqlConnection _sqlConnection;

        public Manager(string dbName)
        {
            _dbName = dbName;
        }

        private string GetConnectionString()
        {
            // Will return the correct ConnectionString with the correct DB given
            // TODO : May be check dbname before made the concat..

            return @"Server=" + _server + ";Database="
                + _dbName + "; Trusted_Connection=True;";
        }

        protected Server GetConnectedServer()
        {
            if (_smoServer == null)
            {
                _smoServer = new Server(_server);
                _smoServer.ConnectionContext.LoginSecure = true;
                //_smoServer.ConnectionContext.Login = _serverUserId;
                //_smoServer.ConnectionContext.Password = _serverPassword;
                _smoServer.ConnectionContext.Connect();
            }
            return _smoServer;
        }

        protected Database GetDatabase()
        {
            return _smoDatabase ?? (_smoDatabase = GetConnectedServer().Databases[_dbName]);
        }

        protected SqlConnection GetOpenConnection()
        {
            if (_sqlConnection == null)
            {
                _sqlConnection = new SqlConnection(GetConnectionString());
                _sqlConnection.Open();
            }
            return _sqlConnection;
        }

        public void Dispose()
        {
            if( _smoServer != null ) 
            {
                if (_smoServer.ConnectionContext.IsOpen)
                    _smoServer.ConnectionContext.Disconnect();
                _smoServer = null;
            }

            if (_sqlConnection != null)
            {
                if (_sqlConnection.State == ConnectionState.Open)
                    _sqlConnection.Close();
                _sqlConnection = null;                  
            }
        }
    }
}