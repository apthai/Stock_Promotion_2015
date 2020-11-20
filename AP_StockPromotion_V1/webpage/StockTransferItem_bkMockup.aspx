<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockTransferItem_bkMockup.aspx.cs" 
    Inherits="AP_StockPromotion_V1.web.StockTransferItem_bkMockup" MasterPageFile="~/Master/MasterPage.Master"  %>


<asp:Content ID="Contenthead" ContentPlaceHolderId="head" runat="server">
    <style type="text/css">

    </style>
    <script type="text/javascript">
        jQuery(function ($) {
            $('#accordion-style').on('click', function (ev) {
                var target = $('input', ev.target);
                var which = parseInt(target.val());
                if (which == 2) $('#accordion').addClass('accordion-style2');
                else $('#accordion').removeClass('accordion-style2');
            });
        });
        
        function popupStockTransferItemDetail(ItemCountMethod) {
            Popup80('StockTransferItemDetail.aspx?ItemCountMethod=' + ItemCountMethod);
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
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderId="ContentPlaceHolder1" runat="server">

    <!-- #section:elements.accordion -->
    <div id="accordion" class="accordion-style1 panel-group">
        <div class="panel panel-default">
            <div class="panel-heading">
                <h4 class="panel-title">
                    <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion" href="#collapseOne">
                        <i class="ace-icon fa fa-angle-down bigger-110" data-icon-hide="ace-icon fa fa-angle-down" data-icon-show="ace-icon fa fa-angle-right"></i>
                        &nbsp;Filter
                    </a>
                </h4>
            </div>

            <div class="panel-collapse collapse in" id="collapseOne">
                <div class="panel-body" style="height: 100px;">
                </div>
            </div>
        </div>
    </div>

    <!-- #accordion -->
    <div class="col-sm-12" >
        <asp:GridView ID="grdData" runat="server" AutoGenerateColumns="False" CellPadding="4" 
            ForeColor="#333333" GridLines="None" class="col-sm-12" OnRowCommand="grdData_RowCommand" OnRowDataBound="grdData_RowDataBound">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="RequisitionDate" HeaderText="วันที่เบิก" />
                <asp:BoundField DataField="RequisitionBy" HeaderText="ผู้เบิก" />
                <asp:BoundField DataField="DocRefNo" HeaderText="เลขที่เอกสารอ้างอิง" />
                <asp:BoundField DataField="ProjectName" HeaderText="โครงการ" />
                <asp:BoundField DataField="ItemName" HeaderText="สินค้าโปรโมชั่น" />
                <asp:BoundField DataField="ReceiveAmount" HeaderText="จำนวน" />
                <asp:BoundField DataField="ItemUnit" HeaderText="หน่วย" />
                <asp:TemplateField HeaderText="สถานะ" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="grdlbStatus" runat="server" ></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="จัดของ" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:ImageButton ID="imgEdit" runat="server" ImageUrl="~/img/align_just.png" Width="23px" Style="vertical-align: baseline;" CommandName='<%# Eval("RequisitionStatus") %>' CommandArgument='<%# Eval("ItemCountMethod") %>' />
                        <asp:HiddenField ID="hdfProjectId" runat="server" Value='<%# Eval("ProjectName") %>' />
                        <asp:HiddenField ID="hdfItemId" runat="server" Value='<%# Eval("ItemId") %>' />
                        <asp:HiddenField ID="hdfRequisitionStatus" runat="server" Value='<%# Eval("RequisitionStatus") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
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

</asp:Content>