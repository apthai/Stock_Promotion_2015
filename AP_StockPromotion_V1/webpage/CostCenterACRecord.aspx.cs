using AP_StockPromotion_V1.Class;
using Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI.WebControls;

namespace AP_StockPromotion_V1.webpage
{
    public partial class CostCenterACRecord : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static object GetListDataPromotionItems()
        {
            DAStockPromotion dasp = new DAStockPromotion();
            var ret = (from t1 in dasp.GetProdutcDetailsData().AsEnumerable()
                       select new PromotionItemsData
                       {
                           ItemNO = (string)t1.ItemArray[0],
                           ItemName = (string)t1.ItemArray[1]
                       }).ToList();
            return new
            {
                Data = ret
            };
        }

        public class PromotionItemsData
        {
            public string ItemNO { get; set; }
            public string ItemName { get; set; }
        }

        [WebMethod]
        public static List<ListDataMasterItems> GetListDataMasterItems(string docid, string itemno, string stddate, string enddate, int IsPostAcc)
        {
            try
            {
                DataTable dt = new DataTable();
                DAStockPromotion dasp = new DAStockPromotion();

                var lstItems = (from t1 in dasp.GetListDataMasterItems(docid, itemno, stddate, enddate, IsPostAcc).AsEnumerable()
                                select new ListDataMasterItems
                                {
                                    ID = Convert.ToInt32(t1.ItemArray[0]),
                                    RQRNO = t1.ItemArray[1].ToString(),
                                    COSTCENTER = t1.ItemArray[2].ToString(),
                                    PROPN = t1.ItemArray[3].ToString().Split('-').Length == 2 ? t1.ItemArray[3].ToString().Split('-')[1].ToString().Trim() : t1.ItemArray[3].ToString().Trim(),
                                    Date = t1.ItemArray[4].ToString(),
                                    PROMOTN = t1.ItemArray[5].ToString(),
                                    QUANT = Convert.ToInt32(t1.ItemArray[6].ToString()),
                                    REQN = t1.ItemArray[7].ToString(),
                                    SAPID = t1.ItemArray[8].ToString()
                                }).ToList();

                return lstItems;
            }
            catch
            {
                return new List<ListDataMasterItems>();
            }
        }

