using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AP_StockPromotion_V1.web
{
    public partial class RequisitionCreate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            {
                initPage();
                
            }
        }

        private void initPage()
        {
            
        }

        private void loadDataMockup()
        {
            DataTable dt = new DataTable("tbStockRevceive");
            initTablePurchaseMockupStructure(ref dt);
            initTablePurchaseMockupData(ref dt);
            bindData(dt);
        }

        private void initTablePurchaseMockupStructure(ref DataTable dt)
        {
            dt.Columns.Add(new DataColumn("DocRefNo", typeof(String)));
            dt.Columns.Add(new DataColumn("ProjectId", typeof(int)));
            dt.Columns.Add(new DataColumn("ProjectName", typeof(String)));
            dt.Columns.Add(new DataColumn("ItemId", typeof(int)));
            dt.Columns.Add(new DataColumn("ItemName", typeof(String)));
            dt.Columns.Add(new DataColumn("ReceiveAmount", typeof(int)));
            dt.Columns.Add(new DataColumn("ItemUnit", typeof(String)));
            dt.Columns.Add(new DataColumn("Cost", typeof(decimal)));
            dt.Columns.Add(new DataColumn("TotalCost", typeof(decimal)));
            dt.AcceptChanges();
        }

        private void initTablePurchaseMockupData(ref DataTable dt)
        {
            DataRow dr = dt.NewRow();
            dr["DocRefNo"] = "MM:Memo" + DateTime.Now.ToString("YYMM") + Convert.ToInt32(2).ToString("000000");
            dr["ProjectId"] = 1;
            dr["ProjectName"] = "The City งามวงศ์วาน";
            dr["ItemId"] = 1;
            dr["ItemName"] = "Gift Voucher Starbuck";
            dr["ReceiveAmount"] = 300;
            dr["ItemUnit"] = "ใบ";
            dr["Cost"] = 100;
            dr["TotalCost"] = 30000;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["DocRefNo"] = "MM:Memo" + DateTime.Now.ToString("YYMM") + Convert.ToInt32(4).ToString("000000");
            dr["ProjectId"] = 1;
            dr["ProjectName"] = "Centro อ่อนนุช";
            dr["ItemId"] = 1;
            dr["ItemName"] = "iPhone6 16GB มูลค่า 25,000 บาท";
            dr["ReceiveAmount"] = 100;
            dr["ItemUnit"] = "เครื่อง";
            dr["Cost"] = 25000;
            dr["TotalCost"] = 2500000;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["DocRefNo"] = "MM:Memo" + DateTime.Now.ToString("YYMM") + Convert.ToInt32(5).ToString("000000");
            dr["ProjectId"] = 1;
            dr["ProjectName"] = "Centro พระราม 9";
            dr["ItemId"] = 1;
            dr["ItemName"] = "สร้อยคอทองคำ หนัก 5 บาท";
            dr["ReceiveAmount"] = 100;
            dr["ItemUnit"] = "เส้น";
            dr["Cost"] = 50000;
            dr["TotalCost"] = 5000000;
            dt.Rows.Add(dr);

            dt.AcceptChanges();
        }

        private void bindData(DataTable dt)
        {
            grdData.DataSource = dt;
            grdData.DataBind();
        }

    }
}