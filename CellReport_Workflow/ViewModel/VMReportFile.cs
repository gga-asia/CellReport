namespace CellReport_Workflow
{
    public class VMReportFile
    {
        public List<string>? CurrentReports { get; set; }
        public List<string>? HistoryReports { get; set; }
        public List<string>? UploadReports { get; set; }
        public VMReportFile()
        {
            CurrentReports = new();
            HistoryReports = new();
            UploadReports = new();
        }
    }
}
