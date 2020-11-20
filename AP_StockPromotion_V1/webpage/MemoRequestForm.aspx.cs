using AP_StockPromotion_V1.Class;
using AP_StockPromotion_V1.Models;
using AP_StockPromotion_V1.ws_authorize;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using static AP_StockPromotion_V1.Class.DAStockPromotion;

namespace AP_StockPromotion_V1.webpage
{
    public partial class MemoRequestForm : Page
    {
        string msg = "";
        string WebRoot = ConfigurationManager.AppSettings["WebRoot"].ToString();
        string LoginWebRoot = ConfigurationManager.AppSettings["Application.Settings.LoginPage"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string guid = "";
                string action = "";

                string[] source = ((Page)sender).ClientQueryString.Split('&');

                if (source.Count() > 2)
                {
                    if (source[0].Split('=')[1] == "RequestStatus")
                    {
                        if (Session["ByPass"] != null)
                        {
                            Session.Remove("ByPass");
                        }

                        Session["ByPass"] = "ByPass";

                        guid = source[1].Split('=')[1];
                        action = source[2].Split('=')[1];

                        switch (source[2].Split('=')[1])
                        {
                            case "0":
                                RejectStatus(guid, action, source, ref msg);
                                break;
                            case "1":
                                RequestStatus(guid, action, source, ref msg);
                                break;
                        }
                    }
                }

                //if ((guId ?? "") != "")
                //{
                //    WSAuthorizeModel userLogin = new Class.WSAuthorizeModel();
                //    DAStockPromotion da = new DAStockPromotion();
                //    DataTable dt = new DataTable();
                //    string msgErr = "";
                //    dt = da.getUserAuth(guId, "", "");
                //    if (dt != null)
                //    {
                //        if (dt.Rows.Count > 0)
                //        {
                //            LoginX LoginPage = new LoginX();
                //            if (!LoginPage.UserLogin("", "", ref userLogin, dt.Rows[0]["UserNameLogin"].ToString(), ref msgErr))
                //            {
                //                return;
                //            }
                //            else
                //            {
                //                if (Cache["URLFromEmail"] != null)
                //                {
                //                    Response.Redirect(Cache["URLFromEmail"].ToString());
                //                }
                //                //else
                //                //{
                //                //    string originalPath = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
                //                //    Response.Redirect(originalPath + "webpage/Default.aspx");
                //                //}

                //            }
                //        }
                //    }
                //}

                if (Session["ByPass"] != null)
                {
                    DAStockPromotion daSP = new DAStockPromotion();

                    DataTable dt = daSP.GetUserByGUID(guid);
                    if (dt.Rows.Count > 0)
                    {
                        Session["FirstName"] = dt.Rows[0]["FirstName"].ToString();
                        Session["LastName"] = dt.Rows[0]["LastName"].ToString();
                        Session["EmpId"] = dt.Rows[0]["EmpCode"].ToString();
                    }
                    else
                    {
                        Session["FirstName"] = "";
                        Session["LastName"] = "";
                        Session["EmpId"] = "";
                    }
                    return;

                }

                if (Session["userInfo_" + Session.SessionID] == null) return;
                AutorizeData auth = new AutorizeData();
                auth = (AutorizeData)Session["userInfo_" + Session.SessionID];

                if (auth.EmployeeID != null)
                {
                    var authUser = JObject.Parse(auth.SysUserData);
                    var muser = JsonConvert.DeserializeObject<Models.User>(authUser["User"].ToString());

                    Session["FirstName"] = muser.FirstName;
                    Session["LastName"] = muser.LastName;
                    Session["EmpId"] = auth.EmployeeID;

                    var roleCode = JsonConvert.DeserializeObject<dynamic>(auth.SysUserRoles);
                    foreach (dynamic roles in roleCode.Roles)
                    {
                        switch ((string)roles.RoleCode)
                        {
                            case "Admin":
                                Session["RoleCode"] = null;
                                Session["RoleCode"] = "Admin";
                                break;
                            case "AdminCenter":
                                Session["RoleCode"] = null;
                                Session["RoleCode"] = "Admin";
                                break;
                            case "CLevel":
                                Session["RoleCode"] = null;
                                Session["RoleCode"] = "CLevel";
                                break;
                            case "HeadOf":
                                Session["RoleCode"] = null;
                                Session["RoleCode"] = "HeadOf";
                                break;
                            case "LCM":
                                Session["RoleCode"] = null;
                                Session["RoleCode"] = "LCM";
                                break;
                            case "HR":
                                Session["RoleCode"] = null;
                                Session["RoleCode"] = "HR";
                                break;
                            case "ETC":
                                Session["RoleCode"] = null;
                                Session["RoleCode"] = "ETC";
                                break;
                            case "MKT":
                                Session["RoleCode"] = null;
                                Session["RoleCode"] = "MKT";
                                break;
                            case "BG":
                                Session["RoleCode"] = null;
                                Session["RoleCode"] = "BG";
                                break;
                            case "ACC":
                                Session["RoleCode"] = null;
                                Session["RoleCode"] = "ACC";
                                break;
                            //case "AdminEService":
                            //    Session["RoleCode"] = null;
                            //    Session["RoleCode"] = "Admin";
                            //    break;
                            case "CFI":
                                Session["RoleCode"] = null;
                                Session["RoleCode"] = "CFI";
                                break;
                                //default:
                                //    Session["RoleCode"] = null;
                                //    Session["RoleCode"] = "OTH";
                                //    break;
                        }
                    }
                    var user = JsonConvert.DeserializeObject<UserData>(auth.SysUserData);
                    Session["UserGUID"] = user.User.UserGUID;

                }
                else
                {
                    Cache["URLFromEmail"] = Request.Url.ToString();
                    Session["userInfo_" + Session.SessionID] = null;
                }
            }
        }

