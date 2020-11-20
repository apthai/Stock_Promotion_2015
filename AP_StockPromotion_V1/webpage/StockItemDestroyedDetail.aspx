<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="StockItemDestroyedDetail.aspx.cs" 
    Inherits="AP_StockPromotion_V1.webpage.StockItemDestroyedDetail"   MasterPageFile="~/Master/MasterPage.Master" %>

<asp:Content ID="Contenthead" ContentPlaceHolderID="head" runat="server">
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
            $('#divNavx').html('ตัดสต๊อกสูญเสีย >> รายละเอียด');
            $(".js-example-basic-single").select2({ width: '100%' });
            $('.js-example-basic-single-ws').select2({ minimumResultsForSearch: Infinity, width: '100%' });
            setTextNumericOnly();
            setTextDigitOnly();
            setHeightGrid();
            setTextDate();
        });

        function setTextDate() {
            $("#<%= txtPostingDate.ClientID %>").datepicker({ dateFormat: "dd/mm/yy" });
        }

        function setHeightGrid() {
            $('#divGrid').height(screen.height - 410);
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
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="col-sm-12">
        <div class="col-sm-5">
            <div class="col-sm-12">
                <div class="col-sm-3 div-caption">โครงการ</div>
                <div class="col-sm-9">
                    <asp:TextBox ID="txtProject" runat="server" Text="" class="col-sm-12" ReadOnly="true"></asp:TextBox>
                    <%--<asp:TextBox ID="txtProject" runat="server" Text=""></asp:TextBox>--%>
                </div>
            </div>
            <div class="col-sm-12">
                <div class="col-sm-3 div-caption">ผู้ดำเนินการ</div>
                <div class="col-sm-9">
                    <asp:TextBox ID="txtDestroyBy" runat="server" Text="" class="col-sm-12" ReadOnly="true"></asp:TextBox>
                    <%--<asp:TextBox ID="txtDestroyBy" runat="server" class="col-sm-12"></asp:TextBox>--%>
                </div>
            </div>
            <div class="col-sm-12">
                <div class="col-sm-3 div-caption">วันที่ดำเนินการ</div>
                <div class="col-sm-9">
                    <asp:TextBox ID="txtDestroyDate" runat="server" Text="" class="col-sm-12" ReadOnly="true"></asp:TextBox>
                   <%-- <asp:TextBox ID="txtDestroyDate" runat="server" class="col-sm-12"></asp:TextBox>--%>
                </div>
            </div>
            <div class="col-sm-12">
                <div class="col-sm-3 div-caption">SAP DocNo.</div>
                <div class="col-sm-9"><!--DL.OBJ_KEY, DL.OBJ_SYS, DL.OBJ_TYPE-->
                    <asp:TextBox ID="txtSapDocNo" runat="server" Text="" class="col-sm-12" ReadOnly="true"></asp:TextBox>
                    <asp:HiddenField ID="hdfOBJ_KEY" runat="server" />
                    <asp:HiddenField ID="hdfOBJ_SYS" runat="server" />
                    <asp:HiddenField ID="hdfOBJ_TYPE" runat="server" />
                </div>
            </div>
        </div>
        <div class="col-sm-7">
            <div class="col-sm-12">
                <div class="col-sm-3 div-caption">เหตุผล</div>
                <div class="col-sm-9">
                    <asp:TextBox ID="txtReason" runat="server" TextMode="MultiLine" Height="105px" class="col-sm-12" ReadOnly="true"></asp:TextBox>
                    <asp:Button ID="btnReasonEdit" runat="server" class="btn btn-white btn-info btn-sm" Text="แก้ไขเหตุผล" OnClick="btnReasonEdit_Click"  />
                    <asp:Button ID="btnReasonSave" runat="server" class="btn btn-white btn-info btn-sm" Text="บันทึกเหตุผล" OnClick="btnReasonSave_Click"  Visible ="false" />
                </div>
            </div>        
        </div>
    </div>



    <div class="col-sm-12"></div>
    <div class="col-sm-12">&nbsp;</div>

    <div class="col-sm-12" id="divGrid">
        <!--	
             						
            -->

        <asp:GridView ID="grdData" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" class="col-sm-12" 
            EmptyDataText="No data." ShowHeaderWhenEmpty="True">
            <AlternatingRowStyle BackColor="White" />
            <Columns>              
                <asp:BoundField DataField="Serial" HeaderText="Serial No." HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="ItemName" HeaderText="สินค้าโปรโมชั่น" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Model" HeaderText="รุ่น" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Color" HeaderText="สี" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Price" HeaderText="มูลค่า" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}" />
                <asp:BoundField DataField="ProduceDate" HeaderText="วันผลิต" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="ExpireDate" HeaderText="วันหมดอายุ" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="Detail" HeaderText="รายละเอียด" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Remark" HeaderText="หมายเหตุ" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />  
                <asp:BoundField DataField="TrListId" HeaderText="เลขที่ใบนำจ่าย" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="DestroyAmount" HeaderText="สูญเสีย" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />  
                
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
    <!--
        <asp:TemplateField HeaderText="ขนาด(กว้างxยาวxสูง)" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <asp:Label ID="grdLbDimension" runat="server" Text='<%# Eval("DimensionWidth") + " x " + Eval("DimensionLong") + " x " + Eval("DimensionHeight") + " " + Eval("DimensionUnit") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="น้ำหนัก" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <asp:Label ID="grdLbWeight" runat="server" Text='<%# Eval("Weight") + " " + Eval("WeightUnit") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        -->
    <div class="col-sm-12" style="text-align: right;">
        <div class="col-sm-5" >
            <div class="col-sm-4" style="text-align: right;">Posting Date</div>
            <div class="col-sm-4">
                <asp:TextBox ID="txtPostingDate" runat="server" ></asp:TextBox>
            </div>
        </div>
        <div class="col-sm-5">
            <div class="col-sm-4" style="text-align: right;">Reason</div>
            <div class="col-sm-4">
                <asp:DropDownList ID="ddlReverseReason" runat="server" class="col-sm-12 js-example-basic-single-ws js-states form-control hide"></asp:DropDownList>
            </div>
        </div>
        <div class="col-sm-2" ><asp:Button ID="btnCancelDestroy" runat="server" Text="ยกเลิกการตัดสต๊อกสูญเสีย" class="btn btn-white btn-warning btn-sm" OnClick="btnCancelDestroy_Click"  /></div>        
    </div>
</asp:Content>
