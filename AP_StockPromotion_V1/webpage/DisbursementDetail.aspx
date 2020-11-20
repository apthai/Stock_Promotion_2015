<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DisbursementDetail.aspx.cs"
    Inherits="AP_StockPromotion_V1.webpage.DisbursementDetail" MasterPageFile="~/Master/MasterPage.Master" %>

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
            $('#divNavx').html('จ่ายสินค้าให้โครงการ');
            $(".js-example-basic-single").select2({ width: '100%' });
            $('.js-example-basic-single-ws').select2({ minimumResultsForSearch: Infinity, width: '100%' });
            initTextInput();
        });

        function SetEqualtation(set, msgerr) {
            if (set) {
                $('.equatation').removeClass('hide');
                $('.TableCheckBox').removeClass('hide');
                if (msgerr != '') {
                    alert(msgerr);
                }
            }
            else
            {
                $('.equatation').removeClass('hide').addClass('hide');
                $('.TableCheckBox').removeClass('hide').addClass('hide');
                if (msgerr != '') {
                    alert(msgerr);
                }
            }
        }

        function CheckItem(chkId, ReqId) {
            //alert(id);

            var f = document.all;

            var tb = f.ContentPlaceHolder1_grdRequest;
            if (tb) {
                //var all = 1;

                for (var i = 1; i < tb.rows.length; i++) {
                    var r = tb.rows[i];
                    var chk = f[r.getAttribute("CheckBoxID")];
                    //var ReqID = r.getAttribute["ReqID"];

                    if (chk) {
                       

                        if (r.getAttribute("CheckBoxID") == chkId) {
                            chk.checked = chk.checked;
                        }
                        else
                        {
                            chk.checked = false;
                        }

                        //if (chk.checked) {
                        //    alert(ReqID);
                        //}
                       
                    }
                    
                }
            }
        }

        function setDatePicker() {
            // $("#").datepicker({ dateFormat: "dd/mm/yy" }); // { dateFormat: "dd-mm-yy" }
        }

        function initTextInput() {
            $('#<%= txtSerial.ClientID %>').keypress(function (e) {
                if (e.which == 13) {
                    e.preventDefault();
                    document.getElementById('<%= imgCheckSerial.ClientID %>').click();
                }
            });

            $('#<%= txtQuantityAndSerial.ClientID %>').keypress(function (e) {
                if (e.which == 13) {
                    e.preventDefault();
                    document.getElementById('<%= imgCheckSerialOrItem.ClientID %>').click();
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

    <asp:HiddenField ID="hdfReqIdList" runat="server" Value="" />
    <asp:HiddenField ID="hdfReqIdAmount" runat="server" Value="" />
     
                                       
    <div class="row">
        <div class="col-sm-12">
            <div id="accordion" class="accordion-style1 panel-group">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h4 class="panel-title">
                            <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion" href="#collapseOne">
                                <i class="ace-icon fa fa-angle-down bigger-110" data-icon-hide="ace-icon fa fa-angle-down" data-icon-show="ace-icon fa fa-angle-right"></i>
                                จ่ายสินค้าให้กับโครงการ
                            </a>
                        </h4>
                    </div>
                    <div class="panel-collapse collapse in" id="collapseOne">
                        <div class="panel-body">

                            <div id="req" class="tab-pane fade active in">

                                <div class="col-sm-12">
                                    <div class="col-sm-4">
                                        <div class="col-sm-3 div-caption">โครงการ</div>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="txtProject" runat="server" class="col-sm-12" ReadOnly="true" placeholder="Project" onclick="$('.js-example-basic-single').prop('disable',true);"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-sm-4">
                                        <div class="col-sm-3 div-caption">ผู้ขอเบิก</div>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="txtRequestBy" runat="server" class="col-sm-12" ReadOnly="true" placeholder="Request By"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-sm-4">
                                        <div class="col-sm-3 div-caption">เบิกสำหรับ</div>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="txtReqType" runat="server" class="col-sm-12" ReadOnly="true" placeholder="Request No."></asp:TextBox>
                                            <asp:DropDownList ID="ddlReqType" runat="server" Style="display: none;"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-12 div-caption">&nbsp;</div>

                                <div class="col-sm-12" id="divCheckSerial">
                                    <div class="col-sm-5">
                                        <div class="col-sm-8">
                                            <div class="col-sm-3 div-caption">Serial No.</div>
                                            <div class="col-sm-9">
                                                <asp:TextBox ID="txtSerial" runat="server" class="col-sm-12" placeHolder="Serial No."></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-sm-4">
                                            <asp:ImageButton ID="imgCheckSerial" runat="server" ImageUrl="~/img/checkbox_checked.png" Width="23px" Style="vertical-align: baseline;" OnClick="imgCheckSerial_Click" />
                                        </div>
                                    </div>
                                    <div class="col-sm-7 hide equatation">
                                        <div class="col-sm-2 div-caption">
                                            <asp:Label ID="lblEqtText" Text="" runat="server" />สินค้าเทียบเท่า</div>
                                        <div class="col-sm-5">
                                            <asp:DropDownList ID="dllEqtItems" runat="server" class="col-sm-12 js-example-basic-single js-states form-control hide"></asp:DropDownList>
                                        </div>
                                        <div class="col-sm-4 div-caption">
                                            <asp:TextBox ID="txtQuantityAndSerial" runat="server" class="col-sm-12" placeHolder="Serial No/Quantity"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-1">
                                            <asp:ImageButton ID="imgCheckSerialOrItem" runat="server" ImageUrl="~/img/checkbox_checked.png" ToolTip="จ่ายเทียบเท่าแบบให้ระบบเลือกสินค้าให้(ของเก่าสุดออกก่อน)" Width="23px" Style="vertical-align: baseline;" OnClick="imgEqtItems_Click" />
                                            <asp:ImageButton ID="ImageEqyByItem" runat="server" ToolTip="จ่ายเทียบเท่าแบบเลือกสินค้าเอง" ImageUrl="~/img/2check.png"   Width="40px" Style="vertical-align: baseline;" OnClick="ImageEqyByItem_Click"  />
                                         


                                        </div>
                                        
                                    </div>
                                </div>

                                <div class="col-sm-12">
                                    <asp:GridView ID="grdRequest" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" class="col-sm-12"
                                        EmptyDataText="No data." ShowHeaderWhenEmpty="True" OnRowCommand="grdRequest_RowCommand" OnRowDataBound="grdRequest_RowDataBound">
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            
                                            <%--<asp:CheckBoxField ID="SelectOpt"
                                                HeaderText="#" 
                                                HeaderStyle-HorizontalAlign="Center" 
                                                ItemStyle-HorizontalAlign="Center" 
                                                ItemStyle-Width="50px" />--%>
                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="SelectOpt" runat="server" CssClass="TableCheckBox" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ReqDocNo" HeaderText="เอกที่เอกสาร" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="123px" />
                                            <asp:BoundField DataField="ReqNo" HeaderText="เลขที่อ้างอิง" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="123px" />
                                            <asp:BoundField DataField="ItemName" HeaderText="สินค้าโปรโมชั่น" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="ReqAmount" HeaderText="จำนวน" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="91px" />
                                            <asp:BoundField DataField="Balance" HeaderText="คงเหลือเบิก" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="91px" />
                                            <asp:BoundField DataField="Transfer" HeaderText="จำนวนที่เบิก" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="91px" ItemStyle-ForeColor="Red" />
                                            <asp:BoundField DataField="ItemUnit" HeaderText="หน่วย" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="75px" Visible="false" />
                                            <asp:BoundField DataField="ProStartDate" HeaderText="วันที่ใช้งาน" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="123px" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField DataField="ProEndDate" HeaderText="วันที่สิ้นสุด" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="123px" DataFormatString="{0:dd/MM/yyyy}" />
                                            
                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgDelItem" runat="server" ImageUrl="~/img/delete.png" Width="23px" Style="vertical-align: baseline;" CommandName="DelItem" CommandArgument='<%# Eval("ReqId") %>' />
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

                                                    <asp:HiddenField ID="grdHdfFunction" runat="server" Value='<%# Eval("Function") %>' />

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
                                <asp:Button ID="btnTransferItemToProject" runat="server" class="btn btn-white btn-info btn-sm" Style="width: 123px;" Text="บันทึก" OnClick="btnTransferItemToProject_Click" />
                            </div>

                            <div class="col-sm-12 div-caption">&nbsp;</div>
                            <div class="col-sm-12" style="font-weight: bold; font-size: x-large">
                                ประวัติการจ่ายสินค้าให้กับโครงการ
                            </div>
                            <div class="col-sm-12">
                                <asp:GridView ID="grdTransferHistory" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" class="col-sm-12"
                                    EmptyDataText="No data." ShowHeaderWhenEmpty="True" OnRowCommand="grdTransferHistory_RowCommand" OnRowDataBound="grdTransferHistory_RowDataBound">
                                    <AlternatingRowStyle BackColor="White" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="41px">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgPrt" runat="server" ImageUrl="~/img/printer_and_fax.png" Width="23px" Style="vertical-align: baseline;" CommandName="PrtTrn" />&nbsp;&nbsp;
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ReqNo" HeaderText="เอกที่เอกสาร" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="center" />
                                        <asp:BoundField DataField="ItemName" HeaderText="สินค้าโปรโมชั่น" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="ItemAmount" HeaderText="จำนวน" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="91px" />
                                        <asp:BoundField DataField="CreateDate" HeaderText="วันที่" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="123px" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="FullName" HeaderText="ผู้ใช้งาน" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="234px" />
                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="41px">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgDel" runat="server" ImageUrl="~/img/delete.png" Width="23px" Style="vertical-align: baseline;" CommandName="DelTrn" CommandArgument='<%# Eval("TrListId").ToString() + "," + Eval("ReqId").ToString() %>' 
                                                    OnClientClick="return confirm('คุณต้องการลบ Transection การจ่ายสินค้าใช่หรือไม่ !?')" />
                                                <asp:HiddenField ID="grdHdfTrListId" runat="server" Value='<%# Eval("TrListId") %>' />
                                                <asp:HiddenField ID="grdHdfProject_Id" runat="server" Value='<%# Eval("Project_Id") %>' />
                                                <asp:HiddenField ID="grdHdfTrStatus" runat="server" Value='<%# Eval("TrStatus") %>' />
                                                <asp:HiddenField ID="grdHdfDelvAmt" runat="server" Value='<%# Eval("DelvAmt") %>' />
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
                                <asp:Button ID="btnBack" runat="server" class="btn btn-white btn-warning btn-sm" Style="width: 124px;" Text="<< กลับ" OnClick="btnBack_Click" />
                            </div>
                            <!-- รับค่าจาก StockTransferItemCheckAmount ว่าเอา ItemId ไหนบ้าง แล้วกดปุ่ม -->
                            <asp:HiddenField ID="hdfItemIdList" runat="server" Value="" />
                            <asp:Button ID="btnCheckItemByAmount" runat="server" Width="0px" Height="0px" Style="width: 0px; height: 0px; display: none; opacity: 0;" OnClick="btnCheckItemByAmount_Click" />


                             <asp:HiddenField ID="hdfItemIdListEqt" runat="server" Value="" />
                             <asp:HiddenField ID="hdfReqIdAmountEqt" runat="server" Value="" />
                             <asp:Button ID="btnCheckItemByAmountEqt" runat="server" Width="0px" Height="0px" Style="width: 0px; height: 0px; display: none; opacity: 0;" OnClick="btnCheckItemByAmountEqt_Click" />


                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
