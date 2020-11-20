using AP_StockPromotion_V1.Class;
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
    public partial class RequisitionEdit : System.Web.UI.Page
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
            bindDDLProject();
            bindDDLItem();
            bindDDLReqType();
            bindDDLUser();
            if (Request.QueryString["mode"] + "" == "Create")
            {
                Class.DARequisition req = new Class.DARequisition();
                DataTable dt = req.getDataRequest(-1, 0);
                Session["grdRequest"] = dt;
                grdData.DataSource = dt;
                grdData.DataBind();
                txtRequestDate.Text = DateTime.Now.ToString(convertDate.formatDate);
            }
            else
            {
                Int64 reqId = 0;
                hdfReqId.Value = Request.QueryString["reqId"] + "";
                if (!Int64.TryParse(hdfReqId.Value, out reqId))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ข้อมูลผิดพลาด กรุณาติดต่อผู้ดูแลระบบ'); doNothing();", true);
                    return;
                }
                Class.DARequisition req = new Class.DARequisition();
                DataTable dt = req.getDataRequest(reqId, 0);
                if (dt.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ข้อมูลผิดพลาด กรุณาติดต่อผู้ดูแลระบบ'); doNothing();", true);
                    return;
                }
                Session["grdRequest"] = dt;
                grdData.DataSource = dt;
                grdData.DataBind();
                setContentDateForEdit(dt);
                btnOK.Text = "บันทึกการแก้ไข";
                btnDelReqRemark.Attributes.Add("style", "");
                if (dt.Rows[0]["ReqHeaderStatus"] + "" != "1") // 0:ยกเลิก  1:รายการใหม่  2:ระหว่างดำเนินการ  3:เสร็จสิ้น
                {
                    btnOK.Attributes.Add("style", "display:none;");
                    btnDelReqRemark.Attributes.Add("style", "display:none;");
                }
            }
        }

        private void bindDDLProject()
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            DataTable dt = dasp.getDataMasterProject(true);
            ddlProject.DataSource = dt;
            ddlProject.DataTextField = "ProjectName";
            ddlProject.DataValueField = "ProjectID";
            ddlProject.DataBind();
            ddlProject.Items.Insert(0, new ListItem("-- โปรดระบุ --", ""));
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
            ddlItem.Items.Insert(0, new ListItem("-- โปรดระบุ --", ""));
        }

        private void bindDDLReqType(string isCRM = "")
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            DataTable dt = dasp.getDataStatus("6");
            ddlReqType.DataSource = dt;
            ddlReqType.DataTextField = "StatusText";
            ddlReqType.DataValueField = "StatusValue";
            ddlReqType.DataBind();
            if (isCRM != "Y")
            {
                ddlReqType.Items.Remove(ddlReqType.Items.FindByValue("1"));
            }
            ddlReqType.Items.Insert(0, new ListItem("-- โปรดระบุ --", ""));
        }

        private void bindDDLUser()
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            DataTable dt = dasp.getDataUser("", "");
            ddlUser.DataSource = dt;
            ddlUser.DataTextField = "FullNoName";
            ddlUser.DataValueField = "EmpCode";
            ddlUser.DataBind();
            ddlUser.Items.Insert(0, new ListItem("-- เลือก --", ""));
        }

        protected void btnAddRequest_Click(object sender, EventArgs e)
        {
            hdfTabActive.Value = "lireqdetail";
            string Project_Id = ddlProject.SelectedItem.Value;
            string ProjectName = ddlProject.SelectedItem.Text;
            string ItemId = ddlItem.SelectedItem.Value;
            string ItemName = ddlItem.SelectedItem.Text;

            if (Project_Id == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('กรุณาเลือกโครงการให้ถูกต้อง !');", true);
                return;
            }
            if (ItemId == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('กรุณาเลือกสินค้าโปรโมชั่นให้ถูกต้อง !');", true);
                return;
            }
            // string startDate = txtDateStart.Text;
            // string endDate = txtDateEnd.Text;
            string ReqAmount = txtReqAmount.Text;
            string ItemUnit = lbUnit.Text;

            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();

            if (!convertDate.getDateFromString(txtDateStart.Text, ref startDate))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('วันที่โปรโมชั่นไม่ถูกต้อง !'); $('#" + txtDateStart.ClientID + "').focus();", true);
                return;
            }
            if (!convertDate.getDateFromString(txtDateEnd.Text, ref endDate))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('วันที่โปรโมชั่นไม่ถูกต้อง !'); $('#" + txtDateEnd.ClientID + "').focus();", true);
                return;
            }
            
            if (ReqAmount == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('กรุณาระบุจำนวนที่ต้องการเบิก');", true);
                return;
            }

            int proAlertDate = 0;
            int.TryParse(txtAlertDate.Text, out proAlertDate);

            DataTable dt = (DataTable)Session["grdRequest"];

            DataRow[] drSel = dt.Select("Project_Id = " + Project_Id + " and ItemId = " + ItemId + " and ReqStatus = '1'");
            if (drSel.Length == 0)
            {
                DataRow dr = dt.NewRow();
                dr["ReqId"] = 0;
                dr["Project_Id"] = Project_Id;
                dr["ProjectName"] = ProjectName;
                dr["ItemId"] = ItemId;
                dr["ItemName"] = ItemName;
                dr["ProStartDate"] = startDate;
                dr["ProEndDate"] = endDate;
                dr["ProAlertDate"] = proAlertDate;
                dr["ReqAmount"] = ReqAmount;
                dr["ReqStatus"] = "1";
                dr["ItemUnit"] = ItemUnit;
                dt.Rows.Add(dr);
            }
            else
            {
                drSel[0]["ProStartDate"] = startDate;
                drSel[0]["ProEndDate"] = endDate;
                drSel[0]["ReqAmount"] = ReqAmount;
                drSel[0]["ProAlertDate"] = proAlertDate;
            }
            dt.AcceptChanges();
            Session["grdRequest"] = dt;
            dt.DefaultView.RowFilter = "ReqStatus <> '0'";
            grdData.DataSource = dt.DefaultView;
            grdData.DataBind();
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            // ตรวจสอบ
            DataTable dt = (DataTable)Session["grdRequest"];
            if (txtReqNo.Text == "" || ddlUser.SelectedItem.Value == "" || txtRequestDate.Text == "" || ddlReqType.SelectedItem.Value == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('กรุณากรอกข้อมูลให้ครบถ้วน');", true);
                hdfTabActive.Value = "lireq";
                return;
            }
            if (dt.Select("ReqStatus <> '0'").Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ไม่พบรายการสินค้าที่ต้องการเบิก');", true);
                hdfTabActive.Value = "lireqdetail";
                return;
            }

            if (ddlUser.SelectedItem.Value == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "$('#" + ddlUser.ClientID + "').click();", true);
                hdfTabActive.Value = "lireq";
                return;
            }

            if (ddlReqType.SelectedItem.Value != "3")
            {
                DataRow[] drs = dt.Select("Project_Id = 99998");
                if (drs.Length > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('โครงการ \\'ไม่ระบุโครงการ\\' ใช้สำหรับ Event (Prebooking) เท่านั้น !');", true);
                    hdfTabActive.Value = "lireqdetail";
                    return;
                }
            }


            // บันทึก
            if (Request.QueryString["mode"] + "" == "Create")
            {
                SaveCreateRequest();
            }
            else
            {
                SaveEditRequest();
            }
        }

        public void SaveCreateRequest()
        {
            Entities.RequisitionInfo req = new Entities.RequisitionInfo();
            req.ReqNo = txtReqNo.Text;

            DateTime reqDocDate = new DateTime();
            if (!convertDate.getDateFromString(txtReqDocDate.Text, ref reqDocDate))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('วันที่รับเอกสารขอเบิก ไม่ถูกต้อง !');", true);
                return;
            }
            DateTime reqDate = new DateTime();
            if (!convertDate.getDateFromString(txtRequestDate.Text, ref reqDate))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('วันที่รับเอกสารขอเบิก ไม่ถูกต้อง !');", true);
                return;
            }
            if (ddlReqType.SelectedItem.Value == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('โปรดระบุประเภทโปรโมชั่น !');", true);
                return;
            }

            AutorizeData auth = (AutorizeData)Session["userInfo_" + Session.SessionID];

            req.ReqDocDate = txtReqDocDate.Text;
            req.ReqDate = txtRequestDate.Text;
            req.ReqBy = ddlUser.SelectedItem.Value;//txtRequestByCode.Text;//txtRequestBy.Text;
            req.ReqType = ddlReqType.SelectedItem.Value;
            req.ReqHeaderRemark = txtReqHeaderRemark.Text;

            List<Entities.RequisitionInfo> reqdetail = new List<Entities.RequisitionInfo>();
            DataTable dtDraft = (DataTable)Session["grdRequest"];
            dtDraft.DefaultView.RowFilter = "ReqStatus <> '0'";
            DataTable dt = dtDraft.DefaultView.ToTable();
            foreach (DataRow dr in dt.Rows)
            {
                Entities.RequisitionInfo reqItem = new Entities.RequisitionInfo();
                reqItem.Project_Id = (int)dr["Project_Id"];
                reqItem.ProjectName = (string)dr["ProjectName"];
                int proAlertDate = 0;
                int.TryParse(dr["ProAlertDate"] + "", out proAlertDate);
                reqItem.ProAlertDate = proAlertDate;
                reqItem.ProStartDate = ((DateTime)dr["ProStartDate"]).ToString();
                reqItem.ProEndDate = ((DateTime)dr["ProEndDate"]).ToString();
                reqItem.ItemId = (int)dr["ItemId"];
                // reqItem.ItemNo = (string)dr["ItemNo"];
                reqItem.ItemName = (string)dr["ItemName"];
                reqItem.ReqAmount = (int)dr["ReqAmount"];
                // reqItem.ItemUnit = (string)dr["ItemUnit"];
                req.UpdateBy = auth.EmployeeID;
                reqdetail.Add(reqItem);
            }

            string reqDocNo = "";
            Class.DARequisition cls_req = new Class.DARequisition();
            bool rst = cls_req.createRequest(req, reqdetail,ref reqDocNo);
            if (rst)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('บันทึกข้อมูลการเบิกเสร็จสิ้น');window.location.replace('Requisition.aspx?bindData=Y&ReqDocNo=" + reqDocNo + "');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('บันทึกข้อมูลผิดพลาด กรุณาติดต่อผู้ดูแลระบบ !!');", true);
            }
        }


        private void setContentDateForEdit(DataTable dt)
        {
            txtReqDocNo.Text = dt.Rows[0]["ReqDocNo"] + "";
            txtReqNo.Text = dt.Rows[0]["ReqNo"] + "";
            string reqDate = "";
            if (convertDate.getStringFromDate(dt.Rows[0]["ReqDate"], ref reqDate))
            {
                txtRequestDate.Text = reqDate;//((DateTime)dt.Rows[0]["ReqDate"]).ToString("M/d/yyyy");
            }
            string reqDocDate = "";
            if (convertDate.getStringFromDate(dt.Rows[0]["ReqDocDate"], ref reqDocDate))
            {
                txtReqDocDate.Text = reqDocDate;//((DateTime)dt.Rows[0]["ReqDate"]).ToString("M/d/yyyy");
            }
            ddlUser.SelectedIndex = ddlUser.Items.IndexOf(ddlUser.Items.FindByValue(dt.Rows[0]["ReqBy"] + ""));
            //txtRequestByCode.Text = ;
            //txtRequestBy.Text = dt.Rows[0]["FullName"] + "";

            // Chitaphon.pen 2015 10 30
            if (dt.Rows[0]["ReqType"] + "" == "1")
            {
                bindDDLReqType("Y");
                btnOK.Attributes.Add("style", "display:none;");
            }
            ddlReqType.SelectedIndex = ddlReqType.Items.IndexOf(ddlReqType.Items.FindByValue(dt.Rows[0]["ReqType"] + ""));
            ddlReqType.Attributes.Add("ReadOnly", "true");
            txtReqHeaderRemark.Text = dt.Rows[0]["ReqHeaderRemark"] + "";
            ScriptManager.RegisterStartupScript(this, GetType(), "js", "$('.js-example-basic-single-plc').prop('disabled', true);", true);
        }

        public void SaveEditRequest()
        {
            Entities.RequisitionInfo req = new Entities.RequisitionInfo();
            req.ReqHeaderId = Convert.ToInt64(hdfReqId.Value);
            req.ReqNo = txtReqNo.Text;

            DateTime reqDocDate = new DateTime();
            if (!convertDate.getDateFromString(txtReqDocDate.Text, ref reqDocDate))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('วันที่รับเอกสารขอเบิก ไม่ถูกต้อง !');", true);
                return;
            }
            DateTime reqDate = new DateTime();
            if (!convertDate.getDateFromString(txtRequestDate.Text, ref reqDate))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('วันที่รับเอกสารขอเบิก ไม่ถูกต้อง !');", true);
                return;
            }
            if (ddlReqType.SelectedItem.Value == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('โปรดระบุประเภทโปรโมชั่น !');", true);
                return;
            }

            req.ReqDocDate = txtRequestDate.Text;
            req.ReqDate = txtRequestDate.Text;

            req.ReqBy = ddlUser.SelectedItem.Value;//txtRequestByCode.Text;
            req.ReqType = ddlReqType.SelectedItem.Value;
            req.ReqHeaderRemark = txtReqHeaderRemark.Text;

            List<Entities.RequisitionInfo> reqdetail = new List<Entities.RequisitionInfo>();
            DataTable dt = (DataTable)Session["grdRequest"];

            if (dt.Select("ReqStatus <> '0'").Length == 0){
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ไม่พบรายการสินค้าที่ต้องการเบิก');", true);
                hdfTabActive.Value = "lireqdetail";
                return;
            }

            foreach (DataRow dr in dt.Rows)
            {
                Entities.RequisitionInfo reqItem = new Entities.RequisitionInfo();
                reqItem.ReqHeaderId = Convert.ToInt64(hdfReqId.Value);                
                reqItem.ReqId = Convert.ToInt32(dr["ReqId"]);
                reqItem.Project_Id = (int)dr["Project_Id"];
                // reqItem.ProjectID = (string)dr["ProjectID"];
                reqItem.ProjectName = (string)dr["ProjectName"];
                reqItem.ProStartDate = ((DateTime)dr["ProStartDate"]).ToString();
                reqItem.ProEndDate = ((DateTime)dr["ProEndDate"]).ToString();
                int proAlertDate = 0;
                int.TryParse(dr["ProAlertDate"] + "", out proAlertDate);
                reqItem.ProAlertDate = proAlertDate;
                reqItem.ItemId = (int)dr["ItemId"];
                // reqItem.ItemNo = (string)dr["ItemNo"];
                reqItem.ItemName = (string)dr["ItemName"];
                reqItem.ReqAmount = (int)dr["ReqAmount"];
                // reqItem.ItemUnit = (string)dr["ItemUnit"];
                reqItem.ReqStatus = dr["ReqStatus"] + "";
                reqdetail.Add(reqItem);
            }

            Class.DARequisition cls_req = new Class.DARequisition();
            bool rst = cls_req.editRequest(req, reqdetail);
            if (rst)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('บันทึกข้อมูลเสร็จสิ้น'); window.location.replace('Requisition.aspx?bindData=Y&ReqDocNo=" + txtReqDocNo.Text + "');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('บันทึกข้อมูลผิดพลาด กรุณาติดต่อผู้ดูแลระบบ');", true);
            }
        }

        protected void grdData_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "delReq")
            {
                DataTable dt = (DataTable)Session["grdRequest"];
                DataRow[] dr = dt.Select("ReqId=" + e.CommandArgument);
                dr[0]["ReqStatus"] = "0";
                dt.AcceptChanges();
                Session["grdRequest"] = dt;
                dt.DefaultView.RowFilter = "ReqStatus <> '0'";
                grdData.DataSource = dt.DefaultView;
                grdData.DataBind();
                hdfTabActive.Value = "lireqdetail";
            }
        }

        /* - Chitaphon.pen 2015 11 06 - */
        //protected void btnDelReq_Click(object sender, EventArgs e)
        //{
        //    // hdfReqId.Value
        //    Class.DARequisition cls_req = new Class.DARequisition();
        //    if (cls_req.deleteRequest(Convert.ToInt64(hdfReqId.Value)))
        //    {
        //        ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ยกเลิกข้อมูลเสร็จสิ้น'); window.location.replace('Requisition.aspx');", true);
        //    }
        //    else
        //    {
        //        ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('บันทึกข้อมูลผิดพลาด กรุณาติดต่อผู้ดูแลระบบ');", true);
        //    }

        //}

        protected void btnDelReq_Click(object sender, EventArgs e)
        {
            // hdfReqId.Value
            Class.DARequisition cls_req = new Class.DARequisition();
            AutorizeData auth = (AutorizeData)Session["userInfo_" + Session.SessionID];
            if (cls_req.deleteRequest(Convert.ToInt64(hdfReqId.Value), hdfReqDelRemark.Value, auth.EmployeeID))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ยกเลิกข้อมูลเสร็จสิ้น'); window.location.replace('Requisition.aspx');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ยกเลิกข้อมูลใบเบิกผิดพลาด กรุณาติดต่อผู้ดูแลระบบ');", true);
            }

        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("Requisition.aspx?sCond=Y");
        }
        


        //protected void imgFindUser_Click(object sender, ImageClickEventArgs e)
        //{
        //    DAStockPromotion cls = new DAStockPromotion();
        //    DataTable dt = cls.getDataUserByEmpCode(txtRequestBy.Text.Trim());
        //    if (dt.Rows.Count == 1)
        //    {
        //        DataRow dr = dt.Rows[0];
        //        hdfUserId.Value = dr["UserID"] + "";
        //        hdfEmpCode.Value = dr["EmpCode"] + "";
        //        txtRequestByCode.Text = dr["EmpCode"] + "";
        //        hdfUserName.Value = dr["FirstName"] + " " + dr["LastName"];
        //        txtRequestBy.Text = dr["FirstName"] + " " + dr["LastName"];
        //    }
        //    else
        //    {
        //        string[] name = txtRequestBy.Text.Split(' ');
        //        string fn = name[0];
        //        string ln = "";
        //        if (name.Length > 1)
        //        {
        //            ln = name[1];
        //        }
        //        dt = cls.getDataUser(fn, ln);
        //        if (dt.Rows.Count == 1)
        //        {
        //            DataRow dr = dt.Rows[0];
        //            hdfUserId.Value = dr["UserID"] + "";
        //            hdfEmpCode.Value = dr["EmpCode"] + "";
        //            txtRequestByCode.Text = dr["EmpCode"] + "";
        //            hdfUserName.Value = dr["FirstName"] + " " + dr["LastName"];
        //            txtRequestBy.Text = dr["FirstName"] + " " + dr["LastName"];
        //        }
        //        else
        //        {
        //            ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('กรุณาตรวจสอบผู้เบิก และทำการบันทึกอีกครั้ง'); OpenColorBox('GetUser.aspx?btnFindUserId=" + imgFindUser.ClientID + "&txtFindUserId=" + txtRequestBy.ClientID + "&sUser=" + txtRequestBy.Text.Replace(" ", "_") + "','80%','80%');", true);
        //            return;
        //        }
        //    }
        //}

    }
}