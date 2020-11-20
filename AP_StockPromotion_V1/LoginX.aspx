<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginX.aspx.cs" 
    Inherits="AP_StockPromotion_V1.LoginX" MasterPageFile="~/master/MasterLogin.Master" %>
<asp:Content ID="Contenthead" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#divNavx').html('Login');
            //$(".js-example-basic-single").select2({ width: '100%' });
            //$('.js-example-basic-single-ws').select2({ minimumResultsForSearch: Infinity, width: '100%' });
            setLogo();
            $('#<%= txtUser.ClientID %>').focus();
            localStorage.clear();
        });

        $(window).resize(function () {
            setLogo();
        });

        function setLogo() {
            var w = $(window).width();
            var h = $(window).height();
            $('#imgLogo').width(w / 100 * 23);
            $('#div1').height(h / 100 * 15);
        }


        function Popup80(url) {

            var scr_H = screen.height;
            var scr_W = screen.width;
            var h = (scr_H / 100 * 80);
            var w = (scr_W / 100 * 90);
            var t = (scr_H / 100 * 5);
            var l = (scr_W / 100 * 5);
            window.open(url, '', 'height=' + h + ',width=' + w + ',top=' + t + ',left=' + l + ',status=yes,scrollbars=no,resizable=yes,menubar=no;toolbar=no,titlebar=no');
        }

        function PopupFullScr(url) {
            var scr_H = screen.height;
            var scr_W = screen.width;
            var h = (scr_H / 100 * 95);
            var w = (scr_W / 100 * 99);
            var t = 0;
            var l = 0;
            window.open(url, '', 'height=' + h + ',width=' + w + ',top=' + t + ',left=' + l + ',status=no,scrollbars=yes,resizable=yes,menubar=no;toolbar=no,titlebar=no');
        }
        
        function PopupFullScrX(url) {
            var scr_H = screen.height;
            var scr_W = screen.width;
            var h = (scr_H / 100 * 95);
            var w = (scr_W / 100 * 99);
            var t = 0;
            var l = 0;
            window.open(url, '', 'fullscreen=yes,status=yes,scrollbars=no,resizable=yes,menubar=no;toolbar=no,titlebar=no');
        }
        function getDefaultPage() {
            PopupFullScr('webpage/Default.aspx');
            $('#<%= btnLogin.ClientID %>').attr('disabled', 'true');
            $('#<%= txtUser.ClientID %>').attr('readonly', 'true');
            $('#<%= txtPassword.ClientID %>').attr('readonly', 'true');
            $('.main-content').attr('style', 'display:none;');
            window.close();
        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="col-sm-12" id="div1" style="text-align: center;"></div>
    <div class="col-sm-12" id="divAPLogo" style="text-align: center;">
        <img id="imgLogo" src="img/head_login.jpg" />
    </div>
    <div class="col-sm-12" id="divInp">
        <div class="col-sm-3"></div>
        <div class="col-sm-6">
            <div class="col-sm-5" style="text-align:right">User name</div>
            <div class="col-sm-7">
                <asp:TextBox ID="txtUser" runat="server" placeholder="User Name" class="col-sm-6"></asp:TextBox>
            </div>

            <div class="col-sm-5" style="text-align:right">Password</div>
            <div class="col-sm-7">
                <asp:TextBox ID="txtPassword" runat="server" placeholder="Password" class="col-sm-6" TextMode="Password"></asp:TextBox>
            </div>
            
            <div class="col-sm-12">&nbsp;</div>

            <div class="col-sm-5" style="text-align:right"></div>
            <div class="col-sm-7">
                <div class="col-sm-3"></div>
                <asp:Button ID="btnLogin" runat="server" CssClass="btn btn-white btn-info btn-xs col-sm-3" Text="Login" OnClick="btnLogin_Click" />
            </div>
            
            <div class="col-sm-5" style="text-align:right"></div>
            <div class="col-sm-7">
                <div class="col-sm-6" style="text-align:right">
                    <asp:Label ID="lbVersion" runat="server" ForeColor="#9999ff"></asp:Label>
                </div>
                <div class="col-sm-6" style="text-align:right"></div>
            </div>
        </div>
        <div class="col-sm-3"></div>
    </div>

</asp:Content>
