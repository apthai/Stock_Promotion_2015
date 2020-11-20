﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeliveryPostSum.aspx.cs" 
    Inherits="AP_StockPromotion_V1.webpage.DeliveryPostSum" MasterPageFile="~/Master/MasterPopup.Master" %>

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
            $('#divNavx').html('XXXXXXXXXXXXXXXXXXXX');
            $(".js-example-basic-single").select2({ width: '100%' });
            $('.js-example-basic-single-ws').select2({ minimumResultsForSearch: Infinity, width: '100%' });

            setDatePicker();
          
        });     

        function closePage() {
            parent.jQuery.colorbox.close();
        }

        function bindDataParentPage() {
            parent.$("#ContentPlaceHolder1_btnSearch").click();
            closePage(); 
        }

        function setDatePicker() {
            try{
                $('#<%= txtDate.ClientID %>').datepicker({ dateFormat: "dd/mm/yy" }); // { dateFormat: "dd-mm-yy" }
            } catch (e) { alert(e.toLocaleString()) }
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

        function PopupFullScr(url) {
            if (mypopUp != undefined) {
                mypopUp.close();
            }
            var scr_H = screen.height;
            var scr_W = screen.width;
            var h = (scr_H);
            var w = (scr_W);
            var t = 0;
            var l = 0;
            myWindow = window.open(url, '', 'height=' + h + ',width=' + w + ',top=' + t + ',left=' + l + ',status=yes,scrollbars=no,resizable=yes,menubar=no;toolbar=no,titlebar=no');
        }
       
        

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <asp:HiddenField ID="hdfCompanySAPCode" runat="server" />
    <asp:HiddenField ID="hdfProfitHO" runat="server" />

    

    
    <asp:Button ID="BtnReversePR" runat="server" Text="ถอย Delete PR" OnClick="BtnReversePR_Click"  Visible="false"/>

    

    
    <div class="col-sm-12" style="font-weight:bold; font-size:1.12em;">
        <div class="col-sm-4 div">
            <div class="col-sm-5 div-caption">Posting Date</div>
            <div class="col-sm-7">
                <asp:TextBox ID="txtDate" runat="server" class="col-sm-12" placeholder="dd/MM/yyyy"></asp:TextBox>                 
            </div>
        </div>
        <div class="col-sm-4 div">
            <div class="col-sm-5 div-caption">Reference</div>
            <div class="col-sm-7">
                <asp:TextBox ID="txtReference" runat="server" class="col-sm-12" MaxLength="16"></asp:TextBox>
            </div>
        </div>
        <div class="col-sm-4 div">
            <div class="col-sm-5 div-caption">Ref Key 3</div>
            <div class="col-sm-7">
                <asp:TextBox ID="txtRefKey3" runat="server" class="col-sm-12" MaxLength="16"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="col-sm-12">&nbsp;</div>

    <div class="col-sm-12">

        <!-- #section:elements.accordion -->
        <div id="accordion" class="accordion-style1 panel-group">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        <a class="accordion-toggle" data-toggle="collapse" href="#collapseOne"><!-- data-parent="#accordion"-->
                            <i class="ace-icon fa fa-angle-down bigger-110" data-icon-hide="ace-icon fa fa-angle-down" data-icon-show="ace-icon fa fa-angle-right"></i>
                            &nbsp;สรุปรายละเอียดตามค่าใช้จ่าย WBS
                        </a>
                    </h4>
                </div>
            
                <div class="panel-collapse collapse in" id="collapseOne">
                    <div class="panel-body">
                        <div class="col-sm-12">
                        
                            <div class="col-sm-12">
                                <asp:GridView ID="grdData" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                    ForeColor="#333333" GridLines="None" class="col-sm-12">
                                    <AlternatingRowStyle BackColor="White" />
                                    <Columns>
                                        <asp:BoundField DataField="ItemNo" HeaderText="เลขที่สินค้าโปรโมชั่น" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="141px" />
                                        <asp:BoundField DataField="ItemName" HeaderText="สินค้าโปรโมชั่น" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="Amount" HeaderText="จำนวน" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="123px" />
                                        <asp:BoundField DataField="Total" HeaderText="มูลค่ารวม" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="123px" DataFormatString="{0:#,##0.00}" />
                                        <asp:BoundField DataField="CostCenter" HeaderText="Cost Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="123px" />
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

            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        <a class="accordion-toggle collapsed"  data-toggle="collapse" href="#collapseTwo"><!-- data-parent="#accordion"-->
                            <i class="ace-icon fa fa-angle-down bigger-110" data-icon-hide="ace-icon fa fa-angle-down" data-icon-show="ace-icon fa fa-angle-right"></i>
                            &nbsp;บันทึกบัญชีตัดสต๊อก (WBS)
                        </a>
                    </h4>
                </div>
            
                <div class="panel-collapse collapse in" id="collapseTwo">
                    <div class="panel-body">
                        <div class="col-sm-12">

                            <div class="col-sm-12">
                                <asp:GridView ID="grdDataSum" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                    ForeColor="#333333" GridLines="None" class="col-sm-12">
                                    <AlternatingRowStyle BackColor="White" />
                                    <Columns>
                                        <asp:BoundField DataField="GLNo" HeaderText="GL No." HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="GLName" HeaderText="GL Name" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="WBS_SAP" HeaderText="WBS" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="ProfitCenter" HeaderText="Profit Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="Total" HeaderText="Debit" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:#,##0.00}" />
                                        <asp:BoundField DataField="TotalCredit" HeaderText="Credit" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:#,##0.00}" />
                                        <asp:BoundField DataField="ItemText" HeaderText="Item Text" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
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
        </div>
        <!-- #accordion -->

    </div>
    
    <div class="col-sm-12">&nbsp;</div>
    <div class="col-sm-12 div-caption">
        <asp:Button ID="btnPostAccount" runat="server" Text="บันทึกบัญชี" class="btn btn-white btn-info btn-sm" Font-Bold="true" OnClick="btnPostAccount_Click" />
    </div>
        
</asp:Content>