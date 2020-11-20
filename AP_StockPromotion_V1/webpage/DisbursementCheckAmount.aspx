<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DisbursementCheckAmount.aspx.cs"
    Inherits="AP_StockPromotion_V1.webpage.DisbursementCheckAmount" MasterPageFile="~/Master/MasterPopup.Master" %>

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
            $('#divNavx').html('โอนสินค้าโปรโมชั่นเข้าโครงการ [ระบุจำนวน]');
            $(".js-example-basic-single").select2({ width: '100%' });
            $('.js-example-basic-single-ws').select2({ minimumResultsForSearch: Infinity, width: '100%' });
            setDivItemListHeight();
        });

        function reCalcTransClick() {
            $("#<%= btnReCalcTrans.ClientID %>").click();
        }
        
        function calcBalance(txtId, hdfId) {
            var inp = parseInt($('#' + txtId + '').val());
            var bal = parseInt($('#' + hdfId + '').val());
            if (isNaN(inp)) {
                inp = 0;
                $('#' + txtId + '').val(0);
            }
            if (inp > bal) {
                $('#' + txtId + '').val($('#' + hdfId + '').val());
            }
            // txtSelectAmount
            var sum = 0;
            var arrT = $('#<%=grdData.ClientID %>').find('input:text');
            $.each(arrT, function (index, t) {
                var x = parseInt(t.value);
                if (isNaN(x)) {
                    x = 0;
                }
                sum += x;
            });
            $('#ContentPlaceHolder1_grdRequest_grdLbTransfer_0').html(sum);
        }

        function setFocusControl(_inp) {
            $("#<%= hdfTextFocusTo.ClientID %>").val(document.activeElement.id);
        }

        function setFocusTextBox(v) {
            $('#' + v).focus();
        }

        function setDivItemListHeight() {
            $('#ivItemListHeight').height($(window).height() - 200);
        }

        function initGridScroll() {
            $('#<%=grdData.ClientID %>').Scrollable({
                ScrollHeight: (($(window).height() / 100 * 70) - 100),
                IsInUpdatePanel: false
            });
        }

        function sendDataItemAmountList(itemList, reqId) {
            parent.$("#ContentPlaceHolder1_hdfItemIdList").val(itemList);
            parent.$("#ContentPlaceHolder1_hdfReqIdAmount").val(reqId);
            parent.$("#ContentPlaceHolder1_btnCheckItemByAmount").click();
            window.close();
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdfReqId" runat="server" />
    <div class="col-sm-12">
        
        <asp:HiddenField ID="hdfMasterItemID" runat="server" />
        <asp:HiddenField ID="hdfTextFocusFrom" runat="server" />
        <asp:HiddenField ID="hdfTextFocusTo" runat="server" />
        <asp:HiddenField ID="hdfWaitAmount" runat="server" Value="0" />
        <asp:HiddenField ID="hdfTranAmount" runat="server" Value="0" />
        <asp:Button ID="btnReCalcTrans" runat="server" style="opacity:0; display:none; width:0; height:0;" OnClick="btnReCalcTrans_Click" />

        <asp:GridView ID="grdRequest" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" class="col-sm-12"
            EmptyDataText="No data." ShowHeader="false" OnRowCommand="grdRequest_RowCommand" OnRowDataBound="grdRequest_RowDataBound">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:TemplateField HeaderText="สินค้าโปรโมชั่น" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" FooterStyle-Font-Bold="true" ControlStyle-Font-Size="Large">
                    <ItemTemplate>
                        <asp:Label ID="grdLbItemName" runat="server" Text='<%# Eval("ItemName") + " (" + Eval("ItemUnit") + ")" %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Caption" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="100px">
                    <ItemTemplate>                        
                        <asp:Label ID="grdLb1" runat="server" Text="จำนวน :"></asp:Label><br />
                        <asp:Label ID="grdLb2" runat="server" Text="คงเหลือเบิก :"></asp:Label><br />
                        <asp:Label ID="grdLb3" runat="server" Text="จำนวนเบิก :" ForeColor="Red"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="DataVal" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="50px">
                    <ItemTemplate>
                        <asp:Label ID="grdLbTotalRequest" runat="server"></asp:Label><br />
                        <asp:Label ID="grdLbBalRequest" runat="server"></asp:Label><br />
                        <asp:Label ID="grdLbTransfer" runat="server" ForeColor="Red"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:HiddenField ID="grdHdfReqHeaderId" runat="server" Value='<%# Eval("ReqHeaderId") %>' />
                        <asp:HiddenField ID="grdHdfReqNo" runat="server" Value='<%# Eval("ReqNo") %>' />
                        <asp:HiddenField ID="grdHdfReqDate" runat="server" Value='<%# Eval("ReqDate") %>' />
                        <asp:HiddenField ID="grdHdfReqBy" runat="server" Value='<%# Eval("ReqBy") %>' />
                        <asp:HiddenField ID="grdHdfReqType" runat="server" Value='<%# Eval("ReqType") %>' />
                        <asp:HiddenField ID="grdHdfReqHeaderRemark" runat="server" Value='<%# Eval("ReqHeaderRemark") %>' />
                        <asp:HiddenField ID="grdHdfReqId" runat="server" Value='<%# Eval("ReqId") %>' />
                        <asp:HiddenField ID="grdHdfProject_Id" runat="server" Value='<%# Eval("Project_Id") %>' />
                        <asp:HiddenField ID="grdHdfProjectID" runat="server" Value='<%# Eval("ProjectID") %>' />
                        <asp:HiddenField ID="grdHdfItemId" runat="server" Value='<%# Eval("ItemId") %>' />
                        <asp:HiddenField ID="grdHdfItemNo" runat="server" Value='<%# Eval("ItemNo") %>' />
                        <asp:HiddenField ID="grdHdfItemName" runat="server" Value='<%# Eval("ItemName") %>' />
                        <asp:HiddenField ID="grdHdfReqAmount" runat="server" Value='<%# Eval("ReqAmount") %>' />
                        <asp:HiddenField ID="grdHdfItemUnit" runat="server" Value='<%# Eval("ItemUnit") %>' />
                        <asp:HiddenField ID="grdHdfItemCountMethod" runat="server" Value='<%# Eval("ItemCountMethod") %>' />
                        <asp:HiddenField ID="grdHdfTransferredAmount" runat="server" Value='<%# Eval("TransferredAmount") %>' />

                        <asp:HiddenField ID="grdHdfTransfer" runat="server" Value='<%# Eval("Transfer") %>' />
                        <asp:HiddenField ID="grdHdfBalance" runat="server" Value='<%# Eval("Balance") %>' />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
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

    <div class="col-sm-12" id="divItemList">
        <asp:GridView ID="grdData" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" class="col-sm-12"
            EmptyDataText="No data." ShowHeaderWhenEmpty="true" OnRowDataBound="grdData_RowDataBound">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="PO_No" HeaderText="PO_No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="GR_No" HeaderText="GR_No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="ItemName" HeaderText="ItemName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Model" HeaderText="Model" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Color" HeaderText="Color" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Price" HeaderText="Price" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="ExpireDate" DataFormatString="{0:dd/MM/yyyy}" HeaderText="ExpireDate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Detail" HeaderText="Detail" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Remark" HeaderText="Remark" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="StockBalance" HeaderText="StockBalance" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" />
                <asp:TemplateField HeaderText="จำนวน" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:TextBox ID="grdTxtSelectAmount" runat="server" placeholder="0" Width="36px" class="label-caption txtSelectAmount" onclick="setFocusControl(this);" ></asp:TextBox>
                        <asp:HiddenField ID="grdHdfReceiveHeaderID" runat="server" Value='<%# Eval("ReceiveHeaderID") %>' />
                        <asp:HiddenField ID="grdHdfPO_No" runat="server" Value='<%# Eval("PO_No") %>' />
                        <asp:HiddenField ID="grdHdfGR_No" runat="server" Value='<%# Eval("GR_No") %>' />
                        <asp:HiddenField ID="grdHdfReceiveDetailId" runat="server" Value='<%# Eval("ReceiveDetailId") %>' />
                        <asp:HiddenField ID="grdHdfItemName" runat="server" Value='<%# Eval("ItemName") %>' />
                        <asp:HiddenField ID="grdHdfModel" runat="server" Value='<%# Eval("Model") %>' />
                        <asp:HiddenField ID="grdHdfColor" runat="server" Value='<%# Eval("Color") %>' />
                        <asp:HiddenField ID="grdHdfDimensionWidth" runat="server" Value='<%# Eval("DimensionWidth") %>' />
                        <asp:HiddenField ID="grdHdfDimensionLong" runat="server" Value='<%# Eval("DimensionLong") %>' />
                        <asp:HiddenField ID="grdHdfDimensionHeight" runat="server" Value='<%# Eval("DimensionHeight") %>' />
                        <asp:HiddenField ID="grdHdfDimensionUnit" runat="server" Value='<%# Eval("DimensionUnit") %>' />
                        <asp:HiddenField ID="grdHdfWeight" runat="server" Value='<%# Eval("Weight") %>' />
                        <asp:HiddenField ID="grdHdfWeightUnit" runat="server" Value='<%# Eval("WeightUnit") %>' />
                        <asp:HiddenField ID="grdHdfPrice" runat="server" Value='<%# Eval("Price") %>' />
                        <asp:HiddenField ID="grdHdfProduceDate" runat="server" Value='<%# Eval("ProduceDate") %>' />
                        <asp:HiddenField ID="grdHdfExpireDate" runat="server" Value='<%# Eval("ExpireDate") %>' />
                        <asp:HiddenField ID="grdHdfDetail" runat="server" Value='<%# Eval("Detail") %>' />
                        <asp:HiddenField ID="grdHdfRemark" runat="server" Value='<%# Eval("Remark") %>' />
                        <asp:HiddenField ID="grdHdfStockBalance" runat="server" Value='<%# Eval("StockBalance") %>' />
                        <asp:HiddenField ID="grdHdfStockItemIdList" runat="server" Value='<%# Eval("StockItemIdList") %>' />
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
    <!--
                        <asp:BoundField DataField="CreateDate" HeaderText="CreateDate" HeaderStyle-HorizontalAlign="Center" />
                        <asp:HiddenField ID="grdHdfCreateBy" runat="server" Value='<%# Eval("CreateBy") %>' />
                        <asp:HiddenField ID="grdHdfCreateDate" runat="server" Value='<%# Eval("CreateDate") %>' />
        -->
    <div class="col-sm-12">&nbsp;</div>
    <div class="col-sm-12 div-caption">
        <asp:Button ID="btnOK" runat="server" class="btn btn-white btn-info btn-sm" Text="OK" Width="100px" OnClick="btnOK_Click" />
    </div>
</asp:Content>
