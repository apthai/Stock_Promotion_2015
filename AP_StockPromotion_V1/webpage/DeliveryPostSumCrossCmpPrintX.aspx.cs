using AP_StockPromotion_V1.Class;
using AP_StockPromotion_V1.ws_authorize;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

namespace AP_StockPromotion_V1.webpage
{
    public partial class DeliveryPostSumCrossCmpPrintX : System.Web.UI.Page
    {
        Entities.FormatDate convertDate = new Entities.FormatDate();
        //Session["dtDelvPost"] // GetDataDeliveryItemFromDelvLst
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                txtDate.Text = DateTime.Now.ToString(convertDate.formatDate);

                string DelvLst = Request.QueryString["DelvLstId"] + "";
                Class.DADelivery cls = new Class.DADelivery();
                DataTable dt = cls.GetDataDeliveryItemFromDelvLst(DelvLst);
                Session["Posting"] = dt;
                DataTable PCRTHO = dt.DefaultView.ToTable(true, "CompanySAPCode", "ProfitCenterHO");
                if (PCRTHO.Rows.Count != 1)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('กรุณาเลือกข้อมูลใบส่งมอบจาก Company เดียวกันเท่านั้น !!'); bindDataParentPage();", true);
                    return;
                }
                else
                {
                    hdfCompanySAPCode.Value = PCRTHO.Rows[0]["CompanySAPCode"] + "";
                    hdfProfitHO.Value = PCRTHO.Rows[0]["ProfitCenterHO"] + "";
                    PCRTHO.Dispose();
                    PCRTHO = null;
                }

                grdAccountsPayable.DataSource = getDataAccountsPayable(dt);
                grdAccountsPayable.DataBind();
                grdDataPostAccount.DataSource = getDataPostAccounts(dt);
                grdDataPostAccount.DataBind();
                grdAccountsReceivable.DataSource = getDataAccountsReceivable(dt);
                grdAccountsReceivable.DataBind();





                // string x = cls.lookTable(dt);

                //ItemNo	ItemName	ItemId	Price	CostCenter
                DataTable dtsum = dt.DefaultView.ToTable(true, "ItemNo", "ItemName", "ProjectID", "CostCenter");
                dtsum.Columns.Add(new DataColumn("Amount", typeof(int)));
                dtsum.Columns.Add(new DataColumn("Total", typeof(decimal)));
                foreach (DataRow drsum in dtsum.Rows)
                {
                    string cond = "ISNULL(ItemNo,'') = '" + drsum["ItemNo"] + "' AND ISNULL(ItemName,'') = '" + drsum["ItemName"] + "' AND ISNULL(CostCenter,'') = '" + drsum["CostCenter"] + "' AND ISNULL(ProjectID,'') = '" + drsum["ProjectID"] + "'";
                    drsum["Amount"] = dt.Compute("COUNT(ItemId)", cond);
                    drsum["Total"] = dt.Compute("SUM(Price)", cond);
                }
                dtsum.AcceptChanges();
                grdData.DataSource = dtsum;
                grdData.DataBind();
            }
        }

        private DataTable getDataAccountsPayable(DataTable dt_A)
        {
            DASAPConnector sap = new DASAPConnector();
            dt_A.DefaultView.RowFilter = "";
            DataTable dtAP = dt_A.DefaultView.ToTable(true, "ItemNo", "ItemName", "CostCenter", "ProfitCenterHO");

            DataTable dt = dtAP.Clone();
            try
            {
                // string x = new Class.DADelivery().lookTable(dt);
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
                    dr["itemtext"] = dr["ItemName"] + " " + itmcnt;
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
            catch (Exception ex)
            {
                return null;
            }
            return dt;
        }

        private DataTable getDataPostAccounts(DataTable dt_A)
        {
            DASAPConnector sap = new DASAPConnector();
            dt_A.DefaultView.RowFilter = "";
            DataTable dt = dt_A.DefaultView.ToTable(true, "WBS_SAP", "ItemName", "isNonePR");
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
                    //drTotal["Amount"] = dt.Compute("COUNT(ItemId)", cond);
                    drTotal["WBS_SAP"] = "" + drTotal["WBS_SAP"];
                    //drTotal["CostCenter"] = "P" + drTotal["CostCenter"];
                    //drTotal["ItemName"] = dt.Compute("COUNT(WBS_SAP)", cond);
                    drTotal["Debit"] = (decimal)dt_A.Compute("SUM(Price)", cond);
                    string preTxt = ""; if (drTotal["isNonePR"] + "" == "Y") { preTxt = "O-"; }
                    string ItemText = preTxt + " " + drTotal["ItemName"] + (int)dt.Compute("COUNT(WBS_SAP)", cond);
                    drTotal["ItemText"] = ItemText;
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
                    dr["ProfitCenter"] = hdfProfitHO.Value;//(drTotal["CostCenter"] + "").Replace("MK", "P");
                    dr["Credit"] = (-1) * (decimal)dt_A.Compute("SUM(Price)", cond);
                    // dr["Debit"] = DBNull.Value;
                    dt.Rows.Add(dr);
                }
                dtx.Dispose(); dtx = null;
                dt.AcceptChanges();
                grdDataPostAccount.DataSource = dt;
                grdDataPostAccount.DataBind();
            }
            catch (Exception ex)
            {
                return null;
            }
            return dt;
        }

        private DataTable getDataAccountsReceivable(DataTable dt_A)
        {
            DASAPConnector sap = new DASAPConnector();
            dt_A.DefaultView.RowFilter = "";
            DataTable dtAP = dt_A.DefaultView.ToTable(true, "CompanySAPCode", "ItemNo", "ItemName");
            // CompanySAPCode,  getGLCompName,  'P11000',   Sum(),  itmTxt
            DataTable dt = dtAP.Clone();
            try
            {
                // string x = new Class.DADelivery().lookTable(dt);
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
                    dr["itemtext"] = dr["ItemName"] + " " + itmcnt;
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return dt;
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=ReportIO.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            this.Page.RenderControl(hw);
            StringReader sr = new StringReader(sw.ToString());
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.Write(pdfDoc);
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }
    }
}