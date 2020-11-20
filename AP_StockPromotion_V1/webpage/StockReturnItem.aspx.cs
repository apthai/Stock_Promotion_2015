using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AP_StockPromotion_V1.web
{
    public partial class StockReturnItem : System.Web.UI.Page
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
            bindDDLProject();
            bindDDLItem();
            // bindData();
        }

        private void bindDDLProject()
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            DataTable dt = dasp.getDataMasterProject(true);
            ddlProject.DataSource = dt;
            ddlProject.DataTextField = "ProjectName";
            ddlProject.DataValueField = "ProjectID";
            ddlProject.DataBind();
            ddlProject.Items.Insert(0, new ListItem("ทั้งหมด", "0"));
        }

        private void bindDDLItem() 
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            Entities.MasterItemInfo item = new Entities.MasterItemInfo();
            item.ItemCostEnd = 9999999;
            DataTable dt = dasp.getDataMasterItem(item);
            ddlItem.DataSource = dt;
            ddlItem.DataTextField = "ItemNoName";
            // ddlItem.DataValueField = "ItemNo";
            ddlItem.DataValueField = "MasterItemId";
            ddlItem.DataBind();
            ddlItem.Items.Insert(0, new ListItem("ทั้งหมด", "0"));
        }

        private void bindData()
        {
            Class.DAStockReturn cls = new Class.DAStockReturn();
            int projectId = 0;
            int itemId = 0;
            DateTime begDate;
            DateTime endDate;

            int.TryParse(ddlProject.SelectedItem.Value, out projectId);
            int.TryParse(ddlItem.SelectedItem.Value, out itemId);
            if (!DateTime.TryParse(txtDateFrom.Text, out begDate)) { begDate = DateTime.Now.AddYears(-25); }
            if (!DateTime.TryParse(txtDateTo.Text, out endDate)) { endDate = DateTime.Now.AddYears(1); }
            
            DataTable dt = cls.getDataReturned(projectId, itemId, begDate, endDate);
            DataTable dtList = dt.DefaultView.ToTable(true, "ReturnListId", "Project_Id", "ProjectName", "MasterItemId", "ItemName", "CreateDate", "CreateBy", "FullName", "ReturnReason");
            dtList.Columns.Add(new DataColumn("Amount", typeof(int)));
            foreach (DataRow dr in dtList.Rows)
            {
                dr["Amount"] = dt.Compute("Count(ReturnListId)", "ReturnListId=" + dr["ReturnListId"] + " and  MasterItemId=" + dr["MasterItemId"]);
            }
            //grdData.DataSource = dtList;
            //grdData.DataBind();

            Session["grdData"] = dtList;
            bindGridData();

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            bindData();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtDateFrom.Text = "";
            txtDateTo.Text = "";
            ddlItem.SelectedIndex = 0;
            ddlProject.SelectedIndex = 0;
        }

        protected void btnAddDataReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("StockReturnItemEdit.aspx");
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

        private void bindGridData()
        {
            DataTable dt = (DataTable)Session["grdData"];
            if (Session["grdSort"] + "" != "") { dt.DefaultView.Sort = Session["grdSort"] + ""; }
            grdData.DataSource = dt.DefaultView;
            if (Session["grdPage"] + "" != "") { grdData.PageIndex = (int)Session["grdPage"]; }
            grdData.DataBind();
        }


    }
}