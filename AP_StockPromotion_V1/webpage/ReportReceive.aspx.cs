using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AP_StockPromotion_V1.webpage
{
    public partial class ReportReceive : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnExportReport_Click(object sender, EventArgs e)
        {
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

            string reqRPT = "Receive";

            string urlRptRQ01 = "frmReport.aspx?reqRPT=" + reqRPT;
            ScriptManager.RegisterStartupScript(this, GetType(), "js", "Popup60('" + urlRptRQ01 + "');", true);
            return;
        }
    }
}