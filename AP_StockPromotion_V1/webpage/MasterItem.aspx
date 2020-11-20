<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MasterItem.aspx.cs" 
    Inherits="AP_StockPromotion_V1.webpage.MasterItem" MasterPageFile="~/Master/MasterPage.Master"  %>

<asp:Content ID="Contenthead" ContentPlaceHolderId="head" runat="server">
    <style type="text/css">
        .label-caption {
            text-align:right;
        }
        .div-caption {
            text-align:right;
            padding-right:5px;
        }


        .ui-autocomplete {
            max-height: 200px;
            overflow-y: auto;
            /* prevent horizontal scrollbar */
            overflow-x: hidden;
        }
        /* IE 6 doesn't support max-height
	     * we use height instead, but this forces the menu to always be this tall
	     */
        * html .ui-autocomplete {
            height: 200px;
        }
	
    </style>
    <script type="text/javascript">
        jQuery(function ($) {
            $('#divNavx').html('ข้อมูลตั้งต้น >> รายละเอียดสินค้า');
            $(".js-example-basic-single").select2({ width: '100%' });
            $('.js-example-basic-single-ws').select2({ minimumResultsForSearch: Infinity, width: '100%' });
            
            setTextNumericOnly();
            setAutoComplete();
            $('#accordion-style').on('click', function (ev) {
                var target = $('input', ev.target);
                var which = parseInt(target.val());
                if (which == 2) $('#accordion').addClass('accordion-style2');
                else $('#accordion').removeClass('accordion-style2');
            });
        });

        function popupMasterItemCreate() {
            Popup80('MasterItemDetail.aspx?mode=Create');
        }

        function popupMasterItemDetail(MasterItemId) {
            Popup80('MasterItemDetail.aspx?mode=Edit&MasterItemId=' + MasterItemId);
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

        function setTextNumericOnly() {
            jQuery('.numericOnly').keyup(function () {
                this.value = this.value.replace(/[^0-9\.]/g, '');
            });
        }

        function setAutoComplete() {
            $('#<%= txtItemName.ClientID %>').autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "Default.aspx/GetCategoryMasterItem",
                        data: "{'itemName':'" + $('#<%= txtItemName.ClientID %>').val() + "'}",
                        dataType: "json",
                        success: function (data) {
                            response(data.d);
                        },
                        error: function (result) {
                            alert("Error");
                        }
                    });
                }
            });
        }

    </script>

    <!-- Add fancyBox -->
    <link rel="stylesheet" href="/fancybox/source/jquery.fancybox.css?v=2.1.5" type="text/css" media="screen" />
    <script type="text/javascript" src="/fancybox/source/jquery.fancybox.pack.js?v=2.1.5"></script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderId="ContentPlaceHolder1" runat="server">
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
                        <div class="col-sm-2 div-caption">
                            <asp:Label ID="lbItemNo" runat="server" Text="รหัสสินค้า :" class="col-sm-12 label-caption"></asp:Label>
                        </div>
                        <div class="col-sm-3">
                            <asp:TextBox ID="txtItemNo" runat="server" placeholder="รหัสสินค้า" class="col-sm-12"></asp:TextBox>
                        </div>
                        <div class="col-sm-2 div-caption">
                            <asp:Label ID="lbItemName" runat="server" Text="ชื่อสินค้า :" class="col-sm-12 label-caption"></asp:Label>
                        </div>
                        <div class="col-sm-3">
                            <asp:TextBox ID="txtItemName" runat="server" placeholder="ชื่อสินค้า" class="col-sm-12"></asp:TextBox>
                        </div>
                        <div class="col-sm-2 div-caption">&nbsp;
                        </div>
                    </div>
                    
                    <%-- Display: None --%>
                    <div class="col-sm-12" style="display:none;">
                        <div class="col-sm-2 div-caption">
                            <asp:Label ID="lbItemCostBeg" runat="server" Text="ช่วงราคาเริ่มต้น :" class="col-sm-12 label-caption"></asp:Label>
                        </div>
                        <div class="col-sm-3">
                            <asp:TextBox ID="txtItemCostBeg" runat="server" placeholder="0.00" class="numericOnly col-sm-12 label-caption numeric"></asp:TextBox>
                        </div>
                        <div class="col-sm-2 div-caption">
                            <asp:Label ID="Label2" runat="server" Text="ถึง :" class="col-sm-12 label-caption"></asp:Label>
                        </div>
                        <div class="col-sm-3">
                            <asp:TextBox ID="txtItemCostEnd" runat="server" placeholder="9,999,999.99" class="numericOnly col-sm-12 label-caption numeric"></asp:TextBox>
                        </div>
                        <div class="col-sm-2 div-caption">&nbsp;
                        </div>
                    </div>
                    <%-- ------------- --%>

                    <div class="col-sm-12">
                        <div class="col-sm-2 div-caption">
                            <asp:Label ID="lb" runat="server" Text="วิธีการตรวจนับ :" class="col-sm-12 label-caption"></asp:Label>
                        </div>
                        <div class="col-sm-3">
                            <asp:DropDownList ID="ddlItemCountMethod" runat="server" class="col-sm-12 js-example-basic-single-ws js-states form-control hide"></asp:DropDownList>
                        </div>
                        <div class="col-sm-2 div-caption">
                            <asp:Label ID="Label3" runat="server" Text="สต็อกกลาง :" class="col-sm-12 label-caption"></asp:Label>
                        </div>
                        <div class="col-sm-3">
                            <asp:DropDownList ID="ddlItemStock" runat="server" class="col-sm-12 js-example-basic-single-ws js-states form-control hide"></asp:DropDownList>
                        </div>
                        <div class="col-sm-2 div-caption">&nbsp;
                        </div>
                    </div>
                    <div class="col-sm-12">
                        <div class="col-sm-2 div-caption">
                            <asp:Label ID="lbGroup" runat="server" Text="หมวดสินค้า :" class="col-sm-12 label-caption"></asp:Label>
                        </div>
                        <div class="col-sm-3">
                            <asp:DropDownList ID="ddlItemGroup" runat="server" class="col-sm-12 js-example-basic-single js-states form-control hide"></asp:DropDownList>
                        </div>
                        <div class="col-sm-2 div-caption">
                            <asp:Label ID="lbStatus" runat="server" Text="สถานะ :" class="col-sm-12 label-caption"></asp:Label>
                        </div>
                        <div class="col-sm-3">
                            <asp:DropDownList ID="ddlItemStatus" runat="server" class="col-sm-12 js-example-basic-single-ws js-states form-control hide"></asp:DropDownList>
                        </div>
                        <div class="col-sm-2 div-caption">&nbsp;</div>
                    </div>
                    <div class="col-sm-12">
                        <div class="col-sm-2 div-caption">
                            <asp:Label ID="lbForceExpire" runat="server" Text="ระบุวันหมดอายุ :" class="col-sm-12 label-caption"></asp:Label>
                        </div>
                        <div class="col-sm-3">
                            <asp:DropDownList ID="ddlForceExpire" runat="server" class="col-sm-12 js-example-basic-single-ws js-states form-control hide"></asp:DropDownList>
                        </div>
                        <div class="col-sm-2 div-caption">
                            
                        </div>
                        <div class="col-sm-3">
                            
                        </div>
                        <div class="col-sm-2 div-caption">&nbsp;</div>
                    </div>
                    <div class="col-sm-12" style="text-align: center;">

                        <asp:Button ID="btnSearch" runat="server" Text="Search" class="btn btn-white btn-info btn-sm" Width="75px" OnClick="btnSearch_Click" />
                        &nbsp;
                            <asp:Button ID="btnClear" runat="server" Text="Clear" class="btn btn-white btn-info btn-sm" Width="75px" OnClick="btnClear_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- #accordion -->

    <div class="col-sm-12">
        <%--<button class="btn btn-white btn-info btn-sm" onclick="popupMasterItemCreate();">
            <i class="ace-icon fa glyphicon-plus bigger-120 blue"></i>เพิ่มรายการสินค้า
        </button> --%>       
        <asp:Button ID="btnSyncMaster" runat="server" Text="ปรับปรุงข้อมูล" OnClick="btnSyncMaster_Click"  style="display:none;" class="btn btn-white btn-info btn-sm" />
    </div>
    <div class="col-sm-12" >
        <asp:GridView ID="grdData" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" class="col-sm-12" 
            EmptyDataText="No data." ShowHeaderWhenEmpty="True" AllowSorting="true" OnSorting="grdData_Sorting" AllowPaging="true"
            OnRowCommand="grdData_RowCommand" OnRowDataBound="grdData_RowDataBound" OnPageIndexChanging="grdData_PageIndexChanging" PageSize="15" CssClass="table table-hover table-striped">
            <AlternatingRowStyle BackColor="White" />
            <Columns>                
                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:ImageButton ID="imgEdit" runat="server" ImageUrl="~/img/writing_file.png" Width="23px" Style="vertical-align: baseline;" CommandName="MasterItemEdit" CommandArgument='<%# Eval("MasterItemId") %>' />
                        <asp:HiddenField ID="hdfMasterItemId" runat="server" Value='<%# Eval("MasterItemId") %>' />
                        <asp:HiddenField ID="hdfItemCountMethod" runat="server" Value='<%# Eval("ItemCountMethod") %>' />
                        <asp:HiddenField ID="hdfItemStock" runat="server" Value='<%# Eval("ItemStock") %>' />
                        <asp:HiddenField ID="hdfItemStatus" runat="server" Value='<%# Eval("ItemStatus") %>' />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>
                <asp:BoundField DataField="ItemNo" HeaderText="รหัสสินค้า" SortExpression="ItemNo" HeaderStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="ItemName" HeaderText="ชื่อสินค้า" SortExpression="ItemName" HeaderStyle-HorizontalAlign="Center" />                
                <asp:BoundField DataField="ItemPricePerUnit" HeaderText="มูลค่า(รวมVat)" SortExpression="ItemPricePerUnit" DataFormatString="{0:N2}" HeaderStyle-HorizontalAlign="Center" >
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="MasterItemGroupName" SortExpression="MasterItemGroupName" HeaderText="หมวดสินค้า" HeaderStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="ItemCountMethodText" SortExpression="ItemCountMethodText" HeaderText="วิธีการตรวจนับ" HeaderStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="ItemStockText" SortExpression="ItemStockText" HeaderText="สต็อกกลาง" HeaderStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="ItemForceExpireText" SortExpression="ItemForceExpireText" HeaderText="ระบุวันหมดอายุ" HeaderStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="ItemStatusText" SortExpression="ItemStatusText" HeaderText="สถานะ" HeaderStyle-HorizontalAlign="Center" />
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


</asp:Content>