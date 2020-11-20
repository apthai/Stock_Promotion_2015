using System;
using System.Globalization;

namespace Entities
{
    public class FormatDate
    {
        public string formatDate = "dd/MM/yyyy";
        public DateTime getDateFormat(string v)
        {
            try
            {
                return DateTime.ParseExact(v, formatDate, CultureInfo.InvariantCulture);
            }
            catch (Exception) { }
            DateTime sDate = new DateTime();
            DateTime.TryParse(v, out sDate);
            return sDate;
        }
        public bool getDateFromString(string v, ref DateTime dateVal)
        {
            try
            {
                dateVal = DateTime.ParseExact(v, formatDate, CultureInfo.InvariantCulture);
                return true;
            }
            catch (Exception) { }
            return false;
        }
        public bool getStringFromDate(object v, ref String dateStr)
        {
            try
            {
                dateStr = ((DateTime)v).ToString(formatDate);
                return true;
            }
            catch (Exception) { }
            return false;
        }
    }

    public class MasterItemInfo
    {
        public int MasterItemId { get; set; }
        public int MasterItemGroupId { get; set; }
        public string ItemNo { get; set; }
        public string ItemName { get; set; }
        public string ItemUnit { get; set; }
        public decimal ItemCost { get; set; }
        public decimal ItemCostIncVat { get; set; }
        public string ItemCountMethod { get; set; }
        public string ItemStock { get; set; }
        public string ItemForceExpire { get; set; }
        public string ItemStatus { get; set; }
        public string UpdateBy { get; set; }
        public DateTime _UpdateDate;
        public string UpdateDate
        {
            get { return _UpdateDate.ToString(new FormatDate().formatDate); }
            set { _UpdateDate = new FormatDate().getDateFormat(value); }
        }


        public decimal ItemCostBeg { get; set; }
        public decimal ItemCostEnd { get; set; }
    }


    public class MasterItemGroupInfo
    {
        public int MasterItemGroupId { get; set; }
        public string ItemGroupName { get; set; }
        public string UpdateBy { get; set; }
        public DateTime _UpdateDate;
        public string UpdateDate
        {
            get { return _UpdateDate.ToString(new FormatDate().formatDate); }
            set { _UpdateDate = new FormatDate().getDateFormat(value); }
        }
    }


    public class StockReceiveDetail
    {
        public long ReceiveDetailId { get; set; }
        public long ReceiveHeaderId { get; set; }
        public int ProjectId { get; set; }
        public int MasterItemId { get; set; }
        public string ItemName { get; set; }
        public int OrderAmount { get; set; }
        public string ItemColor { get; set; }
        public decimal Height { get; set; }
        public decimal Width { get; set; }
        public decimal Weight { get; set; }
        public decimal ItemCost { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime _EndOfWarranty;
        public string EndOfWarranty
        {
            get { return _EndOfWarranty.ToString(new FormatDate().formatDate); }
            set { _EndOfWarranty = new FormatDate().getDateFormat(value); }
        }
        public DateTime _ExpireDate;
        public string ExpireDate
        {
            get { return _ExpireDate.ToString(new FormatDate().formatDate); }
            set { _ExpireDate = new FormatDate().getDateFormat(value); }
        }
        public DateTime _AlertExpireDate;
        public string AlertExpireDate
        {
            get { return _AlertExpireDate.ToString(new FormatDate().formatDate); }
            set { _AlertExpireDate = new FormatDate().getDateFormat(value); }
        }
        public string DocRef1 { get; set; }
        public string DocPath1 { get; set; }
        public string DocRef2 { get; set; }
        public string DocPath2 { get; set; }
        public string DocRef3 { get; set; }
        public string DocPath3 { get; set; }
        public string Status { get; set; }
        public string UpdateBy { get; set; }
        public DateTime _UpdateDate;
        public string UpdateDate
        {
            get { return _UpdateDate.ToString(new FormatDate().formatDate); }
            set { _UpdateDate = new FormatDate().getDateFormat(value); }
        }
    }


