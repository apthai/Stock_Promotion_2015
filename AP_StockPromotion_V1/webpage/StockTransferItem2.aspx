<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockTransferItem2.aspx.cs"
    Inherits="AP_StockPromotion_V1.web.StockTransferItem2" MasterPageFile="~/Master/MasterPage.Master" %>


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

            $('#divNavx').html('จ่ายสินค้าให้โครงการ ');
            $(".js-example-basic-single").select2({ width: '100%' });
            $('.js-example-basic-single-ws').select2({ minimumResultsForSearch: Infinity, width: '100%' });

            setDatePicker();
            $('#accordion-style').on('click', function (ev) {
                var target = $('input', ev.target);
                var which = parseInt(target.val());
                if (which == 2) $('#accordion').addClass('accordion-style2');
                else $('#accordion').removeClass('accordion-style2');
            });

            $('#btn-preview').on('click', function (e) {
                e.preventDefault();
                console.log($('#formFilter').serialize());
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "StockTransferItem2.aspx/GetReportUrl",
                    data: "{'StartOrderDate' : '" + $("#<%= txtDateStartFilter.ClientID %>").val() + "','EndOrderDate' : '" + $("#<%= txtDateEndFilter.ClientID %>").val()
                        + "','DOCNO' : '" + $("#<%= txtReqDocNoFilter.ClientID %>").val()
                        + "','REFNO' : '" + $("#<%= txtDocNoFilter.ClientID %>").val()
                        + "','PROJECTID' : '" + $("#<%= ddlProjectFilter.ClientID %>").val()
                        + "','MasterItemID' : '" + $("#<%= ddlItemFilter.ClientID %>").val()
                        + "'}",
                    success: function (data) {
                        Popup60(data.d);
                    },
                    complete: function () { }
                });
            });
        });

        $(document).on('click', '.btn-show-modal', function () {
            $('#<%= txtReqDocNoFilter.ClientID %>').val('');
            $('#<%= txtDocNoFilter.ClientID %>').val('');
            $('#<%= txtDateStartFilter.ClientID %>').val('');
            $('#<%= txtDateEndFilter.ClientID %>').val('');
            $('#<%= ddlProjectFilter.ClientID %>').select2('val', '0');
            $('#<%= ddlItemFilter.ClientID %>').select2('val', '0');
        });

        $(document).on('click', '.btn-show-modal2', function () {
            $('#<%= txtReqDocNoFilter2.ClientID %>').val('');
            $('#<%= txtDocNoFilter2.ClientID %>').val('');
            $('#<%= txtDateStartFilter2.ClientID %>').val('');
            $('#<%= txtDateEndFilter2.ClientID %>').val('');
            $('#<%= ddlProjectFilter2.ClientID %>').select2('val', '0');
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
            $("#<%= txtDateStartFilter.ClientID %>").datepicker({ dateFormat: "dd/mm/yy" });
            $("#<%= txtDateEndFilter.ClientID %>").datepicker({ dateFormat: "dd/mm/yy" });

            $("#<%= txtDateStartFilter2.ClientID %>").datepicker({ dateFormat: "dd/mm/yy" });
            $("#<%= txtDateEndFilter2.ClientID %>").datepicker({ dateFormat: "dd/mm/yy" });
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
                        &nbsp;ค้นหาใบเบิกสินค้าโปรโมชั่น
                    </a>
                </h4>
            </div>

            <div class="panel-collapse collapse in" id="collapseOne">
                <div class="panel-body">
                    <div class="col-sm-12">
                        <div class="col-sm-2 div-caption">เลขที่เอกสารใบเบิก</div>
                        <div class="col-sm-3">
                            <asp:TextBox ID="txtReqDocNo" runat="server" placeholder="เลขที่เอกสาร" class="col-sm-12"></asp:TextBox>
                        </div>
                        <div class="col-sm-2 div-caption">เลขที่เอกสารอ้างอิง</div>
                        <div class="col-sm-3">
                            <asp:TextBox ID="txtReqNo" runat="server" placeholder="เลขที่เอกสารอ้างอิง" class="col-sm-12"></asp:TextBox>
                        </div>
                        <div class="col-sm-2 div-caption"></div>
                    </div>
                    <div class="col-sm-12">
                        <div class="col-sm-2 div-caption">วันที่เบิก</div>
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
                        <div class="col-sm-2 div-caption">โครงการ/หน่วยงาน</div>
                        <div class="col-sm-3">
                            <asp:DropDownList ID="ddlProject" runat="server" class="col-sm-12 js-example-basic-single js-states form-control hide"></asp:DropDownList>
                        </div>
                        <div class="col-sm-2 div-caption">รายละเอียดสินค้า</div>
                        <div class="col-sm-3">
                            <asp:DropDownList ID="ddlItem" runat="server" class="col-sm-12 js-example-basic-single js-states form-control hide"></asp:DropDownList>
                        </div>
                        <div class="col-sm-2 div-caption"></div>
                    </div>
                    <div class="col-sm-12">
                        <div class="col-sm-2 div-caption">สถานะรายการ</div>
                        <div class="col-sm-3">
                            <asp:DropDownList ID="ddlReqStatus" runat="server" class="col-sm-12 js-example-basic-single-ws js-states form-control hide"></asp:DropDownList>
                        </div>
                        <div class="col-sm-2 div-caption"></div>
                        <div class="col-sm-3">
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

    <!-- #accordion -->
    <div class="col-sm-12">
        <asp:Button ID="btnTrnMulReq" runat="server" Text="จ่ายสินค้าให้โครงการ" class="btn btn-white btn-info btn-sm" OnClick="btnTrnMulReq_Click" />
        <a class="btn btn-white btn-danger btn-sm btn-show-modal" data-toggle="modal" data-target="#myModal">รายงานสินค้าที่ยังไม่จ่าย</a>

        <span style="float: right;"><a class="btn btn-white btn-info btn-sm btn-show-modal2" data-toggle="modal" data-target="#myModal2">ทอง Aurora</a></span>
    </div>
    <div class="col-sm-12">
        <asp:GridView ID="grdData" runat="server" AutoGenerateColumns="False" CellPadding="4" PageSize="20" Font-Size="0.9em"
            ForeColor="#333333" GridLines="None" class="col-sm-12" OnRowCommand="grdData_RowCommand" OnRowDataBound="grdData_RowDataBound"
            AllowPaging="True" AllowSorting="true" OnPageIndexChanging="grdData_PageIndexChanging" OnSorting="grdData_Sorting">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:TemplateField HeaderText="เลือก" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkReq" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <span title="Print Transfer Items PreBooking">PB</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:ImageButton ID="imgPrtPB" runat="server" title="Print Transfer Items PreBooking" ImageUrl="~/img/printer_and_fax.png" Width="23px" Style="vertical-align: baseline;" CommandName="PrtTrnPB" />&nbsp;&nbsp;
                   
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:ImageButton ID="imgEdit" runat="server" ImageUrl="~/img/ToProject.png" Width="23px" Style="vertical-align: baseline;" CommandName="takeReq" CommandArgument='<%# Eval("ReqId") %>' Visible="false" />
                        <asp:HiddenField ID="grdHdfReqHeaderId" runat="server" Value='<%# Eval("ReqHeaderId") %>' />
                        <asp:HiddenField ID="grdHdfReqId" runat="server" Value='<%# Eval("ReqId") %>' />
                        <asp:HiddenField ID="grdHdfReqDocNo" runat="server" Value='<%# Eval("ReqDocNo") %>' />
                        <asp:HiddenField ID="grdHdfReqNo" runat="server" Value='<%# Eval("ReqNo") %>' />
                        <asp:HiddenField ID="grdHdfReqDate" runat="server" Value='<%# Eval("ReqDate") %>' />
                        <asp:HiddenField ID="grdHdfReqBy" runat="server" Value='<%# Eval("ReqBy") %>' />
                        <asp:HiddenField ID="grdHdfReqType" runat="server" Value='<%# Eval("ReqType") %>' />
                        <asp:HiddenField ID="grdHdfReqHeaderRemark" runat="server" Value='<%# Eval("ReqHeaderRemark") %>' />
                        <asp:HiddenField ID="grdHdfProject_Id" runat="server" Value='<%# Eval("Project_Id") %>' />
                        <asp:HiddenField ID="grdHdfProjectName" runat="server" Value='<%# Eval("ProjectName") %>' />
                        <asp:HiddenField ID="grdHdfItemName" runat="server" Value='<%# Eval("ItemName") %>' />
                        <asp:HiddenField ID="grdHdfReqStatus" runat="server" Value='<%# Eval("ReqStatus") %>' />
                        <asp:HiddenField ID="grdHdfFunction" runat="server" Value='<%# Eval("Function") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ReqDate" SortExpression="ReqDate" HeaderText="วันที่เบิก" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="ReqDocNo" SortExpression="ReqDocNo" HeaderText="เลขที่เอกสารใบเบิก" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="ReqNo" SortExpression="ReqNo" HeaderText="เลขที่เอกสารอ้างอิง" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="ProjectName" SortExpression="ProjectName" HeaderText="โครงการ/หน่วยงาน" />
                <asp:BoundField DataField="UnitNo" SortExpression="UnitNo" HeaderText="ห้อง/แปลง" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="FullName" SortExpression="FullName" HeaderText="ผู้ขอเบิก" />
                <asp:BoundField DataField="ItemName" SortExpression="ItemName" HeaderText="รายละเอียดสินค้า" />
                <asp:BoundField DataField="ReqAmount" SortExpression="ReqAmount" HeaderText="จำนวนเบิก" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="91px" />
                <asp:BoundField DataField="PRNo" SortExpression="PRNo" HeaderText="PR No." HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="91px" />
                <asp:BoundField DataField="ProEndDate" SortExpression="ProEndDate" HeaderText="วันที่สิ้นสุด" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="grdLbReqStatus" runat="server" Text='<%# Eval("ReqStatusText") %>' SortExpression="ReqStatusText" HeaderText="สถานะ"></asp:Label>
                    </ItemTemplate>
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
    <!-- Modal -->
    <div class="modal fade" id="myModal" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="z-index: 100 !important;">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <h4 class="modal-title" id="myModalLabel">รายงานสินค้าที่ยังไม่จ่าย</h4>
                </div>
                <div class="modal-body">
                    <div class="panel-body">
                        <div class="col-sm-12">
                            <div class="col-sm-2 div-caption">เลขที่เอกสารใบเบิก</div>
                            <div class="col-sm-3">
                                <asp:TextBox ID="txtReqDocNoFilter" runat="server" placeholder="เลขที่เอกสาร" class="col-sm-12"></asp:TextBox>
                            </div>
                            <div class="col-sm-2 div-caption">เลขที่เอกสารอ้างอิง</div>
                            <div class="col-sm-3">
                                <asp:TextBox ID="txtDocNoFilter" runat="server" placeholder="เลขที่เอกสารอ้างอิง" class="col-sm-12"></asp:TextBox>
                            </div>
                            <div class="col-sm-2 div-caption"></div>
                        </div>
                        <div class="col-sm-12">
                            <div class="col-sm-2 div-caption">วันที่เบิก</div>
                            <div class="col-sm-3">
                                <asp:TextBox ID="txtDateStartFilter" runat="server" placeholder="วันที่เริ่มต้น" class="col-sm-12" autocomplete="off"></asp:TextBox>
                            </div>
                            <div class="col-sm-2 div-caption">ถึงวันที่</div>
                            <div class="col-sm-3">
                                <asp:TextBox ID="txtDateEndFilter" runat="server" placeholder="ถึงวันที่" class="col-sm-12" autocomplete="off"></asp:TextBox>
                            </div>
                            <div class="col-sm-2 div-caption"></div>
                        </div>
                        <div class="col-sm-12">
                            <div class="col-sm-2 div-caption">โครงการ/หน่วยงาน</div>
                            <div class="col-sm-3">
                                <asp:DropDownList ID="ddlProjectFilter" runat="server" class="col-sm-12 js-example-basic-single js-states form-control hide"></asp:DropDownList>
                            </div>
                            <div class="col-sm-2 div-caption">รายละเอียดสินค้า</div>
                            <div class="col-sm-3">
                                <asp:DropDownList ID="ddlItemFilter" runat="server" class="col-sm-12 js-example-basic-single js-states form-control hide"></asp:DropDownList>
                            </div>
                            <div class="col-sm-2 div-caption"></div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="bntExcelPurchesingOrder" runat="server" class="btn btn-info" Text="Excel" OnClick="bntExcelPurchesingOrder_Click" />&nbsp;&nbsp;
                   
                    <button type="button" class="btn btn-primary" id="btn-preview">แสดงรายงาน</button>&nbsp;&nbsp;
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">ปิด</button>
                    <%--&nbsp;&nbsp;<asp:Button ID="ButtonX" runat="server" class="btn btn-info" Text="X" OnClick="ButtonX_Click" />--%>
                </div>
            </div>
        </div>
    </div>


    <div class="modal fade" id="myModal2" role="dialog" aria-labelledby="myModalLabel2" aria-hidden="true" style="z-index: 100 !important;">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <h4 class="modal-title" id="myModalLabe2">ทอง Aurora</h4>
                </div>
                <div class="modal-body">
                    <div class="panel-body">
                        <div class="col-sm-12">
                            <div class="col-sm-2 div-caption">เลขที่เอกสารใบเบิก</div>
                            <div class="col-sm-3">
                                <asp:TextBox ID="txtReqDocNoFilter2" runat="server" placeholder="เลขที่เอกสาร" class="col-sm-12"></asp:TextBox>
                            </div>
                            <div class="col-sm-2 div-caption">เลขที่เอกสารอ้างอิง</div>
                            <div class="col-sm-3">
                                <asp:TextBox ID="txtDocNoFilter2" runat="server" placeholder="เลขที่เอกสารอ้างอิง" class="col-sm-12"></asp:TextBox>
                            </div>
                            <div class="col-sm-2 div-caption"></div>
                        </div>
                        <div class="col-sm-12">
                            <div class="col-sm-2 div-caption">วันที่เบิก</div>
                            <div class="col-sm-3">
                                <asp:TextBox ID="txtDateStartFilter2" runat="server" placeholder="วันที่เริ่มต้น" class="col-sm-12" autocomplete="off"></asp:TextBox>
                            </div>
                            <div class="col-sm-2 div-caption">ถึงวันที่</div>
                            <div class="col-sm-3">
                                <asp:TextBox ID="txtDateEndFilter2" runat="server" placeholder="ถึงวันที่" class="col-sm-12" autocomplete="off"></asp:TextBox>
                            </div>
                            <div class="col-sm-2 div-caption"></div>
                        </div>
                        <div class="col-sm-12">
                            <div class="col-sm-2 div-caption">โครงการ/หน่วยงาน</div>
                            <div class="col-sm-3">
                                <asp:DropDownList ID="ddlProjectFilter2" runat="server" class="col-sm-12 js-example-basic-single js-states form-control hide"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-12">
                            <asp:GridView ID="gvExcelFile" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None">
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <EditRowStyle BackColor="#999999" />
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
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
                <div class="modal-footer">
                    <div class="col-sm-12">
                        <div class="col-sm-2 left">
                            <asp:Button ID="bntExportAurora" runat="server" Text="ทอง Aurora" class="btn btn-primary" OnClick="BntExportAurora_Click" /></div>
                        <div class="col-sm-4 left">Import Excel File :
                            <asp:FileUpload ID="FileUpload1" runat="server" />
                            <asp:Label ID="Label1" runat="server"></asp:Label>
                            <asp:Button ID="bntImportAurora" runat="server" Text="ทอง Aurora" class="btn btn-info" OnClick="BntImportAurora_Click" /></div>
                        <div class="col-sm-2"></div>
                        <div class="col-sm-2"></div>
                        <div class="col-sm-2">
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">ปิด</button></div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
