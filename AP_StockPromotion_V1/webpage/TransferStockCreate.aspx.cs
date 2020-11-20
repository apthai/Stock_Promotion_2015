using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AP_StockPromotion_V1.web
{
    public partial class TransferStockCreate : System.Web.UI.Page
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
            DataTable dt = new DataTable("tbRequisition");
            initTablePurchaseMockupStructure(ref dt);
            initTablePurchaseMockupData(ref dt);
            bindData(dt);
        }

        private void initTablePurchaseMockupStructure(ref DataTable dt)
        {
            dt.Columns.Add(new DataColumn("Item", typeof(String)));
            dt.Columns.Add(new DataColumn("Balance", typeof(int)));
            dt.Columns.Add(new DataColumn("Amount", typeof(int)));
            dt.Columns.Add(new DataColumn("ItemCouontMethod", typeof(String)));
            dt.AcceptChanges();
        }

        private void initTablePurchaseMockupData(ref DataTable dt)
        {
            DataRow dr = dt.NewRow();
            dr["Item"] = "Gift Voucher Major";
            dr["Amount"] = 50;
            dr["Balance"] = 71;
            dr["ItemCouontMethod"] = "2";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Item"] = "Gift Voucher Central Card";
            dr["Balance"] = 75;
            dr["ItemCouontMethod"] = "2";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Item"] = "Gift Voucher Starbuck";
            dr["Amount"] = 150;
            dr["Balance"] = 350;
            dr["ItemCouontMethod"] = "2";
            dr["Amount"] = 1;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Item"] = "Gift Voucher Lotus";
            dr["Balance"] = 117;
            dr["ItemCouontMethod"] = "2";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Item"] = "Gift Voucher Paragon";
            dr["Balance"] = 168;
            dr["ItemCouontMethod"] = "2";
            dr["Amount"] = 1;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Item"] = "สร้อยคอทองคำ หนัก 1 บาท";
            dr["Balance"] = 17;
            dr["Amount"] = 1;
            dr["ItemCouontMethod"] = "3";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Item"] = "iPhone6 16GB มูลค่า 25,000 บาท";
            dr["Amount"] = 10;
            dr["Balance"] = 23;
            dr["ItemCouontMethod"] = "1";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Item"] = "iPhone5 8GB มูลค่า 21,500 บาท";
            dr["Balance"] = 15;
            dr["ItemCouontMethod"] = "1";
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
            if (e.CommandName + "" == "StockTransferDetailCreate")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "jsPopupSrDetail", "popupTransferStockDetail('Create','" + e.CommandArgument + "');", true);
            }
        }

        protected void grdData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
            }
        }


    }
}