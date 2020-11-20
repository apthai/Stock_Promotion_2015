<%@ Page Title="Management InternalOrder" Language="C#" MasterPageFile="~/master/MasterPage.Master"
    AutoEventWireup="true" CodeBehind="MasterInternalOrder.aspx.cs" Inherits="AP_StockPromotion_V1.webpage.MasterInternalOrder" %>



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

                                            <div class="col-sm-2 div-caption">Internal Order</div>
                                            <div class="col-sm-3">
                                                <asp:TextBox ID="tbInternalOrder" runat="server" placeholder="InternalOrder" class="col-sm-12"></asp:TextBox>
                                            </div>

                                            <div class="col-sm-2 div-caption">Description</div>
                                            <div class="col-sm-3">
                                                <asp:TextBox ID="tbDescription" runat="server" placeholder="Description" class="col-sm-12"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-2 div-caption"></div>
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
                                    OnPageIndexChanging="OnPaging" PageSize="15" AutoGenerateColumns="false"
                                    OnSorting="OnSorting" OnRowCommand="gvData_RowCommand"
                                    OnRowDeleting="gvData_RowDeleting"
                                    CellPadding="4" ForeColor="#333333" GridLines="None"
                                    ShowHeader="true">
                                    <Columns>
                                        <asp:TemplateField HeaderText="" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Right" ControlStyle-Font-Bold="true">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>.
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Button Text="Delete" runat="server" CommandName="Delete" class="btn btn-xs btn-danger" CommandArgument="<%# Container.DataItemIndex %>" Width="80px" />
                                                <asp:HiddenField ID="hdID" runat="server" Value='<%# Eval("ID") %>' />
                                            </ItemTemplate>

                                            <HeaderTemplate>
                                                <div>
                                                    <h5>Action</h5>
                                                </div>
                                                <div>
                                                    <asp:Button Text="Insert" runat="server" CommandName="Add" class="btn btn-xs btn-primary" Width="80px" />
                                                </div>
                                            </HeaderTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="InternalOrder" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <%# Eval("InternalOrder") %>
                                            </ItemTemplate>

                                            <HeaderTemplate>
                                                <div>
                                                    <h5>InternalOrder</h5>
                                                </div>
                                                <div>
                                                    <asp:TextBox ID="tbAddInternalOrder" runat="server" placeholder="New InternalOrder" class="col-sm-12"></asp:TextBox>
                                                </div>
                                            </HeaderTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Description" ItemStyle-Width="50%" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <%# Eval("Description") %>
                                            </ItemTemplate>

                                            <HeaderTemplate>
                                                <div>
                                                    <h5>Description</h5>
                                                </div>
                                                <div>
                                                    <asp:TextBox ID="tbAddDescription" runat="server" placeholder="New Description" class="col-sm-12"></asp:TextBox>
                                                </div>
                                            </HeaderTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <%--<%# Eval("IsActive") %>--%>
                                                <asp:CheckBox ID="cbIsActive" runat="server" Checked='<%# (bool)DataBinder.Eval(Container.DataItem, "IsActive") %>' Text="Active" Enabled="False" />
                                            </ItemTemplate>

                                            <HeaderTemplate>
                                                <div>
                                                    <h5>Status</h5>
                                                </div>
                                                <div>
                                                    <asp:CheckBox ID="cbAddIsActive" runat="server" placeholder="Active ?" Checked="true" Text="Active" />
                                                </div>
                                            </HeaderTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <AlternatingRowStyle BackColor="White" />
                                    <EditRowStyle BackColor="#2461BF" />
                                    <FooterStyle BackColor="#808080" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#999999" Font-Bold="True" ForeColor="White" />
                                    <PagerSettings PageButtonCount="5" />
                                    <PagerStyle BackColor="#808080" ForeColor="White" HorizontalAlign="Center" />
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
