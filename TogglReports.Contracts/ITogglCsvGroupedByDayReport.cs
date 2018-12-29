namespace TogglReports.Contracts
{
    using System;

    using TogglReports.Models;

    public interface ITogglCsvGroupedByDayReport
    {
        /// <summary>Gets the report data.</summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        CsvGroupedByDayReportCollection GetReportData(DateTime start, DateTime end);
    }
}
