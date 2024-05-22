namespace CellReport_Workflow.Interface
{
    public interface IMailService
    {
        public string SendMail(string Subject, string To, string Body);
        public string SendToLabMailBodyFormat(string FromWho, string stage, string DataID, string Content);
    }
}
