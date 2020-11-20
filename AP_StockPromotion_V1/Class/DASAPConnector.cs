using com.apthai.QisWeb.Data.Extensions;
using Entities;
using NSAPConnector;
using NSAPConnector.Extension;
using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using Utility;

namespace AP_StockPromotion_V1.Class // com.apthai.QisWeb.Data.Services
{
    public class DASAPConnector
    {
        const string TAG = "SAPServices";

        private readonly string connectionName;

        private DataTable dtProject = null;
        private DataTable dtCompany = null;

        DAStockPromotion daStk = new DAStockPromotion();

        public DASAPConnector()
        {
            connectionName = ConfigurationManager.AppSettings["Application.Settings.SAPConnectionName"];
        }

        public string getGLNo300Cus()
        {
            return ConfigurationManager.AppSettings["GLNo300Cus"] + "";
        }
        public string getGLNo300Emp()
        {
            return ConfigurationManager.AppSettings["GLNo300Emp"] + "";
        }
        public string getGLNoNormal()
        {
            return ConfigurationManager.AppSettings["GLNoNormal"] + "";
        }
        public string getGLNoDamage()
        {
            return ConfigurationManager.AppSettings["GLNoDamage"] + "";
        }
        public string getGLNoCredit()
        {
            return ConfigurationManager.AppSettings["GLNoCredit"] + "";
        }
        public string getGLNoAPVendor()
        {
            return ConfigurationManager.AppSettings["GLNoAPVendor"] + "";
        }

        public string getGLNo300CusName()
        {
            return ConfigurationManager.AppSettings["GLNo300CusName"] + "";
        }
        public string getGLNo300EmpName()
        {
            return ConfigurationManager.AppSettings["GLNo300EmpName"] + "";
        }
        public string getGLNoNormalName()
        {
            return ConfigurationManager.AppSettings["GLNoNormalName"] + "";
        }
        public string getGLNoDamageName()
        {
            return ConfigurationManager.AppSettings["GLNoDamageName"] + "";
        }
        public string getGLNoCreditName()
        {
            return ConfigurationManager.AppSettings["GLNoCreditName"] + "";
        }
        public string getGLNoAPVendorName()
        {
            return ConfigurationManager.AppSettings["GLNoAPVendorName"] + "";
        }

        public string getProfitCenterByProjectCode(string ProjectCode)
        {
            string rst = "";
            try
            {
                if (dtProject == null)
                {
                    dtProject = daStk.getDataMasterProject();
                }
                rst = dtProject.Select("ProjectCode='" + ProjectCode + "'")[0]["ProfitCenter"] + "";
            }
            catch (Exception)
            {
            }
            return rst;
        }

        public string getCostCenterByProjectCode(string ProjectCode)
        {
            string rst = "";
            try
            {
                if (dtProject == null)
                {
                    dtProject = daStk.getDataMasterProject();
                }
                rst = dtProject.Select("ProjectCode='" + ProjectCode + "'")[0]["CostCenter"] + "";//CostCenter
            }
            catch (Exception) { }
            return rst;
        }

        public string getSAPGLCompany(string ProjectCode)
        {
            string rst = "";
            try
            {
                if (dtCompany == null)
                {
                    dtCompany = daStk.getDataSAPCompany();
                }
                rst = dtCompany.Select("CompanyCode='" + ProjectCode + "'")[0]["CompanyName"] + "";
            }
            catch (Exception) { }
            return rst;
        }

        public DataSet GetDataPO(string PONo)
        {
            DataSet resultDataSet = null;
            using (var connection = new SapConnection(connectionName))
            {
                try
                {
                    connection.Open();
                    var command = new SapCommand("ZRFC_GET_PO_STOCK", connection);
                    command.Parameters.Add("PO_NUMBER", PONo);

                    resultDataSet = command.ExecuteDataSet();
                }
                catch (Exception ex)
                {
                    string errMsg = ex.Message;
                }
            }

            return resultDataSet;
        }



