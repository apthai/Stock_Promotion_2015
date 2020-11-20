<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockReturnItem.aspx.cs" 
    Inherits="AP_StockPromotion_V1.web.StockReturnItem" MasterPageFile="~/Master/MasterPage.Master"  %>

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

            $('#divNavx').html('คืนสินค้าโปรโมชั่น');
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
                        &nbsp;ค้นหาข้อมูลคืนสินค้าโปรโมชั่น
                    </a>
                </h4>
            </div>
            
            <div class="panel-collapse collapse in" id="collapseOne">
                <div class="panel-body">
                    <div class="col-sm-12">
                        <div class="col-sm-2 div-caption">วันที่คืน</div>
                        <div class="col-sm-3">
                            <asp:TextBox ID="txtDateFrom" runat="server" placeholder="วันที่เริ่มต้น" class="col-sm-12"></asp:TextBox>
                        </div>
                        <div class="col-sm-2 div-caption">ถึงวันที่</div>
                        <div class="col-sm-3">
                            <asp:TextBox ID="txtDateTo" runat="server" placeholder="ถึงวันที่" class="col-sm-12"></asp:TextBox>
                        </div>
                        <div class="col-sm-2 div-caption"></div>
                    </div>
                    <div class="col-sm-12">
                        <div class="col-sm-2 div-caption">โครงการ</div>
                        <div class="col-sm-3">
                            <asp:DropDownList ID="ddlProject" runat="server" class="col-sm-12 js-example-basic-single js-states form-control hide"></asp:DropDownList>
                        </div>
                        <div class="col-sm-2 div-caption">สินค้าโปรโมชั่น</div>
                        <div class="col-sm-3">
                            <asp:DropDownList ID="ddlItem" runat="server" class="col-sm-12 js-example-basic-single js-states form-control hide"></asp:DropDownList>
                        </div>
                        <div class="col-sm-2 div-caption"></div>
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
    <div class="col-sm-12" >
        <asp:Button ID="btnAddDataReturn" runat="server" Text="+ คืนสินค้าโปรโมชั่น" class="btn btn-white btn-info btn-sm" OnClick="btnAddDataReturn_Click" />
    </div>
    <div class="col-sm-12" >
        <asp:GridView ID="grdData" runat="server" AutoGenerateColumns="False" CellPadding="4" PageSize="20" 
            ForeColor="#333333" GridLines="None" class="col-sm-12" AllowPaging="True" OnPageIndexChanging="grdData_PageIndexChanging" 
            OnSorting="grdData_Sorting" AllowSorting="true">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" Visible="false">
                    <ItemTemplate>
                        <asp:ImageButton ID="imgCancelReturn" runat="server" ImageUrl="~/img/ToProject.png" Width="23px" Style="vertical-align: baseline;" CommandName="CancelReturn" CommandArgument='<%# Eval("ReturnListId") %>' />
                        <asp:HiddenField ID="grdHdfProjectName" runat="server" Value='<%# Eval("ReturnListId") %>' />
                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("Project_Id") %>' />
                        <asp:HiddenField ID="HiddenField2" runat="server" Value='<%# Eval("ProjectName") %>' />
                        <asp:HiddenField ID="HiddenField3" runat="server" Value='<%# Eval("MasterItemId") %>' />
                        <asp:HiddenField ID="HiddenField4" runat="server" Value='<%# Eval("ItemName") %>' />
                        <asp:HiddenField ID="HiddenField6" runat="server" Value='<%# Eval("CreateDate") %>' />
                        <asp:HiddenField ID="HiddenField7" runat="server" Value='<%# Eval("CreateBy") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ProjectName" SortExpression="ProjectName" HeaderText="โครงการ" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="ItemName" SortExpression="ItemName" HeaderText="สินค้าโปรโมชั่น" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Amount" SortExpression="Amount" HeaderText="จำนวน" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="91px" />
                <asp:BoundField DataField="CreateDate" SortExpression="CreateDate" HeaderText="วันที่คืน" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="123px" />
                <asp:BoundField DataField="FullName" SortExpression="FullName" HeaderText="ผู้ดำเนินการ" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="234px" />
                <asp:BoundField DataField="ReturnReason" SortExpression="ReturnReason" HeaderText="เหตุผล" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
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