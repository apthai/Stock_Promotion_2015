<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockDeliveryItemEditCheckAmount.aspx.cs" 
    Inherits="AP_StockPromotion_V1.webpage.StockDeliveryItemEditCheckAmount" MasterPageFile="~/Master/MasterPopup.Master" %>

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
            $('#divNavx').html('คืนสินค้าโปรโมชั่น [ระบุจำนวน]');
            $(".js-example-basic-single").select2({ width: '100%' });
            $('.js-example-basic-single-ws').select2({ minimumResultsForSearch: Infinity, width: '100%' });
            setDivItemListHeight();

            setTextNumericOnly();
            setTextDigitOnly();
        });


        function setTextNumericOnly() {
            jQuery('.numericOnly').keyup(function () {
                this.value = this.value.replace(/[^0-9]/g, '');
            });
        }

        function setTextDigitOnly() {
            jQuery('.digitOnly').keyup(function () {
                this.value = this.value.replace(/[^0-9\.]/g, '');
            });
        }

        function setDivItemListHeight() {
            $('#divItemList').height($(window).height() - 200);
        }

        function initGridScroll() {
            //$().Scrollable({
            //    ScrollHeight: (($(window).height() / 100 * 70) - 100),
            //    IsInUpdatePanel: false
            //});
        }

        function sendDataItemAmountList(itemList) {
            //$("#ContentPlaceHolder1_hdfItemIdList", opener.document).val(itemList);
            //$("#ContentPlaceHolder1_btnCheckItemByAmount", opener.document).click();
            //window.close();
        }

        function checkReturnLimit(hdfLimitId, _inpId) {
            var lim = parseInt($('#' + hdfLimitId + '').val());
            var amt = parseInt($('#' + _inpId + '').val());
            if (lim < amt) {
                $('#' + _inpId + '').val(lim + '');
            }
        }


        function btnSelectAmtClick() {
            parent.$("#ContentPlaceHolder1_btnSelectAmt").click();
            window.close();
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdfMasterItemId" runat="server" />
    <div class="col-sm-12">        
        <asp:Label ID="lbMasterItemName" runat="server" style="font-size:xx-large; font-weight:bold;"></asp:Label>
    </div>

    <div class="col-sm-12" id="divItemList">
        <asp:GridView ID="grdData" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" class="col-sm-12"
            EmptyDataText="No data." ShowHeaderWhenEmpty="true" OnRowDataBound="grdData_RowDataBound">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="ItemName" HeaderText="สินค้าโปรโมชั่น" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Model" HeaderText="รุ่น" HeaderStyle-HorizontalAlign="Center" Visible="false" />
                <asp:BoundField DataField="Color" HeaderText="สี" HeaderStyle-HorizontalAlign="Center" Visible="false" />
                <asp:TemplateField HeaderText="ขนาด(กว้างxยาวxสูง)" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="grdHdfLbDimension" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="น้ำหนัก" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="grdHdfLbWeight" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Price" HeaderText="ราคา" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}" />
                <asp:BoundField DataField="ProduceDate" HeaderText="ProduceDate" HeaderStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" Visible="false" />
                <asp:BoundField DataField="ExpireDate" HeaderText="ExpireDate" HeaderStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="Detail" HeaderText="Detail" HeaderStyle-HorizontalAlign="Center" Visible="false" />
                <asp:BoundField DataField="Remark" HeaderText="Remark" HeaderStyle-HorizontalAlign="Center" Visible="false" />
                <asp:BoundField DataField="ReqDocNo" HeaderText="เลขที่ใบเบิก" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="UnitNo" HeaderText="แปลง/ห้อง" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Amt" HeaderText="Amount" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />

                <asp:TemplateField HeaderText="จำนวน" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:TextBox ID="grdTxtSelectAmount" runat="server" placeholder="0" Width="36px" class="label-caption numericOnly" Text='<%# Eval("DelvAmt") %>'></asp:TextBox>
                        <asp:HiddenField runat="server" ID="grdHdfBarcode" Value='<%# Eval("Barcode") %>' />
                        <asp:HiddenField runat="server" ID="grdHdfItemName" Value='<%# Eval("ItemName") %>' />
                        <asp:HiddenField runat="server" ID="grdHdfModel" Value='<%# Eval("Model") %>' />
                        <asp:HiddenField runat="server" ID="grdHdfColor" Value='<%# Eval("Color") %>' />
                        <asp:HiddenField runat="server" ID="grdHdfDimensionWidth" Value='<%# Eval("DimensionWidth") %>' />
                        <asp:HiddenField runat="server" ID="grdHdfDimensionLong" Value='<%# Eval("DimensionLong") %>' />
                        <asp:HiddenField runat="server" ID="grdHdfDimensionHeight" Value='<%# Eval("DimensionHeight") %>' />
                        <asp:HiddenField runat="server" ID="grdHdfDimensionUnit" Value='<%# Eval("DimensionUnit") %>' />
                        <asp:HiddenField runat="server" ID="grdHdfWeight" Value='<%# Eval("Weight") %>' />
                        <asp:HiddenField runat="server" ID="grdHdfWeightUnit" Value='<%# Eval("WeightUnit") %>' />
                        <asp:HiddenField runat="server" ID="grdHdfPrice" Value='<%# Eval("Price") %>' />
                        <asp:HiddenField runat="server" ID="grdHdfProduceDate" Value='<%# Eval("ProduceDate") %>' />
                        <asp:HiddenField runat="server" ID="grdHdfExpireDate" Value='<%# Eval("ExpireDate") %>' />
                        <asp:HiddenField runat="server" ID="grdHdfDetail" Value='<%# Eval("Detail") %>' />
                        <asp:HiddenField runat="server" ID="grdHdfRemark" Value='<%# Eval("Remark") %>' />
                        <asp:HiddenField runat="server" ID="grdHdfAmt" Value='<%# Eval("Amt") %>' />
                        <asp:HiddenField runat="server" ID="grdHdfRetAmt" Value='<%# Eval("DelvAmt") %>' />
                        <asp:HiddenField runat="server" ID="grdHdfReqDocNo" Value='<%# Eval("ReqDocNo") %>' />
                        <asp:HiddenField runat="server" ID="grdHdfUnitNo" Value='<%# Eval("UnitNo") %>' />

                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>ไม่พบรายการสินค้าโปรโมชั่น</EmptyDataTemplate>
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
    
    <div class="col-sm-12">&nbsp;</div>
    <div class="col-sm-12 div-caption">
        <asp:Button ID="btnOK" runat="server" class="btn btn-white btn-info btn-sm" Text="OK" Width="100px" OnClick="btnOK_Click" />
    </div>
</asp:Content>
