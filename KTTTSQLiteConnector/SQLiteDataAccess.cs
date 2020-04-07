using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using Dapper;
using KTTTDataInterface;

namespace KTTTSQLiteConnector
{
    /// <summary>
    /// SQLite functionality provider.
    /// </summary>
    public class SQLiteDataAccess : IDataAccess
    {
        private readonly string connectionString;

        public SQLiteDataAccess(in string connString)
        {
            connectionString = connString;
            using (IDbConnection cnn = new SQLiteConnection(connectionString))
            {
                try
                {
                    cnn.Execute("CREATE TABLE IF NOT EXISTS \"WorkDay\" (\"calWeek\" INTEGER, \"date\" TEXT UNIQUE, \"startTime\" TEXT, \"endTime\" TEXT, \"hoursActive\" REAL, PRIMARY KEY(\"date\"))");
                } catch (Exception)
                {
                    Console.WriteLine($"Error 14: Failed to create Database table.");
                }
            }
        }

        /// <summary>
        /// Dump table contents.
        /// </summary>
        /// <returns></returns>
        public List<WorkDayModel> GetEntries()
        {
            using (IDbConnection cnn = new SQLiteConnection(connectionString))
            {
                try
                {
                    var output = cnn.Query<WorkDayModel>("SELECT * FROM WorkDay", new DynamicParameters());
                    return output.ToList();
                } catch (Exception) 
                {
                    Console.WriteLine($"Error 11: Database missing or Table is corrupted!");
                }
            }
            return new List<WorkDayModel>();
        }

        /// <summary>
        /// Add new entry or update existing one.
        /// </summary>
        /// <param name="entry"> Data model object</param>
        public void StoreEntry(in WorkDayModel entry)
        {
            using (IDbConnection cnn = new SQLiteConnection(connectionString))
            {
                try
                {
                    cnn.Execute("insert or replace into WorkDay (date, startTime, endTime, calWeek)values (@date, COALESCE((select startTime from WorkDay where date = @date), @startTime), @endTime, @calWeek)", entry);
                } catch (Exception) 
                {
                    Console.WriteLine($"Error 12: Database missing or Table is corrupted!");
                }
            }
        }
    }
}