    public class RequisitionInfo
    {
        public long ReqHeaderId { get; set; }
        public string ReqNo { get; set; }
        public DateTime _ReqDate;
        public string ReqDate
        {
            get { return _ReqDate.ToString(new FormatDate().formatDate); }
            set { _ReqDate = new FormatDate().getDateFormat(value); }
        }
        public string ReqDocNo { get; set; }
        public DateTime _ReqDocDate;
        public string ReqDocDate
        {
            get { return _ReqDocDate.ToString(new FormatDate().formatDate); }
            set { _ReqDocDate = new FormatDate().getDateFormat(value); }
        }
        public string ReqBy { get; set; }
        public string ReqType { get; set; }
        public string ReqHeaderRemark { get; set; }
        public string UpdateBy { get; set; }
        public long ReqId { get; set; }
        public int Project_Id { get; set; }
        public string ProjectID { get; set; }
        public string ProjectName { get; set; }
        public DateTime _ProStartDate;
        public string ProStartDate
        {
            get { return _ProStartDate.ToString(new FormatDate().formatDate); }
            set { _ProStartDate = new FormatDate().getDateFormat(value); }
        }
        public DateTime _ProEndDate;
        public string ProEndDate
        {
            get { return _ProEndDate.ToString(new FormatDate().formatDate); }
            set { _ProEndDate = new FormatDate().getDateFormat(value); }
        }
        public int ProAlertDate { get; set; }
        public string ReqStatus { get; set; }
        public int ItemId { get; set; }
        public string ItemNo { get; set; }
        public string ItemName { get; set; }
        public int ReqAmount { get; set; }
        public string ItemUnit { get; set; }
        public string WithCRMData { get; set; }

        public DateTime _ReqDateFrom;
        public string ReqDateFrom
        {
            get { return _ReqDateFrom.ToString(new FormatDate().formatDate); }
            set { _ReqDateFrom = new FormatDate().getDateFormat(value); }
        }
        public DateTime _ReqDateTo;
        public string ReqDateTo
        {
            get { return _ReqDateTo.ToString(new FormatDate().formatDate); }
            set { _ReqDateTo = new FormatDate().getDateFormat(value); }
        }

    }


    public class StockReceiveInfo
    {
        /* -= Receive Header =- */
        public long ReceiveHeaderID { get; set; }
        public string PO_No { get; set; }
        public string GR_No { get; set; }
        public string GR_Year { get; set; }
        public string GRCancel_No { get; set; }
        public string GRCancel_Year { get; set; }
        public string Vendor { get; set; }

        public DateTime _DocDate;
        public string DocDate
        {
            get { return _DocDate.ToString(new FormatDate().formatDate); }
            set { _DocDate = new FormatDate().getDateFormat(value); }
        }
        public DateTime _PostingDate;
        public string PostingDate
        {
            get { return _PostingDate.ToString(new FormatDate().formatDate); }
            set { _PostingDate = new FormatDate().getDateFormat(value); }
        }
        public string DocRefNo { get; set; }

        public string CreateBy { get; set; }
        public DateTime _CreateDate;
        public string CreateDate
        {
            get { return _CreateDate.ToString(new FormatDate().formatDate); }
            set { _CreateDate = new FormatDate().getDateFormat(value); }
        }
        public string ReceiveHeaderStatus { get; set; }
        public DateTime _UpdateDate;
        public string UpdateDate
        {
            get { return _UpdateDate.ToString(new FormatDate().formatDate); }
            set { _UpdateDate = new FormatDate().getDateFormat(value); }
        }
        public string UpdateBy { get; set; }

        /* -= Receive Detail =- */
        public long ReceiveHeaderId { get; set; }
        public int MasterItemId { get; set; }
        public int MasterItemGroupId { get; set; }
        public string ItemNo { get; set; }
        public decimal PricePerUnit { get; set; }
        public decimal ReceiveAmount { get; set; }
        public string Status { get; set; }

