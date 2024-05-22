using CellReport_Workflow.Models.Sql;
using CellReport_Workflow.ViewModel;
using Microsoft.Data.SqlClient;

namespace CellReport_Workflow.Models.MailAddress
{
    public class MailAddress_ADO
    {
        public string Insert_GROUPEMP(string Group_ID, string Group_Name, string EMP_ID, string CRE_EMP_ID)  //新增群組人員
        {
            string sql = " IF NOT EXISTS (SELECT 1 FROM GROUPEMP WHERE P_ID = @K_P_ID AND EMP_ID = @K_EMP_ID) ";
            sql += " BEGIN ";
            sql += " INSERT INTO GROUPEMP (P_ID, NAME, TYPE, EMP_ID, G_ID, CRE_EMP_ID, CRE_DATE, ENABLE) ";
            sql += " VALUES (@K_P_ID, @K_NAME, @K_TYPE, @K_EMP_ID, @K_G_ID, @K_CRE_EMP_ID, @K_CRE_DATE, @K_ENABLE); ";
            sql += " END ";
            sql += " ELSE ";
            sql += " BEGIN ";
            sql += " PRINT '資料已存在，不需新增。'; ";
            sql += " END ";
            List<SqlParameter> Paras = new()
            {
                new("K_P_ID", Group_ID),
                new("K_NAME", Group_Name),
                new("K_TYPE", "1"),
                new("K_EMP_ID", EMP_ID),
                new("K_G_ID", "0"),
                new("K_CRE_EMP_ID", CRE_EMP_ID),
                new("K_CRE_DATE", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")),
                new("K_ENABLE", "1")
            };
            return new SqlBase().SqlNonQuery(sql, Paras);
        }
        public string DeleteUserFromGROUPEMP(string Group_ID, string EMP_ID)  //新增群組人員
        {
            if (string.IsNullOrEmpty(EMP_ID) || string.IsNullOrEmpty(Group_ID))
                return CDictionary.ERR;
            string sql = " DELETE FROM GROUPEMP WHERE P_ID = @K_P_ID AND EMP_ID = @K_EMP_ID ";
           
            List<SqlParameter> Paras = new()
            {
                new("K_P_ID", Group_ID),
                new("K_EMP_ID", EMP_ID)
            };
            return new SqlBase().SqlNonQuery(sql, Paras);
        }
    }
}
