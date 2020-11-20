<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProvidePromotionNonCustomerDetail.aspx.cs" 
    Inherits="AP_StockPromotion_V1.web.ProvidePromotionNonCustomerDetail" MasterPageFile="~/Master/MasterPopup.Master" %>


<asp:Content ID="Contenthead" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">

    </style>
    <script type="text/javascript">

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
                    <asp:Label ID="lb_To" runat="server" Text="บริษัท: " Font-Bold="true"></asp:Label>
                </div>
                <div class="col-sm-4">
                    <asp:Label ID="lbCompany" runat="server" Text="บจก.เอเชี่ยน พร็อพเพอร์ตี้" Font-Bold="true"></asp:Label>
                </div>
                <div class="col-sm-2">
                    <asp:Label ID="lb_Subject" runat="server" Text="โครงการ: " Font-Bold="true"></asp:Label>
                </div>
                <div class="col-sm-4">
                    <asp:Label ID="lbProject" runat="server" Text="The City งามวงศ์วาน" Font-Bold="true"></asp:Label>                    
                </div>
                

                <div class="col-sm-2" >
                    <asp:Label ID="Label1" runat="server" Text="ช่วงเวลาโปรโมชั่น: " Font-Bold="true"></asp:Label>
                </div>
                <div class="col-sm-4" style="text-align:left;">
                    <asp:Label ID="Label3" runat="server" Text="2015-02-01" Font-Bold="true"></asp:Label>
                </div>
                <div class="col-sm-2">
                    <asp:Label ID="Label2" runat="server" Text=" ถึง " Font-Bold="true"></asp:Label>
                </div>
                <div class="col-sm-4" style="text-align:left;">
                    <asp:Label ID="Label4" runat="server" Text="2015-03-31" Font-Bold="true"></asp:Label>
                </div>
                

                <div class="col-sm-2" >
                    <asp:Label ID="Label5" runat="server" Text="สินค้า: " Font-Bold="true"></asp:Label>
                </div>
                <div class="col-sm-10" style="text-align:left;">
                    <asp:Label ID="Label6" runat="server" Text="[Gift Voucher] Major มูลค่า 200 บาท" Font-Bold="true"></asp:Label>
                </div>
                
                
                <div class="col-sm-2">
                    <asp:Label ID="Label7" runat="server" Text="ยอดเบิก:" Font-Bold="true"></asp:Label>
                </div>
                <div class="col-sm-4" style="text-align:left;">
                    <asp:Label ID="Label8" runat="server" Text="750" Font-Bold="true"></asp:Label>
                </div>
                <div class="col-sm-2">
                    <asp:Label ID="Label9" runat="server" Text="คงเหลือ:" Font-Bold="true"></asp:Label>
                </div>
                <div class="col-sm-4" style="text-align:left;">
                    <asp:Label ID="Label10" runat="server" Text="701" Font-Bold="true"></asp:Label>
                </div>
                
                <div class="col-sm-12" style="border-bottom:1px solid;">&nbsp;&nbsp; </div>
                <div class="col-sm-12" >&nbsp;&nbsp; </div>
                
                <div class="col-sm-2">
                    <asp:Label ID="Label11" runat="server" Text="เลขบัตรประชาชน: " ></asp:Label>                    
                </div>
                <div class="col-sm-4">
                    <asp:TextBox ID="txtProvideToId" runat="server"></asp:TextBox>                
                </div>
                <div class="col-sm-2">
                    <asp:Label ID="Label13" runat="server" Text="ชื่อ-นามสกุล: " ></asp:Label>                    
                </div>
                <div class="col-sm-4">
                    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>                
                </div>
                <div class="col-sm-2">
                    <asp:Label ID="Label12" runat="server" Text="เบอร์โทร: " ></asp:Label>                    
                </div>
                <div class="col-sm-4">
                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>                
                </div>
                <div class="col-sm-2">
                    <asp:Label ID="Label14" runat="server" Text="เลขที่อ้างอิง: " ></asp:Label>                    
                </div>
                <div class="col-sm-4">
                    <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>   
                    &nbsp;
                    <button class="btn btn-white btn-info btn-bold" >
						<i class="ace-icon fa glyphicon-plus smaller-75 blue"></i> 
					</button>
                </div>
                
                <div class="col-sm-12" >&nbsp;&nbsp; </div>

                <div class="col-sm-12" style="height:250px; min-height:250px;">
                    <asp:GridView ID="grdData" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" Width="100%">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:BoundField DataField="ProvideDate" HeaderText="วัน-เวลา" />
                            <asp:BoundField DataField="ProvideToId" HeaderText="เลขบัตรประชาชน" />
                            <asp:BoundField DataField="ProvideToName" HeaderText="ชื่อ-นามสกุลผู้รับ" />
                            <asp:BoundField DataField="ProvideToTel" HeaderText="หมายเลขโทรศัพท์" />
                            <asp:BoundField DataField="ProvideItemRefNo" HeaderText="เลขที่อ้างอิง" />
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

                <div class="col-sm-12" >&nbsp;&nbsp; </div>

                <div class="col-sm-12">                     
                    <asp:Button ID="Button1" runat="server" class="btn btn-white btn-sm btn-primary" Text="ตกลง" Width="75px" OnClick="Button1_Click" />&nbsp;&nbsp;                    
                    <asp:Button ID="Button2" runat="server" class="btn btn-white btn-sm btn-primary" Text="ยกเลิก" Width="75px" OnClick="Button2_Click" />
                </div>

            </td>
            <td></td>
        </tr>


    </table>

</asp:Content>

