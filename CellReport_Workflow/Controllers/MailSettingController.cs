using CellReport_Workflow.Interface;
using CellReport_Workflow.Models.MailAddress;
using CellReport_Workflow.Models.User;
using CellReport_Workflow.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace CellReport_Workflow.Controllers
{
    public class MailSettingController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogService _logService;
        public MailSettingController(IConfiguration configuration, ILogService logService)
        {
            _logService = logService;
            _configuration = configuration;

        }
        public IActionResult Setting()
        {
            User? user = GetUserSession();
            if (user != null)
                ViewBag.U = user;
            else
                return RedirectToAction("Login", "Login");
            return View();
        }

        public IActionResult getMemberList(string Group)
        {
            string Group_id = "";
            if (Group == CDictionary.C_LAB)
            {
                Group_id = _configuration["GroupID_CELL"];
            }
            else if (Group == CDictionary.B_LAB)
            {
                Group_id = _configuration["GroupID_BLOOD"];
            }
            else if (Group == CDictionary.Information_Section)
            {
                Group_id = _configuration["GroupID_Information"];
            }
            else if (Group == CDictionary.LAB_MANAGER)
            {
                Group_id = _configuration["GroupID_LAB_MANAGER"];
            }
            else if (Group == CDictionary.LAB_QCMANAGER)
            {
                Group_id = _configuration["GroupID_LAB_QCMANAGER"];
            }
            if (string.IsNullOrEmpty(Group_id))
            {
                _logService.TXTLog($"Group_id={Group}");
                return BadRequest("群組資料異常");
            }

            List<MailAddress> maildatas = new MailAddressFactory().GETGroupAddress(Group_id);
            return Json(maildatas);
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
                user.Department_Rank = data.Department_Rank;
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

