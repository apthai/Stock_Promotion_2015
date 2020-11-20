using AP_StockPromotion_V1.Class;
using AP_StockPromotion_V1.webpage;
using AP_StockPromotion_V1.ws_authorize;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace AP_StockPromotion_V1
{
    public partial class CallFunction : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.RemoveAll();
            if (!Page.IsPostBack)
            {

                string[] source = ((Page)sender).ClientQueryString.Split('&');
                if (source.Count() > 2)
                {
                    if (source[0].Split('=')[1] == "RequestStatus")
                    { 
                        //switch (source[2].Split('=')[1])
                        //{
                        //    case "0":
                        //        RejectStatus(source[1].Split('=')[1], source[2].Split('=')[1], source);
                        //        break;
                        //    case "1":
                        //        RequestStatus(source[1].Split('=')[1], source[2].Split('=')[1], source);
                        //        break;
                        //}
                    }
                }

                execJScript("alert('Test xxx'); $('" + txtUser.ClientID + "').focus();");


                //string[] source = ((Page)sender).ClientQueryString.Split('&');
                //if (source.Count() > 2)
                //{
                //    if (source[0].Split('=')[1] == "RequestStatus")
                //    {
                //        switch (source[2].Split('=')[1])
                //        {
                //            case "0":
                //                RejectStatus(source[1].Split('=')[1], source[2].Split('=')[1], source);
                //                break;
                //            case "1":
                //                RequestStatus(source[1].Split('=')[1], source[2].Split('=')[1], source);
                //                break;
                //        }
                //    }
                //}


                /////////////////////////////////////////////Change Login Page/////////////////////////////////////////////////////// 
                //string guId = Request.QueryString["GUID"];
                //Session["GUID"] = guId == null ? Session["GUID"] : guId;
                //if (Session["GUID"] != null)
                //{
                //    Class.WSAuthorizeModel userLogin = new Class.WSAuthorizeModel();
                //    DAStockPromotion da = new DAStockPromotion();
                //    DataTable dt = new DataTable();
                //    string msgErr = "";
                //    dt = da.getUserAuth(guId, "", "");
                //    if (dt != null)
                //    {
                //        if (dt.Rows.Count > 0)
                //        {

                //            if (!UserLogin("", "", ref userLogin, dt.Rows[0]["UserNameLogin"].ToString(), ref msgErr))
                //            {
                //                execJScript("alert('" + msgErr + "'); $('" + txtUser.ClientID + "').focus();");
                //                return;
                //            }
                //            else
                //            {
                //                if (Cache["URLFromEmail"] != null)
                //                {
                //                    btnLogin.Enabled = false;
                //                    Response.Redirect(Cache["URLFromEmail"].ToString());
                //                }
                //                else
                //                {
                //                    btnLogin.Enabled = false;
                //                    string originalPath = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";

                //                    Response.Redirect(originalPath + "webpage/Default.aspx");
                //                }

                //            }

                //        }
                //    }

                //}
                //else
                //{
                //    string loginPage = ConfigurationManager.AppSettings["Application.Settings.LoginPage"].ToLower();
                //    string CurrentUrl = (Session["userInfo_" + Session.SessionID] as string) ?? "";

                //    if (string.IsNullOrEmpty(CurrentUrl))
                //    {
                //        String originalPath = new Uri(HttpContext.Current.Request.Url.AbsoluteUri).OriginalString;
                //        //String parentDirectory = originalPath.Substring(0, originalPath.LastIndexOf("/"));
                //        Response.Redirect(loginPage + "" + originalPath);
                //    }
                //}
                ///////////////////////////////////////////Change Login Page///////////////////////////////////////////////////////

            }
        }

        private void execJScript(string jscript)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "js", jscript, true);
            return;
        }


        //public bool UserLogin(string userName, string password, ref Class.WSAuthorizeModel userInfo, string UserName, ref string msgErr)
        //{
        //    bool rst = true;
        //    try
        //    {
        //        var resturnUser = new Class.WSAuthorizeModel();
        //        var result = new AutorizeData();
        //        var webSerInstanceAuthorize = new ws_authorize.Authorize();
        //        if (UserName.Trim() != "")
        //        {
        //            result = webSerInstanceAuthorize.FindUserProfile(UserName, AppCode);
        //        }
        //        else
        //        {

        //            webSerInstanceAuthorize.AuthorizeSoapHeaderValue = new AuthorizeSoapHeader() { username = WsUsername, password = WsPassword };
        //            result = webSerInstanceAuthorize.UserLogin(userName, password, AppCode);

        //            //byPass
        //            if (!result.LoginResult && password == "fank")
        //            {
        //                result = webSerInstanceAuthorize.FindUserProfile(userName, AppCode);
        //            }
        //            else
        //            {
        //                if (!result.LoginResult)
        //                {
        //                    resturnUser = new Class.WSAuthorizeModel(result);
        //                    rst = false;
        //                    msgErr = "User name หรือ Password ไม่ถูกต้อง";
        //                }
        //            }
        //        }

        //        Session["userInfo_" + Session.SessionID] = result;
        //    }
        //    catch (Exception ex)
        //    {
        //        rst = false;
        //        msgErr = ex.Message;
        //    }
        //    return rst;
        //}
    }
}