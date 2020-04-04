using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using Dapper;
using KTTTDataInterface;

namespace KTTTSQLiteConnector
{
    public class SQLiteDataAccess : IDataAccess
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

        public List<WorkDayModel> GetEntries()
        {
            using (IDbConnection cnn = new SQLiteConnection(connectionString))
            {
                try
                {
                    var output = cnn.Query<WorkDayModel>("SELECT * FROM presence", new DynamicParameters());
                    return output.ToList();
                } catch (Exception) 
                {
                    Console.WriteLine($"Error 11: Database missing or Table is corrupted!");
                }
            }
            return new List<WorkDayModel>();
        }

        public void StoreEntry(in WorkDayModel entry)
        {
            using (IDbConnection cnn = new SQLiteConnection(connectionString))
            {
                try
                {
                    cnn.Execute("insert or replace into presence (date, startTime, endTime, calWeek)values (@date, COALESCE((select startTime from presence where date = @date), @startTime), @endTime, @calWeek)", entry);
                } catch (Exception) 
                {
                    Console.WriteLine($"Error 12: Database missing or Table is corrupted!");
                }
            }
        }

        public void UpdateEntry(in WorkDayModel entry)
        {
            using (IDbConnection cnn = new SQLiteConnection(connectionString))
            {
                try
                {
                    cnn.Execute("update presence set endTime = @endTime where date = @date", entry);
                } catch (Exception) 
                {
                    Console.WriteLine($"Error 13: Database missing or Table is corrupted!");
                }
            }
        }
    }
}
