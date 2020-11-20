<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RequisitionDeleteRemark.aspx.cs"
    Inherits="AP_StockPromotion_V1.webpage.RequisitionDeleteRemark" MasterPageFile="~/Master/MasterPopup.Master" %>

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
            $('#divNavPop').html('หมายเหตุ');
            //$(".js-example-basic-single").select2({ width: '100%' });
            //$('.js-example-basic-single-ws').select2({ minimumResultsForSearch: Infinity, width: '100%' });
        });

        function delRequest(remark) {
            parent.$("#ContentPlaceHolder1_hdfReqDelRemark").val(remark);
            parent.$("#ContentPlaceHolder1_btnDelReq").click();
            window.close();
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdfReqId" runat="server" />
    
    <div class="col-sm-12" style="width:100%">
        <asp:TextBox ID="txtRemark" runat="server" Width="99%" Height="99px" TextMode="MultiLine"></asp:TextBox>
    </div>
    <div class="col-sm-12" style="width:100%">&nbsp;</div>
    <div class="col-sm-12 div-caption" style="width:100%">
        <asp:Button ID="btnOK" runat="server" class="btn btn-white btn-danger btn-sm" Text="ยืนยัน" Width="99px" OnClick="btnOK_Click" />
    </div>
</asp:Content>