        [WebMethod]
        public static object[] CheckingPR(string costCenterNo, string totalPrice, string docType, string docNo)
        {
            try
            {
                string MsgErr = "";

                bool isHasPR = false;

                DAStockPromotion dastp = new DAStockPromotion();
                if (dastp.CheckingPR(costCenterNo, totalPrice, docType, docNo, out MsgErr)) isHasPR = true;
                return new object[]
                {
                    isHasPR,
                    MsgErr
                };
            }
            catch (Exception ex)
            {
                return new object[]
                {
                    false,
                    ex.Message.ToString()
                };
            }
        }

        [WebMethod]
        public static string GetFullName()
        {
            string EmpId = HttpContext.Current.Session["EmpId"].ToString();
            string FirstName = HttpContext.Current.Session["FirstName"].ToString();
            string LastName = HttpContext.Current.Session["LastName"].ToString();

            DAStockPromotion dasp = new DAStockPromotion();
            DataTable dtCostCenter = dasp.GetCostCenter(EmpId) ?? new DataTable();

            string Costcenter = "";
            if (dtCostCenter.Rows.Count > 0)
            {
                Costcenter = dtCostCenter.Rows[0].ItemArray[0].ToString() + ":" + dtCostCenter.Rows[0].ItemArray[1].ToString();
            }

            return EmpId + ":" + FirstName + ":" + LastName + ":" + Costcenter;
        }

        [WebMethod]
        public static List<Itmes> GetItems()
        {
            try
            {
                DAStockPromotion dasp = new DAStockPromotion();
                var lstItems = (from t1 in dasp.GetItems().AsEnumerable()
                                select new Itmes
                                {
                                    ItemNo = t1.ItemArray[0].ToString(),
                                    ItemName = t1.ItemArray[1].ToString(),
                                    UnitPrice = Convert.ToDecimal(t1.ItemArray[2]),
                                    ItemUnit = t1.ItemArray[3].ToString()
                                }).ToList();
                return lstItems;
            }
            catch (Exception)
            {
                return new List<Itmes>();
            }
        }

        [WebMethod]
        public static List<Itmes> GetAllMatItems()
        {
            try
            {
                DAStockPromotion dasp = new DAStockPromotion();
                var lstItems = (from t1 in dasp.GetAllMatItems().AsEnumerable()
                                select new Itmes
                                {
                                    ItemNo = t1.ItemArray[0].ToString(),
                                    ItemName = t1.ItemArray[1].ToString(),
                                    UnitPrice = Convert.ToDecimal(t1.ItemArray[2]),
                                    ItemUnit = t1.ItemArray[3].ToString()
                                }).ToList();
                return lstItems;
            }
            catch (Exception)
            {
                return new List<Itmes>();
            }
        }

        [WebMethod]
        public static ListRetMemoData SaveMemoItems(int savemode, List<MemoItems> DataValue, string EmpCode)
        {
            try
            {
                List<MemoReturnData> ret = null;
                DAStockPromotion dasp = new DAStockPromotion();
                string docno;
                if (dasp.SaveMemoItems(savemode, DataValue, EmpCode, out docno))
                {
                    ret = GetDataMemo("", "", "", "");
                }

                ListRetMemoData lstMemoData = new ListRetMemoData();
                lstMemoData.RetMemoData = ret;
                lstMemoData.DOCNO = docno;
                return lstMemoData;
            }
            catch (Exception)
            {
                return new ListRetMemoData();
            }
        }

        [WebMethod]
        public static List<MemoReturnData> GetDataMemo(string DocNo, string StdDate, string EndDate, string status)
        {
            try
            {
                string RoleCode = (HttpContext.Current.Session["RoleCode"] ?? "").ToString() == "" ? "OTH" : HttpContext.Current.Session["RoleCode"].ToString();

                string EmpId = HttpContext.Current.Session["EmpId"].ToString();

                DAStockPromotion dasp = new DAStockPromotion();
                var res = (from t1 in dasp.GetDataMemo(DocNo, StdDate, EndDate, status, (RoleCode == "Admin" ? "" : EmpId)).AsEnumerable()
                           select new MemoReturnData
                           {
                               DOCNO = t1.ItemArray[0].ToString(),
                               MEMOCREATEDATE = t1.ItemArray[1].ToString(),
                               CREATORNAME = t1.ItemArray[2].ToString(),
                               CCNAME = t1.ItemArray[3].ToString(),
                               REASON = t1.ItemArray[4].ToString(),
                               STATUSNAME = t1.ItemArray[5].ToString(),
                               REJECTREASON = t1.ItemArray[6].ToString()
                           }).ToList();

                return res;
            }

            catch (Exception)
            {
                return new List<MemoReturnData>();
            }
        }

