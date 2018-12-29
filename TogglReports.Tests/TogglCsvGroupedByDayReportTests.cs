
namespace TogglReports.Tests
{
    using System;
    using System.Collections.Generic;

    using AutoFixture;

    using FluentAssertions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using Toggl;
    using Toggl.Interfaces;
    using Toggl.QueryObjects;

    using TogglReports.Repositories;

    [TestClass]
    public class TogglCsvGroupedByDayReportTests
    {
        /// <summary>The fixture</summary>
        private Fixture fixture;

        [TestInitialize]
        public void TestInitialize()
        {
            this.fixture = new Fixture();
        }

        /// <summary>Gets the report data with valid dates and API key that returns valid time entries.</summary>
        [TestMethod]
        public void GetReportData_WithValidDatesAndApiKey_ThatReturnsValidTimeEntries()
        {
            var now = DateTime.Now;
            var startDate = now.AddDays(-14);
            var endDate = now.AddDays(14);
            var duration = endDate - startDate;
            var mockTimeEntries = this.fixture.Create<List<TimeEntry>>();
            foreach (var item in mockTimeEntries)
            {
                item.Start = startDate.ToString();
                item.Stop = endDate.ToString();
            }
            var timeEntryService = this.fixture.Freeze<Mock<ITimeEntryService>>();
            timeEntryService
                .Setup(c => c.List(It.IsAny<TimeEntryParams>()))
                .Returns(mockTimeEntries);
            var report = new TogglCsvGroupedByDayReport(timeEntryService.Object);
            var result = report
                .GetReportData(start: It.IsAny<DateTime>(), end: It.IsAny<DateTime>());
            result.Should().NotBeNull();
            result.Value.Should().NotBeNull();
            result.Value.Length.Should().BeGreaterThan(0);
            foreach (var item in result.Value)
            {
                item.Duration.Should().Be(duration);
            }
        }
    }
}
