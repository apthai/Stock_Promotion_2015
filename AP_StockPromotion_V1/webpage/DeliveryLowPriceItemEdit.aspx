<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeliveryLowPriceItemEdit.aspx.cs" 
    Inherits="AP_StockPromotion_V1.webpage.DeliveryLowPriceItemEdit" MasterPageFile="~/Master/MasterPage.Master"  %>

<asp:Content ID="Contenthead" ContentPlaceHolderId="head" runat="server">
    <style type="text/css">
        .label-caption {
            text-align:right;
        }
        .div-caption {
            text-align:right;
            padding-right:5px;
        }
    </style>
    <script type="text/javascript">
        jQuery(function ($) {
            $('#divNavx').html('เคลียร์เอกสารส่งมอบ (Marketing)');
            $(".js-example-basic-single").select2({ width: '100%' });
            $('.js-example-basic-single-ws').select2({ minimumResultsForSearch: Infinity, width: '100%' });
            initTextInput();
            setDatePicker();
        });

        function popupStockDeliveryItemEditCheckAmount(masterItemId) {
            Popup80('StockDeliveryItemEditCheckAmount.aspx?MasterItemId=' + masterItemId);
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


        function setDatePicker() {
            $("#<%= txtDocDate.ClientID %>").datepicker({ dateFormat: "dd/mm/yy" });
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


        function initTextInput() {
            $('#<%= txtSerial.ClientID %>').keypress(function (e) {
                if (e.which == 13) {
                    e.preventDefault();
                    $('#<%= imgCheckSerial.ClientID %>').click();
                }
            });
        }

        function closePage() {
            parent.jQuery.colorbox.close();
            // window.close();
        }

        function bindDataParentPage() {
            parent.$("#ContentPlaceHolder1_btnSearch").click();
            closePage();
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderId="ContentPlaceHolder1" runat="server">

    <asp:Button ID="btnDummy" runat="server" Text="" Width="0px" Height="0px" style="opacity:0.0;" Enabled="False" />
    <asp:Button ID="btnSelectAmt" runat="server" Text="" class="btn btn-white btn-info btn-sm" style="display:none;" OnClick="btnSelectAmt_Click" />



    <!-- #section:elements.accordion -->
    <div id="accordion" class="accordion-style1 panel-group">
        <div class="panel panel-default">
            <div class="panel-heading">
                <h4 class="panel-title">
                    <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion" href="#collapseOne">
                        <i class="ace-icon fa fa-angle-down bigger-110" data-icon-hide="ace-icon fa fa-angle-down" data-icon-show="ace-icon fa fa-angle-right"></i>
                        &nbsp;ค้นหารายละเอียดสินค้า
                    </a>
                </h4>
            </div>

            <div class="panel-collapse collapse in" id="collapseOne">
                <div class="panel-body">
                                        
                    <div class="col-sm-12">
                        <div class="col-sm-4">
                            <div class="col-sm-3 div-caption">โครงการ</div>
                            <div class="col-sm-9">
                                <asp:DropDownList ID="ddlProject" runat="server" class="col-sm-12 js-example-basic-single js-states form-control hide"></asp:DropDownList>
                                <asp:HiddenField ID="hdfProject" runat="server" />
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <div class="col-sm-5 div-caption">วันที่เอกสาร</div>
                            <div class="col-sm-7">
                                <asp:TextBox ID="txtDocDate" runat="server" placeholder="วันที่" class="col-sm-12"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="col-sm-4 div-caption">Internal Order</div>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlInternalOrder" runat="server" class="col-sm-12 js-example-basic-single js-states form-control hide"></asp:DropDownList>
                                <asp:HiddenField ID="hdfInternalOrder" runat="server" />
                                <%--<asp:TextBox ID="txtCostCenter" runat="server" placeholder="Cost Center" class="col-sm-12"></asp:TextBox>--%>
                            </div>
                        </div>
                        <div class="col-sm-1">
                            <div class="col-sm-12 div-caption">
                                <asp:Button ID="btnSelectProject" runat="server" Text="เลือก" class="btn btn-white btn-info btn-sm" OnClick="btnSelectProject_Click" Width="75px" />
                            </div>
                        </div>

                    </div>

                </div>
            </div>
        </div>
    </div>
    <!-- #accordion -->
    
    <div class="col-sm-12" id="divCheckSerial" runat="server" style="display:none;">
        <div class="col-sm-1 div-caption">Serial No.</div>
        <div class="col-sm-3">
            <asp:TextBox ID="txtSerial" runat="server" class="col-sm-12" placeHolder="Serial No."></asp:TextBox>
        </div>
        <div class="col-sm-8">
            <asp:ImageButton ID="imgCheckSerial" runat="server" ImageUrl="~/img/checkbox_checked.png" Width="23px" Style="vertical-align: baseline;" OnClick="imgCheckSerial_Click"/>
        </div>
    </div>
    
    <div class="col-sm-12" >
        <asp:GridView ID="grdData" runat="server" AutoGenerateColumns="False" CellPadding="4" PageSize="20" 
            ForeColor="#333333" GridLines="None" class="col-sm-12" AllowPaging="True" OnRowDataBound="grdData_RowDataBound" OnRowCommand="grdData_RowCommand">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px">
                    <ItemTemplate>
                        <asp:ImageButton ID="imgDelvSel" runat="server" ImageUrl="~/img/write_document.png" Width="23px" Style="vertical-align: baseline;" CommandName="SelItemAmt" CommandArgument='<%# Eval("MasterItemId") %>' />
                        <asp:HiddenField ID="grdHdfMasterItemId" runat="server" Value='<%# Eval("MasterItemId") %>' />
                        <asp:HiddenField ID="grdHdfMasterItemName" runat="server" Value='<%# Eval("MasterItemName") %>' />
                        <asp:HiddenField ID="grdHdfItemCountMethod" runat="server" Value='<%# Eval("ItemCountMethod") %>' />
                        <asp:HiddenField ID="grdHdfStockAmount" runat="server" Value='<%# Eval("StockAmount") %>' />
                        <asp:HiddenField ID="grdHdfStockDelivery" runat="server" Value='<%# Eval("StockDelivery") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="MasterItemName" HeaderText="สินค้าโปรโมชั่น" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="StockAmount" HeaderText="จำนวนคงเหลือ" HeaderStyle-Width="123px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="StockDelivery" HeaderText="จำนวนส่งมอบ" HeaderStyle-Width="123px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" />
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
        <asp:Button ID="btnSave" runat="server" Text="บันทึกการส่งมอบ(Marketing)" class="btn btn-white btn-info btn-sm" OnClick="btnSave_Click" />
    </div>
</asp:Content>