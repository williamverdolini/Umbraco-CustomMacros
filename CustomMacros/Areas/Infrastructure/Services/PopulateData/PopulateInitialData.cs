using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using CustomMacros.Areas.Infrastructure.Services;
using System.Transactions;
using System.Data.SqlClient;
using System.Configuration;

namespace CustomMacros.Areas.Infrastructure.Services.PopulateData
{

    public class PopulateInitialData : IInitializationService, IPopulateInitialData
    {
        private const string UMBRACO_CONNECTION_NAME = "umbracoDbDSN";
        private string UmbracoConnection;
        

        public void Initialize(System.Configuration.ConnectionStringSettingsCollection connections)
        {
            foreach (ConnectionStringSettings c in connections)
            {
                if (c.Name.Equals(UMBRACO_CONNECTION_NAME))
                {
                    UmbracoConnection = c.ConnectionString;
                    if (!IsInitialized)
                    {
                        PopulateData();
                    }
                }
            }
        }

        private bool IsInitialized { 
            get {
                return ExecuteScalar<bool>(UmbracoConnection, CustomMacros.IsInitialized);
            }
        }

        private void PopulateData()
        {
            ExecuteCommand(UmbracoConnection, CustomMacros.PopulateCustomMacroData);
        }

        private void ExecuteCommand(string connectionString, string command)
        {

            using (TransactionScope scope = new TransactionScope())
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    int myResult = 0;
                    SqlCommand SQL = new SqlCommand(command, connection);
                    connection.Open();
                    myResult = SQL.ExecuteNonQuery();
                    connection.Close();
                    scope.Complete();
                }
                catch (Exception)
                {
                    if (scope != null)
                    {
                        scope.Complete();
                    }
                    throw;
                }
            }
        }

        private T ExecuteScalar<T>(string connectionString, string command)
        {

            using (TransactionScope scope = new TransactionScope())
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    T myResult;
                    SqlCommand SQL = new SqlCommand(command, connection);
                    connection.Open();
                    myResult = (T)SQL.ExecuteScalar();
                    connection.Close();
                    scope.Complete();
                    return myResult;
                }
                catch (Exception)
                {
                    if (scope != null)
                    {
                        scope.Complete();
                    }
                    throw;
                }
            }
        }
    }
}