        [WebMethod]
        public static bool CheckCrossCompany(List<ListOfID> lstID)
        {
            try
            {
                bool ret = false;
                DAStockPromotion dasp = new DAStockPromotion();
                DataTable dt = dasp.CheckComCodeRetValue(lstID);
                if (dt.Rows.Count == 1)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i].ItemArray[0].ToString() != "1000")
                            ret = true;
                    }
                }
                return ret;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        [WebMethod]
        public static bool CheckComCode(List<ListOfID> lstID)
        {
            try
            {
                DAStockPromotion dasp = new DAStockPromotion();
                DataTable dt = dasp.CheckComCode(lstID);
                bool ret = false;
                if (dt.Rows.Count == 1)
                {
                    ret = true;
                }
                return ret;
            }
            catch
            {
                return false;
            }
        }

        [WebMethod]
        public static ListItemDetailCrossCenter GetListDataItemsCrossCompanyByID(List<ListOfID> lstID, int mode, string ref_doc_no, string ref_key_3, string posting_date, string empid)
        {
            try
            {
                bool IsMarketing = false;
                string msg = "";
                ListItemDetailCrossCenter _lstDetail = new ListItemDetailCrossCenter();
                _lstDetail.MsgErr = "";
                _lstDetail.SapDocNo = "";
                DataTable dt = new DataTable();
                DAStockPromotion dasp = new DAStockPromotion();

                if ((lstID ?? new List<ListOfID>()).Count > 0)
                {
                    IsMarketing = dasp.checkIsMarketing(lstID[0].ID, ref msg);
                }

                var lstItemsCrossDetail = (from t1 in dasp.GetListDataItemsCrossCompanyByID(lstID, mode, ref_doc_no, ref_key_3, posting_date).AsEnumerable()
                                           select new ListOfItemsCrossDetail
                                           {
                                               ITEMNO_ACC = t1[0].ToString(),
                                               USERNAME = t1[1].ToString(),
                                               RQRNO = t1[2].ToString(),
                                               COMP_CODE = t1[3].ToString(),
                                               COMP_NAME = t1[4].ToString(),
                                               DOC_DATE = t1[5].ToString(),
                                               PSTNG_DATE = t1[6].ToString(),
                                               FISC_YEAR = t1[7].ToString(),
                                               PAYABLE_DOC_TYPE = t1[8].ToString(),
                                               CUTSTOCK_DOC_TYPE = t1[9].ToString(),
                                               RECEIVEABLE_DOC_TYPE = t1[10].ToString(),
                                               REF_DOC_NO = t1[11].ToString(),
                                               GL_ACCOUNT = t1[12].ToString(),
                                               GL_Name = t1[13].ToString(),
                                               VENDOR_NO = t1[14].ToString(),
                                               CUSTOMER = t1[15].ToString(),
                                               ITEM_ID = t1[16].ToString(),
                                               ITEM_NAME = t1[17].ToString(),
                                               ITEM_TEXT = t1[18].ToString(),
                                               ITEM_QUAN = t1[19].ToString(),
                                               REF_KEY_2 = t1[20].ToString(),
                                               REF_KEY_3 = t1[21].ToString(),
                                               BUSINESSPLACE = t1[22].ToString(),
                                               COSTCENTER = t1[23].ToString(),
                                               COSTCENTER_NAME = t1[24].ToString(),
                                               PROFIT_CTR = t1[25].ToString(),
                                               ALLOC_NMBR = t1[26].ToString(),
                                               CURRENCY = t1[27].ToString(),
                                               AMT_DOCCUR = t1[28].ToString()
                                           }).OrderBy(q => q.RQRNO).ToList();
                _lstDetail.lstItemsCrossDetail = lstItemsCrossDetail;


                if (mode == 0)
                {
                    string SAPDOCNO = "";
                    string MSGERR = "";

                    SAPCROSSCOM_DOCUMENTHEADER HEADER = new SAPCROSSCOM_DOCUMENTHEADER();
                    List<SAPREQUISITION_PAYABLE> PAYABLE_DETAILS = new List<SAPREQUISITION_PAYABLE>();
                    List<SAPREQUISITION_ACCOUNTGL> CUTSTOCK_DETAILS = new List<SAPREQUISITION_ACCOUNTGL>();
                    List<SAPREQUISITION_RECIEVEABLE> RECEIVEABLE_DETAILS = new List<SAPREQUISITION_RECIEVEABLE>();
                    List<SAPREQUISITION_CURRENCYAMOUNT> CURRENCY = new List<SAPREQUISITION_CURRENCYAMOUNT>();

                    HEADER.USERNAME = lstItemsCrossDetail[0].USERNAME;
                    HEADER.COMP_CODE = lstItemsCrossDetail[0].COMP_CODE;
                    HEADER.DOC_DATE = lstItemsCrossDetail[0].DOC_DATE;
                    HEADER.PSTNG_DATE = lstItemsCrossDetail[0].PSTNG_DATE;
                    HEADER.FISC_YEAR = Convert.ToInt32(lstItemsCrossDetail[0].FISC_YEAR);
                    HEADER.DOC_TYPE = lstItemsCrossDetail[0].PAYABLE_DOC_TYPE;
                    HEADER.REF_DOC_NO = lstItemsCrossDetail[0].REF_DOC_NO;

                    foreach (var item in lstItemsCrossDetail)
                    {
                        SAPREQUISITION_PAYABLE payable = new SAPREQUISITION_PAYABLE();
                        payable.ITEMNO_ACC = Convert.ToInt32(item.ITEMNO_ACC);
                        payable.GL_ACCOUNT = item.GL_ACCOUNT;
                        payable.VENDOR_NO = item.VENDOR_NO;
                        payable.ITEM_TEXT = item.ITEM_TEXT;
                        payable.REF_KEY_3 = item.REF_KEY_3;
                        payable.PROFIT_CTR = item.PROFIT_CTR;
                        payable.BUSINESSPLACE = item.BUSINESSPLACE;
                        payable.ALLOC_NMBR = item.ALLOC_NMBR;

                        SAPREQUISITION_ACCOUNTGL accGL = new SAPREQUISITION_ACCOUNTGL();
                        accGL.ITEMNO_ACC = Convert.ToInt32(item.ITEMNO_ACC);
                        accGL.GL_ACCOUNT = item.GL_ACCOUNT;
                        accGL.ITEM_TEXT = item.ITEM_TEXT;
                        accGL.REF_KEY_2 = item.REF_KEY_2;
                        accGL.REF_KEY_3 = item.REF_KEY_3;
                        accGL.COSTCENTER = item.COSTCENTER;
                        accGL.PROFIT_CTR = item.PROFIT_CTR;
                        accGL.ALLOC_NMBR = item.ALLOC_NMBR;

                        SAPREQUISITION_RECIEVEABLE recieveable = new SAPREQUISITION_RECIEVEABLE();
                        recieveable.ITEMNO_ACC = Convert.ToInt32(item.ITEMNO_ACC);
                        recieveable.CUSTOMER = item.CUSTOMER;
                        recieveable.GL_ACCOUNT = item.GL_ACCOUNT;
                        recieveable.ITEM_TEXT = item.ITEM_TEXT;
                        recieveable.REF_KEY_3 = item.REF_KEY_3;
                        recieveable.PROFIT_CTR = item.PROFIT_CTR;
                        recieveable.BUSINESSPLACE = item.BUSINESSPLACE;
                        recieveable.ALLOC_NMBR = item.ALLOC_NMBR;

                        SAPREQUISITION_CURRENCYAMOUNT currency = new SAPREQUISITION_CURRENCYAMOUNT();
                        currency.ITEMNO_ACC = Convert.ToInt32(item.ITEMNO_ACC);
                        currency.CURRENCY = item.CURRENCY;
                        currency.AMT_DOCCUR = Convert.ToDecimal(item.AMT_DOCCUR);

                        PAYABLE_DETAILS.Add(payable);
                        CUTSTOCK_DETAILS.Add(accGL);
                        RECEIVEABLE_DETAILS.Add(recieveable);
                        CURRENCY.Add(currency);
                    }

                    string sap_PayableDocNo = "";
                    string sap_CutstockDocNo = "";
                    string sap_RecieveableDocNo = "";

                    DASAPConnector dasap = new DASAPConnector();
                    if (dasap.COSTCENTERRECORD_CROSSACCOUNT(HEADER, PAYABLE_DETAILS, CUTSTOCK_DETAILS, RECEIVEABLE_DETAILS, CURRENCY, IsMarketing, ref SAPDOCNO, ref MSGERR, out sap_PayableDocNo, out sap_CutstockDocNo, out sap_RecieveableDocNo))
                    {
                        DAStockPromotion _dasp = new DAStockPromotion();

                        List<CREDITOR> lstCr = new List<CREDITOR>();
                        List<CUTSTOCK> lstCs = new List<CUTSTOCK>();
                        List<DEDTOR> lstDdt = new List<DEDTOR>();

                        CREDITOR cr = null;
                        CUTSTOCK cs = null;
                        DEDTOR ddt = null;

                        string a_RQRNO = ""; string a_ACCRECTYPECODE = ""; string a_GLNO = ""; string a_GLNAME = ""; string a_COSTCENTERID = "";
                        string a_PROFITCENTER = ""; decimal? a_DEBIT = null; decimal? a_CREDIT = null; string a_ITEMTEXT = ""; string a_ITEMNO = "";

                        string b_RQRNO = ""; string b_ACCRECTYPECODE = ""; string b_GLNO = ""; string b_GLNAME = ""; string b_COSTCENTERID = "";
                        string b_PROFITCENTER = ""; decimal? b_DEBIT = null; decimal? b_CREDIT = null; string b_ITEMTEXT = ""; string b_ITEMNO = "";

                        for (int i = 0; i < lstItemsCrossDetail.Count; i++)
                        {
                            cr = new CREDITOR();
                            cs = new CUTSTOCK();
                            ddt = new DEDTOR();

                            if (lstItemsCrossDetail[i].COSTCENTER != "")
                            {
                                cr.RQRNO = lstItemsCrossDetail[i].RQRNO;
                                cr.ACCRECTYPECODE = 1;
                                cr.GLNO = "1580010";
                                cr.GLNAME = "สินทรัพย์ส่งเสริมการขาย";
                                cr.COSTCENTERID = "";
                                cr.PROFITCENTER = lstItemsCrossDetail[i].REF_KEY_2;
                                cr.DEBIT = lstItemsCrossDetail[i].AMT_DOCCUR;
                                cr.CREDIT = "";
                                cr.ITEMTEXT = lstItemsCrossDetail[i].ITEM_TEXT;
                                cr.ITEMNO = lstItemsCrossDetail[i].ITEM_ID;

                                cs.RQRNO = lstItemsCrossDetail[i].RQRNO;
                                cs.ACCRECTYPECODE = 2;
                                cs.GLNO = lstItemsCrossDetail[i].GL_ACCOUNT;
                                cs.GLNAME = lstItemsCrossDetail[i].GL_Name;
                                cs.COSTCENTERID = lstItemsCrossDetail[i].COSTCENTER;
                                cs.PROFITCENTER = lstItemsCrossDetail[i].REF_KEY_2;
                                cs.DEBIT = lstItemsCrossDetail[i].AMT_DOCCUR;
                                cs.CREDIT = "";
                                cs.ITEMTEXT = lstItemsCrossDetail[i].ITEM_TEXT;
                                cs.ITEMNO = lstItemsCrossDetail[i].ITEM_ID;

                                if (lstItemsCrossDetail[i].COMP_CODE != "1000")
                                {
                                    if (a_RQRNO == "")
                                    {
                                        a_RQRNO = lstItemsCrossDetail[i].RQRNO;
                                    }

                                    if (a_ACCRECTYPECODE == "")
                                    {
                                        a_ACCRECTYPECODE = "3";
                                    }

                                    if (a_GLNO == "")
                                    {
                                        a_GLNO = lstItemsCrossDetail[i].COMP_CODE;
                                    }

                                    if (a_GLNAME == "")
                                    {
                                        a_GLNAME = lstItemsCrossDetail[i].COMP_NAME;
                                    }

                                    if (a_COSTCENTERID == "")
                                    {
                                        a_COSTCENTERID = "";
                                    }

                                    if (a_PROFITCENTER == "")
                                    {
                                        a_PROFITCENTER = lstItemsCrossDetail[i].REF_KEY_2;
                                    }

                                    if (a_DEBIT == null)
                                    {
                                        a_DEBIT = Convert.ToDecimal(lstItemsCrossDetail[i].AMT_DOCCUR);
                                    }
                                    else
                                    {
                                        a_DEBIT += Convert.ToDecimal(lstItemsCrossDetail[i].AMT_DOCCUR);
                                    }

                                    if (a_CREDIT == null)
                                    {
                                        a_CREDIT = null;
                                    }

                                    if (a_ITEMTEXT == "")
                                    {
                                        a_ITEMTEXT = "";
                                    }

                                    if (a_ITEMNO == "")
                                    {
                                        a_ITEMNO = "";
                                    }

                                    lstCr.Add(cr);
                                    lstCs.Add(cs);
                                }
                                else
                                {
                                    lstCr.Add(cr);
                                    lstCs.Add(cs);
                                    if (ddt.RQRNO != null)
                                    {
                                        lstDdt.Add(ddt);
                                    }
                                }
                            }
                            else
                            {
                                cs.RQRNO = lstItemsCrossDetail[i].RQRNO;
                                cs.ACCRECTYPECODE = 2;
                                cs.GLNO = "1580010";
                                cs.GLNAME = "สินทรัพย์ส่งเสริมการขาย";
                                cs.COSTCENTERID = "";
                                cs.PROFITCENTER = lstItemsCrossDetail[i].PROFIT_CTR;
                                cs.DEBIT = "";
                                cs.CREDIT = lstItemsCrossDetail[i].AMT_DOCCUR;
                                cs.ITEMTEXT = lstItemsCrossDetail[i].ITEM_TEXT;
                                cs.ITEMNO = lstItemsCrossDetail[i].ITEM_ID;

                                ddt.RQRNO = lstItemsCrossDetail[i].RQRNO;
                                ddt.ACCRECTYPECODE = 3;
                                ddt.GLNO = "1580010";
                                ddt.GLNAME = "สินทรัพย์ส่งเสริมการขาย";
                                ddt.COSTCENTERID = "";
                                ddt.PROFITCENTER = lstItemsCrossDetail[i].PROFIT_CTR;
                                ddt.DEBIT = "";
                                ddt.CREDIT = lstItemsCrossDetail[i].AMT_DOCCUR;
                                ddt.ITEMTEXT = lstItemsCrossDetail[i].ITEM_TEXT;
                                ddt.ITEMNO = lstItemsCrossDetail[i].ITEM_ID;

                                if (lstItemsCrossDetail[i].VENDOR_NO == "1000")
                                {
                                    if (b_RQRNO == "")
                                    {
                                        b_RQRNO = lstItemsCrossDetail[i].RQRNO;
                                    }
                                    if (b_ACCRECTYPECODE == "")
                                    {
                                        b_ACCRECTYPECODE = "1";
                                    }
                                    if (b_GLNO == "")
                                    {
                                        b_GLNO = lstItemsCrossDetail[i].VENDOR_NO;
                                    }
                                    if (b_GLNAME == "")
                                    {
                                        b_GLNAME = "บริษัท เอพี (ไทยแลนด์) จำกัด";
                                    }
                                    if (b_COSTCENTERID == "")
                                    {
                                        b_COSTCENTERID = "";
                                    }
                                    if (b_PROFITCENTER == "")
                                    {
                                        b_PROFITCENTER = lstItemsCrossDetail[i].PROFIT_CTR;
                                    }
                                    if (b_DEBIT == null)
                                    {
                                        b_DEBIT = null;
                                    }

                                    if (b_CREDIT == null)
                                    {
                                        b_CREDIT = Convert.ToDecimal(lstItemsCrossDetail[i].AMT_DOCCUR);
                                    }
                                    else
                                    {
                                        b_CREDIT += Convert.ToDecimal(lstItemsCrossDetail[i].AMT_DOCCUR);
                                    }

                                    if (b_ITEMTEXT == "")
                                    {
                                        b_ITEMTEXT = "";
                                    }
                                    if (b_ITEMNO == "")
                                    {
                                        b_ITEMNO = "";
                                    }

                                    lstCs.Add(cs);
                                    lstDdt.Add(ddt);
                                }
                                else
                                {
                                    if (cr.RQRNO != null)
                                    {
                                        lstCr.Add(cr);
                                    }
                                    lstCs.Add(cs);
                                    lstDdt.Add(ddt);
                                }

                            }
                        }

                        ddt = new DEDTOR();
                        ddt.RQRNO = a_RQRNO;
                        ddt.ACCRECTYPECODE = Convert.ToInt32(a_ACCRECTYPECODE);
                        ddt.GLNO = a_GLNO;
                        ddt.GLNAME = a_GLNAME;
                        ddt.COSTCENTERID = a_COSTCENTERID;
                        ddt.PROFITCENTER = a_PROFITCENTER;
                        ddt.DEBIT = a_DEBIT.ToString();
                        ddt.CREDIT = a_CREDIT.ToString() ?? "";
                        ddt.ITEMTEXT = a_ITEMTEXT;
                        ddt.ITEMNO = a_ITEMNO;
                        lstDdt.Add(ddt);

                        cr = new CREDITOR();
                        cr.RQRNO = b_RQRNO;
                        cr.ACCRECTYPECODE = Convert.ToInt32(b_ACCRECTYPECODE);
                        cr.GLNO = b_GLNO;
                        cr.GLNAME = b_GLNAME;
                        cr.COSTCENTERID = b_COSTCENTERID;
                        cr.PROFITCENTER = b_PROFITCENTER;
                        cr.DEBIT = b_DEBIT.ToString();
                        cr.CREDIT = b_CREDIT.ToString() ?? "";
                        cr.ITEMTEXT = b_ITEMTEXT;
                        cr.ITEMNO = b_ITEMNO;
                        lstCr.Add(cr);

                        SAPDOCNO = sap_PayableDocNo.Substring(0, 10) + ", " + sap_CutstockDocNo.Substring(0, 10) + ", " + sap_RecieveableDocNo.Substring(0, 10);
                        dasp = new DAStockPromotion();

                        object[] _retObj = dasp.SaveRecordingAccount(lstCr, lstCs, lstDdt, sap_PayableDocNo, sap_CutstockDocNo, sap_RecieveableDocNo, ref_doc_no, ref_key_3, posting_date, empid);
                        if (_retObj[0].ToString() == "")
                        {
                            lstItemsCrossDetail = (from t1 in dasp.GetListDataItemsCrossCompanyByID(lstID, mode, ref_doc_no, ref_key_3, posting_date).AsEnumerable()
                                                   select new ListOfItemsCrossDetail
                                                   {
                                                       ITEMNO_ACC = t1[0].ToString(),
                                                       USERNAME = t1[1].ToString(),
                                                       RQRNO = t1[2].ToString(),
                                                       COMP_CODE = t1[3].ToString(),
                                                       COMP_NAME = t1[4].ToString(),
                                                       DOC_DATE = t1[5].ToString(),
                                                       PSTNG_DATE = t1[6].ToString(),
                                                       FISC_YEAR = t1[7].ToString(),
                                                       PAYABLE_DOC_TYPE = t1[8].ToString(),
                                                       CUTSTOCK_DOC_TYPE = t1[9].ToString(),
                                                       RECEIVEABLE_DOC_TYPE = t1[10].ToString(),
                                                       REF_DOC_NO = t1[11].ToString(),
                                                       GL_ACCOUNT = t1[12].ToString(),
                                                       GL_Name = t1[13].ToString(),
                                                       VENDOR_NO = t1[14].ToString(),
                                                       CUSTOMER = t1[15].ToString(),
                                                       ITEM_ID = t1[16].ToString(),
                                                       ITEM_NAME = t1[17].ToString(),
                                                       ITEM_TEXT = t1[18].ToString(),
                                                       ITEM_QUAN = t1[19].ToString(),
                                                       REF_KEY_2 = t1[20].ToString(),
                                                       REF_KEY_3 = t1[21].ToString(),
                                                       BUSINESSPLACE = t1[22].ToString(),
                                                       COSTCENTER = t1[23].ToString(),
                                                       COSTCENTER_NAME = t1[24].ToString(),
                                                       PROFIT_CTR = t1[25].ToString(),
                                                       ALLOC_NMBR = t1[26].ToString(),
                                                       CURRENCY = t1[27].ToString(),
                                                       AMT_DOCCUR = t1[28].ToString()
                                                   }).OrderBy(q => q.RQRNO).ToList();
                            _lstDetail.MsgErr = MSGERR;
                            _lstDetail.SapDocNo = SAPDOCNO;
                            _lstDetail.lstItemsCrossDetail = lstItemsCrossDetail;
                        }
                    }
                    else
                    {
                        _lstDetail.MsgErr = MSGERR;
                        _lstDetail.SapDocNo = SAPDOCNO;
                        _lstDetail.lstItemsCrossDetail = lstItemsCrossDetail;
                    }
                }

                return _lstDetail;
            }
            catch (Exception ex)
            {
                ListItemDetailCrossCenter _lstDetail = new ListItemDetailCrossCenter();
                _lstDetail.MsgErr = ex.Message.ToString();
                _lstDetail.SapDocNo = "";
                _lstDetail.lstItemsCrossDetail = new List<ListOfItemsCrossDetail>();
                return _lstDetail;
            }
        }

        [WebMethod]
        public static ListItemDetails GetListDataItemsByID(List<ListOfID> lstID, int mode, string ref_doc_no, string ref_key_3, string posting_date, string empid)
        {
            try
            {
                bool IsMarketing = false;
                string msg = "";
                ListItemDetails _lstDetail = new ListItemDetails();
                _lstDetail.MsgErr = "";
                _lstDetail.SapDocNo = "";
                DataTable dt = new DataTable();
                DAStockPromotion dasp = new DAStockPromotion();

                if ((lstID ?? new List<ListOfID>()).Count > 0)
                {
                    IsMarketing = dasp.checkIsMarketing(lstID[0].ID, ref msg);
                }

                var lstItemsDetail = (from t1 in dasp.GetListDataItemsByID(lstID, mode, ref_doc_no, ref_key_3, posting_date).AsEnumerable()
                                      select new ListOfItemsDetail
                                      {
                                          ITEMNO_ACC = t1[0].ToString(),
                                          USERNAME = t1[1].ToString(),
                                          COMP_CODE = t1[2].ToString(),
                                          DOC_DATE = t1[3].ToString(),
                                          PSTNG_DATE = t1[4].ToString(),
                                          FISC_YEAR = t1[5].ToString(),
                                          DOC_TYPE = t1[6].ToString(),
                                          REF_DOC_NO = t1[7].ToString(),
                                          GL_ACCOUNT = t1[8].ToString(),
                                          GL_Name = (t1[9].ToString().Split(':').Length > 1 ? t1[9].ToString().Split(':')[1].ToString().Trim() : t1[9].ToString()),
                                          ITEM_ID = t1[10].ToString(),
                                          ITEM_NAME = t1[11].ToString(),
                                          ITEM_TEXT = t1[12].ToString(),
                                          ITEM_QUAN = t1[13].ToString(),
                                          REF_KEY_2 = t1[14].ToString(),
                                          REF_KEY_3 = t1[15].ToString(),
                                          COSTCENTER = t1[16].ToString(),
                                          COSTCENTER_NAME = t1[17].ToString(),
                                          PROFIT_CTR = t1[18].ToString(),
                                          ALLOC_NMBR = t1[19].ToString(),
                                          CURRENCY = t1[20].ToString(),
                                          AMT_DOCCUR = t1[21].ToString(),
                                          RQRNO = t1[22].ToString()
                                      }).OrderBy(x => x.RQRNO).ToList();
                _lstDetail.lstItemsDetail = lstItemsDetail;

                if (mode == 0)
                {
                    SapDelivery_DOCUMENTHEADER docH = new SapDelivery_DOCUMENTHEADER();
                    docH.USERNAME = lstItemsDetail[0].USERNAME;
                    docH.COMP_CODE = lstItemsDetail[0].COMP_CODE;
                    docH.DOC_DATE = lstItemsDetail[0].DOC_DATE;
                    docH.PSTNG_DATE = lstItemsDetail[0].PSTNG_DATE;
                    docH.FISC_YEAR = Convert.ToInt32(lstItemsDetail[0].FISC_YEAR);
                    docH.DOC_TYPE = lstItemsDetail[0].DOC_TYPE;
                    docH.REF_DOC_NO = lstItemsDetail[0].REF_DOC_NO;

                    List<SAPREQUISITION_ACCOUNTGL> lstACCGL = new List<SAPREQUISITION_ACCOUNTGL>();
                    List<SapDelivery_CURRENCYAMOUNT> lstCurAmt = new List<SapDelivery_CURRENCYAMOUNT>();
                    foreach (var item in lstItemsDetail)
                    {
                        SAPREQUISITION_ACCOUNTGL _lstACCGL = new SAPREQUISITION_ACCOUNTGL();
                        SapDelivery_CURRENCYAMOUNT _lstCurAmt = new SapDelivery_CURRENCYAMOUNT();
                        if (item.COSTCENTER != "")
                        {
                            _lstACCGL.ITEMNO_ACC = Convert.ToInt32(item.ITEMNO_ACC);
                            _lstACCGL.GL_ACCOUNT = item.GL_ACCOUNT;
                            _lstACCGL.ITEM_TEXT = item.ITEM_TEXT;
                            _lstACCGL.REF_KEY_2 = item.REF_KEY_2;
                            _lstACCGL.REF_KEY_3 = item.REF_KEY_3;
                            _lstACCGL.COSTCENTER = item.COSTCENTER;
                            _lstACCGL.ALLOC_NMBR = item.ALLOC_NMBR;

                            _lstCurAmt.ITEMNO_ACC = Convert.ToInt32(item.ITEMNO_ACC);
                            _lstCurAmt.CURRENCY = item.CURRENCY;
                            _lstCurAmt.AMT_DOCCUR = Convert.ToDecimal(item.AMT_DOCCUR);
                        }
                        else if (item.PROFIT_CTR != "")
                        {
                            _lstACCGL.ITEMNO_ACC = Convert.ToInt32(item.ITEMNO_ACC);
                            _lstACCGL.GL_ACCOUNT = item.GL_ACCOUNT;
                            _lstACCGL.ITEM_TEXT = item.ITEM_TEXT;
                            _lstACCGL.REF_KEY_3 = item.REF_KEY_3;
                            _lstACCGL.PROFIT_CTR = item.PROFIT_CTR;
                            _lstACCGL.ALLOC_NMBR = item.ALLOC_NMBR;

                            _lstCurAmt.ITEMNO_ACC = Convert.ToInt32(item.ITEMNO_ACC);
                            _lstCurAmt.CURRENCY = item.CURRENCY;
                            _lstCurAmt.AMT_DOCCUR = Convert.ToDecimal(item.AMT_DOCCUR);
                        }
                        lstACCGL.Add(_lstACCGL);
                        lstCurAmt.Add(_lstCurAmt);
                    }
                    string SAPDOCNO = "";
                    string msgErr = "";

                    DASAPConnector dasap = new DASAPConnector();
                    if (dasap.COSTCENTERRECORD_ACCOUNT(docH, lstACCGL, lstCurAmt, IsMarketing, out SAPDOCNO, out msgErr))
                    {

                        _lstDetail.MsgErr = msgErr;
                        _lstDetail.SapDocNo = SAPDOCNO;
                        List<ACCREC> lstAcc = new List<ACCREC>();

                        ACCREC acc = null;
                        foreach (var item in lstItemsDetail)
                        {
                            acc = new ACCREC();
                            if (item.COSTCENTER != "")
                            {
                                acc.RQRNO = item.RQRNO;
                                acc.ACCRECTYPECODE = 4;
                                acc.GLNO = item.GL_ACCOUNT;
                                acc.GLNAME = item.GL_Name;
                                acc.COSTCENTERID = item.COSTCENTER;
                                acc.PROFITCENTER = "";
                                acc.DEBIT = item.AMT_DOCCUR;
                                acc.CREDIT = "";
                                acc.ITEMTEXT = item.ITEM_TEXT;
                                acc.ITEMNO = item.ITEM_ID;
                            }
                            else
                            {
                                acc.RQRNO = item.RQRNO;
                                acc.ACCRECTYPECODE = 4;
                                acc.GLNO = item.GL_ACCOUNT;
                                acc.GLNAME = item.GL_Name;
                                acc.COSTCENTERID = item.COSTCENTER;
                                acc.PROFITCENTER = item.PROFIT_CTR;
                                acc.DEBIT = "";
                                acc.CREDIT = item.AMT_DOCCUR;
                                acc.ITEMTEXT = item.ITEM_TEXT;
                                acc.ITEMNO = item.ITEM_ID;
                            }
                            lstAcc.Add(acc);
                        }

                        if (dasp.UpdateSapDocIdToRequisitionRequest(lstID, SAPDOCNO, ref_doc_no, ref_key_3, posting_date, lstAcc, empid))
                        {
                            lstItemsDetail = (from t1 in dasp.GetListDataItemsByID(lstID, mode, ref_doc_no, ref_key_3, posting_date).AsEnumerable()
                                              select new ListOfItemsDetail
                                              {
                                                  ITEMNO_ACC = t1[0].ToString(),
                                                  USERNAME = t1[1].ToString(),
                                                  COMP_CODE = t1[2].ToString(),
                                                  DOC_DATE = t1[3].ToString(),
                                                  PSTNG_DATE = t1[4].ToString(),
                                                  FISC_YEAR = t1[5].ToString(),
                                                  DOC_TYPE = t1[6].ToString(),
                                                  REF_DOC_NO = t1[7].ToString(),
                                                  GL_ACCOUNT = t1[8].ToString(),
                                                  GL_Name = t1[9].ToString(),
                                                  ITEM_ID = t1[10].ToString(),
                                                  ITEM_NAME = t1[11].ToString(),
                                                  ITEM_TEXT = t1[12].ToString(),
                                                  ITEM_QUAN = t1[13].ToString(),
                                                  REF_KEY_2 = t1[14].ToString(),
                                                  REF_KEY_3 = t1[15].ToString(),
                                                  COSTCENTER = t1[16].ToString(),
                                                  COSTCENTER_NAME = t1[17].ToString(),
                                                  PROFIT_CTR = t1[18].ToString(),
                                                  ALLOC_NMBR = t1[19].ToString(),
                                                  CURRENCY = t1[20].ToString(),
                                                  AMT_DOCCUR = t1[21].ToString(),
                                                  RQRNO = t1[22].ToString()
                                              }).ToList();
                        }

                        _lstDetail.lstItemsDetail = lstItemsDetail;
                    }
                    else
                    {
                        _lstDetail.MsgErr = msgErr;
                        _lstDetail.SapDocNo = SAPDOCNO;
                        _lstDetail.lstItemsDetail = lstItemsDetail;
                    }
                }

                return _lstDetail;
            }
            catch (Exception ex)
            {
                ListItemDetails _lstDetail = new ListItemDetails();
                _lstDetail.MsgErr = ex.Message.ToString();
                _lstDetail.SapDocNo = "";
                _lstDetail.lstItemsDetail = new List<ListOfItemsDetail>();
                return _lstDetail;
            }
        }

        [WebMethod]
        public static object[] ReversingAccounts(string docid, string remark, string postingDate, string reasonId, string reasonName, string empid)
        {
            try
            {
                DAStockPromotion dasp = new DAStockPromotion();
                object[] objData = dasp.GetDataForReverseDoc(docid);

                DataTable dt = (DataTable)objData[0];

                string Mode = "";
                List<string> lstStr = new List<string>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Mode = dt.Rows[i].ItemArray[0].ToString();

                    lstStr.Add(dt.Rows[i].ItemArray[1].ToString());
                    lstStr.Add(dt.Rows[i].ItemArray[2].ToString());
                    lstStr.Add(dt.Rows[i].ItemArray[3].ToString());
                    lstStr.Add(dt.Rows[i].ItemArray[4].ToString());
                }

                string revSapDocNo = "";
                string revSapDocNo_1 = "";
                string revSapDocNo_2 = "";
                string revSapDocNo_3 = "";
                string msgErr = "";
                DASAPConnector dasap = new DASAPConnector();
                if (Mode == "0")
                {
                    string OBJ_TYPE = "BKPFF";
                    string OBJ_SYS = "APQ";
                    string OBJ_KEY_R = lstStr[0].ToString();
                    DateTime PSTNG_DATE = Convert.ToDateTime(postingDate.Replace("/", "."));
                    string FIS_PERIOD = postingDate.Substring(3, 2);
                    string COMP_CODE = lstStr[0].ToString().Substring(10, 4);
                    string REASON_REV = reasonId;
                    string AC_DOC_NO = lstStr[0].ToString().Substring(0, 10);

                    dasap.ReverseAccount(OBJ_TYPE, OBJ_SYS, OBJ_KEY_R, PSTNG_DATE, FIS_PERIOD, COMP_CODE, REASON_REV, AC_DOC_NO, out revSapDocNo, out msgErr);
                    if (msgErr == "")
                    {
                        string msgerr = dasp.InsertReverseAccount(docid, remark, postingDate, reasonId, reasonName, revSapDocNo, empid);
                        if (msgerr == "")
                        {
                            return new object[] {
                                revSapDocNo.Substring(0, 10), GetAccountingRecorded()
                            };
                        }
                    }
                    return new object[]
                    {
                        msgErr
                    };
                }
                else
                {
                    string OBJ_TYPE = "BKPFF";
                    string OBJ_SYS = "APQ";
                    string OBJ_KEY_R_1 = lstStr[1].ToString();
                    string OBJ_KEY_R_2 = lstStr[2].ToString();
                    string OBJ_KEY_R_3 = lstStr[3].ToString();
                    DateTime PSTNG_DATE = Convert.ToDateTime(postingDate.Replace("/", "."));
                    string FIS_PERIOD = postingDate.Substring(3, 2);
                    string COMP_CODE_1 = lstStr[1].ToString().Substring(10, 4);
                    string COMP_CODE_2 = lstStr[2].ToString().Substring(10, 4);
                    string COMP_CODE_3 = lstStr[3].ToString().Substring(10, 4);
                    string REASON_REV = reasonId;
                    string AC_DOC_NO_1 = lstStr[1].ToString().Substring(0, 10);
                    string AC_DOC_NO_2 = lstStr[2].ToString().Substring(0, 10);
                    string AC_DOC_NO_3 = lstStr[3].ToString().Substring(0, 10);

                    dasap.ReverseAccountCrossComCode(OBJ_TYPE, OBJ_SYS,
                                                     OBJ_KEY_R_1, OBJ_KEY_R_2, OBJ_KEY_R_3,
                                                     PSTNG_DATE, FIS_PERIOD,
                                                     COMP_CODE_1, COMP_CODE_2, COMP_CODE_3,
                                                     REASON_REV,
                                                     AC_DOC_NO_1, AC_DOC_NO_2, AC_DOC_NO_3,
                                                     out revSapDocNo_1, out revSapDocNo_2, out revSapDocNo_3, out msgErr);
                    if (msgErr == "")
                    {
                        string msgerr = dasp.InsertReverseAccountCrossCenter(docid, remark, postingDate, reasonId, reasonName,
                                                                  revSapDocNo_1, revSapDocNo_2, revSapDocNo_3, empid);
                        if (msgerr == "")
                        {
                            return new object[] {
                                revSapDocNo_1.Substring(0, 10) + ", " + revSapDocNo_2.Substring(0, 10) + ", " + revSapDocNo_3.Substring(0, 10)
                                ,GetAccountingRecorded()
                            };
                        }
                    }
                    return new object[]
                    {
                        msgErr
                    };
                }
            }
            catch (Exception ex)
            {
                return new object[]
                {
                    ex.Message.ToString()
                };
            }
        }

        [WebMethod]
        public static object[] GetAccountingRecorded()
        {
            try
            {
                DAStockPromotion dasp = new DAStockPromotion();
                DataTable dt = (DataTable)dasp.GetAccountingRecorded()[0];
                //DataTable dt = (DataTable)res[0];
                var res = (from t1 in dt.AsEnumerable()
                           select new AccountRecorded
                           {
                               SAPREFID = t1.ItemArray[0].ToString(),
                               SAPID = t1.ItemArray[1].ToString(),
                               PSTDATE = t1.ItemArray[2].ToString(),
                               DOCNO = t1.ItemArray[3].ToString(),
                               ITEM = t1.ItemArray[4].ToString(),
                           }).ToList();

                var json = new JavaScriptSerializer().Serialize(res);

                return new object[]
                {
                    json
                };
            }
            catch (Exception ex)
            {
                return new object[]
                {
                    ex.Message.ToString()
                };
            }
        }

        [WebMethod]
        public static string GenerateReport(string sapid, string status)
        {
            frmReport _frmRpt = new frmReport();
            return status != "cross" ? _frmRpt.ReportAccountRecCostCenter(sapid) : _frmRpt.ReportAccountRecCostCenterCrossComCode(sapid);
        }

        #region ################ Class ################

        public class ClassReturn
        {
            public string MsgErr { get; set; }
            public string SapDocNo { get; set; }
        }

        public class ListItemDetailCrossCenter : ClassReturn
        {
            public virtual List<ListOfItemsCrossDetail> lstItemsCrossDetail { get; set; }
        }

        public class ListItemDetails : ClassReturn
        {
            public virtual List<ListOfItemsDetail> lstItemsDetail { get; set; }
        }

        public class AccountRecorded
        {
            public string SAPREFID { get; set; }
            public string SAPID { get; set; }
            public string PSTDATE { get; set; }
            public string DOCNO { get; set; }
            public string ITEM { get; set; }
        }

        public class ListOfItemsDetail
        {
            public string ITEMNO_ACC { get; set; }
            public string USERNAME { get; set; }
            public string RQRNO { get; set; }
            public string COMP_CODE { get; set; }
            public string DOC_DATE { get; set; }
            public string PSTNG_DATE { get; set; }
            public string FISC_YEAR { get; set; }
            public string DOC_TYPE { get; set; }
            public string REF_DOC_NO { get; set; }
            public string GL_ACCOUNT { get; set; }
            public string GL_Name { get; set; }
            public string ITEM_ID { get; set; }
            public string ITEM_NAME { get; set; }
            public string ITEM_TEXT { get; set; }
            public string ITEM_QUAN { get; set; }
            public string REF_KEY_2 { get; set; }
            public string REF_KEY_3 { get; set; }
            public string COSTCENTER { get; set; }
            public string COSTCENTER_NAME { get; set; }
            public string PROFIT_CTR { get; set; }
            public string ALLOC_NMBR { get; set; }
            public string CURRENCY { get; set; }
            public string AMT_DOCCUR { get; set; }
        }

        public class ListOfItemsCrossDetail
        {
            public string ITEMNO_ACC { get; set; }
            public string USERNAME { get; set; }
            public string RQRNO { get; set; }
            public string COMP_CODE { get; set; }
            public string COMP_NAME { get; set; }
            public string DOC_DATE { get; set; }
            public string PSTNG_DATE { get; set; }
            public string FISC_YEAR { get; set; }
            public string PAYABLE_DOC_TYPE { get; set; }
            public string CUTSTOCK_DOC_TYPE { get; set; }
            public string RECEIVEABLE_DOC_TYPE { get; set; }
            public string REF_DOC_NO { get; set; }
            public string GL_ACCOUNT { get; set; }
            public string GL_Name { get; set; }
            public string VENDOR_NO { get; set; }
            public string CUSTOMER { get; set; }
            public string ITEM_ID { get; set; }
            public string ITEM_NAME { get; set; }
            public string ITEM_TEXT { get; set; }
            public string ITEM_QUAN { get; set; }
            public string REF_KEY_2 { get; set; }
            public string REF_KEY_3 { get; set; }
            public string BUSINESSPLACE { get; set; }
            public string COSTCENTER { get; set; }
            public string COSTCENTER_NAME { get; set; }
            public string PROFIT_CTR { get; set; }
            public string ALLOC_NMBR { get; set; }
            public string CURRENCY { get; set; }
            public string AMT_DOCCUR { get; set; }
        }

        public class ListDataMasterItems
        {
            public int ID { get; set; }
            public string RQRNO { get; set; }
            public string COSTCENTER { get; set; }
            public string PROPN { get; set; }
            public string Date { get; set; }
            public string PROMOTN { get; set; }
            public int QUANT { get; set; }
            public string REQN { get; set; }
            public string SAPID { get; set; }
        }

        public class ListOfID
        {
            public int ID { get; set; }
        }

        #endregion
    }
}