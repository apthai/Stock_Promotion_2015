<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangeResponsibleList.aspx.cs" 
    Inherits="AP_StockPromotion_V1.webpage.ChangeResponsibleList" MasterPageFile="~/master/MasterPage.Master" %>


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
            $('#divNavx').html('รายการโอนสิทธิ์ผู้รับผิดชอบ');
            $(".js-example-basic-single").select2({ width: '100%' });
            $('.js-example-basic-single-ws').select2({ minimumResultsForSearch: Infinity, width: '100%' });

            $('#accordion-style').on('click', function (ev) {
                var target = $('input', ev.target);
                var which = parseInt(target.val());
                if (which == 2) $('#accordion').addClass('accordion-style2');
                else $('#accordion').removeClass('accordion-style2');
            });

            setDatePicker();
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
            $("#<%= txtDateBeg.ClientID %>").datepicker({ dateFormat: "dd/mm/yy" });
            $("#<%= txtDateEnd.ClientID %>").datepicker({ dateFormat: "dd/mm/yy" });
        }

        //function setTextNumericOnly() {
        //    jQuery('.numericOnly').keyup(function () {
        //        this.value = this.value.replace(/[^0-9\.]/g, '');
        //    });
        //}

    </script>

    <!-- Add fancyBox -->
    <link rel="stylesheet" href="/fancybox/source/jquery.fancybox.css?v=2.1.5" type="text/css" media="screen" />
    <script type="text/javascript" src="/fancybox/source/jquery.fancybox.pack.js?v=2.1.5"></script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderId="ContentPlaceHolder1" runat="server">
    <!-- #section:elements.accordion -->
    <div id="accordion" class="accordion-style1 panel-group">
        <div class="panel panel-default">
            <div class="panel-heading">
                <h4 class="panel-title">
                    <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion" href="#collapseOne">
                        <i class="ace-icon fa fa-angle-down bigger-110" data-icon-hide="ace-icon fa fa-angle-down" data-icon-show="ace-icon fa fa-angle-right"></i>
                        &nbsp;แสดงรายการโอนสิทธิ์ผู้รับผิดชอบ
                    </a>
                </h4>
            </div>

            <div class="panel-collapse collapse in" id="collapseOne">
                <div class="panel-body">
                    <div class="col-sm-12">
                        <div class="col-sm-2 div-caption">ผู้รับผิดชอบเดิม</div>
                        <div class="col-sm-3">
                            <asp:DropDownList ID="ddlCurResp" runat="server" class="col-sm-12 js-example-basic-single js-states form-control hide"></asp:DropDownList>
                            <!--<asp:TextBox ID="txtOldResponsibleUser" runat="server" class="col-sm-12" placeholder="ผู้รับผิดชอบเดิม"></asp:TextBox>-->
                        </div>
                        <div class="col-sm-2 div-caption">ผู้รับผิดชอบใหม่</div>
                        <div class="col-sm-3">
                            <asp:DropDownList ID="ddlNewResp" runat="server" class="col-sm-12 js-example-basic-single js-states form-control hide"></asp:DropDownList>
                            <!--<asp:TextBox ID="txtNewResponsibleUser" runat="server" class="col-sm-12" placeholder="ผู้รับผิดชอบใหม่"></asp:TextBox>-->
                        </div>
                        <div class="col-sm-2 div-caption"></div>                        
                    </div>
                    <div class="col-sm-12">
                        <div class="col-sm-2 div-caption">โครงการ</div>
                        <div class="col-sm-3">
                            <asp:DropDownList ID="ddlProject" runat="server" class="col-sm-12 js-example-basic-single js-states form-control hide"></asp:DropDownList>
                        </div>
                        <div class="col-sm-2 div-caption">ช่วงวันที่</div>
                        <div class="col-sm-3">
                            <asp:TextBox ID="txtDateBeg" runat="server" class="col-sm-5" placeholder="วันที่เริ่มต้น"></asp:TextBox>
                            <div class="col-sm-2" style="text-align:center;">-</div>
                            <asp:TextBox ID="txtDateEnd" runat="server" class="col-sm-5" placeholder="วันที่สิ้นสุด"></asp:TextBox>
                        </div>
                        <div class="col-sm-2 div-caption"></div>                        
                    </div>
                    <div class="col-sm-12">
                        <div class="col-sm-5" style="text-align:center;"></div>
                        <div class="col-sm-5">
                            <asp:Button ID="btnSearch" runat="server" Text="Search" class="btn btn-white btn-info btn-sm" Width="75px" OnClick="btnSearch_Click" />
                            &nbsp;
                            <asp:Button ID="btnClear" runat="server" Text="Clear" class="btn btn-white btn-info btn-sm" Width="75px" OnClick="btnClear_Click" />
                        </div> 
                        <div class="col-sm-2 div-caption"></div> 
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- #accordion -->

    <div class="col-sm-12">     
        <asp:Button ID="btnChangeResponsible" runat="server" Text="+ โอนสิทธิ์ผู้รับผิดชอบ" class="btn btn-white btn-info btn-sm" OnClick="btnChangeResponsible_Click" />
    </div>
    <div class="col-sm-12" >
        <asp:GridView ID="grdData" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" class="col-sm-12" 
            EmptyDataText="No data." ShowHeaderWhenEmpty="True" OnRowCommand="grdData_RowCommand" >
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:ImageButton ID="imgEdit" runat="server" ImageUrl="~/img/writing_file.png" Width="23px" Style="vertical-align: baseline;" CommandName="viewChangeResp" CommandArgument='<%# Eval("CRListId") %>' />
                        <asp:HiddenField ID="hdfCRListId" runat="server" Value='<%# Eval("CRListId") %>' />
                        <asp:HiddenField ID="hdfProject_Id" runat="server" Value='<%# Eval("Project_Id") %>' />
                        <asp:HiddenField ID="hdfCRFrom" runat="server" Value='<%# Eval("CRFrom") %>' />
                        <asp:HiddenField ID="hdfCRTo" runat="server" Value='<%# Eval("CRTo") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ProjectName" HeaderText="โครงการ" HeaderStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="UserFrom" HeaderText="ผู้รับผิดชอบเดิม" HeaderStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="UserTo" HeaderText="ผู้รับผิดชอบใหม่" HeaderStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="DocRefNo" HeaderText="เอกสารอ้างอิง" HeaderStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="DocDate" HeaderText="วันที่เอกสาร" HeaderStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:ImageButton ID="imgFile" runat="server" ToolTip='<%# Eval("FileAttch") %>' ImageUrl="~/img/download.png" Width="23px" Style="vertical-align: baseline;" CommandName="getFile" CommandArgument='<%# Eval("FileAttchName") %>' />                                
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
    <div class="col-sm-12" style="text-align: right;">&nbsp;</div>


</asp:Content>