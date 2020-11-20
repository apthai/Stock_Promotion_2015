<%@ Page Title="Memo Request Form" Language="C#" MasterPageFile="~/master/MasterPage.Master"
    AutoEventWireup="true" CodeBehind="MemoRequestForm.aspx.cs" Inherits="AP_StockPromotion_V1.webpage.MemoRequestForm" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        .gray-color {
            background-color: #808080 !important;
        }

        .ui-dialog {
            top: 50px !important;
        }

        .align-right {
            text-align: right;
        }

        .align-left {
            text-align: left;
        }

        .max-width {
            width: 100%;
        }

        .width-ninety {
            width: 90%;
        }

        .no-padding {
            padding-left: 0 !important;
            padding-right: 0 !important;
        }

        .margin-bottom-menu {
            margin-bottom: 15px;
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

        .div-caption {
            text-align: right;
            padding-right: 5px;
            font-size: 12px;
        }
    </style>

    <script type="text/javascript">
        var RoleCode = '<%=Session["RoleCode"] %>';
        var ListItems;
        var optItems, optApprove;
        var pdf_link = '';
        var mailBodyStr = '';
        var subApproveEmail;
        var approverEmail;
        var lstApproverEmail;
        var listApproverName;
        var ApvOrRej = '';
        var GUID = '';
        var msg = '';
        var ApproveId = '';
        var SubApproveId = '';
        var subApproverEmail;
        var savemode = 0;
        var DocType = '';

        (function (a) {
            a.createModal = function (b) {
                defaults = {
                    title: "Memo Report", message: "", closeButton: true, scrollable: false
                };
                var b = a.extend({}, defaults, b);
                var c = (b.scrollable === true) ? 'style="max-height: 450px;overflow-y: auto;"' : "";
                html = '<div class="modal fade" id="myModal">';
                html += '<div class="modal-dialog modal-lg">';
                html += '<div class="modal-content">';
                html += '<div class="modal-header">';
                html += '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>';
                if (b.title.length > 0) {
                    html += '<h4 class="modal-title">' + b.title + "</h4>"
                }
                html += "</div>";
                html += '<div class="modal-body modal-fixed-height" ' + c + ">";
                html += b.message; html += "</div>";
                html += "</div>";
                html += "</div>";
                html += "</div>";
                a("body").prepend(html);
                a("#myModal").modal().on("hidden.bs.modal", function () {
                    a(this).remove()
                })
            }
        })(jQuery);

        jQuery(function ($) {

            //if (RoleCode == 'Admin' || RoleCode == 'AdminCenter') {
            //    $('#divNavx').html('ตั้งเบิกภายในองค์กร >> ใบบันทึกขอเบิก (Memo) ' + '(' + RoleCode + ')');
            //} else {
            //    $('#divNavx').html('ตั้งเบิกภายในองค์กร >> ใบบันทึกขอเบิก (Memo) ' + '(' + RoleCode + ')');
            //}

            $('#divNavx').html('ตั้งเบิกภายในองค์กร >> ใบบันทึกขอเบิก (Memo) ' + '(' + RoleCode + ')');

            if (RoleCode == "MKT") {
                $('.create-panel').html('สร้าง Memo ขอซื้อ (Marketing)');
            } else {
                $('.create-panel').html('สร้างเอกสารตั้งเบิก');
            }

            $('#STDDATE').datepicker({
                showOtherMonths: true,
                selectOtherMonths: false,
                dateFormat: 'dd/mm/yy'
            });

            $('#ENDDATE').datepicker({
                showOtherMonths: true,
                selectOtherMonths: false,
                dateFormat: 'dd/mm/yy'
            });

            $('#memoCreateDate').datepicker({
                showOtherMonths: true,
                selectOtherMonths: false,
                dateFormat: 'dd/mm/yy'
            });

            $('#memoUseDate').datepicker({
                showOtherMonths: true,
                selectOtherMonths: false,
                dateFormat: 'dd/mm/yy'
            });

            $('#memoEndDate').datepicker({
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
                        $(this).find('.chosen- input').css('width', '200px');
                    });
                }
            })

            if (!ace.vars['touch']) {
                $('.chosen-select').chosen({ allow_single_deselect: true });

                $(window)
                    .off('resize.chosen')
                    .on('resize.chosen', function () {
                        $('.chosen-select').each(function () {
                            var $this = $(this);
                            $this.next().css({ 'width': 100 + '%' });
                        })
                    }).trigger('resize.chosen');
                $(document).on('settings.ace.chosen', function (e, event_name, event_val) {
                    if (event_name != 'sidebar_collapsed') return;
                    $('.chosen-select').each(function () {
                        var $this = $(this);
                        $this.next().css({ 'width': 100 + '%' });
                    })
                });
            }

        });

        $(document).ready(function () {
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

            $('.numeric').autoNumeric('init', {
                vMin: "0",
                vMax: "999999999",
                aSep: ""
            });


            setTimeout(
                function () {

                    if (GUID != '') {
                        if (ApvOrRej == 0) {
                            $('#iApproved').modal({ backdrop: 'static', keyboard: false }, 'toggle');
                        }
                        else if (ApvOrRej == 1) {
                            MemoAlert('<h2><center>' + msg + '!</center></h2>', "");
                        }

                        ChangeUrl("", "MemoRequestForm.aspx");
                    }

                    $('#addnew').fadeOut();
                    var date = new Date();

                    $('#memoCreateDate').val((date.getDate().length == 1 ? '0' + date.getDate().toString() : date.getDate()) + '/' + (date.getMonth() + 1) + '/' + date.getFullYear());
                    $('#memoUseDate').val((date.getDate().length == 1 ? '0' + date.getDate().toString() : date.getDate()) + '/' + (date.getMonth() + 1) + '/' + date.getFullYear());
                    var myDate = date.getDate() + '/' + (date.getMonth() + 1) + '/' + date.getFullYear();
                    $('#memoEndDate').val(moment(myDate, "DD/MM/YYYY").add(1, 'months').format("DD/MM/YYYY"));

                    console.log(moment(myDate, "DD/MM/YYYY").add(1, 'months').format("DD/MM/YYYY"));

                    //$('#imemo').DataTable();

                    GetMemoData();

                    //$('#memo-table').DataTable();
                    $.ajax({
                        type: "POST",
                        url: "MemoRequestForm.aspx/GetFullName",
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                                textStatus + "\n\nError: " + errorThrown);
                        },
                        success: function (result) {
                            try {
                                var res = result.d.split(':');
                                $('#FullName').val(res[0] + ':' + res[1] + ' ' + res[2]/*$('#lbUserName').text()*/);

                                $('#cbxCostCenter option:selected').text(res[3] + ' : ' + res[4]);
                                $('#cbxCostCenter').trigger("chosen:updated");
                            } catch (e) { }
                        }
                    });
                    SetItems();

                    ///----------------------  Validate Combobox and table  ----------------------///
                    if (RoleCode == 'MKT') {
                        $('#FullName').removeClass('hide');
                        $('#cbxUserName').removeClass('hide').addClass('hide');
                        $('#cbxUserName_chosen').removeClass('hide').addClass('hide');
                        $('.isnotmkt').removeClass('hide').addClass('hide');
                        $('.projectRow').removeClass('hide').addClass('hide');
                        console.log(RoleCode);
                        $('.tblProjectId').removeClass('hide').addClass('hide');
                        $('.tblProjectName').removeClass('hide').addClass('hide');

                        $('#txtareaReason').val('Project : \nObjective : \nCondition : \n\nLead-Time-To Use : \nLC Name : \nDelivery Date : ');
                        $('#lbReasonRemark').html('* กรุณาระบุข้อมูล Project, Objective, Condition, Lead-Time-To Use, LC Name, Delivery Date');
                    }
                    else if (RoleCode == 'Admin') {
                        $('#FullName').removeClass('hide').addClass('hide');
                        $('.projectRow').removeClass('hide');
                        $('.tblProjectId').removeClass('hide');
                        $('.tblProjectName').removeClass('hide');
                        $.ajax({
                            type: "POST",
                            url: "MemoRequestForm.aspx/GetUsers",
                            contentType: 'application/json; charset=utf-8',
                            dataType: 'json',
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                                    textStatus + "\n\nError: " + errorThrown);
                            },
                            success: function (result) {
                                try {
                                    var res = result.d;
                                    if (res.isCheck) {
                                        var data = '<option></option>';
                                        $.each(res.ListUsersData, function (key, val) {
                                            data += '<option value="' + val.ID + '">' + val.ID + ' : ' + val.FName + ' ' + val.LName + '</option>';
                                        });
                                        $('#cbxUserName').append(data);
                                        $('#cbxUserName').trigger("chosen:updated");
                                    }
                                    else {
                                        bootbox.alert('<center><h4>' + res.Message + '</h4></center>');
                                    }
                                } catch (e) { }
                            }
                        });

                        $('#cbxUserName').removeClass('hide');
                        $('#cbxUserName_chosen').removeClass('hide');
                        $('.isnotmkt').removeClass('hide');
                        console.log($('#lbUserId').text());
                        $.ajax({
                            type: "POST",
                            url: "MemoRequestForm.aspx/CheckMarketingUser",
                            contentType: 'application/json; charset=utf-8',
                            data: JSON.stringify({ empcode: $('#lbUserId').text() }),
                            dataType: 'json',
                            async: false,
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                                    textStatus + "\n\nError: " + errorThrown);
                            },
                            success: function (result) {

                                $('#cbxPromotion').empty();
                                $('#cbxGL').empty();
                                $('#cbxObj').empty();
                                $('#cbxProject').empty();

                                $('#cbxPromotion').trigger("chosen:updated");
                                $('#cbxGL').trigger("chosen:updated");
                                $('#cbxObj').trigger("chosen:updated");
                                $('#cbxProject').trigger("chosen:updated");

                                try {
                                    var res = result.d;
                                    if (res.IsMKT) {
                                        $('.isnotmkt').removeClass('hide').addClass('hide');
                                        $('.projectRow').removeClass('hide').addClass('hide');
                                        $('.tblProjectId').removeClass('hide').addClass('hide');
                                        $('.tblProjectName').removeClass('hide').addClass('hide');
                                    }
                                    else {
                                        $('.isnotmkt').removeClass('hide');
                                        $('.projectRow').removeClass('hide');
                                        $('.tblProjectId').removeClass('hide');
                                        $('.tblProjectName').removeClass('hide');
                                    }

                                    if (res.IsCheck) {

                                        var glData = '<option></option>';
                                        $.each(res.GL, function (key, glVal) {
                                            glData += '<option value="' + glVal.GLNO + '">' + glVal.GLNO + ' : ' + glVal.GLName + '</option>';
                                        });

                                        var objData = '<option></option>';
                                        $.each(res.OBJ, function (key, objVal) {
                                            objData += '<option value="' + objVal.ObjId + '">' + objVal.ObjId + ' : ' + objVal.ObjName + '</option>';
                                        });

                                        var projects = '<option></option><option value="999777">999777 : -- ไม่ระบุโครงการ -- </option>';
                                        $.each(res.PROJ, function (key, projVal) {
                                            projects += '<option value="' + projVal.ProduectId + '">' + projVal.ProduectId + ' : ' + projVal.Project + '</option>';
                                        });

                                        var promoData = '<option></option>';
                                        promoData += '<option value="1">Marketing Campaign</option>';
                                        promoData += '<option value="2">Event</option>';
                                        promoData += '<option value="3">อื่นๆ</option>';

                                        $('#cbxPromotion').append(promoData);
                                        $('#cbxGL').append(glData);
                                        $('#cbxObj').append(objData);
                                        $('#cbxProject').append(projects);

                                        $('#cbxPromotion').trigger("chosen:updated");
                                        $('#cbxGL').trigger("chosen:updated");
                                        $('#cbxObj').trigger("chosen:updated");
                                        $('#cbxProject').trigger("chosen:updated");
                                    }
                                    else {
                                        if (res.Message != '') {
                                            bootbox.alert('<center><h4>' + res.Message + '</h4></center>');
                                        }
                                    }

                                } catch (e) { }
                            }
                        });
                    } else {
                        $('.projectRow').removeClass('hide');
                        $('#FullName').removeClass('hide');
                        $('#cbxUserName').removeClass('hide').addClass('hide');
                        $('#cbxUserName_chosen').removeClass('hide').addClass('hide');
                        $('.isnotmkt').removeClass('hide');

                        console.log($('#lbUserId').text());

                        $.ajax({
                            type: "POST",
                            url: "MemoRequestForm.aspx/CheckMarketingUser",
                            contentType: 'application/json; charset=utf-8',
                            data: JSON.stringify({ empcode: $('#lbUserId').text() }),
                            dataType: 'json',
                            async: false,
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                                    textStatus + "\n\nError: " + errorThrown);
                            },
                            success: function (result) {

                                $('#cbxPromotion').empty();
                                $('#cbxGL').empty();
                                $('#cbxObj').empty();
                                $('#cbxProject').empty();

                                $('#cbxPromotion').trigger("chosen:updated");
                                $('#cbxGL').trigger("chosen:updated");
                                $('#cbxObj').trigger("chosen:updated");
                                $('#cbxProject').trigger("chosen:updated");

                                try {
                                    var res = result.d;
                                    if (res.IsMKT) {
                                        $('.isnotmkt').removeClass('hide').addClass('hide');
                                        $('.projectRow').removeClass('hide').addClass('hide');
                                    }
                                    else {
                                        $('.isnotmkt').removeClass('hide');
                                        $('.projectRow').removeClass('hide');
                                    }

                                    if (res.IsCheck) {

                                        var glData = '<option></option>';
                                        $.each(res.GL, function (key, glVal) {
                                            glData += '<option value="' + glVal.GLNO + '">' + glVal.GLNO + ' : ' + glVal.GLName + '</option>';
                                        });

                                        var objData = '<option></option>';
                                        $.each(res.OBJ, function (key, objVal) {
                                            objData += '<option value="' + objVal.ObjId + '">' + objVal.ObjId + ' : ' + objVal.ObjName + '</option>';
                                        });

                                        var projects = '<option></option><option value="999777">999777 : -- ไม่ระบุโครงการ -- </option>';
                                        $.each(res.PROJ, function (key, projVal) {
                                            projects += '<option value="' + projVal.ProduectId + '">' + projVal.ProduectId + ' : ' + projVal.Project + '</option>';
                                        });

                                        var promoData = '<option></option>';
                                        promoData += '<option value="1">Marketing Campaign</option>';
                                        promoData += '<option value="2">Event</option>';
                                        promoData += '<option value="3">อื่นๆ</option>';

                                        $('#cbxPromotion').append(promoData);
                                        $('#cbxGL').append(glData);
                                        $('#cbxObj').append(objData);
                                        $('#cbxProject').append(projects);

                                        $('#cbxPromotion').trigger("chosen:updated");
                                        $('#cbxGL').trigger("chosen:updated");
                                        $('#cbxObj').trigger("chosen:updated");
                                        $('#cbxProject').trigger("chosen:updated");
                                    }
                                    else {

                                        if (res.Message != '') {
                                            bootbox.alert('<center><h4>' + res.Message + '</h4></center>');
                                        }
                                    }

                                } catch (e) { }
                            }
                        });

                        $('#txtareaReason').val('');
                        $('#lbReasonRemark').html('');
                    }

                    $.unblockUI();
                }, 1000);
        });

        $(document)
            .on('change', '#cbxUserName', function (e) {
                console.log($('#cbxUserName').val());
                $.ajax({
                    type: "POST",
                    url: "MemoRequestForm.aspx/CheckMarketingUser",
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify({ empcode: $('#cbxUserName').val() }),
                    dataType: 'json',
                    async: false,
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                            textStatus + "\n\nError: " + errorThrown);
                    },
                    success: function (result) {
                        $('#cbxPromotion').empty();
                        $('#cbxGL').empty();
                        $('#cbxObj').empty();
                        $('#cbxProject').empty();
                        $('#cbxPromotion').trigger("chosen:updated");
                        $('#cbxGL').trigger("chosen:updated");
                        $('#cbxObj').trigger("chosen:updated");
                        $('#cbxProject').trigger("chosen:updated");
                        try {
                            var res = result.d;
                            if (res.IsMKT) {
                                $('.isnotmkt').removeClass('hide').addClass('hide');
                                $('.projectRow').removeClass('hide').addClass('hide');
                            }
                            else {
                                $('.isnotmkt').removeClass('hide');
                                $('.projectRow').removeClass('hide');
                            }

                            if (res.IsCheck) {

                                var glData = '<option></option>';
                                $.each(res.GL, function (key, glVal) {
                                    glData += '<option value="' + glVal.GLNO + '">' + glVal.GLNO + ' : ' + glVal.GLName + '</option>';
                                });

                                var objData = '<option></option>';
                                $.each(res.OBJ, function (key, objVal) {
                                    objData += '<option value="' + objVal.ObjId + '">' + objVal.ObjId + ' : ' + objVal.ObjName + '</option>';
                                });

                                var projects = '<option></option><option value="999777">999777 : -- ไม่ระบุโครงการ -- </option>';
                                $.each(res.PROJ, function (key, projVal) {
                                    projects += '<option value="' + projVal.ProduectId + '">' + projVal.ProduectId + ' : ' + projVal.Project + '</option>';
                                });

                                var promoData = '<option></option>';
                                promoData += '<option value="1">Marketing Campaign</option>';
                                promoData += '<option value="2">Event</option>';
                                promoData += '<option value="3">อื่นๆ</option>';

                                $('#cbxPromotion').append(promoData);
                                $('#cbxGL').append(glData);
                                $('#cbxObj').append(objData);
                                $('#cbxProject').append(projects);

                                $('#cbxPromotion').trigger("chosen:updated");
                                $('#cbxGL').trigger("chosen:updated");
                                $('#cbxObj').trigger("chosen:updated");
                                $('#cbxProject').trigger("chosen:updated");
                            }
                            else {

                                if (res.Message != '') {
                                    bootbox.alert('<center><h4>' + res.Message + '</h4></center>');
                                }
                            }

                        } catch (e) { }
                    }
                });
            })
            .on('click', '.btnConfirmMemo', function (e) {
                if ($('.btnConfirmMemo').text() == 'ยกเลิกการจัดซื้อ') {

                    $.ajax({
                        type: "POST",
                        url: "MemoRequestForm.aspx/iUpdateApproveMemoRequest",
                        contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify({ docno: $('#decuid').val() }),
                        dataType: 'json',
                        async: false,
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                                textStatus + "\n\nError: " + errorThrown);
                        },
                        success: function (result) {
                            if (result.d == true) {
                                bootbox.alert('<h4><center>ยกเลิกการจัดซื้อ<br /><br />สำเร็จ!</center></h4>');
                                $('.btnConfirmMemo').text('ยืนยันการจัดซื้อ');
                                $('.btnConfirmMemo').removeClass('btn-danger').addClass('btn-success');
                            }
                            else {
                                bootbox.alert('<h4><center>' + result.d + '</center></h4>');
                            }
                        }
                    });
                }
                else {
                    $.ajax({
                        type: "POST",
                        url: "MemoRequestForm.aspx/UpdateFinishMemoRequest",
                        contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify({ docno: $('#decuid').val() }),
                        dataType: 'json',
                        async: false,
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                                textStatus + "\n\nError: " + errorThrown);
                        },
                        success: function (result) {
                            if (result.d == '') {
                                bootbox.alert('<h4><center>ยืนยันการจัดซื้อ<br /><br />สำเร็จ!</center></h4>');
                                $('.btnConfirmMemo').text('ยกเลิกการจัดซื้อ');
                                $('.btnConfirmMemo').removeClass('btn-success').addClass('btn-danger');
                            }
                            else {
                                bootbox.alert('<h4><center>' + result.d + '</center></h4>');
                            }
                        }
                    });
                }
            })
            .on('change', '#memoCreateDate', function (e) {
                var a = $('#memoUseDate').val().split('/');
                var c = $('#memoCreateDate').val().split('/');

                var d = new Date(a[2], a[1] - 1, a[0]);
                var f = new Date(c[2], c[1] - 1, c[0]);

                if (d < f) {
                    $('#memoUseDate').val($('#memoCreateDate').val());
                    var myDate = $('#memoCreateDate').val();
                    $('#memoEndDate').val(moment(myDate, "DD/MM/YYYY").add(1, 'months').format("DD/MM/YYYY"));
                    //$('#memoEndDate').val($('#memoCreateDate').val());
                }

            })
            .on('change', '#memoUseDate', function (e) {
                var a = $('#memoUseDate').val().split('/');
                var b = $('#memoEndDate').val().split('/');
                var c = $('#memoCreateDate').val().split('/');

                var d = new Date(a[2], a[1] - 1, a[0]);
                var e = new Date(b[2], b[1] - 1, b[0]);
                var f = new Date(c[2], c[1] - 1, c[0]);

                if (d < f) {
                    bootbox.alert("<center><h4>วันที่ต้องการใช้ ต้องไม่น้อยกว่า วันที่สร้างคำขอ</h4></center>");
                    $('#memoUseDate').val($('#memoCreateDate').val());
                }

                if (e < d) {
                    var myDate = $('#memoUseDate').val();
                    $('#memoEndDate').val(moment(myDate, "DD/MM/YYYY").add(1, 'months').format("DD/MM/YYYY"));
                    //$('#memoEndDate').val($('#memoUseDate').val());
                }

            })
            .on('change', '#memoEndDate', function (e) {
                var a = $('#memoUseDate').val().split('/');
                var b = $('#memoEndDate').val().split('/');
                var c = $('#memoCreateDate').val().split('/');

                var d = new Date(a[2], a[1] - 1, a[0]);
                var e = new Date(b[2], b[1] - 1, b[0]);
                var f = new Date(c[2], c[1] - 1, c[0]);

                if (e < d || e < f) {
                    bootbox.alert("<center><h4>วันที่สิ้นสุดการขอใช้ ต้องไม่น้อยกว่า <br /> วันที่ต้องการใช้ และวันที่สร้างคำขอ</h4></center>");
                    //var date = new Date();
                    var myDate = $('#memoUseDate').val();
                    $('#memoEndDate').val(moment(myDate, "DD/MM/YYYY").add(1, 'months').format("DD/MM/YYYY"));
                    //$('#memoEndDate').val($('#memoUseDate').val());
                }

            })
            .on('click', '.btnAddMemo', function (e) {
                savemode = 0;
                $('.totals').text('-');
                var date = new Date();
                //$('#STDDATE').val(date.getDate() + '/' + ((date.getMonth() + 0).length == 1 ? '0' + (date.getMonth() + 0) : (date.getMonth() + 0)) + '/' + date.getFullYear());
                //$('#ENDDATE').val(date.getDate() + '/' + (date.getMonth() + 1) + '/' + date.getFullYear());

                $('#memoCreateDate').val(date.getDate() + '/' + (date.getMonth() + 1) + '/' + date.getFullYear());
                $('#memoUseDate').val(date.getDate() + '/' + (date.getMonth() + 1) + '/' + date.getFullYear());
                var myDate = date.getDate() + '/' + (date.getMonth() + 1) + '/' + date.getFullYear();
                $('#memoEndDate').val(moment(myDate, "DD/MM/YYYY").add(1, 'months').format("DD/MM/YYYY"));
                if ($('#addnew').is(':visible')) {
                    e.preventDefault();
                    $('#addnew').fadeOut();
                    GetMemoData();

                    $('#memo-table').removeClass('hide');
                    $('#memo-table').fadeIn();
                    $('.btnAddMemo').text('สร้างใหม่');

                    $('.btnMemoSave').removeClass('hide');

                    $('.btnMemoPrintView').addClass('hide');
                    $('.btnMemoSendApprove').addClass('hide');

                    $('.btnConfirmMemo').addClass('hide');
                    $('#decuid').val('');
                    $('#memoCreateDate').val('');
                    $('#memoUseDate').val('');
                    $('#memoEndDate').val('');
                    $('#FullName').val('');

                    $('#tbApprover').val('');

                    if (RoleCode == "MKT") {
                        $('#txtareaReason').val('Project : \nObjective : \nCondition : \n\nLead-Time-To Use : \nLC Name : \nDelivery Date : ');
                        $('#lbReasonRemark').html('* กรุณาระบุข้อมูล Project, Objective, Condition, Lead-Time-To Use, LC Name, Delivery Date');
                    } else {
                        $('#txtareaReason').val('');
                        $('#lbReasonRemark').html('');
                    }

                    $('#imemobody').empty();

                    $('#accordion').fadeIn();
                } else {
                    $('#addnew').removeClass('hide');
                    $('#addnew').fadeIn();

                    if ($.fn.DataTable.isDataTable('#memo-table')) {
                        var dt = $('#memo-table').DataTable();
                        dt.destroy();
                    }

                    $('#memo-table').addClass('hide');
                    $('#memo-table').fadeOut();
                    $('.btnAddMemo').text('ปิด');
                    $('#accordion').fadeOut();
                    GetCostCenter();

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
                            GetName();


                            $.unblockUI();
                        }, 1000);
                }
            })
            .on('click', '.btnMemoSave', function (e) {
                if ($('#decuid').val() == "") {
                    savemode = 0;
                }
                else {
                    savemode = 1
                }

                if (SaveItemMemo()) {

                    $('.btnMemoPrintView').removeClass('hide');
                    $('.btnMemoSendApprove').removeClass('hide');

                }
            })
            .on('click', '#clsBtnRej', function (e) {
                if ($('#rejId').val() != '') {
                    var docno = GUID.split(':')[1];
                    var reason = $('#rejId').val();
                    $.ajax({
                        type: "POST",
                        url: "MemoRequestForm.aspx/UpdateRejectreason",
                        contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify({ docno: docno, reason: reason }),
                        dataType: 'json',
                        async: false,
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                                textStatus + "\n\nError: " + errorThrown);
                        },
                        success: function (result) {
                            $('#iApproved').modal('toggle');
                            GetMemoData();
                        }
                    });
                }
            })
            .on('click', '.btnMemoPrintView', function (e) {

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
                        GenerateRpt();
                        var iframe = '<div class="iframe-container"><iframe src="' + pdf_link + '"></iframe></div>';
                        console.log(pdf_link);
                        var pdf_link_length = pdf_link.split('/').length - 1;
                        $.createModal({
                            title: pdf_link.split('/')[pdf_link_length],
                            message: iframe,
                            closeButton: true,
                            scrollable: false
                        });
                        $.unblockUI();
                    }, 1000);
            })
            .on('click', '.btnMemoSendApprove', function (e) {

                if (GetMemoDataByDocIDInFn($('#decuid').val()) === false) {
                    bootbox.alert('หน่วยงาน/โครงการ ยังไม่ได้กำหนดผู้อนุมัติ PR กรุณาติดต่อ Itconsult yy');
                    $.unblockUI();
                    return;
                }
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
                        GenerateRpt();
                        GetUser();
                        GetUserPosition();
                        mailBodyStr = '<p>เรื่อง ขออนุมัติจัดซื้อรายการส่งเสริมการขาย</p>' +
                            '<p>&nbsp;&nbsp;&nbsp;&nbsp;วัตถุประสงค์การขอซื้อ: {0}</p>' +
                            '<p>&nbsp;&nbsp;&nbsp;&nbsp;คำขอเลขที่: {1} วันที่: {2}</p>' +
                            '<p>&nbsp; &nbsp; {3}</p>' +
                            '<p>ชื่อ {4}</p>' +
                            '<p>ตำแหน่ง {5}</p>' +
                            '<p>เบอร์โทร {6}</p>';
                        $('#toApprove').val(listApproverName.replace('null', ''));
                        //alert(listApproverName);

                        var pdf_link_length = pdf_link.split('/').length - 1;
                        $('#rptPdf').text(pdf_link.split('/')[pdf_link_length]);
                        $('#rptPdf').attr('href', pdf_link);

                        var tmpMailBodyStr = '<p>เรื่อง ขออนุมัติจัดซื้อรายการส่งเสริมการขาย</p>' +
                            '<p>&nbsp;&nbsp;&nbsp;&nbsp;วัตถุประสงค์การขอซื้อ: <b>' +
                            $('#txtareaReason').val() +
                            '</b></p>' +
                            '<p>&nbsp;&nbsp;&nbsp;&nbsp;คำขอเลขที่: <b>' +
                            $('#decuid').val() +
                            '</b> วันที่: <b>' +
                            $('#memoCreateDate').val() +
                            '</b></p>';

                        //$('#fromUsr').val($('#FullName').val());
                        $('#position').val();
                        $('#tel').val();

                        if (RoleCode != "MKT") {
                            $('#subject').val($('#subject').val().replace('Marketing ', ''));
                        }

                        $('.summernote').summernote('code', tmpMailBodyStr);

                        $('#iSendEmailApprove').modal('toggle');

                        $.unblockUI();
                    }, 1000);
            })
            .on('click', '.btnSendApprove', function (e) {
                $.blockUI({
                    baseZ: 50000,
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
                        SendEmail();

                        $.unblockUI();
                    }, 1000);

            })
            .on('click', '.btnEditClick', function (e) {
                savemode = 1;
                $('.totals').text('-');
                var docno = e.currentTarget.id;
                GetCostCenter();
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
                GetMemoDataByDocIDCallBack(docno);
            })
            .on('click', '.btnDelReqClick', function (e) {
                var docno = e.currentTarget.id;
                bootbox.confirm("<center><h4>คุณต้องการลบรายการ Memo เลขที่: " + e.currentTarget.id + "<br />ใช่หรือไม่!</h4></center>", function (result) {
                    if (result) {
                        DeleteMemoRequest(docno);
                    }
                });
            })
            .on('click', '.btnMemoItemAdd', function (e) {
                $('#cbxitems').val('');
                $('#cbxitems option:selected').text('');
                $('#cbxitems').trigger("chosen:updated");
                $('#price').val('');
                $('#quantity').val('');
                $('#total-price').val('');
            })
            .on('change', '#cbxitems', function (e) {
                var _price = parseFloat($('#cbxitems option:selected').val().split(':')[1]);
                var price = _price.toFixed(2).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                $('#price').val(price);

                if ($('#quantity').val() != '') {
                    var total = parseFloat($('#price').val().replace(',', '')) * $('#quantity').val();
                    $('#total-price').val(total.toFixed(2).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                }
            })
            .on('keyup', '#quantity', function (e) {
                var total = parseFloat($('#price').val().replace(',', '')) * $('#quantity').val();
                $('#total-price').val(total.toFixed(2).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
            })
            .on('click', '.btnItemSetToDatatable', function (e) {
                if (ValidateSaveToMemo()) {
                    SetItemToimemo();
                    $('#iMemoItemAdd').modal('toggle');
                }
            })
            .on('click', '.btnDelClick', function (e) {
                var _this = this;
                var itemPrice = "";

                console.log(e.currentTarget.parentElement.parentNode.cells['5'].innerText);

                if (RoleCode == 'MKT') {
                    itemPrice = e.currentTarget.parentElement.parentNode.cells['5'].innerText;
                } else {
                    itemPrice = e.currentTarget.parentElement.parentNode.cells['7'].innerText;
                }

                var itemName = e.currentTarget.parentElement.parentNode.cells['1'].innerText;

                var total;
                total = parseFloat($('.totals').text().replace(',', '')) - parseFloat(itemPrice.replace(',', ''));

                $('.totals').text(total.toLocaleString('en'));

                //$('.totals').text(parseFloat($('.totals').text().replace(',', '')) - parseFloat(itemPrice.replace(',', ''))).toFixed(2).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                $(_this).closest('tr').remove();
                savemode = 1;
            })
            .on('click', '#clear-search', function (e) {
                location.reload();
            })
            .on('click', '#form-search', function (e) {

                //if ($('#STDDATE').val() == '') {
                //    MemoAlert('กรุณาเลือกวันที่เริ่มต้น', 'STDDATE');
                //    return;
                //}
                //else if ($('#ENDDATE').val() == '') {
                //    MemoAlert('กรุณาเลือกวันที่สิ้นสุด', 'ENDDATE');
                //    return;
                //}

                GetMemoData();
            });

        function GetCostCenter() {
            $.ajax({
                type: "POST",
                url: "MemoRequestForm.aspx/GetCostCenter",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                async: false,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                        textStatus + "\n\nError: " + errorThrown);
                },
                success: function (result) {
                    try {
                        $('#cbxCostCenter').empty();

                        var cbxCostCenterItem = '';
                        $.each(result, function (k, Val) {
                            for (var i = 0; i < Val.ListCostCenter.length; i++) {
                                cbxCostCenterItem += '<option value="' + Val.ListCostCenter[i].CostCenterID + '">' + Val.ListCostCenter[i].CostCenterID + ' : ' + Val.ListCostCenter[i].CostCenterName + '</option>';
                            }
                        });

                        $('#cbxCostCenter').append(cbxCostCenterItem);
                        $('#cbxCostCenter').trigger("chosen:updated");
                        $('.btnMemoSave').removeClass('hide');

                    } catch (e) {
                        console.log('Error GetCostCenter()')
                        console.log(e)
                    }
                }
            });

            var MemoType = '<%=Session["RoleCode"] %>';
            var EmpId = '<%=Session["EmpId"] %>';

            console.log(MemoType);
            console.log(EmpId);

            //if (MemoType == 'MKT') {
            //    if (EmpId == 'AP001167' || EmpId == 'AP003588') {
            //        $('#cbxCostCenter').attr('disabled', false);
            //    } else {
            //        $('#cbxCostCenter').attr('disabled', true);
            //    }
            //}
            //else {
            //    $('#cbxCostCenter').attr('disabled', false);
            //}

        }

        function GetName() {
            $.ajax({
                type: "POST",
                url: "MemoRequestForm.aspx/GetFullName",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
                },
                success: function (result) {
                    try {
                        var res = result.d.split(':');

                        $('#FullName').val(res[0] + ':' + res[1] + ' ' + res[2]);

                        $('#cbxCostCenter').val(res[3]);
                        $('#cbxCostCenter').trigger("chosen:updated");

                        $('#cbxUserName ').val('');
                        $('#cbxUserName ').trigger("chosen:updated");

                        $('#cbxPromotion ').val('');
                        $('#cbxPromotion ').trigger("chosen:updated");

                        $('#cbxGL ').val('');
                        $('#cbxGL ').trigger("chosen:updated");

                        $('#cbxObj ').val('');
                        $('#cbxObj ').trigger("chosen:updated");

                    } catch (e) { }
                }
            });
        }

        function DeleteMemoRequest(docno) {
            $.ajax({
                type: "POST",
                url: "MemoRequestForm.aspx/DeleteMemoRequest",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ docno: docno }),
                dataType: 'json',
                async: false,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
                },
                success: function (result) {
                    GetMemoData();
                }
            });
        }

        function GetUser() {
            optApprove = '<option value="">  </option>';
            $.ajax({
                type: "POST",
                url: "MemoRequestForm.aspx/GetUser",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                async: false,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
                },
                success: function (result) {
                    try {
                        ListItems = result;
                        $.each(result, function (k, Val) {
                            for (var i = 0; i < Val.length; i++) {

                                if (RoleCode == "MKT") {
                                    optApprove += '<option value="' + Val[i].UserName + ' <' + Val[i].UserEmail + '>' + '" selected>' + Val[i].UserName + '</option>';
                                } else {
                                    optApprove += '<option value="' + Val[i].UserName + ' <' + Val[i].UserEmail + '>' + '">' + Val[i].UserName + '</option>';
                                }
                            }
                        });

                        $('#ccApprove').append(optApprove);
                        $('#ccApprove').val('');
                        $('#ccApprove').trigger("chosen:updated");
                    } catch (e) {
                        console.log(e);
                    }
                }
            });
        }

        function GetUserPosition() {
            $.ajax({
                type: "POST",
                url: "MemoRequestForm.aspx/GetUserPositionByDocNo",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ docno: $('#decuid').val() }),
                dataType: 'json',
                async: false,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
                },
                success: function (result) {
                    try {

                        if (result.d.split('|').length == 3) {
                            $('#fromUsr').val(result.d.split('|')[0]);
                            $('#position').val(result.d.split('|')[1]);
                            $('#tel').val(result.d.split('|')[2]);
                        } else {
                            $('#fromUsr').val(result.d.split('|')[0]);
                            $('#position').val(result.d.split('|')[1]);
                            $('#tel').val('-');
                        }

                    } catch (e) {
                    }
                }
            });
        }

        function SetItems() {
            optItems = '<option value="">  </option>';
            $.ajax({
                type: "POST",
                //url: "MemoRequestForm.aspx/GetItems",
                // คิม kim
                url: "MemoRequestForm.aspx/GetAllMatItems",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                async: false,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                        textStatus + "\n\nError: " + errorThrown);
                },
                success: function (result) {
                    try {
                        ListItems = result;
                        $.each(result, function (k, Val) {
                            for (var i = 0; i < Val.length; i++) {
                                optItems += '<option value="' + Val[i].ItemNo + ':' + Val[i].UnitPrice + ':' + Val[i].ItemUnit + '">' + Val[i].ItemNo + ' - ' + Val[i].ItemName + ' ราคา ' + numberWithCommas(Val[i].UnitPrice) + '</option>';
                            }
                        });
                        $('#cbxitems').append(optItems);
                        $('#cbxitems').trigger("chosen:updated");
                    } catch (e) {

                    }
                }
            });
        }

        function MemoAlert(str, ctlName) {
            bootbox.alert({
                message: '<h2><center>' + str + '</center></h2>',
                callback: function () {
                    $('#' + ctlName + '').focus();
                }
            });
        }

        function numberWithCommas(x) {
            return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        }

        function GetMemoDataByDocID(docid) {
            $.ajax({
                type: "POST",
                url: "MemoRequestForm.aspx/GetDataMemoByDocID",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ DocNo: docid }),
                dataType: 'json',
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                        textStatus + "\n\nError: " + errorThrown);
                },
                success: function (result) {
                    $('#imemobody').empty();
                    approverEmail = '';
                    listApproverName = '';
                    try {
                        $.each(result, function (k, Val) {
                            $.each(Val, function (i, res) {
                                $('#decuid').val(res.DocNo);
                                $('#memoCreateDate').val(res.CreateDate);
                                $('#memoUseDate').val(res.UsingDate);
                                $('#memoEndDate').val(res.EndingDate);

                                if (RoleCode == 'Admin') {
                                    $('#cbxUserName ').val(res.EmpCode);
                                    $('#cbxUserName ').trigger("chosen:updated");

                                    $('#cbxPromotion ').val(res.PromotionID);
                                    $('#cbxPromotion ').trigger("chosen:updated");

                                    $('#cbxGL ').val(res.GLNO);
                                    $('#cbxGL ').trigger("chosen:updated");

                                    $('#cbxObj ').val(res.ObjID);
                                    $('#cbxObj ').trigger("chosen:updated");
                                }
                                else {
                                    $('#FullName').val(res.EmpCode.toUpperCase() + ' : ' + res.UserCreateName);

                                    $('#cbxPromotion ').val(res.PromotionID);
                                    $('#cbxPromotion ').trigger("chosen:updated");

                                    $('#cbxGL ').val(res.GLNO);
                                    $('#cbxGL ').trigger("chosen:updated");

                                    $('#cbxObj ').val(res.ObjID);
                                    $('#cbxObj ').trigger("chosen:updated");
                                }

                                $('#cbxCostCenter option:selected').text(res.CostCenterCode + ' : ' + res.CostCenterName);
                                $('#cbxCostCenter').trigger("chosen:updated");

                                $('#txtareaReason').val(res.Reason);

                                ApproveId = res.ApproveId;
                                SubApproveId = res.SubApproveId;
                                var totalsCount = 0;
                                $.each(res.DataItemsValue, function (i, resDetail) {
                                    totalsCount += parseFloat(resDetail.TotalPrice.replace(',', ''));
                                    var data = SetMemoDetails(resDetail);
                                    $('#imemo').append(data);
                                });
                                $('.totals').text(totalsCount.toFixed(2).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));

                                $.each(res.ApproveEmail, function (i, setMail) {
                                    SetEmail(setMail);
                                });

                                if (res.Status == 5) {
                                    $('.btnMemoSave').removeClass('hide').addClass('hide');
                                    $('.btnMemoPrintView').removeClass('hide');
                                    $('.btnMemoSendApprove').removeClass('hide').addClass('hide');

                                    $('.btnConfirmMemo').removeClass('hide');
                                    $('.btnConfirmMemo').text('ยกเลิกการจัดซื้อ')
                                    $('.btnConfirmMemo').removeClass('btn-success').addClass('btn-danger');
                                }
                                else if (res.Status == 3 || res.Status == 4) {
                                    $('.btnMemoSave').removeClass('hide').addClass('hide');
                                    $('.btnMemoPrintView').removeClass('hide');
                                    $('.btnMemoSendApprove').removeClass('hide').addClass('hide');

                                    if (RoleCode == "Admin" && res.Status == 3) {
                                        $('.btnConfirmMemo').removeClass('hide');
                                        $('.btnConfirmMemo').text('ยืนยันการจัดซื้อ');
                                    }
                                }
                                else if (res.Status == 2) {
                                    $('.btnMemoSave').removeClass('hide').addClass('hide');
                                    $('.btnMemoPrintView').removeClass('hide');
                                    $('.btnMemoSendApprove').removeClass('hide');
                                    $('.btnConfirmMemo').addClass('hide');
                                }
                                else {
                                    $('.btnMemoSave').removeClass('hide');
                                    $('.btnMemoPrintView').removeClass('hide');
                                    $('.btnMemoSendApprove').removeClass('hide');
                                    $('.btnConfirmMemo').addClass('hide');
                                }
                            });
                        });
                    } catch (e) { }

                    $('#addnew').removeClass('hide');
                    $('#addnew').fadeIn();
                    GetMemoData();

                    var dt = $('#memo-table').DataTable();
                    dt.destroy();
                    $('#memo-table').removeClass('hide').addClass('hide');
                    $('#memo-table').fadeOut();
                    $('#accordion').fadeOut();
                    $('.btnAddMemo').text('ปิด');
                }
            });
        }

        function GetMemoDataByDocIDCallBack(docid) {
            $.ajax({
                type: "POST",
                url: "MemoRequestForm.aspx/GetDataMemoByDocID",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ DocNo: docid }),
                dataType: 'json',
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
                },
                success: function (result) {
                    console.log(result);
                    $('#imemobody').empty();
                    approverEmail = '';
                    listApproverName = '';

                    if (result.d.length === 0) {
                        bootbox.alert('หน่วยงาน/โครงการ ยังไม่ได้กำหนดผู้อนุมัติ PR กรุณาติดต่อ Itconsult');
                        $.unblockUI();
                        return;
                    }

                    try {
                        $.each(result, function (k, Val) {
                            $.each(Val, function (i, res) {
                                $('#decuid').val(res.DocNo);
                                $('#memoCreateDate').val(res.CreateDate);
                                $('#memoUseDate').val(res.UsingDate);
                                $('#memoEndDate').val(res.EndingDate);

                                DocType = res.DocType;

                                if (RoleCode == 'Admin') {

                                    $('#cbxUserName').val(res.EmpCode);
                                    $('#cbxUserName').trigger("chosen:updated");
                                    $('#cbxPromotion').val('' + res.PromotionID + '');
                                    $('#cbxPromotion').trigger("chosen:updated");
                                    console.log(res.PromotionID);

                                    $('#cbxGL').val('' + res.GLNO + '');
                                    $('#cbxGL').trigger("chosen:updated");
                                    console.log(res.GLNO);

                                    $('#cbxObj').val('' + res.ObjID + '');
                                    $('#cbxObj').trigger("chosen:updated");

                                    CheckUserRole(res.EmpCode);
                                }
                                else {
                                    $('#FullName').val(res.EmpCode.toUpperCase() + ' : ' + res.UserCreateName);

                                    $('#cbxPromotion').val(res.PromotionID);
                                    $('#cbxPromotion').trigger("chosen:updated");

                                    $('#cbxGL').val(res.GLNO);
                                    $('#cbxGL').trigger("chosen:updated");

                                    $('#cbxObj').val(res.ObjID);
                                    $('#cbxObj').trigger("chosen:updated");

                                    CheckUserRole(res.EmpCode);
                                }

                                $('#cbxCostCenter option:selected').text(res.CostCenterCode + ' : ' + res.CostCenterName);
                                $('#cbxCostCenter').trigger("chosen:updated");
                                $('#txtareaReason').val(res.Reason);

                                ApproveId = res.ApproveId;
                                SubApproveId = res.SubApproveId;
                                var totalsCount = 0;

                                $.each(res.DataItemsValue, function (i, resDetail) {
                                    totalsCount += parseFloat(resDetail.TotalPrice.replace(',', ''));
                                    var data = SetMemoDetails(resDetail);
                                    $('#imemo').append(data);
                                });

                                $('.totals').text(totalsCount.toFixed(2).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));

                                $.each(res.ApproveEmail, function (i, setMail) {
                                    SetEmail(setMail);
                                });

                                if (res.Status == 5) {
                                    $('.btnMemoSave').removeClass('hide').addClass('hide');
                                    $('.btnMemoPrintView').removeClass('hide');
                                    $('.btnMemoSendApprove').removeClass('hide').addClass('hide');

                                    $('.btnConfirmMemo').removeClass('hide');
                                    $('.btnConfirmMemo').text('ยกเลิกการจัดซื้อ')
                                    $('.btnConfirmMemo').removeClass('btn-success').addClass('btn-danger');
                                }
                                else if (res.Status == 3 || res.Status == 4) {
                                    $('.btnMemoSave').removeClass('hide').addClass('hide');
                                    $('.btnMemoPrintView').removeClass('hide');
                                    $('.btnMemoSendApprove').removeClass('hide').addClass('hide');

                                    if (RoleCode == "Admin" && res.Status == 3) {
                                        $('.btnConfirmMemo').removeClass('hide');
                                        $('.btnConfirmMemo').text('ยืนยันการจัดซื้อ');
                                    }
                                }
                                else if (res.Status == 2) {
                                    $('.btnMemoSave').removeClass('hide').addClass('hide');
                                    $('.btnMemoPrintView').removeClass('hide');
                                    $('.btnMemoSendApprove').removeClass('hide');
                                    $('.btnConfirmMemo').addClass('hide');
                                }
                                else {
                                    $('.btnMemoSave').removeClass('hide');
                                    $('.btnMemoPrintView').removeClass('hide');
                                    $('.btnMemoSendApprove').removeClass('hide');
                                    $('.btnConfirmMemo').addClass('hide');
                                }
                            });
                        });
                    } catch (e) {
                    }

                    $('#addnew').removeClass('hide');
                    $('#addnew').fadeIn();
                    GetMemoData();

                    var dt = $('#memo-table').DataTable();
                    dt.destroy();
                    $('#memo-table').removeClass('hide').addClass('hide');
                    $('#memo-table').fadeOut();
                    $('#accordion').fadeOut();
                    $('.btnAddMemo').text('ปิด');

                    if (DocType == 'M-TYPE') {
                        $('.tblProjectId').removeClass('hide').addClass('hide');
                        $('.tblProjectName').removeClass('hide').addClass('hide');
                    } else {
                        $('.tblProjectId').removeClass('hide');
                        $('.tblProjectName').removeClass('hide');
                    }

                }
            });
        }

        function CheckingPR(costCenterNo, totalPrice, docType, docNo) {
            var ret;
            $.ajax({
                type: "POST",
                url: "MemoRequestForm.aspx/CheckingPR",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ costCenterNo: costCenterNo, totalPrice: totalPrice, docType: docType, docNo: docNo }),
                dataType: 'json',
                async: false,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
                },
                success: function (result) {
                    try {
                        var res = result.d;
                        ret = res[0];
                    } catch (e) {
                        return false;
                    }
                }
            });
            return ret;
        }

        function CheckUserRole(UserName) {
            setTimeout(
                function () {

                    $.ajax({
                        type: "POST",
                        url: "MemoRequestForm.aspx/CheckMarketingUser",
                        contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify({ empcode: UserName }),
                        dataType: 'json',
                        async: false,
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
                        },
                        success: function (result) {

                            try {
                                var res = result.d;
                                if (res.IsMKT) {
                                    $('.isnotmkt').removeClass('hide').addClass('hide');
                                    $('.projectRow').removeClass('hide').addClass('hide');
                                }
                                else {
                                    $('.isnotmkt').removeClass('hide');
                                    $('.projectRow').removeClass('hide');
                                }

                            } catch (e) { }
                        }
                    });
                    $.unblockUI();
                }, 1000);
        }

        function GetMemoDataByDocIDInFn(docid) {
            $.ajax({
                type: "POST",
                url: "MemoRequestForm.aspx/GetDataMemoByDocID",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ DocNo: docid }),
                dataType: 'json',
                async: false,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                        textStatus + "\n\nError: " + errorThrown);
                },
                success: function (result) {

                    if (result.d.length == 0) {
                        return false;
                    }

                    $('#imemobody').empty();
                    approverEmail = '';
                    listApproverName = '';
                    try {
                        $.each(result, function (k, Val) {
                            $.each(Val, function (i, res) {
                                $('#decuid').val(res.DocNo);
                                $('#memoCreateDate').val(res.CreateDate);
                                $('#memoUseDate').val(res.UsingDate);
                                $('#memoEndDate').val(res.EndingDate);
                                $('#FullName').val($('#lbUserName').text());

                                $('#cbxCostCenter option:selected').text(res.CostCenterCode + ' : ' + res.CostCenterName);
                                $('#cbxCostCenter').trigger("chosen:updated");
                                $('#txtareaReason').val(res.Reason);

                                ApproveId = res.ApproveId;
                                SubApproveId = res.SubApproveId;
                                var totalsCount = 0;
                                $.each(res.DataItemsValue, function (i, resDetail) {
                                    totalsCount += parseFloat(resDetail.TotalPrice.replace(',', ''));
                                    var data = SetMemoDetails(resDetail);
                                    $('#imemo').append(data);
                                });
                                $('.totals').text(totalsCount.toFixed(2).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                                $.each(res.ApproveEmail, function (i, setMail) {
                                    SetEmail(setMail);
                                });

                            });
                        });
                    } catch (e) { }
                    return true;
                }
            });
        }

        function SetEmail(res) {
            approverEmail += res.ApproveName + ' <' + res.ApproveEmail + '>';

            if (res.AuthorityName !== "") {
                approverEmail += "; " + res.AuthorityName + " <" + res.AuthorityEmail + ">";
            }

            listApproverName += res.ApproveName;

            if (res.AuthorityName !== "") {
                listApproverName += "; " + res.AuthorityName + " <" + res.AuthorityEmail + ">";
            }

            subApproverEmail += res.AuthorityName + ' <' + res.AuthorityEmail + '>; ';
            lstApproverEmail = res.ApproveEmail;

            //$('#lbApprover').val(res.ApproveName);

            $('#tbApprover').val(res.ApproveName);

            //lbApprover
            //lbApprovedDate
        }

        function SetMemoDetails(res) {
            var data;
            data = '<tr>' +
                (DocType == 'M-TYPE' ? '' :
                    '<td class="center">' + res.ProjectId + '</td>' +
                    '<td class="center">' + res.ProjectName + '</td>'
                ) +

                '<td class="center">' + res.ItemNo + '</td>' +
                '<td class="align-left">' + res.ItemName + '</td>' +
                '<td class="align-right">' + res.PricePerUnit + '</td>' +
                '<td class="align-right">' + res.Quantity + '</td>' +
                '<td class="center">' + res.Type + '</td>' +
                '<td class="align-right">' + res.TotalPrice + '</td>' +
                '<td class="center">' +
                '<a href="" id="" onclick="return false;" class="btn btn-xs btn-danger btnDelClick">ลบ</a>' +
                '</td>' +
                '</tr>';
            return data;
        }

        function GetMemoData() {
            $.ajax({
                type: "POST",
                url: "MemoRequestForm.aspx/GetDataMemo",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ DocNo: $('#docId').val(), StdDate: $('#STDDATE').val(), EndDate: $('#ENDDATE').val(), status: $('#cbxstatus option:selected').val() }),
                dataType: 'json',
                async: false,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                        textStatus + "\n\nError: " + errorThrown);
                },
                success: function (result) {
                    if ($.fn.DataTable.isDataTable('#memo-table')) {
                        var dt = $('#memo-table').DataTable();
                        dt.destroy();
                    }
                    $('#memoDocBody').empty();
                    try {
                        $.each(result, function (k, Val) {
                            $.each(Val, function (i, res) {
                                var data = SetMemoSearchData(res);
                                $('#memo-table').append(data);
                            });
                        });

                        if (result.d.length == 0) {
                            $('#memo-table').append('<tr class="center"><th colspan="8">No data available</th></tr>');
                        }
                        else {
                            $('#memo-table').DataTable();
                        }

                    } catch (e) {
                    }
                }
            });
        }

        function SetMemoSearchData(res) {
            var data;
            //รอดำเนินการ
            //รออนุมัติ
            //อนุมัติ
            //ไม่อนุมัติ
            var status;
            if (res.STATUSNAME == 'รอดำเนินการ') {
                status = '<span class="label label-info" style="border-radius: 25px">' + res.STATUSNAME + '</span>';
            } else if (res.STATUSNAME == 'รออนุมัติ') {
                status = '<span class="label label-yellow" style="border-radius: 25px">' + res.STATUSNAME + '</span>';
            } else if (res.STATUSNAME == 'อนุมัติ') {
                status = '<span class="label label-success" style="border-radius: 25px">' + res.STATUSNAME + '</span>';
            } else if (res.STATUSNAME == 'ไม่อนุมัติ') {
                status = '<span class="label label-danger" style="border-radius: 25px">' + res.STATUSNAME + '</span>';
            } else if (res.STATUSNAME == 'เสร็จสิ้น') {
                status = '<span class="label label-hardgreen" style="border-radius: 25px">' + res.STATUSNAME + '</span>';
            }

            data = '<tr>' +
                '<td class="center style="width: 110px;">' +
                '<a href="" id="' + res.DOCNO + '" onclick="return false;" class="btn btn-xs btn-primary btnEditClick">แสดง</a>' +
                ' ' +
                (res.STATUSNAME != 'อนุมัติ' && res.STATUSNAME != 'เสร็จสิ้น'
                    ? '<a href="" id="' + res.DOCNO + '" onclick="return false;" class="btn btn-xs btn-danger btnDelReqClick">ลบ</a>'
                    : '<a href="" id="' + res.DOCNO + '" onclick="return false;" class="btn btn-xs btn-danger btnDelReqClick" disabled="disabled">ลบ</a>') +
                '</td>' +
                '<td class="center">' + res.DOCNO + '</td>' +
                '<td class="center">' + res.MEMOCREATEDATE + '</td>' +
                '<td style="width: 150px;">' + res.CREATORNAME + '</td>' +
                '<td class="center">' + res.CCNAME + '</td>' +
                '<td class="center">' + res.REASON + '</td>' +
                '<td class="center">' + status + '</td>' +
                '<td class="center">' + res.REJECTREASON + '</td>' +
                '</tr>';


            if (res == '') {
                data = '<tr class="center">No data avvailable</tr>'
            }

            return data;
        }

        function ValidateSaveToMemo() {
            if ($('#price').val() == 0) {
                bootbox.alert('<h2><center>สินค้าที่ท่านเลือก ยังไม่ได้ระบุราคา</center></h2>');
                return false;
            } else if ($('#cbxitems').val() == '') {
                bootbox.alert('<h2><center>กรุณาเลือก สินค้าที่ต้องการเบิก/ซื้อ</center></h2>');
                return false;
            } else if ($('#price').val() == '') {
                bootbox.alert('<h2><center>กรุณาเลือก สินค้าที่ต้องการเบิก/ซื้อ</center></h2>');
                return false;
            } else if ($('#quantity').val() == '') {
                bootbox.alert('<h2><center>กรุณาระบุจำนวนที่ต้องการเบิก/ซื้อ</center></h2>');
                return false;
            } else if ($('#total-price').val() == '') {
                bootbox.alert('<h2><center>กรุณาเลือก สินค้าที่ต้องการเบิก/ซื้อ</center></h2>');
                return false;
            }

            if (!$('.projectRow').hasClass('hide') && ($('#cbxProject').val() == '' || $('#cbxProject').val() == null)) {
                bootbox.alert('<h2><center>กรุณาเลือก โครงการ/หน่วยงาน</center></h2>');
                return false;
            }

            var quantity = $('#quantity').val();
            if (quantity !== "" && !$.isNumeric(quantity)) {
                bootbox.alert('<h2><center>กรุณาระบุจำนวนที่ต้องการเบิก/ซื้อ</center></h2>');
                $('#quantity').val('');
                $('#total-price').val('');
                return false;
            }

            if (quantity == 0) {
                bootbox.alert('<h2><center>กรุณาระบุจำนวนที่ต้องการเบิก/ซื้อ</center></h2>');
                $('#quantity').val('');
                $('#total-price').val('');
                return false;
            }

            return true;
        }

        function SaveItemMemo() {
            var isSave = false;

            var docType = '';

            if (RoleCode == 'MKT') {
                docType = 'M-TYPE';
            } else if (RoleCode == 'HR') {
                docType = 'H-TYPE';
            } else {
                docType = 'O-TYPE';
            }

            if (ValidateBeforeSave(docType)) {

                var dataItemsValue = [];

                var dataValue = [];

                for (var i = 0; i < $('#imemobody tr').length; i++) {

                    if (RoleCode == 'MKT') {
                        //alert('x');
                        dataItemsValue.push({
                            'ItemNo': $('#imemobody')[0].childNodes[($('#imemobody')[0].childNodes[0].nodeName == '#text' ? i + 1 : i)].childNodes[0].innerText,
                            'ItemName': $('#imemobody')[0].childNodes[($('#imemobody')[0].childNodes[0].nodeName == '#text' ? i + 1 : i)].childNodes[1].innerText,
                            'PricePerUnit': $('#imemobody')[0].childNodes[($('#imemobody')[0].childNodes[0].nodeName == '#text' ? i + 1 : i)].childNodes[2].innerText,
                            'Quantity': $('#imemobody')[0].childNodes[($('#imemobody')[0].childNodes[0].nodeName == '#text' ? i + 1 : i)].childNodes[3].innerText,
                            'Type': $('#imemobody')[0].childNodes[($('#imemobody')[0].childNodes[0].nodeName == '#text' ? i + 1 : i)].childNodes[4].innerText,
                            'TotalPrice': $('#imemobody')[0].childNodes[($('#imemobody')[0].childNodes[0].nodeName == '#text' ? i + 1 : i)].childNodes[5].innerText,
                        });
                    }
                    else {
                        //alert('y');
                        dataItemsValue.push({
                            'ProjectID': $('#imemobody')[0].childNodes[($('#imemobody')[0].childNodes[0].nodeName == '#text' ? i + 1 : i)].childNodes[0].innerText,
                            'ProjectName': $('#imemobody')[0].childNodes[($('#imemobody')[0].childNodes[0].nodeName == '#text' ? i + 1 : i)].childNodes[1].innerText,

                            'ItemNo': $('#imemobody')[0].childNodes[($('#imemobody')[0].childNodes[0].nodeName == '#text' ? i + 1 : i)].childNodes[2].innerText,
                            'ItemName': $('#imemobody')[0].childNodes[($('#imemobody')[0].childNodes[0].nodeName == '#text' ? i + 1 : i)].childNodes[3].innerText,
                            'PricePerUnit': $('#imemobody')[0].childNodes[($('#imemobody')[0].childNodes[0].nodeName == '#text' ? i + 1 : i)].childNodes[4].innerText,
                            'Quantity': $('#imemobody')[0].childNodes[($('#imemobody')[0].childNodes[0].nodeName == '#text' ? i + 1 : i)].childNodes[5].innerText,
                            'Type': $('#imemobody')[0].childNodes[($('#imemobody')[0].childNodes[0].nodeName == '#text' ? i + 1 : i)].childNodes[6].innerText,
                            'TotalPrice': $('#imemobody')[0].childNodes[($('#imemobody')[0].childNodes[0].nodeName == '#text' ? i + 1 : i)].childNodes[7].innerText,
                        });
                    }
                }

                if ($('#cbxUserName option:selected').val() != null) {
                    if ($('#lbUserId').text() != $('#cbxUserName option:selected').val()) {
                        docType = 'NONE-TYPE';
                    }
                }

                var costcenter = $('#cbxCostCenter option:selected').text().split(':')[1].trim();

                dataValue.push({
                    'DocType': docType,
                    'DocNo': $('#decuid').val(),
                    'CreateDate': $('#memoCreateDate').val(),
                    'UsingDate': $('#memoUseDate').val(),
                    'EndingDate': $('#memoEndDate').val(),
                    'UserCreateName': (RoleCode != 'Admin' ? $('#FullName').val().split(':')[1].trim() : $('#cbxUserName option:selected').text().split(':')[1].trim()),
                    'CostCenterCode': $('#cbxCostCenter option:selected').text().split(':')[0].trim(),
                    'CostCenterName': costcenter,  //$('#cbxCostCenter').text(),
                    'Reason': $('#txtareaReason').val(),
                    'DataItemsValue': dataItemsValue,
                    'PromotionID': (RoleCode != 'MKT' ? $('#cbxPromotion option:selected').attr('value') : ''),
                    'Promotion': (RoleCode != 'MKT' ? $('#cbxPromotion option:selected').text() : ''),
                    'GLNO': (RoleCode != 'MKT' ? $('#cbxGL option:selected').attr('value') : ''),
                    'GLName': (RoleCode != 'MKT' ? $('#cbxGL option:selected').text() : ''),
                    'ObjID': (RoleCode != 'MKT' ? $('#cbxObj option:selected').attr('value') : ''),
                    'ObjName': (RoleCode != 'MKT' ? $('#cbxObj option:selected').text() : ''),
                });

                if (RoleCode != 'Admin') {
                    var empid = $('#FullName').val().split(':')[0].trim();
                }
                else if (RoleCode == 'Admin') {
                    var empid = $('#cbxUserName option:selected').val().split(':')[0].trim();
                }

                $.ajax({
                    type: "POST",
                    url: "MemoRequestForm.aspx/SaveMemoItems",
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify({ savemode: savemode, DataValue: dataValue, EmpCode: empid }),
                    dataType: 'json',
                    async: false,
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                            textStatus + "\n\nError: " + errorThrown);
                    },
                    success: function (result) {
                        isSave = true;
                        $('#decuid').val(result.d.DOCNO);

                        bootbox.alert({
                            message: '<h2><center>บันทึกสำเร็จ</center></h2>',
                            callback: function () {
                                $('.btnMemoPrintView').prop('disabled', false);
                                $('.btnMemoSendApprove').prop('disabled', false);
                            }
                        });
                    }
                });

                $('#tbApprover').val('');

                return isSave;
            }
        }

        function ValidateBeforeSave(docType) {

            if ($('#txtareaReason').val() == '') {
                bootbox.alert('<h2><center>กรุณากรอก วัตถุประสงค์ หรือเหตุผลการขอซื้อ</center></h2>');
                return false;
            }
            if ($('#imemobody tr').length == 0) {
                bootbox.alert('<h2><center>กรุณาเพิ่มรายการเบิก/ซื้อ อย่างน้อย 1 รายการ</center></h2>');
                return false;
            }

            if ($('#cbxPromotion option:selected').val() == '' && !$('.isnotmkt').hasClass('hide')) {
                bootbox.alert('<h2><center>กรุณาเลือก ปรโมชั่น</center></h2>');
                return false;
            }
            if ($('#cbxGL option:selected').val() == '' && !$('.isnotmkt').hasClass('hide')) {
                bootbox.alert('<h2><center>กรุณาเลือก วัตถุประสงค์</center></h2>');
                return false;
            }
            if ($('#cbxObj option:selected').val() == '' && !$('.isnotmkt').hasClass('hide')) {
                bootbox.alert('<h2><center>กรุณาเลือก วิธีการแจก</center></h2>');
                return false;
            }
            if ($('#cbxUserName option:selected').val() == '') {
                bootbox.alert('<h2><center>กรุณาเลือก ชื่อผู้จัดทำเอกสาร</center></h2>');
                return false;
            }

            var costCenterNo = $('#cbxCostCenter option:selected').val() + '|' + $('#cbxCostCenter option:selected').text().split(' : ')[1];
            var totalPrice = $('.totals').text();

            var docNo = $('#decuid').val();

            if (CheckingPR(costCenterNo, totalPrice, docType, docNo) == false) {
                bootbox.alert('หน่วยงาน/โครงการ ยังไม่ได้กำหนดผู้อนุมัติ PR กรุณาติดต่อ Itconsult');
                return false;
            }

            return true;
        }

        function SetItemToimemo() {
            var iRow = $('#imemobody tr').length + 1;

            $('#imemobody').append(
                '<tr>' +

                (RoleCode == 'MKT' ? '' :
                    '<td class="center">' + $('#cbxProject option:selected').val() + '</td>' +
                    '<td class="align-left">' + $('#cbxProject option:selected').text() + '</td>'
                ) +

                '<td class="center">' + $('#cbxitems option:selected').val().split(':')[0] + '</td>' +
                '<td class="align-left">' + $('#cbxitems option:selected').text() + '</td>' +
                '<td class="align-right">' + $('#price').val().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</td>' +
                '<td class="align-right">' + parseInt($('#quantity').val()) + '</td>' +
                '<td class="center">' + $('#cbxitems option:selected').val().split(':')[2] + '</td>' +
                '<td class="align-right">' + $('#total-price').val().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</td>' +
                '<td class="center">' +
                '<a href="" id="' + $('#cbxitems option:selected').text() + '" onclick="return false;" class="btn btn-xs btn-danger btnDelClick">ลบ</a>' +
                '</td>' +
                '</tr>'
            );

            var totalscount = 0;
            if ($('.totals').text() == '-') {
                totalscount = parseFloat($('#total-price').val().replace(',', ''));
                $('.totals').text(totalscount.toFixed(2).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
            }
            else {
                totalscount = parseFloat($('.totals').text().replace(',', '')) + parseFloat($('#total-price').val().replace(',', ''));
                $('.totals').text(totalscount.toFixed(2).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
            }
        }

        function GenerateRpt() {
            $.ajax({

                type: "POST",
                url: "MemoRequestForm.aspx/GenerateReport",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ docno: $('#decuid').val(), roleCode: RoleCode }),
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

                    if (result.d === "") {
                        bootbox.alert("กรุณาตรวจสอบการตั้งค่า PR ของ โครงการ/หน่วยงาน ที่เลือก");
                        console.log('GenerateReport');
                        return false;
                    }

                    pdf_link = result.d;
                }
            });
        }

        function AlertData(res, text, fn) {
            GUID = res;
            msg = text;
            ApvOrRej = fn;
        }

        function ChangeUrl(page, url) {
            if (typeof (history.pushState) != "undefined") {
                var obj = { Page: page, Url: url };
                history.pushState(obj, obj.Page, obj.Url);
            } else {
                alert("Browser does not support HTML5.");
            }
        }

        function SendMainApproveEmail() {
            $.ajax({
                type: "POST",
                url: "MemoRequestForm.aspx/SendingEmail",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ docno: $('#decuid').val(), imessage: mailBodyStr }),
                dataType: 'json',
                async: false,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                        textStatus + "\n\nError: " + errorThrown);
                },
                success: function (result) {
                    MemoAlert((result.d == '' ? '<h2><center>ส่งอีเมลล์รายการขออนุมัติ<br />สำเร็จ!</center></h2>' : result.d), "Send Email");
                }
            });
        }

        function SendEmail() {

            var requeststr = '<p>พิจารณาเห็นสมควร&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; <b>' +
                '<a href="{0}">' +
                '<font color="#3984c6">อนุมัติ</font>' +
                '</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; ' +
                '<a href="{1}">' +
                '<font color="#ff0000">ไม่อนุมัติ</font>' +
                '</a></b><br></p>' +
                '<p>{2}</p>' +
                '<p><b>{3}</b><br></p>' +
                '<p><b>{4}</b></p>' +
                '<p><b>Tel:&nbsp;{5}</b></p>';

            var to, from, subject, body, requeststr, attachlink, fromname, position, tel, docno, fff;

            to = approverEmail;

            fff = $('#ccApprove').val();

            from = $('#toApprove').val();

            subject = $('#subject').val();
            attachlink = pdf_link;
            body = $('.summernote').val();

            fromname = $('#fromUsr').val();
            position = $('#position').val();
            tel = $('#tel').val();

            docno = $('#decuid').val();

            var dataValue = JSON.stringify({
                Approver: lstApproverEmail,
                //to: ApproveId, //approverEmail,
                to: to, //approverEmail,
                subject: $('#subject').val(),
                body: $('.summernote').val(),
                requeststr: requeststr,
                attachlink: pdf_link,
                fromname: $('#fromUsr').val(),
                position: $('#position').val(),
                tel: $('#tel').val(),
                docno: docno
            });

            $.ajax({
                type: "POST",
                url: "MemoRequestForm.aspx/SendEmailToApprover",
                contentType: 'application/json; charset=utf-8',
                data: dataValue,
                method: false,
                dataType: 'json',
                async: false,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                        textStatus + "\n\nError: " + errorThrown);
                },
                success: function (result) {
                    if (result.d != '') {
                        MemoAlert((result.d == '' ? '<h2><center>' + result.d + '<br />ไม่สำเร็จ!</center></h2>' : result.d), "Send Email");
                        $('#iSendEmailApprove').modal('toggle');
                    }

                    if (CCApproveEmailSending() == '') {
                        MemoAlert((result.d == '' ? '<h2><center>ส่งอีเมลล์รายการขออนุมัติ<br />สำเร็จ!</center></h2>' : result.d), "Send Email");
                        //$('.btnMemoSave').prop('disabled', true);
                        $('.btnMemoSave').addClass('hide');
                        $('#iSendEmailApprove').modal('toggle');
                    }

                }
            });
        }

        function CCApproveEmailSending() {
            var retData = '';
            var lstEmail = '';

            var requeststr = '</a></b><br></p>' +
                '<p>{0}</p>' +
                '<p><b>{1}</b><br></p>' +
                '<p><b>{2}</b></p>' +
                '<p><b>Tel:&nbsp;{3}</b></p>';

            docno = $('#decuid').val();
            var email = $('#ccApprove').val();

            if ((email || "") != "") {
                //kim alert('line : 2031');
                for (var i = 0; i < email.length; i++) {
                    lstEmail += email[i] + ';';
                }
            }
            //kim alert(subApproverEmail);
            //kim alert(lstEmail);

            var dataValue = JSON.stringify({
                to: lstEmail,
                subject: $('#subject').val(),
                body: $('.summernote').val(),
                requeststr: requeststr,
                attachlink: pdf_link,
                fromname: $('#fromUsr').val(),
                position: $('#position').val(),
                tel: $('#tel').val(),
                docno: docno
            });

            $.ajax({
                type: "POST",
                url: "MemoRequestForm.aspx/SendEmailToSubApprover",
                contentType: 'application/json; charset=utf-8',
                data: dataValue,
                method: false,
                dataType: 'json',
                async: false,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    retData = XMLHttpRequest.toString();
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                        textStatus + "\n\nError: " + errorThrown);
                },
                success: function (result) {
                    retData = result.d;
                    if (result.d != '') {
                        MemoAlert((result.d == '' ? '<h2><center>' + result.d + '<br />ไม่สำเร็จ!</center></h2>' : result.d), "Send Email");
                    }
                }
            });
            return retData;
        }

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- Modal Approve -->
    <div id="iApproved" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close hide" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title clsAddText">เหตุผลที่ไม่อนุมัติ</h4>
                </div>
                <div class="modal-body">
                    <div class="col-sm-12">
                        <input type="text" id="rejId" class="max-width" />
                    </div>
                    <br />
                    <br />
                </div>
                <div class="modal-footer">
                    <div class="pull-right">
                        <button type="button" class="btn btn-sm btn-danger" id="clsBtnRej" onclick="return false;">ไม่อนุมัติ</button>
                    </div>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>
    <!-- /.modal -->

    <!-- Modal Send Email Approve -->
    <div id="iSendEmailApprove" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">ส่งรายการขออนุมัติ</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <form class="form-horizontal">

                            <div class="form-group">
                                <label for="toApprove" class="col-sm-2 control-label align-right">To:</label>
                                <div class="col-sm-10">
                                    <input type="text" class="form-control max-width" id="toApprove" placeholder="" disabled="disabled" />
                                </div>
                            </div>
                            <br />
                            <br />
                            <div class="form-group">
                                <label for="ccApprove" class="col-sm-2 control-label align-right">CC:</label>
                                <div class="col-sm-10">
                                    <select multiple="" class="chosen-select form-control" id="ccApprove" data-placeholder="Choose a State...">
                                    </select>
                                </div>
                            </div>
                            <br />
                            <br />
                            <div class="form-group">
                                <label for="subject" class="col-sm-2 control-label align-right">Subject:</label>
                                <div class="col-sm-10">
                                    <input type="text" class="form-control max-width" id="subject" placeholder="" value="Marketing ขออนุมัติจัดซื้อรายการส่งเสริมการขาย" />
                                </div>
                            </div>
                            <br />
                            <br />
                            <div class="form-group">
                                <label for="attachfile" class="col-sm-2 control-label align-right">Attach File:</label>
                                <div class="col-sm-10">
                                    <a href="" id="rptPdf" target="_blank"></a>
                                    <%--../Report/ReportMemoRequest.rpt_20161026_153538.pdf--%>
                                </div>
                            </div>
                            <br />
                            <br />
                            <div class="form-group">
                                <label for="body" class="col-sm-2 control-label align-right">Detail:</label>
                                <div class="col-sm-10">
                                    <div id="body" class="summernote">
                                    </div>
                                </div>
                            </div>
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <div class="form-group">
                                <label for="fromUsr" class="col-sm-2 control-label align-right">From:</label>
                                <div class="col-sm-10">
                                    <div class="col-sm-5 no-padding">
                                        <input type="text" class="form-control max-width" id="fromUsr" placeholder="" value="" readonly="readonly" />
                                    </div>
                                    <div class="col-sm-7">
                                    </div>
                                </div>
                            </div>
                            <br />
                            <br />
                            <br />
                            <div class="form-group">
                                <label for="position" class="col-sm-2 control-label align-right">Position:</label>
                                <div class="col-sm-10">
                                    <div class="col-sm-5 no-padding">
                                        <input type="text" class="form-control max-width" id="position" placeholder="" value="" readonly="readonly" />
                                    </div>
                                    <div class="col-sm-7">
                                    </div>
                                </div>
                            </div>
                            <br />
                            <br />
                            <div class="form-group">
                                <label for="tel" class="col-sm-2 control-label align-right">Tel:</label>
                                <div class="col-sm-10">
                                    <div class="col-sm-5 no-padding">
                                        <input type="text" class="form-control max-width" id="tel" placeholder="" value="" />
                                    </div>
                                    <div class="col-sm-7">
                                    </div>
                                </div>
                            </div>
                        </form>
                    </div>

                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">ปิด</button>
                    <button type="button" class="btn btn-primary btnSendApprove">ส่งอนุมัติ</button>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>
    <!-- /.modal -->

    <!-- Modal MemoItem -->
    <div id="iMemoItemAdd" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">รายการสินค้า</h4>
                </div>
                <div class="modal-body">
                    <div class="row projectRow hide">
                        <div class="col-sm-3 align-right">
                            โครงการ/หน่วยงาน
                       
                        </div>
                        <div class="col-sm-9">
                            <select class="chosen-select form-control max-width" id="cbxProject" data-placeholder="">
                            </select>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-sm-3 align-right">
                            รายการสินค้า
                       
                        </div>
                        <div class="col-sm-9">
                            <select class="chosen-select form-control max-width" id="cbxitems" data-placeholder="">
                            </select>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-sm-3 align-right">
                            ราคาสินค้า
                       
                        </div>
                        <div class="col-sm-9">
                            <input type="text" class="align-right" id="price" value="" disabled="disabled" />
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-sm-3 align-right">
                            จำนวน
                       
                        </div>
                        <div class="col-sm-9">
                            <input type="text" class="align-right numeric" id="quantity" value="" />
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-sm-3 align-right">
                            มูลค่ารวม
                       
                        </div>
                        <div class="col-sm-9">
                            <input type="text" class="align-right" id="total-price" value="" disabled="disabled" />
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">ปิด</button>
                    <button type="button" class="btn btn-primary btnItemSetToDatatable">เลือก</button>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>
    <!-- /.modal -->

    <div class="row">
        <div class="col-md-12">
            <div id="accordion" class="accordion-style1 panel-group">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h4 class="panel-title">
                            <a class="accordion-toggle aToggleBtn" data-toggle="collapse" data-parent="#accordion" href="#collapseOne">
                                <i class="ace-icon fa fa-angle-down bigger-110" data-icon-hide="ace-icon fa fa-angle-down" data-icon-show="ace-icon fa fa-angle-right"></i>
                                Memo Request Form
                            </a>
                        </h4>
                    </div>
                    <div class="panel-collapse collapse in" id="collapseOne">
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-sm-12 margin-bottom-menu">
                                    <div class="col-sm-1"></div>

                                    <div class="col-sm-5">
                                        <div class="form-group">
                                            <label class="col-sm-5 control-label move-right" for="docSearchID">เลขที่เอกสารบันทึกขอเบิก</label>
                                            <div class="col-sm-7">
                                                <input id="docId" type="text" class="max-width" name="txtBox" value="" placeholder="เลขที่เอกสารบันทึกขอเบิก" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-sm-5">
                                        <div class="input-group input-group-sm">
                                            <span class="input-group-addon">วันที่สร้างคำขอ
                                            </span>
                                            <input type="text" id="STDDATE" class="form-control" placeholder="" readonly="readonly" />
                                            <span class="input-group-addon">
                                                <i class="ace-icon fa fa-calendar"></i>
                                            </span>
                                            <span class="input-group-addon" style="border: none; background-color: white;">~
                                            </span>
                                            <input type="text" id="ENDDATE" class="form-control" placeholder="" readonly="readonly" />
                                            <span class="input-group-addon">
                                                <i class="ace-icon fa fa-calendar"></i>
                                            </span>
                                        </div>
                                    </div>

                                    <div class="col-sm-1"></div>
                                </div>
                                <div class="col-sm-12 margin-bottom-menu">
                                    <div class="col-sm-1"></div>

                                    <div class="col-sm-5">
                                        <div class="form-group">
                                            <label class="col-sm-5 control-label move-right" for="docSearchID">สถานะเอกสาร</label>
                                            <div class="col-sm-7">
                                                <select class="chosen-select form-control max-width" id="cbxstatus" data-placeholder="">
                                                    <option value=""></option>
                                                    <option value="1">รอดำเนินการ</option>
                                                    <option value="2">รออนุมัติ</option>
                                                    <option value="3">อนุมัติ</option>
                                                    <option value="4">ไม่อนุมัติ</option>
                                                    <option value="5">เสร็จสิ้น</option>
                                                </select>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-sm-5">
                                    </div>

                                    <div class="col-sm-1"></div>
                                </div>

                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="col-sm-8"></div>
                                        <div class="col-sm-1">
                                            <button type="button" id="clear-search" class="btn btn-sm btn-white btn-info">
                                                <i class="ace-icon glyphicon glyphicon-refresh"></i>เคลียร์
                                           
                                            </button>
                                        </div>
                                        <div class="col-sm-2">
                                            <button type="button" id="form-search" class="btn btn-sm btn-white btn-primary max-width">
                                                <i class="ace-icon glyphicon glyphicon-search"></i>ค้นหา
                                           
                                            </button>
                                        </div>
                                        <div class="col-sm-1"></div>
                                    </div>
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
            <a href="" class="btn btn-sm btn-primary btnAddMemo" onclick="return false;">สร้างใหม่</a>

            <div id="addnew" class="hide">
                <br />
                <div class="panel panel-default">
                    <div class="panel-heading create-panel">สร้าง Memo ขอซื้อ (Marketing)</div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-sm-3 col-sm-offset-9">
                                <div class="input-group input-group-sm">
                                    <span class="input-group-addon" style="border: none; background-color: white;">เลขที่เอกสารบันทึกขอเบิก</span>
                                    <input type="text" id="decuid" class="form-control" disabled="disabled" />
                                </div>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <label class="col-sm-1 div-caption">วันที่สร้างคำขอ</label>
                            <div class="col-sm-3">
                                <div class="input-group input-group-sm">
                                    <input type="text" id="memoCreateDate" class="form-control" placeholder="" readonly="readonly" />
                                    <span class="input-group-addon">
                                        <i class="ace-icon fa fa-calendar"></i>
                                    </span>
                                </div>
                            </div>
                            <label class="col-sm-1 div-caption">วันที่ต้องการใช้</label>
                            <div class="col-sm-3">
                                <div class="input-group input-group-sm" style="width: 100%;">
                                    <input type="text" id="memoUseDate" class="form-control" placeholder="" readonly="readonly" />
                                    <span class="input-group-addon">
                                        <i class="ace-icon fa fa-calendar"></i>
                                    </span>
                                </div>
                            </div>
                            <label class="col-sm-1 div-caption">วันที่สิ้นสุดการขอใช้</label>
                            <div class="col-sm-3">
                                <div class="input-group input-group-sm" style="width: 100%;">
                                    <input type="text" id="memoEndDate" class="form-control" placeholder="" readonly="readonly" />
                                    <span class="input-group-addon">
                                        <i class="ace-icon fa fa-calendar"></i>
                                    </span>
                                </div>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <label class="col-sm-1 div-caption">ชื่อผู้จัดทำเอกสาร</label>
                            <div class="col-sm-3">
                                <div class="input-group input-group-sm" style="width: 100%;">
                                    <input type="text" id="FullName" class="max-width hide" disabled="disabled" />
                                    <select class="chosen-select form-control max-width hide" id="cbxUserName" data-placeholder="กรุณาเลือกผู้จัดทำเอกสาร">
                                    </select>
                                </div>
                            </div>
                            <label class="col-sm-1 div-caption">Cost Center</label>
                            <div class="col-sm-3">
                                <div class="input-group input-group-sm" style="width: 100%;">
                                    <select class="chosen-select form-control max-width hide" id="cbxCostCenter" data-placeholder="">
                                    </select>
                                </div>
                            </div>
                        </div>
                        <div class="isnotmkt hide">
                            <div class="row">
                                <label class="col-sm-1 div-caption">โปรโมชั่น</label>
                                <div class="col-sm-3">
                                    <div class="input-group input-group-sm" style="width: 100%;">
                                        <select class="chosen-select form-control max-width hide" id="cbxPromotion" data-placeholder="กรุณาเลือกโปรโมชั่น">
                                        </select>
                                    </div>
                                </div>
                                <label class="col-sm-1 div-caption">วัตถุประสงค์</label>
                                <div class="col-sm-3">
                                    <div class="input-group input-group-sm" style="width: 100%;">
                                        <select class="chosen-select form-control max-width" id="cbxGL" data-placeholder="กรุณาเลือกวัตถุประสงค์">
                                        </select>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="row isnotmkt hide">
                                <label class="col-sm-1 div-caption">วิธีการแจก</label>
                                <div class="col-sm-3">
                                    <div class="input-group input-group-sm" style="width: 100%;">
                                        <select class="chosen-select form-control max-width" id="cbxObj" data-placeholder="กรุณาเลือกวิธีการแจก">
                                        </select>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <label class="col-sm-1 div-caption">ผู้อนุมัติ</label>
                            <div class="col-sm-3">
                                <input id="tbApprover" type="text" class="max-width" name="txtBox" value="" placeholder="ผู้อนุมัติ" readonly />
                            </div>
                            <%--<label class="col-sm-1 div-caption">วันที่อนุมัติ</label>
                            <div class="col-sm-3">
                              <label id="lbApprovedDate" class="max-width" />
                            </div>--%>
                        </div>
                        <br />
                        <div class="col-sm-12" style="margin-bottom: 10px;">
                            <span>วัตถุประสงค์ หรือเหตุผลการขอซื้อ</span>
                            <textarea class="form-control" id="txtareaReason" rows="7"></textarea>
                        </div>
                        <div class="col-sm-12">
                            <label id="lbReasonRemark" style="color: #DC143C"></label>
                        </div>
                        <br />
                        <br />
                        <div class="col-sm-2 col-sm-offset-10 align-right">
                            <a href="#" class="btn btn-xs btn-primary btnMemoItemAdd" id="" data-toggle="modal"
                                data-target="#iMemoItemAdd" data-id="">เพิ่มสินค้าขอเบิก
                            </a>
                        </div>
                        <br />
                        <div class="col-sm-12">
                            <table id="imemo" class="table table-hover" cellspacing="0" width="100%">
                                <thead>
                                    <tr>
                                        <th colspan="10" class="align-left">รายการขอสั่งซื้อ</th>
                                    </tr>
                                    <tr>
                                        <th class="center tblProjectId">รหัสโครงการ/หน่วยงาน</th>
                                        <th class="center tblProjectName">โครงการ/หน่วยงาน</th>
                                        <th class="center">รหัส Material</th>
                                        <th class="center">รายละเอียด</th>
                                        <th class="center">ราคา/หน่วย</th>
                                        <th class="center">จำนวนขอซื้อ</th>
                                        <th class="center">หน่วยนับ</th>
                                        <th class="center">รวมมูลค่า</th>
                                        <th class="center">#</th>
                                    </tr>
                                </thead>
                                <tbody id="imemobody">
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <ul class="list-group">
                        <div class="row">
                            <div class="col-sm-8">
                            </div>
                            <b>
                                <div class="col-sm-2 align-right">
                                    รวมมูลค่าขอซื้อ:
                               
                                </div>
                                <div class="col-sm-2">
                                    <span class="totals">-</span>
                                </div>
                            </b>
                        </div>
                        <br />
                        <br />
                    </ul>
                    <ul class="list-group">
                        <li class="list-group-item align-right">
                            <button type="button" class="btn btn-xs btn-primary btnMemoSave">บันทึก</button>
                            <button type="button" class="btn btn-xs btn-primary hide btnMemoPrintView">แสดงเอกสาร</button>
                            <button type="button" class="btn btn-xs btn-primary hide btnMemoSendApprove">ส่งอนุมัติ</button>
                            <button type="button" class="btn btn-xs btn-success hide btnConfirmMemo">ยืนยันการจัดซื้อ</button>
                        </li>
                    </ul>
                </div>
            </div>

            <table id="memo-table" class="table table-hover" cellspacing="0" width="100%">
                <thead>
                    <tr>
                        <th colspan="8">รายการขอสั่งซื้อ</th>
                    </tr>
                    <tr>
                        <th style="width: 110px;"></th>
                        <th>เลขที่เอกสาร</th>
                        <th>วันที่สร้าง</th>
                        <th>ผู้จัดทำเอกสาร</th>
                        <th>หน่วยงานที่ขอเบิก</th>
                        <th>วัตถุประสงค์</th>
                        <th>สถานะ</th>
                        <th>หมายเหตุ</th>
                    </tr>
                </thead>
                <tbody id="memoDocBody">
                </tbody>
            </table>

        </div>
    </div>

</asp:Content>

