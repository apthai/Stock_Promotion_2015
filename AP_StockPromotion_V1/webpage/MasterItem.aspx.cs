using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AP_StockPromotion_V1.webpage
{
    public partial class MasterItem : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                initPage();
            }
        }

        private void initPage()
        {
            Session["grdData"] = null; Session["grdSort"] = null; Session["grdPage"] = null;
            bindDDLItemCountMethod();
            bindDDLItemStock(); 
            bindDDLItemGroup();
            bindDDLItemStatus();
            bindDDLItemForceExpire();
            // bindData(); เปิดเข้ามา ยังไม่ bind data
            if (Request.QueryString["mode"] + "" == "Edit")
            {
                InitSearch();
            }
        }

        private void InitSearch()
        {
            txtItemNo.Text = Request.QueryString["ItemNo"] + "";
            bindData();
        }

        private void bindDDLItemForceExpire()
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            DataTable dt = dasp.getDataStatus("15");
            ddlForceExpire.DataSource = dt;
            ddlForceExpire.DataTextField = "StatusText";
            ddlForceExpire.DataValueField = "StatusValue";
            ddlForceExpire.DataBind();
            ddlForceExpire.Items.Insert(0, new ListItem("ทั้งหมด", ""));
        }        

        private void bindDDLItemCountMethod()
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            DataTable dt = dasp.getDataStatus("1");
            ddlItemCountMethod.DataSource = dt;
            ddlItemCountMethod.DataTextField = "StatusText";
            ddlItemCountMethod.DataValueField = "StatusValue";
            ddlItemCountMethod.DataBind();
            ddlItemCountMethod.Items.Insert(0, new ListItem("ทั้งหมด", ""));
        }

        private void bindDDLItemStock()
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            DataTable dt = dasp.getDataStatus("2");
            ddlItemStock.DataSource = dt;
            ddlItemStock.DataTextField = "StatusText";
            ddlItemStock.DataValueField = "StatusValue";
            ddlItemStock.DataBind();
            ddlItemStock.Items.Insert(0, new ListItem("ทั้งหมด", ""));
        }

        private void bindDDLItemGroup()
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            Entities.MasterItemGroupInfo itmGrp = new Entities.MasterItemGroupInfo();
            itmGrp.MasterItemGroupId = 0;
            itmGrp.ItemGroupName = "";
            DataTable dt = dasp.getDataMasterItemGroup(itmGrp);
            ddlItemGroup.DataSource = dt;
            ddlItemGroup.DataTextField = "MasterItemGroupName";
            ddlItemGroup.DataValueField = "MasterItemGroupId";
            ddlItemGroup.DataBind();
            ddlItemGroup.Items.Insert(0, new ListItem("ทั้งหมด", "0"));
        }

        private void bindDDLItemStatus()
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            DataTable dt = dasp.getDataStatus("3");
            ddlItemStatus.DataSource = dt;
            ddlItemStatus.DataTextField = "StatusText";
            ddlItemStatus.DataValueField = "StatusValue";
            ddlItemStatus.DataBind();
            ddlItemStatus.Items.Insert(0, new ListItem("ทั้งหมด", ""));
        }

        private void bindData()
        {
            Entities.MasterItemInfo item = new Entities.MasterItemInfo();
            int itmGrpId = 0;
            int.TryParse(ddlItemGroup.SelectedItem.Value, out itmGrpId);
            item.MasterItemGroupId = itmGrpId;
            item.ItemNo = txtItemNo.Text;
            item.ItemName = txtItemName.Text;
            decimal itemCostBeg = 0;
            decimal itemCostEnd = 9999999;
            decimal.TryParse(txtItemCostBeg.Text, out itemCostBeg);
            decimal.TryParse(txtItemCostEnd.Text, out itemCostEnd);
            if (itemCostEnd == 0) { itemCostEnd = 9999999; }
            item.ItemCostBeg = itemCostBeg;
            item.ItemCostEnd = itemCostEnd;
            item.ItemCountMethod = ddlItemCountMethod.SelectedItem.Value;
            item.ItemStock = ddlItemStock.SelectedItem.Value;
            item.ItemStatus = ddlItemStatus.SelectedItem.Value;

            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            Session["grdData"] = dasp.getDataMasterItem(item);
            bindGridData();

        }

        private void bindGridData()
        {
            DataTable dt = (DataTable)Session["grdData"] ?? new DataTable();
            if ((Session["grdSort"] ?? "").ToString() != "") {
                dt.DefaultView.Sort = Session["grdSort"] + "";
            }
            grdData.DataSource = dt.DefaultView;

            if ((Session["grdPage"] ?? "").ToString() != "") {
                grdData.PageIndex = (int)Session["grdPage"];
            }
            grdData.DataBind();
        }

        protected void grdData_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "MasterItemEdit")
            {
                // ScriptManager.RegisterStartupScript(this, GetType(), "js", "popupMasterItemDetail('"+ e.CommandArgument +"');", true);
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "OpenColorBox('MasterItemDetail.aspx?mode=Edit&MasterItemId=" + e.CommandArgument + "','90%','430px');", true);
                // return;
            }
        }

        protected void grdData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            bindData();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            clearFilter();
        }

        private void clearFilter()
        {
            txtItemNo.Text = "";
            txtItemName.Text = "";
            txtItemCostBeg.Text = "";
            txtItemCostEnd.Text = "";
            ddlItemCountMethod.SelectedIndex = 0;
            ddlItemStatus.SelectedIndex = 0;
            ddlItemStock.SelectedIndex = 0;            
        }

        protected void btnFancyBox_Click(object sender, EventArgs e)
        {
            
        }

        protected void btnSyncMaster_Click(object sender, EventArgs e)
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            dasp.SyncMasterDataItem();
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
            bindGridData();
        }

        protected void grdData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Session["grdPage"] = e.NewPageIndex;
            bindGridData();
        }
    }
}