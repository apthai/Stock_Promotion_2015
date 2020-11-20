using AP_StockPromotion_V1.ws_authorize;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;

namespace AP_StockPromotion_V1.webpage
{
    public partial class DeliveryLowPricePostSum : Page
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
                Session["PostingLowPrice"] = dt;
                // string x = cls.lookTable(dt);
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
                //ItemNo	ItemName	ItemId	Price	CostCenter
                DataTable dtsum = dt.DefaultView.ToTable(true, "ItemNo", "ItemName", "CostCenter");// 
                dtsum.Columns.Add(new DataColumn("Amount", typeof(int)));
                dtsum.Columns.Add(new DataColumn("Total", typeof(decimal)));
                foreach (DataRow drsum in dtsum.Rows)
                {
                    string cond = "ISNULL(ItemNo,'') = '" + drsum["ItemNo"] + "' AND ISNULL(ItemName,'') = '" + drsum["ItemName"] + "' AND ISNULL(CostCenter,'') = '" + drsum["CostCenter"] + "'";
                    drsum["Amount"] = dt.Compute("COUNT(ItemId)", cond);
                    drsum["Total"] = dt.Compute("SUM(Price)", cond);
                }
                dtsum.AcceptChanges();
                grdData.DataSource = dtsum;
                grdData.DataBind();

