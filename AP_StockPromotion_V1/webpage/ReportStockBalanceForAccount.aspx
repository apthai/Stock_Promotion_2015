﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportStockBalanceForAccount.aspx.cs" 
    Inherits="AP_StockPromotion_V1.webpage.ReportStockBalanceForAccount"  MasterPageFile="~/Master/MasterPage.Master"  %>

<asp:Content ID="Contenthead" ContentPlaceHolderId="head" runat="server">
    <style type="text/css">
        .label-caption {
            text-align:right;
        }
        .div-caption {
            text-align:right;
            padding-right:5px;
        }
        .div-caption-c {
            text-align:center;
            padding-right:5px;
        }
    </style>
    <script type="text/javascript">
        jQuery(function ($) {
            $('#divNavx').html('รายงาน >> รายงานสินค้าโปรโมชั่นคงเหลือสำหรับตรวจนับ');
            $(".js-example-basic-single").select2({ width: '100%' });
            $('.js-example-basic-single-ws').select2({ minimumResultsForSearch: Infinity, width: '100%' });
            $('.widget-body').height(screen.height - 200);
            setDateText();
        });

        function setDateText() {
            $("#<%= txtDateBeg.ClientID %>").datepicker({ dateFormat: "dd/mm/yy" });
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

<asp:Content ID="Content2" ContentPlaceHolderId="ContentPlaceHolder1" runat="server">
    
    <div class="col-sm-12">
        <div class="widget-box">
            <div class="widget-header">
                <h4 class="smaller">&nbsp;</h4>
            </div>

            <div class="widget-body">
                <div class="widget-main">
                    <div class="col-sm-12">
                        <div class="col-sm-2 div-caption">เงื่อนไข-วันที่</div>
                        <div class="col-sm-3">
                            <asp:TextBox ID="txtDateBeg" runat="server" class="col-sm-5"></asp:TextBox>
                        </div>
                        <div class="col-sm-7">
                        </div>
                        <div class="col-sm-5"></div>
                    </div>
                    <div class="col-sm-12">
                        <div class="col-sm-2 div-caption">เงื่อนไข-บริษัท</div>
                        <div class="col-sm-4">
                            <asp:CheckBox ID="chkAllCompany" runat="server" Text="ทั้งหมด" AutoPostBack="true" OnCheckedChanged="chkAllCompany_CheckedChanged"></asp:CheckBox>
                        </div>
                        <div class="col-sm-6">
                            <asp:CheckBox ID="chkNoneProject" runat="server" Text="ไม่ระบุโครงการ" ></asp:CheckBox>
                        </div>
                    </div>
                    <div class="col-sm-12">
                        <div class="col-sm-2 div-caption"></div>
                        <div class="col-sm-10">
                            <asp:CheckBoxList ID="chkCompany" runat="server" RepeatColumns="3" class="col-sm-12"></asp:CheckBoxList>
                        <div class="col-sm-4"></div>
                        </div>
                    </div>
                    <div class="col-sm-12">
                        <div class="col-sm-2 div-caption">เงื่อนไข-หมวดสินค้า</div>
                        <div class="col-sm-10">
                            <asp:CheckBox ID="chkAllMatType" runat="server" Text="ทั้งหมด" AutoPostBack="true" OnCheckedChanged="chkAllMatType_CheckedChanged"></asp:CheckBox>
                        </div>
                    </div>
                    <div class="col-sm-12">
                        <div class="col-sm-2 div-caption"></div>
                        <div class="col-sm-10">
                            <asp:CheckBoxList ID="chkMatType" runat="server" RepeatColumns="4" class="col-sm-12"></asp:CheckBoxList>
                        <div class="col-sm-4"></div>
                        </div>
                    </div>
                    <div class="col-sm-12">
                        <div class="col-sm-12 div-caption">
                            <asp:Button id="btnExportReport" runat="server" class="btn btn-white btn-info btn-sm" Text="แสดงรายงาน" OnClick="btnExportReport_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
</asp:Content>
