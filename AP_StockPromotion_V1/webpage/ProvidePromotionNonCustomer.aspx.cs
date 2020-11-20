using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
namespace AP_StockPromotion_V1.web
{
    public partial class ProvidePromotionNonCustomer : System.Web.UI.Page
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
            dr["Balance"] = 126;
            dr["ItemCouontMethod"] = "2";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Item"] = "Gift Voucher Central Card";
            dr["Balance"] = 147;
            dr["ItemCouontMethod"] = "2";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Item"] = "Gift Voucher Starbuck";
            dr["Balance"] = 350;
            dr["ItemCouontMethod"] = "2";
            dr["Amount"] = 1;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Item"] = "Gift Voucher Lotus";
            dr["Balance"] = 147;
            dr["ItemCouontMethod"] = "2";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Item"] = "Gift Voucher Paragon";
            dr["Balance"] = 350;
            dr["ItemCouontMethod"] = "2";
            dr["Amount"] = 1;
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
            if (e.CommandName == "ProvidePromotionNonCustomerDetail")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "jsPopupSrDetail", "popupProvidePromotionNonCustomerDetail();", true);
            }
        }


    }
}