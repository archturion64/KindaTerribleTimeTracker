﻿using System;
using KTTTDataInterface;
using KTTTDataConnector;

namespace KTTTApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // read config
            AppliedConfig appConfig = new AppliedConfig();

            // establish data access
            IDataAccess connector = DataConnector.getDBConnector(appConfig.ConnnectionString);

            WorkDay workDay = new WorkDay(appConfig.Culture, connector);

            // closest thing to UI at the moment
            foreach (var entry in connector.GetEntries())
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
