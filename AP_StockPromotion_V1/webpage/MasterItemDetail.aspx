<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MasterItemDetail.aspx.cs"
    Inherits="AP_StockPromotion_V1.webpage.MasterItemDetail" MasterPageFile="~/Master/MasterPopup.Master" %>

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

        function bindDataParentPage(mino) {
            parent.$("#ContentPlaceHolder1_txtItemNo").val(mino)
            parent.$("#ContentPlaceHolder1_btnSearch").click();
            closePage();
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdfMasterItemId" runat="server" />
    <div class="col-sm-12">
        <div class="col-sm-12">
            <div class="col-sm-2 div-caption">
                <asp:Label ID="lbItemNo" runat="server" Text="รหัสสินค้าโปรโมชั่น :" class="col-sm-12 label-caption"></asp:Label>
            </div>
            <div class="col-sm-3">
                <asp:TextBox ID="txtItemNo" runat="server" placeholder="รหัสสินค้าโปรโมชั่น" class="col-sm-12"></asp:TextBox>
            </div>
            <div class="col-sm-2 div-caption">
                <asp:Label ID="lbItemName" runat="server" Text="ชื่อ :" class="col-sm-12 label-caption"></asp:Label>
            </div>
            <div class="col-sm-3">
                <asp:TextBox ID="txtItemName" runat="server" placeholder="ชื่อสินค้าโปรโมชั่น" class="col-sm-12"></asp:TextBox>
            </div>
            <div class="col-sm-2 div-caption">
                &nbsp;
            </div>
        </div>
        <div class="col-sm-12">
            <div class="col-sm-2 div-caption">
                <asp:Label ID="lbItemCostBeg" runat="server" Text="มูลค่า :" class="col-sm-12 label-caption"></asp:Label>
            </div>
            <div class="col-sm-3">
                <asp:TextBox ID="txtItemCost" runat="server" placeholder="0.00" class="col-sm-12 label-caption"></asp:TextBox>
            </div>
            <div class="col-sm-2 div-caption">
                <asp:Label ID="lbItemCostBegIncVat" runat="server" Text="มูลค่ารวม Vat :" class="col-sm-12 label-caption"></asp:Label>
            </div>
            <div class="col-sm-3">
                <asp:TextBox ID="txtItemCostIncVat" runat="server" placeholder="0.00" class="col-sm-12 label-caption"></asp:TextBox>
            </div>
            <div class="col-sm-2 div-caption">
                &nbsp;
            </div>
        </div>
        <div class="col-sm-12">
            <div class="col-sm-2 div-caption">
                <asp:Label ID="lbGroup" runat="server" Text="หมวดสินค้า :" class="col-sm-12 label-caption"></asp:Label>
            </div>
            <div class="col-sm-3">
                <asp:DropDownList ID="ddlItemGroup" runat="server" class="col-sm-12 js-example-basic-single js-states form-control hide"></asp:DropDownList>
            </div>
            <div class="col-sm-2 div-caption">
                <asp:Label ID="lbStatus" runat="server" Text="สถานะ :" class="col-sm-12 label-caption"></asp:Label>
            </div>
            <div class="col-sm-3">
                <asp:DropDownList ID="ddlItemStatus" runat="server" class="col-sm-12 js-example-basic-single-ws js-states form-control hide"></asp:DropDownList>
            </div>
            <div class="col-sm-2 div-caption">&nbsp;</div>
        </div>
        <div class="col-sm-12">
            <div class="col-sm-2 div-caption">
                <asp:Label ID="lb" runat="server" Text="วิธีการตรวจนับ :" class="col-sm-12 label-caption"></asp:Label>
            </div>
            <div class="col-sm-3">
                <asp:DropDownList ID="ddlItemCountMethod" runat="server" class="col-sm-12 js-example-basic-single-ws js-states form-control hide"></asp:DropDownList>
            </div>
            <div class="col-sm-2 div-caption">
                <%--<asp:Label ID="Label3" runat="server" Text="สต็อกกลาง :" class="col-sm-12 label-caption"></asp:Label>--%>
            </div>
            <div class="col-sm-3">
                <%--<asp:DropDownList ID="ddlItemStock" runat="server" class="col-sm-12 js-example-basic-single-ws js-states form-control hide" ></asp:DropDownList>--%>
                <asp:CheckBox ID="chkForceExpire" runat="server" Text="&nbsp;ระบุวันหมดอายุ (Expire Date)" ForeColor="Blue" />
            </div>
            <div class="col-sm-2 div-caption">&nbsp;</div>
        </div>
        <div class="col-sm-12">
            <div class="col-sm-2 div-caption"></div>
            <div class="col-sm-3"></div>
            <div class="col-sm-2 div-caption"></div>
            <div class="col-sm-3"></div>
            <div class="col-sm-2 div-caption">&nbsp;</div>
        </div>
        <div class="col-sm-12" style="text-align: center;">&nbsp;</div>
        <div class="col-sm-12" style="text-align: center;">
            <asp:Button ID="btnSave" runat="server" Text="บันทึก" class="btn btn-white btn-info btn-sm" Width="75px" OnClick="btnSave_Click" />
            &nbsp;
            <asp:Button ID="btnClose" runat="server" Text="ยกเลิก" class="btn btn-white btn-info btn-sm" Width="75px" OnClick="btnClose_Click" />
        </div>
        <div class="col-sm-12" style="text-align: right;">
            <asp:Label ID="lbWarinigNoSave" runat="server" Text="** ไม่สามารถแก้ไขได้ เนื่องจากมีสินค้าโปรโมชั่นอยู่ระหว่างดำเนินการ" ForeColor="LightSalmon" Style="display: none;"></asp:Label>
        </div>
    </div>

</asp:Content>
