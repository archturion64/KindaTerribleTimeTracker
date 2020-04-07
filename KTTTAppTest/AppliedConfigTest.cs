using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using KTTTApp;
using KTTTDataInterface;

namespace KTTTAppTest
{
    [TestClass]
    public class AppliedConfigTest
    {
        private Mock<IConfigurationBuilder> confBuildMock = null;

        private Mock<IConfigurationRoot> iConfRootMock = null;

        [TestInitialize()]
        public void Initialize()
        {
            confBuildMock = new Mock<IConfigurationBuilder>();
            iConfRootMock = new Mock<IConfigurationRoot>();
            var iConfMock = new Mock<IConfigurationSection>();
            iConfRootMock.Setup(i => i.GetSection(It.IsAny<string>())).Returns(iConfMock.Object);
            confBuildMock.Setup(c => c.Build()).Returns(iConfRootMock.Object);
        }

        [TestCleanup()]
        public void Cleanup()
        {
            confBuildMock = null;
        }

        [TestMethod]
        public void Constructor_ininitializedConfig()
        {
            var SUT = new AppliedConfig(confBuildMock.Object);

            SUT.Should().NotBeNull();
        }

        [TestMethod]
        public void Constructor_cultureNotNull()
        {
            var SUT = new AppliedConfig(confBuildMock.Object);

            SUT.Culture.Should().NotBeNull();
        }

        [TestMethod]
        public void Constructor_onNullConfig()
        {
            var SUT = new AppliedConfig(null);

            SUT.Should().NotBeNull();
        }

        [TestMethod]
        public void Constructor_defaultCulture()
        {
            var SUT = new AppliedConfig(null);

            SUT.Culture.ToString().Should().BeEquivalentTo(new CultureInfo("en-US").ToString());
        }

        [TestMethod]
        public void Constructor_defaultConnType()
        {
            var SUT = new AppliedConfig(null);

            SUT.ConnectorType.Should().Be(EDataConnector.SQLite);
        }
    }
}
