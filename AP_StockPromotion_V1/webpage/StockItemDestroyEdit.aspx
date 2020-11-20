<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockItemDestroyEdit.aspx.cs" 
    Inherits="AP_StockPromotion_V1.webpage.StockItemDestroyEdit"  MasterPageFile="~/Master/MasterPage.Master" %>

<asp:Content ID="Contenthead" ContentPlaceHolderID="head" runat="server">
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
            $('#divNavx').html('ตัดสต๊อกสูญเสีย');
            $(".js-example-basic-single").select2({ width: '100%' });
            $('.js-example-basic-single-ws').select2({ minimumResultsForSearch: Infinity, width: '100%' });
            //initGridScroll();
            setTextDate();
            setTextNumericOnly();
            setTextDigitOnly();
            setHeightGrid();
            setButtonDialogClick();
            setDDLDisable();
        });

        function setDDLDisable() {
            var proj = $('#<%= hdfSelProject.ClientID %>').val();
            var itm = $('#<%= hdfSelItem.ClientID %>').val();
            if (proj != '') {
                $('.js-example-basic-single').prop('disabled', true);
            }

        }

        function initGridScroll() {
            $('#<%= grdData.ClientID %>').Scrollable({
                ScrollHeight: ($(window).height() - 300),
                IsInUpdatePanel: false
            });
        }

        function setHeightGrid() {
            $('#divGrid').height(screen.height - 270);
            $('#divGrid').attr('style', 'font-size: 0.875em;');
        }

        function setTextDate() {
            $("#<%= txtPostingDate.ClientID %>").datepicker({ dateFormat: "dd/mm/yy" });
        }

        function setTextNumericOnly() {
            jQuery('.numericOnly').keyup(function () {
                this.value = this.value.replace(/[^0-9]/g, '');
            });
        }

        function setTextDigitOnly() {
            jQuery('.digitOnly').keyup(function () {
                this.value = this.value.replace(/[^0-9\.]/g, '');
            });
        }

        function checkDestroyLimit(hdfLimitId, _inpId) {
            var lim = parseInt($('#' + hdfLimitId + '').val());
            var amt = parseInt($('#' + _inpId + '').val());
            if (lim < amt) {
                $('#' + _inpId + '').val(lim + '');
            }
        }

        function setButtonDialogClick() {
            $("#<%= btnSaveReason.ClientID %>").bind('click', function (e) {

                // Prevents the default action to be triggered. 
                e.preventDefault();

                // Triggering bPopup when click event is fired
                $('#element_to_pop_up').bPopup({
                    modalClose: false,
                    opacity: 0.6,
                    positionStyle: 'fixed' //'fixed' or 'absolute'
                });

            });
        }

        function clickSaveDrestroy() {
            $("#<%= btnSave.ClientID %>").click();
        }

        function keyTextReason(_inp) {
            $("#<%= hdfReason.ClientID %>").val($("#<%= txtReason.ClientID %>").val());
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdfReason" runat="server" Value="" />
    <asp:HiddenField ID="hdfSelProject" runat="server" />
    <asp:HiddenField ID="hdfSelItem" runat="server" />

    <asp:HiddenField ID="hdfCompanySAPCode" runat="server" />
    <asp:HiddenField ID="hdfProfitHO" runat="server" />

    <div class="col-sm-12">
        <div class="col-sm-12">
            <div class="col-sm-1 div-caption">โครงการ</div>
            <div class="col-sm-3">
                <asp:DropDownList ID="ddlProject" runat="server" class="col-sm-12 js-example-basic-single js-states form-control hide"></asp:DropDownList>
            </div>
            <div class="col-sm-1 div-caption">สินค้า</div>
            <div class="col-sm-3">
                <asp:DropDownList ID="ddlItem" runat="server" class="col-sm-12 js-example-basic-single js-states form-control hide"></asp:DropDownList>
            </div>
            <div class="col-sm-1 div-caption">Posting Date</div>
            <div class="col-sm-2">
                <asp:TextBox ID="txtPostingDate" runat="server" class="label-caption"></asp:TextBox>
            </div>
            <div class="col-sm-1 div-caption">
                <asp:Button ID="btnSelect" runat="server" Text="ตกลง" class="btn btn-white btn-info btn-sm" Width="75px" OnClick="btnSelect_Click" />
            </div>
        </div>
    </div>
    
    <div class="col-sm-12">&nbsp;</div>




    <div class="col-sm-12" id="divGrid">
        <asp:GridView ID="grdData" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" class="col-sm-12" 
            EmptyDataText="No data." ShowHeaderWhenEmpty="True" OnRowDataBound="grdData_RowDataBound" AllowPaging="True" AllowSorting="True" OnPageIndexChanging="grdData_PageIndexChanging" OnSorting="grdData_Sorting">
            <AlternatingRowStyle BackColor="White" />
            <Columns>                      
                <asp:TemplateField HeaderStyle-Width="85%" HeaderText="<div class='col-sm-2' style='font-weight: bold;'>ชื่อสินค้า</div><div class='col-sm-4' style='font-weight: bold;'>รายละเอียดสินค้า</div><div class='col-sm-2 div-caption'>คุณลักษณะสินค้า</div><div class='col-sm-3'>หมายเลข Serial No.</div>">
                    <ItemTemplate>
                        <div class="col-sm-12">
                            <div class="col-sm-3" style="font-weight:bold;"><asp:Label ID="Label1" runat="server" Text='<%# Eval("ItemName") %>' ></asp:Label></div>
                            <div class="col-sm-3" style="font-weight:bold;"><asp:Label ID="Label2" runat="server" Text='<%# Eval("Model") %>' ></asp:Label></div>
                            <div class="col-sm-1 div-caption">ผู้รับผิดชอบ</div>
                            <div class="col-sm-2">
                                <asp:Label ID="grdlbUserResponse" runat="server" Text='<%# Eval("FullName") %>' ></asp:Label>
                            </div>
                            <div class="col-sm-1 div-caption">Serial No.</div>
                            <div class="col-sm-2">
                                <asp:Label ID="grdlbSerial" runat="server" Text='<%# Eval("Serial") %>' ></asp:Label>
                            </div>                            
                        </div>
                        <div class="col-sm-12">
                            <div class="col-sm-1 div-caption">สี</div>
                            <div class="col-sm-2">
                                <asp:Label ID="Label3" runat="server" Text='<%# Eval("Color") %>' ></asp:Label>
                            </div>
                            <div class="col-sm-1 div-caption">ผลิต</div>
                            <div class="col-sm-2">
                                <asp:Label ID="LabelProduceDate" runat="server" Text='<%# Eval("ProduceDate","{0:dd/MM/yyyy}") %>' DataFormatString="{0:dd/MM/yyyy}" ></asp:Label>
                            </div>
                            <div class="col-sm-1 div-caption">หมดอายุ</div>
                            <div class="col-sm-2">
                                <asp:Label ID="LabelExpireDate" runat="server" Text='<%# Eval("ExpireDate", "{0:dd/MM/yyyy}") %>' DataFormatString="{0:dd/MM/yyyy}" ></asp:Label>
                            </div>
                            <div class="col-sm-1 div-caption">มูลค่า</div>
                            <div class="col-sm-2">
                                <asp:Label ID="Label5" runat="server" Text='<%# Eval("Price","{0:#,##0.00}") %>' DataFormatString="{0:#,##0.00}" ></asp:Label>
                            </div>
                        </div>

                        <div class="col-sm-12">
                            <div class="col-sm-1 div-caption">Detail</div>
                            <div class="col-sm-2">
                                <asp:Label ID="LabelDetail" runat="server" Text='<%# Eval("Detail") %>'></asp:Label>
                            </div>
                            <div class="col-sm-1 div-caption">Remark</div>
                            <div class="col-sm-2">
                                <asp:Label ID="LabelRemark" runat="server" Text='<%# Eval("Remark") %>' ></asp:Label>
                            </div>
                            <div class="col-sm-1 div-caption"></div>
                            <div class="col-sm-2">
                            </div>
                            <div class="col-sm-1 div-caption">เลขที่ใบนำจ่าย</div>
                            <div class="col-sm-2">
                                <asp:Label ID="LabelTrListId" runat="server" Text='<%# Eval("TrListId") %>' ></asp:Label>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ItemAmount" HeaderText="จำนวนสต๊อก" HeaderStyle-HorizontalAlign="Center" />  
                <asp:TemplateField HeaderText="จำนวนสูญเสีย" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:TextBox ID="grdTxtDestroyAmount" runat="server" placeholder="0" class="numericOnly div-caption" Width="50px"></asp:TextBox>
                        <asp:HiddenField ID="grdHdfItemAmount" runat="server" Value='<%# Eval("ItemAmount") %>' />
                        <asp:HiddenField ID="grdHdfSerial" runat="server" Value='<%# Eval("Serial") %>' />
                        <asp:HiddenField ID="grdHdfBarcode" runat="server" Value='<%# Eval("Barcode") %>' />
                        <asp:HiddenField ID="grdHdfItemName" runat="server" Value='<%# Eval("ItemName") %>' />
                        <asp:HiddenField ID="grdHdfModel" runat="server" Value='<%# Eval("Model") %>' />
                        <asp:HiddenField ID="grdHdfColor" runat="server" Value='<%# Eval("Color") %>' />
                        <asp:HiddenField ID="grdHdfDimensionWidth" runat="server" Value='<%# Eval("DimensionWidth") %>' />
                        <asp:HiddenField ID="grdHdfDimensionLong" runat="server" Value='<%# Eval("DimensionLong") %>' />
                        <asp:HiddenField ID="grdHdfDimensionHeight" runat="server" Value='<%# Eval("DimensionHeight") %>' />
                        <asp:HiddenField ID="grdHdfDimensionUnit" runat="server" Value='<%# Eval("DimensionUnit") %>' />
                        <asp:HiddenField ID="grdHdfWeight" runat="server" Value='<%# Eval("Weight") %>' />
                        <asp:HiddenField ID="grdHdfWeightUnit" runat="server" Value='<%# Eval("WeightUnit") %>' />
                        <asp:HiddenField ID="grdHdfPrice" runat="server" Value='<%# Eval("Price") %>' />
                        <asp:HiddenField ID="grdHdfProduceDate" runat="server" Value='<%# Eval("ProduceDate") %>' />
                        <asp:HiddenField ID="grdHdfExpireDate" runat="server" Value='<%# Eval("ExpireDate") %>' />
                        <asp:HiddenField ID="grdHdfDetail" runat="server" Value='<%# Eval("Detail") %>' />
                        <asp:HiddenField ID="grdHdfRemark" runat="server" Value='<%# Eval("Remark") %>' />
                        <asp:HiddenField ID="grdHdfUserResponse" runat="server" Value='<%# Eval("UserResponse") %>' />
                        <asp:HiddenField ID="grdHdfTrListId" runat="server" Value='<%# Eval("TrListId") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                ไม่มีพบรายการสินค้า...
            </EmptyDataTemplate>
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

    <%--<div class="col-sm-1 div-caption">ขนาด</div>
    <div class="col-sm-2">
        <asp:Label ID="Label4" runat="server" Text='<%# Eval("DimensionWidth") + " x " + Eval("DimensionLong") + " x " + Eval("DimensionHeight") + " " + Eval("DimensionUnit") %>' ></asp:Label>
    </div>
    <div class="col-sm-1 div-caption">น้ำหนัก</div>
    <div class="col-sm-2">
        <asp:Label ID="grdLbWeight" runat="server" Text='<%# Eval("Weight") + " " + Eval("WeightUnit") %>'></asp:Label>
    </div>--%>

    <div class="col-sm-12" style="text-align: right;">
        <asp:Button ID="btnSaveReason" runat="server" Text="บันทึก" class="btn btn-white btn-warning btn-sm" Width="75px" />  
        <asp:Button ID="btnSave" runat="server" Text="ตัดสต๊อกสูญเสีย" class="btn btn-white btn-info btn-sm" OnClick="btnSave_Click" style="display:none;"/>      
    </div>

    <div id="element_to_pop_up" style="background-color:white; width:400px; height:300px; padding:10px 10px 10px 10px;">
        <span class="button b-close"><span>X</span></span>
        <div>
            <div class="col-sm-12">
                <asp:Label ID="lbReason" runat="server" style="font-size:x-large;">เหตุผล</asp:Label>
            </div>
            <div class="col-sm-12" style="text-align: right;">&nbsp;</div>
            <div class="col-sm-12">
                <asp:TextBox ID="txtReason" runat="server" TextMode="MultiLine" Height="150px" class="col-sm-12" onkeyup="keyTextReason(this.id);"></asp:TextBox>
            </div>
            <div class="col-sm-12" style="text-align: right;">&nbsp;</div>
            <div class="col-sm-12" style="text-align: right;">
                <asp:Button ID="btnSaveDummy" runat="server" Text="ตัดสต๊อกสูญเสีย" class="btn btn-white btn-info btn-sm" OnClientClick="clickSaveDrestroy();" />  
            </div>
        </div>
    </div>

    
</asp:Content>
