<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProvidePromotionNonCustomer.aspx.cs" 
    Inherits="AP_StockPromotion_V1.web.ProvidePromotionNonCustomer" MasterPageFile="~/Master/MasterPage.Master" %>


<asp:Content ID="Contenthead" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">

    </style>
    <script type="text/javascript">

        function popupProvidePromotionNonCustomerDetail() {
            Popup80('ProvidePromotionNonCustomerDetail.aspx');
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

    
    <table style="width: 100%; padding-left:120px;">
        <!-- | Header | -->
        <!-- | Content | -->
        <tr>
            <td></td>
            <td>
                <div class="col-sm-2">
                    <asp:Label ID="lb_To" runat="server" Text="บริษัท: "></asp:Label>
                </div>
                <div class="col-sm-4">
                    <asp:DropDownList ID="DropDownList1" runat="server">
                        <asp:ListItem Text="บจก.เอเชี่ยน พร็อพเพอร์ตี้" Value="1"></asp:ListItem>
                        <asp:ListItem Text="บมจ.เอพี (ไทยแลนด์)" Value="2"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-sm-2">
                    <asp:Label ID="lb_Subject" runat="server" Text="โครงการ: "></asp:Label>
                </div>
                <div class="col-sm-4">
                    <asp:DropDownList ID="DropDownList2" runat="server">
                        <asp:ListItem Text="The City งามวงศ์วาน" Value="1"></asp:ListItem>
                        <asp:ListItem Text="The City รามอินทรา" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Centro รามอินทรา 109" Value="3"></asp:ListItem>
                        <asp:ListItem Text="Centro อ่อนนุช" Value="4"></asp:ListItem>
                        <asp:ListItem Text="Centro พระราม 9" Value="5"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                
                <div class="col-sm-12">&nbsp; </div>

                <div class="col-sm-2" >
                    <asp:Label ID="Label1" runat="server" Text="ช่วงเวลาโปรโมชั่น: "></asp:Label>
                </div>
                <div class="col-sm-4" style="text-align:left;">
                    <div class="input-group input-group-sm">
                        <asp:TextBox runat="server" type="text" ID="txtBegDate" class="form-control hasDatepicker" Text="2015-02-01" />
                        <span class="input-group-addon">
                            <i class="ace-icon fa fa-calendar"></i>
                        </span>
                    </div>
                </div>
                <div class="col-sm-2">
                    <asp:Label ID="Label2" runat="server" Text=" ถึง "></asp:Label>
                </div>
                <div class="col-sm-4" style="text-align:left;">
                    <div class="input-group input-group-sm">
                            <asp:TextBox runat="server" type="text" id="txtEndgDate" class="form-control hasDatepicker" Text="2015-03-31" />
                            <span class="input-group-addon">
                                <i class="ace-icon fa fa-calendar"></i>
                            </span>
                        </div>
                </div>
                
                <div class="col-sm-12">&nbsp;&nbsp; </div>

                <div class="col-sm-12">
                    <asp:GridView ID="grdData" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" Width="100%" OnRowCommand="grdData_RowCommand">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:BoundField DataField="Item" HeaderText="สินค้าโปรโมชั่น" />
                            <asp:BoundField DataField="Balance" HeaderText="คงเหลือ" HeaderStyle-Width="150px" />
                            <asp:TemplateField HeaderText="ให้โปรโมชั่น" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgEdit" runat="server" ImageUrl="~/img/ToCustomer.png" Width="23px" Style="vertical-align: baseline;" CommandName="ProvidePromotionNonCustomerDetail" />
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

            </td>
            <td></td>
        </tr>


    </table>

</asp:Content>

