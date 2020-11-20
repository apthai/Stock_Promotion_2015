<%@ Page Title="บันทึกบัญชี(Cost Center)" Language="C#" MasterPageFile="~/master/MasterPage.Master"
    AutoEventWireup="true" CodeBehind="CostCenterACRecord.aspx.cs" Inherits="AP_StockPromotion_V1.webpage.CostCenterACRecord" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        textarea {
            width: 90%;
            height: 100px;
        }

        .div-caption {
            text-align: right;
            padding-right: 5px;
        }

        .ui-dialog {
            top: 50px !important;
        }

        .align-right {
            text-align: right;
        }

        .max-width {
            width: 100% !important;
        }

        .width-ninety {
            width: 90%;
        }

        .margin-bottom-menu {
            margin-bottom: 5px;
        }

        .set-padding {
            padding: 0 0 0 0;
        }

        .set-black {
            color: black;
        }

        .move-center {
            text-align: center;
        }

        .ui-datepicker {
            z-index: 1150 !important;
        }

        .right {
            text-align: right;
        }

        .left {
            text-align: left;
        }

        .iframe-container {
            padding-bottom: 60%;
            padding-top: 30px;
            height: 0;
            overflow: hidden;
        }

            .iframe-container iframe,
            .iframe-container object,
            .iframe-container embed {
                position: absolute;
                top: 0;
                left: 0;
                width: 100%;
                height: 100%;
            }

        .modal-fixed-height {
            height: calc(100vh - 125px);
        }
    </style>

    <script type="text/javascript">
        var tableSetting, titleText, source, mode, crossCompany;
        tableSetting = [{ "bSortable": false }, null, null, null, null, null, null, null, null];
        tableManagementAccountSetting = [
            { "bSortable": false },
            { "bSortable": true, "sClass": "center" },
            { "bSortable": true, "sClass": "center" },
            { "bSortable": true, "sClass": "center" },
            { "bSortable": false, "sClass": "left" },
            { "bSortable": false, "sClass": "center" },
            { "bSortable": false, "sClass": "center" }
        ];

        mode = 1;

        var a1, a2, a3, a4;
        var b1, b2, b3, b4;

        var dedtor = 0;

        (function (a) {
            a.createModal = function (b) {

                defaults = {
                    title: "Memo Report", message: "", closeButton: true, scrollable: false
                };

                var b = a.extend({}, defaults, b);
                var c = (b.scrollable === true) ? 'style="max-height: 450px;overflow-y: auto;"' : "";
                html = '<div class="modal fade" id="myModal" style="z-index: 9999999 !important;">';
                html += '<div class="modal-dialog modal-lg">';
                html += '<div class="modal-content">';
                html += '<div class="modal-header">';
                html += '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>';

                if (b.title.length > 0) {
                    html += '<h4 class="modal-title">' + b.title + "</h4>";
                }

                html += "</div>";
                html += '<div class="modal-body modal-fixed-height" ' + c + ">";
                html += b.message; html += "</div>";

                html += "</div>";
                html += "</div>";
                html += "</div>";
                a("body").prepend(html);
                a("#myModal").modal().on("hidden.bs.modal", function () {
                    a(this).remove();
                });
            };
        })(jQuery);

        jQuery(function ($) {

            CallDatatable();
            $('#manageaccount-body').dataTable({
                sdom: 'Bfrtip',
                select: true,
                bAutoWidth: false,
                "aoColumns": tableManagementAccountSetting,
                "aaSorting": [],
                "bDestroy": true
            });
            GetPromotionItems();

            $("#STDDATE").datepicker({
                showOtherMonths: true,
                selectOtherMonths: false,
                dateFormat: 'dd/mm/yy'
            });

            $("#ENDDATE").datepicker({
                showOtherMonths: true,
                selectOtherMonths: false,
                dateFormat: 'dd/mm/yy'
            });

            $("#PSTDATE").datepicker({
                showOtherMonths: true,
                selectOtherMonths: false,
                dateFormat: 'dd/mm/yy'
            });

            $("#CROSSPSTDATE").datepicker({
                showOtherMonths: true,
                selectOtherMonths: false,
                dateFormat: 'dd/mm/yy'
            });

            $("#REVPSTDATE").datepicker({
                showOtherMonths: true,
                selectOtherMonths: false,
                dateFormat: 'dd/mm/yy'
            });

            $.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
                _title: function (title) {
                    var $title = this.options.title || '&nbsp;'
                    if (("title_html" in this.options) && this.options.title_html == true)
                        title.html($title);
                    else title.text($title);
                }
            }));

            $('#modal-form').on('shown.bs.modal', function () {
                if (!ace.vars['touch']) {
                    $(this).find('.chosen-container').each(function () {
                        $(this).find('a:first-child').css('width', '210px');
                        $(this).find('.chosen-drop').css('width', '210px');
                        $(this).find('.chosen-search input').css('width', '200px');
                    });
                }
            });

            if (!ace.vars['touch']) {
                $('.chosen-select').chosen({ allow_single_deselect: true });

                $(window)
                    .off('resize.chosen')
                    .on('resize.chosen', function () {
                        $('.chosen-select').each(function () {
                            var $this = $(this);
                            $this.next().css({ 'width': 100 + '%' });
                        });
                    }).trigger('resize.chosen');
                $(document).on('settings.ace.chosen', function (e, event_name, event_val) {
                    if (event_name != 'sidebar_collapsed') return;
                    $('.chosen-select').each(function () {
                        var $this = $(this);
                        $this.next().css({ 'width': 100 + '%' });
                    });
                });
            }
        });

        function GetPromotionItems() {

            $.ajax({
                type: "POST",
                url: "CostCenterACRecord.aspx/GetListDataPromotionItems",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                        textStatus + "\n\nError: " + errorThrown);
                },
                success: function (result) {
                    $('#cbxPromotion').append('<option value="All"></option>')
                    $(result.d.Data).each(function (k, v) {
                        //console.log(v);
                        var option = '<option value="' + v.ItemNO + '">' + v.ItemName + '</option>';
                        $('#cbxPromotion').append(option);
                    });
                    $('#cbxPromotion').select2({

                        allowClear: true
                    });
                    $("#cbxPromotion").select2('val', 'All');
                }
            });

        }

        function CallDatatable() {

            var oTable1 = $("#dynamic-table").dataTable({
                sdom: 'Bfrtip',
                select: true,
                bAutoWidth: false,
                "aoColumns": tableSetting,
                "aaSorting": [],
                "ordering": false
            });

            $('th input[type=checkbox], td input[type=checkbox]').prop('checked', false);

            $('#dynamic-table > thead > tr > th input[type=checkbox]').eq(0).on('click', function () {
                var th_checked = this.checked;

                $(this).closest('table').find('tbody > tr').each(function () {
                    var row = this;
                    if (th_checked) tableTools_obj.fnSelect(row);
                    else tableTools_obj.fnDeselect(row);
                });
            });

            $(document).on('click', '#dynamic-table .dropdown-toggle', function (e) {
                e.stopImmediatePropagation();
                e.stopPropagation();
                e.preventDefault();
            });
        }

        $(document).ready(function () {
            var date = new Date();

            $('#PSTDATE').val(date.getDate() + '/' + (date.getMonth() + 1) + '/' + date.getFullYear());
            $('#CROSSPSTDATE').val(date.getDate() + '/' + (date.getMonth() + 1) + '/' + date.getFullYear());
            $('#REVPSTDATE').val(date.getDate() + '/' + (date.getMonth() + 1) + '/' + date.getFullYear());

            $('#btnSearch').on('click', function (e) {
                var docid = $('#docID').val();
                var itemno = $('#cbxPromotion').val();
                var stddate = $('#STDDATE').val();
                var enddate = $('#ENDDATE').val();
                var IsPostAcc = $('#cbxDelvStatus').val();

                var dataValue = JSON.stringify({ docid: docid, itemno: itemno, stddate: stddate, enddate: enddate, IsPostAcc: IsPostAcc });

                $.ajax({
                    type: "POST",
                    url: "CostCenterACRecord.aspx/GetListDataMasterItems",
                    data: dataValue,
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                            textStatus + "\n\nError: " + errorThrown);
                    },
                    success: function (result) {
                        localStorage.clear();
                        $("#iBody").empty();
                        if ($.fn.dataTable.isDataTable('#dynamic-table')) {
                            table = $('#dynamic-table').DataTable({
                                retrieve: true,
                                paging: false
                            });
                            table.destroy();
                        }
                        $("#iBody").empty();
                        $.each(result, function (k, Val) {
                            $.each(Val, function (key, Value) {

                                var data = RenderDatatable(Value);
                                $("#iBody").append(data);

                            });
                        });
                        CallDatatable();
                    }
                });

            });

            $("#btnAdd").on('click', function (e) {
                $('#PSTDATE').val(date.getDate() + '/' + (date.getMonth() + 1) + '/' + date.getFullYear());

                $('#CROSSPSTDATE').val(date.getDate() + '/' + (date.getMonth() + 1) + '/' + date.getFullYear());

                if (GetItems()) {
                    e.preventDefault();

                    //console.log('-x--x--x-');
                    //console.log(crossCompany);
                    //console.log('-x--x--x-');

                    if (!crossCompany) {
                        var dialog = $("#dialog-message").removeClass('hide').dialog({
                            width: screen.width / 100 * 80,
                            modal: true,
                            title: "<div class='widget-header widget-header-small' id='iHeader'> " +
                                "<h4 class='smaller'><i class='ace-icon glyphicon glyphicon-plus'></i> Accounting Record (Cost Center)</h4></div>",
                            title_html: true,

                            buttons: [
                                {
                                    text: "บันทึกบัญชี",
                                    "class": "btn btn-primary btn-minier",
                                    click: function () {

                                        if ($('#ref-doc').val() == "" || $('#ref-text').val() == "" || $('#PSTDATE').val() == "") {
                                            bootbox.alert("Please fill Posting date, Reference and Ref Key 3.");
                                            return;
                                        }

                                        var strID = [];
                                        var _dt = $('#dynamic-table').dataTable();

                                        $("input:checked", _dt.fnGetNodes()).each(function () {
                                            //console.log($(this).val());
                                            strID.push({
                                                "ID": $(this).val()
                                            });
                                        });

                                        if (strID.length > 0) {
                                            mode = 0;
                                            var ret = LoadItems(strID);
                                            if (ret) {
                                                $('#btnSearch').click();
                                                $('#dialog-message').dialog('close');
                                            }
                                            $('#ref-doc').focusin();
                                            return ret;
                                        }
                                        else {
                                            return false;
                                        }

                                        $(this).dialog("close");
                                    }
                                }
                            ]
                        });
                    } else {
                        var dialog = $("#cross-company").removeClass('hide').dialog({
                            width: screen.width / 100 * 80,
                            modal: true,
                            title: "<div class='widget-header widget-header-small' id='iHeader'> " +
                                "<h4 class='smaller'><i class='ace-icon glyphicon glyphicon-plus'></i> Accounting Record (Cross Company)</h4></div>",
                            title_html: true,

                            buttons: [
                                {
                                    text: "บันทึกบัญชี",
                                    "class": "btn btn-primary btn-minier",
                                    click: function () {

                                        if ($('#cross-refDoc').val() == "" || $('#cross-refText').val() == "" || $('#CROSSPSTDATE').val() == "") {
                                            bootbox.alert("Please fill Posting date, Reference and Ref Key 3.");
                                            return;
                                        }

                                        var strID = [];
                                        var _dt = $('#dynamic-table').dataTable();
                                        $("input:checked", _dt.fnGetNodes()).each(function () {
                                            console.log($(this).val());
                                            strID.push({
                                                "ID": $(this).val()
                                            });
                                        });

                                        if (strID.length > 0) {
                                            mode = 0;
                                            var ret = LoadItems(strID);

                                            $('#btnSearch').click();
                                            $(this).dialog("close");
                                        }
                                        else {
                                            $(this).dialog("close");
                                        }
                                    }
                                }
                            ]
                        });
                    }
                }
            });

            $('#clear').on('click', function (e) {
                $('#docID').val('');

                $("#cbxPromotion").select2('val', 'All');
                $('#cbxDelvStatus').val('0').trigger("change");
            });

            $('#btnSearch').click();

            $(document)
                .on('click', '.accmanage-modal', function (e) {

                    $.ajax({
                        type: "POST",
                        url: "CostCenterACRecord.aspx/GetAccountingRecorded",
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                                textStatus + "\n\nError: " + errorThrown);
                        },
                        success: function (result) {
                            //console.log(result.d);

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
                            if (res !== '') {
                                $.each(res, function (k, Val) {
                                    //console.log(Val);

                                    //sapid = Val.SAPID;
                                    if (sapid !== Val.SAPID || sapid === '') {

                                        if (sapid !== '') {
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
                                                '    </td>',
                                                '    <td class="center">' +
                                                '        <a data-toggle="modal" data-id="' + _data[0].sapid + '" title="" class="btn btn-xs btn-primary print" onclick="return false;">' +
                                                '            <i class="ace-icon fa fa-print align-middle"></i>' +
                                                '        </a>' +
                                                '    </td>'
                                            ]);

                                            _data = [];
                                            docno = '';
                                            item = '';
                                        }

                                        sapid = Val.SAPID;
                                        pstdate = Val.PSTDATE;
                                        docno += Val.DOCNO + '<br />';
                                        item += Val.ITEM + '<br />';
                                    }
                                    else if (sapid === Val.SAPID) {
                                        docno += Val.DOCNO + '<br />';
                                        item += Val.ITEM + '<br />';
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
                                    '    </td>',
                                    '    <td class="center">' +
                                    '        <a data-toggle="modal" data-id="' + _data[0].sapid + '" title="" class="btn btn-xs btn-primary print" onclick="return false;">' +
                                    '            <i class="ace-icon fa fa-print align-middle"></i>' +
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
                                    url: "CostCenterACRecord.aspx/ReversingAccounts",
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
                                                //console.log(Val);

                                                //sapid = Val.SAPID;
                                                if (sapid != Val.SAPID || sapid == '') {

                                                    if (sapid != '') {
                                                        _data.push({
                                                            'sapid': sapid,
                                                            'pstdate': pstdate,
                                                            'docno': docno,
                                                            'item': item
                                                        });
                                                        //data = RenderAccManagemenr(_data[0]);

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
                                                            '    </td>',
                                                            '    <td class="center">' +
                                                            '        <a data-toggle="modal" data-id="' + _data[0].sapid + '" title="" class="btn btn-xs btn-primary print" href="#cancellation-modal" onclick="return false;">' +
                                                            '            <i class="ace-icon fa fa-print align-middle"></i>' +
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
                                                '    </td>',
                                                '    <td class="center">' +
                                                '        <a data-toggle="modal" data-id="' + _data[0].sapid + '" title="" class="btn btn-xs btn-primary print" onclick="return false;">' +
                                                '            <i class="ace-icon fa fa-print align-middle"></i>' +
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

                .on('click', '.print', function (e) {
                    var docid = $(this).data('id');
                    if (docid != '') {
                        try {
                            if (docid.split(',').length > 1) {
                                $.blockUI({
                                    css: {
                                        border: 'none',
                                        padding: '15px',
                                        backgroundColor: '#000',
                                        '-webkit-border-radius': '10px',
                                        '-moz-border-radius': '10px',
                                        opacity: .5,
                                        color: '#fff'
                                    }
                                });
                                setTimeout(
                                    function () {
                                        GenerateRpt(docid, 'cross');
                                        var iframe = '<div class="iframe-container"><iframe src="' + pdf_link + '"></iframe></div>'
                                        var pdf_link_length = pdf_link.split('/').length - 1;
                                        $.createModal({
                                            title: pdf_link.split('/')[pdf_link_length],
                                            message: iframe,
                                            closeButton: true,
                                            scrollable: false
                                        });
                                        $.unblockUI();
                                    }, 1000);
                            }
                        } catch (e) {
                            $.blockUI({
                                css: {
                                    border: 'none',
                                    padding: '15px',
                                    backgroundColor: '#000',
                                    '-webkit-border-radius': '10px',
                                    '-moz-border-radius': '10px',
                                    opacity: .5,
                                    color: '#fff'
                                }
                            });
                            setTimeout(
                                function () {
                                    GenerateRpt(docid, '');
                                    var iframe = '<div class="iframe-container"><iframe src="' + pdf_link + '"></iframe></div>'
                                    var pdf_link_length = pdf_link.split('/').length - 1;
                                    $.createModal({
                                        title: pdf_link.split('/')[pdf_link_length],
                                        message: iframe,
                                        closeButton: true,
                                        scrollable: false
                                    });
                                    $.unblockUI();
                                }, 1000);
                        }
                    }
                });
        });

        function GetItems() {
            $("#Summary-Table-Body").empty();
            $("#RecordBody").empty();
            var strID = [];
            var _dt = $('#dynamic-table').dataTable();
            $("input:checked", _dt.fnGetNodes()).each(function () {
                //console.log($(this).val());
                strID.push({
                    "ID": $(this).val()
                });
            });

            if (strID.length > 0) {
                var ret = LoadItems(strID);
                $('#ref-doc').focusin();
                if (ret == false) {
                    bootbox.alert("<center><h4>ไม่สามารถบันทึกบัญชีต่างบริษัทได้<h4></center>");
                }
                return ret;
            }
            else {
                bootbox.alert("<center><h4>กรุณาเลือกรายการบันทึกบัญชี<h4></center>");
                return false;
            }
        }

        function CheckComCode(id) {
            var ret = false;
            var dataValue = JSON.stringify({ lstID: id });
            $.ajax({
                type: "POST",
                url: "CostCenterACRecord.aspx/CheckComCode",
                data: dataValue,
                async: false,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                        textStatus + "\n\nError: " + errorThrown);
                },
                success: function (result) {
                    $.each(result, function (k, Val) {
                        ret = Val;
                    });
                }
            });
            return ret;
        }

        function CheckCrossCompany(id) {
            var ret = false;
            var dataValue = JSON.stringify({ lstID: id });
            $.ajax({
                type: "POST",
                url: "CostCenterACRecord.aspx/CheckCrossCompany",
                data: dataValue,
                async: false,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                        textStatus + "\n\nError: " + errorThrown);
                },
                success: function (result) {
                    $.each(result, function (k, Val) {
                        ret = Val;
                    });

                }
            });
            return ret;
        }

        function LoadItems(id) {
            console.log(id);
            if (CheckComCode(id)) {
                console.log('CheckComCode = true');
                if (!CheckCrossCompany(id)) {

                    console.log('CheckCrossCompany = false');

                    //alert(1);
                    crossCompany = false;
                    var dataValue = JSON.stringify({
                        lstID: id,
                        mode: mode,
                        ref_doc_no: $('#ref-doc').val(),
                        ref_key_3: $('#ref-text').val(),
                        posting_date: $('#PSTDATE').val(),
                        empid: $('#lbUserId').text()
                    });

                    $.ajax({
                        type: "POST",
                        url: "CostCenterACRecord.aspx/GetListDataItemsByID",
                        data: dataValue,
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        async: false,
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                                textStatus + "\n\nError: " + errorThrown);
                        },
                        success: function (result) {

                            var res = result.d;

                            if (res.MsgErr != '') {
                                bootbox.alert(res.MsgErr);
                                $('#ref-doc').val('');
                                $('#ref-text').val('');
                                mode = 1;
                                return;
                            }

                            $('#Summary-Table-Body').empty();
                            $('#RecordBody').empty();
                            //$.each(res.lstItemsDetail, function (k, Val) {
                            $.each(res.lstItemsDetail, function (key, Value) {

                                if (Value.COSTCENTER != "") {
                                    var data = RenderSummaryTable(Value);
                                    $("#Summary-Table").append(data);
                                }

                                var data = RenderRecordTable(Value);
                                $("#Record-Table").append(data);

                            });
                            //});
                            if (res.SapDocNo != '') {
                                bootbox.alert('SAP Document No : ' + res.SapDocNo.substring(0, 10));
                            }

                            $('#ref-doc').val('');
                            $('#ref-text').val('');
                            mode = 1;

                            if (res.SapDocNo != '') {
                                $.blockUI({
                                    css: {
                                        border: 'none',
                                        padding: '15px',
                                        backgroundColor: '#000',
                                        '-webkit-border-radius': '10px',
                                        '-moz-border-radius': '10px',
                                        opacity: .5,
                                        color: '#fff'
                                    }
                                });
                                setTimeout(
                                    function () {
                                        GenerateRpt(res.SapDocNo.substring(0, 10), '');
                                        var iframe = '<div class="iframe-container"><iframe src="' + pdf_link + '"></iframe></div>'
                                        var pdf_link_length = pdf_link.split('/').length - 1;
                                        $.createModal({
                                            title: pdf_link.split('/')[pdf_link_length],
                                            message: iframe,
                                            closeButton: true,
                                            scrollable: false
                                        });
                                        $.unblockUI();
                                    }, 1000);
                            }
                        }
                    });
                } else {

                    console.log('CheckCrossCompany = true');

                    var sapid = '';
                    crossCompany = true;
                    var dataValue = JSON.stringify({
                        lstID: id,
                        mode: mode,
                        ref_doc_no: $('#cross-refDoc').val(),
                        ref_key_3: $('#cross-refText').val(),
                        posting_date: $('#CROSSPSTDATE').val(),
                        empid: $('#lbUserId').text()
                    });

                    $("#sumCrossTableBody").empty();

                    $.ajax({
                        type: "POST",
                        url: "CostCenterACRecord.aspx/GetListDataItemsCrossCompanyByID",
                        data: dataValue,
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        async: false,
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                                textStatus + "\n\nError: " + errorThrown);
                        },
                        success: function (result) {

                            var res = result.d;

                            if (res.MsgErr != '') {
                                bootbox.alert(res.MsgErr);
                                $('#cross-refDoc').val('');
                                $('#cross-refText').val('');
                                mode = 1;
                                return;
                            }

                            $("#payableTableBody").empty();
                            $("#cutStockTableBody").empty();
                            $("#receiveableTableBody").empty();

                            creditor = 0;
                            dedtor = 0;

                            $.each(res.lstItemsCrossDetail, function (key, Value) {

                                if (Value.COSTCENTER != "") {
                                    var data = RenderSumCrossTable(Value);
                                    $("#sumCrossTable").append(data);
                                }

                                var data = RenderCrossRecordTable(Value);

                                $("#payableTable").append(data[0].payableTable);
                                $("#cutStockTable").append(data[0].cutStockTable);
                                $("#receiveableTable").append(data[0].receiveableTable);

                            });

                            console.log('1');
                            console.log(a4);

                            $("#payableTable").append('<tr>' +
                                '<td class="center">' + a1 + '</td>' +
                                '<td>' + a2 + '</td>' +
                                '<td class="center">' + '' + '</td>' +
                                '<td class="center">' + '' + '</td>' +
                                '<td class="center">' + a3 + '</td>' +
                                '<td class="right">' + '' + '</td>' +
                                '<td class="right">' + numberWithCommas(a4.toFixed(2)) + '</td>' +
                                '<td></td>' +
                                '</tr>');

                            console.log('2');
                            console.log(b4);

                            $("#receiveableTable").append('<tr>' +
                                '<td class="center">' + b1 + '</td>' +
                                '<td>' + b2 + '</td>' +
                                '<td class="center">' + '' + '</td>' +
                                '<td class="center">' + '' + '</td>' +
                                '<td class="center">' + 'P11000' + '</td>' +
                                '<td class="right">' + numberWithCommas(b4.toFixed(2)) + '</td>' +
                                '<td class="right">' + '' + '</td>' +
                                '<td></td>' +
                                '</tr>');

                            console.log('3');

                            a1 = null;
                            a2 = null;
                            a3 = null;
                            a4 = null;

                            b1 = null;
                            b2 = null;
                            b3 = null;
                            b4 = null;

                            if (res.SapDocNo != '') {
                                bootbox.alert('SAP Document No : ' + res.SapDocNo);
                            }
                            $('#cross-refDoc').val('');
                            $('#cross-refText').val('');

                            mode = 1;

                            if (res.SapDocNo != '') {
                                $.blockUI({
                                    css: {
                                        border: 'none',
                                        padding: '15px',
                                        backgroundColor: '#000',
                                        '-webkit-border-radius': '10px',
                                        '-moz-border-radius': '10px',
                                        opacity: .5,
                                        color: '#fff'
                                    }
                                });
                                setTimeout(
                                    function () {
                                        GenerateRpt(res.SapDocNo, 'cross');
                                        var iframe = '<div class="iframe-container"><iframe src="' + pdf_link + '"></iframe></div>'
                                        var pdf_link_length = pdf_link.split('/').length - 1;
                                        $.createModal({
                                            title: pdf_link.split('/')[pdf_link_length],
                                            message: iframe,
                                            closeButton: true,
                                            scrollable: false
                                        });
                                        $.unblockUI();
                                    }, 1000);
                            }
                        }
                    });

                }

                return true;
            }

            return false;
        }

        function GenerateRpt(sapid, status) {
            $.ajax({
                type: "POST",
                url: "CostCenterACRecord.aspx/GenerateReport",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ sapid: sapid, status: status }),
                beforeSend: function () {
                    console.log("Set blockui");

                },
                dataType: 'json',
                async: false,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                        textStatus + "\n\nError: " + errorThrown);
                },
                success: function (result) {
                    //if (result.d === "") {
                    //    bootbox.alert("กรุณาตรวจสอบการตั้งค่า PR ของ โครงการ/หน่วยงาน ที่เลือก");
                    //    return false;
                    //}
                    pdf_link = result.d;

                }
            });
        }

        function numberWithCommas(x) {
            var parts = x.toString().split(".");
            parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            return parts.join(".");
        }

        function RenderCrossRecordTable(val) {
            var payableTable, cutStockTable, receiveableTable;
            var lstStr = [];

            payableTable = '';
            cutStockTable = '';
            receiveableTable = '';

            if (val.COSTCENTER != "")
            {
                payableTable = '<tr>' +
                    '<td class="center">' + '1580010' + '</td>' +
                    '<td>' + 'สินทรัพย์ส่งเสริมการขาย' + '</td>' +
                    '<td class="center">' + val.RQRNO + '</td>' +
                    '<td class="center">' + val.COSTCENTER + '</td>' +
                    '<td class="center">' + val.REF_KEY_2 + '</td>' +
                    '<td class="right">' + numberWithCommas(val.AMT_DOCCUR) + '</td>' +
                    '<td class="right">' + '' + '</td>' +
                    '<td>' + val.ITEM_TEXT + '</td>' +
                    '</tr>';

                cutStockTable = '<tr>' +
                    '<td class="center">' + val.GL_ACCOUNT + '</td>' +
                    '<td>' + val.GL_Name + '</td>' +
                    '<td class="center">' + val.RQRNO + '</td>' +
                    '<td class="center">' + val.COSTCENTER + '</td>' +
                    '<td class="center">' + val.REF_KEY_2 + '</td>' +
                    '<td class="right">' + numberWithCommas(val.AMT_DOCCUR) + '</td>' +
                    '<td class="right">' + '' + '</td>' +
                    '<td>' + val.ITEM_TEXT + '</td>' +
                    '</tr>';

                if (val.COMP_CODE != '1000') {
                    if (b1 == null) {
                        b1 = val.COMP_CODE;
                    }
                    if (b2 == null) {
                        b2 = val.COMP_NAME;
                    }
                    if (b3 == null) {
                        b3 = val.REF_KEY_2;
                    }

                    console.log('xxxxxxxxxxxxxxx');
                    console.log(b4);
                    console.log('xxxxxxxxxxxxxxx');

                    if (b4 == null) {
                        b4 = parseFloat(val.AMT_DOCCUR);
                    }
                    else {
                        b4 += parseFloat(val.AMT_DOCCUR);
                    }
                }

            }
            else
            {
                if (val.VENDOR_NO == '1000') {
                    if (a1 == null) {
                        a1 = val.VENDOR_NO;
                    }
                    if (a2 == null) {
                        a2 = 'บริษัท เอพี (ไทยแลนด์) จำกัด';
                    }
                    if (a3 == null) {
                        a3 = val.PROFIT_CTR;
                    }

                    if (a4 == null) {
                        a4 = parseFloat(val.AMT_DOCCUR);
                    }
                    else {
                        a4 += parseFloat(val.AMT_DOCCUR);
                    }
                }

                cutStockTable = '<tr>' +
                    '<td class="center">' + '1580010' + '</td>' +
                    '<td>' + 'สินทรัพย์ส่งเสริมการขาย' + '</td>' +
                    '<td class="center">' + val.RQRNO + '</td>' +
                    '<td class="center">' + '' + '</td>' +
                    '<td class="center">' + val.PROFIT_CTR + '</td>' +
                    '<td class="right">' + '' + '</td>' +
                    '<td class="right">' + numberWithCommas(val.AMT_DOCCUR) + '</td>' +
                    '<td>' + val.ITEM_TEXT + '</td>' +
                    '</tr>';

                receiveableTable = '<tr>' +
                    '<td class="center">' + '1580010' + '</td>' +
                    '<td>' + 'สินทรัพย์ส่งเสริมการขาย' + '</td>' +
                    '<td class="center">' + val.RQRNO + '</td>' +
                    '<td class="center">' + '' + '</td>' +
                    '<td class="center">' + 'P11000' + '</td>' +
                    '<td class="right">' + '' + '</td>' +
                    '<td class="right">' + numberWithCommas(val.AMT_DOCCUR) + '</td>' +
                    '<td>' + val.ITEM_TEXT + '</td>' +
                    '</tr>';
            }

            lstStr.push({
                'payableTable': payableTable,
                'cutStockTable': cutStockTable,
                'receiveableTable': receiveableTable
            });

            return lstStr;
        }

        function RenderSumCrossTable(result) {
            var res = '<tr>' +
                '<td class="center">' + result.ITEM_ID + '</td>' +
                '<td>' + result.ITEM_NAME + '</td>' +
                '<td style="text-align: right">' + result.ITEM_QUAN + '</td>' +
                '<td style="text-align: right">' + numberWithCommas(result.AMT_DOCCUR) + '</td>' +
                '<td class="center">' + result.COSTCENTER + '</td>' +
                '</tr>';
            return res;
        }

        function RenderDatatable(str) {
            var res = '<tr>' +
                '<td class="center">' +
                (str.SAPID == "" ?
                    '<label class="pos-rel">' +
                    '<input type="checkbox" class="ace" value="' + str.ID + '" /><span class="lbl"></span>' +
                    '</label>'
                    : '') +
                '</td>' +
                '<td class="center">' + str.RQRNO + '</td>' +
                '<td class="center">' + str.COSTCENTER + '</td>' +
                '<td>' + str.PROPN + '</td>' +
                '<td class="center">' + str.Date + '</td>' +
                '<td>' + str.PROMOTN + '</td>' +
                '<td class="right">' + str.QUANT + '</td>' +
                '<td>' + str.REQN + '</td>' +
                '<td class="center">' + str.SAPID + '</td>' +
                '</tr>';
            return res;

        }

        function RenderSummaryTable(result) {
            var res = '<tr>' +
                '<td>' + result.ITEM_ID + '</td>' +
                '<td>' + result.ITEM_NAME + '</td>' +
                '<td style="text-align: right">' + result.ITEM_QUAN + '</td>' +
                '<td style="text-align: right">' + numberWithCommas(result.AMT_DOCCUR) + '</td>' +
                '<td style="text-align: right">' + result.COSTCENTER + '</td>' +
                '</tr>';
            return res;
        }

        function RenderRecordTable(result) {
            var res = '<tr>' +
                '<td class="center">' + result.GL_ACCOUNT + '</td>' +
                '<td class="left">' + result.GL_Name + '</td>' +
                '<td class="left">' + result.RQRNO + '</td>' +
                '<td class="center">' + result.COSTCENTER + '</td>' +
                '<td style="text-align: center">' + result.PROFIT_CTR + '</td>' +
                '<td style="text-align: right">' + (result.COSTCENTER != '' ? numberWithCommas(result.AMT_DOCCUR) : '') + '</td>' +
                '<td style="text-align: right">' + (result.PROFIT_CTR != '' ? numberWithCommas(result.AMT_DOCCUR) : '') + '</td>' +
                '<td style="text-align: right">' + result.ITEM_TEXT + '</td>' +
                '</tr>';
            return res;
        }

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div id="accordion" class="accordion-style1 panel-group">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h4 class="panel-title">
                            <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion" href="#collapseOne">
                                <i class="ace-icon fa fa-angle-down bigger-110" data-icon-hide="ace-icon fa fa-angle-down" data-icon-show="ace-icon fa fa-angle-right"></i>
                                บันทึกบัญชี (Cost Center)
                            </a>
                        </h4>
                    </div>

                    <div class="panel-collapse collapse in" id="collapseOne" style="padding-bottom: 5px">
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-12 margin-bottom-menu">
                                    <div class="col-md-2 div-caption">เลขที่เอกสารใบส่งมอบ</div>
                                    <div class="col-md-4">
                                        <input id="docID" type="text" class="max-width" name="txtBox" value="" placeholder="เลขที่เอกสารใบส่งมอบ" />
                                    </div>
                                    <div class="col-md-2 div-caption">สินค้าโปรโมชั่น</div>
                                    <div class="col-md-4">
                                        <select class="form-control hide" style="width: 100%" id="cbxPromotion" data-placeholder="--- กรุณาเลือก โปรโมชั่น ---">
                                        </select>
                                    </div>
                                </div>

                                <div class="col-md-12 margin-bottom-menu">
                                    <div class="col-md-2 div-caption">สถานะ</div>
                                    <div class="col-md-2">
                                        <select class="form-control" style="width: 100%" id="cbxDelvStatus">
                                            <option value="0" selected="selected">-- ทั้งหมด --</option>
                                            <option value="1">รายการใหม่</option>
                                            <option value="2">เสร็จสิ้น</option>
                                        </select>
                                    </div>
                                    <div class="col-md-2"></div>
                                    <div class="col-md-1"></div>
                                    <div class="col-md-5">
                                        <div class="input-group input-group-sm">
                                            <span class="input-group-addon">วันที่ส่งมอบ
                                            </span>
                                            <input type="text" id="STDDATE" class="form-control" placeholder="" />
                                            <span class="input-group-addon">
                                                <i class="ace-icon fa fa-calendar"></i>
                                            </span>
                                            <span class="input-group-addon" style="border: none; background-color: white;">~
                                            </span>
                                            <input type="text" id="ENDDATE" class="form-control" placeholder="" />
                                            <span class="input-group-addon">
                                                <i class="ace-icon fa fa-calendar"></i>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row" style="margin-top: 5px;">

                                <div class="col-sm-12" style="text-align: center;">
                                    <button type="button" id="btnSearch" class="btn btn-white btn-info btn-sm" style="width: 75px">
                                        Search</button>

                                    <button type="button" id="clear" class="btn btn-white btn-info btn-sm" style="width: 75px">
                                        Clear</button>
                                </div>

                                <%-------------------------------%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12 pull-left">
            <div class="row">
                <div class="col-sm-6 left">
                    <button type="button" id="btnAdd" class="btn btn-white btn-info">
                        <i class="ace-icon glyphicon glyphicon-plus"></i>
                        บันทึกบัญชี (Cost Center)                   
                    </button>
                </div>
                <div class="col-sm-6 right">
                    <a data-toggle="modal" data-id="" title="" class="btn btn-white btn-primary accmanage-modal" href="#accountmanagement-modal" onclick="return false;">
                        <i class="ace-icon glyphicon glyphicon-cog"></i>จัดการเอกสารบันทึกบัญชี
                    </a>
                </div>
            </div>
        </div>

        <div id="dialog-message" class="hide">
            <div class="col-md-12 margin-bottom-menu">
                <div class="col-md-1">
                    <input type="hidden" autofocus="autofocus" />
                </div>
                <div class="col-md-4">
                    <div class="col-md-4 div-caption"><span>Posting Date: </span></div>
                    <div class="col-md-8">
                        <div class="input-group input-group-sm">
                            <input type="text" id="PSTDATE" class="form-control" placeholder="" />
                            <span class="input-group-addon">
                                <i class="ace-icon fa fa-calendar"></i>
                            </span>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <input type="text" class="max-width" name="txt" id="ref-doc" placeholder="Reference" />
                </div>
                <div class="col-md-4">
                    <input type="text" class="max-width" name="txt" id="ref-text" placeholder="REF KEY 3" />
                </div>

            </div>
            <div class="col-md-12">
                <div id="accordion2" class="accordion-style1 panel-group">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h4 class="panel-title">
                                <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion2" href="#collapseOne2">
                                    <i class="ace-icon fa fa-angle-down bigger-110" data-icon-hide="ace-icon fa fa-angle-down" data-icon-show="ace-icon fa fa-angle-right"></i>
                                    สรุปรายละเอียดค่าใช้จ่าย
                                </a>
                            </h4>
                        </div>

                        <div class="panel-collapse collapse in" id="collapseOne2">
                            <div class="panel-body">
                                <div class="row">
                                    <table class="table" id="Summary-Table">
                                        <thead>
                                            <tr>
                                                <th class="center">เลขที่สินค้าโปรโมชั่น</th>
                                                <th class="center">สินค้าโปรโมชั่น</th>
                                                <th class="center" style="text-align: right">จำนวน</th>
                                                <th class="center" style="text-align: right">มูลค่ารวม</th>
                                                <th class="center">Cost Center</th>
                                            </tr>
                                        </thead>
                                        <tbody id="Summary-Table-Body">
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-12">
                <div class="accordion-style1 panel-group">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h4 class="panel-title">
                                <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion3" href="#collapseOne3">
                                    <i class="ace-icon fa fa-angle-down bigger-110" data-icon-hide="ace-icon fa fa-angle-down" data-icon-show="ace-icon fa fa-angle-right"></i>
                                    บันทึกบัญชี
                                </a>
                            </h4>
                        </div>

                        <div class="panel-collapse collapse in" id="collapseOne3">
                            <div class="panel-body">
                                <div class="row">

                                    <table class="table" id="Record-Table">
                                        <thead>
                                            <tr>
                                                <th class="center">GL No</th>
                                                <th class="center">GL Name</th>
                                                <th class="center">Doc No</th>
                                                <th class="center">Cost Center</th>
                                                <th style="text-align: center">Profit Center</th>
                                                <th style="text-align: right">Debit</th>
                                                <th style="text-align: right">Credit</th>
                                                <th style="text-align: center">Item Text</th>
                                            </tr>
                                        </thead>
                                        <tbody id="RecordBody">
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div id="cross-company" class="hide">
            <div class="col-md-12 margin-bottom-menu">
                <div class="col-md-1">
                    <input type="hidden" autofocus="autofocus" />
                </div>
                <div class="col-md-4">
                    <div class="col-md-4 div-caption"><span>Posting Date: </span></div>
                    <div class="col-md-8">
                        <div class="input-group input-group-sm">
                            <input type="text" id="CROSSPSTDATE" class="form-control" placeholder="" />
                            <span class="input-group-addon">
                                <i class="ace-icon fa fa-calendar"></i>
                            </span>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <input type="text" class="max-width" name="txt" id="cross-refDoc" placeholder="Reference" />
                </div>
                <div class="col-md-4">
                    <input type="text" class="max-width" name="txt" id="cross-refText" placeholder="REF KEY 3" />
                </div>

            </div>
            <div class="col-md-12">
                <div id="accordion7" class="accordion-style1 panel-group">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h4 class="panel-title">
                                <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion7" href="#collapseOne7">
                                    <i class="ace-icon fa fa-angle-down bigger-110" data-icon-hide="ace-icon fa fa-angle-down" data-icon-show="ace-icon fa fa-angle-right"></i>
                                    สรุปรายละเอียดค่าใช้จ่าย
                                </a>
                            </h4>
                        </div>

                        <div class="panel-collapse collapse in" id="collapseOne7">
                            <div class="panel-body">
                                <div class="row">
                                    <table class="table" id="sumCrossTable">
                                        <thead>
                                            <tr>
                                                <th class="center">เลขที่สินค้าโปรโมชั่น</th>
                                                <th class="center">สินค้าโปรโมชั่น</th>
                                                <th class="center" style="text-align: right">จำนวน</th>
                                                <th class="center" style="text-align: right">มูลค่ารวม</th>
                                                <th class="center">Cost Center</th>
                                            </tr>
                                        </thead>
                                        <tbody id="sumCrossTableBody">
                                            <%--<tr>
                                                <td class="center">10101010</td>
                                                <td class="left">Test Test Test Test Test</td>
                                                <td class="right">1</td>
                                                <td class="right">312.1200</td>
                                                <td class="center">611010</td>
                                            </tr>--%>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-12">
                <div id="accordion3" class="accordion-style1 panel-group">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h4 class="panel-title">
                                <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion3" href="#collapseOne4">
                                    <i class="ace-icon fa fa-angle-down bigger-110" data-icon-hide="ace-icon fa fa-angle-down" data-icon-show="ace-icon fa fa-angle-right"></i>
                                    บันทึกบัญชีตั้งเจ้าหนี้
                                </a>
                            </h4>
                        </div>
                        <div class="panel-collapse collapse in" id="collapseOne4">
                            <div class="panel-body">
                                <div class="row">
                                    <table class="table" id="payableTable">
                                        <thead>
                                            <tr>
                                                <th class="center">GL No</th>
                                                <th class="center">GL Name</th>
                                                <th class="center">Doc No</th>
                                                <th class="center">Cost Center</th>
                                                <th class="center">Profit Center</th>
                                                <th class="right">Debit</th>
                                                <th class="right">Credit</th>
                                                <th class="center">Item Text</th>
                                            </tr>
                                        </thead>
                                        <tbody id="payableTableBody">
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-12">
                <div id="accordion4" class="accordion-style1 panel-group">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h4 class="panel-title">
                                <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion4" href="#collapseOne5">
                                    <i class="ace-icon fa fa-angle-down bigger-110" data-icon-hide="ace-icon fa fa-angle-down" data-icon-show="ace-icon fa fa-angle-right"></i>
                                    บันทึกบัญชีตัดสต็อก
                                </a>
                            </h4>
                        </div>
                        <div class="panel-collapse collapse in" id="collapseOne5">
                            <div class="panel-body">
                                <div class="row">
                                    <table class="table" id="cutStockTable">
                                        <thead>
                                            <tr>
                                                <th class="center">GL No</th>
                                                <th class="center">GL Name</th>
                                                <th class="center">Doc No</th>
                                                <th class="center">Cost Center</th>
                                                <th class="center">Profit Center</th>
                                                <th class="right">Debit</th>
                                                <th class="right">Credit</th>
                                                <th class="center">Item Text</th>
                                            </tr>
                                        </thead>
                                        <tbody id="cutStockTableBody">
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-12">
                <div id="accordion5" class="accordion-style1 panel-group">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h4 class="panel-title">
                                <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion5" href="#collapseOne6">
                                    <i class="ace-icon fa fa-angle-down bigger-110" data-icon-hide="ace-icon fa fa-angle-down" data-icon-show="ace-icon fa fa-angle-right"></i>
                                    บันทึกบัญชีตั้งลูกหนี้
                                </a>
                            </h4>
                        </div>
                        <div class="panel-collapse collapse in" id="collapseOne6">
                            <div class="panel-body">
                                <div class="row">
                                    <table class="table" id="receiveableTable">
                                        <thead>
                                            <tr>
                                                <th class="center">GL No</th>
                                                <th class="center">GL Name</th>
                                                <th class="center">Doc No</th>
                                                <th class="center">Cost Center</th>
                                                <th class="center">Profit Center</th>
                                                <th class="right">Debit</th>
                                                <th class="right">Credit</th>
                                                <th class="center">Item Text</th>
                                            </tr>
                                        </thead>
                                        <tbody id="receiveableTableBody">
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <table id="dynamic-table" class="table table-striped table-bordered table-hover">
                <thead>
                    <tr id="iHeader">
                        <th class="">
                            <label class="pos-rel">
                                <input type="checkbox" class="ace" />
                            </label>
                        </th>
                        <th class="center" style="width: 140px;">เลขที่เอกสารใบส่งมอบ</th>
                        <th class="center" style="">Cost Center</th>
                        <th class="center" style="">สินค้าโปรโมชั่น</th>
                        <th class="center" style="">วันที่</th>
                        <th class="center" style="">Promotion</th>
                        <th class="center" style="">Quantity</th>
                        <th class="center" style="">ผู้เบิก</th>
                        <th class="center" style="">SAP Document No</th>
                        <%--<th class="center" style="">Reverse</th>
                        <th class="center" style="">Report</th>--%>
                    </tr>
                </thead>
                <tbody id="iBody">
                </tbody>
            </table>
        </div>
    </div>
    <div class="modal fade" id="accountmanagement-modal" tabindex="-1" role="dialog" aria-labelledby="accountmanagement-modal-label">
        <div class="modal-lg modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="accountmanagement-modal-label"><i class="ace-icon glyphicon glyphicon-cog"></i>จัดการเอกสารบันทึกบัญชี</h4>
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
                                    <th class="center" style="width: 50px">Report</th>
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
