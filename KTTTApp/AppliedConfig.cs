using System;
using System.Globalization;
using Microsoft.Extensions.Configuration;

namespace KTTTApp
{
    public class AppliedConfig
    {
        /// <summary>
        /// Default language / culture to be used if non / invalid one is specified.
        /// </summary>
        private const string DEFAULT_CULTURE_STRING = "en-US";

        /// <summary>
        /// Key in the connection string section of the application config.
        /// </summary>
        private const string CONNECTION_STRING_KEY = "Default";

        /// <summary>
        /// connection string as expected by the DB connector
        /// </summary>
        /// <value> DB specific string</value>
        public string ConnnectionString { get; private set; } = "Data Source=presence.db;";

        /// <summary>
        /// used to determine date/time format 
        /// </summary>
        public CultureInfo Culture = null;

        /// <summary>
        /// Parse and expose running config for other onjects to use.
        /// </summary>
        /// <param name="config">config with predefined file name and path</param>
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

        /// <summary>
        /// Read out Culture key from the config and set the class member Culture accordingly.
        /// </summary>
        /// <param name="configuration">loaded config</param>
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

        /// <summary>
        /// Read out the CONNECTION_STRING_KEY key out of ConnectionStrings from the config and set the class member ConnnectionString accordingly.
        /// </summary>
        /// <param name="configuration"></param>
        private void updateConnectionString(in IConfigurationRoot configuration)
        {
            // get connection string
            string connection = configuration.GetConnectionString(CONNECTION_STRING_KEY);
            if(!string.IsNullOrEmpty(connection))
            {
                ConnnectionString = connection;
            }
        }
    }
}
