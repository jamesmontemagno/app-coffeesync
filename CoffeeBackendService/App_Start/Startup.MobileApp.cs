using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Web.Http;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Authentication;
using Microsoft.Azure.Mobile.Server.Config;
using CoffeeBackendService.DataObjects;
using CoffeeBackendService.Models;
using Owin;

namespace CoffeeBackendService
{
    public partial class Startup
    {
        public static void ConfigureMobileApp(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            //For more information on Web API tracing, see http://go.microsoft.com/fwlink/?LinkId=620686 
            config.EnableSystemDiagnosticsTracing();

            new MobileAppConfiguration()
                .UseDefaultConfiguration()
                .ApplyTo(config);

            // Use Entity Framework Code First to create database tables based on your DbContext
            Database.SetInitializer(new europecoffeeInitializer());

            // To prevent Entity Framework from modifying your database schema, use a null database initializer
            // Database.SetInitializer<europecoffeeContext>(null);

            MobileAppSettingsDictionary settings = config.GetMobileAppSettingsProvider().GetMobileAppSettings();

            if (string.IsNullOrEmpty(settings.HostName))
            {
                // This middleware is intended to be used locally for debugging. By default, HostName will
                // only have a value when running in an App Service application.
                app.UseAppServiceAuthentication(new AppServiceAuthenticationOptions
                {
                    SigningKey = ConfigurationManager.AppSettings["SigningKey"],
                    ValidAudiences = new[] { ConfigurationManager.AppSettings["ValidAudience"] },
                    ValidIssuers = new[] { ConfigurationManager.AppSettings["ValidIssuer"] },
                    TokenHandler = config.GetAppServiceTokenHandler()
                });
            }
            app.UseWebApi(config);
        }
    }

    public class europecoffeeInitializer : CreateDatabaseIfNotExists<CoffeeBackendContext>
    {
        protected override void Seed(CoffeeBackendContext context)
        {
            List<Coffee> Coffees = new List<Coffee>
            {
                new Coffee { Id = Guid.NewGuid().ToString(), Name = "Cafe Victrola", Latitude = 47.622359, Longitude = -122.312894, Stars = 5 },
                new Coffee { Id = Guid.NewGuid().ToString(), Name = "First item", Latitude = 47.611705, Longitude = -122341253, Stars = 5},
                new Coffee { Id = Guid.NewGuid().ToString(), Name = "Faculty Coffee", Latitude = 52.478902, Longitude = -1.900021, Stars = 5},
                new Coffee { Id = Guid.NewGuid().ToString(), Name = "Copenhagen Coffee Lab", Latitude = 55.677463, Longitude = 12.581852, Stars = 5}
            };

            foreach (Coffee Coffee in Coffees)
            {
                context.Set<Coffee>().Add(Coffee);
            }

            base.Seed(context);
        }
    }
}

