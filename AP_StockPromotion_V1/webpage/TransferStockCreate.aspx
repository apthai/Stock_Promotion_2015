<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TransferStockCreate.aspx.cs" 
    Inherits="AP_StockPromotion_V1.web.TransferStockCreate" MasterPageFile="~/Master/MasterPopup.Master"  %>

<asp:Content ID="Contenthead" ContentPlaceHolderId="head" runat="server">
    <style type="text/css">

    </style>
    <script type="text/javascript">

        function popupTransferStockDetail(mode,ItemCountMethod) {
            Popup80('StockTransferDetail.aspx?mode=' + mode + '&ItemCountMethod=' + ItemCountMethod);
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

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="col-sm-12">
        <div class="col-sm-2" style="text-align: right;">โอนจาก: </div>
        <div class="col-sm-3">
            <asp:DropDownList ID="ddlStock" runat="server" class="col-sm-12">
                <asp:ListItem Text="The City งามวงศ์วาน" Value="1"></asp:ListItem>
                <asp:ListItem Text="The City รามอินทรา" Value="2"></asp:ListItem>
                <asp:ListItem Text="Centro รามอินทรา 109" Value="3"></asp:ListItem>
                <asp:ListItem Text="Centro อ่อนนุช" Value="4"></asp:ListItem>
                <asp:ListItem Text="Centro พระราม 9" Value="5"></asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="col-sm-2" style="text-align: right;">ไปยัง: </div>
        <div class="col-sm-3">
            <asp:DropDownList ID="DropDownList1" runat="server" class="col-sm-12">
                <asp:ListItem Text="The City งามวงศ์วาน" Value="1"></asp:ListItem>
                <asp:ListItem Text="The City รามอินทรา" Value="2"></asp:ListItem>
                <asp:ListItem Text="Centro รามอินทรา 109" Value="3"></asp:ListItem>
                <asp:ListItem Text="Centro อ่อนนุช" Value="4"></asp:ListItem>
                <asp:ListItem Text="Centro พระราม 9" Value="5"></asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="col-sm-2" style="text-align: right;"></div>
    </div>
    <div class="col-sm-12" style="text-align: right;"></div>
    

    <div class="col-sm-12">
        <asp:GridView ID="grdData" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" Width="100%" OnRowCommand="grdData_RowCommand" OnRowDataBound="grdData_RowDataBound">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="Item" HeaderText="สินค้าโปรโมชั่น" />
                <asp:BoundField DataField="Amount" HeaderText="ยอดโอน" HeaderStyle-Width="75px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center"/>
                <asp:BoundField DataField="Balance" HeaderText="คงเหลือ" HeaderStyle-Width="75px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center"/>
                <asp:TemplateField HeaderText="ส่งคืนคลังหลัก" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:ImageButton ID="imgTransfer" runat="server" ImageUrl="~/img/transfer_right_left.png" Width="23px" Style="vertical-align: baseline;" 
                            CommandName="StockTransferDetailCreate" CommandArgument='<%# Eval("ItemCouontMethod") %>' />                        
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

    
    <div class="col-sm-12" style="text-align: right;" id="div1" runat="server"></div>
    <div class="col-sm-12" style="text-align: right;" id="div_btnAccept" runat="server">
        <button class="btn btn-white btn-info btn-sm">
            <i class="ace-icon fa fa-floppy-o bigger-120 blue"></i>บันทึก
        </button>
    </div>
</asp:Content>