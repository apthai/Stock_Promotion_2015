<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetUser.aspx.cs" 
    Inherits="AP_StockPromotion_V1.webpage.GetUser" MasterPageFile="~/Master/MasterPopup.Master" %>

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
        jQuery(function ($) {
            $('#divNavx').html('Get User');
            $(".js-example-basic-single").select2({ width: '100%' });
            $('.js-example-basic-single-ws').select2({ minimumResultsForSearch: Infinity, width: '100%' });
            $('#accordion-style').on('click', function (ev) {
                var target = $('input', ev.target);
                var which = parseInt(target.val());
                if (which == 2) $('#accordion').addClass('accordion-style2');
                else $('#accordion').removeClass('accordion-style2');
            });
        });

        function closePage() {
            parent.jQuery.colorbox.close();
        }

        function bindDataParentPage(_btnUserSearchId, _txtEmpCodeId, empCodeVal) {
            parent.$("#" + _txtEmpCodeId + "").val(empCodeVal);
            parent.$("#" + _btnUserSearchId + "").click();
            closePage();
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="accordion" class="accordion-style1 panel-group">
        <div class="panel panel-default">
            <div class="panel-heading">
                <h4 class="panel-title">
                    <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion" href="#collapseOne">
                        <i class="ace-icon fa fa-angle-down bigger-110" data-icon-hide="ace-icon fa fa-angle-down" data-icon-show="ace-icon fa fa-angle-right"></i>
                        &nbsp;ค้นหารายการชื่อผู้ใช้งาน
                    </a>
                </h4>
            </div>

            <div class="panel-collapse collapse in" id="collapseOne">
                <div class="panel-body">
                    <div class="col-sm-12">
                        <div class="col-sm-2 div-caption">
                            <asp:Label ID="lbFirstName" runat="server" Text="ชื่อ :" class="col-sm-12 label-caption"></asp:Label>
                        </div>
                        <div class="col-sm-3">
                            <asp:TextBox ID="txtFirstName" runat="server" placeholder="ชื่อผู้ใช้งาน" class="col-sm-12"></asp:TextBox>
                        </div>                   
                        <div class="col-sm-2 div-caption">
                            <asp:Label ID="lbLastName" runat="server" Text="นามสกุล :" class="col-sm-12 label-caption"></asp:Label>
                        </div>
                        <div class="col-sm-3">
                            <asp:TextBox ID="txtLastName" runat="server" placeholder="นามสกุล" class="col-sm-12"></asp:TextBox>
                        </div>
                        <div class="col-sm-2 div-caption">
                            &nbsp;
                        </div>
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
    <div class="col-sm-12">
        <asp:GridView ID="grdData" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" class="col-sm-12" 
            EmptyDataText="No data." ShowHeaderWhenEmpty="True" OnRowCommand="grdData_RowCommand" >
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:TemplateField HeaderText="EmpID" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="grdLnkEmpCode" runat="server" Text='<%# Eval("EmpCode") %>' CommandName="selEmp" CommandArgument='<%# Eval("EmpCode") %>'></asp:LinkButton>
                        <asp:HiddenField ID="hdfUserID" runat="server" Value='<%# Eval("UserID") %>' />
                        <asp:HiddenField ID="hdfEmpCode" runat="server" Value='<%# Eval("EmpCode") %>' />
                        <asp:HiddenField ID="hdfFirstName" runat="server" Value='<%# Eval("FirstName") %>' />
                        <asp:HiddenField ID="hdfLastName" runat="server" Value='<%# Eval("LastName") %>' />
                        <asp:HiddenField ID="hdfEmail" runat="server" Value='<%# Eval("Email") %>' />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ชื่อ-นามสกุล">
                    <ItemTemplate>
                        <asp:Label ID="grdLbName" runat="server" Text='<%# Eval("FirstName") + " " + Eval("LastName") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Left" />       
                </asp:TemplateField>
                <asp:BoundField DataField="Email" HeaderText="Email" HeaderStyle-HorizontalAlign="Center" />
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
