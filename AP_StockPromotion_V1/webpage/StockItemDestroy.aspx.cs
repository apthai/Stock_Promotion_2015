using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace AP_StockPromotion_V1.webpage
{
    public partial class StockItemDestroy : System.Web.UI.Page
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
            bindDDLProject();
            bindDDLItemDestroy();
            // bindData();
        }

        private void bindDDLProject()
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            DataTable dt = dasp.getDataMasterProject();
            ddlProject.DataSource = dt;
            ddlProject.DataTextField = "ProjectName";
            ddlProject.DataValueField = "ProjectID";
            ddlProject.DataBind();
            ddlProject.Items.Insert(0, new ListItem("ทั้งหมด", ""));
            ddlProject.Items.Insert(1, new ListItem("สต๊อกกลาง", "0"));
        }

        private void bindDDLItemDestroy()
        {

            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            Entities.MasterItemInfo item = new Entities.MasterItemInfo();
            item.ItemCostEnd = 9999999;
            DataTable dt = dasp.getDataMasterItem(item);
            ddlItem.DataSource = dt;
            ddlItem.DataTextField = "ItemNoName";
            ddlItem.DataValueField = "MasterItemId";
            ddlItem.DataBind();
            ddlItem.Items.Insert(0, new ListItem("ทั้งหมด", "0"));
        }

        private void bindData()
        {
            //getDataStockItemProjectDestroyed 
            Class.DAStockItemDestroy cls = new Class.DAStockItemDestroy();
            int ProjectID = 0;
            int.TryParse(ddlProject.SelectedItem.Value, out ProjectID);
            int masteritem_id = 0;
            int.TryParse (ddlItem.SelectedItem.Value , out masteritem_id);
            DataTable dtDestroyedDetail = cls.getDataStockItemProjectDestroyed(ProjectID, masteritem_id);

            DataTable dtDestroyed = dtDestroyedDetail.DefaultView.ToTable(true, "DestroyListId", "ProjectID", "ProjectName", "MasterItemId", "ItemName", "CreateBy", "FullName", "CreateDate", "OBJ_KEY", "OBJKEY");
            dtDestroyed.Columns.Add(new DataColumn("DestroyAmount", typeof(int)));
            foreach (DataRow dr in dtDestroyed.Rows)
            {
                string cond = " 1=1 ";
                cond += " and DestroyListId = " + dr["DestroyListId"];
                if (dr["ProjectID"] == DBNull.Value)
                {
                    cond += " and ProjectID is null ";
                }
                else
                {
                    cond += " and ProjectID=" + dr["ProjectID"];
                }
                cond += " and MasterItemId = " + dr["MasterItemId"];
                dr["DestroyAmount"] = dtDestroyedDetail.Select(cond).Length;
            }
            dtDestroyed.AcceptChanges();
            //grdData.DataSource = dtDestroyed;
            //grdData.DataBind();            

            Session["grdData"] = dtDestroyed;
            bindGridData();
        }


        protected void btnDestroyItem_Click(object sender, EventArgs e)
        {
            Response.Redirect("StockItemDestroyEdit.aspx?mode=Create");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            bindData();
        }

        protected void grdData_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CancelDestroy")
            {
                Response.Redirect("StockItemDestroyedDetail.aspx?DestroyListId=" + e.CommandArgument);
            }
        }

        protected void grdData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Session["grdPage"] = e.NewPageIndex;
            bindGridData();
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

        private void bindGridData()
        {
            DataTable dt = (DataTable)Session["grdData"];
            if (Session["grdSort"] + "" != "") { dt.DefaultView.Sort = Session["grdSort"] + ""; }
            grdData.DataSource = dt.DefaultView;
            if (Session["grdPage"] + "" != "") { grdData.PageIndex = (int)Session["grdPage"]; }
            grdData.DataBind();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ddlItem.SelectedIndex = 0;
            ddlProject.SelectedIndex = 0;
        }


    }
}