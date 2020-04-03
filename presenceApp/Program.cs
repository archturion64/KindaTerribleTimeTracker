using System;
using SqLiteConnector;


namespace presenceApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // read config
            AppliedConfig appConfig = new AppliedConfig();

            // establish DB access
            SQLiteDataAccess dbAccess = new SQLiteDataAccess(connString: appConfig.ConnnectionString);

            WorkDay workDay = new WorkDay(appConfig.Culture, dbAccess);

            // closest thing to UI at the moment
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
    }
}
