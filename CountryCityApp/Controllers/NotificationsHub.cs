using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace CountryCityApp.Controllers
{
    public class NotificationsHub:Hub
    {
        public void NotifyAllClients(string msg, string name)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotificationsHub>();
            context.Clients.All.displayNotification(msg, name);
        }
    }
}