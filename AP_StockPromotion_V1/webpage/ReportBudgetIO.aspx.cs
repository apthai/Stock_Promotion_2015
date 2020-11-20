using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AP_StockPromotion_V1.webpage
{
    public partial class ReportBudgetIO : System.Web.UI.Page
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
            bindChkCompany();
            bindChkProject();
            txtCompany.Text = "";
            txtProject.Text = "";
            txtCompany.Focus();
        }

        private void bindChkCompany()
        {
            Class.DAStockPromotion cls = new Class.DAStockPromotion();
            Session["dtCompany"] = cls.getDataMasterCompany();
            Session["dtCompanySel"] = null;
            grdCompany.DataSource = (DataTable)Session["dtCompanySel"];
            grdCompany.DataBind();
        }

        private void bindChkProject()
        {
            Class.DAStockPromotion cls = new Class.DAStockPromotion();
            Session["dtProject"] = cls.getDataMasterProject();
            Session["dtProjectSel"] = null;
            grdCompany.DataSource = ((DataTable)Session["dtProject"]).Clone();//(DataTable)Session["dtProjectSel"];
            grdCompany.DataBind();
        }






        protected void btnEnterCmp_Click(object sender, EventArgs e)
        {
            DataTable dtCmp = (DataTable)Session["dtCompany"];
            dtCmp.DefaultView.RowFilter = "FullName like '%" + txtCompany.Text + "%'";
            DataTable dt = dtCmp.DefaultView.ToTable();
            if (dt.Rows.Count != 1)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('กรุณาระบุบริษัทให้ถูกต้อง !!');", true);
                txtCompany.Text = "";
                txtCompany.Focus();
                return;
            }
            DataTable dtCmpSel = (DataTable)Session["dtCompanySel"];
            if (dtCmpSel == null)
            {
                Session["dtCompanySel"] = dt;
            }
            else
            {
                if (dtCmpSel.Select("FullName = '" + dt.Rows[0]["FullName"] + "'").Length == 0)
                {
                    dtCmpSel.ImportRow(dt.Rows[0]);
                    Session["dtCompanySel"] = dtCmpSel;
                }
            }
            grdCompany.DataSource = (DataTable)Session["dtCompanySel"];
            grdCompany.DataBind();
            txtCompany.Text = "";
            txtCompany.Focus();
        }

        protected void grdCompany_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "delCmp")
            {
                DataTable dt = (DataTable)Session["dtCompanySel"];
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["CompanyID"] + "" == e.CommandArgument + "")
                    {
                        dr.Delete();
                        break;
                    }
                }
                dt.AcceptChanges();
                Session["dtCompanySel"] = dt;
                grdCompany.DataSource = dt;
                grdCompany.DataBind();
                txtCompany.Text = "";
                txtCompany.Focus();
            }
        }

        protected void grdProject_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "delPrj")
            {
                DataTable dt = (DataTable)Session["dtProjectSel"];
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["ProjectCode"] + "" == e.CommandArgument + "")
                    {
                        dr.Delete();
                        break;
                    }
                }
                dt.AcceptChanges();
                Session["dtProjectSel"] = dt;
                grdProject.DataSource = dt;
                grdProject.DataBind();
                txtProject.Text = "";
                txtProject.Focus();
            }
        }





        protected void btnEnterPrj_Click(object sender, EventArgs e)
        {
            DataTable dtPrj = (DataTable)Session["dtProject"];
            dtPrj.DefaultView.RowFilter = "ProjectName like '%" + txtProject.Text + "%'";
            DataTable dt = dtPrj.DefaultView.ToTable();
            if (dt.Rows.Count != 1)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('กรุณาระบุโครงการให้ถูกต้อง !!');", true);
                txtProject.Text = "";
                txtProject.Focus();
                return;
            }
            DataTable dtPrjSel = (DataTable)Session["dtProjectSel"];
            if (dtPrjSel == null)
            {
                Session["dtProjectSel"] = dt;
            }
            else
            {
                if (dtPrjSel.Select("ProjectName = '" + dt.Rows[0]["ProjectName"] + "'").Length == 0)
                {
                    dtPrjSel.ImportRow(dt.Rows[0]);
                    Session["dtProjectSel"] = dtPrjSel;
                }
            }
            grdProject.DataSource = (DataTable)Session["dtProjectSel"];
            grdProject.DataBind();
            txtProject.Text = "";
            txtProject.Focus();
        }


















        protected void btnExportReport_Click(object sender, EventArgs e)
        {
            /*
                I_PROJ	Project
                I_BUKRS	Company Code
                I_AUFNR	IO Number
             */
            //DataTable dtcmp = (DataTable)Session["dtCompanySel"];
            //DataTable dtprj = (DataTable)Session["dtProjectSel"];
            

            //Session["I_PROJ"] = "X".Remove(0, 1);
            //Session["I_BUKRS"] = "X".Remove(0, 1);
            //Session["I_AUFNR"] = "X".Remove(0, 1);


            string reqRPT = "BudgetIO";

            string urlRptRQ01 = "frmReport.aspx?reqRPT=" + reqRPT;
            ScriptManager.RegisterStartupScript(this, GetType(), "js", "Popup60('" + urlRptRQ01 + "');", true);
            return;
        }

        
    }
}