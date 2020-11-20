<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangeProjectResponsibleRequest.aspx.cs"
    Inherits="AP_StockPromotion_V1.webpage.ChangeProjectResponsibleRequest" MasterPageFile="~/Master/MasterPopup.Master" %>

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
            closePage();
        }        

        function setDatePicker() {
            $("#<%= txtReqDocDate.ClientID %>").datepicker({ dateFormat: "dd/mm/yy" });
            //$("# txtRequestDate.ClientID ").datepicker({ dateFormat: "dd/mm/yy" });
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


    <div class="col-sm-12">
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
                    <label style="color: red">*</label>
                </div>
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
    <div class="col-sm-12 div-caption">
        <asp:Button ID="btnSave" runat="server" Text="บันทึก" class="btn btn-white btn-info btn-sm" Font-Bold="true" />
    </div>

</asp:Content>
