using Microsoft.VisualStudio.TestTools.UnitTesting;
using KTTTDataInterface;
using KTTTDataConnector;
using FluentAssertions;
using KTTTSQLiteConnector;
using KTTTEntityFwConnector;

namespace KTTTDataConnectorTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void getDBConnector_typeSqliteNotNullRef()
        {
            var SUT = DataConnector.getDBConnector(null , EDataConnector.SQLite);

            SUT.Should().NotBeNull();
        }

        [TestMethod]
        public void getDBConnector_typeSqliteReturn()
        {
            var SUT = DataConnector.getDBConnector(null , EDataConnector.SQLite);

            SUT.Should().NotBeOfType<EntityFwDataAccess>();
            SUT.Should().BeOfType<SQLiteDataAccess>();
        }

        [TestMethod]
        public void getDBConnector_typeEntityNotNullRef()
        {
            var SUT = DataConnector.getDBConnector(null , EDataConnector.Entity);

            SUT.Should().NotBeNull();
        }

        [TestMethod]
        public void getDBConnector_typeEntityReturn()
        {
            var SUT = DataConnector.getDBConnector(null , EDataConnector.Entity);

            SUT.Should().NotBeOfType<SQLiteDataAccess>();
            SUT.Should().BeOfType<EntityFwDataAccess>();
        }
    }
}
