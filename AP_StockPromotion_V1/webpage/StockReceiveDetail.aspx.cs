using AP_StockPromotion_V1.ws_authorize;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AP_StockPromotion_V1.web
{
    public partial class StockReceiveDetail : System.Web.UI.Page
    {

        Entities.FormatDate convertDate = new Entities.FormatDate();
        private string ReceiveDetailId = "";
        private Int64 receiveDetailId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                initPage();
            }
        }

        private void initPage()
        {
            Session["grdData"] = null; Session["grdSort"] = null; Session["grdPage"] = null;
            ReceiveDetailId = Request.QueryString["receiveDetailId"] + "";
            if (ReceiveDetailId == ""){
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('มีการเข้าถึง website โดยไม่ถูกต้อง !!'); $('body').hide(); window.close();", true);
                return;
            }
            if (!Int64.TryParse(ReceiveDetailId, out receiveDetailId)){
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('มีการเข้าถึง website โดยไม่ถูกต้อง !!'); $('body').hide(); window.close();", true);
                return;
            }

            Class.DAStockReceive cls = new Class.DAStockReceive();
            DataTable dt = cls.getStockReceiveDetailItem(receiveDetailId);

            if (dt.Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ข้อมูลไม่ถูกต้อง กรุณาติดต่อผู้ดูแลระบบ !!'); $('body').hide(); window.close();", true);
                return;
            }

            DataRow dr = dt.Rows[0];
            if (dr["ItemCountMethod"] + "" == "0" || dr["ItemStock"] + "0" == "" || dr["MasterItemStatus"] + "" == "2")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('กรุณาตรวจสอบ Master สินค้าโปรโมชั่น รหัส " + dr["ItemNo"] + " !!'); parent.window.location='MasterItem.aspx?mode=Edit&ItemNo=" + dr["ItemNo"] + "';", true);
                return;
            }

            if (dr["ItemForceExpire"] + "" == "Y") { lbForceExpire.Text = "*"; }

            // if ()

            Session["dtDetailItem"] = dt;
            loadDataItem(dt);
            //loadDataItemDetail();
            bindData();

            ScriptManager.RegisterStartupScript(this, GetType(), "js", "setButtonEditItem();", true);
            
        }

        private void loadDataItem(DataTable dtDetailItem)
        {
            DataTable dt = dtDetailItem.DefaultView.ToTable(true, "PO_No", "GR_No", "Vendor", "ItemName", "ItemName1", "ReceiveAmount", "PricePerUnit", "ItemCountMethod", "Status", "CreateBy", "FullName", "CreateDate", "ItemUnit", "ItemCountMethodText", "ReceiveHeaderStatusText");
            DataRow dr = dt.Rows[0];
            
            txtPONo.Text = dr["PO_No"] + "";
            txtGRNo.Text = dr["GR_No"] + "";
            txtVendor.Text = dr["Vendor"] + "";
            txtItem.Text = dr["ItemName"] + ""; if (txtItem.Text == "") { txtItem.Text = dr["ItemName1"] + ""; }
            txtItemName.Text = txtItem.Text;

            txtAmount.Text = dr["ReceiveAmount"] + "";
            txtPricePerUnit.Text = dr["PricePerUnit"] + "";
            txtPrice.Text = dr["PricePerUnit"] + "";

            txtItemUnit.Text = dr["ItemUnit"] + "";
            txtCountMethod.Text = dr["ItemCountMethodText"] + "";
            txtReceiveStatus.Text = dr["ReceiveHeaderStatusText"] + "";
            hdfReceiveBy.Value = dr["CreateBy"] + "";
            txtReceiveBy.Text = dr["FullName"] + "";
            txtReceiveDate.Text = ((DateTime)dr["CreateDate"]).ToString(new Entities.FormatDate().formatDate) + "";

            divCountBySerial.Attributes.Add("style", "display:none;");
            divCountBySequence.Attributes.Add("style", "display:none;");
            divCountByAmount.Attributes.Add("style", "display:none;"); 
            if (dr["ItemCountMethod"] + "" == "1") { divCountBySerial.Attributes.Add("style", ""); }
            else if (dr["ItemCountMethod"] + "" == "2") { divCountBySequence.Attributes.Add("style", ""); }
            else if (dr["ItemCountMethod"] + "" == "3") { divCountByAmount.Attributes.Add("style", ""); }
            hdfItemCountMethod.Value = dr["ItemCountMethod"] + "";

            //if (dr["Status"] + "" == "3")
            //{
            //    btnSave.Visible = false;
            //}
        }

        private void bindData()
        {
            hdfShowPopupEdit.Value = "";
            DataTable dtDetailItem = (DataTable)Session["dtDetailItem"];
            
            dtDetailItem.DefaultView.RowFilter = "(ItemStatus <> '0' AND ItemStatus <> '1') AND Serial is not null"; // ไม่ใช่ของ Inactive และ ไม่ใช่สินค้ารอตรวจสอบ
            dtDetailItem.DefaultView.Sort = "ItemId desc";
            DataTable dt = dtDetailItem.DefaultView.ToTable();
            Session["grdData"] = dt;
            bindGridData();
        }
        private void bindGridData()
        {
            DataTable dt = (DataTable)Session["grdData"];
            lb_CountChecked.Text = dt.Rows.Count + "";
            lb_CountNotChecked.Text = (Convert.ToInt32(txtAmount.Text) - dt.Rows.Count) + "";
            if (Session["grdSort"] + "" != "") { dt.DefaultView.Sort = Session["grdSort"] + ""; }
            grdData.DataSource = dt.DefaultView;
            if (Session["grdPage"] + "" != "") { grdData.PageIndex = (int)Session["grdPage"]; }
            grdData.DataBind();
        }


        protected void imgChkSerial_Click(object sender, EventArgs e)
        {
            hdfTabActive.Value = "itemCheck";
            hdfShowPopupEdit.Value = "";
            if (lbForceExpire.Text == "*" && txtExpireDate.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('กรุณาระบุ Expire Date !!');", true);
                return;
            }

            if (txtSerialNo.Text.Trim() != "")
            {
                Class.DAStockReceive cls = new Class.DAStockReceive();
                DataTable dtx = cls.getDataItemFromSerial(txtSerialNo.Text.Trim());
                DataRow[] drx = dtx.Select("ReceiveDetailId <> " + Request.QueryString["receiveDetailId"] + "");
                if (drx.Length > 0)
                {
                    txtSerialNo.Text = "";
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('Serial No.ซ้ำ กรุณาตรวจสอบอีกครั้ง !!!');", true);
                    return;
                }

                DataTable dt = (DataTable)Session["dtDetailItem"];
                if (dt.Select("(ItemStatus = '1C' OR ItemStatus = '2') and Serial = '" + txtSerialNo.Text.Trim() + "'").Length > 0)
                {
                    txtSerialNo.Text = "";
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('Serial No.ซ้ำ กรุณาตรวจสอบอีกครั้ง !!');", true);
                    return;
                }

                string msgErr = enterItemCheck(txtSerialNo.Text.Trim());
                txtSerialNo.Text = "";
                bindData();
                if (msgErr != "")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('" + msgErr + "');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('กรุณาระบุ Serial No. !!');", true);
                return;
            }
            txtSerialNo.Focus();
        }

        protected void imgChkSeq_Click(object sender, ImageClickEventArgs e)
        {
            hdfTabActive.Value = "itemCheck";
            hdfShowPopupEdit.Value = "";
            if (lbForceExpire.Text == "*" && txtExpireDate.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('กรุณาระบุ Expire Date !!');", true);
                return;
            }

            if (txtSeqStart.Text.Trim() == "" || txtSeqStart.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('กรุณาระบุเลขลำดับ เริ่มต้น-สิ้นสุด !!');", true);
                return;
            }
            Int64 seqStart = 0;
            Int64 seqEnd = 0;
            if (!Int64.TryParse(txtSeqStart.Text.Trim(), out seqStart))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('เลขลำดับเริ่มต้นไม่ถูกต้อง !!');", true);
                return;
            }
            if (!Int64.TryParse(txtSeqEnd.Text.Trim(), out seqEnd))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('เลขลำดับสิ้นสุดไม่ถูกต้อง !!');", true);
                return;
            }
            for (Int64 ii = seqStart; ii <= seqEnd; ii++)
            {
                enterItemCheck(ii + "");
            }
            txtSeqStart.Text = "";
            txtSeqEnd.Text = "";
            bindData();
            txtSeqStart.Focus();
        }

        protected void imgChkAmount_Click(object sender, EventArgs e)
        {
            hdfTabActive.Value = "itemCheck";
            hdfShowPopupEdit.Value = "";
            if (lbForceExpire.Text == "*" && txtExpireDate.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('กรุณาระบุ Expire Date !!');", true);
                return;
            }
            int amount= 0;
            if (!int.TryParse(txtCountAmount.Text.Trim(), out amount))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('จำนวนสินค้าโปรโมชั่นไม่ถูกต้อง !!');", true);
                return;
            }
            string msgErr = "";
            for (int ii = 0; ii < amount; ii++)
            {
                msgErr = enterItemCheck("");
                if (msgErr != "")
                {
                    break;
                }
            }
            bindData();
            txtCountAmount.Text = "";
            if (msgErr != "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "jsx", "alert('" + msgErr + "');", true);
            }
            txtCountAmount.Focus();
        }




        protected void grdData_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //delChkItem
            if (e.CommandName + "" == "delChkItem")
            {
                hdfShowPopupEdit.Value = "";
                DataTable dtDetailItem = (DataTable)Session["dtDetailItem"];
                string itemId = e.CommandArgument + "";

                DataRow[] dr = dtDetailItem.Select("itemId = " + itemId);
                dr[0]["ItemStatus"] = "1";
                dr[0]["Serial"] = DBNull.Value;

                dr[0]["Barcode"] = DBNull.Value;
                dr[0]["ItemName"] = DBNull.Value;
                dr[0]["Model"] = DBNull.Value;
                dr[0]["Color"] = DBNull.Value;
                dr[0]["DimensionWidth"] = DBNull.Value;
                dr[0]["DimensionLong"] = DBNull.Value;
                dr[0]["DimensionHeight"] = DBNull.Value;
                dr[0]["DimensionUnit"] = DBNull.Value;
                dr[0]["Weight"] = DBNull.Value;
                dr[0]["WeightUnit"] = DBNull.Value;
                dr[0]["Price"] = DBNull.Value;
                dr[0]["ProduceDate"] = DBNull.Value;
                dr[0]["ExpireDate"] = DBNull.Value;
                dr[0]["Detail"] = DBNull.Value;
                dr[0]["Remark"] = DBNull.Value;

                dtDetailItem.AcceptChanges();
                bindData();
            }
            else if (e.CommandName + "" == "editChkItem")
            {
                hdfShowPopupEdit.Value = "Y";
                DataTable dt = (DataTable)Session["dtDetailItem"];
                DataRow[] drS = dt.Select("ItemId=" + e.CommandArgument);
                if (drS.Length > 0)
                {
                    DataRow dr = drS[0];
                    txtEditItemId.Text = dr["ItemId"] + "";
                    //txtEdit.Text = dr["ItemStatus"] + "";
                    txtEditSerial.Text = dr["Serial"] + "";
                    // txtEdit.Text = dr["Barcode"] + "";
                    txtEditItemName.Text = dr["ItemName"] + "";
                    txtEditModelName.Text = dr["Model"] + "";
                    txtEditColor.Text = dr["Color"] + "";
                    txtEditDimensionWidth.Text = dr["DimensionWidth"] + "";
                    txtEditDimensionLong.Text = dr["DimensionLong"] + "";
                    txtEditDimensionHeight.Text = dr["DimensionHeight"] + "";
                    txtEditDimensionUnit.Text = dr["DimensionUnit"] + "";
                    txtEditWeight.Text = dr["Weight"] + "";
                    txtEditWeightUnit.Text = dr["WeightUnit"] + "";
                    txtEditPrice.Text = dr["Price"] + "";

                    hdfEditItemId.Value = dr["ItemId"] + "";
                    //hdfEdit.Text = dr["ItemStatus"] + "";
                    hdfEditSerial.Value = dr["Serial"] + "";
                    // hdfEdit.Text = dr["Barcode"] + "";
                    hdfEditItemName.Value = dr["ItemName"] + "";
                    hdfEditModelName.Value = dr["Model"] + "";
                    hdfEditColor.Value = dr["Color"] + "";
                    hdfEditDimensionWidth.Value = dr["DimensionWidth"] + "";
                    hdfEditDimensionLong.Value = dr["DimensionLong"] + "";
                    hdfEditDimensionHeight.Value = dr["DimensionHeight"] + "";
                    hdfEditDimensionUnit.Value = dr["DimensionUnit"] + "";
                    hdfEditWeight.Value = dr["Weight"] + "";
                    hdfEditWeightUnit.Value = dr["WeightUnit"] + "";
                    hdfEditPrice.Value = dr["Price"] + "";

                    string pDate = "";

                    if (convertDate.getStringFromDate(dr["ProduceDate"], ref  pDate))
                    {
                        txtEditProduceDate.Text = pDate;
                    }
                    else
                    {
                        txtEditProduceDate.Text = "";
                    }
                    hdfEditProduceDate.Value = txtEditProduceDate.Text;

                    string eDate = "";
                    if (convertDate.getStringFromDate(dr["ExpireDate"], ref eDate))
                    {
                        txtEditExpireDate.Text = eDate;
                    }
                    else
                    {
                        txtEditExpireDate.Text = "";
                    }
                    hdfEditExpireDate.Value = txtEditExpireDate.Text;
                    txtEditDetail.Text = dr["Detail"] + "";
                    hdfEditDetail.Value = txtEditDetail.Text;
                    // txtEdit.Text = dr["Remark"] + "";
                    // Click ปุ่มเปิด Popup Edit clickEditItem
                    // ScriptManager.RegisterStartupScript(this, GetType(), "js", "clickEditItem();", true);

                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "openPopupEdit();", true);
                    return;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ระบบไม่สามารถทำการแก้ไขได้ !!');", true);
                    return;
                }
            }
        }

        private string enterItemCheck(string serial)
        {
            string msgErr = "";
            DataTable dt = (DataTable)Session["dtDetailItem"];

            DataRow[] dr = dt.Select("ItemStatus = '1'");
            if (dr.Length > 0)
            {
                dr[0]["serial"] = serial;
                dr[0]["ItemStatus"] = "2";
                // dr[0]["Barcode"] = "";
                dr[0]["ItemName"] = txtItemName.Text;
                dr[0]["Model"] = txtModelName.Text;
                dr[0]["Color"] = txtColor.Text;
                dr[0]["DimensionWidth"] = txtDimensionWidth.Text;
                dr[0]["DimensionLong"] = txtDimensionLong.Text;
                dr[0]["DimensionHeight"] = txtDimensionHeight.Text;
                dr[0]["DimensionUnit"] = txtDimensionUnit.Text;
                dr[0]["Weight"] = txtWeight.Text;
                dr[0]["WeightUnit"] = txtWeightUnit.Text;
                decimal p = 0;
                if (decimal.TryParse(txtPrice.Text, out p))
                {
                    dr[0]["Price"] = p;
                }

                DateTime pD = new DateTime();
                if (convertDate.getDateFromString(txtProduceDate.Text, ref pD))
                {
                    dr[0]["ProduceDate"] = pD;
                }
                else
                {
                    dr[0]["ProduceDate"] = DBNull.Value;
                }
                DateTime pE = new DateTime();
                if (convertDate.getDateFromString(txtExpireDate.Text, ref pE))
                {
                    dr[0]["ExpireDate"] = pE;
                }
                else
                {
                    dr[0]["ExpireDate"] = DBNull.Value;
                }
                dr[0]["Detail"] = txtDetail.Text;
                // dr[0]["Remark"] = "2";

                dr[0]["ItemStatus"] = "1C";

                dt.AcceptChanges();
            }
            else
            {
                // จำนวนสินค้าโปรโมชั่นที่ตรวจสอบ เกินกว่าสินค้าที่ทำการรับ
                //ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('จำนวนสินค้าโปรโมชั่นที่ตรวจสอบ เกินกว่าสินค้าที่ทำการรับ !!');", true);
                //return;
                msgErr = "จำนวนสินค้าโปรโมชั่นที่ตรวจสอบ เกินกว่าสินค้าที่ทำการรับ !!";
            }
            return msgErr;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveCheckStockItem();
        }

        private void SaveCheckStockItem()
        {
            DataTable dt = (DataTable)Session["dtDetailItem"];

            foreach (GridViewRow gr in grdData.Rows)
            {
                HiddenField hdfItemId = (HiddenField)gr.FindControl("grdHdfItemId");
                TextBox txtRemark = (TextBox)gr.FindControl("grdTxtRemark");

                DataRow dr = dt.Select("ItemId = " + hdfItemId.Value)[0];
                dr["Remark"] = txtRemark.Text;
            }

            dt.AcceptChanges();
            
            AutorizeData auth = (AutorizeData)Session["userInfo_" + Session.SessionID];
            Class.DAStockReceive cls = new Class.DAStockReceive();
            if (cls.FullFillDataStockReceiveItemDetail(dt, auth.EmployeeID))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "bindDataParentPage();", true);
                // Response.Redirect("StockReceiptList.aspx");
                return;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('เกิดข้อผิดพลาดในการบันทึกข้อมูล กรุณาติดต่อผู้ดูแลระบบ !!');", true);
                return;
            }

        }

        protected void btnX_Click(object sender, EventArgs e)
        {
            hdfShowPopupEdit.Value = "";
            hdfTabActive.Value = "itemCheck";
            bindData();
            ScriptManager.RegisterStartupScript(this, GetType(), "js", "$('#liItemCheck').removeAttr('onclick');", true);
        }

        protected void grdData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdfItemStatus = (HiddenField)e.Row.FindControl("grdHdfItemStatus");
                if (!(hdfItemStatus.Value == "1C" || hdfItemStatus.Value == "2"))
                {
                    ImageButton imgDel = (ImageButton)e.Row.FindControl("imgDel");
                    ImageButton imgEdit = (ImageButton)e.Row.FindControl("imgEdit");
                    TextBox txtRemark = (TextBox)e.Row.FindControl("grdTxtRemark");
                    imgDel.CommandName = "";
                    imgDel.Attributes.Add("style", "display:none;");
                    imgEdit.CommandName = "";
                    imgEdit.Attributes.Add("style", "display:none;");
                    txtRemark.Enabled = false;
                }
            }
        }

        protected void btnSaveEditItem_Click(object sender, EventArgs e)
        {
            
        }

        protected void btnSaveEditItemHidden_Click(object sender, EventArgs e)
        {
            hdfShowPopupEdit.Value = "";
            DataTable dt = (DataTable)Session["dtDetailItem"];
            if (hdfEditSerial.Value != "")
            {
                Class.DAStockReceive cls = new Class.DAStockReceive();
                DataTable dtx = cls.getDataItemFromSerial(hdfEditSerial.Value.Trim());
                DataRow[] drx = dtx.Select("ReceiveDetailId <> " + Request.QueryString["receiveDetailId"] + "");
                if (drx.Length > 0)
                {
                    txtSerialNo.Text = "";
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('Serial No.ซ้ำ กรุณาตรวจสอบอีกครั้ง !!!');", true);
                    return;
                }

                foreach (DataRow drc in dt.Rows)
                {
                    if (hdfEditItemId.Value != drc["ItemID"] + "")
                    {
                        if (hdfEditSerial.Value == drc["Serial"] + "")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('Serial No.ซ้ำ กรุณาตรวจสอบอีกครั้ง !!');", true);
                            return;
                        }
                    }
                }
            }
            DataRow[] drS = dt.Select("ItemId=" + txtEditItemId.Text);
            if (drS.Length > 0)
            {
                DataRow dr = drS[0];
                dr["ItemId"] = hdfEditItemId.Value;
                dr["Serial"] = hdfEditSerial.Value;
                dr["ItemName"] = hdfEditItemName.Value;
                dr["Model"] = hdfEditModelName.Value;
                dr["Color"] = hdfEditColor.Value;
                dr["DimensionWidth"] = hdfEditDimensionWidth.Value;
                dr["DimensionLong"] = hdfEditDimensionLong.Value;
                dr["DimensionHeight"] = hdfEditDimensionHeight.Value;
                dr["DimensionUnit"] = hdfEditDimensionUnit.Value;
                dr["Weight"] = hdfEditWeight.Value;
                dr["WeightUnit"] = hdfEditWeightUnit.Value;
                dr["Price"] = hdfEditPrice.Value;

                DateTime pDate = new DateTime();
                if (convertDate.getDateFromString(hdfEditProduceDate.Value, ref pDate))
                {
                    dr["ProduceDate"] = pDate;
                }
                else
                {
                    dr["ProduceDate"] = DBNull.Value;
                }

                DateTime eDate = new DateTime();
                if (convertDate.getDateFromString(hdfEditExpireDate.Value + "", ref eDate))
                {
                    dr["ExpireDate"] = eDate;
                }
                else
                {
                    dr["ExpireDate"] = DBNull.Value;
                }
                dr["Detail"] = hdfEditDetail.Value;
                // hdfEdit.Text = dr["Remark"] + "";
                dt.AcceptChanges();
                Session["dtDetailItem"] = dt;
            }
            bindData();
        }

        protected void btnDeleteAllChecked_Click(object sender, EventArgs e)
        {
            hdfShowPopupEdit.Value = "";
            DataTable dt = (DataTable)Session["dtDetailItem"];

            DataRow[] drChecked = dt.Select("ItemStatus = '2' OR ItemStatus = '1C'");
            foreach (DataRow dr in drChecked)
            {
                dr["ItemStatus"] = "1";
                dr["Serial"] = DBNull.Value;
                dr["Barcode"] = DBNull.Value;
                dr["ItemName"] = DBNull.Value;
                dr["Model"] = DBNull.Value;
                dr["Color"] = DBNull.Value;
                dr["DimensionWidth"] = DBNull.Value;
                dr["DimensionLong"] = DBNull.Value;
                dr["DimensionHeight"] = DBNull.Value;
                dr["DimensionUnit"] = DBNull.Value;
                dr["Weight"] = DBNull.Value;
                dr["WeightUnit"] = DBNull.Value;
                dr["Price"] = DBNull.Value;
                dr["ProduceDate"] = DBNull.Value;
                dr["ExpireDate"] = DBNull.Value;
                dr["Detail"] = DBNull.Value;
                dr["Remark"] = DBNull.Value;
            }
            dt.AcceptChanges();
            Session["dtDetailItem"] = dt;
            bindData();
        }

        protected void grdData_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Session["grdSort"] + "" == e.SortExpression)
            {
                Session["grdSort"] = e.SortExpression + " desc ";
            }
            else
            {
                Session["grdSort"] = e.SortExpression;
            }
            bindGridData();
        }

        protected void grdData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Session["grdPage"] = e.NewPageIndex;
            bindGridData();
        }


    }
}