        public DateTime _CreateDateFrom;
        public string CreateDateFrom
        {
            get { return _CreateDateFrom.ToString(new FormatDate().formatDate); }
            set { _CreateDateFrom = new FormatDate().getDateFormat(value); }
        }
        public DateTime _CreateDateTo;
        public string CreateDateTo
        {
            get { return _CreateDateTo.ToString(new FormatDate().formatDate); }
            set { _CreateDateTo = new FormatDate().getDateFormat(value); }
        }

        public string SAP_EBELN { get; set; }

        public decimal SAP_EBELP { get; set; }
        public string SAP_BSART { get; set; }
        public string SAP_BUKRS { get; set; }
        public string SAP_WERKS { get; set; }
        public string SAP_MATNR { get; set; }
        public string SAP_TXZ01 { get; set; }
        public int SAP_MENGE { get; set; }
        public int SAP_MENGE_A { get; set; }
        public string SAP_MEINS { get; set; }
        public decimal SAP_NETPR { get; set; }
        public decimal SAP_NETWR { get; set; }
        public decimal SAP_NAVNW { get; set; }
        public decimal SAP_EFFWR { get; set; }
        public decimal SAP_WAERS { get; set; }
        public string SAP_BANFN { get; set; }
        public decimal SAP_BNFPO { get; set; }
        public string SAP_KOSTL { get; set; }
        public string SAP_NPLNR { get; set; }
        public decimal SAP_PS_PSP_PNR { get; set; }
        public string SAP_WBS_SHOW { get; set; }

        public string SAP_LIFNR { get; set; }
        public string SAP_VENDOR_NAME { get; set; }
        public string SAP_ZTERM { get; set; }
    }



    public class ReqItemList
    {
        public long reqId { get; set; }
        public string ItemList { get; set; }
        public long? totalAmount { get; set; }
        public long? refMatId { get; set; }
    }

    public class SAPCROSSCOM_DOCUMENTHEADER
    {
        public DateTime doc_date;
        public DateTime pstng_date;
        public string USERNAME { get; set; }
        public string COMP_CODE { get; set; }
        public string DOC_DATE { get; set; }
        public string PSTNG_DATE { get; set; }
        public int FISC_YEAR { get; set; }
        public string DOC_TYPE { get; set; }
        public string REF_DOC_NO { get; set; }
    }
    public class SAPCROSSCOM_ACCGLANDPAYABLE
    {
        public int ITEMNO_ACC { get; set; }
        public string GL_ACCOUNT { get; set; }
        public string VENDOR_NO { get; set; }
        public string ITEM_TEXT { get; set; }
        public string BUSINESSPLACE { get; set; }
        public string REF_KEY_3 { get; set; }
        public string PROFIT_CTR { get; set; }
        public string ALLOC_NMBR { get; set; }
    }
    public class SAPCROSSCOM_CURRENCYAMOUNT
    {
        public int ITEMNO_ACC { get; set; }
        public string CURRENCY { get; set; }
        public decimal AMT_DOCCUR { get; set; }
    }


    public class SapDelivery_DOCUMENTHEADER
    {
        public string USERNAME { get; set; }
        public string COMP_CODE { get; set; }
        public DateTime _DOC_DATE;
        public string DOC_DATE
        {
            get { return _DOC_DATE.ToString(new FormatDate().formatDate); }
            set { _DOC_DATE = new FormatDate().getDateFormat(value); }
        }
        public DateTime _PSTNG_DATE;
        public string PSTNG_DATE
        {
            get { return _PSTNG_DATE.ToString(new FormatDate().formatDate); }
            set { _PSTNG_DATE = new FormatDate().getDateFormat(value); }
        }
        public int FISC_YEAR { get; set; }
        public string DOC_TYPE { get; set; }
        public string REF_DOC_NO { get; set; }
        public string REF_KEY_3 { get; set; }
    }

