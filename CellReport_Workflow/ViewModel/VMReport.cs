using CellReport_Workflow.Models.Report;
using CellReport_Workflow.Models.User;

namespace CellReport_Workflow.ViewModel
{
    public class VMReport
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
        public string? Final_Date { get; set; }
        public string? CurrentVersion { get; set; }
        public string? WhoSign { get; set; }
        public VMReportFile? VMReportFile { get; set; }

        //public VMReport()
        //{

        //}

        //public VMReport(List<Report> report)
        //{
        //    UserFactory userFactory = new UserFactory();
        //    User Lab_C_M = userFactory.GetLabManager(CDictionary.C_LAB); //Lab_C_M.Account
        //    User Lab_B_M = userFactory.GetLabManager(CDictionary.B_LAB);
        //    List<VMReport> datas = new();
        //    for(int i = 0; i<report.Count; i++)
        //    {
        //        VMReport x = new();
        //        x.Id = report[i].Id;
        //        x.cus_ct_id = report[i].cus_ct_id;
        //        x.SEQ_NUM = report[i].SEQ_NUM;
        //        x.ProductType = report[i].ProductType;
        //        x.ProjectType = report[i].ProjectType;
        //        x.MOTHER_NAME = report[i].MOTHER_NAME;
        //        x.stage = report[i].stage;
        //        x.REC_DATE = report[i].REC_DATE;
        //        x.ReportDate = report[i].ReportDate;
        //        x.Final_Date = report[i].Final_Date;
        //        x.CurrentVersion = report[i].CurrentVersion;
        //        //x.Next =
        //        if(x.ProductType =="ADSC" || x.ProductType=="MSC" || x.ProductType == "TSC")
        //        {
        //            if(x.stage == CDictionary.)
        //        }
        //        else if (x.ProductType == "CB" || x.ProductType == "PBSC")
        //        {

        //        }
        //            datas.Add(x);
        //    }
        //}
    }
}
