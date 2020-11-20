using AP_StockPromotion_V1.Class;
using AP_StockPromotion_V1.ws_authorize;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AP_StockPromotion_V1.web
{
    public partial class StockReceiveEdit : System.Web.UI.Page
    {
        string formatDate = new Entities.FormatDate().formatDate;
        Entities.FormatDate convertDate = new Entities.FormatDate();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                initPage();
            }
        }

        private void initPage()
        {
            string paraPO_No = Request.QueryString["PO_No"] + "";
            if (paraPO_No != "") { txtPONo.Text = paraPO_No; }
            Session["CountItemPO"] = 0;
            txtDocDate.Text = DateTime.Now.ToString(convertDate.formatDate);
            txtPostingDate.Text = DateTime.Now.ToString(convertDate.formatDate);
            bindData();
        }
        private void bindData()
        {
            string PONo = txtPONo.Text;
            if (PONo == "") { PONo = "X"; }
            bindDataReceive(PONo);
            bindDataHistory(PONo);
            hdfPO_No.Value = PONo;
        }

        private void bindDataReceive(string PONo)
        {
            try
            {
                DASAPConnector sap = new DASAPConnector();
                DataSet ds = sap.GetDataPO(PONo);

                //Class.DAStockReceive clsRec = new Class.DAStockReceive();
                //DataTable dt = clsRec.getDataPODraft(PONo);
                Session["dtPO"] = ds.Tables[0];
                grdData.DataSource = ds.Tables[0];//dt;
                grdData.DataBind();
                Session["CountItemPO"] = ds.Tables[0].Rows.Count;
                if (ds.Tables[0].Rows.Count == 0)
                {
                    //btnAddReceive.Attributes.Add("style", "display:none;");
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "notFoundPO();", true);

                    return;
                }
                else
                {
                    // btnAddReceive.Attributes.Add("style", "");
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "foundPO();", true);
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private void bindDataHistory(string PONo)
        {
            try
            {
                Class.DAStockReceive clsRec = new Class.DAStockReceive();
                DataTable dt = clsRec.getDataReceiveHistory(PONo);
                grdHistory.DataSource = dt;
                grdHistory.DataBind();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        protected void imgPO_Click(object sender, ImageClickEventArgs e)
        {
            bindData();
            if ((int)Session["CountItemPO"] == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ไม่พบข้อมูล PO รหัส " + txtPONo.Text.Trim() + " !');", true);
                return;
            }
        }

        protected void grdData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField EBELN = (HiddenField)e.Row.FindControl("hdfSAP_EBELN");        //	CHAR	10	Purchasing Document Number	
                HiddenField EBELP = (HiddenField)e.Row.FindControl("hdfSAP_EBELP");        //	NUMC	5	Item Number of Purchasing Document	ลำดับ Item ของ SAP 10 , 20 , 30 ,..
                HiddenField BSART = (HiddenField)e.Row.FindControl("hdfSAP_hdfSAP_BSART"); //	CHAR	4	Order Type (Purchasing)	PO Type รอสรุป
                HiddenField BUKRS = (HiddenField)e.Row.FindControl("hdfSAP_BUKRS");        //	CHAR	4	Company Code	com ที่เปิด PO AP:1000
                HiddenField WERKS = (HiddenField)e.Row.FindControl("hdfSAP_WERKS");        //	CHAR	4	Plant	Plant: โครงการ
                HiddenField MATNR = (HiddenField)e.Row.FindControl("hdfSAP_MATNR");        //	CHAR	18	Material Number	รหัส สินค้าของ SAP
                HiddenField TXZ01 = (HiddenField)e.Row.FindControl("hdfSAP_TXZ01");        //	CHAR	40	Short Text	ชื่อสินค้า
                HiddenField MENGE = (HiddenField)e.Row.FindControl("hdfSAP_MENGE_X");        //	QUAN	13	Purchase Order Quantity	จำนวนสั่ง
                HiddenField MENGE_A = (HiddenField)e.Row.FindControl("hdfSAP_MENGE_A");    //	QUAN	13	Purchase Order Quantity	จำนวนที่เหลือ
                HiddenField MEINS = (HiddenField)e.Row.FindControl("hdfSAP_MEINS");        //	UNIT	3	Purchase Order Unit of Measure	หน่วย
                HiddenField NETPR = (HiddenField)e.Row.FindControl("hdfSAP_NETPR");        //	CURR	11	Net price	ราคาต่อหน่วย
                HiddenField NETWR = (HiddenField)e.Row.FindControl("hdfSAP_NETWR");        //	CURR	15	Net Value in Document Currency	ราคารวม
                HiddenField NAVNW = (HiddenField)e.Row.FindControl("hdfSAP_NAVNW");        //	CURR	13	Non-deductible input tax	ภาษี
                HiddenField EFFWR = (HiddenField)e.Row.FindControl("hdfSAP_EFFWR");        //	CURR	13	Effective value of item	ราคารวม Vat
                HiddenField WAERS = (HiddenField)e.Row.FindControl("hdfSAP_WAERS");        //	CUKY	5	Currency Key	หน่วย ไทยบาท
                HiddenField BANFN = (HiddenField)e.Row.FindControl("hdfSAP_BANFN");        //	CHAR	10	Purchase Requisition Number	เลขที่ PR
                HiddenField BNFPO = (HiddenField)e.Row.FindControl("hdfSAP_BNFPO");        //	NUMC	5	Item Number of Purchase Requisition	ลำดับ Item ของ SAP ใน PR 10 , 20 , 30 ,..
                HiddenField KOSTL = (HiddenField)e.Row.FindControl("hdfSAP_KOSTL");        //	CHAR	10	Cost Center	งบประมาณ (จากไหน)
                HiddenField NPLNR = (HiddenField)e.Row.FindControl("hdfSAP_NPLNR");        //	CHAR	12	Order Number	เลข Network ใน SAP
                HiddenField PS_PSP_PNR = (HiddenField)e.Row.FindControl("hdfSAP_PS_PSP_PNR");//	NUMC	8	ไม่ต้องใช้	
                HiddenField WBS_SHOW = (HiddenField)e.Row.FindControl("hdfSAP_WBS_SHOW");  //	CHAR	40	WBS Element Show Text	บ้านแต่หละหลัง

                TextBox grdTxtReceive = (TextBox)e.Row.FindControl("grdTxtReceive");
                TextBox grdTxtPricePerUnit = (TextBox)e.Row.FindControl("grdTxtPricePerUnit");
                HiddenField grdHdfPricePerUnit = (HiddenField)e.Row.FindControl("grdHdfPricePerUnit");
                Label grdLbReceived = (Label)e.Row.FindControl("grdLbReceived");
               
                grdHdfPricePerUnit.Value = (Convert.ToDecimal(EFFWR.Value) / Convert.ToDecimal(MENGE.Value)).ToString("N4") + "";
                grdTxtPricePerUnit.Text = grdHdfPricePerUnit.Value.Split('.')[0] + "." + grdHdfPricePerUnit.Value.Split('.')[1].Substring(0, 2);

                grdTxtReceive.Text = Convert.ToDecimal(MENGE_A.Value).ToString("0"); //(Convert.ToInt32(MENGE.Value) - Convert.ToInt32(hdfReceived.Value)) + "";
                //hdfWaitAmount.Value = MENGE_A.Value;
                grdLbReceived.Text = (Convert.ToDecimal(MENGE.Value) - Convert.ToDecimal(MENGE_A.Value)).ToString("0") + "/" + Convert.ToDecimal(MENGE.Value).ToString("0");
                DropDownList grdDDLItem = (DropDownList)e.Row.FindControl("grdDDLItem");
                Label grdLbItem = (Label)e.Row.FindControl("grdLbItem");
                bindDDLItem(ref grdDDLItem, ref grdLbItem, MATNR.Value);

                grdTxtReceive.Attributes.Add("onkeyup", "chkReceiveChange('" + grdTxtReceive.ClientID + "','" + MENGE_A.ClientID + "')");
            }
        }

        private void bindDDLItem(ref DropDownList grdDDLItem, ref Label grdLbItem, string itemNo)
        {
            Class.DAStockReceive cls = new Class.DAStockReceive();
            DataTable dt = cls.getDataMasterItem(itemNo);
            if (dt.Rows.Count == 0)
            {
                DataTable dtPO = (DataTable)Session["dtPO"];
                Class.DAStockPromotion cls_master = new Class.DAStockPromotion();
                Entities.MasterItemInfo item = new Entities.MasterItemInfo();
                DataRow dr = dtPO.Select("MATNR = '" + itemNo + "'")[0];
                item.ItemNo = itemNo;
                item.ItemName = (string)dr["TXZ01"];
                item.ItemUnit = "";// dr[""];
                decimal NETPR = 0;
                decimal.TryParse((string)dr["NETPR"], out NETPR);
                item.ItemCost = NETPR;

                // (SAP_NAVNW	/	SAP_MENGE)	+	SAP_NETPR	
                decimal SAP_NAVNW = 0; decimal.TryParse((string)dr["NAVNW"], out SAP_NAVNW);
                decimal SAP_MENGE = 0; decimal.TryParse((string)dr["MENGE"], out SAP_MENGE);
                decimal SAP_NETPR = 0; decimal.TryParse((string)dr["NETPR"], out SAP_NETPR);
                item.ItemCostIncVat = (SAP_NAVNW / SAP_MENGE) + SAP_NETPR;

                item.ItemCountMethod = "0"; // dr[""];
                item.ItemStock = "0"; // dr[""];
                item.ItemStatus = "2"; //dr[""];
                item.UpdateBy = "sys"; //dr[""];
                cls_master.InsertDataMasterItem(item);
                // ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ไม่พบข้อมูล Master สินค้าโปรโมชั่นรหัส " + itemNo + " !');", true);
                // return;
                dt = cls.getDataMasterItem(itemNo);
            }
            grdDDLItem.DataSource = dt;
            grdDDLItem.DataValueField = "MasterItemId";
            grdDDLItem.DataTextField = "ItemNoName";
            grdDDLItem.DataBind();

            if (dt.Rows.Count < 2)
            {
                grdLbItem.Text = grdDDLItem.SelectedItem.Text;
                grdLbItem.Visible = true;
                grdDDLItem.Visible = false;
            }
        }

        protected void btnAddReceive_Click(object sender, EventArgs e)
        {
            saveDataReceive();
        }

        private void saveDataReceive()
        {
            /* = Check Header = */
            if (txtRefDocNo.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('กรุณาระบุ Delivery Note !'); $('#" + txtRefDocNo.ClientID + "').focus();", true);
                return;
            }
            DateTime chkDate = new DateTime();
            if (!convertDate.getDateFromString(txtDocDate.Text, ref chkDate))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('Document Date ไม่ถูกต้อง !');  $('#" + txtDocDate.ClientID + "').focus();", true);
                return;
            }
            if (!convertDate.getDateFromString(txtPostingDate.Text, ref chkDate))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('Posting Date ไม่ถูกต้อง !');  $('#" + txtPostingDate.ClientID + "').focus();", true);
                return;
            }

            List<Entities.StockReceiveInfo> lst = new List<Entities.StockReceiveInfo>();

            foreach (GridViewRow gr in grdData.Rows)
            {
                if (gr.RowType == DataControlRowType.DataRow)
                {
                    DropDownList grdDDLItem = (DropDownList)gr.FindControl("grdDDLItem");
                    TextBox grdTxtReceive = (TextBox)gr.FindControl("grdTxtReceive");
                    TextBox grdTxtPricePerUnit = (TextBox)gr.FindControl("grdTxtPricePerUnit");

                    HiddenField EBELN = (HiddenField)gr.FindControl("hdfSAP_EBELN");        //	CHAR	10	Purchasing Document Number	
                    HiddenField EBELP = (HiddenField)gr.FindControl("hdfSAP_EBELP");        //	NUMC	5	Item Number of Purchasing Document	ลำดับ Item ของ SAP 10 , 20 , 30 ,..
                    HiddenField BSART = (HiddenField)gr.FindControl("hdfSAP_BSART"); //	CHAR	4	Order Type (Purchasing)	PO Type รอสรุป
                    HiddenField BUKRS = (HiddenField)gr.FindControl("hdfSAP_BUKRS");        //	CHAR	4	Company Code	com ที่เปิด PO AP:1000
                    HiddenField WERKS = (HiddenField)gr.FindControl("hdfSAP_WERKS");        //	CHAR	4	Plant	Plant: โครงการ
                    HiddenField MATNR = (HiddenField)gr.FindControl("hdfSAP_MATNR");        //	CHAR	18	Material Number	รหัส สินค้าของ SAP
                    HiddenField TXZ01 = (HiddenField)gr.FindControl("hdfSAP_TXZ01");        //	CHAR	40	Short Text	ชื่อสินค้า
                    HiddenField MENGE = (HiddenField)gr.FindControl("hdfSAP_MENGE_X");        //	QUAN	13	Purchase Order Quantity	จำนวนสั่ง
                    HiddenField MENGE_A = (HiddenField)gr.FindControl("hdfSAP_MENGE_A");    //	QUAN	13	Purchase Order Quantity	จำนวนที่เหลือ
                    HiddenField MEINS = (HiddenField)gr.FindControl("hdfSAP_MEINS");        //	UNIT	3	Purchase Order Unit of Measure	หน่วย
                    HiddenField NETPR = (HiddenField)gr.FindControl("hdfSAP_NETPR");        //	CURR	11	Net price	ราคาต่อหน่วย
                    HiddenField NETWR = (HiddenField)gr.FindControl("hdfSAP_NETWR");        //	CURR	15	Net Value in Document Currency	ราคารวม
                    HiddenField NAVNW = (HiddenField)gr.FindControl("hdfSAP_NAVNW");        //	CURR	13	Non-deductible input tax	ภาษี
                    HiddenField EFFWR = (HiddenField)gr.FindControl("hdfSAP_EFFWR");        //	CURR	13	Effective value of item	ราคารวม Vat
                    HiddenField WAERS = (HiddenField)gr.FindControl("hdfSAP_WAERS");        //	CUKY	5	Currency Key	หน่วย ไทยบาท
                    HiddenField BANFN = (HiddenField)gr.FindControl("hdfSAP_BANFN");        //	CHAR	10	Purchase Requisition Number	เลขที่ PR
                    HiddenField BNFPO = (HiddenField)gr.FindControl("hdfSAP_BNFPO");        //	NUMC	5	Item Number of Purchase Requisition	ลำดับ Item ของ SAP ใน PR 10 , 20 , 30 ,..
                    HiddenField KOSTL = (HiddenField)gr.FindControl("hdfSAP_KOSTL");        //	CHAR	10	Cost Center	งบประมาณ (จากไหน)
                    HiddenField NPLNR = (HiddenField)gr.FindControl("hdfSAP_NPLNR");        //	CHAR	12	Order Number	เลข Network ใน SAP
                    HiddenField PS_PSP_PNR = (HiddenField)gr.FindControl("hdfSAP_PS_PSP_PNR");//	NUMC	8	ไม่ต้องใช้	
                    HiddenField WBS_SHOW = (HiddenField)gr.FindControl("hdfSAP_WBS_SHOW");  //	CHAR	40	WBS Element Show Text	บ้านแต่หละหลัง

                    HiddenField LIFNR = (HiddenField)gr.FindControl("hdfSAP_LIFNR");
                    HiddenField VENDOR_NAME = (HiddenField)gr.FindControl("hdfSAP_VENDOR_NAME");
                    HiddenField ZTERM = (HiddenField)gr.FindControl("hdfSAP_ZTERM");

                    AutorizeData auth = new AutorizeData();
                    auth = (AutorizeData)Session["userInfo_" + Session.SessionID];

                    Entities.StockReceiveInfo receive = new Entities.StockReceiveInfo();
                    receive.PO_No = EBELN.Value;
                    receive.Vendor = "";
                    receive.CreateDate = DateTime.Now.ToString(formatDate);
                    receive.CreateBy = auth.EmployeeID;
                    receive.ReceiveHeaderStatus = "1";

                    int itemId = 0;
                    if (grdDDLItem.Items.Count > 0)
                    {
                        if (!int.TryParse(grdDDLItem.SelectedItem.Value, out itemId))
                        {
                            // ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('กรุณาตรวจสอบสินค้ารหัส " + MATNR.Value + "');", true);
                            // itemId = Convert.ToInt32(MATNR.Value);
                            // return;
                        }
                    }
                    decimal ppu = 0;
                    decimal ra = 0;
                    decimal.TryParse(grdTxtReceive.Text, out ra);
                    decimal aa = 0;
                    decimal.TryParse(MENGE.Value, out aa);
                    decimal wa = 0;
                    decimal.TryParse(MENGE_A.Value, out wa);
                    decimal effwr = 0;
                    decimal.TryParse(EFFWR.Value, out effwr);
                    if (ra != 0)
                    {
                        ppu = effwr / aa;
                    }
                    if (ra > wa)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('จำนวน " + TXZ01.Value + " ไม่ถูกต้อง');", true);
                        return;
                    }
                    receive.MasterItemId = itemId;
                    receive.ItemNo = MATNR.Value;
                    receive.PricePerUnit = ppu;
                    receive.ReceiveAmount = ra;
                    receive.Status = "1";

                    receive.UpdateDate = DateTime.Now.ToString(formatDate);
                    receive.UpdateBy = auth.EmployeeID;

                    receive.SAP_EBELN = EBELN.Value;

                    decimal SAP_EBELP = 0;
                    decimal.TryParse(EBELP.Value, out SAP_EBELP);
                    receive.SAP_EBELP = SAP_EBELP;
                    receive.SAP_BSART = BSART.Value;
                    receive.SAP_BUKRS = BUKRS.Value;
                    receive.SAP_WERKS = WERKS.Value;
                    receive.SAP_MATNR = MATNR.Value;
                    receive.SAP_TXZ01 = TXZ01.Value;

                    decimal SAP_MENGE = 0;
                    decimal.TryParse(MENGE.Value, out SAP_MENGE);
                    receive.SAP_MENGE = (int)SAP_MENGE;

                    decimal SAP_MENGE_A = 0;
                    decimal.TryParse(MENGE_A.Value, out SAP_MENGE_A);
                    receive.SAP_MENGE_A = (int)SAP_MENGE_A;

                    receive.SAP_MEINS = MEINS.Value;

                    decimal SAP_NETPR = 0;
                    decimal.TryParse(NETPR.Value, out SAP_NETPR);
                    receive.SAP_NETPR = SAP_NETPR;

                    decimal SAP_NETWR = 0;
                    decimal.TryParse(NETWR.Value, out SAP_NETWR);
                    receive.SAP_NETWR = SAP_NETWR;

                    decimal SAP_NAVNW = 0;
                    decimal.TryParse(NAVNW.Value, out SAP_NAVNW);
                    receive.SAP_NAVNW = SAP_NAVNW;

                    decimal SAP_EFFWR = 0;
                    decimal.TryParse(EFFWR.Value, out SAP_EFFWR);
                    receive.SAP_EFFWR = SAP_EFFWR;

                    decimal SAP_WAERS = 0;
                    decimal.TryParse(WAERS.Value, out SAP_WAERS);
                    receive.SAP_WAERS = SAP_WAERS;
                    receive.SAP_BANFN = BANFN.Value;

                    decimal SAP_BNFPO = 0;
                    decimal.TryParse(BNFPO.Value, out SAP_BNFPO);
                    receive.SAP_BNFPO = SAP_BNFPO;

                    receive.SAP_KOSTL = KOSTL.Value;
                    receive.SAP_NPLNR = NPLNR.Value;

                    decimal SAP_PS_PSP_PNR = 0;
                    decimal.TryParse(PS_PSP_PNR.Value, out SAP_PS_PSP_PNR);
                    receive.SAP_PS_PSP_PNR = SAP_PS_PSP_PNR;
                    receive.SAP_WBS_SHOW = WBS_SHOW.Value;

                    receive.SAP_LIFNR = LIFNR.Value;
                    receive.SAP_VENDOR_NAME = VENDOR_NAME.Value;
                    receive.SAP_ZTERM = ZTERM.Value;


                    /* - 2015.05.12 :: เพิ่ม Posting Date , Doc Date , Doc Ref No.  - */
                    receive.PostingDate = txtPostingDate.Text;
                    receive.DocDate = txtDocDate.Text;
                    receive.DocRefNo = txtRefDocNo.Text;

                    if (receive.ReceiveAmount > 0)
                    {
                        lst.Add(receive);
                    }
                }
            }

            if (lst.Count > 0)
            {
                string msgErr = "";
                DASAPConnector clsSAP = new DASAPConnector();
                if (clsSAP.createGoodsReceipt(lst, ref msgErr))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "saveCompleted('" + hdfPO_No.Value + "','" + lst[0].GR_No + "'); calcTotal();", true);
                    return;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('" + msgErr + "'); calcTotal();", true);
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ไม่พบรายการตรวจรับ');", true);
                return;
            }
        }

        public void prtReportPO(string PO_No)
        {
            DAReport rptConfig = new DAReport();
            string msgErr = "";
            DASAPConnector sap = new DASAPConnector();
            if (sap.createSAPReportPO(PO_No, ref msgErr))
            {
                string pdfPath = rptConfig.getSAPExProt_APD();
                string pdfName = PO_No + ".pdf";
                string sourcePath = pdfPath + "\\" + pdfName;
                string desPath = Server.MapPath("../Report/" + pdfName);
                using (new Impersonation(rptConfig.getSAPExProt_Domain(), rptConfig.getSAPExProt_User(), rptConfig.getSAPExProt_Password()))
                {
                    if (File.Exists(sourcePath))
                    {
                        File.Copy(sourcePath, desPath, true);
                    }
                }
                if (File.Exists(desPath))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "Popup80('" + "../Report/" + pdfName + "');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ไม่พบไฟล์ Report !');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('" + msgErr + "');", true);
            }
        }

        public void prtReportGR(string GR_No, string GRYear)
        {
            DAReport rptConfig = new DAReport();
            string msgErr = "";
            DASAPConnector sap = new DASAPConnector();
            if (sap.createSAPReportGR(GR_No, GRYear, ref msgErr))
            {
                string pdfPath = rptConfig.getSAPExProt_APD();
                string pdfName = GR_No + "" + GRYear + ".pdf";
                string sourcePath = pdfPath + "\\" + pdfName;
                string desPath = Server.MapPath("../Report/" + pdfName);
                using (new Impersonation(rptConfig.getSAPExProt_Domain(), rptConfig.getSAPExProt_User(), rptConfig.getSAPExProt_Password()))
                {
                    if (File.Exists(sourcePath))
                    {
                        File.Copy(sourcePath, desPath, true);
                    }
                }
                if (File.Exists(desPath))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "Popup80('" + "../Report/" + pdfName + "');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ไม่พบไฟล์ Report !');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('" + msgErr + "');", true);
            }
        }

        protected void imgPrtPO_Click(object sender, ImageClickEventArgs e)
        {
            prtReportPO(hdfPO_No.Value);
        }

        protected void grdHistory_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "PrtGR")
            {
                GridViewRow gvr = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                HiddenField hdfGR_No = (HiddenField)gvr.FindControl("hdfGR_No");
                HiddenField hdfGR_Year = (HiddenField)gvr.FindControl("hdfGR_Year");
                string GR_No = hdfGR_No.Value;
                string GRYear = hdfGR_Year.Value;

                DAReport rptConfig = new DAReport();

                string msgErr = "";
                DASAPConnector sap = new DASAPConnector();
                if (sap.createSAPReportGR(GR_No, GRYear, ref msgErr))
                {
                    string pdfPath = rptConfig.getSAPExProt_APD();
                    string pdfName = GR_No + "" + GRYear + ".pdf";
                    string sourcePath = pdfPath + "\\" + pdfName;
                    string desPath = Server.MapPath("../Report/" + pdfName);
                    using (new Impersonation(rptConfig.getSAPExProt_Domain(), rptConfig.getSAPExProt_User(), rptConfig.getSAPExProt_Password()))
                    {
                        if (File.Exists(sourcePath))
                        {
                            File.Copy(sourcePath, desPath, true);
                        }
                    }
                    if (File.Exists(desPath))
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "js", "Popup80('" + "../Report/" + pdfName + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ไม่พบไฟล์ Report !');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('" + msgErr + "');", true);
                }
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("StockReceive.aspx?sCond=Y");
        }


    }
}