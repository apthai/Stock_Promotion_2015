using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AP_StockPromotion_V1.web
{
    public partial class Requisition : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                initPage();
                // bindData();
            }
        }

        private void initPage()
        {
            Session["grdData"] = null; Session["grdSort"] = null; Session["grdPage"] = null;
            bindDDLReqStatus();
            bindDDLProject();
            bindDDLItem();

            if (Request.QueryString["bindData"] + "" == "Y")
            {
                txtReqDocNo.Text = Request.QueryString["ReqDocNo"] + "";
                bindData();
            }
            else if (Request.QueryString["sCond"] + "" != "" && Session["sCond"] != null)
            {
                Entities.ConditionSearchInfo sCond = (Entities.ConditionSearchInfo)Session["sCond"];
                txtReqNo.Text = sCond.PO_No;
                txtReqDocNo.Text = sCond.ReqRefNo;
                txtDateFrom.Text = sCond.DateBeg;
                sCond.DateEnd = txtDateTo.Text;
                ddlProject.SelectedIndex = ddlProject.Items.IndexOf(ddlProject.Items.FindByValue(sCond.ProjectId));
                ddlItem.SelectedIndex = ddlItem.Items.IndexOf(ddlItem.Items.FindByValue(sCond.MatID));
                ddlReqStatus.SelectedIndex = ddlReqStatus.Items.IndexOf(ddlReqStatus.Items.FindByValue(sCond.Status));
                bindData();
            }

        }

        private void bindDDLReqStatus()
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            DataTable dt = dasp.getDataStatus("8");
            ddlReqStatus.DataSource = dt;
            ddlReqStatus.DataTextField = "StatusText";
            ddlReqStatus.DataValueField = "StatusValue";
            ddlReqStatus.DataBind();
            ddlReqStatus.Items.Insert(0, new ListItem("ทั้งหมด", ""));
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            bindData();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtReqNo.Text = "";
            txtReqDocNo.Text = "";
            txtDateFrom.Text = "";
            txtDateTo.Text = "";
            ddlItem.SelectedIndex = 0;
            ddlProject.SelectedIndex = 0;
            ddlReqStatus.SelectedIndex = 0;
            chkFindDelRequestCRM.Checked = false;
        }

        private void keepConditionSearch()
        {
            Entities.ConditionSearchInfo sCond = new Entities.ConditionSearchInfo();
            sCond.PO_No = txtReqNo.Text;
            sCond.ReqRefNo = txtReqDocNo.Text;
            sCond.DateBeg = txtDateFrom.Text;
            sCond.DateEnd = txtDateTo.Text;
            sCond.ProjectId = ddlProject.SelectedItem.Value;
            sCond.MatID = ddlItem.SelectedItem.Value;
            sCond.Status = ddlReqStatus.SelectedItem.Value;
            Session["sCond"] = sCond;
        }

        private void bindData()
        {
            keepConditionSearch();
            Class.DARequisition cls_req = new Class.DARequisition();
            Entities.RequisitionInfo req = new Entities.RequisitionInfo();
            req.ReqHeaderId	 = 0; // จะเอาหมด
            req.ReqNo = txtReqNo.Text;
            DateTime dFrom = new DateTime();
            string formatDate = new Entities.FormatDate().formatDate;
            if (!DateTime.TryParse(txtDateFrom.Text, out dFrom))
            {
                req.ReqDateFrom = DateTime.Now.AddYears(-5).ToString(formatDate);
            }
            else
            {
                req.ReqDateFrom = txtDateFrom.Text;
            }
            DateTime dTo = new DateTime();
            if (!DateTime.TryParse(txtDateTo.Text, out dTo))
            {
                req.ReqDateTo = DateTime.Now.AddYears(5).ToString(formatDate);
            }
            else
            {
                req.ReqDateTo = txtDateTo.Text;
            }
            req.ReqDocNo = txtReqDocNo.Text;
            req.Project_Id = Convert.ToInt32(ddlProject.SelectedItem.Value);
            req.ItemId = Convert.ToInt32(ddlItem.SelectedItem.Value);


            DataTable dt;//= cls_req.getDataRequest(req);

            if (!chkFindDelRequestCRM.Checked)
            {
                req.WithCRMData = "N";
                dt = cls_req.getDataRequest(req, 0);
            }
            else
            {
                req.WithCRMData = "Y";
                dt = cls_req.getDataRequest(req, 0);
            }

            dt.DefaultView.RowFilter = "ReqStatus = '" + ddlReqStatus.SelectedItem.Value + "' OR '" + ddlReqStatus.SelectedItem.Value + "' = ''";
            dt = dt.DefaultView.ToTable();
            dt.DefaultView.RowFilter = "";
            // grdData.DataSource = dt.DefaultView.ToTable(true, "ReqHeaderId", "ReqNo", "ReqDate", "ReqBy", "ReqType", "ReqHeaderRemark", "ReqId", "Project_Id", "ProjectID", "ProjectName", "ProStartDate", "ProEndDate", "ReqStatus", "ReqStatusText");


            Session["grdData"] = dt.DefaultView.ToTable(true, "ReqHeaderId", "ReqDocNo", "ReqNo", "ReqDocDate", "ReqDate", "ReqBy", "FullName", "ReqType", "ReqHeaderRemark", "Project_Id", "ProjectName", "ProStartDate", "ProEndDate", "ReqStatus", "ReqStatusText");
            bindGrid();
            //grdData.DataSource = dt.DefaultView.ToTable(true, "ReqHeaderId", "ReqDocNo", "ReqNo", "ReqDocDate", "ReqDate", "ReqBy", "FullName", "ReqType", "ReqHeaderRemark", "Project_Id", "ProjectName", "ProStartDate", "ProEndDate", "ReqStatus", "ReqStatusText");

            //grdData.DataBind();
        }

        protected void grdData_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "editReq")
            {
                Response.Redirect("RequisitionEdit.aspx?mode=Edit&reqId=" + e.CommandArgument);
                //ScriptManager.RegisterStartupScript(this, GetType(), "js", "popupRequisitionEdit('" + e.CommandArgument + "');", true);
            }
        }

        protected void grdData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lbStatusText = (Label)e.Row.FindControl("grdLbStatusText");
                HiddenField hdfReqStatus = (HiddenField)e.Row.FindControl("grdHdfReqStatus");
                if (hdfReqStatus.Value == "1")
                {
                    lbStatusText.ForeColor = System.Drawing.Color.Red;
                }
                if (hdfReqStatus.Value == "2")
                {
                    lbStatusText.ForeColor = System.Drawing.Color.Blue;
                }
                if (hdfReqStatus.Value == "3")
                {
                    lbStatusText.ForeColor = System.Drawing.Color.Green;
                }
            }
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
                //Label lbTotalAmount = (Label)e.Row.FindControl("lbTotalAmount");
                //HiddenField grdHdfItemPricePerUnit = (HiddenField)e.Row.FindControl("grdHdfItemPricePerUnit");
                //HiddenField grdHdfReqAmount = (HiddenField)e.Row.FindControl("grdHdfReqAmount");
                
                //decimal price = 0;
                //decimal amount = 0;

                //decimal.TryParse(grdHdfItemPricePerUnit.Value, out price);
                //decimal.TryParse(grdHdfReqAmount.Value, out amount);

                //lbTotalAmount.Text = (price * amount).ToString("#,##0.00");
            //}
        }

        protected void btnAddRequest_Click(object sender, EventArgs e)
        {
            Response.Redirect("RequisitionEdit.aspx?mode=Create");
        }

        private void bindGrid()
        {
            DataTable dt = (DataTable)Session["grdData"];
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