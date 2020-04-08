using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using Moq;
using FluentAssertions;
using KTTTApp;
using KTTTDataInterface;
using System.Collections.Generic;

namespace KTTTAppTest
{
    [TestClass]
    public class WorkDayTest
    {

        private Mock<IDataAccess> dataAccMock = null;

        private WorkDayModel wdModel = null;

        [TestInitialize()]
        public void Initialize()
        {
            dataAccMock = new Mock<IDataAccess>();
            wdModel = new WorkDayModel(){
                calWeek = -1,
                date = "",
                startTime = "",
                endTime = "",
                hoursActive = 0
            };
            dataAccMock.Setup(x => x.StoreEntry(in wdModel));
            dataAccMock.Setup(x => x.GetEntries()).Returns(new List<WorkDayModel>());
        }

        [TestCleanup()]
        public void Cleanup()
        {
            dataAccMock = null;
            wdModel = null;
        }

        [TestMethod]
        public void Constructor_WithEmptyStrings()
        {
            var culture = new CultureInfo("");

            var SUT = new WorkDay(culture, dataAccMock.Object);

            SUT.Should().NotBeNull();
        }

        [TestMethod]
        public void Constructor_UpdateEntryToday()
        {
            var culture = new CultureInfo("en-US"); 
            string expectedDate = DateTime.Now.ToString("ddd dd.MM.yyyy", culture);
            wdModel.date = expectedDate;

            var SUT = new WorkDay(culture, dataAccMock.Object);

            SUT.entryToday.ToString("ddd dd.MM.yyyy", culture).Should().BeEquivalentTo(expectedDate);
        }

        [TestMethod]
        public void Constructor_UpdateEntryOnStartup()
        {
            var culture = new CultureInfo("dummy"); 

            var SUT = new WorkDay(culture, dataAccMock.Object);

            dataAccMock.Verify(v => v.StoreEntry(in It.Ref<WorkDayModel>.IsAny), Times.Once());
        }

        [TestMethod]
        public void Constructor_UpdateEntryDifferentCulture()
        {
            var culture = new CultureInfo("de-DE"); 
            string expectedDate = DateTime.Now.ToString("ddd dd.MM.yyyy", culture);
            wdModel.date = expectedDate;

            var SUT = new WorkDay(culture, dataAccMock.Object);

            SUT.entryToday.ToString("ddd dd.MM.yyyy", culture).Should().BeEquivalentTo(expectedDate);
        }

        [TestMethod]
        public void Constructor_cultureNull()
        {
            Action act = () => new WorkDay(null, dataAccMock.Object);

            act.Should()
                .Throw<ArgumentNullException>().And.ParamName
                .Should()
                .Be("Parameter cannot be null");
        }

        [TestMethod]
        public void Constructor_dbAccessNull()
        {
            var culture = new CultureInfo("de-DE"); 

            Action act = () => new WorkDay(culture, null);

            act.Should()
                .Throw<ArgumentNullException>().And.ParamName
                .Should()
                .Be("Parameter cannot be null");
        }

        [TestMethod]
        public void Constructor_generateEntry()
        {
            var culture = new CultureInfo("de-DE"); 
            var obj = new WorkDay(culture, dataAccMock.Object);

            var SUT = obj.generateNewEntry();

            SUT.hoursActive.Should().Be(0);
            SUT.date.Should().BeEquivalentTo(DateTime.Now.ToString("ddd dd.MM.yyyy", culture));
        }

    }
}
