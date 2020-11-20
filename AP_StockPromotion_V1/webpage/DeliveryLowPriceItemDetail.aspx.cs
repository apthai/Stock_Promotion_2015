using AP_StockPromotion_V1.ws_authorize;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AP_StockPromotion_V1.webpage
{
    public partial class DeliveryLowPriceItemDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                initPage();
            }
        }

        private void initPage()
        {
            string DelvLstId = Request.QueryString["DelvLstId"] + "";
            string mode = Request.QueryString["mode"] + "";
            if (mode == "LowPrice")
            {

            }
            Class.DADelivery cls = new Class.DADelivery();
            Int64 delvLstId = 0;
            Int64.TryParse(DelvLstId, out delvLstId);
            DataTable dt = cls.GetDataDeliveryItem(delvLstId);
            if (dt.Rows.Count > 0)
            {
                /*
                 DelvPromotionId	PromotionId DelvBy	DelvDate
                 ItemId  Serial	ItemName	Model	Color	DimensionWidth	DimensionLong	DimensionHeight	DimensionUnit	Weight	WeightUnit	Price	ProduceDate	ExpireDate	Detail	Remark
                 */
                DataRow dr = dt.Rows[0];
                txtDelvNo.Text = dr["DelvPromotionId"] + "";
                if (dr["isPostAcc"] + "" == "Y")
                {
                    txtDelvStatus.Text = "บันทึกบัญชีแล้ว"; 
                    btnDelDeliv.Attributes.Add("style", "display:none;");
                }
                else
                {
                    txtDelvStatus.Text = "รอการบันทึกบัญชี";
                }
                txtDelvBy.Text = dr["DelvBy"] + "";
                Entities.FormatDate convertDate = new Entities.FormatDate();
                string dDate = "";
                convertDate.getStringFromDate(dr["DelvDate"], ref dDate);
                txtDelvDate.Text = dDate;

                DataTable dtGrd = dt.DefaultView.ToTable(true, "ItemName","Model","Color","Price","ProduceDate","ExpireDate","Detail","Remark");
                dtGrd.Columns.Add(new DataColumn("Amount", typeof(int)));
                dtGrd.Columns.Add(new DataColumn("Total", typeof(decimal)));
                foreach(DataRow drGrd in dtGrd.Rows){
                    string cond = "isNull(ItemName,'') = '"+ drGrd["ItemName"] +"' AND isNull(Model,'') = '"+ drGrd["Model"] +"' AND isNull(Color,'') = '"+ drGrd["Color"] +"' AND isNull(Price,'') = '"+ drGrd["Price"] +"' AND isNull(ProduceDate,'') = '"+ drGrd["ProduceDate"] +"' AND isNull(ExpireDate,'') = '"+ drGrd["ExpireDate"] +"' AND isNull(Detail,'') = '"+ drGrd["Detail"] +"' AND isNull(Remark,'') = '"+ drGrd["Remark"] +"'";
                    drGrd["Amount"] = dt.Compute("COUNT(ItemId)",cond);
                    drGrd["Total"] = (decimal)drGrd["Price"] * (int)drGrd["Amount"];
                }
                dtGrd.AcceptChanges();
                grdData.DataSource = dtGrd;
                grdData.DataBind();
            }
        }

        protected void btnDelDeliv_Click(object sender, EventArgs e)
        {
            string DelvLstId = Request.QueryString["DelvLstId"] + "";
            AutorizeData auth = new AutorizeData();
            auth = (AutorizeData)Session["userInfo_" + Session.SessionID];
            Class.DADelivery cls = new Class.DADelivery();
            Int64 delvLstId = 0;
            Int64.TryParse(DelvLstId, out delvLstId);
            // if (cls.cancelVerifyDeliveryItem(delvLstId, auth.EmployeeID))
            if (cls.cancelDeliveryItem(delvLstId, auth.EmployeeID))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ยกเลิกการตรวจสอบรายการส่งมอบสินค้าโปรโมชั่น เสร็จสิ้น.'); bindDataParentPage();", true);
                return;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ไม่สามารถยกเลิกการตรวจสอบรายการส่งมอบสินค้าโปรโมชั่นได้ กรุณาลองใหม่อีกครั้ง !! ');", true);
                return;
            }
            //dbo.spCancelDeliveryItem

        }
    }
}