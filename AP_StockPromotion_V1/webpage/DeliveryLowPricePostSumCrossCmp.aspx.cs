using AP_StockPromotion_V1.Class;
using AP_StockPromotion_V1.ws_authorize;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;

namespace AP_StockPromotion_V1.webpage
{
    public partial class DeliveryLowPricePostSumCrossCmp : Page
    {
        Entities.FormatDate convertDate = new Entities.FormatDate();
        //Session["dtDelvPost"] // GetDataDeliveryItemFromDelvLst
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                txtDate.Text = DateTime.Now.ToString(convertDate.formatDate);

                string DelvLst = Request.QueryString["DelvLstId"] + "";
                DADelivery cls = new DADelivery();
                DataTable dt = cls.GetDataDeliveryItemFromDelvLst(DelvLst);
                Session["PostingLowPrice"] = dt;
                grdAccountsPayable.DataSource = getDataAccountsPayable(dt);
                grdAccountsPayable.DataBind();
                grdDataPostAccount.DataSource = getDataPostAccounts(dt);
                grdDataPostAccount.DataBind();
                grdAccountsReceivable.DataSource = getDataAccountsReceivable(dt);
                grdAccountsReceivable.DataBind();
                //string x = cls.lookTable(xx);

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



                DataTable dt1 = getDataAccountsPayable(dt);
                DataTable dt2 = getDataPostAccounts(dt);
                DataTable dt3 = getDataAccountsReceivable(dt);
                DataSet ds = new DataSet();
                dt1.TableName = "dt1"; ds.Tables.Add(dt1);
                dt2.TableName = "dt2"; ds.Tables.Add(dt2);
                dt3.TableName = "dt3"; ds.Tables.Add(dt3);
                // ds.WriteXml(@"D:\PstAccIOCrs.xml",XmlWriteMode.WriteSchema);
                Session["dsPstAccIO"] = ds;

                //Class.DASAPConnector sap = newDASAPConnector();
                ////ItemNo	ItemName	ItemId	Price	CostCenter isNonePR  AND ISNULL(isNonePR,'') = '" + drsum["isNonePR"] + "'
                //DataTable dtTotal = dt.DefaultView.ToTable(true, "CostCenter", "ItemName", "isNonePR");
                //dtTotal.Columns.Add(new DataColumn("GLNo", typeof(string)));
                //dtTotal.Columns.Add(new DataColumn("GLName", typeof(string)));
                //dtTotal.Columns.Add(new DataColumn("ProfitCenter", typeof(string)));
                //dtTotal.Columns.Add(new DataColumn("Amount", typeof(int)));
                //dtTotal.Columns.Add(new DataColumn("Total", typeof(decimal)));
                //dtTotal.Columns.Add(new DataColumn("TotalCredit", typeof(decimal)));
                //dtTotal.Columns.Add(new DataColumn("ItemText", typeof(string)));

                //foreach (DataRow drTotal in dtTotal.Rows)
                //{
                //    drTotal["GLNo"] = sap.getGLNo300Cus();
                //    drTotal["GLName"] = sap.getGLNo300CusName();
                //    string cond = "ISNULL(CostCenter,'') = '" + drTotal["CostCenter"] + "' AND ISNULL(ItemName,'') = '" + drTotal["ItemName"] + "' AND ISNULL(isNonePR,'') = '" + drTotal["isNonePR"] + "'";
                //    //drTotal["Amount"] = dt.Compute("COUNT(ItemId)", cond);
                //    drTotal["CostCenter"] = drTotal["CostCenter"] + ""; // "MK" + 
                //    drTotal["Total"] = dt.Compute("SUM(Price)", cond);
                //    string preTxt = ""; if (drTotal["isNonePR"] + "" == "Y") { preTxt = "O-"; }
                //    drTotal["ItemText"] = preTxt + drTotal["ItemName"] + " " + dt.Compute("COUNT(ItemName)", cond) + " " + DateTime.Now.ToString("MM.yyyy");
                //}
                //DataRow dr = dtTotal.NewRow();
                //dr["GLNo"] = sap.getGLNoCredit();
                //dr["GLName"] = sap.getGLNoCreditName();
                //dr["ProfitCenter"] = hdfProfitHO.Value;
                //dr["TotalCredit"] = (-1) * (decimal)dt.Compute("SUM(Price)", "");
                //dr["Total"] = DBNull.Value;
                //dtTotal.Rows.Add(dr);

