using AP_StockPromotion_V1.Class;
using AP_StockPromotion_V1.ws_authorize;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;

namespace AP_StockPromotion_V1.webpage
{
    public partial class RequisitionRequest : Page
    {
        string EmployeeID = "";

        protected void Page_Load(object sender, EventArgs e)
        {

            AutorizeData autorizeDatum = new AutorizeData();
            autorizeDatum = (AutorizeData)Session[string.Concat("userInfo_", Session.SessionID)] ?? new AutorizeData();

            if ((autorizeDatum.EmployeeID ?? "") == "")
            {
                Cache["URLFromEmail"] = Request.Url.ToString();
                Session[string.Concat("userInfo_", Session.SessionID)] = null;
            }
            else
            {
                AutorizeData auth = new AutorizeData();
                auth = (AutorizeData)Session["userInfo_" + Session.SessionID];

                if (autorizeDatum != null)
                {
                    Session["Usd_FirstName"] = auth.FirstName;
                    Session["Usd_LastName"] = auth.LastName;
                    Session["Usd_Email"] = auth.Email;
                    Session["Usd_EmployeeID"] = autorizeDatum.EmployeeID;

                    EmployeeID = autorizeDatum.EmployeeID;

                    Permission permission = new Permission();

                    var roleCode = JsonConvert.DeserializeObject<dynamic>(auth.SysUserRoles);
                    foreach (dynamic roles in roleCode.Roles)
                    {

                        string str = (string)roles.RoleCode;
                        switch (str)
                        {
                            case "Admin":
                                {
                                    permission.Admin = true;
                                    Session["Usd_Mode"] = "Admin";
                                    break;
                                }
                            case "CLevel":
                                {
                                    permission.CLevel = true;
                                    Session["Usd_Mode"] = "CLevel";
                                    break;
                                }
                            case "HeadOf":
                                {
                                    permission.HeadOf = true;
                                    Session["Usd_Mode"] = "HO";
                                    break;
                                }
                            case "LCM":
                                {
                                    permission.LCM = true;
                                    Session["Usd_Mode"] = "O";
                                    break;
                                }
                            case "HR":
                                {
                                    permission.HR = true;
                                    Session["Usd_Mode"] = "H";
                                    break;
                                }
                            case "ETC":
                                {
                                    permission.ETC = true;
                                    Session["Usd_Mode"] = "O";
                                    break;
                                }
                            case "MKT":
                                {
                                    permission.MKT = true;
                                    Session["Usd_Mode"] = "M";
                                    break;
                                }
                            case "LC":
                                {
                                    permission.ETC = true;
                                    Session["Usd_Mode"] = "O";
                                    break;
                                }
                            case "AdminCenter":
                                {
                                    permission.Admin = true;
                                    Session["Usd_Mode"] = "Admin";
                                    break;
                                }
                            case "CFI":
                                {
                                    permission.Admin = true;
                                    Session["Usd_Mode"] = "CFI";
                                    break;
                                }
                        }
                    }

                    if ((Session["Usd_Mode"] ?? "").ToString() == "")
                    {
                        permission.ETC = true;
                        Session["Usd_Mode"] = "Other";
                    }

                    Permission permission1 = GerUserPermission(permission, Session["Usd_EmployeeID"].ToString());
                    Session["Permission"] = (new JavaScriptSerializer()).Serialize(permission1);
                    Session["CostCenter"] = (new JavaScriptSerializer()).Serialize(GetCostCenterAll());
                    Session["CostCenterForNewReq"] = (new JavaScriptSerializer()).Serialize(GetCostCenterForNewReq());
                    Session["GL"] = (new JavaScriptSerializer()).Serialize(GetGLData(permission.MKT));
                    Session["Objective"] = (new JavaScriptSerializer()).Serialize(GetObjectiveData());
                    Session["Projects"] = (new JavaScriptSerializer()).Serialize(GetProjectsData());
                    Session["PromotionItems"] = (new JavaScriptSerializer()).Serialize(GetPromotionItemsData());
                    USRTYPE userData = GetUserData(permission1);
                    Session["AllUser"] = (new JavaScriptSerializer()).Serialize(userData.UserAllType);
                    Session["CLevelUser"] = (new JavaScriptSerializer()).Serialize(userData.UserCLevelType);
                    Session["HeadOfUser"] = (new JavaScriptSerializer()).Serialize(userData.UserHeadOfType);
                    Session["LCMUser"] = (new JavaScriptSerializer()).Serialize(userData.UserLCMType);
                    Session["HRUser"] = (new JavaScriptSerializer()).Serialize(userData.UserHRType);
                    Session["MKTUser"] = (new JavaScriptSerializer()).Serialize(userData.UserMarketingType);
                    Session["ETCUser"] = (new JavaScriptSerializer()).Serialize(userData.UserETCType);

                    if (Session["MKTUser"] != null)
                    {
                        lbCostCenter.Text = "เพื่อโครงการ (Internal Order)";
                    }
                }
            }
        }

        [WebMethod]
        public static object CheckListRef(List<string> lstRef)
        {
            object variable;
            try
            {
                bool flag = true;
                string str = " Success";
                flag = new DAStockPromotion().CheckRefNoStatus(ref lstRef, ref str);
                variable = new { Success = flag, Message = str, InvalidRefNo = lstRef };
            }
            catch (Exception exception)
            {
                variable = new { Success = false, exception.Message };
            }
            return variable;
        }

