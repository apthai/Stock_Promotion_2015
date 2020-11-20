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
    public partial class StockItemDestroyEdit : Page
    {
        Entities.FormatDate convertDate = new Entities.FormatDate();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                initPage();
            }
            if (hdfSelProject.Value != "")
            {
                ddlProject.SelectedIndex = ddlProject.Items.IndexOf(ddlProject.Items.FindByValue(hdfSelProject.Value));
                ddlItem.SelectedIndex = ddlItem.Items.IndexOf(ddlItem.Items.FindByValue(hdfSelItem.Value));
            }
        }

        private void initPage()
        {
            txtPostingDate.Text = DateTime.Now.ToString(convertDate.formatDate);
            bindDDLProject();
            bindDDLItemDestroy();
            btnSaveReason.Attributes.Add("style", "display:none;");
        }

        private void bindDDLProject()
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            DataTable dt = dasp.getDataMasterProject();
            ddlProject.DataSource = dt;
            ddlProject.DataTextField = "ProjectName";
            ddlProject.DataValueField = "ProjectCode";
            ddlProject.DataBind();
            ddlProject.Items.Insert(0, new ListItem("สต๊อกกลาง", "0"));
        }

        private void bindDDLItemDestroy()
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            Entities.MasterItemInfo item = new Entities.MasterItemInfo();
            item.ItemCostEnd = 9999999;
            DataTable dt = dasp.getDataMasterItem(item);
            ddlItem.DataSource = dt;
            ddlItem.DataTextField = "ItemNoName";
            ddlItem.DataValueField = "MasterItemId";
            ddlItem.DataBind();
            ddlItem.Items.Insert(0, new ListItem("ทั้งหมด", "0"));
        }

        protected void btnSelect_Click(object sender, EventArgs e)
        {
            if (btnSelect.Text == "ตกลง")
            {
                Session["grdSort"] = ""; Session["grdPage"] = 0;
                int project = 0;
                int.TryParse(ddlProject.SelectedItem.Value, out project);
                int item = 0;
                int.TryParse(ddlItem.SelectedItem.Value, out item);

                Class.DAStockItemDestroy cls = new Class.DAStockItemDestroy();
                DataTable dt = null;

                if (ddlProject.SelectedItem.Value == "0")
                {
                    dt = cls.getDataStockItemFormCenterStockDestroy(item);
                }
                else
                {

                    dt = cls.getDataStockItemFormProjectDestroy(project, item);
                }

                Session["dtDestroy"] = dt;

                if (dt.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ไม่พบข้อมูลสินค้าโปรโมชั่น !!');", true);
                    return;
                }
                DataTable PCRTHO = dt.DefaultView.ToTable(true, "CompanySAPCode", "ProfitCenterHO");
                if (PCRTHO.Rows.Count != 1)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('พบข้อผิดพลาดในระบบ[ProfitCenterHO] กรุณาติดต่อผู้ดูแลระบบ  !!');", true);
                    return;
                }
                else
                {
                    hdfCompanySAPCode.Value = PCRTHO.Rows[0]["CompanySAPCode"] + "";
                    hdfProfitHO.Value = PCRTHO.Rows[0]["ProfitCenterHO"] + "";
                    PCRTHO.Dispose();
                    PCRTHO = null;
                }

                DataTable dtDestroyList = dt.DefaultView.ToTable(true, "Serial", "Barcode", "ItemName", "Model", "Color", "DimensionWidth", "DimensionLong", "DimensionHeight", "DimensionUnit", "Weight", "WeightUnit", "Price", "ProduceDate", "ExpireDate", "Detail", "Remark", "UserResponse", "FullName", "TrListId");
                dtDestroyList.Columns.Add(new DataColumn("ItemAmount", typeof(int)));
                foreach (DataRow dr in dtDestroyList.Rows)
                {
                    string cond = getConditionGroupItem(dr);
                    dr["ItemAmount"] = dt.Select(cond).Length;
                }

                DataColumn dc = new DataColumn("SelAmt", typeof(int));
                dc.DefaultValue = 0;
                dtDestroyList.Columns.Add(dc);

                dtDestroyList.AcceptChanges();
                Session["dtDestroyList"] = dtDestroyList;
                grdData.DataSource = dtDestroyList;
                grdData.DataBind();
                if (dt.Rows.Count > 0)
                {
                    hdfSelProject.Value = ddlProject.SelectedItem.Value;
                    hdfSelItem.Value = ddlItem.SelectedItem.Value;
                    btnSelect.Text = "ยกเลิก";
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "$('.js-example-basic-single').prop('disabled', true);", true);
                    btnSaveReason.Attributes.Add("style", "");
                }
            }
            else
            {
                btnSelect.Text = "ตกลง";
                hdfSelProject.Value = "";
                hdfSelItem.Value = "";
                hdfCompanySAPCode.Value = "";
                hdfProfitHO.Value = "";
                DataTable dt = (DataTable)Session["dtDestroy"];
                dt = dt.Clone();
                grdData.DataSource = dt;
                grdData.DataBind();
                btnSaveReason.Attributes.Add("style", "display:none;");
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "$('.js-example-basic-single').prop('disabled', false);", true);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            keepSelAmt();

            AutorizeData auth = (AutorizeData)Session["userInfo_" + Session.SessionID];
            string destroyItemList = "";
            DataTable dtDestroy = (DataTable)Session["dtDestroy"];

            DataTable dtDestroyList = (DataTable)Session["dtDestroyList"];
            foreach (DataRow dr in dtDestroyList.Rows)
            {
                DataRow[] drDestroy = dtDestroy.Select(getConditionGroupItem(dr));
                int amt = (int)dr["SelAmt"];
                for (int i = 0; i < amt; i++)
                {
                    destroyItemList += "," + drDestroy[i]["ItemId"];
                }
            }

            if (destroyItemList != "")
            {
                Class.DASAPConnector sap = new Class.DASAPConnector();

                /* - = DOCUMENTHEADER = - */
                Entities.SapDestroy_DOCUMENTHEADER DOCUMENTHEADER = new Entities.SapDestroy_DOCUMENTHEADER();
                DOCUMENTHEADER.USERNAME = auth.EmployeeID;
                DOCUMENTHEADER.COMP_CODE = hdfCompanySAPCode.Value;
                DOCUMENTHEADER.DOC_DATE = txtPostingDate.Text;
                DOCUMENTHEADER.PSTNG_DATE = txtPostingDate.Text;
                DateTime sDate = new DateTime();
                convertDate.getDateFromString(txtPostingDate.Text, ref sDate);
                DOCUMENTHEADER.FISC_YEAR = sDate.Year;
                DOCUMENTHEADER.DOC_TYPE = "IB";

                destroyItemList = destroyItemList.Remove(0, 1);

                dtDestroy.DefaultView.RowFilter = "ItemId in (" + destroyItemList + ")";
                dtDestroy = dtDestroy.DefaultView.ToTable();
                DataTable dtGrp = dtDestroy.DefaultView.ToTable(true, "ItemNo", "ItemName", "isNonePR", "PO_No", "PO_Item");

                List<Entities.SapDestroy_ACCOUNTGL> lstACCOUNTGL = new List<Entities.SapDestroy_ACCOUNTGL>();
                Entities.SapDestroy_ACCOUNTGL ACCOUNTGL = null;
                List<Entities.SapDestroy_CURRENCYAMOUNT> lstCAMOUNT = new List<Entities.SapDestroy_CURRENCYAMOUNT>();
                Entities.SapDestroy_CURRENCYAMOUNT CAMOUNT = null;

                int ii = 0;

                foreach (DataRow drGrp in dtGrp.Rows)
                {
                    ii++;
                    string cond = "ItemNo = '" + drGrp["ItemNo"] + "' AND ItemName = '" + drGrp["ItemName"] + "' AND isNonePR = '" + drGrp["isNonePR"] + "' AND PO_No = '" + drGrp["PO_No"] + "' AND PO_Item = '" + drGrp["PO_Item"] + "'";
                    int cntDmg = (int)dtDestroy.Compute("COUNT(Price)", cond);
                    decimal sumDmg = (decimal)dtDestroy.Compute("SUM(Price)", cond);
                    string preDesc = "D-";
                    if (drGrp["isNonePR"] + "" == "Y") { preDesc = "DO-"; }

                    ACCOUNTGL = new Entities.SapDestroy_ACCOUNTGL();
                    ACCOUNTGL.ITEMNO_ACC = ii;
                    ACCOUNTGL.GL_ACCOUNT = sap.getGLNoDamage();
                    ACCOUNTGL.ITEM_TEXT = preDesc + drGrp["ItemName"] + " " + cntDmg;
                    ACCOUNTGL.PROFIT_CTR = hdfProfitHO.Value;

                    lstACCOUNTGL.Add(ACCOUNTGL);

                    CAMOUNT = new Entities.SapDestroy_CURRENCYAMOUNT();
                    CAMOUNT.ITEMNO_ACC = ii;
                    CAMOUNT.CURRENCY = "THB";
                    CAMOUNT.AMT_DOCCUR = sumDmg;
                    lstCAMOUNT.Add(CAMOUNT);
                }

                foreach (DataRow drGrp in dtGrp.Rows)
                {
                    ii++;
                    string cond = "ItemNo = '" + drGrp["ItemNo"] + "' AND ItemName = '" + drGrp["ItemName"] + "' AND isNonePR = '" + drGrp["isNonePR"] + "' AND PO_No = '" + drGrp["PO_No"] + "' AND PO_Item = '" + drGrp["PO_Item"] + "'";
                    int cntDmg = (int)dtDestroy.Compute("COUNT(Price)", cond);
                    decimal sumDmg = (decimal)dtDestroy.Compute("SUM(Price)", cond);
                    string preDesc = "D-"; if (drGrp["isNonePR"] + "" == "Y") { preDesc = "DO-"; }

                    ACCOUNTGL = new Entities.SapDestroy_ACCOUNTGL();
                    ACCOUNTGL.ITEMNO_ACC = ii;
                    ACCOUNTGL.GL_ACCOUNT = sap.getGLNoCredit();
                    ACCOUNTGL.ITEM_TEXT = preDesc + drGrp["ItemName"] + " " + cntDmg;
                    ACCOUNTGL.PROFIT_CTR = hdfProfitHO.Value;

                    string PO_Item = "00000" + drGrp["PO_Item"];
                    ACCOUNTGL.REF_KEY_3 = drGrp["PO_No"] + "" + PO_Item.Substring(PO_Item.Length - 5, 5);

                    lstACCOUNTGL.Add(ACCOUNTGL);

                    CAMOUNT = new Entities.SapDestroy_CURRENCYAMOUNT();
                    CAMOUNT.ITEMNO_ACC = ii;
                    CAMOUNT.CURRENCY = "THB";
                    CAMOUNT.AMT_DOCCUR = (-1) * sumDmg;
                    lstCAMOUNT.Add(CAMOUNT);
                }

                string msgErr = "";
                string sapDocNo = "";

                if (sap.SAPDamage(DOCUMENTHEADER, lstACCOUNTGL, lstCAMOUNT, destroyItemList, hdfReason.Value, auth.EmployeeID, ref sapDocNo, ref msgErr))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('การดำเนินการเสร็จสิ้น SAP DocNo. " + sapDocNo + "'); window.location.replace('StockItemDestroy.aspx');", true);
                    return;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('การดำเนินการผิดพลาด กรุณาติดต่อผู้ดูแลระบบ !!\\n" + msgErr + "');", true);
                    return;
                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ไม่มีรายการตัดสต๊อกสูญเสีย !');", true);
                return;
            }
        }

        protected void grdData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField grdHdfItemAmount = (HiddenField)e.Row.FindControl("grdHdfItemAmount");
                TextBox grdTxtDestroyAmount = (TextBox)e.Row.FindControl("grdTxtDestroyAmount");
                grdTxtDestroyAmount.Attributes.Add("onkeyup", "return checkDestroyLimit('" + grdHdfItemAmount.ClientID + "','" + grdTxtDestroyAmount.ClientID + "');");
            }
        }

        private string getConditionGroupItem(DataRow dr)
        {
            string cond = " isnull(Serial,'') = '" + dr["Serial"] + "'";
            cond += " and isnull(Barcode,'') = '" + dr["Barcode"] + "'";
            cond += " and isnull(ItemName,'') = '" + dr["ItemName"] + "'";
            cond += " and isnull(Model,'') = '" + dr["Model"] + "'";
            cond += " and isnull(Color,'') = '" + dr["Color"] + "'";
            cond += " and isnull(DimensionWidth,'') = '" + dr["DimensionWidth"] + "'";
            cond += " and isnull(DimensionLong,'') = '" + dr["DimensionLong"] + "'";
            cond += " and isnull(DimensionHeight,'') = '" + dr["DimensionHeight"] + "'";
            cond += " and isnull(DimensionUnit,'') = '" + dr["DimensionUnit"] + "'";
            cond += " and isnull(Weight,'') = '" + dr["Weight"] + "'";
            cond += " and isnull(WeightUnit,'') = '" + dr["WeightUnit"] + "'";
            cond += " and isnull(TrListId,'') = '" + dr["TrListId"] + "'";
            if (dr["Price"] == DBNull.Value)
            {
                cond += " and Price is null ";
            }
            else
            {
                cond += " and Price = '" + dr["Price"] + "'";
            }

            if (dr["ProduceDate"] == DBNull.Value)
            {
                cond += " and ProduceDate is null ";
            }
            else
            {
                cond += " and ProduceDate = '" + dr["ProduceDate"] + "'";
            }
            if (dr["ExpireDate"] == DBNull.Value)
            {
                cond += " and ExpireDate is null ";
            }
            else
            {
                cond += " and ExpireDate = '" + dr["ExpireDate"] + "'";
            }
            cond += " and isnull(Detail,'') = '" + dr["Detail"] + "'";
            cond += " and isnull(Remark,'') = '" + dr["Remark"] + "'";

            return cond;
        }

        private string getConditionGroupItem(GridViewRow gr)
        {
            string cond = " isnull(Serial,'') = '" + ((HiddenField)gr.FindControl("grdHdfSerial")).Value + "'";
            cond += " and isnull(Barcode,'') = '" + ((HiddenField)gr.FindControl("grdHdfBarcode")).Value + "'";
            cond += " and isnull(ItemName,'') = '" + ((HiddenField)gr.FindControl("grdHdfItemName")).Value + "'";
            cond += " and isnull(Model,'') = '" + ((HiddenField)gr.FindControl("grdHdfModel")).Value + "'";
            cond += " and isnull(Color,'') = '" + ((HiddenField)gr.FindControl("grdHdfColor")).Value + "'";
            cond += " and isnull(DimensionWidth,'') = '" + ((HiddenField)gr.FindControl("grdHdfDimensionWidth")).Value + "'";
            cond += " and isnull(DimensionLong,'') = '" + ((HiddenField)gr.FindControl("grdHdfDimensionLong")).Value + "'";
            cond += " and isnull(DimensionHeight,'') = '" + ((HiddenField)gr.FindControl("grdHdfDimensionHeight")).Value + "'";
            cond += " and isnull(DimensionUnit,'') = '" + ((HiddenField)gr.FindControl("grdHdfDimensionUnit")).Value + "'";
            cond += " and isnull(Weight,'') = '" + ((HiddenField)gr.FindControl("grdHdfWeight")).Value + "'";
            cond += " and isnull(WeightUnit,'') = '" + ((HiddenField)gr.FindControl("grdHdfWeightUnit")).Value + "'";
            cond += " and isnull(TrListId,'') = '" + ((HiddenField)gr.FindControl("grdHdfTrListId")).Value + "'";

            if (((HiddenField)gr.FindControl("grdHdfPrice")).Value + "" == "")
            {
                cond += " and Price is null ";
            }
            else
            {
                cond += " and Price = '" + ((HiddenField)gr.FindControl("grdHdfPrice")).Value + "'";
            }

            cond += " and Price = '" + ((HiddenField)gr.FindControl("grdHdfPrice")).Value + "'";

            if (((HiddenField)gr.FindControl("grdHdfProduceDate")).Value == "")
            {
                cond += " and ProduceDate is null ";
            }
            else
            {
                cond += " and ProduceDate = '" + ((HiddenField)gr.FindControl("grdHdfProduceDate")).Value + "'";
            }
            if (((HiddenField)gr.FindControl("grdHdfExpireDate")).Value == "")
            {
                cond += " and ExpireDate is null ";
            }
            else
            {
                cond += " and ExpireDate = '" + ((HiddenField)gr.FindControl("grdHdfExpireDate")).Value + "'";
            }
            cond += " and isnull(Detail,'') = '" + ((HiddenField)gr.FindControl("grdHdfDetail")).Value + "'";
            cond += " and isnull(Remark,'') = '" + ((HiddenField)gr.FindControl("grdHdfRemark")).Value + "'";
            return cond;
        }

        protected void grdData_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dtDestroyList = (DataTable)Session["dtDestroyList"];
            dtDestroyList.DefaultView.RowFilter = "";
            if (Session["grdSort"] + "" == dtDestroyList.DefaultView.Sort + "")
            {
                dtDestroyList.DefaultView.Sort += " Desc";
            }
            else
            {
                dtDestroyList.DefaultView.Sort = Session["grdSort"] + "";
            }
            grdData.DataSource = dtDestroyList.DefaultView;
            grdData.DataBind();
        }

        protected void grdData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            keepSelAmt();
            Session["grdPage"] = e.NewPageIndex;
            DataTable dtDestroyList = (DataTable)Session["dtDestroyList"];
            dtDestroyList.DefaultView.RowFilter = "";
            grdData.DataSource = dtDestroyList.DefaultView;
            grdData.PageIndex = (int)Session["grdPage"];
            grdData.DataBind();
            setSelAmt();
        }

        private void keepSelAmt()
        {
            DataTable dtDestroyList = (DataTable)Session["dtDestroyList"];
            foreach (GridViewRow gr in grdData.Rows)
            {
                if (gr.RowType == DataControlRowType.DataRow)
                {
                    TextBox txtSelAmt = (TextBox)gr.FindControl("grdTxtDestroyAmount");
                    string cond = getConditionGroupItem(gr);
                    DataRow[] dr = dtDestroyList.Select(cond);
                    if (dr.Length == 1)
                    {
                        int selAmt = 0;
                        if (int.TryParse(txtSelAmt.Text, out selAmt))
                        {
                            dr[0]["SelAmt"] = selAmt;
                        }
                    }
                }
            }
            Session["dtDestroyList"] = dtDestroyList;
        }


        private void setSelAmt()
        {
            DataTable dtDestroyList = (DataTable)Session["dtDestroyList"];
            foreach (GridViewRow gr in grdData.Rows)
            {
                if (gr.RowType == DataControlRowType.DataRow)
                {
                    string cond = getConditionGroupItem(gr);
                    DataRow[] dr = dtDestroyList.Select(cond);
                    if (dr.Length == 1)
                    {
                        TextBox txtSelAmt = (TextBox)gr.FindControl("grdTxtDestroyAmount");
                        txtSelAmt.Text = dr[0]["SelAmt"] + "";
                    }
                }
            }
        }

    }
}