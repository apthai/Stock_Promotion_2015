<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockTransferItem_bk20150422.aspx.cs" 
    Inherits="AP_StockPromotion_V1.web.StockTransferItem_bk20150422" MasterPageFile="~/Master/MasterPage.Master"  %>


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

            $('#divNavx').html('โอนสินค้าโปรโมชั่นเข้าโครงการ');
            $(".js-example-basic-single").select2();
            $('.js-example-basic-single-ws').select2({ minimumResultsForSearch: Infinity });

            setDatePicker();
            $('#accordion-style').on('click', function (ev) {
                var target = $('input', ev.target);
                var which = parseInt(target.val());
                if (which == 2) $('#accordion').addClass('accordion-style2');
                else $('#accordion').removeClass('accordion-style2');
            });
        });

        function popupStockTransfrtItemEdit(reqId) {
            Popup80('StockTransferItemEdit.aspx?mode=Edit&reqId=' + reqId);
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

        function setDatePicker() {
            $("#<%= txtDateFrom.ClientID %>").datepicker({ dateFormat: "dd/mm/yy" });
            $("#<%= txtDateTo.ClientID %>").datepicker({ dateFormat: "dd/mm/yy" });
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderId="ContentPlaceHolder1" runat="server">

    <!-- #section:elements.accordion -->
    <div id="accordion" class="accordion-style1 panel-group">
        <div class="panel panel-default">
            <div class="panel-heading">
                <h4 class="panel-title">
                    <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion" href="#collapseOne">
                        <i class="ace-icon fa fa-angle-down bigger-110" data-icon-hide="ace-icon fa fa-angle-down" data-icon-show="ace-icon fa fa-angle-right"></i>
                        &nbsp;Filter
                    </a>
                </h4>
            </div>
            
            <div class="panel-collapse collapse in" id="collapseOne">
                <div class="panel-body">
                    <div class="col-sm-12">
                        <div class="col-sm-2 div-caption">เลขที่ใบเบิก</div>
                        <div class="col-sm-3">
                            <asp:TextBox ID="txtReqNo" runat="server" placeholder="เลขที่ใบเบิก" class="col-sm-12"></asp:TextBox>
                        </div>
                        <div class="col-sm-2 div-caption">สถานะรายการ</div>
                        <div class="col-sm-3">
                            <asp:DropDownList ID="ddlReqStatus" runat="server" class="col-sm-12 js-example-basic-single-ws js-states form-control"></asp:DropDownList>
                        </div>
                        <div class="col-sm-2 div-caption"></div>
                    </div>
                    <div class="col-sm-12">
                        <div class="col-sm-2 div-caption">วันที่เบิก</div>
                        <div class="col-sm-3">
                            <asp:TextBox ID="txtDateFrom" runat="server" placeholder="วันที่เริ่มต้น" class="col-sm-12"></asp:TextBox>
                        </div>
                        <div class="col-sm-2" style="text-align:center;">-</div>
                        <div class="col-sm-3">
                            <asp:TextBox ID="txtDateTo" runat="server" placeholder="ถึงวันที่" class="col-sm-12"></asp:TextBox>
                        </div>
                        <div class="col-sm-2 div-caption"></div>
                    </div>
                    <div class="col-sm-12">
                        <div class="col-sm-2 div-caption">โครงการ</div>
                        <div class="col-sm-3">
                            <asp:DropDownList ID="ddlProject" runat="server" class="col-sm-12 js-example-basic-single js-states form-control"></asp:DropDownList>
                        </div>
                        <div class="col-sm-2 div-caption">สินค้าโปรโมชั่น</div>
                        <div class="col-sm-3">
                            <asp:DropDownList ID="ddlItem" runat="server" class="col-sm-12 js-example-basic-single js-states form-control"></asp:DropDownList>
                        </div>
                        <div class="col-sm-2 div-caption"></div>
                    </div>
                    <div class="col-sm-12" style="text-align:center;">
                        <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" class="btn btn-white btn-info btn-sm" Width="75px" OnClick="btnSearch_Click" />
                        &nbsp;
                        <asp:Button ID="btnClear" runat="server" Text="ล้าง" class="btn btn-white btn-info btn-sm" Width="75px" OnClick="btnClear_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- #accordion -->
    <div class="col-sm-12" >
        <asp:GridView ID="grdData" runat="server" AutoGenerateColumns="False" CellPadding="4" PageSize="20" 
            ForeColor="#333333" GridLines="None" class="col-sm-12" OnRowCommand="grdData_RowCommand" OnRowDataBound="grdData_RowDataBound" AllowPaging="True" OnPageIndexChanging="grdData_PageIndexChanging">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:ImageButton ID="imgEdit" runat="server" ImageUrl="~/img/ToProject.png" Width="23px" Style="vertical-align: baseline;" CommandName="takeReq" CommandArgument='<%# Eval("ReqId") %>' />
                        <asp:HiddenField ID="grdHdfReqHeaderId" runat="server" Value='<%# Eval("ReqHeaderId") %>' />
                        <asp:HiddenField ID="grdHdfReqNo" runat="server" Value='<%# Eval("ReqNo") %>' />
                        <asp:HiddenField ID="grdHdfReqDate" runat="server" Value='<%# Eval("ReqDate") %>' />
                        <asp:HiddenField ID="grdHdfReqBy" runat="server" Value='<%# Eval("ReqBy") %>' />
                        <asp:HiddenField ID="grdHdfReqType" runat="server" Value='<%# Eval("ReqType") %>' />
                        <asp:HiddenField ID="grdHdfReqHeaderRemark" runat="server" Value='<%# Eval("ReqHeaderRemark") %>' />
                        <asp:HiddenField ID="grdHdfReqId" runat="server" Value='<%# Eval("ReqId") %>' />
                        <asp:HiddenField ID="grdHdfProject_Id" runat="server" Value='<%# Eval("Project_Id") %>' />
                        <asp:HiddenField ID="grdHdfProjectName" runat="server" Value='<%# Eval("ProjectName") %>' />
                        <asp:HiddenField ID="grdHdfProStartDate" runat="server" Value='<%# Eval("ProStartDate") %>' />
                        <asp:HiddenField ID="grdHdfProEndDate" runat="server" Value='<%# Eval("ProEndDate") %>' />
                        <asp:HiddenField ID="grdHdfItemId" runat="server" Value='<%# Eval("ItemId") %>' />
                        <asp:HiddenField ID="grdHdfItemName" runat="server" Value='<%# Eval("ItemName") %>' />
                        <asp:HiddenField ID="grdHdfReqAmount" runat="server" Value='<%# Eval("ReqAmount") %>' />
                        <asp:HiddenField ID="grdHdfItemUnit" runat="server" Value='<%# Eval("ItemUnit") %>' />
                        <asp:HiddenField ID="grdHdfItemPricePerUnit" runat="server" Value='<%# Eval("ItemPricePerUnit") %>' />
                        <asp:HiddenField ID="grdHdfReqStatus" runat="server" Value='<%# Eval("ReqStatus") %>' />
                        <asp:HiddenField ID="grdHdfReqStatusText" runat="server" Value='<%# Eval("ReqStatusText") %>' />
                        <asp:HiddenField ID="grdHdfItemCountMethod" runat="server" Value='<%# Eval("ItemCountMethod") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ReqDate" HeaderText="วันที่เบิก" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="ReqNo" HeaderText="เลขที่เอกสารอ้างอิง" />
                <asp:BoundField DataField="ProjectName" HeaderText="โครงการ" />
                <asp:BoundField DataField="ItemName" HeaderText="สินค้าโปรโมชั่น" />
                <asp:BoundField DataField="ReqAmount" HeaderText="จำนวน" />
                <asp:BoundField DataField="ItemUnit" HeaderText="หน่วย" />
                <asp:BoundField DataField="ItemPricePerUnit" HeaderText="มูลค่า" DataFormatString="{0:#,##0.00}" />
                <asp:TemplateField HeaderText="จำนวนเงินรวม" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lbTotalAmount" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ReqStatusText" HeaderText="สถานะ" />
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