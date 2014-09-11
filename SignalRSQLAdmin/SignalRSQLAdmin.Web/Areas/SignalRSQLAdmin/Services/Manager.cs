using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SignalRSQLAdmin.Web.Areas.SignalRSQLAdmin.Services
{
    public abstract partial class Manager : IDisposable
    {
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
            if (IsTrustedConnection)
            {
                return String.Format(@"Server={0};Database={1};Trusted_Connection=True;", _server, _dbName);
            }
            return String.Format(@"Server={0};Database={1};User Id={2};Password={3};", _server, _dbName, _serverUserId, _serverPassword);
        }


        protected Server GetConnectedServer()
        {
            if (_smoServer == null)
            {
                _smoServer = new Server(_server);
                _smoServer.ConnectionContext.LoginSecure = true;
                if(!IsTrustedConnection)
                { 
                    _smoServer.ConnectionContext.LoginSecure = false;
                    _smoServer.ConnectionContext.Login = _serverUserId;
                    _smoServer.ConnectionContext.Password = _serverPassword;
                }             
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