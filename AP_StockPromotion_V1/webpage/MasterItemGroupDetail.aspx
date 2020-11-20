<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MasterItemGroupDetail.aspx.cs"
    Inherits="AP_StockPromotion_V1.webpage.MasterItemGroupDetail" MasterPageFile="~/Master/MasterPopup.Master" %>

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
            $('#divNavx').html('Master Data >> สินค้าโปรโมชั่น');
            $(".js-example-basic-single").select2({ width: '100%' });
            $('.js-example-basic-single-ws').select2({ minimumResultsForSearch: Infinity, width: '100%' });
        });

        function closePage() {
            parent.jQuery.colorbox.close();
            // window.close();
        }

        function bindDataParentPage(gname) {
            parent.$('#ContentPlaceHolder1_txtGroupName').val(gname)
            parent.$("#ContentPlaceHolder1_btnSearch").click();
            closePage();
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdfMasterItemGroupId" runat="server" />
    <div class="col-sm-12">
        <div class="col-sm-12">
            <div class="col-sm-2 div-caption">ชื่อหมวดสินค้า</div>
            <div class="col-sm-3">
                <asp:TextBox ID="txtMasterItemGroupName" runat="server" placeholder="ชื่อหมวดสินค้า" class="col-sm-12"></asp:TextBox>
            </div>
            <div class="col-sm-2 div-caption"></div>
            <div class="col-sm-3">
                <asp:Button ID="btnSave" runat="server" Text="บันทึก" class="btn btn-white btn-info btn-sm" Width="75px" OnClick="btnSave_Click" />
                &nbsp;
                <asp:Button ID="btnClose" runat="server" Text="ยกเลิก" class="btn btn-white btn-info btn-sm" Width="75px" OnClick="btnClose_Click" />
            </div>
            <div class="col-sm-2 div-caption"></div>
        </div>
        <%--<div class="col-sm-12">
            <div class="col-sm-2 div-caption">วิธีการตรวจนับ</div>
            <div class="col-sm-3">
                <asp:DropDownList ID="ddlItemCountMethod" runat="server" class="col-sm-12 js-example-basic-single-ws js-states form-control hide"></asp:DropDownList>
            </div>
            <div class="col-sm-2 div-caption">สต๊อกกลาง</div>
            <div class="col-sm-3">
                <asp:DropDownList ID="ddlItemStock" runat="server" class="col-sm-12 js-example-basic-single-ws js-states form-control hide"></asp:DropDownList>
            </div>
            <div class="col-sm-2 div-caption"></div>
        </div>--%>
        <div class="col-sm-12">&nbsp;</div>
        <div class="col-sm-12">
            <div class="col-sm-2 div-caption"></div>
            <div class="col-sm-3"></div>
            <div class="col-sm-5">
                
            </div>
            <div class="col-sm-2 div-caption"></div>
        </div>
    </div>

</asp:Content>
