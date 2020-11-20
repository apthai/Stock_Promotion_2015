using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AP_StockPromotion_V1.webpage
{
    public partial class DeliveryItem : System.Web.UI.Page
    {
        Entities.FormatDate convertDate = new Entities.FormatDate();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                initPage();
                // bindGrid();
            }
        }

        private void initPage()
        {
            clearSelectDelv();
            //Session["dtDeliveryList"] = null;
            Session["grdSort"] = null; Session["dtGrd"] = null; Session["grdPage"] = null;
            bindDDLProject();
            bindDDLItem();
            //bindDDLConfirmStatus();
            string rqBind = Request.QueryString["bindData"] + "";
            if (rqBind == "Y") { }
        }

        private void bindDDLProject()
        {
            ddlProject.Items.Clear();
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            DataTable dt = dasp.getDataMasterProject();
            ddlProject.DataSource = dt;
            ddlProject.DataTextField = "ProjectName";
            ddlProject.DataValueField = "ProjectCode";
            ddlProject.DataBind();
            ddlProject.Items.Insert(0, new ListItem("ทั้งหมด", "0"));
        }

        private void bindDDLItem()
        {
            ddlItem.Items.Clear();
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            Entities.MasterItemInfo item = new Entities.MasterItemInfo();
            item.ItemCostEnd = 9999999;
            DataTable dt = dasp.getDataMasterItem(item);
            ddlItem.DataSource = dt;
            ddlItem.DataTextField = "ItemNoName";
            ddlItem.DataValueField = "ItemNo";
            // ddlItem.DataValueField = "MasterItemId";
            ddlItem.DataBind();
            ddlItem.Items.Insert(0, new ListItem("ทั้งหมด", ""));
        }

        protected void btnDeliveryItemLowPrice_Click(object sender, EventArgs e)
        {
            Response.Redirect("DeliveryLowPriceItemEdit.aspx?mode=Create");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            clearSelectDelv();
            DateTime dateBeg = new DateTime();
            if (!convertDate.getDateFromString(txtDateFrom.Text, ref dateBeg))
            {
                dateBeg = DateTime.Now.AddYears(-20);
            }
            DateTime dateEnd = new DateTime();
            if (!convertDate.getDateFromString(txtDateTo.Text, ref dateEnd))
            {
                dateEnd = DateTime.Now.AddYears(20);
            }

            string IsPostAcc = "", IsConfirm = "";
            if (cbxDelvStatus.SelectedValue == "1") { IsConfirm = "N"; IsPostAcc = "N"; };
            if (cbxDelvStatus.SelectedValue == "2") { IsConfirm = "Y"; IsPostAcc = "N"; };
            if (cbxDelvStatus.SelectedValue == "3") { IsConfirm = "Y"; IsPostAcc = "Y"; };

            Class.DADelivery cls = new Class.DADelivery();
            DataTable dt = cls.GetDataDeliveryItem(txtDeliveryNo.Text, ddlProject.SelectedItem.Value, ddlItem.SelectedItem.Value, dateBeg, dateEnd, txtWBS.Text, "N", IsConfirm, IsPostAcc);

            if (dt != null)
            {
                DataTable dtGrd = dt.DefaultView.ToTable(true, "DelvLstId", "isConfirm", "isPostAcc", "DelvPromotionId", "DelvDate", "ItemId", "ItemName", "CostCenter", "ProjectCode", "ProjectName", "WBS", "UnitNo");
                dtGrd.Columns.Add(new DataColumn("Amount", typeof(int)));
                dtGrd.Columns.Add(new DataColumn("StatusText", typeof(string)));
                foreach (DataRow dr in dtGrd.Rows)
                {
                    string cond = "ISNULL(DelvLstId,'') = '" + dr["DelvLstId"] + "' AND ISNULL(isConfirm,'') = '" + dr["isConfirm"] + "' AND ISNULL(isPostAcc,'') = '" + dr["isPostAcc"] + "' AND ISNULL(DelvPromotionId,'') = '" + dr["DelvPromotionId"] + "' AND ISNULL(ItemId,'') = '" + dr["ItemId"] + "' AND ISNULL(ProjectCode,'') = '" + dr["ProjectCode"] + "' AND ISNULL(WBS,'') = '" + dr["WBS"] + "' AND ISNULL(UnitNo,'') = '" + dr["UnitNo"] + "'";// AND ISNULL(DelvDate,'') = '" + dr["DelvDate"] + "'
                    dr["Amount"] = dt.Compute("COUNT(ItemId)", cond);
                    if (dr["isConfirm"] + "" == "Y")
                    {
                        if (dr["isPostAcc"] + "" == "Y")
                        {
                            dr["StatusText"] = "เสร็จสิ้น";
                        }
                        else
                        {
                            dr["StatusText"] = "รอบันทึกบัญชี";
                        }
                    }
                    else
                    {
                        dr["StatusText"] = "รายการใหม่";
                    }
                }

                dtGrd.AcceptChanges();

                //if (ddlConfirmStatus.SelectedItem.Value != "0")
                //{
                //    dtGrd.DefaultView.RowFilter = "StatusText = '" + ddlConfirmStatus.SelectedItem.Text + "'";
                //}

                DataView dv = dtGrd.DefaultView;
                dv.Sort = "DelvDate desc";

                dtGrd = dv.ToTable();

                Session["dtGrd"] = dtGrd.DefaultView.ToTable();
                bindGrid();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('เกิดข้อผิดพลาดในการค้นหาข้อมูล กรุณาลองใหม่อีกครั้ง !!');", true);
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtDateFrom.Text = "";
            txtDateTo.Text = "";
            txtDeliveryNo.Text = "";
            ddlProject.SelectedIndex = 0;
            ddlItem.SelectedIndex = 0;
            cbxDelvStatus.SelectedIndex = 0;
        }

        protected void grdData_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "viewDetail")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "OpenColorBox('DeliveryItemDetail.aspx?DelvLstId=" + e.CommandArgument + "','91%','75%');", true);
                return;
            }
        }

        protected void grdData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string selDelv = Session["SelDelv"] + "";
                CheckBox grdChkPrt = (CheckBox)e.Row.FindControl("grdChkPrt");
                HiddenField grdHdfisConfirm = (HiddenField)e.Row.FindControl("grdHdfisConfirm");
                HiddenField grdHdfisPostAcc = (HiddenField)e.Row.FindControl("grdHdfisPostAcc");
                HiddenField grdHdfDelvLstId = (HiddenField)e.Row.FindControl("grdHdfDelvLstId");
                if (!(grdHdfisConfirm.Value == "Y" && grdHdfisPostAcc.Value != "Y"))
                {
                    grdChkPrt.Attributes.Add("style", "display:none;");
                }
                else
                {
                    if (selDelv.IndexOf("#" + grdHdfDelvLstId.Value + "#") != -1)
                    {
                        grdChkPrt.Checked = true;
                    }
                }
            }
        }

        protected void grdData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            KeepSelDelv();
            Session["grdPage"] = e.NewPageIndex;
            bindGrid();
        }

        protected void grdData_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sort = Session["grdSort"] + "";
            if (sort == e.SortExpression) { Session["grdSort"] = e.SortExpression + " desc"; }
            else { Session["grdSort"] = e.SortExpression; }
            bindGrid();
        }

        private void bindGrid()
        {
            DataTable dt = (DataTable)Session["dtGrd"];
            dt.DefaultView.Sort = Session["grdSort"] + "";
            grdData.DataSource = dt.DefaultView;
            if (Session["grdPage"] + "" != "")
            {
                grdData.PageIndex = (int)Session["grdPage"];
            }
            grdData.DataBind();
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            KeepSelDelv();
            if (Session["SelDelv"] + "" == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('กรุณาเลือกใบส่งมอบ !!');", true);
                return;
            }

            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            string DelvLst = (Session["SelDelv"] + "").Replace("##", "','");
            DelvLst = "'" + DelvLst.Remove(DelvLst.Length - 1, 1).Remove(0, 1) + "'";
            string CompanySAPCode = dasp.GetCompanySAPCode(DelvLst);
            //Session["SelDelv"].ToString().Replace("#", "'")
            if (CompanySAPCode == "1000")
            {
                string urlRpt = "frmReport.aspx?reqRPT=DelvCnfWBS";
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "Popup60('" + urlRpt + "');", true);
            }
            else
            {
                string urlRpt = "frmReport.aspx?reqRPT=DelvCnfWBSCrossCom";
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "Popup60('" + urlRpt + "');", true);
            }

            bindGrid();
        }

        private void KeepSelDelv()
        {
            string selDelv = Session["SelDelv"] + "";

            // ลบรายการเก่าในหน้านี้
            foreach (GridViewRow gr in grdData.Rows)
            {
                if (gr.RowType == DataControlRowType.DataRow)
                {
                    HiddenField grdHdfDelvLstId = (HiddenField)gr.FindControl("grdHdfDelvLstId");
                    selDelv = selDelv.Replace("#" + grdHdfDelvLstId.Value + "#", "");
                }
            }

            // เก็บรายการที่ติ๊กถูกในหน้านี้
            foreach (GridViewRow gr in grdData.Rows)
            {
                if (gr.RowType == DataControlRowType.DataRow)
                {
                    CheckBox grdChkPrt = (CheckBox)gr.FindControl("grdChkPrt");
                    HiddenField grdHdfDelvLstId = (HiddenField)gr.FindControl("grdHdfDelvLstId");
                    if (grdChkPrt.Checked == true)
                    {
                        selDelv = selDelv + "#" + grdHdfDelvLstId.Value + "#";
                    }
                }
            }
            Session["SelDelv"] = selDelv;
        }

        private void clearSelectDelv()
        {
            Session["SelDelv"] = null;
        }
    }
}