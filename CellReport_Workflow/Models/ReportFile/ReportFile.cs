namespace CellReport_Workflow.Models.ReportFile
{
    public class ReportFile //此Class於Service操作
    {
        public string? cus_ct_id { get; set; }
        public string? ReportType { get; set; }
        public string? FilePath { get; set; }
        public string? IsRelease { get; set; }

    }
}
