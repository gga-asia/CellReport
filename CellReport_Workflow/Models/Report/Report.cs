using CellReport_Workflow.ViewModel;

namespace CellReport_Workflow.Models.Report
{
    public class Report
    {
        public string? Id { get; set; }
        public string? cus_ct_id { get; set; }
        public string? SEQ_NUM { get; set; }
        public string? ProductType { get; set; }
        public string? ProjectType { get; set; }
        public string? MOTHER_NAME { get; set; }
        public string? stage { get; set; }
        public string? REC_DATE { get; set; }
        public string? ReportDate { get; set; }
        public string? Upload_Date { get; set; }
        public string? Final_Date { get; set; }
        public string? CurrentVersion { get; set; }
       public string? WhoSign { get; set; }
        public VMReportFile? VMReportFile { get; set; }
    }
}
