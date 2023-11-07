using CellReport_Workflow.Models.Sql;
using CellReport_Workflow.Models.User;
using CellReport_Workflow.ViewModel;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using System.Collections.Generic;

namespace CellReport_Workflow.Models.Report
{
    public class ReportFactory
    {
        SqlBase sqlBase = new();

        public List<Report> GetReportsList(string Id, string ProductType, string Status, string Finish_s, string Finish_e, User.User user = null)
        {
            List<SqlParameter> Paras = new();
            List<Report> datas = new();
            string sql = "SELECT top(1000) CM.cus_ct_id, CM.ProductType, CM.ProjectType, CM.CurrentVersion, CM.stage, CM.SEQ_NUM, ";
            sql += " C.MOTHER_NAME, MSC.Final_Date , MSC.REC_DATE";
            sql += " FROM cr_main CM ";
            sql += " Left JOIN AP_MSC_T MSC ON MSC.SEQ_NUM = CM.SEQ_NUM ";
            sql += " INNER JOIN Customer C ON C.SEQ_NUM = MSC.SEQ_NUM ";
            sql += "WHERE 1=1 ";
            //sql += " And CM.ProductType = 'ADSC' OR CM.ProductType = 'MSC' ";
            if (!string.IsNullOrEmpty(Id))
            {                
                sql += " And CM.cus_ct_id LIKE @K_cus_ct_id";
                Paras.Add(new SqlParameter("K_cus_ct_id", "%" + Id + "%"));
            }
            if (!string.IsNullOrEmpty(ProductType))
            {
                sql += " And CM.ProductType = @K_ProductType ";
                Paras.Add(new SqlParameter("K_ProductType", ProductType));
            }
            if (!string.IsNullOrEmpty(Status))
            {
                sql += " And CM.stage = @K_stage ";
                Paras.Add(new SqlParameter("K_stage", Status));
            }
            if (!string.IsNullOrEmpty(Finish_s))
            {
                sql += " And MSC.Final_Date >= @K_Final_Date_s ";
                Paras.Add(new SqlParameter("K_Final_Date_s", Finish_s));
            }
            if (!string.IsNullOrEmpty(Finish_e))
            {
                sql += " And MSC.Final_Date <= @K_Final_Date_e ";
                Paras.Add(new SqlParameter("K_Final_Date_e", Finish_e));
            }
            if (user != null) 
            {
                if(user.Department == CDictionary.C_LAB && user.Rank == CDictionary.LAB_MANAGER)
                {
                    sql += $" And (CM.stage = '{CDictionary.STAGE_QC_SING}' OR CM.stage = '{CDictionary.STAGE_FINISH}' )";
                }
                else if (user.Department == CDictionary.B_LAB && user.Rank == CDictionary.LAB_MANAGER)
                {
                    sql += $" And CM.stage = '{CDictionary.STAGE_LAB_SEND}' ";
                }
            }

            datas = ReportReader(datas, sql, Paras);
            Paras.Clear();

            sql = "SELECT  top(1000) CM.cus_ct_id, CM.ProductType, CM.ProjectType, CM.CurrentVersion, CM.stage, CM.SEQ_NUM, ";
            sql += " CB.Final_Date, C.MOTHER_NAME, CB.REC_DATE";
            sql += " FROM cr_main CM ";
            sql += " Left JOIN AP_CB_T CB ON CB.SEQ_NUM = CM.SEQ_NUM ";
            sql += " INNER JOIN Customer C WITH(NOLOCK) ON C.SEQ_NUM = CB.SEQ_NUM ";
            sql += "WHERE 1=1 ";
            sql += "";
            if (!string.IsNullOrEmpty(Id))
            {
                sql += " And CM.cus_ct_id LIKE @K_cus_ct_id";
                Paras.Add(new SqlParameter("K_cus_ct_id", "%" + Id + "%"));               
            }
            if (!string.IsNullOrEmpty(ProductType))
            {
                sql += " And CM.ProductType = @K_ProductType ";
                Paras.Add(new SqlParameter("K_ProductType", ProductType));
            }
            if (!string.IsNullOrEmpty(Status))
            {
                sql += " And CM.stage = @K_stage ";
                Paras.Add(new SqlParameter("K_stage", Status));
            }
            if (!string.IsNullOrEmpty(Finish_s))
            {
                sql += " And CB.Final_Date >= @K_Final_Date_s ";
                Paras.Add(new SqlParameter("K_Final_Date_s", Finish_s));
            }
            if (!string.IsNullOrEmpty(Finish_e))
            {
                sql += " And CB.Final_Date <= @K_Final_Date_e ";
                Paras.Add(new SqlParameter("K_Final_Date_e", Finish_e));
            }

            if (user != null)
            {
                if (user.Department == CDictionary.C_LAB && user.Rank == CDictionary.LAB_MANAGER)
                {
                    sql += $" And CM.stage = '{CDictionary.STAGE_LAB_SEND}' ";
                }
                else if (user.Department == CDictionary.B_LAB && user.Rank == CDictionary.LAB_MANAGER)
                {
                    sql += $" And (CM.stage = '{CDictionary.STAGE_QC_SING}' OR CM.stage = '{CDictionary.STAGE_FINISH}' )";
                }
            }

            datas = ReportReader(datas, sql, Paras);
            return datas;
        }


