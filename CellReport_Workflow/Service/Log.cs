using CellReport_Workflow.Interface;

namespace CellReport_Workflow.Service
{
    public class Log : ILogService
    {
        private readonly IConfiguration _configuration;
        public Log(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void TXTLog(String logMsg)
        {
            //檔案名稱 使用現在日期
            String logFileName = DateTime.Now.Year.ToString() + int.Parse(DateTime.Now.Month.ToString()).ToString("00") + int.Parse(DateTime.Now.Day.ToString()).ToString("00") + ".txt";

            //Log檔內的時間 使用現在時間
            String nowTime = int.Parse(DateTime.Now.Hour.ToString()).ToString("00") + ":" + int.Parse(DateTime.Now.Minute.ToString()).ToString("00") + ":" + int.Parse(DateTime.Now.Second.ToString()).ToString("00");

            if (!Directory.Exists(_configuration["TXTLogPath"]))
            {
                //建立資料夾
                Directory.CreateDirectory(_configuration["TXTLogPath"]);
            }

            if (!File.Exists(_configuration["TXTLogPath"] + "\\" + logFileName))
            {
                //建立檔案
                File.Create(_configuration["TXTLogPath"] + "\\" + logFileName).Close();
            }

            using (StreamWriter sw = File.AppendText(_configuration["TXTLogPath"] + "\\" + logFileName))
            {
                sw.WriteLine("-----------------------------------------------------------------------------------------");
                sw.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " : " + logMsg);
            }
        }
    }
}
