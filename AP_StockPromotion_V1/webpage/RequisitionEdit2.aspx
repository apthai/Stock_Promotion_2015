<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RequisitionEdit2.aspx.cs" 
    Inherits="AP_StockPromotion_V1.web.RequisitionEdit2" MasterPageFile="~/Master/MasterPopup.Master"  %>

<asp:Content ID="Contenthead" ContentPlaceHolderId="head" runat="server">
    <style type="text/css">

    </style>
    <script type="text/javascript">
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderId="ContentPlaceHolder1" runat="server">
    
    <div class="col-sm-12">
        <div class="col-sm-3" style="text-align:right;">
            เอกสาร: 
        </div>
        <div class="col-sm-9">
            <asp:TextBox ID="txtRefDocNo" runat="server" placeholder="เอกสารอ้างอิง" class="col-sm-6"></asp:TextBox>
        </div>
    </div>
    <div class="col-sm-12">
        <div class="col-sm-3" style="text-align:right;">
            โครงการ: 
        </div>
        <div class="col-sm-9">
            <asp:DropDownList ID="ddlProject" runat="server" class="col-sm-6">
                <asp:ListItem Text="The City งามวงศ์วาน" Value="1"></asp:ListItem>
                <asp:ListItem Text="The City รามอินทรา" Value="2"></asp:ListItem>
                <asp:ListItem Text="Centro รามอินทรา 109" Value="3"></asp:ListItem>
                <asp:ListItem Text="Centro อ่อนนุช" Value="4"></asp:ListItem>
                <asp:ListItem Text="Centro พระราม 9" Value="5"></asp:ListItem>
            </asp:DropDownList>
        </div>
    </div>
    <div class="col-sm-12">
        <div class="col-sm-3" style="text-align:right;">
            สินค้าโปรโมชั่น: 
        </div>
        <div class="col-sm-9">
            <asp:DropDownList ID="ddlItem" runat="server" class="col-sm-6">
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
    </div>
    <div class="col-sm-12">
        <div class="col-sm-3" style="text-align:right;">
            จำนวน: 
        </div>
        <div class="col-sm-9">
            <asp:TextBox ID="txtRequisitionAmount" runat="server" Style="text-align: right;" placeholder="0" class="col-sm-6"></asp:TextBox>
        </div>
    </div>
    <div class="col-sm-12">
        <div class="col-sm-3" style="text-align:right;">
            มูลค่า: 
        </div>
        <div class="col-sm-9">
            <asp:TextBox ID="txtCost" runat="server" Style="text-align: right;" ReadOnly="true" placeholder="0.00" class="col-sm-6"></asp:TextBox>
        </div>
    </div>
    <div class="col-sm-12">
        <div class="col-sm-3" style="text-align:right;">
            จำนวนเงิน: 
        </div>
        <div class="col-sm-9">
            <asp:TextBox ID="txtTotalCost" runat="server" Style="text-align: right;" ReadOnly="true" placeholder="0.00" class="col-sm-6"></asp:TextBox>
        </div>
    </div>
    <div class="col-sm-12">
        <div class="col-sm-3" style="text-align:right;">

        </div>
        <div class="col-sm-9">

        </div>
    </div>
    <div class="col-sm-12">
        <div class="col-sm-3" style="text-align:right;">

        </div>
        <div class="col-sm-9">

        </div>
    </div>
    <div class="col-sm-12">
        <div class="col-sm-3" style="text-align:right;">

        </div>
        <div class="col-sm-9">

        </div>
    </div>
    <div class="col-sm-12" style="text-align: right;">&nbsp;</div>
    <div class="col-sm-12" style="text-align: center;">
        
        <button class="btn btn-white btn-success btn-sm" onclick="popupStockReceiveCreate();">
            <i class="ace-icon fa fa-floppy-o bigger-120 green"></i>บันทึก
        </button>
        
            &nbsp;
            &nbsp;
        
        <button class="btn btn-white btn-warning btn-sm" onclick="popupStockReceiveCreate();">
            <i class="ace-icon fa fa-undo bigger-120 orange"></i>ยกเลิก
        </button>
    </div>
</asp:Content>