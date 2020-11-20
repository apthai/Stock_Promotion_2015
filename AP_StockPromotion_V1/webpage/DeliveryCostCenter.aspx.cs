using AP_StockPromotion_V1.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace AP_StockPromotion_V1.webpage
{
    public partial class DeliveryCostCenter : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static object[] InitialCbxCostCenter()
        {
            try
            {
                string MsgErr = "";
                List<ListOfCostCenter> lstOfCC = new List<ListOfCostCenter>();
                DAStockPromotion dasp = new DAStockPromotion();
                object[] res = dasp.GetCbxCostCenter();
                if (res[0].ToString() == "")
                {
                    lstOfCC = (from t1 in ((DataTable)res[1]).AsEnumerable()
                               select new ListOfCostCenter
                               {
                                   CCID = t1.ItemArray[0].ToString(),
                                   CCNAME = t1.ItemArray[1].ToString()
                               }).ToList();

                }
                else
                {
                    MsgErr = res[0].ToString();
                }

                return new object[]
                {
                    MsgErr, lstOfCC
                };
            }
            catch (Exception ex)
            {
                return new object[]
                {
                    ex.Message.ToString(),
                    new List<ListOfCostCenter>()
                };
            }
        }

        [WebMethod]
        public static object[] InitialCbxItemPromotion()
        {
            try
            {
                string MsgErr = "";
                List<ListOfItems> lstOfItems = new List<ListOfItems>();
                DAStockPromotion dasp = new DAStockPromotion();
                object[] res = dasp.GetCbxItemPromotion();
                if (res[0].ToString() == "")
                {
                    lstOfItems = (from t1 in ((DataTable)res[1]).AsEnumerable()
                                  select new ListOfItems
                                  {
                                      ITEMID = t1.ItemArray[0].ToString(),
                                      ITEMNAME = t1.ItemArray[1].ToString()
                                  }).ToList();
                }
                else
                {
                    MsgErr = res[0].ToString();
                }

                return new object[]
                {
                    MsgErr, lstOfItems
                };
            }
            catch (Exception ex)
            {
                return new object[]
                {
                    ex.Message.ToString(),
                    new List<ListOfCostCenter>()
                };
            }
        }

        [WebMethod]
        public static object[] OnCommandSearch(string docno, string costcenter, string itemno, string startdate, string enddate, int DelvStatus)
        {
            try
            {
                string MsgErr = "";
                DAStockPromotion da = new DAStockPromotion();
                var res = (from t1 in da.GetDataDeliveryCostCenterSearch(docno, costcenter, itemno, startdate, enddate, DelvStatus, out MsgErr).AsEnumerable()
                           select new DeliveryData
                           {
                               ID = t1.ItemArray[0].ToString(),
                               DocNo = t1.ItemArray[1].ToString(),
                               CostCenterCode = t1.ItemArray[2].ToString(),
                               CostCenterName = t1.ItemArray[3].ToString(),
                               ItemNo = t1.ItemArray[4].ToString(),
                               ItemName = t1.ItemArray[5].ToString(),
                               Quantity = t1.ItemArray[6].ToString(),
                               RecordingDate = t1.ItemArray[7].ToString(),
                               RecordingBy = t1.ItemArray[8].ToString(),
                               Status = t1.ItemArray[9].ToString(),
                               StatusId = t1.ItemArray[10].ToString(),
                               GLID = long.Parse((t1.ItemArray[11] ?? 0).ToString()),
                               GLN = t1.ItemArray[12].ToString()
                           }).ToList();
                return new object[]
                {
                    "",
                    res
                };
            }
            catch (Exception ex)
            {
                return new object[]
                {
                    ex.Message.ToString(),
                    ""
                };
            }
        }

        [WebMethod]
        public static object[] DeliveryAddDetail(int id, string amount, string pstingdate, string empcode)
        {
            try
            {
                string NewDocNo = "";
                string MsgErr = "";
                string Msg = "";
                bool isSuccess = false;
                DAStockPromotion dasp = new DAStockPromotion();
                NewDocNo = dasp.DeliveryAddSerialDetail(id, amount, pstingdate, empcode, out MsgErr, out isSuccess, out Msg);
                if (!isSuccess && Msg != "SERIAL")
                {
                    NewDocNo = dasp.DeliveryAddDetail(id, amount, pstingdate, empcode, out MsgErr);
                }

                switch (MsgErr)
                {
                    case "Error converting data type varchar to int.":
                        MsgErr = "ลักษณะสินค้าเป็นชิ้น กรุณาระบุจำนวนที่เป็นตัวเลข";
                        break;
                }

                return new object[]
                {
                    MsgErr,
                    NewDocNo,
                    Msg
                };
            }
            catch (Exception ex)
            {
                return new object[]
                {
                    ex.Message.ToString(),
                    null,
                    null
                };
            }
        }

        [WebMethod]
        public static object[] GetDeliveryCostCenterDetail(int id)
        {
            try
            {
                string MsgErr = "";
                DAStockPromotion dasp = new DAStockPromotion();
                var res = (from t1 in dasp.GetDeliveryCostCenterDetail(id, out MsgErr).AsEnumerable()
                           select new ListOfDelvDetail
                           {
                               ID = Convert.ToInt32(t1.ItemArray[0]),
                               DOCNO = t1.ItemArray[1].ToString(),
                               ITEMNO = t1.ItemArray[2].ToString(),
                               ITEMNAME = t1.ItemArray[3].ToString(),
                               QUANTITY = Convert.ToInt32(t1.ItemArray[4]),
                               STATUS = t1.ItemArray[5].ToString(),
                           }).ToList();

                return new object[]
                {
                    "",
                    res
                };
            }
            catch (Exception ex)
            {
                return new object[]
                {
                    ex.Message.ToString(),
                    ""
                };
            }
        }

        [WebMethod]
        public static object[] DeliveryDelDetail(int id)
        {
            try
            {
                string MsgErr = "";
                DAStockPromotion dasp = new DAStockPromotion();
                bool isPass = dasp.DeliveryDelDetail(id, out MsgErr);
                return new object[]
                {
                    MsgErr,
                    isPass
                };
            }
            catch (Exception ex)
            {
                return new object[]
                {
                    ex.Message.ToString(),
                    null
                };
            }
        }

        [WebMethod]
        public static object[] DeliveryGetReport(List<ListOfDeliveryID> ListOfID, string CostCenter)
        {
            try
            {
                string lstDelvId = "";
                lstDelvId = string.Join(",", (ListOfID.Select(e => e.DelvID.ToString()).Count() > 0 ? ListOfID.Select(e => e.DelvID.ToString()) : default(IEnumerable<string>)));
                string res = GenerateReportDeliveryCostCenter(lstDelvId, CostCenter);

                return new object[]
                {
                    "", res
                };
            }
            catch (Exception ex)
            {
                return new object[]
                {
                    ex.Message.ToString(), ""
                };
            }
        }

        public static string GenerateReportDeliveryCostCenter(string ListDelvId, string CostCenter)
        {
            string MegErr = "";
            frmReport frmRpt = new frmReport();
            return (CostCenter == "1" ? frmRpt.ReportDeliveryCostCenterDetail(ListDelvId, out MegErr) : frmRpt.ReportDeliveryCostCenterCrossComDetail(ListDelvId, out MegErr));
        }

        [WebMethod]
        public static object[] GetRecQuantity(int id)
        {
            try
            {
                string MsgErr = "";
                DAStockPromotion dastp = new DAStockPromotion();
                int quantity = dastp.GetRecQuantity(id, out MsgErr);
                return new object[]
                {
                    MsgErr,
                    quantity
                };
            }
            catch (Exception ex)
            {
                return new object[]
                {
                    ex.Message.ToString(),
                    ""
                };
            }
        }

        [WebMethod]
        public static object[] GetCostCenterData()
        {
            try
            {
                string MsgErr = "";
                DAStockPromotion dastp = new DAStockPromotion();
                var res = (from t1 in dastp.GetCostCenterData(out MsgErr).AsEnumerable()
                           select new CostCenter
                           {
                               CostCenterID = t1.ItemArray[0].ToString(),
                               CostCenterName = t1.ItemArray[1].ToString()
                           }).ToList();

                return new object[]
                {
                    "", res
                };
            }
            catch (Exception ex)
            {
                return new object[]
                {
                    ex.Message.ToString(), ""
                };
            }
        }

        [WebMethod]
        public static object[] GetCostCenterData2()
        {
            try
            {
                string MsgErr = "";
                DAStockPromotion dastp = new DAStockPromotion();
                var res = (from t1 in dastp.GetCostCenterData(out MsgErr).AsEnumerable()
                           select new CostCenter
                           {
                               CostCenterID = t1.ItemArray[0].ToString(),
                               CostCenterName = t1.ItemArray[1].ToString()
                           }).ToList();
                return new object[]
                {
                    "",
                    res
                };
            }
            catch (Exception ex)
            {
                return new object[]
                {
                    ex.Message.ToString(),
                    ""
                };
            }
        }

        [WebMethod]
        public static object[] GetGLData()
        {
            try
            {
                string MsgErr = "";
                DAStockPromotion dastp = new DAStockPromotion();
                var res = (from t1 in dastp.GetGLData(out MsgErr).AsEnumerable()
                           select new GL
                           {
                               GLID = long.Parse(t1.ItemArray[0].ToString()),
                               GLN = t1.ItemArray[1].ToString()
                           }).ToList();
                return new object[]
                {
                    "",
                    res
                };
            }
            catch (Exception ex)
            {
                return new object[]
                {
                    ex.Message.ToString(),
                    ""
                };
            }
        }

        [WebMethod]
        public static object[] EditCostCenter(string currentCostcenterId, int docId)
        {
            try
            {
                string MsgErr = "";
                DAStockPromotion dastp = new DAStockPromotion();
                dastp.EditCostCenter(docId, currentCostcenterId, out MsgErr);
                return new object[]
                {
                    MsgErr
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
        public static object[] EditGL(string currentGLId, int docId)
        {
            try
            {
                string MsgErr = "";
                DAStockPromotion dastp = new DAStockPromotion();
                dastp.EditGL(docId, currentGLId, out MsgErr);
                return new object[]
                {
                    MsgErr
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

        public class ListOfDeliveryID
        {
            public int DelvID { get; set; }
        }
        private class ListOfDelvDetail
        {
            public int ID { get; set; }
            public string DOCNO { get; set; }
            public string ITEMNO { get; set; }
            public string ITEMNAME { get; set; }
            public int QUANTITY { get; set; }
            public string STATUS { get; set; }
        }
        private class ListOfCostCenter
        {
            public string CCID { get; set; }
            public string CCNAME { get; set; }
        }
        private class ListOfItems
        {
            public string ITEMID { get; set; }
            public string ITEMNAME { get; set; }
        }
        private class DeliveryData
        {
            public string ID { get; set; }
            public string DocNo { get; set; }
            public string CostCenterCode { get; set; }
            public string CostCenterName { get; set; }
            public string ItemNo { get; set; }
            public string ItemName { get; set; }
            public string Quantity { get; set; }
            public string RecordingDate { get; set; }
            public string RecordingBy { get; set; }
            public string Status { get; set; }
            public string StatusId { get; set; }

            public long GLID { get; set; }
            public string GLN { get; set; }
        }
    }
}