using KTTTDataInterface;
using KTTTSQLiteConnector;

namespace KTTTDataConnector
{
    /// <summary>
    /// DB connection dispatcher.
    /// Choose which database to use and provide it to the caller.
    /// </summary>
    public class DataConnector
    {
        /// <summary>
        /// Provides an object that wraps manualy implemented SQlite functionality.
        /// </summary>
        /// <param name="dbConnectionString"> database specific connection string</param>
        /// <returns>instance to use for database functionality</returns>
        public static IDataAccess getDBConnector(in string dbConnectionString)
        {
            return new SQLiteDataAccess(connString: dbConnectionString);
        }
    }
}