                //// DataTable dtx = dtTotal.Copy();
                ////foreach (DataRow drTotal in dtx.Rows)
                ////{
                ////    DataRow dr = dtTotal.NewRow();
                ////    dr["GLNo"] = sap.getGLNoCredit();
                ////    dr["GLName"] = sap.getGLNoCreditName();
                ////    dr["ProfitCenter"] = hdfProfitHO.Value;//(drTotal["CostCenter"] + "").Replace("MK", "P");
                ////    dr["TotalCredit"] = (-1) * (decimal)drTotal["Total"];
                ////    dr["Total"] = DBNull.Value;
                ////    dtTotal.Rows.Add(dr);
                ////}
                ////dtx.Dispose(); dtx = null;
                //dtTotal.AcceptChanges();
                //grdDataSum.DataSource = dtTotal;
                //grdDataSum.DataBind();
            }
        }

        protected void btnPostAccount_Click(object sender, EventArgs e)
        {
            AutorizeData auth = (AutorizeData)Session["userInfo_" + Session.SessionID];

            if (txtReference.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('" + "กรุณาระบุ Reference !!" + "'); ", true); return;
            }

            if (txtRefKey3.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('" + "กรุณาระบุ Ref Key 3 !!" + "'); ", true); return;
            }

            Entities.SapDeliveryCrossCmpStep1_DOCUMENTHEADER hs1 = getSapDeliveryMKCrossCmpStep1_DOCUMENTHEADER();
            List<Entities.SapDeliveryCrossCmpStep1_ACCOUNTGL> AccGLS1 = new List<Entities.SapDeliveryCrossCmpStep1_ACCOUNTGL>();
            List<Entities.SapDeliveryCrossCmpStep1_ACCOUNTPAYABLE> AccPaS1 = new List<Entities.SapDeliveryCrossCmpStep1_ACCOUNTPAYABLE>();
            List<Entities.SapDeliveryCrossCmpStep1_CURRENCYAMOUNT> CurAmtS1 = new List<Entities.SapDeliveryCrossCmpStep1_CURRENCYAMOUNT>();
            int ii = 0;
            getSapDeliveryMKCrossCmpStep1_Debit(ref ii, ref AccGLS1, ref CurAmtS1);
            getSapDeliveryMKCrossCmpStep1_Credit(ref ii, ref AccPaS1, ref CurAmtS1);

            Entities.SapDeliveryCrossCmpStep2_DOCUMENTHEADER hs2 = getSapDeliveryMKCrossCmpStep2_DOCUMENTHEADER();
            List<Entities.SapDeliveryCrossCmpStep2_ACCOUNTGL> AccGLS2 = new List<Entities.SapDeliveryCrossCmpStep2_ACCOUNTGL>();
            List<Entities.SapDeliveryCrossCmpStep2_CURRENCYAMOUNT> CurAmtS2 = new List<Entities.SapDeliveryCrossCmpStep2_CURRENCYAMOUNT>();
            ii = 0;
            getSapDeliveryMKCrossCmpStep2_DebitCredit(ref ii, ref AccGLS2, ref CurAmtS2);

            Entities.SapDeliveryCrossCmpStep3_DOCUMENTHEADER hs3 = getSapDeliveryMKCrossCmpStep3_DOCUMENTHEADER();
            List<Entities.SapDeliveryCrossCmpStep3_ACCOUNTGL> AccGLS3 = new List<Entities.SapDeliveryCrossCmpStep3_ACCOUNTGL>();
            List<Entities.SapDeliveryCrossCmpStep3_ACCOUNTRECEIVABLE> AccRcS3 = new List<Entities.SapDeliveryCrossCmpStep3_ACCOUNTRECEIVABLE>();
            List<Entities.SapDeliveryCrossCmpStep3_CURRENCYAMOUNT> CurAmtS3 = new List<Entities.SapDeliveryCrossCmpStep3_CURRENCYAMOUNT>();
            ii = 0;
            getSapDeliveryMKCrossCmpStep3_Debit(ref ii, ref AccRcS3, ref CurAmtS3);
            getSapDeliveryMKCrossCmpStep3_Credit(ref ii, ref AccGLS3, ref CurAmtS3);

            DataTable dt = (DataTable)Session["PostingLowPrice"];
            string delvListNo = "";
            DataTable dtDelvLst = dt.DefaultView.ToTable(true, "DelvLstId");
            foreach (DataRow dr in dtDelvLst.Rows) { delvListNo += dr["DelvLstId"] + ";"; }
            delvListNo = delvListNo.Remove(delvListNo.Length - 1, 1);
            DADelivery dad = new DADelivery();
            string msgErr = "";
            DASAPConnector cls = new DASAPConnector();
            if (!cls.deliveryLowPricePostAccountCrossCmpCheck(hs1, AccGLS1, AccPaS1, CurAmtS1, hs2, AccGLS2, CurAmtS2, hs3, AccGLS3, AccRcS3, CurAmtS3, ref msgErr))
            {
                dad.WriteLogFile("btnPostAccount_Click(ลองบันทึกบัญชีผิดพลาด MSG:" + msgErr + "):----------------------------------------");
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ทดลองบันทึกบัญชีผิดพลาด !! \\n" + msgErr + "');", true);
                return;
            }


            string SAPDocNo1 = "";
            string SAPDocNo2 = "";
            string SAPDocNo3 = "";
            //bool sapChk = ;

            bool rstPost = cls.deliveryLowPricePostAccountCrossCmp(delvListNo, hs1, AccGLS1, AccPaS1, CurAmtS1, hs2, AccGLS2, CurAmtS2, hs3, AccGLS3, AccRcS3, CurAmtS3, ref SAPDocNo1, ref SAPDocNo2, ref SAPDocNo3, auth.EmployeeID, ref msgErr);
            if (rstPost)
            {
                Session["selDelv"] = "";
                Session["SAPDOC"] = SAPDocNo1;
                string urlRptPstAccIO = "frmReport.aspx?reqRPT=PstAccIOCrs";
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "Popup60('" + urlRptPstAccIO + "'); bindDataParentPage();", true);
                //ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('บันทึกบัญชีเสร็จสิ้น \\n DOCNO1: " + SAPDocNo1 + " \\n DOCNO2: " + SAPDocNo2 + " \\n DOCNO3: " + SAPDocNo3 + " '); bindDataParentPage();", true);
                return;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('บันทึกบัญชีผิดพลาด !! " + msgErr + "');", true);
                return;
            }
        }

        private Entities.SapDeliveryCrossCmpStep1_DOCUMENTHEADER getSapDeliveryMKCrossCmpStep1_DOCUMENTHEADER()
        {
            DataTable dt = (DataTable)Session["PostingLowPrice"];
            Entities.SapDeliveryCrossCmpStep1_DOCUMENTHEADER hs1 = new Entities.SapDeliveryCrossCmpStep1_DOCUMENTHEADER();
            hs1.USERNAME = "APCOMMP2";
            hs1.COMP_CODE = dt.Rows[0]["CompanySAPCode"] + "";
            hs1.DOC_DATE = txtDate.Text;
            hs1.PSTNG_DATE = txtDate.Text;
            hs1.FISC_YEAR = hs1._PSTNG_DATE.Year;
            hs1.DOC_TYPE = "KR";
            hs1.REF_DOC_NO = txtReference.Text;
            hs1.REF_KEY_3 = txtRefKey3.Text;
            return hs1;
        }

        private void getSapDeliveryMKCrossCmpStep1_Debit(ref int ii, ref List<Entities.SapDeliveryCrossCmpStep1_ACCOUNTGL> AccGLS1, ref List<Entities.SapDeliveryCrossCmpStep1_CURRENCYAMOUNT> curAmtS1)
        {
            DataTable dt = (DataTable)Session["PostingLowPrice"];
            DASAPConnector sap = new DASAPConnector();
            //List<Entities.SapDeliveryMKCrossCmpStep1_ACCOUNTGL> AccGLS1 = new List<Entities.SapDeliveryMKCrossCmpStep1_ACCOUNTGL>();
            //List<Entities.SapDeliveryMKCrossCmpStep1_CURRENCYAMOUNT> curAmt = new List<Entities.SapDeliveryMKCrossCmpStep1_CURRENCYAMOUNT>();
            DateTime sDate = new DateTime();
            convertDate.getDateFromString(txtDate.Text, ref sDate);
            DataTable dtPst = dt.DefaultView.ToTable(true, "ItemNo", "ItemName", "CostCenter", "ProfitCenterHO", "isNonePR", "PO_No", "PO_Item");
            foreach (DataRow dr in dtPst.Rows)
            {
                ii++;
                string cond = "ItemNo = '" + dr["ItemNo"] + "' AND ItemName = '" + dr["ItemName"] + "' AND CostCenter = '" + dr["CostCenter"] + "' AND ProfitCenterHO = '" + dr["ProfitCenterHO"] + "' AND isNonePR = '" + dr["isNonePR"] + "' AND PO_No = '" + dr["PO_No"] + "' AND PO_Item = '" + dr["PO_Item"] + "'"; 
                int cntitm = (int)dt.Compute("COUNT(ItemId)", cond);
                decimal sumitm = (decimal)dt.Compute("SUM(Price)", cond);
                Entities.SapDeliveryCrossCmpStep1_ACCOUNTGL accGL = new Entities.SapDeliveryCrossCmpStep1_ACCOUNTGL();
                accGL.ITEMNO_ACC = ii;
                accGL.GL_ACCOUNT = sap.getGLNoCredit();
                string preTxt = ""; if (dr["isNonePR"] + "" == "Y") { preTxt = "O-"; }
                accGL.ITEM_TEXT = preTxt + dr["ItemName"] + "" + cntitm; // + " " + sDate.ToString("MM.yyyy");
                accGL.PROFIT_CTR = dr["ProfitCenterHO"] + "";
                accGL.REF_KEY_3 = txtRefKey3.Text;

                string _PO_Item = "";
                //switch (dr["PO_Item"].ToString().Length)
                //{
                //    case 1:
                //        _PO_Item = "0000" + dr["PO_Item"].ToString();
                //        break;
                //    case 2:
                //        _PO_Item = "000" + dr["PO_Item"].ToString();
                //        break;
                //    case 3:
                //        _PO_Item = "00" + dr["PO_Item"].ToString();
                //        break;
                //    case 4:
                //        _PO_Item = "0" + dr["PO_Item"].ToString();
                //        break;
                //}

                _PO_Item = "00000" + dr["PO_Item"].ToString();
                _PO_Item = _PO_Item.Substring(_PO_Item.Length - 5, 5);

                accGL.ALLOC_NMBR = dr["PO_No"].ToString() + _PO_Item;
                AccGLS1.Add(accGL);

                Entities.SapDeliveryCrossCmpStep1_CURRENCYAMOUNT curAmt = new Entities.SapDeliveryCrossCmpStep1_CURRENCYAMOUNT();
                curAmt.ITEMNO_ACC = ii;
                curAmt.CURRENCY = "THB";
                curAmt.AMT_DOCCUR = sumitm;
                curAmtS1.Add(curAmt);
            }
        }

        private void getSapDeliveryMKCrossCmpStep1_Credit(ref int ii, ref List<Entities.SapDeliveryCrossCmpStep1_ACCOUNTPAYABLE> AccPaS1, ref List<Entities.SapDeliveryCrossCmpStep1_CURRENCYAMOUNT> curAmtS1)
        {
            DataTable dt = (DataTable)Session["PostingLowPrice"];
            DASAPConnector sap = new DASAPConnector();
            //List<Entities.SapDeliveryMKCrossCmpStep1_ACCOUNTGL> AccGLS1 = new List<Entities.SapDeliveryMKCrossCmpStep1_ACCOUNTGL>();
            //List<Entities.SapDeliveryMKCrossCmpStep1_CURRENCYAMOUNT> curAmt = new List<Entities.SapDeliveryMKCrossCmpStep1_CURRENCYAMOUNT>();
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
                //if (dr["isNonePR"] + "" == "Y") { preTxt = "O-"; }
                accPa.ITEM_TEXT = "";// preTxt + dr["ItemName"] + " " + cntitm + " " + sDate.ToString("MM.yyyy"); //CHAR	50	Gift Voucher Lotus 2 ใบ 05.2015	

                AccPaS1.Add(accPa);

                Entities.SapDeliveryCrossCmpStep1_CURRENCYAMOUNT curAmt = new Entities.SapDeliveryCrossCmpStep1_CURRENCYAMOUNT();
                curAmt.ITEMNO_ACC = ii;
                curAmt.CURRENCY = "THB";
                curAmt.AMT_DOCCUR = (-1) * sumitm;
                curAmtS1.Add(curAmt);
            }
        }

        private Entities.SapDeliveryCrossCmpStep2_DOCUMENTHEADER getSapDeliveryMKCrossCmpStep2_DOCUMENTHEADER()
        {
            DataTable dt = (DataTable)Session["PostingLowPrice"];
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

        private void getSapDeliveryMKCrossCmpStep2_DebitCredit(ref int ii, ref List<Entities.SapDeliveryCrossCmpStep2_ACCOUNTGL> AccGLS2, ref List<Entities.SapDeliveryCrossCmpStep2_CURRENCYAMOUNT> curAmtS2)
        {
            DataTable dt = (DataTable)Session["PostingLowPrice"];
            DASAPConnector sap = new DASAPConnector();
            //List<Entities.SapDeliveryMKCrossCmpStep1_ACCOUNTGL> AccGLS1 = new List<Entities.SapDeliveryMKCrossCmpStep1_ACCOUNTGL>();
            //List<Entities.SapDeliveryMKCrossCmpStep1_CURRENCYAMOUNT> curAmt = new List<Entities.SapDeliveryMKCrossCmpStep1_CURRENCYAMOUNT>();
            DateTime sDate = new DateTime();
            convertDate.getDateFromString(txtDate.Text, ref sDate);
            DataTable dtPst = dt.DefaultView.ToTable(true, "ItemNo", "ItemName", "CostCenter", "ProfitCenterHO", "isNonePR", "PO_No", "PO_Item");
            foreach (DataRow dr in dtPst.Rows)
            {
                ii++;
                string cond = "ItemNo = '" + dr["ItemNo"] + "' AND ItemName = '" + dr["ItemName"] + "' AND CostCenter = '" + dr["CostCenter"] + "' AND ProfitCenterHO = '" + dr["ProfitCenterHO"] + "' AND isNonePR = '" + dr["isNonePR"] + "' AND PO_No = '" + dr["PO_No"] + "' AND PO_Item = '" + dr["PO_Item"] + "'"; 
                int cntitm = (int)dt.Compute("COUNT(ItemId)", cond);
                decimal sumitm = (decimal)dt.Compute("SUM(Price)", cond);
                Entities.SapDeliveryCrossCmpStep2_ACCOUNTGL accGL = new Entities.SapDeliveryCrossCmpStep2_ACCOUNTGL();
                accGL.ITEMNO_ACC = ii;
                accGL.GL_ACCOUNT = sap.getGLNo300Cus();
                string preTxt = ""; if (dr["isNonePR"] + "" == "Y") { preTxt = "O-"; }
                accGL.ITEM_TEXT = preTxt + dr["ItemName"] + "" + cntitm; //+ " " + sDate.ToString("MM.yyyy");
                accGL.REF_KEY_3 = txtRefKey3.Text;

                string _PO_Item = "";
                //switch (dr["PO_Item"].ToString().Length)
                //{
                //    case 1:
                //        _PO_Item = "0000" + dr["PO_Item"].ToString();
                //        break;
                //    case 2:
                //        _PO_Item = "000" + dr["PO_Item"].ToString();
                //        break;
                //    case 3:
                //        _PO_Item = "00" + dr["PO_Item"].ToString();
                //        break;
                //    case 4:
                //        _PO_Item = "0" + dr["PO_Item"].ToString();
                //        break;
                //}

                _PO_Item = "00000" + dr["PO_Item"].ToString();
                _PO_Item = _PO_Item.Substring(_PO_Item.Length - 5, 5);

                accGL.ALLOC_NMBR = dr["PO_No"].ToString() + _PO_Item;
                accGL.ORDERID = dr["CostCenter"] + "";
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
                string cond = "ItemNo = '" + dr["ItemNo"] + "' AND ItemName = '" + dr["ItemName"] + "' AND CostCenter = '" + dr["CostCenter"] + "' AND ProfitCenterHO = '" + dr["ProfitCenterHO"] + "' AND isNonePR = '" + dr["isNonePR"] + "' AND PO_No = '" + dr["PO_No"] + "' AND PO_Item = '" + dr["PO_Item"] + "'";
                int cntitm = (int)dt.Compute("COUNT(ItemId)", cond);
                decimal sumitm = (decimal)dt.Compute("SUM(Price)", cond);
                Entities.SapDeliveryCrossCmpStep2_ACCOUNTGL accGL = new Entities.SapDeliveryCrossCmpStep2_ACCOUNTGL();
                accGL.ITEMNO_ACC = ii;
                accGL.GL_ACCOUNT = sap.getGLNoCredit();
                accGL.ITEM_TEXT = dr["ItemName"] + "" + cntitm; // + " " + sDate.ToString("MM.yyyy");
                accGL.REF_KEY_3 = txtRefKey3.Text;

                string _PO_Item = "";
                //switch (dr["PO_Item"].ToString().Length)
                //{
                //    case 1:
                //        _PO_Item = "0000" + dr["PO_Item"].ToString();
                //        break;
                //    case 2:
                //        _PO_Item = "000" + dr["PO_Item"].ToString();
                //        break;
                //    case 3:
                //        _PO_Item = "00" + dr["PO_Item"].ToString();
                //        break;
                //    case 4:
                //        _PO_Item = "0" + dr["PO_Item"].ToString();
                //        break;
                //}

                _PO_Item = "00000" + dr["PO_Item"].ToString();
                _PO_Item = _PO_Item.Substring(_PO_Item.Length - 5, 5);

                accGL.ALLOC_NMBR = dr["PO_No"].ToString() + _PO_Item;
                accGL.PROFIT_CTR = dr["ProfitCenterHO"] + "";
                AccGLS2.Add(accGL);

                Entities.SapDeliveryCrossCmpStep2_CURRENCYAMOUNT curAmt = new Entities.SapDeliveryCrossCmpStep2_CURRENCYAMOUNT();
                curAmt.ITEMNO_ACC = ii;
                curAmt.CURRENCY = "THB";
                curAmt.AMT_DOCCUR = (-1) * sumitm;
                curAmtS2.Add(curAmt);
            }
        }

        private Entities.SapDeliveryCrossCmpStep3_DOCUMENTHEADER getSapDeliveryMKCrossCmpStep3_DOCUMENTHEADER()
        {
            DataTable dt = (DataTable)Session["PostingLowPrice"];
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
        private void getSapDeliveryMKCrossCmpStep3_Debit(ref int ii, ref List<Entities.SapDeliveryCrossCmpStep3_ACCOUNTRECEIVABLE> AccRcS3, ref List<Entities.SapDeliveryCrossCmpStep3_CURRENCYAMOUNT> curAmtS3)
        {
            DataTable dt = (DataTable)Session["PostingLowPrice"];
            DASAPConnector sap = new DASAPConnector();
            //List<Entities.SapDeliveryMKCrossCmpStep1_ACCOUNTGL> AccGLS1 = new List<Entities.SapDeliveryMKCrossCmpStep1_ACCOUNTGL>();
            //List<Entities.SapDeliveryMKCrossCmpStep1_CURRENCYAMOUNT> curAmt = new List<Entities.SapDeliveryMKCrossCmpStep1_CURRENCYAMOUNT>();
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
                string cuscde = ("0000000000" + dr["CompanySAPCode"]);
                cuscde = cuscde.Substring(cuscde.Length - 10, 10);
                int x = 0;
                if (!int.TryParse(dr["CompanySAPCode"] + "", out x))
                {
                    cuscde = dr["CompanySAPCode"] + "";
                }
                accRc.CUSTOMER = cuscde;//cuscde.Substring(cuscde.Length - 10, 10);
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
        private void getSapDeliveryMKCrossCmpStep3_Credit(ref int ii, ref List<Entities.SapDeliveryCrossCmpStep3_ACCOUNTGL> AccGLS3, ref List<Entities.SapDeliveryCrossCmpStep3_CURRENCYAMOUNT> curAmtS3)
        {
            DataTable dt = (DataTable)Session["PostingLowPrice"];
            DASAPConnector sap = new DASAPConnector();
            //List<Entities.SapDeliveryMKCrossCmpStep1_ACCOUNTGL> AccGLS1 = new List<Entities.SapDeliveryMKCrossCmpStep1_ACCOUNTGL>();
            //List<Entities.SapDeliveryMKCrossCmpStep1_CURRENCYAMOUNT> curAmt = new List<Entities.SapDeliveryMKCrossCmpStep1_CURRENCYAMOUNT>();
            DateTime sDate = new DateTime();
            convertDate.getDateFromString(txtDate.Text, ref sDate);
            DataTable dtPst = dt.DefaultView.ToTable(true, "ItemNo", "ItemName", "CostCenter", "ProfitCenterHO", "CompanySAPCode", "isNonePR", "PO_No", "PO_Item");
            foreach (DataRow dr in dtPst.Rows)
            {
                ii++;
                // string cond = "ItemNo = '" + dr["ItemNo"] + "' AND ItemName = '" + dr["ItemName"] + "' AND CostCenter = '" + dr["CostCenter"] + "' AND ProfitCenterHO = '" + dr["ProfitCenterHO"] + "' AND CompanySAPCode = '" + dr["CompanySAPCode"] + "' AND ISNULL(isNonePR,'') = '" + dr["isNonePR"] + "' AND ISNULL(PO_No,'') = '" + dr["PO_No"] + "' AND ISNULL(PO_Item,'') = '" + dr["PO_Item"] + "'";
                string cond = "ItemNo = '" + dr["ItemNo"] + "' AND ItemName = '" + dr["ItemName"] + "' AND CostCenter = '" + dr["CostCenter"] + "' AND ProfitCenterHO = '" + dr["ProfitCenterHO"] + "' AND CompanySAPCode = '" + dr["CompanySAPCode"] + "' AND ISNULL(isNonePR,'') = '" + dr["isNonePR"] + "' AND ISNULL(PO_No,'') = '" + dr["PO_No"] + "'";
                if (dr["PO_Item"] == DBNull.Value) { cond += " AND PO_Item IS NULL "; }
                else { cond += " AND PO_Item = " + dr["PO_Item"] + ""; }

                int cntitm = (int)dt.Compute("COUNT(ItemId)", cond);
                decimal sumitm = (decimal)dt.Compute("SUM(Price)", cond);
                Entities.SapDeliveryCrossCmpStep3_ACCOUNTGL accGL = new Entities.SapDeliveryCrossCmpStep3_ACCOUNTGL();
                accGL.ITEMNO_ACC = ii;
                accGL.GL_ACCOUNT = sap.getGLNoCredit();
                accGL.PROFIT_CTR = "P11000";
                string preTxt = ""; if (dr["isNonePR"] + "" == "Y") { preTxt = "O-"; }
                accGL.ITEM_TEXT = preTxt + dr["ItemName"] + "" + cntitm; // + " " + sDate.ToString("MM.yyyy");

                accGL.REF_KEY_3 = txtRefKey3.Text;

                string _PO_Item = "";
                //switch (dr["PO_Item"].ToString().Length)
                //{
                //    case 1:
                //        _PO_Item = "0000" + dr["PO_Item"].ToString();
                //        break;
                //    case 2:
                //        _PO_Item = "000" + dr["PO_Item"].ToString();
                //        break;
                //    case 3:
                //        _PO_Item = "00" + dr["PO_Item"].ToString();
                //        break;
                //    case 4:
                //        _PO_Item = "0" + dr["PO_Item"].ToString();
                //        break;
                //}

                _PO_Item = "00000" + dr["PO_Item"].ToString();
                _PO_Item = _PO_Item.Substring(_PO_Item.Length - 5, 5);

                accGL.ALLOC_NMBR = dr["PO_No"].ToString() + _PO_Item;

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
                // string x = newDADelivery().lookTable(dt);
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

                DataTable dtAPCr = dt_A.DefaultView.ToTable(true, "ProfitCenterHO");
                foreach (DataRow drAP in dtAPCr.Rows)
                {
                    string cond = "ProfitCenterHO = '" + drAP["ProfitCenterHO"] + "'";
                    int itmcnt = (int)dt_A.Compute("Count(ItemId)", cond);
                    decimal itmtotal = (decimal)dt_A.Compute("SUM(Price)", cond);
                    DataRow dr = dt.NewRow();
                    //dr["ItemNo"] = drAP["ItemNo"];
                    //dr["ItemName"] = drAP["ItemName"];
                    //dr["CostCenter"] = drAP["CostCenter"];
                    dr["ProfitCenterHO"] = drAP["ProfitCenterHO"];
                    dr["GLNo"] = sap.getGLNoAPVendor();
                    dr["GLName"] = sap.getGLNoAPVendorName();
                    dr["profitCenter"] = dr["ProfitCenterHO"];
                    // dr["debit"] = "";
                    dr["credit"] = ((-1) * itmtotal);//itmtotal;
                    //dr["itemtext"] = dr["ItemName"] + " " + itmcnt;
                    dt.Rows.Add(dr);
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return null;
            }


            return dt;
        }

        private DataTable getDataPostAccounts(DataTable dt_A)
        {
            DASAPConnector sap = new DASAPConnector();
            dt_A.DefaultView.RowFilter = "";
            DataTable dtAP = dt_A.DefaultView.ToTable(true, "ItemNo", "ItemName", "CostCenter", "ProfitCenterHO");
            DataTable dtAPCr = dt_A.DefaultView.ToTable(true, "CostCenter", "ProfitCenterHO");

            DataTable dt = dtAP.Clone();
            try
            {
                // string x = newDADelivery().lookTable(dt);
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
                    dr["itemtext"] = dr["ItemName"] + " " + itmcnt;
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
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
                // string x = newDADelivery().lookTable(dt);
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
                    dr["itemtext"] = dr["ItemName"] + " " + itmcnt;
                    dt.Rows.Add(dr);
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return null;
            }
            return dt;
        }


    }
}