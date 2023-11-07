using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.ConstrainedExecution;
using CellReport_Workflow.Models.User;
using System.DirectoryServices;
using CellReport_Workflow.ViewModel;
using CellReport_Workflow.Interface;

namespace CellReport_Workflow.Controllers
{
    public class LoginController : Controller
    {
        private readonly IEncryptService _IEncryptService;

        public LoginController(IEncryptService IEncryptService)
        {
            _IEncryptService = IEncryptService;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(User cuser)
        {
            string sta = string.Empty;
            if (string.IsNullOrEmpty(cuser.Account) || string.IsNullOrEmpty(cuser.Password))
                return Json("請輸入完整資料");
            cuser.BuildUserByAccount();
            if (!string.IsNullOrEmpty(cuser.Emp_Id)) //有權限就查AD
            {
                //if (cuser.CheckPassword())
                if (true)
                {
                    string json = "";
                    cuser.Password = "";
                    json = System.Text.Json.JsonSerializer.Serialize(cuser);
                    HttpContext.Session.Remove(CDictionary.SK_USE_FOR_USER_INFO_SESSION_CELLREPORT);
                    if (!HttpContext.Session.Keys.Contains(CDictionary.SK_USE_FOR_USER_INFO_SESSION_CELLREPORT))
                        HttpContext.Session.SetString(CDictionary.SK_USE_FOR_USER_INFO_SESSION_CELLREPORT, json);
                    sta = "OK";
                }
                else
                {
                    sta = "帳密錯誤";
                }
            }
            else
                sta = "查無權限";
            return Json(sta);
        }
        public IActionResult Logout()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_USE_FOR_USER_INFO_SESSION_CELLREPORT))
                HttpContext.Session.Remove(CDictionary.SK_USE_FOR_USER_INFO_SESSION_CELLREPORT);
            return RedirectToAction("Login", "Login");
        }

        public IActionResult QuickLogin(string p, string c)
        {
            string para = _IEncryptService.Decrypt(p);

            if (para.Contains('*'))
            {
                User user = new();
                string[] words = para.Split("*");
                string datetime = words[0].Replace("_", "/").Replace("-", " ");
                //string Now = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                DateTime checktime = DateTime.Parse(datetime);
                if (DateTime.Now.AddMinutes(-1) <= checktime)
                {
                    user.Emp_Id = words[1];
                    user.BuildUserByID();
                    if (!string.IsNullOrEmpty(user.Account))
                    {
                        HttpContext.Session.Remove(CDictionary.SK_USE_FOR_USER_INFO_SESSION_CELLREPORT);
                        string json = "";
                        json = System.Text.Json.JsonSerializer.Serialize(user);
                        //HttpContext.Session.Remove(CDictionary.SK_USE_FOR_USER_INFO_SESSION_CELLREPORT);
                        if (!HttpContext.Session.Keys.Contains(CDictionary.SK_USE_FOR_USER_INFO_SESSION_CELLREPORT))
                            HttpContext.Session.SetString(CDictionary.SK_USE_FOR_USER_INFO_SESSION_CELLREPORT, json);
                        if (!string.IsNullOrEmpty(c))
                        {
                            return RedirectToAction("DetailApply", "Modify", new { id = c });
                        }
                        else
                        {
                            return RedirectToAction("Index", "Report");
                        }

                    }
                }
            }
            return RedirectToAction("Login", "Login");
        }

































        // GET: LoginController
        public ActionResult Index()
        {
            return View();
        }

        // GET: LoginController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LoginController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LoginController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LoginController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LoginController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LoginController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LoginController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
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
    }
}
