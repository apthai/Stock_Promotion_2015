<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeliveryLowPriceItemDetail.aspx.cs"
    Inherits="AP_StockPromotion_V1.webpage.DeliveryLowPriceItemDetail" MasterPageFile="~/Master/MasterPopup.Master" %>

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
            $('#divNavx').html('รายละเอียดการส่งมอบสินค้าให้ลูกค้า');
            $(".js-example-basic-single").select2({ width: '100%' });
            $('.js-example-basic-single-ws').select2({ minimumResultsForSearch: Infinity, width: '100%' });
        });

        function closePage() {
            parent.jQuery.colorbox.close();
        }

        function bindDataParentPage() {
            parent.$("#ContentPlaceHolder1_btnSearch").click();
            closePage();
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdfMasterItemId" runat="server" />
    <div class="col-sm-12">
        <div class="col-sm-12">
            <div class="col-sm-2 div-caption">
                <asp:Label ID="lbItemNo" runat="server" Text="เลขที่เอกสาร" class="col-sm-12 label-caption"></asp:Label>
            </div>
            <div class="col-sm-3">
                <asp:TextBox ID="txtDelvNo" runat="server" ReadOnly="true" class="col-sm-12"></asp:TextBox>
            </div>
            <div class="col-sm-2 div-caption">
                <asp:Label ID="lbItemName" runat="server" Text="สถานะ" class="col-sm-12 label-caption"></asp:Label>
            </div>
            <div class="col-sm-3">
                <asp:TextBox ID="txtDelvStatus" runat="server" ReadOnly="true" class="col-sm-12"></asp:TextBox>
            </div>
            <div class="col-sm-2 div-caption">
                &nbsp;
            </div>
        </div>
        <div class="col-sm-12">
            <div class="col-sm-2 div-caption">	
                <asp:Label ID="lbItemCostBeg" runat="server" Text="ผู้ดำเนินการ" class="col-sm-12 label-caption"></asp:Label>
            </div>
            <div class="col-sm-3">
                <asp:TextBox ID="txtDelvBy" runat="server" ReadOnly="true" class="col-sm-12"></asp:TextBox>
            </div>
            <div class="col-sm-2 div-caption">
                <asp:Label ID="lbItemCostBegIncVat" runat="server" Text="วันที่" class="col-sm-12 label-caption"></asp:Label>
            </div>
            <div class="col-sm-3">
                <asp:TextBox ID="txtDelvDate" runat="server" ReadOnly="true" class="col-sm-12"></asp:TextBox>
            </div>
            <div class="col-sm-2 div-caption">
                &nbsp;
            </div>
        </div>
        <div class="col-sm-12">&nbsp;</div>
    </div>

    <div class="col-sm-12">
        <asp:GridView ID="grdData" runat="server" AutoGenerateColumns="False" CellPadding="4"
            ForeColor="#333333" GridLines="None" class="col-sm-12">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="ItemName" HeaderText="ชื่อสินค้า" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Model" HeaderText="รุ่น" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Color" HeaderText="สี" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Price" HeaderText="มูลค่า" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}" />			
                <asp:BoundField DataField="ProduceDate" HeaderText="วันผลิต" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="71px" />
                <asp:BoundField DataField="ExpireDate" HeaderText="วันหมดอายุ" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="71px"  />
                <asp:BoundField DataField="Detail" HeaderText="รายละเอียด" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Remark" HeaderText="หมายเหตุ" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Amount" HeaderText="จำนวน" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="71px" />
                <asp:BoundField DataField="Total" HeaderText="รวมมูลค่า" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}" />
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
        <asp:Button ID="btnDelDeliv" runat="server" Text="ยกเลิกใบส่งมอบ" class="btn btn-white btn-danger btn-sm" Width="175px" OnClientClick="if (!confirm('คุณต้องการลบใบส่งมอบนี้ ใช่หรือไม่?')) return false;"  OnClick="btnDelDeliv_Click" />
    </div>
</asp:Content>
