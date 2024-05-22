using Microsoft.Data.SqlClient;

namespace CellReport_Workflow.Models.Sql
{
    public interface ISql
    {
        public SqlConnection CONN_Builder();
        public SqlConnection MAIL_CONN_Builder();
        public string SqlNonQuery(string sql, List<SqlParameter> Paras);
        public string MAIL_SqlNonQuery(string sql, List<SqlParameter> Paras);
        public string SqlInsert_ReturnID(string sql, List<SqlParameter> Paras);
        public string StringReader(string sql, string ColumnName, List<SqlParameter> Paras);
        public List<string> List_StringReader(string sql, string ColumnName, List<SqlParameter> Paras);
        public List<T> DynamicDataBuidler<T>(string sql, List<SqlParameter> Paras);
        public string GetCV_PT_ForPBSC(string PBSC_FULL_ID);
    }
}
