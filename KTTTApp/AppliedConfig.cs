using System;
using System.Globalization;
using Microsoft.Extensions.Configuration;
using KTTTDataInterface;

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
        /// type of the connector that shal be used for data storage.
        /// </summary>
        /// <value>one of pre-defined connector values</value>
        public EDataConnector ConnectorType { get; private set; } = EDataConnector.SQLite;

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
                Console.WriteLine($"Error 6: Null reference to configuration object passed. Using fallback.");
            } else {
                // parse config file
                try {
                    var configuration = config.Build();

                    updateCultureInfo(configuration);
                    updateConnectionString(configuration);
                    updateConnectorType(configuration);
                    
                    Console.WriteLine($" Using culture {Culture.ToString()}, connector type {ConnectorType.ToString()} and DB connection {ConnnectionString}.");

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

        /// <summary>
        /// parse connector preference from config and apply to this object's property.
        /// </summary>
        /// <param name="configuration"></param>
        private void updateConnectorType(in IConfigurationRoot configuration)
        {
            string connectorString = configuration.GetSection("AppOptions")["KTTTConnectorType"];
            
            if(connectorString == EDataConnector.SQLite.ToString())
            {
                ConnectorType = EDataConnector.SQLite;
            } else if (connectorString == EDataConnector.Entity.ToString())
            {
                ConnectorType = EDataConnector.Entity;
            } else {
                Console.WriteLine($"Error 7: Unrecognisable KTTTConnectorType in config. Using fallback.");
            }
        }
    }
}
