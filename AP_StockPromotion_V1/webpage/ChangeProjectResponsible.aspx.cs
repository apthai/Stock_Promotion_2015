using AP_StockPromotion_V1.ws_authorize;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AP_StockPromotion_V1.webpage
{
    public partial class ChangeProjectResponsible : System.Web.UI.Page
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
            bindDDLProject();
            bindDDLItem();
            ddlProjectSend.Attributes.Add("onchange", "setValueHdf('" + ddlProjectSend.ClientID + "','" + hdfProjectSend.ClientID + "');");
            ddlProjectReceive.Attributes.Add("onchange", "setValueHdf('" + ddlProjectReceive.ClientID + "','" + hdfProjectReceive.ClientID + "');");
            ddlItem.Attributes.Add("onchange", "setValueHdf('" + ddlItem.ClientID + "','" + hdfItemSend.ClientID + "');");
        }

        private void bindDDLProject()
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            DataTable dt = dasp.getDataMasterProject();
            ddlProjectSend.DataSource = dt;
            ddlProjectSend.DataTextField = "ProjectName";
            ddlProjectSend.DataValueField = "ProjectID";
            ddlProjectSend.DataBind();
            ddlProjectSend.Items.Insert(0, new ListItem("-- เลือก --", ""));


            ddlProjectReceive.DataSource = dt;
            ddlProjectReceive.DataTextField = "ProjectName";
            ddlProjectReceive.DataValueField = "ProjectID";
            ddlProjectReceive.DataBind();
            ddlProjectReceive.Items.Insert(0, new ListItem("-- เลือก --", ""));
        }

        private void bindDDLItem()
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            Entities.MasterItemInfo item = new Entities.MasterItemInfo();
            item.ItemCostEnd = 9999999;
            DataTable dt = dasp.getDataMasterItem(item);
            ddlItem.DataSource = dt;
            ddlItem.DataTextField = "ItemNoName";
            // ddlItem.DataValueField = "ItemNo";
            ddlItem.DataValueField = "MasterItemId";
            ddlItem.DataBind();
            ddlItem.Items.Insert(0, new ListItem("ทั้งหมด", "0"));
        }

        protected void btnShowItemList_Click(object sender, EventArgs e)
        {
            btnSave.Text = "X";
            btnSave.Attributes.Add("style", "display:none;");
            if (btnShowItemList.Text == "แสดงรายการสินค้า")
            {
                if (chkHeaderValue())
                {
                    bindData();
                    //fileMemo.Enabled = false;
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "$('.js-example-basic-single').prop('disabled', true);", true);
                    btnShowItemList.Text = "ยกเลิก/เปลี่ยน";
                    //btnSave.Attributes.Add("style", "");
                }
            }
            else
            {
                Session["ProjectResponsibility"] = null;
                grdData.DataSource = null;
                grdData.DataBind();
                hdfProjectSend.Value = "";
                hdfProjectReceive.Value = "";
                // fileMemo.Enabled = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "$('.js-example-basic-single').prop('disabled', false);", true);
                btnShowItemList.Text = "แสดงรายการสินค้า";
                //btnSave.Attributes.Add("style", "display:none;");
            }
        }

        private bool chkHeaderValue()
        {
            bool rst = true;
            if (ddlProjectSend.SelectedItem.Value == "")
            {
                execJScript("alert('กรุณาระบุโครงการที่ต้องการ !!'); $('#" + ddlProjectSend.ClientID + "').focus();");
                return false;
            }
            if (ddlProjectReceive.SelectedItem.Value == "")
            {
                execJScript("alert('กรุณาระบุผู้รับผิดชอบที่ต้องการถ่ายโอนสิทธิ์ !!'); $('#" + ddlProjectReceive.ClientID + "').focus();");
                return false;
            }
            return rst;
        }

        private void bindData()
        {
            int project_id = 0;
            int.TryParse(ddlProjectSend.SelectedItem.Value, out project_id);
            int Item = 0;
            int.TryParse(ddlItem.SelectedItem.Value, out Item);
            Class.DAChangeResponsible cls = new Class.DAChangeResponsible();
            DataTable dtItemsResponsibility = cls.getItemsResponsibilityByProject(project_id, Item);
            DataTable dt = dtItemsResponsibility.DefaultView.ToTable(true, "Serial", "Barcode", "ItemName", "Model", "Color", "Dimension", "Weight", "Price", "ProduceDate", "ExpireDate", "Detail", "Remark", "UserResponse", "FullName", "ReqDocNo", "UnitNo");
            Session["ProjectResponsibility"] = dtItemsResponsibility;
            dt.Columns.Add(new DataColumn("Amount", typeof(int)));
            foreach (DataRow dr in dt.Rows)
            {
                dr["Amount"] = dtItemsResponsibility.Compute("COUNT(ItemId)", getStrFilter(dr));
            }
            dt.DefaultView.RowFilter = "";
            dt.AcceptChanges();
            grdData.DataSource = dt;
            grdData.DataBind();
            btnSave.Text = ddlProjectSend.SelectedItem.Text + " >> " + ddlProjectReceive.SelectedItem.Text;
            btnSave.Attributes.Add("style", "");
        }

        protected void grdData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //  hdfAmount || grdTxtAmount
                HiddenField hdfAmount = (HiddenField)e.Row.FindControl("hdfAmount");
                TextBox txtAmount = (TextBox)e.Row.FindControl("grdTxtAmount");
                txtAmount.Attributes.Add("onkeyup", "chkAmountChange('" + txtAmount.ClientID + "','" + hdfAmount.ClientID + "')");
            }
        }

        private string getStrFilter(DataRow dr)
        {
            string rst = "";
            rst += " isnull(Serial,'') = '" + dr["Serial"] + "' ";
            rst += " and isnull(Barcode,'') = '" + dr["Barcode"] + "' ";
            rst += " and isnull(ItemName,'') = '" + dr["ItemName"] + "' ";
            rst += " and isnull(Model,'') = '" + dr["Model"] + "' ";
            rst += " and isnull(Color,'') = '" + dr["Color"] + "' ";
            rst += " and isnull(Dimension,'') = '" + dr["Dimension"] + "' ";
            rst += " and isnull(Weight,'') = '" + dr["Weight"] + "' ";
            rst += " and isnull(Price,'') = '" + dr["Price"] + "' ";
            if (dr["ProduceDate"] == DBNull.Value)
            {
                rst += " and ProduceDate is null ";
            }
            else
            {
                rst += " and ProduceDate = '" + dr["ProduceDate"] + "'";
            }
            if (dr["ExpireDate"] == DBNull.Value)
            {
                rst += " and ExpireDate is null ";
            }
            else
            {
                rst += " and ExpireDate = '" + dr["ExpireDate"] + "'";
            }
            rst += " and isnull(Detail,'') = '" + dr["Detail"] + "' ";
            rst += " and isnull(Remark,'') = '" + dr["Remark"] + "' ";
            rst += " and isnull(UserResponse,'') = '" + dr["UserResponse"] + "' ";
            rst += " and isnull(FullName,'') = '" + dr["FullName"] + "' ";
            rst += " and isnull(ReqDocNo,'') = '" + dr["ReqDocNo"] + "' ";
            rst += " and isnull(UnitNo,'') = '" + dr["UnitNo"] + "' ";
            return rst;
        }

        private void execJScript(string jscript)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "js", jscript, true);
            return;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Session["ProjectResponsibility"];

            

            string itemList = "";
            foreach (GridViewRow gr in grdData.Rows)
            {
                TextBox txtAmt = (TextBox)gr.FindControl("grdTxtAmount");
                int amt = 0;
                int.TryParse(txtAmt.Text, out amt);

                DataRow[] dr = dt.Select(getStrFilter(gr));
                for (int ii = 0; ii < amt; ii++)
                {
                    itemList += "," + dr[ii]["ItemId"] + "";
                }
            }
            if (itemList.Length > 0)
            {

                AutorizeData auth = (AutorizeData)Session["userInfo_" + Session.SessionID];
                itemList = itemList.Remove(0, 1);

                string projectSend = hdfProjectSend.Value;
                string projectReceive = hdfProjectReceive.Value;
                string itemSend = hdfItemSend.Value;

                ddlProjectSend.SelectedIndex = ddlProjectSend.Items.IndexOf(ddlProjectSend.Items.FindByValue(projectSend));
                ddlProjectReceive.SelectedIndex = ddlProjectReceive.Items.IndexOf(ddlProjectReceive.Items.FindByValue(projectReceive));
                ddlItem.SelectedIndex = ddlItem.Items.IndexOf(ddlItem.Items.FindByValue(itemSend));

                string url = "ChangeProjectResponsibleRequest.aspx?projectSend=" + projectSend + "&projectReceive=" + projectReceive + "&itemList="+ itemList;
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "OpenColorBox('" + url + "','90%','90%'); $('.js-example-basic-single').prop('disabled', true);", true);
                //  คืน

                //  สร้างใบเบิก

                //  จ่าย
            }
        }

        private string getStrFilter(GridViewRow gr)
        {
            string rst = "";
            rst += " isnull(Serial,'') = '" + ((HiddenField)gr.FindControl("hdfSerial")).Value + "' ";
            // rst += " and isnull(Barcode,'') = '" + ((HiddenField)gr.FindControl("hdfBarcode")).Value + "' ";
            rst += " and isnull(ItemName,'') = '" + ((HiddenField)gr.FindControl("hdfItemName")).Value + "' ";
            rst += " and isnull(Model,'') = '" + ((HiddenField)gr.FindControl("hdfModel")).Value + "' ";
            rst += " and isnull(Color,'') = '" + ((HiddenField)gr.FindControl("hdfColor")).Value + "' ";
            rst += " and isnull(Dimension,'') = '" + ((HiddenField)gr.FindControl("hdfDimension")).Value + "' ";
            rst += " and isnull(Weight,'') = '" + ((HiddenField)gr.FindControl("hdfWeight")).Value + "' ";
            rst += " and isnull(Price,'') = '" + ((HiddenField)gr.FindControl("hdfPrice")).Value + "' ";
            //if (((HiddenField)gr.FindControl("hdfProduceDate")).Value == "")
            //{
            //    rst += " and ProduceDate is null ";
            //}
            //else
            //{
            //    rst += " and ProduceDate = '" + ((HiddenField)gr.FindControl("hdfProduceDate")).Value + "'";
            //}
            if (((HiddenField)gr.FindControl("hdfExpireDate")).Value == "")
            {
                rst += " and ExpireDate is null ";
            }
            else
            {
                rst += " and ExpireDate = '" + ((HiddenField)gr.FindControl("hdfExpireDate")).Value + "'";
            }
            rst += " and isnull(Detail,'') = '" + ((HiddenField)gr.FindControl("hdfDetail")).Value + "' ";
            rst += " and isnull(Remark,'') = '" + ((HiddenField)gr.FindControl("hdfRemark")).Value + "' ";

            rst += " and isnull(UserResponse,'') = '" + ((HiddenField)gr.FindControl("hdfUserResponse")).Value + "' ";
            rst += " and isnull(FullName,'') = '" + ((HiddenField)gr.FindControl("hdfFullName")).Value + "' ";
            rst += " and isnull(ReqDocNo,'') = '" + ((HiddenField)gr.FindControl("hdfReqDocNo")).Value + "' ";
            rst += " and isnull(UnitNo,'') = '" + ((HiddenField)gr.FindControl("hdfUnitNo")).Value + "' ";

            return rst;
        }

    }
}