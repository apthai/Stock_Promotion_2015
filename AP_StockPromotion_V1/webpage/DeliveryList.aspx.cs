using AP_StockPromotion_V1.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AP_StockPromotion_V1.webpage
{
    public partial class DeliveryList : Page
    {
        Entities.FormatDate convertDate = new Entities.FormatDate();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                initPage();
                Session["selDelv"] = "";
                Session["grdData"] = null; Session["grdSort"] = null; Session["grdPage"] = null;
            }
        }

        private void initPage()
        {
            bindDDLProject();
            bindDDLItem();
            string rqBind = Request.QueryString["bindData"] + "";
            if (rqBind == "Y") { bindDDLProject(); }
        }

        private void bindDDLProject()
        {
            ddlProject.Items.Clear();
            DAStockPromotion dasp = new DAStockPromotion();
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
            DAStockPromotion dasp = new DAStockPromotion();
            Entities.MasterItemInfo item = new Entities.MasterItemInfo();
            item.ItemCostEnd = 9999999;
            DataTable dt = dasp.getDataMasterItem(item);
            ddlItem.DataSource = dt;
            ddlItem.DataTextField = "ItemNoName";
            ddlItem.DataValueField = "ItemNo";
            ddlItem.DataBind();
            ddlItem.Items.Insert(0, new ListItem("ทั้งหมด", ""));
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Session["selDelv"] = "";
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

            string IsPostBack = "";
            if (cbxDelvStatus.SelectedValue == "1") { IsPostBack = "N"; };
            if (cbxDelvStatus.SelectedValue == "2") { IsPostBack = "Y"; };

            DADelivery cls = new DADelivery();
            DataTable dt = cls.GetDataDeliveryItem(txtDeliveryNo.Text, ddlProject.SelectedItem.Value, ddlItem.SelectedItem.Value, dateBeg, dateEnd, "", "N", "Y", IsPostBack);
            if (dt != null)
            {
                Session["dtDelvPost"] = dt;
                DataTable dtGrd = dt.DefaultView.ToTable(true, "DelvLstId", "isConfirm", "isPostAcc", "DelvPromotionId", "DelvDate", "ItemId", "ItemName", "CostCenter", "PostRet_Key", "PostRetKey", "ProjectCode", "ProjectName", "WBS", "WBS_SAP", "ProjectID", "CompanySAPCode", "PRItem", "PRNo");
                dtGrd.Columns.Add(new DataColumn("Amount", typeof(int)));
                foreach (DataRow dr in dtGrd.Rows)
                {
                    string cond = "ISNULL(DelvLstId,'') = '" + dr["DelvLstId"] + "' AND ISNULL(isConfirm,'') = '" + dr["isConfirm"] + "' AND ISNULL(isPostAcc,'') = '" + dr["isPostAcc"] + "' AND ISNULL(DelvPromotionId,'') = '" + dr["DelvPromotionId"] + "' AND ISNULL(ItemId,'') = '" + dr["ItemId"] + "' AND ISNULL(PostRetKey,'') = '" + dr["PostRetKey"] + "' AND ISNULL(ProjectCode,'') = '" + dr["ProjectCode"] + "' AND ISNULL(WBS,'') = '" + dr["WBS"] + "' AND ISNULL(WBS_SAP,'') = '" + dr["WBS_SAP"] + "' AND ISNULL(ProjectID,'') = '" + dr["ProjectID"] + "' AND ISNULL(CompanySAPCode,'') = '" + dr["CompanySAPCode"] + "'"; //  AND ISNULL(DelvDate,'') = '" + dr["DelvDate"] + "'
                    dr["Amount"] = dt.Compute("COUNT(ItemId)", cond);
                }
                dtGrd.AcceptChanges();

                Session["grdData"] = dtGrd;
                bindGrid();
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Session["grdData"] = null; Session["grdSort"] = null; Session["grdPage"] = null;
            txtDateFrom.Text = "";
            txtDateTo.Text = "";
            txtDeliveryNo.Text = "";
            ddlProject.SelectedIndex = 0;
            cbxDelvStatus.SelectedIndex = 0;
        }

        private void keepSelDelv()
        {
            string selDelv = Session["selDelv"] + "";

            foreach (GridViewRow gr in grdData.Rows)
            {
                CheckBox chkSelDelv = (CheckBox)gr.FindControl("grdChkSelDelv");
                if (chkSelDelv.Style.Value != "display:none;")
                {
                    HiddenField hdfDelvLstId = (HiddenField)gr.FindControl("grdHdfDelvLstId");
                    selDelv = selDelv.Replace("#" + hdfDelvLstId.Value + "#", "");
                }
            }

            foreach (GridViewRow gr in grdData.Rows)
            {
                CheckBox chkSelDelv = (CheckBox)gr.FindControl("grdChkSelDelv");
                if (chkSelDelv.Style.Value != "display:none;")
                {
                    HiddenField hdfDelvLstId = (HiddenField)gr.FindControl("grdHdfDelvLstId");
                    if (chkSelDelv.Checked)
                    {
                        selDelv = selDelv + "#" + hdfDelvLstId.Value + "#";
                    }
                }
            }
            Session["selDelv"] = selDelv;
        }

        protected void btnPostAccount_Click(object sender, EventArgs e)
        {
            keepSelDelv();
            string selDelv = Session["selDelv"] + "";
            if (selDelv == "")
            {
                execJScript("alert('กรุณาเลือกรายการใบส่งมอบที่ต้องการ !!');");
                return;
            }
            selDelv = selDelv.Remove(selDelv.Length - 1, 1).Remove(0, 1).Replace("##", ",");

            DataTable dtGrd_C = ((DataTable)Session["grdData"]).Copy();
            dtGrd_C.DefaultView.RowFilter = "DelvLstId in (" + selDelv + ")";

            /* - 2016.05.03 บันทึกบัญชี บริษัทเดียวกันเท่านั้น [เดิมตรวสอบระดับโครงการ] - */
            // if (dtGrd_C.DefaultView.ToTable(true, "CompanySAPCode", "ProjectID").Rows.Count != 1)
            if (dtGrd_C.DefaultView.ToTable(true, "CompanySAPCode").Rows.Count != 1)
            {
                // ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('กรุณาเลือกส่งมอบจากโครงการเดียวเท่านั้น !!');", true);
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('กรุณาเลือกรายการจากบริษัทเดียวเท่านั้น !!');", true);
                return;
            }

            string CompanySAPCode = dtGrd_C.DefaultView.ToTable(true, "CompanySAPCode").Rows[0]["CompanySAPCode"] + "";
            dtGrd_C.DefaultView.RowFilter = "";

            if (CompanySAPCode == "1000")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "OpenColorBox('DeliveryPostSum.aspx?DelvLstId=" + selDelv + "','91%','79%');", true);
                return;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "OpenColorBox('DeliveryPostSumCrossCmp.aspx?DelvLstId=" + selDelv + "','91%','79%');", true);
                return;
            }
        }

        protected void grdData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                Session["TopDelvId"] = "";
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string topDelvId = Session["TopDelvId"] + "";
                ImageButton imgPrt = (ImageButton)e.Row.FindControl("imgPrt");
                HiddenField hdfDelvLstId = (HiddenField)e.Row.FindControl("grdHdfDelvLstId");
                CheckBox chkSelDelv = (CheckBox)e.Row.FindControl("grdChkSelDelv");
                if (topDelvId == hdfDelvLstId.Value)
                {
                    chkSelDelv.Attributes.Add("style", "display:none;");
                }
                else
                {
                    Session["TopDelvId"] = hdfDelvLstId.Value;
                }
                HiddenField isPostAcc = (HiddenField)e.Row.FindControl("grdHdfIsPostAcc");
                chkSelDelv.Attributes.Remove("style");
                if (isPostAcc.Value == "Y")
                {
                    chkSelDelv.Attributes.Add("style", "display:none;");
                }
                else
                {
                    imgPrt.Attributes.Add("style", "display:none;");
                    string selDelv = Session["selDelv"] + "";
                    if (selDelv.IndexOf("#" + hdfDelvLstId.Value + "#") > -1)
                    {
                        chkSelDelv.Checked = true;
                    }
                }
            }

        }

        private void execJScript(string jscript)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "js", jscript, true);
            return;
        }

        private void bindGrid()
        {
            DataTable dt = (DataTable)Session["grdData"];
            if (Session["grdSort"] + "" != "") { dt.DefaultView.Sort = Session["grdSort"] + ""; }
            grdData.DataSource = dt.DefaultView;
            if (Session["grdPage"] + "" != "") { grdData.PageIndex = (int)Session["grdPage"]; }
            grdData.DataBind();
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

        protected void grdData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            keepSelDelv();
            Session["grdPage"] = e.NewPageIndex;
            bindGrid();
        }

        protected void grdData_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName + "" == "PrtDoc")
            {
                Session["SAPDOC"] = e.CommandArgument;
                GridViewRow gvr = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                HiddenField grdHdfCompanySAPCode = (HiddenField)gvr.FindControl("grdHdfCompanySAPCode");
                string urlRptPstAccIO = "frmReport.aspx?reqRPT=PstAccWBS";
                if (grdHdfCompanySAPCode.Value != "1000")
                {
                    urlRptPstAccIO = "frmReport.aspx?reqRPT=PstAccWBSCrs";
                }
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "Popup60('" + urlRptPstAccIO + "');", true);
            }
        }

        [WebMethod]
        public static object[] GetAccountingRecorded()
        {
            try
            {
                DADelivery dadrv = new DADelivery();
                DataTable dt = (DataTable)dadrv.GetAccountingRecordedWBS()[0];
                var res = (from t1 in dt.AsEnumerable()
                           select new AccountRecorded
                           {
                               SAPREFID = t1.ItemArray[0].ToString(),
                               SAPID = t1.ItemArray[1].ToString(),
                               PSTDATE = t1.ItemArray[2].ToString(),
                               DOCNO = t1.ItemArray[3].ToString(),
                               ITEM = t1.ItemArray[4].ToString(),
                           }).ToList();

                var json = new JavaScriptSerializer().Serialize(res);

                return new object[]
                {
                    json
                };
            }
            catch (Exception ex)
            {
                return new object[]
                {
                    ex.Message.ToString()
                };
            }
        }

        [WebMethod]
        public static object[] ReversingAccountsWBS(string docid, string remark, string postingDate, string reasonId, string reasonName, string empid)
        {
            try
            {
                DADelivery dadrv = new DADelivery();
                object[] objData = dadrv.GetDataForReverseDoc(docid);

                DataTable dt = (DataTable)objData[0];

                if ((dt ?? new DataTable()).Rows.Count == 0)
                {
                    return new object[] { "Data not Found : spGetDataForReverseDocWBS @SAPID = " + docid ?? "" };
                }

                string Mode = "";
                List<string> lstStr = new List<string>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Mode = dt.Rows[i].ItemArray[0].ToString();

                    lstStr.Add(dt.Rows[i].ItemArray[1].ToString());
                    lstStr.Add(dt.Rows[i].ItemArray[2].ToString());
                    lstStr.Add(dt.Rows[i].ItemArray[3].ToString());
                    lstStr.Add(dt.Rows[i].ItemArray[4].ToString());
                }
                string revSapDocNo = "";
                string revSapDocNo_1 = "";
                string revSapDocNo_2 = "";
                string revSapDocNo_3 = "";
                string msgErr = "";
                DASAPConnector dasap = new DASAPConnector();
                if (Mode == "0")
                {
                    string OBJ_TYPE = "BKPFF";
                    string OBJ_SYS = "APQ";
                    string OBJ_KEY_R = lstStr[0].ToString();
                    DateTime PSTNG_DATE = Convert.ToDateTime(postingDate.Replace("/", "."));
                    string FIS_PERIOD = postingDate.Substring(3, 2);
                    string COMP_CODE = lstStr[0].ToString().Substring(10, 4);
                    string REASON_REV = reasonId;
                    string AC_DOC_NO = lstStr[0].ToString().Substring(0, 10);

                    dasap.ReverseAccount(OBJ_TYPE, OBJ_SYS, OBJ_KEY_R, PSTNG_DATE, FIS_PERIOD, COMP_CODE, REASON_REV, AC_DOC_NO, out revSapDocNo, out msgErr);
                    if (msgErr == "")
                    {
                        object[] objret_data = dadrv.GetObjectPRNo(AC_DOC_NO);

                        if (objret_data.Length > 0 && objret_data[0].ToString() == "")
                        {
                            var lstPRNo = ((DataTable)objret_data[1]).AsEnumerable()
                                                    .GroupBy(q => new { PRNo = q["PRNo"] })
                                                    .OrderBy(o => o.Key.PRNo)
                                                    .ToList();

                            for (int i = 0; i < lstPRNo.Count; i++)
                            {
                                var lstPRItem = ((DataTable)objret_data[1]).AsEnumerable()
                                                .Where(q => q["PRNo"].Equals(lstPRNo[i].Key.PRNo))
                                                .OrderBy(o => o["PRItem"])
                                                .ToList();

                                //cancel delete PR
                                dasap.CancelDeletePR(lstPRNo[i].Key.PRNo.ToString(), "", "", lstPRItem, out msgErr);
                            }
                        }
                        if (msgErr == "")
                        {
                            string msgerr = dadrv.InsertReverseAccount(docid, remark, postingDate, reasonId, reasonName, revSapDocNo, empid);
                            if (msgerr == "")
                            {
                                return new object[] {
                                    revSapDocNo.Substring(0, 10)
                                    ,GetAccountingRecorded()
                                };
                            }
                        }
                    }
                    return new object[]
                    {
                        msgErr
                    };
                }
                else
                {
                    string OBJ_TYPE = "BKPFF";
                    string OBJ_SYS = "APQ";
                    string OBJ_KEY_R_1 = lstStr[1].ToString();
                    string OBJ_KEY_R_2 = lstStr[2].ToString();
                    string OBJ_KEY_R_3 = lstStr[3].ToString();
                    DateTime PSTNG_DATE = Convert.ToDateTime(postingDate.Replace("/", "."));
                    string FIS_PERIOD = postingDate.Substring(3, 2);
                    string COMP_CODE_1 = lstStr[1].ToString().Substring(10, 4);
                    string COMP_CODE_2 = lstStr[2].ToString().Substring(10, 4);
                    string COMP_CODE_3 = lstStr[3].ToString().Substring(10, 4);
                    string REASON_REV = reasonId;
                    string AC_DOC_NO_1 = lstStr[1].ToString().Substring(0, 10);
                    string AC_DOC_NO_2 = lstStr[2].ToString().Substring(0, 10);
                    string AC_DOC_NO_3 = lstStr[3].ToString().Substring(0, 10);

                    //reverse account
                    dasap.ReverseAccountCrossComCode(OBJ_TYPE, OBJ_SYS,
                                                     OBJ_KEY_R_1, OBJ_KEY_R_2, OBJ_KEY_R_3,
                                                     PSTNG_DATE, FIS_PERIOD,
                                                     COMP_CODE_1, COMP_CODE_2, COMP_CODE_3,
                                                     REASON_REV,
                                                     AC_DOC_NO_1, AC_DOC_NO_2, AC_DOC_NO_3,
                                                     out revSapDocNo_1, out revSapDocNo_2, out revSapDocNo_3, out msgErr);

                    if (msgErr == "")
                    {
                        object[] objret_data = dadrv.GetObjectPRNo(AC_DOC_NO_1);

                        if (objret_data.Length > 0 && objret_data[0].ToString() == "")
                        {

                            var lstPRNo = ((DataTable)objret_data[1]).AsEnumerable()
                                                    .GroupBy(q => new { PRNo = q["PRNo"] })
                                                    .OrderBy(o => o.Key.PRNo)
                                                    .ToList();

                            for (int i = 0; i < lstPRNo.Count; i++)
                            {
                                var lstPRItem = ((DataTable)objret_data[1]).AsEnumerable()
                                                .Where(q => q["PRNo"].Equals(lstPRNo[i].Key.PRNo))
                                                .OrderBy(o => o["PRItem"])
                                                .ToList();

                                //cancel delete PR
                                dasap.CancelDeletePR(lstPRNo[i].Key.PRNo.ToString(), "", "", lstPRItem, out msgErr);
                            }
                        }

                        if (msgErr == "")
                        {
                            string msgerr = dadrv.InsertReverseAccountCrossCenter(docid, remark, postingDate, reasonId, reasonName,
                                                                  revSapDocNo_1, revSapDocNo_2, revSapDocNo_3, empid);
                            if (msgerr == "")
                            {
                                return new object[] {
                                    revSapDocNo_1.Substring(0, 10) + ", " + revSapDocNo_2.Substring(0, 10) + ", " + revSapDocNo_3.Substring(0, 10)
                                    ,GetAccountingRecorded()
                                };
                            }
                        }
                    }
                    return new object[]
                    {
                        msgErr
                    };
                }
            }
            catch (Exception ex)
            {
                return new object[]
                {
                    ex.Message.ToString()
                };
            }
        }

        public class AccountRecorded
        {
            public string SAPREFID { get; set; }
            public string SAPID { get; set; }
            public string DOCNO { get; set; }
            public string ITEM { get; set; }
            public string PSTDATE { get; set; }
        }
    }
}