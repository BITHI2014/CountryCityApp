using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CountryCityApp.Models;

namespace CountryCityApp.Controllers
{
    public class CityController : Controller
    {
        //
        // GET: /City/
     CityDBGateway aCityDBGateway=new CityDBGateway();
     DBGatway aDbGateway = new DBGatway();
        public ActionResult Save()
        {
            ViewBag.cities = aCityDBGateway.GetAllCity().OrderBy(x => x.Name).ToList();
            ViewBag.countryId = new SelectList(aDbGateway.GetAll(), "Id", "Name");


            return View();
        }
        [HttpPost]
        public ActionResult Save(City aCity)
        {
            aCityDBGateway.Save(aCity);
            ViewBag.cities = aCityDBGateway.GetAllCity().OrderBy(x => x.Name).ToList();
            ViewBag.countryId = new SelectList(aDbGateway.GetAll(), "Id", "Name");

            return View();
        }

        public JsonResult CheckName(string name)
        {
            var city = aCityDBGateway.GetAllCity().FirstOrDefault(x => x.Name == name);
            if (city == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        public void SendNotifications()
        {
            string message = string.Empty;
            string name = string.Empty;
            string conStr = ConfigurationManager.ConnectionStrings["CountryConnectionStr"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(conStr))
            {
                string query = "SELECT [name],[about] FROM [dbo].[t_City] ORDER By [ID]";

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
            ViewBag.countryId = new SelectList(aDbGateway.GetAll(), "Id", "Name");
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
            ViewBag.countryId = new SelectList(aDbGateway.GetAll(), "Id", "Name");
            ViewBag.GetCountryByName = aDbGateway.GetAll().Where(x => x.Name.ToLower().Contains(name.ToLower())).ToList();
            foreach (Country country in ViewBag.GetCountryByName)
            {
                country.NoOfCities = aCityDBGateway.GetNoOfCities(country.Id);
                country.NoOfDwellers = aCityDBGateway.GetNoOfDwellers(country.Id);

            }

            return View();
        }

	}
}