                Class.DASAPConnector sap = new Class.DASAPConnector();
                //ItemNo	ItemName	ItemId	Price	CostCenter isNonePR  AND ISNULL(isNonePR,'') = '" + drsum["isNonePR"] + "'
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
                    //drTotal["Amount"] = dt.Compute("COUNT(ItemId)", cond);
                    drTotal["CostCenter"] = drTotal["CostCenter"] + ""; // "MK" + 
                    drTotal["Total"] = dt.Compute("SUM(Price)", cond);
                    string preTxt = ""; if (drTotal["isNonePR"] + "" == "Y") { preTxt = "O-"; }
                    drTotal["ItemText"] = preTxt + drTotal["ItemName"] + " " + dt.Compute("COUNT(ItemName)", cond); // +" " + DateTime.Now.ToString("MM.yyyy");
                }
                DataRow dr = dtTotal.NewRow();
                dr["GLNo"] = sap.getGLNoCredit();
                dr["GLName"] = sap.getGLNoCreditName();
                dr["ProfitCenter"] = hdfProfitHO.Value;
                dr["TotalCredit"] = (-1) * (decimal)dt.Compute("SUM(Price)", "");
                dr["Total"] = DBNull.Value;
                dtTotal.Rows.Add(dr);

                // DataTable dtx = dtTotal.Copy();
                //foreach (DataRow drTotal in dtx.Rows)
                //{
                //    DataRow dr = dtTotal.NewRow();
                //    dr["GLNo"] = sap.getGLNoCredit();
                //    dr["GLName"] = sap.getGLNoCreditName();
                //    dr["ProfitCenter"] = hdfProfitHO.Value;//(drTotal["CostCenter"] + "").Replace("MK", "P");
                //    dr["TotalCredit"] = (-1) * (decimal)drTotal["Total"];
                //    dr["Total"] = DBNull.Value;
                //    dtTotal.Rows.Add(dr);
                //}
                //dtx.Dispose(); dtx = null;
                dtTotal.AcceptChanges();
                grdDataSum.DataSource = dtTotal;
                grdDataSum.DataBind();

               
                DataSet ds = new DataSet();
                dtTotal.TableName = "NewDataSet/IO";
                foreach (DataTable dtx in ds.Tables)
                {
                    ds.Tables.Remove(dtx);
                }
                ds.AcceptChanges();
                ds.Tables.Add(dtTotal.Copy());
                Session["dsPstAccIO"] = ds;
                //ds.WriteXml(@"D:\PstAccIO.xml", XmlWriteMode.WriteSchema);
            }
        }

        protected void btnPostAccount_Click(object sender, EventArgs e)
        {
            //string delvNo = Request.QueryString["delvNo"] + "";
            //DataTable dtDelivery = (DataTable)Session["dtDeliveryList"];
            //dtDelivery.DefaultView.RowFilter = "isnull(DeliveryNo,'')='" + delvNo + "'";
            //DataTable dt = dtDelivery.DefaultView.ToTable();
            //dtDelivery.DefaultView.RowFilter = "";
            if (txtReference.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('" + "กรุณาระบุ Reference !!" + "'); ", true); return;
            }
            Entities.SapDelivery_DOCUMENTHEADER dochdr = new Entities.SapDelivery_DOCUMENTHEADER();
            List<Entities.SapDeliveryLowPrice_ACCOUNTGL> lstAccGL = new List<Entities.SapDeliveryLowPrice_ACCOUNTGL>();
            List<Entities.SapDelivery_CURRENCYAMOUNT> lstCurAmt = new List<Entities.SapDelivery_CURRENCYAMOUNT>();
            Entities.SapDeliveryLowPrice_ACCOUNTGL AccGL;
            Entities.SapDelivery_CURRENCYAMOUNT CurAmt;

            /* - SapDelivery_DOCUMENTHEADER - */
            AutorizeData auth = (AutorizeData)Session["userInfo_" + Session.SessionID];
            dochdr.USERNAME = auth.EmployeeID;
            dochdr.COMP_CODE = hdfCompanySAPCode.Value;
            dochdr.DOC_DATE = txtDate.Text;// sDate.ToString("dd.MM.yyyy"); 
            dochdr.PSTNG_DATE = txtDate.Text;//sDate.ToString("dd.MM.yyyy"); 
            DateTime sDate = new DateTime();
            convertDate.getDateFromString(txtDate.Text, ref  sDate);
            dochdr.FISC_YEAR = Convert.ToInt16(sDate.ToString("yyyy")); 
            dochdr.DOC_TYPE = "IA";
            dochdr.REF_DOC_NO = txtReference.Text;
            dochdr.REF_KEY_3 = txtRefKey3.Text;

            DataTable dt = (DataTable)Session["PostingLowPrice"];
            /* ItemNo	ItemName	CostCenter	DelvDate    ItemId	Price	 */
            DataTable dtPost = dt.DefaultView.ToTable(true, "ItemNo", "ItemName", "CostCenter", "DelvDate", "isNonePR", "PO_No", "PO_Item");
            dtPost.Columns.Add(new DataColumn("Amount", typeof(int)));
            dtPost.Columns.Add(new DataColumn("Total", typeof(decimal)));
            foreach (DataRow drPost in dtPost.Rows)
            { 
                // string cond = "ISNULL(ItemNo,'') = '" + drPost["ItemNo"] + "' AND ISNULL(ItemName,'') = '" + drPost["ItemName"] + "' AND ISNULL(CostCenter,'') = '" + drPost["CostCenter"] + "' AND ISNULL(DelvDate,'') = '" + drPost["DelvDate"] + "' AND ISNULL(isNonePR,'') = '" + drPost["isNonePR"] + "' AND ISNULL(PO_No,'') = '" + drPost["PO_No"] + "' AND ISNULL(PO_Item,'') = '" + drPost["PO_Item"] + "'";
                string cond = "ISNULL(ItemNo,'') = '" + drPost["ItemNo"] + "' AND ISNULL(ItemName,'') = '" + drPost["ItemName"] + "' AND ISNULL(CostCenter,'') = '" + drPost["CostCenter"] + "' AND ISNULL(DelvDate,'') = '" + drPost["DelvDate"] + "' AND ISNULL(isNonePR,'') = '" + drPost["isNonePR"] + "' AND ISNULL(PO_No,'') = '" + drPost["PO_No"] + "'";
                if (drPost["PO_Item"] == DBNull.Value) { cond += " AND PO_Item IS NULL "; }
                else { cond += " AND PO_Item = " + drPost["PO_Item"] + ""; }

                drPost["Amount"] = dt.Compute("COUNT(ItemId)", cond);
                drPost["Total"] = dt.Compute("SUM(Price)", cond);
            }
            dtPost.AcceptChanges();

            Class.DASAPConnector sap = new Class.DASAPConnector();
            int ii = 0; // Debit
            foreach (DataRow dr in dtPost.Rows)
            {
                ii++;
                AccGL = new Entities.SapDeliveryLowPrice_ACCOUNTGL(); 
                AccGL.ITEMNO_ACC = ii;
                AccGL.GL_ACCOUNT = sap.getGLNo300Cus();
                if (dr["isNonePR"] + "" != "Y")
                {
                    AccGL.ITEM_TEXT = dr["ItemName"] + " " + dr["Amount"]; //+ " " + ((DateTime)dr["DelvDate"]).ToString("MM.yyyy");
                }
                else
                {
                    AccGL.ITEM_TEXT = "O-" + dr["ItemName"] + " " + dr["Amount"]; //+ " " + ((DateTime)dr["DelvDate"]).ToString("MM.yyyy");
                }
                AccGL.ORDERID = "" + dr["CostCenter"];// "MK" + 
                AccGL.REF_KEY_3 = txtRefKey3.Text;
                string PO_Item = "00000" + dr["PO_Item"];
                AccGL.ALLOC_NMBR = dr["PO_No"] + "" + PO_Item.Substring(PO_Item.Length - 5, 5);
                lstAccGL.Add(AccGL);

                CurAmt = new Entities.SapDelivery_CURRENCYAMOUNT();
                CurAmt.ITEMNO_ACC = ii; 
                CurAmt.CURRENCY = "THB";
                decimal TotalAmount = 0;
                decimal.TryParse(dr["Total"] + "", out TotalAmount);
                CurAmt.AMT_DOCCUR = TotalAmount;
                lstCurAmt.Add(CurAmt);
            }
            foreach (DataRow dr in dtPost.Rows)
            {
                ii++;
                AccGL = new Entities.SapDeliveryLowPrice_ACCOUNTGL(); // Credit
                AccGL.ITEMNO_ACC = ii;
                AccGL.GL_ACCOUNT = sap.getGLNoCredit();
                //AccGL.ITEM_TEXT = dr["ItemName"] + " " + dr["Amount"] + " " + ((DateTime)dr["DelvDate"]).ToString("MM.yyyy");
                if (dr["isNonePR"] + "" != "Y")
                {
                    AccGL.ITEM_TEXT = dr["ItemName"] + " " + dr["Amount"]; // + " " + ((DateTime)dr["DelvDate"]).ToString("MM.yyyy");
                }
                else
                {
                    AccGL.ITEM_TEXT = "O-" + dr["ItemName"] + " " + dr["Amount"]; // + " " + ((DateTime)dr["DelvDate"]).ToString("MM.yyyy");
                }
                AccGL.PROFIT_CTR = hdfProfitHO.Value;
                string PO_Item = "00000" + dr["PO_Item"];
                AccGL.REF_KEY_3 = txtRefKey3.Text;
                AccGL.ALLOC_NMBR = dr["PO_No"] + "" + PO_Item.Substring(PO_Item.Length - 5, 5);
                lstAccGL.Add(AccGL);

                CurAmt = new Entities.SapDelivery_CURRENCYAMOUNT();
                CurAmt.ITEMNO_ACC = ii;
                CurAmt.CURRENCY = "THB";
                decimal TotalAmount = 0;
                decimal.TryParse(dr["Total"] + "", out TotalAmount);
                CurAmt.AMT_DOCCUR = TotalAmount * (-1);
                lstCurAmt.Add(CurAmt);
            }

            string delvListNo = "";
            DataTable dtDelvLst = dt.DefaultView.ToTable(true, "DelvLstId");
            foreach (DataRow dr in dtDelvLst.Rows) { delvListNo += dr["DelvLstId"] + ";"; }
            delvListNo = delvListNo.Remove(delvListNo.Length - 1, 1);

            string SAPDocNo = "";
            string msgErr = "";
            Class.DASAPConnector cls = new Class.DASAPConnector();
            bool rstPost = cls.deliveryLowPricePostAccount(delvListNo, dochdr, lstAccGL, lstCurAmt, ref SAPDocNo, ref msgErr);
            if (rstPost)
            {
                Session["selDelv"] = "";
                Session["SAPDOC"] = SAPDocNo;
                string urlRptPstAccIO = "frmReport.aspx?reqRPT=PstAccIO";
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "Popup60('" + urlRptPstAccIO + "'); bindDataParentPage();", true);
                //ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('SAP Document No. " + SAPDocNo + " บันทึกบัญชีเสร็จสิ้น'); bindDataParentPage();", true);
                return;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('บันทึกบัญชีผิดพลาด !! " + msgErr + "');", true);
                return;
            }
        }


    }
}