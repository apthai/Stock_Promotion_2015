<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockReceiveDetail.aspx.cs"
    Inherits="AP_StockPromotion_V1.web.StockReceiveDetail" MasterPageFile="~/Master/MasterPopup.Master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../plugin/ScrollableGrid.js"></script>
    <style type="text/css">
        .label-caption {
            text-align: right;
        }

        .div-caption {
            text-align: right;
            padding-right: 5px;
        }

        #element_to_pop_up {
            display: none;
            background-color: #fff;
            border-radius: 10px 10px 10px 10px;
            box-shadow: 0 0 25px 5px #999;
            color: #111;
            display: none;
            min-width: 450px;
            padding: 25px;
        }


        .button.b-close, .button.bClose {
            color:white;
            border-radius: 7px;
            box-shadow: none;
            font: bold 131% sans-serif;
            padding: 0 6px 2px;
            position: absolute;
            right: -7px;
            top: -7px;
            background-color: #2b91af;       
            cursor:pointer;     
        }
    </style>
    <script type="text/javascript">
        jQuery(function ($) {
            $('#divNavx').html('รับสินค้าโปรโมชั่น >> ตรวจสอบพร้อมจ่าย >> ตรวจสอบ');
            tabContentHeight();
            setDatePicker();
            switchTab();
            // initGridScroll();

            setTextDigitOnly();
            setTextNumericOnly();
            // initKeyPress();

        });

        $(window).resize(function () {
            tabContentHeight();
        });

        function initGridScroll() {
            $('#<%=grdData.ClientID %>').Scrollable({
                ScrollHeight: (($(window).height() / 100 * 70) - 100),
                IsInUpdatePanel: false
            });            
        }

        function tabContentHeight() {
            var scr_H = $(window).height();
            var h = (scr_H / 100 * 70);
            $('.tab-content').height(h);
        }


        function setDatePicker() {
            try { $("#<%= txtExpireDate.ClientID %>").datepicker({ dateFormat: "dd/mm/yy" }); } catch (e) { }
            try { $("#<%= txtProduceDate.ClientID %>").datepicker({ dateFormat: "dd/mm/yy" }); } catch (e) { }
            try { $("#<%= txtEditExpireDate.ClientID %>").datepicker({ dateFormat: "dd/mm/yy" }); } catch (e) { }
            try { $("#<%= txtEditProduceDate.ClientID %>").datepicker({ dateFormat: "dd/mm/yy" }); } catch (e) { }
        }


        function switchTab() {
            var tab = $("#<%= hdfTabActive.ClientID %>").val();
            if (tab == 'item') {
                switchTabItem();
            } else if (tab == 'itemDetail') {
                switchTabItemDetail();
            } else if (tab == 'itemCheck') {
                switchTabItemCheck();
            } else {
            }
            if ($('#<%= hdfShowPopupEdit.ClientID %>').val() != '') {
                setButtonEditItem();
                openPopupEdit();
            } else {
                var countMethod = $('#<%= hdfItemCountMethod.ClientID %>').val();
                if (countMethod == "1") { $('#<%= txtSerialNo.ClientID %>').focus(); }
                else if (countMethod == "2") { $('#<%= txtSeqStart.ClientID %>').focus(); }
                else if (countMethod == "3") { $('#<%= txtCountAmount.ClientID %>').focus(); }
            }
        }
        function switchTabItem() {
            $('#liItem').removeClass('active');
            $('#liItemDetail').removeClass('active');
            $('#liItemCheck').removeClass('active');
            $('#liItem').addClass('in active');

            $('#item').removeClass('active');
            $('#itemDetail').removeClass('active');
            $('#itemCheck').removeClass('active');
            $('#item').addClass('in active');

            $('#liItemDetail > a').attr("aria-expanded", "false");
            $('#liItemCheck > a').attr("aria-expanded", "false");
            $('#liItem > a').attr("aria-expanded", "true");
        }

        function switchTabItemDetail() {
            $('#liItem').removeClass('active');
            $('#liItemDetail').removeClass('active');
            $('#liItemCheck').removeClass('active');
            $('#liItemDetail').addClass('in active');

            $('#item').removeClass('active');
            $('#itemDetail').removeClass('active');
            $('#itemCheck').removeClass('active');
            $('#itemDetail').addClass('in active');

            $('#liItemDetail > a').attr("aria-expanded", "true");
            $('#liItemCheck > a').attr("aria-expanded", "false");
            $('#liItem > a').attr("aria-expanded", "false");
        }

        function switchTabItemCheck() {
            $('#liItem').removeClass('active');
            $('#liItemDetail').removeClass('active');
            $('#liItemCheck').removeClass('active');
            $('#liItemCheck').addClass('in active');

            $('#item').removeClass('active');
            $('#itemDetail').removeClass('active');
            $('#itemCheck').removeClass('active');
            $('#itemCheck').addClass('in active');

            $('#liItemDetail > a').attr("aria-expanded", "false");
            $('#liItem > a').attr("aria-expanded", "false");
            $('#liItemCheck > a').attr("aria-expanded", "true");
        }

        function closePage() {
            window.close();
        }

        function bindDataParentPage() {
            parent.$("#ContentPlaceHolder1_btnSearch").click();
            closePage();
        }

        function initKeyPress() {
            $('#<%= txtSerialNo.ClientID %>').keypress(function (e) {
                if (e.which == 13) {
                    $('#<%= imgChkSerial.ClientID %>').click();
                }
            });
            $('#<%= txtSeqStart.ClientID %>').keypress(function (e) {
                if (e.which == 13) {
                    $('#<%= txtSeqEnd.ClientID %>').focus();
                }
            });
            $('#<%= txtSeqEnd.ClientID %>').keypress(function (e) {
                if (e.which == 13) {
                    $('#<%= imgChkSeq.ClientID %>').click();
                }
            });
            $('#<%= txtCountAmount.ClientID %>').keypress(function (e) {
                if (e.which == 13) {
                    return false;
                } else {
                    var charCode = e.which;
                    if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                        return false;
                    } else {
                        return true;
                    }
                }
            });
        }


        function keyEnterSerialNo(e) {
            if (e.which == 13) {
                e.preventDefault();
                document.getElementById('<%= imgChkSerial.ClientID %>').click();
            }
        }

        function keyEnterSequenceStart(e) {
            if (e.which == 13) {
                document.getElementById('<%= txtSeqEnd.ClientID %>').focus();
            }
        }

        function keyEnterSequenceEnd(e) {
            if (e.which == 13) {
                document.getElementById('<%= imgChkSeq.ClientID %>').click();
            }
        }

        function keyEnterCountAmount(e) {
            if (e.which == 13) {
                return false; //document.getElementById('<%= imgChkAmount.ClientID %>').click();
            }
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

        function openPopupEdit() {
            $('#btnPopupEdit').click();
        }


        function setButtonEditItem() {
            $('#btnPopupEdit').bind('click', function (e) {
                e.preventDefault();

                // Triggering bPopup when click event is fired
                $('#element_to_pop_up').bPopup({
                    modalClose: false,
                    opacity: 0.6,
                    positionStyle: 'fixed' //'fixed' or 'absolute'
                });
            });
        }

        function clickSaveEditItem() {
            $('#<%= btnSaveEditItemHidden.ClientID %>').click();
        }


        function keyTextItemId() { $("#<%= hdfEditItemId.ClientID %>").val($("#<%= txtEditItemId.ClientID %>").val()); }
        function keyTextSerial() { $("#<%= hdfEditSerial.ClientID %>").val($("#<%= txtEditSerial.ClientID %>").val()); }
        function keyTextItemName() { $("#<%= hdfEditItemName.ClientID %>").val($("#<%= txtEditItemName.ClientID %>").val()); }
        function keyTextModelName() { $("#<%= hdfEditModelName.ClientID %>").val($("#<%= txtEditModelName.ClientID %>").val()); }
        function keyTextColor() { $("#<%= hdfEditColor.ClientID %>").val($("#<%= txtEditColor.ClientID %>").val()); }
        function keyTextDimensionWidth() { $("#<%= hdfEditDimensionWidth.ClientID %>").val($("#<%= txtEditDimensionWidth.ClientID %>").val()); }
        function keyTextDimensionLong() { $("#<%= hdfEditDimensionLong.ClientID %>").val($("#<%= txtEditDimensionLong.ClientID %>").val()); }
        function keyTextDimensionHeight() { $("#<%= hdfEditDimensionHeight.ClientID %>").val($("#<%= txtEditDimensionHeight.ClientID %>").val()); }
        function keyTextDimensionUnit() { $("#<%= hdfEditDimensionUnit.ClientID %>").val($("#<%= txtEditDimensionUnit.ClientID %>").val()); }
        function keyTextWeight() { $("#<%= hdfEditWeight.ClientID %>").val($("#<%= txtEditWeight.ClientID %>").val()); }
        function keyTextWeightUnit() { $("#<%= hdfEditWeightUnit.ClientID %>").val($("#<%= txtEditWeightUnit.ClientID %>").val()); }
        function keyTextPrice() { $("#<%= hdfEditPrice.ClientID %>").val($("#<%= txtEditPrice.ClientID %>").val()); }
        function keyTextProduceDate() { $("#<%= hdfEditProduceDate.ClientID %>").val($("#<%= txtEditProduceDate.ClientID %>").val()); }
        function keyTextExpireDate() { $("#<%= hdfEditExpireDate.ClientID %>").val($("#<%= txtEditExpireDate.ClientID %>").val()); }
        function keyTextDetail() { $("#<%= hdfEditDetail.ClientID %>").val($("#<%= txtEditDetail.ClientID %>").val()); }

    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">    
    <asp:Button ID="btnDummy" runat="server" Text="" Width="0px" Height="0px" Style="opacity: 0.0;" />
    <asp:HiddenField ID="hdfTabActive" runat="server" Value="item" />
    <asp:HiddenField ID="hdfReqId" runat="server" Value="" />
    <asp:HiddenField ID="hdfShowPopupEdit" runat="server" Value="" />
    <asp:HiddenField ID="hdfItemCountMethod" runat="server" Value="" />
    <asp:HiddenField ID="hdfAlertText" runat="server" />

    <div class="tabbable">
        <ul class="nav nav-tabs" id="Ul1">
            <li id="liItem" class="active">
                <a data-toggle="tab" href="#item" aria-expanded="true">
                    <i class="green ace-icon fa fa-folder-open-o bigger-120"></i>
                    รายละเอียดการรับสินค้า
                </a>
            </li>

            <li id="liItemDetail" class="">
                <a data-toggle="tab" href="#itemDetail" aria-expanded="true">
                    <i class="green ace-icon glyphicon glyphicon-list bigger-120"></i>
                    รายละเอียดสินค้า
                </a>
            </li>

            <li id="liItemCheck" class="" onclick="document.getElementById('<%= btnX.ClientID %>').click();">
                <a data-toggle="tab" href="#itemCheck" aria-expanded="true">
                    <i class="green ace-icon glyphicon glyphicon-list bigger-120"></i>
                    ระบุ จำนวน/Serial
                </a>
            </li>
        </ul>

        <div class="tab-content">
            <!-- สินค้าโปรโมชั่น -->
            <div id="item" class="tab-pane fade active in">
                <div class="col-sm-12">
                    <div class="col-sm-2 div-caption">PO No</div>
                    <div class="col-sm-3">
                        <asp:TextBox ID="txtPONo" runat="server" ReadOnly="true" placeholder="PO:XXXXXXXXXXXXXXXX" class="col-sm-12"></asp:TextBox>
                    </div>
                    <div class="col-sm-2 div-caption">GR No</div>
                    <div class="col-sm-3">
                        <asp:TextBox ID="txtGRNo" runat="server" ReadOnly="true" placeholder="GR:XXXXXXXXXXXXXXXX" class="col-sm-12"></asp:TextBox>
                    </div>
                    <div class="col-sm-2 div-caption"></div>
                </div>
                <div class="col-sm-12">&nbsp;</div>
                <div class="col-sm-12">
                    <div class="col-sm-2 div-caption">Vendor</div>
                    <div class="col-sm-3">
                        <asp:TextBox ID="txtVendor" runat="server" ReadOnly="true" placeholder="บริษัทผู้ค้า" class="col-sm-12"></asp:TextBox>
                    </div>
                    <div class="col-sm-2 div-caption">ชื่อสินค้า</div>
                    <div class="col-sm-3">
                        <asp:TextBox ID="txtItem" runat="server" ReadOnly="true" placeholder="สินค้าโปรโมชั่น" class="col-sm-12"></asp:TextBox>
                    </div>
                    <div class="col-sm-2 div-caption"></div>
                </div>
                <div class="col-sm-12">&nbsp;</div>
                <div class="col-sm-12">
                    <div class="col-sm-2 div-caption">จำนวน</div>
                    <div class="col-sm-3">
                        <asp:TextBox ID="txtAmount" runat="server" ReadOnly="true" placeholder="0" class="col-sm-6 label-caption"></asp:TextBox>
                        <asp:TextBox ID="txtItemUnit" runat="server" ReadOnly="true" placeholder="หน่วย" class="col-sm-6 label-caption"></asp:TextBox>
                    </div>
                    <div class="col-sm-2 div-caption">มูลค่าต่อหน่วย</div>
                    <div class="col-sm-3">
                        <asp:TextBox ID="txtPricePerUnit" runat="server" ReadOnly="true" placeholder="มูลค่า" class="col-sm-12"></asp:TextBox>
                    </div>
                    <div class="col-sm-2 div-caption"></div>
                </div>
                <div class="col-sm-12">&nbsp;</div>
                <div class="col-sm-12">
                    <div class="col-sm-2 div-caption">วิธีการตรวจนับ</div>
                    <div class="col-sm-3">
                        <asp:TextBox ID="txtCountMethod" runat="server" ReadOnly="true" placeholder="วิธีการตรวจนับ" class="col-sm-12"></asp:TextBox>
                    </div>
                    <div class="col-sm-2 div-caption">สถานะ</div>
                    <div class="col-sm-3">
                        <asp:TextBox ID="txtReceiveStatus" runat="server" ReadOnly="true" placeholder="สถานะ" class="col-sm-12"></asp:TextBox>
                    </div>
                    <div class="col-sm-2 div-caption">
                        <%--<asp:HiddenField ID="hdfH1" runat="server" />
                        <asp:HiddenField ID="hdfReceiveHeaderID" runat="server" />
                        <asp:HiddenField ID="hdfPO_No" runat="server" />
                        <asp:HiddenField ID="hdfGR_No" runat="server" />
                        <asp:HiddenField ID="hdfGR_Year" runat="server" />
                        <asp:HiddenField ID="hdfGRCancel_No" runat="server" />
                        <asp:HiddenField ID="hdfGRCancel_Year" runat="server" />
                        <asp:HiddenField ID="hdfVendor" runat="server" />
                        <asp:HiddenField ID="hdfCreateBy" runat="server" />
                        <asp:HiddenField ID="hdfCreateDate" runat="server" />
                        <asp:HiddenField ID="hdfReceiveHeaderStatus" runat="server" />
                        <asp:HiddenField ID="hdfSAP_EBELN" runat="server" />--%>
                        <%--<asp:HiddenField ID="hdfH2" runat="server" />
                        <asp:HiddenField ID="hdfReceiveDetailId" runat="server" />
                        <asp:HiddenField ID="hdfItemNo" runat="server" />
                        <asp:HiddenField ID="hdfPricePerUnit" runat="server" />
                        <asp:HiddenField ID="hdfReceiveAmount" runat="server" />
                        <asp:HiddenField ID="hdfStatus" runat="server" />
                        <asp:HiddenField ID="hdfSAP_EBELP" runat="server" />
                        <asp:HiddenField ID="hdfSAP_BSART" runat="server" />
                        <asp:HiddenField ID="hdfSAP_BUKRS" runat="server" />
                        <asp:HiddenField ID="hdfSAP_WERKS" runat="server" />
                        <asp:HiddenField ID="hdfSAP_MATNR" runat="server" />
                        <asp:HiddenField ID="hdfSAP_TXZ01" runat="server" />
                        <asp:HiddenField ID="hdfSAP_MENGE_X" runat="server" />
                        <asp:HiddenField ID="hdfSAP_MENGE_A" runat="server" />
                        <asp:HiddenField ID="hdfSAP_MEINS" runat="server" />
                        <asp:HiddenField ID="hdfSAP_NETPR" runat="server" />
                        <asp:HiddenField ID="hdfSAP_NETWR" runat="server" />
                        <asp:HiddenField ID="hdfSAP_NAVNW" runat="server" />
                        <asp:HiddenField ID="hdfSAP_EFFWR" runat="server" />
                        <asp:HiddenField ID="hdfSAP_WAERS" runat="server" />
                        <asp:HiddenField ID="hdfSAP_BANFN" runat="server" />
                        <asp:HiddenField ID="hdfSAP_BNFPO" runat="server" />
                        <asp:HiddenField ID="hdfSAP_KOSTL" runat="server" />
                        <asp:HiddenField ID="hdfSAP_NPLNR" runat="server" />
                        <asp:HiddenField ID="hdfSAP_PS_PSP_PNR" runat="server" />
                        <asp:HiddenField ID="hdfSAP_WBS_SHOW" runat="server" />--%>
                        <%--<asp:HiddenField ID="hdfH3" runat="server" />
                        <asp:HiddenField ID="hdfItemId" runat="server" />
                        <asp:HiddenField ID="hdfItemStatus" runat="server" />
                        <asp:HiddenField ID="hdfSerial" runat="server" />
                        <asp:HiddenField ID="hdfBarcode" runat="server" />
                        <asp:HiddenField ID="hdfItemName" runat="server" />
                        <asp:HiddenField ID="hdfModel" runat="server" />
                        <asp:HiddenField ID="hdfColor" runat="server" />
                        <asp:HiddenField ID="hdfDimensionWidth" runat="server" />
                        <asp:HiddenField ID="hdfDimensionLong" runat="server" />
                        <asp:HiddenField ID="hdfDimensionHeight" runat="server" />
                        <asp:HiddenField ID="hdfDimensionUnit" runat="server" />
                        <asp:HiddenField ID="hdfWeight" runat="server" />
                        <asp:HiddenField ID="hdfWeightUnit" runat="server" />
                        <asp:HiddenField ID="hdfPrice" runat="server" />
                        <asp:HiddenField ID="hdfProduceDate" runat="server" />
                        <asp:HiddenField ID="hdfExpireDate" runat="server" />
                        <asp:HiddenField ID="hdfDetail" runat="server" />
                        <asp:HiddenField ID="hdfRemark" runat="server" />--%>
                        <%--<asp:HiddenField ID="hdfH4" runat="server" />
                        <asp:HiddenField ID="hdfMasterItemId" runat="server" />
                        <asp:HiddenField ID="hdfItemNo1" runat="server" />
                        <asp:HiddenField ID="hdfItemName1" runat="server" />
                        <asp:HiddenField ID="hdfItemUnit" runat="server" />
                        <asp:HiddenField ID="hdfItemPricePerUnit" runat="server" />
                        <asp:HiddenField ID="hdfItemBasePricePerUnit" runat="server" />
                        <asp:HiddenField ID="hdfItemCountMethod" runat="server" />
                        <asp:HiddenField ID="hdfItemStock" runat="server" />--%>
                    </div>
                </div>
                <div class="col-sm-12">&nbsp;</div>
                <div class="col-sm-12">
                    <div class="col-sm-2 div-caption">รับสินค้าโดย</div>
                    <div class="col-sm-3">
                        <asp:HiddenField ID="hdfReceiveBy" runat="server" />
                        <asp:TextBox ID="txtReceiveBy" runat="server" ReadOnly="true" placeholder="รับสินค้าโดย" class="col-sm-12"></asp:TextBox>
                    </div>
                    <div class="col-sm-2 div-caption">วันที่รับ</div>
                    <div class="col-sm-3">
                        <asp:TextBox ID="txtReceiveDate" runat="server" ReadOnly="true" placeholder="วันที่รับ" class="col-sm-12"></asp:TextBox>
                    </div>
                    <div class="col-sm-2 div-caption"></div>
                </div>
                <div class="col-sm-12">&nbsp;</div>
            </div>

            <!-- รายละเอียดสินค้าโปรโมชั่น -->
            <div id="itemDetail" class="tab-pane fade">
                <div class="col-sm-12">
                    <div class="col-sm-2 div-caption">ชื่อสินค้า</div>
                    <div class="col-sm-3">
                        <asp:TextBox ID="txtItemName" runat="server" placeholder="ชื่อสินค้า" class="col-sm-12"></asp:TextBox>
                    </div>
                    <div class="col-sm-2 div-caption">ชื่อรุ่น</div>
                    <div class="col-sm-3">
                        <asp:TextBox ID="txtModelName" runat="server" placeholder="ชื่อรุ่น" class="col-sm-12"></asp:TextBox>
                    </div>
                    <div class="col-sm-2 div-caption"></div>
                </div>
                <div class="col-sm-12">&nbsp;</div>
                <div class="col-sm-12">
                    <div class="col-sm-2 div-caption">สี</div>
                    <div class="col-sm-3">
                        <asp:TextBox ID="txtColor" runat="server" placeholder="สี" class="col-sm-12"></asp:TextBox>
                    </div>
                    <div class="col-sm-2 div-caption">ขนาด</div>
                    <div class="col-sm-3">
                        <asp:TextBox ID="txtDimensionWidth" runat="server" placeholder="กว้าง" class="col-sm-3 digitOnly"></asp:TextBox>
                        <asp:TextBox ID="txtDimensionLong" runat="server" placeholder="ยาว" class="col-sm-3 digitOnly"></asp:TextBox>
                        <asp:TextBox ID="txtDimensionHeight" runat="server" placeholder="สูง" class="col-sm-3 digitOnly"></asp:TextBox>
                        <asp:TextBox ID="txtDimensionUnit" runat="server" placeholder="หน่วย" class="col-sm-3"></asp:TextBox>
                    </div>
                    <div class="col-sm-2 div-caption"></div>
                </div>
                <div class="col-sm-12">&nbsp;</div>

                <div class="col-sm-12">
                    <div class="col-sm-2 div-caption">น้ำหนัก</div>
                    <div class="col-sm-3">
                        <asp:TextBox ID="txtWeight" runat="server" placeholder="น้ำหนัก" class="col-sm-9 digitOnly"></asp:TextBox>
                        <asp:TextBox ID="txtWeightUnit" runat="server" placeholder="หน่วย" class="col-sm-3"></asp:TextBox>
                    </div>
                    <div class="col-sm-2 div-caption">มูลค่า</div>
                    <div class="col-sm-3">
                        <asp:TextBox ID="txtPrice" runat="server" placeholder="0.00" class="col-sm-12 label-caption digitOnly" ReadOnly="true"></asp:TextBox>
                    </div>
                    <div class="col-sm-2 div-caption"></div>
                </div>
                <div class="col-sm-12">&nbsp;</div>
                <div class="col-sm-12">
                    <div class="col-sm-2 div-caption">วันผลิต</div>
                    <div class="col-sm-3">
                        <asp:TextBox ID="txtProduceDate" runat="server" placeholder="dd/MM/yyyy" class="col-sm-12"></asp:TextBox>
                    </div>
                    <div class="col-sm-2 div-caption">วันหมดอายุ</div>
                    <div class="col-sm-3">
                        <asp:TextBox ID="txtExpireDate" runat="server" placeholder="dd/MM/yyyy" class="col-sm-11"></asp:TextBox>
                        <asp:Label ID="lbForceExpire" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                    <div class="col-sm-2 div-caption"></div>
                </div>
                <div class="col-sm-12">&nbsp;</div>
                <div class="col-sm-12">
                    <div class="col-sm-2 div-caption">รายละเอียด</div>
                    <div class="col-sm-8">
                        <asp:TextBox ID="txtDetail" runat="server" placeholder="รายละเอียด" class="col-sm-12" TextMode="MultiLine" Height="100px"></asp:TextBox>
                    </div>
                    <div class="col-sm-2 div-caption"></div>
                </div>
                <div class="col-sm-12">&nbsp;</div>
            </div>

            <!-- ตรวจสอบสินค้าโปรโมชั่น -->
            <div id="itemCheck" class="tab-pane fade">
                <div class="col-sm-12" id="divCountBySerial" runat="server">
                    <div class="col-sm-2 div-caption">Serial No.</div>
                    <div class="col-sm-3">
                        <asp:TextBox ID="txtSerialNo" runat="server" placeholder="Serial No." class="col-sm-12" onkeypress="keyEnterSerialNo(event);"></asp:TextBox>
                    </div>
                    <div class="col-sm-2">
                        <%--<asp:ImageButton ID="imgChkSerial" ImageUrl="~/img/write_document.png" runat="server" Width="23px" OnClick="imgChkSerial_Click" />--%>
                        <asp:LinkButton ID="imgChkSerial" runat="server" OnClick="imgChkSerial_Click" Text="ยืนยัน" ></asp:LinkButton>
                    </div>
                    <div class="col-sm-3">&nbsp;</div>
                    <div class="col-sm-2 div-caption">&nbsp;</div>
                </div>
                <div class="col-sm-12" id="divCountBySequence" runat="server">
                    <div class="col-sm-2 div-caption">Sequence No.</div>
                    <div class="col-sm-3">
                        <asp:TextBox ID="txtSeqStart" runat="server" placeholder="" class="col-sm-12" onkeypress="keyEnterSequenceStart(event);"></asp:TextBox>
                    </div>
                    <div class="col-sm-1" style="text-align: center;">&nbsp;-&nbsp;</div>
                    <div class="col-sm-3">
                        <asp:TextBox ID="txtSeqEnd" runat="server" placeholder="" class="col-sm-12" onkeypress="keyEnterSequenceEnd(event);"></asp:TextBox>
                    </div>
                    <div class="col-sm-3">
                        <asp:ImageButton ID="imgChkSeq" ImageUrl="~/img/write_document.png" runat="server" Width="23px" OnClick="imgChkSeq_Click" />
                    </div>
                </div>
                <div class="col-sm-12" id="divCountByAmount" runat="server">
                    <div class="col-sm-2 div-caption">จำนวน</div>
                    <div class="col-sm-3">
                        <asp:TextBox ID="txtCountAmount" runat="server" placeholder="0" class="col-sm-12 label-caption numericOnly" onkeypress="keyEnterCountAmount(event);"></asp:TextBox>
                    </div>
                    <div class="col-sm-2">
                        <%--<asp:ImageButton ID="imgChkAmount" ImageUrl="~/img/write_document.png" runat="server" Width="23px" OnClick="imgChkAmount_Click" />--%>
                        <asp:LinkButton ID="imgChkAmount" runat="server" Text="ยืนยัน" OnClick="imgChkAmount_Click" ></asp:LinkButton>
                    </div>
                    <div class="col-sm-3">&nbsp;</div>
                    <div class="col-sm-2 div-caption">&nbsp;</div>
                </div>
                <div class="col-sm-12">&nbsp;</div>
                <div class="col-sm-12">
                    <asp:GridView ID="grdData" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" class="col-sm-12"
                        EmptyDataText="No data." ShowHeaderWhenEmpty="True" OnRowCommand="grdData_RowCommand" OnRowDataBound="grdData_RowDataBound" AllowSorting="True" AllowPaging="true" OnPageIndexChanging="grdData_PageIndexChanging" OnSorting="grdData_Sorting" PageSize="8">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>                            
                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px" ItemStyle-Width="30px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgEdit" runat="server" ImageUrl="~/img/file_edit.png" Width="23px" Style="vertical-align: baseline;" CommandName="editChkItem" CommandArgument='<%# Eval("ItemId") %>' />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Serial" SortExpression="Serial" HeaderText="SerialNo" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="175px" />
                            <asp:BoundField DataField="ItemName" SortExpression="ItemName" HeaderText="สินค้าโปรโมชั่น" HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Model" SortExpression="Model" HeaderText="รุ่น" HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Color" SortExpression="Color" HeaderText="สี" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="ExpireDate" SortExpression="ExpireDate" HeaderText="วันหมดอายุ" HeaderStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-HorizontalAlign="Center" />
                            <asp:TemplateField HeaderText="หมายเหตุ" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="200px">
                                <ItemTemplate>
                                    <asp:TextBox ID="grdTxtRemark" runat="server" Text='<%# Eval("Remark") %>' placeholder="Remark" Width="200px"></asp:TextBox>
                                    <asp:HiddenField ID="grdHdfItemStatus" runat="server" Value='<%# Eval("ItemStatus") %>'></asp:HiddenField>
                                    <asp:HiddenField ID="grdHdfItemId" runat="server" Value='<%# Eval("ItemId") %>'></asp:HiddenField>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgDel" runat="server" ImageUrl="~/img/delete.png" Width="23px" Style="vertical-align: baseline;" CommandName="delChkItem" CommandArgument='<%# Eval("ItemId") %>' />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
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
                
                <div class="col-sm-4" style="font-weight:bold">
                    <asp:Label ID="lb_CountChecked_BF" runat="server" Text="ระบุจำนวน/Serial แล้ว"></asp:Label>
                    &nbsp;
                    <asp:Label ID="lb_CountChecked" runat="server" Text="0"></asp:Label>
                    &nbsp;
                    <asp:Label ID="lb_CountChecked_AF" runat="server" Text="รายการ"></asp:Label>
                </div>
                <div class="col-sm-4" style="font-weight:bold">
                    <asp:Label ID="lb_CountNotChecked_BF" runat="server" Text="เหลือจำนวน"></asp:Label>
                    &nbsp;
                    <asp:Label ID="lb_CountNotChecked" runat="server" Text="0"></asp:Label>
                    &nbsp;
                    <asp:Label ID="lb_CountNotChecked_AF" runat="server" Text="รายการ"></asp:Label>
                </div>
                <div class="col-sm-4 div-caption">
                    <asp:Button ID="btnDeleteAllChecked" runat="server" Text="ลบรายการทั้งหมด" Width="125px" class="btn btn-white btn-warning btn-sm" OnClick="btnDeleteAllChecked_Click" OnClientClick="return confirm('คุณต้องการลบรายการตรวจสอบสินค้าโปรโมชั่นที่ตรวจสอบแล้วทั้งหมด หรือไม่?');" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btnSave" runat="server" class="btn btn-white btn-info btn-sm" Width="125px" Text="บันทึก" OnClick="btnSave_Click" />
                </div>
                <div class="col-sm-12" style="opacity:0; width:0px; height:0px;">
                    <asp:Button ID="btnX" runat="server" Width="0px" Height="0px" OnClick="btnX_Click" />
                    <a id="btnPopupEdit"></a>
                    <asp:Button ID="btnSaveEditItemHidden" runat="server" Width="0px" Height="0px" OnClick="btnSaveEditItemHidden_Click" />

                    <asp:HiddenField ID="hdfEditItemId" runat="server" />
                    <asp:HiddenField ID="hdfEditSerial" runat="server" />
                    <asp:HiddenField ID="hdfEditItemName" runat="server" />
                    <asp:HiddenField ID="hdfEditModelName" runat="server" />
                    <asp:HiddenField ID="hdfEditColor" runat="server" />
                    <asp:HiddenField ID="hdfEditDimensionWidth" runat="server" />
                    <asp:HiddenField ID="hdfEditDimensionLong" runat="server" />
                    <asp:HiddenField ID="hdfEditDimensionHeight" runat="server" />
                    <asp:HiddenField ID="hdfEditDimensionUnit" runat="server" />
                    <asp:HiddenField ID="hdfEditWeight" runat="server" />
                    <asp:HiddenField ID="hdfEditWeightUnit" runat="server" />
                    <asp:HiddenField ID="hdfEditPrice" runat="server" />
                    <asp:HiddenField ID="hdfEditProduceDate" runat="server" />
                    <asp:HiddenField ID="hdfEditExpireDate" runat="server" />
                    <asp:HiddenField ID="hdfEditDetail" runat="server" />
                </div>
            </div>

        </div>
    </div>



    <div id="element_to_pop_up" style="background-color: white; width: 700px; height: 450px; padding: 10px 10px 10px 10px;">
        <span class="button b-close"><span>X</span></span>
        <div>
            <div class="col-sm-12">
                <asp:Label ID="lbEditHeader" runat="server" style="font-size:x-large;">แก้ไขข้อมูลสินค้า</asp:Label>
            </div>
            <div class="col-sm-12">&nbsp;</div>
            <div class="col-sm-12">
                <div class="col-sm-2 div-caption">รหัสสินค้า</div>
                <div class="col-sm-4">
                    <asp:TextBox ID="txtEditItemId" runat="server" onkeyup="keyTextItemId(this.id);" placeholder="Item Id" class="col-sm-12" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="col-sm-2 div-caption">Serial No.</div>
                <div class="col-sm-4">
                    <asp:TextBox ID="txtEditSerial" runat="server" onkeyup="keyTextSerial(this.id);" placeholder="Serial No." class="col-sm-12"></asp:TextBox>
                </div>
            </div>
            <div class="col-sm-12">&nbsp;</div>
            <div class="col-sm-12">
                <div class="col-sm-2 div-caption">ชื่อสินค้า</div>
                <div class="col-sm-4">
                    <asp:TextBox ID="txtEditItemName" runat="server" onkeyup="keyTextItemName(this.id);" placeholder="ชื่อสินค้า" class="col-sm-12"></asp:TextBox>
                </div>
                <div class="col-sm-2 div-caption">ชื่อรุ่น</div>
                <div class="col-sm-4">
                    <asp:TextBox ID="txtEditModelName" runat="server" onkeyup="keyTextModelName(this.id);" placeholder="ชื่อรุ่น" class="col-sm-12"></asp:TextBox>
                </div>
            </div>
            <div class="col-sm-12">&nbsp;</div>
            <div class="col-sm-12">
                <div class="col-sm-2 div-caption">สี</div>
                <div class="col-sm-4">
                    <asp:TextBox ID="txtEditColor" runat="server" onkeyup="keyTextColor(this.id);" placeholder="สี" class="col-sm-12"></asp:TextBox>
                </div>
                <div class="col-sm-2 div-caption">ขนาด</div>
                <div class="col-sm-4">
                    <asp:TextBox ID="txtEditDimensionWidth" runat="server" onkeyup="keyTextDimensionWidth(this.id);" placeholder="กว้าง" class="col-sm-3 digitOnly"></asp:TextBox>
                    <asp:TextBox ID="txtEditDimensionLong" runat="server" onkeyup="keyTextDimensionLong(this.id);" placeholder="ยาว" class="col-sm-3 digitOnly"></asp:TextBox>
                    <asp:TextBox ID="txtEditDimensionHeight" runat="server" onkeyup="keyTextDimensionHeight(this.id);" placeholder="สูง" class="col-sm-3 digitOnly"></asp:TextBox>
                    <asp:TextBox ID="txtEditDimensionUnit" runat="server" onkeyup="keyTextDimensionUnit(this.id);" placeholder="หน่วย" class="col-sm-3"></asp:TextBox>
                </div>
            </div>
            <div class="col-sm-12">&nbsp;</div>

            <div class="col-sm-12">
                <div class="col-sm-2 div-caption">น้ำหนัก</div>
                <div class="col-sm-4">
                    <asp:TextBox ID="txtEditWeight" runat="server" onkeyup="keyTextWeight(this.id);" placeholder="น้ำหนัก" class="col-sm-9 digitOnly"></asp:TextBox>
                    <asp:TextBox ID="txtEditWeightUnit" runat="server" onkeyup="keyTextWeightUnit(this.id);" placeholder="หน่วย" class="col-sm-3"></asp:TextBox>
                </div>
                <div class="col-sm-2 div-caption">มูลค่า</div>
                <div class="col-sm-4">
                    <asp:TextBox ID="txtEditPrice" runat="server" onkeyup="keyTextPrice(this.id);" placeholder="0.00" class="col-sm-12 label-caption digitOnly" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
            <div class="col-sm-12">&nbsp;</div>
            <div class="col-sm-12">
                <div class="col-sm-2 div-caption">วันผลิต</div>
                <div class="col-sm-4">
                    <asp:TextBox ID="txtEditProduceDate" runat="server" onkeyup="keyTextProduceDate(this.id);" placeholder="dd/MM/yyyy" class="col-sm-12"></asp:TextBox>
                </div>
                <div class="col-sm-2 div-caption">วันหมดอายุ</div>
                <div class="col-sm-4">
                    <asp:TextBox ID="txtEditExpireDate" runat="server" onkeyup="keyTextExpireDate(this.id);" placeholder="dd/MM/yyyy" class="col-sm-12"></asp:TextBox>
                </div>
            </div>
            <div class="col-sm-12">&nbsp;</div>
            <div class="col-sm-12">
                <div class="col-sm-2 div-caption">รายละเอียด</div>
                <div class="col-sm-10">
                    <asp:TextBox ID="txtEditDetail" runat="server" onkeyup="keyTextDetail(this.id);" placeholder="รายละเอียด" class="col-sm-12" TextMode="MultiLine" Height="75px"></asp:TextBox>
                </div>
            </div>
            <div class="col-sm-12">&nbsp;</div>

            <div class="col-sm-12" style="text-align: right;">
                <div class="col-sm-12">
                    <asp:Button ID="btnSaveEditItem" runat="server" Text="แก้ไข" Width="100px" class="btn btn-white btn-info btn-sm" OnClientClick="clickSaveEditItem()" />
                </div>
            </div>
        </div>
    </div>
    <%--<div class="col-sm-12" style="text-align: right;" id="divBtnCreate">
        <asp:Button ID="btnOK" runat="server" Text="ตั้งเรื่องขอเบิก" class="btn btn-white btn-info btn-sm" Width="100px" OnClick="btnOK_Click" />
        &nbsp;
        <asp:Button ID="btnDelReq" runat="server" Text="ยกเลิกใบเบิก" class="btn btn-white btn-danger btn-sm" Width="100px" Style="display: none;" OnClick="btnDelReq_Click" />
    </div>--%>
</asp:Content>

