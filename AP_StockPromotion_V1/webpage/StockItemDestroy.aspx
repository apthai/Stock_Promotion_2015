<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockItemDestroy.aspx.cs"
    Inherits="AP_StockPromotion_V1.webpage.StockItemDestroy" MasterPageFile="~/Master/MasterPage.Master" %>

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
            $('#divNavx').html('รายการตัดสต๊อกสูญเสีย');
            $(".js-example-basic-single").select2({ width: '100%' });
            $('.js-example-basic-single-ws').select2({ minimumResultsForSearch: Infinity, width: '100%' });

            $('#accordion-style').on('click', function (ev) {
                var target = $('input', ev.target);
                var which = parseInt(target.val());
                if (which == 2) $('#accordion').addClass('accordion-style2');
                else $('#accordion').removeClass('accordion-style2');
            });
        });

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

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- #section:elements.accordion -->
    <div id="accordion" class="accordion-style1 panel-group">
        <div class="panel panel-default">
            <div class="panel-heading">
                <h4 class="panel-title">
                    <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion" href="#collapseOne">
                        <i class="ace-icon fa fa-angle-down bigger-110" data-icon-hide="ace-icon fa fa-angle-down" data-icon-show="ace-icon fa fa-angle-right"></i>
                        &nbsp;ค้นหาประวัติการตัดสต๊อกสูญเสีย
                    </a>
                </h4>
            </div>

            <div class="panel-collapse collapse in" id="collapseOne">
                <div class="panel-body">
                    <div class="col-sm-12">
                        <div class="col-sm-2 div-caption">โครงการ</div>
                        <div class="col-sm-3">
                            <asp:DropDownList ID="ddlProject" runat="server" class="col-sm-12 js-example-basic-single js-states form-control hide"></asp:DropDownList>
                        </div>
                        <div class="col-sm-2 div-caption">สินค้าโปรโมชั่น</div>
                        <div class="col-sm-3">
                            <asp:DropDownList ID="ddlItem" runat="server" class="col-sm-12 js-example-basic-single js-states form-control hide"></asp:DropDownList>
                        </div>
                        <div class="col-sm-2 div-caption"></div>
                    </div>
                    <div class="col-sm-12" style="text-align: center;">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" class="btn btn-white btn-info btn-sm" Width="75px" OnClick="btnSearch_Click" /><%-- OnClick="btnSearch_Click"--%>
                        &nbsp;
                        <asp:Button ID="btnClear" runat="server" Text="Clear" class="btn btn-white btn-info btn-sm" Width="75px" OnClick="btnClear_Click" /><%-- OnClick="btnClear_Click"--%>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- #accordion -->

    <div class="col-sm-12">
        <asp:Button ID="btnDestroyItem" runat="server" class="btn btn-white btn-info btn-sm" Text="+ ตัดสต๊อกสูญเสีย" OnClick="btnDestroyItem_Click" />
    </div>
    

    <div class="col-sm-12">
        <asp:GridView ID="grdData" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" class="col-sm-12"
            EmptyDataText="No data." ShowHeaderWhenEmpty="True" OnRowCommand="grdData_RowCommand" AllowSorting="True" 
            OnPageIndexChanging="grdData_PageIndexChanging" OnSorting="grdData_Sorting" PageSize="20" AllowPaging="true">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="41px">
                    <ItemTemplate>
                        <asp:ImageButton ID="imgCancelDestroy" runat="server" ImageUrl="~/img/stock_edit.png" Width="23px" Style="vertical-align: baseline;" CommandName="CancelDestroy" CommandArgument='<%# Eval("DestroyListId") %>' />
                        <asp:HiddenField ID="grdHdfDestroyListId" runat="server" Value='<%# Eval("DestroyListId") %>' />
                        <asp:HiddenField ID="grdHdfProject_Id" runat="server" Value='<%# Eval("ProjectID") %>' />
                        <asp:HiddenField ID="grdHdfProjectName" runat="server" Value='<%# Eval("ProjectName") %>' />
                        <asp:HiddenField ID="grdHdfMasterItemId" runat="server" Value='<%# Eval("MasterItemId") %>' />
                        <asp:HiddenField ID="grdHdfItemName" runat="server" Value='<%# Eval("ItemName") %>' />
                        <asp:HiddenField ID="grdHdfCreateBy" runat="server" Value='<%# Eval("CreateBy") %>' />
                        <asp:HiddenField ID="grdHdfCreateDate" runat="server" Value='<%# Eval("CreateDate") %>' />
                        <asp:HiddenField ID="grdHdfDestroyAmount" runat="server" Value='<%# Eval("DestroyAmount") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="OBJKEY" SortExpression="OBJKEY" HeaderText="SAP DocNo." HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="ProjectName" SortExpression="ProjectName" HeaderText="โครงการ" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="ItemName" SortExpression="ItemName" HeaderText="สินค้าโปรโมชั่น" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="FullName" SortExpression="FullName" HeaderText="ผู้ดำเนินการ" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="234px" />
                <asp:BoundField DataField="CreateDate" SortExpression="CreateDate" HeaderText="วันที่" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Width="123px" />
                <asp:BoundField DataField="DestroyAmount" SortExpression="DestroyAmount" HeaderText="จำนวน" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="91px" />                
            </Columns>
            <EmptyDataTemplate>
                ไม่มีพบรายการสินค้าสูญเสีย...
            </EmptyDataTemplate>
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
