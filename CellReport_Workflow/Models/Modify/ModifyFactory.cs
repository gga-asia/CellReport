using CellReport_Workflow.Models.Sql;
using CellReport_Workflow.ViewModel;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace CellReport_Workflow.Models.Modify
{
    public class ModifyFactory
    {
        SqlBase sqlBase = new();

        public List<Modify> GetModifyList(string Id, string ProductType, /*string Status,*/ string Date_s, string Date_e, string ModifyId = "")
        {
            List<SqlParameter> Paras = new();
            List<Modify> datas = new();

            string sql = "SELECT M.[Id], M.[cus_ct_id], M.[Apply_Date], M.[Apply_By], M.[Reson], M.[reply_Date], M.[reply_By], M.[reply], M.[reply_remark] FROM cr_Modify M";

            sql += " Left JOIN cr_main CM ON M.cus_ct_id = CM.cus_ct_id ";
            sql += "WHERE 1=1 ";


            if (!string.IsNullOrEmpty(Id))
            {
                sql += " And CM.cus_ct_id = @K_cus_ct_id ";
                Paras.Add(new SqlParameter("K_cus_ct_id", Id));
            }
            if (!string.IsNullOrEmpty(ProductType))
            {
                sql += " And CM.ProductType = @K_ProductType ";
                Paras.Add(new SqlParameter("K_ProductType", ProductType));
            }
            if (!string.IsNullOrEmpty(Date_s))
            {
                sql += " And M.Apply_Date >= @K_Date_s ";
                Paras.Add(new SqlParameter("K_Date_s", Date_s));
            }
            if (!string.IsNullOrEmpty(Date_e))
            {
                sql += " And M.Apply_Date <= @K_Date_e ";
                Paras.Add(new SqlParameter("K_Date_e", Date_e));
            }
            if (!string.IsNullOrEmpty(ModifyId))
            {
                sql += " And M.[Id] = @K_Id ";
                Paras.Add(new SqlParameter("K_Id", ModifyId));
            }

            sql += " order by M.[Apply_Date] ";

            datas = ModifytReader(datas, sql, Paras);

            return datas;
        }

        public List<Modify> GetModifyListByLab(string Id, string ProductType, string Date_s, string Date_e, string Lab)
        {
            List<SqlParameter> Paras = new();
            List<Modify> datas = new();

            string sql = "SELECT M.[Id], M.[cus_ct_id], M.[Apply_Date], M.[Apply_By], M.[Reson], M.[reply_Date], M.[reply_By], M.[reply], M.[reply_remark] FROM cr_Modify M";

            sql += " Left JOIN cr_main CM ON M.cus_ct_id = CM.cus_ct_id ";
            sql += "WHERE 1=1 ";

            if (Lab == CDictionary.C_LAB && string.IsNullOrEmpty(ProductType))
            {
                sql += " And (CM.ProductType = 'ADSC' OR CM.ProductType = 'MSC' OR CM.ProductType = 'TSC' )";
            }
            else if (Lab == CDictionary.B_LAB && string.IsNullOrEmpty(ProductType))
            {
                sql += " And (CM.ProductType = 'CB' OR CM.ProductType = 'PBSC' )";
            }


            if (!string.IsNullOrEmpty(Id))
            {
                sql += " And CM.cus_ct_id = @K_cus_ct_id ";
                Paras.Add(new SqlParameter("K_cus_ct_id", Id));
            }
            if (!string.IsNullOrEmpty(ProductType))
            {
                sql += " And CM.ProductType = @K_ProductType ";
                Paras.Add(new SqlParameter("K_ProductType", ProductType));
            }
            if (!string.IsNullOrEmpty(Date_s))
            {
                sql += " And M.Apply_Date >= @K_Date_s ";
                Paras.Add(new SqlParameter("K_Date_s", Date_s));
            }
            if (!string.IsNullOrEmpty(Date_e))
            {
                sql += " And M.Apply_Date <= @K_Date_e ";
                Paras.Add(new SqlParameter("K_Date_e", Date_e));
            }


            sql += " order by M.[Apply_Date] ";

            datas = ModifytReader(datas, sql, Paras);

            return datas;
        }


        public List<VMModify> GetModifyRecordList(string Id)
        {
            List<Modify> MDatas = GetModifyList(Id, "", "", "");
            MDatas = MDatas.OrderBy(x => x.Id).ToList();
            List<VMModify> VMDatas = new();
            if (MDatas.Count > 0)
            {
                for (int i = 0; i <= MDatas.Count - 1; i++)
                {
                    VMModify z;
                    if (!string.IsNullOrEmpty(MDatas[i].reply_Date))
                    {
                        z = new();
                        z.Datetime = MDatas[i].reply_Date;
                        z.Emp_Id = MDatas[i].reply_By;
                        z.Status = MDatas[i].reply; ;
                        z.Remark = MDatas[i].reply_remark;
                        VMDatas.Add(z);
                    }
                    else
                    {
                        z = new();
                        z.Datetime = "待回覆";
                        z.Emp_Id = MDatas[i].reply_By;
                        z.Status = "待回覆";
                        z.Remark = "待回覆";
                        VMDatas.Add(z);
                    }
                    z = new();
                    z.Datetime = MDatas[i].Apply_Date;
                    z.Emp_Id = MDatas[i].Apply_By;
                    z.Status = "申請";
                    z.Remark = MDatas[i].Reson;
                    VMDatas.Add(z);
                }
                return VMDatas;
            }
            else return new();
        }

        public string CheckApply(string cus_ct_id)
        {
            string sql = "SELECT Id, cus_ct_id, Apply_Date, Apply_By, Reson,reply_Date, reply_By, reply,reply_remark  FROM cr_Modify where cus_ct_id = @K_cus_ct_id and reply is null";
            List<SqlParameter> Paras = new() { new SqlParameter("K_cus_ct_id", cus_ct_id) };
            string DATA = sqlBase.StringReader(sql, "Apply_By", Paras);
            return DATA;
        }






        private List<Modify> ModifytReader(List<Modify> datas, string sql, List<SqlParameter> Paras)
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
                            Modify x = new();
                            x.Id = !dr.IsDBNull(dr.GetOrdinal("Id")) ? dr["Id"].ToString() : "";//
                            x.cus_ct_id = !dr.IsDBNull(dr.GetOrdinal("cus_ct_id")) ? dr["cus_ct_id"].ToString() : "";//
                            x.Reson = !dr.IsDBNull(dr.GetOrdinal("Reson")) ? dr["Reson"].ToString() : "";//
                            x.Apply_By = !dr.IsDBNull(dr.GetOrdinal("Apply_By")) ? dr["Apply_By"].ToString() : "";//
                            x.Apply_Date = !dr.IsDBNull(dr.GetOrdinal("Apply_Date")) ? DateTimeConverter(dr["Apply_Date"].ToString()) : "";
                            x.reply_By = !dr.IsDBNull(dr.GetOrdinal("reply_By")) ? dr["reply_By"].ToString() : "";//
                            x.reply_Date = !dr.IsDBNull(dr.GetOrdinal("reply_Date")) ? DateTimeConverter(dr["reply_Date"].ToString()) : "";//
                            x.reply = !dr.IsDBNull(dr.GetOrdinal("reply")) ? dr["reply"].ToString() : "";
                            x.reply_remark = !dr.IsDBNull(dr.GetOrdinal("reply_remark")) ? dr["reply_remark"].ToString() : "";
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
                string msg = ex.Message;
                return datas;
            }
        }


        public Modify Get_cr_Modify(string Id)
        {
            List<Modify> data = new();
            string sql = "SELECT top(1) [Id], [cus_ct_id], [Apply_Date], [Apply_By], [Reson], [reply_Date], [reply_By], [reply], [reply_remark] FROM cr_Modify";

            sql += " where Id = @K_Id ";
            List<SqlParameter> Paras = new() { new SqlParameter("K_Id", Id) };
            data = sqlBase.DynamicDataBuidler<Modify>(sql, Paras);
            if (data.Count > 0)
                return data[0];
            else
                return new();
        }
        public List<string> Get_Modify_ProductType(string Id)
        {
            List<string> datas = new();
            string sql = "SELECT top(1) ProductType FROM cr_main M";
            sql += " Left JOIN cr_Modify MO ON M.cus_ct_id = MO.cus_ct_id ";
            sql += " where MO.Id = @K_Id ";
            List<SqlParameter> Paras = new() { new SqlParameter("K_Id", Id) };
            datas.Add(sqlBase.StringReader(sql, "ProductType", Paras));

            sql = "SELECT top(1) SEQ_NUM FROM cr_main M";
            sql += " Left JOIN cr_Modify MO ON M.cus_ct_id = MO.cus_ct_id ";
            sql += " where MO.Id = @K_Id ";
            List<SqlParameter> Paras1 = new() { new SqlParameter("K_Id", Id) };
            datas.Add(sqlBase.StringReader(sql, "SEQ_NUM", Paras1));

            sql = "SELECT top(1) M.cus_ct_id FROM cr_main M";
            sql += " Left JOIN cr_Modify MO ON M.cus_ct_id = MO.cus_ct_id ";
            sql += " where MO.Id = @K_Id ";
            List<SqlParameter> Paras2 = new() { new SqlParameter("K_Id", Id) };
            datas.Add(sqlBase.StringReader(sql, "cus_ct_id", Paras2));
            //sql = "SELECT top(1) CurrentVersion FROM cr_main M";
            //sql += " Left JOIN cr_Modify MO ON M.cus_ct_id = MO.cus_ct_id ";
            //sql += " where MO.Id = @K_Id ";
            //datas[3] = sqlBase.StringReader(sql, "CurrentVersion", Paras);
            return datas;
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
