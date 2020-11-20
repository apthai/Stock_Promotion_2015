<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeliveryLowPriceList.aspx.cs"
    Inherits="AP_StockPromotion_V1.webpage.DeliveryLowPriceList" MasterPageFile="~/Master/MasterPage.Master" %>

<asp:Content ID="Contenthead" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .label-caption {
            text-align: right;
        }

        .div-caption {
            text-align: right;
            padding-right: 5px;
        }

        .right {
            text-align: right;
        }
        .left {
            text-align: left;
        }
        .ui-datepicker {
            z-index: 1150 !important;
        }
    </style>
    <script type="text/javascript">
        var date = new Date();
        tableManagementAccountSetting = [
            { "bSortable": false },
            { "bSortable": true, "sClass": "center" },
            { "bSortable": true, "sClass": "center", "sType": "date-uk" },
            { "bSortable": true, "sClass": "center" },
            { "bSortable": false, "sClass": "left" },
            { "bSortable": false, "sClass": "center" }
        ];
        jQuery(function ($) {
            $('#divNavx').html('บันทึกบัญชี(Marketing)');
            $(".js-example-basic-single").select2({ width: '100%' });
            $('.js-example-basic-single-ws').select2({ minimumResultsForSearch: Infinity, width: '100%' });

            setDatePicker();
            $('#accordion-style').on('click', function (ev) {
                var target = $('input', ev.target);
                var which = parseInt(target.val());
                if (which == 2) $('#accordion').addClass('accordion-style2');
                else $('#accordion').removeClass('accordion-style2');
            });

            $('#manageaccount-body').dataTable({
                sdom: 'Bfrtip',
                select: true,
                bAutoWidth: false,
                "aoColumns": tableManagementAccountSetting,
                "aaSorting": [],
                "bDestroy": true
            });

            jQuery.extend(jQuery.fn.dataTableExt.oSort, {
                "date-uk-pre": function (a) {
                    var ukDatea = a.split('/');
                    return (ukDatea[2] + ukDatea[1] + ukDatea[0]) * 1;
                },

                "date-uk-asc": function (a, b) {
                    return ((a < b) ? -1 : ((a > b) ? 1 : 0));
                },

                "date-uk-desc": function (a, b) {
                    return ((a < b) ? 1 : ((a > b) ? -1 : 0));
                }
            });

            $("#REVPSTDATE").datepicker({
                dateFormat: 'dd/mm/yy'
            });
        });

        $(document).ready(function () {
            $('#REVPSTDATE').val(date.getDate() + '/' + (date.getMonth() + 1) + '/' + date.getFullYear());
        });

        $(document)
                .on('click', '.accmanage-modal', function (e) {
                    $.ajax({
                        type: "POST",
                        url: "DeliveryLowPriceList.aspx/GetAccountingRecorded",
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        async: false,
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                                    textStatus + "\n\nError: " + errorThrown);
                        },
                        success: function (result) {
                            console.log(result.d);

                            var _data = [];
                            var sapid = '';
                            var pstdate = '';
                            var saprefid = '';
                            var docno = '';
                            var item = '';
                            var res = JSON.parse(result.d[0]);
                            var table1 = $('#manageaccount-body').DataTable();
                            table1.destroy();
                            $("#mngBody").empty();

                            var data = '';
                            if (res != '') {
                                $.each(res, function (k, Val) {
                                    console.log(Val);

                                    //sapid = Val.SAPID;
                                    if (sapid != Val.SAPID || sapid == '') {

                                        if (sapid != '') {
                                            _data.push({
                                                'sapid': sapid,
                                                'pstdate': pstdate,
                                                'docno': docno,
                                                'item': item
                                            });

                                            var dateString = _data[0].pstdate;
                                            var momentObj = moment(dateString, 'DD/MM/YYYY');
                                            var momentString = momentObj.format('DD/MM/YYYY');
                                            $('#manageaccount-body').dataTable().fnAddData([
                                                '',
                                                _data[0].sapid,
                                                momentString,
                                                _data[0].docno,
                                                _data[0].item,
                                                '    <td class="center"">' +
                                                '        <a data-toggle="modal" data-id="' + _data[0].sapid + '" title="" class="btn btn-xs btn-danger revert" href="#cancellation-modal" onclick="return false;">' +
                                                '            <i class="ace-icon fa fa-reply align-middle"></i>' +
                                                '        </a>' +
                                                '    </td>'
                                            ]);

                                            //$("#mngBody").append(data);
                                            _data = [];
                                            docno = '';
                                            item = '';
                                        }
                                        sapid = Val.SAPID;
                                        pstdate = Val.PSTDATE;
                                        docno += (Val.DOCNO + '<br />');
                                        item += (Val.ITEM + '<br />');
                                    }
                                    else if (sapid == Val.SAPID) {
                                        docno += (Val.DOCNO + '<br />');
                                        item += (Val.ITEM + '<br />');
                                    }
                                });
                                _data.push({
                                    'sapid': sapid,
                                    'pstdate': pstdate,
                                    'docno': docno,
                                    'item': item
                                });

                                var dateString = _data[0].pstdate;
                                var momentObj = moment(dateString, 'DD/MM/YYYY');
                                var momentString = momentObj.format('DD/MM/YYYY');
                                $('#manageaccount-body').dataTable().fnAddData([
                                    '',
                                    _data[0].sapid,
                                    momentString,
                                    _data[0].docno,
                                    _data[0].item,
                                    '    <td class="center">' +
                                    '        <a data-toggle="modal" data-id="' + _data[0].sapid + '" title="" class="btn btn-xs btn-danger revert" href="#cancellation-modal" onclick="return false;">' +
                                    '            <i class="ace-icon fa fa-reply align-middle"></i>' +
                                    '        </a>' +
                                    '    </td>'
                                ]);

                            }
                            $('#manageaccount-body').dataTable({
                                sdom: 'Bfrtip',
                                select: true,
                                bAutoWidth: false,
                                "aoColumns": tableManagementAccountSetting,
                                "aaSorting": [],
                                "bDestroy": true
                            });
                            $('#btnSearch').click();
                        }
                    });
                })
                .on('click', '.revert', function () {
                    var docid = $(this).data('id');
                    $('.modal-body #docId').val(docid);
                    $('#REMREV').val('');
                    $('#REVPSTDATE').val($(this)[0].parentElement.parentNode.cells[2].innerText);
                })
                .on('click', '.reverse-save', function (e) {
                    //validate REMREV REVPSTDATE REASREV
                    if ($('#REMREV').val() == '') {
                        bootbox.alert('กรุณาระบุ Remark');
                        return;
                    }
                    if ($('#REVPSTDATE').val() == '') {
                        bootbox.alert('กรุณาระบุ Posting Date');
                        return;
                    }
                    if ($('#REASREV').val() == '') {
                        bootbox.alert('กรุณาระบุ Reason');
                        return;
                    }


                    bootbox.confirm({
                        message: "คุณต้องการ Reverse Documents นี่หรือไม่?",
                        buttons: {
                            confirm: {
                                label: 'ใช่',
                                className: 'btn-success'
                            },
                            cancel: {
                                label: 'ไม่',
                                className: 'btn-danger'
                            }
                        },
                        callback: function (result) {
                            if (result) {
                                //alert($('#docId').val());


                                var data = JSON.stringify({
                                    docid: $('#docId').val(),
                                    remark: $('#REMREV').val(),
                                    postingDate: $('#REVPSTDATE').val(),
                                    reasonId: $('#REASREV option:selected').val(),
                                    reasonName: $('#REASREV option:selected').text(),
                                    empid: $('#lbUserId').text()
                                });

                                $.ajax({
                                    type: "POST",
                                    url: "DeliveryLowPriceList.aspx/ReversingAccountsIO",
                                    data: data,
                                    contentType: 'application/json; charset=utf-8',
                                    dataType: 'json',
                                    async: false,
                                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                                        alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                                                textStatus + "\n\nError: " + errorThrown);
                                    },
                                    success: function (result) {
                                        $('#cancellation-modal').modal('toggle');

                                        console.log(result.d);

                                        var _data = [];
                                        var sapid = '';
                                        var pstdate = '';
                                        var saprefid = '';
                                        var docno = '';
                                        var item = '';
                                        var sap_doc = result.d[0];
                                        try {
                                            var res = JSON.parse(result.d[1]);
                                        } catch (e) {
                                            bootbox.alert(result.d[0]);
                                            return;
                                        }

                                        var table1 = $('#manageaccount-body').DataTable();
                                        table1.destroy();
                                        $("#mngBody").empty();

                                        var data = '';
                                        if (res != "") {

                                            $.each(res, function (k, Val) {
                                                console.log(Val);

                                                //sapid = Val.SAPID;
                                                if (sapid != Val.SAPID || sapid == '') {

                                                    if (sapid != '') {
                                                        _data.push({
                                                            'sapid': sapid,
                                                            'pstdate': pstdate,
                                                            'docno': docno,
                                                            'item': item
                                                        });

                                                        var dateString = _data[0].pstdate;
                                                        var momentObj = moment(dateString, 'DD/MM/YYYY');
                                                        var momentString = momentObj.format('DD/MM/YYYY');
                                                        $('#manageaccount-body').dataTable().fnAddData([
                                                            '',
                                                            _data[0].sapid,
                                                            momentString,
                                                            _data[0].docno,
                                                            _data[0].item,
                                                            '    <td class="center"">' +
                                                            '        <a data-toggle="modal" data-id="' + _data[0].sapid + '" title="" class="btn btn-xs btn-danger revert" href="#cancellation-modal" onclick="return false;">' +
                                                            '            <i class="ace-icon fa fa-reply align-middle"></i>' +
                                                            '        </a>' +
                                                            '    </td>'
                                                        ]);

                                                        //$("#mngBody").append(data);
                                                        _data = [];
                                                        docno = '';
                                                        item = '';
                                                    }
                                                    sapid = Val.SAPID;
                                                    pstdate = Val.PSTDATE;
                                                    docno += (Val.DOCNO + '<br />');
                                                    item += (Val.ITEM + '<br />');
                                                }
                                                else if (sapid == Val.SAPID) {
                                                    docno += (Val.DOCNO + '<br />');
                                                    item += (Val.ITEM + '<br />');
                                                }
                                            });
                                            _data.push({
                                                'sapid': sapid,
                                                'pstdate': pstdate,
                                                'docno': docno,
                                                'item': item
                                            });

                                            var dateString = _data[0].pstdate;
                                            var momentObj = moment(dateString, 'DD/MM/YYYY');
                                            var momentString = momentObj.format('DD/MM/YYYY');
                                            $('#manageaccount-body').dataTable().fnAddData([
                                                '',
                                                _data[0].sapid,
                                                momentString,
                                                _data[0].docno,
                                                _data[0].item,
                                                '    <td class="center">' +
                                                '        <a data-toggle="modal" data-id="' + _data[0].sapid + '" title="" class="btn btn-xs btn-danger revert" href="#cancellation-modal" onclick="return false;">' +
                                                '            <i class="ace-icon fa fa-reply align-middle"></i>' +
                                                '        </a>' +
                                                '    </td>'
                                            ]);

                                        }
                                        $('#manageaccount-body').dataTable({
                                            sdom: 'Bfrtip',
                                            select: true,
                                            bAutoWidth: false,
                                            "aoColumns": tableManagementAccountSetting,
                                            "aaSorting": [],
                                            "bDestroy": true
                                        });
                                        $('#btnSearch').click();
                                        bootbox.alert('Reversed complete<br />Reversed Document No : ' + sap_doc);
                                    }
                                });
                            }
                        }
                    });
                })
            ;


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
            
            $('#<%= txtDateFrom.ClientID %>').datepicker({ dateFormat: "dd/mm/yy" }); // { dateFormat: "dd-mm-yy" }
            $('#<%= txtDateTo.ClientID %>').datepicker({ dateFormat: "dd/mm/yy" });
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- #section:elements.accordion -->
    <div id="accordion" class="accordion-style1 panel-group">
        <div class="panel panel-default">
            <div class="panel-heading">
                <h4 class="panel-title">
                    <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion" href="#collapseOne">
                        <i class="ace-icon fa fa-angle-down bigger-110" data-icon-hide="ace-icon fa fa-angle-down" data-icon-show="ace-icon fa fa-angle-right"></i>
                        &nbsp;รายการส่งมอบสินค้าโปรโมชั่น(Marketing)
                    </a>
                </h4>
            </div>

            <div class="panel-collapse collapse in" id="collapseOne">
                <div class="panel-body">
                    <div class="col-sm-12">
                        <div class="col-sm-2 div-caption">เลขที่เอกสารใบส่งมอบ</div>
                        <div class="col-sm-3">
                            <asp:TextBox ID="txtDeliveryNo" runat="server" placeholder="เลขที่เอกสาร" class="col-sm-12"></asp:TextBox>
                        </div>
                        <div class="col-sm-2 div-caption">โครงการ</div>
                        <div class="col-sm-3">
                            <asp:DropDownList ID="ddlProject" runat="server" class="col-sm-12 js-example-basic-single js-states form-control hide"></asp:DropDownList>
                        </div>
                        <div class="col-sm-2 div-caption"></div>
                    </div>
                    <div class="col-sm-12" style="padding-bottom: 5px">
                        <div class="col-sm-2 div-caption">สินค้าโปรโมชั่น</div>
                        <div class="col-sm-3">
                            <asp:DropDownList ID="ddlItem" runat="server" class="col-sm-12 js-example-basic-single js-states form-control hide"></asp:DropDownList>
                        </div>
                        <div class="col-sm-2 div-caption">วันที่ส่งมอบ</div>
                        <div class="col-sm-3">
                            <div class="col-sm-5" style="padding-left: 0; padding-right: 0;">
                                <asp:TextBox ID="txtDateFrom" runat="server" placeholder="วันที่เริ่มต้น" class="col-sm-12"></asp:TextBox>
                            </div>
                            <div class="col-sm-2" style="text-align: center;">-</div>
                            <div class="col-sm-5" style="padding-left: 0; padding-right: 0;">
                                <asp:TextBox ID="txtDateTo" runat="server" placeholder="ถึงวันที่" class="col-sm-12"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-2 div-caption"></div>
                    </div>

                    <div class="col-sm-12" style="padding-bottom: 5px">
                        <div class="col-sm-2 div-caption">สถานะ</div>
                        <div class="col-sm-2">
                            <asp:DropDownList id="cbxDelvStatus" runat="server" style="width: 100%">
                                <asp:ListItem value="0" Selected="True">-- ทั้งหมด --</asp:ListItem>
                                <asp:ListItem  value="1">รายการใหม่</asp:ListItem>
                                <asp:ListItem  value="2">เสร็จสิ้น</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-sm-8"></div>
                    </div>

                    <div class="col-sm-12" style="text-align: center;">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" class="btn btn-white btn-info btn-sm" Width="75px" OnClick="btnSearch_Click" />
                        &nbsp;
                        <asp:Button ID="btnClear" runat="server" Text="Clear" class="btn btn-white btn-info btn-sm" Width="75px" OnClick="btnClear_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- #accordion -->

    <div class="col-sm-12">
        <div class="col-sm-6 left">
            <asp:Button ID="btnPostAccountLowPrice" runat="server" Text="+ บันทึกบัญชี(Marketing)" class="btn btn-white btn-info btn-sm" Font-Bold="true" OnClick="btnPostAccountLowPrice_Click" />
        </div>
        <div class="col-sm-6 right">
            <a data-toggle="modal" data-id="" title="" class="btn btn-white btn-info btn-sm accmanage-modal" href="#accountmanagement-modal" onclick="return false;">
                <i class="ace-icon glyphicon glyphicon-cog"></i>จัดการเอกสารบันทึกบัญชี
            </a>
        </div>
    </div>

    <div class="col-sm-12">
        <asp:GridView ID="grdData" runat="server" AutoGenerateColumns="False" CellPadding="4" AllowPaging="true" AllowSorting="true" PageSize="20"
            ForeColor="#333333" GridLines="None" class="col-sm-12" OnRowDataBound="grdData_RowDataBound" OnPageIndexChanging="grdData_PageIndexChanging" OnSorting="grdData_Sorting" OnRowCommand="grdData_RowCommand">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="34px">
                    <ItemTemplate>
                        <asp:CheckBox ID="grdChkSelDelv" runat="server" />
                        <asp:HiddenField ID="grdHdfDelvLstId" runat="server" Value='<%# Eval("DelvLstId") %>' />
                        <asp:HiddenField ID="grdHdfisConfirm" runat="server" Value='<%# Eval("isConfirm") %>' />
                        <asp:HiddenField ID="grdHdfisPostAcc" runat="server" Value='<%# Eval("isPostAcc") %>' />
                        <asp:HiddenField ID="grdHdfDelvPromotionId" runat="server" Value='<%# Eval("DelvPromotionId") %>' />
                        <asp:HiddenField ID="grdHdfDelvDate" runat="server" Value='<%# Eval("DelvDate") %>' />
                        <asp:HiddenField ID="grdHdfItemId" runat="server" Value='<%# Eval("ItemId") %>' />
                        <asp:HiddenField ID="grdHdfItemName" runat="server" Value='<%# Eval("ItemName") %>' />
                        <asp:HiddenField ID="grdHdfAmount" runat="server" Value='<%# Eval("Amount") %>' />
                        <asp:HiddenField ID="grdHdfPostRetKey" runat="server" Value='<%# Eval("PostRetKey") %>' />
                        <asp:HiddenField ID="grdHdfProjectCode" runat="server" Value='<%# Eval("ProjectCode") %>' />
                        <asp:HiddenField ID="grdHdfCompanySAPCode" runat="server" Value='<%# Eval("CompanySAPCode") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="DelvPromotionId" SortExpression="DelvPromotionId" HeaderText="เลขที่เอกสารใบส่งมอบ" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="141px" />
                <asp:BoundField DataField="ProjectName" HeaderText="โครงการ" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="WBS" HeaderText="WBS" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="159px" Visible="false" />
                <asp:BoundField DataField="ItemName" SortExpression="ItemName" HeaderText="สินค้าโปรโมชั่น" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Amount" SortExpression="Amount" HeaderText="จำนวน" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="71px" />
                <asp:BoundField DataField="DelvDate" SortExpression="DelvDate" HeaderText="วันที่ส่งมอบ" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="123px" />
                <asp:BoundField DataField="CostCenter" SortExpression="CostCenter" HeaderText="Internal Order" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="123px" />
                <asp:BoundField DataField="PostRetKey" SortExpression="PostRetKey" HeaderText="SAP Document No." HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="123px" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton ID="imgPrt" runat="server" CommandName="PrtDoc" CommandArgument='<%# Eval("PostRet_Key") %>' ImageUrl="~/img/printer_and_fax.png" Width="23px" />
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
    <%----%>
    <div class="col-sm-12" style="text-align: right;">&nbsp;</div>


    <div class="modal fade" id="accountmanagement-modal" tabindex="-1" role="dialog" aria-labelledby="accountmanagement-modal-label">
        <div class="modal-lg modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="accountmanagement-modal-label">Reverse Data Documents</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <table id="manageaccount-body" class="table table-hover">
                            <thead>
                                <tr>
                                    <th style="width: 150px"></th>
                                    <th class="center" style="width: 140px;">SAPDOCNO</th>
                                    <th class="center" style="">Posting Date</th>
                                    <th class="center" style="">DOCNO</th>
                                    <th class="center" style="">ITEM</th>
                                    <th class="center" style="width: 50px">Reverse</th>
                                </tr>
                            </thead>
                            <tbody id="mngBody">
                            </tbody>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="cancellation-modal" tabindex="-1" role="dialog" aria-labelledby="cancellation-modal-label">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="cancellation-modal-label">Reverse Documents</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <input type="text" class="hide" name="docId" id="docId" value="" />
                        <div class="col-sm-3 right">
                            <p>Remark :</p>
                        </div>
                        <div class="col-sm-9">
                            <textarea id="REMREV" rows="1" cols="10" style="width: 100% !important"></textarea>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-3 right">
                            <p>Posting Date :</p>
                        </div>
                        <div class="col-md-3">
                            <div class="input-group input-group-sm">
                                <input type="text" id="REVPSTDATE" class="form-control" placeholder="" />
                                <span class="input-group-addon">
                                    <i class="ace-icon fa fa-calendar"></i>
                                </span>
                            </div>
                        </div>
                        <div class="col-md-2 right">
                            <p>Reason :</p>
                        </div>
                        <div class="col-md-4">
                            <select class="form-control" id="REASREV">
                                <option value="01">ภายในเดือน</option>
                                <option value="02">ข้ามเดือน</option>
                            </select>
                        </div>

                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    <button type="button" class="btn btn-primary reverse-save">Save</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
