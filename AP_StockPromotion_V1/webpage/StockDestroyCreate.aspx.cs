using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AP_StockPromotion_V1.web
{
    public partial class StockDestroyCreate : System.Web.UI.Page
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
            string RequisitionStatus = Request.QueryString["tbStockDestroyCreate"] + "";
            if (RequisitionStatus == "3")
            {
                // div_btnAccept.Attributes.Add("style", "display:none;");
            }
        }

        private void loadDataMockup()
        {
            DataTable dt = new DataTable("tbStockDestroyCreate");
            initTablePurchaseMockupStructure(ref dt);
            Session["tbStockDestroyCreate"] = dt.Copy();
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
            DataTable dt = (DataTable)Session["tbStockDestroyCreate"];
            for (int ii = 0; ii <= 10; ii++)
            {
                DataRow dr = dt.NewRow();
                Random rnd = new Random();
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
                lbItemName.Text = ddlItem.SelectedItem.Text;
                lbExpireDate.Text = DateTime.Now.AddDays(rnd.Next(1, 10) * -1).ToString("dd/MM/yyyy");
                lbEndOfWarranty.Text = DateTime.Now.AddDays(rnd.Next(1, 10) * -1).ToString("dd/MM/yyyy");
            }
        }

        protected void grdData_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "destroyItem")
            {
                ((ImageButton)e.CommandSource).Attributes.Add("style", "display:none;");
            }
        }
    }
}