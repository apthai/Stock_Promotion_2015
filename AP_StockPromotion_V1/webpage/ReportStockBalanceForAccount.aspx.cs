using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AP_StockPromotion_V1.webpage
{
    public partial class ReportStockBalanceForAccount : System.Web.UI.Page
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
            txtDateBeg.Text = DateTime.Now.ToString("dd/MM/yyyy");
            bindChkCompany();
            bindChkMatType();
        }

        private void bindChkCompany()
        {
            Class.DAStockPromotion cls = new Class.DAStockPromotion();
            DataTable dt = cls.getDataMasterCompany();
            chkCompany.DataSource = dt;
            chkCompany.DataTextField = "FullName";
            chkCompany.DataValueField = "CompanyID";
            chkCompany.DataBind();
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
            Entities.FormatDate convertDate = new Entities.FormatDate();
            DateTime bDate = new DateTime();
            if (convertDate.getDateFromString(txtDateBeg.Text, ref bDate))
            {
                Session["DateBeg"] = bDate;// new DateTime(bDate.Year, bDate.Month, bDate.Day, 0, 0, 1);//bDate;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('กรุณาระบุวันที่!!');", true);
                return;
            }

            string CmpLst = "";
            string CmpLstText = "";
            foreach (ListItem chk in chkCompany.Items)
            {
                if (chk.Selected)
                {
                    CmpLst += "," + chk.Value;
                    CmpLstText += "," + chk.Text.Split(' ')[0];
                }
            }
            if (chkNoneProject.Checked)
            {
                CmpLst += CmpLst == "" ? "," : "";
                CmpLstText += "," + "ไม่ระบุโครงการ";
            }
            if (CmpLst.Length == 0 && chkNoneProject.Checked == false)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('กรุณาระบุบริษัท!!');", true);
                return;
            }

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


            Session["MatGrp"] = matType.Remove(0, 1);
            Session["MatGrpText"] = matTypeText.Remove(0, 1).Replace(",", ", ");

            Session["CompanyCode"] = CmpLst.Remove(0, 1);
            Session["CompanyName"] = CmpLstText.Remove(0, 1).Replace(",", ", ");            
            Session["isIChkNoneProject"] = chkNoneProject.Checked ? "Y" : "";
            string reqRPT = "BL_Acc";

            string urlRptRQ01 = "frmReport.aspx?reqRPT=" + reqRPT;
            ScriptManager.RegisterStartupScript(this, GetType(), "js", "Popup60('" + urlRptRQ01 + "');", true);
            return;
        }

        protected void chkAllCompany_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ListItem chk in chkCompany.Items)
            {
                chk.Selected = chkAllCompany.Checked;
            }
            chkNoneProject.Checked = chkAllCompany.Checked;
        }
    }
}