using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AP_StockPromotion_V1.web
{
    public partial class StockTransferItem_bk20150422 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                initPage();
                bindGrid();
            }
        }

        private void initPage()
        {
            bindDDLReqStatus();
            bindDDLProject();
            bindDDLItem();
        }

        private void bindDDLReqStatus()
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            DataTable dt = dasp.getDataStatus("8");
            ddlReqStatus.DataSource = dt;
            ddlReqStatus.DataTextField = "StatusText";
            ddlReqStatus.DataValueField = "StatusValue";
            ddlReqStatus.DataBind();
            ddlReqStatus.Items.Insert(0, new ListItem("ไม่ระบุ", ""));
        }

        private void bindDDLProject()
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            DataTable dt = dasp.getDataMasterProject();
            ddlProject.DataSource = dt;
            ddlProject.DataTextField = "ProjectName";
            ddlProject.DataValueField = "Project_Id";
            ddlProject.DataBind();
            ddlProject.Items.Insert(0, new ListItem("ไม่ระบุ", "0"));
        }

        private void bindDDLItem()
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            Entities.MasterItemInfo item = new Entities.MasterItemInfo();
            item.ItemCostEnd = 9999999;
            DataTable dt = dasp.getDataMasterItem(item);
            ddlItem.DataSource = dt;
            ddlItem.DataTextField = "ItemName";
            // ddlItem.DataValueField = "ItemNo";
            ddlItem.DataValueField = "MasterItemId";
            ddlItem.DataBind();
            ddlItem.Items.Insert(0, new ListItem("ไม่ระบุ", "0"));
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            bindGrid();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {

        }

        private void bindGrid(int pageIndex = 0)
        {
            Class.DATransferToProject cls_req = new Class.DATransferToProject();
            Entities.RequisitionInfo req = new Entities.RequisitionInfo();
            req.ReqHeaderId = 0; // จะเอาหมด
            req.ReqNo = txtReqNo.Text;
            DateTime dFrom = new DateTime();
            if (!DateTime.TryParse(txtDateFrom.Text, out dFrom))
            {
                dFrom = new DateTime(2000, 01, 01);
            }
            DateTime dTo = new DateTime();
            if (!DateTime.TryParse(txtDateTo.Text, out dTo))
            {
                dTo = new DateTime(3500, 12, 31);
            }
            req.ReqDateFrom = dFrom.ToString();
            req.ReqDateTo = dTo.ToString();
            req.Project_Id = Convert.ToInt32(ddlProject.SelectedItem.Value);
            req.ItemId = Convert.ToInt32(ddlItem.SelectedItem.Value);

            DataTable dt = cls_req.getDataRequestDetail(req);
            grdData.DataSource = dt;
            grdData.PageIndex = pageIndex;
            grdData.DataBind();
        }

        protected void grdData_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "takeReq")
            {
                //ScriptManager.RegisterStartupScript(this, GetType(), "js", "popupStockTransfrtItemEdit('" + e.CommandArgument + "');", true);
                GridViewRow gvr = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                string reqHeaderId = ((HiddenField)gvr.FindControl("grdHdfReqHeaderId")).Value;
                string reqProject = ((HiddenField)gvr.FindControl("grdHdfProject_Id")).Value;
                Response.Redirect("StockTransferItemDetail.aspx?mode=Edit&reqHeaderId=" + reqHeaderId + "&reqProject=" + reqProject);
            }
        }

        protected void grdData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lbTotalAmount = (Label)e.Row.FindControl("lbTotalAmount");
                HiddenField grdHdfItemPricePerUnit = (HiddenField)e.Row.FindControl("grdHdfItemPricePerUnit");
                HiddenField grdHdfReqAmount = (HiddenField)e.Row.FindControl("grdHdfReqAmount");

                decimal price = 0;
                decimal amount = 0;

                decimal.TryParse(grdHdfItemPricePerUnit.Value, out price);
                decimal.TryParse(grdHdfReqAmount.Value, out amount);

                lbTotalAmount.Text = (price * amount).ToString("#,##0.00");
            }
        }

        protected void grdData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            bindGrid(e.NewPageIndex);
        }
    }
}