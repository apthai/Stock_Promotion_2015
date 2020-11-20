using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AP_StockPromotion_V1.webpage
{
    public partial class ReportStockBalanceOnSiteFull : System.Web.UI.Page
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
            bindDDLUser();
            bindChkMatType();
            txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }
        private void bindDDLProject()
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            DataTable dt = dasp.getDataMasterProject(true);
            ddlProject.DataSource = dt;
            ddlProject.DataTextField = "ProjectName";
            ddlProject.DataValueField = "ProjectCode";
            ddlProject.DataBind();
            ddlProject.Items.Insert(0, new ListItem("-- ทั้งหมด --", ""));
        }
        private void bindDDLUser()
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            DataTable dt = dasp.getDataUser("", "");
            ddlUser.DataSource = dt;
            ddlUser.DataTextField = "FullNoName";
            ddlUser.DataValueField = "EmpCode";
            ddlUser.DataBind();
            ddlUser.Items.Insert(0, new ListItem("-- ทั้งหมด --", ""));
        }

        private void bindChkMatType()
        {
            Class.DAStockPromotion cls = new Class.DAStockPromotion();
            Entities.MasterItemGroupInfo mit = new Entities.MasterItemGroupInfo();
            mit.MasterItemGroupId = 0;
            mit.ItemGroupName = "";
            DataTable dt = cls.getDataMasterItemGroup(mit);
            chkMatType.DataSource = dt;
            chkMatType.DataTextField = "MasterItemGroupName";
            chkMatType.DataValueField = "MasterItemGroupId";
            chkMatType.DataBind();
        }

        protected void chkAllMatType_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ListItem chk in chkMatType.Items)
            {
                chk.Selected = chkAllMatType.Checked;
            }
        }

        protected void btnExportReport_Click(object sender, EventArgs e)
        {
#pragma warning disable CS0219 // The variable 'stockType' is assigned but its value is never used
            string stockType = "";
#pragma warning restore CS0219 // The variable 'stockType' is assigned but its value is never used

            string matType = "";
            string matTypeText = "";
            foreach (ListItem chk in chkMatType.Items)
            {
                if (chk.Selected)
                {
                    matType += "," + chk.Value;
                    matTypeText += "," + chk.Text;
                }
            }
            if (matType.Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('กรุณาระบุหมวดสินค้า!!');", true);
                return;
            }
            Entities.FormatDate convertDate = new Entities.FormatDate();
            DateTime sDate = new DateTime();
            if (convertDate.getDateFromString(txtDate.Text, ref sDate))
            {
                Session["sDate"] = new DateTime(sDate.Year, sDate.Month, sDate.Day, 23, 59, 59);//eDate;
                // Session["sDate"] = sDate;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('กรุณาระบุวันที่!!');", true);
                return;
            }
            Session["ProjectID"] = ddlProject.SelectedItem.Value;
            Session["ProjectTxt"] = ddlProject.SelectedItem.Text;
            Session["UserRespTxt"] = ddlUser.SelectedItem.Text;
            Session["UserID"] = ddlUser.SelectedItem.Value;
            Session["MatGrpLst"] = matType.Remove(0, 1);
            Session["MatGrpTxt"] = matTypeText.Remove(0, 1).Replace(",", ", ");

            string reqRPT = "BL_SiteFull";

            string urlRptRQ01 = "frmReport.aspx?reqRPT=" + reqRPT;
            ScriptManager.RegisterStartupScript(this, GetType(), "js", "Popup60('" + urlRptRQ01 + "');", true);
            return;
        }
    }
}