using AP_StockPromotion_V1.ws_authorize;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AP_StockPromotion_V1.web
{
    public partial class StockReturnItemEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                initPage();              
            }
            if (hdfSelProject.Value != "")
            {
                ddlProject.SelectedIndex = ddlProject.Items.IndexOf(ddlProject.Items.FindByValue(hdfSelProject.Value));
            }
        }

        private void initPage()
        {
            bindDDLProject();
        }

        private void bindDDLProject()
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            string Message = "";
            //DataTable dt = dasp.getDataMasterProjectCostCenter(true);
            //ddlProject.DataTextField = "CostCenterName";
            //ddlProject.DataValueField = "CostCenter";
            //แก้ไข Job REQ2017003495 สิ่งที่ทำนั้นดึง code มาผิดทำให้ใช้ store ผิด 2017-05-23
            DataTable dt = dasp.getDataMasterProject(true);
            ddlProject.DataSource = dt;
            
            ddlProject.DataTextField = "ProjectName";
            ddlProject.DataValueField = "ProjectID";
            ddlProject.DataBind();
            ddlProject.Items.Insert(0, new ListItem("-- โปรดระบุ --", ""));
        }

        protected void btnSelectProject_Click(object sender, EventArgs e)
        {
            divCheckSerial.Attributes.Add("style", "display:none;");
            if (btnSelectProject.Text == "เลือก")
            {
                if (ddlProject.SelectedItem.Value == "")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('โปรดระบุโครงการ');", true);
                    return;
                }
                hdfSelProject.Value = ddlProject.SelectedItem.Value;
                Class.DAStockReturn cls = new Class.DAStockReturn();
                string project_Id = ddlProject.SelectedItem.Value;
                DataTable dtProjectItem = cls.getDataStockProject(project_Id);
                dtProjectItem.Columns.Add(new DataColumn("Return", typeof(string)));
                Session["dtProjectItem"] = dtProjectItem;
                // string x = cls.lookTable(dtProjectItem);
                if (dtProjectItem.Rows.Count > 0)
                {
                    divCheckSerial.Attributes.Add("style", "");
                    bindData();
                    btnSelectProject.Text = "ยกเลิก";
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "$('.js-example-basic-single').prop('disabled', true);", true);
                    return;
                }
                else
                {
                    hdfSelProject.Value = "";
                    Session["dtProjectItem"] = null;
                    grdData.DataSource = null;
                    grdData.DataBind();
                    btnSelectProject.Text = "เลือก";
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "$('.js-example-basic-single').prop('disabled', false);", true);
                    return;
                }
            }
            else
            {
                hdfSelProject.Value = "";
                Session["dtProjectItem"] = null;
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
                ImageButton imgCancelReturn = (ImageButton)e.Row.FindControl("imgCancelReturn");
                if (grdHdfItemCountMethod.Value == "1" || grdHdfItemCountMethod.Value == "2")
                {
                    imgCancelReturn.Attributes.Add("style", "display:none;");
                }
            }
        }

        protected void grdData_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CancelReturn")
            {
                // ScriptManager.RegisterStartupScript(this, GetType(), "js", "popupStockReturnItemEditCheckAmount('"+ e.CommandArgument +"');", true);
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "OpenColorBox('StockReturnItemEditCheckAmount.aspx?MasterItemId=" + e.CommandArgument + "','90%','90%');", true);
            }
        }

        protected void btnSelectAmt_Click(object sender, EventArgs e)
        {
            bindData();
        }

        protected void imgCheckSerial_Click(object sender, ImageClickEventArgs e)
        {
            if (txtSerial.Text.Trim() != "")
            {
                DataTable dtProjectItem = (DataTable)Session["dtProjectItem"];
                DataRow[] dr = dtProjectItem.Select("Serial='" + txtSerial.Text.Trim() + "'");
                if (dr.Length > 0)
                {
                    dr[0]["Return"] = "Y";
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ไม่พบสินค้า Serial:" + txtSerial.Text.Trim() + "');", true);
                    return;
                }
                txtSerial.Text = "";
                bindData();
            }
            txtSerial.Focus();
        }

        private void bindData()
        {
            DataTable dtProjectItem = (DataTable)Session["dtProjectItem"];
            dtProjectItem.DefaultView.RowFilter = "";
            DataTable dtProjectItemList = dtProjectItem.DefaultView.ToTable(true, "MasterItemId", "MasterItemName", "ItemCountMethod");
            dtProjectItemList.Columns.Add(new DataColumn("StockAmount", typeof(int)));
            dtProjectItemList.Columns.Add(new DataColumn("StockReturn", typeof(int)));
            foreach (DataRow dr in dtProjectItemList.Rows)
            {
                dr["StockAmount"] = dtProjectItem.Compute("Count(ItemId)", "MasterItemId=" + dr["MasterItemId"] + "");
                dr["StockReturn"] = dtProjectItem.Compute("Count(ItemId)", "MasterItemId=" + dr["MasterItemId"] + " and Return='Y'");
            }
            dtProjectItemList.AcceptChanges();
            grdData.DataSource = dtProjectItemList;
            grdData.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DataTable dtProjectItem = (DataTable)Session["dtProjectItem"];
            string itemLst = "";
            foreach (DataRow dr in dtProjectItem.Rows)
            {
                if (dr["Return"] + "" == "Y")
                {
                    itemLst += "," + dr["ItemId"];
                }
            }
            if (itemLst != "")
            {
                itemLst = itemLst.Remove(0, 1);

                int project_id = 0;
                if (int.TryParse(ddlProject.SelectedItem.Value, out project_id))
                {
                    AutorizeData auth = (AutorizeData)Session["userInfo_" + Session.SessionID];

                    Class.DAStockReturn cls = new Class.DAStockReturn();
                    if (cls.returnItem(itemLst, project_id, auth.EmployeeID, ""))
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('บันทึกข้อมูลเสร็จสิ้น'); window.location.replace('StockReturnItem.aspx');", true);
                        return;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('บันทึกข้อมูลผิดพลาด กรุณาติดต่อผู้ดูแลระบบ !!');", true);
                        return;
                    }
                }

            }
        }

        protected void btnSaveX_Click(object sender, EventArgs e)
        {
            DataTable dtProjectItem = (DataTable)Session["dtProjectItem"];
            if (dtProjectItem == null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('คุณยังไม่ได้ทำการเลือกสินค้าโปรโมชั่นที่ต้องการคืน !!');", true);
                return;
            }
            string itemLst = "";
            foreach (DataRow dr in dtProjectItem.Rows)
            {
                if (dr["Return"] + "" == "Y")
                {
                    itemLst += "," + dr["ItemId"];
                }
            }
            if (itemLst != "")
            {
                itemLst = itemLst.Remove(0, 1);

                int project_id = 0;
                if (int.TryParse(ddlProject.SelectedItem.Value, out project_id))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "OpenColorBox('StockReturnItemReason.aspx?itemLst="+ itemLst + "&project_id="+ project_id + "','75%','63%');", true);
                    return;
                }

            }


        }
    }
}