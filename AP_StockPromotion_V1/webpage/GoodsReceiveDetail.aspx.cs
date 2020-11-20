using AP_StockPromotion_V1.ws_authorize;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AP_StockPromotion_V1.webpage
{
    public partial class GoodsReceiveDetail : System.Web.UI.Page
    {
        string formatDate = new Entities.FormatDate().formatDate;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                Session["grdDataX"] = null; Session["grdSortX"] = null; Session["grdPageX"] = null;
                string GR_No = Request.QueryString["GR_No"] + "";
                lbGRNo.Text = GR_No;
                bindData();
            }
        }

        private void bindData()
        {
            Entities.StockReceiveInfo rc = new Entities.StockReceiveInfo();
            Class.DAStockReceive dasp = new Class.DAStockReceive();
            rc.GR_No = lbGRNo.Text;
            rc.CreateDateFrom = DateTime.Now.AddYears(-5).ToString(formatDate);
            rc.CreateDateTo = DateTime.Now.AddYears(5).ToString(formatDate);
            DataTable dt = dasp.getDataReceiveHistory(rc);
            Session["grdDataX"] = dt;
            bindGrid();
            //grdData.DataSource = dt;
            //grdData.DataBind();
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    Entities.FormatDate convertDate = new Entities.FormatDate();
                    DataRow dr = dt.Rows[0];
                    lbPO.Text = dr["PO_No"] + "";
                    lbDeliveryNote.Text = dr["DocRefNo"] + "";
                    lbVendor.Text = dr["Vendor"] + "";
                    string postingDate = ""; convertDate.getStringFromDate(dr["PostingDate"], ref postingDate);
                    lbPostingDate.Text = postingDate;
                    hdfReceiveHeaderID.Value = dr["ReceiveHeaderID"] + "";
                    hdfGR_Year.Value = dr["GR_Year"] + "";
                    if (dr["CancelAble"] + "" != "Y")
                    {
                        btnCancelGR.Attributes.Add("style", "display:none;");
                    }
                }
            }
        }

        private void bindGrid()
        {
            DataTable dt = (DataTable)Session["grdDataX"];
            if (Session["grdSortX"] + "" != "") { dt.DefaultView.Sort = Session["grdSortX"] + ""; }
            grdData.DataSource = dt.DefaultView;
            if (Session["grdPageX"] + "" != "") { grdData.PageIndex = (int)Session["grdPageX"]; }
            grdData.DataBind();
        }

        protected void grdData_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Session["grdSortX"] + "" == e.SortExpression)
            {
                Session["grdSortX"] = e.SortExpression + " desc ";
            }
            else
            {
                Session["grdSortX"] = e.SortExpression;
            }
            bindGrid();
        }

        protected void grdData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Session["grdPageX"] = e.NewPageIndex;
            bindGrid();
        }

        protected void btnCancelGR_Click(object sender, EventArgs e)
        {
            AutorizeData auth = (AutorizeData)Session["userInfo_" + Session.SessionID];
            Entities.StockReceiveInfo rec = new Entities.StockReceiveInfo();
            Int64 ReceiveHeaderId = 0;
            Int64.TryParse(hdfReceiveHeaderID.Value, out ReceiveHeaderId);
            string recHID = hdfReceiveHeaderID.Value;
            long RecHID = 0;
            Int64.TryParse(recHID, out RecHID);
            rec.ReceiveHeaderId = RecHID;
            rec.PO_No = lbPO.Text;
            rec.GR_No = lbGRNo.Text;
            rec.GR_Year = hdfGR_Year.Value;
            rec.UpdateBy = auth.EmployeeID;
            string msgErr = "";
            Class.DASAPConnector sap = new Class.DASAPConnector();
            if (sap.cancelGoodsReceipt(rec, ref msgErr))
            {
                // alert : Completed
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ยกเลิกรายการรับสินค้าเสร็จสิ้น'); bindDataParentPage();", true);
                return;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('" + msgErr + " !!');", true);
                return;
            }
        }
    }
}