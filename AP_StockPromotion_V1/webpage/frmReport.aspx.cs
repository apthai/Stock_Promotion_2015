using System;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using AP_StockPromotion_V1.Class;
using System.IO;
using System.Data;
using AP_StockPromotion_V1.ws_authorize;
using System.Globalization;

namespace AP_StockPromotion_V1.webpage
{
    public partial class frmReport : Page
    {
        Entities.FormatDate convertDate = new Entities.FormatDate();
        DADelivery DADelivery = new DADelivery();
        string authEmployeeID = "";
        string authDisplayName = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            AutorizeData auth = new AutorizeData();
            auth = (AutorizeData)Session["userInfo_" + Session.SessionID] ?? new AutorizeData();
            authEmployeeID = auth?.EmployeeID;
            authDisplayName = auth?.DisplayName;

            if (Page.IsPostBack == false)
            {
                string reqRPT = Request.QueryString["reqRPT"] + "";
                if (reqRPT == "Tr01")
                {
                    ReportTransferToProjectList();
                }
                else if (reqRPT == "Tr02")
                {
                    ReportTransferToProjectDetail();
                }
                else if (reqRPT == "RQ")
                {
                    delAllRptFile();
                    ReportRequest();
                }
                else if (reqRPT == "Receive")
                {
                    delAllRptFile();
                    ReportReceive();
                }
                else if (reqRPT == "BL")
                {
                    delAllRptFile();
                    ReportBalance();
                }
                else if (reqRPT == "BL_Site")
                {
                    delAllRptFile();
                    ReportBalanceOnSite();
                }
                else if (reqRPT == "BL_SiteFull")
                {
                    delAllRptFile();
                    ReportBalanceOnSiteFull();
                }
                else if (reqRPT == "BL_Acc")
                {
                    // delAllRptFile();
                    ReportBalanceForAccount();
                }
                else if (reqRPT == "Tr")
                {
                    delAllRptFile();
                    ReportTransferToProject();
                }
                else if (reqRPT == "ReportPurchesingOrder")
                {
                    ReportPurchesingOrder();
                }
                else if (reqRPT == "ReportPurchesingOrder_New")
                {
                    ReportPurchesingOrder_New();
                }
                else if (reqRPT == "PstAccIO")
                {
                    ReportSAPFB03IO(); // ReportTransferToProject();
                }
                else if (reqRPT == "PstAccIOCrs")
                {
                    ReportSAPFB03IOCrs(); // ReportTransferToProject();
                }
                else if (reqRPT == "PstAccWBS")
                {
                    ReportSAPFB03WBS(); // ReportTransferToProject();
                }
                else if (reqRPT == "PstAccWBSCrs")
                {
                    ReportSAPFB03WBSCrs();// ReportTransferToProject();
                }

                else if (reqRPT == "DelvCnfWBS")
                {
                    ReportDelvCnfWBS();
                }
                else if (reqRPT == "DelvCnfWBSCrossCom")
                {
                    ReportDelvCnfWBSCrossCom();
                }
                else if (reqRPT == "DelvCnfIO")
                {
                    ReportDelvCnfIO();
                }
                else if (reqRPT == "DelvCnfIOCrossCom")
                {
                    ReportDelvCnfIOCrossCom();
                }
                else if (reqRPT == "BudgetIO")
                {
                    ReportBudgetIO();
                }
                else if (reqRPT == "Trn2ProBooking")
                {
                    ReportTransferToPreBooking();
                }
                else if (reqRPT == "PstAccIO_Test")
                {
                    string SAPDOC = Request.QueryString["SAPDOC"] + "";
                    ReportSAPFB03IO_Test(SAPDOC);
                }
                else if (reqRPT == "PstAccIOCrs_Test")
                {
                    string SAPDOC = Request.QueryString["SAPDOC"] + "";
                    ReportSAPFB03IOCrs_Test(SAPDOC);
                }
            }
        }

        private void delAllRptFile()
        {
            try
            {
                string pdfPath = Server.MapPath("../Report/");
                if (Directory.Exists(pdfPath))
                {
                    string[] filePaths = Directory.GetFiles(pdfPath);
                    foreach (string filePath in filePaths)
                    {
                        FileInfo fi = new FileInfo(filePath);
                        if (fi.CreationTime < DateTime.Now.AddDays(-7))
                        {
                            string[] fileSpt = filePath.Split('.');
                            if (fileSpt[fileSpt.Length - 1].ToLower() == "pdf"
                                || fileSpt[fileSpt.Length - 1].ToLower() == "xls"
                                || fileSpt[fileSpt.Length - 1].ToLower() == "xlsx")
                            {
                                File.Delete(filePath);
                            }
                        }
                    }
                }
            }
            catch (Exception) { }

        }

        private void ReportTransferToProjectList()
        {

            try
            {
                string reportPath = Server.MapPath("../rpt/rptTransferItemToProjectListGroupDoc.rpt");
                string pdfName = "ReportTransferToProjectList_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string pdfPath = Server.MapPath("../Report/" + pdfName + ".pdf");
                string pdfPopupPath = "../Report/" + pdfName + ".pdf";
                string TrListId = Request.QueryString["TrListId"] + "";
                string ReqFn = Request.QueryString["ReqFn"] + "";
                // string project_Id = Request.QueryString["project_Id"] + "";
                if (TrListId == "")// || project_Id == ""
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alertNClose('Invalid!');", true);
                    return;
                }

                ReportDocument reportDocument = new ReportDocument();
                reportDocument.Load(reportPath);

                DAReport clsRpt = new DAReport();
                clsRpt.ApplyTableLogOnInfo(ref reportDocument);
                clsRpt.SetSubreportLogOnInfo(ref reportDocument);
                // reportDocument.SetDatabaseLogon(clsRpt.getUserID(), clsRpt.getPassword(), clsRpt.getDataSource(), clsRpt.getInitialCatalog());//("", "", "ADMIN-PC\\ADMIN", "dbRMC"); 

                reportDocument.SetParameterValue("@TrListId", TrListId);
                reportDocument.SetParameterValue("@ReqFn", (ReqFn.Length > 1 ? ReqFn.Substring(0, 1) : ReqFn));
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

                callPdf(pdfPopupPath, reportDocument);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alertNClose('Exception: " + ex.Message.Replace("'", "") + "');", true);
                return;
            }
        }
          
        private void ReportTransferToProjectDetail()
        {
            try
            {
                string reportPath = Server.MapPath(@"../rpt/rptTransferItemToProjectDetailGroupDoc.rpt");
                string pdfName = "ReportTransferToProjectDetail_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string pdfPath = Server.MapPath(@"../Report/" + pdfName + ".pdf");
                string pdfPopupPath = @"../Report/" + pdfName + ".pdf";
                string TrListId = Request.QueryString["TrListId"] + "";
                string ReqFn = Request.QueryString["ReqFn"] + "";
                if (TrListId == "")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alertNClose('Invalid!');", true);
                    return;
                }

                ReportDocument reportDocument = new ReportDocument();
                reportDocument.Load(reportPath);

                DAReport clsRpt = new DAReport();
                clsRpt.ApplyTableLogOnInfo(ref reportDocument);
                clsRpt.SetSubreportLogOnInfo(ref reportDocument);

                reportDocument.SetParameterValue("@TrListId", TrListId);
                reportDocument.SetParameterValue("@ReqFn", (ReqFn.Length > 1 ? ReqFn.Substring(0, 1) : ReqFn));

                ExportOptions CrExportOptions;
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                CrDiskFileDestinationOptions.DiskFileName = pdfPath;
                CrExportOptions = reportDocument.ExportOptions;
                {
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                }

                reportDocument.Export();
                callPdf(pdfPopupPath, reportDocument);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alertNClose('Exception: " + ex.Message.Replace("'", "") + "');", true);
                return;
            }
        }

