using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AP_StockPromotion_V1.webpage
{
    public partial class ReportStockBalance : System.Web.UI.Page
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
            //bindChkReqType();
            bindChkMatType();
        }

        //private void bindChkReqType()
        //{
        //    Class.DAStockPromotion dasp = new Class.DAStockPromotion();
        //    DataTable dt = dasp.getDataStatus("2");
        //    chkStockType.DataSource = dt;
        //    chkStockType.DataTextField = "StatusText";
        //    chkStockType.DataValueField = "StatusValue";
        //    chkStockType.DataBind();
        //    chkStockType.Items.Remove(chkStockType.Items.FindByValue("0"));
        //}

        private void bindChkMatType()
        {
            Class.DAStockPromotion cls = new Class.DAStockPromotion();
            Entities.MasterItemGroupInfo mit = new Entities.MasterItemGroupInfo();
            mit.MasterItemGroupId = 0;
            mit.ItemGroupName = "";
            DataTable dt = cls.getDataMasterItemGroup(mit);
            chkMatType.DataSource = dt;
            chkMatType.DataTextField = "MasterItemGroupName";
            chkMatType.DataValueField = "MasterItemGroupId";
            chkMatType.DataBind();
        }

        protected void chkAllMatType_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ListItem chk in chkMatType.Items)
            {
                chk.Selected = chkAllMatType.Checked;
            }
        }

        protected void btnExportReport_Click(object sender, EventArgs e)
        {
            //string stockType = "";
            //string stockTypeText = "";
            //foreach (ListItem chk in chkStockType.Items)
            //{
            //    if (chk.Selected)
            //    {
            //        stockType += "," + chk.Value;
            //        stockTypeText += "," + chk.Text;
            //    }
            //}
            //if (stockType.Length == 0)
            //{
            //    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('กรุณาระบุประเภทโปรโมชั่น!!');", true);
            //    return;
            //}
            string matType = "";
            string matTypeText = "";
            foreach (ListItem chk in chkMatType.Items)
            {
                if (chk.Selected)
                {
                    matType += "," + chk.Value;
                    matTypeText += "," + chk.Text;
                }
            }
            if (matType.Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('กรุณาระบุหมวดสินค้า!!');", true);
                return;
            }
            Entities.FormatDate convertDate = new Entities.FormatDate();
            DateTime bDate = new DateTime();
            if (convertDate.getDateFromString(txtDateBeg.Text, ref bDate))
            {
                Session["DateBeg"] = bDate;// new DateTime(bDate.Year, bDate.Month, bDate.Day, 0, 0, 1);//bDate;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('กรุณาระบุวันที่เริ่มต้น!!');", true);
                return;
            }
            DateTime eDate = new DateTime();
            if (convertDate.getDateFromString(txtDateEnd.Text, ref eDate))
            {
                Session["DateEnd"] = new DateTime(eDate.Year, eDate.Month, eDate.Day, 23, 59, 59);//eDate;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('กรุณาระบุวันที่สิ้นสุด!!');", true);
                return;
            }
            
            //Session["StockType"] = stockType.Remove(0, 1);
            //Session["StockTypeText"] = stockTypeText.Remove(0, 1).Replace(",", ", ");
            Session["MasterItemGroup"] = matType.Remove(0, 1);
            Session["MasterItemGroupText"] = matTypeText.Remove(0, 1).Replace(",", ", ");

            string reqRPT = "BL";

            string urlRptRQ01 = "frmReport.aspx?reqRPT=" + reqRPT;
            ScriptManager.RegisterStartupScript(this, GetType(), "js", "Popup60('" + urlRptRQ01 + "');", true);
            return;
        }
    }
}