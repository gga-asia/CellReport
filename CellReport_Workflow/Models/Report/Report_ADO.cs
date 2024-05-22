using CellReport_Workflow.Models.Sql;
using Microsoft.Data.SqlClient;
using CellReport_Workflow.Models.User;
using System;
using CellReport_Workflow.ViewModel;
using CellReport_Workflow.Models.Record;

namespace CellReport_Workflow.Models.Report
{
    public class Report_ADO
    {
        SqlBase sqlBase = new();
        ReportFactory reportFactory = new();
        Record_ADO record_ADO = new();


        public string uploadRelease(List<string> datas, User.User user)//列表批次更新
        {
            if (string.IsNullOrEmpty(user.Emp_Id))
                return "使用者資料異常請重新登入";
            for (int i = 0; i <= datas.Count - 1; i++)
            {
                Report info = reportFactory.Get_cr_main(datas[i]);
                string sta = "";
                if (info.stage == CDictionary.STAGE_FINISH)
                {
                    sta = update_cr_main_stage(datas[i], CDictionary.STAGE_RELEASE); //更新主檔cr_main
                    if (sta != CDictionary.OK) return sta;
                    sta = record_ADO.Insert_cr_SignRecord(datas[i], DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), user.Emp_Id, CDictionary.STAGE_RELEASE, "上傳", "", info.CurrentVersion);
                    if (sta != CDictionary.OK) return sta;

                    if (info.ProductType == "ADSC" || info.ProductType == "MSC")
                        _ = update_LIS_ADSC_MSC(info.SEQ_NUM, user);
                    else if (info.ProductType == "TSC")
                        _ = update_LIS_TSC(info.cus_ct_id, user);
                    else if (info.ProductType == "CB")
                        _ = update_LIS_CB(info.SEQ_NUM, user);
                    else if (info.ProductType == "PBSC")
                        _ = update_LIS_PBSC(info.cus_ct_id, user);

                }
                else return datas[i] + " 狀態異常";
            }
            return CDictionary.OK;
        }


