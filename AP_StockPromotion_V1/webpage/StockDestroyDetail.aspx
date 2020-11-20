<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockDestroyDetail.aspx.cs" 
    Inherits="AP_StockPromotion_V1.web.StockDestroyDetail"  MasterPageFile="~/Master/MasterPopup.Master" %>

<asp:Content ID="Contenthead" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
    </style>
    <script type="text/javascript">

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="col-sm-12" style="font-weight:bold;">
        <div class="col-sm-2" style="text-align:right;">
            คลังสินค้าโปรโมชั่น: 
        </div>
        <div class="col-sm-2">
            <asp:Label ID="lbStock" runat="server" Text="Centro รามอินทรา 109"></asp:Label>
        </div>
        <div class="col-sm-2" style="text-align:right;">
            สินค้าโปรโมชั่น: 
        </div>
        <div class="col-sm-4">
            <asp:Label ID="lbItem" runat="server" Text="Gift Voucher Starbuck มูลค่า 3000 บาท"></asp:Label>
        </div>
    </div>


    <div class="col-sm-12">&nbsp;</div>

    <div class="col-sm-12">
        <asp:GridView ID="grdData" runat="server" AutoGenerateColumns="False" CellPadding="4"
            ForeColor="#333333" GridLines="None" class="col-sm-12" OnRowDataBound="grdData_RowDataBound" OnRowCommand="grdData_RowCommand">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="SerialNo" HeaderText="Serial No." />
                <asp:TemplateField HeaderText="Barcode">
                    <ItemTemplate>
                        <asp:Label ID="grdtxtBarcode" runat="server" placeholder="Barcode" ></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ชื่อสินค้า">
                    <ItemTemplate>
                        <asp:Label ID="ItemName" runat="server" placeholder="ชื่อสินค้า"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="หน่วยนับ">
                    <ItemTemplate>
                        <asp:Label ID="grdtxtItemUnit" runat="server" placeholder="หน่วยนับ" ></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="วันหมดอายุ">
                    <ItemTemplate>
                        <asp:Label ID="grdtxtExpireDate" runat="server" placeholder="วันหมดอายุ"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="วันหมดประกัน">
                    <ItemTemplate>
                        <asp:Label ID="grdtxtEndOfWarranty" runat="server" placeholder="สิ้นสุดประกัน" ></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="หมายเหตุ">
                    <ItemTemplate>
                        <asp:Label ID="grdtxtRemark" runat="server" placeholder="หมายเหตุ" Text="หมดอายุ" ></asp:Label>
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
