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
    public partial class StockItemDestroyedDetail : System.Web.UI.Page
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
            bindData();
            bindDDLReverseReason();
            txtPostingDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }

        private void bindDDLReverseReason()
        {
            Class.DAStockPromotion dasp = new Class.DAStockPromotion();
            DataTable dt = dasp.getDataStatus("17");
            ddlReverseReason.DataSource = dt;
            ddlReverseReason.DataTextField = "StatusText";
            ddlReverseReason.DataValueField = "StatusValue";
            ddlReverseReason.DataBind();
        }
        protected void bindData()
        {
            int destroyListId = 0;
            if (!int.TryParse(Request.QueryString["DestroyListId"] + "", out destroyListId))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('[url] ไม่ถูกต้อง !!'); window.location.replace('StockItemDestroy.aspx');", true);
                return;
            }

            Class.DAStockItemDestroy cls = new Class.DAStockItemDestroy();
            DataTable dt = cls.getDataStockItemProjectDestroyedByDestroyListId(destroyListId);
            if (dt.Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('[url] ไม่ถูกต้อง !!'); window.location.replace('StockItemDestroy.aspx');", true);
                return;
            }
            DataRow drZero = dt.Rows[0];
            txtProject.Text = drZero["ProjectName"] + "";
            txtDestroyBy.Text = drZero["FullName"] + "";
            txtDestroyDate.Text = ((DateTime)drZero["CreateDate"]).ToString("dd/MM/yyyy");
            txtPostingDate.Text = txtDestroyDate.Text;
            txtReason.Text = drZero["DestroyReason"] + "";
            txtSapDocNo.Text = drZero["OBJKEY"] + "";
            hdfOBJ_KEY.Value = drZero["OBJ_KEY"] + "";
            hdfOBJ_SYS.Value = drZero["OBJ_SYS"] + "";
            hdfOBJ_TYPE.Value = drZero["OBJ_TYPE"] + "";

            DataTable dtDestroyList = dt.DefaultView.ToTable(true, "Serial", "Barcode", "ItemName", "Model", "Color", "DimensionWidth", "DimensionLong", "DimensionHeight", "DimensionUnit", "Weight", "WeightUnit", "Price", "ProduceDate", "ExpireDate", "Detail", "Remark", "TrListId");
            dtDestroyList.Columns.Add(new DataColumn("DestroyAmount",typeof(int)));
            foreach (DataRow dr in dtDestroyList.Rows)
            { 
                string cond = "1=1 ";
                cond += " and isnull(Serial,'') = '" + dr["Serial"] + "'";
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
                cond += " and isnull(Price,'') = '" + dr["Price"] + "'";
                if (dr["ProduceDate"] + "" == "")
                {
                    cond += " and ProduceDate is null ";
                }
                else
                {
                    cond += " and ProduceDate = '" + dr["ProduceDate"] + "'";
                }
                if (dr["ExpireDate"] + "" == "")
                {
                    cond += " and ExpireDate is null ";
                }
                else
                {
                    cond += " and ExpireDate = '" + dr["ExpireDate"] + "'";
                }
                cond += " and isnull(Detail,'') = '" + dr["Detail"] + "'";
                cond += " and isnull(Remark,'') = '" + dr["Remark"] + "'";
                cond += " and isnull(TrListId,'') = '" + dr["TrListId"] + "'";

                dr["DestroyAmount"] = dt.Select(cond).Length;
            }
            dtDestroyList.AcceptChanges();
            grdData.DataSource = dtDestroyList;
            grdData.DataBind();
        }

        protected void btnCancelDestroy_Click(object sender, EventArgs e)
        {
            if (txtPostingDate.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('กรุณาระบุ Posting Date !!');", true);
                return;
            }
            AutorizeData auth = (AutorizeData)Session["userInfo_" + Session.SessionID];
            Entities.SapDestroy_REVERSAL rev = new Entities.SapDestroy_REVERSAL();
            rev.OBJ_TYPE = hdfOBJ_TYPE.Value;
            rev.OBJ_SYS = hdfOBJ_SYS.Value;
            rev.OBJ_KEY_R = hdfOBJ_KEY.Value;
            rev.PostingDate = txtPostingDate.Text;
            DateTime sDate = new DateTime();
            convertDate.getDateFromString(txtPostingDate.Text, ref sDate);
            rev.FIS_PERIOD = sDate.Month.ToString("00");
            rev.COMP_CODE = "1000";
            rev.REASON_REV = ddlReverseReason.SelectedItem.Value;
            rev.AC_DOC_NO = hdfOBJ_KEY.Value.Substring(0, 10);

            int destroyListId = 0;
            int.TryParse(Request.QueryString["DestroyListId"] + "", out destroyListId);


            string msgErr = "";
            Class.DASAPConnector sap = new Class.DASAPConnector();
            string OBJ_TYPE = "";
            string OBJ_KEY = "";
            string OBJ_SYS = "";
            if (sap.SAPReverseDamage(rev, destroyListId, auth.EmployeeID, ref OBJ_TYPE, ref OBJ_KEY, ref OBJ_SYS, ref msgErr))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('การดำเนินการเสร็จสิ้น SAP DocNo. " + OBJ_KEY.Substring(0,10) + "'); window.location.replace('StockItemDestroy.aspx');", true);
                return;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('การดำเนินการผิดพลาด กรุณาติดต่อผู้ดูแลระบบ !! " + msgErr.Replace("'", "") + "');", true);
                return;
            }

            //int destroyListId = 0;
            //int.TryParse(Request.QueryString["DestroyListId"] + "", out destroyListId);
            //Class.DAStockItemDestroy cls = new Class.DAStockItemDestroy();
            //if (cls.cancelDestroyItem(destroyListId, auth.EmployeeID))
            //{
            //    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('การดำเนินการเสร็จสิ้น'); window.location.replace('StockItemDestroy.aspx');", true);
            //    return;
            //}
            //else
            //{
            //    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('การดำเนินการผิดพลาด กรุณาติดต่อผู้ดูแลระบบ !!');", true);
            //    return;
            //}
        }

        protected void btnReasonEdit_Click(object sender, EventArgs e)
        {
            txtReason.ReadOnly = false;
            btnReasonEdit.Visible = false;
            btnReasonSave.Visible = true;
        }

        protected void btnReasonSave_Click(object sender, EventArgs e)
        {           
            string DestroyListId = Request.QueryString["DestroyListId"].Trim();
            Class.DAStockItemDestroy cls = new Class.DAStockItemDestroy();
            AutorizeData auth = (AutorizeData)Session["userInfo_" + Session.SessionID];          
            if (cls.destroyReasonEdit(DestroyListId, txtReason.Text.Trim(),auth.EmployeeID) == true)
            {
                txtReason.ReadOnly = true;
                btnReasonEdit.Visible = true;
                btnReasonSave.Visible = false;
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('แก้ไขข้อมูลเหตุผลเรียบร้อย!!');", true);              
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('เกิดข้อผิดพลาดขณะทำการบันทึก กรุณาติดต่อเจ้าหน้าที่IT!!');", true);
            }
           
        }


    }
}