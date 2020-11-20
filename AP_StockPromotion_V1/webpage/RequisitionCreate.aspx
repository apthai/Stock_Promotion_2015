<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RequisitionCreate.aspx.cs" 
    Inherits="AP_StockPromotion_V1.web.RequisitionCreate" MasterPageFile="~/Master/MasterPopup.Master" %>

<asp:Content ID="Contenthead" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
    </style>
    <script type="text/javascript">

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="col-sm-12">
        <div class="col-sm-2" style="text-align:right;">
            เอกสารอ้างอิง: 
        </div>
        <div class="col-sm-3">
            <asp:TextBox ID="txtRefDocNo" runat="server" placeholder="เอกสารอ้างอิง" class="col-sm-12"></asp:TextBox></div>
        <div class="col-sm-2" style="text-align:right;">
            โครงการ: 
        </div>
        <div class="col-sm-3">
            <asp:DropDownList ID="ddlProject" runat="server" class="col-sm-12">
            </asp:DropDownList>
        </div>        
        <div class="col-sm-2"></div>
    </div>
    <div class="col-sm-12">        
        <div class="col-sm-2" style="text-align:right;">
            สินค้าโปรโมชั่น:
        </div>
        <div class="col-sm-3">
            <asp:DropDownList ID="ddlItem" runat="server" class="col-sm-12">
            </asp:DropDownList>
        </div>
        <div class="col-sm-2" style="text-align:right;">
            จำนวน:
        </div>
        <div class="col-sm-3">
            <asp:TextBox ID="txtRequisitionAmount" runat="server" Style="text-align: right;" placeholder="0" class="col-sm-6"></asp:TextBox>
            <div class="col-sm-6" style="text-align:right;">
                <button class="btn btn-white btn-info btn-sm">
                    <i class="ace-icon fa glyphicon-plus bigger-120 blue"></i>
                    เพิ่ม
                </button>
            </div>
        </div>
        <div class="col-sm-2"></div>
    </div>
    <div class="col-sm-12">
        <div class="col-sm-4">
        </div>
        <div class="col-sm-4">
            
        </div>

    </div>

    <div class="col-sm-12">&nbsp;</div>

    <div class="col-sm-12">
        <asp:GridView ID="grdData" runat="server" AutoGenerateColumns="False" CellPadding="4" 
            ForeColor="#333333" GridLines="None" class="col-sm-12">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="DocRefNo" HeaderText="เลขที่เอกสารอ้างอิง" HeaderStyle-Width="200px" />
                <asp:BoundField DataField="ProjectName" HeaderText="โครงการ" />
                <asp:BoundField DataField="ItemName" HeaderText="สินค้าโปรโมชั่น" />
                <asp:BoundField DataField="ReceiveAmount" HeaderText="จำนวน" />
                <asp:BoundField DataField="ItemUnit" HeaderText="หน่วย" />
                <asp:BoundField DataField="Cost" HeaderText="มูลค่า" />
                <asp:BoundField DataField="TotalCost" HeaderText="จำนวนเงินรวม" />
                <asp:TemplateField HeaderText="ลบ" ItemStyle-HorizontalAlign="Center"><%-- ItemStyle-HorizontalAlign="Center"--%>
                    <ItemTemplate>
                        <asp:ImageButton ID="imgDel" runat="server" ImageUrl="~/img/delete.png" Width="23px" Style="vertical-align: baseline;" /><!--OnClick="imgEdit_Click" -->
                        <asp:HiddenField ID="hdfProjectId" runat="server" Value='<%# Eval("ProjectName") %>' />
                        <asp:HiddenField ID="hdfItemId" runat="server" Value='<%# Eval("ItemId") %>' />
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
    <div class="col-sm-12" style="text-align: right;">
        <button class="btn btn-white btn-info btn-sm">

            <i class="ace-icon fa fa-floppy-o bigger-120 blue"></i>บันทึก
        </button>
    </div>

</asp:Content>
