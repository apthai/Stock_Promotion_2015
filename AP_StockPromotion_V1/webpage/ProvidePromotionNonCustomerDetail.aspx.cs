using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace AP_StockPromotion_V1.web
{
    public partial class ProvidePromotionNonCustomerDetail : System.Web.UI.Page
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
            dt.Columns.Add(new DataColumn("ProvideDate", typeof(String)));
            dt.Columns.Add(new DataColumn("ProvideToId", typeof(String)));
            dt.Columns.Add(new DataColumn("ProvideToName", typeof(String)));
            dt.Columns.Add(new DataColumn("ProvideToTel", typeof(String)));
            dt.Columns.Add(new DataColumn("ProvideItemRefNo", typeof(String)));
            dt.AcceptChanges();
        }

        private void initTablePurchaseMockupData(ref DataTable dt)
        {
            Random rnd = new Random();

            DataRow dr = dt.NewRow();
            dr["ProvideDate"] = DateTime.Now.AddHours(rnd.Next(1, 50) * (-1)).ToString("yyyy-MM-dd HH:mm:ss");
            dr["ProvideToId"] = "XXXXXXXXXXXXX";
            dr["ProvideToName"] = "นาย กกกกกกก ขขขขขขขขขขข";
            dr["ProvideToTel"] = "0917531357";
            dr["ProvideItemRefNo"] = "XXXXXXXXXXXXX";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ProvideDate"] = DateTime.Now.AddHours(rnd.Next(1, 50) * (-1)).ToString("yyyy-MM-dd HH:mm:ss");
            dr["ProvideToId"] = "XXXXXXXXXXXXX";
            dr["ProvideToName"] = "นาย พพพพพพ ฟฟฟฟฟฟฟฟฟฟฟ";
            dr["ProvideToTel"] = "0917585573";
            dr["ProvideItemRefNo"] = "XXXXXXXXXXXXX";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ProvideDate"] = DateTime.Now.AddHours(rnd.Next(1, 50) * (-1)).ToString("yyyy-MM-dd HH:mm:ss");
            dr["ProvideToId"] = "XXXXXXXXXXXXX";
            dr["ProvideToName"] = "นาย ดดดดดด ปปปปปปปปปป";
            dr["ProvideToTel"] = "0615370897";
            dr["ProvideItemRefNo"] = "XXXXXXXXXXXXX";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ProvideDate"] = DateTime.Now.AddHours(rnd.Next(1, 50) * (-1)).ToString("yyyy-MM-dd HH:mm:ss");
            dr["ProvideToId"] = "XXXXXXXXXXXXX";
            dr["ProvideToName"] = "นาย รรรรรรร ลลลลลลลลลลล";
            dr["ProvideToTel"] = "0815315797";
            dr["ProvideItemRefNo"] = "XXXXXXXXXXXXX";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ProvideDate"] = DateTime.Now.AddHours(rnd.Next(1, 50) * (-1)).ToString("yyyy-MM-dd HH:mm:ss");
            dr["ProvideToId"] = "XXXXXXXXXXXXX";
            dr["ProvideToName"] = "นาย ววววววววว ยยยยยยยยยยย";
            dr["ProvideToTel"] = "091351351";
            dr["ProvideItemRefNo"] = "XXXXXXXXXXXXX";
            dt.Rows.Add(dr);

            dt.AcceptChanges();
        }

        private void bindData(DataTable dt)
        {
            dt.DefaultView.Sort = "ProvideDate desc";
            grdData.DataSource = dt.DefaultView.ToTable();
            grdData.DataBind();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("ProvidePromotionNonCustomer.aspx");
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("ProvidePromotionNonCustomer.aspx");
        }


    }
}