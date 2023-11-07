using CellReport_Workflow.Models.Sql;
using Microsoft.Data.SqlClient;

namespace CellReport_Workflow.Models.Record
{
    public class RecordFactory
    {
        SqlBase sqlBase = new();


        public List<Record> GetRecordList(string cus_ct_id)
        {
            List<SqlParameter> Paras = new();
            List<Record> datas = new();
            string sql = "SELECT C.[Id] , C.[cus_ct_id] , C.[datetime] , E.[ENG_NAME] + '(' +  C.[emp_id]+ ')'  AS emp_id, C.[stage] , C.[state] , C.[remark] , C.[Version] FROM [BBCMS].[dbo].[cr_SignRecord] C  ";
            sql += "left join EMPLOYEE E ON E.emp_id = C.emp_id";
            sql += " WHERE  cus_ct_id  = @K_cus_ct_id ";
            Paras.Add(new SqlParameter("K_cus_ct_id", cus_ct_id));
            datas= ReportReader(datas, sql, Paras);
            return datas;
        }


        private List<Record> ReportReader(List<Record> datas, string sql, List<SqlParameter> Paras)
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
                            Record x = new();
                            x.Id = !dr.IsDBNull(dr.GetOrdinal("Id")) ? dr["Id"].ToString() : "";//
                            x.cus_ct_id = !dr.IsDBNull(dr.GetOrdinal("cus_ct_id")) ? dr["cus_ct_id"].ToString() : "";//
                            x.datetime = !dr.IsDBNull(dr.GetOrdinal("datetime")) ? DateTimeConverter(dr["datetime"].ToString()) : "";                            
                            x.emp_id = !dr.IsDBNull(dr.GetOrdinal("emp_id")) ? dr["emp_id"].ToString() : "";
                            x.stage = !dr.IsDBNull(dr.GetOrdinal("stage")) ? dr["stage"].ToString() : "";//
                            x.state = !dr.IsDBNull(dr.GetOrdinal("state")) ? dr["state"].ToString() : "";//
                            x.remark = !dr.IsDBNull(dr.GetOrdinal("remark")) ? dr["remark"].ToString() : "";//
                            x.Version = !dr.IsDBNull(dr.GetOrdinal("Version")) ? dr["Version"].ToString() : "";//
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
