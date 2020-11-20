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
    public partial class MasterItemDetail : System.Web.UI.Page
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
            bindDDLItemCountMethod();
            //bindDDLItemStock();
            bindDDLItemStatus();
            bindDDLItemGroup();
            disableWaitInDDL();
            string mode = Request.QueryString["mode"] + "";
            if (mode == "Edit")
            {
                loadDataForEdit(Request.QueryString["MasterItemId"] + "");
            }
        }

        private void loadDataForEdit(string masterItemId)
        {
            int MasterItemId = 0;
            int.TryParse(masterItemId, out MasterItemId);
            Entities.MasterItemInfo item = new Entities.MasterItemInfo();
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            item.MasterItemId = MasterItemId;
            item.ItemCostEnd = 99999999;
            DataTable dt = dasp.getDataMasterItem(item);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    hdfMasterItemId.Value = dr["MasterItemId"] + "";
                    txtItemNo.Text = dr["ItemNo"] + "";
                    txtItemName.Text = dr["ItemName"] + "";

                    decimal cost = 0;
                    decimal.TryParse(dr["ItemBasePricePerUnit"] + "", out cost);
                    decimal costInccVat = 0;
                    decimal.TryParse(dr["ItemPricePerUnit"] + "", out costInccVat);

                    txtItemCost.Text = cost.ToString("#,##0.00");//dr["ItemBasePricePerUnit"] + "";
                    txtItemCostIncVat.Text = costInccVat.ToString("#,##0.00"); //dr["ItemPricePerUnit"] + "";
                    
                    ddlItemCountMethod.SelectedIndex = ddlItemCountMethod.Items.IndexOf(ddlItemCountMethod.Items.FindByValue(dr["ItemCountMethod"] + ""));
                    //ddlItemStock.SelectedIndex = ddlItemStock.Items.IndexOf(ddlItemStock.Items.FindByValue(dr["ItemStock"] + ""));
                    ddlItemStatus.SelectedIndex = ddlItemStatus.Items.IndexOf(ddlItemStatus.Items.FindByValue(dr["ItemStatus"] + ""));
                    ddlItemGroup.SelectedIndex = ddlItemGroup.Items.IndexOf(ddlItemGroup.Items.FindByValue(dr["MasterItemGroupId"] + ""));
                    if (dr["ItemForceExpire"] + "" == "Y")
                    {
                        chkForceExpire.Checked = true;
                    }

                    if (Convert.ToInt32(dr["ItemInUse"]) > 0 )
                    {
                        btnSave.Attributes.Add("style", "display:none;");
                        btnClose.Attributes.Add("style", "display:none;");
                        lbWarinigNoSave.Attributes.Remove("style");
                    }
                }
            }
            txtItemNo.Attributes.Add("onclick", "blur();");
            txtItemName.Attributes.Add("onclick", "blur();");
            //txtItemCost.Attributes.Add("onclick", "blur();");
            //txtItemCostIncVat.Attributes.Add("onclick", "blur();");

            //ddlItemCountMethod.Attributes.Add("ReadOnly", "true");
            // ddlItemStatus.Attributes.Add("onclick", "blur();");

        }

        private void bindDDLItemCountMethod()
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            DataTable dt = dasp.getDataStatus("1");
            ddlItemCountMethod.DataSource = dt;
            ddlItemCountMethod.DataTextField = "StatusText";
            ddlItemCountMethod.DataValueField = "StatusValue";
            ddlItemCountMethod.DataBind();
            ddlItemCountMethod.Items.Insert(0,new ListItem("-- โปรดระบุ --",""));
        }

        //private void bindDDLItemStock()
        //{
        //    Class.DAStockPromotion dasp = new Class.DAStockPromotion();
        //    DataTable dt = dasp.getDataStatus("2");
        //    ddlItemStock.DataSource = dt;
        //    ddlItemStock.DataTextField = "StatusText";
        //    ddlItemStock.DataValueField = "StatusValue";
        //    ddlItemStock.DataBind();
        //    ddlItemStock.Items.Insert(0, new ListItem("-- โปรดระบุ --", ""));
        //}
        private void bindDDLItemGroup()
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            Entities.MasterItemGroupInfo itmGrp = new Entities.MasterItemGroupInfo();
            itmGrp.MasterItemGroupId = 0;
            itmGrp.ItemGroupName = "";
            DataTable dt = dasp.getDataMasterItemGroup(itmGrp);
            ddlItemGroup.DataSource = dt;
            ddlItemGroup.DataTextField = "MasterItemGroupName";
            ddlItemGroup.DataValueField = "MasterItemGroupId";
            ddlItemGroup.DataBind();
            ddlItemGroup.Items.Insert(0, new ListItem("-- โปรดระบุ --", ""));
        }
        private void bindDDLItemStatus()
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            DataTable dt = dasp.getDataStatus("3");
            ddlItemStatus.DataSource = dt;
            ddlItemStatus.DataTextField = "StatusText";
            ddlItemStatus.DataValueField = "StatusValue";
            ddlItemStatus.DataBind();
        }

        private void disableWaitInDDL()
        {
            ddlItemCountMethod.Items.Remove(ddlItemCountMethod.Items.FindByValue("0"));
            //ddlItemStock.Items.Remove(ddlItemStock.Items.FindByValue("0"));
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            AutorizeData auth = (AutorizeData)Session["userInfo_" + Session.SessionID];

            if (ddlItemGroup.SelectedItem.Value == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('โปรดระบุหมวดสินค้า !!');", true);
                return;
            }
            //if (ddlItemStock.SelectedItem.Value == "")
            //{
            //    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('โปรดระบุสต็อกกลาง !!');", true);
            //    return;
            //}
            if (ddlItemCountMethod.SelectedItem.Value == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('โปรดระบุวิธีการตรวจนับ !!');", true);
                return;
            }

            Entities.MasterItemInfo item = new Entities.MasterItemInfo();
            string mode = Request.QueryString["mode"] + "";
            if (mode == "Edit")
            {
                int itemId = 0;
                int.TryParse(Request.QueryString["MasterItemId"] + "", out itemId);
                item.MasterItemId = itemId;
            }
            int itemGroup = 0;
            int.TryParse(ddlItemGroup.SelectedItem.Value, out itemGroup);
            item.MasterItemGroupId = itemGroup;
            item.ItemNo = txtItemNo.Text;
            item.ItemNo = txtItemNo.Text;
            item.ItemName = txtItemName.Text;
            decimal itemCost= 0;
            decimal.TryParse(txtItemCost.Text, out itemCost);
            item.ItemCost = itemCost;
            decimal itemCostIncVat= 0;
            decimal.TryParse(txtItemCostIncVat.Text, out itemCostIncVat);
            item.ItemCostIncVat = itemCostIncVat;
            item.ItemCountMethod = ddlItemCountMethod.SelectedItem.Value;
            item.ItemStock = "1"; // ddlItemStock.SelectedItem.Value;
            if (chkForceExpire.Checked) { item.ItemForceExpire = "Y"; }
            else { item.ItemForceExpire = "N"; }
            if (mode == "Edit")
            {
                item.ItemStatus = ddlItemStatus.SelectedItem.Value;
            }
            item.UpdateBy = auth.EmployeeID;

            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            if (mode == "Edit")
            {
                dasp.UpdateDataMasterItem(item);
            }
            else
            {
                dasp.InsertDataMasterItem(item);
            }

            ScriptManager.RegisterStartupScript(this, GetType(), "js", "bindDataParentPage('" + item.ItemNo + "');", true);
        }


        protected void btnClose_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this, GetType(), "js", "closePage();", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "js", "parent.jQuery.colorbox.close();", true);
        }
    }
}