using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AP_StockPromotion_V1.web
{
    public partial class StockTransferItem_bkMockup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                loadDataMockup();
            }
        }

        private void loadDataMockup()
        {
            DataTable dt = new DataTable("tbStockReceive");
            initTablePurchaseMockupStructure(ref dt);
            initTablePurchaseMockupData(ref dt);
            bindData(dt);
        }

        private void initTablePurchaseMockupStructure(ref DataTable dt)
        {
            dt.Columns.Add(new DataColumn("RequisitionDate", typeof(String)));
            dt.Columns.Add(new DataColumn("RequisitionBy", typeof(String)));
            dt.Columns.Add(new DataColumn("DocRefNo", typeof(String)));
            dt.Columns.Add(new DataColumn("VendorName", typeof(String)));
            dt.Columns.Add(new DataColumn("ProjectId", typeof(int)));
            dt.Columns.Add(new DataColumn("ProjectName", typeof(String)));
            dt.Columns.Add(new DataColumn("ItemId", typeof(int)));
            dt.Columns.Add(new DataColumn("ItemName", typeof(String)));
            dt.Columns.Add(new DataColumn("ReceiveAmount", typeof(int)));
            dt.Columns.Add(new DataColumn("ItemUnit", typeof(String)));
            dt.Columns.Add(new DataColumn("Cost", typeof(decimal)));
            dt.Columns.Add(new DataColumn("TotalCost", typeof(decimal)));
            dt.Columns.Add(new DataColumn("RequisitionStatus", typeof(String)));
            dt.Columns.Add(new DataColumn("ItemCountMethod", typeof(String)));
            dt.AcceptChanges();
        }

        private void initTablePurchaseMockupData(ref DataTable dt)
        {
            DataRow dr = dt.NewRow();
            dr["RequisitionDate"] = "dd/MM/yyyy";
            dr["RequisitionBy"] = "AP00XXXX";
            dr["DocRefNo"] = "2015-CSP/" + DateTime.Now.ToString("YYMM") + Convert.ToInt32(1).ToString("000000");
            dr["VendorName"] = "บริษัท ABC จำกัด";
            dr["ProjectId"] = 1;
            dr["ProjectName"] = "The City งามวงศ์วาน";
            dr["ItemId"] = 1;
            dr["ItemName"] = "Gift Voucher Starbuck";
            dr["ReceiveAmount"] = 500;
            dr["ItemUnit"] = "ใบ";
            dr["Cost"] = 100;
            dr["TotalCost"] = 50000;
            dr["RequisitionStatus"] = "3";
            dr["ItemCountMethod"] = "2";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["RequisitionDate"] = "dd/MM/yyyy";
            dr["RequisitionBy"] = "AP00XXXX";
            dr["DocRefNo"] = "2015-CSP/" + DateTime.Now.ToString("YYMM") + Convert.ToInt32(2).ToString("000000"); ;
            dr["VendorName"] = "บริษัท ABC จำกัด";
            dr["ProjectId"] = 1;
            dr["ProjectName"] = "The City งามวงศ์วาน";
            dr["ItemId"] = 1;
            dr["ItemName"] = "Gift Voucher Starbuck";
            dr["ReceiveAmount"] = 300;
            dr["ItemUnit"] = "ใบ";
            dr["Cost"] = 100;
            dr["TotalCost"] = 30000;
            dr["RequisitionStatus"] = "3";
            dr["ItemCountMethod"] = "2";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["RequisitionDate"] = "dd/MM/yyyy";
            dr["RequisitionBy"] = "AP00XXXX";
            dr["DocRefNo"] = "2015-CSP/" + DateTime.Now.ToString("YYMM") + Convert.ToInt32(3).ToString("000000"); ;
            dr["VendorName"] = "บริษัท อินโนเรนเจอร์ จำกัด";
            dr["ProjectId"] = 1;
            dr["ProjectName"] = "Centro รามอินทรา 109";
            dr["ItemId"] = 1;
            dr["ItemName"] = "iPhone6 16GB มูลค่า 25,500 บาท";
            dr["ReceiveAmount"] = 100;
            dr["ItemUnit"] = "เครื่อง";
            dr["Cost"] = 25000;
            dr["TotalCost"] = 2500000;
            dr["RequisitionStatus"] = "3";
            dr["ItemCountMethod"] = "1";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["RequisitionDate"] = "dd/MM/yyyy";
            dr["RequisitionBy"] = "AP00XXXX";
            dr["DocRefNo"] = "2015-CSP/" + DateTime.Now.ToString("YYMM") + Convert.ToInt32(4).ToString("000000"); ;
            dr["VendorName"] = "บริษัท อินโนเรนเจอร์ จำกัด";
            dr["ProjectId"] = 1;
            dr["ProjectName"] = "Centro อ่อนนุช";
            dr["ItemId"] = 1;
            dr["ItemName"] = "iPhone6 16GB มูลค่า 25,000 บาท";
            dr["ReceiveAmount"] = 100;
            dr["ItemUnit"] = "เครื่อง";
            dr["Cost"] = 25000;
            dr["TotalCost"] = 2500000;
            dr["RequisitionStatus"] = "1";
            dr["ItemCountMethod"] = "1";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["RequisitionDate"] = "dd/MM/yyyy";
            dr["RequisitionBy"] = "AP00XXXX";
            dr["DocRefNo"] = "2015-CSP/" + DateTime.Now.ToString("YYMM") + Convert.ToInt32(5).ToString("000000"); ;
            dr["VendorName"] = "บริษัท แม่ทองใบ จำกัด";
            dr["ProjectId"] = 1;
            dr["ProjectName"] = "Centro พระราม 9";
            dr["ItemId"] = 1;
            dr["ItemName"] = "สร้อยคอทองคำ หนัก 5 บาท";
            dr["ReceiveAmount"] = 100;
            dr["ItemUnit"] = "เส้น";
            dr["Cost"] = 50000;
            dr["TotalCost"] = 5000000;
            dr["RequisitionStatus"] = "1";
            dr["ItemCountMethod"] = "3";
            dt.Rows.Add(dr);

            dt.AcceptChanges();
        }

        private void bindData(DataTable dt)
        {
            grdData.DataSource = dt;
            grdData.DataBind();
        }

        protected void grdData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // '<%# Eval("RequisitionStatus") %>'
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label grdLbStatus = (Label)e.Row.FindControl("grdlbStatus");
                HiddenField RequisitionStatus = (HiddenField)e.Row.FindControl("hdfRequisitionStatus");
                if (RequisitionStatus.Value == "1")
                {
                    grdLbStatus.Text = "รายการใหม่";
                }
                else if (RequisitionStatus.Value == "2")
                {
                    grdLbStatus.Text = "ปลายทางรอรับ";
                }
                else if (RequisitionStatus.Value == "3")
                {
                    grdLbStatus.Text = "เสร็จสิ้น";
                }
            }
        }

        protected void grdData_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName + "" == "1")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "jsPopupSrDetail", "popupStockTransferItemDetail('" + e.CommandArgument + "');", true);
            }
        }
    }
}