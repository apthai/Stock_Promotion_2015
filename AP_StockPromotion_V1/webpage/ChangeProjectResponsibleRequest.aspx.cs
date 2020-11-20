using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AP_StockPromotion_V1.webpage
{
    public partial class ChangeProjectResponsibleRequest : System.Web.UI.Page
    {
        Entities.FormatDate convertDate = new Entities.FormatDate();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                initPage();
            }
        }

        private void initPage()
        {
            bindDDLReqType();
            bindDDLUser();
            txtRequestDate.Text = DateTime.Now.ToString(convertDate.formatDate);
            txtReqDocDate.Text = DateTime.Now.ToString(convertDate.formatDate);

        }

        private void bindDDLReqType(string isCRM = "")
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            DataTable dt = dasp.getDataStatus("6");
            ddlReqType.DataSource = dt;
            ddlReqType.DataTextField = "StatusText";
            ddlReqType.DataValueField = "StatusValue";
            ddlReqType.DataBind();
            if (isCRM != "Y")
            {
                ddlReqType.Items.Remove(ddlReqType.Items.FindByValue("1"));
            }
            ddlReqType.Items.Insert(0, new ListItem("-- โปรดระบุ --", ""));
        }

        private void bindDDLUser()
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            DataTable dt = dasp.getDataUser("", "");
            ddlUser.DataSource = dt;
            ddlUser.DataTextField = "FullNoName";
            ddlUser.DataValueField = "EmpCode";
            ddlUser.DataBind();
            ddlUser.Items.Insert(0, new ListItem("-- เลือก --", ""));
        }
    }
}