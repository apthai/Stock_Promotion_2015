<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeliveryPostSumCrossCmpPrint.aspx.cs"
    Inherits="AP_StockPromotion_V1.webpage.DeliveryPostSumCrossCmpPrint"
    EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <style type="text/css">
        .label-caption {
            text-align: right;
        }

        .div-caption {
            text-align: right;
            padding-right: 5px;
        }
    </style>

    <script src="../ace/assets/js/jquery.js"></script>
    <script src="../ace/assets/js/jquery-ui.js"></script>
    <script src="../ace/jquery-2.1.3.min.js"></script>

    <title></title>
</head>
<body>
    <form id="form1" runat="server" style="font-family:AngsanaUPC; font-size:9px;">

        <asp:HiddenField ID="hdfCompanySAPCode" runat="server" />
        <asp:HiddenField ID="hdfProfitHO" runat="server" />

        <div class="col-sm-12" style="font-weight: bold; font-size: 1.12em; text-align: center">
            รายงานแสดงข้อมูลงบประมาณ IO
        </div>

        <div class="col-sm-12">
            <div class="col-sm-4 div"></div>
            <div class="col-sm-4 div">
                <div class="col-sm-5" style="text-align: right;padding-right: 5px;">Posting Date</div>
                <div class="col-sm-7">
                    <asp:TextBox ID="txtDate" runat="server" class="col-sm-12" placeholder="dd/MM/yyyy"></asp:TextBox>
                </div>
            </div>
            <div class="col-sm-4 div">
                <div class="col-sm-5" style="text-align: right;padding-right: 5px;">Reference</div>
                <div class="col-sm-7">
                    <asp:TextBox ID="txtReference" runat="server" class="col-sm-12" MaxLength="16"></asp:TextBox>
                </div>
            </div>
        </div>

        <div class="col-sm-12">&nbsp;</div>

        <div class="col-sm-12">
            <div class="col-sm-12" style="font-weight: bold;">
                สรุปรายละเอียดตามค่าใช้จ่าย WBS
            </div>
            <asp:GridView ID="grdData" runat="server" AutoGenerateColumns="False" class="col-sm-12">
                <Columns>
                    <asp:BoundField DataField="ItemNo" HeaderText="เลขที่สินค้าโปรโมชั่น" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="141px">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Center" Width="141px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="ItemName" HeaderText="สินค้าโปรโมชั่น" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Amount" HeaderText="จำนวน" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="123px">
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Right" Width="123px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Total" HeaderText="มูลค่ารวม" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="123px" DataFormatString="{0:#,##0.00}">
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Right" Width="123px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="CostCenter" HeaderText="Cost Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="123px">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Center" Width="123px"></ItemStyle>
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
        </div>

        <div class="col-sm-12">&nbsp;</div>
        <div class="col-sm-12">
            <div class="col-sm-12" style="font-weight: bold;">
                บันทึกบัญชีตั้งเจ้าหนี้
            </div>
            <asp:GridView ID="grdAccountsPayable" runat="server" AutoGenerateColumns="False" class="col-sm-12">
                <Columns>
                    <asp:BoundField DataField="GLNo" HeaderText="GL No." HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="123px">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Center" Width="123px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="GLName" HeaderText="GL Name" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="profitCenter" HeaderText="Profit Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="123px">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Center" Width="123px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="debit" HeaderText="Debit" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}" ItemStyle-Width="123px">
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Right" Width="123px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="credit" HeaderText="Credit" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}" ItemStyle-Width="123px">
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Right" Width="123px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="itemtext" HeaderText="Item Text" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="191px">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Left" Width="191px"></ItemStyle>
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
        </div>

        <div class="col-sm-12">&nbsp;</div>
        <div class="col-sm-12">
            <div class="col-sm-12" style="font-weight: bold;">
                บันทึกบัญชีตัดสต๊อก (WBS)
            </div>
            <asp:GridView ID="grdDataPostAccount" runat="server" AutoGenerateColumns="False" class="col-sm-12">
                <Columns>
                    <asp:BoundField DataField="GLNo" HeaderText="GL No." HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="123px">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Center" Width="123px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="GLName" HeaderText="GL Name" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="WBS_SAP" HeaderText="WBS" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="ProfitCenter" HeaderText="Profit Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="123px">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Center" Width="123px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Debit" HeaderText="Debit" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:#,##0.00}" ItemStyle-Width="123px">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Center" Width="123px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Credit" HeaderText="Credit" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:#,##0.00}" ItemStyle-Width="123px">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Center" Width="123px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="ItemText" HeaderText="Item Text" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="191px">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Left" Width="191px"></ItemStyle>
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
        </div>

        <div class="col-sm-12">&nbsp;</div>
        <div class="col-sm-12">
            <div class="col-sm-12" style="font-weight: bold;">
                AP บันทึกบัญชีตั้งลูกหนี้
            </div>
            <asp:GridView ID="grdAccountsReceivable" runat="server" AutoGenerateColumns="False" class="col-sm-12">
                <Columns>
                    <asp:BoundField DataField="GLNo" HeaderText="GL No." HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="123px">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Center" Width="123px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="GLName" HeaderText="GL Name" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="profitCenter" HeaderText="Profit Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="123px">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Center" Width="123px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="debit" HeaderText="Debit" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}" ItemStyle-Width="123px">
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Right" Width="123px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="credit" HeaderText="Credit" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,##0.00}" ItemStyle-Width="123px">
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Right" Width="123px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="itemtext" HeaderText="Item Text" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="191px">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Left" Width="191px"></ItemStyle>
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
        </div>

        <div class="col-sm-12">&nbsp;</div>
        <div class="col-sm-12" style="text-align: right;padding-right: 5px;">
            <asp:Button ID="btnPrint" runat="server" Text="พิมพ์" class="btn btn-white btn-info btn-sm" Font-Bold="true" OnClick="btnPrint_Click" />
        </div>

    </form>
</body>
</html>