    public class SapDelivery_ACCOUNTGL
    {
        public int ITEMNO_ACC { get; set; }
        public string GL_ACCOUNT { get; set; }
        public string ITEM_TEXT { get; set; }
        public string REF_KEY_2 { get; set; }
        public string COSTCENTER { get; set; }
        public string WBS_ELEMENT { get; set; }
        //public int ITEMNO_ACC { get; set; }
        //public string GL_ACCOUNT { get; set; }
        //public string ITEM_TEXT { get; set; }
        public string PROFIT_CTR { get; set; }
        public string REF_KEY_3 { get; set; }

        public string ALLOC_NMBR { get; set; }
    }

    public class SapDeliveryLowPrice_ACCOUNTGL
    {
        public int ITEMNO_ACC { get; set; }
        public string GL_ACCOUNT { get; set; }
        public string ITEM_TEXT { get; set; }
        public string ORDERID { get; set; }
        public string PROFIT_CTR { get; set; }
        public string REF_KEY_3 { get; set; }
        public string ALLOC_NMBR { get; set; }
    }

    public class SapDelivery_CURRENCYAMOUNT
    {
        public int ITEMNO_ACC { get; set; }
        public string CURRENCY { get; set; }
        public decimal AMT_DOCCUR { get; set; }
        //public int ITEMNO_ACC { get; set; }
        //public string CURRENCY { get; set; }
        //public decimal AMT_DOCCUR { get; set; }
    }

    public class ACCRECLIST
    {
        public string RQRNO { get; set; }
        public int ACCRECTYPECODE { get; set; }
        public int CCAREFREQID { get; set; }
        public string GLNO { get; set; }
        public string GLNAME { get; set; }
        public string COSTCENTERID { get; set; }
        public string PROFITCENTER { get; set; }
        public string DEBIT { get; set; }
        public string CREDIT { get; set; }
        public string ITEMTEXT { get; set; }
        public string ITEMNO { get; set; }
    }
    public class CREDITOR : ACCRECLIST
    {

    }
    public class CUTSTOCK : ACCRECLIST
    {

    }
    public class DEDTOR : ACCRECLIST
    {

    }
    public class ACCREC : ACCRECLIST
    {

    }

    public class SAPREQUISITION_ACCOUNTGL
    {
        public int ITEMNO_ACC { get; set; }
        public string GL_ACCOUNT { get; set; }
        public string ITEM_TEXT { get; set; }
        public string REF_KEY_2 { get; set; }
        public string REF_KEY_3 { get; set; }
        public string COSTCENTER { get; set; }
        public string PROFIT_CTR { get; set; }
        public string ALLOC_NMBR { get; set; }
    }

    public class SAPREQUISITION_PAYABLE
    {
        public int ITEMNO_ACC { get; set; }
        public string GL_ACCOUNT { get; set; }
        public string VENDOR_NO { get; set; }
        public string ITEM_TEXT { get; set; }
        public string BUSINESSPLACE { get; set; }
        public string REF_KEY_3 { get; set; }
        public string PROFIT_CTR { get; set; }
        public string ALLOC_NMBR { get; set; }
    }

    public class SAPREQUISITION_RECIEVEABLE
    {
        public int ITEMNO_ACC { get; set; }
        public string CUSTOMER { get; set; }
        public string GL_ACCOUNT { get; set; }
        public string ITEM_TEXT { get; set; }
        public string BUSINESSPLACE { get; set; }
        public string PROFIT_CTR { get; set; }
        public string REF_KEY_3 { get; set; }
        public string ALLOC_NMBR { get; set; }
    }

    public class SAPREQUISITION_CURRENCYAMOUNT : SapDelivery_CURRENCYAMOUNT
    {

    }





    /* - ตัดสต๊อกสูญเสีย - */

