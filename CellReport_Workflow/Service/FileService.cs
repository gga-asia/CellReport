using CellReport_Workflow.Interface;
using CellReport_Workflow.Models.Sql;
using CellReport_Workflow.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
//using iText.Kernel.Pdf;
//using iText.Layout;
//using iText.Layout.Element;
//using iText.Kernel.Utils;
using iTextSharp.text.pdf;
using iTextSharp.text;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using System;
using CellReport_Workflow.Models.ReportFile;

namespace CellReport_Workflow.Service
{
    public class FileService : IFileService
    {
        private readonly IConfiguration _configuration;
        private readonly ISql _SQL;
        private readonly ILogService _logService;
        private readonly IWebHostEnvironment _environment;

        public FileService(IConfiguration configuration, ISql sql, ILogService logService, IWebHostEnvironment Environment)
        {
            _configuration = configuration;
            _SQL = sql;
            _logService = logService;
            _environment = Environment;
        }
        public VMReportFile GetReportsPath(string cus_ct_id, string Upload_Date)
        {
            VMReportFile vMReportFile = new();
            if (!string.IsNullOrEmpty(Upload_Date))
            {
                vMReportFile.UploadReports = Scan_UPLOAD_PDFFilesURL(cus_ct_id);
            }
            vMReportFile.CurrentReports = ScanAllPDFFilesUrl("InProgress", cus_ct_id);
            vMReportFile.HistoryReports = ScanAllPDFFilesUrl("Backup", cus_ct_id);

            return vMReportFile;
        }



        public List<string> SearchPDF(string FileType, string cus_ct_id)
        {
            //string FilePathURL = _configuration["FilePathURL"];
            //string SubFile = GetFileName(cus_ct_id);
            List<string> datas = new();

            //if (FileType == "Upload")
            //{
            //    return Scan_In_PDFFilesPath(cus_ct_id);
            //}
            //else if (FileType == "InProgress")
            if (FileType == "InProgress")
            {
                return ScanAllPDFFilesUrl("InProgress", cus_ct_id);
            }
            else if (FileType == "Backup")
            {
                return ScanAllPDFFilesUrl("Backup", cus_ct_id);
            }
            return datas;
        }

