using AP_StockPromotion_V1.ws_authorize;
using AP_StockPromotion_V1.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AP_StockPromotion_V1.webpage
{
    public partial class DeliveryListEdit : System.Web.UI.Page
    {
        Entities.FormatDate convertDate = new Entities.FormatDate();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                    initPageEdit();
            }
        }

        private void initPageEdit()
        {
            string delvNo = Request.QueryString["delvNo"] + "";
            DADelivery cls = new DADelivery();
            DataTable dt = cls.getDataDeliveryList(delvNo, "", "", "", "", "");
            Session["dtDeliveryList"] = dt;
            bindDataDelivery(delvNo);

            string mode = Request.QueryString["mode"] + ""; // Edit
            if (mode == "View") { btnPostAccount.Attributes.Add("style", "display:none;"); }
        }


        private void bindDataDelivery(string  delvNo)
        {
            

            string mode = Request.QueryString["mode"] + ""; // Edit
            string itemNo = Request.QueryString["itemNo"] + ""; // Edit
            DataTable dtDelivery = (DataTable)Session["dtDeliveryList"];
            if (mode == "Edit")
            {
                dtDelivery.DefaultView.RowFilter = "isnull(DeliveryNo,'')='" + delvNo + "'";
            }
            else if (mode == "View")
            {
                dtDelivery.DefaultView.RowFilter = "isnull(DeliveryNo,'')='" + delvNo + "' and isnull(ItemNo,'') = '" + itemNo + "'";
            }
            DataTable dt = dtDelivery.DefaultView.ToTable();

            DataTable dtGrd = dt.DefaultView.ToTable(true, "DeliveryNo", "DeliveryDate", "WBS", "ProjectName", "isPostAcc", "PostingStatus");
            dtGrd.Columns.Add(new DataColumn("Total", typeof(int)));
            foreach (DataRow dr in dtGrd.Rows)
            {
                string cond = "DeliveryNo " + (dr["DeliveryNo"] != DBNull.Value ? " = '" + dr["DeliveryNo"] + "'" : " is null ");
                cond += " and DeliveryDate " + (dr["DeliveryDate"] != DBNull.Value ? " = '" + dr["DeliveryDate"] + "'" : " is null ");
                cond += " and WBS " + (dr["WBS"] != DBNull.Value ? " = '" + dr["WBS"] + "'" : " is null ");
                cond += " and ProjectName " + (dr["ProjectName"] != DBNull.Value ? " = '" + dr["ProjectName"] + "'" : " is null ");
                cond += " and PostingStatus " + (dr["PostingStatus"] != DBNull.Value ? " = '" + dr["PostingStatus"] + "'" : " is null ");
                dr["Total"] = dt.Compute("SUM(TotalAmount)", cond);
            }
            string delvDate = "";
            convertDate.getStringFromDate(dtGrd.Rows[0]["DeliveryDate"], ref delvDate);
            txtDeliveryNo.Text = dtGrd.Rows[0]["DeliveryNo"] + "";
            if (convertDate.getStringFromDate(dtGrd.Rows[0]["DeliveryDate"], ref delvDate))
            {
                txtDeliveryDate.Text = delvDate;
            }
            txtProjectName.Text = dtGrd.Rows[0]["ProjectName"] + "";
            txtWBS.Text = dtGrd.Rows[0]["WBS"] + "";
            txtPostingStatus.Text = dtGrd.Rows[0]["PostingStatus"] + "";
            txtTotal.Text = dtGrd.Rows[0]["Total"] + "";
            if (dtGrd.Rows[0]["isPostAcc"] + "" != "1") { btnPostAccount.Attributes.Add("style", "display:none;"); }
            grdData.DataSource = dt;
            grdData.DataBind();
            dtDelivery.DefaultView.RowFilter = "";
        }

        protected void btnPostAccount_Click(object sender, EventArgs e)
        {
            string delvNo = Request.QueryString["delvNo"] + "";
            DataTable dtDelivery = (DataTable)Session["dtDeliveryList"];
            dtDelivery.DefaultView.RowFilter = "isnull(DeliveryNo,'')='" + delvNo + "'";
            DataTable dt = dtDelivery.DefaultView.ToTable();
            dtDelivery.DefaultView.RowFilter = "";
            Entities.SapDelivery_DOCUMENTHEADER dochdr = new Entities.SapDelivery_DOCUMENTHEADER();
            List<Entities.SapDelivery_ACCOUNTGL> lstAccGL = new List<Entities.SapDelivery_ACCOUNTGL>();
            List<Entities.SapDelivery_CURRENCYAMOUNT> lstCurAmt = new List<Entities.SapDelivery_CURRENCYAMOUNT>();
            Entities.SapDelivery_ACCOUNTGL AccGL;
            Entities.SapDelivery_CURRENCYAMOUNT CurAmt;

            /* - SapDelivery_DOCUMENTHEADER - */
            AutorizeData auth = (AutorizeData)Session["userInfo_" + Session.SessionID];
            dochdr.USERNAME = auth.EmployeeID;
            dochdr.COMP_CODE = "1000";
            string delvDate = "";
            if (convertDate.getStringFromDate(dt.Rows[0]["DeliveryDate"], ref  delvDate))
            {
                dochdr.DOC_DATE = delvDate;
            }
            else { dochdr.DOC_DATE = ""; }
            dochdr.PSTNG_DATE = DateTime.Now.ToString(convertDate.formatDate);
            dochdr.FISC_YEAR = DateTime.Now.Year;
            dochdr.DOC_TYPE = "SA";

            int ii = 0;
            foreach (DataRow dr in dt.Rows)
            {
                ii++;
                AccGL = new Entities.SapDelivery_ACCOUNTGL(); // Debit
                AccGL.ITEMNO_ACC = ii;
                AccGL.GL_ACCOUNT = "0006011020";
                AccGL.ITEM_TEXT = dr["ItemName"] + ""; // "ตัด Stock ประจำเดือน พค.58";
                AccGL.REF_KEY_2 = "P" + dr["CostCenter"] + "";
                AccGL.COSTCENTER = "00000" + dr["CostCenter"] + "";
                AccGL.WBS_ELEMENT = dr["WBS"] + "";
                lstAccGL.Add(AccGL);
                
                CurAmt = new Entities.SapDelivery_CURRENCYAMOUNT();
                CurAmt.ITEMNO_ACC = ii; // 	NUMC	10	1
                CurAmt.CURRENCY = "THB"; // CUKY	3	THB
                decimal TotalAmount = 0;
                decimal.TryParse(dr["TotalAmount"] + "", out TotalAmount);
                CurAmt.AMT_DOCCUR = TotalAmount;
                lstCurAmt.Add(CurAmt);
                // [ชื่อ]
                ii++;
                AccGL = new Entities.SapDelivery_ACCOUNTGL(); // Credit
                AccGL.ITEMNO_ACC = ii;
                AccGL.GL_ACCOUNT = "0001580010";
                AccGL.ITEM_TEXT = dr["ItemName"] + ""; // "ตัด Stock ประจำเดือน พค.58"; 
                AccGL.PROFIT_CTR = "P" + dr["CostCenter"] + "";
                lstAccGL.Add(AccGL);

                CurAmt = new Entities.SapDelivery_CURRENCYAMOUNT();
                CurAmt.ITEMNO_ACC = ii;
                CurAmt.CURRENCY = "THB";
                CurAmt.AMT_DOCCUR = TotalAmount * (-1);
                lstCurAmt.Add(CurAmt);
            }

            string msgErr = "";
            string SAPDOCNO = "";

            DASAPConnector dasap = new DASAPConnector();
            DAStockPromotion dastk = new DAStockPromotion();

            string[] delvNoList = delvNo.Split(';');
            
            bool rstPost = dasap.deliveryPostAccount(delvNo, dochdr, lstAccGL, lstCurAmt, auth.EmployeeID, ref SAPDOCNO, ref msgErr);
            if (rstPost)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('บันทึกบัญชีเสร็จสิ้น " + SAPDOCNO + "'); bindDataParentPage();", true);
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