using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AP_StockPromotion_V1.webpage
{
    public partial class MasterItemGroup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                initPage();
                bindData();
            }
        }

        private void initPage()
        {
            Session["grdData"] = null; Session["grdSort"] = null; Session["grdPage"] = null;  
            //bindDDLItemCountMethod();
            //bindDDLItemStock();
            //bindDDLItemForceExpire();
        }

        //private void bindDDLItemCountMethod()
        //{
        //    Class.DAStockPromotion dasp = new Class.DAStockPromotion();
        //    DataTable dt = dasp.getDataStatus("1");
        //    ddlItemCountMethod.DataSource = dt;
        //    ddlItemCountMethod.DataTextField = "StatusText";
        //    ddlItemCountMethod.DataValueField = "StatusValue";
        //    ddlItemCountMethod.DataBind();
        //    ddlItemCountMethod.Items.Insert(0, new ListItem("ทั้งหมด", ""));
        //}

        //private void bindDDLItemStock()
        //{
        //    Class.DAStockPromotion dasp = new Class.DAStockPromotion();
        //    DataTable dt = dasp.getDataStatus("2");
        //    ddlItemStock.DataSource = dt;
        //    ddlItemStock.DataTextField = "StatusText";
        //    ddlItemStock.DataValueField = "StatusValue";
        //    ddlItemStock.DataBind();
        //    ddlItemStock.Items.Insert(0, new ListItem("ทั้งหมด", ""));
        //}

        //private void bindDDLItemForceExpire()
        //{
        //    Class.DAStockPromotion dasp = new Class.DAStockPromotion();
        //    DataTable dt = dasp.getDataStatus("15");
        //    ddlForceExpire.DataSource = dt;
        //    ddlForceExpire.DataTextField = "StatusText";
        //    ddlForceExpire.DataValueField = "StatusValue";
        //    ddlForceExpire.DataBind();
        //    ddlForceExpire.Items.Insert(0, new ListItem("ทั้งหมด", ""));
        //}
        
        private void bindData()
        {
            Session["grdSort"] = null; Session["grdPage"] = null;  
            Entities.MasterItemGroupInfo iGrp = new Entities.MasterItemGroupInfo();
            iGrp.ItemGroupName = txtGroupName.Text;
            //iGrp.ItemCountMethod = ddlItemCountMethod.SelectedItem.Value;
            //iGrp.ItemStock = ddlItemStock.SelectedItem.Value;
            //iGrp.ItemForceExpire = ddlForceExpire.SelectedItem.Value;
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            Session["grdData"] = dasp.getDataMasterItemGroup(iGrp);
            //DataTable dt = dasp.getDataMasterItemGroup(iGrp);
            //if (Session["grdSort"] + "" != "") { dt.DefaultView.Sort = Session["grdSort"] + ""; }
            //grdData.DataSource = dt.DefaultView;
            //grdData.DataBind();
            bindGrid();
        }

        private void bindGrid()
        {
            DataTable dt = (DataTable)Session["grdData"];
            if (Session["grdSort"] + "" != "") { dt.DefaultView.Sort = Session["grdSort"] + ""; }
            grdData.DataSource = dt.DefaultView;
            if (Session["grdPage"] + "" != "") { grdData.PageIndex = (int)Session["grdPage"]; }
            grdData.DataBind();
        }

        protected void grdData_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "MasterItemGroupEdit")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "OpenColorBox('MasterItemGroupDetail.aspx?mode=Edit&MasterItemGroupId=" + e.CommandArgument + "','75%','51%');", true);
                return;
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
            txtGroupName.Text = "";
        }

        protected void btnFancyBox_Click(object sender, EventArgs e)
        {

        }

        protected void btnAddMasterItemGroup_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "js", "OpenColorBox('MasterItemGroupDetail.aspx?mode=Create','75%','51%');", true);
            return;
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
    }
}