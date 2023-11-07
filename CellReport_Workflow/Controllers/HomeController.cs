using CellReport_Workflow.Models;
using CellReport_Workflow.Models.User;
using CellReport_Workflow.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Dynamic;
using System.Runtime.ConstrainedExecution;
using System.Security.Policy;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace CellReport_Workflow.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IWebHostEnvironment _environment;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
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


        public IActionResult Index()
        {
            User? user = GetUserSession();
            if (user != null)
                return View();
            else
                return RedirectToAction("Login", "Login");
        }
        [HttpPost]
        public IActionResult Index(string Id, string ProductType, string Status,string Finish_s, string Finish_e)
        {





            string Path = _environment.WebRootPath + "/json/allDate.json";

            using StreamReader reader = new(Path);
            var json = reader.ReadToEnd();
            var jarray = JArray.Parse(json);
            //var obj = JsonConvert.DeserializeObject<ExpandoObject>(json);

            List<test> tests = new(JsonConvert.DeserializeObject<List<test>>(json));
            //{
            //    new(1, "2017/06/16", "ASB10604240001", "黃慧娟", "2017/05/09 ", "完成"),
            //    new(2, "2013/05/09", "ASC10203260001", "陳菀欣", "2013/04/26", "簽核"),
            //    new(3, "2013/05/02", "ASC10204020002", "林雅滇", "2013/05/21", "簽核"),
            //    new(4, "2013/06/03", "ASC10204060003", "黃筱雯", "2013/05/07", "簽核"),
            //    new(5, "2013/05/20", "SC10204110004", "鄭婷尹", "2013/05/07", "簽核")
            //};
            tests = JsonConvert.DeserializeObject<List<test>>(json);
            if (Status != null || Status != "")
            {
                tests = tests.Where(x => x.Status == Status).ToList();
            }
            return Json(tests);
        }
        public IActionResult Detail(string Id, string type = "this")
        {
            using StreamReader reader = new($"{_environment.WebRootPath}/json/allDate.json");
            var json = reader.ReadToEnd();
            List<test> tests = new(JsonConvert.DeserializeObject<List<test>>(json).OrderBy(x => x.Cus_CT_ID).ToList());
            int index = tests.FindIndex(x => x.Cus_CT_ID == Id);
            if (type == "this") { }
            else if (type == "last" && index - 1 > -1)
                Id = tests[index - 1].Cus_CT_ID;
            else if (type == "next" && index + 1 < tests.Count - 1)
                Id = tests[index + 1].Cus_CT_ID;
            ViewBag.Id = Id;
            return View();
        }
        [HttpPost]
        public IActionResult Details(string Id)
        {
            //string Path = _environment.WebRootPath + "/json/allDate.json";

            using StreamReader reader = new($"{_environment.WebRootPath}/json/allDate.json");
            var json = reader.ReadToEnd();
            List<test> tests = new(JsonConvert.DeserializeObject<List<test>>(json));
            test test = tests.FirstOrDefault(x => x.Cus_CT_ID == Id);
            if (System.IO.File.Exists($"{_environment.WebRootPath}/file/{Id}_HLACheck_C.pdf"))
            {
                test.HLA = "Y";
            }
            return Json(test);
        }
        [HttpPost]
        public IActionResult DetailsSingeRecord(string Id)
        {
            using StreamReader reader = new($"{_environment.WebRootPath}/json/SignRecord.json");
            var json = reader.ReadToEnd();
            List<testRecord> tests = new(JsonConvert.DeserializeObject<List<testRecord>>(json).Where(z => z.cus_ct_id == Id).OrderBy(x => x.Id).ToList());
            return Json(tests);
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
    }
}