using KTTTDataInterface;
using KTTTSQLiteConnector;

namespace KTTTDataConnector
{
    public class DataConnector
    {
        public static IDataAccess getDBConnector(in string dbConnectionString)
        {
            return new SQLiteDataAccess(connString: dbConnectionString);
        }
    }
}
