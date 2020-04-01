using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using SqLiteConnector;
using System.Timers;

namespace presenceApp
{
    class Program
    {
        private const int TIMER_INTERVAL = 600000; // in ms
        private static Timer timer; 

        private static IConfigurationRoot configuration;

        private static SQLiteDataAccess dbAccess = null;

        private static int getCalenderWeek(in DateTime date, in CultureInfo culture)
        {
            int retVal = -1;
            try
            {
                Calendar myCal = culture.Calendar;
                retVal = myCal.GetWeekOfYear(date, culture.DateTimeFormat.CalendarWeekRule, culture.DateTimeFormat.FirstDayOfWeek);
            } catch (System.ArgumentOutOfRangeException)
            {
                Console.WriteLine($"Error 2: Failed to get calender week number.");
            }
            return retVal;
        }

        private static PresenceModel generateNewEntry()
        {
            DateTime startWork = DateTime.Now;
            var culture = new CultureInfo("en-US");

            return new PresenceModel{
                calWeek = getCalenderWeek(startWork, culture),
                date = startWork.ToString("ddd dd.MM.yyyy", culture),
                startTime = startWork.ToString("T", culture),
                endTime = startWork.ToString("T", culture),
                hoursActive = 0
            };
        }

        public static PresenceModel entryToday { get; set; }

        static void Main(string[] args)
        {
            // extract db connection string from config file
            string connectionString = "Data Source=presence.db;Version=3;";
            try {
                configuration = new ConfigurationBuilder()
			        .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
			        .AddJsonFile("appsettings.json", false)
			        .Build();
                connectionString = configuration.GetConnectionString("Default");
            } catch (System.IO.FileNotFoundException)
            {
                Console.WriteLine($"Error 1: Configuration file appsettings.json missing. Using fallback.");
            }

            // generate current entry
            entryToday = generateNewEntry();

            dbAccess = new SQLiteDataAccess(connectionString);
            if (dbAccess.StoreEntry(entryToday) == false)
            {
                if (dbAccess.CreateTable())
                {
                    dbAccess.StoreEntry(entryToday);
                } else {
                    Console.WriteLine("Missing database. Could not recover. Exiting.");
                    return;
                }

            }

            // start timer to progress the end time
            timer = new System.Timers.Timer();
            timer.Interval = TIMER_INTERVAL;
            timer.Elapsed += OnIntervalElapsed;
            timer.AutoReset = true;
            timer.Enabled = true;

            foreach (var entry in dbAccess.GetEntries())
            {
                Console.WriteLine("Date: {0} Week: {1} Start Time {2}, End Time {3}",
                                  entry.date,
                                  entry.calWeek.ToString("00"),
                                  entry.startTime,
                                  entry.endTime);
            }
            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        }

        private static void OnIntervalElapsed(Object source, System.Timers.ElapsedEventArgs e)
        {
            var now = generateNewEntry();
            // for those that work past midnight
            if (now.date != entryToday.date)
            {
                entryToday = now;
                dbAccess.StoreEntry(entryToday);
            } else {
                dbAccess.UpdateEntry(now);
            }
        }
    }
}
