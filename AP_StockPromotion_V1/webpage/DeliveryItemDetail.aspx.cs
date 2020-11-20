using AP_StockPromotion_V1.ws_authorize;
using System;
using System.Data;
using System.Web.UI;

namespace AP_StockPromotion_V1.webpage
{
    public partial class DeliveryItemDetail : System.Web.UI.Page
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
            string DelvLstId = Request.QueryString["DelvLstId"] + "";
            Class.DADelivery cls = new Class.DADelivery();
            Int64 delvLstId = 0;
            Int64.TryParse(DelvLstId, out delvLstId);
            DataTable dt = cls.GetDataDeliveryItem(delvLstId);
            Session["DeliveryItem"] = dt.Copy();
            if (dt.Rows.Count > 0)
            {
                /*
                 DelvPromotionId	PromotionId DelvBy	DelvDate
                 ItemId  Serial	ItemName	Model	Color	DimensionWidth	DimensionLong	DimensionHeight	DimensionUnit	Weight	WeightUnit	Price	ProduceDate	ExpireDate	Detail	Remark
                 */
                DataRow dr = dt.Rows[0];
                txtDelvNo.Text = dr["DelvPromotionId"] + "";
                if (dr["isPostAcc"] + "" == "Y")
                {
                    txtDelvStatus.Text = "บันทึกบัญชีแล้ว";
                    btnConfirmDelv.Attributes.Add("style", "display:none;");
                    btnDelDeliv.Attributes.Add("style", "display:none;");
                }
                else
                {
                    if (dr["isConfirm"] + "" == "Y")
                    {
                        btnConfirmDelv.Attributes.Add("style", "display:none;");
                        txtDelvStatus.Text = "รอการบันทึกบัญชี";
                    }
                    else
                    {
                        txtDelvStatus.Text = "รายการใหม่";
                        btnDelDeliv.Attributes.Add("style", "display:none;");
                    }
                }
                txtDelvBy.Text = dr["DelvBy"] + "";
                Entities.FormatDate convertDate = new Entities.FormatDate();
                string dDate = "";
                convertDate.getStringFromDate(dr["DelvDate"], ref dDate);
                txtDelvDate.Text = dDate;

                DataTable dtGrd = dt.DefaultView.ToTable(true, "ItemName", "Serial", "Model", "Color", "Price", "ProduceDate", "ExpireDate", "Detail", "Remark", "ReqDocNo", "TrListId");
                dtGrd.Columns.Add(new DataColumn("Amount", typeof(int)));
                dtGrd.Columns.Add(new DataColumn("Total", typeof(decimal)));
                foreach (DataRow drGrd in dtGrd.Rows)
                {
                    string cond = "isNull(ItemName,'') = '" + drGrd["ItemName"] + "' AND isNull(Serial,'') = '" + drGrd["Serial"] + "' AND isNull(Model,'') = '" + drGrd["Model"] + "' AND isNull(Color,'') = '" + drGrd["Color"] + "' AND isNull(Price,'') = '" + drGrd["Price"] + "' AND isNull(ProduceDate,'') = '" + drGrd["ProduceDate"] + "' AND isNull(ExpireDate,'') = '" + drGrd["ExpireDate"] + "' AND isNull(Detail,'') = '" + drGrd["Detail"] + "' AND isNull(Remark,'') = '" + drGrd["Remark"] + "' AND isNull(ReqDocNo,'') = '" + drGrd["ReqDocNo"] + "' AND isNull(TrListId,'') = '" + drGrd["TrListId"] + "'";
                    drGrd["Amount"] = dt.Compute("COUNT(ItemId)", cond);
                    drGrd["Total"] = (decimal)drGrd["Price"] * (int)drGrd["Amount"];
                }
                dtGrd.AcceptChanges();
                grdData.DataSource = dtGrd;
                grdData.DataBind();
            }
        }

        protected void btnDelDeliv_Click(object sender, EventArgs e)
        {
            string DelvLstId = Request.QueryString["DelvLstId"] + "";
            AutorizeData auth = new AutorizeData();
            auth = (AutorizeData)Session["userInfo_" + Session.SessionID];
            Class.DADelivery cls = new Class.DADelivery();
            Int64 delvLstId = 0;
            Int64.TryParse(DelvLstId, out delvLstId);
            if (cls.verifyDeliveryItem(delvLstId, auth.EmployeeID, "N"))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ยกเลิกการยืนยันใบส่งมอบสินค้าโปรโมชั่น เสร็จสิ้น.'); bindDataParentPage();", true);
                return;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ไม่สามารถยกเลิกการยืนยันใบส่งมอบสินค้าโปรโมชั่นได้ กรุณาลองใหม่อีกครั้ง !! ');", true);
                return;
            }
        }

        protected void btnConfirmDelv_Click(object sender, EventArgs e)
        {
            Class.DADelivery cls = new Class.DADelivery();
            string msgErr = "";
            string oriItemNo = "";
            DataTable dtPrItem = null;
            DataTable dtDelvItem = (DataTable)Session["DeliveryItem"];
            dtDelvItem.DefaultView.RowFilter = "";
            DataTable dtPr = dtDelvItem.DefaultView.ToTable(true, "PRNo", "ItemId", "ItemNo", "PRItem");
            Class.DASAPConnector sap = new Class.DASAPConnector();
            foreach (DataRow dr in dtPr.Rows)
            {
                if (sap.SAPGetPRDetail(dr["PRNo"] + "", ref dtPrItem, ref msgErr))
                {
                    string prItem = "";
                    DataRow[] drPrMat = dtPrItem.Select("MATERIAL='" + dr["ItemId"] + "' AND DELETE_IND <> 'X'");
                    //if (drPrMat.Length > 1)
                    //{
                    //    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('พบ Material มากกว่า 1 ใน PR เดียวกัน กรุณาติดต่อผู้ดูแลระบบ PRNo:" + dr["PRNo"] + ",Material:" + dr["ItemId"] + " !! ');", true);
                    //    return;
                    //}
                    //else
                    if (drPrMat.Length == 1)
                    {
                        prItem = drPrMat[0]["PREQ_ITEM"] + "";
                    }
                    else if (drPrMat.Length < 1)
                    { /* - หาที่เป็น X - */

                        drPrMat = dtPrItem.Select("MATERIAL='" + dr["ItemId"] + "'");

                        //if (drPrMat.Length > 1)
                        //{
                        //    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('พบ Material มากกว่า 1 ใน PR เดียวกัน กรุณาติดต่อผู้ดูแลระบบ PRNo:" + dr["PRNo"] + ",Material:" + dr["ItemId"] + " !! ');", true);
                        //    return;
                        //}

                        if (drPrMat.Length == 1)
                        {
                            prItem = drPrMat[0]["PREQ_ITEM"] + "";
                        }

                        if (drPrMat.Length < 1)
                        {
                            drPrMat = dtPrItem.Select("MATERIAL='" + dr["ItemNo"] + "' AND DELETE_IND <> 'X'");

                            if (drPrMat.Length < 1)
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('พบข้อผิดพลาด กรุณาติดต่อผู้ดูแลระบบ PRNo:" + dr["PRNo"] + ", Material SAP:" + dtPrItem.Rows[0]["MATERIAL"] + " <> Material Stock:" + dr["ItemNo"] + " !! ');", true);
                                return;
                            }

                            oriItemNo = dr["ItemNo"].ToString();
                        }
                    }

                    if (prItem == "")
                    {
                        prItem = dr["PRItem"].ToString();
                    }

                    if (!cls.UpdatePRItem(dr["PRNo"] + "", (oriItemNo != "" ? dr["ItemId"] : oriItemNo) + "", prItem, ref msgErr))
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ไม่สามารถแก้ไข PRItem ของ " + dr["PRNo"] + " ได้ กรุณาติดต่อผู้ดูแลระบบ !! ');", true);
                        return;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ไม่สามารถเรียกดูข้อมูล PR " + dr["PRNo"] + " กรุณาติดต่อผู้ดูแลระบบ !! ');", true);
                    return;
                }
            }

            string DelvLstId = Request.QueryString["DelvLstId"] + "";
            AutorizeData auth = new AutorizeData();
            auth = (AutorizeData)Session["userInfo_" + Session.SessionID];
            Int64 delvLstId = 0;
            Int64.TryParse(DelvLstId, out delvLstId);
            if (cls.verifyDeliveryItem(delvLstId, auth.EmployeeID, "Y"))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ยืนยันใบส่งมอบสินค้าโปรโมชั่น เสร็จสิ้น.'); bindDataParentPage();", true);
                return;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "js", "alert('ไม่สามารถยืนยันใบส่งมอบสินค้าโปรโมชั่นได้ กรุณาลองใหม่อีกครั้ง !! ');", true);
                return;
            }
        }

    }
}