using AP_StockPromotion_V1.Class;
using AP_StockPromotion_V1.ws_authorize;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;

namespace AP_StockPromotion_V1.webpage
{
    public partial class DeliveryPostSum : Page
    {
        Entities.FormatDate convertDate = new Entities.FormatDate();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                txtDate.Text = DateTime.Now.ToString(convertDate.formatDate);

                string DelvLst = Request.QueryString["DelvLstId"] + "";
                DADelivery cls = new DADelivery();

                DataTable dt = null;
                bool isEqual = cls.CheckEqualtation(DelvLst);
                if (isEqual)
                {
                    Session["isEqual"] = true;
                }
                else
                {
                    Session["isEqual"] = false;
                }

                dt = cls.GetDataDeliveryItemFromDelvLst(DelvLst);
                Session["Posting"] = dt;

                DataTable PCRTHO = dt.DefaultView.ToTable(true, "CompanySAPCode", "ProfitCenterHO");
                if (PCRTHO.Rows.Count != 1)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('กรุณาเลือกข้อมูลใบส่งมอบจาก Company เดียวกันเท่านั้น !!'); bindDataParentPage();", true);
                    return;
                }
                else if ((PCRTHO.Rows[0]["ProfitCenterHO"] ?? "").ToString() == "")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", string.Format("alert('ไม่พบ ProfitCode ของ Company {0} !!'); bindDataParentPage();", PCRTHO.Rows[0]["CompanySAPCode"] ?? ""), true);
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

