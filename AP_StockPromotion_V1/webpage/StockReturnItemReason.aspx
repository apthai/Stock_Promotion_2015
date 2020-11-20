<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockReturnItemReason.aspx.cs"
    Inherits="AP_StockPromotion_V1.webpage.StockReturnItemReason" MasterPageFile="~/Master/MasterPopup.Master" %>

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
            $('#divNavx').html('คืนสินค้าโปรโมชั่น >> เหตุผล');
            //$(".js-example-basic-single").select2({ width: '100%' });
            //$('.js-example-basic-single-ws').select2({ minimumResultsForSearch: Infinity, width: '100%' });
        });

        function closePage() {
            parent.jQuery.colorbox.close();
            // window.close();
        }

        function bindDataParentPage(gname) {
            //parent.$('#ContentPlaceHolder1_txtGroupName').val(gname)
            //parent.$("#ContentPlaceHolder1_btnSearch").click();
            closePage();
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdfItemLst" runat="server" />
    <asp:HiddenField ID="hdfProject_id" runat="server" />
    <div class="col-sm-12">
        <div class="col-sm-12">
            <h2>เหตุผลการส่งคืน</h2>
            <div class="col-sm-12">
                <asp:TextBox ID="txtReason" runat="server" placeholder="เหตุผลในการคืนสินค้าโปรโมชั่น" class="col-sm-12" TextMode="MultiLine" Height="125px"></asp:TextBox>
            </div>
            <div class="col-sm-12">&nbsp;</div>
            <div class="col-sm-12 caption" style="text-align:right;">
                <asp:Button ID="btnSave" runat="server" Text="บันทึก" class="btn btn-white btn-info btn-sm" Width="75px" OnClick="btnSave_Click" />
                &nbsp;
                <asp:Button ID="btnClose" runat="server" Text="ยกเลิก" class="btn btn-white btn-info btn-sm" Width="75px" OnClick="btnClose_Click" />
            </div>
        </div>
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
