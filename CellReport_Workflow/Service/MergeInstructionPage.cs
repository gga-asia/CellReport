using CellReport_Workflow.Interface;
using CellReport_Workflow.Models.Sql;
using CellReport_Workflow.ViewModel;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace CellReport_Workflow.Service
{
    public class MergeInstructionPageService : IMergeInstructionPageService
    {
        public readonly IFileService _fileService;
        private readonly IWebHostEnvironment _Environment;
        private readonly ILogService _logService;
        private readonly ISql _sql;
        public MergeInstructionPageService(IFileService fileService, IWebHostEnvironment environment, ILogService logService, ISql sql)
        {
            _fileService = fileService;
            _Environment = environment;
            _logService = logService;
            _sql = sql;
        }
        public string MergeInstruction(string id)
        {
            List<string> FilePath = _fileService.ScanAllPDFFilesPath("InProgress", id);
            Boolean STU = false;
            string outputFilePath = _Environment.WebRootPath + $@"\temp\{id}.pdf";
            for (int i = 0; i < FilePath.Count; i++)
            {
                if (FilePath[i].Contains("HLA") || FilePath[i].Contains("Store"))
                    continue;

                if (FilePath[i].Contains("\\ADSC_C\\"))
                    STU = AppendPdfToEnd(FilePath[i], _Environment.WebRootPath + $@"\InstructionPage\脂肪間質幹細胞鑑定報告書說明.pdf", FilePath[i], id);
                else if (FilePath[i].Contains("\\ADSC_E\\"))
                    STU = AppendPdfToEnd(FilePath[i], _Environment.WebRootPath + $@"\InstructionPage\脂肪間質幹細胞鑑定報告書說明_E.pdf", FilePath[i], id);
                else if (FilePath[i].Contains("\\Blood_C\\") && FilePath[i].Contains("ADSC"))
                    STU = AppendPdfToEnd(FilePath[i], _Environment.WebRootPath + $@"\InstructionPage\血液檢驗報告書說明.pdf", FilePath[i], id);
               
                else if (FilePath[i].Contains("\\Blood_E\\") && FilePath[i].Contains("ADSC"))
                    STU = AppendPdfToEnd(FilePath[i], _Environment.WebRootPath + $@"\InstructionPage\血液檢驗報告書說明_E.pdf", FilePath[i], id);
                else if (FilePath[i].Contains("\\Blood_C\\") && FilePath[i].Contains("MSC"))
                    STU = AppendPdfToEnd(FilePath[i], _Environment.WebRootPath + $@"\InstructionPage\血液檢驗報告書說明.pdf", FilePath[i], id);
                else if (FilePath[i].Contains("\\Blood_E\\") && FilePath[i].Contains("MSC"))
                    STU = AppendPdfToEnd(FilePath[i], _Environment.WebRootPath + $@"\InstructionPage\血液檢驗報告書說明_E.pdf", FilePath[i], id);
                else if (FilePath[i].Contains("\\MSC_C\\"))
                    STU = AppendPdfToEnd(FilePath[i], _Environment.WebRootPath + $@"\InstructionPage\臍帶間質幹細胞鑑定報告書說明.pdf", FilePath[i], id);
                else if (FilePath[i].Contains("\\MSC_E\\"))
                    STU = AppendPdfToEnd(FilePath[i], _Environment.WebRootPath + $@"\InstructionPage\臍帶間質幹細胞鑑定報告書說明_E.pdf", FilePath[i], id);
                else if (FilePath[i].Contains("\\TSC\\"))
                    STU = AppendPdfToEnd(FilePath[i], _Environment.WebRootPath + $@"\InstructionPage\牙齒幹細胞鑑定報告書說明.pdf", FilePath[i], id);
                else if (FilePath[i].Contains("\\Blood\\") && FilePath[i].Contains("TSC"))
                    STU = AppendPdfToEnd(FilePath[i], _Environment.WebRootPath + $@"\InstructionPage\血液檢驗報告書說明.pdf", FilePath[i], id);
                else if (FilePath[i].Contains("\\CB_C\\") || FilePath[i].Contains("\\CB_E\\") || FilePath[i].Contains("\\TB\\"))
                    STU = AppendPdfToEnd(FilePath[i], _Environment.WebRootPath + $@"\InstructionPage\臍血檢驗報告單說明.pdf", FilePath[i], id);
                else if (FilePath[i].Contains("PBSC"))
                {
                    string CV_PT = _sql.GetCV_PT_ForPBSC(id);
                    string PBSC_PageName = "";
                    if (CV_PT == "周邊血及免疫細胞" || CV_PT == "周邊血及免疫細胞PIB")
                        PBSC_PageName = "周邊血幹細胞及免疫細胞鑑定報告書說明.pdf";
                    else if (CV_PT == "免疫細胞" || CV_PT == "免疫細胞MNC")
                        PBSC_PageName = "免疫細胞鑑定報告書說明.pdf";
                    else
                        PBSC_PageName = "周邊血幹細胞鑑定報告書說明.pdf";
                    STU = AppendPdfToEnd(FilePath[i], _Environment.WebRootPath + $@"\InstructionPage\{PBSC_PageName}", FilePath[i], id);
                }

                if (File.Exists(FilePath[i]) && File.Exists(outputFilePath) && STU == true)
                {
                    File.Delete(FilePath[i]);
                    File.Move(outputFilePath, FilePath[i]);
                }
                else
                {
                 
                        if (File.Exists(outputFilePath))
                            File.Delete(outputFilePath);
                    return $"{id}說明頁合併失敗";
                }

                //else if (FilePath[i].Contains(""))
                //    AppendPdfToEnd(FilePath[i], "", FilePath[i]);
                //else if (FilePath[i].Contains(""))
                //    AppendPdfToEnd(FilePath[i], "", FilePath[i]);
            }
            return CDictionary.OK;
        }

        private Boolean AppendPdfToEnd(string sourcePdfPath, string appendPdfPath, string mergedPdfPath, string id)
        {
            string outputFilePath = _Environment.WebRootPath + $@"\temp\{id}.pdf";
            try
            {
                using (FileStream stream = new(outputFilePath, FileMode.Create))
                {
                    Document document = new();
                    PdfCopy pdf = new(document, stream);
                    document.Open();

                    PdfReader readerSource = new(sourcePdfPath);
                    PdfReader readerAppend = new(appendPdfPath);

                    int totalPagesSource = readerSource.NumberOfPages;
                    int totalPagesAppend = readerAppend.NumberOfPages;

                    for (int i = 1; i <= totalPagesSource; i++)
                    {
                        PdfImportedPage importedPage = pdf.GetImportedPage(readerSource, i);
                        pdf.AddPage(importedPage);
                    }

                    for (int i = 1; i <= totalPagesAppend; i++)
                    {
                        PdfImportedPage importedPage = pdf.GetImportedPage(readerAppend, i);
                        pdf.AddPage(importedPage);
                    }

                    pdf.Close();
                    document.Close();

                    readerSource.Close();
                    readerAppend.Close();
                    return true;
                }
            }
            catch (Exception e)
            {
                _logService.TXTLog($"AppendPdfToEnd-- {e.Message}");
                return false;
            }
        }

    }
}
