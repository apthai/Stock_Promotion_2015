using AP_StockPromotion_V1.Class;
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
    public partial class DisbursementDetail : System.Web.UI.Page
    {
        Entities.FormatDate convertDate = new Entities.FormatDate();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                initPage();
            }
            else
            {
                if (Session["CRMChecker"] != null)
                {
                    string _dt = Session["CRMChecker"].ToString();

                    if (_dt != "" && (_dt.Substring(0, 2).ToLower().Equals("rt") || _dt.Substring(0, 2).ToLower().Equals("rs")))
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "js", "SetEqualtation(true, '');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "js", "SetEqualtation(false, '');", true);
                    }
                }
            }
        }

        private void initPage()
        {
            Session["requestList"] = null;
            Session["ItemTransfer"] = null;
            bindDDLReqType();
            hdfReqIdList.Value = Request.QueryString["reqIdList"] + "";
            string selFn = Request.QueryString["reqFnType"] + "";
            Session["SelFn"] = null;
            Session["SelFn"] = selFn;

            DATransferToProject req = new DATransferToProject();
            //แก้ส่วนนนี้เนื่องจากต้องกรองข้อมูลเฉพาะข้อมูลที่จะนำไปจ่ายจริงๆ ซึ่งถ้าจ่ายแล้วจะมีสถานะบอกซึ่งไม่ติดอ่ะไร
            //แต่ถ้ายังไม่จ่าย หรือ ยกเลิกการจ่ายแล้วจะจ่ายใหม่นั้น จะต้องกรองข้อมูลล่าสุดจริงๆไปทำรายการไม่งั้นระบบจะแสดงซ้ำ record
            //ซึ่งตรงนี้ไม่แน่ใจว่า เดิมระบบนี้ทำไมถึง select ข้อมูลแล้วเทียบกับตาราง Tranferprojectdetail เพราะยังไงก็ต้องยกเลิกจ่ายใหม่ได้อยู่แล้ว
            DataTable dt = req.getDataRequestListByRequestList(hdfReqIdList.Value, selFn);
            DataView dv = new DataView();
            if (dt.Rows != null)
            {
                if (dt.Rows.Count > 1)
                {
                    dv = new DataView(dt, "TrStatus=0 OR isnull(TrStatus,99)=99 ", string.Empty, DataViewRowState.CurrentRows);
                    dt = dv.ToTable("dtReqList");
                    if (dt.Rows.Count <= 0)
                    {
                        dt = req.getDataRequestListByRequestList(hdfReqIdList.Value, selFn);
                        dv = new DataView(dt, string.Empty, string.Empty, DataViewRowState.CurrentRows);
                        dt = dv.ToTable(true);
                    }
                }
            }

            insertColumnTransferBalance(ref dt);

            // dt.Select("TrStatus=''").CopyToDataTable();//กัน store เดิมเวลามีการแอดของ ลบของบ่อยๆ เหมือน store ตัวเดิมดึงข้อมูลมาแสดงมั่วไปหมด เลยแบ่งไม่เอาที่มี Status = 0
            Session["requestList"] = dt;

            Session["OriReqList"] = dt;
            var _dt = (from t1 in dt.AsEnumerable()
                       group t1 by new
                       {
                           reqno = t1.Field<string>("reqno")
                       } into reqno
                       select new
                       {
                           _reqno = reqno.Key.reqno
                       }).ToList();

            if (_dt.Count == 1)
            {
                Session["CRMChecker"] = _dt[0]._reqno;
                if (_dt[0]._reqno != "" && (_dt[0]._reqno.Substring(0, 2).ToLower().Equals("rt") || _dt[0]._reqno.Substring(0, 2).ToLower().Equals("rs")))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "SetEqualtation(true, '');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "SetEqualtation(false, '');", true);
                }
            }

            bindDDLEqtItems();
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {

                    setContentDate(dt);
                    bindGridTransferHistory(hdfReqIdList.Value, selFn);
                }
            }

        }

        private void insertColumnTransferBalance(ref DataTable dt)
        {
            dt.Columns.Add(new DataColumn("Transfer", Type.GetType("System.Int32")));
            dt.Columns.Add(new DataColumn("Balance", Type.GetType("System.Int32")));
            dt.AcceptChanges();
            foreach (DataRow dr in dt.Rows)
            {
                string ReqAmount = dr["ReqAmount"].ToString();
                string TransferredAmount = dr["TransferredAmount"].ToString();
                dr["Transfer"] = 0;
                dr["Balance"] = (((Int32)dr["ReqAmount"] - (Int32)dr["TransferredAmount"]) < 0 ? 0 : ((Int32)dr["ReqAmount"] - (Int32)dr["TransferredAmount"]));
            }
            dt.AcceptChanges();
        }

        private void bindDDLReqType()
        {
            DAStockPromotion dasp = new DAStockPromotion();
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
            txtProject.Text = dt.Rows[0]["ProjectName"] + "";
            ddlReqType.SelectedIndex = ddlReqType.Items.IndexOf(ddlReqType.Items.FindByValue(dt.Rows[0]["ReqType"] + ""));
            txtReqType.Text = ddlReqType.SelectedItem.Text;

            txtRequestBy.Text = dt.Rows[0]["FullName"] + "";
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
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "js", "OpenColorBox('DisbursementCheckAmount.aspx?reqId=" + reqId + "','90%','90%');", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "js", "alert('รายละเอียดสินค้านี้ยังไม่ได้ active !!');", true);
                }

                //DataTable dtItemTransferToProject = (DataTable)Session["ItemTransfer"];
                //DataRow[] dr = dtItemTransferToProject.Select("ItemId = " + e.CommandArgument);
                //dtItemTransferToProject.Rows.Remove(dr[0]);
                //dtItemTransferToProject.AcceptChanges();
            }
            else if (e.CommandName == "DelItem")
            {
                DataTable dtRequestList = (DataTable)Session["requestList"];
                DataTable dtItemTransfer = (DataTable)Session["ItemTransfer"];
                GridViewRow gvr = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                string ItemName = ((HiddenField)gvr.FindControl("grdHdfItemName")).Value;
                string Transfer = ((HiddenField)gvr.FindControl("grdHdfTransfer")).Value;

                DataTable _dt = dtRequestList.Clone();
                foreach (DataRow row in dtRequestList.Rows)
                {
                    if (row["ItemName"].ToString() != ItemName || row["Transfer"].ToString() != Transfer)
                    {
                        _dt.ImportRow(row);
                    }
                }
                DataTable _dtItem = dtItemTransfer.Clone();
                foreach (DataRow itemRow in dtItemTransfer.Rows)
                {
                    if (itemRow["ItemName"].ToString() != ItemName.Replace("(เทียบเท่า) ", "") || itemRow["TotalAmount"].ToString() != Transfer)
                    {
                        _dtItem.ImportRow(itemRow);
                    }
                }
                Session["requestList"] = _dt; //dtRequestList;
                Session["ItemTransfer"] = (_dtItem.Rows.Count == 0 ? null : _dtItem);
                grdRequest.DataSource = _dt; //dtRequestList;
                grdRequest.DataBind();
            }
        }


        protected void imgEqtItems_Click(object sender, ImageClickEventArgs e)
        {
            if (Session["EquivalentsMode"] == null)
            {
                Session["EquivalentsMode"] = 1;
            }

            if (txtQuantityAndSerial.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "js", "alert('กรุณาระบุจำนวน หรือ Serial No.');", true);
                txtQuantityAndSerial.Text = "";
                txtQuantityAndSerial.Focus();
                return;
            }

            if (dllEqtItems.SelectedItem.Value == "0")
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "js", "alert('กรุณาเลือกสินค้าเทียบเท่า');", true);
                txtQuantityAndSerial.Text = "";
                dllEqtItems.Focus();
                return;
            }



            string msgerr = "";
            Int64 iSerialOrQuantity = 0;
            string iSerialOrQuantityText = txtQuantityAndSerial.Text;
            int itemID = Convert.ToInt32(dllEqtItems.SelectedItem.Value);

            DAStockPromotion dasp = null;



            dasp = new DAStockPromotion();
            var res = (from t1 in dasp.GetStockItemTypeByMasterItemId(itemID, out msgerr).AsEnumerable()
                       select new
                       {
                           typeId = t1.ItemArray[0],
                           typeName = t1.ItemArray[1]
                       }).ToList().FirstOrDefault();
            if (res != null)
            {
                if (Convert.ToInt32(res.typeId) == 1)
                {
                    dasp = new DAStockPromotion();
                    if (dasp.GetStockItemByMasterItemId(itemID, txtQuantityAndSerial.Text, out msgerr).AsEnumerable().Count() == 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "js", "alert('ไม่พบ Serial No. นี้ ในระบบ !!');", true);
                        txtQuantityAndSerial.Focus();
                        return;
                    }
                }
            }

            dasp = new DAStockPromotion();


            int Amount = 0;
            string Serial = string.Empty;
            if (!int.TryParse(iSerialOrQuantityText, out Amount))
            {
                Amount = 1;
                Serial = iSerialOrQuantityText;
            }

            DataTable dt = dasp.GetDataMasterItemByID(itemID, Amount.ToString(), Serial, "");

            if (dt.Rows.Count == 0 && Serial.Trim() == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "js", "alert('ไม่พบสินค้านี้ในระบบ หรือ สินค้าอาจไม่มีในสต็อกกลาง !!');", true);
                txtSerial.Text = "";
                txtSerial.Focus();
            }
            else if (dt.Rows.Count == 0 && Serial.Trim() != "")
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "js", "alert('ไม่พบ Serial No. นี้ ในระบบ !!');", true);
                txtSerial.Text = "";
                txtSerial.Focus();
            }
            else
            {
                int itemRow = 0;
                DataTable dtRequestList = (DataTable)Session["requestList"];
                bool isCheck = false;
                string RefItemId = "";
                foreach (GridViewRow row in grdRequest.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chkRow = (row.Cells[0].FindControl("SelectOpt") as CheckBox);
                        if (chkRow.Checked)
                        {
                            isCheck = true;
                            DAStockPromotion dsp = null;


                            //item from CRM
                            string crmMsgErr = "";
                            foreach (DataRow item in dtRequestList.Rows)
                            {
                                if (row.Cells[3].Text.Equals(item.ItemArray[19].ToString()))
                                {
                                    RefItemId = item.ItemArray[18].ToString();
                                }
                            }

                            string reqId = (((DataRow)dtRequestList.Rows[itemRow])).ItemArray[1].ToString();
                            string materialId = (((DataRow)dtRequestList.Rows[itemRow])).ItemArray[18].ToString();
                            string materialName = (((DataRow)dtRequestList.Rows[itemRow])).ItemArray[19].ToString();
                            int amount = Convert.ToInt32((((DataRow)dtRequestList.Rows[itemRow])).ItemArray[20]);
                            if (amount == 0)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, GetType(), "js", "alert('สินค้าที่เลือก ไม่มีจำนวนที่เบิก');", true);
                                txtQuantityAndSerial.Text = "";
                                return;
                            }
                            dsp = new DAStockPromotion();
                            double sapItemPrice = dsp.GetPRItemPriceByReqNo(reqId, materialId, out crmMsgErr);
                            double crmTotalPrice = (sapItemPrice * amount);


                            //item from quatation
                            string equMsgErr = "";
                            iSerialOrQuantity = (Convert.ToInt32(res.typeId) == 1 ? 1 : Convert.ToInt64(txtQuantityAndSerial.Text));
                            int equItemID = Convert.ToInt32(dllEqtItems.SelectedItem.Value);
                            dsp = new DAStockPromotion();
                            double stockItemPrice = dsp.GetPriceFromMasterItemByMasterId(equItemID, out equMsgErr);
                            double stockTotalPrice = (stockItemPrice * iSerialOrQuantity);

                            if (crmTotalPrice < stockTotalPrice)
                            {
                                //ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ไม่สามารถเทียบเท่าได้ เนื่องจากราคาสินค้าที่จะเทียบเท่า มากกว่าสิ้นค้าที่เบิก');", true);
                                txtQuantityAndSerial.Focus();

                                string msgalt = "ไม่สามารถเทียบเท่าได้ เนื่องจากราคาสินค้าที่จะเทียบเท่า มากกว่าสิ้นค้าที่เบิก";
                                string _dt = Session["CRMChecker"].ToString();

                                if (_dt.Substring(0, 2).ToLower().Equals("rt") || _dt.Substring(0, 2).ToLower().Equals("rs"))
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "js", "SetEqualtation(true, '" + msgalt + "');", true);
                                    txtQuantityAndSerial.Text = "";
                                    txtQuantityAndSerial.Focus();
                                }
                                else
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "js", "SetEqualtation(false, '');", true);
                                    txtQuantityAndSerial.Text = "";
                                    txtQuantityAndSerial.Focus();
                                }
                                return;
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(this, GetType(), "js", "alert('ราคาสินค้าเทียบเท่าน้อยกว่าสินค้าตั้งเบิก');", true);
                                txtQuantityAndSerial.Text = "";
                                txtQuantityAndSerial.Focus();
                            }

                        }
                    }
                    itemRow++;
                }

                if (!isCheck)
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "js", "alert('กรุณาเลือกสินค้าที่ต้องการเทียบเท่า.');", true);
                    return;
                }

                DataTable _dtRequestList = (DataTable)Session["requestList"];

                int _iCount = 0;

                dt.Columns.Add(new DataColumn("ReqId", typeof(Int64)));
                dt.Columns.Add(new DataColumn("RefItemId", typeof(Int64)));
                dt.Columns.Add(new DataColumn("TotalAmount", typeof(Int32)));
                foreach (DataRow dr in dt.Rows)
                {
                    _iCount = 0;
                    dr["TotalAmount"] = (Convert.ToInt32(res.typeId) == 1 ? 1 : iSerialOrQuantity);
                    if (dr["RefItemId"].ToString() == "")
                    {
                        dr["RefItemId"] = RefItemId.ToString();
                    }
                    foreach (GridViewRow row in grdRequest.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            CheckBox chkRow = (row.Cells[0].FindControl("SelectOpt") as CheckBox);
                            if (chkRow.Checked)
                            {
                                dr["ReqId"] = _dtRequestList.Rows[_iCount]["ReqId"].ToString();

                                break;
                            }
                        }
                        _iCount++;
                    }
                }
                addDataTransferToProjectRowByEqtItem(dt);
            }

        }

        protected void imgCheckSerial_Click(object sender, ImageClickEventArgs e)
        {
            // ตรวจสอบปริมาณ
            DataTable dtItemTransferToProject = (DataTable)Session["ItemTransfer"];
            DATransferToProject req = new DATransferToProject();
            string serial = txtSerial.Text.Trim();
            string currItemLst = getCurrentItemStringList();

            DataTable dt = req.getItemForTransferToProjectBySerial(serial, currItemLst);
            if (dt.Rows.Count > 0)
            {
                // ตรวจสอบสินค้าโปรโมชั่นซ้ำกัน
                if (isItemDuplicate(dt))
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "js", "alert('สินค้าโปรโมชั่นชิ้นนี้ มีอยู่ในรายการเตรียมจัดส่งแล้ว !!');", true);
                    txtSerial.Text = "";
                    txtSerial.Focus();
                    return;
                }

                // ไม่ match กับ Item Request
                if (matchingItemRequestSerial(dt))
                {
                    addDataTransferToProjectRow(dt);
                    DataTable dtRequestList = (DataTable)Session["requestList"];
                    dtRequestList.DefaultView.RowFilter = "";
                    grdRequest.DataSource = dtRequestList;
                    grdRequest.DataBind();
                    txtSerial.Text = "";
                    txtSerial.Focus();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "js", "alert('สินค้าโปรโมชั่นชิ้นนี้ ไม่อยู่ในใบเบิก !!');", true);
                    txtSerial.Text = "";
                    txtSerial.Focus();
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "js", "alert('ไม่พบ Serial No. นี้ ในระบบ !!');", true);
                txtSerial.Text = "";
                txtSerial.Focus();
                return;
            }
            txtSerial.Text = "";
            txtSerial.Focus();
        }

        private void bindDDLEqtItems()
        {
            DAStockPromotion dasp = new DAStockPromotion();
            Entities.MasterItemInfo item = new Entities.MasterItemInfo();
            item.ItemCostEnd = 9999999;
            DataTable dt = dasp.getDataMasterItem(item);
            dllEqtItems.DataSource = dt;
            dllEqtItems.DataTextField = "ItemNoName";
            // ddlItem.DataValueField = "ItemNo";
            dllEqtItems.DataValueField = "MasterItemId";
            dllEqtItems.DataBind();
            dllEqtItems.Items.Insert(0, new ListItem("", "0"));
        }

        private void addDataTransferToProjectRowByEqtItem(DataTable dt)
        {
            DataTable dtRequestList = (DataTable)Session["requestList"];

            DataTable dtItemTransferToProject = (DataTable)Session["ItemTransfer"];

            if (dtItemTransferToProject == null)
            {
                Session["ItemTransfer"] = dt;
            }
            else
            {
                bool isContails = false;
                int iRow = 0;

                DataTable tmpDt = dt.Copy();

                foreach (DataRow item in tmpDt.Rows)
                {
                    foreach (DataRow dtItemRow in dtItemTransferToProject.Rows)
                    {
                        if (dtItemRow.ItemArray[1].Equals(item.ItemArray[1]) &&
                            dtItemRow.ItemArray[2].Equals(item.ItemArray[2]) &&
                            dtItemRow.ItemArray[3].Equals(item.ItemArray[3]) &&
                            dtItemRow.ItemArray[23].Equals("0000000000" + item.ItemArray[23].ToString()) &&
                            dtItemRow.ItemArray[9].Equals(item.ItemArray[9]) &&
                            dtItemRow.ItemArray[25].Equals(item.ItemArray[25]) &&
                            dtItemRow.ItemArray[6].Equals(item.ItemArray[6]) &&
                            dtItemRow.ItemArray[30].Equals(item.ItemArray[30]))
                        {
                            isContails = true;
                            dtItemRow["TotalAmount"] = Convert.ToInt32(dtItemTransferToProject.Rows[iRow].ItemArray[9]) + Convert.ToInt32(dtItemRow["TotalAmount"]);
                            break;
                        }
                        else
                        {
                            isContails = false;
                        }
                    }
                    if (!isContails)
                    {
                        dtItemTransferToProject.Rows.Add(item.ItemArray);
                    }
                    iRow++;
                }
                Session["ItemTransfer"] = dtItemTransferToProject;
            }

            int itemRow = 0;
            bool isDuplicate = false;
            DataTable __dt = dtRequestList.Clone();
            DataTable _dt = dtRequestList.Clone();

            foreach (DataRow _item in dtRequestList.Rows)
            {
                if (_item["ReqNo"].ToString() != "")
                {
                    __dt.ImportRow(_item);
                }
                else
                {
                    var dd = dt.AsEnumerable().Where(q => q.ItemArray[5].ToString().Equals(_item.ItemArray[19].ToString().Replace("(เทียบเท่า)", "").Trim())).ToList();
                    if (dd.Count > 0)
                    {
                        isDuplicate = true;
                        _item["Transfer"] = Convert.ToInt32(((DataRow)dd[0]).ItemArray[9]) + Convert.ToInt32(_item["Transfer"]);
                        __dt.ImportRow(_item);
                    }
                    else
                    {
                        __dt.ImportRow(_item);
                    }
                }
            }

            if (!isDuplicate)
            {
                foreach (GridViewRow row in grdRequest.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chkRow = (row.Cells[0].FindControl("SelectOpt") as CheckBox);
                        if (chkRow.Checked && row.Cells[1].Text != "")
                        {
                            DataRow new_row = dtRequestList.Rows[itemRow];
                            _dt.ImportRow(new_row);

                            _dt.Rows.Add();
                            _dt.Rows[_dt.Rows.Count - 1]["ItemName"] = "(เทียบเท่า) " + dt.Rows[0]["ItemName"];
                            _dt.Rows[_dt.Rows.Count - 1]["Transfer"] = dt.Rows[0]["TotalAmount"];

                        }
                        else
                        {
                            DataRow new_row = dtRequestList.Rows[itemRow];
                            _dt.ImportRow(new_row);
                        }
                    }
                    itemRow++;
                }
            }

            if (!isDuplicate)
            {
                Session["requestList"] = _dt;
                grdRequest.DataSource = _dt;
                grdRequest.DataBind();
            }
            else
            {
                Session["requestList"] = __dt;
                grdRequest.DataSource = __dt;
                grdRequest.DataBind();
            }

        }

        private void addDataTransferToProjectRow(DataTable dt)
        {
            dt.Columns.Add(new DataColumn("ReqId", typeof(Int64)));
            dt.Columns.Add(new DataColumn("RefItemId", typeof(Int64)));
            dt.Columns.Add(new DataColumn("TotalAmount", typeof(Int32)));
            foreach (DataRow dr in dt.Rows)
            {
                dr["ReqId"] = Convert.ToInt64(hdfReqIdAmount.Value);
            }
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
                if (e.Row.RowIndex > -1)
                {
                    DataRowView dr = ((DataRowView)e.Row.DataItem);
                    CheckBox chk = ((CheckBox)e.Row.Cells[0].FindControl("SelectOpt"));
                    chk.Attributes.Add("onclick", "CheckItem('" + chk.ClientID + "','" + dr["ReqId"].ToString() + "');");

                    e.Row.Attributes.Add("CheckBoxID", chk.ClientID);
                }

                DataTable dt = (DataTable)Session["requestList"];
                if (dt.Rows[e.Row.RowIndex]["ReqHeaderId"].ToString() != "")
                {
                    ImageButton imgTran = (ImageButton)e.Row.FindControl("imgTran");
                    HiddenField grdHdfItemCountMethod = (HiddenField)e.Row.FindControl("grdHdfItemCountMethod");
                    if (grdHdfItemCountMethod.Value == "1")
                    {
                        imgTran.Attributes.Add("style", "display:none;");
                    }
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[e.Row.RowIndex]["ReqStatusText"].ToString().Trim() == "จ่ายครบแล้ว")
                        {
                            imgTran.Attributes.Add("style", "display:none;");
                        }
                    }
                    ImageButton imgDelItem = (ImageButton)e.Row.FindControl("imgDelItem");
                    imgDelItem.Attributes.Add("style", "display:none;");
                    CheckBox selectOpt = (CheckBox)e.Row.FindControl("SelectOpt");
                    selectOpt.Attributes.Add("style", "");
                }
                else
                {
                    ImageButton imgTran = (ImageButton)e.Row.FindControl("imgTran");
                    imgTran.Attributes.Add("style", "display:none;");
                    ImageButton imgDelItem = (ImageButton)e.Row.FindControl("imgDelItem");
                    imgDelItem.Attributes.Add("style", "");
                    CheckBox selectOpt = (CheckBox)e.Row.FindControl("SelectOpt");
                    selectOpt.Attributes.Add("style", "display:none;");
                }

            }
        }

        private bool matchingItemRequestSerial(DataTable dt)
        {
            bool bRst = false;
            DataTable dtRequestList = (DataTable)Session["requestList"];
            dtRequestList.DefaultView.RowFilter = "balance > transfer";
            if (dtRequestList.DefaultView.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "js", "alert('จำนวนสินค้าโปรโมชั่นเกินปริมาณที่ต้องการ !!');", true);
            }
            else
            {
                foreach (DataRowView dr in dtRequestList.DefaultView)
                {
                    if (dt.Rows[0]["MasterItemId"] + "" == dr["ItemId"] + "")
                    {
                        if (dt.Rows.Count <= (Int32)dr["Balance"] - (Int32)dr["Transfer"])
                        {
                            dr["Transfer"] = (Int32)dr["Transfer"] + dt.Rows.Count;
                            dtRequestList.AcceptChanges();
                            bRst = true;
                            hdfReqIdAmount.Value = dr["ReqId"] + "";
                            break;
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, GetType(), "js", "alert('จำนวนสินค้าโปรโมชั่นเกินปริมาณที่ต้องการ !!');", true);
                        }
                    }
                }
            }
            dtRequestList.DefaultView.RowFilter = "";
            return bRst;
        }

        private void matchingItemRequestAmountByEqtItem(DataTable dt)
        {
            DataTable dtRequestList = (DataTable)Session["requestList"];

            dtRequestList.Rows.Add();
        }

        private bool matchingItemRequestAmount(DataTable dt)
        {
            bool bRst = false;
            DataTable dtRequestList = (DataTable)Session["requestList"];
            foreach (DataRow dr in dtRequestList.Rows)
            {
                if (hdfReqIdAmount.Value == dr["ReqId"] + "")
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
                            ScriptManager.RegisterClientScriptBlock(this, GetType(), "js", "alert('จำนวนสินค้าโปรโมชั่นเกินปริมาณที่ต้องการ !!');", true);
                        }
                    }
                }
            }
            return bRst;
        }

        private void bindGridTransferHistory(string ReqIdList, string selFn)
        {
            // spGetDateTransferItemToProjectHistory

            DATransferToProject req = new DATransferToProject();
            DataTable dt = req.getDataTransferItemToProjectHistoryByReqIdList(ReqIdList, selFn);
            grdTransferHistory.DataSource = dt;
            grdTransferHistory.DataBind();
        }

        protected void btnCheckItemByAmount_Click(object sender, EventArgs e)
        {
            // hdfItemIdList.Value
            string currItemLst = getCurrentItemStringList();
            DATransferToProject req = new DATransferToProject();
            string[] itemLst = hdfItemIdList.Value.Split(',');
            int itemId = 0;
            foreach (string item in itemLst)
            {
                if (int.TryParse(item, out itemId))
                {
                    DataTable dt = req.getItemForTransferToProjectByAmount(itemId, currItemLst);
                    // hdfReqIdAmount
                    if (dt.Rows.Count > 0)
                    {
                        // ตรวจสอบสินค้าโปรโมชั่นซ้ำกัน
                        if (isItemDuplicate(dt))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, GetType(), "js", "alert('สินค้าโปรโมชั่นชิ้นนี้ มีอยู่ในรายการเตรียมจัดส่งแล้ว !!');", true);
                            hdfItemIdList.Value = "";
                            txtSerial.Text = "";
                            txtSerial.Focus();
                            hdfReqIdAmount.Value = "";
                            return;
                        }

                        // ไม่ match กับ Item Request
                        if (matchingItemRequestAmount(dt))
                        {
                            addDataTransferToProjectRow(dt);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, GetType(), "js", "alert('สินค้าโปรโมชั่นชิ้นนี้ ไม่อยู่ในใบเบิก !!');", true);
                            hdfItemIdList.Value = "";
                            txtSerial.Text = "";
                            txtSerial.Focus();
                            hdfReqIdAmount.Value = "";
                            return;
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "js", "alert('ไม่มีสินค้าโปรโมชั่นที่ต้องการ !!');", true);
                        hdfItemIdList.Value = "";
                        txtSerial.Text = "";
                        txtSerial.Focus();
                        hdfReqIdAmount.Value = "";
                        return;
                    }
                }
            }

            DataTable dtRequestList = (DataTable)Session["requestList"];
            dtRequestList.DefaultView.RowFilter = "";
            grdRequest.DataSource = dtRequestList;
            grdRequest.DataBind();
            hdfItemIdList.Value = "";
            txtSerial.Text = "";
            txtSerial.Focus();
            hdfReqIdAmount.Value = "";
        }

        protected void btnTransferItemToProject_Click(object sender, EventArgs e)
        {

            DataTable dtRequestList = (DataTable)Session["requestList"];
            DataTable dtItemTransferToProject = (DataTable)Session["ItemTransfer"];
            if (Session["EquivalentsMode"] != null && Convert.ToInt32(Session["EquivalentsMode"]) == 1)
            {
                dtRequestList = dtRequestList.Select("ReqDocNo <> ''").CopyToDataTable();
            }

            if (dtItemTransferToProject == null)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "js", "alert('ไม่มีรายการส่งมอบสินค้าโปรโมชั่น');", true);
                return;
            }

            DATransferToProject cls = new DATransferToProject();

            string ItemLst = "";
            Dictionary<string, Entities.ReqItemList> reqItemList = new Dictionary<string, Entities.ReqItemList>();
            foreach (DataRow dr in dtItemTransferToProject.Rows)
            {
                ItemLst += "," + dr["ItemId"];

                if (!reqItemList.ContainsKey(dr["ReqId"] + ""))
                {
                    Entities.ReqItemList ReqItm = new Entities.ReqItemList();
                    ReqItm.reqId = Convert.ToInt64(dr["ReqId"]);
                    ReqItm.ItemList = dr["ItemId"] + "";

                    ReqItm.totalAmount = (!dtItemTransferToProject.Columns.Contains("TotalAmount") ? 0 : (dr["TotalAmount"].ToString() != "" ? Convert.ToInt32(dr["TotalAmount"]) : 0));
                    ReqItm.refMatId = (!dtItemTransferToProject.Columns.Contains("RefItemId") ? 0 : (dr["RefItemId"].ToString() != "" ? Convert.ToInt32(dr["RefItemId"]) : 0));

                    reqItemList.Add(dr["ReqId"] + "", ReqItm);
                }
                else
                {
                    reqItemList[dr["ReqId"] + ""].ItemList += "," + dr["ItemId"];
                }
            }

            if (ItemLst.Length > 0)
            {
                AutorizeData auth = (AutorizeData)Session["userInfo_" + Session.SessionID];
                long reqHeaderId = (long)Convert.ToInt64(dtRequestList.Rows[0]["ReqHeaderId"]);
                int reqFn = Convert.ToInt32(dtRequestList.Rows[0]["Function"]);
                int project_Id = 0;
                //int.TryParse(hdfReqProject.Value, out project_Id);
                ItemLst = ItemLst.Remove(0, 1);
                string msgErr = "";
                long TrListId = 0;
                if (cls.saveDataTransferItemToProject(reqItemList, ItemLst, reqHeaderId, reqFn, project_Id, auth.EmployeeID, ref TrListId, ref msgErr))
                {
                    //  reqRPT  TrListId    project_Id
                    string urlRptTr01 = "frmReport.aspx?reqRPT=Tr01&TrListId=" + TrListId + "&project_Id=" + project_Id + "&ReqFn=" + Session["SelFn"].ToString() + "";
                    string urlRptTr02 = "frmReport.aspx?reqRPT=Tr02&TrListId=" + TrListId + "&project_Id=" + project_Id + "&ReqFn=" + Session["SelFn"].ToString() + "";
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "js", "Popup60('" + urlRptTr01 + "'); Popup60('" + urlRptTr02 + "'); window.location.replace('StockTransferItem2.aspx?bindData=Y&');", true);
                    return;
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "js", "alert('" + msgErr + "');", true);
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "js", "alert('ไม่มีรายการส่งมอบสินค้าโปรโมชั่น');", true);
                return;
            }
        }

        protected void grdTransferHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string TrListId = ((HiddenField)e.Row.FindControl("grdHdfTrListId")).Value;
                string project_Id = ((HiddenField)e.Row.FindControl("grdHdfProject_Id")).Value;
                string DelvAmt = ((HiddenField)e.Row.FindControl("grdHdfDelvAmt")).Value;
                ImageButton img = (ImageButton)e.Row.FindControl("imgPrt");
                ImageButton imgDel = (ImageButton)e.Row.FindControl("imgDel");
                img.CommandArgument = "TrListId=" + TrListId + "&project_Id=" + project_Id;
                int delvAmt = 0;
                int.TryParse(DelvAmt, out delvAmt);
                if (delvAmt > 0)
                {
                    imgDel.Attributes.Add("style", "display:none;");
                }
            }
        }

        protected void grdTransferHistory_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DelTrn")
            {
                if (e.CommandArgument != null)
                {
                    //GridViewRow gvr = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    // dbo.spCancelTransferItemToProject

                    DATransferToProject cls = new DATransferToProject();

                    string[] Arg = e.CommandArgument.ToString().Split(',');

                    long TrLstId = 0;
                    long ReqId = 0;

                    if (!long.TryParse(Arg[0] + "", out TrLstId) || !long.TryParse(Arg[1] + "", out ReqId))
                    {
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "js", "alert('ไม่สามารถดำเนินการได้ ไม่พบเลข TrLstId หรือ ReqID !!');", true);
                        return;
                    }

                    string msgErr = "";

                    AutorizeData auth = (AutorizeData)Session["userInfo_" + Session.SessionID];
                    if (cls.cancelTransferItemToProject(TrLstId, ReqId, auth.EmployeeID, Session["SelFn"].ToString(), ref msgErr))
                    {
                        bindGridTransferHistory(hdfReqIdList.Value, Session["SelFn"].ToString());
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "js", "alert('ดำเนินการเสร็จสิ้น');window.location = window.location.href;", true);
                        initPage();
                        return;
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "js", "alert('ไม่สามารถดำเนินการได้ " + msgErr + " !!');", true);
                        return;
                    }
                }
            }
            if (e.CommandName == "PrtTrn")
            {
                string urlRptTr01 = "frmReport.aspx?reqRPT=Tr01&" + e.CommandArgument + "&ReqFn=" + Session["SelFn"].ToString();
                string urlRptTr02 = "frmReport.aspx?reqRPT=Tr02&" + e.CommandArgument + "&ReqFn=" + Session["SelFn"].ToString();
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "js", "Popup60('" + urlRptTr01 + "'); Popup60('" + urlRptTr02 + "'); window.location.replace('StockTransferItem2.aspx');", true);
                return;
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("StockTransferItem2.aspx?sCond=Y");
        }

        public class StockItemsList
        {
            public string MasterItemId { get; set; }
            public string MatGroup { get; set; }
            public string ItemNo { get; set; }
            public string ItemName { get; set; }
            public string ItemUnit { get; set; }
            public string ItemPricePerUnit { get; set; }
            public string ItemId { get; set; }
            public string Serial { get; set; }
            public string Price { get; set; }
            public string TransferId { get; set; }
            public string msdaStatusVal { get; set; }
            public string msdaStatusText { get; set; }
            public string msdbStatusVal { get; set; }
            public string msdbStatusText { get; set; }
            public string msdcStatusVal { get; set; }
            public string msdcStatusText { get; set; }
        }

        protected void ImageEqyByItem_Click(object sender, ImageClickEventArgs e)
        {
            foreach (GridViewRow row in grdRequest.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("SelectOpt") as CheckBox);
                    if (chkRow.Checked)
                    {
                        string ReqID = ((HiddenField)row.FindControl("grdHdfReqId")).Value;
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "js", "OpenColorBox('DisbursementCheckAmountForEqt.aspx?ItemId=" + dllEqtItems.SelectedValue.ToString() + "&ReqID=" + ReqID + "','90%','90%');", true);
                    }

                }
            }

        }



        protected void btnCheckItemByAmountEqt_Click(object sender, EventArgs e)
        {
            DataTable dtItemChoseEtq = new DataTable();
            string ItemID = string.Empty;

            if (hdfItemIdListEqt != null)
            {
                if (hdfItemIdListEqt.Value.Trim() != "")
                {
                    ItemID = hdfItemIdListEqt.Value;
                    ItemID = "'" + ItemID.Trim().Replace(",", "','") + "'";
                }
            }



            if (Session["ItemChoseEtq"] != null)
            {
                dtItemChoseEtq = (DataTable)Session["ItemChoseEtq"];
                //-------------------------------------------------------------------------------------------------------
                //ใส่ code ที่มาจาก ปุ่ม imgEqtItems_Click และ ทำให้เข้า flow เดิมให้ได้แต่เลือกชิ้นสินค้าตามที่เลือกมาจากหน้า DisbursementCheckAmountForEqt
                if (Session["EquivalentsMode"] == null)
                {
                    Session["EquivalentsMode"] = 1;
                }




                string msgerr = "";
                Int64 iSerialOrQuantity = 0;
                string iSerialOrQuantityText = txtQuantityAndSerial.Text;
                int itemID = Convert.ToInt32(dllEqtItems.SelectedItem.Value);

                DAStockPromotion dasp = null;

                dasp = new DAStockPromotion();
                var res = (from t1 in dasp.GetStockItemTypeByMasterItemId(itemID, out msgerr).AsEnumerable()
                           select new
                           {
                               typeId = t1.ItemArray[0],
                               typeName = t1.ItemArray[1]
                           }).ToList().FirstOrDefault();
                if (res != null)
                {
                    if (Convert.ToInt32(res.typeId) == 1)
                    {
                        dasp = new DAStockPromotion();
                        if (dasp.GetStockItemByMasterItemId(itemID, txtQuantityAndSerial.Text, out msgerr).AsEnumerable().Count() == 0)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, GetType(), "js", "alert('ไม่พบ Serial No. นี้ ในระบบ !!');", true);
                            txtQuantityAndSerial.Focus();
                            return;
                        }
                    }
                }

                dasp = new DAStockPromotion();
                string Serial = string.Empty;


                DataTable dt = dasp.GetDataMasterItemByID(itemID, "", Serial, ItemID);

                if (dt.Rows.Count == 0 && Serial.Trim() == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "js", "alert('ไม่พบสินค้านี้ในระบบ หรือ สินค้าอาจไม่มีในสต็อกกลาง !!');", true);
                    txtSerial.Text = "";
                    txtSerial.Focus();
                }
                else if (dt.Rows.Count == 0 && Serial.Trim() != "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "js", "alert('ไม่พบ Serial No. นี้ ในระบบ !!');", true);
                    txtSerial.Text = "";
                    txtSerial.Focus();
                }
                else
                {
                    int itemRow = 0;
                    DataTable dtRequestList = (DataTable)Session["requestList"];
                    bool isCheck = false;
                    string RefItemId = "";
                    foreach (GridViewRow row in grdRequest.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            CheckBox chkRow = (row.Cells[0].FindControl("SelectOpt") as CheckBox);
                            if (chkRow.Checked)
                            {
                                isCheck = true;
                                DAStockPromotion dsp = null;


                                //item from CRM
                                string crmMsgErr = "";
                                foreach (DataRow item in dtRequestList.Rows)
                                {
                                    if (row.Cells[3].Text.Equals(item.ItemArray[19].ToString()))
                                    {
                                        RefItemId = item.ItemArray[18].ToString();
                                    }
                                }

                                string reqId = (((DataRow)dtRequestList.Rows[itemRow])).ItemArray[1].ToString();
                                string materialId = (((DataRow)dtRequestList.Rows[itemRow])).ItemArray[18].ToString();
                                string materialName = (((DataRow)dtRequestList.Rows[itemRow])).ItemArray[19].ToString();
                                int amount = Convert.ToInt32((((DataRow)dtRequestList.Rows[itemRow])).ItemArray[20]);
                                if (amount == 0)
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "js", "alert('สินค้าที่เลือก ไม่มีจำนวนที่เบิก');", true);
                                    txtQuantityAndSerial.Text = "";
                                    return;
                                }
                                dsp = new DAStockPromotion();
                                double sapItemPrice = dsp.GetPRItemPriceByReqNo(reqId, materialId, out crmMsgErr);
                                double crmTotalPrice = (sapItemPrice * amount);


                                //item from quatation                                                          
                                dsp = new DAStockPromotion();
                                iSerialOrQuantity = dtItemChoseEtq.Rows.Count;
                                double stockTotalPrice = Convert.ToDouble((decimal)dtItemChoseEtq.Compute("SUM(PricePerUnit)", ""));


                                if (crmTotalPrice < stockTotalPrice)
                                {

                                    txtQuantityAndSerial.Focus();

                                    string msgalt = "ไม่สามารถเทียบเท่าได้ เนื่องจากราคาสินค้าที่จะเทียบเท่ามากกว่าสิ้นค้าที่เบิก! ชื่อสินค้า:" + materialName + ",ราคารวมในใบเบิก:(" + sapItemPrice + " x " + amount + ") =" + crmTotalPrice + ",ราคารวมที่ต้องการตัดสต็อก:(" + stockTotalPrice / iSerialOrQuantity + " x " + iSerialOrQuantity + ") =" + stockTotalPrice;
                                    string _dt = Session["CRMChecker"].ToString();

                                    if (_dt.Substring(0, 2).ToLower().Equals("rt") || _dt.Substring(0, 2).ToLower().Equals("rs"))
                                    {
                                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "js", "SetEqualtation(true, '" + msgalt + "');", true);
                                        txtQuantityAndSerial.Text = "";
                                        txtQuantityAndSerial.Focus();
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "js", "SetEqualtation(false, '');", true);
                                        txtQuantityAndSerial.Text = "";
                                        txtQuantityAndSerial.Focus();
                                    }
                                    return;
                                }
                                else
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "js", "alert('ราคาสินค้าเทียบเท่าน้อยกว่าสินค้าตั้งเบิก');", true);
                                    txtQuantityAndSerial.Text = "";
                                    txtQuantityAndSerial.Focus();
                                }

                            }
                        }
                        itemRow++;
                    }

                    if (!isCheck)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "js", "alert('กรุณาเลือกสินค้าที่ต้องการเทียบเท่า.');", true);
                        return;
                    }

                    DataTable _dtRequestList = (DataTable)Session["requestList"];

                    int _iCount = 0;

                    dt.Columns.Add(new DataColumn("ReqId", typeof(Int64)));
                    dt.Columns.Add(new DataColumn("RefItemId", typeof(Int64)));
                    dt.Columns.Add(new DataColumn("TotalAmount", typeof(Int32)));
                    foreach (DataRow dr in dt.Rows)
                    {
                        _iCount = 0;
                        dr["TotalAmount"] = iSerialOrQuantity; /*(Convert.ToInt32(res.typeId) == 1 ? 1 : iSerialOrQuantity);*/
                        if (dr["RefItemId"].ToString() == "")
                        {
                            dr["RefItemId"] = RefItemId.ToString();
                        }
                        foreach (GridViewRow row in grdRequest.Rows)
                        {
                            if (row.RowType == DataControlRowType.DataRow)
                            {
                                CheckBox chkRow = (row.Cells[0].FindControl("SelectOpt") as CheckBox);
                                if (chkRow.Checked)
                                {
                                    dr["ReqId"] = _dtRequestList.Rows[_iCount]["ReqId"].ToString();

                                    break;
                                }
                            }
                            _iCount++;
                        }
                    }
                    addDataTransferToProjectRowByEqtItem(dt);
                }
                //-------------------------------------------------------------------------------------------------------




            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "js", "alert('ไม่พบ item ที่เลือกกรุณาติดต่อเจ้าหน้าที่ IT Error:Session ItemChoseEtq is null from DisbursementCheckAmountForEqt.aspx');window.location = window.location.href;", true);
            }
        }




    }
}