        public string UpdateQC_S(string id, User.User user, string ReplyRemark)
        {
            if (string.IsNullOrEmpty(ReplyRemark))
                ReplyRemark = " ";
            if (string.IsNullOrEmpty(user.Emp_Id))
                return "使用者資料異常請重新登入";

            Report info = reportFactory.Get_cr_main(id);
            string sta = "";
            if (info.stage == CDictionary.STAGE_LAB_SEND)
            {
                sta = update_cr_main_stage(id, CDictionary.STAGE_QC_SING);
                if (sta != CDictionary.OK) return sta;
                sta = record_ADO.Insert_cr_SignRecord(id, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), user.Emp_Id, CDictionary.STAGE_QC_SING, "簽核", ReplyRemark, info.CurrentVersion);
                if (sta != CDictionary.OK) return sta;
            }
            return CDictionary.OK;
        }
        //public string Update_All_QC_S(List<string> datas)
        //{
        //    if (datas == null)
        //        return "資料異常(Null)";

        //    if (datas.Count > 0)
        //    {
        //        for(int i =0;  i < datas.Count; i++)
        //        {

        //        }
        //        Report info = reportFactory.Get_cr_main(id);
        //        string sta = "";
        //        if (info.stage == CDictionary.STAGE_LAB_SEND)
        //        {
        //            sta = update_cr_main_stage(id, CDictionary.STAGE_QC_SING);
        //            if (sta != CDictionary.OK) return sta;
        //            sta = record_ADO.Insert_cr_SignRecord(id, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), user.Emp_Id, CDictionary.STAGE_QC_SING, "簽核", ReplyRemark, info.CurrentVersion);
        //            if (sta != CDictionary.OK) return sta;
        //        }

        //    }

        //    return CDictionary.OK;
        //}
        public string UpdateLab_S(string id, User.User user, string ReplyRemark)
        {
            if (string.IsNullOrEmpty(ReplyRemark))
                ReplyRemark = " ";
            if (string.IsNullOrEmpty(user.Emp_Id))
                return "使用者資料異常請重新登入";

            Report info = reportFactory.Get_cr_main(id);
            if (info.stage == CDictionary.STAGE_QC_SING)
            {
                if (!string.IsNullOrEmpty(info.SEQ_NUM))
                {
                    //_ = update_cr_main_stage(id, CDictionary.STAGE_FINISH);
                    _ = update_cr_main_stage(id, CDictionary.STAGE_FINISH);
                    _ = record_ADO.Insert_cr_SignRecord(id, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), user.Emp_Id, CDictionary.STAGE_LAB_SING, "簽核", ReplyRemark, info.CurrentVersion);
                    _ = record_ADO.Insert_cr_SignRecord(id, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), user.Emp_Id, CDictionary.STAGE_FINISH, "", "", info.CurrentVersion);

                    //if (info.ProductType == "ADSC" || info.ProductType == "MSC")
                    //    _ = update_LIS_ADSC_MSC(info.SEQ_NUM, user);
                    //else if (info.ProductType == "TSC")
                    //    _ = update_LIS_TSC(id, user);
                    //else if (info.ProductType == "CB")
                    //    _ = update_LIS_CB(info.SEQ_NUM, user);
                    //else if (info.ProductType == "PBSC")
                    //    _ = update_LIS_PBSC(id, user);
                    return CDictionary.OK;
                }
                else return "SEQ_NUM_NULL_S";
            }
            else return "報告狀態錯誤_S";
        }
        public string UpdateSendLab(string id, User.User user, string ReplyRemark, Report info)
        {
            if (string.IsNullOrEmpty(ReplyRemark))
                ReplyRemark = " ";
            if (string.IsNullOrEmpty(user.Emp_Id))
                return "使用者資料異常請重新登入";

            if (info.stage == CDictionary.STAGE_QC_SING || info.stage == CDictionary.STAGE_LAB_SEND || info.stage == CDictionary.STAGE_LAB_SING)
            {
                if (!string.IsNullOrEmpty(info.SEQ_NUM))
                {
                    _ = update_cr_main_stage(id, CDictionary.STAGE_NOTIFY);
                    _ = record_ADO.Insert_cr_SignRecord(id, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), user.Emp_Id, CDictionary.STAGE_NOTIFY, "", ReplyRemark, info.CurrentVersion);
                    //string sql = "";
                    //if (info.ProductType == "ADSC" || info.ProductType == "MSC")
                    //{
                    //    sql = "  Update AP_MSC_T Set Status_Code = 'F' Where SEQ_NUM = @K_SEQ_NUM ";
                    //    List<SqlParameter> Paras2 = new() { new SqlParameter("K_SEQ_NUM", info.SEQ_NUM) };
                    //     sqlBase.SqlNonQuery(sql, Paras2);
                    //}
                    //else if (info.ProductType == "TSC")
                    //{
                    //    sql = $"Update TSC_TOOTH set  STATUSCODE = 'F' Where TOOTH_NO = @K_TOOTH_NO ";
                    //    List<SqlParameter> Paras2 = new() { new SqlParameter("K_TOOTH_NO", info.SEQ_NUM) };
                    //     sqlBase.SqlNonQuery(sql, Paras2);
                    //}
                    //else if (info.ProductType == "CB")
                    //{
                    //    sql = " Update AP_CB_T Set Status_Code = 'F', Update_By2 = @K_Emp_Id, Update_Date2 = GetDate()  Where SEQ_NUM = @K_SEQ_NUM ";
                    //    List<SqlParameter> Paras2 = new() { new SqlParameter("K_Emp_Id", user.Emp_Id), new SqlParameter("K_SEQ_NUM", info.SEQ_NUM) };
                    //    sqlBase.SqlNonQuery(sql, Paras2);
                    //}
                    //else if (info.ProductType == "PBSC")
                    //{
                    //    sql = " Update AP_PBSC_T Set Status_Code = 'F', Update_By2 = @K_Emp_Id, Update_Date2 = GetDate() Where PBSC_FULL_ID = @K_PBSC_FULL_ID";
                    //    List<SqlParameter> Paras2 = new() { new SqlParameter("K_Emp_Id", user.Emp_Id), new SqlParameter("K_PBSC_FULL_ID", info.SEQ_NUM) };
                    //    sqlBase.SqlNonQuery(sql, Paras2);
                    //}

                    return CDictionary.OK;
                }
                else return "SEQ_NUM_NULL_S";
            }
            else return "報告狀態錯誤_S";
        }


        //===================更新簽核系統
        public string update_cr_main_stage(string id, string stage)  //更新主檔_stage
        {
            List<SqlParameter> Paras = new();
            string sql = " update cr_main set stage =@K_stage ";
            if (stage == CDictionary.STAGE_RELEASE)
            {
                sql += " ,Upload_Date = @K_Upload_Date ";
                Paras.Add(new SqlParameter("K_Upload_Date", DateTime.Now.ToString("yyyy-MM-dd")));
            }
            else if(stage == CDictionary.STAGE_MODIFY)
            {
                sql += " ,Upload_Date = '' ";
            }
            sql += " where cus_ct_id = @K_cus_ct_id ";
            Paras.Add(new SqlParameter("K_cus_ct_id", id));
            Paras.Add(new SqlParameter("K_stage", stage));
            return sqlBase.SqlNonQuery(sql, Paras);
        }


        //===================更新LIS
        public string update_LIS_ADSC_MSC(string id, User.User user)
        {
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(user.Emp_Id))
            {
                string sql = $" Update AP_MSC_T Set Esign = 'F' Where SEQ_NUM = @K_ID ";
                List<SqlParameter> Paras = new(){new("K_ID", id)};
                return sqlBase.SqlNonQuery(sql, Paras);
            }
            return CDictionary.NULL;
        }
        public string update_LIS_TSC(string id, User.User user)  //TSC_F_Delete_UnLockForm     //TSC_F_Update_TOOTH_HW
        {
            string TOOTH_NO = reportFactory.Get_TOOTH_NO(id);
            if (!string.IsNullOrEmpty(TOOTH_NO) && !string.IsNullOrEmpty(user.Emp_Id))
            {
                string sql = $"Update TSC_TOOTH set  Esign = 'F'  Where TOOTH_NO = @K_TOOTH_NO ";
                List<SqlParameter> Paras = new(){new("K_TOOTH_NO", TOOTH_NO)};
                return sqlBase.SqlNonQuery(sql, Paras);
            }
            return CDictionary.NULL;
        }
        public string update_LIS_CB(string id, User.User user)
        {
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(user.Emp_Id))
            {
                string sql = " Update AP_CB_T Set Esign = 'F' Where SEQ_NUM =  @K_ID ";
                List<SqlParameter> Paras = new(){new("K_ID", id)};
                return sqlBase.SqlNonQuery(sql, Paras);
            }
            return CDictionary.NULL;
        }
        public string update_LIS_PBSC(string id, User.User user)
        {
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(user.Emp_Id))
            {
                string sql = " Update AP_PBSC_T Set Esign = 'F' Where PBSC_FULL_ID = @K_ID ";
                List<SqlParameter> Paras = new(){new("K_ID", id)};
                return sqlBase.SqlNonQuery(sql, Paras);
            }
            return CDictionary.NULL;
        }
    }
}
