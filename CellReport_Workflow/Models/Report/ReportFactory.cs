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
            sql += " C.MOTHER_NAME, MSC.Final_Date , MSC.REC_DATE, E.ENG_NAME ";
            sql += " FROM cr_main CM ";
            sql += " Left JOIN AP_MSC_T MSC ON MSC.SEQ_NUM = CM.SEQ_NUM ";
            sql += " INNER JOIN Customer C ON C.SEQ_NUM = MSC.SEQ_NUM ";
            sql += " left join cr_SignRecord S on cm.cus_ct_id = S.cus_ct_id AND S.stage =CM.stage  AND S.Id = (SELECT TOP(1) Id FROM cr_SignRecord WHERE  cus_ct_id = cm.cus_ct_id ORDER BY ID DESC) ";
            sql += " left join EMPLOYEE E ON S.emp_id = E.EMP_ID ";
            sql += "WHERE 1=1 ";
            //sql += " And CM.ProductType = 'ADSC' OR CM.ProductType = 'MSC' ";
            if (!string.IsNullOrEmpty(Id))
            {
                sql += " And CM.cus_ct_id LIKE @K_cus_ct_id";
                Paras.Add(new("K_cus_ct_id", "%" + Id + "%"));
            }
            if (!string.IsNullOrEmpty(ProductType))
            {
                sql += " And CM.ProductType = @K_ProductType ";
                Paras.Add(new("K_ProductType", ProductType));
            }
            if (!string.IsNullOrEmpty(Status))
            {
                if (Status == "ALL") { }
                else
                {
                    sql += " And CM.stage = @K_stage ";
                    Paras.Add(new("K_stage", Status));
                }
            }
            else
            {
                sql += $" And CM.stage <> '{CDictionary.STAGE_RELEASE}' ";
            }
            if (!string.IsNullOrEmpty(Finish_s))
            {
                sql += " And MSC.Final_Date >= @K_Final_Date_s ";
                Paras.Add(new("K_Final_Date_s", Finish_s));
            }
            if (!string.IsNullOrEmpty(Finish_e))
            {
                sql += " And MSC.Final_Date <= @K_Final_Date_e ";
                Paras.Add(new("K_Final_Date_e", Finish_e));
            }
            if (user != null)
            {
                if (user.Rank == CDictionary.LAB_MANAGER)
                {
                    sql += $" And (CM.stage = '{CDictionary.STAGE_QC_SING}' OR CM.stage = '{CDictionary.STAGE_FINISH}' )";
                }
                else if (user.Rank == CDictionary.LAB_QCMANAGER)
                {
                    sql += $" And CM.stage = '{CDictionary.STAGE_LAB_SEND}' ";
                }
            }

            datas = ReportReader(datas, sql, Paras);
            Paras.Clear();

            sql = "SELECT  top(1000) CM.cus_ct_id, CM.ProductType, CM.ProjectType, CM.CurrentVersion, CM.stage, CM.SEQ_NUM, ";
            sql += " CB.Final_Date, C.MOTHER_NAME, CB.REC_DATE , E.ENG_NAME";
            sql += " FROM cr_main CM ";
            sql += " Left JOIN AP_CB_T CB ON CB.SEQ_NUM = CM.SEQ_NUM ";
            sql += " INNER JOIN Customer C WITH(NOLOCK) ON C.SEQ_NUM = CB.SEQ_NUM ";
            sql += " left join cr_SignRecord S on cm.cus_ct_id = S.cus_ct_id AND S.stage =CM.stage  AND S.Id = (SELECT TOP(1) Id FROM cr_SignRecord WHERE  cus_ct_id = cm.cus_ct_id ORDER BY ID DESC) ";
            sql += " left join EMPLOYEE E ON S.emp_id = E.EMP_ID ";
            sql += "WHERE 1=1 ";
            sql += "";
            if (!string.IsNullOrEmpty(Id))
            {
                sql += " And CM.cus_ct_id LIKE @K_cus_ct_id";
                Paras.Add(new("K_cus_ct_id", "%" + Id + "%"));
            }
            if (!string.IsNullOrEmpty(ProductType))
            {
                sql += " And CM.ProductType = @K_ProductType ";
                Paras.Add(new("K_ProductType", ProductType));
            }
            if (!string.IsNullOrEmpty(Status))
            {
                if (Status == "ALL") { }
                else
                {
                    sql += " And CM.stage = @K_stage ";
                    Paras.Add(new("K_stage", Status));
                }
            }
            else
            {
                sql += $" And CM.stage <> '{CDictionary.STAGE_RELEASE}' ";
            }
            if (!string.IsNullOrEmpty(Finish_s))
            {
                sql += " And CB.Final_Date >= @K_Final_Date_s ";
                Paras.Add(new("K_Final_Date_s", Finish_s));
            }
            if (!string.IsNullOrEmpty(Finish_e))
            {
                sql += " And CB.Final_Date <= @K_Final_Date_e ";
                Paras.Add(new("K_Final_Date_e", Finish_e));
            }
            if (user != null)
            {
                if (user.Rank == CDictionary.LAB_MANAGER)
                {
                    sql += $" And (CM.stage = '{CDictionary.STAGE_QC_SING}' OR CM.stage = '{CDictionary.STAGE_FINISH}' )";
                }
                else if (user.Rank == CDictionary.LAB_QCMANAGER)
                {
                    sql += $" And CM.stage = '{CDictionary.STAGE_LAB_SEND}' ";
                }
            }

            datas = ReportReader(datas, sql, Paras);
            Paras.Clear();


            sql = "SELECT  top(1000) CM.cus_ct_id, CM.ProductType, CM.ProjectType, CM.CurrentVersion, CM.stage, CM.SEQ_NUM,  C.MOTHER_NAME,";
            sql += " BLOOD_NO, P.SEQ_NUM,P.SID ,P.PBSC_FULL_ID, P.SEQ_NUM, T.Product_Type, T.Code AS ProjectType, P.Final_Date, P.REC_DATE , E.ENG_NAME ";
            sql += " FROM cr_main CM  ";
            sql += " left join AP_PBSC_T P ON CM.cus_ct_id = P.PBSC_FULL_ID ";
            sql += " INNER JOIN CUSTOMER C ON P.SEQ_NUM = C.SEQ_NUM    ";
            sql += " INNER JOIN  AP_ProjectType_T T ON C.ProjectType = T.SID  ";
            sql += " left join cr_SignRecord S on cm.cus_ct_id = S.cus_ct_id AND S.stage =CM.stage  AND S.Id = (SELECT TOP(1) Id FROM cr_SignRecord WHERE  cus_ct_id = cm.cus_ct_id ORDER BY ID DESC) ";
            sql += " left join EMPLOYEE E ON S.emp_id = E.EMP_ID ";
            sql += "WHERE 1=1 ";
            sql += "";
            if (!string.IsNullOrEmpty(Id))
            {
                sql += " And CM.cus_ct_id LIKE @K_cus_ct_id";
                Paras.Add(new("K_cus_ct_id", "%" + Id + "%"));
            }
            if (!string.IsNullOrEmpty(ProductType))
            {
                if (ProductType == "PBSC")
                {
                    sql += "  And (CM.ProductType = 'PBSC' OR CM.ProductType = 'WB') ";
                }
                else
                {
                    sql += " And CM.ProductType = @K_ProductType ";
                    Paras.Add(new("K_ProductType", ProductType));
                }
            }
            if (!string.IsNullOrEmpty(Status))
            {
                if (Status == "ALL") { }
                else
                {
                    sql += " And CM.stage = @K_stage ";
                    Paras.Add(new("K_stage", Status));
                }
            }
            else
            {
                sql += $" And CM.stage <> '{CDictionary.STAGE_RELEASE}' ";
            }
            if (!string.IsNullOrEmpty(Finish_s))
            {
                sql += " And CB.Final_Date >= @K_Final_Date_s ";
                Paras.Add(new("K_Final_Date_s", Finish_s));
            }
            if (!string.IsNullOrEmpty(Finish_e))
            {
                sql += " And CB.Final_Date <= @K_Final_Date_e ";
                Paras.Add(new("K_Final_Date_e", Finish_e));
            }
            if (user != null)
            {
                if (user.Rank == CDictionary.LAB_MANAGER)
                {
                    sql += $" And (CM.stage = '{CDictionary.STAGE_QC_SING}' OR CM.stage = '{CDictionary.STAGE_FINISH}' )";
                }
                else if (user.Rank == CDictionary.LAB_QCMANAGER)
                {
                    sql += $" And CM.stage = '{CDictionary.STAGE_LAB_SEND}' ";
                }
            }


            datas = ReportReader(datas, sql, Paras);
            Paras.Clear();

            sql = " SELECT  top(1000) CM.cus_ct_id, CM.ProductType, CM.ProjectType, CM.CurrentVersion, CM.stage, CM.SEQ_NUM,  C.MOTHER_NAME, ";
            sql += " BLOOD_NO, C.SEQ_NUM ,T.TSC_FULL_ID, T.TOOTH_NO, PJ.Product_Type, PJ.Code AS ProjectType, T.F_DATE as Final_Date, T.TOOTH_COLLECTION_TIME as REC_DATE , E.ENG_NAME ";
            sql += " FROM cr_main CM ";
            sql += " left join TSC_TOOTH T ON CM.cus_ct_id = T.TSC_FULL_ID ";
            sql += " INNER JOIN TSC_COLLECT_DATA TCD ON T.CASE_ID = TCD.CASE_ID  ";
            sql += " INNER JOIN CUSTOMER C ON TCD.SEQ_NUM = C.SEQ_NUM  ";
            sql += " INNER JOIN  AP_ProjectType_T PJ ON C.ProjectType = PJ.SID ";
            sql += " left join cr_SignRecord S on cm.cus_ct_id = S.cus_ct_id AND S.stage =CM.stage  AND S.Id = (SELECT TOP(1) Id FROM cr_SignRecord WHERE  cus_ct_id = cm.cus_ct_id ORDER BY ID DESC) ";
            sql += " left join EMPLOYEE E ON S.emp_id = E.EMP_ID ";
            sql += " WHERE 1=1 ";

            if (!string.IsNullOrEmpty(Id))
            {
                sql += " And CM.cus_ct_id LIKE @K_cus_ct_id";
                Paras.Add(new("K_cus_ct_id", "%" + Id + "%"));
            }
            if (!string.IsNullOrEmpty(ProductType))
            {
                sql += " And CM.ProductType = @K_ProductType ";
                Paras.Add(new("K_ProductType", ProductType));
            }
            if (!string.IsNullOrEmpty(Status))
            {
                if (Status == "ALL") { }
                else
                {
                    sql += " And CM.stage = @K_stage ";
                    Paras.Add(new("K_stage", Status));
                }
            }
            else
            {
                sql += $" And CM.stage <> '{CDictionary.STAGE_RELEASE}' ";
            }
            if (!string.IsNullOrEmpty(Finish_s))
            {
                sql += " And CB.Final_Date >= @K_Final_Date_s ";
                Paras.Add(new("K_Final_Date_s", Finish_s));
            }
            if (!string.IsNullOrEmpty(Finish_e))
            {
                sql += " And CB.Final_Date <= @K_Final_Date_e ";
                Paras.Add(new("K_Final_Date_e", Finish_e));
            }
            if (user != null)
            {
                if (user.Rank == CDictionary.LAB_MANAGER)
                {
                    sql += $" And (CM.stage = '{CDictionary.STAGE_QC_SING}' OR CM.stage = '{CDictionary.STAGE_FINISH}' )";
                }
                else if (user.Rank == CDictionary.LAB_QCMANAGER)
                {
                    sql += $" And CM.stage = '{CDictionary.STAGE_LAB_SEND}' ";
                }
            }

            datas = ReportReader(datas, sql, Paras);
            Paras.Clear();


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
            string sql = "SELECT top(1) [cus_ct_id],[SEQ_NUM],[ProductType],[ProjectType],[CurrentVersion],[stage],[Upload_Date] FROM cr_main";
            //string sql = "SELECT  top(1) CM.cus_ct_id, CM.ProductType, CM.ProjectType, CM.CurrentVersion, CM.stage, CM.SEQ_NUM, ";
            //sql += " CB.Final_Date, C.MOTHER_NAME, CB.REC_DATE";
            //sql += " FROM cr_main CM ";
            //sql += " Left JOIN AP_CB_T CB ON CB.SEQ_NUM = CM.SEQ_NUM ";
            //sql += " INNER JOIN Customer C WITH(NOLOCK) ON C.SEQ_NUM = CB.SEQ_NUM ";

            sql += " where cus_ct_id = @K_cus_ct_id ";
            List<SqlParameter> Paras = new() { new SqlParameter("K_cus_ct_id", Id) };
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
            string sql = " SELECT TOOTH_NO FROM TSC_TOOTH WHERE TSC_FULL_ID = @K_TSC_FULL_ID ";
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
                            x.WhoSign = !dr.IsDBNull(dr.GetOrdinal("ENG_NAME")) ? dr["ENG_NAME"].ToString() : "";//
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
