<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangeProjectResponsible.aspx.cs" 
    Inherits="AP_StockPromotion_V1.webpage.ChangeProjectResponsible" MasterPageFile="~/master/MasterPage.Master" %>

<asp:Content ID="Contenthead" ContentPlaceHolderId="head" runat="server">
    <style type="text/css">
        .label-caption {
            text-align:right;
        }
        .div-caption {
            text-align:right;
            padding-right:5px;
        }


        .ui-autocomplete {
            max-height: 200px;
            overflow-y: auto;
            /* prevent horizontal scrollbar */
            overflow-x: hidden;
        }
        /* IE 6 doesn't support max-height
	     * we use height instead, but this forces the menu to always be this tall
	     */
        * html .ui-autocomplete {
            height: 200px;
        }
	
    </style>
    <script type="text/javascript">
        jQuery(function ($) {
            $('#divNavx').html('โอนของข้ามโครงการ');
            $(".js-example-basic-single").select2({ width: '100%' });
            $('.js-example-basic-single-ws').select2({ minimumResultsForSearch: Infinity, width: '100%' });
        });
        //function popupMasterItemCreate() {
        //    Popup80('MasterItemDetail.aspx?mode=Create');
        //}

        //function popupMasterItemDetail(MasterItemId) {
        //    Popup80('MasterItemDetail.aspx?mode=Edit&MasterItemId=' + MasterItemId);
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

        function chkAmountChange(_txtId, hdfId) {
            var amt = $('#' + _txtId + '').val();
            var mxv = $('#' + hdfId + '').val();

            var r = parseInt(amt); if (isNaN(r)) { r = 0; $('#' + _txtId + '').val('0'); }
            var m = parseInt(mxv); if (isNaN(m)) { m = 0; }

            if (r > m) { $('#' + _txtId + '').val(m); }
        }

        function setValueHdf(_ddlId, _hdfId) {
            var sel = $('#' + _ddlId + ' option:selected').val();
            $('#' + _hdfId + '').val(sel);
        }

    </script>

    <!-- Add fancyBox -->