        [WebMethod]
        public static List<MemoItems> GetDataMemoByDocID(string DocNo)
        {
            try
            {
                DAStockPromotion dasp = new DAStockPromotion();

                var res = (from t1 in dasp.GetDataMemoByDocID(DocNo).AsEnumerable()
                           group t1 by new
                           {
                               DOCTYPE = t1.Field<string>("DOCTYPE"),
                               DOCNO = t1.Field<string>("DOCNO"),
                               MEMOCREATEDATE = t1.Field<DateTime>("MEMOCREATEDATE"),
                               USEDATE = t1.Field<DateTime>("USEDATE"),
                               ENDDATE = t1.Field<DateTime>("ENDDATE"),
                               EMPCODE = t1.Field<string>("EMPCODE"),
                               CREATORNAME = t1.Field<string>("CREATORNAME"),
                               CCCODE = t1.Field<string>("CCCODE"),
                               CCNAME = t1.Field<string>("CCNAME"),
                               PROMOID = t1.Field<int>("PROMOID"),
                               PROMONAME = t1.Field<string>("PROMONAME"),
                               GLNO = t1.Field<string>("GLNO"),
                               GLNAME = t1.Field<string>("GLNAME"),
                               OBJID = t1.Field<int?>("OBJID"),
                               OBJNAME = t1.Field<string>("OBJNAME"),
                               STATUS = t1.Field<int?>("STATUS"),
                               REASON = t1.Field<string>("REASON")
                           } into t
                           select new MemoItems
                           {
                               DocType = t.Key.DOCTYPE,
                               DocNo = t.Key.DOCNO,
                               CreateDate = t.Key.MEMOCREATEDATE.ToShortDateString(),
                               UsingDate = t.Key.USEDATE.ToShortDateString(),
                               EndingDate = t.Key.ENDDATE.ToShortDateString(),
                               EmpCode = t.Key.EMPCODE,
                               UserCreateName = t.Key.CREATORNAME,
                               CostCenterCode = t.Key.CCCODE,
                               CostCenterName = t.Key.CCNAME,
                               PromotionID = t.Key.PROMOID.ToString(),
                               Promotion = t.Key.PROMONAME,
                               GLNO = t.Key.GLNO,
                               GLName = t.Key.GLNAME,
                               ObjID = t.Key.OBJID.ToString(),
                               ObjName = t.Key.OBJNAME,
                               Status = t.Key.STATUS.ToString(),
                               Reason = t.Key.REASON,
                               DataItemsValue = (from t1 in t
                                                 group t1 by new
                                                 {
                                                     ProjectId = t1.Field<string>("PROJECTID"),
                                                     ProjectName = t1.Field<string>("PROJECTNAME"),
                                                     ItemNo = t1.Field<string>("ITEMNO"), //.ItemArray[8].ToString(),
                                                     ItemName = t1.Field<string>("ITEMNAME"),
                                                     PricePerUnit = t1.Field<decimal>("PRICEPERUNIT").ToString("#,###.00"),
                                                     Quantity = t1.Field<int>("QUANTITY").ToString(),
                                                     Type = t1.Field<string>("TYPE"),
                                                     TotalPrice = t1.Field<decimal>("TOTALPRICE").ToString("#,###.00"),
                                                 } into tt
                                                 select new ListItems
                                                 {
                                                     ProjectId = tt.Key.ProjectId,
                                                     ProjectName = tt.Key.ProjectName,
                                                     ItemNo = tt.Key.ItemNo,
                                                     ItemName = tt.Key.ItemName,
                                                     PricePerUnit = tt.Key.PricePerUnit,
                                                     Quantity = tt.Key.Quantity,
                                                     Type = tt.Key.Type,
                                                     TotalPrice = tt.Key.TotalPrice,
                                                 }).ToList(),
                               ApproveEmail = (from t2 in t
                                               group t2 by new
                                               {
                                                   APPVNAME = t2.Field<string>("F1"),
                                                   APPVEMAIL = t2.Field<string>("F1EMAIL"),
                                                   AUTHNAME = t2.Field<string>("F2"),
                                                   AUTHEMAIL = t2.Field<string>("F2EMAIL"),
                                               } into c
                                               select new ListEmail
                                               {
                                                   ApproveName = c.Key.APPVNAME,
                                                   ApproveEmail = c.Key.APPVEMAIL,
                                                   AuthorityName = c.Key.AUTHNAME ?? "",
                                                   AuthorityEmail = c.Key.AUTHEMAIL ?? "",
                                               }).ToList()
                           }).ToList();

                return res;
            }
            catch (Exception)
            {
                return new List<MemoItems>();
            }
        }

        [WebMethod]
        public static string GenerateReport(string docno, string roleCode)
        {
            frmReport _frmRpt = new frmReport();
            string type = "";

            DAStockPromotion dasp = new DAStockPromotion();
            type = dasp.GetDocMemoType(docno);

            return (type == "M-TYPE" ? _frmRpt.ReportMemoRequest(docno) : _frmRpt.ReportMemoRequestNoneMKT(docno));
        }

        [WebMethod]
        public static List<User> GetUser()
        {
            try
            {
                DAStockPromotion dasp = new DAStockPromotion();
                var res = (from t1 in dasp.GetUserEmail().AsEnumerable()
                           select new User
                           {
                               UserName = t1.ItemArray[1].ToString() + " " + t1.ItemArray[2].ToString(),
                               UserEmail = t1.ItemArray[3].ToString()
                           }).ToList();
                return res;
            }
            catch (Exception)
            {
                return new List<User>();
            }
        }

        [WebMethod]
        public static string GetUserPositionByDocNo(string docno)
        {
            try
            {
                DAStockPromotion dasp = new DAStockPromotion();
                string res = dasp.GetUserPositionByDocNo(docno);
                return res;
            }
            catch (Exception)
            {
                return "";
            }
        }

