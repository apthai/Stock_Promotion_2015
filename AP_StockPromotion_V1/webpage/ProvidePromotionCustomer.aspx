<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProvidePromotionCustomer.aspx.cs" 
    Inherits="AP_StockPromotion_V1.web.ProvidePromotionCustomer" MasterPageFile="~/Master/MasterPage.Master" %>


<asp:Content ID="Contenthead" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">

    </style>
    <script type="text/javascript">
        
        function popupProvidePromotionCustomerEdit(ItemCountMethod) {
            Popup80('ProvidePromotionCustomerEdit.aspx?ItemCountMethod=' + ItemCountMethod);
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
                
                <div class="col-sm-12" ></div>

                <div class="col-sm-2" >
                    <asp:Label ID="Label1" runat="server" Text="แปลง/ยูนิตเลขที่: "></asp:Label>
                </div>
                <div class="col-sm-4" style="text-align:left;">
                    <asp:TextBox ID="txtUnitNo" runat="server"></asp:TextBox>
                </div>
                <div class="col-sm-2">
                    <asp:Label ID="Label2" runat="server" Text="Promotion:"></asp:Label>
                </div>
                <div class="col-sm-4" style="text-align: left;">
                    <asp:RadioButton ID="rdbSell" runat="server" GroupName="StepPromotion" Text="Promotion ขาย" Checked="true" />&nbsp;&nbsp;
                </div>

                <div class="col-sm-12" ></div>

                <div class="col-sm-2" >
                    <asp:Label ID="Label5" runat="server" Text="ชื่อ-นามสกุล ลูกค้า: "></asp:Label>
                </div>
                <div class="col-sm-6" style="text-align:left;">
                    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                </div>
                <div class="col-sm-4" style="text-align: left;">
                    <asp:RadioButton ID="rdbTran" runat="server" GroupName="StepPromotion" Text="Promotion โอน" Checked="false" />
                </div>
                                
                <div class="col-sm-12" ></div>

                <div class="col-sm-2" >
                    <asp:Label ID="Label3" runat="server" Text="เลขที่จอง: "></asp:Label>
                </div>
                <div class="col-sm-4" style="text-align:left;">
                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                </div>
                <div class="col-sm-2">
                    <asp:Label ID="Label4" runat="server" Text="วันที่จอง:"></asp:Label>
                </div>
                <div class="col-sm-4" style="text-align: left;">                    
                    <div class="input-group input-group-sm">
                        <asp:TextBox runat="server" type="text" ID="txtBegDate" class="form-control hasDatepicker" Text="2015-02-01" />
                        <span class="input-group-addon">
                            <i class="ace-icon fa fa-calendar"></i>
                        </span>
                    </div>
                </div>
                
                <div class="col-sm-12" >&nbsp;&nbsp; </div>

                <div class="col-sm-12">
                    <asp:GridView ID="grdData" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" Width="100%" OnRowCommand="grdData_RowCommand">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:BoundField DataField="Item" HeaderText="สินค้าโปรโมชั่น" />
                            <asp:BoundField DataField="Balance" HeaderText="จำนวนคงเหลือ" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right" />
                            <asp:TemplateField HeaderText="จำนวนเบิกจ่าย" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtProvide" runat="server" Width="50px" style="text-align:right;" Text='<%# Eval("Amount") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ให้โปรโมชั่น" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgEdit" runat="server" ImageUrl="~/img/ToCustomer.png" Width="23px" Style="vertical-align: baseline;" CommandName="ProvidePromotionCustomer" CommandArgument='<%# Eval("ItemCouontMethod") %>' />
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

                <div class="col-sm-12" style="text-align:center;" >&nbsp;&nbsp;</div>

                <div class="col-sm-12" style="text-align:center;" >
                    <asp:Button ID="Button1" runat="server" class="btn btn-white btn-sm btn-primary" Text="ตกลง" Width="75px" OnClick="Button1_Click" />&nbsp;&nbsp;                    
                    <asp:Button ID="Button2" runat="server" class="btn btn-white btn-sm btn-primary" Text="ยกเลิก" Width="75px" OnClick="Button2_Click" />
                </div>

            </td>
            <td></td>
        </tr>


    </table>

</asp:Content>

