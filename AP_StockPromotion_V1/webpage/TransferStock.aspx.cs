using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AP_StockPromotion_V1.web
{
    public partial class TransferStock : System.Web.UI.Page
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
        }

        private void loadDataMockup()
        {
            DataTable dt = new DataTable("tbTransferStock");
            initTablePurchaseMockupStructure(ref dt);
            initTablePurchaseMockupData(ref dt);
            bindData(dt);
        }

        private void initTablePurchaseMockupStructure(ref DataTable dt)
        {
            dt.Columns.Add(new DataColumn("TransferDate", typeof(String)));
            dt.Columns.Add(new DataColumn("TransferBy", typeof(String)));
            dt.Columns.Add(new DataColumn("SourceProject", typeof(String)));
            dt.Columns.Add(new DataColumn("DesProject", typeof(String)));
            dt.Columns.Add(new DataColumn("Item", typeof(String)));
            dt.Columns.Add(new DataColumn("Amount", typeof(int)));
            dt.Columns.Add(new DataColumn("ItemCouontMethod", typeof(String)));
            dt.AcceptChanges();
        }

        private void initTablePurchaseMockupData(ref DataTable dt)
        {
            DataRow dr = dt.NewRow();
            dr["TransferDate"] = DateTime.Now.ToString("dd/MM/yyyy");
            dr["TransferBy"] = "AP00XXXX";
            dr["SourceProject"] = "The City งามวงศ์วาน";
            dr["DesProject"] = "The City รามอินทรา";
            dr["Item"] = "Gift Voucher Major";
            dr["Amount"] = 150;
            dr["ItemCouontMethod"] = "2";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["TransferDate"] = DateTime.Now.ToString("dd/MM/yyyy");
            dr["TransferBy"] = "AP00XXXX";
            dr["SourceProject"] = "The City งามวงศ์วาน";
            dr["DesProject"] = "The City รามอินทรา";
            dr["Item"] = "Gift Voucher Central Card";
            dr["Amount"] = 100;
            dr["ItemCouontMethod"] = "2";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["TransferDate"] = DateTime.Now.ToString("dd/MM/yyyy");
            dr["TransferBy"] = "AP00XXXX";
            dr["SourceProject"] = "The City งามวงศ์วาน";
            dr["DesProject"] = "The City รามอินทรา";
            dr["Item"] = "Gift Voucher Starbuck";
            dr["Amount"] = 50;
            dr["ItemCouontMethod"] = "2";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["TransferDate"] = DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy");
            dr["TransferBy"] = "AP00XXXX";
            dr["SourceProject"] = "Centro พระราม 9";
            dr["DesProject"] = "Centro รามอินทรา 109";
            dr["Item"] = "iPhone6 16GB มูลค่า 25,000 บาท";
            dr["Amount"] = 50;
            dr["ItemCouontMethod"] = "1";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["TransferDate"] = DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy");
            dr["TransferBy"] = "AP00XXXX";
            dr["SourceProject"] = "Centro พระราม 9";
            dr["DesProject"] = "Centro รามอินทรา 109";
            dr["Item"] = "สร้อยคอทองคำ หนัก 1 บาท";
            dr["Amount"] = 10;
            dr["ItemCouontMethod"] = "3";
            dt.Rows.Add(dr);
            
            dt.AcceptChanges();
        }

        private void bindData(DataTable dt)
        {
            grdData.DataSource = dt;
            grdData.DataBind();
        }

        protected void grdData_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName + "" == "TransferStockDetail")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "jsPopupSrDetail", "popupTransferStockDetail('Detail','" + e.CommandArgument + "');", true);
            }
        }

        protected void grdData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
        }
    }
}