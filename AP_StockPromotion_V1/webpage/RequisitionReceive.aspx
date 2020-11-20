<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RequisitionReceive.aspx.cs" 
    Inherits="AP_StockPromotion_V1.web.RequisitionReceive" MasterPageFile="~/Master/MasterPopup.Master"  %>

<asp:Content ID="Contenthead" ContentPlaceHolderId="head" runat="server">
    <style type="text/css">

    </style>
    <script type="text/javascript">
    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div id="div1" runat="server" class="col-sm-12">

        <div class="col-sm-12" style="font-weight: bold;">
            <div class="col-sm-1" style="text-align: right;">
                วันที่:
            </div>
            <div class="col-sm-2">
                <asp:Label ID="Label2" runat="server" Text="dd/MM/yyyy"></asp:Label>
            </div>
            <div class="col-sm-2" style="text-align: right;">
                ผู้เบิกสินค้าโปรโมชั่น: 
            </div>
            <div class="col-sm-3">
                <asp:Label ID="Label1" runat="server" Text="AP00XXXX"></asp:Label>
            </div>
            <div class="col-sm-2" style="text-align: right;">
                เลขที่เอกสารอ้างอิง: 
            </div>
            <div class="col-sm-2">
                <asp:Label ID="lbStockDest" runat="server" Text="MM:MemoYY03000001"></asp:Label>
            </div>
        </div>
        <div class="col-sm-12" style="font-weight: bold;">
            <div class="col-sm-1" style="text-align: right;">
                โครงการ:
            </div>
            <div class="col-sm-2">
                <asp:Label ID="Label3" runat="server" Text="The City รามอินทรา"></asp:Label>
            </div>
            <div class="col-sm-2" style="text-align: right;">
                สินค้าโปรโมชั่น: 
            </div>
            <div class="col-sm-3">
                <asp:Label ID="Label4" runat="server" Text="iPhone6 16GB มูลค่า 25,500 บาท"></asp:Label>
            </div>
            <div class="col-sm-2" style="text-align: right;">
                จำนวน:
            </div>
            <div class="col-sm-1">
                <asp:Label ID="Label5" runat="server" Text="50"></asp:Label>
            </div>
            <div class="col-sm-1">
                <asp:Label ID="Label6" runat="server" Text="เครื่อง"></asp:Label>
            </div>
        </div>
        <div class="col-sm-12" style="text-align: right; border-top: 1px solid #808080;">&nbsp;</div>

    </div>


    
    <div class="col-sm-12" style="text-align: right;">
        <asp:GridView ID="grdData" runat="server" AutoGenerateColumns="False" CellPadding="4"
            ForeColor="#333333" GridLines="None" class="col-sm-12">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="SerialNo" HeaderText="Serial No." />
                <asp:TemplateField HeaderText="Barcode">
                    <ItemTemplate>
                        <asp:TextBox ID="grdtxtBarcode" runat="server" placeholder="Barcode" ></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ชื่อสินค้า">
                    <ItemTemplate>
                        <asp:TextBox ID="ItemName" runat="server" placeholder="ชื่อสินค้า"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="หน่วยนับ">
                    <ItemTemplate>
                        <asp:TextBox ID="grdtxtItemUnit" runat="server" placeholder="หน่วยนับ" ></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="วันหมดอายุ">
                    <ItemTemplate>
                        <asp:TextBox ID="grdtxtExpireDate" runat="server" placeholder="วันหมดอายุ"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="วันหมดประกัน">
                    <ItemTemplate>
                        <asp:TextBox ID="grdtxtEndOfWarranty" runat="server" placeholder="สิ้นสุดประกัน" ></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="หมายเหตุ">
                    <ItemTemplate>
                        <asp:TextBox ID="grdtxtRemark" runat="server" placeholder="หมายเหตุ" ></asp:TextBox>
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
    <div class="col-sm-12" style="text-align: right;">&nbsp;</div>
    <div class="col-sm-12" style="text-align: right;" id="div_btnAccept" runat="server">
        <button class="btn btn-white btn-info btn-sm">
            <i class="ace-icon fa fa-check bigger-120 blue"></i>ได้รับแล้ว
        </button>
    </div>
</asp:Content>