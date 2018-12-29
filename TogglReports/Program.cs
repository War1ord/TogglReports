namespace TogglReports
{
    using System;

    using Toggl.Services;

    using TogglReports.Repositories;

    /// <summary>
    /// The Main Program
    /// </summary>
    internal class Program : IDisposable
    {
        /// <summary>Defines the entry point of the application.</summary>
        /// <param name="args">The arguments.</param>
        private static void Main(string[] args)
        {
            using (var program = new Program())
            {
                program.Run(args);
            }
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {

        }

        /// <summary>Runs this instance.</summary>
        /// <param name="args">The arguments.</param>
        private void Run(string[] args)
        {
            Console.WriteLine();
            Console.WriteLine("This application allows you to generate a report from your Toggl account via the API. You need to provide a API token to use this function.");
            Console.WriteLine();
            var apiToken = GetAPITokenFromUser();
            var startDate = GetDateFromUser("Please enter a valid Start Date with format yyyy-MM-dd? ");
            var endDate = GetDateFromUser("Please enter a valid End Date with format yyyy-MM-dd? ");
            Console.WriteLine("Generating Report...");
            var reportData = new TogglCsvGroupedByDayReport(new TimeEntryService(apiToken))
                .GetReportData(startDate, endDate);
            Console.WriteLine("Saving to the Desktop...");
            new ReportDataManager()
                .SaveCsvReportDataToDesktop(reportData);
        }

        /// <summary>Gets the API token from user.</summary>
        /// <returns>The API Token</returns>
        private static string GetAPITokenFromUser()
        {
            string apiToken;
            var isValidAPIToken = false;
            do
            {
                Console.Write("Please enter a valid API token? ");
                apiToken = Console.ReadLine();
                Console.WriteLine();
                isValidAPIToken = !string.IsNullOrWhiteSpace(apiToken);
            } while (!isValidAPIToken);
            return apiToken;
        }

        /// <summary>Gets the date from user.</summary>
        /// <param name="message">The message.</param>
        /// <returns>a Date entered from User</returns>
        private static DateTime GetDateFromUser(string message)
        {
            DateTime date;
            var isValidDate = false;
            do
            {
                Console.Write(message);
                var dateString = Console.ReadLine();
                Console.WriteLine();
                isValidDate = DateTime.TryParse(dateString, out date);
            } while (!isValidDate);
            return date;
        }

    }
}
