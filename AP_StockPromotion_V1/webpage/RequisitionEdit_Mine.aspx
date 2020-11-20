<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RequisitionEdit_Mine.aspx.cs"
    Inherits="AP_StockPromotion_V1.web.RequisitionEdit_Mine" MasterPageFile="~/Master/MasterPage.Master" %>

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
            $('#divNavx').html('ตั้งเบิกสินค้าโปรโมชั่น');
            tabContentHeight();
            setDatePicker();
            switchTab();
        });

        $(window).resize(function () {
            tabContentHeight();
        });

        function tabContentHeight() {
            var scr_H = $(window).height();
            var h = (scr_H - 230);
            $('.tab-content').height(h);
        }

        function doNothing() {
            window.location.replace("Requisition.aspx");
        }

        function setDatePicker() {
            $("#<%= txtRequestDate.ClientID %>").datepicker({ dateFormat: "dd/mm/yy" });
            $("#<%= txtDateStart.ClientID %>").datepicker({ dateFormat: "dd/mm/yy" });
            $("#<%= txtDateEnd.ClientID %>").datepicker({ dateFormat: "dd/mm/yy" }); // { dateFormat: "dd-mm-yy" }
        }


        function switchTab() {
            var tab = $("#<%= hdfTabActive.ClientID %>").val();
            if (tab == 'lireq') {
                switchTabReq();
            } else {
                switchTabReqDetail();
            }
        }
        function switchTabReq() {
            $('#lireq').removeClass('active');
            $('#lireqdetail').removeClass('active');
            $('#lireq').addClass('active');

            $('#req').removeClass('active');
            $('#reqdetail').removeClass('active');
            $('#req').addClass('in active');

            $('#lireqdetail > a').attr("aria-expanded", "false");
            $('#lireq > a').attr("aria-expanded", "true");
        }

        function switchTabReqDetail() {
            $('#lireq').removeClass('active');
            $('#lireqdetail').removeClass('active');
            $('#lireqdetail').addClass('active');

            $('#req').removeClass('active');
            $('#reqdetail').removeClass('active');
            $('#reqdetail').addClass('in active');

            $('#lireq > a').attr("aria-expanded", "false");
            $('#lireqdetail > a').attr("aria-expanded", "true");
        }

        function closePage() {
            window.close();
        }

        function bindDataParentPage() {
            $("#ContentPlaceHolder1_btnSearch", opener.document).click();
            closePage();
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:HiddenField ID="hdfTabActive" runat="server" Value="lireq" />
    <asp:HiddenField ID="hdfReqId" runat="server" Value="" />

    <div class="tabbable">
        <ul class="nav nav-tabs" id="myTab">
            <li id="lireq" class="active">
                <a data-toggle="tab" href="#req" aria-expanded="true">
                    <i class="green ace-icon fa fa-folder-open-o bigger-120"></i>
                    ใบเบิก
                </a>
            </li>

            <li id="lireqdetail" class="">
                <a data-toggle="tab" href="#reqdetail" aria-expanded="true">
                    <i class="green ace-icon glyphicon glyphicon-list bigger-120"></i>
                    รายการเบิก
                </a>
            </li>
        </ul>

        <div class="tab-content">
            <div id="req" class="tab-pane fade active in">
                <div class="col-sm-12">
                    <div class="col-sm-2"></div>
                    <div class="col-sm-2 div-caption">เลขที่เอกสารอ้างอิง</div>
                    <div class="col-sm-4">
                        <asp:TextBox ID="txtReqNo" runat="server" class="col-sm-12" placeholder="Request No."></asp:TextBox>
                    </div>
                    <div class="col-sm-4"></div>
                </div>
                <div class="col-sm-12 div-caption">&nbsp;</div>
                <div class="col-sm-12">
                    <div class="col-sm-2"></div>
                    <div class="col-sm-2 div-caption">วันที่เบิก</div>
                    <div class="col-sm-4">
                        <asp:TextBox ID="txtRequestDate" runat="server" class="col-sm-12" placeholder="วันที่เบิก"></asp:TextBox>
                    </div>
                    <div class="col-sm-4"></div>
                </div>
                <div class="col-sm-12 div-caption">&nbsp;</div>
                <div class="col-sm-12">
                    <div class="col-sm-2"></div>
                    <div class="col-sm-2 div-caption">ผู้เบิก</div>
                    <div class="col-sm-4">
                        <asp:TextBox ID="txtRequestBy" runat="server" class="col-sm-12" placeholder="Request By"></asp:TextBox>
                    </div>
                    <div class="col-sm-4"></div>
                </div>
                <div class="col-sm-12 div-caption">&nbsp;</div>
                <div class="col-sm-12">
                    <div class="col-sm-2"></div>
                    <div class="col-sm-2 div-caption">เบิกสำหรับ</div>
                    <div class="col-sm-4">
                        <asp:DropDownList ID="ddlReqType" runat="server" class="col-sm-12"></asp:DropDownList>
                    </div>
                    <div class="col-sm-4"></div>
                </div>
                <div class="col-sm-12 div-caption">&nbsp;</div>
                <div class="col-sm-12">
                    <div class="col-sm-2"></div>
                    <div class="col-sm-2 div-caption">หมายเหตุ</div>
                    <div class="col-sm-4">
                        <asp:TextBox ID="txtReqHeaderRemark" runat="server" class="col-sm-12" placeholder="หมายเหตุ" TextMode="MultiLine" Height="100px"></asp:TextBox>
                    </div>
                    <div class="col-sm-4"></div>
                </div>
                <div class="col-sm-12 div-caption">&nbsp;</div>
            </div>

            <div id="reqdetail" class="tab-pane fade">
                <div class="col-sm-12">
                    <div class="col-sm-2 div-caption">โครงการ</div>
                    <div class="col-sm-3">
                        <asp:DropDownList ID="ddlProject" runat="server" class="col-sm-12"></asp:DropDownList>
                    </div>
                    <div class="col-sm-2 div-caption">วันที่</div>
                    <div class="col-sm-3">
                        <asp:TextBox ID="txtDateStart" runat="server" placeholder="วันที่เริ่มต้น" class="col-sm-5"></asp:TextBox>
                        <div class="col-sm-2" style="text-align: center;">-</div>
                        <asp:TextBox ID="txtDateEnd" runat="server" placeholder="วันที่สิ้นสุด" class="col-sm-5"></asp:TextBox>
                    </div>
                    <div class="col-sm-2"></div>
                </div>
                <div class="col-sm-12">
                    <div class="col-sm-2 div-caption">สินค้าโปรโมชั่น</div>
                    <div class="col-sm-3">
                        <asp:DropDownList ID="ddlItem" runat="server" class="col-sm-12"></asp:DropDownList>
                    </div>
                    <div class="col-sm-2 div-caption">จำนวน</div>
                    <div class="col-sm-3">
                        <asp:TextBox ID="txtReqAmount" runat="server" placeholder="จำนวน" class="col-sm-5 label-caption" onkeyup="if (/\D/g.test(this.value)) this.value = this.value.replace(/\D/g,'');"></asp:TextBox>
                        <div class="col-sm-2" style="text-align: center;">
                            <asp:Label ID="lbUnit" runat="server" Text="ชิ้น" class="label-caption"></asp:Label>
                        </div>
                        <asp:Button ID="btnAddRequest" runat="server" Text="เพิ่มรายการเบิก" class="btn btn-white btn-info btn-sm" OnClick="btnAddRequest_Click" />

                    </div>
                    <div class="col-sm-12">&nbsp;</div>
                    <div class="col-sm-12">
                        <asp:GridView ID="grdData" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" class="col-sm-12"
                            EmptyDataText="No data." ShowHeaderWhenEmpty="True" OnRowCommand="grdData_RowCommand">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgDel" runat="server" ImageUrl="~/img/delete.png" Width="23px" Style="vertical-align: baseline;" CommandName="delReq" CommandArgument='<%# Eval("ReqId") %>' />
                                        <asp:HiddenField ID="grdHdfReqHeaderId" runat="server" Value='<%# Eval("ReqHeaderId") %>' />
                                        <asp:HiddenField ID="grdHdfReqNo" runat="server" Value='<%# Eval("ReqNo") %>' />
                                        <asp:HiddenField ID="grdHdfReqDate" runat="server" Value='<%# Eval("ReqDate") %>' />
                                        <asp:HiddenField ID="grdHdfReqBy" runat="server" Value='<%# Eval("ReqBy") %>' />
                                        <asp:HiddenField ID="grdHdfReqType" runat="server" Value='<%# Eval("ReqType") %>' />
                                        <asp:HiddenField ID="grdHdfReqHeaderRemark" runat="server" Value='<%# Eval("ReqHeaderRemark") %>' />
                                        <asp:HiddenField ID="grdHdfReqId" runat="server" Value='<%# Eval("ReqId") %>' />
                                        <asp:HiddenField ID="grdHdfProject_Id" runat="server" Value='<%# Eval("Project_Id") %>' />
                                        <asp:HiddenField ID="grdHdfProjectID" runat="server" Value='<%# Eval("ProjectID") %>' />
                                        <asp:HiddenField ID="grdHdfItemId" runat="server" Value='<%# Eval("ItemId") %>' />
                                        <asp:HiddenField ID="grdHdfItemNo" runat="server" Value='<%# Eval("ItemNo") %>' />
                                        <asp:HiddenField ID="grdHdfItemName" runat="server" Value='<%# Eval("ItemName") %>' />
                                        <asp:HiddenField ID="grdHdfReqAmount" runat="server" Value='<%# Eval("ReqAmount") %>' />
                                        <asp:HiddenField ID="grdHdfItemUnit" runat="server" Value='<%# Eval("ItemUnit") %>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ProjectName" HeaderText="โครงการ" HeaderStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="ItemName" HeaderText="สินค้าโปรโมชั่น" HeaderStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="ReqAmount" HeaderText="จำนวน" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="ItemUnit" HeaderText="หน่วย" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="75px" />
                                <asp:BoundField DataField="ProStartDate" HeaderText="วันเริ่มต้น" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="ProEndDate" HeaderText="วันสิ้นสุด" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" DataFormatString="{0:dd/MM/yyyy}" />
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
                </div>
            </div>
        </div>
    </div>
    <div class="col-sm-12" style="text-align: right;" id="divBtnCreate">
        <asp:Button ID="btnOK" runat="server" Text="ตั้งเรื่องขอเบิก" class="btn btn-white btn-info btn-sm" Width="100px" OnClick="btnOK_Click" />
        &nbsp;
        <asp:Button ID="btnDelReq" runat="server" Text="ยกเลิกใบเบิก" class="btn btn-white btn-danger btn-sm" Width="100px" Style="display: none;" OnClick="btnDelReq_Click" />
    </div>
</asp:Content>
