using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Web.Http;
using AirHockeyMobileService.Models;
using Microsoft.WindowsAzure.Mobile.Service;

namespace AirHockeyMobileService
{
    public static class WebApiConfig
    {
        public static void Register()
        {
            // Use this class to set configuration options for your mobile service
            ConfigOptions options = new ConfigOptions();

            // Use this class to set WebAPI configuration options
            HttpConfiguration config = ServiceConfig.Initialize(new ConfigBuilder(options));

            // To display errors in the browser during development, uncomment the following
            // line. Comment it out again when you deploy your service for production use.
            // config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            Database.SetInitializer(new MobileServiceInitializer());
        }
    }

    public class MobileServiceInitializer : DropCreateDatabaseIfModelChanges<MobileServiceContext>
    {
        /*protected override void Seed(MobileServiceContext context)
        {
            List<Player> players = new List<Player>
            {
                new Player { Id = Guid.NewGuid().ToString(), FirstName = "Amit", LastName = "Ohayon" },
                new Player { Id = Guid.NewGuid().ToString(), FirstName = "Amir", LastName = "Sagiv" },
                new Player { Id = Guid.NewGuid().ToString(), FirstName = "Oren", LastName = "Afek" }
            };

            foreach (Player player in players)
            {
                context.Set<Player>().Add(player);
            }

            base.Seed(context);
        }*/
    }
}

