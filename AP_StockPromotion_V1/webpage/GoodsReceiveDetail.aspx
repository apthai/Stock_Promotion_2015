<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GoodsReceiveDetail.aspx.cs"
    Inherits="AP_StockPromotion_V1.webpage.GoodsReceiveDetail" MasterPageFile="~/Master/MasterPopup.Master" %>

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
            $('#divNavx').html('รายละเอียดดการรับสินค้า');
            $(".js-example-basic-single").select2({ width: '100%' });
            $('.js-example-basic-single-ws').select2({ minimumResultsForSearch: Infinity, width: '100%' });
        });

        function closePage() {
            parent.jQuery.colorbox.close();
            // window.close();
        }

        function bindDataParentPage(gname) {
            parent.$("#ContentPlaceHolder1_btnSearch").click();
            closePage();
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="col-sm-12">
        <div class="col-sm-12">
            <div class="col-sm-12">
                <asp:Label ID="Label1" Font-Size="X-Large" runat="server" Text="GR : " class="col-sm-2 label-caption"></asp:Label>
                <asp:Label ID="lbGRNo" Font-Size="X-Large" runat="server" Text="XXXXXXXXX" class="col-sm-6"></asp:Label>
                <asp:HiddenField ID="hdfGR_Year" runat="server" />
                <asp:HiddenField ID="hdfReceiveHeaderID" runat="server" />
            </div>
        </div>
        <div class="col-sm-12">
            <div class="col-sm-6">
                <asp:Label ID="Label4" runat="server" Text="PO : " class="col-sm-4 label-caption"></asp:Label>
                <asp:Label ID="lbPO" runat="server" Text="" class="col-sm-8"></asp:Label>
            </div>
            <div class="col-sm-6">
                <asp:Label ID="Label6" runat="server" Text="Delivery Note : " class="col-sm-4 label-caption"></asp:Label>
                <asp:Label ID="lbDeliveryNote" runat="server" Text="" class="col-sm-8"></asp:Label>
            </div>
        </div>
        <div class="col-sm-12">
            <div class="col-sm-6">
                <asp:Label ID="Label2" runat="server" Text="Vendor : " class="col-sm-4 label-caption"></asp:Label>
                <asp:Label ID="lbVendor" runat="server" Text="" class="col-sm-8"></asp:Label>
            </div>
            <div class="col-sm-6">
                <asp:Label ID="Label8" runat="server" Text="Posting Date : " class="col-sm-4 label-caption"></asp:Label>
                <asp:Label ID="lbPostingDate" runat="server" Text="" class="col-sm-8"></asp:Label>
            </div>
        </div>
        
        <div class="col-sm-12">&nbsp;</div>
        <div class="col-sm-12">
            <asp:GridView ID="grdData" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" class="col-sm-12"
                EmptyDataText="No data." ShowHeaderWhenEmpty="True" AllowPaging="true" AllowSorting="true" OnPageIndexChanging="grdData_PageIndexChanging" OnSorting="grdData_Sorting" PageSize="15">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="ItemName" HeaderText="รายละเอียดสินค้า" HeaderStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="ReceiveAmount" HeaderText="จำนวนรับ" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px" />                    
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
        
        <div class="col-sm-12">&nbsp;</div>
        <div class="col-sm-12 div-caption">
            <asp:Button ID="btnCancelGR" runat="server" Text="ยกเลิกรายการรับสินค้า" class="btn btn-white btn-info btn-sm" OnClick="btnCancelGR_Click" />
        </div>
    </div>

</asp:Content>
