<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeliveryLowPriceItem.aspx.cs"
    Inherits="AP_StockPromotion_V1.webpage.DeliveryLowPriceItem" MasterPageFile="~/Master/MasterPage.Master" %>

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
            $('#divNavx').html('เคลียร์เอกสารส่งมอบ (Marketing)');
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
            $('#<%= txtDateFrom.ClientID %>').datepicker({ dateFormat: "dd/mm/yy" }); // { dateFormat: "dd-mm-yy" }
            $('#<%= txtDateTo.ClientID %>').datepicker({ dateFormat: "dd/mm/yy" });
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
                        &nbsp;รายการเคลียร์เอกสารส่งมอบ (Marketing)
                    </a>
                </h4>
            </div>

            <div class="panel-collapse collapse in" id="collapseOne">
                <div class="panel-body">
                    <div class="col-sm-12">
                        <div class="col-sm-2 div-caption">เลขที่เอกสารส่งมอบ</div>
                        <div class="col-sm-3">
                            <asp:TextBox ID="txtDeliveryNo" runat="server" placeholder="เลขที่เอกสาร" class="col-sm-12"></asp:TextBox>
                        </div>
                        <div class="col-sm-2 div-caption">โครงการ</div>
                        <div class="col-sm-3">
                            <asp:DropDownList ID="ddlProject" runat="server" class="col-sm-12 js-example-basic-single js-states form-control hide"></asp:DropDownList>
                        </div>
                        <div class="col-sm-2 div-caption"></div>
                    </div>
                    <div class="col-sm-12" style="padding-bottom: 5px">
                        <div class="col-sm-2 div-caption">สินค้าโปรโมชั่น</div>
                        <div class="col-sm-3">
                            <asp:DropDownList ID="ddlItem" runat="server" class="col-sm-12 js-example-basic-single js-states form-control hide"></asp:DropDownList>
                        </div>
                        <div class="col-sm-2 div-caption">วันที่ส่งมอบ</div>
                        <div class="col-sm-3">
                            <div class="col-sm-5" style="padding-left: 0; padding-right: 0;">
                                <asp:TextBox ID="txtDateFrom" runat="server" placeholder="วันที่เริ่มต้น" class="col-sm-12"></asp:TextBox>
                            </div>
                            <div class="col-sm-2" style="text-align: center;">-</div>
                            <div class="col-sm-5" style="padding-left: 0; padding-right: 0;">
                                <asp:TextBox ID="txtDateTo" runat="server" placeholder="ถึงวันที่" class="col-sm-12"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-2 div-caption"></div>
                    </div>

                    <div class="col-sm-12" style="padding-bottom: 5px">
                        <div class="col-sm-2 div-caption">สถานะ</div>
                        <div class="col-sm-2">
                            <asp:DropDownList ID="cbxDelvStatus" runat="server" Style="width: 100%">
                                <asp:ListItem Value="0" Selected="True">-- ทั้งหมด --</asp:ListItem>
                                <asp:ListItem Value="1">รายการใหม่</asp:ListItem>
                                <asp:ListItem Value="2">รอบันทึกบัญชี</asp:ListItem>
                                <asp:ListItem Value="3">เสร็จสิ้น</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-sm-8"></div>
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
        <asp:Button ID="btnDeliveryItemLowPrice" runat="server" Text="+ เคลียร์เอกสารส่งมอบ (Marketing)" class="btn btn-white btn-info btn-sm" Font-Bold="true" OnClick="btnDeliveryItemLowPrice_Click" />
    </div>

    <div class="col-sm-12">
        <asp:GridView ID="grdData" runat="server" AutoGenerateColumns="False" CellPadding="4" AllowPaging="true" AllowSorting="true"
            ForeColor="#333333" GridLines="None" class="col-sm-12" OnRowCommand="grdData_RowCommand" OnRowDataBound="grdData_RowDataBound"
            OnPageIndexChanging="grdData_PageIndexChanging" OnSorting="grdData_Sorting">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="34px">
                    <ItemTemplate>
                        <asp:ImageButton ID="imgEdit" runat="server" ImageUrl="~/img/file_edit.png" Width="23px" Style="vertical-align: baseline;" CommandName="viewDetail" CommandArgument='<%# Eval("DelvLstId") %>' />
                        <asp:HiddenField ID="grdHdfDelvLstId" runat="server" Value='<%# Eval("DelvLstId") %>' />
                        <asp:HiddenField ID="grdHdfisConfirm" runat="server" Value='<%# Eval("isConfirm") %>' />
                        <asp:HiddenField ID="grdHdfisPostAcc" runat="server" Value='<%# Eval("isPostAcc") %>' />
                        <asp:HiddenField ID="grdHdfDelvPromotionId" runat="server" Value='<%# Eval("DelvPromotionId") %>' />
                        <asp:HiddenField ID="grdHdfDelvDate" runat="server" Value='<%# Eval("DelvDate") %>' />
                        <asp:HiddenField ID="grdHdfItemId" runat="server" Value='<%# Eval("ItemId") %>' />
                        <asp:HiddenField ID="grdHdfItemName" runat="server" Value='<%# Eval("ItemName") %>' />
                        <asp:HiddenField ID="grdHdfAmount" runat="server" Value='<%# Eval("Amount") %>' />
                    </ItemTemplate>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Center" Width="34px"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="34px">
                    <ItemTemplate>
                        <asp:CheckBox ID="grdChkPrt" runat="server" ToolTip="พิมพ์ใบปะหน้าสำหรับบัญชี" />
                    </ItemTemplate>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Center" Width="34px"></ItemStyle>
                </asp:TemplateField>
                <asp:BoundField DataField="DelvPromotionId" HeaderText="เลขที่เอกสารส่งมอบ" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="141px">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Center" Width="141px"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="ProjectName" HeaderText="โครงการ" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="WBS" HeaderText="WBS" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="159px" Visible="false">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Center" Width="159px"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="ItemName" HeaderText="สินค้าโปรโมชั่น" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="Amount" HeaderText="จำนวน" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="71px">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Right" Width="71px"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="DelvDate" HeaderText="วันที่ส่งมอบ" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="123px">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Center" Width="123px"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="CostCenter" HeaderText="Internal Order" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="123px">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Center" Width="123px"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="StatusText" HeaderText="สถานะ" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="123px">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Center" Width="123px"></ItemStyle>
                </asp:BoundField>
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
        <asp:Button ID="btnPrint" runat="server" Text="พิมพ์รายการใบส่งมอบ" class="btn btn-white btn-info btn-sm" Width="151px" OnClick="btnPrint_Click" />
    </div>
</asp:Content>
