using CellReport_Workflow.Models.Sql;
using CellReport_Workflow.ViewModel;
using Microsoft.Data.SqlClient;
using System;
using System.Data.SqlTypes;

namespace CellReport_Workflow.Models.EditLog
{
    public class EditLog_ADO
    {
        SqlBase sqlBase = new();

        public string BackReportLog(string SEQ_NUM, string UPLOADDATE,string EMP_ID, string REASON, List<string> BackupURL, List<string> OldURL)
        {
            string stu = "";
            List<EditLog> editLogs = new();
            for (int i = 0; i < BackupURL.Count; i++)
            {
                EditLog x = new();
                x.APATH = BackupURL[i];
                x.BPATH = OldURL[i];
                x.SEQ_NUM = SEQ_NUM;
                x.UPLOADDATE = UPLOADDATE;
                x.BACKUPDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                x.EMP_ID = EMP_ID;
                x.REASON = REASON;
                editLogs.Add(x);
            }
            if (editLogs.Count > 0)
                stu = INSERT_EditLog(editLogs);
            else
                stu = CDictionary.ERR;
            return stu;
        }

        public string INSERT_EditLog(List<EditLog> Data)
        {
            List<SqlParameter> Paras = new();
            string sql = $" INSERT INTO cr_EditLog (SEQ_NUM,  BPATH, APATH, UPLOADDATE, EMP_ID, BACKUPDATE, REASON) VALUES ";
            for (int i = 0; i < Data.Count; i++)
            {
                sql += $" (@K_SEQ_NUM{i},  @K_BPATH{i}, @K_APATH{i}, @K_UPLOADDATE{i}, @K_EMP_ID{i}, @K_BACKUPDATE{i}, @K_REASON{i}),";

                Paras.Add(new SqlParameter($"K_SEQ_NUM{i}", Data[i].SEQ_NUM));
                Paras.Add(new SqlParameter($"K_BPATH{i}", Data[i].BPATH));
                Paras.Add(new SqlParameter($"K_APATH{i}", Data[i].APATH));
                Paras.Add(new SqlParameter($"K_EMP_ID{i}", Data[i].EMP_ID));
                Paras.Add(new SqlParameter($"K_UPLOADDATE{i}", Data[i].UPLOADDATE));
                Paras.Add(new SqlParameter($"K_BACKUPDATE{i}", Data[i].BACKUPDATE));
                Paras.Add(new SqlParameter($"K_REASON{i}", Data[i].REASON));
            }
            if (sql.EndsWith(","))
                sql = sql.TrimEnd(',');

            return sqlBase.SqlNonQuery(sql, Paras);
        }


    }
}