<%--    <link rel="stylesheet" href="/fancybox/source/jquery.fancybox.css?v=2.1.5" type="text/css" media="screen" />
    <script type="text/javascript" src="/fancybox/source/jquery.fancybox.pack.js?v=2.1.5"></script>--%>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderId="ContentPlaceHolder1" runat="server">
    
    <asp:HiddenField ID="hdfProjectSend" runat="server" />
    <asp:HiddenField ID="hdfProjectReceive" runat="server" />
    <asp:HiddenField ID="hdfItemSend" runat="server" />

    <div id="accordion" class="accordion-style1 panel-group">
        <div class="panel panel-default">
            <div class="panel-heading">
                <h4 class="panel-title">
                    <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion" href="#collapseOne">
                        <i class="ace-icon fa fa-angle-down bigger-110" data-icon-hide="ace-icon fa fa-angle-down" data-icon-show="ace-icon fa fa-angle-right"></i>
                        &nbsp;โอนของข้ามโครงการ
                    </a>
                </h4>
            </div>

            <div class="panel-collapse collapse in" id="collapseOne">
                <div class="panel-body">

                    <div class="col-sm-12">
                        <div class="col-sm-5">
                            <div class="col-sm-4 div-caption">โครงการที่ถือครอง</div>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlProjectSend" runat="server" class="col-sm-12 js-example-basic-single js-states form-control hide" ></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-5">
                            <div class="col-sm-4 div-caption">โครงการรับโอน</div>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlProjectReceive" runat="server" class="col-sm-12 js-example-basic-single js-states form-control hide" ></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-2">
                            <div class="col-sm-12">                                
                            </div>
                        </div>
                    </div>

                    <div class="col-sm-12">
                        <div class="col-sm-5">
                            <div class="col-sm-4 div-caption">สินค้า</div>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlItem" runat="server" class="col-sm-12 js-example-basic-single js-states form-control hide" ></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-5">
                            <div class="col-sm-4 div-caption"></div>
                            <div class="col-sm-8">
                                <asp:Button ID="btnShowItemList" runat="server" Text="แสดงรายการสินค้า" class="btn btn-white btn-info btn-sm" Width="168px" OnClick="btnShowItemList_Click" />
                            </div>
                        </div>
                        <div class="col-sm-2">
                            <div class="col-sm-12">                                
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>






    <div class="col-sm-12">
        <h6>
            <asp:Label ID="lbSelectedProject" runat="server" Text=""></asp:Label>
        </h6>
        <asp:GridView ID="grdData" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" class="col-sm-12"
            EmptyDataText="No data." ShowHeaderWhenEmpty="True" OnRowDataBound="grdData_RowDataBound">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="ItemName" HeaderText="ItemName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Model" HeaderText="Model" HeaderStyle-HorizontalAlign="Center" Visible="false" />
                <asp:BoundField DataField="Color" HeaderText="Color" HeaderStyle-HorizontalAlign="Center" Visible="false" />
                <asp:BoundField DataField="Dimension" HeaderText="Dimension" HeaderStyle-HorizontalAlign="Center" Visible="false" />
                <asp:BoundField DataField="Weight" HeaderText="Weight" HeaderStyle-HorizontalAlign="Center" Visible="false" />
                <asp:BoundField DataField="Price" HeaderText="Price" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="Serial" HeaderText="Serial" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Detail" HeaderText="Detail" HeaderStyle-HorizontalAlign="Center" Visible="false" />
                <asp:BoundField DataField="Remark" HeaderText="Remark" HeaderStyle-HorizontalAlign="Center" Visible="false" />
                <asp:BoundField DataField="FullName" HeaderText="ผู้รับผิดชอบ" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="ReqDocNo" HeaderText="เลขที่ใบเบิก" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="UnitNo" HeaderText="เลขแปลง/ห้อง" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Amount" HeaderText="คงเหลือ" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                <asp:TemplateField HeaderText="จำนวน" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:TextBox ID="grdTxtAmount" runat="server" CssClass="label-caption numericOnly" placeholder="0" Width="39px"></asp:TextBox>
                        <asp:HiddenField ID="hdfSerial" runat="server" Value='<%# Eval("Serial") %>' />
                        <asp:HiddenField ID="hdfItemName" runat="server" Value='<%# Eval("ItemName") %>' />
                        <asp:HiddenField ID="hdfModel" runat="server" Value='<%# Eval("Model") %>' />
                        <asp:HiddenField ID="hdfColor" runat="server" Value='<%# Eval("Color") %>' />
                        <asp:HiddenField ID="hdfDimension" runat="server" Value='<%# Eval("Dimension") %>' />
                        <asp:HiddenField ID="hdfWeight" runat="server" Value='<%# Eval("Weight") %>' />
                        <asp:HiddenField ID="hdfPrice" runat="server" Value='<%# Eval("Price") %>' />
                        <asp:HiddenField ID="hdfExpireDate" runat="server" Value='<%# Eval("ExpireDate") %>' />
                        <asp:HiddenField ID="hdfDetail" runat="server" Value='<%# Eval("Detail") %>' />
                        <asp:HiddenField ID="hdfRemark" runat="server" Value='<%# Eval("Remark") %>' />
                        <asp:HiddenField ID="hdfUserResponse" runat="server" Value='<%# Eval("UserResponse") %>' />
                        <asp:HiddenField ID="hdfFullName" runat="server" Value='<%# Eval("FullName") %>' />
                        <asp:HiddenField ID="hdfAmount" runat="server" Value='<%# Eval("Amount") %>' />
                        <asp:HiddenField ID="hdfReqDocNo" runat="server" Value='<%# Eval("ReqDocNo") %>' />
                        <asp:HiddenField ID="hdfUnitNo" runat="server" Value='<%# Eval("UnitNo") %>' />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" Width="30px"></ItemStyle>
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
    <div class="col-sm-12" style="text-align: right;">&nbsp;</div>
    <div class="col-sm-12 div-caption">
        <asp:Button ID="btnSave" runat="server" Text="XXXXXXXX-SAVE-XXXXXXXX" class="btn btn-white btn-info btn-sm" Style="display: none;" OnClick="btnSave_Click" />
    </div>

</asp:Content>