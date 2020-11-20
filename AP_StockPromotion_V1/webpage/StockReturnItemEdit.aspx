<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockReturnItemEdit.aspx.cs" 
    Inherits="AP_StockPromotion_V1.web.StockReturnItemEdit"  MasterPageFile="~/Master/MasterPage.Master"  %>

<asp:Content ID="Contenthead" ContentPlaceHolderId="head" runat="server">
    <style type="text/css">
        .label-caption {
            text-align:right;
        }
        .div-caption {
            text-align:right;
            padding-right:5px;
        }
    </style>
    <script type="text/javascript">
        jQuery(function ($) {
            $('#divNavx').html('คืนสินค้าโปรโมชั่น');
            $(".js-example-basic-single").select2({ width: '100%' });
            $('.js-example-basic-single-ws').select2({ minimumResultsForSearch: Infinity, width: '100%' });
            initTextInput();
            setDDLProject();
        });

        function popupStockReturnItemEditCheckAmount(masterItemId) {
            Popup80('StockReturnItemEditCheckAmount.aspx?MasterItemId=' + masterItemId);
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

       function setDDLProject() {
            var prj = $('#<%= ddlProject.ClientID %>').val();
            if (prj != '') {
                $('.js-example-basic-single').prop('disabled', true);
            }
        }

        function initTextInput() {
            $('#<%= txtSerial.ClientID %>').keypress(function (e) {
                if (e.which == 13) {
                    e.preventDefault();
                    $('#<%= imgCheckSerial.ClientID %>').click();
                }
            });
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderId="ContentPlaceHolder1" runat="server">

    <asp:Button ID="btnDummy" runat="server" Text="" Width="0px" Height="0px" style="opacity:0.0;" Enabled="False" />
    <asp:Button ID="btnSelectAmt" runat="server" Text="" class="btn btn-white btn-info btn-sm" style="display:none;" OnClick="btnSelectAmt_Click" />

    <asp:HiddenField ID="hdfSelProject" runat="server" />

    <div class="col-sm-12">
        <div class="col-sm-1 div-caption">โครงการ/หน่วยงาน</div>
        <div class="col-sm-4">
            <asp:DropDownList ID="ddlProject" runat="server" class="col-sm-12 js-example-basic-single js-states form-control hide"></asp:DropDownList>
        </div>
        <div class="col-sm-3">
            <asp:Button ID="btnSelectProject" runat="server" Text="เลือก" class="btn btn-white btn-info btn-sm" OnClick="btnSelectProject_Click" Width="75px" />
        </div>
        <div class="col-sm-2 div-caption"></div>
    </div>
    
    <div class="col-sm-12">&nbsp;</div>
    <div class="col-sm-12" id="divCheckSerial" runat="server" style="display:none;">
        <div class="col-sm-1 div-caption">Serial No.</div>
        <div class="col-sm-3">
            <asp:TextBox ID="txtSerial" runat="server" class="col-sm-12" placeHolder="Serial No."></asp:TextBox>
        </div>
        <div class="col-sm-8">
            <asp:ImageButton ID="imgCheckSerial" runat="server" ImageUrl="~/img/checkbox_checked.png" Width="23px" Style="vertical-align: baseline;" OnClick="imgCheckSerial_Click"/>
        </div>
    </div>
    
    <div class="col-sm-12" >
        <asp:GridView ID="grdData" runat="server" AutoGenerateColumns="False" CellPadding="4" PageSize="20" 
            ForeColor="#333333" GridLines="None" class="col-sm-12" AllowPaging="True" OnRowDataBound="grdData_RowDataBound" OnRowCommand="grdData_RowCommand">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px">
                    <ItemTemplate>
                        <asp:ImageButton ID="imgCancelReturn" runat="server" ImageUrl="~/img/write_document.png" Width="23px" Style="vertical-align: baseline;" CommandName="CancelReturn" CommandArgument='<%# Eval("MasterItemId") %>' />
                        <asp:HiddenField ID="grdHdfMasterItemId" runat="server" Value='<%# Eval("MasterItemId") %>' />
                        <asp:HiddenField ID="grdHdfMasterItemName" runat="server" Value='<%# Eval("MasterItemName") %>' />
                        <asp:HiddenField ID="grdHdfItemCountMethod" runat="server" Value='<%# Eval("ItemCountMethod") %>' />
                        <asp:HiddenField ID="grdHdfStockAmount" runat="server" Value='<%# Eval("StockAmount") %>' />
                        <asp:HiddenField ID="grdHdfStockReturn" runat="server" Value='<%# Eval("StockReturn") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="MasterItemName" HeaderText="สินค้าโปรโมชั่น" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="StockAmount" HeaderText="คงเหลือ" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="95px" />
                <asp:BoundField DataField="StockReturn" HeaderText="คืน" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="95px" />
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
    <div class="col-sm-12" style="text-align: right;">&nbsp;</div>
    <div class="col-sm-12" style="text-align: right;">
        <asp:Button ID="btnSaveX" runat="server" Text="บันทึกการคืนสินค้าโปรโมชั่น" class="btn btn-white btn-info btn-sm" OnClick="btnSaveX_Click" />
        <%--<asp:Button ID="btnSave" runat="server" Text="บันทึกการคืนสินค้าโปรโมชั่น (ปุ่มเดิม)" class="btn btn-white btn-info btn-sm" OnClick="btnSave_Click" />--%>
    </div>
</asp:Content>