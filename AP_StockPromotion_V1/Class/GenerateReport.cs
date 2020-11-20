using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AP_StockPromotion_V1.Class
{
    public class GenerateReport
    {
        public bool RequisitionRequestReport()
        {

            try
            {
                string reportPath = GlobalValiable.CrystalReportPath + "rptTransferItemToProjectListGroupDocWithReason.rpt";
                string pdfName = "ReportTransferToProjectListGroupDocWithReason_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string pdfPath = GlobalValiable.PDFReportPath + pdfName + ".pdf";
                //string pdfPopupPath = "Report/" + pdfName + ".pdf";
                string TrListId = "1162";

                // string project_Id = Request.QueryString["project_Id"] + "";
                if (TrListId == "")
                {
                    return false;
                }

                ReportDocument reportDocument = new ReportDocument();
                reportDocument.Load(reportPath);

                Class.DAReport clsRpt = new Class.DAReport();
                clsRpt.ApplyTableLogOnInfo(ref reportDocument);
                clsRpt.SetSubreportLogOnInfo(ref reportDocument);
                // reportDocument.SetDatabaseLogon(clsRpt.getUserID(), clsRpt.getPassword(), clsRpt.getDataSource(), clsRpt.getInitialCatalog());//("", "", "ADMIN-PC\\ADMIN", "dbRMC"); 

                reportDocument.SetParameterValue("@TrListId", TrListId);
                // reportDocument.SetParameterValue("@project_Id", project_Id);

                ExportOptions CrExportOptions;
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                //PdfRtfWordFormatOptions CrFormatTypeOptions = new ExcelFormatOptions();
                CrDiskFileDestinationOptions.DiskFileName = pdfPath; // "C:\\SampleReport.pdf";
                CrExportOptions = reportDocument.ExportOptions;
                {
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                    //CrExportOptions.FormatOptions = CrFormatTypeOptions;
                }
                reportDocument.Export();
                //callPdf(pdfPopupPath, reportDocument);

                return true;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return false;
            }
        }

    }
}