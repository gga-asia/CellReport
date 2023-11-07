using Microsoft.Data.SqlClient;

namespace CellReport_Workflow.Models.Sql
{
    public interface ISql
    {
        public string SqlNonQuery(string sql, List<SqlParameter> Paras);
    }
}
