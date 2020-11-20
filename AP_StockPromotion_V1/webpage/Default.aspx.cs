using AP_StockPromotion_V1.Class;
using AP_StockPromotion_V1.ws_authorize;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Services;
using System.Web.UI;

namespace AP_StockPromotion_V1.web
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                AutorizeData autorizeDatum = new AutorizeData();
                autorizeDatum = (AutorizeData)Session[string.Concat("userInfo_", Session.SessionID)] ?? new AutorizeData();

                if ((autorizeDatum.EmployeeID ?? "") != "") {

                    string EmpCode = autorizeDatum.EmployeeID;

                    DAStockPromotion dasp = new DAStockPromotion();
                    string GUID = dasp.GetGUIDByEmpCode(EmpCode);

                    hlinkReportStockBakance.NavigateUrl = hlinkReportStockBakance.NavigateUrl.Replace("[@GUID]", GUID);
                }
            }

            if (!GlobalValiable.IsPath)
            {
                GlobalValiable.IsPath = true;
                GlobalValiable.CrystalReportPath = Server.MapPath("../rpt/");
                GlobalValiable.PDFReportPath = Server.MapPath("../pdf/");
            }

        }

        protected void NewRecordBTN_Click(object sender, EventArgs e)
        {
            //OpenColorBox
            ScriptManager.RegisterStartupScript(this, GetType(), "js", "OpenColorBox('" + "MasterItem.aspx" + "');", true);
            return;
        }


        /* = Auto Complete = */
        [WebMethod]
        public static string[] GetCategoryMasterItem(string itemName)
        {
            List<string> retCategory = new List<string>();
            Entities.MasterItemInfo itm = new Entities.MasterItemInfo();
            itm.MasterItemId = 0;
            itm.ItemNo = "";
            itm.ItemName = itemName;
            itm.ItemUnit = "";
            itm.ItemCostBeg = 0;
            itm.ItemCostEnd = 999999;
            itm.ItemCountMethod = "";
            itm.ItemStock = "";
            itm.ItemStatus = "";

            Class.DAStockPromotion cls = new Class.DAStockPromotion();
            DataTable dt = cls.getDataMasterItem(itm);

            foreach (DataRow dr in dt.Rows)
            {
                retCategory.Add(dr["ItemName"] + "");
            }
            return retCategory.ToArray();
        }

        [WebMethod]
        public static string[] GetCategoryMasterCompany(string cmpName)
        {
            List<string> lstComanyName = new List<string>();
            Class.DAStockPromotion cls = new Class.DAStockPromotion();
            DataTable dt = cls.getDataMasterCompany();

            dt.DefaultView.RowFilter = "FullName like '%" + cmpName + "%'";
            foreach (DataRow dr in dt.DefaultView.ToTable().Rows)
            {
                lstComanyName.Add(dr["FullName"] + "");
            }
            return lstComanyName.ToArray();
        }

        [WebMethod]
        public static string[] GetCategoryMasterProject(string prjName)
        {
            List<string> lstProjectName = new List<string>();
            Class.DAStockPromotion cls = new Class.DAStockPromotion();
            DataTable dt = cls.getDataMasterProject();

            dt.DefaultView.RowFilter = "ProjectName like '%" + prjName + "%'";
            foreach (DataRow dr in dt.DefaultView.ToTable().Rows)
            {
                lstProjectName.Add(dr["ProjectName"] + "");
            }
            return lstProjectName.ToArray();
        }



    }
}