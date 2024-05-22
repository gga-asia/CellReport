using CellReport_Workflow.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace CellReport_Workflow.Interface
{
    public interface IFileService
    {
        public VMReportFile GetReportsPath(string cus_ct_id, string Upload_Date);
        public string ReleaseReport(string cus_ct_id);
        //public string backupeReport(string cus_ct_id);
        //public string AddImg(string ProductType, string cus_ct_id);
        public List<string> ScanAllPDFFilesUrl(string FileType, string cus_ct_id);
        public List<string> ScanAllPDFFilesPath(string FileType, string cus_ct_id);
        public string GetMergePDF(List<string> cus_ct_id);
        public string UnSignBackUp(string cus_ct_id);
        public string UnSignBackUp_To_InProgress(string cus_ct_id);
        public List<List<string>> backupeUploadReport(string cus_ct_id, string Version, string Upload_Date); //[0] 舊路徑  [1] 新路徑
        public void REFRESH_FileList(string cus_ct_id);
    }
}
