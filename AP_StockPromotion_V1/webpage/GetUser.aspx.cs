using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AP_StockPromotion_V1.webpage
{
    public partial class GetUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                string searchStr = Request.QueryString["sUser"] + "";
                if (searchStr != "")
                {
                    string[] n = searchStr.Split('_');
                    txtFirstName.Text = n[0];
                    txtLastName.Text = "";
                    if (searchStr.Length > 1)
                    {
                        txtLastName.Text = n[1];
                    }
                    bindData();
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            bindData();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
        }

        private void bindData()
        {
            Class.DAStockPromotion cls = new Class.DAStockPromotion();
            DataTable dt = cls.getDataUser(txtFirstName.Text, txtLastName.Text);
            grdData.DataSource = dt;
            grdData.DataBind();
        }

        protected void grdData_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "selEmp")
            {
                string reqBtnId = Request.QueryString["btnFindUserId"] + "";
                string txtEmpCodeId = Request.QueryString["txtFindUserId"] + "";
                //GridViewRow gvr = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "bindDataParentPage('" + reqBtnId + "','" + txtEmpCodeId + "','" + e.CommandArgument + "');", true);
                return;
            }
        }
    }
}