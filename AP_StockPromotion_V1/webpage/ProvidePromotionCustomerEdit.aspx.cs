using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AP_StockPromotion_V1.web
{
    public partial class ProvidePromotionCustomerEdit : System.Web.UI.Page
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
            string ItemCountMethod = (string)Request.QueryString["ItemCountMethod"];
            if (ItemCountMethod == "1") { div1.Attributes.Add("style", ""); }
            else if (ItemCountMethod == "2") { div1.Attributes.Add("style", ""); }
            else if (ItemCountMethod == "3") { div3.Attributes.Add("style", ""); }
            string mode = (string)Request.QueryString["mode"];
            if (mode + "" == "Detail")
            {
                divinput1.Attributes.Add("style", "display:none;");
                divinput2.Attributes.Add("style", "display:none;");
                divinput3.Attributes.Add("style", "display:none;");

                div_btnAccept.Attributes.Add("style", "display:none;");
            }
        }

        private void loadDataMockup()
        {
            DataTable dt = new DataTable("ProvidePromotionCustomerEdit");
            initTablePurchaseMockupStructure(ref dt);
            Session["ProvidePromotionCustomerEdit"] = dt.Copy();
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
            DataTable dt = (DataTable)Session["ProvidePromotionCustomerEdit"];

            Random rnd = new Random();
            for (int ii = 0; ii <= rnd.Next(1, 3); ii++)
            {
                DataRow dr = dt.NewRow();
                dr["SerialNo"] = rnd.Next(1, 10).ToString() + rnd.Next(1, 10).ToString() + rnd.Next(1, 10).ToString() + rnd.Next(1, 10).ToString() + rnd.Next(1, 10).ToString() + rnd.Next(1, 10).ToString() + rnd.Next(1, 10).ToString() + rnd.Next(1, 10).ToString() + rnd.Next(1, 10).ToString() + rnd.Next(1, 10).ToString() + rnd.Next(1, 10).ToString() + rnd.Next(1, 10).ToString() + rnd.Next(1, 10).ToString() + rnd.Next(1, 10).ToString() + rnd.Next(1, 10).ToString() + rnd.Next(1, 10).ToString();
                dt.Rows.Add(dr);
            }
            dt.AcceptChanges();
            bindData(dt);
        }

        protected void grdData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lbBarcode = (Label)e.Row.FindControl("grdtxtBarcode");
                Label lbItemName = (Label)e.Row.FindControl("ItemName");
                Label lbItemUnit = (Label)e.Row.FindControl("grdtxtItemUnit");
                Label lbExpireDate = (Label)e.Row.FindControl("grdtxtExpireDate");
                Label lbEndOfWarranty = (Label)e.Row.FindControl("grdtxtEndOfWarranty");

                Random rnd = new Random();
                lbBarcode.Text = rnd.Next(1, 10).ToString() + rnd.Next(1, 10).ToString() + rnd.Next(1, 10).ToString() + rnd.Next(1, 10).ToString() + rnd.Next(1, 10).ToString() + rnd.Next(1, 10).ToString() + rnd.Next(1, 10).ToString() + rnd.Next(1, 10).ToString() + rnd.Next(1, 10).ToString() + rnd.Next(1, 10).ToString() + rnd.Next(1, 10).ToString() + rnd.Next(1, 10).ToString() + rnd.Next(1, 10).ToString() + rnd.Next(1, 10).ToString();
                lbItemName.Text = "สินค้าโปรโมชั่น";
                lbExpireDate.Text = DateTime.Now.AddDays(rnd.Next(1, 10) * -1).ToString("dd/MM/yyyy");
                lbEndOfWarranty.Text = DateTime.Now.AddDays(rnd.Next(1, 10) * -1).ToString("dd/MM/yyyy");
            }
        }
    }
}