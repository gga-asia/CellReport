using CellReport_Workflow.Interface;
using CellReport_Workflow.Models.Sql;
using CellReport_Workflow.ViewModel;
using iTextSharp.text.pdf;
using System.Reflection.PortableExecutable;

namespace CellReport_Workflow.Service
{
    public class StampService : IStampService
    {
        private readonly IConfiguration _Configuration;
        private readonly IFileService _FileService;
        private readonly ILogService _LogService;
        private readonly IWebHostEnvironment _Environment;
        //_environment.WebRootPath + "/File/" + path;
        public StampService(IConfiguration Configuration, IFileService fileService, ILogService logService, IWebHostEnvironment environment)
        {
            _Configuration = Configuration;
            _FileService = fileService;
            _LogService = logService;
            _Environment = environment;
        }
        //List<string> type = new() { "\\ADSC\\", "\\MSC_C\\", "\\MSC_E\\", "\\PBSC\\", "\\TSC\\", "\\CB_C\\", "\\CB_E\\", "\\HLA\\", "\\HLA_C\\", "\\HLA_E\\", "\\Blood\\", "\\Blood_C\\", "\\Blood_E\\", "\\Store\\", "\\Store_C\\", "\\Store_E\\", "\\Indonesia\\" };
        public string Stamp(string cus_ct_id)
        {
            try
            {
                List<string> FileList = _FileService.ScanAllPDFFilesPath("InProgress", cus_ct_id);
                if (FileList.Count > 0)
                {
                    for (int i = 0; i < FileList.Count; i++)
                        if (!FileList[i].Contains("CBINDFIVEDAY"))
                            if (StampForPdf(FileList[i], cus_ct_id))
                                if (File.Exists(FileList[i]))
                                {
                                    File.Delete(FileList[i]);
                                    File.Move(_Environment.WebRootPath + $@"\temp\{cus_ct_id}.pdf", FileList[i]);
                                }
                                else
                                    return CDictionary.ERR;
                    return CDictionary.OK;
                }
                else
                    return CDictionary.ERR;
            }
            catch (Exception ex)
            {
                _LogService.TXTLog($"Stamp 錯誤 -- {ex.Message}");
                return CDictionary.ERR;
            }
        }
        private bool StampForPdf(string Path, string cus_ct_id)
        {
            string pdfFilePath = Path;
            string outputFilePath = _Environment.WebRootPath + $@"\temp\{cus_ct_id}.pdf";
            string stampImagePathQC_Stramp = _Environment.WebRootPath + @"\img\Stamp\QC.png";
            string stampImagePath_LAB_Stramp = _Environment.WebRootPath + @"\img\Stamp\LAB.png";
            string stampImagePath_COM_Stramp = _Environment.WebRootPath + @"\img\Stamp\COM.png";

            StempPosition stempPosition = GetStempPosition(Path);
            try
            {
                using (PdfReader reader = new(pdfFilePath))
                {
                    using (FileStream fs = new(outputFilePath, FileMode.Create))
                    {
                        using (PdfStamper stamper = new(reader, fs))
                        {
                            // 載入QC章圖檔
                            iTextSharp.text.Image stampImage1 = iTextSharp.text.Image.GetInstance(stampImagePathQC_Stramp);
                            float x1 = stempPosition.QC_X; // X 座標
                            float y1 = stempPosition.QC_Y; // Y 座標
                            float width1 = 47; // 寬度
                            float height1 = 15; // 高度
                            stampImage1.SetAbsolutePosition(x1, y1);
                            stampImage1.ScaleAbsolute(width1, height1);

                            // 載入LAB章圖檔
                            iTextSharp.text.Image stampImage2 = iTextSharp.text.Image.GetInstance(stampImagePath_LAB_Stramp);
                            float x2 = stempPosition.LAB_X; // X 座標
                            float y2 = stempPosition.LAB_Y; // Y 座標
                            float width2 = 45; // 寬度
                            float height2 = 15; // 高度
                            stampImage2.SetAbsolutePosition(x2, y2);
                            stampImage2.ScaleAbsolute(width2, height2);

                            // 載入公司章圖檔
                            iTextSharp.text.Image stampImage3 = iTextSharp.text.Image.GetInstance(stampImagePath_COM_Stramp);
                            float x3 = stempPosition.COM_X; // X 座標
                            float y3 = stempPosition.COM_Y; // Y 座標
                            float width3 = 45; // 寬度
                            float height3 = 45; // 高度
                            stampImage3.SetAbsolutePosition(x3, y3);
                            stampImage3.ScaleAbsolute(width3, height3);

                            PdfContentByte pdfContent = stamper.GetOverContent(1); //第一頁

                            pdfContent.AddImage(stampImage1);
                            pdfContent.AddImage(stampImage2);
                            pdfContent.AddImage(stampImage3);

                            stamper.Close();
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                _LogService.TXTLog($"StampForPdf 錯誤 : {ex.Message}");
                return false;
            }
        }
        private StempPosition GetStempPosition(string Path)
        {
            StempPosition position = new();
            string conf = "StampSetting:";
            if (!string.IsNullOrEmpty(Path))
            {
                if (Path.Contains("ADSC"))
                {
                    conf += "ADSC:";
                    if (Path.Contains("\\ADSC_C\\"))
                        conf += "ADSC_C:";
                    else if (Path.Contains("\\ADSC_E\\"))
                        conf += "ADSC_E:";
                    else if (Path.Contains("\\Blood_C\\"))
                        conf += "Blood_C:";
                    else if (Path.Contains("\\Blood_E\\"))
                        conf += "Blood_E:";
                    else if (Path.Contains("\\Store_C\\"))
                        conf += "Store_C:";
                    else if (Path.Contains("\\Store_E\\"))
                        conf += "Store_E:";
                    else if (Path.Contains("\\HLA_C\\"))
                        conf += "HLA_C:";
                    else if (Path.Contains("\\HLA_E\\"))
                        conf += "HLA_E:";
                }
                else if (Path.Contains("MSC"))
                {
                    conf += "MSC:";
                    if (Path.Contains("\\MSC_C\\"))
                        conf += "MSC_C:";
                    else if (Path.Contains("\\MSC_E\\"))
                        conf += "MSC_E:";
                    else if (Path.Contains("\\Blood_C\\"))
                        conf += "Blood_C:";
                    else if (Path.Contains("\\Blood_E\\"))
                        conf += "Blood_E:";
                    else if (Path.Contains("\\Store_C\\"))
                        conf += "Store_C:";
                    else if (Path.Contains("\\Store_E\\"))
                        conf += "Store_E:";
                    else if (Path.Contains("\\HLA_C\\"))
                        conf += "HLA_C:";
                    else if (Path.Contains("\\HLA_E\\"))
                        conf += "HLA_E:";
                }
                else if (Path.Contains("TSC"))
                {
                    conf += "TSC:";
                    if (Path.Contains("\\TSC\\"))
                        conf += "TSC:";
                    else if (Path.Contains("\\Blood\\"))
                        conf += "Blood:";
                    else if (Path.Contains("\\Store\\"))
                        conf += "Store:";
                    else if (Path.Contains("\\HLA\\"))
                        conf += "HLA:";
                }
                else if (Path.Contains("CB"))
                {
                    conf += "CB:";
                    if (Path.Contains("\\CB_C\\"))
                        conf += "CB_C:";
                    else if (Path.Contains("\\CB_E\\"))
                        conf += "CB_E:";
                    else if (Path.Contains("\\HLA_C\\"))
                        conf += "HLA_C:";
                    else if (Path.Contains("\\HLA_E\\"))
                        conf += "HLA_E:";
                    else if (Path.Contains("\\TB\\"))
                        conf += "TB:";
                }
                else if (Path.Contains("PBSC"))
                {
                    conf += "PBSC:";
                    if (Path.Contains("\\PBSC\\"))
                        conf += "PBSC:";
                    else if (Path.Contains("\\HLA\\"))
                        conf += "HLA:";
                }
                else
                    return new();
                position.QC_X = float.Parse(_Configuration[conf + "QC:X"]);
                position.QC_Y = float.Parse(_Configuration[conf + "QC:Y"]);
                position.LAB_X = float.Parse(_Configuration[conf + "LAB:X"]);
                position.LAB_Y = float.Parse(_Configuration[conf + "LAB:Y"]);
                position.COM_X = float.Parse(_Configuration[conf + "COM:X"]);
                position.COM_Y = float.Parse(_Configuration[conf + "COM:Y"]);
            }
            return position;
        }
    }
    internal class StempPosition
    {
        public float QC_X { get; set; }
        public float QC_Y { get; set; }
        public float LAB_X { get; set; }
        public float LAB_Y { get; set; }
        public float COM_X { get; set; }
        public float COM_Y { get; set; }
    }
}
