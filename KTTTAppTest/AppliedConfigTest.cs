using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using KTTTApp;

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
            var testedObj = new AppliedConfig(confBuildMock.Object);

            testedObj.Should().NotBeNull();
        }

        [TestMethod]
        public void Constructor_cultureNotNull()
        {
            var testedObj = new AppliedConfig(confBuildMock.Object);

            testedObj.Culture.Should().NotBeNull();
        }

        [TestMethod]
        public void Constructor_onNullConfig()
        {
            var testedObj = new AppliedConfig(null);

            testedObj.Should().NotBeNull();
        }

        [TestMethod]
        public void Constructor_defaultCulture()
        {
            var testedObj = new AppliedConfig(null);

            testedObj.Culture.ToString().Should().BeEquivalentTo(new CultureInfo("en-US").ToString());
        }

    }
}
