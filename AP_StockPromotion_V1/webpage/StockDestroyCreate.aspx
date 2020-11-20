<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockDestroyCreate.aspx.cs"
    Inherits="AP_StockPromotion_V1.web.StockDestroyCreate" MasterPageFile="~/Master/MasterPopup.Master" %>

<asp:Content ID="Contenthead" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
    </style>
    <script type="text/javascript">

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="col-sm-12">
        <div class="col-sm-2" style="text-align:right;">
            คลังสินค้าโปรโมชั่น: 
        </div>
        <div class="col-sm-2">
            <asp:DropDownList ID="ddlStock" runat="server" class="col-sm-12">
                <asp:ListItem Text="คลังหลัก" Value=""></asp:ListItem>
                <asp:ListItem Text="The City งามวงศ์วาน" Value="1"></asp:ListItem>
                <asp:ListItem Text="The City รามอินทรา" Value="2"></asp:ListItem>
                <asp:ListItem Text="Centro รามอินทรา 109" Value="3"></asp:ListItem>
                <asp:ListItem Text="Centro อ่อนนุช" Value="4"></asp:ListItem>
                <asp:ListItem Text="Centro พระราม 9" Value="5"></asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="col-sm-2" style="text-align:right;">
            สินค้าโปรโมชั่น: 
        </div>
        <div class="col-sm-4">
            <asp:DropDownList ID="ddlItem" runat="server" class="col-sm-12">
                <asp:ListItem Text="Gift Voucher Starbuck มูลค่า 1000 บาท" Value="1"></asp:ListItem>
                <asp:ListItem Text="Gift Voucher Starbuck มูลค่า 3000 บาท" Value="2"></asp:ListItem>
                <asp:ListItem Text="Gift Voucher Major มูลค่า 1500 บาท" Value="3"></asp:ListItem>
                <asp:ListItem Text="Gift Voucher Major มูลค่า 3000 บาท" Value="4"></asp:ListItem>
                <asp:ListItem Text="Gift Voucher Lotus มูลค่า 2000 บาท" Value="5"></asp:ListItem>
                <asp:ListItem Text="Gift Voucher Lotus มูลค่า 3000 บาท" Value="6"></asp:ListItem>
                <asp:ListItem Text="iPhone6 16GB มูลค่า 25,500 บาท" Value="7"></asp:ListItem>
                <asp:ListItem Text="ทองคำแท่ง หนัก 10 บาท" Value="8"></asp:ListItem>
                <asp:ListItem Text="สร้อยคอทองคำ หนัก 5 บาท" Value="9"></asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="col-sm-2">
            <button class="btn btn-white btn-info btn-sm" onclick="popupRequisitionCreate();">
                <i class="ace-icon fa fa-pencil bigger-120 blue"></i>เลือก
            </button> 
        </div>
    </div>


    <div class="col-sm-12"></div>

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
                        <asp:TextBox ID="grdtxtRemark" runat="server" placeholder="หมายเหตุ" ></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="นำออก">
                    <ItemTemplate>
                        <asp:ImageButton ID="imgDel" runat="server" ImageUrl="~/img/trash_recyclebin_empty_closed.png" Width="23px" Style="vertical-align: baseline;" CommandName="destroyItem" />                        
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
    
    <div class="col-sm-12" style="text-align: right;" id="div_btnAccept" runat="server">
        <button class="btn btn-white btn-info btn-sm">
            <i class="ace-icon fa fa-floppy-o bigger-120 blue"></i>บันทึก
        </button>
    </div>

</asp:Content>
