using CellReport_Workflow.Models.Sql;
using Microsoft.Data.SqlClient;

namespace CellReport_Workflow.Models.Record
{
    public class Record_ADO
    {
        SqlBase sqlBase = new();

        public string Insert_cr_SignRecord(string cus_ct_id, string datetime, string emp_id, string stage, string state, string remark, string Version)  //新增簽核紀錄
        {
            string sql = " INSERT INTO cr_SignRecord (cus_ct_id, [datetime], emp_id, stage, state, remark, Version) VALUES (@K_cus_ct_id, @K_datetime, @K_emp_id, @K_stage, @K_state, @K_remark, @K_Version); ";
            List<SqlParameter> Paras = new()
            {
                new SqlParameter("K_cus_ct_id", cus_ct_id),
                new SqlParameter("K_datetime", datetime),
                new SqlParameter("K_emp_id", emp_id),
                new SqlParameter("K_stage", stage),
                new SqlParameter("K_state", state),
                new SqlParameter("K_remark", remark),
                new SqlParameter("K_Version", Version)
            };
            return sqlBase.SqlNonQuery(sql, Paras);
        }
    }
}
