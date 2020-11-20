<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportBudgetIO.aspx.cs"
    Inherits="AP_StockPromotion_V1.webpage.ReportBudgetIO" MasterPageFile="~/Master/MasterPage.Master" %>

<asp:Content ID="Contenthead" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .label-caption {
            text-align: right;
        }

        .div-caption {
            text-align: right;
            padding-right: 5px;
        }

        .div-caption-c {
            text-align: center;
            padding-right: 5px;
        }
        .btnHidden {
            opacity:0.0;
            width:0px;
            height:0px;
        }
    </style>
    <script type="text/javascript">
        jQuery(function ($) {
            $('#divNavx').html('รายงาน >> รายงานแสดงงบประมาณ IO โปรโมชั่น');
            $(".js-example-basic-single").select2({ width: '100%' });
            $('.js-example-basic-single-ws').select2({ minimumResultsForSearch: Infinity, width: '100%' });
            $('.widget-body').height(screen.height - 200);
            setAutoCompleteCompany();
            initTextInput();
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

        function setTextDigitOnly() {
            jQuery('.digitOnly').keyup(function () {
                this.value = this.value.replace(/[^0-9\.]/g, '');
            });
        }

        function setTextNumericOnly() {
            jQuery('.numericOnly').keyup(function () {
                this.value = this.value.replace(/[^0-9]/g, '');
            });
        }

        function setAutoCompleteCompany() {
            /* - Auto Complete Company - */
            $('#<%= txtCompany.ClientID %>').autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "Default.aspx/GetCategoryMasterCompany",
                        data: "{'cmpName':'" + $('#<%= txtCompany.ClientID %>').val() + "'}",
                        dataType: "json",
                        success: function (data) {
                            response(data.d);
                        },
                        error: function (result) {
                            alert("Error");
                        }
                    });
                },
                select: function (event, ui) {
                    // alert(ui.item.label);
                    $('#<%= txtCompany.ClientID %>').val(ui.item.label);
                    document.getElementById('<%= btnEnterCmp.ClientID %>').click();
                }
            });

            /* - Auto Complete Project - */
            $('#<%= txtProject.ClientID %>').autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "Default.aspx/GetCategoryMasterProject",
                        data: "{'prjName':'" + $('#<%= txtProject.ClientID %>').val() + "'}",
                        dataType: "json",
                        success: function (data) {
                            response(data.d);
                        },
                        error: function (result) {
                            alert("Error");
                        }
                    });
                },
                select: function (event, ui) {
                    // alert(ui.item.label);
                    $('#<%= txtProject.ClientID %>').val(ui.item.label);
                    document.getElementById('<%= btnEnterPrj.ClientID %>').click();
                }
            });
        }


        function initTextInput() {
            $('#<%= txtCompany.ClientID %>').keypress(function (e) {
                if (e.which == 13) {
                    e.preventDefault();
                    $('#<%= btnEnterCmp.ClientID %>').click();
                }
            });
            $('#<%= txtProject.ClientID %>').keypress(function (e) {
                if (e.which == 13) {
                    e.preventDefault();
                    $('#<%= btnEnterPrj.ClientID %>').click();
                }
            });
        }


    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <asp:Button ID="Button1" runat="server" CssClass="btnHidden"  />

    <div class="col-sm-12">
        <div class="widget-box">
            <div class="widget-header">
                <h4 class="smaller">&nbsp;</h4>
            </div>

            <div class="widget-body">
                <div class="widget-main">

                    <div class="col-sm-6">
                        <div class="col-sm-12">
                            <div class="col-sm-3 div-caption">เงื่อนไข-บริษัท</div>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtCompany" runat="server" placeholder="บริษัท.." class="col-sm-9"></asp:TextBox>
                                <asp:Button ID="btnEnterCmp" runat="server" CssClass="btnHidden" OnClick="btnEnterCmp_Click" />
                            </div>
                        </div>
                        <div class="col-sm-12">
                            <div class="col-sm-3 div-caption"></div>
                            <div class="col-sm-9">
                                <asp:GridView ID="grdCompany" runat="server" ShowHeader="False" AutoGenerateColumns="False" class="col-sm-9"
                                    CellPadding="4" ForeColor="#333333" GridLines="None" OnRowCommand="grdCompany_RowCommand">
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    <Columns>
                                        <asp:BoundField DataField="FullName" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgDelCmp" runat="server" ImageUrl="~/img/delete_icon.png" Width="23px" CommandName="delCmp" CommandArgument='<%# Eval("CompanyID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EditRowStyle BackColor="#999999" />
                                    <FooterStyle BackColor="#5D7B9D" ForeColor="White" Font-Bold="True" />
                                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="col-sm-12">
                            <div class="col-sm-3 div-caption">เงื่อนไข-โครงการ</div>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtProject" runat="server" placeholder="โครงการ.." class="col-sm-9"></asp:TextBox>
                                <asp:Button ID="btnEnterPrj" runat="server" CssClass="btnHidden" OnClick="btnEnterPrj_Click" />
                            </div>
                        </div>
                        <div class="col-sm-12">
                            <div class="col-sm-3 div-caption"></div>
                            <div class="col-sm-9">
                                <asp:GridView ID="grdProject" runat="server" ShowHeader="False" AutoGenerateColumns="False" class="col-sm-9"
                                    CellPadding="4" ForeColor="#333333" GridLines="None" OnRowCommand="grdProject_RowCommand">
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    <Columns>
                                        <asp:BoundField DataField="ProjectName" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgDelPrj" runat="server" ImageUrl="~/img/delete_icon.png" Width="23px" CommandName="delPrj" CommandArgument='<%# Eval("ProjectCode") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EditRowStyle BackColor="#999999" />
                                    <FooterStyle BackColor="#5D7B9D" ForeColor="White" Font-Bold="True" />
                                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                    
                    <div class="col-sm-12" style="text-align:right;">
                        <asp:Button ID="btnExportReport" runat="server" Text="แสดงรายงาน" class="btn btn-white btn-info btn-sm" Width="123px" OnClick="btnExportReport_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
