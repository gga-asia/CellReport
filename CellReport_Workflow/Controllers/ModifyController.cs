using CellReport_Workflow.Interface;
using CellReport_Workflow.Models.Modify;
using CellReport_Workflow.Models.Report;
using CellReport_Workflow.Models.User;
using CellReport_Workflow.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CellReport_Workflow.Controllers
{
    public class ModifyController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFileService _IFileService;
        private IWebHostEnvironment _environment;

        public ModifyController(ILogger<HomeController> logger, IWebHostEnvironment environment, IFileService fileService)
        {
            _logger = logger;
            _environment = environment;
            _IFileService = fileService;
        }

        Report_ADO report_ADO = new();
        ModifyFactory ModifyFactory = new();
        ReportFactory ReportFactory = new();
        Modify_ADO modify_ADO = new();



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



        public IActionResult Index()//列表
        {
            User? user = GetUserSession();
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_USE_FOR_REPORT_LIST))
                HttpContext.Session.Remove(CDictionary.SK_USE_FOR_REPORT_LIST);
            if (user != null)
            {
                ViewBag.U = user;
                return View();
            }
            else return RedirectToAction("Login", "Login");
        }
        [HttpPost]
        public IActionResult Index(string Id, string ProductType,/* string Status, */string Finish_s, string Finish_e)
        {
            User? user = GetUserSession();
            List<Modify> datas;
            if (user.Department == CDictionary.C_LAB)
            {
                datas = ModifyFactory.GetModifyListByLab(Id, ProductType, Finish_s, Finish_e, CDictionary.C_LAB);
            }
            else if (user.Department == CDictionary.B_LAB)
            {
                datas = ModifyFactory.GetModifyListByLab(Id, ProductType, Finish_s, Finish_e, CDictionary.B_LAB);
            }
            else
            {
                datas = ModifyFactory.GetModifyListByLab(Id, ProductType, Finish_s, Finish_e, "");
            }
            
            return Json(datas);
        }



        public IActionResult SearchNewReport(string Id)
        {
            User? user = GetUserSession();
            List<Report> datas;// = ReportFactory.GetReportsList(Id, "", CDictionary.STAGE_RELEASE, "", "");
            if (user.Department == CDictionary.B_LAB)
            {
                datas = ReportFactory.GetReportsList(Id, "CB", CDictionary.STAGE_RELEASE, "", "");
                datas.AddRange(ReportFactory.GetReportsList(Id, "PBSC", CDictionary.STAGE_RELEASE, "", ""));
            }
            else if (user.Department == CDictionary.C_LAB)
            {
                datas = ReportFactory.GetReportsList(Id, "MSC", CDictionary.STAGE_RELEASE, "", "");
                datas.AddRange(ReportFactory.GetReportsList(Id, "ADSC", CDictionary.STAGE_RELEASE, "", ""));
                datas.AddRange(ReportFactory.GetReportsList(Id, "TSC", CDictionary.STAGE_RELEASE, "", ""));
            }
            else
            {
                datas = ReportFactory.GetReportsList(Id, "", CDictionary.STAGE_RELEASE, "", "");
            }
            return Json(datas);
        }


        [HttpPost]
        public IActionResult DetailsModifyRecord(string Id)
        {
            List<VMModify> datas = ModifyFactory.GetModifyRecordList(Id);
            return Json(datas);
        }
        [HttpPost]
        public IActionResult DetailModify(string Id)
        {
            List<Modify> datas = ModifyFactory.GetModifyList("", "", "", "", Id);
            if (datas.Count == 1)
                return Json(datas[0]);
            else
                return Json(datas);
        }


        public IActionResult Detail(string Id, string cusctid)//細項
        {
            User? user = GetUserSession();
            if (user != null)
            {
                ViewBag.U = user;
                ViewBag.Id = cusctid;
                ViewBag.ModifyId = Id;
            }
            else
                return RedirectToAction("Login", "Login");


            return View();
        }
        public IActionResult Details()
        {
            return Json("");
        }



        public IActionResult DetailApply(string Id)//申請細項
        {
            User? user = GetUserSession();
            if (user != null)
                ViewBag.U = user;
            else
                return RedirectToAction("Login", "Login");
            ViewBag.Id = Id;
            return View();
        }
        [HttpPost]
        public IActionResult Apply(string Id, string Reson)//新申請
        {
            User? user = GetUserSession();
            string sta = "";
            if (!string.IsNullOrEmpty(user.Emp_Id))
            {
                sta = modify_ADO.INSERT_ModifyApply(Id, Reson, user);
                return Json(sta);
            }
            else return Json("請重新登入");

        }
        [HttpPost]
        public IActionResult ApplyCheck(string Id)//新申請檢查
        {
            string sta = ModifyFactory.CheckApply(Id);
            if (!string.IsNullOrEmpty(sta) && sta != CDictionary.ERR) 
                sta = $"{Id} 已由 {sta} 提出申請";
            return Json(sta);
        }

        [HttpPost]
        public IActionResult Reply(string Id, string reply, string reply_remark)//主管回復申請
        {
            User? user = GetUserSession();
            string sta = "";
            if (!string.IsNullOrEmpty(user.Emp_Id) && user.Rank == CDictionary.LAB_MANAGER)
            {
                sta = modify_ADO.Updata_ModifyReply(Id, reply, reply_remark, user);


                return Json(sta);
            }
            else return Json("請重新登入");
        }
    }
}
