using AP_StockPromotion_V1.Class;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AP_StockPromotion_V1.webpage
{
    public partial class MasterInternalOrder : Page
    {
        DASAPConnector DASap = new DASAPConnector();
        DARequisition DAReq = new DARequisition();
        DAStockPromotion DAStp = new DAStockPromotion();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }

        protected void BindData()
        {
            DataTable dt = DAReq.GetInternalOrder("", "", null);

            ViewState["dtgvData"] = dt;

            gvData.DataSource = dt;
            gvData.DataBind();
        }

        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            ViewState["gvPageIndex"] = e.NewPageIndex;
            gvData.PageIndex = e.NewPageIndex;

            DataTable dt = ViewState["dtgvData"] != null ? (DataTable)ViewState["dtgvData"] : new DataTable();

            dt.DefaultView.Sort = ViewState["gvSort"] != null ? ViewState["gvSort"].ToString() : "";

            gvData.DataSource = dt;
            gvData.DataBind();
        }

        protected void OnSorting(object sender, GridViewSortEventArgs e)
        {
            //DataTable dt = gvData.DataSource as DataTable;

            DataTable dt = ViewState["dtgvData"] != null ? (DataTable)ViewState["dtgvData"] : new DataTable();

            string gvSort = e.SortExpression // column name
                + " " + SortDir(e.SortExpression); // sort direction
            ViewState["gvSort"] = gvSort;
            dt.DefaultView.Sort = gvSort;

            gvData.PageIndex = ViewState["gvPageIndex"] != null ? (int)ViewState["gvPageIndex"] : 0;

            gvData.DataSource = dt;
            gvData.DataBind();
        }

        private string SortDir(string sColumn)
        {
            string sDir = "asc"; // ascending by default
            string sPreviousColumnSorted = ViewState["SortColumn"] != null ? ViewState["SortColumn"].ToString() : "";

            if (sPreviousColumnSorted == sColumn) // same column clicked? revert sort direction
                sDir = ViewState["SortDir"].ToString() == "asc" ? "desc" : "asc";
            else
            {
                ViewState["SortColumn"] = sColumn; // store current column clicked
            }

            ViewState["SortDir"] = sDir; // store current direction

            return sDir;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string _msg = "";
            DataTable dt = DAReq.GetInternalOrder(tbInternalOrder.Text, tbDescription.Text, null);

            ViewState["dtgvData"] = dt;

            gvData.DataSource = dt;
            gvData.DataBind();
            upDetail.Update();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            tbInternalOrder.Text = "";
            tbDescription.Text = "";

            BindData();
            upDetail.Update();
        }

        protected void gvData_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string _msg = "";
            if (e.CommandName == "Delete")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                GridViewRow row = gvData.Rows[rowIndex];

                string ID = (row.FindControl("hdID") as HiddenField).Value;

                if (DAReq.DeleteInternalOrderByID(int.Parse(ID), ref _msg))
                {
                    _msg = "ลบข้อมูลเรียบร้อย";
                }
                else
                {
                    _msg = "ลบข้อมูลไม่สำเร็จ " + _msg;
                }

                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('" + _msg + "');", true);
            }

            if (e.CommandName == "Add")
            {
                string NewInternalOrder = (gvData.HeaderRow.FindControl("tbAddInternalOrder") as TextBox).Text;
                string NewDescription = (gvData.HeaderRow.FindControl("tbAddDescription") as TextBox).Text;
                bool cbIsActive = (gvData.HeaderRow.FindControl("cbAddIsActive") as CheckBox).Checked;

                int IsActive = cbIsActive ? 1 : 0;

                if (DAReq.InsertInternalOrderByID(NewInternalOrder, NewDescription, IsActive, ref _msg))
                {
                    _msg = "เพิ่มข้อมูลเรียบร้อยแล้ว";
                    BindData();
                    upDetail.Update();
                }
                else
                {
                    _msg = "เพิ่มข้อมูลไม่สำเร็จ " + _msg;
                }

                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('" + _msg + "');", true);
            }

        }

        protected void gvData_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            BindData();
            upDetail.Update();
        }
    }
}