        public List<string> ScanAllPDFFilesUrl(string FileType, string cus_ct_id)
        {
            string rootFolder = _configuration["FilePath"];
            string filePath = rootFolder + FileType;
            List<string> type = new() { "\\ADSC_C\\", "\\ADSC_E\\", "\\MSC_C\\", "\\MSC_E\\", "\\PBSC\\", "\\TSC\\", "\\CB_C\\", "\\CB_E\\", "\\HLA\\", "\\HLA_C\\", "\\HLA_E\\", "\\Blood\\", "\\Blood_C\\", "\\Blood_E\\", "\\Store\\", "\\Store_C\\", "\\Store_E\\", "\\Indonesia\\" };
            List<string> result = new();
            try
            {
                rootFolder += FileType + "\\";
                //_logService.TXTLog(rootFolder);
                List<string> pdfFiles = Directory.GetFiles(filePath, "*.pdf", System.IO.SearchOption.AllDirectories)
                    .Where(file => Path.GetFileName(file).Contains(cus_ct_id)).ToList();
                //_logService.TXTLog(pdfFiles.Count.ToString());
                if (pdfFiles.Count > 0)
                {
                    for (int i = 0; i < type.Count; i++)
                    {
                        string? F = pdfFiles.Find(s => s.Contains(type[i]));
                        if (!string.IsNullOrEmpty(F))
                            result.Add(F.Replace(filePath, $"{_configuration["FilePathURL"]}{FileType}").Replace("\\", "/"));
                    }
                    return result;
                }
                else
                {
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logService.TXTLog($"錯誤 -- ScanAllPDFFilesUrl()  {ex.Message}");
                return result;
            }
        }
        public List<string> ScanAllPDFFilesPath(string FileType, string cus_ct_id)
        {
            string rootFolder = _configuration["FilePath"];
            string filePath = rootFolder + FileType;
            List<string> type = new() { "\\ADSC_C\\", "\\ADSC_E\\", "\\MSC_C\\", "\\MSC_E\\", "\\PBSC\\", "\\TSC\\", "\\CB_C\\", "\\CB_E\\", "\\HLA\\", "\\HLA_C\\", "\\HLA_E\\", "\\Blood\\", "\\Blood_C\\", "\\Blood_E\\", "\\Store\\", "\\Store_C\\", "\\Store_E\\", "\\Indonesia\\" };
            List<string> result = new();
            try
            {
                rootFolder += FileType + "\\";
                //_logService.TXTLog(rootFolder);
                result = Directory.GetFiles(filePath, "*.pdf", System.IO.SearchOption.AllDirectories)
                    .Where(file => Path.GetFileName(file).Contains(cus_ct_id)).ToList();
                //_logService.TXTLog(result.Count.ToString());
                return result;
            }
            catch (Exception ex)
            {
                _logService.TXTLog($"錯誤 -- ScanAllPDFFilesPath()  {ex.Message}");
                return result;
            }
        }
        public List<string> Scan_UPLOAD_PDFFilesURL(string cus_ct_id)
        {
            string rootFolder = _configuration["FilePath"] + "Upload\\";
            string filePath = rootFolder;// + GetFileName(cus_ct_id);
            List<string> type = new() { "\\ADSC_C\\", "\\ADSC_E\\", "\\MSC_C\\", "\\MSC_E\\", "\\PBSC\\", "\\TSC\\", "\\CB_C\\", "\\CB_E\\", "\\HLA\\", "\\HLA_C\\", "\\HLA_E\\", "\\Blood\\", "\\Blood_C\\", "\\Blood_E\\", "\\Store\\", "\\Store_C\\", "\\Store_E\\", "\\Indonesia\\" };
            List<string> result = new();
            try
            {
                //rootFolder += FileType + "\\";
                //_logService.TXTLog(rootFolder);
                List<string> pdfFiles = Directory.GetFiles(filePath, "*.pdf", System.IO.SearchOption.AllDirectories)
                    .Where(file => Path.GetFileName(file).Contains(cus_ct_id)).ToList();
                //_logService.TXTLog(result.Count.ToString());
                if (pdfFiles.Count > 0)
                {
                    for (int i = 0; i < type.Count; i++)
                    {
                        string? F = pdfFiles.Find(s => s.Contains(type[i]));
                        if (!string.IsNullOrEmpty(F))
                            result.Add(F.Replace(filePath, $"{_configuration["FilePathURL"]}Upload/").Replace("\\", "/"));
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                _logService.TXTLog($"錯誤 -- ScanAllPDFFilesPath()  {ex.Message}");
                return result;
            }
        }


        public string ReleaseReport(string cus_ct_id)
        {
            List<string> InProgressFileList = ScanAllPDFFilesPath("InProgress", cus_ct_id);
            try
            {
                for (int i = 0; i < InProgressFileList.Count; i++)
                {
                    string FilePath = InProgressFileList[i].Replace($"\\{cus_ct_id}.pdf", "").Replace("InProgress", "Upload");

                    if (!string.IsNullOrEmpty(FilePath))
                    {
                        if (!Directory.Exists(FilePath))
                            Directory.CreateDirectory(FilePath);
                        File.Move(InProgressFileList[i], $"{FilePath}\\{cus_ct_id}.pdf");
                        _logService.TXTLog($" File.Move({InProgressFileList[i]}, {FilePath}\\{cus_ct_id}.pdf)");
                        if (!File.Exists($"{FilePath}\\{cus_ct_id}.pdf"))
                        {
                            _logService.TXTLog($"Err --{FilePath}\\{cus_ct_id}.pdf -- 上傳失敗");
                            return CDictionary.ERR;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        _logService.TXTLog($"ReleaseReport : {cus_ct_id} 空路徑");
                        return CDictionary.ERR;
                    }
                }
                return CDictionary.OK;
            }
            catch (Exception ex)
            {
                _logService.TXTLog($"ReleaseReport 錯誤 : {ex.Message}");
                return CDictionary.ERR;
            }
        }
        #region "以上傳日期整理上傳"
        //public string ReleaseReport(string cus_ct_id)
        //{
        //    List<string> InProgressFileList = ScanAllPDFFilesPath("InProgress", cus_ct_id);
        //    try
        //    {
        //        for (int i = 0; i < InProgressFileList.Count; i++)
        //        {
        //            string FilePath = InProgressFileList[i].Replace($"\\{cus_ct_id}.pdf", "").Replace("InProgress", $"Upload\\{DateTime.Now.ToString("yyyyMMdd")}");
        //            int lastIndex = FilePath.LastIndexOf("\\");
        //            int secondLastIndex = FilePath.LastIndexOf("\\", lastIndex - 1);

        //            // 删除子字符串
        //            if (secondLastIndex != -1 && lastIndex != -1)
        //            {
        //                FilePath = FilePath.Substring(0, secondLastIndex) + FilePath.Substring(lastIndex);
        //            }
        //            if (!string.IsNullOrEmpty(FilePath))
        //            {
        //                if (!Directory.Exists(FilePath))
        //                    Directory.CreateDirectory(FilePath);
        //                File.Move(InProgressFileList[i], $"{FilePath}\\{cus_ct_id}.pdf");
        //                _logService.TXTLog($" File.Move({InProgressFileList[i]}, {FilePath}\\{cus_ct_id}.pdf)");
        //                if (!File.Exists($"{FilePath}\\{cus_ct_id}.pdf"))
        //                {
        //                    _logService.TXTLog($"Err --{FilePath}\\{cus_ct_id}.pdf -- 上傳失敗");
        //                    return CDictionary.ERR;
        //                }
        //                else
        //                {
        //                    continue;
        //                }
        //            }
        //            else
        //            {
        //                _logService.TXTLog($"ReleaseReport : {cus_ct_id} 空路徑");
        //                return CDictionary.ERR;
        //            }
        //        }
        //        return CDictionary.OK;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logService.TXTLog($"ReleaseReport 錯誤 : {ex.Message}");
        //        return CDictionary.ERR;
        //    }
        //}
        #endregion

        public string GetMergePDF(List<string> cus_ct_id_LIST)
        {
            string ReturnPath = "";
            List<string> AllPDF = new();
            for (int i = 0; i < cus_ct_id_LIST.Count; i++)
            {
                if (!string.IsNullOrEmpty(cus_ct_id_LIST[i]))
                {
                    AllPDF.AddRange(ScanAllPDFFilesPath("InProgress", cus_ct_id_LIST[i]));
                    AllPDF.AddRange(ScanAllPDFFilesPath("Upload", cus_ct_id_LIST[i]));
                }
                else
                    _logService.TXTLog($"Err GetMergePDF -- 空值");
            }
            if (AllPDF.Count > 0)
            {
                ReturnPath = _environment.WebRootPath + $@"\temp\Merge{DateTime.Now.ToString("yyyy_MM_dd_HHmmss")}.pdf";
                MergePdfFiles(AllPDF.ToArray(), ReturnPath);
            }
            else
                _logService.TXTLog($"Err GetMergePDF -- AllPDF.Count = 0");

            return ReturnPath;
        }

        private void MergePdfFiles(string[] pdfFiles, string mergedPdfPath)
        {
            try
            {
                using (Document doc = new())
                using (FileStream fileStream = new(mergedPdfPath, FileMode.Create))
                using (PdfCopy copy = new(doc, fileStream))
                {
                    doc.Open();
                    foreach (string filename in pdfFiles)
                    {
                        PdfReader reader = new(filename);
                        copy.AddDocument(reader);
                        reader.Close();
                    }
                    doc.Close();
                }
            }
            catch (Exception ex)
            {
                _logService.TXTLog($"錯誤 MergePdfFiles -- {ex.Message}");
            }
        }
        public string UnSignBackUp(string cus_ct_id)
        {
            List<string> InProgressFileList = ScanAllPDFFilesPath("InProgress", cus_ct_id);
            try
            {
                for (int i = 0; i < InProgressFileList.Count; i++)
                {
                    string FilePath = InProgressFileList[i].Replace($"\\{cus_ct_id}.pdf", "").Replace("InProgress", "UnSignBackUp");
                    if (!string.IsNullOrEmpty(FilePath))
                    {
                        if (!Directory.Exists(FilePath))
                            Directory.CreateDirectory(FilePath);
                        File.Copy(InProgressFileList[i], $"{FilePath}\\{cus_ct_id}.pdf", true);
                        _logService.TXTLog($" File.Copy({InProgressFileList[i]}, {FilePath}\\{cus_ct_id}.pdf)");
                        if (!File.Exists($"{FilePath}\\{cus_ct_id}.pdf"))
                        {
                            _logService.TXTLog($"Err --{FilePath}\\{cus_ct_id}.pdf -- 備份失敗");
                            return CDictionary.ERR;
                        }
                        else
                            continue;
                    }
                    else
                    {
                        _logService.TXTLog($"UnSignBackUp : {cus_ct_id} 空路徑");
                        return CDictionary.ERR;
                    }
                }
                return CDictionary.OK;
            }
            catch (Exception ex)
            {
                _logService.TXTLog($"UnSignBackUp 錯誤 : {ex.Message}");
                return CDictionary.ERR;
            }
        }
        public string UnSignBackUp_To_InProgress(string cus_ct_id)
        {
            List<string> InProgressFileList = ScanAllPDFFilesPath("UnSignBackUp", cus_ct_id);
            try
            {
                for (int i = 0; i < InProgressFileList.Count; i++)
                {
                    string FilePath = InProgressFileList[i].Replace($"\\{cus_ct_id}.pdf", "").Replace("UnSignBackUp", "InProgress");
                    if (!string.IsNullOrEmpty(FilePath))
                    {
                        if (!Directory.Exists(FilePath))
                            Directory.CreateDirectory(FilePath);
                        File.Copy(InProgressFileList[i], $"{FilePath}\\{cus_ct_id}.pdf", true);
                        _logService.TXTLog($" File.Copy({InProgressFileList[i]}, {FilePath}\\{cus_ct_id}.pdf)");
                        if (!File.Exists($"{FilePath}\\{cus_ct_id}.pdf"))
                        {
                            _logService.TXTLog($"Err --{FilePath}\\{cus_ct_id}.pdf -- 搬移失敗");
                            return CDictionary.ERR;
                        }
                        else
                            continue;
                    }
                    else
                    {
                        _logService.TXTLog($"UnSignBackUp_To_InProgress : {cus_ct_id} 空路徑");
                        return CDictionary.ERR;
                    }
                }
                return CDictionary.OK;
            }
            catch (Exception ex)
            {
                _logService.TXTLog($"UnSignBackUp_To_InProgress 錯誤 : {ex.Message}");
                return CDictionary.ERR;
            }
        }
        public List<List<string>> backupeUploadReport(string cus_ct_id, string Version, string Upload_Date)
        {
            List<List<string>> pathData = new();

            List<string> InProgressFileList = ScanAllPDFFilesPath("Upload", cus_ct_id);
            List<string> BackUp = new();

            try
            {
                for (int i = 0; i < InProgressFileList.Count; i++)
                {
                    string FilePath = InProgressFileList[i].Replace($"\\{cus_ct_id}.pdf", "").Replace("Upload", "Backup");
                    //FilePath = FilePath.Replace(Upload_Date.Replace("-",""), GetFileName(cus_ct_id));
                    if (!string.IsNullOrEmpty(FilePath))
                    {
                        if (!Directory.Exists(FilePath))
                            Directory.CreateDirectory(FilePath);
                        string movePath = $"{FilePath}\\{cus_ct_id}_{DateTime.Now.ToString("yyyyMMdd")}_V{Version}.pdf";
                        File.Move(InProgressFileList[i], movePath);
                        _logService.TXTLog($" File.Move({InProgressFileList[i]}, {movePath}");
                        if (!File.Exists(movePath))
                        {
                            _logService.TXTLog($"Err --{movePath} -- 備份失敗");
                            return new();
                        }
                        else
                        {
                            BackUp.Add(movePath);
                            continue;
                        }
                    }
                    else
                    {
                        _logService.TXTLog($"UnSignBackUp_To_InProgress : {cus_ct_id} 空路徑");
                        return new();
                    }
                }
                if (BackUp.Count != InProgressFileList.Count)
                {
                    _logService.TXTLog($"UnSignBackUp_To_InProgress : BackUp.Count != InProgressFileList.Count  {BackUp.Count} : {InProgressFileList.Count} ");
                    return new();
                }
                pathData.Add(InProgressFileList); //[0] 舊路徑
                pathData.Add(BackUp); //[1] 新路徑



                return pathData;
            }
            catch (Exception ex)
            {
                _logService.TXTLog($"UnSignBackUp_To_InProgress 錯誤 : {ex.Message}");
                return new();
            }
        }

        public string GET_ReportType_Name(string FilePath, string cus_ct_id)
        {
            if (FilePath.Contains("\\ADSC_C\\"))
                return "脂肪間質幹細胞鑑定報告";
            else if (FilePath.Contains("\\ADSC_E\\"))
                return "脂肪間質幹細胞鑑定報告書(英)";
            else if (FilePath.Contains("\\Blood_C\\") && FilePath.Contains("ADSC"))
                return "血液檢驗報告書";
            else if (FilePath.Contains("\\Blood_E\\") && FilePath.Contains("ADSC"))
                return "血液檢驗報告書(英)";
            else if (FilePath.Contains("\\Blood_C\\") && FilePath.Contains("MSC"))
                return "血液檢驗報告書";
            else if (FilePath.Contains("\\Blood\\") && FilePath.Contains("TSC"))
                return "血液檢驗報告書";
            else if (FilePath.Contains("\\Blood_E\\") && FilePath.Contains("MSC"))
                return "血液檢驗報告書(英)";
            else if (FilePath.Contains("\\MSC_C\\"))
                return "臍帶間質幹細胞鑑定報告書";
            else if (FilePath.Contains("\\MSC_E\\"))
                return "臍帶間質幹細胞鑑定報告書(英)";
            else if (FilePath.Contains("\\TSC\\"))
                return "牙齒幹細胞鑑定報告書";
            else if (FilePath.Contains("\\CB_C\\") || FilePath.Contains("\\CB_E\\") || FilePath.Contains("\\TB\\"))
                return "臍血檢驗報告單";
            else if (FilePath.Contains("HLA_E"))
                return "人類白血球抗原(HLA)分型結果(英)";
            else if (FilePath.Contains("HLA") || FilePath.Contains("HLA_C"))
                return "人類白血球抗原(HLA)分型結果(中)";
            else if (FilePath.Contains("Store_E") && FilePath.Contains("MSC"))
                return "臍帶組織儲存報告書(英)";
            else if ((FilePath.Contains("Store") || FilePath.Contains("Store_C")) && FilePath.Contains("MSC"))
                return "臍帶組織儲存報告書(中)";
            else if (FilePath.Contains("Store_E") && FilePath.Contains("ADSC"))
                return "脂肪組織儲存報告書(英)";
            else if ((FilePath.Contains("Store") || FilePath.Contains("Store_C")) && FilePath.Contains("ADSC"))
                return "脂肪組織儲存報告書(中)";
            else if (FilePath.Contains("PBSC"))
            {
                string CV_PT = _SQL.GetCV_PT_ForPBSC(cus_ct_id);
                if (CV_PT == "周邊血及免疫細胞" || CV_PT == "周邊血及免疫細胞PIB")
                    return "周邊血幹細胞及免疫細胞鑑定報告書說明.pdf";
                else if (CV_PT == "免疫細胞" || CV_PT == "免疫細胞MNC")
                    return "免疫細胞鑑定報告書說明.pdf";
                else
                    return "周邊血幹細胞鑑定報告書說明.pdf";
            }
            else
                return "";
        }

        private List<ReportFile> GET_ReportFileList(string cus_ct_id) //不找備份
        {
            if (string.IsNullOrEmpty(cus_ct_id))
                return new();
            List<string> FileList = new();
            List<ReportFile> reportFiles = new();
            FileList.AddRange(ScanAllPDFFilesPath("InProgress", cus_ct_id));
            FileList.AddRange(ScanAllPDFFilesPath("Upload", cus_ct_id));
            for (int i = 0; i < FileList.Count; i++)
            {
                ReportFile z = new();
                z.cus_ct_id = cus_ct_id;
                z.FilePath = FileList[i];
                z.ReportType = GET_ReportType_Name(FileList[i], cus_ct_id);
                if (FileList[i].Contains("InProgress"))
                    z.IsRelease = "N";
                else
                    z.IsRelease = "Y";
                reportFiles.Add(z);
            }
            return reportFiles;
        }
        private Boolean Delete_FileList(string cus_ct_id)
        {
            try
            {
                if (!string.IsNullOrEmpty(cus_ct_id))
                {
                    string sql = "DELETE FROM cr_FileList WHERE cus_ct_id = @K_cus_ct_id";
                    List<SqlParameter> Paras = new() { new("K_cus_ct_id", cus_ct_id) };
                    if (_SQL.SqlNonQuery(sql, Paras) == CDictionary.OK)
                        return true;
                    else return false;
                }
                else return false;
            }
            catch (Exception ex)
            {
                _logService.TXTLog($"Delete_FileList 錯誤 --  {ex.Message} ");
                return false;
            }
        }

        public void REFRESH_FileList(string cus_ct_id)
        {
            List<ReportFile> reportFiles = GET_ReportFileList(cus_ct_id);
            if (reportFiles != null || reportFiles.Count != 0)
            {
                if (Delete_FileList(cus_ct_id))
                {
                    for (int i = 0; i < reportFiles.Count; i++)
                    {
                        string sql = " INSERT INTO cr_FileList (cus_ct_id, ReportType, FilePath, IsRelease) VALUES (@K_cus_ct_id, @K_ReportType, @K_FilePath, @K_IsRelease) ";
                        List<SqlParameter> Paras = new()
                        {
                            new("K_cus_ct_id", reportFiles[i].cus_ct_id),
                            new("K_ReportType", reportFiles[i].ReportType),
                            new("K_FilePath", reportFiles[i].FilePath),
                            new("K_IsRelease", reportFiles[i].IsRelease)
                        };
                        _SQL.SqlNonQuery(sql, Paras);
                    }
                }
            }
        }


        #region "棄用"
        public string GETProductType(string cus_ct_id)
        {
            List<SqlParameter> Paras = new() { new SqlParameter("K_cus_ct_id", cus_ct_id) };
            return _SQL.StringReader("SELECT ProductType FROM cr_main WHERE cus_ct_id = @K_cus_ct_id", "ProductType", Paras);
        }
        public string GetFileName(string cus_ct_id)
        {
            string ProductType = GETProductType(cus_ct_id);
            if (ProductType == "ADSC")
            {
                if (cus_ct_id.StartsWith("BM") || cus_ct_id.StartsWith("AW") || cus_ct_id.StartsWith("AO") || cus_ct_id.StartsWith("HO"))
                    ProductType += cus_ct_id.Substring(2, 3);
                else if (cus_ct_id.StartsWith("ASC") || cus_ct_id.StartsWith("ASB") || cus_ct_id.StartsWith("ASM") || cus_ct_id.StartsWith("ASX") || cus_ct_id.StartsWith("AST") || cus_ct_id.StartsWith("EQK") || cus_ct_id.StartsWith("ACC"))
                    ProductType += cus_ct_id.Substring(3, 3);
            }
            else if (ProductType == "MSC")
            {
                if (cus_ct_id.StartsWith("MDW_D"))
                    ProductType += cus_ct_id.Substring(4, 3);
                else if (cus_ct_id.StartsWith("MBT") || cus_ct_id.StartsWith("MHT") || cus_ct_id.StartsWith("MFT") || cus_ct_id.StartsWith("MDW") || cus_ct_id.StartsWith("MLA") || cus_ct_id.StartsWith("MLB") || cus_ct_id.StartsWith("MLC") || cus_ct_id.StartsWith("MST") || cus_ct_id.StartsWith("MSX") || cus_ct_id.StartsWith("EQK") || cus_ct_id.StartsWith("MCA") || cus_ct_id.StartsWith("MCB") || cus_ct_id.StartsWith("MCC"))
                    ProductType += cus_ct_id.Substring(3, 3);
                else if (cus_ct_id.StartsWith("MH") || cus_ct_id.StartsWith("MB") || cus_ct_id.StartsWith("TM") || cus_ct_id.StartsWith("MS") || cus_ct_id.StartsWith("MT") || cus_ct_id.StartsWith("HO"))
                    ProductType += cus_ct_id.Substring(2, 3);
                else if (cus_ct_id.StartsWith("MST1"))
                    ProductType += cus_ct_id.Substring(4, 3);
                else if (cus_ct_id.StartsWith("M"))
                    ProductType += cus_ct_id.Substring(1, 3);
            }
            else if (ProductType == "CB")
            {
                if (cus_ct_id.StartsWith("DHS") || cus_ct_id.StartsWith("DHN") || cus_ct_id.StartsWith("HBS") || cus_ct_id.StartsWith("TBS") || cus_ct_id.StartsWith("HBN") || cus_ct_id.StartsWith("CHN") || cus_ct_id.StartsWith("CHS") || cus_ct_id.StartsWith("ECB"))
                    ProductType += cus_ct_id.Substring(3, 3);
                else if (cus_ct_id.StartsWith("HN") || cus_ct_id.StartsWith("HS") || cus_ct_id.StartsWith("DN") || cus_ct_id.StartsWith("DS") || cus_ct_id.StartsWith("BN") || cus_ct_id.StartsWith("BS") || cus_ct_id.StartsWith("TB") || cus_ct_id.StartsWith("TN") || cus_ct_id.StartsWith("BN") || cus_ct_id.StartsWith("BS") || cus_ct_id.StartsWith("BN") || cus_ct_id.StartsWith("BS"))
                    ProductType += cus_ct_id.Substring(2, 3);
                else if (cus_ct_id.StartsWith("N") || cus_ct_id.StartsWith("S") || cus_ct_id.StartsWith("E"))
                    ProductType += cus_ct_id.Substring(1, 3);
            }
            else if (ProductType == "PBSC")
            {
                if (cus_ct_id.StartsWith("PIN") || cus_ct_id.StartsWith("PIS") || cus_ct_id.StartsWith("PIB") || cus_ct_id.StartsWith("PMN") || cus_ct_id.StartsWith("PMS") || cus_ct_id.StartsWith("PIM") || cus_ct_id.StartsWith("WBN") || cus_ct_id.StartsWith("MNC"))
                    ProductType += cus_ct_id.Substring(3, 3);
                else if (cus_ct_id.StartsWith("PN") || cus_ct_id.StartsWith("PS"))
                    ProductType += cus_ct_id.Substring(2, 3);
            }
            else if (ProductType == "TSC")
                if (cus_ct_id.StartsWith("TSC") || cus_ct_id.StartsWith("TSA") || cus_ct_id.StartsWith("TSA") || cus_ct_id.StartsWith("THC") || cus_ct_id.StartsWith("THA") || cus_ct_id.StartsWith("THA"))
                    ProductType += cus_ct_id.Substring(3, 3);

            return ProductType;
        }
        #endregion      
    }
}
