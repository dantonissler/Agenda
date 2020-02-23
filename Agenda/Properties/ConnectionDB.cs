using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace Repository
{
    public class ConnectionDB
    {
        private static readonly string Server = "localhost";
        private static readonly string Database = "Agenda";
        private static readonly string User = "root";
        private static readonly string Password = "Dan214255";
        private readonly string ConnectionString = $"Server={Server};Database={Database};Uid={User};Pwd={Password};Sslmode=none;";

        public void Execute(string script, Dictionary<string, object> parameters)
        {
            using MySqlConnection connection = new MySqlConnection(ConnectionString);
            connection.Open();
            try
            {
                using MySqlCommand command = new MySqlCommand(script, connection);
                command.Prepare();

                foreach (var item in parameters)
                    command.Parameters.AddWithValue(item.Key, item.Value);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }

        public DataSet SelectDataFromMySql(string script, Dictionary<string, object> parameters = null)
        {
            using MySqlConnection connection = new MySqlConnection(ConnectionString);
            connection.Open();
            DataTable dataTable = new DataTable();
            DataSet dataSet = new DataSet();

            try
            {
                using MySqlCommand command = new MySqlCommand(script, connection);
                command.Prepare();

                if (parameters != null)
                {
                    foreach (var item in parameters)
                        command.Parameters.AddWithValue(item.Key, item.Value);
                }

                MySqlDataReader mySqlDataReader = command.ExecuteReader();
                dataTable.Load(mySqlDataReader);
                dataSet.Tables.Add(dataTable);
                return dataSet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
                connection.Dispose();
                dataTable.Dispose();
                dataSet.Dispose();
            }
        }
    }
}