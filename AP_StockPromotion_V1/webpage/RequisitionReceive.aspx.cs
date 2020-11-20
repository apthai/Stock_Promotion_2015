using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AP_StockPromotion_V1.web
{
    public partial class RequisitionReceive : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                initPage();
                loadDataMockup();
            }
        }

        private void initPage()
        {
            string RequisitionStatus = Request.QueryString["RequisitionStatus"] + "";
            if (RequisitionStatus == "3")
            {
                div_btnAccept.Attributes.Add("style", "display:none;");
            }
        }

        private void loadDataMockup()
        {
            DataTable dt = new DataTable("tbRequisitionReceive");
            initTablePurchaseMockupStructure(ref dt);
            Session["tbRequisitionReceive"] = dt.Copy();
            enterData();
        }

        private void initTablePurchaseMockupStructure(ref DataTable dt)
        {
            dt.Columns.Add(new DataColumn("SerialNo", typeof(String)));
            dt.AcceptChanges();
        }

        private void bindData(DataTable dt)
        {
            grdData.DataSource = dt;
            grdData.DataBind();
        }

        private void enterData()
        {
            DataTable dt = (DataTable)Session["tbRequisitionReceive"];
            Random rnd = new Random();
            for (int ii = 0; ii <= rnd.Next(1, 10); ii++)
            {
                DataRow dr = dt.NewRow();
                dr["SerialNo"] = rnd.Next(1, 10).ToString() + rnd.Next(1, 10).ToString() + rnd.Next(1, 10).ToString() + rnd.Next(1, 10).ToString() + rnd.Next(1, 10).ToString() + rnd.Next(1, 10).ToString() + rnd.Next(1, 10).ToString() + rnd.Next(1, 10).ToString() + rnd.Next(1, 10).ToString() + rnd.Next(1, 10).ToString() + rnd.Next(1, 10).ToString() + rnd.Next(1, 10).ToString() + rnd.Next(1, 10).ToString() + rnd.Next(1, 10).ToString() + rnd.Next(1, 10).ToString() + rnd.Next(1, 10).ToString();
                dt.Rows.Add(dr);
            }
            dt.AcceptChanges();
            bindData(dt);
        }

    }
}