    public class SapDestroy_DOCUMENTHEADER
    {
        public string USERNAME { get; set; }
        public string COMP_CODE { get; set; }
        public DateTime _DOC_DATE;
        public string DOC_DATE
        {
            get { return _DOC_DATE.ToString(new FormatDate().formatDate); }
            set { _DOC_DATE = new FormatDate().getDateFormat(value); }
        }
        public DateTime _PSTNG_DATE;
        public string PSTNG_DATE
        {
            get { return _PSTNG_DATE.ToString(new FormatDate().formatDate); }
            set { _PSTNG_DATE = new FormatDate().getDateFormat(value); }
        }
        public int FISC_YEAR { get; set; }
        public string DOC_TYPE { get; set; }
    }

    public class SapDestroy_ACCOUNTGL
    {
        public int ITEMNO_ACC { get; set; }
        public string GL_ACCOUNT { get; set; }
        public string ITEM_TEXT { get; set; }
        public string PROFIT_CTR { get; set; }
        public string REF_KEY_3 { get; set; }
    }

    public class SapDestroy_CURRENCYAMOUNT
    {
        public int ITEMNO_ACC { get; set; }
        public string CURRENCY { get; set; }
        public decimal AMT_DOCCUR { get; set; }
    }

    /* - ยกเลิกตัดสต๊อกสูญเสีย - */

    public class SapDestroy_REVERSAL
    {
        public string OBJ_TYPE { get; set; }
        public string OBJ_SYS { get; set; }
        public string OBJ_KEY_R { get; set; }
        public DateTime _PostingDate;
        public string PostingDate
        {
            get { return _PostingDate.ToString(new FormatDate().formatDate); }
            set { _PostingDate = new FormatDate().getDateFormat(value); }
        }
        public string FIS_PERIOD { get; set; }
        public string COMP_CODE { get; set; }
        public string REASON_REV { get; set; }
        public string AC_DOC_NO { get; set; }
    }


    /* - MK ตัด Stock ข้าม Company  - */
    public class SapDeliveryCrossCmpStep1_DOCUMENTHEADER
    {
        public string USERNAME { get; set; }
        public string COMP_CODE { get; set; }
        public DateTime _DOC_DATE;
        public string DOC_DATE
        {
            get { return _DOC_DATE.ToString(new FormatDate().formatDate); }
            set { _DOC_DATE = new FormatDate().getDateFormat(value); }
        }
        public DateTime _PSTNG_DATE;
        public string PSTNG_DATE
        {
            get { return _PSTNG_DATE.ToString(new FormatDate().formatDate); }
            set { _PSTNG_DATE = new FormatDate().getDateFormat(value); }
        }
        public int FISC_YEAR { get; set; }
        public string DOC_TYPE { get; set; }
        public string REF_DOC_NO { get; set; }
        public string REF_KEY_3 { get; set; }
        
    }

    public class SapDeliveryCrossCmpStep1_ACCOUNTGL
    {
        public int ITEMNO_ACC { get; set; }
        public string GL_ACCOUNT { get; set; }
        public string ITEM_TEXT { get; set; }
        public string PROFIT_CTR { get; set; }
        public string REF_KEY_3 { get; set; }
        public string ALLOC_NMBR { get; set; }
    }

    public class SapDeliveryCrossCmpStep1_ACCOUNTPAYABLE
    {
        public int ITEMNO_ACC { get; set; }
        public string VENDOR_NO { get; set; }
        public string BUSINESSPLACE { get; set; }
        public string PROFIT_CTR { get; set; }
        public string ITEM_TEXT { get; set; }
    }

    public class SapDeliveryCrossCmpStep1_CURRENCYAMOUNT
    {
        public int ITEMNO_ACC { get; set; }
        public string CURRENCY { get; set; }
        public decimal AMT_DOCCUR { get; set; }
    }

