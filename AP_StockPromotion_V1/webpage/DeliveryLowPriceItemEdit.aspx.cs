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
    public partial class DeliveryLowPriceItemEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                initPage();
            }
            if (hdfProject.Value != "")
            {
                ddlProject.SelectedIndex = ddlProject.Items.IndexOf(ddlProject.Items.FindByValue(hdfProject.Value));
                ddlInternalOrder.SelectedIndex = ddlInternalOrder.Items.IndexOf(ddlInternalOrder.Items.FindByValue(hdfInternalOrder.Value));
                btnSave.Visible = true;
                //ScriptManager.RegisterStartupScript(this, GetType(), "js", "$('.js-example-basic-single').prop('disabled', true);", true);
            }
            else
            {
                btnSave.Visible = false;
            }
        }

        private void initPage()
        {
            bindDDLProject();
            bindDDLInternalOrder();
        }

        private void bindDDLProject()
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            DataTable dt = dasp.getDataMasterProject();
            ddlProject.DataSource = dt;
            ddlProject.DataTextField = "ProjectName";
            ddlProject.DataValueField = "ProjectID";
            ddlProject.DataBind();
            ddlProject.Items.Insert(0, new ListItem("-- โปรดระบุ --", ""));
        }

        private void bindDDLInternalOrder()
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            DataTable dt = dasp.getDataMasterInternalOrder();
            ddlInternalOrder.DataSource = dt;
            ddlInternalOrder.DataTextField = "FullName";
            ddlInternalOrder.DataValueField = "InternalOrder";
            ddlInternalOrder.DataBind();
            ddlInternalOrder.Items.Insert(0, new ListItem("-- โปรดระบุ --", ""));
        }

        protected void btnSelectProject_Click(object sender, EventArgs e)
        {
            divCheckSerial.Attributes.Add("style", "display:none;");
            //if (txtCostCenter.Text == "")
            //{
            //    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('โปรดระบุ Internal Order !');", true);
            //    return;
            //}
            if (txtDocDate.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('โปรดระบุวันที่ !');", true);
                return;
            }
            //if (txtDocRefNo.Text == "")
            //{
            //    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('โปรดระบุเลขที่เอกสาร !');", true);
            //    return;
            //}
            if (btnSelectProject.Text == "เลือก")
            {
                if (ddlProject.SelectedItem.Value == "")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('โปรดระบุโครงการ !');", true);
                    return;
                }
                if (ddlInternalOrder.SelectedItem.Value == "")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('Internal Order !');", true);
                    return;
                }
                Class.DAStockReturn cls = new Class.DAStockReturn();
                int project_Id = 0;
                int.TryParse(ddlProject.SelectedItem.Value, out project_Id);
                DataTable dtProjectItem = cls.getDataStockLowPriceProject(project_Id);
                dtProjectItem.Columns.Add(new DataColumn("Delivery", typeof(string)));
                Session["dtProjectItem"] = dtProjectItem;
                // string x = cls.lookTable(dtProjectItem);
                if (dtProjectItem.Rows.Count > 0)
                {
                    hdfProject.Value = ddlProject.SelectedItem.Value;
                    hdfInternalOrder.Value = ddlInternalOrder.SelectedItem.Value;
                    divCheckSerial.Attributes.Add("style", "");
                    txtDocDate.ReadOnly = true;
                    // txtDocRefNo.ReadOnly = true;
                    // txtCostCenter.ReadOnly = true;
                    bindData();
                    btnSelectProject.Text = "ยกเลิก";
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "$('.js-example-basic-single').prop('disabled', true);", true);
                    return;
                }
            }
            else
            {
                Session["dtProjectItem"] = null;
                hdfProject.Value = "";
                hdfInternalOrder.Value = "";
                txtDocDate.ReadOnly = false;
                //txtDocRefNo.ReadOnly = false;
                // txtCostCenter.ReadOnly = false;
                grdData.DataSource = null;
                grdData.DataBind();
                btnSelectProject.Text = "เลือก";
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "$('.js-example-basic-single').prop('disabled', false);", true);
                return;
            }
        }

        protected void grdData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField grdHdfItemCountMethod = (HiddenField)e.Row.FindControl("grdHdfItemCountMethod");
                ImageButton imgDelvSel = (ImageButton)e.Row.FindControl("imgDelvSel");
                if (grdHdfItemCountMethod.Value == "1" || grdHdfItemCountMethod.Value == "2")
                {
                    imgDelvSel.Attributes.Add("style", "display:none;");
                }
            }
        }

        protected void grdData_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SelItemAmt")
            {                
                // ScriptManager.RegisterStartupScript(this, GetType(), "js", "popupStockDeliveryItemEditCheckAmount('"+ e.CommandArgument +"');", true);
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "$('.js-example-basic-single').prop('disabled', true); OpenColorBox('StockDeliveryItemEditCheckAmount.aspx?MasterItemId=" + e.CommandArgument + "','90%','90%'); $('.js-example-basic-single').prop('disabled', true);", true);
            }
        }

        protected void btnSelectAmt_Click(object sender, EventArgs e)
        {
            bindData();
            ddlProject.SelectedIndex = ddlProject.Items.IndexOf(ddlProject.Items.FindByValue(hdfProject.Value));
            ScriptManager.RegisterStartupScript(this, GetType(), "js", "$('.js-example-basic-single').prop('disabled', true);", true);

        }

        protected void imgCheckSerial_Click(object sender, ImageClickEventArgs e)
        {
            if (txtSerial.Text.Trim() != "")
            {
                DataTable dtProjectItem = (DataTable)Session["dtProjectItem"];
                DataRow[] dr = dtProjectItem.Select("Serial='" + txtSerial.Text.Trim() + "'");
                if (dr.Length > 0)
                {
                    dr[0]["Delivery"] = "Y";
                    ddlProject.SelectedIndex = ddlProject.Items.IndexOf(ddlProject.Items.FindByValue(hdfProject.Value));
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "$('.js-example-basic-single').prop('disabled', true);", true);
                    txtSerial.Text = "";
                    bindData();
                    txtSerial.Focus();
                    return;
                }
                else
                {
                    ddlProject.SelectedIndex = ddlProject.Items.IndexOf(ddlProject.Items.FindByValue(hdfProject.Value));
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "$('.js-example-basic-single').prop('disabled', true); alert('ไม่พบสินค้า Serial:" + txtSerial.Text.Trim() + "');", true);
                    txtSerial.Focus();
                    return;
                }
            }
            txtSerial.Focus();
            ScriptManager.RegisterStartupScript(this, GetType(), "js", "$('.js-example-basic-single').prop('disabled', true);", true);
        }

        private void bindData()
        {
            DataTable dtProjectItem = (DataTable)Session["dtProjectItem"];
            dtProjectItem.DefaultView.RowFilter = "";
            DataTable dtProjectItemList = dtProjectItem.DefaultView.ToTable(true, "MasterItemId", "MasterItemName", "ItemCountMethod");
            dtProjectItemList.Columns.Add(new DataColumn("StockAmount", typeof(int)));
            dtProjectItemList.Columns.Add(new DataColumn("StockDelivery", typeof(int)));
            foreach (DataRow dr in dtProjectItemList.Rows)
            {
                dr["StockAmount"] = dtProjectItem.Compute("Count(ItemId)", "MasterItemId=" + dr["MasterItemId"] + "");
                dr["StockDelivery"] = dtProjectItem.Compute("Count(ItemId)", "MasterItemId=" + dr["MasterItemId"] + " and Delivery='Y'");
            }
            dtProjectItemList.AcceptChanges();
            grdData.DataSource = dtProjectItemList;
            grdData.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DataTable dtProjectItem = (DataTable)Session["dtProjectItem"];
            dtProjectItem.DefaultView.RowFilter = "Delivery = 'Y'";
            DataTable dtItemNo = dtProjectItem.DefaultView.ToTable(true, "ItemNo");
            string TrLst = "";
            foreach (DataRow drItemNo in dtItemNo.Rows)
            {
                TrLst += drItemNo["ItemNo"] + ":";
                dtProjectItem.DefaultView.RowFilter = "Delivery = 'Y' AND ItemNo = '" + drItemNo["ItemNo"] + "'";
                DataTable dtTrLst = dtProjectItem.DefaultView.ToTable();
                foreach (DataRow drTr in dtTrLst.Rows)
                {
                    TrLst += drTr["TrId"] + "|";
                }
                TrLst = TrLst.Remove(TrLst.Length - 1, 1) + ";";
            }


            TrLst = TrLst.Remove(TrLst.Length - 1, 1);
            Class.DADelivery cls = new Class.DADelivery();

            AutorizeData auth = (AutorizeData)Session["userInfo_" + Session.SessionID];
            string DelvPromotionId = "XXX";// txtDocRefNo.Text;
            string UserId = auth.EmployeeID;
            string DocDate = txtDocDate.Text;
            string CostCenter = ddlInternalOrder.SelectedItem.Value; // txtCostCenter.Text;
            string ProjectID = ddlProject.SelectedItem.Value;
            string msgErr = "";

            if (cls.deliveryItemLowPrice(ref DelvPromotionId, ProjectID, UserId, DocDate, CostCenter, TrLst, ref msgErr))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('[" + DelvPromotionId + "] บันทึกการส่งมอบสินค้าโปรโมชั่น(Marketing) เสร็จสิ้น.'); window.location = 'DeliveryLowPriceItem.aspx?bindData=Y';", true);
                return;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ไม่สามารถตัดสต๊อกโครงการได้ กรุณาลองใหม่อีกครั้ง !! " + msgErr.Replace("'", "") + "');", true);
                return;
            }
        }

    }
}