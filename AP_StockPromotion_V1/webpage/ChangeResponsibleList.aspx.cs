using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AP_StockPromotion_V1.webpage
{
    public partial class ChangeResponsibleList : System.Web.UI.Page
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
            bindDDLCurrResp();
            bindDDLNewResp();
        }

        private void bindDDLProject()
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            DataTable dt = dasp.getDataMasterProject();
            ddlProject.DataSource = dt;
            ddlProject.DataTextField = "ProjectName";
            ddlProject.DataValueField = "ProjectID";
            ddlProject.DataBind();
            ddlProject.Items.Insert(0, new ListItem("ทั้งหมด", "0"));
        }

        private void bindDDLCurrResp()
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            DataTable dt = dasp.getDataUser("", "");
            ddlCurResp.DataSource = dt;
            ddlCurResp.DataTextField = "FullNoName";
            ddlCurResp.DataValueField = "EmpCode";
            ddlCurResp.DataBind();
            ddlCurResp.Items.Insert(0, new ListItem("-- เลือก --", ""));
        }

        private void bindDDLNewResp()
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            DataTable dt = dasp.getDataUser("", "");
            ddlNewResp.DataSource = dt;
            ddlNewResp.DataTextField = "FullNoName";
            ddlNewResp.DataValueField = "EmpCode";
            ddlNewResp.DataBind();
            ddlNewResp.Items.Insert(0, new ListItem("-- เลือก --", ""));
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Class.DAChangeResponsible cls = new Class.DAChangeResponsible();
            DataTable dt = cls.getChangeResponsibleListHistory(ddlProject.SelectedItem.Value, ddlCurResp.SelectedItem.Value, ddlNewResp.SelectedItem.Value, txtDateBeg.Text, txtDateEnd.Text);
            grdData.DataSource = dt;
            grdData.DataBind();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ddlCurResp.SelectedIndex = 0;
            ddlNewResp.SelectedIndex = 0;
            ddlProject.SelectedIndex = 0;
            txtDateBeg.Text = "";
            txtDateEnd.Text = "";
            txtNewResponsibleUser.Text = "";
            txtOldResponsibleUser.Text = "";
        }

        protected void btnChangeResponsible_Click(object sender, EventArgs e)
        {
            //Response.Redirect("ChangeResponsibleDetail.aspx?mode=Create");
            ScriptManager.RegisterStartupScript(this, GetType(), "js", "OpenColorBox('ChangeResponsibleDetail.aspx?mode=Create" + "','90%','85%');", true);
            return;
        }

        protected void grdData_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "viewChangeResp")
            {
                // Response.Redirect("ChangeResponsibleDetail.aspx?mode=Edit&CRListId=" + e.CommandArgument);
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "OpenColorBox('ChangeResponsibleDetail.aspx?mode=Edit&CRListId=" + e.CommandArgument + "','90%','85%');", true);
                return;
            }
            else if (e.CommandName == "getFile")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "Popup80('frmOpenFile.aspx?filePath=" + "../FileUpload/" + e.CommandArgument + "');", true);
                return;
            }
        }


    }
}