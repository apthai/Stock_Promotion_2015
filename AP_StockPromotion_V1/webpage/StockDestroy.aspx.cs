using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AP_StockPromotion_V1.web
{
    public partial class StockDestroy : System.Web.UI.Page
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
            DataTable dt = new DataTable("tbStockDestroy");
            initTablePurchaseMockupStructure(ref dt);
            initTablePurchaseMockupData(ref dt);
            bindData(dt);
        }

        private void initTablePurchaseMockupStructure(ref DataTable dt)
        {            
            dt.Columns.Add(new DataColumn("DestroyDate", typeof(String)));
            dt.Columns.Add(new DataColumn("UserNo", typeof(String)));
            dt.Columns.Add(new DataColumn("Project", typeof(String)));
            dt.Columns.Add(new DataColumn("Item", typeof(String)));
            dt.Columns.Add(new DataColumn("Amount", typeof(int)));
            dt.Columns.Add(new DataColumn("ItemCouontMethod", typeof(String)));
            dt.AcceptChanges();
        }

        private void initTablePurchaseMockupData(ref DataTable dt)
        {
            DataRow dr = dt.NewRow();
            dr["DestroyDate"] = "23/03/2015";
            dr["UserNo"] = "AP00XXXX";
            dr["Project"] = "Centro รามอินทรา 109";
            dr["Item"] = "Gift Voucher Major มูลค่า 150 บาท";
            dr["Amount"] = 13;
            dr["ItemCouontMethod"] = "2";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["DestroyDate"] = "23/03/2015";
            dr["UserNo"] = "AP00XXXX";
            dr["Project"] = "Centro อ่อนนุช";
            dr["Item"] = "Gift Voucher Starbuck มูลค่า 100 บาท";
            dr["Amount"] = 5;
            dr["ItemCouontMethod"] = "2";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["DestroyDate"] = "23/03/2015";
            dr["UserNo"] = "AP00XXXX";
            dr["Project"] = "Centro อ่อนนุช";
            dr["Item"] = "iPhone6 16GB มูลค่า 25,500 บาท";
            dr["Amount"] = 1;
            dr["ItemCouontMethod"] = "1";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["DestroyDate"] = "23/03/2015";
            dr["UserNo"] = "AP00XXXX";
            dr["Project"] = "The City รามอินทรา";
            dr["Item"] = "Gift Voucher Starbuck มูลค่า 100 บาท";
            dr["Amount"] = 53;
            dr["ItemCouontMethod"] = "2";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["DestroyDate"] = "23/03/2015";
            dr["UserNo"] = "AP00XXXX";
            dr["Project"] = "The City รามอินทรา";
            dr["Item"] = "Gift Voucher Central Card มูลค่า 100 บาท";
            dr["Amount"] = 26;
            dr["ItemCouontMethod"] = "2";
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
            if (e.CommandName + "" == "destroyItem")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "jsPopupSrDetail", "popupStockDestroyDetail();", true);
            }
        }

        protected void grdData_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
    }
}