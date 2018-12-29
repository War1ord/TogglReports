namespace TogglReports.Repositories
{
    using System;
    using System.IO;
    using System.Text;

    using TogglReports.Contracts;
    using TogglReports.Models;

    /// <summary>
    /// The Report Data Manager
    /// </summary>
    /// <seealso cref="IReportDataManager" />
    public class ReportDataManager : IReportDataManager
    {
        /// <summary>Saves the CSV report data to desktop.</summary>
        /// <param name="collection">The collection.</param>
        public void SaveCsvReportDataToDesktop(CsvGroupedByDayReportCollection collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (collection.Value == null)
            {
                throw new ArgumentNullException(nameof(collection.Value));
            }

            if (collection.Value.Length < 1)
            {
                throw new ArgumentException("There is not data to save.");
            }

            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("StartDate,TaskId,Description,Duration");
            foreach (var item in collection.Value)
            {
                csvBuilder
                    .Append(item.StartDate.Date.ToString("yyyy-MM-dd"))
                    .Append(", ")
                    .Append(item.TaskId?.ToString() ?? "")
                    .Append(", ")
                    .Append(item.Description?.Replace(",", "") ?? "")
                    .Append(", ")
                    .AppendLine(item.Duration.TotalHours.ToString("0.##"));
            }
            var filePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                $"CsvGroupedByDayReport_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv");
            File.WriteAllText(filePath, csvBuilder.ToString());
        }
    }
}
