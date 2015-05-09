using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CountryCityApp.Models;

namespace CountryCityApp.Controllers
{
    public class CountryController : Controller
    {
        CityDBGateway aCityDBGateway=new CityDBGateway();
        DBGatway aDbGateway = new DBGatway();
        [HttpGet]
        public ActionResult CountrySave()
        {
            ViewData["Country"]= aDbGateway.GetAll();
            return View();
        }

        [HttpPost]
        public ActionResult CountrySave(Country aCountry, HttpPostedFileBase file)
        {
            var fileName = Path.GetFileName(file.FileName);
            var imagePath = Path.Combine(Server.MapPath("/Images"), fileName);
            file.SaveAs(imagePath);
            aCountry.Images = "/Images/" + fileName;
            aDbGateway.Save(aCountry);
            ViewData["Country"] = aDbGateway.GetAll();
            return View();

        }

        public void SendNotifications()
        {
            string message = string.Empty;
            string name = string.Empty;
            string conStr = ConfigurationManager.ConnectionStrings["CountryConnectionStr"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(conStr))
            {
                string query = "SELECT [name],[about] FROM [dbo].[tbl_country] ORDER By [ID]";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Notification = null;
                    SqlDependency.Start(conStr);
                    SqlDependency dependency = new SqlDependency(command);
                    dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            name = reader[0].ToString();
                            message = reader[1].ToString();
                        }
                    }
                 }
            }

            NotificationsHub nHub = new NotificationsHub();
            nHub.NotifyAllClients(name, message);
        }

        private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                SendNotifications();
            }
        }

        public ActionResult Show()
        {
            ViewBag.GetCountryByName = aDbGateway.GetAll().ToList();
            foreach (Country country in ViewBag.GetCountryByName)
            {
                country.NoOfCities = aCityDBGateway.GetNoOfCities(country.Id);
                country.NoOfDwellers = aCityDBGateway.GetNoOfDwellers(country.Id);

            }
            return View();
        }
        [HttpPost]
        public ActionResult Show(string name)
        {

            SendNotifications();
           // var country = aDbGateway.GetAll();

            ViewBag. GetCountryByName = aDbGateway.GetAll().Where(x => x.Name.ToLower().Contains(name.ToLower())).ToList();
            foreach (Country country in ViewBag.GetCountryByName)
            {
                country.NoOfCities = aCityDBGateway.GetNoOfCities(country.Id);
                country.NoOfDwellers = aCityDBGateway.GetNoOfDwellers(country.Id);

            }
           
            return View();
        }
        public ActionResult View(int? id)
        {
            ViewBag.countries = aDbGateway.GetAll().OrderBy(x => x.Name).ToList();
            ViewBag.Country = aDbGateway.GetAll().Find(x => x.Id == id);
            return View("CountrySave");
        }

	}
}