        [WebMethod]
        public static string SendEmailToApprover(string Approver, string to, string subject, string body,
                                         string requeststr, string attachlink, string fromname, string position,
                                         string tel, string docno)
        {
            try
            {
                DAStockPromotion dasp = new DAStockPromotion();

                DataTable dtDataMemoReques = dasp.GetDataMemoByDocID(docno) ?? new DataTable();

                string APVID = (dtDataMemoReques.Rows[0]["APVID"] ?? 0).ToString();

                string ApproverEmail = "";
                string ApproverGUID = "";

                if (APVID != "")
                {
                    ApproverEmail = dasp.GetUserEmailByuserID(int.Parse(APVID)) ?? "";
                    ApproverGUID = dasp.GetUserGUIDByUserId(APVID) ?? "";
                }

                if (dtDataMemoReques.Rows.Count > 0 && ApproverGUID != "")
                {
                    if (ApproverEmail != "")
                    {
                        UserByMemoDo res = (from t1 in dasp.GetUserByMemoDocNo(docno).AsEnumerable()
                                            select new UserByMemoDo
                                            {
                                                REASON = t1.ItemArray[0].ToString(),
                                                DOCNO = t1.ItemArray[1].ToString(),
                                                MEMOCREATEDATE = t1.ItemArray[2].ToString(),
                                                CREATORNAME = t1.ItemArray[3].ToString(),
                                                Email = t1.ItemArray[4].ToString(),
                                                PositionName = t1.ItemArray[5].ToString(),
                                                PhoneExt = t1.ItemArray[6].ToString(),
                                            }).FirstOrDefault();

                        string approvelink, rejectlink;
                        approvelink = ConfigurationManager.AppSettings["WebRoot"].ToString()
                                        + "/webpage/MemoRequestForm.aspx?method=RequestStatus" + "&guid="
                                        + ApproverGUID
                                        + "&action=1&docno=" + docno;
                        rejectlink = ConfigurationManager.AppSettings["WebRoot"].ToString()
                                        + "/webpage/MemoRequestForm.aspx?method=RequestStatus" + "&guid="
                                        + ApproverGUID
                                        + "&action=0&docno=" + docno;

                        string uri = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + attachlink.Replace("../", "").Replace("/", "\\");

                        string currUri = HttpContext.Current.Request.Url.AbsoluteUri;
                        string imessage = body + string.Format(requeststr, approvelink, rejectlink, "<p>โปรดอนุมัติ</p>", fromname, position, tel);

                        string TestSendMail = ConfigurationManager.AppSettings["TestSendMail"].ToString();
                        string[] DevMail = ConfigurationManager.AppSettings["DeveloperEmail"].ToString().Split(';');

                        string mailto = to.Contains(ApproverEmail) ? to : (ApproverEmail + ";" + to);

                        string[] mailToStr = mailto.Split(';');

                        if (TestSendMail == "T")
                        {
                            mailToStr = DevMail;
                            imessage += "<br/><br/>" + mailto;
                            //imessage += "<br/>" + uri;
                        }

                        string[] mailCCStr = new string[] { "" };
                        string[] mailBccStr = DevMail;
                        string msg = "";
                        classMail clsmail = new classMail();
                        clsmail.sendMultiEmail(
                                         "Stock-Promotion@Apthai.com", // From
                                         mailToStr, //to
                                         mailCCStr, //CC
                                         mailBccStr, //BCC
                                         "(โปรดอนุมัติ) " + subject, //Subject
                                         imessage, //body
                                         uri, //attach file
                                         ref msg);
                        if (msg == "")
                        {
                            dasp.UpdateSendForApproveMemoRequest(docno, Approver);
                        }

                        return msg;
                    }
                    else
                    {
                        return "Error : ไม่พบอีเมล์ของผู้อนุมัติ UserID : " + APVID;
                    }
                }
                else
                {
                    return "Error : ไม่พบข้อมูลจาก DocNo. : " + docno;
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        [WebMethod]
        public static string SendEmailToSubApprover(string to, string subject, string body,
                                                    string requeststr, string attachlink, string fromname, string position,
                                                    string tel, string docno)
        {
            try
            {
                DAStockPromotion dasp = new DAStockPromotion();

                DataTable dtDataMemoReques = dasp.GetDataMemoByDocID(docno) ?? new DataTable();

                if (dtDataMemoReques.Rows.Count > 0)
                {
                    string SIGNID = (dtDataMemoReques.Rows[0]["SIGNID"] ?? 0).ToString();

                    string SignerEmail = "";

                    if (SIGNID != "")
                    {
                        SignerEmail = dasp.GetUserEmailByuserID(int.Parse(SIGNID)); // get Approver Mail
                    }

                    if (SignerEmail.Trim() != "" || to.Trim() != "")
                    {
                        UserByMemoDo res = (from t1 in dasp.GetUserByMemoDocNo(docno).AsEnumerable()
                                            select new UserByMemoDo
                                            {
                                                REASON = t1.ItemArray[0].ToString(),
                                                DOCNO = t1.ItemArray[1].ToString(),
                                                MEMOCREATEDATE = t1.ItemArray[2].ToString(),
                                                CREATORNAME = t1.ItemArray[3].ToString(),
                                                Email = t1.ItemArray[4].ToString(),
                                                PositionName = t1.ItemArray[5].ToString(),
                                                PhoneExt = t1.ItemArray[6].ToString(),
                                            }).FirstOrDefault();


                        string uri = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + attachlink.Replace("../", "").Replace("/", "\\");

                        string currUri = HttpContext.Current.Request.Url.AbsoluteUri;

                        string imessage = body + string.Format(requeststr, "<p>แจ้งเพื่อทราบ</p>", fromname, position, tel);

                        //imessage += "<br/>" + uri;

                        string TestSendMail = ConfigurationManager.AppSettings["TestSendMail"].ToString();
                        string[] DevMail = ConfigurationManager.AppSettings["DeveloperEmail"].ToString().Split(';');

                        string[] mailToStr = (TestSendMail == "T") ? DevMail : (SignerEmail + ";" + to).Split(';');
                        string[] mailCCStr = new string[] { "" };
                        string[] mailBccStr = DevMail;

                        string msg = "";
                        classMail clsmail = new classMail();
                        clsmail.sendMultiEmail(
                                         "Stock-Promotion@Apthai.com", // From
                                         mailToStr, //to   resEmail
                                         mailCCStr, //CC
                                         mailBccStr, //BCC
                                         "(แจ้งเพื่อทราบ) " + subject, //Subject
                                         imessage, //body
                                         uri, //attach file
                                         ref msg);
                        return msg;
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "Error : ไม่พบข้อมูลจาก DocNo. : " + docno;
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        [WebMethod]
        public bool CheckStatusBeforeApprove(string docno)
        {
            try
            {
                DAStockPromotion dasp = new DAStockPromotion();
                return dasp.CheckStatusBeforeApprove(docno);
            }

            catch (Exception)

            {
                return false;
            }
        }

        [WebMethod]
        public static bool DeleteMemoRequest(string docno)
        {
            try
            {
                DAStockPromotion dasp = new DAStockPromotion();
                return dasp.DeleteMemoRequest(docno);
            }

            catch (Exception)

            {
                return false;
            }
        }

        [WebMethod]
        public bool UpdateApproveMemoRequest(string docno)
        {
            try
            {
                DAStockPromotion dasp = new DAStockPromotion();
                dasp.spUpdateApproveMemoRequest(docno);
                return true;
            }

            catch (Exception)

            {
                return false;
            }
        }

        private bool CreateRequisitionRequest(string memoDocNo, ref bool IsMkt)
        {
            try
            {
                DAStockPromotion dasp = new DAStockPromotion();
                dasp.spAutoGenerateRequisition(memoDocNo, ref IsMkt);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [WebMethod]
        public static bool UpdateRejectreason(string docno, string reason)
        {
            try
            {
                DAStockPromotion dasp = new DAStockPromotion();
                dasp.UpdateRejectReason(docno, reason);
                return true;
            }

            catch (Exception)

            {
                return false;
            }
        }

        [WebMethod]
        public bool CheckStatusBeforeReject(string docno)
        {
            try
            {
                DAStockPromotion dasp = new DAStockPromotion();
                return dasp.CheckStatusBeforeReject(docno);
            }

            catch (Exception)

            {
                return false;
            }
        }

        [WebMethod]
        public bool UpdateRejectMemoRequest(string docno)
        {
            try
            {
                DAStockPromotion dasp = new DAStockPromotion();
                dasp.UpdateRejectMemoRequest(docno);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private int GetMemoStatus(string docno)
        {
            try
            {
                DAStockPromotion dasp = new DAStockPromotion();
                return dasp.GetMemoStatus(docno);
            }

            catch (Exception)
            {
                return 0;
            }
        }

        private static string GenerateMailBody(string rqrno)
        {
            string rstBody = "";

            rstBody += "<strong><span style=\"color:#4b0082;font-size:14px\">ถึงผู้ที่เกี่ยวข้อง,</span></strong><br /><br />";
            rstBody += "<span style=\"color:#800080;font-size:12px\">&nbsp;&nbsp; ระบบ Stock Promotion มีใบบันทึกขอเบิกใบใหม่เลขที่ " + rqrno + " เพื่อรอการตรวจสอบ และรับทราบ</span><br />";
            rstBody += "<span style=\"color:#800080;font-size:12px\">&nbsp;&nbsp;คุณสามารถเข้าไปตรวจสอบได้ตามลิ้งด้านล่างนี้</span><br />";
            rstBody += "<span style=\"color:#800080;font-size:12px\">&nbsp;&nbsp;Program :</span> <strong><a href=\"http://192.168.0.152/apstp/loginx.aspx\">";
            rstBody += "<span style=\"color:#0000cd;font-size:12px\"> Stock Promotion </span></a></strong><br /><br />";
            rstBody += "<strong><span style=\"color:#4b0082;font-size:14px\"> ขอแสดงความนับถือ</span></strong><br />";
            rstBody += "<strong><span style=\"color:#4b0082;font-size:14px\"> ระบบ Stock Promotion AP </span></strong>";

            return rstBody;
        }

        public bool RequestStatus(string guid, string action, string[] source, ref string msg)
        {
            DAStockPromotion dasp = new DAStockPromotion();

            string _msg = "";
            bool valid = true;

            if (Session["ByPass"] == null && Session["userInfo_" + Session.SessionID] == null)
            {
                WSAuthorizeModel userLogin = new WSAuthorizeModel();
                UserLoginByGUID(guid, ref userLogin, ref _msg);
            }

            if (_msg == "")
            {
                int status = GetMemoStatus(source[3].Split('=')[1]);
                if (status != 3 && status != 4)
                {
                    if (UpdateApproveMemoRequest(source[3].Split('=')[1]))
                    {
                        string TestSendMail = ConfigurationManager.AppSettings["TestSendMail"].ToString();
                        string DevMail = (TestSendMail == "T") ? ConfigurationManager.AppSettings["DeveloperEmail"].ToString() : "";

                        dasp.SendMailResponseApproved(source[3].Split('=')[1], DevMail, ref _msg);

                        bool IsMkt = true;
                        if (CreateRequisitionRequest(source[3].Split('=')[1], ref IsMkt))
                        {
                            //อนุมัติใบเบิก

                            //if (IsMkt)
                            //{
                            //    SendEmail sem = new SendEmail();
                            //    string recipients = "ฐิรวุฒิ สุวรรณศรี <thirawuth_s@apthai.com>;อริยดา ตึกดี <ariyada_t@apthai.com>";
                            //    string mailSubject = "(แจ้งรับงาน) ใบบันทึกขอเบิก (MKT)";
                            //    string mailbody = GenerateMailBody(source[3].Split('=')[1]);
                            //    string mailAttachPath = "";
                            //    if (sem.Send(recipients, mailSubject, mailbody, mailAttachPath))
                            //    {
                            //        //do something
                            //    }
                            //}
                            //else
                            //{
                            //    SendEmail sem = new SendEmail();
                            //    string recipients = "ฐิรวุฒิ สุวรรณศรี <thirawuth_s@apthai.com>;อริยดา ตึกดี <ariyada_t@apthai.com>";
                            //    string mailSubject = "(แจ้งรับงาน) เอกสารตั้งเบิก (Other)";
                            //    string mailbody = GenerateMailBody(source[3].Split('=')[1]);
                            //    string mailAttachPath = "";
                            //    if (sem.Send(recipients, mailSubject, mailbody, mailAttachPath))
                            //    {
                            //        //do something
                            //    }
                            //}


                            if (action == "Bypass")
                            {
                                msg = "อนุมัติ สำเร็จ!";
                            }
                            else
                            {
                                //ScriptManager.RegisterStartupScript(
                                //this, GetType(), "js", "alert('" + source[3].Split('=')[1] + " : อนุมัติ สำเร็จ!');"
                                //+ LoginWebRoot + WebRoot + "/LoginX.aspx';", true);

                                ScriptManager.RegisterStartupScript(
                                this, GetType(), "js",
                                "alert('" + source[3].Split('=')[1] + " : อนุมัติ สำเร็จ!');"
                                + "window.location =  '" + LoginWebRoot + WebRoot + "/LoginX.aspx';", true);
                            }
                        }
                    }
                }
                else
                {
                    if (action == "Bypass")
                    {
                        msg = "รายการนี้ถูก อนุมัติแล้ว!<br />ไม่สามารถทำรายการซ้ำได้ : รายการนี้ถูก ปฎิเสทการอนุมัติแล้ว!<br />ไม่สามารถทำรายการซ้ำได้";
                    }
                    else
                    {
                        if (GetMemoStatus(source[3].Split('=')[1]) == 3)
                        {
                            msg = "รายการนี้ถูก อนุมัติแล้ว!";
                        }
                        else
                        {
                            msg = "รายการนี้ถูก ปฎิเสทการอนุมัติแล้ว";
                        }

                        //ScriptManager.RegisterStartupScript(
                        //    this, GetType(), "js", "alert('" + source[3].Split('=')[1] + " : ไม่สามารถทำรายการซ้ำได้ เนื่องจาก " + msg + "');"
                        //    + WebRoot + "/LoginX.aspx';", true);

                        ScriptManager.RegisterStartupScript(
                        this, GetType(), "js",
                        "alert('" + source[3].Split('=')[1] + " : ไม่สามารถทำรายการซ้ำได้ เนื่องจาก " + msg + "');"
                        + "window.location =  '" + LoginWebRoot + WebRoot + "/LoginX.aspx';", true);

                    }
                    valid = false;
                }
            }
            else
            {
                msg = _msg;
                valid = false;
            }

            return valid;
        }

        private bool RejectStatus(string guid, string action, string[] source, ref string msg)
        {
            DAStockPromotion dasp = new DAStockPromotion();
            string _msg = "";
            bool valid = true;

            if (Session["ByPass"] == null && Session["userInfo_" + Session.SessionID] == null)
            {
                WSAuthorizeModel userLogin = new WSAuthorizeModel();
                UserLoginByGUID(guid, ref userLogin, ref _msg);
            }

            if (_msg == "")
            {
                int status = GetMemoStatus(source[3].Split('=')[1]);
                if (status != 3 && status != 4)
                {
                    if (UpdateRejectMemoRequest(source[3].Split('=')[1]))
                    {

                        string TestSendMail = ConfigurationManager.AppSettings["TestSendMail"].ToString();
                        string DevMail = (TestSendMail == "T") ? ConfigurationManager.AppSettings["DeveloperEmail"].ToString() : "";

                        dasp.SendMailResponseApproved(source[3].Split('=')[1], DevMail, ref _msg);

                        if (action == "Bypass")
                        {
                            msg = "ไม่อนุมัติ!";
                        }
                        else
                        {

                            ScriptManager.RegisterStartupScript(
                                this, GetType(), "js", "alert('" + source[3].Split('=')[1] + " : ไม่อนุมัติ');"
                                + LoginWebRoot + WebRoot + "/LoginX.aspx';", true);
                        }
                    }
                }
                else
                {
                    if (action == "Bypass")
                    {
                        msg = "รายการนี้ถูก อนุมัติแล้ว!<br />ไม่สามารถทำรายการซ้ำได้ : รายการนี้ถูก ปฎิเสทการอนุมัติแล้ว!<br />ไม่สามารถทำรายการซ้ำได้";
                    }
                    else
                    {

                        if (GetMemoStatus(source[3].Split('=')[1]) == 3)
                        {
                            msg = "รายการนี้ถูก อนุมัติแล้ว!";
                        }
                        else
                        {
                            msg = "รายการนี้ถูก ปฎิเสทการอนุมัติแล้ว";
                        }

                        ScriptManager.RegisterStartupScript(
                            this, GetType(), "js", "alert('" + source[3].Split('=')[1] + " : ไม่สามารถทำรายการซ้ำได้ เนื่องจาก " + msg + "');"
                            + WebRoot + "/LoginX.aspx';", true);
                    }

                    valid = false;
                }
            }
            else
            {
                msg = _msg;
                valid = false;
            }

            return valid;
        }

        private string WsUsername = ConfigurationManager.AppSettings["Webservice.Username"];

        private string WsPassword = ConfigurationManager.AppSettings["Webservice.Password"];

        private string AppCode = ConfigurationManager.AppSettings["Webservice.AppCode"];

        public void UserLoginByGUID(string GUID, ref WSAuthorizeModel userInfo, ref string msgErr)
        {

            bool rst = true;
            string userName = "";
            try
            {
                DAStockPromotion dasp = new DAStockPromotion();
                string resEmail = dasp.GetUserEmailByuserGUID(GUID);

                userName = resEmail.Split('@')[0];

                var resturnUser = new Class.WSAuthorizeModel();
                var result = new AutorizeData();
                var webSerInstanceAuthorize = new ws_authorize.Authorize();
                webSerInstanceAuthorize.AuthorizeSoapHeaderValue = new AuthorizeSoapHeader() { username = WsUsername, password = WsPassword };

                result = webSerInstanceAuthorize.FindUserProfile(userName, AppCode);
                if (!result.LoginResult)
                {
                    resturnUser = new Class.WSAuthorizeModel(result);
                    rst = false;
                    msgErr = "User name หรือ Password ไม่ถูกต้อง";
                }
                Session["userInfo_" + Session.SessionID] = result;
            }
            catch (Exception ex)
            {
                rst = false;
                msgErr = ex.Message;
            }
        }

        [WebMethod]
        public static ListOfCostCenter GetCostCenter()
        {
            string msgerr;
            try
            {
                DAStockPromotion dasp = new DAStockPromotion();

                var ret = new List<CostCenter>();

                if ((HttpContext.Current.Session["RoleCode"] ?? "").ToString() == "MKT")
                {
                    ret = (from t1 in dasp.GetCostCenterData(out msgerr).AsEnumerable()
                           where t1.Field<bool?>("IsActive") == true
                           select new CostCenter
                           {
                               CostCenterID = t1.ItemArray[0].ToString(),
                               CostCenterName = t1.ItemArray[1].ToString()
                           }).ToList() ?? new List<CostCenter>();
                }
                else
                {
                    ret = (from t1 in dasp.GetCostCenterData(out msgerr).AsEnumerable()
                           select new CostCenter
                           {
                               CostCenterID = t1.ItemArray[0].ToString(),
                               CostCenterName = t1.ItemArray[1].ToString()
                           }).ToList() ?? new List<CostCenter>();
                }

                ListOfCostCenter lstCC = new ListOfCostCenter();
                lstCC.ListCostCenter = ret;
                lstCC.MsgErr = msgerr;
                return lstCC;
            }
            catch (Exception)
            {
                return new ListOfCostCenter();
            }
        }

        [WebMethod]
        public static string UpdateFinishMemoRequest(string docno)
        {
            try
            {
                DAStockPromotion dasp = new DAStockPromotion();
                return dasp.UpdateFinishMemoRequest(docno);
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        [WebMethod]
        public static bool iUpdateApproveMemoRequest(string docno)
        {
            try
            {
                DAStockPromotion dasp = new DAStockPromotion();
                dasp.spUpdateApproveMemoRequest(docno);
                return true;
            }

            catch (Exception)

            {
                return false;
            }
        }

        [WebMethod]
        public static GLAndObjData GetDataGlAndObj()
        {
            GLAndObjData _glandobj = new GLAndObjData();
            try
            {
                DAStockPromotion dasp = new DAStockPromotion();
                List<GLData> gldata = (from t1 in dasp.GetGLData().AsEnumerable()
                                       select new GLData
                                       {
                                           GLID = t1.ItemArray[0].ToString(),
                                           GLDESC = t1.ItemArray[1].ToString()
                                       }).ToList();
                List<ObjectiveData> objdata = (from t1 in dasp.GetObjectiveData().AsEnumerable()
                                               select new ObjectiveData
                                               {
                                                   ID = (int)t1[0],
                                                   Titles = (string)t1[1],
                                                   Details = (string)t1[2]
                                               }).ToList();



                _glandobj.IsCheckMKT = true;
                _glandobj.Message = "";
                _glandobj.GlResult = gldata;
                _glandobj.ObjResult = objdata;
                return _glandobj;
            }
            catch (Exception ex)
            {
                _glandobj.IsCheckMKT = false;
                _glandobj.Message = ex.Message.ToString();
                _glandobj.GlResult = new List<GLData>();
                _glandobj.ObjResult = new List<ObjectiveData>();
                return _glandobj;
            }
        }

        [WebMethod]
        public static ObjUsersData GetUsers()
        {
            ObjUsersData _objdata = new ObjUsersData();
            List<GetUsersData> allType = new List<GetUsersData>();
            DAStockPromotion dasp = new DAStockPromotion();
            try
            {
                allType = (from t1 in dasp.GetUserRole().AsEnumerable()
                           group t1 by new
                           {
                               ID = t1.Field<string>("EmpCode"),
                               FName = t1.Field<string>("FirstName"),
                               LName = t1.Field<string>("LastName")
                           } into t
                           select new GetUsersData
                           {
                               ID = t.Key.ID,
                               FName = t.Key.FName,
                               LName = t.Key.LName
                           }).ToList();
                _objdata.isCheck = true;
                _objdata.Message = "";
                _objdata.ListUsersData = allType;

                return _objdata;
            }
            catch (Exception ex)
            {
                _objdata.isCheck = false;
                _objdata.Message = ex.Message.ToString();
                _objdata.ListUsersData = new List<GetUsersData>();

                return _objdata;
            }
        }

        [WebMethod]
        public static ListGLandObj CheckMarketingUser(string empcode)
        {
            DAStockPromotion dasp = new DAStockPromotion();
            try
            {
                var res = dasp.CheckMarketingUser(empcode);
                return res;
            }
            catch (Exception ex)
            {
                ListGLandObj lst = new ListGLandObj();
                lst.IsCheck = false;
                lst.Message = ex.Message.ToString();
                lst.GL = new List<DAStockPromotion.GLData>();
                lst.OBJ = new List<DAStockPromotion.ObjData>();
                return lst;
            }
        }


        private List<ObjectiveData> GetObjectiveData()
        {
            DAStockPromotion dasp = new DAStockPromotion();
            var ret = (from t1 in dasp.GetObjectiveData().AsEnumerable()
                       select new ObjectiveData
                       {
                           ID = (int)t1[0],
                           Titles = (string)t1[1],
                           Details = (string)t1[2]
                       }).ToList();

            return ret;
        }

        public List<GLData> GetGLData()
        {
            DAStockPromotion dasp = new DAStockPromotion();
            var ret = (from t1 in dasp.GetGLData().AsEnumerable()
                       select new GLData
                       {
                           GLID = t1.ItemArray[0].ToString(),
                           GLDESC = t1.ItemArray[1].ToString()
                       }).ToList();

            return ret;
        }
    }

    public class ObjUsersData
    {
        public bool isCheck { get; set; }
        public string Message { get; set; }
        public List<GetUsersData> ListUsersData { get; set; }
    }

    public class GetUsersData
    {
        public string ID { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
    }

    public class GLAndObjData
    {
        public bool IsCheckMKT { get; set; }
        public string Message { get; set; }
        public List<GLData> GlResult { get; set; }
        public List<ObjectiveData> ObjResult { get; set; }
    }

    public class GLData
    {
        public string GLID { get; set; }
        public string GLDESC { get; set; }
    }

    public class ObjectiveData
    {
        public int ID { get; set; }
        public string Titles { get; set; }
        public string Details { get; set; }
    }

    public class ListOfCostCenter
    {
        public List<CostCenter> ListCostCenter { get; set; }
        public string MsgErr { get; set; }
    }

    public class CostCenter
    {
        public string CostCenterID { get; set; }
        public string CostCenterName { get; set; }
    }

    public class GL
    {
        public long GLID { get; set; }
        public string GLN { get; set; }
    }

    public class ListRetMemoData
    {
        public List<MemoReturnData> RetMemoData { get; set; }
        public string DOCNO { get; set; }
    }

    public class User
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
    }

    public class UserByMemoDo
    {
        public string REASON { get; set; }
        public string DOCNO { get; set; }
        public string MEMOCREATEDATE { get; set; }
        public string CREATORNAME { get; set; }
        public string Email { get; set; }
        public string PositionName { get; set; }
        public string PhoneExt { get; set; }
    }

    public class MemoReturnData
    {
        public string DOCNO { get; set; }
        public string MEMOCREATEDATE { get; set; }
        public string CREATORNAME { get; set; }
        public string CCNAME { get; set; }
        public string REASON { get; set; }
        public string STATUSNAME { get; set; }
        public string REJECTREASON { get; set; }
    }

    public class MemoItems
    {
        public string DocType { get; set; }
        public string DocNo { get; set; }
        public string CreateDate { get; set; }
        public string UsingDate { get; set; }
        public string EndingDate { get; set; }
        public string EmpCode { get; set; }
        public string UserCreateName { get; set; }
        public string CostCenterCode { get; set; }
        public string CostCenterName { get; set; }
        public string Reason { get; set; }
        public string ApproveId { get; set; }
        public string SubApproveId { get; set; }
        public string Status { get; set; }

        public string PromotionID { get; set; }
        public string Promotion { get; set; }
        public string GLNO { get; set; }
        public string GLName { get; set; }
        public string ObjID { get; set; }
        public string ObjName { get; set; }

        public List<ListItems> DataItemsValue { get; set; }
        public List<ListEmail> ApproveEmail { get; set; }
    }

    public class ListEmail
    {
        public string ApproveName { get; set; }
        public string ApproveEmail { get; set; }
        public string AuthorityName { get; set; }
        public string AuthorityEmail { get; set; }
    }

    public class ListItems
    {
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ItemNo { get; set; }
        public string ItemName { get; set; }
        public string PricePerUnit { get; set; }
        public string Quantity { get; set; }
        public string Type { get; set; }
        public string TotalPrice { get; set; }
    }

    public class Itmes
    {
        public string ItemNo { get; set; }
        public string ItemName { get; set; }
        public decimal UnitPrice { get; set; }
        public string ItemUnit { get; set; }
    }
}