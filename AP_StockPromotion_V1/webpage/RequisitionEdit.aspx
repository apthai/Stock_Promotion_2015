<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RequisitionEdit.aspx.cs"
    Inherits="AP_StockPromotion_V1.web.RequisitionEdit" MasterPageFile="~/Master/MasterPage.Master" %>

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
            $(".js-example-basic-single").select2({ width: '100%' });
            $(".js-example-basic-single-plc").select2();
            $('.js-example-basic-single-ws').select2({ minimumResultsForSearch: Infinity, width: '100%' });

            setTextNumericOnly();
            setTextDigitOnly();
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
            $("#<%= txtReqDocDate.ClientID %>").datepicker({ dateFormat: "dd/mm/yy" });
            //$("# txtRequestDate.ClientID ").datepicker({ dateFormat: "dd/mm/yy" });
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
            $('.select2').attr('style', 'width:100%;');
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
            parent.$("#ContentPlaceHolder1_btnSearch").click();
            closePage();
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

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:HiddenField ID="hdfTabActive" runat="server" Value="lireq" />
    <asp:HiddenField ID="hdfReqId" runat="server" Value="" />
    <asp:HiddenField ID="hdfReqDelRemark" runat="server" Value="" />

    <div class="tabbable">
        <ul class="nav nav-tabs" id="myTab">
            <li id="lireq" class="active">
                <a data-toggle="tab" href="#req" aria-expanded="true">
                    <i class="green ace-icon fa fa-folder-open-o bigger-120"></i>
                    รายละเอียดใบเบิก
                </a>
            </li>

            <li id="lireqdetail" class="">
                <a data-toggle="tab" href="#reqdetail" aria-expanded="true">
                    <i class="green ace-icon glyphicon glyphicon-list bigger-120"></i>
                    รายละเอียดสินค้า
                </a>
            </li>
        </ul>

        <div class="tab-content">
            <div id="req" class="tab-pane fade active in">

                <div class="col-sm-6">
                    <div class="col-sm-4 div-caption">เลขที่เอกสารใบเบิก</div>
                    <div class="col-sm-6">
                        <div class="col-sm-11">

                            <asp:TextBox ID="txtReqDocNo" runat="server" class="col-sm-12" ReadOnly="true" placeholder="เลขที่เอกสาร"></asp:TextBox>

                        </div>
                        <div class="col-sm-1">
                            <%--<label style="color: red">*</label>--%>
                        </div>
                    </div>
                    <div class="col-sm-2"></div>
                </div>
                <div class="col-sm-6">
                    <div class="col-sm-3 div-caption">เลขที่เอกสารอ้างอิง  </div>
                    <div class="col-sm-6">
                        <div class="col-sm-11">
                            <asp:TextBox ID="txtReqNo" runat="server" class="col-sm-12" placeholder="เลขที่เอกสารอ้างอิง"></asp:TextBox>
                        </div>
                        <div class="col-sm-1">
                            <label style="color: red">*</label>
                        </div>
                    </div>
                    <div class="col-sm-3"></div>
                </div>
                <div class="col-sm-12 div-caption">&nbsp;</div>

                <div class="col-sm-6">
                    <div class="col-sm-4 div-caption">วันที่รับเอกสารขอเบิก  </div>
                    <div class="col-sm-6">
                        <div class="col-sm-11">
                            <asp:TextBox ID="txtReqDocDate" runat="server" class="col-sm-12" placeholder="วันที่รับเอกสารขอเบิก"></asp:TextBox>
                        </div>
                        <div class="col-sm-1">
                            <label style="color: red">*</label>
                        </div>
                    </div>
                    <div class="col-sm-4"></div>
                </div>
                <div class="col-sm-6">
                    <div class="col-sm-3 div-caption">วันที่บันทึกใบขอเบิก</div>
                    <div class="col-sm-6">
                        <div class="col-sm-11">
                            <asp:TextBox ID="txtRequestDate" runat="server" class="col-sm-12" placeholder="วันที่บันทึกใบขอเบิก" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-sm-1">
                            <%--<label style="color: red">*</label>--%>
                        </div>
                    </div>
                    <div class="col-sm-3"></div>
                </div>
                <div class="col-sm-12 div-caption">&nbsp;</div>

                <div class="col-sm-6">
                    <div class="col-sm-4 div-caption">ผู้ขอเบิก  </div>
                    <div class="col-sm-6">
                        <div class="col-sm-11">
                            <asp:DropDownList ID="ddlUser" runat="server" class="col-sm-12 js-example-basic-single js-states form-control hide"></asp:DropDownList>
                        </div>
                        <div class="col-sm-1">
                            <label style="color: red">*</label></div>
                    </div>
                    <div class="col-sm-2"></div>
                </div>
                <div class="col-sm-6">
                    <div class="col-sm-3 div-caption">ประเภทโปรโมชั่น  </div>
                    <div class="col-sm-6">
                        <div class="col-sm-11">
                            <asp:DropDownList ID="ddlReqType" runat="server" class="col-sm-12 js-example-basic-single-ws js-states form-control hide" Width="100%"></asp:DropDownList>
                        </div>
                        <div class="col-sm-1">
                            <label style="color: red">*</label>
                        </div>
                    </div>
                    <div class="col-sm-3"></div>
                </div>
                <div class="col-sm-12 div-caption">&nbsp;</div>

                <div class="col-sm-12">
                    <div class="col-sm-2 div-caption">หมายเหตุ</div>
                    <div class="col-sm-9">
                        <asp:TextBox ID="txtReqHeaderRemark" runat="server" class="col-sm-12" placeholder="หมายเหตุ" TextMode="MultiLine" Height="100px"></asp:TextBox>
                    </div>
                    <div class="col-sm-1"></div>
                </div>
                <div class="col-sm-12 div-caption">&nbsp;</div>
            </div>

            <div id="reqdetail" class="tab-pane fade">
                <div class="col-sm-12">
                    <div class="col-sm-5">
                        <div class="col-sm-3 div-caption">โครงการ</div>
                        <div class="col-sm-9">
                            <asp:DropDownList ID="ddlProject" runat="server" class="col-sm-12 js-example-basic-single js-states form-control hide"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="col-sm-4 div-caption">ระยะเวลาใช้งาน</div>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtDateStart" runat="server" placeholder="วันที่เริ่มต้น" class="col-sm-5"></asp:TextBox>
                            <div class="col-sm-2" style="text-align: center;">-</div>
                            <asp:TextBox ID="txtDateEnd" runat="server" placeholder="วันที่สิ้นสุด" class="col-sm-5"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-sm-3">
                        <div class="col-sm-6 div-caption">แจ้งเตือนก่อน</div>
                        <asp:TextBox ID="txtAlertDate" runat="server" placeholder="วัน" class="col-sm-4 label-caption numericOnly"></asp:TextBox>
                        <div class="col-sm-2 div-caption">วัน</div>
                    </div>
                    <div class="col-sm-12">&nbsp;</div>

                    <div class="col-sm-5">
                        <div class="col-sm-3 div-caption">สินค้าโปรโมชั่น</div>
                        <div class="col-sm-9">
                            <asp:DropDownList ID="ddlItem" runat="server" class="col-sm-12 js-example-basic-single js-states form-control hide"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="col-sm-4 div-caption">จำนวน</div>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtReqAmount" runat="server" placeholder="จำนวน" class="col-sm-5 label-caption numericOnly"></asp:TextBox>
                            <div class="col-sm-2" style="text-align: center;">
                                <asp:Label ID="lbUnit" runat="server" Text="ชิ้น" class="label-caption"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-3 div-caption">
                        <div class="col-sm-6 div-caption"></div>
                        <asp:Button ID="btnAddRequest" runat="server" Text="เพิ่มรายการเบิก" class="btn btn-white btn-info btn-sm col-sm-6" OnClick="btnAddRequest_Click" />
                    </div>
                </div>

                <div class="col-sm-12">
                    <div class="col-sm-12">&nbsp;</div>
                    <div class="col-sm-12">
                        <asp:GridView ID="grdData" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" class="col-sm-12"
                            EmptyDataText="No data." ShowHeaderWhenEmpty="True" OnRowCommand="grdData_RowCommand">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:BoundField DataField="ProjectName" HeaderText="โครงการ" HeaderStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="ItemName" HeaderText="สินค้าโปรโมชั่น" HeaderStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="ReqAmount" HeaderText="จำนวน" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="ItemUnit" HeaderText="หน่วย" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="75px" Visible="false" />
                                <asp:BoundField DataField="ProStartDate" HeaderText="วันเริ่มต้น" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="ProEndDate" HeaderText="วันสิ้นสุด" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="ProAlertDate" HeaderText="เตือนก่อน(วัน)" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="75px" />
                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgDel" runat="server" ImageUrl="~/img/delete_black.png" Width="23px" Style="vertical-align: baseline;" CommandName="delReq" CommandArgument='<%# Eval("ReqId") %>' />
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
    <div class="col-sm-6">
        <asp:Button ID="btnBack" runat="server" Text="<< กลับ" class="btn btn-white btn-warning btn-sm" Width="123px" OnClick="btnBack_Click" />
    </div>
    <div class="col-sm-6" style="text-align: right;" id="divBtnCreate">
        <asp:Button ID="btnOK" runat="server" Text="บันทึกรายการตั้งเบิก" class="btn btn-white btn-info btn-sm" Width="125px" OnClick="btnOK_Click" />
        &nbsp;
        <asp:Button ID="btnDelReqRemark" runat="server" Text="ยกเลิกใบเบิก" class="btn btn-white btn-danger btn-sm" Width="125px" Style="display: none;" OnClientClick="OpenColorBox('RequisitionDeleteRemark.aspx','51%','43%'); return false;" />
        <asp:Button ID="btnDelReq" runat="server" Text="ยกเลิกใบเบิก(ของจริง)" class="btn btn-white btn-danger btn-sm" Width="125px" Style="display: none;" OnClick="btnDelReq_Click" />
    </div>
</asp:Content>
