using CellReport_Workflow.Models.User;
using CellReport_Workflow.Models;
using CellReport_Workflow.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Diagnostics;
using CellReport_Workflow.Models.Report;
using CellReport_Workflow.Interface;
using CellReport_Workflow.Models.Record;
using System.Data;

namespace CellReport_Workflow.Controllers
{
    public class ReportController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFileService _IFileService;
        private IWebHostEnvironment _environment;

        public ReportController(ILogger<HomeController> logger, IWebHostEnvironment environment, IFileService fileService)
        {
            _logger = logger;
            _environment = environment;
            _IFileService = fileService;
        }
        public User? GetUserSession()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_USE_FOR_USER_INFO_SESSION_CELLREPORT))
            {
                User user = new();
                string json = HttpContext.Session.GetString(CDictionary.SK_USE_FOR_USER_INFO_SESSION_CELLREPORT);
                var data = System.Text.Json.JsonSerializer.Deserialize<User>(json);
                user.Account = data.Account;
                user.Emp_Id = data.Emp_Id;
                user.Department = data.Department;
                user.Rank = data.Rank;
                user.Name = data.Name;
                return user;
            }
            else
            {
                return null;
            }
        }
        Report_ADO report_ADO = new();

        public IActionResult Index()  //列表頁
        {
            User? user = GetUserSession();
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_USE_FOR_REPORT_LIST))
                HttpContext.Session.Remove(CDictionary.SK_USE_FOR_REPORT_LIST);
            if (user != null)
            {
                ViewBag.U = user;
                return View();
            }
            else
                return RedirectToAction("Login", "Login");
        }
        [HttpPost]
        public IActionResult Index(string Id, string ProductType, string Status, string Finish_s, string Finish_e)
        {
            List<Report> datas = new ReportFactory().GetReportsList(Id, ProductType, Status, Finish_s, Finish_e);

            if (datas != null && datas.Count > 0)
            {
                List<string> SK_List = (from a in datas select a.cus_ct_id).ToList();
                string json = System.Text.Json.JsonSerializer.Serialize(SK_List);
                if (HttpContext.Session.Keys.Contains(CDictionary.SK_USE_FOR_REPORT_LIST))
                {
                    HttpContext.Session.Remove(CDictionary.SK_USE_FOR_REPORT_LIST);
                    HttpContext.Session.SetString(CDictionary.SK_USE_FOR_REPORT_LIST, json);
                }
                else HttpContext.Session.SetString(CDictionary.SK_USE_FOR_REPORT_LIST, json);
            }
            return Json(datas);
            #region
            //string Path = _environment.WebRootPath + "/json/allDate.json";

            //using StreamReader reader = new(Path);
            //var json = reader.ReadToEnd();
            //var jarray = JArray.Parse(json);
            ////var obj = JsonConvert.DeserializeObject<ExpandoObject>(json);

            //List<test> tests = new(JsonConvert.DeserializeObject<List<test>>(json));
            ////{
            ////    new(1, "2017/06/16", "ASB10604240001", "黃慧娟", "2017/05/09 ", "完成"),
            ////    new(2, "2013/05/09", "ASC10203260001", "陳菀欣", "2013/04/26", "簽核"),
            ////    new(3, "2013/05/02", "ASC10204020002", "林雅滇", "2013/05/21", "簽核"),
            ////    new(4, "2013/06/03", "ASC10204060003", "黃筱雯", "2013/05/07", "簽核"),
            ////    new(5, "2013/05/20", "SC10204110004", "鄭婷尹", "2013/05/07", "簽核")
            ////};
            //tests = JsonConvert.DeserializeObject<List<test>>(json);
            //if (!string.IsNullOrEmpty(Status))
            //{
            //    tests = tests.Where(x => x.Status == Status).ToList();
            //}
            #endregion
        }

        public IActionResult WaitIndex()  //待簽頁
        {
            User? user = GetUserSession();
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_USE_FOR_REPORT_LIST))
                HttpContext.Session.Remove(CDictionary.SK_USE_FOR_REPORT_LIST);
            if (user != null)
            {
                ViewBag.U = user;
                return View();
            }
            else
                return RedirectToAction("Login", "Login");
        }
        [HttpPost]
        public IActionResult WaitIndex(string Id, string ProductType, string Status, string Finish_s, string Finish_e)
        {
            User? user = GetUserSession();
            if (user != null)
            {
                List<Report> datas = new ReportFactory().GetReportsList(Id, ProductType, Status, Finish_s, Finish_e, user);

                if (datas != null && datas.Count > 0)
                {
                    List<string> SK_List = (from a in datas select a.cus_ct_id).ToList();
                    string json = System.Text.Json.JsonSerializer.Serialize(SK_List);
                    if (HttpContext.Session.Keys.Contains(CDictionary.SK_USE_FOR_REPORT_LIST))
                    {
                        HttpContext.Session.Remove(CDictionary.SK_USE_FOR_REPORT_LIST);
                        HttpContext.Session.SetString(CDictionary.SK_USE_FOR_REPORT_LIST, json);
                    }
                    else HttpContext.Session.SetString(CDictionary.SK_USE_FOR_REPORT_LIST, json);
                }
                return Json(datas);
            }
            else 
                return Json("請重新登入"); 
        }
        public IActionResult Detail(string Id, string type = "this")  //簽核頁
        {
            List<string> dataList = new();
            User? user = GetUserSession();
            if (user != null)
                ViewBag.U = user;
            else
                return RedirectToAction("Login", "Login");


            if (HttpContext.Session.Keys.Contains(CDictionary.SK_USE_FOR_REPORT_LIST))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_USE_FOR_REPORT_LIST);
                dataList = System.Text.Json.JsonSerializer.Deserialize<List<string>>(json);
            }
            int index = dataList.FindIndex(x => x == Id);
            if (type == "this") { }
            else if (type == "last" && index - 1 > -1)
                Id = dataList[index - 1];
            else if (type == "next" && index + 1 <= dataList.Count - 1)
                Id = dataList[index + 1];
            ViewBag.Id = Id;
            return View();
            #region
            //using StreamReader reader = new($"{_environment.WebRootPath}/json/allDate.json");
            //var json = reader.ReadToEnd();
            //List<test> tests = new(JsonConvert.DeserializeObject<List<test>>(json).OrderBy(x => x.Cus_CT_ID).ToList());
            //int index = tests.FindIndex(x => x.Cus_CT_ID == Id);
            //if (type == "this") { }
            //else if (type == "last" && index - 1 > -1)
            //    Id = tests[index - 1].Cus_CT_ID;
            //else if (type == "next" && index + 1 < tests.Count - 1)
            //    Id = tests[index + 1].Cus_CT_ID;
            //ViewBag.Id = Id;
            #endregion
        }
        [HttpPost]
        public IActionResult Details(string Id)   //報告資訊
        {
            List<Report> datas = new ReportFactory().GetReportsList(Id, "", "", "", "");
            if (datas.Count == 1)
                return Json(datas[0]);
            else return Json(null);
            #region
            //string Path = _environment.WebRootPath + "/json/allDate.json";
            //using StreamReader reader = new($"{_environment.WebRootPath}/json/allDate.json");
            //var json = reader.ReadToEnd();
            //List<test> tests = new(JsonConvert.DeserializeObject<List<test>>(json));
            //test test = tests.FirstOrDefault(x => x.Cus_CT_ID == Id);
            //if (System.IO.File.Exists($"{_environment.WebRootPath}/file/{Id}_HLACheck_C.pdf"))
            //{
            //    test.HLA = "Y";
            //}
            #endregion
        }
        [HttpPost]
        public IActionResult DetailsSingeRecord(string Id)   //簽核紀錄
        {
            List<Record> datas = new RecordFactory().GetRecordList(Id);
            datas = datas.Where(z => z.cus_ct_id == Id).OrderBy(x => x.Id).ToList();
            return Json(datas);
            #region
            //using StreamReader reader = new($"{_environment.WebRootPath}/json/SignRecord.json");
            //var json = reader.ReadToEnd();
            //List<testRecord> tests = new(JsonConvert.DeserializeObject<List<testRecord>>(json).Where(z => z.cus_ct_id == Id).OrderBy(x => x.Id).ToList());
            //return Json(tests);
            #endregion
        }

        public IActionResult Modify()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Modify(string Id)
        {
            using StreamReader reader = new($"{_environment.WebRootPath}/json/ModifyDate.json");
            var json = reader.ReadToEnd();
            List<testModify> tests = new(JsonConvert.DeserializeObject<List<testModify>>(json));
            //testModify test = tests.FirstOrDefault(x => x.Cus_CT_ID == Id);
            //if (System.IO.File.Exists($"{_environment.WebRootPath}/file/{Id}_HLACheck_C.pdf"))
            //{
            //    test.HLA = "Y";
            //}
            return Json(tests);
        }
        public IActionResult ModifyDetail(string Id)
        {
            using StreamReader reader = new($"{_environment.WebRootPath}/json/allDate.json");
            var json = reader.ReadToEnd();
            List<test> tests = new(JsonConvert.DeserializeObject<List<test>>(json).OrderBy(x => x.Cus_CT_ID).ToList());
            int index = tests.FindIndex(x => x.Cus_CT_ID == Id);

            ViewBag.Id = Id;
            return View();
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpPost]
        public IActionResult UploadReport(List<string> datas)//批次上傳報告，修改狀態
        {
            User? user = GetUserSession();
            if (user.Rank == CDictionary.LAB_MANAGER)
            {

                string stu = new Report_ADO().uploadRelease(datas, user);
                if (stu == "OK")
                    return Json($"{datas.Count}筆資料上傳成功");
                else
                    return BadRequest(stu);
            }
            else return BadRequest("查無權限");
        }
        [HttpPost]
        public IActionResult GetAllPDF(List<string> datas)//批次下載PDF
        {
            return Ok();
        }



        [HttpPost]
        public IActionResult SingQC(string Id, string type, string stage)//品質主管簽核
        {
            User? user = GetUserSession();
            string sta = "";
            if (stage == CDictionary.STAGE_LAB_SEND)
            {
                if ((type == "ADSC" || type == "MSC" || type == "TSC") && user.Department == CDictionary.B_LAB && user.Rank == CDictionary.LAB_MANAGER)
                    sta = new Report_ADO().UpdateQC_S(Id, user);
                else if ((type == "CB" || type == "PBSC") && user.Department == CDictionary.C_LAB && user.Rank == CDictionary.LAB_MANAGER)
                    sta = new Report_ADO().UpdateQC_S(Id, user);
            }
            else return Json("報告狀態異常");
            return Json(sta);
        }
        [HttpPost]
        public IActionResult SingLAB(string Id, string type, string stage)//LAB主管簽核
        {
            User? user = GetUserSession();
            string sta = "";
            if (stage == CDictionary.STAGE_QC_SING)
            {
                if ((type == "ADSC" || type == "MSC" || type == "TSC") && user.Department == CDictionary.C_LAB && user.Rank == CDictionary.LAB_MANAGER)
                    sta = new Report_ADO().UpdateLab_S(Id, user);
                else if ((type == "CB" || type == "PBSC") && user.Department == CDictionary.B_LAB && user.Rank == CDictionary.LAB_MANAGER)
                    sta = new Report_ADO().UpdateLab_S(Id, user);
            }
            else return Json("報告狀態異常");
            return Json(sta);
        }
        [HttpPost]
        public IActionResult Release(string Id, string type, string stage)//DetailRelease
        {
            User? user = GetUserSession();
            string sta = "";
            if (stage == CDictionary.STAGE_FINISH)
            {
                if ((type == "ADSC" || type == "MSC" || type == "TSC") && user.Department == CDictionary.C_LAB && user.Rank == CDictionary.LAB_MANAGER)
                    sta = new Report_ADO().uploadRelease(new() { Id }, user);
                else if ((type == "CB" || type == "PBSC") && user.Department == CDictionary.B_LAB && user.Rank == CDictionary.LAB_MANAGER)
                    sta = new Report_ADO().uploadRelease(new() { Id }, user);
            }
            else return Json("報告狀態異常");
            return Json(sta);
        }
    }
}
