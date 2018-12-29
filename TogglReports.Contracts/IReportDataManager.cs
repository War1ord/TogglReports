using TogglReports.Models;

namespace TogglReports.Contracts
{
    public interface IReportDataManager
    {
        void SaveCsvReportDataToDesktop(CsvGroupedByDayReportCollection collection);
    }
}
