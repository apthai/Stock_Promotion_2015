<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockTransferDetail.aspx.cs"
    Inherits="AP_StockPromotion_V1.web.StockTransferDetail" MasterPageFile="~/Master/MasterPopup.Master" %>

<asp:Content ID="Contenthead" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
    </style>
    <script type="text/javascript">

        function popupTransferStockDetail(mode, ItemCountMethod) {
            Popup80('StockTransferDetail.aspx?mode=' + mode + '&ItemCountMethod=' + ItemCountMethod);
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
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="col-sm-12">
        <div id="div1" runat="server" class="col-sm-12" style="display: none;">
            <div class="col-sm-12" style="font-weight: bold;">
                <div class="col-sm-12" style="font-weight: bold;">
                    <div class="col-sm-2" style="text-align: right;">
                        สินค้าโปรโมชั่น: 
                    </div>
                    <div class="col-sm-4">
                        <asp:Label ID="Label4" runat="server" Text="iPhone6 16GB มูลค่า 25,500 บาท"></asp:Label>
                    </div>
                    <div class="col-sm-6" style="text-align: right;">
                        &nbsp;
                    </div>
                </div>
                <div class="col-sm-12" style="font-weight: bold;">
                    <div class="col-sm-2" style="text-align: right;">
                        คลังต้นทาง: 
                    </div>
                    <div class="col-sm-2">
                        <asp:Label ID="Label5" runat="server" Text="Centro รามอินทรา 109"></asp:Label>
                    </div>
                    <div class="col-sm-2" style="text-align: right;">
                        คลังปลายทาง: 
                    </div>
                    <div class="col-sm-2">
                        <asp:Label ID="Label6" runat="server" Text="Centro พระราม 9"></asp:Label>
                    </div>
                </div>
            </div>

            <div class="col-sm-12" style="text-align: right; border-top: 1px solid #808080;">&nbsp;</div>
            
            <div class="col-sm-12" id="divinput1" runat="server">
            <div class="col-sm-2">
                        <asp:Label ID="Label10" runat="server" Text="Serail No.: "></asp:Label>
            </div>
            <div class="col-sm-4">
                <asp:TextBox ID="txtSerial" runat="server" placeholder="SN  :800101255236320035" class="col-sm-12"></asp:TextBox>
            </div>
            <div class="col-sm-6">
                <asp:ImageButton ID="imgCheck1" runat="server" ImageUrl="~/img/check.png" Width="23px" Style="vertical-align: baseline;" /><%-- OnClick="imgCheck1_Click"--%>
            </div></div>
        </div>
        <div id="div2" runat="server" class="col-sm-12" style="display: none;">
            <div class="col-sm-12" style="font-weight: bold;">
                <div class="col-sm-12" style="font-weight: bold;">
                    <div class="col-sm-2" style="text-align: right;">
                        สินค้าโปรโมชั่น: 
                    </div>
                    <div class="col-sm-4">
                        <asp:Label ID="Label1" runat="server" Text="Gift Voucher Starbuck มูลค่า 3000 บาท"></asp:Label>
                    </div>
                    <div class="col-sm-6" style="text-align: right;">
                        &nbsp;
                    </div>
                </div>
                <div class="col-sm-12" style="font-weight: bold;">
                    <div class="col-sm-2" style="text-align: right;">
                        คลังต้นทาง: 
                    </div>
                    <div class="col-sm-2">
                        <asp:Label ID="Label2" runat="server" Text="Centro รามอินทรา 109"></asp:Label>
                    </div>
                    <div class="col-sm-2" style="text-align: right;">
                        คลังปลายทาง: 
                    </div>
                    <div class="col-sm-2">
                        <asp:Label ID="Label3" runat="server" Text="Centro พระราม 9"></asp:Label>
                    </div>
                </div>
            </div>

            <div class="col-sm-12" style="text-align: right; border-top: 1px solid #808080;">&nbsp;</div>
            
            <div class="col-sm-12" id="divinput2" runat="server">
            <div class="col-sm-2">
                        <asp:Label ID="Label8" runat="server" Text="ลำดับเริ่มต้น: "></asp:Label>
            </div>
            <div class="col-sm-2">
                <asp:TextBox ID="txtSerialBeg" runat="server" placeholder="SN Start :8000000000000001" class="col-sm-12"></asp:TextBox>
            </div>
            <div class="col-sm-2">
                        <asp:Label ID="Label9" runat="server" Text="ลำดับสิ้นสุด: "></asp:Label>
            </div>
            <div class="col-sm-2">
                <asp:TextBox ID="txtSerialEnd" runat="server" placeholder="SN End   :8000000000000100" class="col-sm-12"></asp:TextBox>
            </div>
            <div class="col-sm-4">
                <asp:ImageButton ID="imgCheck2" runat="server" ImageUrl="~/img/check.png" Width="23px" Style="vertical-align: baseline;" /><%--OnClick="imgCheck2_Click"--%>
            </div></div>
        </div>
        <div id="div3" runat="server" class="col-sm-12" style="display: none;">

            <div class="col-sm-12" style="font-weight: bold;">
                <div class="col-sm-12" style="font-weight: bold;">
                    <div class="col-sm-2" style="text-align: right;">
                        สินค้าโปรโมชั่น: 
                    </div>
                    <div class="col-sm-4">
                        <asp:Label ID="lbItem" runat="server" Text="สร้อยคอทองคำ หนัก 5 บาท"></asp:Label>
                    </div>
                    <div class="col-sm-6" style="text-align: right;">
                        &nbsp;
                    </div>
                </div>
                <div class="col-sm-12" style="font-weight: bold;">
                    <div class="col-sm-2" style="text-align: right;">
                        คลังต้นทาง: 
                    </div>
                    <div class="col-sm-2">
                        <asp:Label ID="lbStockSource" runat="server" Text="Centro รามอินทรา 109"></asp:Label>
                    </div>
                    <div class="col-sm-2" style="text-align: right;">
                        คลังปลายทาง: 
                    </div>
                    <div class="col-sm-2">
                        <asp:Label ID="lbStockDest" runat="server" Text="Centro พระราม 9"></asp:Label>
                    </div>
                </div>
            </div>

            <div class="col-sm-12" style="text-align: right; border-top: 1px solid #808080;">&nbsp;</div>

            <div class="col-sm-12" id="divinput3" runat="server">
            <div class="col-sm-2">
                        <asp:Label ID="Label7" runat="server" Text="จำนวน: "></asp:Label>
            </div>
            <div class="col-sm-4">
                <asp:TextBox ID="txtAmount" runat="server" placeholder="Amount  :   100" class="col-sm-12"></asp:TextBox>
            </div>
            <div class="col-sm-6">
                <asp:ImageButton ID="imgCheck3" runat="server" ImageUrl="~/img/check.png" Width="23px" Style="vertical-align: baseline;" /><%--OnClick="imgCheck3_Click"--%>
            </div></div>
        </div>
    </div>

    <div class="col-sm-12" style="text-align: right;"></div>

    <div class="col-sm-12">
        <asp:GridView ID="grdData" runat="server" AutoGenerateColumns="False" CellPadding="4"
            ForeColor="#333333" GridLines="None" class="col-sm-12" OnRowDataBound="grdData_RowDataBound">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="SerialNo" HeaderText="Serial No." />
                <asp:TemplateField HeaderText="Barcode">
                    <ItemTemplate>
                        <asp:Label ID="grdtxtBarcode" runat="server" placeholder="Barcode"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ชื่อสินค้า">
                    <ItemTemplate>
                        <asp:Label ID="ItemName" runat="server" placeholder="ชื่อสินค้า"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="หน่วยนับ">
                    <ItemTemplate>
                        <asp:Label ID="grdtxtItemUnit" runat="server" placeholder="หน่วยนับ"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="วันหมดอายุ">
                    <ItemTemplate>
                        <asp:Label ID="grdtxtExpireDate" runat="server" placeholder="วันหมดอายุ"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="วันหมดประกัน">
                    <ItemTemplate>
                        <asp:Label ID="grdtxtEndOfWarranty" runat="server" placeholder="สิ้นสุดประกัน"></asp:Label>
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
    </div>


    <div class="col-sm-12" style="text-align: right;"></div>
    <div class="col-sm-12" style="text-align: right;" id="div_btnAccept" runat="server">
        <button class="btn btn-white btn-info btn-sm">
            <i class="ace-icon fa fa-exchange bigger-120 blue"></i>ตกลง
        </button>
        &nbsp;&nbsp;
        <button class="btn btn-white btn-warning btn-sm">
            <i class="ace-icon fa fa-times bigger-120 orange"></i>ยกเลิก
        </button>
    </div>
</asp:Content>
