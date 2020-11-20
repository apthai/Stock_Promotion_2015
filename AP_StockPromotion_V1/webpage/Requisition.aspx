<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Requisition.aspx.cs" 
    Inherits="AP_StockPromotion_V1.web.Requisition" MasterPageFile="~/Master/MasterPage.Master"  %>

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
            $('#divNavx').html('ตรวจสอบยกเลิกขอเบิก CRM');
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

        function popupRequisitionCreate() {
            //Popup80('RequisitionCreate.aspx');
            Popup80('RequisitionEdit.aspx?mode=Create');
        }

        function popupRequisitionEdit(reqId) {
            //Popup80('RequisitionCreate.aspx');
            Popup80('RequisitionEdit.aspx?mode=Edit&reqId=' + reqId);
        }
        //function popupRequisitionEdit() {
        //    Popup80('RequisitionEdit.aspx');
        //}

        //function popupRequisitionReceive(RequisitionStatus) {
        //    Popup80('RequisitionReceive.aspx?RequisitionStatus=' + RequisitionStatus);
        //}

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
            $("#<%= txtDateFrom.ClientID %>").datepicker({ dateFormat: "dd/mm/yy" }); // { dateFormat: "dd-mm-yy" }
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
                        <div class="col-sm-2 div-caption">วันที่บันทึกรายการ</div>
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
                            <asp:CheckBox ID="chkFindDelRequestCRM" runat="server" Text="ใบเบิกจาก CRM" ForeColor="Salmon" />
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
    
    <div class="col-sm-12">
        <%--<button class="btn btn-white btn-info btn-sm" onclick="popupRequisitionCreate();">
            <i class="ace-icon fa glyphicon-plus bigger-120 blue"></i>เบิกสินค้าโปรโมชั่น</button>--%>
        <asp:Button id="btnAddRequest" runat="server" class="btn btn-white btn-info btn-sm" Text="+ สร้างใบเบิกสินค้า" OnClick="btnAddRequest_Click" />
    </div>
    <div class="col-sm-12" >
        <asp:GridView ID="grdData" runat="server" AutoGenerateColumns="False" CellPadding="4" 
            ForeColor="#333333" GridLines="None" class="col-sm-12" OnRowCommand="grdData_RowCommand" OnRowDataBound="grdData_RowDataBound"
            AllowPaging="true" AllowSorting="true" OnPageIndexChanging="grdData_PageIndexChanging" OnSorting="grdData_Sorting" >
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="41px">
                    <ItemTemplate>
                        <asp:ImageButton ID="imgEdit" runat="server" ImageUrl="~/img/writing_file.png" Width="23px" Style="vertical-align: baseline;" CommandName="editReq" CommandArgument='<%# Eval("ReqHeaderId") %>' />
                        <asp:HiddenField ID="grdHdfReqHeaderId" runat="server" Value='<%# Eval("ReqHeaderId") %>' />
                        <asp:HiddenField ID="grdHdfReqDocNo" runat="server" Value='<%# Eval("ReqDocNo") %>' />
                        <asp:HiddenField ID="grdHdfReqDocDate" runat="server" Value='<%# Eval("ReqDocDate") %>' />
                        <asp:HiddenField ID="grdHdfReqNo" runat="server" Value='<%# Eval("ReqNo") %>' />
                        <asp:HiddenField ID="grdHdfReqDate" runat="server" Value='<%# Eval("ReqDate") %>' />
                        <asp:HiddenField ID="grdHdfReqBy" runat="server" Value='<%# Eval("ReqBy") %>' />
                        <asp:HiddenField ID="grdHdfReqType" runat="server" Value='<%# Eval("ReqType") %>' />
                        <asp:HiddenField ID="grdHdfReqHeaderRemark" runat="server" Value='<%# Eval("ReqHeaderRemark") %>' />
                        <asp:HiddenField ID="grdHdfProject_Id" runat="server" Value='<%# Eval("Project_Id") %>' />
                        <asp:HiddenField ID="grdHdfProjectName" runat="server" Value='<%# Eval("ProjectName") %>' />
                        <asp:HiddenField ID="grdHdfProStartDate" runat="server" Value='<%# Eval("ProStartDate") %>' />
                        <asp:HiddenField ID="grdHdfProEndDate" runat="server" Value='<%# Eval("ProEndDate") %>' />
                        <asp:HiddenField ID="grdHdfReqStatus" runat="server" Value='<%# Eval("ReqStatus") %>' />
                        <asp:HiddenField ID="grdHdfReqStatusText" runat="server" Value='<%# Eval("ReqStatusText") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ReqDate" SortExpression="ReqDate" HeaderText="วันที่เบิก" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="123px" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="ReqDocNo" SortExpression="ReqDocNo" HeaderText="เลขที่เอกสารใบเบิก" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="123px" />
                <asp:BoundField DataField="ReqNo" SortExpression="ReqNo" HeaderText="เลขที่เอกสารอ้างอิง" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="123px" />
                <asp:BoundField DataField="ProjectName" SortExpression="ProjectName" HeaderText="โครงการ" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="ProStartDate" SortExpression="ProStartDate" HeaderText="วันที่เริ่มต้น" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="123px" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="ProEndDate" SortExpression="ProEndDate" HeaderText="วันที่สิ้นสุด" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="123px" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="FullName" SortExpression="FullName" HeaderText="ผู้ขอเบิก" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="234px" />
                <asp:BoundField DataField="ReqStatusText" SortExpression="ReqStatusText" HeaderText="สถานะ" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="123px" Visible="false" />
                <asp:TemplateField HeaderText="สถานะ" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="123px">
                    <ItemTemplate>
                        <asp:Label ID="grdLbStatusText" runat="server" Text='<%# Eval("ReqStatusText") %>' />
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
        <!-- <asp:HiddenField ID="grdHdfItemId" runat="server" Value='# Eval("ItemId") ' />
        <asp:HiddenField ID="grdHdfItemName" runat="server" Value='# Eval("ItemName") ' />
        <asp:HiddenField ID="grdHdfReqAmount" runat="server" Value='# Eval("ReqAmount") ' />
        <asp:HiddenField ID="grdHdfItemUnit" runat="server" Value='# Eval("ItemUnit") ' />
        <asp:HiddenField ID="grdHdfItemPricePerUnit" runat="server" Value='# Eval("ItemPricePerUnit") ' />-->
        
        <!--<asp:BoundField DataField="ItemName" HeaderText="สินค้าโปรโมชั่น" />
        <asp:BoundField DataField="ReqAmount" HeaderText="จำนวน" />
        <asp:BoundField DataField="ItemUnit" HeaderText="หน่วย" />
        <asp:BoundField DataField="ItemPricePerUnit" HeaderText="มูลค่า" DataFormatString="{0:#,##0.00}" />
        <asp:TemplateField HeaderText="จำนวนเงินรวม" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <asp:Label ID="lbTotalAmount" runat="server"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>-->
    </div>
    <div class="col-sm-12" style="text-align: right;">&nbsp;</div>
</asp:Content>