
<%@ Page Title="Management PR Expense" Language="C#" MasterPageFile="~/master/MasterPage.Master"
    AutoEventWireup="true" CodeBehind="MasterApproverPR.aspx.cs" Inherits="AP_StockPromotion_V1.webpage.MasterApproverPR" %>



<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="server">

    <link href="../css/UpdProcess.css" rel="stylesheet" />

    <style type="text/css">
        .label-caption {
            text-align: right;
        }

        .div-caption {
            text-align: right;
        }
    </style>

    <script type="text/javascript">

        jQuery(function ($) {
            $(".js-example-basic-single").select2({ width: '100%' });
            $('.js-example-basic-single-ws').select2({ minimumResultsForSearch: Infinity, width: '100%' });
        });

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">

                <asp:UpdatePanel ID="upnmain" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>

                        <div id="accordion" class="accordion-style1 panel-group">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h4 class="panel-title">
                                        <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion" href="#collapseOne">
                                            <i class="ace-icon fa fa-angle-down bigger-110" data-icon-hide="ace-icon fa fa-angle-down" data-icon-show="ace-icon fa fa-angle-right"></i>
                                            &nbsp;ผู้อนุมัติ ตามวงเงิน </a>
                                    </h4>
                                </div>

                                <div class="panel-collapse collapse in" id="collapseOne">
                                    <div class="panel-body">
                                        <div class="col-sm-12">

                                            <div class="col-sm-2 div-caption">Cost Center</div>
                                            <div class="col-sm-3">
                                                <asp:TextBox ID="tbCostCenter" runat="server" placeholder="Cost Center" class="col-sm-12"></asp:TextBox>
                                            </div>

                                            <div class="col-sm-2 div-caption">วงเงิน</div>
                                            <div class="col-sm-3">
                                                <asp:DropDownList ID="ddlExpense" runat="server" class="col-sm-12"></asp:DropDownList>
                                            </div>
                                            <div class="col-sm-2 div-caption"></div>
                                        </div>

                                        <div class="col-sm-12">
                                            <div class="col-sm-2 div-caption">ผู้อนุมัติ</div>
                                            <div class="col-sm-3">
                                                <asp:TextBox ID="tbApprover" runat="server" placeholder="ผู้อนุมัติ" class="col-sm-12"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-2 div-caption"></div>
                                        </div>

                                        <div class="col-sm-12" style="text-align: center;">
                                            <asp:Button ID="btnSearch" runat="server" Text="Search" class="btn btn-white btn-info btn-sm" Width="75px" OnClick="btnSearch_Click" />
                                            &nbsp;
                                           
                                            <asp:Button ID="btnClear" runat="server" Text="Clear" class="btn btn-white btn-info btn-sm" Width="75px" OnClick="btnClear_Click" />
                                        </div>

                                        <div class="col-sm-12" style="text-align: center;">
                                            <asp:Button ID="Button3" runat="server" Text="ดึงข้อมูล PR จาก SAP"
                                                class="btn btn-sm btn-white btn-default btn-round" Style="float: right" OnClick="btnGetPRFromSAP_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <asp:UpdateProgress ID="udpg" runat="server" AssociatedUpdatePanelID="upnmain" DisplayAfter="100">
                            <ProgressTemplate>
                                <div class="UpdProcessModal">
                                    <div class="UpdProcessCenter">
                                        <img alt="" src="../img/BlackCat.gif" />
                                        <div>กำลังโหลดข้อมูล กรุณารอซักครู่...</div>
                                    </div>
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <div class="panel-heading">
                    <div style="width: 100%; text-align: center;">
                    </div>
                </div>

                <asp:UpdatePanel ID="upDetail" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="panel-body">
                            <div class="row" style="margin: auto">
                                <asp:GridView ID="gvData" runat="server" Width="90%" AllowPaging="True" AllowSorting="True"
                                    OnPageIndexChanging="OnPaging" PageSize="20" AutoGenerateColumns="false"
                                    OnSorting="OnSorting"
                                    CellPadding="4" ForeColor="#333333" GridLines="None">
                                    <Columns>
                                        <%--<asp:BoundField DataField="CostCenter" HeaderText="Cost Center" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Left" />--%>

                                        <asp:TemplateField HeaderText="Cost Center" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <table style="width: 90%">
                                                    <tr>
                                                        <td style="text-align: left"><%# Eval("CostCenter") %></td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="วงเงิน" ItemStyle-Width="35%" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <table style="width: 90%">
                                                    <tr>
                                                        <td style="width: 30%; text-align: left"><%# Eval("Expense") %></td>
                                                        <td style="width: 30%; font-weight: bold; text-align: left"><%# Eval("MinExpense","{0:#,#0}") %></td>
                                                        <td style="width: 10%; text-align: center">ถึง</td>
                                                        <td style="width: 30%; font-weight: bold; text-align: left"><%# " " + Eval("MaxExpense","{0:#,#0}") %></td>
                                                    </tr>
                                                </table>

                                                <%--<asp:Label ID="lbExpense" runat="server" Text='<%# "  " + Eval("Expense") + " " + Eval("MinExpense","{0:0,00}") + " ถึง " + Eval("MaxExpense","{0:0,00}") %>' />--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="Approver" HeaderText="ผู้อนุมัติ" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Left" />

                                        <asp:TemplateField HeaderText="อัพเดทล่าสุด" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lbUpdateDate" runat="server" Text='<%# Eval("UpdateDate", "{0:dd/MM/yyyy hh:mm:tt}") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                    <AlternatingRowStyle BackColor="White" />
                                    <EditRowStyle BackColor="#2461BF" />
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <PagerSettings PageButtonCount="5" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <RowStyle BackColor="#EFF3FB" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                    <SortedDescendingHeaderStyle BackColor="#4870BE" />

                                </asp:GridView>
                            </div>
                        </div>
                        <asp:UpdateProgress ID="udpg2" runat="server" AssociatedUpdatePanelID="upDetail" DisplayAfter="100">
                            <ProgressTemplate>
                                <div class="UpdProcessModal">
                                    <div class="UpdProcessCenter">
                                        <img alt="" src="../img/BlackCat.gif" />
                                        <div>กำลังโหลดข้อมูล กรุณารอซักครู่...</div>
                                    </div>
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

</asp:Content>
