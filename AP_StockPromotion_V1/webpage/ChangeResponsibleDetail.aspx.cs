using AP_StockPromotion_V1.ws_authorize;
using System;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace AP_StockPromotion_V1.webpage
{
    public partial class ChangeResponsibleDetail : System.Web.UI.Page
    {

        // protected HtmlInputFile myFile;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                initPage();
                Session["fileMemo"] = null;
                string mode = Request.QueryString["mode"] + "";
                if (mode == "Edit")
                {
                    string CRListId = Request.QueryString["CRListId"] + "";
                    Class.DAChangeResponsible cls = new Class.DAChangeResponsible();
                    DataTable dt = cls.getChangeResponsibleDetailHistory(CRListId);
                    if (dt == null) { execJScript("alert('ไม่พบข้อมูล!!'); window.location = 'ChangeResponsibleList.aspx';"); return; }
                    if (dt.Rows.Count == 0) { execJScript("alert('ไม่พบข้อมูล!!'); window.location = 'ChangeResponsibleList.aspx';"); return; }
                    DataRow dr = dt.Rows[0];
                    ddlProject.SelectedIndex = ddlProject.Items.IndexOf(ddlProject.Items.FindByValue(dr["Project_Id"] + ""));

                    bindDDLOldResp(dt);

                    ddlCurResp.SelectedIndex = ddlCurResp.Items.IndexOf(ddlCurResp.Items.FindByValue(dr["CRFrom"] + ""));
                    ddlNewResp.SelectedIndex = ddlNewResp.Items.IndexOf(ddlNewResp.Items.FindByValue(dr["CRTo"] + ""));
                    
                    txtMemoNo.Text = dr["DocRefNo"] + "";
                    txtMemoNo.ReadOnly = true;
                    //txtMemoNo.Text = 
                    txtMemoDate.Text = ((DateTime)dr["DocDate"]).ToString(new Entities.FormatDate().formatDate);
                    txtMemoDate.ReadOnly = true;
                    lbUploaded.Text = dr["FileAttch"] + "";
                    hdfUploaded.Value = dr["FileAttchName"] + "";
                    fileUploadMemo.Attributes.Add("style", "display:none;");
                    divUploaded.Attributes.Add("style", "");
                    btnSave.Attributes.Add("style", "display:none;");
                    btnShowItemList.Attributes.Add("style", "display:none;");
                    imgDelFile.Attributes.Add("style", "display:none;");
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "$('.js-example-basic-single').prop('disabled', true);", true);
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "$('.js-example-basic-single-ws').prop('disabled', true);", true);

                    bindDataChanged(CRListId);
                    foreach (GridViewRow gr in grdData.Rows)
                    {
                        if (gr.RowType == DataControlRowType.DataRow)
                        {
                            ((TextBox)gr.FindControl("grdTxtAmount")).ReadOnly = true;
                        }
                    }
                }
            }
        }

        private void bindDataChanged(string CRListId)
        {
            Class.DAChangeResponsible cls = new Class.DAChangeResponsible();
            int crListId = 0;
            int.TryParse(CRListId, out crListId);
            DataTable dtItemsResponsibility = cls.getChangeResponsibleDetailItem(CRListId);
            DataTable dt = dtItemsResponsibility.DefaultView.ToTable(true, "Serial", "Barcode", "ItemName", "Model", "Color", "Dimension", "Weight", "Price", "ProduceDate", "ExpireDate", "Detail", "Remark");
            Session["ItemsResponsibility"] = dtItemsResponsibility;
            dt.Columns.Add(new DataColumn("Amount", typeof(int)));
            foreach (DataRow dr in dt.Rows)
            {
                dr["Amount"] = dtItemsResponsibility.Compute("COUNT(ItemId)", getStrFilter(dr));
            }
            dt.DefaultView.RowFilter = "";
            dt.AcceptChanges();
            grdData.DataSource = dt;
            grdData.DataBind();
        }

        private void initPage()
        {
            bindDDLProject();
            bindDDLCurrResp();
            bindDDLNewResp();
            txtMemoDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            ddlCurResp.Attributes.Add("onchange", "setValueHdf('" + ddlCurResp.ClientID + "','" + hdfCurResp.ClientID + "');");
            ddlNewResp.Attributes.Add("onchange", "setValueHdf('" + ddlNewResp.ClientID + "','" + hdfNewResp.ClientID + "');");
        }

        private void bindDDLProject()
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            DataTable dt = dasp.getDataMasterProject();
            ddlProject.DataSource = dt;
            ddlProject.DataTextField = "ProjectName";
            ddlProject.DataValueField = "ProjectID";
            ddlProject.DataBind();
            ddlProject.Items.Insert(0, new ListItem("-- เลือก --", ""));
        }

        private void bindDDLOldResp(DataTable dt)
        {
            ddlCurResp.Items.Clear();
            dt.Columns.Add(new DataColumn("FullName", typeof(string)));
            foreach (DataRow dr in dt.Rows)
            {
                dr["FullName"] = dr["CRFrom"] + " " + dr["UserFrom"];
            }
            dt.AcceptChanges();
            ddlCurResp.DataSource = dt.DefaultView.ToTable(true, "CRFrom", "FullName");
            ddlCurResp.DataTextField = "FullName";
            ddlCurResp.DataValueField = "CRFrom";
            ddlCurResp.DataBind();
        }

        private void bindDDLCurrResp()
        {
            int project_Id = 0;
            int.TryParse(ddlProject.SelectedItem.Value, out project_Id);
            Class.DAChangeResponsible dasp = new Class.DAChangeResponsible();
            DataTable dt = dasp.getResponsibleByProject(project_Id);
            ddlCurResp.DataSource = dt;
            ddlCurResp.DataTextField = "FullNoName";
            ddlCurResp.DataValueField = "UserResponse";
            ddlCurResp.DataBind();
            ddlCurResp.Items.Insert(0, new ListItem("-- เลือก --", ""));
        }

        private void bindDDLNewResp()
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            DataTable dt = dasp.getDataUser("","");
            ddlNewResp.DataSource = dt;
            ddlNewResp.DataTextField = "FullNoName";
            ddlNewResp.DataValueField = "EmpCode";
            ddlNewResp.DataBind();
            ddlNewResp.Items.Insert(0, new ListItem("-- เลือก --", ""));
        }

        protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            bindDDLCurrResp();
        }

        protected void btnShowItemList_Click(object sender, EventArgs e)
        {
            if (btnShowItemList.Text == "แสดงรายการสินค้า")
            {
                if (chkHeaderValue())
                {
                    bindData();
                    txtMemoNo.ReadOnly = true;
                    txtMemoDate.ReadOnly = true;
                    //fileMemo.Enabled = false;
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "$('.js-example-basic-single').prop('disabled', true);", true);
                    btnShowItemList.Text = "ยกเลิก/เปลี่ยน";
                    btnSave.Attributes.Add("style", "");
                }
            }
            else
            {
                Session["ItemsResponsibility"] = null;
                grdData.DataSource = null;
                grdData.DataBind();
                txtMemoNo.ReadOnly = false;
                txtMemoDate.ReadOnly = false;
                // fileMemo.Enabled = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "$('.js-example-basic-single').prop('disabled', false);", true);
                btnShowItemList.Text = "แสดงรายการสินค้า";
                btnSave.Attributes.Add("style", "display:none;");
            }
        }

        private bool chkHeaderValue()
        {
            bool rst = true;
            if (ddlProject.SelectedItem.Value == "")
            {
                execJScript("alert('กรุณาระบุโครงการที่ต้องการ !!'); $('#" + ddlProject.ClientID + "').focus();");
                return false;
            }
            if (ddlCurResp.SelectedItem.Value == "")
            {
                execJScript("alert('กรุณาระบุผู้รับผิดชอบที่ต้องการถ่ายโอนสิทธิ์ !!'); $('#" + ddlCurResp.ClientID + "').focus();");
                return false;
            }
            if (ddlNewResp.SelectedItem.Value == "")
            {
                execJScript("alert('กรุณาระบุผู้รับผิดชอบเพื่อรับโอนสิทธิ์ !!'); $('#" + ddlNewResp.ClientID + "').focus();");
                return false;
            }
            if (txtMemoNo.Text.Trim() == "")
            {
                execJScript("alert('กรุณาระบุเลขที่เอกสารอ้างอิง !!'); $('#" + txtMemoNo.ClientID + "').focus();");
                return false;
            }
            if (txtMemoDate.Text.Trim() == "")
            {
                execJScript("alert('กรุณาระบุวันที่รับแจ้ง !!'); $('#" + txtMemoDate.ClientID + "').focus();");
                return false;
            }
            if (ddlCurResp.SelectedItem.Value == ddlNewResp.SelectedItem.Value)
            {
                execJScript("alert('ผู้รับผิดชอบคนใหม่ที่ท่านเลือก ตรงกับผู้รับผิดชอบปัจจุบัน'); $('#" + ddlNewResp.ClientID + "').focus();");
                return false;
            }
            return rst;
        }

        private void bindData()
        {
            int project_id = 0;
            int.TryParse(ddlProject.SelectedItem.Value, out project_id);
            Class.DAChangeResponsible cls = new Class.DAChangeResponsible();
            DataTable dtItemsResponsibility = cls.getItemsResponsibilityByProject(ddlCurResp.SelectedItem.Value, project_id);            
            DataTable dt = dtItemsResponsibility.DefaultView.ToTable(true, "Serial", "Barcode", "ItemName", "Model", "Color", "Dimension", "Weight", "Price", "ProduceDate", "ExpireDate", "Detail", "Remark");
            Session["ItemsResponsibility"] = dtItemsResponsibility;
            dt.Columns.Add(new DataColumn("Amount", typeof(int)));
            foreach (DataRow dr in dt.Rows)
            {
                dr["Amount"] = dtItemsResponsibility.Compute("COUNT(ItemId)", getStrFilter(dr));
            }
            dt.DefaultView.RowFilter = "";
            dt.AcceptChanges();
            grdData.DataSource = dt;
            grdData.DataBind();
        }


        private string getStrFilter(DataRow dr)
        {
            string rst = "";
            rst += " isnull(Serial,'') = '" + dr["Serial"] + "' ";
            rst += " and isnull(Barcode,'') = '" + dr["Barcode"] + "' ";
            rst += " and isnull(ItemName,'') = '" + dr["ItemName"] + "' ";
            rst += " and isnull(Model,'') = '" + dr["Model"] + "' ";
            rst += " and isnull(Color,'') = '" + dr["Color"] + "' ";
            rst += " and isnull(Dimension,'') = '" + dr["Dimension"] + "' ";
            rst += " and isnull(Weight,'') = '" + dr["Weight"] + "' ";
            rst += " and isnull(Price,'') = '" + dr["Price"] + "' ";
            if (dr["ProduceDate"] == DBNull.Value)
            {
                rst += " and ProduceDate is null ";
            }
            else
            {
                rst += " and ProduceDate = '" + dr["ProduceDate"] + "'";
            }
            if (dr["ExpireDate"] == DBNull.Value)
            {
                rst += " and ExpireDate is null ";
            }
            else
            {
                rst += " and ExpireDate = '" + dr["ExpireDate"] + "'";
            }
            rst += " and isnull(Detail,'') = '" + dr["Detail"] + "' ";
            rst += " and isnull(Remark,'') = '" + dr["Remark"] + "' ";
            return rst;
        }

        private void execJScript(string jscript)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "js", jscript, true);
            return;
        }

        protected void grdData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //  hdfAmount || grdTxtAmount
                HiddenField hdfAmount = (HiddenField)e.Row.FindControl("hdfAmount");
                TextBox txtAmount = (TextBox)e.Row.FindControl("grdTxtAmount");
                txtAmount.Attributes.Add("onkeyup", "chkAmountChange('" + txtAmount.ClientID + "','" + hdfAmount.ClientID + "')");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Session["ItemsResponsibility"];

            string itemList = "";
            foreach (GridViewRow gr in grdData.Rows)
            {
                TextBox txtAmt = (TextBox)gr.FindControl("grdTxtAmount");
                int amt = 0;
                int.TryParse(txtAmt.Text, out amt);
                
                DataRow[] dr = dt.Select(getStrFilter(gr));
                for (int ii = 0; ii < amt; ii++)
                {
                    itemList += "," + dr[ii]["ItemId"] + ""; 
                }
            }
            if (itemList.Length > 0)
            {
                itemList = itemList.Remove(0, 1);
                Class.DAChangeResponsible cls = new Class.DAChangeResponsible();
                int project_Id  = 0;
                int.TryParse(ddlProject.SelectedItem.Value,out project_Id);
                DateTime docDate = new DateTime();
                new Entities.FormatDate().getDateFromString(txtMemoDate.Text ,ref docDate);
                
                AutorizeData auth = (AutorizeData)Session["userInfo_" + Session.SessionID];
                if (cls.changeItemResponsibility(itemList, project_Id, hdfCurResp.Value, hdfNewResp.Value, txtMemoNo.Text, docDate, lbUploaded.Text,hdfUploaded.Value, auth.EmployeeID))
                {
                    execJScript("alert('ดำเนินการเสร็จสิ้น'); closePage();");
                    //execJScript("alert('ดำเนินการเสร็จสิ้น'); window.location = 'ChangeResponsibleList.aspx?bindData=Y';");
                }
                else
                {
                    execJScript("alert('ไม่สามารถดำเนินการได้ !!');");
                }
            }
        }

        private string getStrFilter(GridViewRow gr)
        {
            string rst = "";
            rst += " isnull(Serial,'') = '" + ((HiddenField)gr.FindControl("hdfSerial")).Value + "' ";
            // rst += " and isnull(Barcode,'') = '" + ((HiddenField)gr.FindControl("hdfBarcode")).Value + "' ";
            rst += " and isnull(ItemName,'') = '" + ((HiddenField)gr.FindControl("hdfItemName")).Value + "' ";
            rst += " and isnull(Model,'') = '" + ((HiddenField)gr.FindControl("hdfModel")).Value + "' ";
            rst += " and isnull(Color,'') = '" + ((HiddenField)gr.FindControl("hdfColor")).Value + "' ";
            rst += " and isnull(Dimension,'') = '" + ((HiddenField)gr.FindControl("hdfDimension")).Value + "' ";
            rst += " and isnull(Weight,'') = '" + ((HiddenField)gr.FindControl("hdfWeight")).Value + "' ";
            rst += " and isnull(Price,'') = '" + ((HiddenField)gr.FindControl("hdfPrice")).Value + "' ";
            //if (((HiddenField)gr.FindControl("hdfProduceDate")).Value == "")
            //{
            //    rst += " and ProduceDate is null ";
            //}
            //else
            //{
            //    rst += " and ProduceDate = '" + ((HiddenField)gr.FindControl("hdfProduceDate")).Value + "'";
            //}
            if (((HiddenField)gr.FindControl("hdfExpireDate")).Value == "")
            {
                rst += " and ExpireDate is null ";
            }
            else
            {
                rst += " and ExpireDate = '" + ((HiddenField)gr.FindControl("hdfExpireDate")).Value + "'";
            }
            rst += " and isnull(Detail,'') = '" + ((HiddenField)gr.FindControl("hdfDetail")).Value + "' ";
            rst += " and isnull(Remark,'') = '" + ((HiddenField)gr.FindControl("hdfRemark")).Value + "' ";
            return rst;
        }


        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (fileUploadMemo.HasFile)
            {
                AutorizeData auth = (AutorizeData)Session["userInfo_" + Session.SessionID];
                string fileDirect = Server.MapPath("../FileUpload/");
                if (!Directory.Exists(fileDirect))
                {
                    Directory.CreateDirectory(fileDirect);
                }
                string fileDispName = fileUploadMemo.FileName;
                string filePhysName = auth.EmployeeID +DateTime.Now.ToString("yyMMddHHmmss")+ "_" + fileDispName;
                this.fileUploadMemo.SaveAs(Server.MapPath("../FileUpload/" + filePhysName));
                lbUploaded.Text = fileDispName;                
                hdfUploaded.Value = filePhysName;
                fileUploadMemo.Attributes.Add("style", "display:none;");
                divUploaded.Attributes.Add("style", "");
            }
        }

        protected void imgDelFile_Click(object sender, ImageClickEventArgs e)
        {
            lbUploaded.Text = "";
            hdfUploaded.Value = "";
            fileUploadMemo.Attributes.Add("style", "");
            divUploaded.Attributes.Add("style", "display:none;");
        }

        protected void lbUploaded_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "js", "Popup80('frmOpenFile.aspx?filePath=" + "../FileUpload/" + hdfUploaded.Value + "');", true);
            return;
        }
    }
}