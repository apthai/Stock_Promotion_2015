using AP_StockPromotion_V1.Class;
using AP_StockPromotion_V1.ws_authorize;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;

namespace AP_StockPromotion_V1.webpage
{
    public partial class DeliveryPostSumCrossCmp : Page
    {
        Entities.FormatDate convertDate = new Entities.FormatDate();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                txtDate.Text = DateTime.Now.ToString(convertDate.formatDate);

                string DelvLst = Request.QueryString["DelvLstId"] + "";
                DADelivery cls = new DADelivery();
                DataTable dt = cls.GetDataDeliveryItemFromDelvLst(DelvLst);
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

                grdAccountsPayable.DataSource = getDataAccountsPayable(dt);
                grdAccountsPayable.DataBind();
                grdDataPostAccount.DataSource = getDataPostAccounts(dt);
                grdDataPostAccount.DataBind();
                grdAccountsReceivable.DataSource = getDataAccountsReceivable(dt);
                grdAccountsReceivable.DataBind();

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

        protected void btnPostAccount_Click(object sender, EventArgs e)
        {
            if (txtReference.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('" + "กรุณาระบุ Reference !!" + "'); ", true); return;
            }

            AutorizeData auth = (AutorizeData)Session["userInfo_" + Session.SessionID];

            Entities.SapDeliveryCrossCmpStep1_DOCUMENTHEADER hs1 = getSapDeliveryCrossCmpStep1_DOCUMENTHEADER();
            List<Entities.SapDeliveryCrossCmpStep1_ACCOUNTGL> AccGLS1 = new List<Entities.SapDeliveryCrossCmpStep1_ACCOUNTGL>();
            List<Entities.SapDeliveryCrossCmpStep1_ACCOUNTPAYABLE> AccPaS1 = new List<Entities.SapDeliveryCrossCmpStep1_ACCOUNTPAYABLE>();
            List<Entities.SapDeliveryCrossCmpStep1_CURRENCYAMOUNT> CurAmtS1 = new List<Entities.SapDeliveryCrossCmpStep1_CURRENCYAMOUNT>();
            int ii = 0;
            getSapDeliveryCrossCmpStep1_Debit(ref ii, ref AccGLS1, ref CurAmtS1);
            getSapDeliveryCrossCmpStep1_Credit(ref ii, ref AccPaS1, ref CurAmtS1);

            Entities.SapDeliveryCrossCmpStep2_DOCUMENTHEADER hs2 = getSapDeliveryCrossCmpStep2_DOCUMENTHEADER();
            List<Entities.SapDeliveryCrossCmpStep2_ACCOUNTGL> AccGLS2 = new List<Entities.SapDeliveryCrossCmpStep2_ACCOUNTGL>();
            List<Entities.SapDeliveryCrossCmpStep2_CURRENCYAMOUNT> CurAmtS2 = new List<Entities.SapDeliveryCrossCmpStep2_CURRENCYAMOUNT>();
            ii = 0;
            getSapDeliveryCrossCmpStep2_DebitCredit(ref ii, ref AccGLS2, ref CurAmtS2);

            Entities.SapDeliveryCrossCmpStep3_DOCUMENTHEADER hs3 = getSapDeliveryCrossCmpStep3_DOCUMENTHEADER();
            List<Entities.SapDeliveryCrossCmpStep3_ACCOUNTGL> AccGLS3 = new List<Entities.SapDeliveryCrossCmpStep3_ACCOUNTGL>();
            List<Entities.SapDeliveryCrossCmpStep3_ACCOUNTRECEIVABLE> AccRcS3 = new List<Entities.SapDeliveryCrossCmpStep3_ACCOUNTRECEIVABLE>();
            List<Entities.SapDeliveryCrossCmpStep3_CURRENCYAMOUNT> CurAmtS3 = new List<Entities.SapDeliveryCrossCmpStep3_CURRENCYAMOUNT>();
            ii = 0;
            getSapDeliveryCrossCmpStep3_Debit(ref ii, ref AccRcS3, ref CurAmtS3);
            getSapDeliveryCrossCmpStep3_Credit(ref ii, ref AccGLS3, ref CurAmtS3);

            //---------------------------------------ตรวจสอบ PRItem ก่อนส่งไป post เนื่องจาก CRM ส่ง PRitem มาผิดจึงต้องซ่อมให้(ปรึกษาพี่นนแล้ว)---------------------------------------------
            string DelvLst = Request.QueryString["DelvLstId"] + "";
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
                        DataRow[] drGroup = dtSap.Select("MATERIAL = '" + ItemNo + "' AND PREQ_ITEM = '" + PRItem + "' AND QUANTITY='" + Amount + "'");//ตรวจ PRITEM ถ้าพบแสดงว่าถูกต้องแล้ว
                        if (drGroup.Length > 0)
                        {
                            dtGroup = drGroup.CopyToDataTable();
                        }

                        //-----------------Check PRITEM---------------------
                        if (dtGroup.Rows.Count <= 0) //ถ้าไม่เจอข้อมูลใน Sap ที่ PR,MATERIAL,PRITEM,QUANTITY ตรงกัน แสดงว่า CRM ส่ง PRITEM มาผิด
                        {
                            DataTable dtGroup2 = new DataTable();
                            DataRow[] drGroup2 = dtSap.Select("MATERIAL = '" + ItemNo + "' AND QUANTITY='" + Amount + "'");//ถ้าไม่พบจากด้านบนให้หาด้วย Material,Quantity 
                            if (drGroup2.Length > 0)
                            {
                                dtGroup2 = drGroup2.CopyToDataTable();
                            }

                            //if (dtGroup2.Rows.Count > 1)//มากกว่า 1 แสดงว่ามีสินค้าชนิดเดียวกันทำเบิกมากกว่า 1 รายการใน PR ใบเดียวกัน Alert เตือนเพื่อให้ตรวจสอบเพราะไม่แน่ใจว่าตัวไหนที่ถูกต้อง
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
                                    string Msg = clsda.UpdatePRItemFromSap(dtCheckPOItem.Rows[i]["DelvLstId"].ToString(), PRNo, ItemNo, dtGroup2.Rows[0]["PREQ_ITEM"].ToString());
                                    if (Msg.Trim() != "")
                                    {
                                        ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('เกิดความผิดพลาดขณะทำการบันทึกข้อมูล PRItem ที่ไม่ตรงกับ Sap PRNO:" + PRNo + ", Material:" + ItemNo + ", PRItem(SAP):" + dtGroup2.Rows[0]["PREQ_ITEM"].ToString() + " MsgErr:" + Msg + "'); bindDataParentPage();", true);
                                        return;
                                    }
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ตรวจสอบพบค่า PRItem ไม่ตรงกับใน Sap และเกิดความผิดพลาดก่อน Updateเนื่องจากมีค่าบางค่าเป็นค่าว่าง PRNO:" + PRNo + ", Material:" + ItemNo + ", PRItem(SAP):" + dtGroup2.Rows[0]["PREQ_ITEM"].ToString() + "'); bindDataParentPage();", true);
                                    return;
                                }

                            }
                        }
                    }
                }
                //-----------------Check PRITEM---------------------
            }
            DataTable dtGetRefreshData = new DataTable();
            dtGetRefreshData = clsda.GetDataDeliveryItemFromDelvLst(DelvLst);
            if (dtGetRefreshData != null)
            {
                if (dtGetRefreshData.Rows.Count > 0)
                {
                    Session["Posting"] = dtGetRefreshData;
                }
            }
            //---------------------------------------ตรวจสอบ PRItem ก่อนส่งไป post เนื่องจาก CRM ส่ง PRitem มาผิดจึงต้องซ่อมให้(ปรึกษาพี่นนแล้ว)---------------------------------------------

            DataTable dt = (DataTable)Session["Posting"];
            string delvListNo = "";
            DataTable dtDelvLst = dt.DefaultView.ToTable(true, "DelvLstId");
            foreach (DataRow dr in dtDelvLst.Rows) { delvListNo += dr["DelvLstId"] + ";"; }
            delvListNo = delvListNo.Remove(delvListNo.Length - 1, 1);
            DADelivery dad = new DADelivery();
            string msgErr = "";

            /* - ลองลบ PR - */
            dad.WriteLogFile("[" + DateTime.Now.ToString() + "]----------------------------------DeliveryPostSumCrossCmp----------------------------------------");
            dad.WriteLogFile("btnPostAccount_Click(ลองลบPR):----------------------------------------");
            if (!DelPR("X", ref msgErr)) { ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('" + msgErr + "'); ", true); return; }
            dad.WriteLogFile("btnPostAccount_Click(ลองลบPR):----------------------------------------");
            /* - ลบ PR จริง - */
            dad.WriteLogFile("btnPostAccount_Click(ลบPRจริง):----------------------------------------");
            if (!DelPR("", ref msgErr)) { ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('" + msgErr + "'); ", true); return; }
            dad.WriteLogFile("btnPostAccount_Click(ลบPRจริง):----------------------------------------");


            Class.DASAPConnector cls = new Class.DASAPConnector();
            if (!cls.deliveryPostAccountCrossCmpCheck(hs1, AccGLS1, AccPaS1, CurAmtS1, hs2, AccGLS2, CurAmtS2, hs3, AccGLS3, AccRcS3, CurAmtS3, ref msgErr))
            {
                dad.WriteLogFile("btnPostAccount_Click(ลองส่งPostบัญชีเกิดข้อผิดพลาดทำการยกเลิกDeletePR):----------------------------------------");
                dad.WriteLogFile("btnPostAccount_Click[MessageErr=" + msgErr + "]");
                /* - ยกเลิกลบ PR จริง - */
                CancelDelPR("", ref msgErr);
                dad.WriteLogFile("btnPostAccount_Click(ลองส่งPostบัญชีเกิดข้อผิดพลาดทำการยกเลิกDeletePR):----------------------------------------");
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('บันทึกบัญชีผิดพลาด !! \\n" + msgErr + "');", true);
                return;
            }

            string SAPDocNo1 = "";
            string SAPDocNo2 = "";
            string SAPDocNo3 = "";
            //bool sapChk = ;
            dad.WriteLogFile("btnPostAccount_Click(ส่งPostบัญชีจริง):----------------------------------------");
            bool rstPost = cls.deliveryPostAccountCrossCmp(delvListNo, hs1, AccGLS1, AccPaS1, CurAmtS1, hs2, AccGLS2, CurAmtS2, hs3, AccGLS3, AccRcS3, CurAmtS3, ref SAPDocNo1, ref SAPDocNo2, ref SAPDocNo3, auth.EmployeeID, ref msgErr);
            if (rstPost)
            {

                Session["selDelv"] = "";
                Session["SAPDOC"] = SAPDocNo1;
                string urlRptPstAccIO = "frmReport.aspx?reqRPT=PstAccWBSCrs";
                dad.WriteLogFile("btnPostAccount_Click:[บันทึกบัญชีสำเร็จ]");
                dad.WriteLogFile("btnPostAccount_Click(ส่งPostบัญชีจริง):----------------------------------------");
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "Popup60('" + urlRptPstAccIO + "'); bindDataParentPage();", true);
                return;
            }
            else
            {
                dad.WriteLogFile("btnPostAccount_Click:[บันทึกบัญชีผิดพลาดError:" + msgErr + "]");
                dad.WriteLogFile("btnPostAccount_Click(ส่งPostบัญชีจริง):----------------------------------------");
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('บันทึกบัญชีผิดพลาด !! " + msgErr + "');", true);
                return;
            }
        }


        private bool DelPR(string isTest, ref string msgErr)
        {
            bool rst = true;
            Class.DASAPConnector sap = new DASAPConnector();
            DataTable dtPRDetail = null;
            DataTable dtPosting = (DataTable)Session["Posting"];
            DataTable dtPR = dtPosting.DefaultView.ToTable(true, "PRNo");
            DataTable tmpDataTable = dtPosting.DefaultView.ToTable(true, "RefMatId", "ItemNo", "PRNo");
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
                            // วิธีการคือจะต้องเอาค่า PRITEM ที่ได้จาก Sap ส่งเข้าไป delete ตัวเก่าใน Sap โดยกำหนดค่าให้ใน Datatable นี้ 08/11/2017
                            for (int i = 0; i < dtSapPrDetail_X.Rows.Count; i++)
                            {
                                for (int j = 0; j < dtDelPR.Rows.Count; j++)
                                {
                                    if (dtDelPR.Rows[j]["PRNo"].ToString() == prNo && dtDelPR.Rows[j]["ItemNo"].ToString() == dtSapPrDetail_X.Rows[i]["MATERIAL"].ToString())
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


            ////2017/09/06
            ////-เจอเคส user เลือกรายการหลายๆอัน แล้วทำการ post บัญชี แต่ มีบาง PR ในรายการที่เลือกนั้นถูก delete PR ไปแล้ว หาสาเหตุไม่เจอว่าไปเป็นตอน Step ไหน จึงทำ
            ////ตรวจสอบตรงนี้ก่อนถ้าเจอ PR ที่ถูก delete ไปแล้ว จะตรวจสอบยอดเงินในSAPไม่ได้ (VALUE_ITEM) หากพบรายการนั้นๆให้แจ้งเลข PR ทันทีเพื่อไม่ต้องมานั่ง Debug อีก
            ////ส่วนการแก้ไขอย่างถาวรนั้นต้องไปหาสาเหตุต่อไป หรืออาจะบอกให้ user เลือกรายการ post ไม่เกินเท่าไหร่จึงจะไม่เกิดปัญหาแทน
            //if (dtPRDetail != null)
            //{
            //    if (dtPRDetail.Rows.Count > 0)
            //    {
            //        string PR = string.Empty;
            //        for (int i = 0; i < dtPRDetail.Rows.Count; i++)
            //        {
            //            if (dtPRDetail.Rows[i]["DELETE_IND"].ToString().Trim() == "x" || dtPRDetail.Rows[i]["DELETE_IND"].ToString().Trim() == "X")
            //            {
            //                PR = PR + dtPRDetail.Rows[i]["PRNo"].ToString() + ",";
            //            }

            //        }

            //        if (PR.Trim() != string.Empty)
            //        {
            //            msgErr = "พบข้อมูล PR(ข้าม company code) บางรายการถูก Delete ไปแล้ว กรุณาแจ้งเจ้าหน้าที่ IT เพื่อถอย Delete PR แล้วทำการ Post บัญชีอีกครั้ง ดังนี้:" + PR;
            //            msgErr = msgErr.Trim().Substring(0, msgErr.Length - 1);
            //            return false;
            //        }
            //    }
            //}
            ////----------------------------------------------------


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

                /////2017.07.06 ตรวจสอบว่าถ้าเป็นเคสจ่ายสินค้าทดแทน ไม่ต้องทำการตรวจสอบเรื่องจำนวน ให้ทำการ delete pr ได้เลย
                ////แต่เนื่องจากเงื่อนไขเดิมของโปรแกรมนั้นทำไว้โดยตรวจสอบทั้ง จำนวน และ ยอดเงินทำให้เมื่อคำนวนจำนวนผิด
                ////จะมีการไปทำรายการลดจำนวนในระบบ SAP จึงตรวจสอบและยืนยันกับ (พี่โอ๊ต) ทีม SAP ว่าให้ดำเนินการ delete PR ได้ทันที
                //// แต่ยังคงให้มีการตรวจสอบยอดเงินอยู่ จึงต้อง Set Balance = 0 เพื่อวิ่ง flow ปกติเหมือนไม่ให้ทำการลดจำนวน
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

                /* - 2016.05.03 เลิกส่งจำนวนเงินเข้าไป กรณี Budget Exceed - */

                // PR กันงบ
                if ((decimal)drDelPR["Balance"] < 0)
                {
                    msgErr = "ไม่สามารถยกเลิก PR กันงบได้ เนื่องจากปริมาณสินค้าไม่เพียงพอ !! จำนวนในSap:" + drPrDetail[0]["QUANTITY"] + ",จำนวนที่Stockขอเบิก:" + drDelPR["DelvAmount"];
                    return false;
                }

                //2017/09/06
                //เอาออกเนื่องจากกรณีเดียวที่ Value_Item = 0 คือถูก delete ไปแล้ว พี่บาสแจ้งว่าถ้า pr ถูก delete ไปแล้ว ส่ง delete อีก หรือ post อีกก้จะไม่ติด errr
                //if ((decimal)drDelPR["MoneyBalance"] < 0)
                //{
                //    msgErr = "ไม่สามารถยกเลิก PR กันงบได้ เนื่องจากงบประมาณไม่เพียงพอ !! PR_No:" + drDelPR["PRNo"] + ",ยอดเงินในSap:" + VALUE_ITEM + ",ยอดเงินในStock:" + drDelPR["MoneyPost"];
                //    return false;
                //}

                // PR กันงบ
                if ((decimal)drDelPR["Balance"] == 0)
                {
                    drDelPR["xPRBalance"] = "";
                    drDelPR["isDel"] = "X";
                }
                else if ((decimal)drDelPR["Balance"] > 0)
                {
                    drDelPR["PRBalance"] = drDelPR["Balance"];
                    drDelPR["xPRBalance"] = "X";
                    drDelPR["isDel"] = "";
                }
            }
            dtDelPR.AcceptChanges();

            // return false;
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
                /*/ - - - - - - - - - - - - - - - - - - - - - - - - - -/*/
                /*/- 2016.06.30 : เอาส่วน release PR จากข้างบนลงมาไว้ตรงนี้ - /*/

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
                        foreach (DataRow dr in dtPRDetail.Rows)
                        {
                            dr["PRNo"] = prNo;
                        }
                    }
                    else
                    {
                        dtSapPrDetail_X.Columns.Add(new DataColumn("PRNo", typeof(string)));
                        foreach (DataRow drSapPrDetail_X in dtSapPrDetail_X.Rows)
                        {
                            drSapPrDetail_X["PRNo"] = prNo;
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

            dtDelPR.Columns.Add(new DataColumn("MoneyPost", typeof(decimal)));
            dtDelPR.Columns.Add(new DataColumn("MoneyBalance", typeof(decimal)));
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


        private Entities.SapDeliveryCrossCmpStep1_DOCUMENTHEADER getSapDeliveryCrossCmpStep1_DOCUMENTHEADER()
        {
            DataTable dt = (DataTable)Session["Posting"];
            Entities.SapDeliveryCrossCmpStep1_DOCUMENTHEADER hs1 = new Entities.SapDeliveryCrossCmpStep1_DOCUMENTHEADER();
            hs1.USERNAME = "APCOMMP2";
            hs1.COMP_CODE = dt.Rows[0]["CompanySAPCode"] + "";
            hs1.DOC_DATE = txtDate.Text;
            hs1.PSTNG_DATE = txtDate.Text;
            hs1.FISC_YEAR = hs1._PSTNG_DATE.Year;
            hs1.DOC_TYPE = "KR";
            hs1.REF_DOC_NO = txtReference.Text;
            return hs1;
        }

        private void getSapDeliveryCrossCmpStep1_Debit(ref int ii, ref List<Entities.SapDeliveryCrossCmpStep1_ACCOUNTGL> AccGLS1, ref List<Entities.SapDeliveryCrossCmpStep1_CURRENCYAMOUNT> curAmtS1)
        {
            DataTable dt = (DataTable)Session["Posting"];
            DASAPConnector sap = new DASAPConnector();
            DateTime sDate = new DateTime();
            convertDate.getDateFromString(txtDate.Text, ref sDate);
            DataTable dtPst = dt.DefaultView.ToTable(true, "PRNo", "ItemNo", "ItemName", "ProfitCenterHO", "CostCenter", "WBS_SAP", "isNonePR", "PO_No", "PO_Item");
            foreach (DataRow dr in dtPst.Rows)
            {
                ii++;
                //string cond = "ItemNo = '" + dr["ItemNo"] + "' AND ItemName = '" + dr["ItemName"] + "' AND CostCenter = '" + dr["CostCenter"] + "' AND ProfitCenterHO = '" + dr["ProfitCenterHO"] + "' AND WBS_SAP = '" + dr["WBS_SAP"] + "' AND isNonePR = '" + dr["isNonePR"] + "' AND PO_Item = '" + dr["PO_Item"] + "' AND PO_No = '" + dr["PO_No"] + "'";                                
                string cond = "ItemNo = '" + dr["ItemNo"] + "' AND ItemName = '" + dr["ItemName"] + "' AND CostCenter = '" + dr["CostCenter"] + "' AND ProfitCenterHO = '" + dr["ProfitCenterHO"] + "' AND WBS_SAP = '" + dr["WBS_SAP"] + "' AND isNonePR = '" + dr["isNonePR"] + "' AND PO_Item = '" + dr["PO_Item"] + "' AND PO_No = '" + dr["PO_No"] + "' AND PRNo = '" + dr["PRNo"] + "'";

                DataTable dttmp = dt.Select(cond).CopyToDataTable(); // เอาไว้ Debug
               
                int cntitm = (int)dt.Compute("COUNT(ItemId)", cond);
                var sumprice = dt.Compute("SUM(Price)", cond);
                decimal sumitm = (decimal)sumprice;
                Entities.SapDeliveryCrossCmpStep1_ACCOUNTGL accGL = new Entities.SapDeliveryCrossCmpStep1_ACCOUNTGL();
                accGL.ITEMNO_ACC = ii;
                accGL.GL_ACCOUNT = sap.getGLNoCredit();
                string preTxt = "";
                if (dr["isNonePR"] + "" == "Y") { preTxt = "O-"; }
                accGL.ITEM_TEXT = preTxt + dr["ItemName"] + "" + cntitm; // + " " + sDate.ToString("MM.yyyy"); 
                accGL.PROFIT_CTR = dr["ProfitCenterHO"] + "";

                accGL.REF_KEY_3 = txtRefkey3.Text;
                string PO_Item = "00000" + dr["PO_Item"];
                accGL.ALLOC_NMBR = dr["PO_No"] + "" + PO_Item.Substring(PO_Item.Length - 5, 5);

                AccGLS1.Add(accGL);

                Entities.SapDeliveryCrossCmpStep1_CURRENCYAMOUNT curAmt = new Entities.SapDeliveryCrossCmpStep1_CURRENCYAMOUNT();
                curAmt.ITEMNO_ACC = ii;
                curAmt.CURRENCY = "THB";
                curAmt.AMT_DOCCUR = sumitm;
                curAmtS1.Add(curAmt);
            }
        }

        private void getSapDeliveryCrossCmpStep1_Credit(ref int ii, ref List<Entities.SapDeliveryCrossCmpStep1_ACCOUNTPAYABLE> AccPaS1, ref List<Entities.SapDeliveryCrossCmpStep1_CURRENCYAMOUNT> curAmtS1)
        {
            DataTable dt = (DataTable)Session["Posting"];
            DASAPConnector sap = new DASAPConnector();
            DateTime sDate = new DateTime();
            convertDate.getDateFromString(txtDate.Text, ref sDate);
            DataTable dtPst = dt.DefaultView.ToTable(true, "ProfitCenterHO");
            foreach (DataRow dr in dtPst.Rows)
            {
                ii++;
                string cond = "ProfitCenterHO = '" + dr["ProfitCenterHO"] + "'";
                int cntitm = (int)dt.Compute("COUNT(ItemId)", cond);
                decimal sumitm = (decimal)dt.Compute("SUM(Price)", cond);
                Entities.SapDeliveryCrossCmpStep1_ACCOUNTPAYABLE accPa = new Entities.SapDeliveryCrossCmpStep1_ACCOUNTPAYABLE();
                accPa.ITEMNO_ACC = ii;          //	NUMC	10	0000000002	
                accPa.VENDOR_NO = "0000001000";       //	CHAR	10	1000	Fix
                accPa.BUSINESSPLACE = "0001";   //CHAR	4	0001	Fix
                accPa.PROFIT_CTR = dr["ProfitCenterHO"] + "";   //	CHAR	10	P71000
                //string preTxt = "";
                // if (dr["isNonePR"] + "" == "Y") { preTxt = "O-"; }
                accPa.ITEM_TEXT = "";// preTxt + dr["ItemName"] + " " + cntitm + " " + sDate.ToString("MM.yyyy"); //CHAR	50	Gift Voucher Lotus 2 ใบ 05.2015	

                AccPaS1.Add(accPa);

                Entities.SapDeliveryCrossCmpStep1_CURRENCYAMOUNT curAmt = new Entities.SapDeliveryCrossCmpStep1_CURRENCYAMOUNT();
                curAmt.ITEMNO_ACC = ii;
                curAmt.CURRENCY = "THB";
                curAmt.AMT_DOCCUR = (-1) * sumitm;
                curAmtS1.Add(curAmt);
            }
        }

        private Entities.SapDeliveryCrossCmpStep2_DOCUMENTHEADER getSapDeliveryCrossCmpStep2_DOCUMENTHEADER()
        {
            DataTable dt = (DataTable)Session["Posting"];
            Entities.SapDeliveryCrossCmpStep2_DOCUMENTHEADER hs2 = new Entities.SapDeliveryCrossCmpStep2_DOCUMENTHEADER();
            hs2.USERNAME = "APCOMMP2";
            hs2.COMP_CODE = dt.Rows[0]["CompanySAPCode"] + "";
            hs2.DOC_DATE = txtDate.Text;
            hs2.PSTNG_DATE = txtDate.Text;
            hs2.FISC_YEAR = hs2._PSTNG_DATE.Year;
            hs2.DOC_TYPE = "IA";
            hs2.REF_DOC_NO = txtReference.Text; /* - รอใส่ SAP DOC NO จากการ Post ครั้งแรก - */
            return hs2;
        }

        private void getSapDeliveryCrossCmpStep2_DebitCredit(ref int ii, ref List<Entities.SapDeliveryCrossCmpStep2_ACCOUNTGL> AccGLS2, ref List<Entities.SapDeliveryCrossCmpStep2_CURRENCYAMOUNT> curAmtS2)
        {
            DataTable dt = (DataTable)Session["Posting"];
            DASAPConnector sap = new DASAPConnector();
            DateTime sDate = new DateTime();
            convertDate.getDateFromString(txtDate.Text, ref sDate);
            DataTable dtPst = dt.DefaultView.ToTable(true, "PRNo", "ItemNo", "ItemName", "ProfitCenterHO", "CostCenter", "WBS_SAP", "isNonePR", "PO_No", "PO_Item");
            foreach (DataRow dr in dtPst.Rows)
            {
                ii++;
                //string cond = "ItemNo = '" + dr["ItemNo"] + "' AND ItemName = '" + dr["ItemName"] + "' AND CostCenter = '" + dr["CostCenter"] + "' AND ProfitCenterHO = '" + dr["ProfitCenterHO"] + "' AND WBS_SAP = '" + dr["WBS_SAP"] + "' AND isNonePR = '" + dr["isNonePR"] + "' AND PO_Item = '" + dr["PO_Item"] + "' AND PO_No = '" + dr["PO_No"] + "'";
                string cond = "ItemNo = '" + dr["ItemNo"] + "' AND ItemName = '" + dr["ItemName"] + "' AND CostCenter = '" + dr["CostCenter"] + "' AND ProfitCenterHO = '" + dr["ProfitCenterHO"] + "' AND WBS_SAP = '" + dr["WBS_SAP"] + "' AND isNonePR = '" + dr["isNonePR"] + "' AND PO_Item = '" + dr["PO_Item"] + "' AND PO_No = '" + dr["PO_No"] + "' AND PRNo = '" + dr["PRNo"] + "'";

                //DataTable dttmp = dt.Select(cond).CopyToDataTable(); // เอาไว้ Debug

                int cntitm = (int)dt.Compute("COUNT(ItemId)", cond);
                decimal sumitm = (decimal)dt.Compute("SUM(Price)", cond);
                Entities.SapDeliveryCrossCmpStep2_ACCOUNTGL accGL = new Entities.SapDeliveryCrossCmpStep2_ACCOUNTGL();
                accGL.ITEMNO_ACC = ii;
                accGL.GL_ACCOUNT = sap.getGLNoNormal();
                string preTxt = "";
                if (dr["isNonePR"] + "" == "Y") { preTxt = "O-"; }
                accGL.ITEM_TEXT = preTxt + dr["ItemName"] + " " + cntitm; // + " " + sDate.ToString("MM.yyyy");
                string cstctr = dr["CostCenter"] + "";
                if (cstctr != "") { cstctr = ("0000000000" + cstctr).Substring(("0000000000" + cstctr).Length - 10, 10); }
                int x = 0;
                if (!int.TryParse(dr["CostCenter"] + "", out x))
                {
                    cstctr = dr["CostCenter"] + "";
                }

                accGL.REF_KEY_2 = dr["CostCenter"] + "";//dr["ProfitCenterHO"] + "";
                accGL.COSTCENTER = cstctr; // cstctr;  // dr["CostCenter"] + ""; 
                accGL.WBS_ELEMENT = dr["WBS_SAP"] + "";

                accGL.REF_KEY_3 = txtRefkey3.Text;
                string PO_Item = "00000" + dr["PO_Item"];
                accGL.ALLOC_NMBR = dr["PO_No"] + "" + PO_Item.Substring(PO_Item.Length - 5, 5);

                AccGLS2.Add(accGL);

                Entities.SapDeliveryCrossCmpStep2_CURRENCYAMOUNT curAmt = new Entities.SapDeliveryCrossCmpStep2_CURRENCYAMOUNT();
                curAmt.ITEMNO_ACC = ii;
                curAmt.CURRENCY = "THB";
                curAmt.AMT_DOCCUR = sumitm;
                curAmtS2.Add(curAmt);
            }

            foreach (DataRow dr in dtPst.Rows)
            {
                ii++;
                //string cond = "ItemNo = '" + dr["ItemNo"] + "' AND ItemName = '" + dr["ItemName"] + "' AND CostCenter = '" + dr["CostCenter"] + "' AND ProfitCenterHO = '" + dr["ProfitCenterHO"] + "' AND WBS_SAP = '" + dr["WBS_SAP"] + "' AND PO_Item = '" + dr["PO_Item"] + "' AND PO_No = '" + dr["PO_No"] + "'";
                string cond = "ItemNo = '" + dr["ItemNo"] + "' AND ItemName = '" + dr["ItemName"] + "' AND CostCenter = '" + dr["CostCenter"] + "' AND ProfitCenterHO = '" + dr["ProfitCenterHO"] + "' AND WBS_SAP = '" + dr["WBS_SAP"] + "' AND PO_Item = '" + dr["PO_Item"] + "' AND PO_No = '" + dr["PO_No"] + "' AND PRNo = '" + dr["PRNo"] + "'";
                
                int cntitm = (int)dt.Compute("COUNT(ItemId)", cond);
                decimal sumitm = (decimal)dt.Compute("SUM(Price)", cond);
                Entities.SapDeliveryCrossCmpStep2_ACCOUNTGL accGL = new Entities.SapDeliveryCrossCmpStep2_ACCOUNTGL();
                accGL.ITEMNO_ACC = ii;
                accGL.GL_ACCOUNT = sap.getGLNoCredit();
                accGL.ITEM_TEXT = dr["ItemName"] + " " + cntitm; // + " " + sDate.ToString("MM.yyyy");

                accGL.PROFIT_CTR = dr["ProfitCenterHO"] + "";

                accGL.REF_KEY_3 = txtRefkey3.Text;
                string PO_Item = "00000" + dr["PO_Item"];
                accGL.ALLOC_NMBR = dr["PO_No"] + "" + PO_Item.Substring(PO_Item.Length - 5, 5);

                AccGLS2.Add(accGL);

                Entities.SapDeliveryCrossCmpStep2_CURRENCYAMOUNT curAmt = new Entities.SapDeliveryCrossCmpStep2_CURRENCYAMOUNT();
                curAmt.ITEMNO_ACC = ii;
                curAmt.CURRENCY = "THB";
                curAmt.AMT_DOCCUR = (-1) * sumitm;
                curAmtS2.Add(curAmt);
            }
        }

        private Entities.SapDeliveryCrossCmpStep3_DOCUMENTHEADER getSapDeliveryCrossCmpStep3_DOCUMENTHEADER()
        {
            DataTable dt = (DataTable)Session["Posting"];
            Entities.SapDeliveryCrossCmpStep3_DOCUMENTHEADER hs3 = new Entities.SapDeliveryCrossCmpStep3_DOCUMENTHEADER();
            hs3.USERNAME = "APCOMMP2";
            hs3.COMP_CODE = "1000";//dt.Rows[0]["CompanySAPCode"] + "";
            hs3.DOC_DATE = txtDate.Text;
            hs3.PSTNG_DATE = txtDate.Text;
            hs3.FISC_YEAR = hs3._PSTNG_DATE.Year;
            hs3.DOC_TYPE = "DR";
            hs3.REF_DOC_NO = txtReference.Text; /* - รอใส่ SAP DOC NO จากการ Post ครั้งแรก - */
            return hs3;
        }

        private void getSapDeliveryCrossCmpStep3_Debit(ref int ii, ref List<Entities.SapDeliveryCrossCmpStep3_ACCOUNTRECEIVABLE> AccRcS3, ref List<Entities.SapDeliveryCrossCmpStep3_CURRENCYAMOUNT> curAmtS3)
        {
            DataTable dt = (DataTable)Session["Posting"];
            DASAPConnector sap = new DASAPConnector();
            DateTime sDate = new DateTime();
            convertDate.getDateFromString(txtDate.Text, ref sDate);
            DataTable dtPst = dt.DefaultView.ToTable(true, "CompanySAPCode");
            foreach (DataRow dr in dtPst.Rows)
            {
                ii++;
                string cond = "CompanySAPCode = '" + dr["CompanySAPCode"] + "'";
                int cntitm = (int)dt.Compute("COUNT(ItemId)", cond);
                decimal sumitm = (decimal)dt.Compute("SUM(Price)", cond);
                Entities.SapDeliveryCrossCmpStep3_ACCOUNTRECEIVABLE accRc = new Entities.SapDeliveryCrossCmpStep3_ACCOUNTRECEIVABLE();
                accRc.ITEMNO_ACC = ii;
                string cuscde = "0000000000" + dr["CompanySAPCode"] + "";
                int x = 0;
                if (int.TryParse(dr["CompanySAPCode"] + "", out x))
                {
                    cuscde = cuscde.Substring(cuscde.Length - 10, 10);
                }
                else
                {
                    cuscde = dr["CompanySAPCode"] + "";
                }

                accRc.CUSTOMER = cuscde; // dr["CompanySAPCode"] + "";// cuscde.Substring(cuscde.Length - 10, 10);
                accRc.BUSINESSPLACE = "0001";
                accRc.PROFIT_CTR = "P11000";
                //string preTxt = ""; if (dr["isNonePR"] + "" == "Y") { preTxt = "O-"; }
                accRc.ITEM_TEXT = "";// preTxt + dr["ItemName"] + "" + cntitm + " " + sDate.ToString("MM.yyyy");
                AccRcS3.Add(accRc);

                Entities.SapDeliveryCrossCmpStep3_CURRENCYAMOUNT curAmt = new Entities.SapDeliveryCrossCmpStep3_CURRENCYAMOUNT();
                curAmt.ITEMNO_ACC = ii;
                curAmt.CURRENCY = "THB";
                curAmt.AMT_DOCCUR = sumitm;
                curAmtS3.Add(curAmt);
            }
        }

        private void getSapDeliveryCrossCmpStep3_Credit(ref int ii, ref List<Entities.SapDeliveryCrossCmpStep3_ACCOUNTGL> AccGLS3, ref List<Entities.SapDeliveryCrossCmpStep3_CURRENCYAMOUNT> curAmtS3)
        {
            DataTable dt = (DataTable)Session["Posting"];
            DASAPConnector sap = new DASAPConnector();
            DateTime sDate = new DateTime();
            convertDate.getDateFromString(txtDate.Text, ref sDate);
            DataTable dtPst = dt.DefaultView.ToTable(true, "PRNo", "ItemNo", "ItemName", "CostCenter", "ProfitCenterHO", "WBS_SAP", "CompanySAPCode", "isNonePR", "PO_No", "PO_Item");
            foreach (DataRow dr in dtPst.Rows)
            {
                ii++;
                //string cond = "ItemNo = '" + dr["ItemNo"] + "' AND ItemName = '" + dr["ItemName"] + "' AND CostCenter = '" + dr["CostCenter"] + "' AND ProfitCenterHO = '" + dr["ProfitCenterHO"] + "' AND CompanySAPCode = '" + dr["CompanySAPCode"] + "' AND ISNULL(isNonePR,'') = '" + dr["isNonePR"] + "' AND ISNULL(PO_No,'') = '" + dr["PO_No"] + "' AND ISNULL(PO_Item,'') = '" + dr["PO_Item"] + "'";
                string cond = "ItemNo = '" + dr["ItemNo"] + "' AND ItemName = '" + dr["ItemName"] + "' AND CostCenter = '" + dr["CostCenter"] + "' AND ProfitCenterHO = '" + dr["ProfitCenterHO"] + "' AND CompanySAPCode = '" + dr["CompanySAPCode"] + "' AND ISNULL(isNonePR,'') = '" + dr["isNonePR"] + "' AND ISNULL(PO_No,'') = '" + dr["PO_No"] + "' AND ISNULL(PO_Item,'') = '" + dr["PO_Item"] + "' AND ISNULL(WBS_SAP,'') = '" + dr["WBS_SAP"] + "' AND PRNo = '" + dr["PRNo"] + "'"; 

                if (dr["PO_Item"] == DBNull.Value)
                {
                    cond += " AND PO_Item IS NULL ";
                }
                else
                {
                    cond += " AND PO_Item = " + dr["PO_Item"] + "";
                }

                DataTable dttmp = dt.Select(cond).CopyToDataTable(); // เอาไว้ Debug

                int cntitm = (int)dt.Compute("COUNT(ItemId)", cond);
                decimal sumitm = (decimal)dt.Compute("SUM(Price)", cond);
                Entities.SapDeliveryCrossCmpStep3_ACCOUNTGL accGL = new Entities.SapDeliveryCrossCmpStep3_ACCOUNTGL();
                accGL.ITEMNO_ACC = ii;
                accGL.GL_ACCOUNT = sap.getGLNoCredit();
                accGL.PROFIT_CTR = "P11000";
                string preTxt = "";
                if (dr["isNonePR"] + "" == "Y") { preTxt = "O-"; }
                accGL.ITEM_TEXT = preTxt + dr["ItemName"] + "" + cntitm; // + " " + sDate.ToString("MM.yyyy");

                accGL.REF_KEY_3 = txtRefkey3.Text;
                string PO_Item = "00000" + dr["PO_Item"];
                accGL.ALLOC_NMBR = dr["PO_No"] + "" + PO_Item.Substring(PO_Item.Length - 5, 5);

                AccGLS3.Add(accGL);

                Entities.SapDeliveryCrossCmpStep3_CURRENCYAMOUNT curAmt = new Entities.SapDeliveryCrossCmpStep3_CURRENCYAMOUNT();
                curAmt.ITEMNO_ACC = ii;
                curAmt.CURRENCY = "THB";
                curAmt.AMT_DOCCUR = (-1) * sumitm;
                curAmtS3.Add(curAmt);
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
                    decimal itmtotal = Math.Round((decimal)dt_A.Compute("SUM(Price)", cond), 2); // (decimal)dt_A.Compute("SUM(Price)", cond);

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
                    int itmcnt = (int)dt_A.Compute("Count(ItemId)", cond);
                    //drTotal["Amount"] = dt.Compute("COUNT(ItemId)", cond);
                    drTotal["WBS_SAP"] = "" + drTotal["WBS_SAP"];
                    //drTotal["CostCenter"] = "P" + drTotal["CostCenter"];
                    //drTotal["ItemName"] = dt.Compute("COUNT(WBS_SAP)", cond);
                    drTotal["Debit"] = (decimal)dt_A.Compute("SUM(Price)", cond);
                    string preTxt = ""; if (drTotal["isNonePR"] + "" == "Y") { preTxt = "O-"; }
                    string ItemText = preTxt + " " + drTotal["ItemName"] + " " + itmcnt; //(int)dt.Compute("COUNT(WBS_SAP)", cond);
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
                    dt.Rows.Add(dr);
                }
                dtx.Dispose(); dtx = null;
                dt.AcceptChanges();
                grdDataPostAccount.DataSource = dt;
                grdDataPostAccount.DataBind();
            }
            catch (Exception)
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
            DataTable dt = dtAP.Clone();
            try
            {
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
            catch (Exception)
            {
                return null;
            }
            return dt;
        }


    }
}