namespace CellReport_Workflow.Models
{
    public class test
    {

        //[Newtonsoft.Json.JsonProperty("Id")]
        public int Id { get; set; }
        public string ReportDate { get; set; }
        public string Cus_CT_ID { get; set; }
        public string Cus_Name { get; set; }
        public string Store_Date { get; set; }

        public string Status { get; set; }
        public string HLA { get; set; }
        public test(int Id1, string ReportDate1, string Cus_CT_ID1, string Cus_Name1, string Store_Date1, string Status1, string HLA1)
        {
            Id = Id1;
            ReportDate = ReportDate1;
            Cus_CT_ID = Cus_CT_ID1;
            Cus_Name = Cus_Name1;
            Store_Date = Store_Date1;
            Status = Status1;
            HLA = HLA1;
        }
    }
}
