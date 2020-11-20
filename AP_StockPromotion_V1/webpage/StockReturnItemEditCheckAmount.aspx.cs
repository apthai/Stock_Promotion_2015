using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace AP_StockPromotion_V1.webpage
{
    public partial class StockReturnItemEditCheckAmount : System.Web.UI.Page
    {
        private string[] lstDistinct = { "Barcode", "ItemName", "Model", "Color", "DimensionWidth", "DimensionLong", "DimensionHeight", "DimensionUnit", "Weight", "WeightUnit", "Price", "ProduceDate", "ExpireDate", "Detail", "Remark", "UserResponse", "FullName" };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                initPage();
            }
        }

        private void initPage()
        {
            hdfMasterItemId.Value = Request.QueryString["MasterItemId"] + "";
            bindData();
        }

        private void bindData()
        {
            DataTable dt = (DataTable)Session["dtProjectItem"];
            dt.DefaultView.RowFilter = "MasterItemId=" + hdfMasterItemId.Value;
            lbMasterItemName.Text = dt.DefaultView[0]["MasterItemName"] + "";
            // Return : String
            DataTable dtItemList = dt.DefaultView.ToTable(true, lstDistinct);
            dtItemList.Columns.Add(new DataColumn("Amt", typeof(int)));
            dtItemList.Columns.Add(new DataColumn("RetAmt", typeof(int)));
            foreach (DataRow dr in dtItemList.Rows)
            {
                dr["Amt"] = dt.Compute("COUNT(ItemName)", getConditionGroupItem(dr) + " and MasterItemId = " + hdfMasterItemId.Value + " ");
                dr["RetAmt"] = dt.Compute("COUNT(ItemName)", getConditionGroupItem(dr) + " and MasterItemId = " + hdfMasterItemId.Value + " and Return = 'Y'");
            }
            dtItemList.AcceptChanges();
            grdData.DataSource = dtItemList;
            grdData.DataBind();
        } 

        protected void grdData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Label grdHdfLbDimension = (Label)e.Row.FindControl("grdHdfLbDimension");
                //Label grdHdfLbWeight = (Label)e.Row.FindControl("grdHdfLbWeight");

                //HiddenField grdHdfDimensionWidth = (HiddenField)e.Row.FindControl("grdHdfDimensionWidth");
                //HiddenField grdHdfDimensionLong = (HiddenField)e.Row.FindControl("grdHdfDimensionLong");
                //HiddenField grdHdfDimensionHeight = (HiddenField)e.Row.FindControl("grdHdfDimensionHeight");
                //HiddenField grdHdfDimensionUnit = (HiddenField)e.Row.FindControl("grdHdfDimensionUnit");
                //HiddenField grdHdfWeight = (HiddenField)e.Row.FindControl("grdHdfWeight");
                //HiddenField grdHdfWeightUnit = (HiddenField)e.Row.FindControl("grdHdfWeightUnit");

                //grdHdfLbDimension.Text = grdHdfDimensionWidth.Value + "x" + grdHdfDimensionLong.Value + " x " + grdHdfDimensionHeight.Value + " " + grdHdfDimensionUnit.Value;
                //grdHdfLbWeight.Text = grdHdfWeight.Value + " " + grdHdfWeightUnit.Value;


                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    HiddenField grdHdfAmt = (HiddenField)e.Row.FindControl("grdHdfAmt");
                    TextBox grdTxtSelectAmount = (TextBox)e.Row.FindControl("grdTxtSelectAmount");
                    grdTxtSelectAmount.Attributes.Add("onkeyup", "return checkReturnLimit('" + grdHdfAmt.ClientID + "','" + grdTxtSelectAmount.ClientID + "');");
                }
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Session["dtProjectItem"];
            // hdfMasterItemId.Value
            foreach (GridViewRow gr in grdData.Rows)
            {
                dt.DefaultView.RowFilter = getConditionGroupItem(gr) + " and MasterItemId = " + hdfMasterItemId.Value + " ";
                for (int ii = 0; ii < dt.DefaultView.Count; ii++)
                {
                    dt.DefaultView[ii]["Return"] = "";
                }
                string Amt = ((TextBox)gr.FindControl("grdTxtSelectAmount")).Text;
                int amt = 0;
                int.TryParse(Amt,out amt);
                for (int ii = 0; ii < amt; ii++)
                {
                    dt.DefaultView[ii]["Return"] = "Y";
                }                
            }
            dt.AcceptChanges();
            dt.DefaultView.RowFilter = "";
            ScriptManager.RegisterStartupScript(this, GetType(), "js", "btnSelectAmtClick();", true);
            return;

        }




        private string getConditionGroupItem(DataRow dr)
        {
            string cond = " isnull(Barcode,'') = '" + dr["Barcode"] + "'";
            cond += " and isnull(ItemName,'') = '" + dr["ItemName"] + "'";
            cond += " and isnull(Model,'') = '" + dr["Model"] + "'";
            cond += " and isnull(Color,'') = '" + dr["Color"] + "'";
            cond += " and isnull(DimensionWidth,'') = '" + dr["DimensionWidth"] + "'";
            cond += " and isnull(DimensionLong,'') = '" + dr["DimensionLong"] + "'";
            cond += " and isnull(DimensionHeight,'') = '" + dr["DimensionHeight"] + "'";
            cond += " and isnull(DimensionUnit,'') = '" + dr["DimensionUnit"] + "'";
            cond += " and isnull(Weight,'') = '" + dr["Weight"] + "'";
            cond += " and isnull(WeightUnit,'') = '" + dr["WeightUnit"] + "'";
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
            cond += " and isnull(UserResponse,'') = '" + dr["UserResponse"] + "'";
            return cond;
        }

        private string getConditionGroupItem(GridViewRow gr)
        {
            string cond = " isnull(Barcode,'') = '" + ((HiddenField)gr.FindControl("grdHdfBarcode")).Value + "'";
            cond += " and isnull(ItemName,'') = '" + ((HiddenField)gr.FindControl("grdHdfItemName")).Value + "'";
            cond += " and isnull(Model,'') = '" + ((HiddenField)gr.FindControl("grdHdfModel")).Value + "'";
            cond += " and isnull(Color,'') = '" + ((HiddenField)gr.FindControl("grdHdfColor")).Value + "'";
            cond += " and isnull(DimensionWidth,'') = '" + ((HiddenField)gr.FindControl("grdHdfDimensionWidth")).Value + "'";
            cond += " and isnull(DimensionLong,'') = '" + ((HiddenField)gr.FindControl("grdHdfDimensionLong")).Value + "'";
            cond += " and isnull(DimensionHeight,'') = '" + ((HiddenField)gr.FindControl("grdHdfDimensionHeight")).Value + "'";
            cond += " and isnull(DimensionUnit,'') = '" + ((HiddenField)gr.FindControl("grdHdfDimensionUnit")).Value + "'";
            cond += " and isnull(Weight,'') = '" + ((HiddenField)gr.FindControl("grdHdfWeight")).Value + "'";
            cond += " and isnull(WeightUnit,'') = '" + ((HiddenField)gr.FindControl("grdHdfWeightUnit")).Value + "'";

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
            cond += " and isnull(UserResponse,'') = '" + ((HiddenField)gr.FindControl("grdHdfUserResponse")).Value + "'";
            return cond;
        }
    }
}