        //public List<Report> GetWaitingReportsList(string Id, string ProductType, string Status, string Finish_s, string Finish_e, User.User user)
        //{
        //    List<SqlParameter> Paras = new();
        //    List<Report> datas = new();
        //    if(user.Rank == CDictionary.LAB_MANAGER && user.Department == CDictionary.C_LAB)
        //    {
        //        string sql = "SELECT top(1000) CM.cus_ct_id, CM.ProductType, CM.ProjectType, CM.CurrentVersion, CM.stage, CM.SEQ_NUM, ";
        //        sql += " C.MOTHER_NAME, MSC.Final_Date , MSC.REC_DATE";
        //        sql += " FROM cr_main CM ";
        //        sql += " Left JOIN AP_MSC_T MSC ON MSC.SEQ_NUM = CM.SEQ_NUM ";
        //        sql += " INNER JOIN Customer C ON C.SEQ_NUM = MSC.SEQ_NUM ";
        //        sql += "WHERE 1=1 ";
        //        //sql += " And CM.ProductType = 'ADSC' OR CM.ProductType = 'MSC' ";
        //        if (!string.IsNullOrEmpty(Id))
        //        {
        //            sql += " And CM.cus_ct_id LIKE @K_cus_ct_id";
        //            Paras.Add(new SqlParameter("K_cus_ct_id", "%" + Id + "%"));
        //        }
        //        if (!string.IsNullOrEmpty(ProductType))
        //        {
        //            sql += " And CM.ProductType = @K_ProductType ";
        //            Paras.Add(new SqlParameter("K_ProductType", ProductType));
        //        }
        //        if (!string.IsNullOrEmpty(Status))
        //        {
        //            sql += " And CM.stage = @K_stage ";
        //            Paras.Add(new SqlParameter("K_stage", Status));
        //        }
        //        if (!string.IsNullOrEmpty(Finish_s))
        //        {
        //            sql += " And MSC.Final_Date >= @K_Final_Date_s ";
        //            Paras.Add(new SqlParameter("K_Final_Date_s", Finish_s));
        //        }
        //        if (!string.IsNullOrEmpty(Finish_e))
        //        {
        //            sql += " And MSC.Final_Date <= @K_Final_Date_e ";
        //            Paras.Add(new SqlParameter("K_Final_Date_e", Finish_e));
        //        }
        //        datas = ReportReader(datas, sql, Paras);
        //        Paras.Clear();

        //        sql = "SELECT  top(1000) CM.cus_ct_id, CM.ProductType, CM.ProjectType, CM.CurrentVersion, CM.stage, CM.SEQ_NUM, ";
        //        sql += " CB.Final_Date, C.MOTHER_NAME, CB.REC_DATE";
        //        sql += " FROM cr_main CM ";
        //        sql += " Left JOIN AP_CB_T CB ON CB.SEQ_NUM = CM.SEQ_NUM ";
        //        sql += " INNER JOIN Customer C WITH(NOLOCK) ON C.SEQ_NUM = CB.SEQ_NUM ";
        //        sql += "WHERE 1=1 ";
        //        sql += "";
        //        if (!string.IsNullOrEmpty(Id))
        //        {
        //            sql += " And CM.cus_ct_id LIKE @K_cus_ct_id";
        //            Paras.Add(new SqlParameter("K_cus_ct_id", "%" + Id + "%"));
        //        }
        //        if (!string.IsNullOrEmpty(ProductType))
        //        {
        //            sql += " And CM.ProductType = @K_ProductType ";
        //            Paras.Add(new SqlParameter("K_ProductType", ProductType));
        //        }
        //        if (!string.IsNullOrEmpty(Status))
        //        {
        //            sql += " And CM.stage = @K_stage ";
        //            Paras.Add(new SqlParameter("K_stage", Status));
        //        }
        //        if (!string.IsNullOrEmpty(Finish_s))
        //        {
        //            sql += " And CB.Final_Date >= @K_Final_Date_s ";
        //            Paras.Add(new SqlParameter("K_Final_Date_s", Finish_s));
        //        }
        //        if (!string.IsNullOrEmpty(Finish_e))
        //        {
        //            sql += " And CB.Final_Date <= @K_Final_Date_e ";
        //            Paras.Add(new SqlParameter("K_Final_Date_e", Finish_e));
        //        }
        //        datas = ReportReader(datas, sql, Paras);
        //        return datas;
        //    }
            
        //}









