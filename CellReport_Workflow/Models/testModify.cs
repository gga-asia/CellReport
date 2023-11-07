namespace CellReport_Workflow.Models
{
    public class testModify
    {
        public int Id { get; set; }
        public string ReportDate { get; set; }
        public string Cus_CT_ID { get; set; }
        public string Cus_Name { get; set; }
        public string Store_Date { get; set; }

        public string Apply_Date { get; set; }
        public string Apply_By { get; set; }
        public string Reson { get; set; }
        public string HLA { get; set; }
        public testModify(int Id1, string ReportDate1, string Cus_CT_ID1, string Cus_Name1, string Store_Date1, string Apply_Date1,string Apply_By1,string Reson1, string HLA1)
        {
            Id = Id1;
            ReportDate = ReportDate1;
            Cus_CT_ID = Cus_CT_ID1;
            Cus_Name = Cus_Name1;
            Store_Date = Store_Date1;
            Apply_Date = Apply_Date1;
            Apply_By = Apply_By1;
            Reson = Reson1;
            HLA = HLA1;
        }
    }
}
