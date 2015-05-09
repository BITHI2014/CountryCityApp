using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CountryCityApp.Models
{
    public class DBGatway
    {
        public void Save(Country aCountry)
        {
            string connctionStr = ConfigurationManager.ConnectionStrings["CountryConnectionStr"].ConnectionString;
            SqlConnection aSqlConnection = new SqlConnection();
            aSqlConnection.ConnectionString = connctionStr;
              SqlCommand aSqlCommand;

            aSqlCommand = new SqlCommand("INSERT INTO tbl_country VALUES('" + aCountry.Name + "',@About,'" + aCountry.Images + "')",
               aSqlConnection);
            aSqlCommand.CommandType = CommandType.Text;
            aSqlCommand.Parameters.AddWithValue("@About", aCountry.About);
            aSqlConnection.Open();
            aSqlCommand.ExecuteNonQuery();
            aSqlConnection.Close();
           

        }

        public List<Country> GetAll()
        {
            string connctionStr = ConfigurationManager.ConnectionStrings["CountryConnectionStr"].ConnectionString;
            SqlConnection aSqlConnection = new SqlConnection();
            aSqlConnection.ConnectionString = connctionStr;
            string query = "SELECT * FROM tbl_country";
            aSqlConnection.Open();
            SqlCommand aSqlCommand = new SqlCommand(query, aSqlConnection);
            SqlDataReader aReader = aSqlCommand.ExecuteReader();
            List<Country> countries = new List<Country>();
            while (aReader.Read())
            {
                Country aCountry = new Country();
                aCountry.Id = Convert.ToInt32(aReader["id"]);
                aCountry.Name = aReader["name"].ToString();
                aCountry.Images = aReader["images"].ToString();
                aCountry.About = aReader["about"].ToString();
                countries.Add(aCountry);
            }
            aSqlConnection.Close();
            return countries;

        }
        public Country GetCountryByName(int id)
        {
            string connctionStr = ConfigurationManager.ConnectionStrings["CountryConnectionStr"].ConnectionString;
            SqlConnection aSqlConnection = new SqlConnection();
            aSqlConnection.ConnectionString = connctionStr;
            string query = "SELECT * FROM tbl_country WHERE id='" + id + "'";
            aSqlConnection.Open();
            SqlCommand  aSqlCommand = new SqlCommand(query, aSqlConnection);
            SqlDataReader aSqlDataReader = aSqlCommand.ExecuteReader();
            Country aCountry = new Country();
            while (aSqlDataReader.Read())
            {
                aCountry.Id = Convert.ToInt16(aSqlDataReader["id"]);
                aCountry.Name = aSqlDataReader["name"].ToString();
            }
            aSqlDataReader.Close();
            aSqlConnection.Close();
            return aCountry;


        }

    }
}