using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Capstone.DAL;
using CampgroundCLI;

namespace CampgroundCLI
{
    public class Program
    {
        static void Main(string [] args)
        {
          
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            // Replace world with the key name used for your connection string in the appsettings.json file
            string connectionString = configuration.GetConnectionString("Project");

            //string connectionString = ("Data Source=localhost\\sqlexpress;Initial Catalog=npcamground;Integrated Security=True");
            CampgroundDAO campgroundDAO = new CampgroundDAO(connectionString);
            CampgroundCLI campgroundCLI = new CampgroundCLI(campgroundDAO);
            campgroundCLI.RunCLI();
        }
    }
}
