using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


namespace AP_StockPromotion_V1.webpage
{
    public partial class DisbursementCheckAmountForEqt : System.Web.UI.Page
    {
        string[] strItemGroup = { "ReceiveHeaderID", "PO_No", "GR_No", "ReceiveDetailId", "ItemName", "Model", "Color", "DimensionWidth", "DimensionLong", "DimensionHeight", "DimensionUnit", "Weight", "WeightUnit", "Price", "ProduceDate", "ExpireDate", "Detail", "Remark" };
        /* - "CreateBy", "CreateDate",  - */
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                hdfReqId.Value = Request.QueryString["reqId"] + "";
                initPage(Request.QueryString["ItemId"]);
            }
        }

        private void initPage(string ItemId)
        {
           
            Class.DATransferToProject cls = new Class.DATransferToProject();
            hdfMasterItemID.Value = ItemId;
            DataTable dtStockItemList = cls.getItemForTransferToProjectByMasterItemId(Convert.ToInt32(ItemId));

            //dtStockItemList.DefaultView.RowFilter = "UserResponse = '" + dt.DefaultView[0]["ReqBy"].ToString().Trim() + "'";
            ////เพิ่มตรงนี้เนื่องจาก userResponse ไม่มีการ filter ทำให้ข้อมูลสินค้าที่เป็น ItemMaster เดียวกัน ดึงข้อมุลขึ้นมาทั้งๆที่อาจจะเป็นคนละ ReceiveDetailId ก็ได้
            ////ไม่ใช้เนื่องจากลองไล่ code ดูแล้วบาง record ไม่มี UserResponse

            DataTable dtItemSelected = (DataTable)Session["ItemTransfer"];
            if (dtItemSelected != null)
            {
                dtItemSelected.DefaultView.RowFilter = "ReqId <> " + hdfReqId.Value;
                foreach (DataRow dr in dtItemSelected.DefaultView.ToTable().Rows)
                {
                    DataRow[] drDupItem = dtStockItemList.Select("ItemId=" + dr["ItemId"]);
                    if (drDupItem.Length > 0)
                    {
                        dtStockItemList.Rows.Remove(drDupItem[0]);
                    }
                }
                dtItemSelected.DefaultView.RowFilter = "";
            }
            Session["dtStockItemList"] = dtStockItemList.Copy();

            DataTable dtStockItemBalanceList = dtStockItemList.DefaultView.ToTable(true, strItemGroup);

            dtStockItemBalanceList.Columns.Add(new DataColumn("StockBalance", typeof(int)));
            dtStockItemBalanceList.Columns.Add(new DataColumn("StockItemIdList", typeof(string)));
            dtStockItemBalanceList.AcceptChanges();
            foreach (DataRow dr in dtStockItemBalanceList.Rows)
            {
                dr["StockBalance"] = dtStockItemList.Compute("Count(ItemId)", getFilterMatch(dr, strItemGroup));
                DataRow[] drx = dtStockItemList.Select(getFilterMatch(dr, strItemGroup));
                // string x = new Class.DADelivery().lookTable(dtStockItemList);
                dr["StockItemIdList"] = "";
                foreach (DataRow d in drx)
                {
                    dr["StockItemIdList"] += "," + d["ItemId"].ToString();
                }
                if ((dr["StockItemIdList"] + "").Length > 0) { dr["StockItemIdList"] = (dr["StockItemIdList"] + "").Remove(0, 1); }
            }
            dtStockItemBalanceList.AcceptChanges();
            Session["dtStockItemBalanceList"] = dtStockItemBalanceList.Copy();
            grdData.DataSource = dtStockItemBalanceList;
            grdData.DataBind();

            bindDataRequest();
        }

        private void bindDataRequest()
        {
            string ReqID =  Request.QueryString["ReqID"];
            DataTable dt = (DataTable)Session["requestList"];
            dt.DefaultView[0]["Transfer"] = sumSelectAmount();
            dt = dt.Select("ReqID = '" + ReqID + "'").CopyToDataTable();
            dt.AcceptChanges();                    

            grdRequest.DataSource = dt.DefaultView;
            grdRequest.DataBind();
        }

        private int sumSelectAmount()
        {
            int rst = 0;
            foreach (GridViewRow gr in grdData.Rows)
            {
                int bal = 0;
                int.TryParse(((HiddenField)gr.FindControl("grdHdfStockBalance")).Value, out bal);

                int amt = 0;
                if (!int.TryParse(((TextBox)gr.FindControl("grdTxtSelectAmount")).Text, out amt))
                {
                    ((TextBox)gr.FindControl("grdTxtSelectAmount")).Text = "";
                }
                if (amt < 0)
                {
                    ((TextBox)gr.FindControl("grdTxtSelectAmount")).Text = "";
                    amt = 0;
                }
                if (amt > bal)
                {
                    ((TextBox)gr.FindControl("grdTxtSelectAmount")).Text = bal + "";
                    amt = bal;
                }
                rst += amt;
            }
            return rst;
        }

        private string getFilterMatch(DataRow dr, string[] lstGroup)
        {
            string rst = "";
            foreach (string f in lstGroup)
            {

                if (dr[f].GetType().ToString() == "System.DBNull")
                {
                    rst += " And " + f + " is null ";
                }
                else if (dr[f].GetType().ToString() == "System.String" || dr[f].GetType().ToString() == "System.DateTime")
                {
                    rst += " And " + f + " = '" + dr[f] + "' ";
                }
                else
                {
                    rst += " And " + f + " = " + dr[f] + " ";
                }
            }
            return rst.Remove(0, 4);
        }

        protected void grdRequest_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                Label grdLbTransferred = (Label)e.Row.FindControl("grdLbTransferred");
                HiddenField grdHdfReqAmount = (HiddenField)e.Row.FindControl("grdHdfReqAmount");
                HiddenField grdHdfTransferredAmount = (HiddenField)e.Row.FindControl("grdHdfTransferredAmount");
                //grdLbTransferred.Text = grdHdfTransferredAmount.Value + "/" + grdHdfReqAmount.Value;


                Label grdLbTransfer = (Label)e.Row.FindControl("grdLbTransfer");
                HiddenField grdHdfTransfer = (HiddenField)e.Row.FindControl("grdHdfTransfer");
                HiddenField grdHdfBalance = (HiddenField)e.Row.FindControl("grdHdfBalance");
                //grdLbTransfer.Text = grdHdfTransfer.Value + "/" + grdHdfBalance.Value;


                Label grdLbTotalRequest = (Label)e.Row.FindControl("grdLbTotalRequest");
                Label grdLbBalRequest = (Label)e.Row.FindControl("grdLbBalRequest");

               

                grdLbTotalRequest.Text = grdHdfReqAmount.Value;
                grdLbBalRequest.Text = grdHdfBalance.Value;
                grdLbTransfer.Text = grdHdfTransfer.Value;



            }
        }

        protected void grdRequest_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void grdData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox grdTxtSelectAmount = (TextBox)e.Row.FindControl("grdTxtSelectAmount");
                HiddenField grdHdfStockBalance = (HiddenField)e.Row.FindControl("grdHdfStockBalance");

                int wait = 0;
                int.TryParse(hdfWaitAmount.Value, out wait);
                int tran = 0;
                int.TryParse(hdfTranAmount.Value, out tran);
                int stock = 0;
                int.TryParse(grdHdfStockBalance.Value, out stock);

                int want = wait - tran;
                if (want > 0)
                {
                    if (want >= stock)
                    {
                        grdTxtSelectAmount.Text = stock.ToString();
                        hdfTranAmount.Value = (tran + stock).ToString();

                    }
                    else
                    {
                        grdTxtSelectAmount.Text = want.ToString();
                        hdfTranAmount.Value = (want + stock).ToString();

                    }
                }

                grdTxtSelectAmount.Attributes.Add("onkeyup", "calcBalance('" + grdTxtSelectAmount.ClientID + "','" + grdHdfStockBalance.ClientID + "');");

            }
        }

        protected void btnReCalcTrans_Click(object sender, EventArgs e)
        {
            bindDataRequest();
            // ScriptManager.RegisterStartupScript(this, GetType(), "js", "setFocusTextBox('"+ hdfTextFocusTo.Value +"');", true);     
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            string itemIdList = "";

            DataTable dt = (DataTable)Session["dtStockItemList"];
            foreach (GridViewRow gr in grdData.Rows)
            {
                TextBox grdTxtSelectAmount = (TextBox)gr.FindControl("grdTxtSelectAmount");
                HiddenField grdHdfStockItemIdList = (HiddenField)gr.FindControl("grdHdfStockItemIdList");
                string[] lstItemId = grdHdfStockItemIdList.Value.Split(',');
                int amt = 0;
                int.TryParse(grdTxtSelectAmount.Text, out amt);

                if (amt > 0)
                {
                    for (int ii = 0; ii < amt; ii++)
                    {
                        itemIdList += "," + lstItemId[ii];
                    }
                }
            }
            if (itemIdList.Length > 0)
            {
                itemIdList = itemIdList.Remove(0, 1);
            }

            if (itemIdList.Trim() != "")
            {
                dt = dt.Select(" Itemid in(" + "'" + itemIdList.Trim().Replace(",", "','") + "'" + ")").CopyToDataTable();// set format itemno เพื่อใช้ในการกรองข้อมูลว่า user ได้เลือกสินค้าไหนไปจ่ายบ้าง
                Session["ItemChoseEtq"] = dt;
            }
            else
            {
                Session["ItemChoseEtq"] = null;
            }
            
            ScriptManager.RegisterStartupScript(this, GetType(), "js", "sendDataItemAmountList('" + itemIdList + "', '" + hdfReqId.Value + "');", true);

        }
    }
}