    public class SapDeliveryCrossCmpStep2_DOCUMENTHEADER
    {
        public string USERNAME { get; set; }
        public string COMP_CODE { get; set; }
        public DateTime _DOC_DATE;
        public string DOC_DATE
        {
            get { return _DOC_DATE.ToString(new FormatDate().formatDate); }
            set { _DOC_DATE = new FormatDate().getDateFormat(value); }
        }
        public DateTime _PSTNG_DATE;
        public string PSTNG_DATE
        {
            get { return _PSTNG_DATE.ToString(new FormatDate().formatDate); }
            set { _PSTNG_DATE = new FormatDate().getDateFormat(value); }
        }
        public int FISC_YEAR { get; set; }
        public string DOC_TYPE { get; set; }
        public string REF_DOC_NO { get; set; }
    }
    public class SapDeliveryCrossCmpStep2_ACCOUNTGL
    {
        public int ITEMNO_ACC { get; set; }
        public string GL_ACCOUNT { get; set; }
        public string ITEM_TEXT { get; set; }
        public string REF_KEY_2 { get; set; }
        public string COSTCENTER { get; set; }
        public string WBS_ELEMENT { get; set; }
        public string PROFIT_CTR { get; set; }
        public string ORDERID { get; set; }
        public string DOC_TYPE { get; set; }
        public string REF_KEY_3 { get; set; }
        public string ALLOC_NMBR { get; set; }
    }
    public class SapDeliveryCrossCmpStep2_CURRENCYAMOUNT
    {
        public int ITEMNO_ACC { get; set; }
        public string CURRENCY { get; set; }
        public decimal AMT_DOCCUR { get; set; }
    }

    public class SapDeliveryCrossCmpStep3_DOCUMENTHEADER
    {
        public string USERNAME { get; set; }
        public string COMP_CODE { get; set; }

        public DateTime _DOC_DATE;
        public string DOC_DATE
        {
            get { return _DOC_DATE.ToString(new FormatDate().formatDate); }
            set { _DOC_DATE = new FormatDate().getDateFormat(value); }
        }

        public DateTime _PSTNG_DATE;

        public string PSTNG_DATE
        {
            get { return _PSTNG_DATE.ToString(new FormatDate().formatDate); }
            set { _PSTNG_DATE = new FormatDate().getDateFormat(value); }
        }
        public int FISC_YEAR { get; set; }
        public string DOC_TYPE { get; set; }
        public string REF_DOC_NO { get; set; }
    }
    public class SapDeliveryCrossCmpStep3_ACCOUNTRECEIVABLE
    {
        /*
            ITEMNO_ACC	NUMC	10
            CUSTOMER	CHAR	10
            PROFIT_CTR	CHAR	10
            BUSINESSPLACE	CHAR	4
            ITEM_TEXT	CHAR	50
        */
        public int ITEMNO_ACC { get; set; }
        public string CUSTOMER { get; set; }
        public string PROFIT_CTR { get; set; }
        public string BUSINESSPLACE { get; set; }
        public string ITEM_TEXT { get; set; }
    }
    public class SapDeliveryCrossCmpStep3_ACCOUNTGL
    {
        /*
            ITEMNO_ACC	NUMC	10
            GL_ACCOUNT	CHAR	10
            ITEM_TEXT	CHAR	50
            PROFIT_CTR	CHAR	12
        */
        public int ITEMNO_ACC { get; set; }
        public string GL_ACCOUNT { get; set; }
        public string ITEM_TEXT { get; set; }
        public string PROFIT_CTR { get; set; }
        public string REF_KEY_3 { get; set; }
        public string ALLOC_NMBR { get; set; }
    }

    public class SapDeliveryCrossCmpStep3_CURRENCYAMOUNT
    {
        public int ITEMNO_ACC { get; set; }
        public string CURRENCY { get; set; }
        public decimal AMT_DOCCUR { get; set; }
    }

    /* - Condition Search - */
    public class ConditionSearchInfo
    {
        // public string isActive { get; set; }

        public string PO_No { get; set; }
        public string GR_No { get; set; }
        public string MatGrpID { get; set; }
        public string MatID { get; set; }
        public string MatCode { get; set; }
        public string MatName { get; set; }
        public string DateBeg { get; set; }
        public string DateEnd { get; set; }
        public string ReqNo { get; set; }
        public string ReqRefNo { get; set; }
        public string ProjectId { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string Status { get; set; }
    }

}