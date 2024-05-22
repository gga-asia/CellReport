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
using CellReport_Workflow.Models.Sql;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Hosting.Server;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using CellReport_Workflow.Models.MailAddress;
using System.IO.Compression;

namespace CellReport_Workflow.Controllers
{
    public class ReportController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFileService _IFileService;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly IStampService _stampService;
        private readonly ILogService _logService;
        private readonly IMailService _mailService;
        private readonly IMergeInstructionPageService _MergeInstructionPageService;

        public ReportController(ILogger<HomeController> logger, IWebHostEnvironment environment, IFileService fileService, IConfiguration configuration, IStampService stampService, ILogService logService, IMailService mailService, IMergeInstructionPageService mergeInstruction)
        {
            _logger = logger;
            _environment = environment;
            _IFileService = fileService;
            _configuration = configuration;
            _stampService = stampService;
            _logService = logService;
            _mailService = mailService;
            _MergeInstructionPageService = mergeInstruction;
        }
        Report_ADO report_ADO = new();
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
        }
        [HttpPost]
        public IActionResult Details(string Id)   //報告資訊
        {
            List<Report> datas = new ReportFactory().GetReportsList(Id, "", "ALL", "", "");

            if (datas.Count == 1)
                return Json(datas[0]);
            else return Json(null);
        }
        [HttpPost]
        public IActionResult DetailsSingeRecord(string Id)   //簽核紀錄
        {
            List<Record> datas = new RecordFactory().GetRecordList(Id);
            datas = datas.Where(z => z.cus_ct_id == Id).OrderBy(x => x.Id).ToList();
            return Json(datas);
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
            string all = "";
            if (user.Rank == CDictionary.LAB_MANAGER)
            {
                Report_ADO report_ADO = new();
                for (int i = 0; i < datas.Count; i++)
                {
                    if (CDictionary.OK == _MergeInstructionPageService.MergeInstruction(datas[i]))
                        if (CDictionary.OK == _IFileService.ReleaseReport(datas[i]))
                            if (CDictionary.OK == report_ADO.uploadRelease(new() { datas[i] }, user))
                            {
                                _IFileService.REFRESH_FileList(datas[i]);
                                continue;
                            }
                            else
                            {
                                _logService.TXTLog("UploadReport-- " + datas[i] + " 上傳狀態修失敗");
                                all += $"{datas[i]} ";
                            }
                        else
                        {
                            _logService.TXTLog("UploadReport-- " + datas[i] + " 上傳檔案搬移失敗");
                            all += $"{datas[i]} ";
                        }
                    //else
                    //{
                    //    _logService.TXTLog("UploadReport-- " + datas[i] + " 說明頁合併失敗");
                    //    all += $"{datas[i]} ";
                    //}
                }
                if (string.IsNullOrEmpty(all))
                    return Ok($"{datas.Count}筆資料上傳成功");
                else return BadRequest(all + " 上傳失敗");
            }
            else return BadRequest("查無權限");
        }

        public IActionResult Download(string TempPath)//前端從TEMP下載合併的報告
        {
            if (string.IsNullOrEmpty(TempPath))
                return BadRequest();
            byte[] fileByteArray = System.IO.File.ReadAllBytes(TempPath);
            System.IO.File.Delete(TempPath);
            return File(fileByteArray, "application/pdf", $"MergeReport{DateTime.Now.ToString("yyyy_MM_dd_HHmmss")}.pdf");
        }



        [HttpPost]
        public IActionResult SingQC(string Id, string type, string stage, string ReplyRemark)//品質主管簽核
        {
            User? user = GetUserSession();
            string sta = "";
            if (stage == CDictionary.STAGE_LAB_SEND)
            {
                if (user.Rank == CDictionary.LAB_QCMANAGER)
                    sta = new Report_ADO().UpdateQC_S(Id, user, ReplyRemark);

            }
            else return Json("報告狀態異常");
            return Json(sta);
        }
        [HttpPost]
        public IActionResult SingQC_All(List<string> datas)//批次-品質主管簽核
        {
            User? user = GetUserSession();
            string sta = "";
            if (user.Rank == CDictionary.LAB_QCMANAGER)
            {
                if (datas != null)
                {
                    if (datas.Count > 0)
                    {
                        Report_ADO report_ADO = new();
                        for (int i = 0; i < datas.Count; i++)
                        {
                            string ThisStu = report_ADO.UpdateQC_S(datas[i], user, "");
                            if (ThisStu != CDictionary.OK)
                            {
                                sta += datas[i] + " ";
                                _logService.TXTLog($"SingQC_All 錯誤(QC簽核失敗) -- {ThisStu}");
                            }
                        }
                        if (string.IsNullOrEmpty(sta))
                            return Ok($"成功簽核 {datas.Count} 筆");
                        else
                            return BadRequest(sta + " 簽核失敗");
                    }
                    else
                        return BadRequest("資料異常(0)");
                }
                else
                    return BadRequest("資料異常(Null)");
            }
            else
                return BadRequest("簽核身分異常");
            //return Ok($"成功簽核 {datas.Count} 筆");
        }
        [HttpPost]
        public IActionResult SingLAB(string Id, string type, string stage, string ReplyRemark)//LAB主管簽核
        {
            User? user = GetUserSession();
            string sta = "";
            if (stage == CDictionary.STAGE_QC_SING)
            {
                if (user.Rank == CDictionary.LAB_MANAGER)
                {
                    sta = new Report_ADO().UpdateLab_S(Id, user, ReplyRemark);
                    //_IFileService.UnSignBackUp(Id);
                    sta = _stampService.Stamp(Id);
                    //sta = _MergeInstructionPageService.MergeInstruction(Id);
                    _IFileService.REFRESH_FileList(Id);
                }
            }
            else return Json("報告狀態異常");
            return Json(sta);
        }
        [HttpPost]
        public IActionResult SingLAB_All(List<string> datas)//批次 - LAB主管簽核
        {
            User? user = GetUserSession();
            string sta = "";
            if (user.Rank == CDictionary.LAB_MANAGER)
            {
                if (datas != null)
                {
                    if (datas.Count > 0)
                    {
                        Report_ADO report_ADO = new();
                        for (int i = 0; i < datas.Count; i++)
                        {
                            string ThisStu = report_ADO.UpdateLab_S(datas[i], user, "");
                            if (ThisStu != CDictionary.OK)
                            {
                                sta += datas[i] + "-簽核失敗 ";
                                _logService.TXTLog($"SingLAB_All 錯誤(LAB簽核失敗) -- {ThisStu}");
                            }
                            else
                            {
                                //_IFileService.UnSignBackUp(datas[i]);
                                ThisStu = "";
                                ThisStu = _stampService.Stamp(datas[i]);
                                if (ThisStu != CDictionary.OK)
                                {
                                    sta += datas[i] + "-蓋章失敗 ";
                                    _logService.TXTLog($"SingLAB_All 錯誤(蓋章失敗) -- {ThisStu}");
                                }
                                else
                                {
                                    //ThisStu = "";
                                    //ThisStu = _MergeInstructionPageService.MergeInstruction(datas[i]);
                                    //if (ThisStu != CDictionary.OK)
                                    //{
                                    //    sta += datas[i] + "-說明頁合併失敗 ";
                                    //    _logService.TXTLog($"SingLAB_All 錯誤(說明頁合併失敗) -- {ThisStu}");
                                    //}
                                    _IFileService.REFRESH_FileList(datas[i]);
                                }
                            }
                        }
                        if (string.IsNullOrEmpty(sta))
                            return Ok($"成功簽核 {datas.Count} 筆");
                        else
                            return BadRequest(sta);
                    }
                    else
                        return BadRequest("資料異常(0)");
                }
                else
                    return BadRequest("資料異常(Null)");
            }
            else
                return BadRequest("簽核身分異常");
            //return Ok($"成功簽核 {datas.Count} 筆");
        }
        [HttpPost]
        public IActionResult Release(string Id, string type, string stage)//DetailRelease
        {
            User? user = GetUserSession();
            string sta = "";
            if (stage == CDictionary.STAGE_FINISH)
            {
                if (user.Rank == CDictionary.LAB_MANAGER)
                    sta = new Report_ADO().uploadRelease(new() { Id }, user);
                if (sta == CDictionary.OK)
                    sta = _MergeInstructionPageService.MergeInstruction(Id);
                if (sta == CDictionary.OK)
                    sta = _IFileService.ReleaseReport(Id);

                _IFileService.REFRESH_FileList(Id);
            }
            else return Json("報告狀態異常");
            return Json(sta);
        }
        [HttpPost]
        public IActionResult SendToLab(string Id, string ReplyRemark)//簽核時發送異常通知給實驗室
        {
            User? user = GetUserSession();
            Report report = new ReportFactory().Get_cr_main(Id);
            string sta = "";
            if (user.Rank == CDictionary.LAB_MANAGER || user.Rank == CDictionary.LAB_QCMANAGER)
            {
                sta = new Report_ADO().UpdateSendLab(Id, user, ReplyRemark, report);
                if (sta == CDictionary.OK)
                {
                    string TO = "";
                    if (report.ProductType == "ADSC" || report.ProductType == "MSC" || report.ProductType == "TSC")
                    {
                        TO = new MailAddressFactory().GetEmailByGroup(_configuration["GroupID_CELL"]);
                    }
                    else if (report.ProductType == "CB" || report.ProductType == "PBSC")
                    {
                        TO = new MailAddressFactory().GetEmailByGroup(_configuration["GroupID_BLOOD"]);
                    }
                    _mailService.SendMail($"報告電子簽核系統 - {report.cus_ct_id} 報告須修改", TO, _mailService.SendToLabMailBodyFormat(user.Account, report.stage, report.cus_ct_id, ReplyRemark));
                }
            }

            return Ok(sta);
        }
        [HttpPost]
        public IActionResult SendNext(List<string> datas_ID)//發送下階段簽核通知
        {
            User? user = GetUserSession();
            string sta = "";
            if (user.Rank == CDictionary.LAB_QCMANAGER && datas_ID.Count > 0)
            {
                string TO = new MailAddressFactory().GetEmailByGroup(_configuration["GroupID_LAB_MANAGER"]);
                string Body = $"共 {datas_ID.Count} 筆報告由品質主管簽核 <br>";
                for (int i = 0; i < datas_ID.Count; i++)
                    Body += $"{datas_ID[i]}<br>";
                sta = _mailService.SendMail($"細胞報告-Lab主管簽核", TO, Body);
                if (sta == CDictionary.OK)
                    return Ok("發送成功");
                else
                    return BadRequest("發送失敗");
            }
            else
            {
                return BadRequest("資訊錯誤");
            }
        }
        [HttpPost]
        public IActionResult SendUpload(List<string> datas_ID)//發送報告上傳通知
        {
            User? user = GetUserSession();
            string sta = "";
            if (user.Rank == CDictionary.LAB_MANAGER && datas_ID.Count > 0)
            {
                string TO = new MailAddressFactory().GetEmailByGroup(_configuration["GroupID_Information"]);
                string Body = $"共 {datas_ID.Count} 筆報告由實驗室上傳 <br>";
                for (int i = 0; i < datas_ID.Count; i++)
                    Body += $"{datas_ID[i]}<br>";
                sta = _mailService.SendMail($"細胞報告-實驗室上傳報告", TO, Body);
                if (sta == CDictionary.OK)
                    return Ok("發送成功");
                else
                    return BadRequest("發送失敗");
            }
            else
            {
                return BadRequest("資訊錯誤");
            }
        }
        [HttpPost]
        public IActionResult GetAllPDF(List<string> datas)//批次下載PDF，合併PDF存TEMP
        {
            string TempPath = _IFileService.GetMergePDF(datas);
            if (!string.IsNullOrEmpty(TempPath))
            {
                if (!System.IO.File.Exists(TempPath))
                    return BadRequest("合併失敗");
                return Ok(TempPath);
            }
            else
            {
                return BadRequest("合併失敗-TempPathEmpty");
            }
        }
        [HttpPost]
        public IActionResult GetPdfUrl(string Id)
        {
            Report report = new ReportFactory().Get_cr_main(Id);
            if (report == null)
                return BadRequest("查無報告資料");
            //VMReportFile vMReportFile = _IFileService.GetReportsPath(report.cus_ct_id, report.Upload_Date);
            VMReportFile vMReportFile = new();
            vMReportFile.CurrentReports = _IFileService.ScanAllPDFFilesPath("InProgress", report.cus_ct_id);
            vMReportFile.UploadReports = _IFileService.ScanAllPDFFilesPath("Upload", report.cus_ct_id);
            vMReportFile.HistoryReports = _IFileService.ScanAllPDFFilesPath("Backup", report.cus_ct_id);
            return Json(vMReportFile);
        }

        public IActionResult GetFile(string url)
        {
            var stream = new FileStream($@"{url}", FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");
        }

    }
}
