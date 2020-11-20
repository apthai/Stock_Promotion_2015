using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AP_StockPromotion_V1.web
{
    public partial class StockTransferItem : System.Web.UI.Page
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
            Session["grdData"] = null; Session["grdSort"] = null; Session["grdPage"] = null; 
            bindDDLReqStatus();
            bindDDLProject();
            bindDDLItem();
            Session["SelReq"] = null; // เลือกโครงการไหนบ้าง
            btnTrnMulReq.Attributes.Add("style", "display:none;");
        }

        private void bindDDLReqStatus()
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            DataTable dt = dasp.getDataStatus("8");
            ddlReqStatus.DataSource = dt;
            ddlReqStatus.DataTextField = "StatusText";
            ddlReqStatus.DataValueField = "StatusValue";
            ddlReqStatus.DataBind();
            ddlReqStatus.Items.Insert(0, new ListItem("ทั้งหมด", ""));
        }

        private void bindDDLProject()
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            DataTable dt = dasp.getDataMasterProject(true);
            ddlProject.DataSource = dt;
            ddlProject.DataTextField = "ProjectName";
            ddlProject.DataValueField = "ProjectID";
            ddlProject.DataBind();
            ddlProject.Items.Insert(0, new ListItem("ทั้งหมด", "0"));
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            bindData();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtDateFrom.Text = "";
            txtDateTo.Text = "";
            txtReqDocNo.Text = "";
            txtReqNo.Text = "";
            ddlItem.SelectedIndex = 0;
            ddlProject.SelectedIndex = 0;
            ddlReqStatus.SelectedIndex = 0;
        }

        private void bindData()
        {
            Class.DATransferToProject cls_req = new Class.DATransferToProject();
            Entities.RequisitionInfo req = new Entities.RequisitionInfo();
            req.ReqHeaderId = 0; // จะเอาหมด
            req.ReqNo = txtReqNo.Text;
            DateTime dFrom = new DateTime();
            if (!DateTime.TryParse(txtDateFrom.Text, out dFrom))
            {
                dFrom = new DateTime(2000, 01, 01);
            }
            DateTime dTo = new DateTime();
            if (!DateTime.TryParse(txtDateTo.Text, out dTo))
            {
                dTo = new DateTime(3500, 12, 31);
            }
            req.ReqDateFrom = dFrom.ToString();
            req.ReqDateTo = dTo.ToString();
            req.Project_Id = Convert.ToInt32(ddlProject.SelectedItem.Value);
            req.ItemId = Convert.ToInt32(ddlItem.SelectedItem.Value);
            req.ReqDocNo = txtReqDocNo.Text;
            
            DataTable dt = cls_req.getDataRequestDetail(req, 1);


            DataTable dtGrdShow = dt.DefaultView.ToTable(true, "ReqHeaderId", "ReqNo", "ReqDocNo", "ReqDocDate", "ReqDate", "ReqBy", "ReqType", "ReqHeaderRemark", "Project_Id", "ProjectName");// , "ProStartDate", "ProEndDate"
            dtGrdShow.Columns.Add(new DataColumn("ReqId", typeof(Int64)));
            dtGrdShow.Columns.Add(new DataColumn("ReqStatus", typeof(String)));
            dtGrdShow.Columns.Add(new DataColumn("ReqStatusText", typeof(String)));
            dtGrdShow.AcceptChanges();
            foreach (DataRow dr in dtGrdShow.Rows)
            {
                DataRow[] drSel =  dt.Select("ReqHeaderId=" + dr["ReqHeaderId"] + " and Project_Id = " + dr["Project_Id"]);
                dr["ReqId"] = drSel[0]["ReqId"];
                DataTable dtSel = dt.Clone();
                foreach(DataRow drS in drSel){
                    dtSel.ImportRow(drS);                
                }

                dtSel.AcceptChanges();
                int s0 = dtSel.Select("ReqStatus = '0'").Length;
                int s1 = dtSel.Select("ReqStatus = '1'").Length;
                int s2 = dtSel.Select("ReqStatus = '2'").Length;
                int s3 = dtSel.Select("ReqStatus = '3'").Length;
                if (s0 > 0 && s1 == 0 && s2 == 0 && s3 == 0) { dr["ReqStatus"] = "0"; dr["ReqStatusText"] = "ยกเลิก"; }
                else if (s0 >= 0 && s1 > 0 && s2 == 0 && s3 == 0) { dr["ReqStatus"] = "1"; dr["ReqStatusText"] = "ยังไม่จ่าย"; }
                else if (s0 >= 0 && s1 == 0 && s2 == 0 && s3 > 0) { dr["ReqStatus"] = "3"; dr["ReqStatusText"] = "จ่ายครบแล้ว"; }
                else { dr["ReqStatus"] = "2"; dr["ReqStatusText"] = "ระหว่างดำเนินการ"; } /* if (s0 >= 0 && s1 >= 0 && s2 >= 0 && s3 >= 0)  */
            }
            dtGrdShow.AcceptChanges();
            dtGrdShow.DefaultView.RowFilter = "ReqStatus = '" + ddlReqStatus.SelectedItem.Value + "' OR '" + ddlReqStatus.SelectedItem.Value + "' = ''";
            dtGrdShow = dtGrdShow.DefaultView.ToTable();
            dtGrdShow.DefaultView.RowFilter = "";
            
            Session["grdData"] = dtGrdShow;
            bindGrid();
            //grdData.DataSource = dtGrdShow;
            //grdData.DataBind();
            btnTrnMulReq.Attributes.Add("style","display:none;");
            if (dtGrdShow != null)
            {
                if (dtGrdShow.Rows.Count != 0)
                {
                    btnTrnMulReq.Attributes.Remove("style");
                }
            }
        }

        private void bindGrid()
        {
            DataTable dt = (DataTable)Session["grdData"];
            if (Session["grdSort"] + "" != "") { dt.DefaultView.Sort = Session["grdSort"] + ""; }
            grdData.DataSource = dt.DefaultView;
            if (Session["grdPage"] + "" != "") { grdData.PageIndex = (int)Session["grdPage"]; }
            grdData.DataBind();
        }

        protected void grdData_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "takeReq")
            {
                //ScriptManager.RegisterStartupScript(this, GetType(), "js", "popupStockTransfrtItemEdit('" + e.CommandArgument + "');", true);
                GridViewRow gvr = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                string reqHeaderId = ((HiddenField)gvr.FindControl("grdHdfReqHeaderId")).Value;
                string reqProject = ((HiddenField)gvr.FindControl("grdHdfProject_Id")).Value;
                string reqId = ((HiddenField)gvr.FindControl("grdHdfReqId")).Value;
                Response.Redirect("DisbursementDetail.aspx?mode=Edit&reqIdList=" + reqId);
                // Response.Redirect("StockTransferItemDetail.aspx?mode=Edit&reqHeaderId=" + reqHeaderId + "&reqProject=" + reqProject);
            }
        }

        protected void grdData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.Header) { Session["firstReqHeaderId"] = null; }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Session["SelReq"] != null)
                {
                    string selReq = Session["SelReq"] + "";

                    HiddenField hdfReqId = (HiddenField)e.Row.FindControl("grdHdfReqId");
                    if (selReq.IndexOf("#" + hdfReqId.Value + "#") != -1)
                    {
                        CheckBox chkReq = (CheckBox)e.Row.FindControl("chkReq");
                        chkReq.Checked = true;
                    }
                }
                HiddenField hdfReqStatus = (HiddenField)e.Row.FindControl("grdHdfReqStatus");
                Label lbReqStatus = (Label)e.Row.FindControl("grdLbReqStatus");
                if (hdfReqStatus.Value == "0") { lbReqStatus.ForeColor = System.Drawing.Color.Gray; }
                else if (hdfReqStatus.Value == "1") { lbReqStatus.ForeColor = System.Drawing.Color.Red; }
                else if (hdfReqStatus.Value == "2") { lbReqStatus.ForeColor = System.Drawing.Color.Blue; }
                else if (hdfReqStatus.Value == "3") { lbReqStatus.ForeColor = System.Drawing.Color.Green; }
             
                //HiddenField  hdfReqHeaderId = (HiddenField)e.Row.FindControl("grdHdfReqHeaderId");
                //if (Session["firstReqHeaderId"] + "" == hdfReqHeaderId.Value)
                //{
                //    ((CheckBox)e.Row.FindControl("chkReq")).Attributes.Add("style", "display:none;");
                //}
                //else
                //{
                //    Session["firstReqHeaderId"] = hdfReqHeaderId.Value;
                //}
                //   "#" + dr["ReqId"] + "#";
                //Label lbTotalAmount = (Label)e.Row.FindControl("lbTotalAmount");
                //HiddenField grdHdfItemPricePerUnit = (HiddenField)e.Row.FindControl("grdHdfItemPricePerUnit");
                //HiddenField grdHdfReqAmount = (HiddenField)e.Row.FindControl("grdHdfReqAmount");

                //decimal price = 0;
                //decimal amount = 0;

                //decimal.TryParse(grdHdfItemPricePerUnit.Value, out price);
                //decimal.TryParse(grdHdfReqAmount.Value, out amount);

                //lbTotalAmount.Text = (price * amount).ToString("#,##0.00");
            }
        }

        protected void grdData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            KeepSelReq();
            Session["grdPage"] = e.NewPageIndex;
            bindGrid();
        }

        protected void btnTrnMulReq_Click(object sender, EventArgs e)
        {
            KeepSelReq();
            string selFn = Session["SelFn"] + "";
            string selReq = Session["SelReq"] + "";
            if (selReq == "")
            {
                exeJSript("alert('กรุณาเลือกใบเบิกที่ต้องการ !!');");
                return;
            }
            selReq = selReq.Replace("##", ",").Remove(0, 1);
            selReq = selReq.Remove(selReq.Length - 1, 1);

            selFn = selFn.Remove(selFn.Length - 1, 1);

            Class.DATransferToProject req = new Class.DATransferToProject();
            DataTable dt = req.getDataRequestListByRequestList(selReq, selFn);

            if (dt.DefaultView.ToTable(true, "ReqType", "project_Id").Rows.Count > 1)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('กรุณาเลือกใบเบิกประเภทและโครงการเดียวกัน เท่านั้น !!');", true);
                return;
            }

            Response.Redirect("DisbursementDetail.aspx?mode=Edit&reqIdList=" + selReq);
        }

        private void KeepSelReq()
        {
            string selReq = Session["SelReq"] + "";
            string selFn = string.Empty;

            // ลบรายการเก่าในหน้านี้
            foreach (GridViewRow gr in grdData.Rows)
            {
                if (gr.RowType == DataControlRowType.DataRow)
                {
                    int reqHeaderId = 0;
                    int reqProject = 0;
                    int reqFunction = 0;
                    HiddenField hdfReqHeaderId = (HiddenField)gr.FindControl("grdHdfReqHeaderId");
                    HiddenField hdfReqProject = (HiddenField)gr.FindControl("grdHdfProject_Id");
                    HiddenField hdfReqFunction = (HiddenField)gr.FindControl("grdHdfFunction");

                    if (!int.TryParse(hdfReqHeaderId.Value, out reqHeaderId))
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ข้อมูลผิดพลาด กรุณาติดต่อผู้ดูแลระบบ');", true);
                        return;
                    }
                    if (!int.TryParse(hdfReqProject.Value, out reqProject))
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ข้อมูลผิดพลาด กรุณาติดต่อผู้ดูแลระบบ');", true);
                        return;
                    }
                    if (!int.TryParse(hdfReqFunction.Value, out reqFunction))
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ข้อมูลผิดพลาด กรุณาติดต่อผู้ดูแลระบบ');", true);
                        return;
                    }
                    DataTable dt;
                    Class.DATransferToProject req = new Class.DATransferToProject();
                    dt = req.getDataRequestList(reqHeaderId, reqProject, reqFunction);

                    CheckBox chk = (CheckBox)gr.FindControl("chkReq");
                    foreach (DataRow dr in dt.Rows)
                    {
                        selReq = selReq.Replace("#" + dr["ReqId"] + "#", "");
                    }
                }
            }

            // เก็บรายการที่ติ๊กถูกในหน้านี้
            foreach (GridViewRow gr in grdData.Rows)
            {
                if (gr.RowType == DataControlRowType.DataRow)
                {
                    // grdHdfReqHeaderId || grdHdfProject_Id : เอามาหา ReqId
                    int reqHeaderId = 0;
                    int reqProject = 0;
                    int reqFunction = 0;
                    HiddenField hdfReqHeaderId = (HiddenField)gr.FindControl("grdHdfReqHeaderId");
                    HiddenField hdfReqProject = (HiddenField)gr.FindControl("grdHdfProject_Id");
                    HiddenField hdfReqFunction = (HiddenField)gr.FindControl("grdHdfFunction");

                    if (!int.TryParse(hdfReqHeaderId.Value, out reqHeaderId))
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ข้อมูลผิดพลาด กรุณาติดต่อผู้ดูแลระบบ');", true);
                        return;
                    }
                    if (!int.TryParse(hdfReqProject.Value, out reqProject))
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ข้อมูลผิดพลาด กรุณาติดต่อผู้ดูแลระบบ');", true);
                        return;
                    }
                    if (!int.TryParse(hdfReqFunction.Value, out reqFunction))
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ข้อมูลผิดพลาด กรุณาติดต่อผู้ดูแลระบบ');", true);
                        return;
                    }
                    DataTable dt;
                    Class.DATransferToProject req = new Class.DATransferToProject();
                    dt = req.getDataRequestList(reqHeaderId, reqProject, reqFunction);
                    

                    CheckBox chk = (CheckBox)gr.FindControl("chkReq");
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (chk.Checked)
                        {
                            selReq += "#" + dr["ReqId"] + "#";
                            selFn += dr["Function"].ToString() + ",";
                        }
                    }
                }
            }
            Session["SelReq"] = selReq;
            Session["SelFn"] = selFn;
        }





        private void exeJSript(string txt)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "js", txt, true);
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
            bindGrid();
        }

    }
}