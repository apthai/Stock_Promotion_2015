<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockTransferItemDetail.aspx.cs"
    Inherits="AP_StockPromotion_V1.webpage.StockTransferItemDetail" MasterPageFile="~/Master/MasterPage.Master" %>

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
            $('#divNavx').html('โอนสินค้าโปรโมชั่นเข้าโครงการ');
            $(".js-example-basic-single").select2({ width: '100%' });
            $('.js-example-basic-single-ws').select2({ minimumResultsForSearch: Infinity, width: '100%' });
            initTextInput();            
        });

        function setDatePicker() {
            $("#<%= txtRequestDate.ClientID %>").datepicker({ dateFormat: "dd/mm/yy" }); // { dateFormat: "dd-mm-yy" }
        }

        function initTextInput() {
            $('#<%= txtSerial.ClientID %>').keypress(function (e) {
                if (e.which == 13) {
                    e.preventDefault();
                    document.getElementById('<%= imgCheckSerial.ClientID %>').click();
                }
            });
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
    <asp:Button ID="btnDummy" runat="server" Text="" Width="0px" Height="0px" Style="opacity: 0.0;" />
                
    <asp:HiddenField ID="hdfReqHeaderId" runat="server" Value="" />
    <asp:HiddenField ID="hdfReqProject" runat="server" Value="" />
    
    <div id="req" class="tab-pane fade active in">

        
        <div class="col-sm-6">
            <div class="col-sm-12">
                <div class="col-sm-4 div-caption">เลขที่เอกสาร</div>
                <div class="col-sm-8">
                    <asp:TextBox ID="txtReqDocNo" runat="server" class="col-sm-12" ReadOnly="true" placeholder="เลขที่เอกสาร"></asp:TextBox>
                </div>
            </div>
            <div class="col-sm-12">
                <div class="col-sm-4 div-caption">โครงการ</div>
                <div class="col-sm-8">
                    <asp:TextBox ID="txtProject" runat="server" class="col-sm-12" ReadOnly="true" placeholder="Project" onclick="$('.js-example-basic-single').prop('disable',true);"></asp:TextBox>
                </div>
            </div>
            <div class="col-sm-12">
                <div class="col-sm-4 div-caption">วันที่รับเอกสารขอเบิก</div>
                <div class="col-sm-8">
                    <asp:TextBox ID="txtReqDocDate" runat="server" class="col-sm-12" ReadOnly="true" placeholder="วันที่รับเอกสารขอเบิก"></asp:TextBox>
                </div>
            </div>
            <div class="col-sm-12">
                <div class="col-sm-4 div-caption">วันที่บันทึกใบขอเบิก</div>
                <div class="col-sm-8">
                    <asp:TextBox ID="txtRequestDate" runat="server" class="col-sm-12" ReadOnly="true" placeholder="วันที่บันทึกใบขอเบิก"></asp:TextBox>
                </div>
            </div>
            <div class="col-sm-12">
                <div class="col-sm-4 div-caption">ผู้เบิก</div>
                <div class="col-sm-8">
                    <asp:TextBox ID="txtRequestBy" runat="server" class="col-sm-12" ReadOnly="true" placeholder="Request By"></asp:TextBox>
                </div>
            </div>

        </div>

        <div class="col-sm-6">            
            <div class="col-sm-12">
                <div class="col-sm-4 div-caption">เลขที่เอกสารอ้างอิง</div>
                <div class="col-sm-8">
                    <asp:TextBox ID="txtReqNo" runat="server" class="col-sm-12" ReadOnly="true" placeholder="เลขที่เอกสารอ้างอิง"></asp:TextBox>
                </div>
            </div>
            <div class="col-sm-12">
                <div class="col-sm-4 div-caption">เบิกสำหรับ</div>
                <div class="col-sm-8">
                    <asp:TextBox ID="txtReqType" runat="server" class="col-sm-12" ReadOnly="true" placeholder="Request No."></asp:TextBox>
                    <asp:DropDownList ID="ddlReqType" runat="server" style="display:none;"></asp:DropDownList>
                </div>
            </div>
            <div class="col-sm-12">
                <div class="col-sm-4 div-caption">หมายเหตุ</div>
                <div class="col-sm-8">
                    <asp:TextBox ID="txtReqHeaderRemark" runat="server" ReadOnly="true" class="col-sm-12" placeholder="หมายเหตุ" TextMode="MultiLine" Height="80px"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="col-sm-12 div-caption">&nbsp;</div>
        
        <div class="col-sm-12" id="divCheckSerial">
            <div class="col-sm-1 div-caption">Serial No.</div>
            <div class="col-sm-3">
                <asp:TextBox ID="txtSerial" runat="server" class="col-sm-12" placeHolder="Serial No."></asp:TextBox>
            </div>
            <div class="col-sm-8">
                <asp:ImageButton ID="imgCheckSerial" runat="server" ImageUrl="~/img/checkbox_checked.png" Width="23px" Style="vertical-align: baseline;" OnClick="imgCheckSerial_Click" />
            </div>
        </div>

        <div class="col-sm-12">&nbsp;</div>
        
        <div class="col-sm-12">
            <asp:GridView ID="grdRequest" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" class="col-sm-12"
                EmptyDataText="No data." ShowHeaderWhenEmpty="True" OnRowCommand="grdRequest_RowCommand" OnRowDataBound="grdRequest_RowDataBound">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="ItemName" HeaderText="สินค้าโปรโมชั่น" HeaderStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="ReqAmount" HeaderText="จำนวน" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" />
                    <asp:BoundField DataField="Balance" HeaderText="คงเหลือเบิก" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" />
                    <asp:BoundField DataField="Transfer" HeaderText="จำนวนที่เบิก" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" ItemStyle-ForeColor="Red" />                    
                    <asp:BoundField DataField="ItemUnit" HeaderText="หน่วย" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="75px" />
                    <asp:BoundField DataField="ProStartDate" HeaderText="วันเริ่มต้น" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="ProEndDate" HeaderText="วันสิ้นสุด" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgTran" runat="server" ImageUrl="~/img/ToProject.png" Width="23px" Style="vertical-align: baseline;" CommandName="TrnItem" CommandArgument='<%# Eval("ReqId") %>' />
                            <asp:HiddenField ID="grdHdfReqHeaderId" runat="server" Value='<%# Eval("ReqHeaderId") %>' />
                            <asp:HiddenField ID="grdHdfReqNo" runat="server" Value='<%# Eval("ReqNo") %>' />
                            <asp:HiddenField ID="grdHdfReqDate" runat="server" Value='<%# Eval("ReqDate") %>' />
                            <asp:HiddenField ID="grdHdfReqBy" runat="server" Value='<%# Eval("ReqBy") %>' />
                            <asp:HiddenField ID="grdHdfReqType" runat="server" Value='<%# Eval("ReqType") %>' />
                            <asp:HiddenField ID="grdHdfReqHeaderRemark" runat="server" Value='<%# Eval("ReqHeaderRemark") %>' />
                            <asp:HiddenField ID="grdHdfReqId" runat="server" Value='<%# Eval("ReqId") %>' />
                            <asp:HiddenField ID="grdHdfProject_Id" runat="server" Value='<%# Eval("Project_Id") %>' />
                            <asp:HiddenField ID="grdHdfProjectID" runat="server" Value='<%# Eval("ProjectID") %>' />
                            <asp:HiddenField ID="grdHdfItemId" runat="server" Value='<%# Eval("ItemId") %>' />
                            <asp:HiddenField ID="grdHdfItemNo" runat="server" Value='<%# Eval("ItemNo") %>' />
                            <asp:HiddenField ID="grdHdfItemName" runat="server" Value='<%# Eval("ItemName") %>' />
                            <asp:HiddenField ID="grdHdfReqAmount" runat="server" Value='<%# Eval("ReqAmount") %>' />
                            <asp:HiddenField ID="grdHdfItemUnit" runat="server" Value='<%# Eval("ItemUnit") %>' />
                            <asp:HiddenField ID="grdHdfItemCountMethod" runat="server" Value='<%# Eval("ItemCountMethod") %>' />
                            <asp:HiddenField ID="grdHdfTransferredAmount" runat="server" Value='<%# Eval("TransferredAmount") %>' />
                            
                            <asp:HiddenField ID="grdHdfTransfer" runat="server" Value='<%# Eval("Transfer") %>' />
                            <asp:HiddenField ID="grdHdfBalance" runat="server" Value='<%# Eval("Balance") %>' />
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
    </div>

    <div class="col-sm-12 div-caption">
        <asp:Button id="btnTransferItemToProject" runat="server" class="btn btn-white btn-info btn-sm" Text="ส่งมอบสินค้าไปยังโครงการ" OnClick="btnTransferItemToProject_Click" />
        <!--
            DataTable dtItemTransferToProject = (DataTable)Session["ItemTransfer"];
                if (dtItemTransferToProject != null) { } 
            -->
    </div>
    
    <div class="col-sm-12 div-caption">&nbsp;</div>
    <div class="col-sm-12" style="font-weight:bold; font-size:x-large">
        ประวัติการให้สินค้าโปรโมชั่นกับโครงการ
    </div>
    <div class="col-sm-12">
        <asp:GridView ID="grdTransferHistory" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" class="col-sm-12"
            EmptyDataText="No data." ShowHeaderWhenEmpty="True" OnRowCommand="grdTransferHistory_RowCommand" OnRowDataBound="grdTransferHistory_RowDataBound" >
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="ItemName" HeaderText="สินค้าโปรโมชั่น" HeaderStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="ItemAmount" HeaderText="จำนวน" HeaderStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="CreateDate" HeaderText="วันที่" HeaderStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="FullName" HeaderText="ผู้ใช้งาน" HeaderStyle-HorizontalAlign="Center" />
                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:ImageButton ID="imgPrt" runat="server" ImageUrl="~/img/printer_and_fax.png" Width="23px" Style="vertical-align: baseline;" CommandName="PrtTrn" />
                        <asp:HiddenField ID="grdHdfTrListId" runat="server" Value='<%# Eval("TrListId") %>' />
                        <asp:HiddenField ID="grdHdfProject_Id" runat="server" Value='<%# Eval("Project_Id") %>' />
                        <asp:HiddenField ID="grdHdfTrStatus" runat="server" Value='<%# Eval("TrStatus") %>' />
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

    <!-- รับค่าจาก StockTransferItemCheckAmount ว่าเอา ItemId ไหนบ้าง แล้วกดปุ่ม -->
    <asp:HiddenField ID="hdfItemIdList" runat="server" Value="" />
    <asp:Button ID="btnCheckItemByAmount" runat="server" Width="0px" Height="0px" style="width:0px; height:0px; display:none; opacity:0;" OnClick="btnCheckItemByAmount_Click" />




</asp:Content>
