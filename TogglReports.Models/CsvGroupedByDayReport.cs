namespace TogglReports.Models
{
    using System;

    /// <summary>Csv Grouped By Day Report</summary>
    public class CsvGroupedByDayReport
    {
        /// <summary>Gets or sets the description.</summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>Gets or sets the task identifier.</summary>
        /// <value>The task identifier.</value>
        public int? TaskId { get; set; }

        /// <summary>Gets or sets the start date.</summary>
        /// <value>The start date.</value>
        public DateTime StartDate { get; set; }

        /// <summary>Gets or sets the duration in hours.</summary>
        /// <value>The duration in hours.</value>
        public TimeSpan Duration { get; set; }
    }
}