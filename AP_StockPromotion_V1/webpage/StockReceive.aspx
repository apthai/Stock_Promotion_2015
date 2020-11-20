<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockReceive.aspx.cs"
    Inherits="AP_StockPromotion_V1.web.StockReceive" MasterPageFile="~/Master/MasterPage.Master" %>

<asp:Content ID="Contenthead" ContentPlaceHolderID="head" runat="server">
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
        var mypopUp;

        jQuery(function ($) {
            $('#divNavx').html('ตรวจรับสินค้า >> รับสินค้า');
            $(".js-example-basic-single").select2({ width: '100%' });
            setDatePicker();
            $('#accordion-style').on('click', function (ev) {
                var target = $('input', ev.target);
                var which = parseInt(target.val());
                if (which == 2) $('#accordion').addClass('accordion-style2');
                else $('#accordion').removeClass('accordion-style2');
            });
        });


        function confirmButton(txtConfirm) {
            if (confirm(txtConfirm)) {
                return true;
            }
            return false;
        }

        function popupStockReceiveCreate() {
            PopupFullScr('StockReceiveEdit.aspx');
        }

        function popupStockReceiveEdit() {
            Popup60('StockReceiveEdit.aspx');
        }

        function popupStockReceiveDetail(ItemCountMethod) {
            Popup80('StockReceiveDetail.aspx?ItemCountMethod=' + ItemCountMethod);
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

        function PopupFullScr(url) {
            if (mypopUp != undefined) {
                mypopUp.close();
            }
            var scr_H = screen.height;
            var scr_W = screen.width;
            var h = (scr_H);
            var w = (scr_W);
            var t = 0;
            var l = 0;
            myWindow = window.open(url, '', 'height=' + h + ',width=' + w + ',top=' + t + ',left=' + l + ',status=yes,scrollbars=no,resizable=yes,menubar=no;toolbar=no,titlebar=no');
        }

        function setDatePicker() {
            $("#<%= txtDateFrom.ClientID %>").datepicker({ dateFormat: "dd/mm/yy" });
            $("#<%= txtDateTo.ClientID %>").datepicker({ dateFormat: "dd/mm/yy" });
        }

        function clickBindData() {
            $("#<%= btnSearch.ClientID %>").click();
        }

    </script>
    <!-- Add fancyBox -->
    <link rel="stylesheet" href="/fancybox/source/jquery.fancybox.css?v=2.1.5" type="text/css" media="screen" />
    <script type="text/javascript" src="/fancybox/source/jquery.fancybox.pack.js?v=2.1.5"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- #section:elements.accordion -->
    <div id="accordion" class="accordion-style1 panel-group">
        <div class="panel panel-default">
            <div class="panel-heading">
                <h4 class="panel-title">
                    <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion" href="#collapseOne">
                        <i class="ace-icon fa fa-angle-down bigger-110" data-icon-hide="ace-icon fa fa-angle-down" data-icon-show="ace-icon fa fa-angle-right"></i>
                        &nbsp;ค้นหาประวัติการรับสินค้า
                    </a>
                </h4>
            </div>

            

            <div class="panel-collapse collapse in" id="collapseOne">
                <div class="panel-body">                    
                    <div class="col-sm-12">
                        <div class="col-sm-2 div-caption">PO</div>
                        <div class="col-sm-3">
                            <asp:TextBox ID="txtPO" runat="server" placeholder="PO:XXXXXXXXXXXX" class="col-sm-12"></asp:TextBox>
                        </div>
                        <div class="col-sm-2 div-caption">GR</div>
                        <div class="col-sm-3">
                            <asp:TextBox ID="txtGR" runat="server" placeholder="GR:XXXXXXXXXXXX" class="col-sm-12"></asp:TextBox>
                        </div>
                        <div class="col-sm-2"></div>
                    </div>
                    <div class="col-sm-12">
                        <div class="col-sm-2 div-caption">หมวดสินค้า</div>
                        <div class="col-sm-3">
                            <asp:DropDownList ID="ddlItemGroup" runat="server" class="col-sm-12 js-example-basic-single js-states form-control hide"></asp:DropDownList>
                        </div>
                        <div class="col-sm-2 div-caption">รายละเอียดสินค้า</div>
                        <div class="col-sm-3">
                            <asp:DropDownList ID="ddlItem" runat="server" class="col-sm-12 js-example-basic-single js-states form-control hide"></asp:DropDownList>
                        </div>
                        <!--<div class="col-sm-2 div-caption">ผู้รับสินค้า</div>
                        <div class="col-sm-3">
                            <asp:DropDownList ID="ddlUser" runat="server" class="col-sm-12 js-example-basic-single js-states form-control hide"></asp:DropDownList>
                            <%--<asp:TextBox ID="txtReceiveBy" runat="server" placeholder="AP00XXXX" class="col-sm-9"></asp:TextBox>
                            <div class="col-sm-3" style="text-align:right;">
                                <asp:ImageButton ID="imgFindUser" runat="server" ImageUrl="~/img/find_user.png" Width="23px" Style="vertical-align: baseline;" OnClientClick="" OnClick="imgFindUser_Click" />
                                <asp:HiddenField ID="hdfUserId" runat="server" />
                                <asp:HiddenField ID="hdfEmpCode" runat="server" />
                                <asp:HiddenField ID="hdfUserName" runat="server" />
                            </div>--%>
                        </div>-->
                        <div class="col-sm-2"></div>
                    </div>
                    <div class="col-sm-12">
                        <div class="col-sm-2 div-caption">วันที่รับรายการสินค้า</div>
                        <div class="col-sm-3">
                            <asp:TextBox ID="txtDateFrom" runat="server" placeholder="วันที่เริ่มต้น" class="col-sm-12"></asp:TextBox>
                        </div>
                        <div class="col-sm-2 div-caption">ถึงวันที่</div>
                        <div class="col-sm-3">
                            <asp:TextBox ID="txtDateTo" runat="server" placeholder="วันที่สิ้นสุด" class="col-sm-12"></asp:TextBox>
                        </div>
                        <div class="col-sm-2"></div>
                    </div>
                    <div class="col-sm-12" style="text-align:center;">
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
<%--        <button class="btn btn-white btn-info btn-sm" onclick="popupStockReceiveCreate(); return false;">
            <i class="ace-icon fa glyphicon-plus bigger-120 blue"></i>รับสินค้า</button>--%>
        <asp:Button id="btnAddReceive" runat="server" class="btn btn-white btn-info btn-sm" Text="+ เพิ่มรายการรับสินค้า" OnClick="btnAddReceive_Click" />
    </div>
    <div class="col-sm-12">
        <asp:GridView ID="grdData" runat="server" AutoGenerateColumns="False" CellPadding="4"
            ForeColor="#333333" GridLines="None" class="col-sm-12" OnRowCommand="grdData_RowCommand" OnRowDataBound="grdData_RowDataBound"
            AllowPaging="true" AllowSorting="true" OnPageIndexChanging="grdData_PageIndexChanging" OnSorting="grdData_Sorting" PageSize="20"  >
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:TemplateField HeaderText="เลขที่ PO" SortExpression="PO_No">
                    <ItemTemplate>
                        <asp:LinkButton ID="grdLnkPO" runat="server" Text='<%# Eval("PO_No") %>' CommandName="clickPO" CommandArgument='<%# Eval("PO_No") %>'></asp:LinkButton>
                        <asp:ImageButton ID="grdPrtPO" runat="server" class="imgBtn" ToolTip="Print PO" ImageUrl="~/img/printer_and_fax.png" Width="23px" Style="vertical-align: baseline;" CommandName="PrtPO" CommandArgument='<%# Eval("PO_No") %>' />                        
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="เลขที่ GR" SortExpression="GR_No">
                    <ItemTemplate>
                        <asp:LinkButton ID="grdLnkGR" runat="server" Text='<%# Eval("GR_No") %>' CommandName="clickGR" CommandArgument='<%# Eval("GR_No") %>'></asp:LinkButton>
                        <asp:ImageButton ID="grdPrtGR" runat="server" class="imgBtn" ToolTip="Print GR" ImageUrl="~/img/printer_and_fax.png" Width="23px" Style="vertical-align: baseline;" CommandName="PrtGR" CommandArgument='<%# Eval("ReceiveHeaderID") %>' />                        
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="DocRefNo" SortExpression="DocRefNo" HeaderText="Delivery Note" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Vendor" SortExpression="Vendor" HeaderText="บริษัทผู้ค้า" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="CreateDate" SortExpression="CreateDate" HeaderText="วันที่รับ" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}"/>
                <asp:BoundField DataField="FullName" SortExpression="FullName" HeaderText="ผู้รับ" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"/>
                <asp:BoundField DataField="ItemName" SortExpression="ItemName" HeaderText="รายละเอียดสินค้า" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center"/>
                <asp:BoundField DataField="SAP_MENGE" SortExpression="SAP_MENGE" HeaderText="จำนวนซื้อ" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center"/>
                <asp:BoundField DataField="ReceiveAmount" SortExpression="ReceiveAmount" HeaderText="จำนวนรับ" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center"/>
                <asp:BoundField DataField="ItemUnit" SortExpression="ItemUnit" HeaderText="หน่วย" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Visible="false"/>
                
                <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="grdLbGrNo" runat="server" Text='<%# Eval("GR_No") %>' ></asp:Label>                        
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="grdLbGrYear" runat="server" Text='<%# Eval("GR_Year") %>'></asp:Label>                        
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ยกเลิก" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Visible="false" >
                    <ItemTemplate>
                        <asp:ImageButton ID="imgCancelGR" runat="server" ImageUrl="~/img/delete_black.png" Width="23px" Style="vertical-align: baseline;" CommandName="CancelGR" CommandArgument='<%# Eval("GR_No") %>' />
                        <asp:HiddenField ID="hdfReceiveHeaderID" runat="server" Value='<%# Eval("ReceiveHeaderID") %>' />
                        <asp:HiddenField ID="hdfPO_No" runat="server" Value='<%# Eval("PO_No") %>' />
                        <asp:HiddenField ID="hdfGR_No" runat="server" Value='<%# Eval("GR_No") %>' />
                        <asp:HiddenField ID="hdfGR_Year" runat="server" Value='<%# Eval("GR_Year") %>' />
                        <asp:HiddenField ID="hdfVendor" runat="server" Value='<%# Eval("Vendor") %>' />
                        <asp:HiddenField ID="hdfPostingDate" runat="server" Value='<%# Eval("PostingDate") %>' />
                        <asp:HiddenField ID="hdfDocDate" runat="server" Value='<%# Eval("DocDate") %>' />
                        <asp:HiddenField ID="hdfDocRefNo" runat="server" Value='<%# Eval("DocRefNo") %>' />
                        <asp:HiddenField ID="hdfCreateDate" runat="server" Value='<%# Eval("CreateDate") %>' />
                        <asp:HiddenField ID="hdfCreateBy" runat="server" Value='<%# Eval("CreateBy") %>' />
                        <asp:HiddenField ID="hdfReceiveHeaderStatus" runat="server" Value='<%# Eval("ReceiveHeaderStatus") %>' />
                        <asp:HiddenField ID="hdfReceiveDetailId" runat="server" Value='<%# Eval("ReceiveDetailId") %>' />
                        <asp:HiddenField ID="hdfPricePerUnit" runat="server" Value='<%# Eval("PricePerUnit") %>' />
                        <asp:HiddenField ID="hdfReceiveAmount" runat="server" Value='<%# Eval("ReceiveAmount") %>' />
                        <asp:HiddenField ID="hdfStatus" runat="server" Value='<%# Eval("Status") %>' />
                        <asp:HiddenField ID="hdfMasterItemId" runat="server" Value='<%# Eval("MasterItemId") %>' />
                        <asp:HiddenField ID="hdfItemNo" runat="server" Value='<%# Eval("ItemNo") %>' />
                        <asp:HiddenField ID="hdfItemName" runat="server" Value='<%# Eval("ItemName") %>' />
                        <asp:HiddenField ID="hdfItemUnit" runat="server" Value='<%# Eval("ItemUnit") %>' />
                        <asp:HiddenField ID="hdfItemStatus" runat="server" Value='<%# Eval("ItemStatus") %>' />
                        <asp:HiddenField ID="hdfCancelAble" runat="server" Value='<%# Eval("CancelAble") %>' />
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
</asp:Content>
