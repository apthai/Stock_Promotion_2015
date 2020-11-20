using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace AP_StockPromotion_V1.webpage
{
    public partial class RequisitionDeleteRemark : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                hdfReqId.Value = Request.QueryString["reqId"] + "";
                initPage();
            }
        }

        private void initPage()
        {
            
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "js", "delRequest('" + txtRemark.Text + "');", true);
        }

    }
}