namespace TogglReports.Repositories
{
    using System;
    using System.Linq;

    using Toggl.Interfaces;

    using TogglReports.Contracts;
    using TogglReports.Models;

    /// <summary>
    /// Toggl Csv Grouped By Day Report
    /// </summary>
    /// <seealso cref="TogglReports.Contracts.ITogglCsvGroupedByDayReport" />
    public class TogglCsvGroupedByDayReport : ITogglCsvGroupedByDayReport
    {
        /// <summary>The time entry service</summary>
        private readonly ITimeEntryService timeEntryService;

        /// <summary>Initializes a new instance of the <see cref="TogglCsvGroupedByDayReport"/> class.</summary>
        /// <param name="timeEntryService">The time entry service.</param>
        public TogglCsvGroupedByDayReport(ITimeEntryService timeEntryService)
        {
            this.timeEntryService = timeEntryService;
        }

        /// <summary>Gets the report data.</summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns></returns>
        public CsvGroupedByDayReportCollection GetReportData(DateTime start, DateTime end)
        {
            var entries = this.timeEntryService.List(new Toggl.QueryObjects.TimeEntryParams { StartDate = start, EndDate = end });

            var results = entries
                .AsParallel()
                .Select(i =>
                {
                    var IsValidStartDate = DateTime.TryParse(i.Start, out var startDate);
                    var IsValidEndDate = DateTime.TryParse(i.Stop, out var endDate);
                    return new
                    {
                        i.Description,
                        IsValidStartDate,
                        StartDate = IsValidStartDate ? startDate : (DateTime?)null,
                        IsValidEndDate,
                        EndDate = IsValidEndDate ? endDate : (DateTime?)null,
                        Duration = IsValidStartDate && IsValidEndDate ? endDate - startDate : (TimeSpan?)null,
                    };
                })
                .Where(i => i.IsValidStartDate && i.IsValidEndDate)
                .GroupBy(i => new
                {
                    StartDate = i.StartDate.Value.Date,
                    i.Description,
                })
                .Select(i => new CsvGroupedByDayReport
                {
                    Description = i.Key.Description,
                    StartDate = i.Key.StartDate,
                    Duration = i.Aggregate(TimeSpan.Zero, (a1, a2) => a1 + a2.Duration.Value),
                })
                .ToArray();

            return new CsvGroupedByDayReportCollection { Value = results };
        }
    }
}
