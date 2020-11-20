<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockReceiveEdit.aspx.cs" 
    Inherits="AP_StockPromotion_V1.web.StockReceiveEdit" MasterPageFile="~/Master/MasterPage.Master"  %>

<asp:Content ID="Contenthead" ContentPlaceHolderId="head" runat="server">
    
    <script src="../plugin/ScrollableGrid.js"></script>
    <style type="text/css">
        .label-caption {
            text-align:right;
        }
        .div-caption {
            text-align:right;
            padding-right:5px;
        }
    </style>
    <script type="text/javascript">
        jQuery(function ($) {
            $('#divNavx').html('รับสินค้า >> เพิ่มรายการรับสินค้า');
            $(".js-example-basic-single").select2({ width: '100%' });
            $('.js-example-basic-single-ws').select2({ minimumResultsForSearch: Infinity, width: '100%' });

            initDivHeight();
            initGridScroll();
            setDatePicker();
            initKeyPress();
            setTextNumericOnly();
            setTextDigitOnly();
            //$('#accordion-style').on('click', function (ev) {
            //    var target = $('input', ev.target);
            //    var which = parseInt(target.val());
            //    if (which == 2) $('#accordion').addClass('accordion-style2');
            //    else $('#accordion').removeClass('accordion-style2');
            //});
            calcTotal();
        });

        function initGridScroll() {
            var hs = $(window).height();
            var h1 = hs / 100 * 55;
            var h2 = hs / 100 * 25;
            $('#<%= grdData.ClientID %>').Scrollable({
                ScrollHeight: (h1 - 100),
                IsInUpdatePanel: false
            });
            $('#<%= grdHistory.ClientID %>').Scrollable({
                ScrollHeight: (h2 - 36),
                IsInUpdatePanel: false
            });
        }

        function popupStockTransfrtItemEdit(reqId) {
            // Popup80('StockTransferItemEdit.aspx?mode=Edit&reqId=' + reqId);
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

        function setDatePicker() {
            $("#<%= txtDocDate.ClientID %>").datepicker({ dateFormat: "dd/mm/yy" });
            $("#<%= txtPostingDate.ClientID %>").datepicker({ dateFormat: "dd/mm/yy" });
        }

        function foundPO() {
            calcTotal();
            $('#divFoundPO').removeAttr('style');
            $('#divFoundPOHead').removeAttr('style');
            $('#<%= imgPrtPO.ClientID %>').attr('style', 'vertical-align: baseline;width:23px;');
        }
        function notFoundPO() {
            $('#divFoundPO').attr('style', 'display:none;');
            $('#divFoundPOHead').attr('style', 'display:none;');
            $('#<%= imgPrtPO.ClientID %>').attr('style', 'vertical-align: baseline;width:23px;display:none;');
        }
        function calcTotal() {
            /* จำนวนรับ          */  var $arrReceive = $('#ContentPlaceHolder1_grdData').find('input:text[id^="ContentPlaceHolder1_grdData_grdTxtReceive"]');
            /* ราคาต่อหน่วย       */  var $arrNETPR = $('#ContentPlaceHolder1_grdData').find('input:hidden[id^="ContentPlaceHolder1_grdData_hdfSAP_NETPR_"]');
            /* ราคารวม          */  var $arrNETWR = $('#ContentPlaceHolder1_grdData').find('input:hidden[id^="ContentPlaceHolder1_grdData_hdfSAP_NETWR_"]');
            /* มูลค่า Vat         */  var $arrNAVNW = $('#ContentPlaceHolder1_grdData').find('input:hidden[id^="ContentPlaceHolder1_grdData_hdfSAP_NAVNW_"]');
            /* ราคารวม Inc Vat. */  var $arrEFFWR = $('#ContentPlaceHolder1_grdData').find('input:hidden[id^="ContentPlaceHolder1_grdData_hdfSAP_EFFWR_"]');
            /* จำนวนเบิก         */  var $arrMENGE = $('#ContentPlaceHolder1_grdData').find('input:hidden[id^="ContentPlaceHolder1_grdData_hdfSAP_MENGE_X_"]');
            /* grdlbTotalNoVat */  var $arrTotalNoVat = $('#ContentPlaceHolder1_grdData').find('span[id^="ContentPlaceHolder1_grdData_grdlbTotalNoVat_"]');
            /* grdlbTotalVat    */  var $arrTotalVat = $('#ContentPlaceHolder1_grdData').find('span[id^="ContentPlaceHolder1_grdData_grdlbTotalVat_"]');
            /* grdlbTotalIncVat */  var $arrTotalIncVat = $('#ContentPlaceHolder1_grdData').find('span[id^="ContentPlaceHolder1_grdData_grdlbTotalIncVat_"]');



            var totalPrVat = 0.0000;
            var totalVat = 0.0000;
            var totalIncVat = 0.0000;
            for (var ii = 0 ; ii < $arrReceive.length ; ii++) {
                var receive = parseInt($arrReceive[ii].value); if (isNaN(receive)) { receive = 0;}
                var NETPR = parseFloat($arrNETPR[ii].value); if (isNaN(NETPR)) { NETPR = 0.0; }
                var NETWR = parseFloat($arrNETWR[ii].value); if (isNaN(NETWR)) { NETWR = 0.0; }
                var NAVNW = parseFloat($arrNAVNW[ii].value); if (isNaN(NAVNW)) { NAVNW = 0.0; }
                var MENGE = parseFloat($arrMENGE[ii].value); if (isNaN(MENGE)) { MENGE = 0.0; }
                var EFFWR = parseFloat($arrEFFWR[ii].value); if (isNaN(EFFWR)) { MENGE = 0.0; }
                totalPrVat += NETPR * receive;
                totalVat += (NAVNW / MENGE) * receive;
                totalIncVat += (EFFWR / MENGE) * receive;

                $arrTotalNoVat[ii].innerHTML = formatCurrency(NETPR * receive, 2);
                $arrTotalVat[ii].innerHTML = formatCurrency((NAVNW / MENGE) * receive,2);
                $arrTotalIncVat[ii].innerHTML = formatCurrency((EFFWR / MENGE) * receive,2);
            }

            $('#<%= txtTotalBefVat.ClientID %>').val(formatCurrency(totalPrVat,2));
            $('#<%= txtTotalVat.ClientID %>').val(formatCurrency(totalVat, 2));
            $('#<%= txtTotal.ClientID %>').val(formatCurrency(totalIncVat, 2));

            //HiddenField NETPR = (HiddenField)e.Row.FindControl("hdfSAP_NETPR");        //	CURR	11	Net price	ราคาต่อหน่วย
            //HiddenField NETWR = (HiddenField)e.Row.FindControl("hdfSAP_NETWR");        //	CURR	15	Net Value in Document Currency	ราคารวม
            //HiddenField NAVNW = (HiddenField)e.Row.FindControl("hdfSAP_NAVNW");        //	CURR	13	Non-deductible input tax	ภาษี
            // NETPR : ราคาต่อหน่วย
            // grdTxtReceive.Text
        }

        function chkReceiveChange(_recId, MENGEId) {
            var receive = $('#' + _recId + '').val();
            var MENGEId = $('#' + MENGEId + '').val();

            var r = parseInt(receive);
            if (isNaN(r)) { r = 0; $('#' + _recId + '').val('0'); } else { $('#' + _recId + '').val(r); }
            var m = parseInt(MENGEId); if (isNaN(m)) { m = 0; }

            if (r > m) { $('#' + _recId + '').val(m); }
            calcTotal();
        }
        function initDivHeight() {
            var hs = $(window).height();
            var h1 = hs / 100 * 59;
            var h2 = hs / 100 * 24;
            $('#divReceive').height(h1);
            $('#divHistory').height(h2);
        }

        function initKeyPress(){
            $('#<%= txtPONo.ClientID %>').keypress(function (e) {
                if (e.which == 13) {
                    $('#<%= imgPO.ClientID %>').click();
                    return false;
                }
            });
        }

        function clickBindData() {
            parent.$("#ContentPlaceHolder1_btnSearch").click();
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

        function saveCompleted(PONo, GRNo) {
            alert('PO:' + PONo + ' - GR: ' + GRNo + ' Completed.');
            window.location = 'StockReceive.aspx?bindData=Y&PO_No=' + PONo;
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderId="ContentPlaceHolder1" runat="server">
    
    <div class="col-sm-12">
        <div class="col-sm-1 div-caption">PO:</div>
        <div class="col-sm-3">
            <asp:TextBox ID="txtPONo" runat="server" placeholder="เลขที่ PO" class="col-sm-12"></asp:TextBox>
            <asp:HiddenField ID="hdfPO_No" runat="server" />
        </div>
        <div class="col-sm-4">
            <asp:ImageButton ID="imgPO" runat="server" ImageUrl="~/img/search.png" Width="23px" Style="vertical-align: baseline;" OnClick="imgPO_Click" /><!--OnClick="imgEdit_Click" -->            &nbsp;
            <asp:ImageButton ID="imgPrtPO" runat="server" class="imgBtn" ToolTip="Print PO" ImageUrl="~/img/printer_and_fax.png" Width="23px" Style="vertical-align: baseline;" OnClick="imgPrtPO_Click" />
        </div>
        <div class="col-sm-4">
        </div>
    </div>
    <div class="col-sm-12" style="text-align: right;">&nbsp;</div>
    <div id="divFoundPOHead">
        <div class="col-sm-12">
            <div class="col-sm-2 div-caption">Delivery Note</div>
            <div class="col-sm-2 div">
                <asp:TextBox ID="txtRefDocNo" runat="server"></asp:TextBox>
            </div>
            <div class="col-sm-2 div-caption">Document Date</div>
            <div class="col-sm-2 div">
                <asp:TextBox ID="txtDocDate" runat="server" class="label-caption"></asp:TextBox>
            </div>
            <div class="col-sm-2 div-caption">Posting Date</div>
            <div class="col-sm-2 div">
                <asp:TextBox ID="txtPostingDate" runat="server" class="label-caption"></asp:TextBox>
            </div>
        </div>
        <div class="col-sm-12" style="text-align: right;">&nbsp;</div>
    </div>
    <div class="col-sm-12" id="divReceive">
        <asp:GridView ID="grdData" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" class="col-sm-12"
            EmptyDataText="No data." ShowHeaderWhenEmpty="True" OnRowDataBound="grdData_RowDataBound">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="EBELP" HeaderText="Item" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                <asp:TemplateField HeaderText="สินค้าโปรโมชั่น">
                    <ItemStyle HorizontalAlign="Left" />
                    <ItemTemplate>
                        <asp:Label ID="grdLbItem" runat="server" Visible="false"></asp:Label>
                        <asp:DropDownList ID="grdDDLItem" runat="server" class="col-sm-12 js-example-basic-single-ws js-states form-control hide"></asp:DropDownList>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" Width="60%" />
                </asp:TemplateField>                
                <asp:TemplateField HeaderText="ราคาต่อหน่วย">
                    <HeaderStyle HorizontalAlign="Center" Width="10%" />
                    <ItemStyle HorizontalAlign="Right" />
                    <ItemTemplate>
                        <asp:TextBox ID="grdTxtPricePerUnit" runat="server" placeholder="ราคาต่อหน่วย" Width="100%" ReadOnly="true" class="label-caption digitOnly"></asp:TextBox>
                        <asp:HiddenField ID="grdHdfPricePerUnit" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="รับแล้ว">
                    <ItemStyle HorizontalAlign="Right" />
                    <ItemTemplate>
                        <asp:Label ID="grdLbReceived" runat="server" Text="X/Y" Style="padding-right:23px;"></asp:Label>
                        <asp:HiddenField ID="hdfWaitAmount" runat="server" Value="" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="จำนวน">
                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <asp:TextBox ID="grdTxtReceive" runat="server" placeholder="จำนวน" Width="100%" class="label-caption numericOnly"></asp:TextBox>
                        <asp:HiddenField ID="hdfSAP_EBELN" runat="server" Value='<%# Eval("EBELN") %>' />
                        <asp:HiddenField ID="hdfSAP_EBELP" runat="server" Value='<%# Eval("EBELP") %>' />
                        <asp:HiddenField ID="hdfSAP_BSART" runat="server" Value='<%# Eval("BSART") %>' />
                        <asp:HiddenField ID="hdfSAP_BUKRS" runat="server" Value='<%# Eval("BUKRS") %>' />
                        <asp:HiddenField ID="hdfSAP_WERKS" runat="server" Value='<%# Eval("WERKS") %>' />
                        <asp:HiddenField ID="hdfSAP_MATNR" runat="server" Value='<%# Eval("MATNR") %>' />
                        <asp:HiddenField ID="hdfSAP_TXZ01" runat="server" Value='<%# Eval("TXZ01") %>' />
                        <asp:HiddenField ID="hdfSAP_MENGE_X" runat="server" Value='<%# String.Format("{0:N}", Eval("MENGE")) %>' />
                        <asp:HiddenField ID="hdfSAP_MENGE_A" runat="server" Value='<%# String.Format("{0:N}", Eval("MENGE_A")) %>' />
                        <asp:HiddenField ID="hdfSAP_MEINS" runat="server" Value='<%# Eval("MEINS") %>' />
                        <asp:HiddenField ID="hdfSAP_NETPR" runat="server" Value='<%# Eval("NETPR") %>' />
                        <asp:HiddenField ID="hdfSAP_NETWR" runat="server" Value='<%# Eval("NETWR") %>' />
                        <asp:HiddenField ID="hdfSAP_NAVNW" runat="server" Value='<%# Eval("NAVNW") %>' />
                        <asp:HiddenField ID="hdfSAP_EFFWR" runat="server" Value='<%# Eval("EFFWR") %>' />
                        <asp:HiddenField ID="hdfSAP_WAERS" runat="server" Value='<%# Eval("WAERS") %>' />
                        <asp:HiddenField ID="hdfSAP_BANFN" runat="server" Value='<%# Eval("BANFN") %>' />
                        <asp:HiddenField ID="hdfSAP_BNFPO" runat="server" Value='<%# Eval("BNFPO") %>' />
                        <asp:HiddenField ID="hdfSAP_KOSTL" runat="server" Value='<%# Eval("KOSTL") %>' />
                        <asp:HiddenField ID="hdfSAP_NPLNR" runat="server" Value='<%# Eval("NPLNR") %>' />
                        <asp:HiddenField ID="hdfSAP_PS_PSP_PNR" runat="server" Value='<%# Eval("PS_PSP_PNR") %>' />
                        <asp:HiddenField ID="hdfSAP_WBS_SHOW" runat="server" Value='<%# Eval("WBS_SHOW") %>' />
                        
                        <asp:HiddenField ID="hdfSAP_LIFNR" runat="server" Value='<%# Eval("LIFNR") %>' />
                        <asp:HiddenField ID="hdfSAP_VENDOR_NAME" runat="server" Value='<%# Eval("VENDOR_NAME") %>' />
                        <asp:HiddenField ID="hdfSAP_ZTERM" runat="server" Value='<%# Eval("ZTERM") %>' />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ราคารวม">
                    <HeaderStyle HorizontalAlign="Center" Width="10%" />
                    <ItemStyle HorizontalAlign="Right" />
                    <ItemTemplate>
                        <asp:Label ID="grdlbTotalNoVat" runat="server" class="label-caption" style="display:none;"></asp:Label>    
                        <asp:Label ID="grdlbTotalVat" runat="server" class="label-caption" style="display:none;"></asp:Label>       
                        <asp:Label ID="grdlbTotalIncVat" runat="server" class="label-caption"></asp:Label>                          
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
        <div class="col-sm-12" style="text-align: right;">&nbsp;</div>
        <div id="divFoundPO">
            <div class="col-sm-12">
                <div class="col-sm-2 div-caption">ยอดรวมก่อน Vat.</div>
                <div class="col-sm-2 div">
                    <asp:TextBox ID="txtTotalBefVat" runat="server" class="label-caption digitOnly" placeholder="0.00" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="col-sm-2 div-caption">Vat.</div>
                <div class="col-sm-2 div">
                    <asp:TextBox ID="txtTotalVat" runat="server" class="label-caption digitOnly" placeholder="0.00" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="col-sm-2 div-caption">ยอดรวม</div>
                <div class="col-sm-2 div">
                    <asp:TextBox ID="txtTotal" runat="server" class="label-caption digitOnly" placeholder="0.00" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
            <div class="col-sm-12" style="text-align: right;">&nbsp;</div>
            <div class="col-sm-12">
                <div class="col-sm-12 div-caption">
                    <asp:Button ID="btnAddReceive" runat="server" Text="รับสินค้า" class="btn btn-white btn-info btn-sm" OnClick="btnAddReceive_Click" />
                </div>
            </div>
        </div>
        <div class="col-sm-12" style="text-align: right;">&nbsp;</div>
    </div>
    <div class="col-sm-12" style="text-align: right;">&nbsp;</div>
    <div class="col-sm-12" style="text-align: right;">&nbsp;</div>
    
    
    <div class="col-sm-12" style="font-weight:bold; font-size:large;">ประวัติการรับสินค้าโปรโมชั่น</div>
    <div class="col-sm-12" id="divHistory">
        <asp:GridView ID="grdHistory" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" class="col-sm-12"
            EmptyDataText="No data." ShowHeaderWhenEmpty="True" OnRowCommand="grdHistory_RowCommand">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="GR_No" HeaderText="GR No." HeaderStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="CreateDate" HeaderText="วันที่รับ" HeaderStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="FullName" HeaderText="รับโดย" HeaderStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="ItemName" HeaderText="สินค้าโปรโมชั่น" HeaderStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="ReceiveAmount" HeaderText="จำนวน" HeaderStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="SAP_MEINS" HeaderText="หน่วย" HeaderStyle-HorizontalAlign="Center" />
                <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="75px">
                    <ItemTemplate>
                        <asp:ImageButton ID="grdPrtGR" runat="server" class="imgBtn" ToolTip="Print GR" ImageUrl="~/img/printer_and_fax.png" Width="23px" Style="vertical-align: baseline;" CommandName="PrtGR" />
                        <asp:HiddenField ID="hdfReceiveHeaderID" runat="server" Value='<%# Eval("ReceiveHeaderID") %>' />
                        <asp:HiddenField ID="hdfPO_No" runat="server" Value='<%# Eval("PO_No") %>' />
                        <asp:HiddenField ID="hdfGR_No" runat="server" Value='<%# Eval("GR_No") %>' />
                        <asp:HiddenField ID="hdfGR_Year" runat="server" Value='<%# Eval("GR_Year") %>' />
                        <asp:HiddenField ID="hdfVendor" runat="server" Value='<%# Eval("Vendor") %>' />
                        <asp:HiddenField ID="hdfCreateDate" runat="server" Value='<%# Eval("CreateDate") %>' />
                        <asp:HiddenField ID="hdfCreateBy" runat="server" Value='<%# Eval("CreateBy") %>' />
                        <asp:HiddenField ID="hdfReceiveHeaderStatus" runat="server" Value='<%# Eval("ReceiveHeaderStatus") %>' />
                        <asp:HiddenField ID="hdfReceiveDetailId" runat="server" Value='<%# Eval("ReceiveDetailId") %>' />
                        <asp:HiddenField ID="hdfPricePerUnit" runat="server" Value='<%# Eval("PricePerUnit") %>' />
                        <asp:HiddenField ID="hdfReceiveAmount" runat="server" Value='<%# Eval("ReceiveAmount") %>' />
                        <asp:HiddenField ID="hdfStatus" runat="server" Value='<%# Eval("Status") %>' />
                        <asp:HiddenField ID="hdfMasterItemId" runat="server" Value='<%# Eval("MasterItemId") %>' />
                        <asp:HiddenField ID="hdfItemNo" runat="server" Value='<%# Eval("ItemNo") %>' />
                        <asp:HiddenField ID="hdfItemName" runat="server" Value='<%# Eval("ItemName") %>' />
                        <asp:HiddenField ID="hdfItemUnit" runat="server" Value='<%# Eval("ItemUnit") %>' />
                        <asp:HiddenField ID="hdfItemStatus" runat="server" Value='<%# Eval("ItemStatus") %>' />
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
    <div class="col-sm-12">
        <asp:Button id="btnBack" runat="server" class="btn btn-white btn-warning btn-sm" style="width:123px;" Text="<< กลับ" OnClick="btnBack_Click"  />
    </div>
    
<%--    <div class="col-sm-12">
        <div class="col-sm-2" style="text-align: right;">
            วันที่รับ:<br /><br />
        </div>
        <div class="col-sm-4">
            <asp:TextBox ID="txtReceiveDate" runat="server" placeholder="23/03/2015" class="col-sm-12"></asp:TextBox>
        </div>
        <div class="col-sm-2" style="text-align: right;">
            ผู้รับ:<br /><br />
        </div>
        <div class="col-sm-4">
            <asp:TextBox ID="txtReceiveBy" runat="server" placeholder="AP00XXXX" class="col-sm-12"></asp:TextBox>
        </div>
    </div>

    <div class="col-sm-12" style="text-align: right;">
        <div class="col-sm-2" style="text-align: right;">
            เอกสารอ้างอิง:<br /><br />
        </div>
        <div class="col-sm-4">
            <asp:TextBox ID="txtRefDocNo" runat="server" Text="MM:MemoYY03000XXX" class="col-sm-12"></asp:TextBox>
        </div>
        <div class="col-sm-2" style="text-align: right;">
            โครงการ:<br /><br />
        </div>
        <div class="col-sm-4">
            <asp:DropDownList ID="ddlProject" runat="server" class="col-sm-12">
                <asp:ListItem Text="The City งามวงศ์วาน" Value="1"></asp:ListItem>
                <asp:ListItem Text="The City รามอินทรา" Value="2"></asp:ListItem>
                <asp:ListItem Text="Centro รามอินทรา 109" Value="3"></asp:ListItem>
                <asp:ListItem Text="Centro อ่อนนุช" Value="4"></asp:ListItem>
                <asp:ListItem Text="Centro พระราม 9" Value="5"></asp:ListItem>
            </asp:DropDownList>
        </div>
    </div>

    <div class="col-sm-12" style="text-align: right;">        
        <div class="col-sm-2" style="text-align: right;">
            สินค้า:<br /><br />
        </div>
        <div class="col-sm-4">
            <asp:DropDownList ID="ddlItem" runat="server" class="col-sm-12">
                <asp:ListItem Text="Gift Voucher Starbuck มูลค่า 1000 บาท" Value="1"></asp:ListItem>
                <asp:ListItem Text="Gift Voucher Starbuck มูลค่า 3000 บาท" Value="2"></asp:ListItem>
                <asp:ListItem Text="Gift Voucher Major มูลค่า 1500 บาท" Value="3"></asp:ListItem>
                <asp:ListItem Text="Gift Voucher Major มูลค่า 3000 บาท" Value="4"></asp:ListItem>
                <asp:ListItem Text="Gift Voucher Lotus มูลค่า 2000 บาท" Value="5"></asp:ListItem>
                <asp:ListItem Text="Gift Voucher Lotus มูลค่า 3000 บาท" Value="6"></asp:ListItem>
                <asp:ListItem Text="iPhone6 16GB มูลค่า 25,500 บาท" Value="7"></asp:ListItem>
                <asp:ListItem Text="ทองคำแท่ง หนัก 10 บาท" Value="8"></asp:ListItem>
                <asp:ListItem Text="สร้อยคอทองคำ หนัก 5 บาท" Value="9"></asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="col-sm-2" style="text-align: right;">
            ชื่อสินค้า:<br /><br />
        </div>
        <div class="col-sm-4">
            <asp:TextBox ID="txtDefaultName" runat="server" placeholder="Default Name" class="col-sm-12"></asp:TextBox>
        </div>
    </div>

    <div class="col-sm-12">
        <div class="col-sm-2" style="text-align: right;">
            จำนวน:<br /><br />
        </div>
        <div class="col-sm-4" style="text-align: right;">
            <asp:TextBox ID="txtAmount" runat="server" placeholder="10" class="col-sm-9"></asp:TextBox>
        </div>
        <div class="col-sm-2" style="text-align: right;">
            หน่วย:<br /><br />
        </div>
        <div class="col-sm-4" style="text-align: right;">
            <asp:TextBox ID="txtUnit" runat="server" placeholder="เครื่อง" class="col-sm-9"></asp:TextBox>
        </div>
    </div>
    <div class="col-sm-12">        
        <div class="col-sm-2" style="text-align: right;">
            มูลค่า:<br /><br />
        </div>
        <div class="col-sm-4" style="text-align: right;">
            <asp:TextBox ID="txtCost" runat="server" placeholder="25,000" class="col-sm-9"></asp:TextBox>
        </div>
        <div class="col-sm-2" style="text-align: right;">
            จำนวนเงิน:<br /><br />
        </div>
        <div class="col-sm-4" style="text-align: right;">
            <asp:TextBox ID="txtTotalCost" runat="server" placeholder="25,0000" class="col-sm-9"></asp:TextBox>
        </div>
    </div>
    <div class="col-sm-12" style="text-align: right;">&nbsp;</div>
    <div class="col-sm-12" style="text-align: center;">
        
        <button class="btn btn-white btn-success btn-sm" onclick="popupStockReceiveCreate();">
            <i class="ace-icon fa fa-floppy-o bigger-120 green"></i>บันทึก
        </button>
        
            &nbsp;
            &nbsp;
        
        <button class="btn btn-white btn-warning btn-sm" onclick="popupStockReceiveCreate();">
            <i class="ace-icon fa fa-undo bigger-120 orange"></i>ยกเลิก
        </button>
    </div>--%>
</asp:Content>
