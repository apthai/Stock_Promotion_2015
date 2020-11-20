using AP_StockPromotion_V1.Class;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AP_StockPromotion_V1.webpage
{
    public partial class MasterApproverPR : Page
    {
        DASAPConnector DASap = new DASAPConnector();
        DARequisition DAReq = new DARequisition();
        DAStockPromotion DAStp = new DAStockPromotion();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
                LoadddlExpense();
            }
        }

        protected void BindData()
        {
            DataTable dt = DAReq.GetPRExpenseMemoData("", 0, "");

            ViewState["dtgvData"] = dt;

            gvData.DataSource = dt;
            gvData.DataBind();

            //Response.Write("<script>alert('มาแล้วเด้อ');</script>");
        }

        protected void LoadddlExpense()
        {
            DataTable dt = DAReq.GetAllMasterExpense();

            ddlExpense.Items.Clear();

            ddlExpense.DataTextField = "Expense";
            ddlExpense.DataValueField = "ID";
            ddlExpense.DataSource = dt;
            ddlExpense.DataBind();
            ddlExpense.Items.Insert(0, new ListItem("-- ทั้งหมด --", "0"));
            ddlExpense.SelectedIndex = 0;
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

        protected void btnGetPRFromSAP_Click(object sender, EventArgs e)
        {
            var valid = true;
            DataTable dt = new DataTable();

            dt = DASap.SAP_GET_PRApprover();

            string msg = "";
            if (dt.Rows.Count > 0)
            {
                DataRow[] rows = dt.Select("SUBSTRING(BSART, 2, 3) = 'I01' AND KNTTP = 'K'");

                if (rows.Length > 0)
                {
                    DataTable dt2 = rows.CopyToDataTable();
                    valid = DAReq.InsertSAP_Approver_PR(dt2, ref msg);
                }
                else
                {
                    msg = "ไม่พบข้อมูล PR จาก SAP (ZRFCMM06 BSART = I01, KNTTP = K)";
                    valid = false;
                }
            }
            else
            {
                msg = "ไม่พบข้อมูล PR จาก SAP (ZRFCMM06)";
                valid = false;
            }

            if (valid)
            {
                msg = "บันทึกเรียบร้อย";
                //Response.Write("<script>alert('" + msg + "');</script>");
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('" + msg + "'); ", true);
            }
            else
            {
                msg = "บันทึกข้อมูลไม่สำเร็จ : " + msg;
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('" + msg + "'); ", true);

                dt = DAReq.GetPRExpenseMemoData(tbCostCenter.Text, int.Parse(ddlExpense.SelectedValue), tbApprover.Text);

                ViewState["dtgvData"] = dt;

                gvData.DataSource = dt;
                gvData.DataBind();
                upDetail.Update();
            }

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            DataTable dt = DAReq.GetPRExpenseMemoData(tbCostCenter.Text, int.Parse(ddlExpense.SelectedValue), tbApprover.Text);

            ViewState["dtgvData"] = dt;

            gvData.DataSource = dt;
            gvData.DataBind();
            upDetail.Update();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            tbCostCenter.Text = "";
            ddlExpense.SelectedIndex = 0;
            tbApprover.Text = "";
        }
    }
}