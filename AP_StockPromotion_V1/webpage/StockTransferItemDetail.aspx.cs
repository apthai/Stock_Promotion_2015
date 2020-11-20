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
    public partial class StockTransferItemDetail : System.Web.UI.Page
    {
        Entities.FormatDate convertDate = new Entities.FormatDate();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                initPage();
            }
        }

        private void initPage()
        {
            Session["ItemTransfer"] = null;
            bindDDLReqType();

            Int64 reqHeaderId = 0;
            Int64 reqProject = 0;
            hdfReqHeaderId.Value = Request.QueryString["reqHeaderId"] + "";
            hdfReqProject.Value = Request.QueryString["reqProject"] + "";
            if (!Int64.TryParse(hdfReqHeaderId.Value, out reqHeaderId))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ข้อมูลผิดพลาด กรุณาติดต่อผู้ดูแลระบบ');window.close();", true);
                return;
            }
            if (!Int64.TryParse(hdfReqProject.Value, out reqProject)) 
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ข้อมูลผิดพลาด กรุณาติดต่อผู้ดูแลระบบ');window.close();", true);
                return;
            }

            DataTable dt;
            Class.DATransferToProject req = new Class.DATransferToProject();
            dt = req.getDataRequestList(reqHeaderId, reqProject, null);
            

            insertColumnTransferBalance(ref dt);
            Session["grdRequestList"] = dt;

            setContentDate(dt);
            bindGridTransferHistory(reqHeaderId, reqProject);
        }

        private void insertColumnTransferBalance(ref DataTable dt)
        {
            dt.Columns.Add(new DataColumn("Transfer", Type.GetType("System.Int32")));
            dt.Columns.Add(new DataColumn("Balance", Type.GetType("System.Int32")));
            dt.AcceptChanges();
            foreach (DataRow dr in dt.Rows)
            {
                dr["Transfer"] = 0;
                dr["Balance"] = (Int32)dr["ReqAmount"] - (Int32)dr["TransferredAmount"];
            }
            dt.AcceptChanges();
        }

        private void bindDDLReqType()
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            DataTable dt = dasp.getDataStatus("6");
            ddlReqType.DataSource = dt;
            ddlReqType.DataTextField = "StatusText";
            ddlReqType.DataValueField = "StatusValue";
            ddlReqType.DataBind();
        }
        
        private void setContentDate(DataTable dt)
        {
            grdRequest.DataSource = dt;
            grdRequest.DataBind();
            txtReqDocNo.Text = dt.Rows[0]["ReqDocNo"] + "";
            txtReqNo.Text = dt.Rows[0]["ReqNo"] + "";
            txtProject.Text = dt.Rows[0]["ProjectName"] + "";
            ddlReqType.SelectedIndex = ddlReqType.Items.IndexOf(ddlReqType.Items.FindByValue(dt.Rows[0]["ReqType"] + ""));
            txtReqType.Text = ddlReqType.SelectedItem.Text;
            string reqDate = "";
            if (convertDate.getStringFromDate(dt.Rows[0]["ReqDate"], ref reqDate))
            {
                txtRequestDate.Text = reqDate;
            }
            string reqDocDate = "";
            if (convertDate.getStringFromDate(dt.Rows[0]["ReqDocDate"], ref reqDocDate))
            {
                txtReqDocDate.Text = reqDocDate;
            }
            //txtRequestDate.Text = ((DateTime)dt.Rows[0]["ReqDate"]).ToString("M/d/yyyy");

            txtRequestBy.Text = dt.Rows[0]["FullName"] + "";
            txtReqHeaderRemark.Text = dt.Rows[0]["ReqHeaderRemark"] + "";
            hdfReqHeaderId.Value = dt.Rows[0]["ReqHeaderId"] + "";
            hdfReqProject.Value = dt.Rows[0]["Project_Id"] + "";

            hdfReqProject.Value = dt.Rows[0]["ReqDocNo"] + "";
            hdfReqProject.Value = dt.Rows[0]["ReqDocDate"] + "";
        }

        protected void grdRequest_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "TrnItem")
            {
                GridViewRow gvr = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                string reqId = ((HiddenField)gvr.FindControl("grdHdfReqId")).Value;
                string countMethod = ((HiddenField)gvr.FindControl("grdHdfItemCountMethod")).Value;

                if (countMethod == "1")
                {
                    // Response.Redirect("StockTransferItemDetailCheckSerial.aspx?reqId=" + reqId);
                }
                else if (countMethod == "2")
                {
                    // Response.Redirect("StockTransferItemDetailCheckSequence.aspx?reqId=" + reqId);
                }
                else if (countMethod == "3")
                {
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "customScript", "<script>Popup80('StockTransferItemDetailCheckAmount.aspx?reqId=" + reqId + "');</script>", false);
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "OpenColorBox('StockTransferItemDetailCheckAmount.aspx?reqId=" + reqId + "');", true);
                    //Response.Redirect("StockTransferItemDetailCheckAmount.aspx?reqId=" + reqId);
                }

                //DataTable dtItemTransferToProject = (DataTable)Session["ItemTransfer"];
                //DataRow[] dr = dtItemTransferToProject.Select("ItemId = " + e.CommandArgument);
                //dtItemTransferToProject.Rows.Remove(dr[0]);
                //dtItemTransferToProject.AcceptChanges();
            }
        }

        protected void imgCheckSerial_Click(object sender, ImageClickEventArgs e)
        {
            // ตรวจสอบปริมาณ
            DataTable dtItemTransferToProject = (DataTable)Session["ItemTransfer"];
            //if (dtItemTransferToProject != null)
            //{
            //    if (dtItemTransferToProject.Rows.Count + 1 > Convert.ToInt32(hdfReqAmount.Value))
            //    {
            //        ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('สินค้าโปรโมชั่น เกินจำนวนที่ต้องการแล้ว !!');", true);
            //        return;
            //    }
            //}

            Class.DATransferToProject req = new Class.DATransferToProject();
            string serial = txtSerial.Text.Trim();
            string currItemLst = getCurrentItemStringList();

            DataTable dt = req.getItemForTransferToProjectBySerial(serial, currItemLst);
            if (dt.Rows.Count > 0)
            {
                // ตรวจสอบสินค้าโปรโมชั่นซ้ำกัน
                if (isItemDuplicate(dt))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('สินค้าโปรโมชั่นชิ้นนี้ มีอยู่ในรายการเตรียมจัดส่งแล้ว !!');", true);
                    txtSerial.Text = "";
                    txtSerial.Focus();
                    return;
                }

                // ไม่ match กับ Item Request
                if (matchingItemRequestSerial(dt))
                {
                    addDataTransferToProjectRow(dt);
                    DataTable dtRequestList = (DataTable)Session["grdRequestList"];
                    dtRequestList.DefaultView.RowFilter = "";
                    grdRequest.DataSource = dtRequestList;
                    grdRequest.DataBind();
                    txtSerial.Text = "";
                    txtSerial.Focus();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('สินค้าโปรโมชั่นชิ้นนี้ ไม่อยู่ในใบเบิก !!');", true);
                    txtSerial.Text = "";
                    txtSerial.Focus();
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ไม่มีสินค้าโปรโมชั่นที่ต้องการ !!');", true);
                txtSerial.Text = "";
                txtSerial.Focus();
                return;
            }
            txtSerial.Text = "";
            txtSerial.Focus();
        }


        private void addDataTransferToProjectRow(DataTable dt)
        {
            DataTable dtItemTransferToProject = (DataTable)Session["ItemTransfer"];
            if (dtItemTransferToProject == null)
            {
                Session["ItemTransfer"] = dt;
            }
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    dtItemTransferToProject.ImportRow(dr);
                }
                Session["ItemTransfer"] = dtItemTransferToProject;
            }
            txtSerial.Text = "";
            txtSerial.Focus();
        }

        private string getCurrentItemStringList()
        {
            string curItem = "";
            DataTable dtItemTransferToProject = (DataTable)Session["ItemTransfer"];
            if (dtItemTransferToProject != null)
            {
                foreach (DataRow dr in dtItemTransferToProject.Rows)
                {
                    curItem += "," + dr["ItemId"];
                }
                if (curItem != "") { curItem = curItem.Remove(0, 1); }
            }
            return curItem;
        }

        private bool isItemDuplicate(DataTable dt)
        {
            bool bRst = false;
            DataTable dtItemTransferToProject = (DataTable)Session["ItemTransfer"];
            if (dtItemTransferToProject != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    foreach (DataRow drTran in dtItemTransferToProject.Rows)
                    {
                        if (dr["ItemId"] + "" == drTran["ItemId"] + "")
                        {
                            bRst = true;
                            break;
                        }
                    }
                }
            }
            return bRst;
        }


        protected void grdRequest_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Label grdLbTransferred = (Label)e.Row.FindControl("grdLbTransferred");
                //HiddenField grdHdfReqAmount = (HiddenField)e.Row.FindControl("grdHdfReqAmount");
                //HiddenField grdHdfTransferredAmount = (HiddenField)e.Row.FindControl("grdHdfTransferredAmount");
                //grdLbTransferred.Text = grdHdfTransferredAmount.Value + "/" + grdHdfReqAmount.Value;
                

                //Label grdLbTransfer = (Label)e.Row.FindControl("grdLbTransfer");
                //HiddenField grdHdfTransfer = (HiddenField)e.Row.FindControl("grdHdfTransfer");
                //HiddenField grdHdfBalance = (HiddenField)e.Row.FindControl("grdHdfBalance");
                //grdLbTransfer.Text = grdHdfTransfer.Value + "/" + grdHdfBalance.Value;

                ImageButton imgTran = (ImageButton)e.Row.FindControl("imgTran");
                HiddenField grdHdfItemCountMethod = (HiddenField)e.Row.FindControl("grdHdfItemCountMethod");
                if (grdHdfItemCountMethod.Value == "1") { imgTran.Attributes.Add("style", "display:none;"); }

            }
        }
        private bool matchingItemRequestSerial(DataTable dt)
        {
            bool bRst = false;
            DataTable dtRequestList = (DataTable)Session["grdRequestList"];
            foreach (DataRow dr in dtRequestList.Rows)
            {
                if (dt.Rows[0]["MasterItemId"] + "" == dr["ItemId"] + "")
                {
                    if (dt.Rows.Count <= (Int32)dr["Balance"] - (Int32)dr["Transfer"])
                    {
                        dr["Transfer"] = (Int32)dr["Transfer"] + dt.Rows.Count;
                        dtRequestList.AcceptChanges();
                        bRst = true;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('จำนวนสินค้าโปรโมชั่นเกินปริมาณที่ต้องการ !!');", true);                        
                    }
                }
            }
            return bRst;
        }

        private bool matchingItemRequestAmount(DataTable dt)
        {
            bool bRst = false;
            DataTable dtRequestList = (DataTable)Session["grdRequestList"];
            foreach (DataRow dr in dtRequestList.Rows)
            {
                if (dt.Rows[0]["MasterItemId"] + "" == dr["ItemId"] + "")
                {
                    if (dt.Rows.Count <= (Int32)dr["Balance"] - (Int32)dr["Transfer"])
                    {
                        dr["Transfer"] = (Int32)dr["Transfer"] + dt.Rows.Count;
                        dtRequestList.AcceptChanges();
                        bRst = true;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('จำนวนสินค้าโปรโมชั่นเกินปริมาณที่ต้องการ !!');", true);                        
                    }
                }
            }
            return bRst;
        }


        private void bindGridTransferHistory(Int64 reqHeaderId, Int64 reqProject)
        {
            // spGetDateTransferItemToProjectHistory
            hdfReqHeaderId.Value = Request.QueryString["reqHeaderId"] + "";
            hdfReqProject.Value = Request.QueryString["reqProject"] + "";
            Class.DATransferToProject req = new Class.DATransferToProject();
            DataTable dt = null;// req.getDataTransferItemToProjectHistoryByReqIdList(reqHeaderId, reqProject);
            grdTransferHistory.DataSource = dt;
            grdTransferHistory.DataBind();
        }



        protected void btnCheckItemByAmount_Click(object sender, EventArgs e)
        {
            // hdfItemIdList.Value
            string currItemLst = getCurrentItemStringList();
            Class.DATransferToProject req = new Class.DATransferToProject();
            string[] itemLst = hdfItemIdList.Value.Split(',');
            int itemId = 0;
            foreach (string item in itemLst)
            {
                if (int.TryParse(item, out itemId))
                {
                     DataTable dt = req.getItemForTransferToProjectByAmount(itemId, currItemLst);
                    if (dt.Rows.Count > 0)
                    {
                        // ตรวจสอบสินค้าโปรโมชั่นซ้ำกัน
                        if (isItemDuplicate(dt))
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('สินค้าโปรโมชั่นชิ้นนี้ มีอยู่ในรายการเตรียมจัดส่งแล้ว !!');", true);
                            hdfItemIdList.Value = "";
                            txtSerial.Text = "";
                            txtSerial.Focus();
                            return;
                        }

                        // ไม่ match กับ Item Request
                        if (matchingItemRequestAmount(dt))
                        {
                            addDataTransferToProjectRow(dt);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('สินค้าโปรโมชั่นชิ้นนี้ ไม่อยู่ในใบเบิก !!');", true);
                            hdfItemIdList.Value = "";
                            txtSerial.Text = "";
                            txtSerial.Focus();
                            return;
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ไม่มีสินค้าโปรโมชั่นที่ต้องการ !!');", true);
                        hdfItemIdList.Value = "";
                        txtSerial.Text = "";
                        txtSerial.Focus();
                        return;
                    }
                }
            }

            DataTable dtRequestList = (DataTable)Session["grdRequestList"];
            dtRequestList.DefaultView.RowFilter = "";
            grdRequest.DataSource = dtRequestList;
            grdRequest.DataBind();
            hdfItemIdList.Value = "";
            txtSerial.Text = "";
            txtSerial.Focus();
        }

        protected void btnTransferItemToProject_Click(object sender, EventArgs e)
        {
            //string urlRptTr1 = "frmReport.aspx?reqRPT=Tr01&TrListId=4&project_Id=3";
            //string urlRptTr2 = "frmReport.aspx?reqRPT=Tr02&TrListId=4&project_Id=3";
            //ScriptManager.RegisterStartupScript(this, GetType(), "js", "Popup80('" + urlRptTr1 + "'); Popup80('" + urlRptTr2 + "'); window.location.replace('StockTransferItem.aspx');", true);
            //return;

            DataTable dtRequestList = (DataTable)Session["grdRequestList"];
            DataTable dtItemTransferToProject = (DataTable)Session["ItemTransfer"];


            Class.DATransferToProject cls = new Class.DATransferToProject();

            //string a = cls.lookTable(dtRequestList);
            //string b = cls.lookTable(dtItemTransferToProject);
            //return;
            string ItemLst = "";
            foreach (DataRow dr in dtItemTransferToProject.Rows)
            {
                ItemLst += "," + dr["ItemId"];
            }

            if (ItemLst.Length > 0)
            {

            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "jsAlt", "<script>alert('ไม่มีรายการส่งมอบสินค้าโปรโมชั่น');</script>", false);
                return;
            }
        }

        protected void grdTransferHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string TrListId = ((HiddenField)e.Row.FindControl("grdHdfTrListId")).Value;
                string project_Id = ((HiddenField)e.Row.FindControl("grdHdfProject_Id")).Value;
                ImageButton img = (ImageButton)e.Row.FindControl("imgPrt");
                img.CommandArgument = "TrListId=" + TrListId + "&project_Id=" + project_Id;
                img.CommandArgument = "TrListId=" + TrListId + "&project_Id=" + project_Id;
            }
        }

        protected void grdTransferHistory_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "PrtTrn")
            {
                string urlRptTr01 = "frmReport.aspx?reqRPT=Tr01&" + e.CommandArgument;
                string urlRptTr02 = "frmReport.aspx?reqRPT=Tr02&" + e.CommandArgument;
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "Popup60('" + urlRptTr01 + "'); Popup60('" + urlRptTr02 + "'); ", true);//window.location.replace('StockTransferItem.aspx');
                return;
            }
        }

    }
}