using System;
using System.Globalization;
using Microsoft.Extensions.Configuration;

namespace KTTTApp
{
    public class AppliedConfig
    {
        private const string DEFAULT_CULTURE_STRING = "en-US";

        private const string DEFAULT_CONNECTION_STRING = "Default";

        public string ConnnectionString { get; private set; } = "Data Source=presence.db;Version=3;";

        public CultureInfo Culture = null;

        public AppliedConfig(IConfigurationBuilder config)
        {
            if (config == null)
            {
                Culture = new CultureInfo(DEFAULT_CULTURE_STRING);
                Console.WriteLine($"Error 1: Null reference to configuration object passed. Using fallback.");
            } else {
                // parse config file
                try {
                    var configuration = config.Build();

                    updateCultureInfo(configuration);
                    updateConnectionString(configuration);  
                    
                    Console.WriteLine($" Using culture {Culture.ToString()} and DB connection {ConnnectionString}.");

                } catch (System.IO.FileNotFoundException)
                {
                    Culture = new CultureInfo(DEFAULT_CULTURE_STRING);
                    Console.WriteLine($"Error 1: Configuration file appsettings.json missing. Using fallback.");
                }
            }
        }

        private void updateCultureInfo(in IConfigurationRoot configuration)
        {
            // get language settings
            string cultureString = configuration.GetSection("AppOptions")["Culture"];
            if(!string.IsNullOrEmpty(cultureString))
            {
                try
                {
                    var cultureVerification = new CultureInfo(cultureString);
                    // if constructor above did not trow, string is a valid culture
                    Culture = cultureVerification;
                } catch (Exception) //System.ArgumentOutOfRangeException or System.Globalization.CultureNotFoundException
                {
                        // use default culture string
                        Culture = new CultureInfo(DEFAULT_CULTURE_STRING);
                }
            } else {
                    // use default culture string
                    Culture = new CultureInfo(DEFAULT_CULTURE_STRING);
            }
        }

        private void updateConnectionString(in IConfigurationRoot configuration)
        {
            // get connection string
            string connection = configuration.GetConnectionString("Default");
            if(!string.IsNullOrEmpty(connection))
            {
                ConnnectionString = connection;
            }
        }
    }
}
