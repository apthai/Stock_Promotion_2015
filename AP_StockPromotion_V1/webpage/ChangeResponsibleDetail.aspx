<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangeResponsibleDetail.aspx.cs"
    Inherits="AP_StockPromotion_V1.webpage.ChangeResponsibleDetail" MasterPageFile="~/master/MasterPopup.Master" %>


<asp:Content ID="Contenthead" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .label-caption {
            text-align: right;
        }

        .div-caption {
            text-align: right;
            padding-right: 5px;
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

        .radiux {
            height: 28px;
            text-align: right;
            margin-right: 30px;
            opacity: 0.6;
            filter: alpha(opacity=60);
            border-radius: 9px 9px 9px 9px;
            -moz-border-radius: 9px 9px 9px 9px;
            -webkit-border-radius: 9px 9px 9px 9px;
            border: 0px solid #000000;
        }

        .grad {
            background: rgb(197,222,234); /* Old browsers */
            background: -moz-linear-gradient(top, rgba(197,222,234,1) 0%, rgba(153,223,229,1) 31%, rgba(6,109,171,1) 100%); /* FF3.6+ */
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,rgba(197,222,234,1)), color-stop(31%,rgba(153,223,229,1)), color-stop(100%,rgba(6,109,171,1))); /* Chrome,Safari4+ */
            background: -webkit-linear-gradient(top, rgba(197,222,234,1) 0%,rgba(153,223,229,1) 31%,rgba(6,109,171,1) 100%); /* Chrome10+,Safari5.1+ */
            background: -o-linear-gradient(top, rgba(197,222,234,1) 0%,rgba(153,223,229,1) 31%,rgba(6,109,171,1) 100%); /* Opera 11.10+ */
            background: -ms-linear-gradient(top, rgba(197,222,234,1) 0%,rgba(153,223,229,1) 31%,rgba(6,109,171,1) 100%); /* IE10+ */
            background: linear-gradient(to bottom, rgba(197,222,234,1) 0%,rgba(153,223,229,1) 31%,rgba(6,109,171,1) 100%); /* W3C */
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#c5deea', endColorstr='#066dab',GradientType=0 ); /* IE6-9 */
        }
    </style>
    <script type="text/javascript">
        jQuery(function ($) {
            $('#divNavx').html('โอนสิทธิ์ผู้รับผิดชอบ');
            $(".js-example-basic-single").select2({ width: '100%' });
            $('.js-example-basic-single-ws').select2({ minimumResultsForSearch: Infinity, width: '100%' });

            setDatePicker();
            setTextNumericOnly();
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

        function setDatePicker() {
            $("#<%= txtMemoDate.ClientID %>").datepicker({ dateFormat: "dd/mm/yy" });
            //$("#").datepicker({ dateFormat: "dd/mm/yy" });
        }

        function setTextNumericOnly() {
            jQuery('.numericOnly').keyup(function () {
                this.value = this.value.replace(/[^0-9]/g, '');
            });
        }

        function setTextDigitOnly() {
            jQuery('.numericOnly').keyup(function () {
                this.value = this.value.replace(/[^0-9\.]/g, '');
            });
        }


        function chkAmountChange(_txtId, hdfId) {
            var amt = $('#' + _txtId + '').val();
            var mxv = $('#' + hdfId + '').val();

            var r = parseInt(amt); if (isNaN(r)) { r = 0; $('#' + _txtId + '').val('0'); }
            var m = parseInt(mxv); if (isNaN(m)) { m = 0; }

            if (r > m) { $('#' + _txtId + '').val(m); }
        }

        function uploadClick() {
            $('#<%= btnUpload.ClientID %>').click();
        }

        function setValueHdf(_ddlId, _hdfId) {
            var sel = $('#' + _ddlId + ' option:selected').val();
            $('#' + _hdfId + '').val(sel);
        }

        function closePage() {
            parent.$("#ContentPlaceHolder1_btnSearch").click();
            parent.jQuery.colorbox.close();
            // window.close();
        }
    </script>

    <!-- Add fancyBox -->
    <link rel="stylesheet" href="/fancybox/source/jquery.fancybox.css?v=2.1.5" type="text/css" media="screen" />
    <script type="text/javascript" src="/fancybox/source/jquery.fancybox.pack.js?v=2.1.5"></script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div id="accordion" class="accordion-style1 panel-group">
        <div class="panel panel-default">
            <div class="panel-heading">
                <h4 class="panel-title">
                    <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion" href="#collapseOne">
                        <i class="ace-icon fa fa-angle-down bigger-110" data-icon-hide="ace-icon fa fa-angle-down" data-icon-show="ace-icon fa fa-angle-right"></i>
                        &nbsp;โอนสิทธิ์ผู้รับผิดชอบ
                    </a>
                </h4>
            </div>

            <div class="panel-collapse collapse in" id="collapseOne">
                <div class="panel-body">

                    <div class="col-sm-12">
                        <div class="col-sm-4">
                            <div class="col-sm-4 div-caption">โครงการ</div>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlProject" runat="server" AutoPostBack="True" class="col-sm-12 js-example-basic-single js-states form-control hide" OnSelectedIndexChanged="ddlProject_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="col-sm-4 div-caption">ผู้รับผิดชอบ</div>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlCurResp" runat="server" class="col-sm-12 js-example-basic-single js-states form-control hide"></asp:DropDownList>
                                <asp:HiddenField ID="hdfCurResp" runat="server" />
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="col-sm-4 div-caption">โอนให้</div>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlNewResp" runat="server" class="col-sm-12 js-example-basic-single js-states form-control hide"></asp:DropDownList>
                                <asp:HiddenField ID="hdfNewResp" runat="server" />
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-12">
                        <div class="col-sm-4">
                            <div class="col-sm-4 div-caption">เอกสารอ้างอิง</div>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtMemoNo" runat="server" class="col-sm-12" placeholder="เลขที่เอกสารอ้างอิง"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="col-sm-4 div-caption">วันที่รับแจ้ง</div>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtMemoDate" runat="server" class="col-sm-12" placeholder="วันที่รับแจ้ง"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="col-sm-4 div-caption">แนบไฟล์</div>
                            <div class="col-sm-8">
                                <div class="col-sm-12" id="divUploaded" runat="server" style="display: none;">
                                    <div class="col-sm-8">
                                        <asp:LinkButton ID="lbUploaded" runat="server" OnClick="lbUploaded_Click"></asp:LinkButton>
                                        <asp:HiddenField ID="hdfUploaded" runat="server" />
                                    </div>
                                    <div class="col-sm-4 div-caption">
                                        <asp:ImageButton ID="imgDelFile" runat="server" ImageUrl="~/img/delete_black.png" Width="23px" Style="padding-top: 3px; padding-right: 5px;" OnClick="imgDelFile_Click" />
                                    </div>
                                </div>
                                <asp:FileUpload ID="fileUploadMemo" class="col-sm-12" runat="server" onchange="uploadClick();" />
                                <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" Style="display: none;" />
                                
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-12 div-caption">
                        <div class="col-sm-12 div-caption">
                            <div class="col-sm-12 div-caption">
                                <asp:Button ID="btnShowItemList" runat="server" Text="แสดงรายการสินค้า" class="btn btn-white btn-info btn-sm" Width="168px" OnClick="btnShowItemList_Click" />
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>






    <div class="col-sm-12">
        <asp:GridView ID="grdData" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" class="col-sm-12"
            EmptyDataText="No data." ShowHeaderWhenEmpty="True" OnRowDataBound="grdData_RowDataBound">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="Serial" HeaderText="Serial" HeaderStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="ItemName" HeaderText="ItemName" HeaderStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Model" HeaderText="Model" HeaderStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Color" HeaderText="Color" HeaderStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Dimension" HeaderText="Dimension" HeaderStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Weight" HeaderText="Weight" HeaderStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Price" HeaderText="Price" HeaderStyle-HorizontalAlign="Center" DataFormatString="{0:N2}" />
                <asp:BoundField DataField="Detail" HeaderText="Detail" HeaderStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Remark" HeaderText="Remark" HeaderStyle-HorizontalAlign="Center" />
                <asp:TemplateField HeaderText="จำนวน" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:TextBox ID="grdTxtAmount" runat="server" CssClass="label-caption numericOnly" Text='<%# Eval("Amount") %>' placeholder="0" Width="39px"></asp:TextBox>
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
                        <asp:HiddenField ID="hdfAmount" runat="server" Value='<%# Eval("Amount") %>' />
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
        <asp:Button ID="btnSave" runat="server" Text="บันทึกการโอนสินค้าโปรโมชั่น" class="btn btn-white btn-info btn-sm" Style="display: none;" Width="168px" OnClick="btnSave_Click" />
    </div>


</asp:Content>
