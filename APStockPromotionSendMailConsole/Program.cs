using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AP_StockPromotion_V1.Class;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;

namespace APStockPromotionSendMailConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            sendMailWarningReturnItemToHO();
            // sendMailWarningReturnItemToResponse();
            sendMailAlertExpireReturnItemToHO();
            // sendMailAlertExpireReturnItemToResponse();
        }
        
        /* = - - - - - - - - - - - - - - - - - - - - = */
        private static void sendMailWarningReturnItemToHO()
        {
            classMail cls = new classMail();
            DataTable dt = cls.getDateWarningReturnItem("N");
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    string reportpath = exportReportWarningReturnItemToHO();
                    string msgErr = "";
                    string mailbody = getBodyWarningReturnItemToHO();
                    cls.sendMail("thirawuth_s@apthai.com", "thirawuth_s@apthai.com", "", "", "รายงานแจ้งเตือนสินค้าใกล้กำหนดส่งคืน" + DateTime.Now.ToString("dd/MM/yyyy"), mailbody, reportpath, ref msgErr);
                }
            }
        }

        private static void sendMailWarningReturnItemToResponse()
        {
            classMail cls = new classMail();
            DataTable dt = cls.getDateWarningReturnItem("N");
            DataTable dtReqBy = dt.DefaultView.ToTable(true, "UserResponse", "Email");
            foreach (DataRow dr in dtReqBy.Rows)
            {
                string reportpath = exportReportWarningReturnItemToResponse(dr["UserResponse"] + "");
                string msgErr = "";
                string mailbody = getBodyWarningReturnItemToHO();
                if (dr["Email"] + "" != "")
                {
                    //cls.sendMail("chitaphon_p@apthai.com", dr["Email"] + "", "", "", "รายงานแจ้งเตือนสินค้าใกล้เวลาเรียกคืน" + DateTime.Now.ToString("dd/MM/yyyy"), mailbody, reportpath, ref msgErr);
                    cls.sendMail("chitaphon_p@apthai.com", "chitaphon_p@apthai.com", "", "", "รายงานแจ้งเตือนสินค้าใกล้กำหนดส่งคืน" + DateTime.Now.ToString("dd/MM/yyyy"), mailbody, reportpath, ref msgErr);
                    //cls.sendMail("chitaphon_p@apthai.com", "polwaritpakorn@apthai.com", "", "", "รายงานแจ้งเตือนสินค้าใกล้เวลาเรียกคืน" + DateTime.Now.ToString("dd/MM/yyyy"), mailbody, reportpath, ref msgErr);
                }
            }
        }

        private static void sendMailAlertExpireReturnItemToHO()
        {
            classMail cls = new classMail();
            DataTable dt = cls.getDateWarningReturnItem("Y");
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    string reportpath = exportReportAlertReturnItemToHO();
                    string msgErr = "";
                    string mailbody = getBodyWarningReturnItemToHO();
                    cls.sendMail("chitaphon_p@apthai.com", "chitaphon_p@apthai.com", "", "", "รายงานแจ้งเตือนสินค้าถึงกำหนดส่งคืน" + DateTime.Now.ToString("dd/MM/yyyy"), mailbody, reportpath, ref msgErr);
                }
            }
        }

        private static void sendMailAlertExpireReturnItemToResponse()
        {
            classMail cls = new classMail();
            DataTable dt = cls.getDateWarningReturnItem("Y");
            DataTable dtReqBy = dt.DefaultView.ToTable(true, "UserResponse", "Email");
            foreach (DataRow dr in dtReqBy.Rows)
            {
                string reportpath = exportReportAlertReturnItemToResponse(dr["UserResponse"] + "");
                string msgErr = "";
                string mailbody = getBodyWarningReturnItemToHO();
                if (dr["Email"] + "" != "")
                {
                    //cls.sendMail("chitaphon_p@apthai.com", dr["Email"] + "", "", "", "รายงานแจ้งเตือนสินค้าใกล้เวลาเรียกคืน" + DateTime.Now.ToString("dd/MM/yyyy"), mailbody, reportpath, ref msgErr);
                    cls.sendMail("chitaphon_p@apthai.com", "chitaphon_p@apthai.com", "", "", "รายงานแจ้งเตือนสินค้าถึงกำหนดส่งคืน" + DateTime.Now.ToString("dd/MM/yyyy"), mailbody, reportpath, ref msgErr);
                    //cls.sendMail("chitaphon_p@apthai.com", "polwaritpakorn@apthai.com", "", "", "รายงานแจ้งเตือนสินค้าใกล้เวลาเรียกคืน" + DateTime.Now.ToString("dd/MM/yyyy"), mailbody, reportpath, ref msgErr);
                }
            }
        }


        /* = - - - - - - - - - - - - - - - - - - - - = */
        private static string exportReportWarningReturnItemToHO()
        {
            string appPath = Directory.GetCurrentDirectory();
            string reportPath = appPath + "/rpt/rptWarningReturn.rpt";
            string exportName = "ReportWarningReturnItemToHO_" + DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + ".xls";
            string exportPath = appPath + "/Report/" + exportName;

            if (!Directory.Exists(appPath + "/Report/")){
                Directory.CreateDirectory(appPath + "/Report/");
            }

            try
            {
                ReportDocument reportDocument = new ReportDocument();
                reportDocument.Load(reportPath);

                DAReport clsRpt = new DAReport();
                reportDocument.SetDatabaseLogon(clsRpt.getUserID(), clsRpt.getPassword(), clsRpt.getDataSource(), clsRpt.getInitialCatalog());//("", "", "ADMIN-PC\\ADMIN", "dbRMC"); 

                reportDocument.SetParameterValue("@UserResponse", "");
                reportDocument.SetParameterValue("@isAlert", "N");

                ExportOptions CrExportOptions;
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                ExcelFormatOptions CrFormatTypeOptions = new ExcelFormatOptions();
                CrDiskFileDestinationOptions.DiskFileName = exportPath; // "C:\\SampleReport.pdf";
                CrExportOptions = reportDocument.ExportOptions;
                {
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                    CrExportOptions.FormatOptions = CrFormatTypeOptions;
                }
                reportDocument.Export();

                return exportPath;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return "";
            }
            
        }
        private static string exportReportWarningReturnItemToResponse(string UserResponse)
        {
            string appPath = Directory.GetCurrentDirectory();
            string reportPath = appPath + "/rpt/rptWarningReturn.rpt";//rptMailAlertNWarningReturnItem.rpt";
            string exportName = "ReportWarningReturnItemToUser_" + DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + ".xls";
            string exportPath = appPath + "/Report/" + exportName;

            if (!Directory.Exists(appPath + "/Report/")){
                Directory.CreateDirectory(appPath + "/Report/");
            }

            try
            {
                ReportDocument reportDocument = new ReportDocument();
                reportDocument.Load(reportPath);

                DAReport clsRpt = new DAReport();
                reportDocument.SetDatabaseLogon(clsRpt.getUserID(), clsRpt.getPassword(), clsRpt.getDataSource(), clsRpt.getInitialCatalog());//("", "", "ADMIN-PC\\ADMIN", "dbRMC"); 

                reportDocument.SetParameterValue("@UserResponse", UserResponse);
                reportDocument.SetParameterValue("@isAlert", "N");

                ExportOptions CrExportOptions;
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                ExcelFormatOptions CrFormatTypeOptions = new ExcelFormatOptions();
                CrDiskFileDestinationOptions.DiskFileName = exportPath; // "C:\\SampleReport.pdf";
                CrExportOptions = reportDocument.ExportOptions;
                {
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                    CrExportOptions.FormatOptions = CrFormatTypeOptions;
                }
                reportDocument.Export();

                return exportPath;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return "";
            }
            
        }
        private static string exportReportAlertReturnItemToHO()
        {
            string appPath = Directory.GetCurrentDirectory();
            string reportPath = appPath + "/rpt/rptWarningReturn.rpt";//rptMailAlertNWarningReturnItem.rpt";
            string exportName = "ReportAlertReturnItemToHO_" + DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + ".xls";
            string exportPath = appPath + "/Report/" + exportName;

            if (!Directory.Exists(appPath + "/Report/"))
            {
                Directory.CreateDirectory(appPath + "/Report/");
            }

            try
            {
                ReportDocument reportDocument = new ReportDocument();
                reportDocument.Load(reportPath);

                DAReport clsRpt = new DAReport();
                reportDocument.SetDatabaseLogon(clsRpt.getUserID(), clsRpt.getPassword(), clsRpt.getDataSource(), clsRpt.getInitialCatalog());//("", "", "ADMIN-PC\\ADMIN", "dbRMC"); 

                reportDocument.SetParameterValue("@UserResponse", "");
                reportDocument.SetParameterValue("@isAlert", "Y");

                ExportOptions CrExportOptions;
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                ExcelFormatOptions CrFormatTypeOptions = new ExcelFormatOptions();
                CrDiskFileDestinationOptions.DiskFileName = exportPath; // "C:\\SampleReport.pdf";
                CrExportOptions = reportDocument.ExportOptions;
                {
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                    CrExportOptions.FormatOptions = CrFormatTypeOptions;
                }
                reportDocument.Export();

                return exportPath;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return "";
            }

        }
        private static string exportReportAlertReturnItemToResponse(string UserResponse)
        {
            string appPath = Directory.GetCurrentDirectory();
            string reportPath = appPath + "/rpt/rptWarningReturn.rpt";//rptMailAlertNWarningReturnItem.rpt";
            string exportName = "ReportAlertReturnItemToUser_" + DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + ".xls";
            string exportPath = appPath + "/Report/" + exportName;

            if (!Directory.Exists(appPath + "/Report/"))
            {
                Directory.CreateDirectory(appPath + "/Report/");
            }

            try
            {
                ReportDocument reportDocument = new ReportDocument();
                reportDocument.Load(reportPath);

                DAReport clsRpt = new DAReport();
                reportDocument.SetDatabaseLogon(clsRpt.getUserID(), clsRpt.getPassword(), clsRpt.getDataSource(), clsRpt.getInitialCatalog());//("", "", "ADMIN-PC\\ADMIN", "dbRMC"); 

                reportDocument.SetParameterValue("@UserResponse", UserResponse);
                reportDocument.SetParameterValue("@isAlert", "Y");

                ExportOptions CrExportOptions;
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                ExcelFormatOptions CrFormatTypeOptions = new ExcelFormatOptions();
                CrDiskFileDestinationOptions.DiskFileName = exportPath; // "C:\\SampleReport.pdf";
                CrExportOptions = reportDocument.ExportOptions;
                {
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                    CrExportOptions.FormatOptions = CrFormatTypeOptions;
                }
                reportDocument.Export();

                return exportPath;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return "";
            }

        }

        /* = - - - - - - - - - - - - - - - - - - - - = */

        private static string getBodyWarningReturnItemToHO()
        {
            string rstBody = "";
            rstBody += "เรียน ทุกท่าน" + "<br /><br />";
            rstBody += "&nbsp;&nbsp;&nbsp;&nbsp;รายงานสรุปรายการสินค้าใกล้หมดช่วงเวลาโปรโมชั่นรายโครงการ" + "<br />";
            rstBody += "รายละเอียดตามเอกสารแนบ" + "<br /><br />";
            return rstBody;
        }
        /* = - - - - - - - - - - - - - - - - - - - - = */

        /* = - - - - - - - - - - - - - - - - - - - - = */

    }
}