                DASAPConnector sap = new DASAPConnector();
                // ItemNo	ItemName	ItemId	Price	CostCenter AND ISNULL(isNonePR,'') = '" + drsum["isNonePR"] + "'
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
                    string ItemText = preTxt + "" + drTotal["ItemName"] + " " + (int)dt.Compute("COUNT(WBS_SAP)", cond); // + " " + DateTime.Now.ToString("MM.yyyy");
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
                    dr["ProfitCenter"] = hdfProfitHO.Value;
                    dr["TotalCredit"] = (-1) * (decimal)dt.Compute("SUM(Price)", cond);
                    dr["Total"] = DBNull.Value;
                    dtTotal.Rows.Add(dr);
                }
                dtx.Dispose(); dtx = null;
                dtTotal.AcceptChanges();
                grdDataSum.DataSource = dtTotal;
                grdDataSum.DataBind();

                DataSet ds = new DataSet();
                dtTotal.TableName = "WBS";
                ds.Tables.Add(dtTotal.Copy());
                Session["dsPstAcc"] = ds;
            }
        }

        protected void btnPostAccount_Click(object sender, EventArgs e)
        {

            //--------------------------------------- น็อต -- ตรวจสอบ PRItem ก่อนส่งไป post เนื่องจาก CRM ส่ง PRitem มาผิดจึงต้องซ่อมให้(ปรึกษาพี่นนแล้ว)---------------------------------------------
            string DelvLst = Request.QueryString["DelvLstId"] + "";

            string[] DelvLst_List = DelvLst.Split(',');

            foreach (string DelvLst_Tmp in DelvLst_List)
            {

                DADelivery clsda = new DADelivery();
                DataTable dtCheckPOItem = clsda.GetDataDeliveryItemFromDelvLst_Check(DelvLst);
                for (int i = 0; i < dtCheckPOItem.Rows.Count; i++)
                {
                    DASAPConnector sSap = new DASAPConnector();
                    DataTable dtSap = new DataTable();

                    string MsgErr = string.Empty;
                    string ItemNo = dtCheckPOItem.Rows[i]["ItemNo"].ToString();
                    string PRNo = dtCheckPOItem.Rows[i]["PRNo"].ToString();
                    string PRItem = dtCheckPOItem.Rows[i]["PRItem"].ToString();
                    string Amount = Convert.ToDecimal(dtCheckPOItem.Rows[i]["Amount"]).ToString("0.000");
                    string SapAmount = string.Empty;
                    string OldItemNo = "";

                    //--------------------------------------- คิม -- หา ItemNo ก่อนการทำ เทียบเท่า แล้วเอาไปใช้หา PRItem ใน SAP ---------------------------------------------

                    int ReqId = int.Parse((dtCheckPOItem.Rows[i]["ReqId"] ?? "0").ToString());

                    DataTable dtOldItem = clsda.getRequestDetailById(ReqId);
                    if (dtOldItem.Rows.Count > 0)
                    {
                        if (!dtOldItem.Rows[0]["ItemNo"].ToString().Equals(ItemNo) && !string.IsNullOrEmpty(dtOldItem.Rows[0]["ItemNo"].ToString()))
                        {
                            OldItemNo = dtOldItem.Rows[0]["ItemNo"].ToString();
                        }
                    }

                    //--------------------------------------- คิม -- หา ItemNo ก่อนการทำ เทียบเท่า แล้วเอาไปใช้หา PRItem ใน SAP ---------------------------------------------

                    sSap.SAPGetPRDetail(PRNo, ref dtSap, ref MsgErr);
                    if (dtSap != null)
                    {
                        if (dtSap.Rows.Count > 0)
                        {
                            DataTable dtGroup = new DataTable();
                            DataRow[] drGroup = dtSap.Select("MATERIAL = '" + (OldItemNo == "" ? ItemNo : OldItemNo) + "' AND PREQ_ITEM = '" + PRItem + "' AND QUANTITY = '" + Amount + "' AND DELETE_IND <> 'X'");//ตรวจ PRITEM ถ้าพบแสดงว่าถูกต้องแล้ว
                            if (drGroup.Length > 0)
                            {
                                dtGroup = drGroup.CopyToDataTable();
                            }

                            //----------------- Check PRITEM ---------------------
                            if (dtGroup.Rows.Count <= 0) //ถ้าไม่เจอข้อมูลใน Sap ที่ PR,MATERIAL,PRITEM,QUANTITY ตรงกัน แสดงว่า CRM ส่ง PRITEM มาผิด
                            {
                                DataTable dtGroup2 = new DataTable();
                                DataRow[] drGroup2 = dtSap.Select("MATERIAL = '" + (OldItemNo == "" ? ItemNo : OldItemNo) + "' AND QUANTITY = '" + Amount + "'");//ถ้าไม่พบจากด้านบนให้หาด้วย Material,Quantity 
                                if (drGroup2.Length > 0)
                                {
                                    dtGroup2 = drGroup2.CopyToDataTable();
                                }

                                //if (dtGroup2.Rows.Count > 1)// มากกว่า 1 แสดงว่ามีสินค้าชนิดเดียวกันทำเบิกมากกว่า 1 รายการใน PR ใบเดียวกัน Alert เตือนเพื่อให้ตรวจสอบเพราะไม่แน่ใจว่าตัวไหนที่ถูกต้อง
                                //{
                                //    string Msg = string.Empty;
                                //    for (int a = 0; a < dtGroup2.Rows.Count; a++)
                                //    {
                                //        Msg = Msg + "(MATERIAL:" + dtGroup2.Rows[a]["MATERIAL"].ToString() + ",PRItem:" + dtGroup2.Rows[a]["PREQ_ITEM"].ToString() + ",QUANTITY:" + dtGroup2.Rows[a]["QUANTITY"].ToString() + ")";
                                //    }
                                //    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('พบข้อมูลที่ PRItem ไม่ถูกต้อง(มีสินค้าชนิดเดียวกันทำเบิกมากกว่า 1 รายการ ใน PR เดียวกัน) PRNo:" + PRNo + "กรุณาติดต่อเจ้าหน้าที่ IT !!รายละเอียด" + Msg + "'); bindDataParentPage();", true);
                                //    return;
                                //}
                                //else 
                                if (dtGroup2.Rows.Count == 1)
                                {
                                    //ทำการ Update
                                    if (PRNo.Trim() != "" && ItemNo != "" && dtGroup2.Rows[0]["PREQ_ITEM"].ToString().Trim() != "")
                                    {
                                        string Msg = clsda.UpdatePRItemFromSap(DelvLst_Tmp, PRNo, ItemNo, dtGroup2.Rows[0]["PREQ_ITEM"].ToString());
                                        if (Msg.Trim() != "")
                                        {
                                            ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('เกิดความผิดพลาดขั้นตอนทำการบันทึกข้อมูล PRItem ที่ไม่ตรงกับ Sap PRNO:" + PRNo + ", Material:" + ItemNo + ", PRItem(SAP):" + dtGroup2.Rows[0]["PREQ_ITEM"].ToString() + " MsgErr:" + Msg + "'); bindDataParentPage();", true);
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ตรวจสอบพบค่า PRItem ไม่ตรงกับใน Sap และเกิดความผิดพลาดก่อน Update เนื่องจากมีค่าบางค่าเป็นค่าว่าง PRNO:" + PRNo + ", Material:" + ItemNo + ", PRItem(SAP):" + dtGroup2.Rows[0]["PREQ_ITEM"].ToString() + "'); bindDataParentPage();", true);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    //-----------------Check PRITEM---------------------
                }

                //return;

                DataTable dtGetRefreshData = new DataTable();
                dtGetRefreshData = clsda.GetDataDeliveryItemFromDelvLst(DelvLst);
                if (dtGetRefreshData != null)
                {
                    if (dtGetRefreshData.Rows.Count > 0)
                    {
                        Session["Posting"] = dtGetRefreshData;
                    }
                }

            }
            //--------------------------------------- น็อต -- ตรวจสอบ PRItem ก่อนส่งไป post เนื่องจาก CRM ส่ง PRitem มาผิดจึงต้องซ่อมให้(ปรึกษาพี่นนแล้ว)---------------------------------------------

            bool isEqual = (Session["isEqual"] == null ? false : (bool)Session["isEqual"]);

            if (txtReference.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('" + "กรุณาระบุ Reference !!" + "'); ", true); return;
            }
            if (txtRefKey3.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('" + "กรุณาระบุ Ref Key 3 !!" + "'); ", true); return;
            }

            DASAPConnector sap = new DASAPConnector();
            Entities.SapDelivery_DOCUMENTHEADER dochdr = new Entities.SapDelivery_DOCUMENTHEADER();
            List<Entities.SapDelivery_ACCOUNTGL> lstAccGL = new List<Entities.SapDelivery_ACCOUNTGL>();
            List<Entities.SapDelivery_CURRENCYAMOUNT> lstCurAmt = new List<Entities.SapDelivery_CURRENCYAMOUNT>();
            Entities.SapDelivery_ACCOUNTGL AccGL;
            Entities.SapDelivery_CURRENCYAMOUNT CurAmt;
            DADelivery dad = new DADelivery();
            AutorizeData auth = (AutorizeData)Session["userInfo_" + Session.SessionID];
            dochdr.USERNAME = auth.EmployeeID;
            dochdr.COMP_CODE = hdfCompanySAPCode.Value;
            dochdr.DOC_DATE = txtDate.Text; 
            dochdr.PSTNG_DATE = txtDate.Text;
            DateTime sDate = new DateTime();
            convertDate.getDateFromString(txtDate.Text, ref sDate);
            dochdr.FISC_YEAR = Convert.ToInt16(sDate.ToString("yyyy"));
            dochdr.DOC_TYPE = "IA";
            dochdr.REF_DOC_NO = txtReference.Text;
            dochdr.REF_KEY_3 = txtRefKey3.Text;

            string msgErr = "";
            DataTable dt = (DataTable)Session["Posting"];
            dad.WriteLogFile("[" + DateTime.Now.ToString() + "]----------------------------------DeliveryPostSum----------------------------------------");

            /* - ลองลบ PR - */
            dad.WriteLogFile("btnPostAccount_Click(ลองลบPR):----------------------------------------");
            if (!DelPR("X", ref msgErr)) { ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('" + msgErr.Replace("'", "") + "'); ", true); return; }
            dad.WriteLogFile("MSG:=" + msgErr);
            dad.WriteLogFile("btnPostAccount_Click(ลองลบPR):----------------------------------------");

            /* - ลบ PR จริง - */
            dad.WriteLogFile("btnPostAccount_Click(ลบPRจริง):----------------------------------------");
            if (!DelPR("", ref msgErr)) { ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('" + msgErr.Replace("'", "") + "'); ", true); return; }
            dad.WriteLogFile("MSG:=" + msgErr);
            dad.WriteLogFile("btnPostAccount_Click(ลบPRจริง):----------------------------------------");

            try
            {
                /* - Post บช. - */
                /* ItemNo	ItemName	CostCenter	DelvDate    ItemId	Price */
                dt.DefaultView.RowFilter = "";
                // DataTable dtPost = dt.DefaultView.ToTable(true, "ItemNo", "ItemName", "ProjectID", "CostCenter", "DelvDate", "WBS_SAP", "isNonePR", "PO_No", "PO_Item");
                DataTable dtPost = dt.DefaultView.ToTable(true, "ItemNo", "ItemName", "ProjectID", "CostCenter", "WBS_SAP", "isNonePR", "PO_No", "PO_Item");
                dtPost.Columns.Add(new DataColumn("Amount", typeof(int)));
                dtPost.Columns.Add(new DataColumn("Total", typeof(decimal)));
                foreach (DataRow drPost in dtPost.Rows)
                {
                    string cond = "ISNULL(ItemNo,'') = '" + drPost["ItemNo"] + "' AND ISNULL(ItemName,'') = '" + drPost["ItemName"] + "' AND ISNULL(ProjectID,'') = '" + drPost["ProjectID"] + "' AND ISNULL(CostCenter,'') = '" + drPost["CostCenter"] + "' AND ISNULL(WBS_SAP,'') = '" + drPost["WBS_SAP"] + "' AND ISNULL(isNonePR,'') = '" + drPost["isNonePR"] + "' AND ISNULL(PO_No,'') = '" + drPost["PO_No"] + "'";
                    if (drPost["PO_Item"] == DBNull.Value) { cond += " AND PO_Item IS NULL "; }
                    else { cond += " AND PO_Item = " + drPost["PO_Item"] + ""; }

                    drPost["Amount"] = dt.Compute("COUNT(ItemId)", cond);
                    drPost["Total"] = dt.Compute("SUM(Price)", cond);
                }
                dtPost.AcceptChanges();

                int ii = 0; // Debit
                foreach (DataRow dr in dtPost.Rows)
                {
                    ii++;
                    AccGL = new Entities.SapDelivery_ACCOUNTGL();
                    AccGL.ITEMNO_ACC = ii;
                    AccGL.GL_ACCOUNT = sap.getGLNoNormal();
                    string preTxt = ""; if (dr["isNonePR"] + "" == "Y") { preTxt = "O-"; }
                    AccGL.ITEM_TEXT = preTxt + dr["ItemName"] + " " + dr["Amount"]; 
                    AccGL.REF_KEY_2 = sap.getProfitCenterByProjectCode(dr["ProjectID"] + ""); 
                    AccGL.REF_KEY_3 = txtRefKey3.Text;
                    string PO_Item = "00000" + dr["PO_Item"];
                    AccGL.ALLOC_NMBR = dr["PO_No"] + "" + PO_Item.Substring(PO_Item.Length - 5, 5);
                    AccGL.COSTCENTER = "00000" + dr["CostCenter"]; 
                    AccGL.WBS_ELEMENT = dr["WBS_SAP"] + "";
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
                    AccGL = new Entities.SapDelivery_ACCOUNTGL(); // Credit
                    AccGL.ITEMNO_ACC = ii;
                    AccGL.GL_ACCOUNT = sap.getGLNoCredit();
                    string preTxt = ""; if (dr["isNonePR"] + "" == "Y") { preTxt = "O-"; }
                    AccGL.ITEM_TEXT = preTxt + dr["ItemName"] + " " + dr["Amount"];
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
                DASAPConnector cls = new DASAPConnector();
                dad.WriteLogFile("btnPostAccount_Click(ส่ง Postบัญชีจริง):----------------------------------------");

                bool rstPost = cls.deliveryPostAccount(delvListNo, dochdr, lstAccGL, lstCurAmt, auth.EmployeeID, ref SAPDocNo, ref msgErr);

                if (rstPost)
                {
                    Session["selDelv"] = "";
                    Session["SAPDOC"] = SAPDocNo;
                    string urlRptPstAccIO = "frmReport.aspx?reqRPT=PstAccWBS";
                    dad.WriteLogFile("btnPostAccount_Click:[บันทึกบัญชีสำเร็จ]");
                    dad.WriteLogFile("btnPostAccount_Click(ส่งPostบัญชีจริง):----------------------------------------");
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "Popup60('" + urlRptPstAccIO + "'); bindDataParentPage();", true);
                    return;
                }
                else
                {

                    /* - ยกเลิกลบ PR จริง - */
                    if (!CancelDelPR("", ref msgErr)) { /* return; */ }
                    dad.WriteLogFile("btnPostAccount_Click(ส่งPostบัญชีจริงเกิดข้อผิดพลาดทำการยกเลิกDeletePR):----------------------------------------");
                    dad.WriteLogFile("MSG:=" + msgErr);
                    dad.WriteLogFile("btnPostAccount_Click(ส่งPostบัญชีจริง):----------------------------------------");
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('บันทึกบัญชีผิดพลาด !! " + msgErr.Replace("'", "") + "');", true);
                    return;
                }
            }
            catch (Exception ex)
            {
                /* - ยกเลิกลบ PR จริง - */
                CancelDelPR("", ref msgErr);
                dad.WriteLogFile("btnPostAccount_Click(บันทึกบัญชีผิดพลาด Catch Exception):----------------------------------------");
                dad.WriteLogFile("MSG:=" + msgErr);
                dad.WriteLogFile("btnPostAccount_Click(บันทึกบัญชีผิดพลาด Catch Exception):----------------------------------------");
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('บันทึกบัญชีผิดพลาด !! " + ex.Message.Replace("'", "") + "');", true);
                return;
            }
        }

        private bool DelPR(string isTest, ref string msgErr)
        {
            bool rst = true;
            DASAPConnector sap = new DASAPConnector();
            DataTable dtPRDetail = null;
            DataTable dtPosting = (DataTable)Session["Posting"];
            DataTable dtPR = dtPosting.DefaultView.ToTable(true, "PRNo");
            DataTable dtSapPrDetail_X = null;

            DataTable dtDelPR = dtPosting.DefaultView.ToTable(true, "PRNo", "PRItem", "ItemNo", "RefMatId");
            dtDelPR.Columns.Add(new DataColumn("DelvAmount", typeof(decimal)));
            dtDelPR.Columns.Add(new DataColumn("PRAmount", typeof(decimal)));
            dtDelPR.Columns.Add(new DataColumn("Balance", typeof(decimal)));
            dtDelPR.Columns.Add(new DataColumn("PRBalance", typeof(decimal)));
            dtDelPR.Columns.Add(new DataColumn("xPRBalance", typeof(string)));
            dtDelPR.Columns.Add(new DataColumn("isDel", typeof(string)));
            dtDelPR.Columns.Add(new DataColumn("MoneyPost", typeof(decimal)));
            dtDelPR.Columns.Add(new DataColumn("MoneyBalance", typeof(decimal)));
            dtDelPR.Columns.Add(new DataColumn("PRItemFromSap", typeof(string)));

            foreach (DataRow drPr in dtPR.Rows)
            {

                string prNo = drPr["PRNo"] + "";
                if (!sap.SAPGetPRDetail(prNo, ref dtSapPrDetail_X, ref msgErr))
                {
                    return false;
                }
                else
                {
                    if (dtPRDetail == null)
                    {
                        dtPRDetail = dtSapPrDetail_X.Copy();
                        dtPRDetail.Columns.Add(new DataColumn("PRNo", typeof(string)));
                        foreach (DataRow dr in dtPRDetail.Rows) { dr["PRNo"] = prNo; }
                    }
                    else
                    {
                        foreach (DataRow drSapPrDetail_X in dtSapPrDetail_X.Rows)
                        {
                            dtPRDetail.ImportRow(drSapPrDetail_X);
                            dtPRDetail.Rows[dtPRDetail.Rows.Count - 1]["PRNo"] = prNo;
                        }
                    }

                    if (dtSapPrDetail_X != null)
                    {
                        if (dtSapPrDetail_X.Rows.Count > 1)//ถ้าใน PR เดียวกัน เจอ ITem มากกว่า 1 ให้เอามาใส่ค่าใน field PRItemFromSap 08/11/2017
                        {
                            //ในส่วนนี้เพิ่มขึ้นมาเนื่องจากพบเคสที่ว่าจ่ายแบบเทียบเท่า แล้วระบบเตรียมข้อมูลเพื่อส่งเข้า delete PR แล้วบังเอิญ รายการที่เทียบเท่านั้น เป็น PRITEM เดียวกันใน datatable ที่ส่งเข้าไปทำให้ Sap ไม่รับ
                            // วิธีการคือจะต้องเอาค่า PRITEM ที่ได้จาก Sap ส่งเข้าไป delete ตัวเก่าใน Sap โดยกำหนดค่าให้ใน DataTable นี้ 08/11/2017
                            for (int i = 0; i < dtSapPrDetail_X.Rows.Count; i++)
                            {
                                for (int j = 0; j < dtDelPR.Rows.Count; j++)
                                {

                                    string refMatID = "";
                                    DataTable tmpDt = new DataTable();

                                    var tmpRow = dtPosting.Select("PRNo = '" + prNo + "'");

                                    if (tmpRow.Any())
                                    {
                                        tmpDt = tmpRow.CopyToDataTable();
                                    }

                                    if (tmpDt.Rows.Count > 0)
                                    {
                                        DataTable tmp = new DataTable();

                                        tmpRow = tmpDt.Select("ItemNo = '" + dtDelPR.Rows[j]["ItemNo"].ToString() + "'");
                                        if (tmpRow.Any())
                                        {
                                            tmp = tmpRow.CopyToDataTable();
                                        }
                                        if (tmp.Rows.Count > 0)
                                        {
                                            refMatID = "000000000000000000" + tmp.Rows[0]["RefMatID"].ToString();
                                            refMatID = refMatID.Substring(refMatID.Length - 18, 18);
                                        }
                                    }

                                    if (dtDelPR.Rows[j]["PRNo"].ToString() == prNo && ((refMatID != "") ? refMatID : dtDelPR.Rows[j]["ItemNo"].ToString()) == dtSapPrDetail_X.Rows[i]["MATERIAL"].ToString())
                                    {
                                        dtDelPR.Rows[j][12] = dtSapPrDetail_X.Rows[i]["PREQ_ITEM"].ToString();
                                    }
                                }

                            }
                        }
                    }

                }
            }

            dtPRDetail.AcceptChanges();

            dtPosting.DefaultView.RowFilter = "";
            DataTable _dt = dtPosting.AsEnumerable().GroupBy(q => new { PRItem = q["PRItem"], Price = q["Price"] }).Select(o => o.OrderBy(q => q["PRItem"]).First()).CopyToDataTable();


            foreach (DataRow drDelPR in dtDelPR.Rows)
            {
                drDelPR["DelvAmount"] = (int)dtPosting.Compute("Count(PRNo)", "PRNo = '" + drDelPR["PRNo"] + "' AND PRItem = '" + drDelPR["PRItem"] + "' AND ItemNo = '" + drDelPR["ItemNo"] + "'");
                drDelPR["MoneyPost"] = (decimal)dtPosting.Compute("SUM(Price)", "PRNo = '" + drDelPR["PRNo"] + "' AND PRItem = '" + drDelPR["PRItem"] + "' AND ItemNo = '" + drDelPR["ItemNo"] + "'");

                DataRow[] drPrDetail = dtPRDetail.Select("PRNo = '" + drDelPR["PRNo"] + "' and PREQ_ITEM = '" + drDelPR["PRItem"] + "'");
                if (drPrDetail.Length != 1)
                {
                    msgErr = "ไม่พบข้อมูล PR " + drDelPR["PRNo"] + " PREQ_ITEM " + drDelPR["PREQ_ITEM"];
                    return false;
                }

                drDelPR["PRAmount"] = drPrDetail[0]["QUANTITY"];
                drDelPR["Balance"] = (decimal)drDelPR["PRAmount"] - (decimal)drDelPR["DelvAmount"];

                ///2017.07.06 ตรวจสอบว่าถ้าเป็นเคสจ่ายสินค้าทดแทน ไม่ต้องทำการตรวจสอบเรื่องจำนวน ให้ทำการ delete pr ได้เลย
                //แต่เนื่องจากเงื่อนไขเดิมของโปรแกรมนั้นทำไว้โดยตรวจสอบทั้ง จำนวน และ ยอดเงินทำให้เมื่อคำนวนจำนวนผิด
                //จะมีการไปทำรายการลดจำนวนในระบบ SAP จึงตรวจสอบและยืนยันกับ (พี่โอ๊ต) ทีม SAP ว่าให้ดำเนินการ delete PR ได้ทันที
                // แต่ยังคงให้มีการตรวจสอบยอดเงินอยู่ จึงต้อง Set Balance = 0 เพื่อวิ่ง flow ปกติเหมือนไม่ให้ทำการลดจำนวน
                if (drDelPR["RefMatId"] != null)
                {
                    if (drDelPR["RefMatId"].ToString().Trim() != "")
                    {
                        drDelPR["Balance"] = 0;
                    }
                }

                decimal VALUE_ITEM = 0;
                if (!decimal.TryParse(drPrDetail[0]["VALUE_ITEM"] + "", out VALUE_ITEM))
                {
                    msgErr = "ไม่พบข้อมูล PR " + drDelPR["PRNo"] + " PREQ_ITEM " + drDelPR["PREQ_ITEM"] + " ที่ VALUE_ITEM";
                    return false;
                }
                drDelPR["MoneyBalance"] = VALUE_ITEM - (decimal)drDelPR["MoneyPost"];

                if ((decimal)drDelPR["Balance"] < 0)
                {
                    msgErr = "ไม่สามารถยกเลิก PR กันงบได้ เนื่องจากปริมาณสินค้าไม่เพียงพอ !! จำนวนในSap:" + drPrDetail[0]["QUANTITY"] + ",จำนวนที่Stockขอเบิก:" + drDelPR["DelvAmount"];
                    return false;
                }
               
                if ((decimal)drDelPR["Balance"] == 0)
                {
                    drDelPR["xPRBalance"] = "";
                    drDelPR["isDel"] = "X";
                }
                else
                if ((decimal)drDelPR["Balance"] > 0)
                {
                    drDelPR["PRBalance"] = drDelPR["Balance"];
                    drDelPR["xPRBalance"] = "X";
                    drDelPR["isDel"] = "";
                }
            }
            dtDelPR.AcceptChanges();

            dtPR = dtDelPR.DefaultView.ToTable(true, "PRNo");
            foreach (DataRow drPR in dtPR.Rows)
            {
                /*/ - 2016.06.30 : เอาส่วน release PR จากข้างบนลงมาไว้ตรงนี้ - /*/
                /*/ - - - - - - - - - - - - - - - - - - - - - - - - - -/*/
                dtSapPrDetail_X = null;
                string prNo = drPR["PRNo"] + "";
                if (!sap.SAPGetPRDetail(prNo, ref dtSapPrDetail_X, ref msgErr))
                {
                    return false;
                }
                else
                {
                    if (dtSapPrDetail_X.Rows[0]["REL_STATUS"] + "" == "X")
                    {
                        if (!sap.SAPUnReleasePR(prNo, ref msgErr))
                        {
                            return false;
                        }
                    }
                }

                dtDelPR.DefaultView.RowFilter = "PRNo = '" + drPR["PRNo"] + "'";
                if (!sap.SAPChangeStatusPR(drPR["PRNo"] + "", isTest, dtDelPR.DefaultView.ToTable(), ref msgErr))
                {
                    return false;
                }
            }

            return rst;
        }

        private bool CancelDelPR(string isTest, ref string msgErr)
        {
            bool rst = true;
            DASAPConnector sap = new DASAPConnector();
            DataTable dtPRDetail = null;
            DataTable dtPosting = (DataTable)Session["Posting"];
            DataTable dtPR = dtPosting.DefaultView.ToTable(true, "PRNo");
            foreach (DataRow drPr in dtPR.Rows)
            {
                DataTable dtSapPrDetail_X = null;
                string prNo = drPr["PRNo"] + "";
                if (!sap.SAPGetPRDetail(prNo, ref dtSapPrDetail_X, ref msgErr))
                {
                    return false;
                }
                else
                {
                    if (dtPRDetail == null)
                    {
                        dtPRDetail = dtSapPrDetail_X.Copy();
                        dtPRDetail.Columns.Add(new DataColumn("PRNo", typeof(string)));
                        foreach (DataRow dr in dtPRDetail.Rows) { dr["PRNo"] = prNo; }
                    }
                    else
                    {
                        foreach (DataRow drSapPrDetail_X in dtSapPrDetail_X.Rows)
                        {
                            dtPRDetail.ImportRow(drSapPrDetail_X);
                        }
                    }
                }
            }
            dtPRDetail.AcceptChanges();

            dtPosting.DefaultView.RowFilter = "";
            DataTable dtDelPR = dtPosting.DefaultView.ToTable(true, "PRNo", "PRItem", "ItemNo");
            dtDelPR.Columns.Add(new DataColumn("DelvAmount", typeof(decimal)));
            dtDelPR.Columns.Add(new DataColumn("PRAmount", typeof(decimal)));
            dtDelPR.Columns.Add(new DataColumn("Balance", typeof(decimal)));
            dtDelPR.Columns.Add(new DataColumn("PRBalance", typeof(decimal)));
            dtDelPR.Columns.Add(new DataColumn("xPRBalance", typeof(string)));
            dtDelPR.Columns.Add(new DataColumn("isDel", typeof(string)));
            dtDelPR.Columns.Add(new DataColumn("PRItemFromSap", typeof(string)));
            foreach (DataRow drDelPR in dtDelPR.Rows)
            {
                drDelPR["DelvAmount"] = (int)dtPosting.Compute("Count(PRNo)", "PRNo = '" + drDelPR["PRNo"] + "' AND PRItem = '" + drDelPR["PRItem"] + "' AND ItemNo = '" + drDelPR["ItemNo"] + "'");
                DataRow[] drPrDetail = dtPRDetail.Select("PRNo = '" + drDelPR["PRNo"] + "' and PREQ_ITEM = '" + drDelPR["PRItem"] + "'");
                if (drPrDetail.Length != 1)
                {
                    msgErr = "ไม่พบข้อมูล PR " + drDelPR["PRNo"] + " PREQ_ITEM " + drDelPR["PRItem"];
                    return false;
                }
                drDelPR["PRAmount"] = drPrDetail[0]["QUANTITY"];
                string DELETE_IND = "" + drPrDetail[0]["DELETE_IND"];
                if (DELETE_IND == "X")
                {
                    drDelPR["isDel"] = "";
                    drDelPR["xPRBalance"] = "";
                }
                else
                {
                    drDelPR["isDel"] = "";
                    drDelPR["xPRBalance"] = "X";
                    drDelPR["PRBalance"] = (decimal)drDelPR["PRAmount"] - (decimal)drDelPR["DelvAmount"];
                }
            }
            dtDelPR.AcceptChanges();

            dtPR = dtDelPR.DefaultView.ToTable(true, "PRNo");
            foreach (DataRow drPR in dtPR.Rows)
            {
                dtDelPR.DefaultView.RowFilter = "PRNo = '" + drPR["PRNo"] + "'";
                if (!sap.SAPChangeStatusPR(drPR["PRNo"] + "", isTest, dtDelPR.DefaultView.ToTable(), ref msgErr))
                {
                    return false;
                }
            }
            return rst;
        }

        protected void BtnReversePR_Click(object sender, EventArgs e)
        {
            string DelvLst = Request.QueryString["DelvLstId"] + "";
            DADelivery cls = new DADelivery();

            DataTable dt = new DataTable();
            dt = cls.GetDataDeliveryItemFromDelvLst(DelvLst);
            Session["Posting"] = dt;
            string msg = "";
            CancelDelPR("", ref msg);
            if (msg.Trim() != "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ถอย Delete PR ไม่สำเร็จ" + msg + "!!'); bindDataParentPage();", true);
            }
        }

    }
}