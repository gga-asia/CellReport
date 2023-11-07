namespace CellReport_Workflow.Interface
{
    public interface IFileService
    {
        public List<string> GetCurrentReportsPath(string cus_ct_id);
        public List<string> GetHistoryReportsPath(string cus_ct_id);
        public string releaseReport(string cus_ct_id);
        public string backupeReport(string cus_ct_id);
        public string AddImg(string ProductType, string cus_ct_id);
    }
}
