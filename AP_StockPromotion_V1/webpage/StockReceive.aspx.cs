using AP_StockPromotion_V1.Class;
using AP_StockPromotion_V1.ws_authorize;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AP_StockPromotion_V1.web
{
    public partial class StockReceive : System.Web.UI.Page
    {
        string formatDate = new Entities.FormatDate().formatDate;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false) {
                initPage();
            }
        }

        private void initPage()
        {
            Session["grdData"] = null; Session["grdSort"] = null; Session["grdPage"] = null;
            bindDDLItem();
            bindDDLItemGroup();
            if (Request.QueryString["bindData"] + "" != "")
            {
                txtPO.Text = Request.QueryString["PO_No"] + "";
                bindData();
            }
            else if (Request.QueryString["sCond"] + "" != "" && Session["sCond"] != null)
            {
                Entities.ConditionSearchInfo sCond = (Entities.ConditionSearchInfo)Session["sCond"];
                txtPO.Text = sCond.PO_No;
                txtGR.Text = sCond.GR_No;
                txtDateFrom.Text = sCond.DateBeg;
                txtDateTo.Text = sCond.DateEnd;
                ddlItemGroup.SelectedIndex = ddlItemGroup.Items.IndexOf(ddlItemGroup.Items.FindByValue(sCond.MatGrpID));
                ddlItem.SelectedIndex = ddlItem.Items.IndexOf(ddlItem.Items.FindByValue(sCond.MatID));
                bindData();
            }
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

        private void bindDDLItemGroup()
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            Entities.MasterItemGroupInfo item = new Entities.MasterItemGroupInfo();
            DataTable dt = dasp.getDataMasterItemGroup(item);
            ddlItemGroup.DataSource = dt;
            ddlItemGroup.DataTextField = "MasterItemGroupName";
            ddlItemGroup.DataValueField = "MasterItemGroupId";
            ddlItemGroup.DataBind();
            ddlItemGroup.Items.Insert(0, new ListItem("ทั้งหมด", "0"));
        }

        //private void bindDDLUser()
        //{
        //    Class.DAStockPromotion dasp = new Class.DAStockPromotion();
        //    DataTable dt = dasp.getDataUser("", "");
        //    ddlUser.DataSource = dt;
        //    ddlUser.DataTextField = "FullNoName";
        //    ddlUser.DataValueField = "EmpCode";
        //    ddlUser.DataBind();
        //    ddlUser.Items.Insert(0, new ListItem("-- เลือก --", ""));
        //}

        private void bindData()
        {
            keepConditionSearch();
            Entities.StockReceiveInfo rc = new Entities.StockReceiveInfo();
            Class.DAStockReceive dasp = new Class.DAStockReceive();
            rc.PO_No = txtPO.Text.Trim();
            rc.GR_No = txtGR.Text.Trim();
            int itemId = 0;
            if (int.TryParse(ddlItem.SelectedItem.Value, out itemId))
            {
                rc.MasterItemId = itemId;
            } 
            int itemGroupId = 0;
            if (int.TryParse(ddlItemGroup.SelectedItem.Value, out itemGroupId))
            {
                rc.MasterItemGroupId = itemGroupId;
            }

            //rc.CreateBy = ddlUser.SelectedItem.Value;// txtReceiveBy.Text.Trim();
            DateTime datefrom;
            if (DateTime.TryParse(txtDateFrom.Text, out datefrom))
            {
                rc.CreateDateFrom = txtDateFrom.Text;
            }
            else
            {
                rc.CreateDateFrom = DateTime.Now.AddYears(-5).ToString(formatDate);
            }
            DateTime dateTo;
            if (DateTime.TryParse(txtDateTo.Text, out dateTo))
            {
                rc.CreateDateTo = txtDateTo.Text;
            }
            else
            {
                rc.CreateDateTo = DateTime.Now.AddYears(5).ToString(formatDate);
            }
            Session["grdData"] = dasp.getDataReceiveHistory(rc);
            bindGrid();
            //DataTable dt = dasp.getDataReceiveHistory(rc);
            //if (Session["grdSort"] + "" != "") { dt.DefaultView.Sort = Session["grdSort"] + ""; }
            //grdData.DataSource = dt.DefaultView;
            //if (Session["grdPage"] + "" != "") { grdData.PageIndex = (int)Session["grdPage"]; }
            //grdData.DataBind();
        }

        private void bindGrid()
        {
            DataTable dt = (DataTable)Session["grdData"];
            if (Session["grdSort"] + "" != "") { dt.DefaultView.Sort = Session["grdSort"] + ""; }
            grdData.DataSource = dt.DefaultView;
            if (Session["grdPage"] + "" != "") { grdData.PageIndex = (int)Session["grdPage"]; }
            grdData.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            bindData();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtPO.Text = "";
            txtGR.Text = "";
            ddlItem.SelectedIndex = 0;
            ddlUser.SelectedIndex = 0;
            txtDateFrom.Text = "";
            txtDateTo.Text = "";
            bindData();
        }

        protected void grdData_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CancelGR")
            {
                string grNo = (string)e.CommandArgument;
                // dbo.spCancelStockReceiveHeader
                GridViewRow gvr = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                HiddenField hdfReceiveHeaderID = (HiddenField)gvr.FindControl("hdfReceiveHeaderID");
                HiddenField hdfPO_No = (HiddenField)gvr.FindControl("hdfPO_No");
                HiddenField hdfGR_No = (HiddenField)gvr.FindControl("hdfGR_No");
                HiddenField hdfGR_Year = (HiddenField)gvr.FindControl("hdfGR_Year");
                AutorizeData auth = (AutorizeData)Session["userInfo_" + Session.SessionID];
                Entities.StockReceiveInfo rec = new Entities.StockReceiveInfo();
                Int64 ReceiveHeaderId = 0;
                Int64.TryParse(hdfReceiveHeaderID.Value,out ReceiveHeaderId);
                rec.ReceiveHeaderId = ReceiveHeaderId;
                rec.PO_No = hdfPO_No.Value;
                rec.GR_No = hdfGR_No.Value;
                rec.GR_Year = hdfGR_Year.Value;
                rec.UpdateBy = auth.EmployeeID;
                string msgErr = "";
                Class.DASAPConnector sap = new Class.DASAPConnector();
                if (sap.cancelGoodsReceipt(rec, ref msgErr))
                {
                    // alert : Completed
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ยกเลิกรายการรับสินค้าเสร็จสิ้น'); clickBindData();", true);
                    return;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('" + msgErr + " !!');", true);
                    return;
                }
            }
            else if (e.CommandName == "clickPO")
            {
                string PO_No = e.CommandArgument + "";
                Response.Redirect("StockReceiveEdit.aspx?PO_No=" + PO_No);
            }
            else if (e.CommandName == "clickGR")
            {
                string GR_No = e.CommandArgument + "";
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "OpenColorBox('GoodsReceiveDetail.aspx?GR_No=" + e.CommandArgument + "','75%','70%');", true);
                return;
            }
            else if (e.CommandName == "PrtPO")
            {
                string PO_No = e.CommandArgument + "";
                DAReport rptConfig = new DAReport();

                string msgErr = "";
                DASAPConnector sap = new DASAPConnector();
                if (sap.createSAPReportPO(PO_No, ref msgErr))
                {
                    string pdfPath = rptConfig.getSAPExProt_APD();
                    string pdfName = PO_No + ".pdf";
                    string sourcePath = pdfPath + "\\" + pdfName;
                    string desPath = Server.MapPath("../Report/" + pdfName);
                    using (new Impersonation(rptConfig.getSAPExProt_Domain(), rptConfig.getSAPExProt_User(), rptConfig.getSAPExProt_Password()))
                    {
                        if (File.Exists(sourcePath))
                        {
                            File.Copy(sourcePath, desPath, true);
                        }
                    }
                    if (File.Exists(desPath))
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "js", "Popup80('" + "../Report/" + pdfName + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ไม่พบไฟล์ Report !');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('" + msgErr + "');", true);
                }
            }
            else if (e.CommandName == "PrtGR")
            {
                GridViewRow gvr = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                Label lbGR_No = (Label)gvr.FindControl("grdLbGrNo");
                Label lbGR_Year = (Label)gvr.FindControl("grdLbGrYear");
                string GR_No = lbGR_No.Text;
                string GRYear = lbGR_Year.Text;

                DAReport rptConfig = new DAReport();

                string msgErr = "";
                DASAPConnector sap = new DASAPConnector();
                if (sap.createSAPReportGR(GR_No,GRYear, ref msgErr))
                {
                    string pdfPath = rptConfig.getSAPExProt_APD();
                    string pdfName = GR_No + "" + GRYear + ".pdf";
                    string sourcePath = pdfPath + "\\" + pdfName;
                    string desPath = Server.MapPath("../Report/" + pdfName);
                    using (new Impersonation(rptConfig.getSAPExProt_Domain(), rptConfig.getSAPExProt_User(), rptConfig.getSAPExProt_Password()))
                    {
                        if (File.Exists(sourcePath))
                        {
                            File.Copy(sourcePath, desPath, true);
                        }
                    }
                    if (File.Exists(desPath))
                    {
                         ScriptManager.RegisterStartupScript(this, GetType(), "js", "Popup80('" + "../Report/" + pdfName + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ไม่พบไฟล์ Report !');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('" + msgErr + "');", true);
                }
            }
        }

        protected void grdData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow){
                ImageButton imb = (ImageButton)e.Row.FindControl("imgCancelGR");
                HiddenField hdfCancelAble = (HiddenField)e.Row.FindControl("hdfCancelAble");

                if (hdfCancelAble.Value == "Y")
                {
                    imb.Attributes.Add("OnClick", "return confirmButton('คุณต้องการยกเลิก GR: " + ((HiddenField)e.Row.FindControl("hdfGR_No")).Value + " ใช่หรือไม่?');");
                }
                else
                {
                    imb.Attributes.Add("style", "display:none;");
                }
                /*
                    System.Int64	System.String	System.String	System.String	System.String	System.DateTime	System.String	System.String	System.Int64	System.Decimal	System.Decimal	System.String	System.Int32	System.String	System.String	System.String	System.String	System.String	System.String	
                    ReceiveHeaderID	PO_No	GR_No	GR_Year	Vendor	CreateDate	CreateBy	ReceiveHeaderStatus	ReceiveDetailId	PricePerUnit	ReceiveAmount	Status	MasterItemId	ItemNo	ItemName	ItemUnit	ItemStatus	StatusValue	StatusText	
                    1	4016001264	5000000022	2014		4/9/2015 3:52:57	AP00XXXX	1	1	10747.66	5	3	106	000000000010000109	ทองคำแท่ง 5 บาท	ชิ้น	1	3	เสร็จสิ้น	
                    2	4016001474	5000000023	2014		4/9/2015 3:58:56	AP00XXXX	1	2	12000.00	4	2	109	000000000010000123	โทรศัพท์มือถือ iPhone 5s	ชิ้น	1	2	ระหว่างดำเนินการ	
                    3	4016001680	5000000024	2014		4/9/2015 4:03:58	AP00XXXX	1	3	100.00	100	1	112	000000000010000443	Gift Voucher Index Furniture 5,000	ชิ้น	1	1	รายการใหม่	
                 */
            }            
        }

        private void keepConditionSearch()
        {
            Entities.ConditionSearchInfo sCond = new Entities.ConditionSearchInfo();
            sCond.PO_No = txtPO.Text;
            sCond.GR_No = txtGR.Text;
            sCond.MatGrpID = ddlItemGroup.SelectedItem.Value;
            sCond.MatID = ddlItem.SelectedItem.Value;
            sCond.DateBeg = txtDateFrom.Text;
            sCond.DateEnd = txtDateTo.Text;
            Session["sCond"] = sCond;
        }

        protected void btnAddReceive_Click(object sender, EventArgs e)
        {
            Response.Redirect("StockReceiveEdit.aspx");
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
            Session["grdPage"] = e.NewPageIndex;
            bindGrid();
        }

        //protected void imgFindUser_Click(object sender, ImageClickEventArgs e)
        //{
        //    DAStockPromotion cls = new DAStockPromotion();
        //    DataTable dt = cls.getDataUserByEmpCode(ddlUser.SelectedItem.Value);
        //    // DataTable dt = cls.getDataUserByEmpCode(txtReceiveBy.Text.Trim());
        //    if (dt.Rows.Count == 1)
        //    {
        //        DataRow dr = dt.Rows[0];
        //        hdfUserId.Value = dr["UserID"] + "";
        //        hdfEmpCode.Value = dr["EmpCode"] + "";
        //        hdfUserName.Value = dr["FirstName"] + " " + dr["LastName"];
        //        txtReceiveBy.Text = dr["FirstName"] + " " + dr["LastName"];
        //    }
        //    else
        //    {
        //        string[] name = txtReceiveBy.Text.Split(' ');
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
        //            hdfUserName.Value = dr["FirstName"] + " " + dr["LastName"];
        //            txtReceiveBy.Text = dr["FirstName"] + " " + dr["LastName"];
        //        }
        //        else
        //        {
        //            ScriptManager.RegisterStartupScript(this, GetType(), "js", "OpenColorBox('GetUser.aspx?btnFindUserId=" + imgFindUser.ClientID + "&txtFindUserId=" + txtReceiveBy.ClientID + "&sUser=" + txtReceiveBy.Text.Replace(" ", "_") + "','80%','80%');", true);
        //            return;
        //        }
        //    }
        //}


        
    }
}