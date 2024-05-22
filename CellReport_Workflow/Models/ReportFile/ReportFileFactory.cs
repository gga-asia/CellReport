using CellReport_Workflow.Models.Sql;
using Microsoft.Data.SqlClient;

namespace CellReport_Workflow.Models.ReportFile
{
    public class ReportFileFactory
    {
        //public List<ReportFile> getReportFiles_UPLOAD(string cus_ct_id)
        //{
        //    string sql = "select cus_ct_id ,ReportType ,FilePath ,IsRelease from cr_FileList ";
        //    sql += " where cus_ct_id = @K_cus_ct_id ";
        //    sql += " AND IsRelease = 'Y' ";
        //    List<SqlParameter> Paras = new(){new("K_cus_ct_id", cus_ct_id)};
        //}



        //private List<ReportFile> ReportReader(List<ReportFile> datas, string sql, List<SqlParameter> Paras)
        //{
        //    try
        //    {
        //        using (SqlConnection con = new(sqlBase.connstr))
        //        {
        //            con.Open();
        //            using (SqlCommand cmd = new(sql, con))
        //            {
        //                if (Paras != null && Paras.Count > 0)
        //                    cmd.Parameters.AddRange(Paras.ToArray());
        //                SqlDataReader dr = cmd.ExecuteReader();
        //                while (dr.Read())
        //                {
        //                    Record x = new();
        //                    x.Id = !dr.IsDBNull(dr.GetOrdinal("Id")) ? dr["Id"].ToString() : "";//
        //                    x.cus_ct_id = !dr.IsDBNull(dr.GetOrdinal("cus_ct_id")) ? dr["cus_ct_id"].ToString() : "";//
        //                    x.datetime = !dr.IsDBNull(dr.GetOrdinal("datetime")) ? DateTimeConverter(dr["datetime"].ToString()) : "";
        //                    x.emp_id = !dr.IsDBNull(dr.GetOrdinal("emp_id")) ? dr["emp_id"].ToString() : "";
        //                    x.stage = !dr.IsDBNull(dr.GetOrdinal("stage")) ? dr["stage"].ToString() : "";//
        //                    x.state = !dr.IsDBNull(dr.GetOrdinal("state")) ? dr["state"].ToString() : "";//
        //                    x.remark = !dr.IsDBNull(dr.GetOrdinal("remark")) ? dr["remark"].ToString() : "";//
        //                    x.Version = !dr.IsDBNull(dr.GetOrdinal("Version")) ? dr["Version"].ToString() : "";//
        //                    datas.Add(x);
        //                }
        //                dr.Close();
        //            }
        //            con.Close();
        //        }
        //        return datas;
        //    }
        //    catch (Exception ex)
        //    {
        //        return datas;
        //    }
        //}
    }
}