        /* = - Create GR - = */
        public bool createGoodsReceipt(List<StockReceiveInfo> receive, ref string msgErr)
        {
            bool rst = true;
            using (var connection = new SapConnection(connectionName))
            {
                connection.Open();

                var transaction = connection.BeginTransaction();
                try
                {
                    IRfcFunction apAPI = connection.CreateFunction("BAPI_GOODSMVT_CREATE");
                    IRfcFunction apCMT = connection.CreateFunction("BAPI_TRANSACTION_COMMIT");
                    IRfcStructure HDR = apAPI.GetStructure("GOODSMVT_HEADER");
                    HDR.SetValue("PSTNG_DATE", SAPNETExtension.ToSAPDateString(receive[0]._PostingDate));
                    HDR.SetValue("DOC_DATE", SAPNETExtension.ToSAPDateString(receive[0]._DocDate));
                    HDR.SetValue("REF_DOC_NO", receive[0].DocRefNo);

                    IRfcStructure COD = apAPI.GetStructure("GOODSMVT_CODE");
                    COD.SetValue("GM_CODE", "01");
                    apAPI.SetValue("TESTRUN", "");
                    IRfcTable tblItem = apAPI.GetTable("GOODSMVT_ITEM");
                    foreach (StockReceiveInfo r in receive)
                    {
                        tblItem.Append();
                        tblItem.SetValue("PLANT", r.SAP_WERKS);
                        tblItem.SetValue("MOVE_TYPE", "101");// ' FIX
                        tblItem.SetValue("ENTRY_QNT", r.ReceiveAmount);
                        tblItem.SetValue("PO_NUMBER", r.SAP_EBELN);
                        tblItem.SetValue("PO_ITEM", ("00000" + r.SAP_EBELP).strRight(5));
                        tblItem.SetValue("MVT_IND", "B"); //' FIX
                    }

                    IRfcTable tbl_RETURN = apAPI.GetTable("RETURN");

                    RfcSessionManager.BeginContext(connection.Destination);
                    apAPI.Invoke(connection.Destination);

                    foreach (StockReceiveInfo r in receive)
                    {
                        r.GR_No = "" + apAPI.GetValue("MATERIALDOCUMENT");
                        r.GR_Year = "" + apAPI.GetValue("MATDOCUMENTYEAR");
                    }

                    bool bStkPmtRst = false;
                    if (receive[0].GR_No != "")
                    {
                        DAStockReceive clsSr = new DAStockReceive();
                        bStkPmtRst = clsSr.InsertDataReceive(receive, ref msgErr);
                    }


                    DataTable dt_RETURN = GetDataTable(tbl_RETURN);
                    dt_RETURN.DefaultView.RowFilter = "TYPE='E'";
                    DataTable dt = dt_RETURN.DefaultView.ToTable();

                    if (dt.Rows.Count == 0 && bStkPmtRst)
                    {
                        apCMT.SetValue("WAIT", "X");
                        apCMT.Invoke(connection.Destination);
                    }
                    else
                    {
                        rst = false;
                    }

                    RfcSessionManager.EndContext(connection.Destination);

                    if (dt.Rows.Count > 0)
                    {
                        transaction.Rollback();
                        msgErr = "CreateGoodsReceipt_Error[" + dt.Rows[0]["NUMBER"] + "] " + dt.Rows[0]["MESSAGE"];
                        rst = false;
                    }
                    else
                    {
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    rst = false;
                    while (ex.InnerException != null)
                        ex = ex.InnerException;

                    transaction.Rollback();
                    msgErr = "CreateGoodsReceipt_Error " + ex.Message;
                }
            }

            return rst;

        }

        /* = - Cancel GR - = */
        public bool cancelGoodsReceipt(StockReceiveInfo receive, ref string msgErr)
        {
            bool rst = true;
            using (var connection = new SapConnection(connectionName))
            {

                connection.Open();

                var transaction = connection.BeginTransaction();
                try
                {
                    IRfcFunction apCAN = connection.CreateFunction("BAPI_GOODSMVT_CANCEL");
                    IRfcFunction apCMT = connection.CreateFunction("BAPI_TRANSACTION_COMMIT");

                    apCAN.SetValue("MATERIALDOCUMENT", receive.GR_No);
                    apCAN.SetValue("MATDOCUMENTYEAR", receive.GR_Year);
                    apCAN.SetValue("GOODSMVT_PSTNG_DATE", SAPNETExtension.ToSAPDateString(DateTime.Now));
                    apCAN.SetValue("GOODSMVT_PR_UNAME", "NCO");

                    IRfcStructure exsturc = apCAN.GetStructure("GOODSMVT_HEADRET");
                    IRfcTable tbl_RETURN = apCAN.GetTable("RETURN");

                    RfcSessionManager.BeginContext(connection.Destination);
                    apCAN.Invoke(connection.Destination);
                    if (tbl_RETURN.RowCount == 0)
                    {

                        receive.GRCancel_No = exsturc["MAT_DOC"].GetValue() + "";
                        receive.GRCancel_Year = exsturc["DOC_YEAR"].GetValue() + "";

                        DAStockReceive clsRec = new DAStockReceive();
                        if (clsRec.cancelGoodsReceipt(receive))
                        {
                            apCMT.SetValue("WAIT", "X");
                            apCMT.Invoke(connection.Destination);
                        }
                        else
                        {
                            rst = false;
                        }
                    }

                    RfcSessionManager.EndContext(connection.Destination);

                    if (tbl_RETURN.RowCount > 0)
                    {
                        DataTable dt = GetDataTable(tbl_RETURN);
                        transaction.Rollback();
                        msgErr = "CancelGoodsReceipt_Error[" + dt.Rows[0]["NUMBER"] + "] " + dt.Rows[0]["MESSAGE"];
                        rst = false;
                    }
                    else
                    {
                        transaction.Commit();
                    }

                }
                catch (Exception ex)
                {
                    rst = false;
                    while (ex.InnerException != null)
                        ex = ex.InnerException;

                    transaction.Rollback();
                    msgErr = "CancelGoodsReceipt_Error " + ex.Message;
                }
                finally
                {
                }
            }
            return rst;
        }


        public bool createSAPReportPO(string PO_No, ref string msgErr)
        {

            bool rst = true;
            using (var connection = new SapConnection(connectionName))
            {

                connection.Open();

                var transaction = connection.BeginTransaction();
                try
                {
                    IRfcFunction apPRTPO = connection.CreateFunction("ZMM_PO_WEBVENDOR_PDF");
                    IRfcFunction apCMT = connection.CreateFunction("BAPI_TRANSACTION_COMMIT");

                    apPRTPO.SetValue("EBELN", PO_No);

                    RfcSessionManager.BeginContext(connection.Destination);
                    apPRTPO.Invoke(connection.Destination);
                    RfcSessionManager.EndContext(connection.Destination);

                    string MESSAGE_RETURN = apPRTPO.GetString("MESSAGE_RETURN");
                    if (MESSAGE_RETURN == "Success.")
                    {
                        apCMT.SetValue("WAIT", "X");
                        apCMT.Invoke(connection.Destination);
                    }

                    if (MESSAGE_RETURN != "Success.")
                    {

                        transaction.Rollback();
                        msgErr = "createSAPReportPO_Error[" + MESSAGE_RETURN.Replace("'", "\'") + "] ";
                        rst = false;
                    }
                    else
                    {
                        transaction.Commit();
                    }

                }
                catch (Exception ex)
                {
                    rst = false;
                    while (ex.InnerException != null)
                        ex = ex.InnerException;

                    transaction.Rollback();
                    msgErr = "createSAPReportPO_Error " + ex.Message;
                }
                finally
                {
                }
            }
            return rst;
        }

        public bool createSAPReportGR(string GR_No, string GRYear, ref string msgErr)
        {
            bool rst = true;
            using (var connection = new SapConnection(connectionName))
            {

                connection.Open();

                var transaction = connection.BeginTransaction();
                try
                {
                    IRfcFunction apPRTGR = connection.CreateFunction("ZMM_GR_WEBVENDOR_PDF");
                    IRfcFunction apCMT = connection.CreateFunction("BAPI_TRANSACTION_COMMIT");

                    apPRTGR.SetValue("MBLNR", GR_No);
                    apPRTGR.SetValue("MJAHR", GRYear);


                    RfcSessionManager.BeginContext(connection.Destination);
                    apPRTGR.Invoke(connection.Destination);
                    string MESSAGE_RETURN = apPRTGR.GetString("MESSAGE_RETURN");
                    RfcSessionManager.EndContext(connection.Destination);


                    if (MESSAGE_RETURN != "Success.")
                    {
                        transaction.Rollback();
                        msgErr = "createSAPReportGR_Error[" + MESSAGE_RETURN + "] ";
                        rst = false;
                    }
                    else
                    {
                        transaction.Commit();
                    }

                }
                catch (Exception ex)
                {
                    rst = false;
                    while (ex.InnerException != null)
                        ex = ex.InnerException;

                    transaction.Rollback();
                    msgErr = "createSAPReportGR_Error " + ex.Message;
                }
                finally
                {
                }
            }
            return rst;
        }

        /* = - Delivery Post Account  - = */
        public bool deliveryPostAccount(string DelvNo
            , SapDelivery_DOCUMENTHEADER docH
            , List<SapDelivery_ACCOUNTGL> lstAccGL
            , List<SapDelivery_CURRENCYAMOUNT> lstCurAmt
            , string EmployeeID, ref string SAPDocNo, ref string msgErr)
        {
            bool rst = true;
            using (var connection = new SapConnection(connectionName))
            {
                connection.Open();

                var transaction = connection.BeginTransaction();
                try
                {
                    IRfcFunction apAPI = connection.CreateFunction("BAPI_ACC_DOCUMENT_POST");
                    IRfcFunction apCMT = connection.CreateFunction("BAPI_TRANSACTION_COMMIT");

                    IRfcStructure DOCHDR = apAPI.GetStructure("DOCUMENTHEADER");
                    DOCHDR.SetValue("USERNAME", docH.USERNAME);
                    DOCHDR.SetValue("COMP_CODE", docH.COMP_CODE);
                    DOCHDR.SetValue("DOC_DATE", SAPNETExtension.ToSAPDateString(docH._DOC_DATE));
                    DOCHDR.SetValue("PSTNG_DATE", SAPNETExtension.ToSAPDateString(docH._PSTNG_DATE));
                    DOCHDR.SetValue("FISC_YEAR", docH.FISC_YEAR);
                    DOCHDR.SetValue("DOC_TYPE", docH.DOC_TYPE);
                    DOCHDR.SetValue("REF_DOC_NO", docH.REF_DOC_NO);

                    IRfcTable ACCGL = apAPI.GetTable("ACCOUNTGL");
                    foreach (SapDelivery_ACCOUNTGL accGL in lstAccGL)
                    {
                        ACCGL.Append();
                        ACCGL.SetValue("ITEMNO_ACC", accGL.ITEMNO_ACC);
                        ACCGL.SetValue("GL_ACCOUNT", accGL.GL_ACCOUNT);
                        ACCGL.SetValue("ITEM_TEXT", accGL.ITEM_TEXT);
                        ACCGL.SetValue("REF_KEY_2", accGL.REF_KEY_2);
                        ACCGL.SetValue("COSTCENTER", accGL.COSTCENTER);
                        ACCGL.SetValue("WBS_ELEMENT", accGL.WBS_ELEMENT);
                        ACCGL.SetValue("REF_KEY_3", accGL.REF_KEY_3);
                        ACCGL.SetValue("ALLOC_NMBR", accGL.ALLOC_NMBR);
                        ACCGL.SetValue("PROFIT_CTR", accGL.PROFIT_CTR);

                    }

                    IRfcTable CURAMT = apAPI.GetTable("CURRENCYAMOUNT");
                    foreach (SapDelivery_CURRENCYAMOUNT curAmt in lstCurAmt)
                    {
                        CURAMT.Append();
                        CURAMT.SetValue("ITEMNO_ACC", curAmt.ITEMNO_ACC);
                        CURAMT.SetValue("CURRENCY", curAmt.CURRENCY);
                        CURAMT.SetValue("AMT_DOCCUR", curAmt.AMT_DOCCUR);
                    }
                    RfcSessionManager.BeginContext(connection.Destination);
                    apAPI.Invoke(connection.Destination);

                    IRfcTable tbl_RETURN = apAPI.GetTable("RETURN");

                    string OBJ_TYPE = "" + apAPI.GetValue("OBJ_TYPE");//   Reference Transaction
                    string OBJ_KEY = "" + apAPI.GetValue("OBJ_KEY");//   Reference Key
                    string OBJ_SYS = "" + apAPI.GetValue("OBJ_SYS");//   Logical system of source document

                    DataTable dt_RETURN = GetDataTable(tbl_RETURN);
                    dt_RETURN.DefaultView.RowFilter = "TYPE='E'";
                    DataTable dt_ret = dt_RETURN.DefaultView.ToTable();
                    if (dt_ret.Rows.Count == 0)
                    {
                        DADelivery cls = new DADelivery();
                        if (cls.UpdateStatusPostAccount(DelvNo, OBJ_TYPE, OBJ_KEY, OBJ_SYS, docH.USERNAME, docH._PSTNG_DATE, docH.REF_DOC_NO, docH.REF_KEY_3, docH.USERNAME))
                        {
                            apCMT.SetValue("WAIT", "X");
                            apCMT.Invoke(connection.Destination);
                            SAPDocNo = OBJ_KEY;
                        }
                        else
                        {
                            rst = false;
                        }
                    }
                    else
                    {
                        rst = false;
                    }
                    RfcSessionManager.EndContext(connection.Destination);

                    if (!rst)
                    {
                        transaction.Rollback();
                        msgErr = "DeliveryPostAccount_Error";
                        foreach (DataRow drMsg in dt_ret.Rows)
                        {
                            msgErr += "\\n [" + drMsg["NUMBER"] + "] " + drMsg["MESSAGE"];
                        }
                        rst = false;
                    }
                    else
                    {
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    rst = false;
                    while (ex.InnerException != null)
                        ex = ex.InnerException;

                    transaction.Rollback();
                    msgErr = "DeliveryPostAccount_Error " + ex.Message;
                }
            }
            return rst;
        }

        /* = - Delivery Post Account  - = */
        public bool deliveryLowPricePostAccount(string DelvLst, SapDelivery_DOCUMENTHEADER docH,
                                                List<SapDeliveryLowPrice_ACCOUNTGL> lstAccGL,
                                                List<SapDelivery_CURRENCYAMOUNT> lstCurAmt,
                                                ref string SAPDocNo, ref string msgErr)
        {
            bool rst = true;
            using (var connection = new SapConnection(connectionName))
            {

                connection.Open();

                var transaction = connection.BeginTransaction();
                try
                {
                    IRfcFunction apAPI = connection.CreateFunction("BAPI_ACC_DOCUMENT_POST");
                    IRfcFunction apCMT = connection.CreateFunction("BAPI_TRANSACTION_COMMIT");

                    IRfcStructure DOCHDR = apAPI.GetStructure("DOCUMENTHEADER");
                    DOCHDR.SetValue("USERNAME", docH.USERNAME);
                    DOCHDR.SetValue("COMP_CODE", docH.COMP_CODE);
                    DOCHDR.SetValue("DOC_DATE", SAPNETExtension.ToSAPDateString(docH._DOC_DATE));
                    DOCHDR.SetValue("PSTNG_DATE", SAPNETExtension.ToSAPDateString(docH._PSTNG_DATE));
                    DOCHDR.SetValue("FISC_YEAR", docH.FISC_YEAR);
                    DOCHDR.SetValue("DOC_TYPE", docH.DOC_TYPE);
                    DOCHDR.SetValue("REF_DOC_NO", docH.REF_DOC_NO);

                    IRfcTable ACCGL = apAPI.GetTable("ACCOUNTGL");
                    foreach (SapDeliveryLowPrice_ACCOUNTGL accGL in lstAccGL)
                    {
                        ACCGL.Append();
                        ACCGL.SetValue("ITEMNO_ACC", accGL.ITEMNO_ACC);
                        ACCGL.SetValue("GL_ACCOUNT", accGL.GL_ACCOUNT);
                        ACCGL.SetValue("ITEM_TEXT", accGL.ITEM_TEXT);
                        ACCGL.SetValue("ORDERID", accGL.ORDERID);
                        ACCGL.SetValue("PROFIT_CTR", accGL.PROFIT_CTR);
                        ACCGL.SetValue("REF_KEY_3", accGL.REF_KEY_3);
                        ACCGL.SetValue("ALLOC_NMBR", accGL.ALLOC_NMBR);
                    }

                    IRfcTable CURAMT = apAPI.GetTable("CURRENCYAMOUNT");
                    foreach (SapDelivery_CURRENCYAMOUNT curAmt in lstCurAmt)
                    {
                        CURAMT.Append();
                        CURAMT.SetValue("ITEMNO_ACC", curAmt.ITEMNO_ACC);
                        CURAMT.SetValue("CURRENCY", curAmt.CURRENCY);
                        CURAMT.SetValue("AMT_DOCCUR", curAmt.AMT_DOCCUR);
                    }

                    RfcSessionManager.BeginContext(connection.Destination);
                    apAPI.Invoke(connection.Destination);

                    IRfcTable tbl_RETURN = apAPI.GetTable("RETURN");

                    string OBJ_TYPE = "" + apAPI.GetValue("OBJ_TYPE");//   Reference Transaction
                    string OBJ_KEY = "" + apAPI.GetValue("OBJ_KEY");//   Reference Key
                    string OBJ_SYS = "" + apAPI.GetValue("OBJ_SYS");//   Logical system of source document

                    DataTable dt_RETURN = GetDataTable(tbl_RETURN);
                    dt_RETURN.DefaultView.RowFilter = "TYPE='E'";
                    DataTable dt_ret = dt_RETURN.DefaultView.ToTable();
                    if (dt_ret.Rows.Count == 0)
                    {
                        SAPDocNo = OBJ_KEY;
                        DADelivery cls = new DADelivery();
                        if (cls.UpdateStatusPostAccount(DelvLst, OBJ_TYPE, OBJ_KEY, OBJ_SYS, docH.USERNAME, docH._PSTNG_DATE, docH.REF_DOC_NO, docH.REF_KEY_3, docH.USERNAME))
                        {
                            apCMT.SetValue("WAIT", "X");
                            apCMT.Invoke(connection.Destination);
                        }
                        else
                        {
                            rst = false;
                        }
                    }
                    else
                    {
                        rst = false;
                    }
                    RfcSessionManager.EndContext(connection.Destination);

                    if (!rst)
                    {

                        transaction.Rollback();
                        msgErr = "DeliveryLowPricePostAccount_Error";
                        foreach (DataRow drMsg in dt_ret.Rows)
                        {
                            msgErr += "\\n [" + drMsg["NUMBER"] + "] " + drMsg["MESSAGE"];
                        }

                        rst = false;
                    }
                    else
                    {
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    rst = false;
                    while (ex.InnerException != null)
                        ex = ex.InnerException;

                    transaction.Rollback();
                    msgErr = "DeliveryLowPricePostAccount_Error " + ex.Message;
                }
            }

            return rst;
        }

        /* = - PR Change Status  - = */
        public bool SAPChangeStatusPR(string PRNo, string isTest, DataTable dtCancPr, ref string msgErr)
        {
            DADelivery dad = new DADelivery();

            bool rst = true;
            using (var connection = new SapConnection(connectionName))
            {
                connection.Open();

                var transaction = connection.BeginTransaction();
                try
                {
                    IRfcFunction apAPI = connection.CreateFunction("BAPI_PR_CHANGE");
                    IRfcFunction apCMT = connection.CreateFunction("BAPI_TRANSACTION_COMMIT");

                    apAPI.SetValue("NUMBER", PRNo);
                    apAPI.SetValue("TESTRUN", isTest);

                    IRfcTable PRITEM = apAPI.GetTable("PRITEM");
                    IRfcTable PRITEMX = apAPI.GetTable("PRITEMX");

                    foreach (DataRow cancPr in dtCancPr.Rows)
                    {
                        PRITEM.Append();
                        // ถ้าค่า PRItemFromSap มี ให้ใช้อันนี้ แต่ถ้าไม่มีให้ใช้ค่า PREQ_ITEM เนื่องจากหากระบบ CRM ส่ง PRITem มาผิด และ เป็นของที่จ่ายเทียบเท่า จะทำให้ระบบ error

                        PRITEM.SetValue("PREQ_ITEM", cancPr["PRItem"]);

                        if (cancPr["PRBalance"] + "" != "")// ตรวจสอบว่ามีจำนวนที่จะเบิกเท่ากับใน Sap หรือไม่ ถ้าไม่ จะลดจำนวนใน PR นั้นๆ
                        {
                            PRITEM.SetValue("QUANTITY", cancPr["PRBalance"]);
                        }

                        PRITEM.SetValue("DELETE_IND", cancPr["isDel"]);

                        PRITEMX.Append();
                        //PRITEMX.SetValue("PREQ_ITEM", cancPr["PRItem"]);// flag X เพื่อเป็นคู่กับ field ในตารางด้านบน ที่ถูกupdate ถ้าตัวfield ไหนจะ update ต้องส่ง xคู่นี้ไป
                        PRITEMX.SetValue("PREQ_ITEM", cancPr["PRItem"]);

                        if (cancPr["PRBalance"] + "" != "")
                        {
                            PRITEMX.SetValue("QUANTITY", cancPr["xPRBalance"]);
                        }

                        PRITEMX.SetValue("DELETE_IND", "X");
                        dad.WriteLogFile("[" + DateTime.Now.ToString() + "]SAPChangeStatusPR(LoopPRItem):[PRNo=" + PRNo + "][PRItem=" + cancPr["PRItem"] + "][isTest=" + isTest + "][isDel= " + cancPr["isDel"] + ",DeleteIND=X][ลดจำนวน=" + cancPr["PRBalance"] + ",xลดจำนวน=" + cancPr["xPRBalance"] + "]");
                    }

                    RfcSessionManager.BeginContext(connection.Destination);
                    apAPI.Invoke(connection.Destination);

                    IRfcTable tbl_RETURN = apAPI.GetTable("RETURN");

                    DataTable dt_RETURN = GetDataTable(tbl_RETURN);
                    dt_RETURN.DefaultView.RowFilter = "TYPE='E'";
                    DataTable dt_ret = dt_RETURN.DefaultView.ToTable();

                    if (dt_ret.Rows.Count != 0)
                    {
                        dad.WriteLogFile("[" + DateTime.Now.ToString() + "]SAPChangeStatusPR(SapInvoke):[PRNo=" + PRNo + "][isTest=" + isTest + "][Msg=TYPE=E]");
                        rst = false;
                    }
                    else
                    {
                        dad.WriteLogFile("[" + DateTime.Now.ToString() + "]SAPChangeStatusPR(SapInvoke):[PRNo=" + PRNo + "][isTest=" + isTest + "][Msg=Finish]");
                        apCMT.SetValue("WAIT", "X");
                        apCMT.Invoke(connection.Destination);
                    }

                    RfcSessionManager.EndContext(connection.Destination);

                    if (!rst)
                    {

                        transaction.Rollback();
                        msgErr = "SAPChangeStatusPR";
                        foreach (DataRow drMsg in dt_ret.Rows)
                        {
                            msgErr += "\\n [" + drMsg["NUMBER"] + "] " + drMsg["MESSAGE"];
                        }
                        rst = false;
                    }
                    else
                    {
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    rst = false;
                    while (ex.InnerException != null)
                        ex = ex.InnerException;

                    transaction.Rollback();
                    msgErr = "SAPChangeStatusPR " + ex.Message;
                    dad.WriteLogFile("[" + DateTime.Now.ToString() + "]SAPChangeStatusPR(Catch Exception):[PRNo=" + PRNo + "][isTest=" + isTest + "][Msg=" + msgErr + "]");
                }

            }
            return rst;
        }


        /* = - Damage Post Account  - = */
        public bool SAPDamage(SapDestroy_DOCUMENTHEADER docH, List<SapDestroy_ACCOUNTGL> lstACCOUNTGL, List<SapDestroy_CURRENCYAMOUNT> lstCAMOUNT, string destroyItemList, string reason, string employeeID, ref string SAPDocNo, ref string msgErr)
        {
            bool rst = true;
            using (var connection = new SapConnection(connectionName))
            {
                connection.Open();
                var transaction = connection.BeginTransaction();
                try
                {
                    IRfcFunction apAPI = connection.CreateFunction("BAPI_ACC_DOCUMENT_POST");
                    IRfcFunction apCMT = connection.CreateFunction("BAPI_TRANSACTION_COMMIT");
                    IRfcStructure DOCHDR = apAPI.GetStructure("DOCUMENTHEADER");
                    DOCHDR.SetValue("USERNAME", docH.USERNAME);
                    DOCHDR.SetValue("COMP_CODE", docH.COMP_CODE);
                    DOCHDR.SetValue("DOC_DATE", SAPNETExtension.ToSAPDateString(docH._DOC_DATE));
                    DOCHDR.SetValue("PSTNG_DATE", SAPNETExtension.ToSAPDateString(docH._PSTNG_DATE));
                    DOCHDR.SetValue("FISC_YEAR", docH.FISC_YEAR);
                    DOCHDR.SetValue("DOC_TYPE", docH.DOC_TYPE);

                    IRfcTable ACCGL = apAPI.GetTable("ACCOUNTGL");
                    foreach (SapDestroy_ACCOUNTGL accGL in lstACCOUNTGL)
                    {
                        ACCGL.Append();
                        ACCGL.SetValue("ITEMNO_ACC", accGL.ITEMNO_ACC);
                        ACCGL.SetValue("GL_ACCOUNT", accGL.GL_ACCOUNT);
                        ACCGL.SetValue("ITEM_TEXT", accGL.ITEM_TEXT);
                        ACCGL.SetValue("PROFIT_CTR", accGL.PROFIT_CTR);
                        ACCGL.SetValue("REF_KEY_3", accGL.REF_KEY_3);
                    }

                    IRfcTable CURAMT = apAPI.GetTable("CURRENCYAMOUNT");
                    foreach (SapDestroy_CURRENCYAMOUNT curAmt in lstCAMOUNT)
                    {
                        CURAMT.Append();
                        CURAMT.SetValue("ITEMNO_ACC", curAmt.ITEMNO_ACC);
                        CURAMT.SetValue("CURRENCY", curAmt.CURRENCY);
                        CURAMT.SetValue("AMT_DOCCUR", curAmt.AMT_DOCCUR);
                    }

                    RfcSessionManager.BeginContext(connection.Destination);
                    apAPI.Invoke(connection.Destination);

                    IRfcTable tbl_RETURN = apAPI.GetTable("RETURN");
                    DataTable dt_RETURN = GetDataTable(tbl_RETURN);
                    dt_RETURN.DefaultView.RowFilter = "TYPE='E'";
                    DataTable dt_ret = dt_RETURN.DefaultView.ToTable();
               
                    if (dt_ret.Rows.Count == 0)
                    {
                        string OBJ_TYPE = "" + apAPI.GetValue("OBJ_TYPE");//   Reference Transaction
                        string OBJ_KEY = "" + apAPI.GetValue("OBJ_KEY");//   Reference Key
                        string OBJ_SYS = "" + apAPI.GetValue("OBJ_SYS");//   Logical system of source document
                        SAPDocNo = OBJ_KEY.Substring(0, 10);
                        DAStockItemDestroy cls = new DAStockItemDestroy();
                        if (cls.destroyItem(destroyItemList, reason, employeeID, OBJ_TYPE, OBJ_KEY, OBJ_SYS))
                        {
                            apCMT.SetValue("WAIT", "X");
                            apCMT.Invoke(connection.Destination);
                        }
                        else
                        {
                            rst = false;
                        }
                    }
                    else
                    {
                        rst = false;
                    }

                    RfcSessionManager.EndContext(connection.Destination);

                    if (!rst)
                    {
                        transaction.Rollback();
                        msgErr = "SAPDamage_Error";
                        foreach (DataRow drMsg in dt_ret.Rows)
                        {
                            msgErr += "\\n [" + drMsg["NUMBER"] + "] " + drMsg["MESSAGE"];
                        }

                        rst = false;
                    }
                    else
                    {
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    rst = false;
                    while (ex.InnerException != null)
                        ex = ex.InnerException;

                    transaction.Rollback();
                    msgErr = "SAPDamage " + ex.Message;
                }
                finally
                {
                }
            }

            return rst;
        }


        /* = - Reverse Damage Post Account  - = */
        public bool SAPReverseDamage(SapDestroy_REVERSAL rev, int destroyListId, string employeeID, ref string OBJ_TYPE, ref string OBJ_KEY, ref string OBJ_SYS, ref string msgErr)
        {
            bool rst = true;
            using (var connection = new SapConnection(connectionName))
            {

                connection.Open();

                var transaction = connection.BeginTransaction();
                try
                {
                    IRfcFunction apAPI = connection.CreateFunction("BAPI_ACC_DOCUMENT_REV_POST");
                    IRfcFunction apCMT = connection.CreateFunction("BAPI_TRANSACTION_COMMIT");

                    apAPI.SetValue("BUS_ACT", "RFBU");
                    IRfcStructure REVERSAL = apAPI.GetStructure("REVERSAL");
                    REVERSAL.SetValue("OBJ_TYPE", rev.OBJ_TYPE);
                    REVERSAL.SetValue("OBJ_SYS", rev.COMP_CODE);
                    REVERSAL.SetValue("OBJ_KEY_R", rev.OBJ_KEY_R);
                    REVERSAL.SetValue("PSTNG_DATE", SAPNETExtension.ToSAPDateString(rev._PostingDate));
                    REVERSAL.SetValue("FIS_PERIOD", rev.FIS_PERIOD);
                    REVERSAL.SetValue("COMP_CODE", rev.COMP_CODE);
                    REVERSAL.SetValue("REASON_REV", rev.REASON_REV);
                    REVERSAL.SetValue("AC_DOC_NO", rev.AC_DOC_NO);


                    RfcSessionManager.BeginContext(connection.Destination);
                    apAPI.Invoke(connection.Destination);

                    IRfcTable tbl_RETURN = apAPI.GetTable("RETURN");
                    DataTable dt_RETURN = GetDataTable(tbl_RETURN);
                    dt_RETURN.DefaultView.RowFilter = "TYPE='E'";
                    DataTable dt_ret = dt_RETURN.DefaultView.ToTable();

                    if (dt_ret.Rows.Count == 0)
                    {
                        OBJ_TYPE = "" + apAPI.GetValue("OBJ_TYPE");//   Reference Transaction
                        OBJ_KEY = "" + apAPI.GetValue("OBJ_KEY");//   Reference Key
                        OBJ_SYS = "" + apAPI.GetValue("OBJ_SYS");//   Logical system of source document

                        DAStockItemDestroy cls = new DAStockItemDestroy();
                        if (cls.cancelDestroyItem(destroyListId, employeeID, OBJ_TYPE, OBJ_KEY, OBJ_SYS))
                        {
                            apCMT.SetValue("WAIT", "X");
                            apCMT.Invoke(connection.Destination);
                        }
                        else
                        {
                            rst = false;
                        }
                    }
                    else
                    {
                        rst = false;
                    }
                    RfcSessionManager.EndContext(connection.Destination);

                    if (!rst)
                    {
                        transaction.Rollback();
                        msgErr = "SAPReverseDamage_Error";
                        foreach (DataRow drMsg in dt_ret.Rows)
                        {
                            msgErr += "\\n [" + drMsg["NUMBER"] + "] " + drMsg["MESSAGE"];
                        }
                        rst = false;
                    }
                    else
                    {
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    rst = false;
                    while (ex.InnerException != null)
                        ex = ex.InnerException;

                    transaction.Rollback();
                    msgErr = "SAPReverseDamage " + ex.Message;
                }
                finally
                {
                }
            }
            return rst;
        }


        /* = - SAP Get PRDetail  - = */
        public bool SAPGetPRDetail(string PrNo, ref DataTable dt_PRITEM, ref string msgErr)
        {
            bool rst = true;
            using (var connection = new SapConnection(connectionName))
            {
                connection.Open();
                var transaction = connection.BeginTransaction();

                try
                {
                    IRfcFunction apAPI = connection.CreateFunction("BAPI_PR_GETDETAIL");

                    apAPI.SetValue("NUMBER", PrNo);

                    RfcSessionManager.BeginContext(connection.Destination);
                    apAPI.Invoke(connection.Destination);

                    IRfcTable tbl_RETURN = apAPI.GetTable("RETURN");
                    DataTable dt_RETURN = GetDataTable(tbl_RETURN);
                    dt_RETURN.DefaultView.RowFilter = "TYPE='E'";
                    DataTable dt_ret = dt_RETURN.DefaultView.ToTable();

                    if (dt_ret.Rows.Count == 0)
                    {
                        IRfcTable tbl_PRITEM = apAPI.GetTable("PRITEM");
                        dt_PRITEM = GetDataTable(tbl_PRITEM);
                    }
                    else
                    {
                        rst = false;
                    }

                    RfcSessionManager.EndContext(connection.Destination);

                    if (!rst)
                    {
                        transaction.Rollback();
                        msgErr = "SAPGetPRDetail_Error";
                        foreach (DataRow drMsg in dt_ret.Rows)
                        {
                            msgErr += "\\n [" + drMsg["NUMBER"] + "] " + drMsg["MESSAGE"];
                        }
                        rst = false;
                    }
                    else
                    {
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    rst = false;
                    while (ex.InnerException != null)
                        ex = ex.InnerException;

                    transaction.Rollback();
                    msgErr = "SAPGetPRDetail " + ex.Message;
                }
                finally
                {
                }
            }
            return rst;
        }


        /* = - SAP Get PRDetail  - = */
        public bool SAPGetPRDetail(string PrNo, SapConnection connection, ref DataTable dt_PRITEM, ref string msgErr)
        {
            bool rst = true;

            SapConnection sapconn = connection;
            sapconn.Open();
            var transaction = sapconn.BeginTransaction();
            try
            {
                IRfcFunction apAPI = sapconn.CreateFunction("BAPI_PR_GETDETAIL");

                apAPI.SetValue("NUMBER", PrNo);

                RfcSessionManager.BeginContext(sapconn.Destination);
                apAPI.Invoke(sapconn.Destination);

                IRfcTable tbl_RETURN = apAPI.GetTable("RETURN");
                DataTable dt_RETURN = GetDataTable(tbl_RETURN);
                dt_RETURN.DefaultView.RowFilter = "TYPE='E'";
                DataTable dt_ret = dt_RETURN.DefaultView.ToTable();

                if (dt_ret.Rows.Count == 0)
                {
                    IRfcTable tbl_PRITEM = apAPI.GetTable("PRITEM");
                    dt_PRITEM = GetDataTable(tbl_PRITEM);
                }
                else
                {
                    rst = false;
                }
                RfcSessionManager.EndContext(sapconn.Destination);

                if (!rst)
                {
                    transaction.Rollback();
                    msgErr = "SAPGetPRDetail_Error";
                    foreach (DataRow drMsg in dt_ret.Rows)
                    {
                        msgErr += "\\n [" + drMsg["NUMBER"] + "] " + drMsg["MESSAGE"];
                    }
                    rst = false;
                }
                else
                {
                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                rst = false;
                while (ex.InnerException != null)
                    ex = ex.InnerException;

                transaction.Rollback();
                msgErr = "SAPGetPRDetail " + ex.Message;
            }
            finally
            {
            }

            return rst;
        }





        /* = - Un Release PR - Post Account  - = */
        public bool SAPUnReleasePR(string PrNo, ref string msgErr)
        {
            bool rst = true;
            using (var connection = new SapConnection(connectionName))
            {
                connection.Open();
                var transaction = connection.BeginTransaction();
                try
                {
                    IRfcFunction apAPI = connection.CreateFunction("BAPI_REQUISITION_RESET_REL_GEN");
                    IRfcFunction apCMT = connection.CreateFunction("BAPI_TRANSACTION_COMMIT");

                    apAPI.SetValue("NUMBER", PrNo);
                    apAPI.SetValue("REL_CODE", "AU");
                    apAPI.SetValue("NO_COMMIT_WORK", "X");

                    RfcSessionManager.BeginContext(connection.Destination);
                    apAPI.Invoke(connection.Destination);

                    IRfcTable tbl_RETURN = apAPI.GetTable("RETURN");
                    DataTable dt_RETURN = GetDataTable(tbl_RETURN);
                    dt_RETURN.DefaultView.RowFilter = "TYPE='E'";
                    DataTable dt_ret = dt_RETURN.DefaultView.ToTable();

                    if (dt_ret.Rows.Count != 0)
                    {
                        rst = false;
                    }
                    else
                    {
                        apCMT.SetValue("WAIT", "X");
                        apCMT.Invoke(connection.Destination);
                    }

                    RfcSessionManager.EndContext(connection.Destination);

                    if (!rst)
                    {
                        transaction.Rollback();
                        msgErr = "SAPUnReleasePR_Error";
                        foreach (DataRow drMsg in dt_ret.Rows)
                        {
                            msgErr += "\\n [" + drMsg["CODE"] + "] " + drMsg["MESSAGE"];
                        }
                        rst = false;
                    }
                    else
                    {
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    rst = false;
                    while (ex.InnerException != null)
                        ex = ex.InnerException;

                    transaction.Rollback();
                    msgErr = "SAPGetPRDetail " + ex.Message;
                }
                finally
                {
                }
            }
            return rst;
        }        




        public DataTable GetDataTable(IRfcTable i_Table)
        {
            DataTable dt = new DataTable();
            GetColumnsFromSapTable(ref dt, i_Table);
            FillRowsFromSapTable(ref dt, i_Table);
            return dt;
        }

        public void FillRowsFromSapTable(ref DataTable i_DataTable, IRfcTable i_Table)
        {
            foreach (IRfcStructure tableRow in i_Table)
            {
                DataRow dr = i_DataTable.NewRow();
                dr.ItemArray = tableRow.Select(structField => structField.GetValue()).ToArray();
                i_DataTable.Rows.Add(dr);
            }
        }

        public void GetColumnsFromSapTable(ref DataTable i_DataTable, IRfcTable i_SapTable)
        {
            var DataColumnsArr = i_SapTable.Metadata.LineType.CreateStructure().ToList().Select
            (structField => new DataColumn(structField.Metadata.Name)).ToArray();
            i_DataTable.Columns.AddRange(DataColumnsArr);
        }


        /* = - Delivery Post Account Marketing Cross Company - = */
        public bool deliveryLowPricePostAccountCrossCmp(string DelvLst
                                                    , SapDeliveryCrossCmpStep1_DOCUMENTHEADER hs1
                                                    , List<SapDeliveryCrossCmpStep1_ACCOUNTGL> AccGLS1
                                                    , List<SapDeliveryCrossCmpStep1_ACCOUNTPAYABLE> accPaS1
                                                    , List<SapDeliveryCrossCmpStep1_CURRENCYAMOUNT> accAmtS1

                                                    , SapDeliveryCrossCmpStep2_DOCUMENTHEADER hs2
                                                    , List<SapDeliveryCrossCmpStep2_ACCOUNTGL> accGLS2
                                                    , List<SapDeliveryCrossCmpStep2_CURRENCYAMOUNT> accAmtS2

                                                    , SapDeliveryCrossCmpStep3_DOCUMENTHEADER hs3
                                                    , List<SapDeliveryCrossCmpStep3_ACCOUNTGL> accGLS3
                                                    , List<SapDeliveryCrossCmpStep3_ACCOUNTRECEIVABLE> accRcS3
                                                    , List<SapDeliveryCrossCmpStep3_CURRENCYAMOUNT> accAmtS3
                                                    , ref string SAPDocNo1, ref string SAPDocNo2, ref string SAPDocNo3, string EmployeeID, ref string msgErr)
        {
            bool rst = true;
            using (var connection = new SapConnection(connectionName))
            {
                connection.Open();
                var transaction = connection.BeginTransaction();
                try
                {
                    /* - Doc1 - */
                    IRfcFunction apAPI = connection.CreateFunction("BAPI_ACC_DOCUMENT_POST");
                    IRfcFunction apCMT = connection.CreateFunction("BAPI_TRANSACTION_COMMIT");
                    /* - Doc2 - */
                    IRfcFunction apAPI2 = connection.CreateFunction("BAPI_ACC_DOCUMENT_POST");
                    IRfcFunction apCMT2 = connection.CreateFunction("BAPI_TRANSACTION_COMMIT");
                    /* - Doc3 - */
                    IRfcFunction apAPI3 = connection.CreateFunction("BAPI_ACC_DOCUMENT_POST");
                    IRfcFunction apCMT3 = connection.CreateFunction("BAPI_TRANSACTION_COMMIT");

                    /* - #Doc1 - */
                    IRfcStructure DOCHDR = apAPI.GetStructure("DOCUMENTHEADER");
                    DOCHDR.SetValue("USERNAME", hs1.USERNAME);
                    DOCHDR.SetValue("COMP_CODE", hs1.COMP_CODE);
                    DOCHDR.SetValue("DOC_DATE", SAPNETExtension.ToSAPDateString(hs1._DOC_DATE));
                    DOCHDR.SetValue("PSTNG_DATE", SAPNETExtension.ToSAPDateString(hs1._PSTNG_DATE));
                    DOCHDR.SetValue("FISC_YEAR", hs1.FISC_YEAR);
                    DOCHDR.SetValue("DOC_TYPE", hs1.DOC_TYPE);
                    DOCHDR.SetValue("REF_DOC_NO", hs1.REF_DOC_NO);

                    IRfcTable ACCGL = apAPI.GetTable("ACCOUNTGL");
                    foreach (SapDeliveryCrossCmpStep1_ACCOUNTGL accGL in AccGLS1)
                    {
                        ACCGL.Append();
                        ACCGL.SetValue("ITEMNO_ACC", accGL.ITEMNO_ACC);
                        ACCGL.SetValue("GL_ACCOUNT", accGL.GL_ACCOUNT);
                        ACCGL.SetValue("ITEM_TEXT", accGL.ITEM_TEXT);
                        ACCGL.SetValue("PROFIT_CTR", accGL.PROFIT_CTR);

                        ACCGL.SetValue("REF_KEY_3", accGL.REF_KEY_3);
                        ACCGL.SetValue("ALLOC_NMBR", accGL.ALLOC_NMBR);
                    }

                    IRfcTable ACCPA = apAPI.GetTable("ACCOUNTPAYABLE");
                    foreach (SapDeliveryCrossCmpStep1_ACCOUNTPAYABLE accGL in accPaS1)
                    {
                        ACCPA.Append();
                        ACCPA.SetValue("ITEMNO_ACC", accGL.ITEMNO_ACC);
                        ACCPA.SetValue("VENDOR_NO", accGL.VENDOR_NO);
                        ACCPA.SetValue("BUSINESSPLACE", accGL.BUSINESSPLACE);
                        ACCPA.SetValue("PROFIT_CTR", accGL.PROFIT_CTR);
                        ACCPA.SetValue("ITEM_TEXT", accGL.ITEM_TEXT);
                    }

                    IRfcTable CURAMT = apAPI.GetTable("CURRENCYAMOUNT");
                    foreach (SapDeliveryCrossCmpStep1_CURRENCYAMOUNT curAmt in accAmtS1)
                    {
                        CURAMT.Append();
                        CURAMT.SetValue("ITEMNO_ACC", curAmt.ITEMNO_ACC);
                        CURAMT.SetValue("CURRENCY", curAmt.CURRENCY);
                        CURAMT.SetValue("AMT_DOCCUR", curAmt.AMT_DOCCUR);
                    }

                    RfcSessionManager.BeginContext(connection.Destination);
                    apAPI.Invoke(connection.Destination);

                    IRfcTable tbl_RETURN = apAPI.GetTable("RETURN");

                    string OBJ_TYPE = "" + apAPI.GetValue("OBJ_TYPE");//   Reference Transaction
                    string OBJ_KEY = "" + apAPI.GetValue("OBJ_KEY");//   Reference Key
                    string OBJ_SYS = "" + apAPI.GetValue("OBJ_SYS");//   Logical system of source document

                    DataTable dt_RETURN = GetDataTable(tbl_RETURN);
                    dt_RETURN.DefaultView.RowFilter = "TYPE='E'";
                    DataTable dt_ret = dt_RETURN.DefaultView.ToTable();
                    if (dt_ret.Rows.Count == 0)
                    {
                        SAPDocNo1 = OBJ_KEY;//.Substring(0, 10);

                        /* - #Doc2 - */
                        IRfcStructure DOCHDR2 = apAPI2.GetStructure("DOCUMENTHEADER");
                        DOCHDR2.SetValue("USERNAME", hs2.USERNAME);
                        DOCHDR2.SetValue("COMP_CODE", hs2.COMP_CODE);
                        DOCHDR2.SetValue("DOC_DATE", SAPNETExtension.ToSAPDateString(hs2._DOC_DATE));
                        DOCHDR2.SetValue("PSTNG_DATE", SAPNETExtension.ToSAPDateString(hs2._PSTNG_DATE));
                        DOCHDR2.SetValue("FISC_YEAR", hs2.FISC_YEAR);
                        DOCHDR2.SetValue("DOC_TYPE", hs2.DOC_TYPE);
                        DOCHDR2.SetValue("REF_DOC_NO", hs2.REF_DOC_NO);

                        IRfcTable ACCGL2 = apAPI2.GetTable("ACCOUNTGL");
                        foreach (SapDeliveryCrossCmpStep2_ACCOUNTGL accGL in accGLS2)
                        {
                            ACCGL2.Append();
                            ACCGL2.SetValue("ITEMNO_ACC", accGL.ITEMNO_ACC);
                            ACCGL2.SetValue("GL_ACCOUNT", accGL.GL_ACCOUNT);
                            ACCGL2.SetValue("ITEM_TEXT", accGL.ITEM_TEXT);
                            ACCGL2.SetValue("ORDERID", accGL.ORDERID);
                            ACCGL2.SetValue("PROFIT_CTR", accGL.PROFIT_CTR);

                            ACCGL2.SetValue("REF_KEY_3", accGL.REF_KEY_3);
                            ACCGL2.SetValue("ALLOC_NMBR", accGL.ALLOC_NMBR);
                        }

                        IRfcTable CURAMT2 = apAPI2.GetTable("CURRENCYAMOUNT");
                        foreach (SapDeliveryCrossCmpStep2_CURRENCYAMOUNT curAmt in accAmtS2)
                        {
                            CURAMT2.Append();
                            CURAMT2.SetValue("ITEMNO_ACC", curAmt.ITEMNO_ACC);
                            CURAMT2.SetValue("CURRENCY", curAmt.CURRENCY);
                            CURAMT2.SetValue("AMT_DOCCUR", curAmt.AMT_DOCCUR);
                        }

                        RfcSessionManager.BeginContext(connection.Destination);
                        apAPI2.Invoke(connection.Destination);

                        IRfcTable tbl_RETURN2 = apAPI2.GetTable("RETURN");

                        string OBJ_TYPE2 = "" + apAPI2.GetValue("OBJ_TYPE");//   Reference Transaction
                        string OBJ_KEY2 = "" + apAPI2.GetValue("OBJ_KEY");//   Reference Key
                        string OBJ_SYS2 = "" + apAPI2.GetValue("OBJ_SYS");//   Logical system of source document

                        DataTable dt_RETURN2 = GetDataTable(tbl_RETURN2);
                        dt_RETURN2.DefaultView.RowFilter = "TYPE='E'";
                        DataTable dt_ret2 = dt_RETURN2.DefaultView.ToTable();
                        if (dt_ret2.Rows.Count == 0)
                        {
                            SAPDocNo2 = OBJ_KEY2;//.Substring(0, 10);

                            /* - #Doc3 - */
                            IRfcStructure DOCHDR3 = apAPI3.GetStructure("DOCUMENTHEADER");
                            DOCHDR3.SetValue("USERNAME", hs3.USERNAME);
                            DOCHDR3.SetValue("COMP_CODE", hs3.COMP_CODE);
                            DOCHDR3.SetValue("DOC_DATE", SAPNETExtension.ToSAPDateString(hs3._DOC_DATE));
                            DOCHDR3.SetValue("PSTNG_DATE", SAPNETExtension.ToSAPDateString(hs3._PSTNG_DATE));
                            DOCHDR3.SetValue("FISC_YEAR", hs3.FISC_YEAR);
                            DOCHDR3.SetValue("DOC_TYPE", hs3.DOC_TYPE);
                            DOCHDR3.SetValue("REF_DOC_NO", hs3.REF_DOC_NO);

                            IRfcTable ACCRC3 = apAPI3.GetTable("ACCOUNTRECEIVABLE");
                            foreach (SapDeliveryCrossCmpStep3_ACCOUNTRECEIVABLE accGL in accRcS3)
                            {
                                ACCRC3.Append();
                                ACCRC3.SetValue("ITEMNO_ACC", accGL.ITEMNO_ACC);
                                ACCRC3.SetValue("CUSTOMER", accGL.CUSTOMER);
                                ACCRC3.SetValue("PROFIT_CTR", accGL.PROFIT_CTR);
                                ACCRC3.SetValue("BUSINESSPLACE", accGL.BUSINESSPLACE);
                                ACCRC3.SetValue("ITEM_TEXT", accGL.ITEM_TEXT);
                            }

                            IRfcTable ACCGL3 = apAPI3.GetTable("ACCOUNTGL");
                            foreach (SapDeliveryCrossCmpStep3_ACCOUNTGL accGL in accGLS3)
                            {
                                ACCGL3.Append();
                                ACCGL3.SetValue("ITEMNO_ACC", accGL.ITEMNO_ACC);
                                ACCGL3.SetValue("GL_ACCOUNT", accGL.GL_ACCOUNT);
                                ACCGL3.SetValue("ITEM_TEXT", accGL.ITEM_TEXT);
                                ACCGL3.SetValue("PROFIT_CTR", accGL.PROFIT_CTR);
                                ACCGL3.SetValue("REF_KEY_3", accGL.REF_KEY_3);

                                ACCGL3.SetValue("REF_KEY_3", accGL.REF_KEY_3);
                                ACCGL3.SetValue("ALLOC_NMBR", accGL.ALLOC_NMBR);
                            }

                            IRfcTable CURAMT3 = apAPI3.GetTable("CURRENCYAMOUNT");
                            foreach (SapDeliveryCrossCmpStep3_CURRENCYAMOUNT curAmt in accAmtS3)
                            {
                                CURAMT3.Append();
                                CURAMT3.SetValue("ITEMNO_ACC", curAmt.ITEMNO_ACC);
                                CURAMT3.SetValue("CURRENCY", curAmt.CURRENCY);
                                CURAMT3.SetValue("AMT_DOCCUR", curAmt.AMT_DOCCUR);
                            }

                            RfcSessionManager.BeginContext(connection.Destination);
                            apAPI3.Invoke(connection.Destination);

                            IRfcTable tbl_RETURN3 = apAPI3.GetTable("RETURN");

                            string OBJ_TYPE3 = "" + apAPI3.GetValue("OBJ_TYPE");//   Reference Transaction
                            string OBJ_KEY3 = "" + apAPI3.GetValue("OBJ_KEY");//   Reference Key
                            string OBJ_SYS3 = "" + apAPI3.GetValue("OBJ_SYS");//   Logical system of source document

                            DataTable dt_RETURN3 = GetDataTable(tbl_RETURN3);
                            dt_RETURN3.DefaultView.RowFilter = "TYPE='E'";
                            DataTable dt_ret3 = dt_RETURN3.DefaultView.ToTable();
                            if (dt_ret3.Rows.Count == 0)
                            {
                                SAPDocNo3 = OBJ_KEY3;//.Substring(0, 10);
                                DADelivery cls = new DADelivery();
                                if (!cls.UpdateStatusPostAccountCrossCmp(DelvLst, OBJ_TYPE, OBJ_KEY, OBJ_SYS, OBJ_TYPE2, OBJ_KEY2, OBJ_SYS2, OBJ_TYPE3, OBJ_KEY3, OBJ_SYS3, hs1.USERNAME, hs1._PSTNG_DATE, hs1.REF_DOC_NO, EmployeeID, ref msgErr))
                                {
                                    rst = false;
                                }
                            }
                            else
                            {
                                rst = false;
                                msgErr += "\\n Error-Step3:";
                                foreach (DataRow drMsg in dt_ret3.Rows)
                                {
                                    msgErr += "\\n [" + drMsg["NUMBER"] + "] " + drMsg["MESSAGE"];
                                }
                            }
                        }
                        else
                        {
                            rst = false;
                            msgErr += "\\n Error-Step2:";
                            foreach (DataRow drMsg in dt_ret2.Rows)
                            {
                                msgErr += "\\n [" + drMsg["NUMBER"] + "] " + drMsg["MESSAGE"];
                            }
                        }
                    }
                    else
                    {
                        rst = false;
                        msgErr += "\\n Error-Step1:";
                        foreach (DataRow drMsg in dt_ret.Rows)
                        {
                            msgErr += "\\n [" + drMsg["NUMBER"] + "] " + drMsg["MESSAGE"];
                        }
                    }

                    if (rst)
                    {
                        apCMT.SetValue("WAIT", "X");
                        apCMT.Invoke(connection.Destination);
                        apCMT2.SetValue("WAIT", "X");
                        apCMT2.Invoke(connection.Destination);
                        apCMT3.SetValue("WAIT", "X");
                        apCMT3.Invoke(connection.Destination);
                    }

                    RfcSessionManager.EndContext(connection.Destination);

                    if (!rst)
                    {
                        msgErr = "deliveryLowPricePostAccountCrossCmp_Error" + msgErr;

                        transaction.Rollback();
                        rst = false;
                    }
                    else
                    {
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    rst = false;
                    while (ex.InnerException != null)
                        ex = ex.InnerException;

                    transaction.Rollback();
                    msgErr = "deliveryLowPricePostAccountCrossCmp_Error " + ex.Message;
                }
                finally
                {
                }
            }
            return rst;
        }


        /* = - Delivery Post Account Marketing Cress Company [Check] - = */
        public bool deliveryLowPricePostAccountCrossCmpCheck(SapDeliveryCrossCmpStep1_DOCUMENTHEADER hs1
                                                    , List<SapDeliveryCrossCmpStep1_ACCOUNTGL> AccGLS1
                                                    , List<SapDeliveryCrossCmpStep1_ACCOUNTPAYABLE> accPaS1
                                                    , List<SapDeliveryCrossCmpStep1_CURRENCYAMOUNT> accAmtS1

                                                    , SapDeliveryCrossCmpStep2_DOCUMENTHEADER hs2
                                                    , List<SapDeliveryCrossCmpStep2_ACCOUNTGL> accGLS2
                                                    , List<SapDeliveryCrossCmpStep2_CURRENCYAMOUNT> accAmtS2

                                                    , SapDeliveryCrossCmpStep3_DOCUMENTHEADER hs3
                                                    , List<SapDeliveryCrossCmpStep3_ACCOUNTGL> accGLS3
                                                    , List<SapDeliveryCrossCmpStep3_ACCOUNTRECEIVABLE> accRcS3
                                                    , List<SapDeliveryCrossCmpStep3_CURRENCYAMOUNT> accAmtS3
                                                    , ref string msgErr)
        {
            bool rst = true;
            using (var connection = new SapConnection(connectionName))
            {
                connection.Open();

                var transaction = connection.BeginTransaction();
                try
                {
                    IRfcFunction apAPI = connection.CreateFunction("BAPI_ACC_DOCUMENT_CHECK");

                    IRfcStructure DOCHDR = apAPI.GetStructure("DOCUMENTHEADER");
                    DOCHDR.SetValue("USERNAME", hs1.USERNAME);
                    DOCHDR.SetValue("COMP_CODE", hs1.COMP_CODE);
                    DOCHDR.SetValue("DOC_DATE", SAPNETExtension.ToSAPDateString(hs1._DOC_DATE));
                    DOCHDR.SetValue("PSTNG_DATE", SAPNETExtension.ToSAPDateString(hs1._PSTNG_DATE));
                    DOCHDR.SetValue("FISC_YEAR", hs1.FISC_YEAR);
                    DOCHDR.SetValue("DOC_TYPE", hs1.DOC_TYPE);
                    DOCHDR.SetValue("REF_DOC_NO", hs1.REF_DOC_NO);

                    IRfcTable ACCGL = apAPI.GetTable("ACCOUNTGL");
                    foreach (SapDeliveryCrossCmpStep1_ACCOUNTGL accGL in AccGLS1)
                    {
                        ACCGL.Append();
                        ACCGL.SetValue("ITEMNO_ACC", accGL.ITEMNO_ACC);
                        ACCGL.SetValue("GL_ACCOUNT", accGL.GL_ACCOUNT);
                        ACCGL.SetValue("ITEM_TEXT", accGL.ITEM_TEXT);
                        ACCGL.SetValue("PROFIT_CTR", accGL.PROFIT_CTR);

                        ACCGL.SetValue("REF_KEY_3", accGL.REF_KEY_3);
                        ACCGL.SetValue("ALLOC_NMBR", accGL.ALLOC_NMBR);
                    }

                    IRfcTable ACCPA = apAPI.GetTable("ACCOUNTPAYABLE");
                    foreach (SapDeliveryCrossCmpStep1_ACCOUNTPAYABLE accGL in accPaS1)
                    {
                        ACCPA.Append();
                        ACCPA.SetValue("ITEMNO_ACC", accGL.ITEMNO_ACC);
                        ACCPA.SetValue("VENDOR_NO", accGL.VENDOR_NO);
                        ACCPA.SetValue("BUSINESSPLACE", accGL.BUSINESSPLACE);
                        ACCPA.SetValue("PROFIT_CTR", accGL.PROFIT_CTR);
                        ACCPA.SetValue("ITEM_TEXT", accGL.ITEM_TEXT);
                    }

                    IRfcTable CURAMT = apAPI.GetTable("CURRENCYAMOUNT");
                    foreach (SapDeliveryCrossCmpStep1_CURRENCYAMOUNT curAmt in accAmtS1)
                    {
                        CURAMT.Append();
                        CURAMT.SetValue("ITEMNO_ACC", curAmt.ITEMNO_ACC);
                        CURAMT.SetValue("CURRENCY", curAmt.CURRENCY);
                        CURAMT.SetValue("AMT_DOCCUR", curAmt.AMT_DOCCUR);
                    }

                    RfcSessionManager.BeginContext(connection.Destination);
                    apAPI.Invoke(connection.Destination);

                    IRfcTable tbl_RETURN = apAPI.GetTable("RETURN");
                    DataTable dt_RETURN = GetDataTable(tbl_RETURN);
                    dt_RETURN.DefaultView.RowFilter = "TYPE='E'";
                    DataTable dt_ret = dt_RETURN.DefaultView.ToTable();
                    if (dt_ret.Rows.Count != 0)
                    {
                        rst = false;
                        msgErr += "\\n Error-Step1:";
                        foreach (DataRow drMsg in dt_ret.Rows)
                        {
                            msgErr += "\\n [" + drMsg["NUMBER"] + "] " + drMsg["MESSAGE"];
                        }
                    }

                    /* - Doc2 - */
                    IRfcFunction apAPI2 = connection.CreateFunction("BAPI_ACC_DOCUMENT_CHECK");

                    IRfcStructure DOCHDR2 = apAPI2.GetStructure("DOCUMENTHEADER");
                    DOCHDR2.SetValue("USERNAME", hs2.USERNAME);
                    DOCHDR2.SetValue("COMP_CODE", hs2.COMP_CODE);
                    DOCHDR2.SetValue("DOC_DATE", SAPNETExtension.ToSAPDateString(hs2._DOC_DATE));
                    DOCHDR2.SetValue("PSTNG_DATE", SAPNETExtension.ToSAPDateString(hs2._PSTNG_DATE));
                    DOCHDR2.SetValue("FISC_YEAR", hs2.FISC_YEAR);
                    DOCHDR2.SetValue("DOC_TYPE", hs2.DOC_TYPE);
                    DOCHDR2.SetValue("REF_DOC_NO", hs2.REF_DOC_NO);

                    IRfcTable ACCGL2 = apAPI2.GetTable("ACCOUNTGL");
                    foreach (SapDeliveryCrossCmpStep2_ACCOUNTGL accGL in accGLS2)
                    {
                        ACCGL2.Append();
                        ACCGL2.SetValue("ITEMNO_ACC", accGL.ITEMNO_ACC);
                        ACCGL2.SetValue("GL_ACCOUNT", accGL.GL_ACCOUNT);
                        ACCGL2.SetValue("ITEM_TEXT", accGL.ITEM_TEXT);
                        ACCGL2.SetValue("ORDERID", accGL.ORDERID);
                        ACCGL2.SetValue("PROFIT_CTR", accGL.PROFIT_CTR);

                        ACCGL2.SetValue("REF_KEY_3", accGL.REF_KEY_3);
                        ACCGL2.SetValue("ALLOC_NMBR", accGL.ALLOC_NMBR);
                    }

                    IRfcTable CURAMT2 = apAPI2.GetTable("CURRENCYAMOUNT");
                    foreach (SapDeliveryCrossCmpStep2_CURRENCYAMOUNT curAmt in accAmtS2)
                    {
                        CURAMT2.Append();
                        CURAMT2.SetValue("ITEMNO_ACC", curAmt.ITEMNO_ACC);
                        CURAMT2.SetValue("CURRENCY", curAmt.CURRENCY);
                        CURAMT2.SetValue("AMT_DOCCUR", curAmt.AMT_DOCCUR);
                    }

                    RfcSessionManager.BeginContext(connection.Destination);
                    apAPI2.Invoke(connection.Destination);

                    IRfcTable tbl_RETURN2 = apAPI2.GetTable("RETURN");
                    DataTable dt_RETURN2 = GetDataTable(tbl_RETURN2);
                    dt_RETURN2.DefaultView.RowFilter = "TYPE='E'";
                    DataTable dt_ret2 = dt_RETURN2.DefaultView.ToTable();
                    if (dt_ret2.Rows.Count != 0)
                    {
                        {
                            rst = false;
                            msgErr += "\\n Error-Step2:";
                            foreach (DataRow drMsg in dt_ret2.Rows)
                            {
                                msgErr += "\\n [" + drMsg["NUMBER"] + "] " + drMsg["MESSAGE"];
                            }
                        }
                    }

                    /* - Doc3 - */
                    IRfcFunction apAPI3 = connection.CreateFunction("BAPI_ACC_DOCUMENT_CHECK");

                    IRfcStructure DOCHDR3 = apAPI3.GetStructure("DOCUMENTHEADER");
                    DOCHDR3.SetValue("USERNAME", hs3.USERNAME);
                    DOCHDR3.SetValue("COMP_CODE", hs3.COMP_CODE);
                    DOCHDR3.SetValue("DOC_DATE", SAPNETExtension.ToSAPDateString(hs3._DOC_DATE));
                    DOCHDR3.SetValue("PSTNG_DATE", SAPNETExtension.ToSAPDateString(hs3._PSTNG_DATE));
                    DOCHDR3.SetValue("FISC_YEAR", hs3.FISC_YEAR);
                    DOCHDR3.SetValue("DOC_TYPE", hs3.DOC_TYPE);
                    DOCHDR3.SetValue("REF_DOC_NO", hs3.REF_DOC_NO);

                    IRfcTable ACCRC3 = apAPI3.GetTable("ACCOUNTRECEIVABLE");
                    foreach (SapDeliveryCrossCmpStep3_ACCOUNTRECEIVABLE accGL in accRcS3)
                    {
                        ACCRC3.Append();
                        ACCRC3.SetValue("ITEMNO_ACC", accGL.ITEMNO_ACC);
                        ACCRC3.SetValue("CUSTOMER", accGL.CUSTOMER);
                        ACCRC3.SetValue("PROFIT_CTR", accGL.PROFIT_CTR);
                        ACCRC3.SetValue("BUSINESSPLACE", accGL.BUSINESSPLACE);
                        ACCRC3.SetValue("ITEM_TEXT", accGL.ITEM_TEXT);
                    }

                    IRfcTable ACCGL3 = apAPI3.GetTable("ACCOUNTGL");
                    foreach (SapDeliveryCrossCmpStep3_ACCOUNTGL accGL in accGLS3)
                    {
                        ACCGL3.Append();
                        ACCGL3.SetValue("ITEMNO_ACC", accGL.ITEMNO_ACC);
                        ACCGL3.SetValue("GL_ACCOUNT", accGL.GL_ACCOUNT);
                        ACCGL3.SetValue("ITEM_TEXT", accGL.ITEM_TEXT);
                        ACCGL3.SetValue("PROFIT_CTR", accGL.PROFIT_CTR);

                        ACCGL3.SetValue("REF_KEY_3", accGL.REF_KEY_3);
                        ACCGL3.SetValue("ALLOC_NMBR", accGL.ALLOC_NMBR);
                    }

                    IRfcTable CURAMT3 = apAPI3.GetTable("CURRENCYAMOUNT");
                    foreach (SapDeliveryCrossCmpStep3_CURRENCYAMOUNT curAmt in accAmtS3)
                    {
                        CURAMT3.Append();
                        CURAMT3.SetValue("ITEMNO_ACC", curAmt.ITEMNO_ACC);
                        CURAMT3.SetValue("CURRENCY", curAmt.CURRENCY);
                        CURAMT3.SetValue("AMT_DOCCUR", curAmt.AMT_DOCCUR);
                    }

                    RfcSessionManager.BeginContext(connection.Destination);
                    apAPI3.Invoke(connection.Destination);

                    IRfcTable tbl_RETURN3 = apAPI3.GetTable("RETURN");
                    DataTable dt_RETURN3 = GetDataTable(tbl_RETURN3);
                    dt_RETURN3.DefaultView.RowFilter = "TYPE='E'";
                    DataTable dt_ret3 = dt_RETURN3.DefaultView.ToTable();
                    if (dt_ret3.Rows.Count != 0)
                    {
                        rst = false;
                        msgErr += "\\n Error-Step3:";
                        foreach (DataRow drMsg in dt_ret3.Rows)
                        {
                            msgErr += "\\n [" + drMsg["NUMBER"] + "] " + drMsg["MESSAGE"];
                        }
                    }

                    RfcSessionManager.EndContext(connection.Destination);

                    if (!rst)
                    {
                        msgErr = "deliveryLowPricePostAccountCrossCmp_Error" + msgErr;
                        transaction.Rollback();
                        rst = false;
                    }
                    else
                    {
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    rst = false;
                    while (ex.InnerException != null)
                        ex = ex.InnerException;

                    transaction.Rollback();
                    msgErr = "deliveryLowPricePostAccountCrossCmp_Error " + ex.Message;
                }
                finally
                {
                }
            }
            return rst;
        }


        /* = - Delivery Post Account WBS Cress Company [Check] - = */
        public bool deliveryPostAccountCrossCmpCheck(SapDeliveryCrossCmpStep1_DOCUMENTHEADER hs1
                                                    , List<SapDeliveryCrossCmpStep1_ACCOUNTGL> AccGLS1
                                                    , List<SapDeliveryCrossCmpStep1_ACCOUNTPAYABLE> accPaS1
                                                    , List<SapDeliveryCrossCmpStep1_CURRENCYAMOUNT> accAmtS1

                                                    , SapDeliveryCrossCmpStep2_DOCUMENTHEADER hs2
                                                    , List<SapDeliveryCrossCmpStep2_ACCOUNTGL> accGLS2
                                                    , List<SapDeliveryCrossCmpStep2_CURRENCYAMOUNT> accAmtS2

                                                    , SapDeliveryCrossCmpStep3_DOCUMENTHEADER hs3
                                                    , List<SapDeliveryCrossCmpStep3_ACCOUNTGL> accGLS3
                                                    , List<SapDeliveryCrossCmpStep3_ACCOUNTRECEIVABLE> accRcS3
                                                    , List<SapDeliveryCrossCmpStep3_CURRENCYAMOUNT> accAmtS3
                                                    , ref string msgErr)
        {
            bool rst = true;
            using (var connection = new SapConnection(connectionName))
            {
                connection.Open();

                var transaction = connection.BeginTransaction();
                try
                {
                    IRfcFunction apAPI = connection.CreateFunction("BAPI_ACC_DOCUMENT_CHECK");

                    IRfcStructure DOCHDR = apAPI.GetStructure("DOCUMENTHEADER");
                    DOCHDR.SetValue("USERNAME", hs1.USERNAME);
                    DOCHDR.SetValue("COMP_CODE", hs1.COMP_CODE);
                    DOCHDR.SetValue("DOC_DATE", SAPNETExtension.ToSAPDateString(hs1._DOC_DATE));
                    DOCHDR.SetValue("PSTNG_DATE", SAPNETExtension.ToSAPDateString(hs1._PSTNG_DATE));
                    DOCHDR.SetValue("FISC_YEAR", hs1.FISC_YEAR);
                    DOCHDR.SetValue("DOC_TYPE", hs1.DOC_TYPE);
                    DOCHDR.SetValue("REF_DOC_NO", hs1.REF_DOC_NO);

                    IRfcTable ACCGL = apAPI.GetTable("ACCOUNTGL");
                    foreach (SapDeliveryCrossCmpStep1_ACCOUNTGL accGL in AccGLS1)
                    {
                        ACCGL.Append();
                        ACCGL.SetValue("ITEMNO_ACC", accGL.ITEMNO_ACC);
                        ACCGL.SetValue("GL_ACCOUNT", accGL.GL_ACCOUNT);
                        ACCGL.SetValue("ITEM_TEXT", accGL.ITEM_TEXT);
                        ACCGL.SetValue("PROFIT_CTR", accGL.PROFIT_CTR);

                        ACCGL.SetValue("REF_KEY_3", accGL.REF_KEY_3);
                        ACCGL.SetValue("ALLOC_NMBR", accGL.ALLOC_NMBR);
                    }

                    IRfcTable ACCPA = apAPI.GetTable("ACCOUNTPAYABLE");
                    foreach (SapDeliveryCrossCmpStep1_ACCOUNTPAYABLE accGL in accPaS1)
                    {
                        ACCPA.Append();
                        ACCPA.SetValue("ITEMNO_ACC", accGL.ITEMNO_ACC);
                        ACCPA.SetValue("VENDOR_NO", accGL.VENDOR_NO);
                        ACCPA.SetValue("BUSINESSPLACE", accGL.BUSINESSPLACE);
                        ACCPA.SetValue("PROFIT_CTR", accGL.PROFIT_CTR);
                        ACCPA.SetValue("ITEM_TEXT", accGL.ITEM_TEXT);
                    }

                    IRfcTable CURAMT = apAPI.GetTable("CURRENCYAMOUNT");
                    foreach (SapDeliveryCrossCmpStep1_CURRENCYAMOUNT curAmt in accAmtS1)
                    {
                        CURAMT.Append();
                        CURAMT.SetValue("ITEMNO_ACC", curAmt.ITEMNO_ACC);
                        CURAMT.SetValue("CURRENCY", curAmt.CURRENCY);
                        CURAMT.SetValue("AMT_DOCCUR", curAmt.AMT_DOCCUR);
                    }

                    RfcSessionManager.BeginContext(connection.Destination);
                    apAPI.Invoke(connection.Destination);

                    IRfcTable tbl_RETURN = apAPI.GetTable("RETURN");
                    DataTable dt_RETURN = GetDataTable(tbl_RETURN);
                    dt_RETURN.DefaultView.RowFilter = "TYPE='E'";
                    DataTable dt_ret = dt_RETURN.DefaultView.ToTable();
                    if (dt_ret.Rows.Count != 0)
                    {
                        rst = false;
                        msgErr += "\\n Error-Step1:";
                        foreach (DataRow drMsg in dt_ret.Rows)
                        {
                            msgErr += "\\n [" + drMsg["NUMBER"] + "] " + drMsg["MESSAGE"];
                        }
                    }

                    /* - Doc2 - */
                    IRfcFunction apAPI2 = connection.CreateFunction("BAPI_ACC_DOCUMENT_CHECK");

                    IRfcStructure DOCHDR2 = apAPI2.GetStructure("DOCUMENTHEADER");
                    DOCHDR2.SetValue("USERNAME", hs2.USERNAME);
                    DOCHDR2.SetValue("COMP_CODE", hs2.COMP_CODE);
                    DOCHDR2.SetValue("DOC_DATE", SAPNETExtension.ToSAPDateString(hs2._DOC_DATE));
                    DOCHDR2.SetValue("PSTNG_DATE", SAPNETExtension.ToSAPDateString(hs2._PSTNG_DATE));
                    DOCHDR2.SetValue("FISC_YEAR", hs2.FISC_YEAR);
                    DOCHDR2.SetValue("DOC_TYPE", hs2.DOC_TYPE);
                    DOCHDR2.SetValue("REF_DOC_NO", hs2.REF_DOC_NO);

                    IRfcTable ACCGL2 = apAPI2.GetTable("ACCOUNTGL");
                    foreach (SapDeliveryCrossCmpStep2_ACCOUNTGL accGL in accGLS2)
                    {
                        ACCGL2.Append();
                        ACCGL2.SetValue("ITEMNO_ACC", accGL.ITEMNO_ACC);
                        ACCGL2.SetValue("GL_ACCOUNT", accGL.GL_ACCOUNT);
                        ACCGL2.SetValue("ITEM_TEXT", accGL.ITEM_TEXT + "");
                        ACCGL2.SetValue("REF_KEY_2", accGL.REF_KEY_2 + "");
                        ACCGL2.SetValue("COSTCENTER", accGL.COSTCENTER + "");
                        ACCGL2.SetValue("WBS_ELEMENT", accGL.WBS_ELEMENT + "");
                        ACCGL2.SetValue("PROFIT_CTR", accGL.PROFIT_CTR + "");

                        ACCGL2.SetValue("REF_KEY_3", accGL.REF_KEY_3);
                        ACCGL2.SetValue("ALLOC_NMBR", accGL.ALLOC_NMBR);
                    }

                    IRfcTable CURAMT2 = apAPI2.GetTable("CURRENCYAMOUNT");
                    foreach (SapDeliveryCrossCmpStep2_CURRENCYAMOUNT curAmt in accAmtS2)
                    {
                        CURAMT2.Append();
                        CURAMT2.SetValue("ITEMNO_ACC", curAmt.ITEMNO_ACC);
                        CURAMT2.SetValue("CURRENCY", curAmt.CURRENCY);
                        CURAMT2.SetValue("AMT_DOCCUR", curAmt.AMT_DOCCUR);
                    }

                    RfcSessionManager.BeginContext(connection.Destination);
                    apAPI2.Invoke(connection.Destination);

                    IRfcTable tbl_RETURN2 = apAPI2.GetTable("RETURN");
                    DataTable dt_RETURN2 = GetDataTable(tbl_RETURN2);
                    dt_RETURN2.DefaultView.RowFilter = "TYPE='E'";
                    DataTable dt_ret2 = dt_RETURN2.DefaultView.ToTable();
                    if (dt_ret2.Rows.Count != 0)
                    {
                        rst = false;
                        msgErr += "\\n Error-Step2:";
                        foreach (DataRow drMsg in dt_ret2.Rows)
                        {
                            msgErr += "\\n [" + drMsg["NUMBER"] + "] " + drMsg["MESSAGE"];
                        }
                    }

                    /* - Doc3 - */
                    IRfcFunction apAPI3 = connection.CreateFunction("BAPI_ACC_DOCUMENT_CHECK");

                    IRfcStructure DOCHDR3 = apAPI3.GetStructure("DOCUMENTHEADER");
                    DOCHDR3.SetValue("USERNAME", hs3.USERNAME);
                    DOCHDR3.SetValue("COMP_CODE", hs3.COMP_CODE);
                    DOCHDR3.SetValue("DOC_DATE", SAPNETExtension.ToSAPDateString(hs3._DOC_DATE));
                    DOCHDR3.SetValue("PSTNG_DATE", SAPNETExtension.ToSAPDateString(hs3._PSTNG_DATE));
                    DOCHDR3.SetValue("FISC_YEAR", hs3.FISC_YEAR);
                    DOCHDR3.SetValue("DOC_TYPE", hs3.DOC_TYPE);
                    DOCHDR3.SetValue("REF_DOC_NO", hs3.REF_DOC_NO);

                    IRfcTable ACCRC3 = apAPI3.GetTable("ACCOUNTRECEIVABLE");
                    foreach (SapDeliveryCrossCmpStep3_ACCOUNTRECEIVABLE accGL in accRcS3)
                    {
                        ACCRC3.Append();
                        ACCRC3.SetValue("ITEMNO_ACC", accGL.ITEMNO_ACC);
                        ACCRC3.SetValue("CUSTOMER", accGL.CUSTOMER);
                        ACCRC3.SetValue("PROFIT_CTR", accGL.PROFIT_CTR);
                        ACCRC3.SetValue("BUSINESSPLACE", accGL.BUSINESSPLACE);
                        ACCRC3.SetValue("ITEM_TEXT", accGL.ITEM_TEXT);
                    }

                    IRfcTable ACCGL3 = apAPI3.GetTable("ACCOUNTGL");
                    foreach (SapDeliveryCrossCmpStep3_ACCOUNTGL accGL in accGLS3)
                    {
                        ACCGL3.Append();
                        ACCGL3.SetValue("ITEMNO_ACC", accGL.ITEMNO_ACC);
                        ACCGL3.SetValue("GL_ACCOUNT", accGL.GL_ACCOUNT);
                        ACCGL3.SetValue("ITEM_TEXT", accGL.ITEM_TEXT);
                        ACCGL3.SetValue("PROFIT_CTR", accGL.PROFIT_CTR);

                        ACCGL3.SetValue("REF_KEY_3", accGL.REF_KEY_3);
                        ACCGL3.SetValue("ALLOC_NMBR", accGL.ALLOC_NMBR);
                    }

                    IRfcTable CURAMT3 = apAPI3.GetTable("CURRENCYAMOUNT");
                    foreach (SapDeliveryCrossCmpStep3_CURRENCYAMOUNT curAmt in accAmtS3)
                    {
                        CURAMT3.Append();
                        CURAMT3.SetValue("ITEMNO_ACC", curAmt.ITEMNO_ACC);
                        CURAMT3.SetValue("CURRENCY", curAmt.CURRENCY);
                        CURAMT3.SetValue("AMT_DOCCUR", curAmt.AMT_DOCCUR);
                    }

                    RfcSessionManager.BeginContext(connection.Destination);
                    apAPI3.Invoke(connection.Destination);

                    IRfcTable tbl_RETURN3 = apAPI3.GetTable("RETURN");
                    DataTable dt_RETURN3 = GetDataTable(tbl_RETURN3);
                    dt_RETURN3.DefaultView.RowFilter = "TYPE='E'";
                    DataTable dt_ret3 = dt_RETURN3.DefaultView.ToTable();
                    if (dt_ret3.Rows.Count != 0)
                    {
                        rst = false;
                        msgErr += "\\n Error-Step3:";
                        foreach (DataRow drMsg in dt_ret3.Rows)
                        {
                            msgErr += "\\n [" + drMsg["NUMBER"] + "] " + drMsg["MESSAGE"];
                        }
                    }

                    RfcSessionManager.EndContext(connection.Destination);

                    if (!rst)
                    {
                        msgErr = "deliveryAccountCrossCmpCheck_Error" + msgErr;
                        transaction.Rollback();
                        rst = false;
                    }
                    else
                    {
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    rst = false;
                    while (ex.InnerException != null)
                        ex = ex.InnerException;

                    transaction.Rollback();
                    msgErr = "deliveryLowPricePostAccountCrossCmp_Error " + ex.Message;
                }
                finally
                {
                }
            }
            return rst;
        }

        /* = - Delivery Post Account WBS Cross Company - = */
        public bool deliveryPostAccountCrossCmp(string DelvLst
                                                    , SapDeliveryCrossCmpStep1_DOCUMENTHEADER hs1
                                                    , List<SapDeliveryCrossCmpStep1_ACCOUNTGL> AccGLS1
                                                    , List<SapDeliveryCrossCmpStep1_ACCOUNTPAYABLE> accPaS1
                                                    , List<SapDeliveryCrossCmpStep1_CURRENCYAMOUNT> accAmtS1

                                                    , SapDeliveryCrossCmpStep2_DOCUMENTHEADER hs2
                                                    , List<SapDeliveryCrossCmpStep2_ACCOUNTGL> accGLS2
                                                    , List<SapDeliveryCrossCmpStep2_CURRENCYAMOUNT> accAmtS2

                                                    , SapDeliveryCrossCmpStep3_DOCUMENTHEADER hs3
                                                    , List<SapDeliveryCrossCmpStep3_ACCOUNTGL> accGLS3
                                                    , List<SapDeliveryCrossCmpStep3_ACCOUNTRECEIVABLE> accRcS3
                                                    , List<SapDeliveryCrossCmpStep3_CURRENCYAMOUNT> accAmtS3
                                                    , ref string SAPDocNo1, ref string SAPDocNo2, ref string SAPDocNo3, string EmployeeID, ref string msgErr)
        {
            bool rst = true;
            using (var connection = new SapConnection(connectionName))
            {

                connection.Open();

                var transaction = connection.BeginTransaction();
                try
                {
                    /* - Doc1 - */
                    IRfcFunction apAPI = connection.CreateFunction("BAPI_ACC_DOCUMENT_POST");
                    IRfcFunction apCMT = connection.CreateFunction("BAPI_TRANSACTION_COMMIT");
                    /* - Doc2 - */
                    IRfcFunction apAPI2 = connection.CreateFunction("BAPI_ACC_DOCUMENT_POST");
                    IRfcFunction apCMT2 = connection.CreateFunction("BAPI_TRANSACTION_COMMIT");
                    /* - Doc3 - */
                    IRfcFunction apAPI3 = connection.CreateFunction("BAPI_ACC_DOCUMENT_POST");
                    IRfcFunction apCMT3 = connection.CreateFunction("BAPI_TRANSACTION_COMMIT");

                    /* - #Doc1 - */
                    IRfcStructure DOCHDR = apAPI.GetStructure("DOCUMENTHEADER");
                    DOCHDR.SetValue("USERNAME", hs1.USERNAME);
                    DOCHDR.SetValue("COMP_CODE", hs1.COMP_CODE);
                    DOCHDR.SetValue("DOC_DATE", SAPNETExtension.ToSAPDateString(hs1._DOC_DATE));
                    DOCHDR.SetValue("PSTNG_DATE", SAPNETExtension.ToSAPDateString(hs1._PSTNG_DATE));
                    DOCHDR.SetValue("FISC_YEAR", hs1.FISC_YEAR);
                    DOCHDR.SetValue("DOC_TYPE", hs1.DOC_TYPE);
                    DOCHDR.SetValue("REF_DOC_NO", hs1.REF_DOC_NO);

                    IRfcTable ACCGL = apAPI.GetTable("ACCOUNTGL");
                    foreach (SapDeliveryCrossCmpStep1_ACCOUNTGL accGL in AccGLS1)
                    {
                        ACCGL.Append();
                        ACCGL.SetValue("ITEMNO_ACC", accGL.ITEMNO_ACC);
                        ACCGL.SetValue("GL_ACCOUNT", accGL.GL_ACCOUNT);
                        ACCGL.SetValue("ITEM_TEXT", accGL.ITEM_TEXT);
                        ACCGL.SetValue("PROFIT_CTR", accGL.PROFIT_CTR);

                        ACCGL.SetValue("REF_KEY_3", accGL.REF_KEY_3);
                        ACCGL.SetValue("ALLOC_NMBR", accGL.ALLOC_NMBR);
                    }

                    IRfcTable ACCPA = apAPI.GetTable("ACCOUNTPAYABLE");
                    foreach (SapDeliveryCrossCmpStep1_ACCOUNTPAYABLE accGL in accPaS1)
                    {
                        ACCPA.Append();
                        ACCPA.SetValue("ITEMNO_ACC", accGL.ITEMNO_ACC);
                        ACCPA.SetValue("VENDOR_NO", accGL.VENDOR_NO);
                        ACCPA.SetValue("BUSINESSPLACE", accGL.BUSINESSPLACE);
                        ACCPA.SetValue("PROFIT_CTR", accGL.PROFIT_CTR);
                        ACCPA.SetValue("ITEM_TEXT", accGL.ITEM_TEXT);
                    }

                    IRfcTable CURAMT = apAPI.GetTable("CURRENCYAMOUNT");
                    foreach (SapDeliveryCrossCmpStep1_CURRENCYAMOUNT curAmt in accAmtS1)
                    {
                        CURAMT.Append();
                        CURAMT.SetValue("ITEMNO_ACC", curAmt.ITEMNO_ACC);
                        CURAMT.SetValue("CURRENCY", curAmt.CURRENCY);
                        CURAMT.SetValue("AMT_DOCCUR", curAmt.AMT_DOCCUR);
                    }

                    RfcSessionManager.BeginContext(connection.Destination);
                    apAPI.Invoke(connection.Destination);

                    IRfcTable tbl_RETURN = apAPI.GetTable("RETURN");

                    string OBJ_TYPE = "" + apAPI.GetValue("OBJ_TYPE");//   Reference Transaction
                    string OBJ_KEY = "" + apAPI.GetValue("OBJ_KEY");//   Reference Key
                    string OBJ_SYS = "" + apAPI.GetValue("OBJ_SYS");//   Logical system of source document

                    DataTable dt_RETURN = GetDataTable(tbl_RETURN);
                    dt_RETURN.DefaultView.RowFilter = "TYPE='E'";
                    DataTable dt_ret = dt_RETURN.DefaultView.ToTable();
                    if (dt_ret.Rows.Count == 0)
                    {
                        SAPDocNo1 = OBJ_KEY;//.Substring(0, 10);

                        /* - #Doc2 - */
                        //IRfcFunction apAPI2 = connection.CreateFunction("BAPI_ACC_DOCUMENT_POST");
                        //IRfcFunction apCMT2 = connection.CreateFunction("BAPI_TRANSACTION_COMMIT");

                        IRfcStructure DOCHDR2 = apAPI2.GetStructure("DOCUMENTHEADER");
                        DOCHDR2.SetValue("USERNAME", hs2.USERNAME);
                        DOCHDR2.SetValue("COMP_CODE", hs2.COMP_CODE);
                        DOCHDR2.SetValue("DOC_DATE", SAPNETExtension.ToSAPDateString(hs2._DOC_DATE));
                        DOCHDR2.SetValue("PSTNG_DATE", SAPNETExtension.ToSAPDateString(hs2._PSTNG_DATE));
                        DOCHDR2.SetValue("FISC_YEAR", hs2.FISC_YEAR);
                        DOCHDR2.SetValue("DOC_TYPE", hs2.DOC_TYPE);
                        DOCHDR2.SetValue("REF_DOC_NO", hs2.REF_DOC_NO);

                        IRfcTable ACCGL2 = apAPI2.GetTable("ACCOUNTGL");
                        foreach (SapDeliveryCrossCmpStep2_ACCOUNTGL accGL in accGLS2)
                        {
                            ACCGL2.Append();
                            ACCGL2.SetValue("ITEMNO_ACC", accGL.ITEMNO_ACC);
                            ACCGL2.SetValue("GL_ACCOUNT", accGL.GL_ACCOUNT);
                            ACCGL2.SetValue("ITEM_TEXT", accGL.ITEM_TEXT + "");
                            ACCGL2.SetValue("REF_KEY_2", accGL.REF_KEY_2 + "");
                            ACCGL2.SetValue("COSTCENTER", accGL.COSTCENTER + "");
                            ACCGL2.SetValue("WBS_ELEMENT", accGL.WBS_ELEMENT + "");
                            ACCGL2.SetValue("PROFIT_CTR", accGL.PROFIT_CTR + "");

                            ACCGL2.SetValue("REF_KEY_3", accGL.REF_KEY_3);
                            ACCGL2.SetValue("ALLOC_NMBR", accGL.ALLOC_NMBR);
                        }

                        IRfcTable CURAMT2 = apAPI2.GetTable("CURRENCYAMOUNT");
                        foreach (SapDeliveryCrossCmpStep2_CURRENCYAMOUNT curAmt in accAmtS2)
                        {
                            CURAMT2.Append();
                            CURAMT2.SetValue("ITEMNO_ACC", curAmt.ITEMNO_ACC);
                            CURAMT2.SetValue("CURRENCY", curAmt.CURRENCY);
                            CURAMT2.SetValue("AMT_DOCCUR", curAmt.AMT_DOCCUR);
                        }

                        RfcSessionManager.BeginContext(connection.Destination);
                        apAPI2.Invoke(connection.Destination);

                        IRfcTable tbl_RETURN2 = apAPI2.GetTable("RETURN");

                        string OBJ_TYPE2 = "" + apAPI2.GetValue("OBJ_TYPE");//   Reference Transaction
                        string OBJ_KEY2 = "" + apAPI2.GetValue("OBJ_KEY");//   Reference Key
                        string OBJ_SYS2 = "" + apAPI2.GetValue("OBJ_SYS");//   Logical system of source document

                        DataTable dt_RETURN2 = GetDataTable(tbl_RETURN2);
                        dt_RETURN2.DefaultView.RowFilter = "TYPE='E'";
                        DataTable dt_ret2 = dt_RETURN2.DefaultView.ToTable();
                        if (dt_ret2.Rows.Count == 0)
                        {
                            SAPDocNo2 = OBJ_KEY2;//.Substring(0, 10);

                            /* - #Doc3 - */
                            //IRfcFunction apAPI3 = connection.CreateFunction("BAPI_ACC_DOCUMENT_POST");
                            //IRfcFunction apCMT3 = connection.CreateFunction("BAPI_TRANSACTION_COMMIT");

                            IRfcStructure DOCHDR3 = apAPI3.GetStructure("DOCUMENTHEADER");
                            DOCHDR3.SetValue("USERNAME", hs3.USERNAME);
                            DOCHDR3.SetValue("COMP_CODE", hs3.COMP_CODE);
                            DOCHDR3.SetValue("DOC_DATE", SAPNETExtension.ToSAPDateString(hs3._DOC_DATE));
                            DOCHDR3.SetValue("PSTNG_DATE", SAPNETExtension.ToSAPDateString(hs3._PSTNG_DATE));
                            DOCHDR3.SetValue("FISC_YEAR", hs3.FISC_YEAR);
                            DOCHDR3.SetValue("DOC_TYPE", hs3.DOC_TYPE);
                            DOCHDR3.SetValue("REF_DOC_NO", hs3.REF_DOC_NO);

                            IRfcTable ACCRC3 = apAPI3.GetTable("ACCOUNTRECEIVABLE");
                            foreach (SapDeliveryCrossCmpStep3_ACCOUNTRECEIVABLE accGL in accRcS3)
                            {
                                ACCRC3.Append();
                                ACCRC3.SetValue("ITEMNO_ACC", accGL.ITEMNO_ACC);
                                ACCRC3.SetValue("CUSTOMER", accGL.CUSTOMER);
                                ACCRC3.SetValue("PROFIT_CTR", accGL.PROFIT_CTR);
                                ACCRC3.SetValue("BUSINESSPLACE", accGL.BUSINESSPLACE);
                                ACCRC3.SetValue("ITEM_TEXT", accGL.ITEM_TEXT);
                            }

                            IRfcTable ACCGL3 = apAPI3.GetTable("ACCOUNTGL");
                            foreach (SapDeliveryCrossCmpStep3_ACCOUNTGL accGL in accGLS3)
                            {
                                ACCGL3.Append();
                                ACCGL3.SetValue("ITEMNO_ACC", accGL.ITEMNO_ACC);
                                ACCGL3.SetValue("GL_ACCOUNT", accGL.GL_ACCOUNT);
                                ACCGL3.SetValue("ITEM_TEXT", accGL.ITEM_TEXT);
                                ACCGL3.SetValue("PROFIT_CTR", accGL.PROFIT_CTR);

                                ACCGL3.SetValue("REF_KEY_3", accGL.REF_KEY_3);
                                ACCGL3.SetValue("ALLOC_NMBR", accGL.ALLOC_NMBR);
                            }

                            IRfcTable CURAMT3 = apAPI3.GetTable("CURRENCYAMOUNT");
                            foreach (SapDeliveryCrossCmpStep3_CURRENCYAMOUNT curAmt in accAmtS3)
                            {
                                CURAMT3.Append();
                                CURAMT3.SetValue("ITEMNO_ACC", curAmt.ITEMNO_ACC);
                                CURAMT3.SetValue("CURRENCY", curAmt.CURRENCY);
                                CURAMT3.SetValue("AMT_DOCCUR", curAmt.AMT_DOCCUR);
                            }

                            RfcSessionManager.BeginContext(connection.Destination);
                            apAPI3.Invoke(connection.Destination);

                            IRfcTable tbl_RETURN3 = apAPI3.GetTable("RETURN");

                            string OBJ_TYPE3 = "" + apAPI3.GetValue("OBJ_TYPE");//   Reference Transaction
                            string OBJ_KEY3 = "" + apAPI3.GetValue("OBJ_KEY");//   Reference Key
                            string OBJ_SYS3 = "" + apAPI3.GetValue("OBJ_SYS");//   Logical system of source document

                            DataTable dt_RETURN3 = GetDataTable(tbl_RETURN3);
                            dt_RETURN3.DefaultView.RowFilter = "TYPE='E'";
                            DataTable dt_ret3 = dt_RETURN3.DefaultView.ToTable();
                            if (dt_ret3.Rows.Count == 0)
                            {
                                SAPDocNo3 = OBJ_KEY3;//.Substring(0, 10);
                                DADelivery cls = new DADelivery();
                                if (!cls.UpdateStatusPostAccountCrossCmp(DelvLst, OBJ_TYPE, OBJ_KEY, OBJ_SYS, OBJ_TYPE2, OBJ_KEY2, OBJ_SYS2, OBJ_TYPE3, OBJ_KEY3, OBJ_SYS3, hs1.USERNAME, hs1._PSTNG_DATE, hs1.REF_DOC_NO, EmployeeID, ref msgErr))
                                {
                                    rst = false;
                                }
                            }
                            else
                            {
                                rst = false;
                                msgErr += "\\n Error-Step3:";
                                foreach (DataRow drMsg in dt_ret3.Rows)
                                {
                                    msgErr += "\\n [" + drMsg["NUMBER"] + "] " + drMsg["MESSAGE"];
                                }
                            }

                        }
                        else
                        {
                            rst = false;
                            msgErr += "\\n Error-Step2:";
                            foreach (DataRow drMsg in dt_ret2.Rows)
                            {
                                msgErr += "\\n [" + drMsg["NUMBER"] + "] " + drMsg["MESSAGE"];
                            }
                        }

                    }
                    else
                    {
                        rst = false;
                        msgErr += "\\n Error-Step1:";
                        foreach (DataRow drMsg in dt_ret.Rows)
                        {
                            msgErr += "\\n [" + drMsg["NUMBER"] + "] " + drMsg["MESSAGE"];
                        }
                    }

                    if (rst)
                    {
                        apCMT.SetValue("WAIT", "X");
                        apCMT.Invoke(connection.Destination);
                        apCMT2.SetValue("WAIT", "X");
                        apCMT2.Invoke(connection.Destination);
                        apCMT3.SetValue("WAIT", "X");
                        apCMT3.Invoke(connection.Destination);
                    }
                    RfcSessionManager.EndContext(connection.Destination);

                    if (!rst)
                    {
                        msgErr = "deliveryPostAccountCrossCmp_Error" + msgErr;

                        transaction.Rollback();
                        rst = false;
                    }
                    else
                    {
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    rst = false;
                    while (ex.InnerException != null)
                        ex = ex.InnerException;

                    transaction.Rollback();
                    msgErr = "deliveryPostAccountCrossCmp_Error " + ex.Message;
                }
                finally
                {
                }
            }
            return rst;
        }



        /* = - Get Data IO Budget - = */
        /*
        I_PROJ	Project
        I_BUKRS	Company Code
        I_AUFNR	IO Number
         */
        public bool SAPGetDataReportIOBudget(string I_PROJ, string I_BUKRS, string I_AUFNR, ref DataTable dtIOBudget, ref string msgErr)
        {// PRITEM[Material,...]
            bool rst = true;
            using (var connection = new SapConnection(connectionName))
            {

                connection.Open();

                var transaction = connection.BeginTransaction();
                try
                {
                    IRfcFunction apAPI = connection.CreateFunction("ZRFC_IO_BUDGET");
                    // IRfcFunction apCMT = connection.CreateFunction("BAPI_TRANSACTION_COMMIT");

                    apAPI.SetValue("I_PROJ", I_PROJ);     //  Project
                    apAPI.SetValue("I_BUKRS", I_BUKRS);    //  Company Code
                    apAPI.SetValue("I_AUFNR", I_AUFNR);    //  IO Number

                    RfcSessionManager.BeginContext(connection.Destination);
                    apAPI.Invoke(connection.Destination);

                    IRfcTable tbl_BUDGET = apAPI.GetTable("T_BUDGET");
                    dtIOBudget = GetDataTable(tbl_BUDGET);

                    RfcSessionManager.EndContext(connection.Destination);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    rst = false;
                    while (ex.InnerException != null)
                        ex = ex.InnerException;

                    transaction.Rollback();
                    msgErr = "SAPGetDataReportIOBudget " + ex.Message;
                }
                finally
                {
                }
            }
            return rst;
        }

        private string GetZeroPlusNumber(int iNumber)
        {
            string retNumber = "";

            int isInteger;
            if (iNumber.ToString().Length > 0)
            {
                if (int.TryParse(iNumber.ToString(), out isInteger))
                {
                    retNumber = "0000000000" + iNumber.ToString();
                    retNumber = retNumber.Substring(retNumber.Length - 10, 10);
                }
                else
                {
                    retNumber = iNumber.ToString();
                }
            }

            return retNumber;
        }

        private string GetZeroPlusNumber(string iNumber)
        {
            string retNumber = "";

            int isInteger;
            if (iNumber.Length > 0)
            {
                if (int.TryParse(iNumber, out isInteger))
                {
                    retNumber = "0000000000" + iNumber;
                    retNumber = retNumber.Substring(retNumber.Length - 10, 10);
                }
                else
                {
                    retNumber = iNumber;
                }
            }

            return retNumber;
        }

        private string GetZeroPlusNumberCostCenter(string iNumber)
        {
            string retNumber = "";
            int tmpInt = 0;

            if (iNumber.Length > 1)
            {
                // ถ้ามีตัวตัวอักษรผสมอยู่ให้ส่งเป็น 5 หลักเข้า SAP ไปเลย
                if (!int.TryParse(iNumber.Substring(0, 1), out tmpInt))
                {
                    retNumber = iNumber;
                }
                else // ถ้าเป็นตัวเลขหมด ให้ส่งเป็น 10 หลัก
                {
                    retNumber = "000000000" + iNumber;
                    retNumber = retNumber.Substring(retNumber.Length - 10, 10);
                }
            }

            return retNumber;
        }

        public bool CHECK_ACCPAYABLE(SAPCROSSCOM_DOCUMENTHEADER HEADER,
                                      List<SAPREQUISITION_PAYABLE> PAYABLE_DETAILS,
                                      List<SAPREQUISITION_ACCOUNTGL> CUTSTOCK_DETAILS,
                                      List<SAPREQUISITION_RECIEVEABLE> RECEIVEABLE_DETAILS,
                                      List<SAPREQUISITION_CURRENCYAMOUNT> CURRENCY,
                                      bool IsMarketing,
                                      ref string sap_msg)
        {
            sap_msg = "";

            bool rst = true;

            using (var connection = new SapConnection(connectionName))
            {

                connection.Open();

                var transaction = connection.BeginTransaction();
                try
                {
                    DataTable dt_RETURN = null;
                    DataTable dt_ret = null;

                    IRfcStructure DOCHDR = null;

                    IRfcTable ACCRC = null;
                    IRfcTable ACCGL = null;
                    IRfcTable ACCPA = null;
                    IRfcTable CURAMT = null;
                    IRfcTable tbl_RETURN = null;

                    IRfcFunction payableAPI = null;
                    IRfcFunction cutstockAPI = null;
                    IRfcFunction recieveableAPIAPI = null;

                    #region "---------  ตั้งเจ้าหนี้  ---------"

                    payableAPI = connection.CreateFunction("BAPI_ACC_DOCUMENT_CHECK");

                    DOCHDR = payableAPI.GetStructure("DOCUMENTHEADER");
                    DOCHDR.SetValue("USERNAME", HEADER.USERNAME);
                    DOCHDR.SetValue("COMP_CODE", HEADER.COMP_CODE);
                    DOCHDR.SetValue("DOC_DATE", Convert.ToDateTime(HEADER.DOC_DATE));
                    DOCHDR.SetValue("PSTNG_DATE", Convert.ToDateTime(HEADER.PSTNG_DATE));
                    DOCHDR.SetValue("FISC_YEAR", HEADER.FISC_YEAR);
                    DOCHDR.SetValue("DOC_TYPE", "KR");
                    DOCHDR.SetValue("REF_DOC_NO", HEADER.REF_DOC_NO);

                    List<ACCRECLIST> accRec = new List<ACCRECLIST>();
                    int iCount = 0;
                    ACCGL = payableAPI.GetTable("ACCOUNTGL");
                    foreach (SAPREQUISITION_PAYABLE item in PAYABLE_DETAILS.Where(q => q.GL_ACCOUNT.Equals("1580010")))
                    {
                        ACCGL.Append();
                        ACCGL.SetValue("ITEMNO_ACC", GetZeroPlusNumber(iCount += 1));
                        ACCGL.SetValue("GL_ACCOUNT", GetZeroPlusNumber(item.GL_ACCOUNT.ToString()));
                        ACCGL.SetValue("ITEM_TEXT", item.ITEM_TEXT);
                        ACCGL.SetValue("REF_KEY_3", item.REF_KEY_3);
                        ACCGL.SetValue("PROFIT_CTR", item.PROFIT_CTR);
                        ACCGL.SetValue("ALLOC_NMBR", (item.ALLOC_NMBR == "" ? "000000000000000" : item.ALLOC_NMBR));
                    }

                    ACCPA = payableAPI.GetTable("ACCOUNTPAYABLE");

                    var c = PAYABLE_DETAILS.Where(q => q.GL_ACCOUNT.Equals("1580010")).Select(q => new
                    {
                        b = q.BUSINESSPLACE,
                        c = q.GL_ACCOUNT,
                        d = q.PROFIT_CTR,
                        e = q.REF_KEY_3,
                        f = q.VENDOR_NO
                    }).Distinct().ToList();

                    for (int i = 0; i < c.Count; i++)
                    {
                        ACCPA.Append();
                        ACCPA.SetValue("ITEMNO_ACC", GetZeroPlusNumber(iCount += 1));
                        ACCPA.SetValue("VENDOR_NO", GetZeroPlusNumber(c[i].f));
                        ACCPA.SetValue("ITEM_TEXT", "");
                        ACCPA.SetValue("BUSINESSPLACE", c[i].b);
                        ACCGL.SetValue("REF_KEY_3", c[i].e);
                        ACCPA.SetValue("PROFIT_CTR", c[i].d);
                    }

                    iCount = 0;
                    CURAMT = payableAPI.GetTable("CURRENCYAMOUNT");
                    decimal negNet = 0;
                    string currency = "";

                    foreach (SAPREQUISITION_CURRENCYAMOUNT item in CURRENCY)
                    {
                        CURAMT.Append();
                        UInt32 isNegative;
                        UInt32.TryParse(Convert.ToInt32(item.AMT_DOCCUR).ToString(), out isNegative);

                        if (isNegative == 0)
                        {
                            currency = item.CURRENCY;
                            negNet += Convert.ToDecimal(item.AMT_DOCCUR);
                        }
                        else
                        {
                            CURAMT.SetValue("ITEMNO_ACC", GetZeroPlusNumber(iCount += 1));
                            CURAMT.SetValue("CURRENCY", item.CURRENCY);
                            CURAMT.SetValue("AMT_DOCCUR", item.AMT_DOCCUR);
                        }
                    }
                    CURAMT.Append();
                    CURAMT.SetValue("ITEMNO_ACC", GetZeroPlusNumber(iCount += 1));
                    CURAMT.SetValue("CURRENCY", currency);
                    CURAMT.SetValue("AMT_DOCCUR", negNet);

                    RfcSessionManager.BeginContext(connection.Destination);

                    tbl_RETURN = payableAPI.GetTable("RETURN");
                    dt_RETURN = GetDataTable(tbl_RETURN);
                    dt_RETURN.DefaultView.RowFilter = "TYPE='E'";
                    dt_ret = dt_RETURN.DefaultView.ToTable();
                    if (dt_ret.Rows.Count != 0)
                    {
                        rst = false;
                        sap_msg += "\n Error-Step1:";
                        foreach (DataRow drMsg in dt_ret.Rows)
                        {
                            sap_msg += "\n [" + drMsg["NUMBER"] + "] " + drMsg["MESSAGE"];
                        }
                    }

                    #endregion

                    #region "---------  ตัดสต็อค  ---------"

                    cutstockAPI = connection.CreateFunction("BAPI_ACC_DOCUMENT_CHECK");

                    DOCHDR = cutstockAPI.GetStructure("DOCUMENTHEADER");
                    DOCHDR.SetValue("USERNAME", HEADER.USERNAME);
                    DOCHDR.SetValue("COMP_CODE", HEADER.COMP_CODE);
                    DOCHDR.SetValue("DOC_DATE", Convert.ToDateTime(HEADER.DOC_DATE));
                    DOCHDR.SetValue("PSTNG_DATE", Convert.ToDateTime(HEADER.PSTNG_DATE));
                    DOCHDR.SetValue("FISC_YEAR", HEADER.FISC_YEAR);
                    DOCHDR.SetValue("DOC_TYPE", "IA");
                    DOCHDR.SetValue("REF_DOC_NO", HEADER.REF_DOC_NO);

                    iCount = 0;
                    ACCGL = cutstockAPI.GetTable("ACCOUNTGL");
                    foreach (SAPREQUISITION_ACCOUNTGL item in CUTSTOCK_DETAILS)
                    {
                        ACCGL.Append();
                        ACCGL.SetValue("ITEMNO_ACC", GetZeroPlusNumber(iCount += 1));
                        ACCGL.SetValue("GL_ACCOUNT", GetZeroPlusNumber(item.GL_ACCOUNT));
                        ACCGL.SetValue("ITEM_TEXT", item.ITEM_TEXT);
                        ACCGL.SetValue("REF_KEY_2", item.REF_KEY_2);
                        ACCGL.SetValue("REF_KEY_3", item.REF_KEY_3);

                        //คิม ถ้าเป็น Marketing ให้ใช้ ORDERID ด้วย
                        if (IsMarketing && !string.IsNullOrEmpty(item.COSTCENTER))
                        {
                            string msg = "";
                            string InternalOrder = daStk.GetInternalOrderByCostCenter(item.COSTCENTER, ref msg);

                            if ((InternalOrder ?? "") == "")
                            {

                                sap_msg = string.Format("ไม่พบ InternalOrder ในระบบ (Cost Center = {0})", item.COSTCENTER);
                                return false;
                            }

                            ACCGL.SetValue("ORDERID", InternalOrder);
                        }

                        ACCGL.SetValue("COSTCENTER", GetZeroPlusNumber(item.COSTCENTER));

                        ACCGL.SetValue("PROFIT_CTR", item.PROFIT_CTR);
                        ACCGL.SetValue("ALLOC_NMBR", item.ALLOC_NMBR);
                    }

                    iCount = 0;
                    CURAMT = cutstockAPI.GetTable("CURRENCYAMOUNT");
                    foreach (SAPREQUISITION_CURRENCYAMOUNT item in CURRENCY)
                    {
                        CURAMT.Append();
                        CURAMT.SetValue("ITEMNO_ACC", GetZeroPlusNumber(iCount += 1));
                        CURAMT.SetValue("CURRENCY", item.CURRENCY);
                        CURAMT.SetValue("AMT_DOCCUR", item.AMT_DOCCUR);
                    }

                    RfcSessionManager.BeginContext(connection.Destination);

                    tbl_RETURN = cutstockAPI.GetTable("RETURN");
                    dt_RETURN = GetDataTable(tbl_RETURN);
                    dt_RETURN.DefaultView.RowFilter = "TYPE='E'";
                    dt_ret = dt_RETURN.DefaultView.ToTable();

                    if (dt_ret.Rows.Count != 0)
                    {
                        rst = false;
                        sap_msg += "\\n Error-Step2:";
                        foreach (DataRow drMsg in dt_ret.Rows)
                        {
                            sap_msg += "\\n [" + drMsg["NUMBER"] + "] " + drMsg["MESSAGE"];
                        }
                    }

                    #endregion

                    #region "---------  ตั้งลูกหนี้  ---------"

                    recieveableAPIAPI = connection.CreateFunction("BAPI_ACC_DOCUMENT_CHECK");

                    DOCHDR = recieveableAPIAPI.GetStructure("DOCUMENTHEADER");
                    DOCHDR.SetValue("USERNAME", HEADER.USERNAME);
                    DOCHDR.SetValue("COMP_CODE", "1000");
                    DOCHDR.SetValue("DOC_DATE", Convert.ToDateTime(HEADER.DOC_DATE));
                    DOCHDR.SetValue("PSTNG_DATE", Convert.ToDateTime(HEADER.PSTNG_DATE));
                    DOCHDR.SetValue("FISC_YEAR", HEADER.FISC_YEAR);
                    DOCHDR.SetValue("DOC_TYPE", "DR");
                    DOCHDR.SetValue("REF_DOC_NO", HEADER.REF_DOC_NO);

                    iCount = 0;
                    ACCRC = recieveableAPIAPI.GetTable("ACCOUNTRECEIVABLE");
                    var cc = RECEIVEABLE_DETAILS.Where(q => q.GL_ACCOUNT.Equals("1580010")).Select(q => new
                    {
                        b = q.BUSINESSPLACE,
                        c = q.GL_ACCOUNT,
                        d = q.PROFIT_CTR,
                        e = q.REF_KEY_3,
                        f = q.CUSTOMER
                    }).Distinct().ToList();

                    for (int i = 0; i < cc.Count; i++)
                    {
                        ACCRC.Append();
                        ACCRC.SetValue("ITEMNO_ACC", GetZeroPlusNumber(iCount += 1));
                        ACCRC.SetValue("CUSTOMER", GetZeroPlusNumber(cc[i].f));
                        ACCRC.SetValue("ITEM_TEXT", "");
                        ACCRC.SetValue("BUSINESSPLACE", cc[i].b);
                        ACCRC.SetValue("REF_KEY_3", cc[i].e);
                        ACCRC.SetValue("PROFIT_CTR", "P11000");
                    }

                    ACCGL = recieveableAPIAPI.GetTable("ACCOUNTGL");
                    foreach (SAPREQUISITION_RECIEVEABLE ITEM in RECEIVEABLE_DETAILS.Where(q => q.GL_ACCOUNT.Equals("1580010")))
                    {
                        ACCGL.Append();
                        ACCGL.SetValue("ITEMNO_ACC", GetZeroPlusNumber(iCount += 1));
                        ACCGL.SetValue("GL_ACCOUNT", GetZeroPlusNumber(ITEM.GL_ACCOUNT));
                        ACCGL.SetValue("ITEM_TEXT", ITEM.ITEM_TEXT);
                        ACCGL.SetValue("PROFIT_CTR", "P11000");
                        ACCGL.SetValue("REF_KEY_3", ITEM.REF_KEY_3);
                        ACCGL.SetValue("ALLOC_NMBR", (ITEM.ALLOC_NMBR == "" ? "000000000000000" : ITEM.ALLOC_NMBR));
                    }

                    iCount = 0;
                    CURAMT = recieveableAPIAPI.GetTable("CURRENCYAMOUNT");
                    decimal negNets = 0;
                    string currencys = "";

                    foreach (SAPREQUISITION_CURRENCYAMOUNT item in CURRENCY)
                    {
                        CURAMT.Append();
                        UInt32 isNegative;
                        UInt32.TryParse(Convert.ToInt32(item.AMT_DOCCUR).ToString(), out isNegative);
                        if (isNegative != 0)
                        {
                            currencys = item.CURRENCY;
                            negNets += Convert.ToDecimal(item.AMT_DOCCUR);
                        }
                    }
                    CURAMT.Append();
                    CURAMT.SetValue("ITEMNO_ACC", GetZeroPlusNumber(iCount += 1));
                    CURAMT.SetValue("CURRENCY", currencys);
                    CURAMT.SetValue("AMT_DOCCUR", negNets);

                    foreach (SAPREQUISITION_CURRENCYAMOUNT item in CURRENCY)
                    {
                        CURAMT.Append();
                        UInt32 isNegative;
                        UInt32.TryParse(Convert.ToInt32(item.AMT_DOCCUR).ToString(), out isNegative);
                        if (isNegative == 0)
                        {
                            CURAMT.SetValue("ITEMNO_ACC", GetZeroPlusNumber(iCount += 1));
                            CURAMT.SetValue("CURRENCY", item.CURRENCY);
                            CURAMT.SetValue("AMT_DOCCUR", item.AMT_DOCCUR);
                        }
                    }

                    RfcSessionManager.BeginContext(connection.Destination);

                    tbl_RETURN = recieveableAPIAPI.GetTable("RETURN");
                    dt_RETURN = GetDataTable(tbl_RETURN);
                    dt_RETURN.DefaultView.RowFilter = "TYPE='E'";
                    dt_ret = dt_RETURN.DefaultView.ToTable();
                    if (dt_ret.Rows.Count != 0)
                    {
                        rst = false;
                        sap_msg += "\\n Error-Step3:";
                        foreach (DataRow drMsg in dt_ret.Rows)
                        {
                            sap_msg += "\\n [" + drMsg["NUMBER"] + "] " + drMsg["MESSAGE"];
                        }
                    }

                    #endregion

                    RfcSessionManager.EndContext(connection.Destination);

                    if (!rst)
                    {
                        sap_msg = "REQUISITION POST ACCOUNT CROSS COMPANY ERROR" + sap_msg;

                        transaction.Rollback();
                        rst = false;
                    }
                    else
                    {
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    rst = false;
                    while (ex.InnerException != null)
                        ex = ex.InnerException;

                    transaction.Rollback();
                    sap_msg = "REQUISITION POST ACCOUNT CROSS CROMPANY ERROR " + ex.Message;
                }
                finally
                {
                }
            }
            return rst;

        }

        //Cost Center Post ComAP บัญชี ปรกติ
        public bool COSTCENTERRECORD_ACCOUNT(SapDelivery_DOCUMENTHEADER docH,
                                            List<SAPREQUISITION_ACCOUNTGL> lstAccGL,
                                            List<SapDelivery_CURRENCYAMOUNT> lstCurAmt,
                                            bool IsMarketing,
                                            out string SAPDocNo, out string msgErr)
        {
            bool rst = false;

            using (var connection = new SapConnection(connectionName))
            {
                connection.Open();

                var transaction = connection.BeginTransaction();
                try
                {

                    IRfcFunction accPost = connection.CreateFunction("BAPI_ACC_DOCUMENT_POST");
                    IRfcFunction accCommit = connection.CreateFunction("BAPI_TRANSACTION_COMMIT");

                    IRfcStructure DOCHDR = accPost.GetStructure("DOCUMENTHEADER");
                    DOCHDR.SetValue("USERNAME", docH.USERNAME);
                    DOCHDR.SetValue("COMP_CODE", docH.COMP_CODE);
                    DOCHDR.SetValue("DOC_DATE", SAPNETExtension.ToSAPDateString(docH._DOC_DATE));
                    DOCHDR.SetValue("PSTNG_DATE", SAPNETExtension.ToSAPDateString(docH._PSTNG_DATE));
                    DOCHDR.SetValue("FISC_YEAR", docH.FISC_YEAR);
                    DOCHDR.SetValue("DOC_TYPE", docH.DOC_TYPE);
                    DOCHDR.SetValue("REF_DOC_NO", docH.REF_DOC_NO);

                    IRfcTable ACCGL = accPost.GetTable("ACCOUNTGL");
                    foreach (SAPREQUISITION_ACCOUNTGL accGL in lstAccGL)
                    {
                        ACCGL.Append();
                        ACCGL.SetValue("ITEMNO_ACC", accGL.ITEMNO_ACC);
                        ACCGL.SetValue("GL_ACCOUNT", "000" + accGL.GL_ACCOUNT.ToString());
                        ACCGL.SetValue("ITEM_TEXT", accGL.ITEM_TEXT);
                        ACCGL.SetValue("REF_KEY_2", accGL.REF_KEY_2);
                        ACCGL.SetValue("REF_KEY_3", accGL.REF_KEY_3);

                        //คิม ถ้าเป็น Marketing ให้ใช้ ORDERID ด้วย
                        if (IsMarketing && accGL.COSTCENTER != null)
                        {
                            string msg = "";
                            string InternalOrder = daStk.GetInternalOrderByCostCenter(accGL.COSTCENTER, ref msg);

                            if ((InternalOrder ?? "") == "")
                            {
                                msgErr = string.Format("ไม่พบ InternalOrder ในระบบ (Cost Center = {0})", accGL.COSTCENTER);
                                SAPDocNo = "";
                                return false;
                            }

                            ACCGL.SetValue("ORDERID", InternalOrder);
                        }

                        string sapCostCenter = "";

                        if (accGL.COSTCENTER != null)
                        {
                            int tmpInt = 0;
                            if (int.TryParse(accGL.COSTCENTER.ToString(), out tmpInt))
                            {
                                sapCostCenter = "0000000000" + accGL.COSTCENTER.ToString();
                                sapCostCenter = sapCostCenter.Substring(sapCostCenter.Length - 10, 10);
                            }
                            else
                            {
                                sapCostCenter = accGL.COSTCENTER.ToString();
                            }
                        }
                        else
                        {
                            sapCostCenter = null;
                        }

                        ACCGL.SetValue("COSTCENTER", sapCostCenter);

                        ACCGL.SetValue("PROFIT_CTR", accGL.PROFIT_CTR);
                        ACCGL.SetValue("ALLOC_NMBR", accGL.ALLOC_NMBR);
                    }

                    IRfcTable CURAMT = accPost.GetTable("CURRENCYAMOUNT");
                    foreach (SapDelivery_CURRENCYAMOUNT curAmt in lstCurAmt)
                    {
                        CURAMT.Append();
                        CURAMT.SetValue("ITEMNO_ACC", curAmt.ITEMNO_ACC);
                        CURAMT.SetValue("CURRENCY", curAmt.CURRENCY);
                        CURAMT.SetValue("AMT_DOCCUR", curAmt.AMT_DOCCUR);
                    }

                    RfcSessionManager.BeginContext(connection.Destination);
                    accPost.Invoke(connection.Destination);

                    IRfcTable tbl_RETURN = accPost.GetTable("RETURN");

                    string OBJ_TYPE = "" + accPost.GetValue("OBJ_TYPE");//   Reference Transaction
                    string OBJ_KEY = "" + accPost.GetValue("OBJ_KEY"); //   Reference Key
                    string OBJ_SYS = "" + accPost.GetValue("OBJ_SYS"); //   Logical system of source document

                    DataTable dt_RETURN = GetDataTable(tbl_RETURN);
                    dt_RETURN.DefaultView.RowFilter = "TYPE='E'";
                    DataTable dt_ret = dt_RETURN.DefaultView.ToTable();

                    if (dt_ret.Rows.Count == 0)
                    {
                        SAPDocNo = OBJ_KEY;

                        accCommit.SetValue("WAIT", "X");
                        accCommit.Invoke(connection.Destination);

                        rst = true;
                    }
                    else
                    {
                        SAPDocNo = "";
                        msgErr = "";
                        rst = false;
                    }

                    RfcSessionManager.EndContext(connection.Destination);

                    if (!rst)
                    {
                        transaction.Rollback();
                        msgErr = "";
                        foreach (DataRow drMsg in dt_ret.Rows)
                        {
                            msgErr += "[" + drMsg["NUMBER"] + "] " + drMsg["MESSAGE"] + "<br />";
                        }

                        SAPDocNo = "";
                        rst = false;
                    }
                    else
                    {
                        msgErr = "";
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    SAPDocNo = "";
                    rst = false;
                    while (ex.InnerException != null)
                        ex = ex.InnerException;

                    transaction.Rollback();
                    msgErr = "DeliveryLowPricePostAccount_Error " + ex.Message;
                }
                finally
                {

                }
            }

            return rst;
        }


        //Cost Center Post CrossCom บัญชีข้ามคอม 
        public bool COSTCENTERRECORD_CROSSACCOUNT(SAPCROSSCOM_DOCUMENTHEADER HEADER,
                                                  List<SAPREQUISITION_PAYABLE> PAYABLE_DETAILS,
                                                  List<SAPREQUISITION_ACCOUNTGL> CUTSTOCK_DETAILS,
                                                  List<SAPREQUISITION_RECIEVEABLE> RECEIVEABLE_DETAILS,
                                                  List<SAPREQUISITION_CURRENCYAMOUNT> CURRENCY,
                                                  bool IsMarketing,
                                                  ref string SAPDOCNO, ref string MSGERR,
                                                  out string sap_PayableDocNo,
                                                  out string sap_CutstockDocNo,
                                                  out string sap_RecieveableDocNo)
        {

            string sap_msg = "";

            if (!CHECK_ACCPAYABLE(HEADER, PAYABLE_DETAILS, CUTSTOCK_DETAILS, RECEIVEABLE_DETAILS, CURRENCY, IsMarketing, ref sap_msg))
            {
                sap_PayableDocNo = "";
                sap_CutstockDocNo = "";
                sap_RecieveableDocNo = "";
                MSGERR = sap_msg;
                return false;
            }

            bool rst = true;
            using (var connection = new SapConnection(connectionName))
            {

                connection.Open();

                var transaction = connection.BeginTransaction();
                try
                {
                    DataTable dt_RETURN = null;
                    DataTable dt_ret = null;

                    IRfcStructure DOCHDR = null;

                    IRfcTable ACCRC = null;
                    IRfcTable ACCGL = null;
                    IRfcTable ACCPA = null;
                    IRfcTable CURAMT = null;
                    IRfcTable tbl_RETURN = null;

                    IRfcFunction apPayableAPI = null;
                    IRfcFunction apCutstockAPI = null;
                    IRfcFunction apRecieveableAPIAPI = null;
                    IRfcFunction payableAPI = null;
                    IRfcFunction cutstockAPI = null;
                    IRfcFunction recieveableAPIAPI = null;

                    #region "---------  ตั้งเจ้าหนี้  ---------"
                    string OBJ_TYPE = "";
                    string OBJ_KEY = "";
                    string OBJ_SYS = "";

                    payableAPI = connection.CreateFunction("BAPI_ACC_DOCUMENT_POST");
                    apPayableAPI = connection.CreateFunction("BAPI_TRANSACTION_COMMIT");

                    DOCHDR = payableAPI.GetStructure("DOCUMENTHEADER");
                    DOCHDR.SetValue("USERNAME", HEADER.USERNAME);
                    DOCHDR.SetValue("COMP_CODE", HEADER.COMP_CODE);
                    DOCHDR.SetValue("DOC_DATE", Convert.ToDateTime(HEADER.DOC_DATE));
                    DOCHDR.SetValue("PSTNG_DATE", Convert.ToDateTime(HEADER.PSTNG_DATE));
                    DOCHDR.SetValue("FISC_YEAR", HEADER.FISC_YEAR);
                    DOCHDR.SetValue("DOC_TYPE", "KR");
                    DOCHDR.SetValue("REF_DOC_NO", HEADER.REF_DOC_NO);

                    List<ACCRECLIST> accRec = new List<ACCRECLIST>();
                    int iCount = 0;
                    ACCGL = payableAPI.GetTable("ACCOUNTGL");
                    foreach (SAPREQUISITION_PAYABLE item in PAYABLE_DETAILS.Where(q => q.GL_ACCOUNT.Equals("1580010")))
                    {
                        ACCGL.Append();
                        ACCGL.SetValue("ITEMNO_ACC", GetZeroPlusNumber(iCount += 1));
                        ACCGL.SetValue("GL_ACCOUNT", GetZeroPlusNumber(item.GL_ACCOUNT.ToString()));
                        ACCGL.SetValue("ITEM_TEXT", item.ITEM_TEXT);
                        ACCGL.SetValue("REF_KEY_3", item.REF_KEY_3);
                        ACCGL.SetValue("PROFIT_CTR", item.PROFIT_CTR);
                        ACCGL.SetValue("ALLOC_NMBR", (item.ALLOC_NMBR == "" ? "000000000000000" : item.ALLOC_NMBR));
                    }

                    ACCPA = payableAPI.GetTable("ACCOUNTPAYABLE");

                    var c = PAYABLE_DETAILS.Where(q => q.GL_ACCOUNT.Equals("1580010")).Select(q => new
                    {
                        b = q.BUSINESSPLACE,
                        c = q.GL_ACCOUNT,
                        d = q.PROFIT_CTR,
                        e = q.REF_KEY_3,
                        f = q.VENDOR_NO
                    }).Distinct().ToList();

                    for (int i = 0; i < c.Count; i++)
                    {
                        ACCPA.Append();
                        ACCPA.SetValue("ITEMNO_ACC", GetZeroPlusNumber(iCount += 1));
                        ACCPA.SetValue("VENDOR_NO", GetZeroPlusNumber(c[i].f));
                        ACCPA.SetValue("ITEM_TEXT", "");
                        ACCPA.SetValue("BUSINESSPLACE", c[i].b);
                        ACCPA.SetValue("REF_KEY_3", c[i].e);
                        ACCPA.SetValue("PROFIT_CTR", c[i].d);
                    }


                    iCount = 0;
                    CURAMT = payableAPI.GetTable("CURRENCYAMOUNT");
                    decimal negNet = 0;
                    string currency = "";

                    foreach (SAPREQUISITION_CURRENCYAMOUNT item in CURRENCY)
                    {
                        CURAMT.Append();
                        uint isNegative;
                        uint.TryParse(Convert.ToInt32(item.AMT_DOCCUR).ToString(), out isNegative);
                        if (isNegative == 0)
                        {
                            currency = item.CURRENCY;
                            negNet += Convert.ToDecimal(item.AMT_DOCCUR);
                        }
                        else
                        {
                            CURAMT.SetValue("ITEMNO_ACC", GetZeroPlusNumber(iCount += 1));
                            CURAMT.SetValue("CURRENCY", item.CURRENCY);
                            CURAMT.SetValue("AMT_DOCCUR", item.AMT_DOCCUR);
                        }
                    }
                    CURAMT.Append();
                    CURAMT.SetValue("ITEMNO_ACC", GetZeroPlusNumber(iCount += 1));
                    CURAMT.SetValue("CURRENCY", currency);
                    CURAMT.SetValue("AMT_DOCCUR", negNet);

                    RfcSessionManager.BeginContext(connection.Destination);
                    payableAPI.Invoke(connection.Destination);

                    tbl_RETURN = payableAPI.GetTable("RETURN");
                    dt_RETURN = GetDataTable(tbl_RETURN);
                    dt_RETURN.DefaultView.RowFilter = "TYPE='E'";
                    dt_ret = dt_RETURN.DefaultView.ToTable();
                    if (dt_ret.Rows.Count != 0)
                    {
                        rst = false;
                        sap_msg += "\n Error-Step1:";
                        foreach (DataRow drMsg in dt_ret.Rows)
                        {
                            sap_msg += "\n [" + drMsg["NUMBER"] + "] " + drMsg["MESSAGE"];
                        }
                    }
                    else
                    {
                        OBJ_TYPE = "" + payableAPI.GetValue("OBJ_TYPE");//   Reference Transaction
                        OBJ_KEY = "" + payableAPI.GetValue("OBJ_KEY");//   Reference Key
                        OBJ_SYS = "" + payableAPI.GetValue("OBJ_SYS");//   Logical system of source document
                    }

                    sap_PayableDocNo = OBJ_KEY;

                    #endregion

                    #region "---------  ตัดสต็อค  ---------"
                    string OBJ_TYPE2 = "";
                    string OBJ_KEY2 = "";
                    string OBJ_SYS2 = "";

                    cutstockAPI = connection.CreateFunction("BAPI_ACC_DOCUMENT_POST");
                    apCutstockAPI = connection.CreateFunction("BAPI_TRANSACTION_COMMIT");

                    DOCHDR = cutstockAPI.GetStructure("DOCUMENTHEADER");
                    DOCHDR.SetValue("USERNAME", HEADER.USERNAME);
                    DOCHDR.SetValue("COMP_CODE", HEADER.COMP_CODE);
                    DOCHDR.SetValue("DOC_DATE", Convert.ToDateTime(HEADER.DOC_DATE));
                    DOCHDR.SetValue("PSTNG_DATE", Convert.ToDateTime(HEADER.PSTNG_DATE));
                    DOCHDR.SetValue("FISC_YEAR", HEADER.FISC_YEAR);
                    DOCHDR.SetValue("DOC_TYPE", "IA");
                    DOCHDR.SetValue("REF_DOC_NO", HEADER.REF_DOC_NO);

                    iCount = 0;
                    ACCGL = cutstockAPI.GetTable("ACCOUNTGL");
                    foreach (SAPREQUISITION_ACCOUNTGL item in CUTSTOCK_DETAILS)
                    {
                        ACCGL.Append();
                        ACCGL.SetValue("ITEMNO_ACC", GetZeroPlusNumber(iCount += 1));
                        ACCGL.SetValue("GL_ACCOUNT", GetZeroPlusNumber(item.GL_ACCOUNT));
                        ACCGL.SetValue("ITEM_TEXT", item.ITEM_TEXT);
                        ACCGL.SetValue("REF_KEY_2", item.REF_KEY_2);
                        ACCGL.SetValue("REF_KEY_3", item.REF_KEY_3);

                        //คิม ถ้าเป็น Marketing ให้ใช้ ORDERID ด้วย
                        if (IsMarketing && !string.IsNullOrEmpty(item.COSTCENTER))
                        {
                            if (!string.IsNullOrEmpty(item.GL_ACCOUNT))
                            {
                                if (item.GL_ACCOUNT.Substring(0, 1) == "6")
                                {
                                    string msg = "";
                                    string InternalOrder = daStk.GetInternalOrderByCostCenter(item.COSTCENTER, ref msg);

                                    if ((InternalOrder ?? "") == "")
                                    {
                                        sap_PayableDocNo = "";
                                        sap_CutstockDocNo = "";
                                        sap_RecieveableDocNo = "";
                                        MSGERR = string.Format("ไม่พบ InternalOrder ในระบบ (Cost Center = {0})", item.COSTCENTER);
                                        return false;
                                    }

                                    ACCGL.SetValue("ORDERID", InternalOrder);
                                }
                            }
                            else
                            {

                                sap_PayableDocNo = "";
                                sap_CutstockDocNo = "";
                                sap_RecieveableDocNo = "";
                                MSGERR = "ไม่พบ GL No.";
                                return false;
                            }

                        }

                        ACCGL.SetValue("COSTCENTER", GetZeroPlusNumberCostCenter(item.COSTCENTER));

                        ACCGL.SetValue("PROFIT_CTR", item.PROFIT_CTR);
                        ACCGL.SetValue("ALLOC_NMBR", item.ALLOC_NMBR);
                    }

                    iCount = 0;
                    CURAMT = cutstockAPI.GetTable("CURRENCYAMOUNT");
                    foreach (SAPREQUISITION_CURRENCYAMOUNT item in CURRENCY)
                    {
                        CURAMT.Append();
                        CURAMT.SetValue("ITEMNO_ACC", GetZeroPlusNumber(iCount += 1));
                        CURAMT.SetValue("CURRENCY", item.CURRENCY);
                        CURAMT.SetValue("AMT_DOCCUR", item.AMT_DOCCUR);
                    }

                    RfcSessionManager.BeginContext(connection.Destination);
                    cutstockAPI.Invoke(connection.Destination);

                    tbl_RETURN = cutstockAPI.GetTable("RETURN");
                    dt_RETURN = GetDataTable(tbl_RETURN);
                    dt_RETURN.DefaultView.RowFilter = "TYPE='E'";
                    dt_ret = dt_RETURN.DefaultView.ToTable();
                    if (dt_ret.Rows.Count != 0)
                    {
                        rst = false;
                        sap_msg += "\\n Error-Step2:";
                        foreach (DataRow drMsg in dt_ret.Rows)
                        {
                            sap_msg += "\\n [" + drMsg["NUMBER"] + "] " + drMsg["MESSAGE"];
                        }
                    }
                    else
                    {
                        OBJ_TYPE2 = "" + cutstockAPI.GetValue("OBJ_TYPE");//   Reference Transaction
                        OBJ_KEY2 = "" + cutstockAPI.GetValue("OBJ_KEY");//   Reference Key
                        OBJ_SYS2 = "" + cutstockAPI.GetValue("OBJ_SYS");//   Logical system of source document
                    }

                    sap_CutstockDocNo = OBJ_KEY2;

                    #endregion

                    #region "---------  ตั้งลูกหนี้  ---------"
                    string OBJ_TYPE3 = "";
                    string OBJ_KEY3 = "";
                    string OBJ_SYS3 = "";

                    recieveableAPIAPI = connection.CreateFunction("BAPI_ACC_DOCUMENT_POST");
                    apRecieveableAPIAPI = connection.CreateFunction("BAPI_TRANSACTION_COMMIT");

                    DOCHDR = recieveableAPIAPI.GetStructure("DOCUMENTHEADER");
                    DOCHDR.SetValue("USERNAME", HEADER.USERNAME);
                    DOCHDR.SetValue("COMP_CODE", "1000");
                    DOCHDR.SetValue("DOC_DATE", Convert.ToDateTime(HEADER.DOC_DATE));
                    DOCHDR.SetValue("PSTNG_DATE", Convert.ToDateTime(HEADER.PSTNG_DATE));
                    DOCHDR.SetValue("FISC_YEAR", HEADER.FISC_YEAR);
                    DOCHDR.SetValue("DOC_TYPE", "DR");
                    DOCHDR.SetValue("REF_DOC_NO", HEADER.REF_DOC_NO);

                    iCount = 0;
                    ACCRC = recieveableAPIAPI.GetTable("ACCOUNTRECEIVABLE");
                    var cc = RECEIVEABLE_DETAILS.Where(q => q.GL_ACCOUNT.Equals("1580010")).Select(q => new
                    {
                        b = q.BUSINESSPLACE,
                        c = q.GL_ACCOUNT,
                        d = q.PROFIT_CTR,
                        e = q.REF_KEY_3,
                        f = q.CUSTOMER
                    }).Distinct().ToList();
                    for (int i = 0; i < cc.Count; i++)
                    {
                        ACCRC.Append();
                        ACCRC.SetValue("ITEMNO_ACC", GetZeroPlusNumber(iCount += 1));
                        ACCRC.SetValue("CUSTOMER", GetZeroPlusNumber(cc[i].f));
                        ACCRC.SetValue("ITEM_TEXT", "");
                        ACCRC.SetValue("BUSINESSPLACE", cc[i].b);
                        ACCRC.SetValue("REF_KEY_3", cc[i].e);
                        ACCRC.SetValue("PROFIT_CTR", "P11000");
                    }

                    ACCGL = recieveableAPIAPI.GetTable("ACCOUNTGL");
                    foreach (SAPREQUISITION_RECIEVEABLE ITEM in RECEIVEABLE_DETAILS.Where(q => q.GL_ACCOUNT.Equals("1580010")))
                    {
                        ACCGL.Append();
                        ACCGL.SetValue("ITEMNO_ACC", GetZeroPlusNumber(iCount += 1));
                        ACCGL.SetValue("GL_ACCOUNT", GetZeroPlusNumber(ITEM.GL_ACCOUNT));
                        ACCGL.SetValue("ITEM_TEXT", ITEM.ITEM_TEXT);
                        ACCGL.SetValue("PROFIT_CTR", "P11000");
                        ACCGL.SetValue("REF_KEY_3", ITEM.REF_KEY_3);
                        ACCGL.SetValue("ALLOC_NMBR", (ITEM.ALLOC_NMBR == "" ? "000000000000000" : ITEM.ALLOC_NMBR));
                    }

                    iCount = 0;
                    CURAMT = recieveableAPIAPI.GetTable("CURRENCYAMOUNT");
                    decimal negNets = 0;
                    string currencys = "";

                    foreach (SAPREQUISITION_CURRENCYAMOUNT item in CURRENCY)
                    {
                        CURAMT.Append();
                        uint isNegative;
                        uint.TryParse(Convert.ToInt32(item.AMT_DOCCUR).ToString(), out isNegative);
                        if (isNegative != 0)
                        {
                            currencys = item.CURRENCY;
                            negNets += Convert.ToDecimal(item.AMT_DOCCUR);
                        }
                    }
                    CURAMT.Append();
                    CURAMT.SetValue("ITEMNO_ACC", GetZeroPlusNumber(iCount += 1));
                    CURAMT.SetValue("CURRENCY", currencys);
                    CURAMT.SetValue("AMT_DOCCUR", negNets);

                    foreach (SAPREQUISITION_CURRENCYAMOUNT item in CURRENCY)
                    {
                        CURAMT.Append();
                        uint isNegative;
                        uint.TryParse(Convert.ToInt32(item.AMT_DOCCUR).ToString(), out isNegative);
                        if (isNegative == 0)
                        {
                            CURAMT.SetValue("ITEMNO_ACC", GetZeroPlusNumber(iCount += 1));
                            CURAMT.SetValue("CURRENCY", item.CURRENCY);
                            CURAMT.SetValue("AMT_DOCCUR", item.AMT_DOCCUR);
                        }
                    }

                    RfcSessionManager.BeginContext(connection.Destination);
                    recieveableAPIAPI.Invoke(connection.Destination);

                    tbl_RETURN = recieveableAPIAPI.GetTable("RETURN");
                    dt_RETURN = GetDataTable(tbl_RETURN);
                    dt_RETURN.DefaultView.RowFilter = "TYPE='E'";
                    dt_ret = dt_RETURN.DefaultView.ToTable();
                    if (dt_ret.Rows.Count != 0)
                    {
                        rst = false;
                        sap_msg += "\\n Error-Step3:";
                        foreach (DataRow drMsg in dt_ret.Rows)
                        {
                            sap_msg += "\\n [" + drMsg["NUMBER"] + "] " + drMsg["MESSAGE"];
                        }
                    }
                    else
                    {
                        OBJ_TYPE3 = "" + recieveableAPIAPI.GetValue("OBJ_TYPE");//   Reference Transaction
                        OBJ_KEY3 = "" + recieveableAPIAPI.GetValue("OBJ_KEY");//   Reference Key
                        OBJ_SYS3 = "" + recieveableAPIAPI.GetValue("OBJ_SYS");//   Logical system of source document
                    }

                    sap_RecieveableDocNo = OBJ_KEY3;

                    #endregion

                    if (rst)
                    {
                        apPayableAPI.SetValue("WAIT", "X");
                        apPayableAPI.Invoke(connection.Destination);
                        apCutstockAPI.SetValue("WAIT", "X");
                        apCutstockAPI.Invoke(connection.Destination);
                        apRecieveableAPIAPI.SetValue("WAIT", "X");
                        apRecieveableAPIAPI.Invoke(connection.Destination);
                    }
                    RfcSessionManager.EndContext(connection.Destination);

                    if (!rst)
                    {
                        sap_msg = "REQUISITION POST ACCOUNT CROSS COMPANY ERROR" + sap_msg;

                        transaction.Rollback();
                        rst = false;
                    }
                    else
                    {
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    rst = false;
                    sap_PayableDocNo = "";
                    sap_CutstockDocNo = "";
                    sap_RecieveableDocNo = "";
                    while (ex.InnerException != null)
                        ex = ex.InnerException;

                    transaction.Rollback();
                    sap_msg = "REQUISITION POST ACCOUNT CROSS CROMPANY ERROR " + ex.Message;
                }
                finally
                {
                    MSGERR = (MSGERR ?? "") == "" ? sap_msg : MSGERR;
                }
            }
            return rst;
        }


        public object[] ReverseAccount(string _OBJ_TYPE, string _OBJ_SYS, string OBJ_KEY_R, DateTime PSTNG_DATE, string FIS_PERIOD,
                                       string COMP_CODE, string REASON_REV, string AC_DOC_NO,
                                       out string revSapDocNo, out string msgErr)
        {
            try
            {
                bool rst = true;
                using (var connection = new SapConnection(connectionName))
                {
                    connection.Open();
                    var transaction = connection.BeginTransaction();
                    try
                    {
                        IRfcFunction revPost = connection.CreateFunction("BAPI_ACC_DOCUMENT_REV_POST");
                        IRfcFunction accCommit = connection.CreateFunction("BAPI_TRANSACTION_COMMIT");

                        IRfcStructure REVACC = revPost.GetStructure("REVERSAL");
                        REVACC.SetValue("OBJ_TYPE", _OBJ_TYPE);
                        REVACC.SetValue("OBJ_SYS", _OBJ_SYS);
                        REVACC.SetValue("OBJ_KEY_R", OBJ_KEY_R);
                        REVACC.SetValue("PSTNG_DATE", PSTNG_DATE);
                        REVACC.SetValue("FIS_PERIOD", FIS_PERIOD);
                        REVACC.SetValue("COMP_CODE", COMP_CODE);
                        REVACC.SetValue("REASON_REV", REASON_REV);
                        REVACC.SetValue("AC_DOC_NO", AC_DOC_NO);

                        RfcSessionManager.BeginContext(connection.Destination);
                        revPost.Invoke(connection.Destination);

                        IRfcTable tbl_RETURN = revPost.GetTable("RETURN");

                        string OBJ_TYPE = "" + revPost.GetValue("OBJ_TYPE");//   Reference Transaction
                        string OBJ_KEY = "" + revPost.GetValue("OBJ_KEY"); //   Reference Key
                        string OBJ_SYS = "" + revPost.GetValue("OBJ_SYS"); //   Logical system of source document

                        DataTable dt_RETURN = GetDataTable(tbl_RETURN);
                        dt_RETURN.DefaultView.RowFilter = "TYPE='E'";
                        DataTable dt_ret = dt_RETURN.DefaultView.ToTable();

                        if (dt_ret.Rows.Count == 0)
                        {
                            revSapDocNo = OBJ_KEY;
                            msgErr = "";
                            accCommit.SetValue("WAIT", "X");
                            accCommit.Invoke(connection.Destination);
                        }
                        else
                        {
                            revSapDocNo = "";
                            rst = false;
                        }
                        RfcSessionManager.EndContext(connection.Destination);

                        if (!rst)
                        {

                            transaction.Rollback();
                            msgErr = "";
                            foreach (DataRow drMsg in dt_ret.Rows)
                            {
                                msgErr += "[" + drMsg["NUMBER"] + "] " + drMsg["MESSAGE"] + "<br />";
                            }
                            revSapDocNo = "";
                        }
                        else
                        {
                            msgErr = "";
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        revSapDocNo = "";
                        while (ex.InnerException != null)
                            ex = ex.InnerException;

                        transaction.Rollback();
                        msgErr = ex.Message;
                    }
                }

                return new object[]
                {
                    ""
                };
            }
            catch (Exception ex)
            {
                revSapDocNo = "";
                msgErr = "";
                return new object[]
                {
                    ex.Message.ToString()
                };
            }
        }

        public object[] ReverseAccountCrossComCode(string _OBJ_TYPE, string _OBJ_SYS,
                                                   string OBJ_KEY_R_1, string OBJ_KEY_R_2, string OBJ_KEY_R_3,
                                                   DateTime PSTNG_DATE, string FIS_PERIOD,
                                                   string COMP_CODE_1, string COMP_CODE_2, string COMP_CODE_3,
                                                   string REASON_REV,
                                                   string AC_DOC_NO_1, string AC_DOC_NO_2, string AC_DOC_NO_3,
                                                   out string revSapDocNo_1, out string revSapDocNo_2, out string revSapDocNo_3, out string msgErr)
        {
            try
            {
                bool rst = true;
                using (var connection = new SapConnection(connectionName))
                {
                    connection.Open();
                    var transaction = connection.BeginTransaction();
                    try
                    {
                        msgErr = "";
                        DataTable dt_RETURN = null;
                        DataTable dt_ret = null;

                        IRfcTable tbl_RETURN = null;
                        IRfcFunction revPost = null;
                        IRfcFunction accCommit = null;

                        revPost = connection.CreateFunction("BAPI_ACC_DOCUMENT_REV_POST");
                        accCommit = connection.CreateFunction("BAPI_TRANSACTION_COMMIT");
                        IRfcStructure REVACC_1 = revPost.GetStructure("REVERSAL");
                        REVACC_1.SetValue("OBJ_TYPE", _OBJ_TYPE);
                        REVACC_1.SetValue("OBJ_SYS", _OBJ_SYS);
                        REVACC_1.SetValue("OBJ_KEY_R", OBJ_KEY_R_1);
                        REVACC_1.SetValue("PSTNG_DATE", PSTNG_DATE);
                        REVACC_1.SetValue("FIS_PERIOD", FIS_PERIOD);
                        REVACC_1.SetValue("COMP_CODE", COMP_CODE_1);
                        REVACC_1.SetValue("REASON_REV", REASON_REV);
                        REVACC_1.SetValue("AC_DOC_NO", AC_DOC_NO_1);

                        RfcSessionManager.BeginContext(connection.Destination);
                        revPost.Invoke(connection.Destination);

                        tbl_RETURN = revPost.GetTable("RETURN");

                        string OBJ_TYPE_1 = "" + revPost.GetValue("OBJ_TYPE");//   Reference Transaction
                        string OBJ_KEY_1 = "" + revPost.GetValue("OBJ_KEY"); //   Reference Key
                        string OBJ_SYS_1 = "" + revPost.GetValue("OBJ_SYS"); //   Logical system of source document

                        dt_RETURN = GetDataTable(tbl_RETURN);
                        dt_RETURN.DefaultView.RowFilter = "TYPE='E'";
                        dt_ret = dt_RETURN.DefaultView.ToTable();

                        if (dt_ret.Rows.Count == 0)
                        {
                            revSapDocNo_1 = OBJ_KEY_1;
                            msgErr = "";
                            accCommit.SetValue("WAIT", "X");
                            accCommit.Invoke(connection.Destination);
                        }
                        else
                        {
                            revSapDocNo_1 = "";
                            foreach (DataRow drMsg in dt_ret.Rows)
                            {
                                msgErr += "[" + drMsg["NUMBER"] + "] " + drMsg["MESSAGE"] + "<br />";
                            }
                            rst = false;
                        }

                        RfcSessionManager.EndContext(connection.Destination);


                        revPost = connection.CreateFunction("BAPI_ACC_DOCUMENT_REV_POST");
                        accCommit = connection.CreateFunction("BAPI_TRANSACTION_COMMIT");
                        IRfcStructure REVACC_2 = revPost.GetStructure("REVERSAL");
                        REVACC_2.SetValue("OBJ_TYPE", _OBJ_TYPE);
                        REVACC_2.SetValue("OBJ_SYS", _OBJ_SYS);
                        REVACC_2.SetValue("OBJ_KEY_R", OBJ_KEY_R_2);
                        REVACC_2.SetValue("PSTNG_DATE", PSTNG_DATE);
                        REVACC_2.SetValue("FIS_PERIOD", FIS_PERIOD);
                        REVACC_2.SetValue("COMP_CODE", COMP_CODE_2);
                        REVACC_2.SetValue("REASON_REV", REASON_REV);
                        REVACC_2.SetValue("AC_DOC_NO", AC_DOC_NO_2);

                        RfcSessionManager.BeginContext(connection.Destination);
                        revPost.Invoke(connection.Destination);

                        tbl_RETURN = revPost.GetTable("RETURN");

                        string OBJ_TYPE_2 = "" + revPost.GetValue("OBJ_TYPE");//   Reference Transaction
                        string OBJ_KEY_2 = "" + revPost.GetValue("OBJ_KEY"); //   Reference Key
                        string OBJ_SYS_2 = "" + revPost.GetValue("OBJ_SYS"); //   Logical system of source document

                        dt_RETURN = GetDataTable(tbl_RETURN);
                        dt_RETURN.DefaultView.RowFilter = "TYPE='E'";
                        dt_ret = dt_RETURN.DefaultView.ToTable();

                        if (dt_ret.Rows.Count == 0)
                        {
                            revSapDocNo_2 = OBJ_KEY_2;
                            msgErr = "";
                            accCommit.SetValue("WAIT", "X");
                            accCommit.Invoke(connection.Destination);
                        }
                        else
                        {
                            revSapDocNo_2 = "";
                            foreach (DataRow drMsg in dt_ret.Rows)
                            {
                                msgErr += "[" + drMsg["NUMBER"] + "] " + drMsg["MESSAGE"] + "<br />";
                            }
                            rst = false;
                        }
                        RfcSessionManager.EndContext(connection.Destination);


                        revPost = connection.CreateFunction("BAPI_ACC_DOCUMENT_REV_POST");
                        accCommit = connection.CreateFunction("BAPI_TRANSACTION_COMMIT");
                        IRfcStructure REVACC = revPost.GetStructure("REVERSAL");
                        REVACC.SetValue("OBJ_TYPE", _OBJ_TYPE);
                        REVACC.SetValue("OBJ_SYS", _OBJ_SYS);
                        REVACC.SetValue("OBJ_KEY_R", OBJ_KEY_R_3);
                        REVACC.SetValue("PSTNG_DATE", PSTNG_DATE);
                        REVACC.SetValue("FIS_PERIOD", FIS_PERIOD);
                        REVACC.SetValue("COMP_CODE", COMP_CODE_3);
                        REVACC.SetValue("REASON_REV", REASON_REV);
                        REVACC.SetValue("AC_DOC_NO", AC_DOC_NO_3);

                        RfcSessionManager.BeginContext(connection.Destination);
                        revPost.Invoke(connection.Destination);

                        tbl_RETURN = revPost.GetTable("RETURN");

                        string OBJ_TYPE_3 = "" + revPost.GetValue("OBJ_TYPE");//   Reference Transaction
                        string OBJ_KEY_3 = "" + revPost.GetValue("OBJ_KEY"); //   Reference Key
                        string OBJ_SYS_3 = "" + revPost.GetValue("OBJ_SYS"); //   Logical system of source document

                        dt_RETURN = GetDataTable(tbl_RETURN);
                        dt_RETURN.DefaultView.RowFilter = "TYPE='E'";
                        dt_ret = dt_RETURN.DefaultView.ToTable();

                        if (dt_ret.Rows.Count == 0)
                        {
                            revSapDocNo_3 = OBJ_KEY_3;
                            foreach (DataRow drMsg in dt_ret.Rows)
                            {
                                msgErr += "[" + drMsg["NUMBER"] + "] " + drMsg["MESSAGE"] + "<br />";
                            }
                            accCommit.SetValue("WAIT", "X");
                            accCommit.Invoke(connection.Destination);
                        }
                        else
                        {
                            revSapDocNo_3 = "";
                            rst = false;
                        }
                        RfcSessionManager.EndContext(connection.Destination);

                        if (!rst)
                        {

                            transaction.Rollback();
                            foreach (DataRow drMsg in dt_ret.Rows)
                            {
                                msgErr += "[" + drMsg["NUMBER"] + "] " + drMsg["MESSAGE"] + "<br />";
                            }
                            revSapDocNo_1 = "";
                            revSapDocNo_2 = "";
                            revSapDocNo_3 = "";
                        }
                        else
                        {
                            msgErr = "";
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        revSapDocNo_1 = "";
                        revSapDocNo_2 = "";
                        revSapDocNo_3 = "";
                        while (ex.InnerException != null)
                            ex = ex.InnerException;

                        transaction.Rollback();
                        msgErr = "DeliveryLowPricePostAccount_Error " + ex.Message;
                    }
                    finally
                    {
                    }
                }
                return new object[]
                {
                    ""
                };
            }
            catch (Exception ex)
            {
                revSapDocNo_1 = "";
                revSapDocNo_2 = "";
                revSapDocNo_3 = "";
                msgErr = ex.Message.ToString();
                return new object[]
                {
                    ex.Message.ToString()
                };
            }
        }


        public object[] CancelDeletePR(string prno, string isTest, string isDel, List<DataRow> lstItem, out string msgerr)
        {
            try
            {
                string MsgErr = "";
                using (var connection = new SapConnection(connectionName))
                {
                    connection.Open();
                    var transaction = connection.BeginTransaction();
                    try
                    {
                        DataTable tbl_ret = null;
                        IRfcTable tbl_RETURN = null;

                        if (SAPGetPRDetail(prno, connection, ref tbl_ret, ref MsgErr))
                        {
                            bool rst = true;
                            IRfcFunction apAPI = connection.CreateFunction("BAPI_PR_CHANGE");
                            IRfcFunction apCMT = connection.CreateFunction("BAPI_TRANSACTION_COMMIT");

                            apAPI.SetValue("NUMBER", prno);

                            IRfcTable PRITEM = apAPI.GetTable("PRITEM");
                            IRfcTable PRITEMX = apAPI.GetTable("PRITEMX");

                            for (int i = 0; i < lstItem.Count; i++)
                            {
                                PRITEM.Append();
                                PRITEM.SetValue("PREQ_ITEM", lstItem[i].ItemArray[4]);
                                PRITEM.SetValue("DELETE_IND", isDel);

                                PRITEMX.Append();
                                PRITEMX.SetValue("PREQ_ITEM", lstItem[i].ItemArray[4]);
                                PRITEMX.SetValue("DELETE_IND", "X");
                            }

                            RfcSessionManager.BeginContext(connection.Destination);
                            apAPI.Invoke(connection.Destination);

                            tbl_RETURN = apAPI.GetTable("RETURN");

                            DataTable dt_RETURN = GetDataTable(tbl_RETURN);
                            dt_RETURN.DefaultView.RowFilter = "TYPE='E'";
                            DataTable dt_ret = dt_RETURN.DefaultView.ToTable();
                            if (dt_ret.Rows.Count != 0)
                            {
                                rst = false;
                            }
                            else
                            {
                                apCMT.SetValue("WAIT", "X");
                                apCMT.Invoke(connection.Destination);
                            }
                            RfcSessionManager.EndContext(connection.Destination);

                            if (!rst)
                            {

                                transaction.Rollback();
                                MsgErr = "SAPChangeStatusPR";
                                foreach (DataRow drMsg in dt_ret.Rows)
                                {
                                    MsgErr += "\\n [" + drMsg["NUMBER"] + "] " + drMsg["MESSAGE"];
                                }
                                rst = false;
                            }
                            else
                            {
                                transaction.Commit();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new object[]
                        {
                            msgerr = ex.Message.ToString()
                        };
                    }
                    finally
                    {
                    }
                }
                return new object[]
                {
                    msgerr = MsgErr
                };
            }
            catch (Exception ex)
            {
                return new object[]{
                    msgerr = ex.Message.ToString()
                };
            }
        }


        //public object[] CancelPR(string _OBJ_TYPE, string _OBJ_SYS, string OBJ_KEY_R, DateTime PSTNG_DATE, string FIS_PERIOD,
        //                               string COMP_CODE, string REASON_REV, string AC_DOC_NO,
        //                               out string revSapDocNo, out string msgErr)
        //{
        //    try
        //    {
        //        bool rst = true;
        //        using (var connection = new SapConnection(connectionName))
        //        {
        //            connection.Open();
        //            var transaction = connection.BeginTransaction();
        //            try
        //            {
        //                IRfcFunction revPost = connection.CreateFunction("BAPI_ACC_DOCUMENT_REV_POST");
        //                IRfcFunction accCommit = connection.CreateFunction("BAPI_TRANSACTION_COMMIT");

        //                IRfcStructure REVACC = revPost.GetStructure("REVERSAL");
        //                REVACC.SetValue("OBJ_TYPE", _OBJ_TYPE);
        //                REVACC.SetValue("OBJ_SYS", _OBJ_SYS);
        //                REVACC.SetValue("OBJ_KEY_R", OBJ_KEY_R);
        //                REVACC.SetValue("PSTNG_DATE", PSTNG_DATE);
        //                REVACC.SetValue("FIS_PERIOD", FIS_PERIOD);
        //                REVACC.SetValue("COMP_CODE", COMP_CODE);
        //                REVACC.SetValue("REASON_REV", REASON_REV);
        //                REVACC.SetValue("AC_DOC_NO", AC_DOC_NO);



        //                RfcSessionManager.BeginContext(connection.Destination);
        //                revPost.Invoke(connection.Destination);

        //                IRfcTable tbl_RETURN = revPost.GetTable("RETURN");

        //                string OBJ_TYPE = "" + revPost.GetValue("OBJ_TYPE");//   Reference Transaction
        //                string OBJ_KEY = "" + revPost.GetValue("OBJ_KEY"); //   Reference Key
        //                string OBJ_SYS = "" + revPost.GetValue("OBJ_SYS"); //   Logical system of source document

        //                DataTable dt_RETURN = GetDataTable(tbl_RETURN);
        //                dt_RETURN.DefaultView.RowFilter = "TYPE='E'";
        //                DataTable dt_ret = dt_RETURN.DefaultView.ToTable();

        //                if (dt_ret.Rows.Count == 0)
        //                {
        //                    revSapDocNo = OBJ_KEY;
        //                    msgErr = "";
        //                    accCommit.SetValue("WAIT", "X");
        //                    accCommit.Invoke(connection.Destination);
        //                }
        //                else
        //                {
        //                    revSapDocNo = "";
        //                    rst = false;
        //                }
        //                RfcSessionManager.EndContext(connection.Destination);

        //                if (!rst)
        //                {

        //                    transaction.Rollback();
        //                    msgErr = "";
        //                    foreach (DataRow drMsg in dt_ret.Rows)
        //                    {
        //                        msgErr += "[" + drMsg["NUMBER"] + "] " + drMsg["MESSAGE"] + "<br />";
        //                    }
        //                    revSapDocNo = "";
        //                }
        //                else
        //                {
        //                    msgErr = "";
        //                    transaction.Commit();
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                revSapDocNo = "";
        //                while (ex.InnerException != null)
        //                    ex = ex.InnerException;

        //                transaction.Rollback();
        //                msgErr = ex.Message;
        //            }
        //            finally
        //            {
        //            }
        //        }

        //        return new object[]
        //        {
        //            ""
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        revSapDocNo = "";
        //        msgErr = "";
        //        return new object[]
        //        {
        //            ex.Message.ToString()
        //        };
        //    }
        //}

        
        public DataTable SAP_GET_PRApprover()
        {

            var returnData = new Dictionary<string, object>();

            using (var connection = new SapConnection(connectionName))
            {

                connection.Open();

                try
                {
                    var apAPI = connection.CreateFunction("ZRFCMM06");

                    RfcSessionManager.BeginContext(connection.Destination);

                    apAPI.Invoke(connection.Destination);

                    RfcSessionManager.EndContext(connection.Destination);

                    var tbl_PROJ = apAPI.GetTable("T_EXPORT");

                    return CreateDataTable(tbl_PROJ);
                }
                catch (Exception ex)
                {

                    while (ex.InnerException != null)
                        ex = ex.InnerException;

                    throw new Exception(TAG + ".SAP_GET_PRApprover() :: Error ::" + ex.Message, ex);
                }

            }
        }
    
        public DataTable CreateDataTable(IRfcTable rfcTable)
        {
            var dataTable = new DataTable();

            for (int element = 0; element < rfcTable.ElementCount; element++)
            {
                RfcElementMetadata metadata = rfcTable.GetElementMetadata(element);
                dataTable.Columns.Add(metadata.Name);
            }

            foreach (IRfcStructure row in rfcTable)
            {
                DataRow newRow = dataTable.NewRow();
                for (int element = 0; element < rfcTable.ElementCount; element++)
                {
                    RfcElementMetadata metadata = rfcTable.GetElementMetadata(element);
                    newRow[metadata.Name] = row.GetString(metadata.Name);
                }
                dataTable.Rows.Add(newRow);
            }

            return dataTable;
        }


    }
}
