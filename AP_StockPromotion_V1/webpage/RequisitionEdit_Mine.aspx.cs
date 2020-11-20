using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AP_StockPromotion_V1.web
{
    public partial class RequisitionEdit_Mine : System.Web.UI.Page
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
            bindDDLReqType();
            if (Request.QueryString["mode"] + "" == "Create")
            {
                Class.DARequisition req = new Class.DARequisition();
                DataTable dt = req.getDataRequest(-1);
                Session["grdRequest"] = dt;
                grdData.DataSource = dt;
                grdData.DataBind();
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
                DataTable dt = req.getDataRequest(reqId);
                Session["grdRequest"] = dt;
                grdData.DataSource = dt;
                grdData.DataBind();
                setContentDateForEdit(dt);
                btnOK.Text = "บันทึกการแก้ไข";
                btnDelReq.Attributes.Add("style", "");
            }
        }

        private void bindDDLProject()
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            DataTable dt = dasp.getDataMasterProject();
            ddlProject.DataSource = dt;
            ddlProject.DataTextField = "ProjectName";
            ddlProject.DataValueField = "Project_Id";
            ddlProject.DataBind();
        }

        private void bindDDLItem()
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            Entities.MasterItemInfo item = new Entities.MasterItemInfo();
            item.ItemCostEnd = 9999999;
            DataTable dt = dasp.getDataMasterItem(item);
            ddlItem.DataSource = dt;
            ddlItem.DataTextField = "ItemName";
            // ddlItem.DataValueField = "ItemNo";
            ddlItem.DataValueField = "MasterItemId";
            ddlItem.DataBind();
        }

        private void bindDDLReqType()
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            DataTable dt = dasp.getDataStatus("6");
            ddlReqType.DataSource = dt;
            ddlReqType.DataTextField = "StatusText";
            ddlReqType.DataValueField = "StatusValue";
            ddlReqType.DataBind();
            ddlReqType.Items.Remove(ddlReqType.Items.FindByValue("1"));
        }

        protected void btnAddRequest_Click(object sender, EventArgs e)
        {
            hdfTabActive.Value = "lireqdetail";
            string Project_Id = ddlProject.SelectedItem.Value;
            string ProjectName = ddlProject.SelectedItem.Text;
            string ItemId = ddlItem.SelectedItem.Value;
            string ItemName = ddlItem.SelectedItem.Text;
            string startDate = txtDateStart.Text;
            string endDate = txtDateEnd.Text;
            string ReqAmount = txtReqAmount.Text;
            string ItemUnit = lbUnit.Text;

//            ReqHeaderId	ReqNo	ReqDate	ReqBy	ReqType	ReqHeaderRemark	Project_Id	ProjectID	ProjectName	ProStartDate	
//            ProEndDate	ItemId	ItemNo	ItemName	ReqAmount	ItemUnit


            if (startDate == "" || endDate == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('กรุณาระบุวันที่โปรโมชั่น');", true);
                return;
            }
            if (ReqAmount == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('กรุณาระบุจำนวนที่ต้องการเบิก');", true);
                return;
            }

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
            if (txtReqNo.Text == "" || txtRequestBy.Text == "" || txtRequestDate.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('กรุณากรอกข้อมูลให้ครบถ้วน');", true);
                hdfTabActive.Value = "lireq";
                return;
            }
            if (dt.Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ไม่พบรายการสินค้าที่ต้องการเบิก');", true);
                hdfTabActive.Value = "lireqdetail";
                return;
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
            req.ReqDate = txtRequestDate.Text;
            req.ReqBy = txtRequestBy.Text;
            req.ReqType = ddlReqType.SelectedItem.Value;
            req.ReqHeaderRemark = txtReqHeaderRemark.Text;

            List<Entities.RequisitionInfo> reqdetail = new List<Entities.RequisitionInfo>();
            DataTable dt = (DataTable)Session["grdRequest"];
            foreach (DataRow dr in dt.Rows)
            {
                Entities.RequisitionInfo reqItem = new Entities.RequisitionInfo();
                reqItem.Project_Id = (int)dr["Project_Id"];
                reqItem.ProjectName = (string)dr["ProjectName"];
                reqItem.ProStartDate = ((DateTime)dr["ProStartDate"]).ToString();
                reqItem.ProEndDate = ((DateTime)dr["ProEndDate"]).ToString();
                reqItem.ItemId = (int)dr["ItemId"];
                // reqItem.ItemNo = (string)dr["ItemNo"];
                reqItem.ItemName = (string)dr["ItemName"];
                reqItem.ReqAmount = (int)dr["ReqAmount"];
                // reqItem.ItemUnit = (string)dr["ItemUnit"];
                reqdetail.Add(reqItem);
            }

            Class.DARequisition cls_req = new Class.DARequisition();
            bool rst = cls_req.createRequest(req, reqdetail);
            if (rst)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "bindDataParentPage();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('บันทึกข้อมูลผิดพลาด กรุณาติดต่อผู้ดูแลระบบ');", true);
            }
        }



        private void setContentDateForEdit(DataTable dt)
        {
            txtReqNo.Text = dt.Rows[0]["ReqNo"] + "";
            txtRequestDate.Text = ((DateTime)dt.Rows[0]["ReqDate"]).ToString("M/d/yyyy");
            txtRequestBy.Text = dt.Rows[0]["ReqBy"] + "";
            ddlReqType.SelectedIndex = ddlReqType.Items.IndexOf(ddlReqType.Items.FindByValue(dt.Rows[0]["ReqType"] + ""));
            txtReqHeaderRemark.Text = dt.Rows[0]["ReqHeaderRemark"] + "";
        }

        public void SaveEditRequest()
        {
            Entities.RequisitionInfo req = new Entities.RequisitionInfo();
            req.ReqHeaderId = Convert.ToInt64(hdfReqId.Value);
            req.ReqNo = txtReqNo.Text;
            req.ReqDate = txtRequestDate.Text;
            req.ReqBy = txtRequestBy.Text;
            req.ReqType = ddlReqType.SelectedItem.Value;
            req.ReqHeaderRemark = txtReqHeaderRemark.Text;

            List<Entities.RequisitionInfo> reqdetail = new List<Entities.RequisitionInfo>();
            DataTable dt = (DataTable)Session["grdRequest"];
            foreach (DataRow dr in dt.Rows)
            {
                Entities.RequisitionInfo reqItem = new Entities.RequisitionInfo();
                reqItem.ReqHeaderId = Convert.ToInt64(hdfReqId.Value);                
                reqItem.ReqId = (long)dr["ReqId"];
                reqItem.Project_Id = (int)dr["Project_Id"];
                // reqItem.ProjectID = (string)dr["ProjectID"];
                reqItem.ProjectName = (string)dr["ProjectName"];
                reqItem.ProStartDate = ((DateTime)dr["ProStartDate"]).ToString();
                reqItem.ProEndDate = ((DateTime)dr["ProEndDate"]).ToString();
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
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "bindDataParentPage();", true);
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

        protected void btnDelReq_Click(object sender, EventArgs e)
        {
            // hdfReqId.Value
            Class.DARequisition cls_req = new Class.DARequisition();
            cls_req.deleteRequest(Convert.ToInt64(hdfReqId.Value));

        }
    }
}