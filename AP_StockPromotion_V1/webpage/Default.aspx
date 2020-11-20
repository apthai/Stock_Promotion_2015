<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs"
    Inherits="AP_StockPromotion_V1.web.Default" MasterPageFile="~/Master/MasterPage.Master" %>

<asp:Content ID="Contenthead" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .Noti {
            text-align: left;
            font-size: medium;
            color: red;
        }

        .Noti2 {
            text-align: left;
            font-size: small;
        }

        .Doc1 {
            text-align: left;
            font-size: small;
            color: blue;
        }

        .Doc2 {
            text-align: left;
            font-size: small;
            color: blue;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#divNavx').html('หน้าหลัก');
            setLogo();
        });

        $(window).resize(function () {
            setLogo();
        });

        function setLogo() {
            var w = $(window).width();
            var h = $(window).height();
            $('#imgLogo').width(w / 100 * 10);
            $('#imgLogo').height(w / 100 * 20);
            $('.div1').height(20);
            $('.div2').height(h / 100 * 5);
            //$('.div3').height(h / 100 * 10);
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

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="col-sm-12 div1" style="text-align: center;"></div>
    <div class="col-sm-12" id="divAPLogo" style="text-align: center;">
        <asp:Image ID="imgLogo" runat="server" Style="width: 80px;" ImageUrl="http://helpdesk.apthai.com/Resources/logo_200x320.png" />
    </div>
    <div class="col-sm-12 div2"></div>
    <div class="col-sm-12" style="text-align: center;">
        <%--        <div class="col-sm-2"></div>
        <div class="col-sm-8">การเบิกสินค้าโปรโมชั่นจะรวบ Order ทุกวันศุกร์ที่ 1 และ 3 ของทุกเดือน และใช้ระยะเวลาในการดำเนินการสั่งซื้อประมาณ 3 สัปดาห์</div>
        <div class="col-sm-2"></div>--%>

        <table align="center">
            <tr>
                <td class="Noti">ประกาศ !&nbsp&nbsp&nbsp</td>
                <td class="Noti">การเบิกสินค้าโปรโมชั่นจะรวบ Order ทุกวันศุกร์ที่ 1 และ 3 ของทุกเดือน และใช้ระยะเวลาในการดำเนินการสั่งซื้อประมาณ 3 สัปดาห์</td>
            </tr>
            <tr>
                <td></td>
                <td class="Noti">รบกวนแต่ละหน่วยงานวางแผนการเบิกล่วงหน้าอย่างน้อย 1 เดือน</td>
            </tr>
            <tr>
                <td></td>
                <td class="Noti2">ทั้งนี้ หากทำเบิกหลังจากศุกร์ที่ 1 จะทำการรวมยอดเพื่อทำเบิกในวันศุกร์ที่ 3 บวกระยะเวลาดำเนินการอีก 3 สัปดาห์</td>
            </tr>
            <tr>
                <td></td>
                <td class="Noti2">ดังตัวอย่างต่อไปนี้</td>
            </tr>
            <tr>
                <td></td>
                <td class="Noti2">เดือนพ.ค. 62</td>
            </tr>
            <tr>
                <td></td>
                <td class="Noti2">- ทำเบิกก่อนวันที่ 3 (ศุกร์ที่ 1) สามารถรับของได้ประมาณวันที่ 27 พ.ค.</td>
            </tr>
            <tr>
                <td></td>
                <td class="Noti2">- ทำเบิกวันที่ 6 จะตัดยอดการเบิกไปรวมกับการเบิกวันที่ 17(ศุกร์ที่ 3) สามารถรับของได้ประมาณวันที่ 10 มิ.ย.</td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <br />
                </td>
            </tr>
            <tr>
                <td class="Doc1">คู่มือการใช้งาน&nbsp&nbsp&nbsp</td>
                <td class="Doc2">คู่มือการเบิกสำหรับแผนกทั่วไป&nbsp&nbsp
                  <b><a href="../Document/HR ขั้นตอนการตั้งเบิกสินค้าภายในองค์กร.pdf" target="_blank">click</a></b>
                </td>
            </tr>
            <tr>
                <td></td>
                <td class="Doc2">คู่มือการเบิกสำหรับแผนก Marketing&nbsp&nbsp
                     <b><a href="../Document/MKT ขั้นตอนการตั้งเบิกสินค้าภายในองค์กร.pdf" target="_blank">click</a></b>
                </td>
            </tr>
            <tr>
                <td></td>
                <td class="Doc2">Report Portal --> STOCK PROMOTION --> รายงานสินค้าโปรโมชั่นคงเหลือในสต๊อกกลาง&nbsp&nbsp
                    <asp:HyperLink ID="hlinkReportStockBakance" runat="server" NavigateUrl="http://rpt.apthai.com/?GUID=[@GUID]" CssClass="Doc2" Target="_blank"><b>click</b></asp:HyperLink>
                </td>
            </tr>
        </table>
        
        <%--<br />
        Diff Lead for Call Center<br />
        <br />
        <table border="1" style="margin-left: 20px; width: 90%; max-width: 1000px">
            <tr>
                <th style="background-color:#66ff00"><b>LeadsID<b></th>
                <th style="background-color:#66ff00"><b>RefID<b></th>
                <th style="background-color:#66ff00"><b>LeadDate<b></th>
                <th style="background-color:#66ff00"><b>Fullname<b></th>
                <th style="background-color:#66ff00"><b>Call Description<b></th>
            </tr>
            <tr>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
        </table>--%>

    </div>
</asp:Content>
