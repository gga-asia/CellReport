namespace CellReport_Workflow.ViewModel
{
    public class CDictionary
    {
        public static readonly string SK_USE_FOR_USER_INFO_SESSION_CELLREPORT = "SK_USE_FOR_USER_INFO_SESSION_CELLREPORT";
        public static readonly string SK_USE_FOR_GET_PARAMETER_CELLREPORT = "SK_USE_FOR_GET_PARAMETER_CELLREPORT";
        public static readonly string SK_USE_FOR_REPORT_LIST = "SK_USE_FOR_REPORT_LIST";
        public static readonly string IIS_FILE_PATH = @"http://localhost/in";


        public static readonly string ERR = "ERR";
        public static readonly string NULL = "NULL";
        public static readonly string OK = "OK";


        public static readonly string C_LAB = "細胞實驗室";
        public static readonly string B_LAB = "造血實驗室";
        public static readonly string Information_Section= "資料組";
        public static readonly string IT = "資訊";

        public static readonly string MANAGER = "主管";//各實驗室的主管
        public static readonly string LAB_MANAGER = "實驗室主管"; //Lab主管複核
        public static readonly string LAB_QCMANAGER = "品質主管";//品質主管複核
        public static readonly string Information_MANAGER = "資料組主管";//資料組上傳報告
        public static readonly string LAB_ASSISTANTMANAGER = "單位主管";// 暫無用


        public static readonly string STAGE_MODIFY = "報告修正";
        public static readonly string STAGE_STOP = "未啟動";
        public static readonly string STAGE_LAB_SEND = "Lab送出資料";
        public static readonly string STAGE_QC_SING = "品質主管複核";
        public static readonly string STAGE_LAB_SING = "Lab主管複核";
        public static readonly string STAGE_FINISH = "簽核完成";
        public static readonly string STAGE_LABDONE = "報告釋出";
        public static readonly string STAGE_RELEASE = "報告上傳";
        public static readonly string STAGE_NOTIFY = "通知修改";



    }
}
