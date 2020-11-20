using ClosedXML.Excel;
using ExcelExtensions;
using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility;

namespace AP_StockPromotion_V1.web
{
    public partial class StockTransferItem2 : Page
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
            Session["grdData"] = null; Session["grdSort"] = null; Session["grdPage"] = null;
            bindDDLReqStatus();
            bindDDLProject();
            bindDDLItem();
            bindDDLProjectFilter();
            bindDDLItemFilter();
            Session["SelReq"] = null; // เลือกโครงการไหนบ้าง
            btnTrnMulReq.Attributes.Add("style", "display:none;");
            if (Request.QueryString["sCond"] + "" != "" && Session["sCond"] != null)
            {
                Entities.ConditionSearchInfo sCond = (Entities.ConditionSearchInfo)Session["sCond"];
                txtReqNo.Text = sCond.PO_No;
                txtReqDocNo.Text = sCond.ReqRefNo;
                txtDateFrom.Text = sCond.DateBeg;
                sCond.DateEnd = txtDateTo.Text;
                ddlProject.SelectedIndex = ddlProject.Items.IndexOf(ddlProject.Items.FindByValue(sCond.ProjectId));
                ddlItem.SelectedIndex = ddlItem.Items.IndexOf(ddlItem.Items.FindByValue(sCond.MatID));
                ddlReqStatus.SelectedIndex = ddlReqStatus.Items.IndexOf(ddlReqStatus.Items.FindByValue(sCond.Status));
                bindData();
            }

        }


        private void keepConditionSearch()
        {
            Entities.ConditionSearchInfo sCond = new Entities.ConditionSearchInfo();
            sCond.PO_No = txtReqNo.Text;
            sCond.ReqRefNo = txtReqDocNo.Text;
            sCond.DateBeg = txtDateFrom.Text;
            sCond.DateEnd = txtDateTo.Text;
            sCond.ProjectId = ddlProject.SelectedItem.Value;
            sCond.MatID = ddlItem.SelectedItem.Value;
            sCond.Status = ddlReqStatus.SelectedItem.Value;
            Session["sCond"] = sCond;
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

        private void bindDDLProjectFilter()
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            DataTable dtFilter = dasp.getDataMasterProject(true);
            ddlProjectFilter.DataSource = dtFilter;
            ddlProjectFilter.DataTextField = "ProjectName";
            ddlProjectFilter.DataValueField = "ProjectID";
            ddlProjectFilter.DataBind();
            ddlProjectFilter.Items.Insert(0, new ListItem("ทั้งหมด", "0"));

            ddlProjectFilter2.DataSource = dtFilter;
            ddlProjectFilter2.DataTextField = "ProjectName";
            ddlProjectFilter2.DataValueField = "ProjectID";
            ddlProjectFilter2.DataBind();
            ddlProjectFilter2.Items.Insert(0, new ListItem("ทั้งหมด", "0"));
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

        private void bindDDLItemFilter()
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            Entities.MasterItemInfo item = new Entities.MasterItemInfo();
            item.ItemCostEnd = 9999999;
            DataTable dt = dasp.getDataMasterItem(item);
            ddlItemFilter.DataSource = dt;
            ddlItemFilter.DataTextField = "ItemNoName";
            ddlItemFilter.DataValueField = "MasterItemId";
            ddlItemFilter.DataBind();
            ddlItemFilter.Items.Insert(0, new ListItem("ทั้งหมด", "0"));
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
            Session["SelReq"] = null;
            Class.DATransferToProject cls_req = new Class.DATransferToProject();
            Entities.RequisitionInfo req = new Entities.RequisitionInfo();
            req.ReqHeaderId = 0; // จะเอาหมด
            req.ReqNo = txtReqNo.Text;
            DateTime dFrom = new DateTime();
            string formatDate = new Entities.FormatDate().formatDate;

            if (DateTime.TryParse(txtDateFrom.Text, out dFrom))
            {
                req.ReqDateFrom = txtDateFrom.Text;
            }
            else
            {
                req.ReqDateFrom = DateTime.Now.AddYears(-5).ToString(formatDate);
            }
            DateTime dTo = new DateTime();
            if (DateTime.TryParse(txtDateTo.Text, out dTo))
            {
                req.ReqDateTo = txtDateTo.Text;
            }
            else
            {
                req.ReqDateTo = DateTime.Now.AddYears(5).ToString(formatDate);
            }

            req.Project_Id = Convert.ToInt32(ddlProject.SelectedItem.Value);
            req.ItemId = Convert.ToInt32(ddlItem.SelectedItem.Value);
            req.ReqDocNo = txtReqDocNo.Text;

            DataTable dt = cls_req.getDataRequestDetail(req, 1);

            if (dt == null) return;

            DataTable dtGrdShow = dt.DefaultView.ToTable(true, "ReqHeaderId", "ReqNo", "ReqDocNo", "FullName", "ReqDocDate", "ReqDate", "ReqBy", "ReqType", "ReqHeaderRemark", "Project_Id", "ProjectName", "PRNo", "ProStartDate", "ProEndDate", "ItemNo", "ItemName", "ReqAmount", "WBSReq", "UnitNo", "Function");

            dtGrdShow.Columns.Add(new DataColumn("ReqId", typeof(long)));
            dtGrdShow.Columns.Add(new DataColumn("ReqStatus", typeof(string)));
            dtGrdShow.Columns.Add(new DataColumn("ReqStatusText", typeof(string)));

            DataView dv = dtGrdShow.DefaultView;
            dv.Sort = "ReqDate Desc";
            dtGrdShow = dv.ToTable();

            dtGrdShow.AcceptChanges();

            int ii = 0;
            foreach (DataRow dr in dtGrdShow.Rows)
            {
                ii++;
                DataRow[] drSel = dt.Select("ReqHeaderId=" + dr["ReqHeaderId"]
                                    + " and Project_Id = '" + dr["Project_Id"]
                                    + "' and ItemName = '" + dr["ItemName"] + "'");
                dr["ReqId"] = drSel[0]["ReqId"];
                DataTable dtSel = dt.Clone();
                foreach (DataRow drS in drSel)
                {
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
            btnTrnMulReq.Attributes.Add("style", "display:none;");
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
            if (e.CommandName == "PrtTrnPB")
            {
                GridViewRow gvr = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                string reqHeaderId = ((HiddenField)gvr.FindControl("grdHdfReqHeaderId")).Value;

                Session["RqHLst"] = reqHeaderId;
                string urlRptTrPB1 = "frmReport.aspx?reqRPT=Trn2ProBooking";
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "Popup60('" + urlRptTrPB1 + "');", true);
                return;
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

                ImageButton imgPrtPB = (ImageButton)e.Row.FindControl("imgPrtPB");
                HiddenField ReqType = (HiddenField)e.Row.FindControl("grdHdfReqType");
                if (ReqType.Value != "3")
                {
                    imgPrtPB.Attributes.Add("style", "display:none;");
                }



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

            //selReq = selReq.Remove(selReq.Length - 1, 1);

            if (!string.IsNullOrEmpty(selFn))
            {
                selFn = selFn.Remove(selFn.Length - 1, 1);
            }

            Class.DATransferToProject req = new Class.DATransferToProject();
            DataTable dt = req.getDataRequestListByRequestList(selReq, selFn);
            if (dt != null)
            {
                if (dt.DefaultView.ToTable(true, "ReqType", "project_Id", "ReqBy").Rows.Count > 1)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('กรุณาเลือกใบเบิกประเภท โครงการ และผู้เบิกเดียวกันเท่านั้น !!');", true);
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('พบข้อผิดพลาดขณะค้นหารายการเพื่อทำการจ่ายกรุณาติดต่อเจ้าหน้าที่ IT !!');", true);
                return;
            }

            Response.Redirect("DisbursementDetail.aspx?mode=Edit&reqIdList=" + selReq + "&reqFnType=" + selFn);
        }

        private void KeepSelReq()
        {
            string selReq = string.Empty;
            string selFn = string.Empty;

            // ลบรายการเก่าในหน้านี้
            //foreach (GridViewRow gr in grdData.Rows)
            //{
            //    if (gr.RowType == DataControlRowType.DataRow)
            //    {
            //        Int64 reqHeaderId = 0;
            //        int reqProject = 0;
            //        int reqFunction = 0;
            //        HiddenField hdfReqHeaderId = (HiddenField)gr.FindControl("grdHdfReqHeaderId");
            //        HiddenField hdfReqProject = (HiddenField)gr.FindControl("grdHdfProject_Id");
            //        HiddenField hdfReqFunction = (HiddenField)gr.FindControl("grdHdfFunction");

            //        if (!Int64.TryParse(hdfReqHeaderId.Value, out reqHeaderId))
            //        {
            //            ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ข้อมูลผิดพลาด กรุณาติดต่อผู้ดูแลระบบ');", true);
            //            return;
            //        }
            //        if (!int.TryParse(hdfReqProject.Value, out reqProject))
            //        {
            //            ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ข้อมูลผิดพลาด กรุณาติดต่อผู้ดูแลระบบ');", true);
            //            return;
            //        }
            //        if (!int.TryParse(hdfReqFunction.Value, out reqFunction))
            //        {
            //            ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ข้อมูลผิดพลาด กรุณาติดต่อผู้ดูแลระบบ');", true);
            //            return;
            //        }
            //        DataTable dt;
            //        Class.DATransferToProject req = new Class.DATransferToProject();
            //        dt = req.getDataRequestList(reqHeaderId, reqProject, reqFunction);

            //        CheckBox chk = (CheckBox)gr.FindControl("chkReq");
            //        foreach (DataRow dr in dt.Rows)
            //        {
            //            selReq = selReq.Replace("#" + dr["ReqId"] + "#", "");
            //        }
            //    }
            //}

            // เก็บรายการที่ติ๊กถูกในหน้านี้
            foreach (GridViewRow gr in grdData.Rows)
            {
                if (gr.RowType == DataControlRowType.DataRow)
                {
                    // grdHdfReqHeaderId || grdHdfProject_Id : เอามาหา ReqId
                    Int64 reqHeaderId = 0;
                    int reqProject = 0;
                    int reqFunction = 0;
                    HiddenField hdfReqHeaderId = (HiddenField)gr.FindControl("grdHdfReqHeaderId");
                    HiddenField hdfReqProject = (HiddenField)gr.FindControl("grdHdfProject_Id");
                    HiddenField hdfReqFunction = (HiddenField)gr.FindControl("grdHdfFunction");

                    if (!Int64.TryParse(hdfReqHeaderId.Value, out reqHeaderId))
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ข้อมูลผิดพลาด กรุณาติดต่อผู้ดูแลระบบ');", true);
                        return;
                    }
                    //if (!int.TryParse(hdfReqProject.Value, out reqProject))
                    //{
                    //    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ข้อมูลผิดพลาด กรุณาติดต่อผู้ดูแลระบบ');", true);
                    //    return;
                    //}
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
                            if (!selReq.Contains(dr["ReqId"].ToString()))
                            {
                                selReq += dr["ReqId"] + ",";
                                selFn += dr["Function"].ToString() + ",";
                            }


                        }
                        //else
                        //{
                        //    selReq = selReq.Replace("#", ",");
                        //    //selReq = selReq.Replace("#" + dr["ReqId"] + "#", "");
                        //    //selFn = selFn.Replace(dr["Function"].ToString() + ",", "");
                        //}
                    }
                }
            }


            if (selReq.Length > 0)
            {
                Session["SelReq"] = selReq.Remove(selReq.Length - 1, 1);
            }
            else
            {
                Session["SelReq"] = null;
            }

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

        public static DateTime? TryParse(string text)
        {
            DateTime date;
            if (DateTime.TryParseExact(text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                return date;
            }
            else
            {
                return null;
            }
        }

        [WebMethod]
        public static string GetReportUrl(string StartOrderDate = "", string EndOrderDate = "", string DOCNO = "", string REFNO = "", string PROJECTID = "", int MasterItemID = 0)
        {
            //string PurchesingOrder = @"frmReport.aspx?reqRPT=ReportPurchesingOrder";

            //HttpContext.Current.Session["StartOrderDate"] = StartOrderDate.Trim() == "" ? "" : Convert.ToDateTime(StartOrderDate).ToString("yyyy-MM-dd");
            //HttpContext.Current.Session["EndOrderDate"] = EndOrderDate.Trim() == "" ? "" : Convert.ToDateTime(EndOrderDate).ToString("yyyy-MM-dd");
            //HttpContext.Current.Session["DOCNO"] = DOCNO;
            //HttpContext.Current.Session["REFNO"] = REFNO;
            //HttpContext.Current.Session["PROJECTID"] = PROJECTID == "0" ? "" : PROJECTID;
            //HttpContext.Current.Session["MasterItemID"] = MasterItemID;

            string PurchesingOrder = string.Format("frmReport.aspx?reqRPT={0}&StartOrderDate={1}&EndOrderDate={2}&DOCNO={3}&REFNO={4}&PROJECTID={5}&MasterItemID={6}"
                , "ReportPurchesingOrder_New", Convert.ToDateTime(StartOrderDate).ToString("yyyy-MM-dd"), Convert.ToDateTime(EndOrderDate).ToString("yyyy-MM-dd"), DOCNO, REFNO, PROJECTID == "" ? "0" : PROJECTID, MasterItemID);

            return PurchesingOrder;
        }

        protected void bntExcelPurchesingOrder_Click(object sender, EventArgs e)
        {
            string StartOrderDate = "";
            if (txtDateStartFilter.Text.Trim() != "")
                StartOrderDate = txtDateStartFilter.Text.ToDate().ToString("yyyy-MM-dd");

            string EndOrderDate = "";
            if (txtDateEndFilter.Text.Trim() != "")
                EndOrderDate = txtDateEndFilter.Text.ToDate().ToString("yyyy-MM-dd");

            string DOCNO = txtReqDocNoFilter.Text;
            string REFNO = txtDocNoFilter.Text;
            string PROJECTID = ddlProjectFilter.SelectedValue;
            string MasterItemID = ddlItemFilter.SelectedValue;

            //string StartOrderDate = "20200801";
            //string EndOrderDate = "20200930";
            //string DOCNO = "";
            //string REFNO = "";
            //string PROJECTID = "";
            //string MasterItemID = "0";

            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            DataTable dt = dasp.Get_spRptPurchesingOrder(StartOrderDate, EndOrderDate, DOCNO, REFNO, PROJECTID, MasterItemID, false);

            if (dt.Rows.Count > 0)
            {
                dt.Columns["RowNumber"].ColumnName = "ลำดับที่";
                dt.Columns["DATE"].ColumnName = "วันที่เบิก";
                dt.Columns["DOCNO"].ColumnName = "เลขที่เบิก";
                dt.Columns["REFNO"].ColumnName = "เลขที่อ้างอิง";
                dt.Columns["PROJDETAILS"].ColumnName = "โครงการ";
                dt.Columns["UnitNo"].ColumnName = "แปลง";
                dt.Columns["REQBY"].ColumnName = "ผู้ขอเบิก";
                dt.Columns["CustomerName"].ColumnName = "ชื่อลูกค้า";
                dt.Columns["CustomerCitizenId"].ColumnName = "เลขที่ประชาชน";
                dt.Columns["CustomerTelNo"].ColumnName = "เบอร์โทร";
                dt.Columns["ItemName"].ColumnName = "รายการสั่งซื้อ";
                dt.Columns["ReqAmount"].ColumnName = "จำนวน";
                //dt.Columns.Remove("ItemNo");

                // Export Excel V1
                XLWorkbook wb = new XLWorkbook();
                var ws = wb.Worksheets.Add(dt, "Report");
                ws.Columns().AdjustToContents();

                MemoryStream MyMemoryStream = new MemoryStream();
                wb.SaveAs(MyMemoryStream);

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                var Filename = string.Format("PurchesingOrder_{0}_{1}.xls", (DateTime.Now.Year < 2500 ? DateTime.Now.Year : DateTime.Now.Year - 543).ToString(), DateTime.Now.ToString("MM-dd_HHmmss"));
                Response.AddHeader("content-disposition", "attachment;filename=" + Filename);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ไม่พบข้อมูล ตามเงื่อนไขที่ค้นหา !!');", true);
            }
        }

        protected void BntExportAurora_Click(object sender, EventArgs e)
        {
            string StartOrderDate = "";
            if (txtDateStartFilter2.Text.Trim() != "")
                StartOrderDate = txtDateStartFilter2.Text.ToDate().ToString("yyyy-MM-dd");

            string EndOrderDate = "";
            if (txtDateEndFilter2.Text.Trim() != "")
                EndOrderDate = txtDateEndFilter2.Text.ToDate().ToString("yyyy-MM-dd");

            string DOCNO = txtReqDocNoFilter2.Text;
            string REFNO = txtDocNoFilter2.Text;
            string PROJECTID = ddlProjectFilter2.SelectedValue;

            //string StartOrderDate = "20200801";
            //string EndOrderDate = "20200930";
            //string DOCNO = "";
            //string REFNO = "";
            //string PROJECTID = "";
            //string MasterItemID = "0";

            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            DataTable dt = dasp.Get_spRptPurchesingOrder(StartOrderDate, EndOrderDate, DOCNO, REFNO, PROJECTID, null, true);

            if (dt.Rows.Count > 0)
            {
                // Export Excel V1
                XLWorkbook wb = new XLWorkbook();
                var ws = wb.Worksheets.Add(dt, "Report");
                ws.Columns().AdjustToContents();

                MemoryStream MyMemoryStream = new MemoryStream();
                wb.SaveAs(MyMemoryStream);

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                var Filename = string.Format("Aurora_{0}_{1}.xls", (DateTime.Now.Year < 2500 ? DateTime.Now.Year : DateTime.Now.Year - 543).ToString(), DateTime.Now.ToString("MM-dd_HHmmss"));
                Response.AddHeader("content-disposition", "attachment;filename=" + Filename);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ไม่พบข้อมูล ตามเงื่อนไขที่ค้นหา !!');", true);
            }
        }

        protected void ButtonX_Click(object sender, EventArgs e)
        {
            string sqlQuery = txtReqDocNoFilter.Text;

            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            var result = dasp.TestX(sqlQuery);

            if (string.IsNullOrEmpty(result))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('Finish');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", $"alert('{result}');", true);
            }
        }

        protected void BntImportAurora_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                string FileName = Path.GetFileName(FileUpload1.PostedFile.FileName);

                #region

                Stream fs = FileUpload1.PostedFile.InputStream;
                BinaryReader br = new BinaryReader(fs);
                byte[] bytes = br.ReadBytes((Int32)fs.Length);

                bool hasHeader = true;
                using (Stream stream = new MemoryStream(XLSToXLSXConverter.ReadFully(fs)))
                {

                    using (MemoryStream xlsxStream = new MemoryStream(bytes))
                    using (var pck = new OfficeOpenXml.ExcelPackage(xlsxStream))
                    {
                        var ws = pck.Workbook.Worksheets.First();
                        DataTable tbl = new DataTable();
                        foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                        {
                            tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                        }

                        var startRow = hasHeader ? 2 : 1;
                        for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                        {
                            var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                            DataRow row = tbl.Rows.Add();
                            foreach (var cell in wsRow)
                            {
                                row[cell.Start.Column - 1] = cell.Text;
                            }
                        }

                        if ((tbl ?? new DataTable()).Rows.Count > 0)
                            ScriptManager.RegisterStartupScript(this, GetType(), "js", $"alert('ผ่าน !!!);", true);
                    }
                }

                #endregion

            }
        }
    }
}