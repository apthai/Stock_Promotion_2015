using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AP_StockPromotion_V1.Class;
using AP_StockPromotion_V1.ws_authorize;

using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

namespace AP_StockPromotion_V1.webpage
{
    public partial class DeliveryPostSumCrossCmpPrint : System.Web.UI.Page
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

                DataSet ds = new DataSet();
                DataTable dt1 = getDataAccountsPayable(dt);
                DataTable dt2 = getDataPostAccounts(dt);
                DataTable dt3 = getDataAccountsReceivable(dt);
                dt1.TableName = "AccountsPayable";
                dt2.TableName = "PostAccounts";
                dt3.TableName = "AccountsReceivable";
                ds.Tables.Add(dt1);
                ds.Tables.Add(dt2);
                ds.Tables.Add(dt3);
                ds.WriteXml(@"D:\FB03.xml");
                ds.WriteXmlSchema(@"D:\FB03.xsd");
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
            
        }
    }



    // ********  ส่วน 2 class ด้านล่างเป็นการทำ page number , header , footer (ถ้าไม่อยากแก้อะไรมาแค่ copy วาง จบ) **********//

    public class PDFFooter : PdfPageEventHelper
    {

        // หัวข้อเฉพาะหน้าแรก
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            base.OnOpenDocument(writer, document);

            /*
            PdfPTable tabFot = new PdfPTable(new float[] { 1F });
        
            tabFot.SpacingAfter = 10F;
            PdfPCell cell;
            tabFot.TotalWidth = 300F;

            int pageN = writer.PageNumber;
            String text = "Page " + pageN + " of ";
        
            cell = new PdfPCell(new Phrase(text));
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;

            tabFot.DefaultCell.Border = 0;
            tabFot.AddCell(cell);
            tabFot.WriteSelectedRows(0, -1, 150, document.Top, writer.DirectContent);
             */
        }

        // write on start of each page
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            base.OnStartPage(writer, document);
        }

        // write on end of each page
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            /*---- set font ----*/
            BaseFont bf = BaseFont.CreateFont(
                             BaseFont.TIMES_ROMAN,
                             BaseFont.CP1252,
                             BaseFont.EMBEDDED);
            Font font = new Font(bf, 12);

            /*---- set page number ----*/
            base.OnEndPage(writer, document);
            PdfPTable tabFot = new PdfPTable(new float[] { 1F });
            tabFot.TotalWidth = 300F;
            int pageN = writer.PageNumber;
            String text = "Page : " + pageN;
            PdfPCell cell = new PdfPCell(new Phrase(text, font));
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            Rectangle pageSize = document.PageSize;
            tabFot.DefaultCell.Border = 0;
            tabFot.AddCell(cell);
            document.SetMargins(25f, 25f, 35f, 15f);
            tabFot.WriteSelectedRows(0, -1, pageSize.GetLeft(280), pageSize.GetTop(10), writer.DirectContent);


            /*---- set footer----*/


        }

        //write on close of document
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);
        }
    }


    public class PageEventHelper : PdfPageEventHelper
    {
        public class TwoColumnHeaderFooter : PdfPageEventHelper
        {
            // This is the contentbyte object of the writer
            PdfContentByte cb;

            // we will put the final number of pages in a template
            PdfTemplate template;

            // this is the BaseFont we are going to use for the header / footer
            BaseFont bf = null;

            // This keeps track of the creation time
            DateTime PrintTime = DateTime.Now;

            #region Properties
            private string _Title;
            public string Title
            {
                get { return _Title; }
                set { _Title = value; }
            }

            private string _HeaderLeft;
            public string HeaderLeft
            {
                get { return _HeaderLeft; }
                set { _HeaderLeft = value; }
            }

            private string _HeaderRight;
            public string HeaderRight
            {
                get { return _HeaderRight; }
                set { _HeaderRight = value; }
            }

            private Font _HeaderFont;
            public Font HeaderFont
            {
                get { return _HeaderFont; }
                set { _HeaderFont = value; }
            }

            private Font _FooterFont;
            public Font FooterFont
            {
                get { return _FooterFont; }
                set { _FooterFont = value; }
            }
            #endregion

            // we override the onOpenDocument method
            public override void OnOpenDocument(PdfWriter writer, Document document)
            {
                try
                {
                    PrintTime = DateTime.Now;
                    bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    cb = writer.DirectContent;
                    template = cb.CreateTemplate(50, 50);
                }
                catch (DocumentException de)
                {
                }
                catch (System.IO.IOException ioe)
                {
                }
            }

            public override void OnStartPage(PdfWriter writer, Document document)
            {
                base.OnStartPage(writer, document);

                Rectangle pageSize = document.PageSize;

                if (Title != string.Empty)
                {
                    cb.BeginText();
                    cb.SetFontAndSize(bf, 15);
                    cb.SetRGBColorFill(50, 50, 200);
                    cb.SetTextMatrix(pageSize.GetLeft(40), pageSize.GetTop(40));
                    cb.ShowText(Title);
                    cb.EndText();
                }

                if (HeaderLeft + HeaderRight != string.Empty)
                {
                    PdfPTable HeaderTable = new PdfPTable(2);
                    HeaderTable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    HeaderTable.TotalWidth = pageSize.Width - 80;
                    HeaderTable.SetWidthPercentage(new float[] { 45, 45 }, pageSize);

                    PdfPCell HeaderLeftCell = new PdfPCell(new Phrase(8, HeaderLeft, HeaderFont));
                    HeaderLeftCell.Padding = 5;
                    HeaderLeftCell.PaddingBottom = 8;
                    HeaderLeftCell.BorderWidthRight = 0;
                    HeaderTable.AddCell(HeaderLeftCell);

                    PdfPCell HeaderRightCell = new PdfPCell(new Phrase(8, HeaderRight, HeaderFont));
                    HeaderRightCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    HeaderRightCell.Padding = 5;
                    HeaderRightCell.PaddingBottom = 8;
                    HeaderRightCell.BorderWidthLeft = 0;
                    HeaderTable.AddCell(HeaderRightCell);

                    cb.SetRGBColorFill(0, 0, 0);
                    HeaderTable.WriteSelectedRows(0, -1, pageSize.GetLeft(40), pageSize.GetTop(50), cb);
                }
            }

            public override void OnEndPage(PdfWriter writer, Document document)
            {
                base.OnEndPage(writer, document);

                int pageN = writer.PageNumber;
                String text = "Page " + pageN + " of ";
                float len = bf.GetWidthPoint(text, 8);

                Rectangle pageSize = document.PageSize;

                cb.SetRGBColorFill(100, 100, 100);

                cb.BeginText();
                cb.SetFontAndSize(bf, 8);
                cb.SetTextMatrix(pageSize.GetLeft(40), pageSize.GetBottom(30));
                cb.ShowText(text);
                cb.EndText();

                cb.AddTemplate(template, pageSize.GetLeft(40) + len, pageSize.GetBottom(30));

                cb.BeginText();
                cb.SetFontAndSize(bf, 8);
                cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT,
                    "Printed On " + PrintTime.ToString(),
                    pageSize.GetRight(40),
                    pageSize.GetBottom(30), 0);
                cb.EndText();
            }

            public override void OnCloseDocument(PdfWriter writer, Document document)
            {
                base.OnCloseDocument(writer, document);

                template.BeginText();
                template.SetFontAndSize(bf, 8);
                template.SetTextMatrix(0, 0);
                template.ShowText("" + (writer.PageNumber - 1));
                template.EndText();
            }

        }
    }
}