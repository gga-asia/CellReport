using CellReport_Workflow.Interface;
using CellReport_Workflow.Models.Sql;
using Microsoft.Data.SqlClient;

namespace CellReport_Workflow.Service
{
    public class MailService:IMailService
    {
        private readonly ISql  _sql;
        public MailService(ISql sql)
        {
            _sql = sql;
        }
        public string SendMail(string Subject, string To, string Body)
        {
            string sql = "INSERT INTO mail_log (Subject,[From],[to], Cc, Bcc, Body, IsBodyHtml, SendUtcTime, SendStatus) ";
            sql += $" Values (@K_Subject,'Service@BionetCorp.com', @K_To,'','',@K_Body ,'1',GETUTCDATE(),'0') ";
            List<SqlParameter> Paras = new()
            {
                new("K_Subject", Subject),
                new("K_Body", Body),
                new("K_To", To)
            };

            return _sql.MAIL_SqlNonQuery(sql, Paras);
        }

        public string SendToLabMailBodyFormat(string FromWho, string stage, string DataID, string Content)
        {
            string Body = $"簽核者 : {FromWho}<br/>";
            Body += $"簽核階段: {stage}<br/>"; 
            Body += $"客戶代號: {DataID}<br/>";
            Body += $"需修改內容: {Content}<br/>"; 
            return Body;            
        }
    }
}