        private void ReportRequest()
        {

            try
            {
                string reportPath = Server.MapPath("../rpt/rptRequestPromotionItem.rpt");
                string pdfName = "RequestPromotionItem_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string pdfPath = Server.MapPath("../Report/" + pdfName + ".xls");
                string pdfPopupPath = "../Report/" + pdfName + ".xls";

                ReportDocument reportDocument = new ReportDocument();
                reportDocument.Load(reportPath);

                DAReport clsRpt = new DAReport();
                clsRpt.ApplyTableLogOnInfo(ref reportDocument);
                clsRpt.SetSubreportLogOnInfo(ref reportDocument);
                // reportDocument.SetDatabaseLogon(clsRpt.getUserID(), clsRpt.getPassword(), clsRpt.getDataSource(), clsRpt.getInitialCatalog());//("", "", "ADMIN-PC\\ADMIN", "dbRMC"); 

                reportDocument.SetParameterValue("@DateBeg", (DateTime)Session["DateBeg"]);
                reportDocument.SetParameterValue("@DateEnd", (DateTime)Session["DateEnd"]);
                reportDocument.SetParameterValue("@ReqType", Session["ReqType"] + "");
                reportDocument.SetParameterValue("@MasterItemGroup", Session["MasterItemGroup"] + "");
                reportDocument.SetParameterValue("ReqTypeText", Session["ReqTypeText"] + "");
                reportDocument.SetParameterValue("MasterItemGroupText", Session["MasterItemGroupText"] + "");
                // reportDocument.SetParameterValue("@project_Id", project_Id);

                ExportOptions CrExportOptions;
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                //PdfRtfWordFormatOptions CrFormatTypeOptions = new ExcelFormatOptions();
                CrDiskFileDestinationOptions.DiskFileName = pdfPath; // "C:\\SampleReport.pdf";
                CrExportOptions = reportDocument.ExportOptions;
                {
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                    //CrExportOptions.FormatOptions = CrFormatTypeOptions;
                }
                reportDocument.Export();
                callPdf(pdfPopupPath, reportDocument);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alertNClose('Exception: " + ex.Message.Replace("'", "") + "');", true);
                return;
            }

            Session["DateBeg"] = null;
            Session["DateEnd"] = null;
            Session["ReqType"] = null;
            Session["MasterItemGroup"] = null;
            Session["ReqTypeText"] = null;
            Session["MasterItemGroupText"] = null;
        }

        private void ReportBalance()
        {

            try
            {
                string reportPath = Server.MapPath("../rpt/rptStockBalance.rpt");
                string pdfName = "StockBalance_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string pdfPath = Server.MapPath("../Report/" + pdfName + ".xls");
                string pdfPopupPath = "../Report/" + pdfName + ".xls";

                ReportDocument reportDocument = new ReportDocument();
                reportDocument.Load(reportPath);

                DAReport clsRpt = new DAReport();
                clsRpt.ApplyTableLogOnInfo(ref reportDocument);
                clsRpt.SetSubreportLogOnInfo(ref reportDocument);
                // reportDocument.SetDatabaseLogon(clsRpt.getUserID(), clsRpt.getPassword(), clsRpt.getDataSource(), clsRpt.getInitialCatalog());//("", "", "ADMIN-PC\\ADMIN", "dbRMC"); 

                reportDocument.SetParameterValue("@DateBeg", (DateTime)Session["DateBeg"]);
                reportDocument.SetParameterValue("@DateEnd", (DateTime)Session["DateEnd"]);
                //reportDocument.SetParameterValue("@ItemStockList", Session["StockType"] + "");
                reportDocument.SetParameterValue("@MasterItemGroupIdList", Session["MasterItemGroup"] + "");
                //reportDocument.SetParameterValue("@ItemStockListText", Session["StockTypeText"] + "");
                reportDocument.SetParameterValue("@MasterItemGroupIdListText", Session["MasterItemGroupText"] + "");
                // reportDocument.SetParameterValue("@project_Id", project_Id);

                ExportOptions CrExportOptions;
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                //PdfRtfWordFormatOptions CrFormatTypeOptions = new ExcelFormatOptions();
                CrDiskFileDestinationOptions.DiskFileName = pdfPath; // "C:\\SampleReport.pdf";
                CrExportOptions = reportDocument.ExportOptions;
                {
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                    //CrExportOptions.FormatOptions = CrFormatTypeOptions;
                }
                reportDocument.Export();
                callPdf(pdfPopupPath, reportDocument);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alertNClose('Exception: " + ex.Message.Replace("'", "") + "');", true);
                return;
            }

            Session["DateBeg"] = null;
            Session["DateEnd"] = null;
            Session["ReqType"] = null;
            Session["MasterItemGroup"] = null;
            Session["ReqTypeText"] = null;
            Session["MasterItemGroupText"] = null;
        }

        private void ReportBalanceOnSite()
        {
            try
            {
                string reportPath = Server.MapPath("../rpt/rptStockBalanceOnSite.rpt");
                string pdfName = "StockBalanceOnSite_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string pdfPath = Server.MapPath("../Report/" + pdfName + ".xls");
                string pdfPopupPath = "../Report/" + pdfName + ".xls";

                ReportDocument reportDocument = new ReportDocument();
                reportDocument.Load(reportPath);

                DAReport clsRpt = new DAReport();
                clsRpt.ApplyTableLogOnInfo(ref reportDocument);
                clsRpt.SetSubreportLogOnInfo(ref reportDocument);
                // reportDocument.SetDatabaseLogon(clsRpt.getUserID(), clsRpt.getPassword(), clsRpt.getDataSource(), clsRpt.getInitialCatalog());//("", "", "ADMIN-PC\\ADMIN", "dbRMC"); 

                reportDocument.SetParameterValue("@ProjectID", Session["ProjectID"]);
                reportDocument.SetParameterValue("@UserID", Session["UserID"]);
                reportDocument.SetParameterValue("@sDate", Session["sDate"]);
                reportDocument.SetParameterValue("@MatGrpList", Session["MatGrpLst"]);
                reportDocument.SetParameterValue("MatGrpTxt", Session["MatGrpTxt"]);
                reportDocument.SetParameterValue("UserRespTxt", Session["UserRespTxt"]);
                reportDocument.SetParameterValue("ProjectTxt", Session["ProjectTxt"]);

                ExportOptions CrExportOptions;
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                //PdfRtfWordFormatOptions CrFormatTypeOptions = new ExcelFormatOptions();
                CrDiskFileDestinationOptions.DiskFileName = pdfPath; // "C:\\SampleReport.pdf";
                CrExportOptions = reportDocument.ExportOptions;
                {
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                    //CrExportOptions.FormatOptions = CrFormatTypeOptions;
                }
                reportDocument.Export();
                callPdf(pdfPopupPath, reportDocument);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alertNClose('Exception: " + ex.Message.Replace("'", "") + "');", true);
                return;
            }
            Session["ProjectID"] = null;
            Session["UserID"] = null;
            Session["sDate"] = null;
            Session["MatGrpLst"] = null;
            Session["UserRespTxt"] = null;
            Session["UserRespTxt"] = null;
            Session["ProjectTxt"] = null;
        }

        private void ReportPurchesingOrder()
        {
            try
            {
                string reportPath = Server.MapPath("../rpt/rptPurchesingOrder.rpt");
                string pdfName = "ReportPurchesingOrder_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string pdfPath = Server.MapPath("../Report/" + pdfName + ".pdf");
                string pdfPopupPath = "../Report/" + pdfName + ".pdf";

                ReportDocument reportDocument = new ReportDocument();
                reportDocument.Load(reportPath);

                DAReport clsRpt = new DAReport();
                clsRpt.ApplyTableLogOnInfo(ref reportDocument);
                clsRpt.SetSubreportLogOnInfo(ref reportDocument);

                reportDocument.SetParameterValue("@StartOrderDate", TryParse(Session["StartOrderDate"].ToString()));
                reportDocument.SetParameterValue("@EndOrderDate", TryParse(Session["EndOrderDate"].ToString()));
                reportDocument.SetParameterValue("@DOCNO", Session["DOCNO"].ToString());
                reportDocument.SetParameterValue("@REFNO", Session["REFNO"].ToString());
                reportDocument.SetParameterValue("@PROJECTID", Session["PROJECTID"].ToString());
                reportDocument.SetParameterValue("@MasterItemID", Session["MasterItemID"].ToString());


                ExportOptions CrExportOptions;
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                CrDiskFileDestinationOptions.DiskFileName = pdfPath; // "C:\\SampleReport.pdf";
                CrExportOptions = reportDocument.ExportOptions;
                {
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                }
                reportDocument.Export();
                callPdf(pdfPopupPath, reportDocument);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alertNClose('Exception: " + ex.Message.Replace("'", "") + "');", true);
                return;
            }
            Session["StartOrderDate"] = null;
            Session["EndOrderDate"] = null;
            Session["DOCNO"] = null;
            Session["REFNO"] = null;
            Session["PROJECTID"] = null;
            Session["MasterItemID"] = null;
        }

        private void ReportPurchesingOrder_New()
        {
            try
            {
                //string StartOrderDate = TryParse(Session["StartOrderDate"].ToString());
                //string EndOrderDate = TryParse(Session["EndOrderDate"].ToString());
                //string DOCNO = Session["DOCNO"].ToString();
                //string REFNO = Session["REFNO"].ToString();
                //string PROJECTID = Session["PROJECTID"].ToString();
                //string MasterItemID = Session["MasterItemID"].ToString();

                string StartOrderDate = "";
                string EndOrderDate = "";
                string DOCNO = "";
                string REFNO = "";
                string PROJECTID = "0";
                string MasterItemID = "0";

                if (Request.QueryString["StartOrderDate"] != null)
                    StartOrderDate = Request.QueryString["StartOrderDate"] + "";

                if (Request.QueryString["EndOrderDate"] != null)
                    EndOrderDate = Request.QueryString["EndOrderDate"] + "";

                if (Request.QueryString["DOCNO"] != null)
                    DOCNO = Request.QueryString["DOCNO"] + "";

                if (Request.QueryString["REFNO"] != null)
                    REFNO = Request.QueryString["REFNO"] + "";

                if (Request.QueryString["PROJECTID"] != null)
                    PROJECTID = Request.QueryString["PROJECTID"] + "";

                if (Request.QueryString["MasterItemID"] != null)
                    MasterItemID = Request.QueryString["MasterItemID"] + "";

                string reportPath = Server.MapPath("../rpt/rptPurchesingOrder_New.rpt");
                string pdfName = "ReportPurchesingOrder_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string pdfPath = Server.MapPath("../Report/" + pdfName + ".pdf");
                string pdfPopupPath = "../Report/" + pdfName + ".pdf";

                ReportDocument reportDocument = new ReportDocument();
                reportDocument.Load(reportPath);

                DAReport clsRpt = new DAReport();
                clsRpt.ApplyTableLogOnInfo(ref reportDocument);
                clsRpt.SetSubreportLogOnInfo(ref reportDocument);

                //reportDocument.SetParameterValue("@StartOrderDate", TryParse(Session["StartOrderDate"].ToString()));
                //reportDocument.SetParameterValue("@EndOrderDate", TryParse(Session["EndOrderDate"].ToString()));
                //reportDocument.SetParameterValue("@DOCNO", Session["DOCNO"].ToString());
                //reportDocument.SetParameterValue("@REFNO", Session["REFNO"].ToString());
                //reportDocument.SetParameterValue("@PROJECTID", Session["PROJECTID"].ToString());
                //reportDocument.SetParameterValue("@MasterItemID", Session["MasterItemID"].ToString());

                reportDocument.SetParameterValue("@StartOrderDate", StartOrderDate);
                reportDocument.SetParameterValue("@EndOrderDate", EndOrderDate);
                reportDocument.SetParameterValue("@DOCNO", DOCNO);
                reportDocument.SetParameterValue("@REFNO", REFNO);
                reportDocument.SetParameterValue("@PROJECTID", PROJECTID);
                reportDocument.SetParameterValue("@MasterItemID", MasterItemID);
                reportDocument.SetParameterValue("@IsAurora", 0);

                ExportOptions CrExportOptions;
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                CrDiskFileDestinationOptions.DiskFileName = pdfPath; // "C:\\SampleReport.pdf";
                CrExportOptions = reportDocument.ExportOptions;
                {
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                }

                reportDocument.Export();
                callPdf(pdfPopupPath, reportDocument);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alertNClose('Exception: " + ex.Message.Replace("'", "") + "');", true);
                return;
            }

            Session["StartOrderDate"] = null;
            Session["EndOrderDate"] = null;
            Session["DOCNO"] = null;
            Session["REFNO"] = null;
            Session["PROJECTID"] = null;
            Session["MasterItemID"] = null;
        }

        private void ReportBalanceOnSiteFull()
        {
            try
            {
                string reportPath = Server.MapPath("../rpt/rptStockBalanceOnSiteFull.rpt");
                string pdfName = "StockBalanceOnSite_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string pdfPath = Server.MapPath("../Report/" + pdfName + ".xls");
                string pdfPopupPath = "../Report/" + pdfName + ".xls";

                ReportDocument reportDocument = new ReportDocument();
                reportDocument.Load(reportPath);

                DAReport clsRpt = new DAReport();
                clsRpt.ApplyTableLogOnInfo(ref reportDocument);
                clsRpt.SetSubreportLogOnInfo(ref reportDocument);
                // reportDocument.SetDatabaseLogon(clsRpt.getUserID(), clsRpt.getPassword(), clsRpt.getDataSource(), clsRpt.getInitialCatalog());//("", "", "ADMIN-PC\\ADMIN", "dbRMC"); 

                reportDocument.SetParameterValue("@ProjectID", Session["ProjectID"]);
                reportDocument.SetParameterValue("@UserID", Session["UserID"]);
                reportDocument.SetParameterValue("@sDate", Session["sDate"]);
                reportDocument.SetParameterValue("@MatGrpList", Session["MatGrpLst"]);
                reportDocument.SetParameterValue("MatGrpTxt", Session["MatGrpTxt"]);
                reportDocument.SetParameterValue("UserRespTxt", Session["UserRespTxt"]);
                reportDocument.SetParameterValue("ProjectTxt", Session["ProjectTxt"]);

                ExportOptions CrExportOptions;
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                //PdfRtfWordFormatOptions CrFormatTypeOptions = new ExcelFormatOptions();
                CrDiskFileDestinationOptions.DiskFileName = pdfPath; // "C:\\SampleReport.pdf";
                CrExportOptions = reportDocument.ExportOptions;
                {
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                    //CrExportOptions.FormatOptions = CrFormatTypeOptions;
                }
                reportDocument.Export();
                callPdf(pdfPopupPath, reportDocument);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alertNClose('Exception: " + ex.Message.Replace("'", "") + "');", true);
                return;
            }
            Session["ProjectID"] = null;
            Session["UserID"] = null;
            Session["sDate"] = null;
            Session["MatGrpLst"] = null;
            Session["UserRespTxt"] = null;
            Session["UserRespTxt"] = null;
            Session["ProjectTxt"] = null;
        }

        private void ReportBalanceForAccount()
        {
            try
            {
                string reportPath = Server.MapPath("../rpt/rptStockBalanceForAccount2.rpt");
                string pdfName = "StockBalanceForAccount_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string pdfPath = Server.MapPath("../Report/" + pdfName + ".pdf");
                string pdfPopupPath = "../Report/" + pdfName + ".pdf";

                ReportDocument reportDocument = new ReportDocument();
                reportDocument.Load(reportPath);

                DAReport clsRpt = new DAReport();
                clsRpt.ApplyTableLogOnInfo(ref reportDocument);
                clsRpt.SetSubreportLogOnInfo(ref reportDocument);
                // reportDocument.SetDatabaseLogon(clsRpt.getUserID(), clsRpt.getPassword(), clsRpt.getDataSource(), clsRpt.getInitialCatalog());//("", "", "ADMIN-PC\\ADMIN", "dbRMC"); 

                reportDocument.SetParameterValue("@sDate", Session["DateBeg"]);
                reportDocument.SetParameterValue("@MatGrpLst", Session["MatGrp"]);
                //reportDocument.SetParameterValue("MatGrpName", Session["MatGrpText"]);
                reportDocument.SetParameterValue("@CompanyCode", Session["CompanyCode"]);
                //reportDocument.SetParameterValue("CompanyName", Session["CompanyName"]);
                reportDocument.SetParameterValue("@isIncNoneProject", Session["isIChkNoneProject"]);

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
                callPdf(pdfPopupPath, reportDocument);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alertNClose('Exception: " + ex.Message.Replace("'", "") + "');", true);
                return;
            }
            Session["DateBeg"] = null;
            Session["MatGrp"] = null;
            Session["MasterItemGroupText"] = null;
            Session["CompanyCode"] = null;
            Session["CompanyName"] = null;
            Session["isIChkNoneProject"] = null;
        }

        private void ReportTransferToProject()
        {

            try
            {
                string reportPath = Server.MapPath("../rpt/rptTransferToProject2.rpt");
                string pdfName = "rptTransferToProject_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string pdfPath = Server.MapPath("../Report/" + pdfName + ".xls");
                string pdfPopupPath = "../Report/" + pdfName + ".xls";

                ReportDocument reportDocument = new ReportDocument();
                reportDocument.Load(reportPath);

                DAReport clsRpt = new DAReport();
                clsRpt.ApplyTableLogOnInfo(ref reportDocument);
                clsRpt.SetSubreportLogOnInfo(ref reportDocument);
                // reportDocument.SetDatabaseLogon(clsRpt.getUserID(), clsRpt.getPassword(), clsRpt.getDataSource(), clsRpt.getInitialCatalog());//("", "", "ADMIN-PC\\ADMIN", "dbRMC"); 

                reportDocument.SetParameterValue("@DateBeg", Session["DateBeg"]);
                reportDocument.SetParameterValue("@DateEnd", Session["DateEnd"]);
                reportDocument.SetParameterValue("@ProjectID", Session["ProjectID"]);
                reportDocument.SetParameterValue("@ReqType", Session["ReqType"]);
                reportDocument.SetParameterValue("@MatGrpList", Session["MatGrpLst"]);
                reportDocument.SetParameterValue("ReqTypeText", Session["ReqTypeText"]);
                reportDocument.SetParameterValue("MatGrpLstTxt", Session["MatGrpLstTxt"]);
                reportDocument.SetParameterValue("@ReqTrnStatus", Session["ReqTrnStatus"]);

                ExportOptions CrExportOptions;
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                //PdfRtfWordFormatOptions CrFormatTypeOptions = new ExcelFormatOptions();
                CrDiskFileDestinationOptions.DiskFileName = pdfPath; // "C:\\SampleReport.pdf";
                CrExportOptions = reportDocument.ExportOptions;
                {
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                    //CrExportOptions.FormatOptions = CrFormatTypeOptions;
                }
                reportDocument.Export();
                callPdf(pdfPopupPath, reportDocument);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alertNClose('Exception: " + ex.Message.Replace("'", "") + "');", true);
                return;
            }

            Session["DateBeg"] = null;
            Session["DateEnd"] = null;
            Session["ProjectID"] = null;
            Session["ReqType"] = null;
            Session["MatGrpLst"] = null;
            Session["ReqTypeText"] = null;
            Session["MatGrpLstTxt"] = null;
            Session["ReqTrnStatus"] = null;
        }

        private void ReportReceive()
        {
            try
            {
                string reportPath = Server.MapPath("../rpt/rptReceiveItem.rpt");
                string pdfName = "ReceiveItem_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string pdfPath = Server.MapPath("../Report/" + pdfName + ".xls");
                string pdfPopupPath = "../Report/" + pdfName + ".xls";

                ReportDocument reportDocument = new ReportDocument();
                reportDocument.Load(reportPath);

                DAReport clsRpt = new DAReport();
                clsRpt.ApplyTableLogOnInfo(ref reportDocument);
                clsRpt.SetSubreportLogOnInfo(ref reportDocument);
                // reportDocument.SetDatabaseLogon(clsRpt.getUserID(), clsRpt.getPassword(), clsRpt.getDataSource(), clsRpt.getInitialCatalog());//("", "", "ADMIN-PC\\ADMIN", "dbRMC"); 

                reportDocument.SetParameterValue("@DateBeg", (DateTime)Session["DateBeg"]);
                reportDocument.SetParameterValue("@DateEnd", (DateTime)Session["DateEnd"]);
                // reportDocument.SetParameterValue("@project_Id", project_Id);

                ExportOptions CrExportOptions;
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                //PdfRtfWordFormatOptions CrFormatTypeOptions = new ExcelFormatOptions();
                CrDiskFileDestinationOptions.DiskFileName = pdfPath; // "C:\\SampleReport.pdf";
                CrExportOptions = reportDocument.ExportOptions;
                {
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                    //CrExportOptions.FormatOptions = CrFormatTypeOptions;
                }
                reportDocument.Export();
                callPdf(pdfPopupPath, reportDocument);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alertNClose('Exception: " + ex.Message.Replace("'", "") + "');", true);
                return;
            }

            Session["DateBeg"] = null;
            Session["DateEnd"] = null;
        }

        private void exReport()
        {
            try
            {
                ReportDocument reportDocument = new ReportDocument();
                // reportDocument.Load(reportPath);


                ExportOptions CrExportOptions;
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                PdfRtfWordFormatOptions CrFormatTypeOptions = new PdfRtfWordFormatOptions();
                CrDiskFileDestinationOptions.DiskFileName = "C:\\SampleReport.pdf";
                CrExportOptions = reportDocument.ExportOptions;
                {
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                    CrExportOptions.FormatOptions = CrFormatTypeOptions;
                }

                // reportDocument.SetParameterValue("@id", QuotationID);

                reportDocument.Export();
            }
            catch (Exception)
            {
                //MessageBox.Show(ex.ToString());
            }
        }

        private void callPdf(string pdfPath, ReportDocument rpt)
        {
            try
            {
                if (rpt != null)
                {
                    rpt.Close();
                    rpt.Dispose();
                    GC.Collect();
                }
            }
            catch (Exception) { }
            ScriptManager.RegisterStartupScript(this, GetType(), "js", "Popup80('" + pdfPath.Replace("\\", "\\\\") + "'); window.close();", true);

            return;
        }

        public static string TryParse(string text)
        {
            DateTime date;
            if (DateTime.TryParseExact(text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                return date.ToShortDateString();
                //date.ToString("yyyy-MM-dd");
            }
            else
            {
                return null;
            }
        }

        private void ReportSAPFB03IO()
        {
            AutorizeData auth = new AutorizeData();
            auth = (AutorizeData)Session["userInfo_" + Session.SessionID] ?? new AutorizeData();
            authEmployeeID = auth?.EmployeeID;
            authDisplayName = auth?.DisplayName;

            try
            {
                string reportPath = Server.MapPath("../rpt/rptSAPFB03_IO.rpt");
                string xmlPath = Server.MapPath("../rpt/PstAccIO.xml");
                string pdfName = "ReportSAPFB03IO_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string pdfPath = Server.MapPath("../Report/" + pdfName + ".pdf");
                string pdfPopupPath = "../Report/" + pdfName + ".pdf";

                string SAPDOC = Session["SAPDOC"] + "";
                DataTable dt = DADelivery.GetDataDeliveryItemFromSAPDocNo(SAPDOC);
                DataSet ds = getDataIOPostAccounts(dt);
                ds.WriteXml(xmlPath, XmlWriteMode.WriteSchema);

                string sDate = DateTime.Now.ToString("dd/MM/yyyy");
                DateTime PSTDTE = (DateTime)dt.Rows[0]["PostAccDate"];
                convertDate.getStringFromDate(PSTDTE, ref sDate);

                string CMPCDE = dt.Rows[0]["CompanySAPCode"] + "";
                string REFTXT = dt.Rows[0]["Reference"] + "";
                string DOCNO = dt.Rows[0]["PostRet_KEY"] + "";

                if (ds == null)// || project_Id == ""
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alertNClose('Invalid!');", true);
                    return;
                }

                ReportDocument reportDocument = new ReportDocument();
                reportDocument.Load(reportPath);
                reportDocument.Refresh();

                reportDocument.SetDataSource(ds);
                reportDocument.SetParameterValue("CMPCDE", CMPCDE);
                reportDocument.SetParameterValue("PSTDTE", sDate);
                reportDocument.SetParameterValue("REFTXT", REFTXT);
                reportDocument.SetParameterValue("DOCNO", DOCNO.Substring(0, 10));

                reportDocument.SetParameterValue("DisplayName", authDisplayName);

                ExportOptions CrExportOptions;
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                CrDiskFileDestinationOptions.DiskFileName = pdfPath;
                CrExportOptions = reportDocument.ExportOptions;
                {
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                }
                reportDocument.Export();
                callPdf(pdfPopupPath, reportDocument);

                Session["SAPDOC"] = "";
                reportDocument.Close();
                reportDocument.Dispose();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alertNClose('Exception: " + ex.Message.Replace("'", "") + "');", true);
                return;
            }
        }

        private void ReportSAPFB03IO_Test(string SAPDOC)
        {
            try
            {
                string reportPath = Server.MapPath("../rpt/rptSAPFB03_IO_Test.rpt");
                //string xmlPath = Server.MapPath("../rpt/PstAccIO.xml");
                string pdfName = "ReportSAPFB03IO_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string pdfPath = Server.MapPath("../Report/" + pdfName + ".pdf");
                string pdfPopupPath = "../Report/" + pdfName + ".pdf";


                DataTable dt = DADelivery.GetDataDeliveryItemFromSAPDocNo(SAPDOC);
                DataSet ds = getDataIOPostAccounts(dt);
                //ds.WriteXml(xmlPath, XmlWriteMode.WriteSchema);

                string sDate = DateTime.Now.ToString("dd/MM/yyyy");
                DateTime PSTDTE = (DateTime)dt.Rows[0]["PostAccDate"];
                convertDate.getStringFromDate(PSTDTE, ref sDate);

                string CMPCDE = dt.Rows[0]["CompanySAPCode"] + "";
                string REFTXT = dt.Rows[0]["Reference"] + "";
                string DOCNO = dt.Rows[0]["PostRet_KEY"] + "";

                if (ds == null)// || project_Id == ""
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alertNClose('Invalid!');", true);
                    return;
                }

                ReportDocument reportDocument = new ReportDocument();
                reportDocument.Load(reportPath);
                reportDocument.Refresh();

                reportDocument.SetDataSource(ds.Tables[0]);
                reportDocument.SetParameterValue("CMPCDE", CMPCDE);
                reportDocument.SetParameterValue("PSTDTE", sDate);
                reportDocument.SetParameterValue("REFTXT", REFTXT);
                reportDocument.SetParameterValue("DOCNO", DOCNO.Substring(0, 10));

                ExportOptions CrExportOptions;
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                CrDiskFileDestinationOptions.DiskFileName = pdfPath;
                CrExportOptions = reportDocument.ExportOptions;
                {
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                }
                reportDocument.Export();
                callPdf(pdfPopupPath, reportDocument);

                reportDocument.Close();
                reportDocument.Dispose();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alertNClose('Exception: " + ex.Message.Replace("'", "") + "');", true);
                return;
            }
        }

        private void ReportSAPFB03IOCrs()
        {
            AutorizeData auth = new AutorizeData();
            auth = (AutorizeData)Session["userInfo_" + Session.SessionID] ?? new AutorizeData();
            authEmployeeID = auth?.EmployeeID;
            authDisplayName = auth?.DisplayName;

            try
            {
                string reportPath = Server.MapPath("../rpt/rptSAPFB03_IOCross.rpt");
                string pdfName = "ReportSAPFB03IO_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string pdfPath = Server.MapPath("../Report/" + pdfName + ".pdf");
                string pdfPopupPath = "../Report/" + pdfName + ".pdf";

                string SAPDOC = Session["SAPDOC"] + "";
                DataTable dt = DADelivery.GetDataDeliveryItemFromSAPDocNo(SAPDOC);
                DataSet ds = new DataSet();
                DataTable dt1 = getDataIOCrossAccountsPayable(dt); dt1.TableName = "dt1";
                DataTable dt2 = getDataIOCrossPostAccounts(dt); dt2.TableName = "dt2";
                DataTable dt3 = getDataIOCrossAccountsReceivable(dt); dt3.TableName = "dt3";
                ds.Tables.Add(dt1);
                ds.Tables.Add(dt2);
                ds.Tables.Add(dt3);

                DateTime PSTDTE = (DateTime)dt.Rows[0]["PostAccDate"];
                string REFTXT = dt.Rows[0]["Reference"] + "";
                string CMPCDE1 = dt.Rows[0]["CompanySAPCode"] + "";
                string CMPCDE2 = dt.Rows[0]["CompanySAPCode"] + "";
                string CMPCDE3 = "1000";
                string SAPDOC1 = dt.Rows[0]["PostRet_KEY"] + "";
                string SAPDOC2 = dt.Rows[0]["PostRet_KEY2"] + "";
                string SAPDOC3 = dt.Rows[0]["PostRet_KEY3"] + "";

                ReportDocument reportDocument = new ReportDocument();
                reportDocument.Load(reportPath);

                string sDate = DateTime.Now.ToString("dd/MM/yyyy");
                convertDate.getStringFromDate(PSTDTE, ref sDate);

                reportDocument.SetDataSource(ds);
                reportDocument.Refresh();
                reportDocument.SetParameterValue("PSTDTE", sDate);
                reportDocument.SetParameterValue("REFTXT", REFTXT);
                reportDocument.SetParameterValue("DOCNO1", SAPDOC1.Substring(0, 10));
                reportDocument.SetParameterValue("CMPCDE1", CMPCDE1);
                reportDocument.SetParameterValue("DOCNO2", SAPDOC2.Substring(0, 10));
                reportDocument.SetParameterValue("CMPCDE2", CMPCDE2);
                reportDocument.SetParameterValue("DOCNO3", SAPDOC3.Substring(0, 10));
                reportDocument.SetParameterValue("CMPCDE3", CMPCDE3);

                reportDocument.SetParameterValue("DisplayName", authDisplayName);

                ExportOptions CrExportOptions;
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                CrDiskFileDestinationOptions.DiskFileName = pdfPath;
                CrExportOptions = reportDocument.ExportOptions;
                {
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                }

                reportDocument.Export();
                callPdf(pdfPopupPath, reportDocument);

                Session["SAPDOC"] = "";
                reportDocument.Close();
                reportDocument.Dispose();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alertNClose('Exception: " + ex.Message.Replace("'", "") + "');", true);
                return;
            }
        }

        private void ReportSAPFB03IOCrs_Test(string SAPDOC)
        {
            try
            {
                string reportPath = Server.MapPath("../rpt/rptSAPFB03_IOCross.rpt");
                string pdfName = "ReportSAPFB03IO_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string pdfPath = Server.MapPath("../Report/" + pdfName + ".pdf");
                string pdfPopupPath = "../Report/" + pdfName + ".pdf";

                DataTable dt = DADelivery.GetDataDeliveryItemFromSAPDocNo(SAPDOC);
                DataSet ds = new DataSet();
                DataTable dt1 = getDataIOCrossAccountsPayable(dt); dt1.TableName = "dt1";
                DataTable dt2 = getDataIOCrossPostAccounts(dt); dt2.TableName = "dt2";
                DataTable dt3 = getDataIOCrossAccountsReceivable(dt); dt3.TableName = "dt3";
                ds.Tables.Add(dt1);
                ds.Tables.Add(dt2);
                ds.Tables.Add(dt3);

                DateTime PSTDTE = (DateTime)dt.Rows[0]["PostAccDate"];
                string REFTXT = dt.Rows[0]["Reference"] + "";
                string CMPCDE1 = dt.Rows[0]["CompanySAPCode"] + "";
                string CMPCDE2 = dt.Rows[0]["CompanySAPCode"] + "";
                string CMPCDE3 = "1000";
                string SAPDOC1 = dt.Rows[0]["PostRet_KEY"] + "";
                string SAPDOC2 = dt.Rows[0]["PostRet_KEY2"] + "";
                string SAPDOC3 = dt.Rows[0]["PostRet_KEY3"] + "";

                ReportDocument reportDocument = new ReportDocument();
                reportDocument.Load(reportPath);

                string sDate = DateTime.Now.ToString("dd/MM/yyyy");
                convertDate.getStringFromDate(PSTDTE, ref sDate);

                reportDocument.SetDataSource(ds);
                reportDocument.Refresh();
                reportDocument.SetParameterValue("PSTDTE", sDate);
                reportDocument.SetParameterValue("REFTXT", REFTXT);
                reportDocument.SetParameterValue("DOCNO1", SAPDOC1.Substring(0, 10));
                reportDocument.SetParameterValue("CMPCDE1", CMPCDE1);
                reportDocument.SetParameterValue("DOCNO2", SAPDOC2.Substring(0, 10));
                reportDocument.SetParameterValue("CMPCDE2", CMPCDE2);
                reportDocument.SetParameterValue("DOCNO3", SAPDOC3.Substring(0, 10));
                reportDocument.SetParameterValue("CMPCDE3", CMPCDE3);

                ExportOptions CrExportOptions;
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                CrDiskFileDestinationOptions.DiskFileName = pdfPath;
                CrExportOptions = reportDocument.ExportOptions;
                {
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                }

                reportDocument.Export();
                callPdf(pdfPopupPath, reportDocument);

                reportDocument.Close();
                reportDocument.Dispose();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alertNClose('Exception: " + ex.Message.Replace("'", "") + "');", true);
                return;
            }
        }

        private void ReportSAPFB03WBS()
        {
            AutorizeData auth = new AutorizeData();
            auth = (AutorizeData)Session["userInfo_" + Session.SessionID] ?? new AutorizeData();
            authEmployeeID = auth?.EmployeeID;
            authDisplayName = auth?.DisplayName;

            try
            {
                string reportPath = Server.MapPath("../rpt/rptSAPFB03_WBS.rpt");
                string pdfName = "ReportSAPFB03WBS_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string pdfPath = Server.MapPath("../Report/" + pdfName + ".pdf");
                string pdfPopupPath = "../Report/" + pdfName + ".pdf";

                string SAPDOC = Session["SAPDOC"] + "";
                DataTable dt = DADelivery.GetDataDeliveryItemFromSAPDocNo(SAPDOC);
                DataSet ds = getDataWBSPostAccounts(dt);

                string sDate = DateTime.Now.ToString("dd/MM/yyyy");
                DateTime PSTDTE = (DateTime)dt.Rows[0]["PostAccDate"];
                convertDate.getStringFromDate(PSTDTE, ref sDate);

                string CMPCDE = dt.Rows[0]["CompanySAPCode"] + "";
                string REFTXT = dt.Rows[0]["Reference"] + "";
                string DOCNO = dt.Rows[0]["PostRet_KEY"] + "";

                if (ds == null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alertNClose('Invalid!');", true);
                    return;
                }

                ReportDocument reportDocument = new ReportDocument();
                reportDocument.Load(reportPath);

                reportDocument.SetDataSource(ds);
                reportDocument.Refresh();
                reportDocument.SetParameterValue("CMPCDE", CMPCDE);
                reportDocument.SetParameterValue("PSTDTE", sDate);
                reportDocument.SetParameterValue("REFTXT", REFTXT);
                reportDocument.SetParameterValue("DOCNO", DOCNO.Substring(0, 10));
                //reportDocument.SetParameterValue("UpdateBy", REFTXT);

                reportDocument.SetParameterValue("DisplayName", authDisplayName);

                ExportOptions CrExportOptions;
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                CrDiskFileDestinationOptions.DiskFileName = pdfPath; // "C:\\SampleReport.pdf";
                CrExportOptions = reportDocument.ExportOptions;
                {
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                }
                reportDocument.Export();
                callPdf(pdfPopupPath, reportDocument);

                Session["SAPDOC"] = "";
                reportDocument.Close();
                reportDocument.Dispose();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alertNClose('Exception: " + ex.Message.Replace("'", "") + "');", true);
                return;
            }
        }

        private void ReportSAPFB03WBSCrs()
        {
            AutorizeData auth = new AutorizeData();
            auth = (AutorizeData)Session["userInfo_" + Session.SessionID] ?? new AutorizeData();
            authEmployeeID = auth?.EmployeeID;
            authDisplayName = auth?.DisplayName;

            try
            {
                string reportPath = Server.MapPath("../rpt/rptSAPFB03_WBSCross.rpt");
                string pdfName = "ReportSAPFB03WBSCrs_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string pdfPath = Server.MapPath("../Report/" + pdfName + ".pdf");
                string pdfPopupPath = "../Report/" + pdfName + ".pdf";

                string SAPDOC = Session["SAPDOC"] + "";
                DataTable dt = DADelivery.GetDataDeliveryItemFromSAPDocNo(SAPDOC);
                DataSet ds = new DataSet();
                DataTable dt1 = getDataWBSCrossAccountsPayable(dt); dt1.TableName = "dt1";
                DataTable dt2 = getDataWBSCrossPostAccounts(dt); dt2.TableName = "dt2";
                DataTable dt3 = getDataWBSCrossAccountsReceivable(dt); dt3.TableName = "dt3";
                ds.Tables.Add(dt1);
                ds.Tables.Add(dt2);
                ds.Tables.Add(dt3);

                DateTime PSTDTE = (DateTime)dt.Rows[0]["PostAccDate"];
                string REFTXT = dt.Rows[0]["Reference"] + "";
                string CMPCDE1 = dt.Rows[0]["CompanySAPCode"] + "";
                string CMPCDE2 = dt.Rows[0]["CompanySAPCode"] + "";
                string CMPCDE3 = "1000";
                string SAPDOC1 = dt.Rows[0]["PostRet_KEY"] + "";
                string SAPDOC2 = dt.Rows[0]["PostRet_KEY2"] + "";
                string SAPDOC3 = dt.Rows[0]["PostRet_KEY3"] + "";

                ReportDocument reportDocument = new ReportDocument();
                reportDocument.Load(reportPath);

                string sDate = DateTime.Now.ToString("dd/MM/yyyy");
                convertDate.getStringFromDate(PSTDTE, ref sDate);

                reportDocument.SetDataSource(ds);
                reportDocument.Refresh();
                reportDocument.SetParameterValue("PSTDTE", sDate);
                reportDocument.SetParameterValue("REFTXT", REFTXT);
                reportDocument.SetParameterValue("DOCNO1", SAPDOC1.Substring(0, 10));
                reportDocument.SetParameterValue("CMPCDE1", CMPCDE1);
                reportDocument.SetParameterValue("DOCNO2", SAPDOC2.Substring(0, 10));
                reportDocument.SetParameterValue("CMPCDE2", CMPCDE2);
                reportDocument.SetParameterValue("DOCNO3", SAPDOC3.Substring(0, 10));
                reportDocument.SetParameterValue("CMPCDE3", CMPCDE3);

                reportDocument.SetParameterValue("DisplayName", authDisplayName);

                ExportOptions CrExportOptions;
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                CrDiskFileDestinationOptions.DiskFileName = pdfPath; // "C:\\SampleReport.pdf";
                CrExportOptions = reportDocument.ExportOptions;
                {
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                }
                reportDocument.Export();
                callPdf(pdfPopupPath, reportDocument);

                Session["SAPDOC"] = null;
                reportDocument.Close();
                reportDocument.Dispose();

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alertNClose('Exception: " + ex.Message.Replace("'", "") + "');", true);
                return;
            }
        }

        #region

        /* = - WBS - = */
        private DataSet getDataWBSPostAccounts(DataTable dt)
        {
            DataTable PCRTHO = dt.DefaultView.ToTable(true, "CompanySAPCode", "ProfitCenterHO");

            string CompanySAPCode = PCRTHO.Rows[0]["CompanySAPCode"] + "";
            string ProfitHO = PCRTHO.Rows[0]["ProfitCenterHO"] + "";
            PCRTHO.Dispose();
            PCRTHO = null;

            DASAPConnector sap = new DASAPConnector();
            DataTable dtTotal = dt.DefaultView.ToTable(true, "WBS_SAP", "ItemName", "isNonePR");
            dtTotal.Columns.Add(new DataColumn("CostCenter", typeof(string)));
            dtTotal.Columns.Add(new DataColumn("GLNo", typeof(string)));
            dtTotal.Columns.Add(new DataColumn("GLName", typeof(string)));
            dtTotal.Columns.Add(new DataColumn("ProfitCenter", typeof(string)));
            dtTotal.Columns.Add(new DataColumn("Amount", typeof(int)));
            dtTotal.Columns.Add(new DataColumn("Total", typeof(decimal)));
            dtTotal.Columns.Add(new DataColumn("TotalCredit", typeof(decimal)));
            dtTotal.Columns.Add(new DataColumn("ItemText", typeof(string)));

            foreach (DataRow drTotal in dtTotal.Rows)
            {
                drTotal["GLNo"] = sap.getGLNoNormal();
                drTotal["GLName"] = sap.getGLNoNormalName();
                string cond = "ISNULL(WBS_SAP,'') = '" + drTotal["WBS_SAP"] + "' AND ISNULL(ItemName,'') = '" + drTotal["ItemName"] + "' AND ISNULL(isNonePR,'') = '" + drTotal["isNonePR"] + "'";
                drTotal["WBS_SAP"] = "" + drTotal["WBS_SAP"];
                drTotal["Total"] = dt.Compute("SUM(Price)", cond);
                string preTxt = ""; if (drTotal["isNonePR"] + "" == "Y") { preTxt = "O-"; }
                string ItemText = preTxt + "" + drTotal["ItemName"] + " " + (int)dt.Compute("COUNT(WBS_SAP)", cond);// + " " + DateTime.Now.ToString("MM.yyyy");
                drTotal["ItemText"] = ItemText;
            }
            DataTable dtx = dt.DefaultView.ToTable(true, "CostCenter");
            dtx.Columns.Add(new DataColumn("WBS_SAP", typeof(string)));
            dtx.Columns.Add(new DataColumn("GLNo", typeof(string)));
            dtx.Columns.Add(new DataColumn("GLName", typeof(string)));
            dtx.Columns.Add(new DataColumn("ProfitCenter", typeof(string)));
            dtx.Columns.Add(new DataColumn("Amount", typeof(int)));
            dtx.Columns.Add(new DataColumn("Total", typeof(decimal)));
            dtx.Columns.Add(new DataColumn("TotalCredit", typeof(decimal)));

            foreach (DataRow drTotal in dtx.Rows)
            {
                DataRow dr = dtTotal.NewRow();
                string cond = "ISNULL(CostCenter,'') = '" + drTotal["CostCenter"] + "'";
                dr["GLNo"] = sap.getGLNoCredit();
                dr["GLName"] = sap.getGLNoCreditName();
                dr["ProfitCenter"] = ProfitHO;
                dr["TotalCredit"] = (-1) * (decimal)dt.Compute("SUM(Price)", cond);
                dr["Total"] = DBNull.Value;
                dtTotal.Rows.Add(dr);
            }

            dtx.Dispose();
            dtx = null;
            dtTotal.AcceptChanges();

            DataSet ds = new DataSet();
            dtTotal.TableName = "WBS";
            ds.Tables.Add(dtTotal.Copy());
            return ds;
        }
        /* = / WBS / = */

        /* = - WBS Cross Company - = */
        private DataTable getDataWBSCrossAccountsPayable(DataTable dt_A)
        {
            DASAPConnector sap = new DASAPConnector();
            dt_A.DefaultView.RowFilter = "";
            string postingDate = "";
            DateTime dtx = new DateTime();
            if (DateTime.TryParse(dt_A.Rows[0]["PostAccDate"] + "", out dtx))
            {
                postingDate = dtx.ToString("MM.yyyy");
            }
            DataTable dtAP = dt_A.DefaultView.ToTable(true, "ItemNo", "ItemName", "CostCenter", "ProfitCenterHO");

            DataTable dt = dtAP.Clone();
            try
            {
                // string x = new DADelivery().lookTable(dt);
                dt.Columns.Add(new DataColumn("GLNo", typeof(string)));
                dt.Columns.Add(new DataColumn("GLName", typeof(string)));
                dt.Columns.Add(new DataColumn("profitCenter", typeof(string)));
                dt.Columns.Add(new DataColumn("debit", typeof(decimal)));
                dt.Columns.Add(new DataColumn("credit", typeof(decimal)));
                dt.Columns.Add(new DataColumn("itemtext", typeof(string)));

                foreach (DataRow drAP in dtAP.Rows)
                {
                    string cond = "ItemNo = '" + drAP["ItemNo"] + "' AND ItemName = '" + drAP["ItemName"] + "' AND CostCenter = '" + drAP["CostCenter"] + "' AND ProfitCenterHO = '" + drAP["ProfitCenterHO"] + "'";
                    int itmcnt = (int)dt_A.Compute("Count(ItemId)", cond);
                    decimal itmtotal = (decimal)dt_A.Compute("SUM(Price)", cond);
                    DataRow dr = dt.NewRow();
                    dr["ItemNo"] = drAP["ItemNo"];
                    dr["ItemName"] = drAP["ItemName"];
                    dr["CostCenter"] = drAP["CostCenter"];
                    dr["ProfitCenterHO"] = drAP["ProfitCenterHO"];
                    dr["GLNo"] = sap.getGLNoCredit();
                    dr["GLName"] = sap.getGLNoCreditName();
                    dr["profitCenter"] = dr["ProfitCenterHO"];
                    dr["debit"] = itmtotal;
                    // dr["credit"] = "";
                    dr["itemtext"] = dr["ItemName"] + " " + itmcnt;// + " " + postingDate;
                    dt.Rows.Add(dr);
                }

                DataTable dtAPCr = dt_A.DefaultView.ToTable(true, "CostCenter", "ProfitCenterHO");
                foreach (DataRow drAP in dtAPCr.Rows)
                {
                    string cond = "CostCenter = '" + drAP["CostCenter"] + "' AND ProfitCenterHO = '" + drAP["ProfitCenterHO"] + "'";
                    int itmcnt = (int)dt_A.Compute("Count(ItemId)", cond);
                    decimal itmtotal = (decimal)dt_A.Compute("SUM(Price)", cond);
                    DataRow dr = dt.NewRow();
                    dr["ItemNo"] = "";// drAP["ItemNo"];
                    dr["ItemName"] = "";//drAP["ItemName"];
                    dr["CostCenter"] = drAP["CostCenter"];
                    dr["ProfitCenterHO"] = drAP["ProfitCenterHO"];
                    dr["GLNo"] = sap.getGLNoAPVendor();
                    dr["GLName"] = sap.getGLNoAPVendorName();
                    dr["profitCenter"] = dr["ProfitCenterHO"];
                    // dr["debit"] = "";
                    dr["credit"] = ((-1) * itmtotal);//itmtotal;
                    dr["itemtext"] = "";//dr["ItemName"] + " " + itmcnt;
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception)
            {
                return null;
            }
            return dt;
        }

        private DataTable getDataWBSCrossPostAccounts(DataTable dt_A)
        {
            // ProfitCenterHO
            DASAPConnector sap = new DASAPConnector();
            dt_A.DefaultView.RowFilter = "";
            string postingDate = "";
            DateTime dtr = new DateTime();
            if (DateTime.TryParse(dt_A.Rows[0]["PostAccDate"] + "", out dtr))
            {
                postingDate = dtr.ToString("MM.yyyy");
            }
            DataTable dt = dt_A.DefaultView.ToTable(true, "WBS_SAP", "ProfitCenterHO", "ItemName", "isNonePR");
            string ProfitCenterHO = dt.Rows[0]["ProfitCenterHO"] + "";
            try
            {
                dt.Columns.Add(new DataColumn("CostCenter", typeof(string)));
                dt.Columns.Add(new DataColumn("GLNo", typeof(string)));
                dt.Columns.Add(new DataColumn("GLName", typeof(string)));
                dt.Columns.Add(new DataColumn("ProfitCenter", typeof(string)));
                dt.Columns.Add(new DataColumn("Amount", typeof(int)));
                dt.Columns.Add(new DataColumn("Debit", typeof(decimal)));
                dt.Columns.Add(new DataColumn("Credit", typeof(decimal)));
                dt.Columns.Add(new DataColumn("ItemText", typeof(string)));

                foreach (DataRow drTotal in dt.Rows)
                {
                    drTotal["GLNo"] = sap.getGLNoNormal();
                    drTotal["GLName"] = sap.getGLNoNormalName();
                    string cond = "ISNULL(WBS_SAP,'') = '" + drTotal["WBS_SAP"] + "' AND ISNULL(ItemName,'') = '" + drTotal["ItemName"] + "' AND ISNULL(isNonePR,'') = '" + drTotal["isNonePR"] + "'";
                    int itmcnt = (int)dt_A.Compute("Count(ItemId)", cond);
                    //drTotal["Amount"] = dt.Compute("COUNT(ItemId)", cond);
                    drTotal["WBS_SAP"] = "" + drTotal["WBS_SAP"];
                    //drTotal["CostCenter"] = "P" + drTotal["CostCenter"];
                    //drTotal["ItemName"] = dt.Compute("COUNT(WBS_SAP)", cond);
                    drTotal["Debit"] = (decimal)dt_A.Compute("SUM(Price)", cond);
                    string preTxt = ""; if (drTotal["isNonePR"] + "" == "Y") { preTxt = "O-"; }
                    string ItemText = preTxt + " " + drTotal["ItemName"] + " " + itmcnt; //(int)dt.Compute("COUNT(WBS_SAP)", cond);
                    drTotal["ItemText"] = ItemText;// +" " + postingDate;
                }
                DataTable dtx = dt_A.DefaultView.ToTable(true, "CostCenter");
                dtx.Columns.Add(new DataColumn("WBS_SAP", typeof(string)));
                dtx.Columns.Add(new DataColumn("GLNo", typeof(string)));
                dtx.Columns.Add(new DataColumn("GLName", typeof(string)));
                dtx.Columns.Add(new DataColumn("ProfitCenter", typeof(string)));
                dtx.Columns.Add(new DataColumn("Amount", typeof(int)));
                dtx.Columns.Add(new DataColumn("Debit", typeof(decimal)));
                dtx.Columns.Add(new DataColumn("Credit", typeof(decimal)));

                foreach (DataRow drTotal in dtx.Rows)
                {
                    DataRow dr = dt.NewRow();
                    string cond = "ISNULL(CostCenter,'') = '" + drTotal["CostCenter"] + "'";
                    dr["GLNo"] = sap.getGLNoCredit();
                    dr["GLName"] = sap.getGLNoCreditName();
                    dr["ProfitCenter"] = ProfitCenterHO;//  hdfProfitHO.Value;
                    dr["Credit"] = (-1) * (decimal)dt_A.Compute("SUM(Price)", cond);
                    // dr["Debit"] = DBNull.Value;
                    dt.Rows.Add(dr);
                }
                dtx.Dispose(); dtx = null;
                dt.AcceptChanges();
            }
            catch (Exception)
            {
                return null;
            }
            return dt;
        }

        private DataTable getDataWBSCrossAccountsReceivable(DataTable dt_A)
        {
            DASAPConnector sap = new DASAPConnector();
            dt_A.DefaultView.RowFilter = "";
            string postingDate = "";
            DateTime dtx = new DateTime();
            if (DateTime.TryParse(dt_A.Rows[0]["PostAccDate"] + "", out dtx))
            {
                postingDate = dtx.ToString("MM.yyyy");
            }
            DataTable dtAP = dt_A.DefaultView.ToTable(true, "CompanySAPCode", "ItemNo", "ItemName");
            // CompanySAPCode,  getGLCompName,  'P11000',   Sum(),  itmTxt
            DataTable dt = dtAP.Clone();
            try
            {
                // string x = new DADelivery().lookTable(dt);
                dt.Columns.Add(new DataColumn("GLNo", typeof(string)));
                dt.Columns.Add(new DataColumn("GLName", typeof(string)));
                dt.Columns.Add(new DataColumn("profitCenter", typeof(string)));
                dt.Columns.Add(new DataColumn("debit", typeof(decimal)));
                dt.Columns.Add(new DataColumn("credit", typeof(decimal)));
                dt.Columns.Add(new DataColumn("itemtext", typeof(string)));

                DataTable dtAPDb = dt_A.DefaultView.ToTable(true, "CompanySAPCode");// , "ItemNo", "ItemName"
                foreach (DataRow drAP in dtAPDb.Rows)
                {
                    string cond = "CompanySAPCode = '" + drAP["CompanySAPCode"] + "'";// AND ItemName = '" + drAP["ItemName"] + "' AND ItemNo = '" + drAP["ItemNo"] + "'";
                    int itmcnt = (int)dt_A.Compute("Count(ItemId)", cond);
                    decimal itmtotal = (decimal)dt_A.Compute("SUM(Price)", cond);
                    DataRow dr = dt.NewRow();
                    dr["CompanySAPCode"] = drAP["CompanySAPCode"];
                    //dr["ItemNo"] = drAP["ItemNo"];
                    //dr["ItemName"] = drAP["ItemName"];

                    dr["GLNo"] = "000000" + drAP["CompanySAPCode"];
                    dr["GLName"] = sap.getSAPGLCompany(drAP["CompanySAPCode"] + "");
                    dr["profitCenter"] = "P11000";
                    dr["debit"] = itmtotal;
                    // dr["credit"] = "";
                    //dr["itemtext"] = dr["ItemName"] + " " + itmcnt;
                    dt.Rows.Add(dr);
                }

                foreach (DataRow drAP in dtAP.Rows)
                {
                    string cond = "CompanySAPCode = '" + drAP["CompanySAPCode"] + "' AND ItemName = '" + drAP["ItemName"] + "' AND ItemNo = '" + drAP["ItemNo"] + "'";
                    int itmcnt = (int)dt_A.Compute("Count(ItemId)", cond);
                    decimal itmtotal = (decimal)dt_A.Compute("SUM(Price)", cond);
                    DataRow dr = dt.NewRow();
                    dr["CompanySAPCode"] = drAP["CompanySAPCode"];
                    dr["ItemNo"] = drAP["ItemNo"];
                    dr["ItemName"] = drAP["ItemName"];

                    dr["GLNo"] = sap.getGLNoCredit();
                    dr["GLName"] = sap.getGLNoCreditName();
                    dr["profitCenter"] = "P11000";
                    // dr["debit"] = itmtotal;
                    dr["credit"] = (-1) * itmtotal;
                    dr["itemtext"] = dr["ItemName"] + " " + itmcnt;// + " " + postingDate;
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception)
            {
                return null;
            }
            return dt;
        }
        /* = / WBS Cross Company / = */

        /* = - IO - = */
        private DataSet getDataIOPostAccounts(DataTable dt)
        {
            DataTable PCRTHO = dt.DefaultView.ToTable(true, "CompanySAPCode", "ProfitCenterHO");

            string CompanySAPCode = PCRTHO.Rows[0]["CompanySAPCode"] + "";
            string ProfitHO = PCRTHO.Rows[0]["ProfitCenterHO"] + "";
            PCRTHO.Dispose();
            PCRTHO = null;

            DASAPConnector sap = new DASAPConnector();
            DataTable dtTotal = dt.DefaultView.ToTable(true, "CostCenter", "ItemName", "isNonePR");
            dtTotal.Columns.Add(new DataColumn("GLNo", typeof(string)));
            dtTotal.Columns.Add(new DataColumn("GLName", typeof(string)));
            dtTotal.Columns.Add(new DataColumn("ProfitCenter", typeof(string)));
            dtTotal.Columns.Add(new DataColumn("Amount", typeof(int)));
            dtTotal.Columns.Add(new DataColumn("Total", typeof(decimal)));
            dtTotal.Columns.Add(new DataColumn("TotalCredit", typeof(decimal)));
            dtTotal.Columns.Add(new DataColumn("ItemText", typeof(string)));

            foreach (DataRow drTotal in dtTotal.Rows)
            {
                drTotal["GLNo"] = sap.getGLNo300Cus();
                drTotal["GLName"] = sap.getGLNo300CusName();
                string cond = "ISNULL(CostCenter,'') = '" + drTotal["CostCenter"] + "' AND ISNULL(ItemName,'') = '" + drTotal["ItemName"] + "' AND ISNULL(isNonePR,'') = '" + drTotal["isNonePR"] + "'";
                drTotal["CostCenter"] = drTotal["CostCenter"] + "";
                drTotal["Total"] = dt.Compute("SUM(Price)", cond);
                string preTxt = "";
                if (drTotal["isNonePR"] + "" == "Y") { preTxt = "O-"; }
                drTotal["ItemText"] = preTxt + drTotal["ItemName"] + " " + dt.Compute("COUNT(ItemName)", cond);
            }

            DataRow dr = dtTotal.NewRow();
            dr["GLNo"] = sap.getGLNoCredit();
            dr["GLName"] = sap.getGLNoCreditName();
            dr["ProfitCenter"] = ProfitHO;
            dr["TotalCredit"] = (-1) * (decimal)dt.Compute("SUM(Price)", "");
            dr["Total"] = DBNull.Value;
            dtTotal.Rows.Add(dr);

            dtTotal.AcceptChanges();

            DataSet ds = new DataSet();
            dtTotal.TableName = "IO";
            foreach (DataTable dtx in ds.Tables)
            {
                ds.Tables.Remove(dtx);
            }
            ds.AcceptChanges();
            ds.Tables.Add(dtTotal.Copy());
            return ds;
        }
        /* = / IO / = */

        /* = - IO Cross Company - = */
        private DataTable getDataIOCrossAccountsPayable(DataTable dt_A)
        {
            DASAPConnector sap = new DASAPConnector();
            dt_A.DefaultView.RowFilter = "";
            string postingDate = "";
            DateTime dtx = new DateTime();
            if (DateTime.TryParse(dt_A.Rows[0]["PostAccDate"] + "", out dtx))
            {
                postingDate = dtx.ToString("MM.yyyy");
            }
            DataTable dtAP = dt_A.DefaultView.ToTable(true, "ItemNo", "ItemName", "CostCenter", "ProfitCenterHO");

            DataTable dt = dtAP.Clone();
            try
            {
                dt.Columns.Add(new DataColumn("GLNo", typeof(string)));
                dt.Columns.Add(new DataColumn("GLName", typeof(string)));
                dt.Columns.Add(new DataColumn("profitCenter", typeof(string)));
                dt.Columns.Add(new DataColumn("debit", typeof(decimal)));
                dt.Columns.Add(new DataColumn("credit", typeof(decimal)));
                dt.Columns.Add(new DataColumn("itemtext", typeof(string)));

                foreach (DataRow drAP in dtAP.Rows)
                {
                    string cond = "ItemNo = '" + drAP["ItemNo"] + "' AND ItemName = '" + drAP["ItemName"] + "' AND CostCenter = '" + drAP["CostCenter"] + "' AND ProfitCenterHO = '" + drAP["ProfitCenterHO"] + "'";
                    int itmcnt = (int)dt_A.Compute("Count(ItemId)", cond);
                    decimal itmtotal = (decimal)dt_A.Compute("SUM(Price)", cond);
                    DataRow dr = dt.NewRow();
                    dr["ItemNo"] = drAP["ItemNo"];
                    dr["ItemName"] = drAP["ItemName"];
                    dr["CostCenter"] = drAP["CostCenter"];
                    dr["ProfitCenterHO"] = drAP["ProfitCenterHO"];
                    dr["GLNo"] = sap.getGLNoCredit();
                    dr["GLName"] = sap.getGLNoCreditName();
                    dr["profitCenter"] = dr["ProfitCenterHO"];
                    dr["debit"] = itmtotal;
                    dr["itemtext"] = dr["ItemName"] + " " + itmcnt;
                    dt.Rows.Add(dr);
                }

                DataTable dtAPCr = dt_A.DefaultView.ToTable(true, "ProfitCenterHO");
                foreach (DataRow drAP in dtAPCr.Rows)
                {
                    string cond = "ProfitCenterHO = '" + drAP["ProfitCenterHO"] + "'";
                    int itmcnt = (int)dt_A.Compute("Count(ItemId)", cond);
                    decimal itmtotal = (decimal)dt_A.Compute("SUM(Price)", cond);
                    DataRow dr = dt.NewRow();
                    dr["ProfitCenterHO"] = drAP["ProfitCenterHO"];
                    dr["GLNo"] = sap.getGLNoAPVendor();
                    dr["GLName"] = sap.getGLNoAPVendorName();
                    dr["profitCenter"] = dr["ProfitCenterHO"];
                    dr["credit"] = (-1) * itmtotal;
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception)
            {
                return null;
            }


            return dt;
        }

        private DataTable getDataIOCrossPostAccounts(DataTable dt_A)
        {
            DASAPConnector sap = new DASAPConnector();
            dt_A.DefaultView.RowFilter = "";
            string postingDate = "";
            DateTime dtx = new DateTime();
            if (DateTime.TryParse(dt_A.Rows[0]["PostAccDate"] + "", out dtx))
            {
                postingDate = dtx.ToString("MM.yyyy");
            }
            DataTable dtAP = dt_A.DefaultView.ToTable(true, "ItemNo", "ItemName", "CostCenter", "ProfitCenterHO");
            DataTable dtAPCr = dt_A.DefaultView.ToTable(true, "CostCenter", "ProfitCenterHO");

            DataTable dt = dtAP.Clone();
            try
            {
                // string x = new DADelivery().lookTable(dt);
                dt.Columns.Add(new DataColumn("GLNo", typeof(string)));
                dt.Columns.Add(new DataColumn("GLName", typeof(string)));
                dt.Columns.Add(new DataColumn("InternalOrder", typeof(string)));
                dt.Columns.Add(new DataColumn("ProfitCenter", typeof(string)));
                dt.Columns.Add(new DataColumn("debit", typeof(decimal)));
                dt.Columns.Add(new DataColumn("credit", typeof(decimal)));
                dt.Columns.Add(new DataColumn("itemtext", typeof(string)));

                foreach (DataRow drAP in dtAP.Rows)
                {
                    string cond = "ItemNo = '" + drAP["ItemNo"] + "' AND ItemName = '" + drAP["ItemName"] + "' AND CostCenter = '" + drAP["CostCenter"] + "' AND ProfitCenterHO = '" + drAP["ProfitCenterHO"] + "'";
                    int itmcnt = (int)dt_A.Compute("Count(ItemId)", cond);
                    decimal itmtotal = (decimal)dt_A.Compute("SUM(Price)", cond);
                    DataRow dr = dt.NewRow();
                    dr["GLNo"] = sap.getGLNo300Cus();
                    dr["GLName"] = sap.getGLNo300CusName();
                    dr["ItemNo"] = drAP["ItemNo"];
                    dr["ItemName"] = drAP["ItemName"];
                    dr["InternalOrder"] = drAP["CostCenter"];
                    // dr["ProfitCenterHO"] = drAP["ProfitCenterHO"];
                    dr["debit"] = itmtotal;
                    // dr["credit"] = "";
                    dr["itemtext"] = dr["ItemName"] + " " + itmcnt;// + " " + postingDate;
                    dt.Rows.Add(dr);
                }

                foreach (DataRow drAP in dtAPCr.Rows)
                {
                    string cond = "CostCenter = '" + drAP["CostCenter"] + "' AND ProfitCenterHO = '" + drAP["ProfitCenterHO"] + "'";
                    int itmcnt = (int)dt_A.Compute("Count(ItemId)", cond);
                    decimal itmtotal = (decimal)dt_A.Compute("SUM(Price)", cond);
                    DataRow dr = dt.NewRow();
                    dr["GLNo"] = sap.getGLNoCredit();
                    dr["GLName"] = sap.getGLNoCreditName();
                    //dr["ItemNo"] = drAP["ItemNo"];
                    //dr["ItemName"] = drAP["ItemName"];
                    // dr["InternalOrder"] = drAP["CostCenter"];
                    dr["ProfitCenterHO"] = drAP["ProfitCenterHO"];
                    // dr["debit"] = itmtotal;
                    dr["credit"] = (-1) * itmtotal;
                    //dr["itemtext"] = dr["ItemName"] + " " + itmcnt;
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception)
            {
                return null;
            }


            return dt;
        }

        private DataTable getDataIOCrossAccountsReceivable(DataTable dt_A)
        {
            DASAPConnector sap = new DASAPConnector();
            dt_A.DefaultView.RowFilter = "";
            string postingDate = "";
            DateTime dtx = new DateTime();
            if (DateTime.TryParse(dt_A.Rows[0]["PostAccDate"] + "", out dtx))
            {
                postingDate = dtx.ToString("MM.yyyy");
            }
            DataTable dtAP = dt_A.DefaultView.ToTable(true, "CompanySAPCode", "ItemNo", "ItemName");
            // CompanySAPCode,  getGLCompName,  'P11000',   Sum(),  itmTxt
            DataTable dt = dtAP.Clone();
            try
            {
                // string x = new DADelivery().lookTable(dt);
                dt.Columns.Add(new DataColumn("GLNo", typeof(string)));
                dt.Columns.Add(new DataColumn("GLName", typeof(string)));
                dt.Columns.Add(new DataColumn("profitCenter", typeof(string)));
                dt.Columns.Add(new DataColumn("debit", typeof(decimal)));
                dt.Columns.Add(new DataColumn("credit", typeof(decimal)));
                dt.Columns.Add(new DataColumn("itemtext", typeof(string)));

                DataTable dtAPDB = dt_A.DefaultView.ToTable(true, "CompanySAPCode");
                foreach (DataRow drAP in dtAPDB.Rows)
                {
                    string cond = "CompanySAPCode = '" + drAP["CompanySAPCode"] + "'";
                    int itmcnt = (int)dt_A.Compute("Count(ItemId)", cond);
                    decimal itmtotal = (decimal)dt_A.Compute("SUM(Price)", cond);
                    DataRow dr = dt.NewRow();
                    dr["CompanySAPCode"] = drAP["CompanySAPCode"];
                    //dr["ItemNo"] = drAP["ItemNo"];
                    //dr["ItemName"] = drAP["ItemName"];

                    dr["GLNo"] = "000000" + drAP["CompanySAPCode"];
                    dr["GLName"] = sap.getSAPGLCompany(drAP["CompanySAPCode"] + "");
                    dr["profitCenter"] = "P11000";
                    dr["debit"] = itmtotal;
                    //// dr["credit"] = "";
                    //dr["itemtext"] = dr["ItemName"] + " " + itmcnt;
                    dt.Rows.Add(dr);
                }

                foreach (DataRow drAP in dtAP.Rows)
                {
                    string cond = "CompanySAPCode = '" + drAP["CompanySAPCode"] + "' AND ItemName = '" + drAP["ItemName"] + "' AND ItemNo = '" + drAP["ItemNo"] + "'";
                    int itmcnt = (int)dt_A.Compute("Count(ItemId)", cond);
                    decimal itmtotal = (decimal)dt_A.Compute("SUM(Price)", cond);
                    DataRow dr = dt.NewRow();
                    dr["CompanySAPCode"] = drAP["CompanySAPCode"];
                    dr["ItemNo"] = drAP["ItemNo"];
                    dr["ItemName"] = drAP["ItemName"];

                    dr["GLNo"] = sap.getGLNoCredit();
                    dr["GLName"] = sap.getGLNoCreditName();
                    dr["profitCenter"] = "P11000";
                    // dr["debit"] = itmtotal;
                    dr["credit"] = (-1) * itmtotal;
                    dr["itemtext"] = dr["ItemName"] + " " + itmcnt;// + " " +postingDate;
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception)
            {
                return null;
            }
            return dt;
        }

        /* = / IO Cross Company / = */
        #endregion


        private void ReportDelvCnfWBS()
        {
            try
            {
                string reportPath = Server.MapPath("../rpt/rptAdminConfirmDeliveryForAccWBS.rpt");
                string pdfName = "ReportConfirmDeliveryForAccWBS_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string pdfPath = Server.MapPath("../Report/" + pdfName + ".pdf");
                string pdfPopupPath = "../Report/" + pdfName + ".pdf";

                ReportDocument reportDocument = new ReportDocument();
                reportDocument.Load(reportPath);
                //reportDocument.Refresh();

                DAReport clsRpt = new DAReport();
                clsRpt.ApplyTableLogOnInfo(ref reportDocument);
                clsRpt.SetSubreportLogOnInfo(ref reportDocument);

                string DelvLst = (Session["SelDelv"] + "").Replace("##", ";");
                DelvLst = DelvLst.Remove(DelvLst.Length - 1, 1).Remove(0, 1);
                reportDocument.SetParameterValue("@DelvLst", DelvLst);
                reportDocument.SetParameterValue("@RptDisplayType", "CLEAR_WBS");

                ExportOptions CrExportOptions;
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                CrDiskFileDestinationOptions.DiskFileName = pdfPath; // "C:\\SampleReport.pdf";
                CrExportOptions = reportDocument.ExportOptions;
                {
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                }
                reportDocument.Export();
                callPdf(pdfPopupPath, reportDocument);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alertNClose('Exception: " + ex.Message.Replace("'", "") + "');", true);
                return;
            }
        }

        private void ReportDelvCnfWBSCrossCom()
        {
            try
            {
                string reportPath = Server.MapPath("../rpt/rptAdminConfirmDeliveryForAccWBSCrossCom.rpt");
                string pdfName = "ReportConfirmDeliveryForAccWBSCrossCom_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string pdfPath = Server.MapPath("../Report/" + pdfName + ".pdf");
                string pdfPopupPath = "../Report/" + pdfName + ".pdf";

                ReportDocument reportDocument = new ReportDocument();
                reportDocument.Load(reportPath);
                //reportDocument.Refresh();

                DAReport clsRpt = new DAReport();
                clsRpt.ApplyTableLogOnInfo(ref reportDocument);
                clsRpt.SetSubreportLogOnInfo(ref reportDocument);
                // reportDocument.SetDatabaseLogon(clsRpt.getUserID(), clsRpt.getPassword(), clsRpt.getDataSource(), clsRpt.getInitialCatalog());//("", "", "ADMIN-PC\\ADMIN", "dbRMC"); 

                string DelvLst = (Session["SelDelv"] + "").Replace("##", ";");
                DelvLst = DelvLst.Remove(DelvLst.Length - 1, 1).Remove(0, 1);
                reportDocument.SetParameterValue("@DelvLst", DelvLst);
                reportDocument.SetParameterValue("@RptDisplayType", "CLEAR_WBS");

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
                callPdf(pdfPopupPath, reportDocument);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alertNClose('Exception: " + ex.Message.Replace("'", "") + "');", true);
                return;
            }
        }

        private void ReportDelvCnfIO()
        {
            try
            {
                string reportPath = Server.MapPath("../rpt/rptAdminConfirmDeliveryForAccIOV2.rpt");
                string pdfName = "ReportConfirmDeliveryForAccIO_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string pdfPath = Server.MapPath("../Report/" + pdfName + ".pdf");
                string pdfPopupPath = "../Report/" + pdfName + ".pdf";

                ReportDocument reportDocument = new ReportDocument();
                reportDocument.Load(reportPath);
                reportDocument.Refresh();

                DAReport clsRpt = new DAReport();
                clsRpt.ApplyTableLogOnInfo(ref reportDocument);
                clsRpt.SetSubreportLogOnInfo(ref reportDocument);
                // reportDocument.SetDatabaseLogon(clsRpt.getUserID(), clsRpt.getPassword(), clsRpt.getDataSource(), clsRpt.getInitialCatalog());//("", "", "ADMIN-PC\\ADMIN", "dbRMC"); 

                string DelvLst = (Session["SelDelv"] + "").Replace("##", ";");
                DelvLst = DelvLst.Remove(DelvLst.Length - 1, 1).Remove(0, 1);
                reportDocument.SetParameterValue("@DelvLst", DelvLst);
                reportDocument.SetParameterValue("@RptDisplayType", "CLEAR_MKT");

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
                callPdf(pdfPopupPath, reportDocument);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alertNClose('Exception: " + ex.Message.Replace("'", "") + "');", true);
                return;
            }
        }
        private void ReportDelvCnfIOCrossCom()
        {
            try
            {
                string reportPath = Server.MapPath("../rpt/rptAdminConfirmDeliveryForAccIOV2CrossCom.rpt");
                string pdfName = "rptAdminConfirmDeliveryForAccIOV2CrossCom_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string pdfPath = Server.MapPath("../Report/" + pdfName + ".pdf");
                string pdfPopupPath = "../Report/" + pdfName + ".pdf";

                ReportDocument reportDocument = new ReportDocument();
                reportDocument.Load(reportPath);
                reportDocument.Refresh();

                DAReport clsRpt = new DAReport();
                clsRpt.ApplyTableLogOnInfo(ref reportDocument);
                clsRpt.SetSubreportLogOnInfo(ref reportDocument);
                // reportDocument.SetDatabaseLogon(clsRpt.getUserID(), clsRpt.getPassword(), clsRpt.getDataSource(), clsRpt.getInitialCatalog());//("", "", "ADMIN-PC\\ADMIN", "dbRMC"); 

                string DelvLst = (Session["SelDelv"] + "").Replace("##", ";");
                DelvLst = DelvLst.Remove(DelvLst.Length - 1, 1).Remove(0, 1);
                reportDocument.SetParameterValue("@DelvLst", DelvLst);
                reportDocument.SetParameterValue("@RptDisplayType", "CLEAR_MKT");

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
                callPdf(pdfPopupPath, reportDocument);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alertNClose('Exception: " + ex.Message.Replace("'", "") + "');", true);
                return;
            }
        }

        private void ReportBudgetIO()
        {
            /* - คำนวณก่อน - */
            string msgErr = "";
            DASAPConnector sap = new DASAPConnector();
            DataTable dtSapRst = null;

            DataTable dtcmp = (DataTable)Session["dtCompanySel"];
            DataTable dtprj = (DataTable)Session["dtProjectSel"];

            if (dtcmp != null)
            {
                foreach (DataRow dr in dtcmp.Rows)
                {
                    DataTable dtx = null;
                    sap.SAPGetDataReportIOBudget("", dr["CompanySAPCode"] + "", "", ref dtx, ref msgErr);
                    foreach (DataRow drx in dtx.Rows)
                    {
                        if (dtSapRst == null) { dtSapRst = dtx.Clone(); }
                        dtSapRst.ImportRow(drx);
                    }
                }
            }

            if (dtprj != null)
            {
                foreach (DataRow dr in dtprj.Rows)
                {
                    DataTable dtx = null;
                    sap.SAPGetDataReportIOBudget(dr["Plant"] + "", "", "", ref dtx, ref msgErr);
                    foreach (DataRow drx in dtx.Rows)
                    {
                        if (dtSapRst == null) { dtSapRst = dtx.Clone(); }
                        if (dtx.Rows.Count != 0)
                        {
                            if (dtSapRst.Select("PROJECT = '" + dtx.Rows[0]["PROJECT"] + "'").Length == 0)
                            {
                                dtSapRst.ImportRow(drx);
                            }
                        }
                    }
                }
            }

            if (dtSapRst == null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alertNClose('กรุณาระบุโครงการหรือบริษัท !!');", true);
                return;
            }
            if (dtSapRst.Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alertNClose('กรุณาระบุโครงการหรือบริษัท !!');", true);
                return;
            }


            DataColumn WaitToPost = new DataColumn("WaitToPost", typeof(decimal));
            WaitToPost.DefaultValue = 0.0;
            dtSapRst.Columns.Add(WaitToPost);
            DataColumn Available = new DataColumn("AvailableX", typeof(decimal));
            Available.DefaultValue = 0.0;
            dtSapRst.Columns.Add(Available);
            dtSapRst.AcceptChanges();


            string projectlst = "";
            foreach (DataRow dr in dtSapRst.Rows)
            {
                projectlst += "," + dr["PROJECT"];
            }

            if (projectlst != "")
            {
                projectlst = projectlst.Remove(0, 1);
            }
            DAReport clsRpt = new DAReport();
            DataTable dtBudgetWebWait = clsRpt.getBudgetWaitToPostGL(projectlst);

            foreach (DataRow dr in dtBudgetWebWait.Rows)
            {
                // DataRow[] drSapRst = dtSapRst.Select("PROJECT = '" + dr["Plant"] + "'");
                DataRow[] drSapRst = dtSapRst.Select("IO = '" + dr["CostCenter"] + "'");
                if (drSapRst.Length > 0)
                {
                    drSapRst[0]["WaitToPost"] = (decimal)drSapRst[0]["WaitToPost"] + (decimal)dr["WaitToPost"];
                }
            }

            foreach (DataRow dr in dtSapRst.Rows)
            {
                decimal decAvailable = 0;
                decimal.TryParse(dr["AVAILABLE"] + "", out decAvailable);
                decimal decWaitToPost = 0;
                decimal.TryParse(dr["WaitToPost"] + "", out decWaitToPost);
                dr["AvailableX"] = decAvailable - decWaitToPost;
            }

            /* - ออกรายงาน - */
            try
            {
                string reportPath = Server.MapPath("../rpt/rptBudgetIO.rpt");
                string xmlPath = Server.MapPath("../rpt/BudgetIO.xml");
                string pdfName = "ReportBudgetIO_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string pdfPath = Server.MapPath("../Report/" + pdfName + ".pdf");
                string pdfPopupPath = "../Report/" + pdfName + ".pdf";

                AutorizeData auth = (AutorizeData)Session["userInfo_" + Session.SessionID];
                dtSapRst.TableName = "BudgetIO";
                dtSapRst.WriteXml(xmlPath, XmlWriteMode.WriteSchema);

                ReportDocument reportDocument = new ReportDocument();
                reportDocument.Load(reportPath);
                reportDocument.Refresh();

                reportDocument.SetDataSource(dtSapRst);
                reportDocument.SetParameterValue("PrintUser", auth.DisplayName);

                ExportOptions CrExportOptions;
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                CrDiskFileDestinationOptions.DiskFileName = pdfPath;
                CrExportOptions = reportDocument.ExportOptions;
                {
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                }
                reportDocument.Export();
                callPdf(pdfPopupPath, reportDocument);

                Session["SAPDOC"] = "";
                reportDocument.Close();
                reportDocument.Dispose();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alertNClose('Exception: " + ex.Message.Replace("'", "") + "');", true);
                return;
            }

        }

        private void ReportTransferToPreBooking()
        {
            try
            {
                string reportPath = Server.MapPath("../rpt/rptTransferItemToPrebooking.rpt");
                string pdfName = "ReportTransferItemToPrebooking_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string pdfPath = Server.MapPath("../Report/" + pdfName + ".xls");
                string pdfPopupPath = "../Report/" + pdfName + ".xls";

                ReportDocument reportDocument = new ReportDocument();
                reportDocument.Load(reportPath);
                reportDocument.Refresh();

                DAReport clsRpt = new DAReport();
                clsRpt.ApplyTableLogOnInfo(ref reportDocument);
                clsRpt.SetSubreportLogOnInfo(ref reportDocument);
                // reportDocument.SetDatabaseLogon(clsRpt.getUserID(), clsRpt.getPassword(), clsRpt.getDataSource(), clsRpt.getInitialCatalog());//("", "", "ADMIN-PC\\ADMIN", "dbRMC"); 

                string RqHLst = (Session["RqHLst"] + "").Replace("##", ";");
                // RqHLst = RqHLst.Remove(RqHLst.Length - 1, 1).Remove(0, 1);
                reportDocument.SetParameterValue("@RqHLst", RqHLst);

                ExportOptions CrExportOptions;
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                //PdfRtfWordFormatOptions CrFormatTypeOptions = new ExcelFormatOptions();
                CrDiskFileDestinationOptions.DiskFileName = pdfPath; // "C:\\SampleReport.pdf";
                CrExportOptions = reportDocument.ExportOptions;
                {
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.Excel;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                    //CrExportOptions.FormatOptions = CrFormatTypeOptions;
                }
                reportDocument.Export();
                callPdf(pdfPopupPath, reportDocument);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alertNClose('Exception: " + ex.Message.Replace("'", "") + "');", true);
                return;
            }
        }

        public string ReportMemoRequest(string docno)
        {
            try
            {
                string reportPath = Server.MapPath("../rpt/rptMemoRequest.rpt");
                string pdfName = "ReportMemoRequestMK.rpt_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string pdfPath = Server.MapPath("../Report/" + pdfName + ".pdf");
                string pdfPopupPath = "../Report/" + pdfName + ".pdf";
                if (docno == "") return "";

                ReportDocument reportDocument = new ReportDocument();
                reportDocument.Load(reportPath);

                DAReport clsRpt = new DAReport();
                clsRpt.ApplyTableLogOnInfo(ref reportDocument);
                clsRpt.SetSubreportLogOnInfo(ref reportDocument);

                reportDocument.SetParameterValue("@DOCNO", docno);

                ExportOptions CrExportOptions;
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                CrDiskFileDestinationOptions.DiskFileName = pdfPath;
                CrExportOptions = reportDocument.ExportOptions;
                {
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                }
                reportDocument.Export();

                return pdfPopupPath;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public string ReportMemoRequestNoneMKT(string docno)
        {
            try
            {
                string reportPath = Server.MapPath("../rpt/rptMemoRequestWithTransfer.rpt");
                string pdfName = "ReportMemoRequest.rpt_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string pdfPath = Server.MapPath("../Report/" + pdfName + ".pdf");
                string pdfPopupPath = "../Report/" + pdfName + ".pdf";
                if (docno == "") return "";

                ReportDocument reportDocument = new ReportDocument();
                reportDocument.Load(reportPath);

                DAReport clsRpt = new DAReport();
                clsRpt.ApplyTableLogOnInfo(ref reportDocument);
                clsRpt.SetSubreportLogOnInfo(ref reportDocument);

                reportDocument.SetParameterValue("@DOCNO", docno);

                ExportOptions CrExportOptions;
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                CrDiskFileDestinationOptions.DiskFileName = pdfPath;
                CrExportOptions = reportDocument.ExportOptions;
                {
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                }
                reportDocument.Export();

                return pdfPopupPath;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public string ReportAccountRecCostCenter(string SAPID)
        {

            AutorizeData auth = new AutorizeData();
            auth = (AutorizeData)Session["userInfo_" + Session.SessionID] ?? new AutorizeData();
            authEmployeeID = auth?.EmployeeID;
            authDisplayName = auth?.DisplayName;

            try
            {

                string reportPath = Server.MapPath("../rpt/rptGetDataCostCenter.rpt");
                string pdfName = "rptGetDataCostCenter.rpt_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string pdfPath = Server.MapPath("../Report/" + pdfName + ".pdf");
                string pdfPopupPath = "../Report/" + pdfName + ".pdf";

                if (SAPID == "") return "";

                ReportDocument reportDocument = new ReportDocument();
                reportDocument.Load(reportPath);

                DAReport clsRpt = new DAReport();
                clsRpt.ApplyTableLogOnInfo(ref reportDocument);
                clsRpt.SetSubreportLogOnInfo(ref reportDocument);

                reportDocument.SetParameterValue("@SAPID", SAPID);
                reportDocument.SetParameterValue("DisplayName", authDisplayName);

                ExportOptions CrExportOptions;
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                CrDiskFileDestinationOptions.DiskFileName = pdfPath;
                CrExportOptions = reportDocument.ExportOptions;
                {
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                }

                reportDocument.Export();

                return pdfPopupPath;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public string ReportAccountRecCostCenterCrossComCode(string SAPID)
        {
            AutorizeData auth = new AutorizeData();
            auth = (AutorizeData)Session["userInfo_" + Session.SessionID] ?? new AutorizeData();
            authEmployeeID = auth?.EmployeeID;
            authDisplayName = auth?.DisplayName;

            try
            {
                string reportPath = Server.MapPath("../rpt/rptGetDataCostCenterCrossComCode.rpt");
                string pdfName = "rptGetDataCostCenterCrossComCode.rpt_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string pdfPath = Server.MapPath("../Report/" + pdfName + ".pdf");
                string pdfPopupPath = "../Report/" + pdfName + ".pdf";
                if (SAPID == "") return "";

                ReportDocument reportDocument = new ReportDocument();
                reportDocument.Load(reportPath);

                DAReport clsRpt = new DAReport();
                clsRpt.ApplyTableLogOnInfo(ref reportDocument);
                clsRpt.SetSubreportLogOnInfo(ref reportDocument);

                reportDocument.SetParameterValue("@SAPID", SAPID);
                reportDocument.SetParameterValue("DisplayName", authDisplayName);

                ExportOptions CrExportOptions;
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                CrDiskFileDestinationOptions.DiskFileName = pdfPath;
                CrExportOptions = reportDocument.ExportOptions;
                {
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                }
                reportDocument.Export();

                return pdfPopupPath;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public string ReportDeliveryCostCenterDetail(string ListDelvId, out string msgerr)
        {
            try
            {
                string reportPath = Server.MapPath("../rpt/rptAdminConfirmDeliveryCostCenter.rpt");
                string pdfName = "rptAdminConfirmDeliveryCostCenter.rpt_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string pdfPath = Server.MapPath("../Report/" + pdfName + ".pdf");
                string pdfPopupPath = "../Report/" + pdfName + ".pdf";
                if (ListDelvId == "")
                {
                    msgerr = "Result is empty";
                    return "";
                }

                ReportDocument reportDocument = new ReportDocument();
                reportDocument.Load(reportPath);

                DAReport clsRpt = new DAReport();
                clsRpt.ApplyTableLogOnInfo(ref reportDocument);
                clsRpt.SetSubreportLogOnInfo(ref reportDocument);

                reportDocument.SetParameterValue("@ListDelvId", ListDelvId);

                ExportOptions CrExportOptions;
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                CrDiskFileDestinationOptions.DiskFileName = pdfPath;
                CrExportOptions = reportDocument.ExportOptions;
                {
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                }
                reportDocument.Export();

                msgerr = "SUCCESS";
                return pdfPopupPath;
            }
            catch (Exception ex)
            {
                msgerr = ex.Message.ToString();
                return "";
            }
        }

        public string ReportDeliveryCostCenterCrossComDetail(string ListDelvId, out string msgerr)
        {
            try
            {
                string reportPath = Server.MapPath("../rpt/rptAdminConfirmDeliveryCostCenterCrossCom.rpt");
                string pdfName = "rptAdminConfirmDeliveryCostCenterCrossCom.rpt_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string pdfPath = Server.MapPath("../Report/" + pdfName + ".pdf");
                string pdfPopupPath = "../Report/" + pdfName + ".pdf";
                if (ListDelvId == "")
                {
                    msgerr = "Result is empty";
                    return "";
                }

                ReportDocument reportDocument = new ReportDocument();
                reportDocument.Load(reportPath);

                DAReport clsRpt = new DAReport();
                clsRpt.ApplyTableLogOnInfo(ref reportDocument);
                clsRpt.SetSubreportLogOnInfo(ref reportDocument);

                reportDocument.SetParameterValue("@ListDelvId", ListDelvId);

                ExportOptions CrExportOptions;
                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                CrDiskFileDestinationOptions.DiskFileName = pdfPath;
                CrExportOptions = reportDocument.ExportOptions;
                {
                    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                }
                reportDocument.Export();

                msgerr = "SUCCESS";
                return pdfPopupPath;
            }
            catch (Exception ex)
            {
                msgerr = ex.Message.ToString();
                return "";
            }
        }

    }
}