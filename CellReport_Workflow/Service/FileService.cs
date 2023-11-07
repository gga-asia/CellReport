using CellReport_Workflow.Interface;

namespace CellReport_Workflow.Service
{
    public class FileService: IFileService
    {
        public List<string> GetCurrentReportsPath(string cus_ct_id)
        {
            List<string> CurrentReportsPath = new()
            {
                cus_ct_id
            };
            return CurrentReportsPath;
        }
        public List<string> GetHistoryReportsPath(string cus_ct_id)
        {
            List<string> HistoryReportsPath = new()
            {
                cus_ct_id
            };
            return HistoryReportsPath;  
        }
        public string releaseReport(string cus_ct_id)
        {
            return "";
        }
        public string backupeReport(string cus_ct_id)
        {
            return "";
        }
        public string AddImg(string ProductType, string cus_ct_id)
        {
            return "";
        }
    }
}
