using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using Dapper;

namespace SqLiteConnector
{
    public class SQLiteDataAccess
    {
        private readonly string connectionString;

        public SQLiteDataAccess(string connString)
        {
            connectionString = connString;
            using (IDbConnection cnn = new SQLiteConnection(connectionString))
            {
                try
                {
                    cnn.Execute("CREATE TABLE IF NOT EXISTS \"presence\" (\"key\" INTEGER NOT NULL UNIQUE, \"calWeek\" INTEGER, \"date\" TEXT UNIQUE, \"startTime\" TEXT, \"endTime\" TEXT, \"hoursActive\" REAL, PRIMARY KEY(\"key\"))");
                } catch (Exception)
                {
                    Console.WriteLine($"Error 14: Failed to create Database table.");
                }
            }
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

        public void StoreEntry(in PresenceModel pm)
        {
            using (IDbConnection cnn = new SQLiteConnection(connectionString))
            {
                try
                {
                    cnn.Execute("insert or replace into presence (date, startTime, endTime, calWeek)values (@date, COALESCE((select startTime from presence where date = @date), @startTime), @endTime, @calWeek)", pm);
                } catch (Exception) 
                {
                    Console.WriteLine($"Error 12: Database missing or Table is corrupted!");
                }
            }
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
