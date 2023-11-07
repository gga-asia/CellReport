namespace CellReport_Workflow.Interface
{
    public interface IEncryptService
    {
        public string Encrypt(string EncryptStr);
        public string Decrypt(string DecryptStr);
    }
}