        [WebMethod]
        public static ListRequisitionDetailsData DeleteDataRequisition(string rqrno)
        {
            ListRequisitionDetailsData listRequisitionDetailsDatum;
            try
            {
                listRequisitionDetailsDatum = !new DAStockPromotion().OnCommandDeleteRequisition(rqrno) ? new ListRequisitionDetailsData() : GetDataRequisitionSearch("", "", "", "", "", "", "", "", "");
            }
            catch (Exception)
            {
                listRequisitionDetailsDatum = new ListRequisitionDetailsData();
            }

            return listRequisitionDetailsDatum;
        }

        [WebMethod]
        public static List<DataDetails> EditDataRequisition(string rqrno)
        {
            try
            {
                DAStockPromotion dasp = new DAStockPromotion();
                var requisitionDataDetails = (from t1 in dasp.OnCommandEditRequisition(rqrno).AsEnumerable()
                                              group t1 by new
                                              {
                                                  RQRNO = t1.Field<string>("RQRNO"),
                                                  RCVD = t1.Field<string>("RCVD"),
                                                  CURD = t1.Field<string>("CURD"),
                                                  REQID = t1.Field<string>("REQID"),
                                                  REQN = t1.Field<string>("REQN"),
                                                  PROMOTID = t1.Field<int>("PROMOTID").ToString(),
                                                  PROMOTN = t1.Field<string>("PROMOTN"),
                                                  DEPTID = t1.Field<string>("DEPTID"),
                                                  DEPTN = t1.Field<string>("DEPTN"),
                                                  GLID = t1.Field<int>("GLID").ToString(),
                                                  GLN = t1.Field<string>("GLN"),
                                                  OBJID = t1.Field<int>("OBJID").ToString(),
                                                  OBJN = t1.Field<string>("OBJN"),
                                                  REMK = t1.Field<string>("REMK"),
                                                  APPVID = t1.Field<string>("APPVID"),
                                                  APPVN = t1.Field<string>("APPVN"),
                                                  STATUS = t1.Field<int>("STATUS").ToString(),
                                              } into t
                                              select new DataDetails
                                              {
                                                  RQRNO = t.Key.RQRNO,
                                                  RCVD = t.Key.RCVD.ToString(),
                                                  CURD = t.Key.CURD.ToString(),
                                                  REQID = t.Key.REQID,
                                                  REQN = t.Key.REQN,
                                                  PROMOTID = t.Key.PROMOTID,
                                                  PROMOTN = t.Key.PROMOTN,
                                                  DEPTID = t.Key.DEPTID,
                                                  DEPTN = t.Key.DEPTN,
                                                  GLID = t.Key.GLID,
                                                  GLN = t.Key.GLN,
                                                  OBJID = t.Key.OBJID,
                                                  OBJN = t.Key.OBJN,
                                                  REMK = t.Key.REMK,
                                                  APPVID = t.Key.APPVID,
                                                  APPVN = t.Key.APPVN,
                                                  STATUS = t.Key.STATUS,
                                                  DRNO = (from t2 in dasp.OnCommandEditRequisition(rqrno).AsEnumerable()
                                                          group t2 by new
                                                          {
                                                              DRNO = t2.Field<string>("DRNO"),
                                                          } into tt
                                                          select new DataRefNO
                                                          {
                                                              DRNO = tt.Key.DRNO,
                                                          }).ToList(),
                                                  dataDetails =
                                                          (from t3 in dasp.OnCommandEditRequisition(rqrno).AsEnumerable()
                                                           select new DataProjectDetails
                                                           {
                                                               DRNOBYITEM = t3["DRNOBYITEM"].ToString(),
                                                               PROJID = t3["PROJID"].ToString(),
                                                               PROJN = t3["PROJN"].ToString(),
                                                               PROPID = t3["PROPID"].ToString(),
                                                               PROPN = t3["PROPN"].ToString(),
                                                               RMDB = t3["RMDB"].ToString(),
                                                               STDDT = t3["STDDT"].ToString(),
                                                               ENDDT = t3["ENDDT"].ToString(),
                                                               QUANT = t3["QUANT"].ToString(),
                                                               ITEMS = t3["ITEMS"].ToString(),
                                                               STOCKBALANCE = t3["StockBalance"].ToString(),
                                                               MEMOBALANCE = t3["BALANCE"].ToString(),
                                                               TYPE = t3["TYPE"].ToString(),
                                                           }).ToList(),
                                              }).ToList();

                string msgerr = "";
                var res = new List<DataDetails>();
                res = requisitionDataDetails;
                foreach (var item in requisitionDataDetails)
                {
                    foreach (var itemDetail in item.dataDetails)
                    {
                        var _ret = from t1 in dasp.GetListItemsMemoByDocNo(itemDetail.DRNOBYITEM, itemDetail.PROPID, out msgerr).AsEnumerable()
                                   select new ITEMSQUANTITY
                                   {
                                       STOCKBALANCE = t1.ItemArray[3].ToString(),
                                       MEMOBALANCE = t1.ItemArray[5].ToString(),
                                       TYPE = t1.ItemArray[6].ToString()
                                   };

                        foreach (var _item in _ret)
                        {
                            itemDetail.STOCKBALANCE = _item.STOCKBALANCE;
                            itemDetail.MEMOBALANCE = _item.MEMOBALANCE;
                            itemDetail.TYPE = _item.TYPE;
                        }
                    }
                }

                return requisitionDataDetails;
            }
            catch (Exception)
            {
                return new List<DataDetails>();
            }
        }

