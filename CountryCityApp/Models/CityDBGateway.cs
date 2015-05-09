using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CountryCityApp.Models
{
    public class CityDBGateway
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["CountryConnectionStr"].ConnectionString;
        SqlConnection aSqlConnection = new SqlConnection(connectionString);
        private SqlCommand aSqlCommand;
        private string query;
        public DBGatway aDbGateway = new DBGatway();

        public void Save(City aCity)
        {
            query = "INSERT INTO t_City VALUES('" + aCity.Name + "','" + aCity.About + "','" + aCity.NoOfDwellers + "','" + aCity.Location + "','" + aCity.Weather + "','" + aCity.CountryId + "')";

            aSqlConnection.Open();
            aSqlCommand = new SqlCommand(query, aSqlConnection);
            aSqlCommand.ExecuteNonQuery();
            aSqlConnection.Close();
        }

        public List<City> GetAllCity()
        {
            query = "SELECT * FROM t_City;";
            aSqlConnection.Open();
            aSqlCommand = new SqlCommand(query, aSqlConnection);
            SqlDataReader aSqlDataReader = aSqlCommand.ExecuteReader();
            List<City> cities = new List<City>();
            while (aSqlDataReader.Read())
            {
                City aCity = new City();
                aCity.Id = Convert.ToInt32(aSqlDataReader["id"]);
                aCity.Name = aSqlDataReader["name"].ToString();
                aCity.About = aSqlDataReader["about"].ToString();
                aCity.NoOfDwellers = Convert.ToInt32(aSqlDataReader["no_of_dwellers"]);
                aCity.Location = aSqlDataReader["location"].ToString();
                aCity.Weather = aSqlDataReader["weather"].ToString();
                aCity.CountryId = Convert.ToInt32(aSqlDataReader["countryId"]);
                aCity.aCountry=new Country();
                aCity.aCountry.Name = aDbGateway.GetCountryByName(aCity.CountryId).Name; 
                cities.Add(aCity);
            }
            aSqlConnection.Close();
            return cities;
        }

        public int GetNoOfCities(int id)
        {
            query = "SELECT Count(countryId) As NoOfCities FROM t_City WHERE countryId='" + id + "'";
            aSqlConnection.Open();
            aSqlCommand = new SqlCommand(query, aSqlConnection);
            SqlDataReader aSqlDataReader = aSqlCommand.ExecuteReader();
            int NoOfCities=0;
            if (aSqlDataReader.HasRows)
            {
                aSqlDataReader.Read();
                NoOfCities = Convert.ToInt32(aSqlDataReader["NoOfCities"]);
            }
            aSqlDataReader.Close();
            aSqlConnection.Close();
            return NoOfCities;
        }
        public int GetNoOfDwellers(int id)
        {
            query = "SELECT Sum(no_of_dwellers) As NoOfDwellers FROM t_City WHERE countryId='" + id + "'";
            aSqlConnection.Open();
            aSqlCommand = new SqlCommand(query, aSqlConnection);
            SqlDataReader aSqlDataReader = aSqlCommand.ExecuteReader();
            int NoOfDwellers = 0;
            if (aSqlDataReader.HasRows)
            {
                aSqlDataReader.Read();
                if (aSqlDataReader["NoOfDwellers"] != DBNull.Value)
                {
                    NoOfDwellers = Convert.ToInt32(aSqlDataReader["NoOfDwellers"]);
                }
           
            }
            aSqlDataReader.Close();
            aSqlConnection.Close();
            return NoOfDwellers;
        }

    }
}
    
