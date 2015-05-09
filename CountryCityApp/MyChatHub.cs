using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace CountryCityApp
{

    [HubName("hitCounter")]
    public class MyChatHub : Hub
    {
        private static string connectionString = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        public SqlConnection ASqlConnection = new SqlConnection(connectionString);
        public SqlCommand ASqlCommand { set; get; }
        public SqlDataReader ASqlDataReader { set; get; }

        public void Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.broadcastMessage(name, message);
        }

        public void RecordHit()
        {
            //_hitCount += 1;

            //this.Clients.All.onHitRecorded(_hitCount);

            Update(1);
            this.Clients.All.onHitRecorded(GetValue());
        }

        public void Update(int i)
        {
            string query = "UPDATE t_value SET value += '1' WHERE id = '" + i + "'";
            ASqlConnection.Open();
            ASqlCommand = new SqlCommand(query, ASqlConnection);

            ASqlCommand.ExecuteNonQuery();
            ASqlCommand.Dispose();
            ASqlConnection.Close();
        }

        public int GetValue()
        {
            int a = 0;
            string query = "SELECT * FROM t_value WHERE id=1";
            ASqlCommand = new SqlCommand(query, ASqlConnection);
            ASqlConnection.Open();
            ASqlDataReader = ASqlCommand.ExecuteReader();

            while (ASqlDataReader.Read())
            {
                a = (int) ASqlDataReader["value"];
            }

            ASqlDataReader.Close();
            ASqlCommand.Dispose();
            ASqlConnection.Close();
            return a;
        }
    }
}