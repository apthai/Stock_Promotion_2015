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
    public partial class MasterItemGroupDetail : System.Web.UI.Page
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
            string mode = Request.QueryString["mode"] + "";
            //bindDDLItemCountMethod();
            //bindDDLItemStock();  
            if (mode == "Edit")
            {
                loadDataForEdit(Request.QueryString["MasterItemGroupId"] + "");
            }
        }

        //private void bindDDLItemCountMethod()
        //{
        //    Class.DAStockPromotion dasp = new Class.DAStockPromotion();
        //    DataTable dt = dasp.getDataStatus("1");
        //    ddlItemCountMethod.DataSource = dt;
        //    ddlItemCountMethod.DataTextField = "StatusText";
        //    ddlItemCountMethod.DataValueField = "StatusValue";
        //    ddlItemCountMethod.DataBind();
        //}

        //private void bindDDLItemStock()
        //{
        //    Class.DAStockPromotion dasp = new Class.DAStockPromotion();
        //    DataTable dt = dasp.getDataStatus("2");
        //    ddlItemStock.DataSource = dt;
        //    ddlItemStock.DataTextField = "StatusText";
        //    ddlItemStock.DataValueField = "StatusValue";
        //    ddlItemStock.DataBind();
        //}

        private void loadDataForEdit(string masterItemGroupId)
        {
            int MasterItemGroupId = 0;
            int.TryParse(masterItemGroupId, out MasterItemGroupId);
            Entities.MasterItemGroupInfo item = new Entities.MasterItemGroupInfo();
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            item.MasterItemGroupId = MasterItemGroupId;
            DataTable dt = dasp.getDataMasterItemGroup(item);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    hdfMasterItemGroupId.Value = dr["MasterItemGroupId"] + "";
                    txtMasterItemGroupName.Text = dr["MasterItemGroupName"] + "";
                    //string ItemCountMethod = dr["ItemCountMethod"] + "";
                    //string ItemStock = dr["ItemStock"] + "";
                    //ddlItemCountMethod.SelectedIndex = ddlItemCountMethod.Items.IndexOf(ddlItemCountMethod.Items.FindByValue(ItemCountMethod));
                    //ddlItemStock.SelectedIndex = ddlItemStock.Items.IndexOf(ddlItemStock.Items.FindByValue(ItemStock));
                    //if (dr["ItemForceExpire"] + "" == "Y") { chkForceExpire.Checked = true; }
                    //else { chkForceExpire.Checked = false; }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            AutorizeData auth = (AutorizeData)Session["userInfo_" + Session.SessionID];
            Entities.MasterItemGroupInfo itemGrp = new Entities.MasterItemGroupInfo();
            string mode = Request.QueryString["mode"] + "";
            if (mode == "Edit")
            {
                int itemId = 0;
                int.TryParse(hdfMasterItemGroupId.Value + "", out itemId);
                itemGrp.MasterItemGroupId = itemId;
            }
            itemGrp.ItemGroupName = txtMasterItemGroupName.Text;
            //itemGrp.ItemCountMethod = ddlItemCountMethod.SelectedItem.Value;
            //itemGrp.ItemStock = ddlItemStock.SelectedItem.Value;
            itemGrp.UpdateBy = auth.EmployeeID;
            //if (chkForceExpire.Checked) { itemGrp.ItemForceExpire = "Y"; }
            //else { itemGrp.ItemForceExpire = "N"; }

            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            if (mode == "Edit")
            {
                dasp.UpdateDataMasterItemGroup(itemGrp);
            }
            else
            {
                dasp.InsertDataMasterItemGroup(itemGrp);
            }

            ScriptManager.RegisterStartupScript(this, GetType(), "js", "bindDataParentPage('" + txtMasterItemGroupName.Text + "');", true);
        }


        protected void btnClose_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this, GetType(), "js", "closePage();", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "js", "parent.jQuery.colorbox.close();", true);
        }
    }
}