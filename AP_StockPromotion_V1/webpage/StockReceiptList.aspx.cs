using AP_StockPromotion_V1.Class;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AP_StockPromotion_V1.web
{
    public partial class StockReceiptList : Page
    {
        string formatDate = new Entities.FormatDate().formatDate;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false) {
                initPage();
            }
        }

        private void initPage()
        {
            Session["grdDataReceipt"] = null; Session["grdSort"] = null; Session["grdPage"] = null;
            bindDDLItem();
            bindDDLItemGroup();
            // bindData();
        }

        private void bindDDLItem()
        {
            DAStockPromotion dasp = new DAStockPromotion();
            Entities.MasterItemInfo item = new Entities.MasterItemInfo();
            item.ItemCostEnd = 9999999;
            DataTable dt = dasp.getDataMasterItem(item);
            ddlItem.DataSource = dt;
            ddlItem.DataTextField = "ItemNoName";
            ddlItem.DataValueField = "MasterItemId";
            ddlItem.DataBind();
            ddlItem.Items.Insert(0, new ListItem("ทั้งหมด", "0"));
        }

        private void bindDDLItemGroup()
        {
            DAStockPromotion dasp = new DAStockPromotion();
            Entities.MasterItemGroupInfo item = new Entities.MasterItemGroupInfo();
            DataTable dt = dasp.getDataMasterItemGroup(item);
            ddlItemGroup.DataSource = dt;
            ddlItemGroup.DataTextField = "MasterItemGroupName";
            ddlItemGroup.DataValueField = "MasterItemGroupId";
            ddlItemGroup.DataBind();
            ddlItemGroup.Items.Insert(0, new ListItem("ทั้งหมด", "0"));
        }

        private void bindData()
        {
            Session["grdDataReceipt"] = null; Session["grdSort"] = null; Session["grdPage"] = null;
            Entities.StockReceiveInfo rc = new Entities.StockReceiveInfo();
            DAStockReceive dasp = new DAStockReceive();
            rc.PO_No = txtPO.Text.Trim();
            rc.GR_No = txtGR.Text.Trim();
            int itemId = 0;
            if (int.TryParse(ddlItem.SelectedItem.Value, out itemId))
            {
                rc.MasterItemId = itemId;
            }
            int itemGroupId = 0;
            if (int.TryParse(ddlItemGroup.SelectedItem.Value, out itemGroupId))
            {
                rc.MasterItemGroupId = itemGroupId;
            }
            DateTime datefrom;
            if (DateTime.TryParse(txtDateFrom.Text, out datefrom))
            {
                rc.CreateDateFrom = txtDateFrom.Text;
            }
            else
            {
                rc.CreateDateFrom = DateTime.Now.AddYears(-5).ToString(formatDate);
            }
            DateTime dateTo;            
            if (DateTime.TryParse(txtDateTo.Text, out dateTo))
            {
                rc.CreateDateTo = txtDateTo.Text;
            }
            else
            {
                rc.CreateDateTo = DateTime.Now.ToString(formatDate);
            }
            Session["grdDataReceipt"] = dasp.getDataReceiveHistory(rc);
            bindGrid();

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            bindData();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtPO.Text = "";
            txtGR.Text = "";
            ddlItem.SelectedIndex = 0;//            txtReceiveBy.Text = "";//
            ddlUser.SelectedIndex = 0; // txtReceiveBy.Text = "";
            txtDateFrom.Text = "";
            txtDateTo.Text = "";
            bindData();
        }

        protected void grdData_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditItem")
            {
                GridViewRow gvr = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                HiddenField hdfReceiveHeaderID = (HiddenField)gvr.FindControl("hdfReceiveHeaderID");
                HiddenField hdfPO_No = (HiddenField)gvr.FindControl("hdfPO_No");
                HiddenField hdfGR_No = (HiddenField)gvr.FindControl("hdfGR_No");
                HiddenField hdfGR_Year = (HiddenField)gvr.FindControl("hdfGR_Year");
                HiddenField hdfVendor = (HiddenField)gvr.FindControl("hdfVendor");
                HiddenField hdfCreateDate = (HiddenField)gvr.FindControl("hdfCreateDate");
                HiddenField hdfCreateBy = (HiddenField)gvr.FindControl("hdfCreateBy");
                HiddenField hdfReceiveHeaderStatus = (HiddenField)gvr.FindControl("hdfReceiveHeaderStatus");
                HiddenField hdfReceiveDetailId = (HiddenField)gvr.FindControl("hdfReceiveDetailId");
                HiddenField hdfPricePerUnit = (HiddenField)gvr.FindControl("hdfPricePerUnit");
                HiddenField hdfReceiveAmount = (HiddenField)gvr.FindControl("hdfReceiveAmount");
                HiddenField hdfStatus = (HiddenField)gvr.FindControl("hdfStatus");
                HiddenField hdfMasterItemId = (HiddenField)gvr.FindControl("hdfMasterItemId");
                HiddenField hdfItemNo = (HiddenField)gvr.FindControl("hdfItemNo");
                HiddenField hdfItemName = (HiddenField)gvr.FindControl("hdfItemName");
                HiddenField hdfItemUnit = (HiddenField)gvr.FindControl("hdfItemUnit");
                HiddenField hdfItemStatus = (HiddenField)gvr.FindControl("hdfItemStatus");
                
                // Response.Redirect("StockReceiveDetail.aspx?mode=FullFill&receiveDetailId=" + hdfReceiveDetailId.Value);

                string url = "StockReceiveDetail.aspx?mode=FullFill&receiveDetailId=" + hdfReceiveDetailId.Value;
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "OpenColorBox('" + url + "','90%','90%');", true);
            }
        }

        protected void grdData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow){
                ImageButton imb = (ImageButton)e.Row.FindControl("imgEditItem");
                HiddenField hdfStatus = (HiddenField)e.Row.FindControl("hdfStatus");
            }
        }
        
        private void bindGrid()
        {
            DataTable dt = (DataTable)Session["grdDataReceipt"];
            if (Session["grdSort"] + "" != "") { dt.DefaultView.Sort = Session["grdSort"] + ""; }
            grdData.DataSource = dt.DefaultView;
            if (Session["grdPage"] + "" != "") { grdData.PageIndex = (int)Session["grdPage"]; }
            grdData.DataBind();
        }

        protected void grdData_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Session["grdSort"] + "" == e.SortExpression)
            {
                Session["grdSort"] = e.SortExpression + " desc ";
            }
            else
            {
                Session["grdSort"] = e.SortExpression;
            }
            bindGrid();
        }

        protected void grdData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Session["grdPage"] = e.NewPageIndex;
            bindGrid();
        }

        
    }
}