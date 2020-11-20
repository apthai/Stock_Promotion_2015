<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeliveryListEdit.aspx.cs"
    Inherits="AP_StockPromotion_V1.webpage.DeliveryListEdit" MasterPageFile="~/Master/MasterPopup.Master" %>

<asp:Content ID="Contenthead" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .label-caption {
            text-align: right;
        }

        .div-caption {
            text-align: right;
            padding-right: 5px;
        }
    </style>
    <script type="text/javascript">
        jQuery(function ($) {
            $('#divNavx').html('ส่งมอบสินค้าโปรโมชั่นให้ลูกค้า');
            $(".js-example-basic-single").select2({ width: '100%' });
            $('.js-example-basic-single-ws').select2({ minimumResultsForSearch: Infinity, width: '100%' });

            setDatePicker();
            setTextNumericOnly();
            setTextDigitOnly();
        });

        function setTextNumericOnly() {
            jQuery('.numericOnly').keyup(function () {
                this.value = this.value.replace(/[^0-9]/g, '');
            });
        }

        function setTextDigitOnly() {
            jQuery('.digitOnly').keyup(function () {
                this.value = this.value.replace(/[^0-9\.]/g, '');
            });
        }

        function Popup60(url) {

            var scr_H = screen.height;
            var scr_W = screen.width;
            var h = (scr_H / 100 * 80);
            var w = (scr_W / 100 * 60);
            var t = (scr_H / 100 * 5);
            var l = (scr_W / 100 * 25);
            window.open(url, '', 'height=' + h + ',width=' + w + ',top=' + t + ',left=' + l + ',status=yes,scrollbars=no,resizable=yes,menubar=no;toolbar=no,titlebar=no');
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

        function setDatePicker() {
            //$('#').datepicker({ dateFormat: "dd/mm/yy" }); // { dateFormat: "dd-mm-yy" }
            //$('#').datepicker({ dateFormat: "dd/mm/yy" });
        }

        function calcBalance(txtId, hdfId) {
            var inp = parseInt($('#' + txtId + '').val());
            var bal = parseInt($('#' + hdfId + '').val());
            if (isNaN(inp)) {
                inp = 0;
                $('#' + txtId + '').val(0);
            }
            if (inp > bal) {
                $('#' + txtId + '').val($('#' + hdfId + '').val());
            }
            // txtSelectAmount
            var sum = 0;
            var arrT = $('#<%=grdData.ClientID %>').find('input:text');
            $.each(arrT, function (index, t) {
                var x = parseInt(t.value);
                if (isNaN(x)) {
                    x = 0;
                }
                sum += x;
            });
            $('#<%= hdfSelAmt.ClientID %>').val(sum);
            $('#ContentPlaceHolder1_grdDelivery_grdLbSumSelect_0').html(sum);
        }

        function closePage() {
            parent.jQuery.colorbox.close();
            // window.close();
        }

        function bindDataParentPage() {
            parent.$("#ContentPlaceHolder1_btnSearch").click();
            closePage();
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdfSelAmt" runat="server" Value="0" />
    <div class="col-sm-12">
        <div class="col-sm-2 div-caption">ใบส่งมอบ</div>
        <div class="col-sm-3">
            <asp:TextBox ID="txtDeliveryNo" runat="server" placeholder="Delivery No" class="col-sm-12" ReadOnly="true" ></asp:TextBox>
        </div>
        <div class="col-sm-2 div-caption">วันที่ส่งมอบ</div>
        <div class="col-sm-3">
            <asp:TextBox ID="txtDeliveryDate" runat="server" placeholder="Delivery Date" class="col-sm-12" ReadOnly="true" ></asp:TextBox>
        </div>
        <div class="col-sm-2 div-caption"></div>        
    </div>
    <div class="col-sm-12">
        <div class="col-sm-2 div-caption">โครงการ</div>
        <div class="col-sm-3">
            <asp:TextBox ID="txtProjectName" runat="server" placeholder="ProjectName" class="col-sm-12" ReadOnly="true" ></asp:TextBox>
        </div>
        <div class="col-sm-2 div-caption">WBS</div>
        <div class="col-sm-3">
            <asp:TextBox ID="txtWBS" runat="server" placeholder="WBS" class="col-sm-12" ReadOnly="true" ></asp:TextBox>
        </div>
        <div class="col-sm-2 div-caption"></div>        
    </div>
    <div class="col-sm-12">
        <div class="col-sm-2 div-caption">สถานะ</div>
        <div class="col-sm-3">
            <asp:TextBox ID="txtPostingStatus" runat="server" placeholder="Status" class="col-sm-12" ReadOnly="true" ></asp:TextBox>
        </div>
        <div class="col-sm-2 div-caption">ยอดรวม</div>
        <div class="col-sm-3">
            <asp:TextBox ID="txtTotal" runat="server" placeholder="Total" class="col-sm-12" ReadOnly="true" ></asp:TextBox>
        </div>
        <div class="col-sm-2 div-caption"></div>        
    </div>
    <!-- #accordion -->
    <div class="col-sm-12">&nbsp;</div>

    <div class="col-sm-12">
        <div class="col-sm-12" style="font-weight:bold; font-size:large;">รายการสินค้าที่ส่งมอบ</div>
        <asp:GridView ID="grdData" runat="server" AutoGenerateColumns="False" CellPadding="4"
            ForeColor="#333333" GridLines="None" class="col-sm-12">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="ReqNo" HeaderText="เลขที่ใบเบิก" />
                <asp:BoundField DataField="WBSReq" HeaderText="WBS เบิก" />
                <asp:BoundField DataField="ItemName" HeaderText="ชื่อสินค้าโปรโมชั่น" />
                <asp:BoundField DataField="Model" HeaderText="รุ่น" />
                <asp:BoundField DataField="Color" HeaderText="สี" />
                <asp:BoundField DataField="ExpireDate" HeaderText="วันหมดอายุ" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="Remark" HeaderText="หมายเหตุ" />
                <asp:BoundField DataField="Serial" HeaderText="Serial No." />
                <asp:BoundField DataField="Price" HeaderText="มูลค่าต่ออหน่วย"  DataFormatString="{0:#,##0.00}" />
            </Columns>
            <EditRowStyle BackColor="#2461BF" />
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#EFF3FB" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#F5F7FB" />
            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
            <SortedDescendingCellStyle BackColor="#E9EBEF" />
            <SortedDescendingHeaderStyle BackColor="#4870BE" />
        </asp:GridView>
    </div>
    <div class="col-sm-12" >&nbsp;</div>
    <div class="col-sm-12" style="text-align: right;">
        <asp:Button id="btnPostAccount" runat="server" class="btn btn-white btn-info btn-sm" Width="150px" Text="บันทึกบัญชี" OnClick="btnPostAccount_Click"/>
    </div>

</asp:Content>
