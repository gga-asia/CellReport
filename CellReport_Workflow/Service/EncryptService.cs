using CellReport_Workflow.Interface;
using System.Security.Cryptography;
using System.Text;

namespace CellReport_Workflow.Service
{
    public class EncryptService: IEncryptService
    {
         private string key { get; } = "CellReport";

        public string Encrypt(string EncryptStr)
        {
            string outputStr= "";
            string strings = EncryptStr.Replace(" ", "");

            byte[] plainTextByte = Encoding.UTF8.GetBytes(strings);
            byte[] keyByte = Encoding.UTF8.GetBytes(key);
            MD5CryptoServiceProvider provider_MD5 = new MD5CryptoServiceProvider();
            byte[] md5Byte = provider_MD5.ComputeHash(keyByte);
            RijndaelManaged aesProvider = new RijndaelManaged();
            ICryptoTransform aesEncrypt = aesProvider.CreateEncryptor(md5Byte, md5Byte);
            byte[] output = aesEncrypt.TransformFinalBlock(plainTextByte, 0, plainTextByte.Length);
            outputStr = BitConverter.ToString(output).Replace("-", "");
            return outputStr;
        }
        public string Decrypt(string DecryptStr)
        {
            string strings = DecryptStr;
            byte[] chipherTextByte = new byte[strings.Length / 2];
            int j = 0;

            for (int i = 0; i < strings.Length / 2; i++)
            {
                chipherTextByte[i] = Byte.Parse(strings[j].ToString() + strings[j + 1].ToString(), System.Globalization.NumberStyles.HexNumber);
                j += 2;
            }

            //密碼轉譯一定都是用byte[] 所以把string都換成byte[]
            byte[] keyByte = Encoding.UTF8.GetBytes(key);

            //加解密函數的key通常都會有固定的長度 而使用者輸入的key長度不定 因此用hash過後的值當做key
            MD5CryptoServiceProvider provider_MD5 = new MD5CryptoServiceProvider();
            byte[] md5Byte = provider_MD5.ComputeHash(keyByte);

            //產生解密實體
            RijndaelManaged aesProvider = new RijndaelManaged();
            ICryptoTransform aesDecrypt = aesProvider.CreateDecryptor(md5Byte, md5Byte);

            //string_secretContent就是解密後的明文
            byte[] plainTextByte = aesDecrypt.TransformFinalBlock(chipherTextByte, 0, chipherTextByte.Length);
            string plainText = Encoding.UTF8.GetString(plainTextByte);
            return plainText;
        }
    }
}
