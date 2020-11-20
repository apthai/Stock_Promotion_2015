<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockReceiptList.aspx.cs"
    Inherits="AP_StockPromotion_V1.web.StockReceiptList" MasterPageFile="~/Master/MasterPage.Master" %>

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
            $('#divNavx').html('ตรวจรับสินค้า >> ระบุจำนวน หรือSerial');
            $(".js-example-basic-single").select2({ width: '100%' });
            $('.js-example-basic-single-ws').select2({ minimumResultsForSearch: Infinity, width: '100%' });

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

        function popupStockReceiptDetail(receiveDetailId) {
            Popup80('StockReceiveDetail.aspx?mode=FullFill&receiveDetailId=' + receiveDetailId);
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
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- #section:elements.accordion -->
    <div id="accordion" class="accordion-style1 panel-group">
        <div class="panel panel-default">
            <div class="panel-heading">
                <h4 class="panel-title">
                    <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion" href="#collapseOne">
                        <i class="ace-icon fa fa-angle-down bigger-110" data-icon-hide="ace-icon fa fa-angle-down" data-icon-show="ace-icon fa fa-angle-right"></i>
                        &nbsp;ค้นหารายการตรวจรับสินค้า
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
                        <div class="col-sm-2 div-caption">วันที่รับสินค้า</div>
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
        <asp:GridView ID="grdData" runat="server" AutoGenerateColumns="False" CellPadding="4" 
            ForeColor="#333333" GridLines="None" class="col-sm-12" OnRowCommand="grdData_RowCommand" OnRowDataBound="grdData_RowDataBound"
            AllowPaging="true" AllowSorting="true" OnPageIndexChanging="grdData_PageIndexChanging" OnSorting="grdData_Sorting" PageSize="20" >
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:TemplateField HeaderText="ระบุข้อมูลสินค้า" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" >
                    <ItemTemplate>
                        <asp:ImageButton ID="imgEditItem" runat="server" ImageUrl="~/img/document_edit.png" Width="23px" Style="vertical-align: baseline;" CommandName="EditItem" />
                        <asp:HiddenField ID="hdfReceiveHeaderID" runat="server" Value='<%# Eval("ReceiveHeaderID") %>' />
                        <asp:HiddenField ID="hdfPO_No" runat="server" Value='<%# Eval("PO_No") %>' />
                        <asp:HiddenField ID="hdfGR_No" runat="server" Value='<%# Eval("GR_No") %>' />
                        <asp:HiddenField ID="hdfGR_Year" runat="server" Value='<%# Eval("GR_Year") %>' />
                        <asp:HiddenField ID="hdfVendor" runat="server" Value='<%# Eval("Vendor") %>' />
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
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="PO_No" SortExpression="PO_No" HeaderText="เลขที่ PO" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="GR_No" SortExpression="GR_No" HeaderText="เลขที่ GR" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"/>                
                <asp:BoundField DataField="CreateDate" SortExpression="CreateDate" HeaderText="วันที่รับสินค้า" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"/>
                <asp:BoundField DataField="FullName" SortExpression="FullName" HeaderText="ผู้รับสินค้า" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"/>
                <asp:BoundField DataField="ItemName" SortExpression="ItemName" HeaderText="รายละเอียดสินค้า" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center"/>
                <asp:BoundField DataField="ReceiveAmount" SortExpression="ReceiveAmount" HeaderText="จำนวน" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center"/>
                <asp:BoundField DataField="SAP_MEINS" SortExpression="SAP_MEINS" HeaderText="หน่วย" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Visible="false"/>
                <asp:BoundField DataField="StatusText" SortExpression="StatusText" HeaderText="สถานะ" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"/>
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
