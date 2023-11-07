using CellReport_Workflow.Models.Record;
using CellReport_Workflow.Models.Report;
using CellReport_Workflow.Models.Sql;
using CellReport_Workflow.Models.User;
using CellReport_Workflow.ViewModel;
using Microsoft.Data.SqlClient;
using System;
using System.Data.SqlTypes;

namespace CellReport_Workflow.Models.Modify
{
    public class Modify_ADO
    {
        SqlBase sqlBase = new();
        ModifyFactory ModifyFactory = new();
        Report_ADO Report_ADO = new();
        Record_ADO Record_ADO = new();

        public string INSERT_ModifyApply(string cus_ct_id, string Reson, User.User user)
        {
            Report.Report reportInfo = new Report.ReportFactory().Get_cr_main(cus_ct_id);
            User.User labm = new();
            if (reportInfo.ProductType == "ADSC" || reportInfo.ProductType == "MSC" || reportInfo.ProductType == "TSC")
            {
                labm = new User.UserFactory().GetLabManager(CDictionary.C_LAB);
            }
            else if (reportInfo.ProductType == "CB" || reportInfo.ProductType == "PBSC")
            {
                labm = new User.UserFactory().GetLabManager(CDictionary.B_LAB); 
            }

            string sql = " INSERT INTO cr_Modify (cus_ct_id, Apply_Date, Apply_By, Reson, reply_By) VALUES (@K_cus_ct_id, @K_Apply_Date, @K_Apply_By, @K_Reson, @K_reply_By) ";

            List<SqlParameter> Paras = new()
            {
                new SqlParameter("K_cus_ct_id", cus_ct_id),
                new SqlParameter("K_Apply_Date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                new SqlParameter("K_Apply_By", user.Emp_Id),
                new SqlParameter("K_Reson", Reson),
                new SqlParameter("K_reply_By", labm.Emp_Id)
            };
            return sqlBase.SqlNonQuery(sql, Paras);
        }
        public string Updata_ModifyReply(string Id, string reply, string reply_remark, User.User user)
        {
            string sql = " Update cr_Modify SET reply_Date = @K_reply_Date ,  reply_By = @K_reply_By , reply = @K_reply , reply_remark = @K_reply_remark ";
            sql += " WHERE Id = @K_Id ";
            List<SqlParameter> Paras = new()
            {
                new SqlParameter("K_Id", Id),
                new SqlParameter("K_reply_Date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                new SqlParameter("K_reply_By", user.Emp_Id),
                new SqlParameter("K_reply", reply),
                new SqlParameter("K_reply_remark", reply_remark),
            };
            string sta1 = sqlBase.SqlNonQuery(sql, Paras);
            Modify Modifydatas = ModifyFactory.GetModifyList("", "", "", "", Id)[0];
            if (reply == "同意")
            {
                string sta2 = "";
                string sta3 = "";
                string sta4 = "";
                List<string> info = ModifyFactory.Get_Modify_ProductType(Id); //[0]ProductType  [1]SEQ_NUM [2]cus_ct_id
                if (info[0] == "ADSC" || info[0] == "MSC")
                {
                    sql = "  Update AP_MSC_T Set Status_Code = 'F' Where SEQ_NUM = @K_SEQ_NUM ";
                    List<SqlParameter> Paras2 = new() { new SqlParameter("K_SEQ_NUM", info[1]) };
                    sta2 = sqlBase.SqlNonQuery(sql, Paras2);
                }
                else if (info[0] == "TSC")
                {
                    sql = $"Update TSC_TOOTH set  STATUSCODE = 'F' Where TOOTH_NO = @K_TOOTH_NO ";
                    List<SqlParameter> Paras2 = new() { new SqlParameter("K_TOOTH_NO", info[2]) };
                    sta2 = sqlBase.SqlNonQuery(sql, Paras2);
                }
                else if (info[0] == "CB")
                {
                    sql = " Update AP_CB_T Set Status_Code = 'F', Update_By2 = @K_Emp_Id, Update_Date2 = GetDate()  Where SEQ_NUM = @K_SEQ_NUM ";
                    List<SqlParameter> Paras2 = new() { new SqlParameter("K_Emp_Id", user.Emp_Id), new SqlParameter("K_SEQ_NUM", info[1]) };
                    sta2 = sqlBase.SqlNonQuery(sql, Paras2);
                }
                else if (info[0] == "PBSC")
                {
                    sql = " Update AP_PBSC_T Set Status_Code = 'F', Update_By2 = @K_Emp_Id, Update_Date2 = GetDate() Where PBSC_FULL_ID = @K_PBSC_FULL_ID";
                    List<SqlParameter> Paras2 = new() { new SqlParameter("K_Emp_Id", user.Emp_Id), new SqlParameter("K_PBSC_FULL_ID", info[2]) };
                    sta2 = sqlBase.SqlNonQuery(sql, Paras2);
                }
                //else
                //{
                //    return "ProductType ERR";
                //}

                sta3 = Report_ADO.update_cr_main_stage(info[2], CDictionary.STAGE_MODIFY);
                sta4 = Record_ADO.Insert_cr_SignRecord(info[2], DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Modifydatas.Apply_By, CDictionary.STAGE_MODIFY, "申請通過", "", "");


                if (sta1 == CDictionary.OK && sta2 == CDictionary.OK && sta3 == CDictionary.OK) 
                    return sta1;
                else 
                    return (sta1 + sta2 + sta3);


            }
            else return sta1;
        }


    }
}