        public Report Get_cr_main(string Id)
        {
            List<Report> data = new();
            string sql = "SELECT top(1) [cus_ct_id],[SEQ_NUM],[ProductType],[ProjectType],[CurrentVersion],[stage] FROM cr_main";
            //string sql = "SELECT  top(1) CM.cus_ct_id, CM.ProductType, CM.ProjectType, CM.CurrentVersion, CM.stage, CM.SEQ_NUM, ";
            //sql += " CB.Final_Date, C.MOTHER_NAME, CB.REC_DATE";
            //sql += " FROM cr_main CM ";
            //sql += " Left JOIN AP_CB_T CB ON CB.SEQ_NUM = CM.SEQ_NUM ";
            //sql += " INNER JOIN Customer C WITH(NOLOCK) ON C.SEQ_NUM = CB.SEQ_NUM ";
           
            sql += " where cus_ct_id = @K_cus_ct_id ";
            List<SqlParameter> Paras = new(){new SqlParameter("K_cus_ct_id", Id)};
            //data = ReportReader(data, sql, Paras);
            data = sqlBase.DynamicDataBuidler<Report>(sql, Paras);
            if (data.Count > 0)
                return data[0];
            else
                return new();
        }
        public List<Report> Get_cr_main_List(string Id)
        {
            List<Report> data = new();
            string sql = "SELECT [cus_ct_id],[SEQ_NUM],[ProductType],[ProjectType],[CurrentVersion],[stage] FROM cr_main";

            sql += " where cus_ct_id = @K_cus_ct_id ";
            List<SqlParameter> Paras = new() { new SqlParameter("K_cus_ct_id", Id) };
            //data = ReportReader(data, sql, Paras);
            data = sqlBase.DynamicDataBuidler<Report>(sql, Paras);
            if (data.Count > 0)
                return data;
            else
                return new();
        }

        public string Get_TOOTH_NO(string TSC_FULL_ID)
        {
            string data = "";
            string sql = " SELECT TOOTH_NO FROM TSC_TOOTH WHERER TSC_FULL_ID = @K_TSC_FULL_ID ";
            List<SqlParameter> Paras = new() { new SqlParameter("K_TSC_FULL_ID", TSC_FULL_ID) };
            data = sqlBase.StringReader(sql, "TOOTH_NO", Paras);
            return data;
        }














        private List<Report> ReportReader(List<Report> datas, string sql, List<SqlParameter> Paras)
        {
            try
            {
                using (SqlConnection con = new(sqlBase.connstr))
                {
                    con.Open();
                    using (SqlCommand cmd = new(sql, con))
                    {
                        if (Paras != null && Paras.Count > 0)
                            cmd.Parameters.AddRange(Paras.ToArray());
                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            Report x = new();
                            x.Id = !dr.IsDBNull(dr.GetOrdinal("cus_ct_id")) ? dr["cus_ct_id"].ToString() : "";//
                            x.cus_ct_id = !dr.IsDBNull(dr.GetOrdinal("cus_ct_id")) ? dr["cus_ct_id"].ToString() : "";//
                            x.SEQ_NUM = !dr.IsDBNull(dr.GetOrdinal("SEQ_NUM")) ? dr["SEQ_NUM"].ToString() : "";//
                            x.MOTHER_NAME = !dr.IsDBNull(dr.GetOrdinal("MOTHER_NAME")) ? dr["MOTHER_NAME"].ToString() : "";
                            x.ReportDate = !dr.IsDBNull(dr.GetOrdinal("SEQ_NUM")) ? dr["SEQ_NUM"].ToString() : "";//
                            x.REC_DATE = !dr.IsDBNull(dr.GetOrdinal("REC_DATE")) ? DateTimeConverter(dr["REC_DATE"].ToString()) : "";
                            x.Final_Date = !dr.IsDBNull(dr.GetOrdinal("Final_Date")) ? DateTimeConverter(dr["Final_Date"].ToString()) : "";//
                            x.stage = !dr.IsDBNull(dr.GetOrdinal("stage")) ? dr["stage"].ToString() : "";//
                            x.ProductType = !dr.IsDBNull(dr.GetOrdinal("ProductType")) ? dr["ProductType"].ToString() : "";//
                            x.ProjectType = !dr.IsDBNull(dr.GetOrdinal("ProjectType")) ? dr["ProjectType"].ToString() : "";//
                            x.CurrentVersion = !dr.IsDBNull(dr.GetOrdinal("CurrentVersion")) ? dr["CurrentVersion"].ToString() : "";//
                            datas.Add(x);
                        }
                        dr.Close();
                    }
                    con.Close();
                }
                return datas;
            }
            catch (Exception ex)
            {
                return datas;
            }
        }










        private string DateTimeConverter(string originalDateTimeString)
        {
            try
            {
                if (!string.IsNullOrEmpty(originalDateTimeString))
                {
                    DateTime originalDateTime = DateTime.Parse(originalDateTimeString);
                    string formattedDateTime = originalDateTime.ToString("yyyy/MM/dd HH:mm:ss");
                    return formattedDateTime;
                }
                else return "Err";
            }
            catch (Exception ex)
            {
                return "Err";
            }
        }







    }
}
