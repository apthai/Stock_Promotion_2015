using AP_StockPromotion_V1.ws_authorize;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AP_StockPromotion_V1.webpage
{
    public partial class StockReturnItemReason : System.Web.UI.Page
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
            hdfItemLst.Value = Request.QueryString["itemLst"] + "";
            hdfProject_id.Value = Request.QueryString["project_id"] + "";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            AutorizeData auth = (AutorizeData)Session["userInfo_" + Session.SessionID];

            int project_id = 0;
            int.TryParse(hdfProject_id.Value, out project_id);

            Class.DAStockReturn cls = new Class.DAStockReturn();
            if (cls.returnItem(hdfItemLst.Value, project_id, auth.EmployeeID, txtReason.Text))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('บันทึกข้อมูลเสร็จสิ้น'); parent.window.location.replace('StockReturnItem.aspx');", true);
                return;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('บันทึกข้อมูลผิดพลาด กรุณาติดต่อผู้ดูแลระบบ !!');", true);
                return;
            }
        }


        protected void btnClose_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "js", "parent.jQuery.colorbox.close();", true);
        }
    }
}