        private void execJScript(string jscript)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "js", jscript, true);
        }

        private static string GenerateMailBody(string rqrno)
        {
            string str = string.Concat("", "<strong><span style=\"color:#4b0082;font-size:14px\">ถึงผู้ที่เกี่ยวข้อง,</span></strong><br /><br />");
            str = string.Concat(str, "<span style=\"color:#800080;font-size:12px\">&nbsp;&nbsp; ระบบ Stock Promotion มีเอกสารตั้งเบิกใบใหม่เลขที่ ", rqrno, " เพื่อรอการตรวจสอบ และรับทราบ</span><br />");
            str = string.Concat(str, "<span style=\"color:#800080;font-size:12px\">&nbsp;&nbsp;คุณสามารถเข้าไปตรวจสอบได้ตามลิ้งด้านล่างนี้</span><br />");
            str = string.Concat(str, "<span style=\"color:#800080;font-size:12px\">&nbsp;&nbsp;Program :</span> <strong><a href=\"http://192.168.0.152/apstp/loginx.aspx\">");
            str = string.Concat(str, "<span style=\"color:#0000cd;font-size:12px\"> Stock Promotion </span></a></strong><br /><br />");
            str = string.Concat(str, "<strong><span style=\"color:#4b0082;font-size:14px\"> ขอแสดงความนับถือ</span></strong><br />");
            return string.Concat(str, "<strong><span style=\"color:#4b0082;font-size:14px\"> ระบบ Stock Promotion AP </span></strong>");
        }

        private Permission GerUserPermission(Permission pms, string EmpID)
        {
            DAStockPromotion dAStockPromotion = new DAStockPromotion();
            pms.UserDetails = dAStockPromotion.GetUserDetailByEmpID(EmpID).AsEnumerable().Select((DataRow t1) => new DefaultUser()
            {
                UserGUID = t1[0].ToString(),
                EmpCode = EmpID,
                FName = t1[1].ToString(),
                LName = t1[2].ToString(),
                UserEmail = t1[3].ToString(),
                CostCenterCode = t1[4].ToString(),
                CostCenterName = t1[5].ToString(),
                LeaderID = t1[7].ToString(),
                LeaderFName = t1[8].ToString(),
                LeaderLName = t1[9].ToString(),
                LeaderEmail = t1[10].ToString()
            }).ToList() ?? new List<DefaultUser>();
            return pms ?? new Permission();
        }

        private List<CostCenterData> GetCostCenterAll()
        {
            string str;
            DAStockPromotion dAStockPromotion = new DAStockPromotion();

            List<CostCenterData> list = dAStockPromotion.GetCostCenterData(out str).AsEnumerable().Select((DataRow t1) => new CostCenterData()
            {
                CostCenter = t1.ItemArray[0].ToString(),
                CostCenterName = (t1.ItemArray[1].ToString() == "" ? "" : t1.ItemArray[1].ToString())
            }
            ).ToList();

            return list;
        }

        private List<CostCenterData> GetCostCenterForNewReq()
        {
            string str;
            DAStockPromotion dAStockPromotion = new DAStockPromotion();

            List<CostCenterData> list = (from t1 in dAStockPromotion.GetCostCenterData(out str).AsEnumerable()
                                         where t1.Field<bool?>("IsActive") == true
                                         select new CostCenterData
                                         {
                                             CostCenter = t1.ItemArray[0].ToString(),
                                             CostCenterName = t1.ItemArray[1].ToString()
                                         }).ToList() ?? new List<CostCenterData>();

            //if (list.Any())
            //{
            //    if (Session["MKTUser"] != null && EmployeeID != "AP001167" && EmployeeID != "AP003588")
            //    {
            //        DataTable dt = dAStockPromotion.GetCostCenter(EmployeeID);
            //        if (dt.Rows.Count > 0)
            //        {
            //            list = list.Where(e => e.CostCenter == dt.Rows[0][0].ToString()).ToList();
            //        }
            //    }
            //}

            return list;
        }

        [WebMethod]
        public static List<RequisitionDetailsData> GetDataRequisition()
        {
            List<RequisitionDetailsData> list;
            try
            {
                DAStockPromotion dAStockPromotion = new DAStockPromotion();
                string str = HttpContext.Current.Session["Usd_EmployeeID"].ToString();
                string str1 = HttpContext.Current.Session["Usd_Mode"].ToString();
                list = dAStockPromotion.GetRequisitionByEmpId(str, str1).AsEnumerable().Select((DataRow t1) => new RequisitionDetailsData()
                {
                    CURD = t1[0].ToString(),
                    REFREQID = t1[1].ToString(),
                    REQID = t1[2].ToString(),
                    RQRNO = t1[3].ToString(),
                    DRNO = t1[4].ToString(),
                    STDDT = t1[5].ToString(),
                    ENDDT = t1[6].ToString(),
                    REQN = t1[7].ToString(),
                    STATUS = t1[8].ToString()
                }).ToList();
            }
            catch (Exception)
            {
                list = new List<RequisitionDetailsData>();
            }
            return list;
        }

        [WebMethod]
        public static ListRequisitionDetailsData GetDataRequisitionSearch(string docSearchID, string tagSearchID, string RecieveDatePicker, string BookDatePicker, string cbxUser, string cbxPromotionType, string cbxCostCenter, string cbxObjective, string cbxGLNo)
        {
            string str;
            ListRequisitionDetailsData listRequisitionDetailsDatum;
            ListRequisitionDetailsData requisitionDetailsDatas = new ListRequisitionDetailsData();
            try
            {
                DAStockPromotion dAStockPromotion = new DAStockPromotion();
                string str1 = HttpContext.Current.Session["Usd_Mode"].ToString();
                string str2 = HttpContext.Current.Session["Usd_EmployeeID"].ToString();
                List<RequisitionDetailsData> list = dAStockPromotion.GetDataRequisition(str1 == "Admin" ? "" : str2, docSearchID, tagSearchID, RecieveDatePicker, BookDatePicker, cbxUser, cbxPromotionType, cbxCostCenter, cbxObjective, cbxGLNo, out str).AsEnumerable().Select((DataRow t1) => new RequisitionDetailsData()
                {
                    CURD = t1[0].ToString(),
                    REFREQID = t1[1].ToString(),
                    REQID = t1[2].ToString(),
                    RQRNO = t1[3].ToString(),
                    DRNO = t1[4].ToString(),
                    STDDT = t1[5].ToString(),
                    ENDDT = t1[6].ToString(),
                    REQN = t1[7].ToString(),
                    STATUS = t1[8].ToString()
                }).ToList();
                requisitionDetailsDatas.lstrr = list;
                requisitionDetailsDatas.MsgErr = str;
                listRequisitionDetailsDatum = requisitionDetailsDatas;
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                requisitionDetailsDatas.lstrr = new List<RequisitionDetailsData>();
                requisitionDetailsDatas.MsgErr = exception.Message.ToString();
                listRequisitionDetailsDatum = requisitionDetailsDatas;
            }
            return listRequisitionDetailsDatum;
        }

        private List<GLData> GetGLData(bool IsMarketing)
        {
            List<GLData> list = new List<GLData>();

            if (IsMarketing)
            {
                var RoleCode = "MTK";
                list = new DAStockPromotion().GetGLDataByRole(RoleCode).AsEnumerable().Select((DataRow t1) => new GLData()
                {
                    GLID = t1.ItemArray[0].ToString(),
                    GLDESC = t1.ItemArray[1].ToString()
                }).ToList();
            }
            else
            {
                list = new DAStockPromotion().GetGLData().AsEnumerable().Select((DataRow t1) => new GLData()
                {
                    GLID = t1.ItemArray[0].ToString(),
                    GLDESC = t1.ItemArray[1].ToString()
                }).ToList();
            }

            return list;
        }

        [WebMethod]
        public static ListItemsMemo GetListItemsMemoByDocNo(string docno, string itemno)
        {
            ListItemsMemo listItemsMemo;
            ListItemsMemo itemsMemos = new ListItemsMemo();
            try
            {
                string str = "";
                DAStockPromotion dAStockPromotion = new DAStockPromotion();
                List<ItemsMemo> list = dAStockPromotion.GetListItemsMemoByDocNo(docno, itemno, out str).AsEnumerable().Select((DataRow t1) => new ItemsMemo()
                {
                    DOCNO = t1.ItemArray[0].ToString(),
                    ITEMNO = t1.ItemArray[1].ToString(),
                    ITEMNAME = t1.ItemArray[2].ToString(),
                    STOCKBALANCE = t1.ItemArray[3].ToString(),
                    QUANTITY = t1.ItemArray[4].ToString(),
                    MEMOBALANCE = t1.ItemArray[5].ToString(),
                    TYPE = t1.ItemArray[6].ToString()
                }).ToList();
                itemsMemos.ItemsMemo = list;
                itemsMemos.MsgErr = str;
                listItemsMemo = itemsMemos;
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                itemsMemos.ItemsMemo = new List<ItemsMemo>();
                itemsMemos.MsgErr = exception.Message.ToString();
                listItemsMemo = itemsMemos;
            }
            return listItemsMemo;
        }

        [WebMethod]
        public static ListMemo GetMemoData(string empcode)
        {
            ListMemo listMemo;
            ListMemo memoDatas = new ListMemo();
            try
            {
                string str = HttpContext.Current.Session["Usd_EmployeeID"].ToString();
                string str1 = "";
                List<MemoData> list = (new DAStockPromotion()).GetMemoData((str == "Admin" ? "" : empcode), out str1).AsEnumerable().Select((DataRow t1) => new MemoData()
                {
                    MemoID = t1.ItemArray[0].ToString()
                }).ToList();
                memoDatas.LstMemoData = list;
                memoDatas.MsgErr = str1;
                listMemo = memoDatas;
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                memoDatas.LstMemoData = new List<MemoData>();
                memoDatas.MsgErr = exception.Message.ToString();
                listMemo = new ListMemo();
            }

            return listMemo;
        }

        private List<ObjectiveData> GetObjectiveData()
        {
            List<ObjectiveData> list = (new DAStockPromotion()).GetObjectiveData().AsEnumerable().Select((DataRow t1) => new ObjectiveData()
            {
                ID = (int)t1[0],
                Titles = (string)t1[1],
                Details = (string)t1[2]
            }).ToList();
            return list;
        }

        private List<ProjectsData> GetProjectsData()
        {
            List<ProjectsData> list = (new DAStockPromotion()).GetProjectsData().AsEnumerable().Select((DataRow t1) => new ProjectsData()
            {
                ProjectID = (string)t1.ItemArray[0],
                ProjectName = (string)t1.ItemArray[1]
            }).ToList();
            return list;
        }

        private List<PromotionItemsData> GetPromotionItemsData()
        {
            List<PromotionItemsData> list = (new DAStockPromotion()).GetProdutcDetailsData().AsEnumerable().Select((DataRow t1) => new PromotionItemsData()
            {
                ItemNO = (string)t1.ItemArray[0],
                ItemName = (string)t1.ItemArray[1]
            }).ToList();
            return list;
        }

        private USRTYPE GetUserData(Permission permit)
        {
            DAStockPromotion dAStockPromotion = new DAStockPromotion();
            USRTYPE uSRTYPE = new USRTYPE();
            List<HRType> hRTypes = new List<HRType>();
            List<ETCType> eTCTypes = new List<ETCType>();
            List<AllType> allTypes = new List<AllType>();
            List<LCMType> lCMTypes = new List<LCMType>();
            List<CLevelType> cLevelTypes = new List<CLevelType>();
            List<HeadOfType> headOfTypes = new List<HeadOfType>();
            List<MarketingType> marketingTypes = new List<MarketingType>();
            if (permit.Admin)
            {
                allTypes = (
                    from t1 in dAStockPromotion.GetUserByType(6).AsEnumerable()
                    group t1 by new { ID = t1.Field<string>("EmpCode"), FName = t1.Field<string>("NameTH"), LName = t1.Field<string>("LastNameTH") } into t
                    select new AllType()
                    {
                        ID = t.Key.ID,
                        FName = t.Key.FName,
                        LName = t.Key.LName
                    }).ToList();
            }
            else if (permit.CLevel)
            {
                cLevelTypes = (
                    from t1 in dAStockPromotion.GetUserByType(0).AsEnumerable()
                    group t1 by new { ID = t1.Field<string>("EmpCode"), FName = t1.Field<string>("NameTH"), LName = t1.Field<string>("LastNameTH") } into t
                    select new CLevelType()
                    {
                        ID = t.Key.ID,
                        FName = t.Key.FName,
                        LName = t.Key.LName
                    }).ToList();
                allTypes = (
                    from t1 in dAStockPromotion.GetUserByType(6).AsEnumerable()
                    group t1 by new { ID = t1.Field<string>("EmpCode"), FName = t1.Field<string>("NameTH"), LName = t1.Field<string>("LastNameTH") } into t
                    select new AllType()
                    {
                        ID = t.Key.ID,
                        FName = t.Key.FName,
                        LName = t.Key.LName
                    }).ToList();
            }
            else if (permit.LCM)
            {
                headOfTypes = (
                    from t1 in dAStockPromotion.GetUserByType(1).AsEnumerable()
                    group t1 by new { ID = t1.Field<string>("EmpCode"), FName = t1.Field<string>("NameTH"), LName = t1.Field<string>("LastNameTH") } into t
                    select new HeadOfType()
                    {
                        ID = t.Key.ID,
                        FName = t.Key.FName,
                        LName = t.Key.LName
                    }).ToList();
                allTypes = (
                    from t1 in dAStockPromotion.GetUserByType(6).AsEnumerable()
                    group t1 by new { ID = t1.Field<string>("EmpCode"), FName = t1.Field<string>("NameTH"), LName = t1.Field<string>("LastNameTH") } into t
                    select new AllType()
                    {
                        ID = t.Key.ID,
                        FName = t.Key.FName,
                        LName = t.Key.LName
                    }).ToList();
            }
            else if (permit.HR)
            {
                cLevelTypes = (
                    from t1 in dAStockPromotion.GetUserByType(0).AsEnumerable()
                    group t1 by new { ID = t1.Field<string>("EmpCode"), FName = t1.Field<string>("NameTH"), LName = t1.Field<string>("LastNameTH") } into t
                    select new CLevelType()
                    {
                        ID = t.Key.ID,
                        FName = t.Key.FName,
                        LName = t.Key.LName
                    }).ToList();
                allTypes = (
                    from t1 in dAStockPromotion.GetUserByType(6).AsEnumerable()
                    group t1 by new { ID = t1.Field<string>("EmpCode"), FName = t1.Field<string>("NameTH"), LName = t1.Field<string>("LastNameTH") } into t
                    select new AllType()
                    {
                        ID = t.Key.ID,
                        FName = t.Key.FName,
                        LName = t.Key.LName
                    }).ToList();
            }
            else if (permit.ETC)
            {
                headOfTypes = (
                    from t1 in dAStockPromotion.GetUserByType(1).AsEnumerable()
                    group t1 by new { ID = t1.Field<string>("EmpCode"), FName = t1.Field<string>("NameTH"), LName = t1.Field<string>("LastNameTH") } into t
                    select new HeadOfType()
                    {
                        ID = t.Key.ID,
                        FName = t.Key.FName,
                        LName = t.Key.LName
                    }).ToList();
                allTypes = (
                    from t1 in dAStockPromotion.GetUserByType(6).AsEnumerable()
                    group t1 by new { ID = t1.Field<string>("EmpCode"), FName = t1.Field<string>("NameTH"), LName = t1.Field<string>("LastNameTH") } into t
                    select new AllType()
                    {
                        ID = t.Key.ID,
                        FName = t.Key.FName,
                        LName = t.Key.LName
                    }).ToList();
            }
            else if (permit.MKT)
            {
                allTypes = (
                    from t1 in dAStockPromotion.GetUserByType(3).AsEnumerable()
                    group t1 by new { ID = t1.Field<string>("EmpCode"), FName = t1.Field<string>("NameTH"), LName = t1.Field<string>("LastNameTH") } into t
                    select new AllType()
                    {
                        ID = t.Key.ID,
                        FName = t.Key.FName,
                        LName = t.Key.LName
                    }).ToList();
                marketingTypes = (
                    from t1 in dAStockPromotion.GetUserByType(3).AsEnumerable()
                    group t1 by new { ID = t1.Field<string>("EmpCode"), FName = t1.Field<string>("NameTH"), LName = t1.Field<string>("LastNameTH") } into t
                    select new MarketingType()
                    {
                        ID = t.Key.ID,
                        FName = t.Key.FName,
                        LName = t.Key.LName
                    }).ToList();
            }
            uSRTYPE.UserAllType = allTypes;
            uSRTYPE.UserCLevelType = cLevelTypes;
            uSRTYPE.UserHeadOfType = headOfTypes;
            uSRTYPE.UserLCMType = lCMTypes;
            uSRTYPE.UserETCType = eTCTypes;
            uSRTYPE.UserHRType = hRTypes;
            uSRTYPE.UserMarketingType = marketingTypes;
            return uSRTYPE;
        }

        [WebMethod]
        public static ListRequisitionDetailsData RejectDataRequisition(string rqrno, string reason)
        {
            ListRequisitionDetailsData listRequisitionDetailsDatum;
            try
            {
                listRequisitionDetailsDatum = (!(new DAStockPromotion()).OnCommandRejectRequisition(rqrno, reason) ? new ListRequisitionDetailsData() : GetDataRequisitionSearch("", "", "", "", "", "", "", "", ""));
            }
            catch (Exception)
            {
                listRequisitionDetailsDatum = new ListRequisitionDetailsData();
            }
            return listRequisitionDetailsDatum;
        }

        [WebMethod]
        public static ListRequisitionDetailsData SaveData(List<DataDetails> data)
        {
            ListRequisitionDetailsData listRequisitionDetailsDatum;
            DAStockPromotion dAStockPromotion = new DAStockPromotion();
            try
            {
                string empty = string.Empty;
                for (int i = 0; i < data[0].DRNO.Count; i++)
                {
                    empty = string.Concat(empty, data[0].DRNO[i].DRNO, ", ");
                }
                empty = empty.Substring(0, empty.Length - 2);
                string str = (data[0].RQRNO == "" ? string.Empty : data[0].RQRNO);
                for (int j = 0; j < data[0].dataDetails.Count; j++)
                {
                    List<string> strs = new List<string>()
                    {
                        str,
                        empty,
                        data[0].RCVD,
                        data[0].CURD,
                        data[0].REFREQID,
                        data[0].REQID,
                        data[0].REQN,
                        data[0].PROMOTID,
                        data[0].PROMOTN,
                        data[0].DEPTID,
                        data[0].DEPTN,
                        data[0].GLID,
                        data[0].GLN,
                        data[0].OBJID,
                        data[0].OBJN,
                        data[0].REMK,
                        data[0].dataDetails[j].PROJID,
                        data[0].dataDetails[j].PROJN,
                        data[0].dataDetails[j].PROPID,
                        data[0].dataDetails[j].PROPN,
                        data[0].dataDetails[j].QUANT,
                        data[0].dataDetails[j].STDDT,
                        data[0].dataDetails[j].ENDDT,
                        data[0].dataDetails[j].RMDB,
                        data[0].dataDetails[j].ITEMS,
                        data[0].APPVID,
                        data[0].APPVN,
                        data[0].STATUS,
                        data[0].dataDetails[j].DRNOBYITEM
                    };
                    try
                    {
                        string str1 = HttpContext.Current.Session["Usd_EmployeeID"].ToString();
                        str = dAStockPromotion.OnCommandSaveRequisition(strs, str1);
                    }
                    catch (Exception)
                    {
                        listRequisitionDetailsDatum = new ListRequisitionDetailsData();
                        return listRequisitionDetailsDatum;
                    }
                }
                if (data[0].STATUS != "0")
                {
                    if ((new SendEmail()).Send("อนุสรณ์ นุ่มทองคำ <anusorn_n@apthai.com>;อริยดา ตึกดี <ariyada_t@apthai.com>", "(แจ้งรับงาน) เอกสารตั้งเบิก", GenerateMailBody(str), ""))
                    {
                    }
                }
                listRequisitionDetailsDatum = GetDataRequisitionSearch("", "", "", "", "", "", "", "", "");
            }
            catch (Exception)
            {
                listRequisitionDetailsDatum = new ListRequisitionDetailsData();
            }
            return listRequisitionDetailsDatum;
        }

        [WebMethod]
        public static ListRequisitionDetailsData SetAcceptRequisition(string rqrno)
        {
            ListRequisitionDetailsData listRequisitionDetailsDatum;
            try
            {
                listRequisitionDetailsDatum = (!(new DAStockPromotion()).OnCommandAcceptRequisition(rqrno) ? new ListRequisitionDetailsData() : GetDataRequisitionSearch("", "", "", "", "", "", "", "", ""));
            }
            catch (Exception)
            {
                listRequisitionDetailsDatum = new ListRequisitionDetailsData();
            }
            return listRequisitionDetailsDatum;
        }

        [WebMethod]
        public static ListRequisitionDetailsData SetApproveRequisition(string rqrno)
        {
            ListRequisitionDetailsData listRequisitionDetailsDatum;
            try
            {
                listRequisitionDetailsDatum = (!(new DAStockPromotion()).OnCommandApproveRequisition(rqrno) ? new ListRequisitionDetailsData() : GetDataRequisitionSearch("", "", "", "", "", "", "", "", ""));
            }
            catch (Exception)
            {
                listRequisitionDetailsDatum = new ListRequisitionDetailsData();
            }
            return listRequisitionDetailsDatum;
        }

        [WebMethod]
        public static ListRequisitionDetailsData SetConfirmRecallRequisition(string rqrno)
        {
            ListRequisitionDetailsData listRequisitionDetailsDatum;
            try
            {
                listRequisitionDetailsDatum = (!(new DAStockPromotion()).OnCommandConfirmRecallRequisition(rqrno) ? new ListRequisitionDetailsData() : GetDataRequisitionSearch("", "", "", "", "", "", "", "", ""));
            }
            catch (Exception)
            {
                listRequisitionDetailsDatum = new ListRequisitionDetailsData();
            }
            return listRequisitionDetailsDatum;
        }

        [WebMethod]
        public static ListRequisitionDetailsData SetOpenRecallRequisition(string rqrno)
        {
            ListRequisitionDetailsData listRequisitionDetailsDatum;
            try
            {
                listRequisitionDetailsDatum = (!(new DAStockPromotion()).OnCommandOpenRecallRequisition(rqrno) ? new ListRequisitionDetailsData() : GetDataRequisitionSearch("", "", "", "", "", "", "", "", ""));
            }
            catch (Exception)
            {
                listRequisitionDetailsDatum = new ListRequisitionDetailsData();
            }
            return listRequisitionDetailsDatum;
        }

        [WebMethod]
        public static ListRequisitionDetailsData SetSentRequisition(string rqrno)
        {
            ListRequisitionDetailsData listRequisitionDetailsDatum;
            try
            {
                if (!(new DAStockPromotion()).OnCommandSetSentRequisition(rqrno))
                {
                    listRequisitionDetailsDatum = new ListRequisitionDetailsData();
                }
                else
                {
                    if ((new SendEmail()).Send("thirawuth_s@apthai.com;อริยดา ตึกดี <ariyada_t@apthai.com>;", "(แจ้งรับงาน) เอกสารตั้งเบิก", GenerateMailBody(rqrno), ""))
                    {
                    }
                    listRequisitionDetailsDatum = GetDataRequisitionSearch("", "", "", "", "", "", "", "", "");
                }
            }
            catch (Exception)
            {
                listRequisitionDetailsDatum = new ListRequisitionDetailsData();
            }
            return listRequisitionDetailsDatum;
        }

        [WebMethod]
        public static ListRequisitionDetailsData UpdateData(List<DataDetails> data)
        {
            ListRequisitionDetailsData listRequisitionDetailsDatum;
            DAStockPromotion dAStockPromotion = new DAStockPromotion();
            try
            {
                string str = "";
                string empty = string.Empty;
                for (int i = 0; i < data[0].DRNO.Count; i++)
                {
                    empty = string.Concat(empty, data[0].DRNO[i].DRNO, ", ");
                }
                empty = empty.Substring(0, empty.Length - 2);
                str = (data[0].RQRNO == "" ? string.Empty : data[0].RQRNO);
                if (dAStockPromotion.OnCommandDeleteRequisitionByRQRNO(str))
                {
                    for (int j = 0; j < data[0].dataDetails.Count; j++)
                    {
                        List<string> strs = new List<string>()
                        {
                            str,
                            empty,
                            data[0].RCVD,
                            data[0].CURD,
                            data[0].REFREQID,
                            data[0].REQID,
                            data[0].REQN,
                            data[0].PROMOTID,
                            data[0].PROMOTN,
                            data[0].DEPTID,
                            data[0].DEPTN,
                            data[0].GLID,
                            data[0].GLN,
                            data[0].OBJID,
                            data[0].OBJN,
                            data[0].REMK,
                            data[0].dataDetails[j].PROJID,
                            data[0].dataDetails[j].PROJN,
                            data[0].dataDetails[j].PROPID,
                            data[0].dataDetails[j].PROPN,
                            data[0].dataDetails[j].QUANT,
                            data[0].dataDetails[j].STDDT,
                            data[0].dataDetails[j].ENDDT,
                            data[0].dataDetails[j].RMDB,
                            data[0].dataDetails[j].ITEMS,
                            data[0].APPVID,
                            data[0].APPVN,
                            data[0].STATUS,
                            data[0].dataDetails[j].DRNOBYITEM
                        };

                        try
                        {
                            string str1 = HttpContext.Current.Session["Usd_EmployeeID"].ToString();
                            dAStockPromotion.OnCommandSaveRequisition(strs, str1);
                        }
                        catch (Exception)
                        {
                            listRequisitionDetailsDatum = new ListRequisitionDetailsData();
                            return listRequisitionDetailsDatum;
                        }
                    }
                }

                if (data[0].STATUS != "0")
                {
                    if ((new SendEmail()).Send("อนุสรณ์ นุ่มทองคำ <anusorn_n@apthai.com>;อริยดา ตึกดี <ariyada_t@apthai.com>;weerawat_s@apthai.com;", "(แจ้งรับงาน) เอกสารตั้งเบิก", GenerateMailBody(str), ""))
                    {
                    }
                }

                listRequisitionDetailsDatum = GetDataRequisitionSearch("", "", "", "", "", "", "", "", "");
            }
            catch (Exception)
            {
                listRequisitionDetailsDatum = new ListRequisitionDetailsData();
            }

            return listRequisitionDetailsDatum;
        }

        [WebMethod]
        public static CostCenter GetCostCenterByMemoDoc(string MemoDoc)
        {
            var dt = new DataTable();
            var Model = new CostCenter();

            if (MemoDoc != "")
            {
                dt = (new DARequisition()).GetCostCenterByMemoDoc(MemoDoc);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        Model.CostCenterID = item["CostCenter"].ToString();
                        Model.CostCenterName = item["CostCenterName"].ToString();
                    }
                }
            }

            return Model;
        }


        public class AllType : STDTYPE { public AllType() { } }

        public class CLevelType : STDTYPE { public CLevelType() { } }

        public class CostCenterData
        {
            public string CostCenter { get; set; }
            public string CostCenterName { get; set; }

            public CostCenterData() { }
        }

        public class DataDetails
        {
            public string APPVID { get; set; }

            public string APPVN { get; set; }

            public string CURD { get; set; }

            public List<DataProjectDetails> dataDetails { get; set; }

            public string DEPTID { get; set; }

            public string DEPTN { get; set; }

            public List<DataRefNO> DRNO { get; set; }

            public string GLID { get; set; }

            public string GLN { get; set; }

            public string OBJID { get; set; }

            public string OBJN { get; set; }

            public string PROMOTID { get; set; }

            public string PROMOTN { get; set; }

            public string RCVD { get; set; }

            public string REFREQID { get; set; }

            public string REMK { get; set; }

            public string REQID { get; set; }

            public string REQN { get; set; }

            public string RQRNO { get; set; }

            public string STATUS { get; set; }

            public DataDetails() { }
        }

        public class DataProjectDetails
        {
            public string DRNOBYITEM { get; set; }

            public string ENDDT { get; set; }

            public string ITEMS { get; set; }

            public string MEMOBALANCE { get; set; }

            public string PROJID { get; set; }

            public string PROJN { get; set; }

            public string PROPID { get; set; }

            public string PROPN { get; set; }

            public string QUANT { get; set; }

            public string RMDB { get; set; }

            public string STDDT { get; set; }

            public string STOCKBALANCE { get; set; }

            public string TYPE { get; set; }

            public DataProjectDetails() { }
        }

        public class DataRefNO
        {
            public string DRNO { get; set; }

            public DataRefNO() { }
        }

        public class DefaultUser
        {
            public string CostCenterCode { get; set; }
            public string CostCenterName { get; set; }

            public string EmpCode { get; set; }

            public string FName { get; set; }

            public string LeaderEmail { get; set; }

            public string LeaderFName { get; set; }

            public string LeaderID { get; set; }

            public string LeaderLName { get; set; }

            public string LName { get; set; }

            public string UserEmail { get; set; }

            public string UserGUID { get; set; }

            public DefaultUser() { }
        }

        public class ETCType : STDTYPE { public ETCType() { } }

        public class GLData
        {
            public string GLDESC { get; set; }

            public string GLID { get; set; }

            public GLData() { }
        }

        public class HeadOfType : STDTYPE { public HeadOfType() { } }

        public class HRType : STDTYPE { public HRType() { } }

        public class ItemsMemo
        {
            public string DOCNO { get; set; }

            public string ITEMNAME { get; set; }

            public string ITEMNO { get; set; }

            public string MEMOBALANCE { get; set; }

            public string QUANTITY { get; set; }

            public string STOCKBALANCE { get; set; }

            public string TYPE { get; set; }

            public ItemsMemo() { }
        }

        public class ITEMSQUANTITY
        {
            public string MEMOBALANCE { get; set; }

            public string STOCKBALANCE { get; set; }

            public string TYPE { get; set; }

            public ITEMSQUANTITY() { }
        }

        public class LCMType : STDTYPE { public LCMType() { } }

        public class ListItemsMemo
        {
            public List<ItemsMemo> ItemsMemo { get; set; }

            public string MsgErr { get; set; }

            public ListItemsMemo() { }
        }

        public class ListMemo
        {
            public List<MemoData> LstMemoData { get; set; }

            public string MsgErr
            {
                get;
                set;
            }

            public ListMemo()
            {
            }
        }

        public class ListRequisitionDetailsData
        {
            public List<RequisitionDetailsData> lstrr
            {
                get;
                set;
            }

            public string MsgErr
            {
                get;
                set;
            }

            public ListRequisitionDetailsData()
            {
            }
        }

        public class MarketingType : STDTYPE
        {
            public MarketingType()
            {
            }
        }

        public class MemoDocNo
        {
            public MemoDocNo() { MemoDoc = ""; }

            public string MemoDoc { get; set; }
        }

        public class MemoData
        {
            public string MemoID { get; set; }

            public MemoData() { }
        }

        public class ObjectiveData
        {
            public string Details { get; set; }

            public int ID { get; set; }

            public string Titles { get; set; }

            public ObjectiveData() { }
        }

        public class Permission
        {
            public bool Admin
            {
                get;
                set;
            }

            public bool CLevel
            {
                get;
                set;
            }

            public bool ETC
            {
                get;
                set;
            }

            public bool HeadOf
            {
                get;
                set;
            }

            public bool HR
            {
                get;
                set;
            }

            public bool LCM
            {
                get;
                set;
            }

            public bool MKT
            {
                get;
                set;
            }

            public bool Other
            {
                get;
                set;
            }

            public List<DefaultUser> UserDetails { get; set; }

            public Permission()
            {
            }
        }

        public class ProjectsData
        {
            public string ProjectID { get; set; }

            public string ProjectName { get; set; }

            public ProjectsData()
            {
            }
        }

        public class PromotionItemsData
        {
            public string ItemName { get; set; }

            public string ItemNO { get; set; }

            public PromotionItemsData()
            {
            }
        }

        public class RequisitionDetailsData
        {
            public string CURD
            {
                get;
                set;
            }

            public string DRNO
            {
                get;
                set;
            }

            public string ENDDT
            {
                get;
                set;
            }

            public string REFREQID
            {
                get;
                set;
            }

            public string REQID
            {
                get;
                set;
            }

            public string REQN
            {
                get;
                set;
            }

            public string RQRNO
            {
                get;
                set;
            }

            public string STATUS
            {
                get;
                set;
            }

            public string STDDT
            {
                get;
                set;
            }

            public RequisitionDetailsData()
            {
            }
        }

        public class STDTYPE
        {
            public string FName { get; set; }

            public string ID { get; set; }

            public string LName { get; set; }

            public STDTYPE()
            {
            }
        }

        public class UserDetail
        {
            public static string Email
            {
                get;
                set;
            }

            public static string EmployeeID
            {
                get;
                set;
            }

            public static string FirstName
            {
                get;
                set;
            }

            public static string LastName
            {
                get;
                set;
            }

            public static string Mode
            {
                get;
                set;
            }

            public static string Source
            {
                get;
                set;
            }

            public UserDetail()
            {
            }
        }

        public class USRTYPE
        {
            public List<AllType> UserAllType { get; set; }

            public List<CLevelType> UserCLevelType { get; set; }

            public List<ETCType> UserETCType { get; set; }

            public List<HeadOfType> UserHeadOfType { get; set; }

            public List<HRType> UserHRType { get; set; }

            public List<LCMType> UserLCMType { get; set; }

            public List<MarketingType> UserMarketingType { get; set; }

            public USRTYPE()
            {
            }
        }
    }
}