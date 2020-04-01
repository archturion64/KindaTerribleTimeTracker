using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.Common;

namespace SqLiteConnector
{
    public class SQLiteDataAccess
    {
        private readonly string connectionString;

        public SQLiteDataAccess(string connString) => connectionString = connString;

        public bool CreateTable()
        {
            bool retVal = false;
            using (IDbConnection cnn = new SQLiteConnection(connectionString))
            {
                try
                {
                    Console.WriteLine("Removing old table.");
                    cnn.Execute("DROP TABLE IF EXISTS \"presence\"");
                    Console.WriteLine("Creating new table.");
                    cnn.Execute("CREATE TABLE \"presence\" (\"key\" INTEGER NOT NULL UNIQUE, \"calWeek\" INTEGER, \"date\" TEXT UNIQUE, \"startTime\" TEXT, \"endTime\" TEXT, \"hoursActive\" REAL, PRIMARY KEY(\"key\"))");
                    retVal = true;
                } catch (Exception) 
                {
                    Console.WriteLine($"Error 14: Failed to create Database table.");
                }
            }
            return retVal;
        }

        public List<PresenceModel> GetEntries()
        {
            using (IDbConnection cnn = new SQLiteConnection(connectionString))
            {
                try
                {
                    var output = cnn.Query<PresenceModel>("SELECT * FROM presence", new DynamicParameters());
                    return output.ToList();
                } catch (Exception) 
                {
                    Console.WriteLine($"Error 11: Database missing or Table is corrupted!");
                }
            }
            return new List<PresenceModel>();
        }

        public bool StoreEntry(in PresenceModel pm)
        {
            bool retVal = false;
            using (IDbConnection cnn = new SQLiteConnection(connectionString))
            {
                try
                {
                    cnn.Execute("insert or replace into presence (date, startTime, endTime, calWeek)values (@date, COALESCE((select startTime from presence where date = @date), @startTime), @endTime, @calWeek)", pm);
                    retVal = true;
                } catch (Exception) 
                {
                    Console.WriteLine($"Error 12: Database missing or Table is corrupted!");
                }
            }
            return retVal;
        }

        public void UpdateEntry(in PresenceModel pm)
        {
            using (IDbConnection cnn = new SQLiteConnection(connectionString))
            {
                try
                {
                    cnn.Execute("update presence set endTime = @endTime where date = @date", pm);
                } catch (Exception) 
                {
                    Console.WriteLine($"Error 13: Database missing or Table is corrupted!");
                }
            }
        }
    }
}
