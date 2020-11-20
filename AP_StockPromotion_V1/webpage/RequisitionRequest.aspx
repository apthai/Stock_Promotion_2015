<%@ Page Title="Requisition Request" Language="C#" MasterPageFile="~/master/MasterPage.Master"
    AutoEventWireup="true" CodeBehind="RequisitionRequest.aspx.cs" Inherits="AP_StockPromotion_V1.webpage.RequisitionRequest" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        .setwidth-cbx {
            width: 35px;
        }

        .setwidth-textbox {
            width: 85px;
        }

        .setwidth-insidetable {
            width: 95px;
        }

        .setwidthstock-insidetable {
            width: 110px;
        }

        .setwidthproject-insidetable {
            width: 180px;
        }

        .setdown-margin-top {
            margin-top: -10px;
        }

        .setdown-margin-bottom {
            margin-bottom: -20px;
        }

        .modal-body {
            max-height: calc(100vh - 210px);
            overflow-y: auto;
        }


        .tool-tip {
            color: #fff;
            background-color: rgba( 0, 0, 0, .7);
            text-shadow: none;
            font-size: .8em;
            visibility: hidden;
            -webkit-border-radius: 7px;
            -moz-border-radius: 7px;
            -o-border-radius: 7px;
            border-radius: 7px;
            text-align: center;
            opacity: 0;
            z-index: 999;
            padding: 3px 8px;
            position: absolute;
            cursor: default;
            -webkit-transition: all 240ms ease-in-out;
            -moz-transition: all 240ms ease-in-out;
            -ms-transition: all 240ms ease-in-out;
            -o-transition: all 240ms ease-in-out;
            transition: all 240ms ease-in-out;
        }

            .tool-tip,
            .tool-tip.top {
                top: auto;
                bottom: 114%;
                left: 50%;
            }

                .tool-tip.top:after,
                .tool-tip:after {
                    position: absolute;
                    bottom: -12px;
                    left: 50%;
                    margin-left: -7px;
                    content: ' ';
                    height: 0px;
                    width: 0px;
                    border: 6px solid transparent;
                    border-top-color: rgba( 0, 0, 0, .7);
                }

                /* default heights, width and margin w/o Javscript */

                .tool-tip,
                .tool-tip.top {
                    width: 80px;
                    height: 22px;
                    margin-left: -43px;
                }



        /* on hover of element containing tooltip default*/

        *:not(.on-focus):hover > .tool-tip,
        .on-focus input:focus + .tool-tip {
            visibility: visible;
            opacity: 1;
            -webkit-transition: all 240ms ease-in-out;
            -moz-transition: all 240ms ease-in-out;
            -ms-transition: all 240ms ease-in-out;
            -o-transition: all 240ms ease-in-out;
            transition: all 240ms ease-in-out;
        }

        *:not(.on-focus) > .tool-tip.slideIn,
        *:not(.on-focus) > .tool-tip.slideIn.top,
        .on-focus > .tool-tip.slideIn,
        .on-focus > .tool-tip.slideIn.top {
            bottom: 50%;
        }

        *:not(.on-focus):hover > .tool-tip.slideIn,
        *:not(.on-focus):hover > .tool-tip.slideIn.top,
        .on-focus > input:focus + .tool-tip.slideIn,
        .on-focus > input:focus + .tool-tip.slideIn.top {
            bottom: 110%;
        }

        .align-right {
            text-align: right;
        }

        .max-width {
            width: 100%;
        }

        .width-ninety {
            width: 90%;
        }

        .margin-bottom-menu {
            margin-bottom: 5px;
        }

        .set-padding {
            padding: 0 0 0 0 !important;
        }

        .ui-dialog {
            top: 50px !important;
        }

        .ui-datepicker {
            z-index: 1150 !important;
        }

        .ui-autocomplete {
            z-index: 1150 !important;
        }

        .move-right {
            text-align: right !important;
        }

        .move-left {
            text-align: left !important;
        }

        .disabledbutton {
            pointer-events: none;
            opacity: 0.0;
        }

        .disabledClass {
            pointer-events: none;
            opacity: 0.85;
        }

        input:focus {
            outline: none;
            border-color: #9ecaed;
            box-shadow: 0 0 10px #9ecaed;
        }

        input[type="text"] {
            text-align: center;
        }

        ::-webkit-input-placeholder {
            text-align: center;
        }

        :-moz-placeholder { /* Firefox 18- */
            text-align: center;
        }

        ::-moz-placeholder { /* Firefox 19+ */
            text-align: center;
        }

        :-ms-input-placeholder {
            text-align: center;
        }
    </style>

    <script type="text/javascript">

        var CostCenter;
        var CostCenterForNewReq;
        var GLData;
        var Objective;
        var Projects;
        var PromotionItems;
        var Permission;
        var AllUser;
        var CLevelUser;
        var HeadOfUser;
        var LCMUser;
        var HRUser;
        var MKTUser;
        var ETCUser;

        var m_add = false;
        var m_edit = false;
        var m_view = false;
        var m_approve = false;
        var m_accept = false;
        var m_reject = false;

        $(document).ready(function () {

            $.blockUI(
                {
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

            setTimeout(function () {
                CostCenter = <%=Session["CostCenter"] %> ;
                CostCenterForNewReq = <%=Session["CostCenterForNewReq"] %> ;
                GLData = <%=Session["GL"] %>;
                Objective = <%=Session["Objective"] %>;
                Projects = <%=Session["Projects"] %>;
                PromotionItems = <%=Session["PromotionItems"] %>;
                Permission = <%=Session["Permission"] %>;
                AllUser = <%=Session["AllUser"] %>;
                CLevelUser = <%=Session["CLevelUser"] %>;
                HeadOfUser = <%=Session["HeadOfUser"] %>;
                LCMUser = <%=Session["LCMUser"] %>;
                HRUser = <%=Session["HRUser"] %>;
                MKTUser = <%=Session["MKTUser"] %>;
                ETCUser = <%=Session["ETCUser"] %>;

                if (Permission.MKT) {
                    $("#rqApprover").removeClass('hide').addClass('hide');
                    $("#rqApprover2").removeClass('hide').addClass('hide');
                }
                else if (Permission.MKT || Permission.Admin) {
                    $('#btnAdd').removeClass('hide');
                }
                else {
                    $('#btnAdd').removeClass('hide').addClass('hide');
                }

                if (Permission.MKT) {
                    $("#lbDetail2").html(" (โปรดระบุชื่อ LC รับสินค้าลงในช่องหมายเหตุ)");
                    $('#txtDetailRemark').removeClass('hide');
                }
                else {
                    $("#lbDetail2").html("");
                    $('#txtDetailRemark').removeClass('hide').addClass('hide');
                }

                $('#divNavx').html('ตั้งเบิกภายในองค์กร >> เอกสารตั้งเบิก');

                $("#RecieveDatePicker1").datepicker({
                    showOtherMonths: true,
                    selectOtherMonths: false,
                    dateFormat: 'dd/mm/yy'
                });
                $("#BookDatePicker1").datepicker({
                    showOtherMonths: true,
                    selectOtherMonths: false,
                    dateFormat: 'dd/mm/yy'
                });

                $('#recieveDocDatePick').blur();
                $("#recieveDocDatePick").datepicker({
                    showOtherMonths: true,
                    selectOtherMonths: false,
                    dateFormat: 'dd/mm/yy'
                });

                $('#bookDocDatePick').blur();
                $("#bookDocDatePick").datepicker({
                    showOtherMonths: true,
                    selectOtherMonths: false,
                    dateFormat: 'dd/mm/yy'
                });

                $("#setStdDatePicker").datepicker({
                    showOtherMonths: true,
                    selectOtherMonths: false,
                    dateFormat: 'dd/mm/yy'
                });
                $("#setEndDatePicker").datepicker({
                    showOtherMonths: true,
                    selectOtherMonths: false,
                    dateFormat: 'dd/mm/yy'
                });
                $("#a_recieveDocDatePick").datepicker({
                    showOtherMonths: true,
                    selectOtherMonths: false,
                    dateFormat: 'dd/mm/yy'
                });
                $("#a_bookDocDatePick").datepicker({
                    showOtherMonths: true,
                    selectOtherMonths: false,
                    dateFormat: 'dd/mm/yy'
                });


                var date = new Date();

                $('#RecieveDatePicker').val((date.getDate().length == 1 ? '0' + date.getDate().toString() : date.getDate()) + '/'
                    + ((date.getMonth() + 1).toString().length == 1 ? '0' + (date.getMonth() + 1) : (date.getMonth() + 1)) + '/'
                    + date.getFullYear());
                $('#BookDatePicker').val((date.getDate().length == 1 ? '0' + date.getDate().toString() : date.getDate()) + '/'
                    + ((date.getMonth() + 1).toString().length == 1 ? '0' + (date.getMonth() + 1) : (date.getMonth() + 1)) + '/'
                    + date.getFullYear());
                $('#RecieveDatePicker1').val((date.getDate().length == 1 ? '0' + date.getDate().toString() : date.getDate()) + '/'
                    + ((date.getMonth() + 1).toString().length == 1 ? '0' + (date.getMonth() + 1) : (date.getMonth() + 1)) + '/'
                    + date.getFullYear());
                $('#BookDatePicker1').val((date.getDate().length == 1 ? '0' + date.getDate().toString() : date.getDate()) + '/'
                    + ((date.getMonth() + 1).toString().length == 1 ? '0' + (date.getMonth() + 1) : (date.getMonth() + 1)) + '/'
                    + date.getFullYear());
                $('#recieveDocDatePick').val((date.getDate().length == 1 ? '0' + date.getDate().toString() : date.getDate()) + '/'
                    + ((date.getMonth() + 1).toString().length == 1 ? '0' + (date.getMonth() + 1) : (date.getMonth() + 1)) + '/'
                    + date.getFullYear());
                $('#bookDocDatePick').val((date.getDate().length == 1 ? '0' + date.getDate().toString() : date.getDate()) + '/'
                    + ((date.getMonth() + 1).toString().length == 1 ? '0' + (date.getMonth() + 1) : (date.getMonth() + 1)) + '/'
                    + date.getFullYear());
                $('#setStdDatePicker').val((date.getDate().length == 1 ? '0' + date.getDate().toString() : date.getDate()) + '/'
                    + ((date.getMonth() + 1).toString().length == 1 ? '0' + (date.getMonth() + 1) : (date.getMonth() + 1)) + '/'
                    + date.getFullYear());
                $('#setEndDatePicker').val((date.getDate().length == 1 ? '0' + date.getDate().toString() : date.getDate()) + '/'
                    + ((date.getMonth() + 1).toString().length == 1 ? '0' + (date.getMonth() + 1) : (date.getMonth() + 1)) + '/'
                    + date.getFullYear());


                $('#chosen-multiple-style .btn').on('click', function (e) {
                    var target = $(this).find('input[type=radio]');
                    var which = parseInt(target.val());
                    if (which == 2) $('#RefDocID').addClass('tag-input-style');
                    else $('#RefDocID').removeClass('tag-input-style');
                });

                $("#RefDocID").chosen(
                    { no_results_text: "No result found. Press enter to add " }
                );

                $("#cbxForMemo").on('change', function () {
                    var index = $("#cbxForMemo option:selected").index();

                    if (index > 0) {
                        $.ajax({
                            type: "POST",
                            url: "RequisitionRequest.aspx/GetCostCenterByMemoDoc",
                            contentType: 'application/json; charset=utf-8',
                            dataType: 'json',
                            data: JSON.stringify({
                                MemoDoc: $('#cbxForMemo option:selected').val()
                            }),
                            success: function (result) {

                                $.each(result, function (d, Val) {

                                if (Val.CostCenterID != "") {

                                    var CostCenter = '<option value="' + Val.CostCenterID + '" selected="selected">' + Val.CostCenterID + ' : ' + Val.CostCenterName + '</option>';

                                    $('#cbxForProject').empty();
                                    $('#cbxForProject').append(CostCenter);

                                    $('#cbxForProject').trigger("chosen:updated");
                                }

                                });

                                console.log(result);

                            }
                        });

                    }

                });



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

                $("#accordion").accordion({
                    collapsible: true,
                    heightStyle: "content",
                    animate: 250,
                    header: ".accordion-header"
                }).sortable({
                    axis: "y",
                    handle: ".accordion-header",
                    stop: function (event, ui) {
                        ui.item.children(".accordion-header").triggerHandler("focusout");
                    }
                }).css({ 'z-index': -1 });

                SetComboBox();

                $.ajax({
                    type: "POST",
                    url: "RequisitionRequest.aspx/GetDataRequisitionSearch",
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    data: JSON.stringify({
                        docSearchID: '',
                        tagSearchID: '',
                        RecieveDatePicker: '',
                        BookDatePicker: '',
                        cbxUser: '',
                        cbxPromotionType: '',
                        cbxCostCenter: '',
                        cbxObjective: '',
                        cbxGLNo: ''
                    }),
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                            textStatus + "\n\nError: " + errorThrown);
                    },
                    success: function (result) {
                        ClearData('#dynamic-table');
                        try {

                            $.each(result, function (k, Val) {

                                if (Val.MsgErr != '') {
                                    bootbox.alert('<h4><center>' + Val.MsgErr + '</center></h4>');
                                }

                                $.each(Val.lstrr, function (key, Value) {
                                    var data = RenderDatatable(Value);
                                    $('#iBody').append(data);
                                });
                            });

                        } catch (e) {

                        }
                        CallDatatable();
                    }
                });

                CallTableProduct();

                $.unblockUI();
            }, 1000);

            $("#btnAddDetail").on('click', function (e) {
                if (validate(4)) {
                    var projectID = $('#cbxAddProject').val();
                    var projectName = $('#cbxAddProject option:selected').text();
                    var promoItemID = $('#cbxAddPromoType').val();
                    var promoItemName = $('#cbxAddPromoType option:selected').text();
                    var strDate = $('#setStdDatePicker').val();
                    var endDate = $('#setEndDatePicker').val();
                    var reminder = $('#reminDay').val();
                    var items = $('#txtQuantity').val();

                    if ($.fn.dataTable.isDataTable('#table-product')) {
                        table = $('#table-product').DataTable({
                            retrieve: true,
                            paging: false
                        });
                        table.destroy();
                    }
                    var t = $('#table-product').DataTable({
                        "aoColumns": [
                            { 'visible': false },
                            null,
                            { 'visible': false },
                            null,
                            null,
                            null,
                            null,
                            null,
                            null
                        ]
                    });

                    t.row.add([
                        projectID,
                        projectName,
                        promoItemID,
                        promoItemName,
                        items,
                        strDate,
                        endDate,
                        reminder,
                        '<td>' +
                        '<div class="hidden-sm hidden-xs action-buttons">' +
                        '<a class="red" href="#" id="' + projectID + '" onclick="deleteUserGroup(' + projectID + ');"><i class="ace-icon fa fa-trash bigger-130"></i></a>' +
                        '</div>' +
                        '</td>'
                    ]).draw(false);
                }
            });

            $('#btnClearDetail').on('click', function (e) {
                $('#cbxAddPromoType').val('0').change();
                $('#cbxAddProject').val('0').change();

                $('#cbxAddPromoType').trigger("chosen:updated");
                $('#cbxAddProject').trigger("chosen:updated");

                $('#txtQuantity').val('');
                $('#setStdDatePicker').val('');
                $('#setEndDatePicker').val('');
                $('#reminDay').val('');
            });

            $("#RefDocID").autocomplete({
                source: function (request, response) {
                    var results = $.ui.autocomplete.filter(GL, request.term);

                    response(results.slice(0, 10));
                }
            });
            $("#tagsID").autocomplete({
                source: function (request, response) {
                    var results = $.ui.autocomplete.filter(GL, request.term);

                    response(results.slice(0, 10));
                }
            });

            $("#clear").on('click', function (e) {
                $("#docID").val("");
                $("#tagsID").val("");
                $("#RecieveDatePicker").val("");
                $("#BookDatePicker").val("");
                $("#user").val("");
                $("#txtType").val("");
                $("#txtDep").val("");
                $("#txtObj").val("");
                $("#txtGL").val("");
            });

            $.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
                _title: function (title) {
                    var $title = this.options.title || '&nbsp;'
                    if (("title_html" in this.options) && this.options.title_html == true)
                        title.html($title);
                    else title.text($title);
                }
            }));

            $("#btnAdd").on('click', function (e) {
                m_edit = false;
                m_add = true;
                m_view = false;
                m_approve = false;
                m_accept = false;
                m_reject = false;

                var dialog = $("#dialog-message").removeClass('hide').dialog({
                    width: screen.width / 100 * 80,
                    modal: true,
                    title: "<div class='widget-header widget-header-small'> " +
                        "<h4 class='smaller'><i class='ace-icon glyphicon glyphicon-plus'></i> สร้างเอกสารใหม่</h4></div>",
                    title_html: true,

                });

                if (!dialog.hasClass('hide')) {
                    $('#newDocID').blur();
                }

                $('#id-StepOne').removeClass('disabledClass');
                $('#id-StepTwo').removeClass('disabledClass');

                $("#RecieveDatePicker").datepicker("refresh");

                $("#BookDatePicker").datepicker("refresh");

                //$('#RecieveDatePicker').datepicker._clearDate(this);
                //$('#BookDatePicker').datepicker._clearDate(this);

                //$('#RecieveDatePicker').text('');
                //$('#BookDatePicker').text('');

                var date = new Date();

                $('#RecieveDatePicker').val((date.getDate().length == 1 ? '0' + date.getDate().toString() : date.getDate()) + '/' + (date.getMonth() + 1) + '/' + date.getFullYear());
                $('#BookDatePicker').val((date.getDate().length == 1 ? '0' + date.getDate().toString() : date.getDate()) + '/' + (date.getMonth() + 1) + '/' + date.getFullYear());

                $('#reminDay').val('');
                $('#txtQuantity').val('');
                $('#setStdDatePicker').val('');
                $('#setEndDatePicker').val('');
                $('#txtRemark').val('');
                $('#newDocID').val('');

                $('#cbxAddProject').val('');
                $('#cbxForProject').val('');
                $('#cbxAddPromoType').val('');
                $('#cbxAddCostCenter').val('');
                $('#cbxCostCenter').val('');
                $('#cbxAddPromotionType').val('');
                $('#cbxPromotionType').val('');
                $('#cbxAddApprover').val('');
                $('#cbxAddUser').val('');
                $('#cbxUser').val('');
                $('#cbxAddObjective').val('');
                $('#cbxObjective').val('');
                $('#cbxAddGLNo').val('');
                $('#cbxGLNo').val('');

                $('#cbxAddProject option:selected').text('');
                $('#cbxForProject option:selected').text('');
                $('#cbxAddPromoType option:selected').text('');
                $('#cbxAddCostCenter option:selected').text('');
                $('#cbxCostCenter option:selected').text('');
                $('#cbxAddPromotionType option:selected').text('');
                $('#cbxPromotionType option:selected').text('');
                $('#cbxAddApprover option:selected').text('');
                $('#cbxAddUser option:selected').text('');
                $('#cbxUser option:selected').text('');
                $('#cbxAddObjective option:selected').text('');
                $('#cbxObjective option:selected').text('');
                $('#cbxAddGLNo option:selected').text('');
                $('#cbxGLNo option:selected').text('');

                $('#RefDocID option:selected').text('');
                $('#RefDocID').trigger("chosen:updated");

                $('#cbxAddApprover').trigger("chosen:updated");
                $('#cbxAddProject').trigger("chosen:updated");
                $('#cbxForProject').trigger("chosen:updated");
                $('#cbxAddPromoType').trigger("chosen:updated");
                $('#cbxAddObjective').trigger("chosen:updated");
                $('#cbxAddGLNo').trigger("chosen:updated");
                $('#cbxObjective').trigger("chosen:updated");
                $('#cbxGLNo').trigger("chosen:updated");
                $('#cbxAddUser').trigger("chosen:updated");
                $('#cbxUser').trigger("chosen:updated");
                $('#cbxAddPromotionType').trigger("chosen:updated");
                $('#cbxPromotionType').trigger("chosen:updated");
                $('#cbxAddCostCenter').trigger("chosen:updated");
                $('#cbxCostCenter').trigger("chosen:updated");
                $('#RefDocID').trigger("chosen:updated");

                SetComboBox();

                if ($.fn.dataTable.isDataTable('#mktTable')) {
                    table = $('#mktTable').DataTable({
                        retrieve: true,
                        paging: false
                    });
                    table.destroy();
                    $('#mktTable').dataTable().fnClearTable();
                }

                if ($.fn.dataTable.isDataTable('#table-product')) {
                    table = $('#table-product').DataTable({
                        retrieve: true,
                        paging: false
                    });
                    table.destroy();
                }
                $('#iProduct').empty();
                CallTableProduct();



                var wizard = $('#fuelux-wizard-container').data('fu.wizard');
                wizard.currentStep = 1;
                wizard.setState();

                //$('#cbxAddApprover option:selected').text('');

                $('#reject').removeClass('hide').addClass('hide');
                $('#accept').removeClass('hide').addClass('hide');
                $('#approve').removeClass('hide').addClass('hide');
                $('#open').removeClass('hide').addClass('hide');
                $('#edit').removeClass('hide').addClass('hide');
                $('#save').removeClass('hide');

                $('html, body').animate({ scrollTop: 0 }, 'fast');
                e.preventDefault();
                localStorage.setItem("Mode", 1);



            });

            var $validation = false;
            $('#fuelux-wizard-container')
                .ace_wizard({

                })
                .on('actionclicked.fu.wizard', function (e, info) {
                    if (info.step == 1 && $validation) {
                        if (!$('#validation-form').valid()) e.preventDefault();
                    }
                })
                .on('finished.fu.wizard', function (e) {
                    $('#save').text('อนุมัติ');
                    messagefn.confirm2('คุณต้องการอนุมัติเอกสารนี้หรือไม่ ??', 0);
                    //SaveData(0);

                    $('#open').addClass('hide');

                    var wizard = $('#fuelux-wizard-container').data('fu.wizard')
                    wizard.currentStep = 1;
                    wizard.setState();
                }).on('stepclick.fu.wizard', function (e) {

                });

            $('#modal-wizard-container').ace_wizard();
            $('#modal-wizard .wizard-actions .btn[data-dismiss=modal]').removeAttr('disabled');

            $('#cancelReject').on('click', function (e) {
                $('#lblTextQ').text('คุณต้องการ อนุมัติ หรือ ปฎิเสธเอกสารนี้หรือไม่ ?');
                $('#confirm-reject').addClass('hide');
            });

            $('#submitReject').on('click', function (e) {
                if ($('#rejectText').val() == "") {
                    bootbox.dialog({
                        message: " ",
                        title: '<h2 style="text-align: center;">กรุณาระบุเหตุผลของการปฎิเสธ</h2>',
                        buttons: {
                            danger: {
                                label: "Agree",
                                className: "btn-danger",
                                callback: function () {
                                    $('#rejectText').focus();
                                }
                            }
                        }
                    });
                    return;
                } else {
                    rejectDataRequisition($('#newDocID').val());
                }
            });

        });

        $(document)

            .on('keyup', '.text-limit', function (e) {
                if (Math.floor(e.currentTarget.value) == e.currentTarget.value && $.isNumeric(e.currentTarget.value)) {
                    if (e.currentTarget.value == 0) {
                        e.currentTarget.value = '';
                        return;
                    }
                    var itemCount = 0;
                    var curr_project = ''
                    var curr_item = ''
                    var net_item = 0;
                    if (m_edit == true) {
                        curr_project = e.currentTarget.parentElement.parentElement.cells[1].innerText.split(' : ')[0];
                        curr_item = e.currentTarget.parentElement.parentElement.cells[3].innerText.split(' - ')[0]
                        net_item = e.currentTarget.parentElement.parentElement.cells[4].innerText;
                    }
                    else {
                        curr_project = e.currentTarget.parentElement.parentElement.cells[1].innerText;
                        curr_item = e.currentTarget.parentElement.parentElement.cells[4].innerText;
                        net_item = e.currentTarget.parentElement.parentElement.cells[6].innerText;
                    }

                    var index = $('.text-limit').index(this);

                    var rowData = $('#mktTable').dataTable().fnGetData();
                    for (var i = 0; i < rowData.length; i++) {
                        if (rowData[i][4] == curr_item && rowData[i][1] == curr_project && i == index)
                        {
                            itemCount += parseInt(e.currentTarget.value);
                        }
                        //else if (rowData[i][4] == curr_item && rowData[i][1] != curr_project){
                        //    itemCount += parseInt($('.text-limit')[i].value);
                        //}
                        else if (rowData[i][4] == curr_item && rowData[i][1] != curr_project) {
                            itemCount += parseInt(($('.text-limit')[i].value == '' ? 0 : $('.text-limit')[i].value));
                        }
                    }

                    //console.log("Index");
                    //console.log(index);

                    //console.log("rowData");
                    //console.log(rowData);
                    //console.log("net_item");
                    //console.log(net_item);
                    //console.log("itemCount");
                    //console.log(itemCount);

                    if (net_item < itemCount) {
                        bootbox.alert('<div class="center">รายการสินค้า ' + e.currentTarget.parentElement.parentElement.cells[5].innerText + ' จำนวนขอเบิกไม่ถูกต้อง <br /> เกินจำนวนที่เบิกได้</div>');
                        e.currentTarget.value = '';
                    }
                }
                else {
                    if (e.currentTarget.value != '') {
                        bootbox.alert('<div class="center">กรุณาใส่เฉพาะตัวเลขเท่านั้น</div>');
                        e.currentTarget.value = '';
                    }
                }
            })
            .on('click', '#b_add', function (e) {
                var oMemoData = $('#mktTable').dataTable();
                var d_oMemoData = oMemoData.fnGetData();
                var oTable = $('#memoTable').dataTable();
                var rowcollection = oTable.$(".call-check:checked");
                if (rowcollection.length == 0) {
                    bootbox.alert('<h4><center>กรุณาเลือกรายการอย่างน้อย 1 รายการ</center></h4>');
                    return;
                }
                var isHas = false;
                var isLimited = false;
                var usedItems = "";
                var isZero = false;
                rowcollection.each(function (index, elem) {
                    try {
                        var itemCount = 0;

                        if (d_oMemoData.length == 0) {
                            var table1 = $('#mktTable').dataTable();
                            $.ajax({
                                type: "POST",
                                url: "RequisitionRequest.aspx/GetListItemsMemoByDocNo",
                                data: JSON.stringify({
                                    docno: $('#cbxForMemo option:selected').val(),
                                    itemno: elem.parentElement.parentElement.cells[3].innerText
                                }),
                                contentType: 'application/json; charset=utf-8',
                                dataType: 'json',
                                error: function (XMLHttpRequest, textStatus, errorThrown) {
                                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                                        textStatus + "\n\nError: " + errorThrown);
                                },
                                success: function (result) {
                                    //bootbox.alert('<h4><center>เพิ่มรายการ เรียบร้อย!</center></h4>');
                                    try {

                                        var textbox = '<input type="text" class="setwidth-textbox text-limit">';
                                        var btnDel = '<a href="" class="btn btn-sm btn-white btn-danger btn-round btnMemoDataDel" onclick="return false;">' +
                                            '<i class="ace-icon fa fa-trash bigger-130"></i></a>';
                                        if (result.d.MsgErr == '') {
                                            //$('#mktTable').dataTable().fnClearTable();
                                            $.each(result, function (k, Val) {
                                                var isDupp = false;
                                                $.each(Val.ItemsMemo, function (key, Value) {
                                                    var table1 = $('#mktTable').dataTable();
                                                    var o_table = table1.fnGetData();
                                                    for (var i = 0; i < o_table.length; i++) {
                                                        if ($('#cbxForProject option:selected').val() != o_table[i][1]) {
                                                            isDupp = true;
                                                        }
                                                        else {
                                                            isDupp = false;
                                                            break;
                                                        }
                                                    }

                                                    if (isDupp == true) {
                                                        table1.fnAddData([
                                                            table1.fnSettings().fnRecordsTotal() + 1,
                                                            $('#cbxForProject option:selected').val(),
                                                            $('#cbxForProject option:selected').text(),
                                                            Value.DOCNO,
                                                            Value.ITEMNO,
                                                            Value.ITEMNAME,
                                                            Value.MEMOBALANCE, //- itemCount
                                                            textbox,
                                                            Value.TYPE,
                                                            btnDel
                                                        ]).draw();
                                                    }
                                                    else {
                                                        table1.fnAddData([
                                                            table1.fnSettings().fnRecordsTotal() + 1,
                                                            $('#cbxForProject option:selected').val(),
                                                            $('#cbxForProject option:selected').text(),
                                                            Value.DOCNO,
                                                            Value.ITEMNO,
                                                            Value.ITEMNAME,
                                                            Value.MEMOBALANCE, //- itemCount
                                                            textbox,
                                                            Value.TYPE,
                                                            btnDel
                                                        ]).draw();
                                                    }
                                                });
                                            });
                                        }
                                        else {
                                            bootbox.alert('<h4><center>' + result.d.MsgErr + '</center></h4>');
                                        }
                                    } catch (e) {
                                        console.log(e);
                                    }

                                    $('.text-limit').each(function () {
                                        var currentTD = $(this).parent();
                                        var limit = currentTD.prev('td').text();
                                        $(this).autoNumeric({
                                            vMin: '0',
                                            vMax: limit
                                        });
                                    });
                                }
                            });
                        }
                        else {
                            for (var i = 0; i < d_oMemoData.length; i++) {
                                if (d_oMemoData[i][1] == elem.parentElement.parentElement.cells[1].innerText.split(' : ')[0].toString() &&
                                    d_oMemoData[i][4] == elem.parentElement.parentElement.cells[3].innerText &&
                                    d_oMemoData[i][3] == elem.parentElement.parentElement.cells[2].innerText) {
                                    isHas = true;
                                    usedItems += elem.parentElement.parentElement.cells[3].innerText + ' : ' + elem.parentElement.parentElement.cells[4].innerText + ' คู่กับ โครงการ ' + elem.parentElement.parentElement.cells[1].innerText + '<br />';
                                    break;
                                }
                            }
                            if (isHas) {
                                //bootbox.alert('<h4><center>บางรายการที่เลือก ถูกจับคู่ไปแล้ว<br />' + usedItems + '</center></h4>');
                            } else if (!isHas) {
                                var table1 = $('#mktTable').dataTable();
                                $.ajax({
                                    type: "POST",
                                    url: "RequisitionRequest.aspx/GetListItemsMemoByDocNo",
                                    data: JSON.stringify({
                                        docno: $('#cbxForMemo option:selected').val(),
                                        itemno: elem.parentElement.parentElement.cells[3].innerText
                                    }),
                                    contentType: 'application/json; charset=utf-8',
                                    dataType: 'json',
                                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                                        alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                                            textStatus + "\n\nError: " + errorThrown);
                                    },
                                    success: function (result) {
                                        //bootbox.alert('<h4><center>เพิ่มรายการ เรียบร้อย!</center></h4>');
                                        try {

                                            var textbox = '<input type="text" class="setwidth-textbox text-limit">';
                                            var btnDel = '<a href="" class="btn btn-sm btn-white btn-danger btn-round btnMemoDataDel" onclick="return false;">' +
                                                '<i class="ace-icon fa fa-trash bigger-130"></i></a>';
                                            if (result.d.MsgErr == '') {
                                                //$('#mktTable').dataTable().fnClearTable();
                                                $.each(result, function (k, Val) {
                                                    var isDupp = false;
                                                    $.each(Val.ItemsMemo, function (key, Value) {

                                                        var rowcollection = $('#memoTable').dataTable().$(".call-check:checked");
                                                        rowcollection.each(function (index, elem) {

                                                            var table1 = $('#mktTable').dataTable();
                                                            var o_table = table1.fnGetData();
                                                            var itemCount = 0;
                                                            for (var i = 0; i < o_table.length; i++) {


                                                                if ($('#cbxForProject option:selected').val() == o_table[i][1]
                                                                    && elem.parentElement.parentElement.cells[3].innerText == o_table[i][4]) {
                                                                    if (m_edit == true) {
                                                                        if (o_table[i][6] == 0) {
                                                                            isDupp = true;
                                                                            isZero = true;
                                                                            return;
                                                                        }
                                                                        else {
                                                                            isDupp = false;
                                                                        }
                                                                    }
                                                                    else {
                                                                        isDupp = true;
                                                                        isZero = true;
                                                                        return;
                                                                    }
                                                                }
                                                                else {
                                                                    isDupp = false;
                                                                    if (m_edit == true) {
                                                                        if (o_table[i][6] == 0) {
                                                                            isDupp = true;
                                                                            isZero = true;
                                                                            usedItems = 'ไม่มีของในสต็อคแล้ว';
                                                                            return;
                                                                        }
                                                                        else if (o_table[i][6] != 0 && elem.parentElement.parentElement.cells[3].innerText == o_table[i][4]) {
                                                                            isDupp = false;
                                                                            isZero = false;
                                                                            break;
                                                                        }
                                                                        else {
                                                                            isDupp = false;
                                                                        }
                                                                    }
                                                                    else {
                                                                        isDupp = false;
                                                                        isZero = false;
                                                                        return;
                                                                    }
                                                                }

                                                            }
                                                        });
                                                        if (isDupp == false && isZero == false) {
                                                            table1.fnAddData([
                                                                table1.fnSettings().fnRecordsTotal() + 1,
                                                                $('#cbxForProject option:selected').val(),
                                                                $('#cbxForProject option:selected').text(),
                                                                Value.DOCNO,
                                                                Value.ITEMNO,
                                                                Value.ITEMNAME,
                                                                Value.MEMOBALANCE, //- itemCount,
                                                                textbox,
                                                                Value.TYPE,
                                                                btnDel
                                                            ]).draw();
                                                        }
                                                    });
                                                });
                                            }
                                            else {
                                                bootbox.alert('<h4><center>' + result.d.MsgErr + '</center></h4>');
                                            }

                                        } catch (e) {
                                            console.log(e);
                                        }


                                        $('.text-limit').each(function () {
                                            var currentTD = $(this).parent();
                                            var limit = currentTD.prev('td').text();
                                            $(this).autoNumeric({
                                                vMin: '0',
                                                vMax: limit
                                            });
                                        });

                                    }
                                });
                            }

                            setTimeout(function () {
                                bootbox.alert('<h4><center>' + (usedItems != "" ? 'มีรายการ ที่ถูกจับคู่ไปแล้ว<br />' + usedItems : 'เพิ่มรายการสำเร็จ') + '<br /></center></h4>');
                                //(isZero == false ? 'เพิ่มรายการสำเร็จ !' : 'รายการนี้ถูกใช้หมดแล้ว !')) + '<br /></center></h4>');
                            }, 300);

                        }
                    } catch (ex) {
                        console.log(ex);
                    }
                });
            })
            .on('click', '.btn-selectmemo', function (e) {

                $("input.text-limit").attr('disabled', true);

                var c_memo = $('#cbxForMemo option:selected').val();
                var c_project = $('#cbxForProject option:selected').val();
                //var c_item = $('#cbxForMemo option:selected').val();

                if (c_memo == '') {
                    bootbox.alert('<h4><center>กรุณาเลือก ใบบันทึกขอเบิก</center></h4>');
                    return;
                }
                if (c_project == 'All') {
                    bootbox.alert('<h4><center>กรุณาเลือก โครงการ</center></h4>');
                    return;
                }

                $('#memo-modal').modal('toggle');
                if ($.fn.dataTable.isDataTable('#memoTable')) {
                    var table1 = $('#memoTable').DataTable({
                        retrieve: true,
                        paging: false
                    });
                    table1.destroy();
                }

                var table2 = $('#memoTable').dataTable({

                    "bInfo": false,
                    "bFilter": false,
                    "bDeferRender": false,

                    "aoColumns": [
                        { "sClass": "center setwidth-cbx", 'bSortable': false },
                        { 'visible': false },
                        { "sClass": "align-left", 'bSortable': false },
                        { "sClass": "center", 'bSortable': false },
                        { "sClass": "center", 'bSortable': false },
                        { "sClass": "center", 'bSortable': false }
                    ],
                    'paging': false,
                    "fnCreatedRow": function (nRow, aData, iDataIndex) {
                        $(nRow).attr('id', aData[0]);
                        var checkBox = $(nRow).find("input[type=checkbox]");
                        checkBox.attr('class', 'call-check');
                    },
                    columnDefs: [{
                        orderable: false,
                        className: 'select-checkbox',
                        targets: 0
                    }],
                    select: {
                        style: 'os',
                        selector: 'td:first-child'
                    },
                    order: [[1, 'asc']]
                });
                var checkbox = '<input type="checkbox">';


                $.ajax({
                    type: "POST",
                    url: "RequisitionRequest.aspx/GetListItemsMemoByDocNo",
                    data: JSON.stringify({ docno: c_memo, itemno: '' }),
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                            textStatus + "\n\nError: " + errorThrown);
                    },
                    success: function (result) {
                        try {
                            if (result.d.MsgErr == '') {
                                $('#memoTable').dataTable().fnClearTable();
                                $.each(result, function (k, Val) {
                                    $.each(Val.ItemsMemo, function (key, Value) {
                                        table2.fnAddData([
                                            checkbox,
                                            $('#cbxForProject option:selected').val(),
                                            $('#cbxForProject option:selected').text(),
                                            Value.DOCNO,
                                            Value.ITEMNO,
                                            Value.ITEMNAME
                                        ]);
                                    });
                                });
                            }
                            else {
                                bootbox.alert('<h4><center>' + result.d.MsgErr + '</center></h4>');
                            }
                        } catch (e) {

                        }
                    }
                });

            })

            .on("click", ".open-myModal", function () {
                editDataRequisition($(this).data('id'));
            })

            .on('click', '#form-search', function (e) {
                var c_user = '';
                if ($('#cbxUser option:selected').text() == '') {
                    c_user = $('#cbxUser option:selected').text();
                }
                else {
                    c_user = $('#cbxUser option:selected').val();
                }

                $.ajax({
                    type: "POST",
                    url: "RequisitionRequest.aspx/GetDataRequisitionSearch",
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    data: JSON.stringify({
                        docSearchID: $('#docSearchID').val(),
                        tagSearchID: $('#tagSearchID').val(),
                        RecieveDatePicker: $('#RecieveDatePicker1').val(),
                        BookDatePicker: $('#BookDatePicker1').val(),
                        cbxUser: c_user,
                        cbxPromotionType: $('#cbxPromotionType option:selected').val(),
                        cbxCostCenter: $('#cbxCostCenter option:selected').val(),
                        cbxObjective: $('#cbxObjective option:selected').val(),
                        cbxGLNo: $('#cbxGLNo option:selected').val()
                    }),
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                            textStatus + "\n\nError: " + errorThrown);
                    },
                    success: function (result) {
                        ClearData('#dynamic-table');
                        try {

                            $.each(result, function (k, Val) {

                                if (Val.MsgErr != '') {
                                    bootbox.alert('<h4><center>' + Val.MsgErr + '</center></h4>');
                                }

                                $.each(Val.lstrr, function (key, Value) {
                                    var data = RenderDatatable(Value);
                                    $('#iBody').append(data);
                                });
                            });

                        } catch (e) {

                        }
                        CallDatatable();
                    }
                });
            })
            .on('click', '#clear-search', function (e) {
                $('#docSearchID').val('');
                $('#tagSearchID').val('');
                $('#RecieveDatePicker').val('');
                $('#BookDatePicker').val('');

                $('#cbxUser').val('All');
                $('#cbxUser').trigger("chosen:updated");
                $('#cbxPromotionType').val('All');
                $('#cbxPromotionType').trigger("chosen:updated");
                $('#cbxCostCenter').val('All');
                $('#cbxCostCenter').trigger("chosen:updated");
                $('#cbxObjective').val('All');
                $('#cbxObjective').trigger("chosen:updated");
                $('#cbxGLNo').val('All');
                $('#cbxGLNo').trigger("chosen:updated");

                $('#RecieveDatePicker1').val(moment().format('DD/MM/YYYY'));
                $('#BookDatePicker1').val(moment().format('DD/MM/YYYY'));
            })
            .on('click', '.btnMemoDataDel', function (e) {
                var table = $('#mktTable').DataTable();
                table.row($(this).parents('tr')).remove().draw();
                //alert(e.currentTarget.parentElement.parentNode.cells[1].innerText);
            })
            ;

        function SetComboBox() {
            if (Permission.Admin) {
                var optUser;
                var optUserSearch = '<option value="All"> -- All -- </option>';
                for (var i = 0; i < AllUser.length; i++) {
                    optUser += '<option value="' + AllUser[i].ID + '">' + AllUser[i].ID + ' : ' + AllUser[i].FName + ' ' + AllUser[i].LName + '</option>';
                }
                optUserSearch += optUser;

                var optUser = "";
                for (var i = 0; i < AllUser.length; i++) {
                    if (Permission.UserDetails[0].EmpCode == AllUser[i].ID) {
                        optUser += '<option value="' + AllUser[i].ID + '" selected="selected">' + AllUser[i].ID + ' : ' + AllUser[i].FName + ' ' + AllUser[i].LName + '</option>';
                    }
                    else {
                        optUser += '<option value="' + AllUser[i].ID + '">' + AllUser[i].ID + ' : ' + AllUser[i].FName + ' ' + AllUser[i].LName + '</option>';
                    }
                }


                var optUserApprove;
                for (var i = 0; i < AllUser.length; i++) {
                    if (Permission.UserDetails[0].LeaderID == AllUser[i].ID) {
                        optUserApprove += '<option value="' + AllUser[i].ID + '" selected="selected">' + AllUser[i].ID + ' : ' + AllUser[i].FName + ' ' + AllUser[i].LName + '</option>';
                    }
                    else {
                        optUserApprove += '<option value="' + AllUser[i].ID + '">' + AllUser[i].ID + ' : ' + AllUser[i].FName + ' ' + AllUser[i].LName + '</option>';
                    }
                }

                $.ajax({
                    type: "POST",
                    url: "RequisitionRequest.aspx/GetMemoData",
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    async: false,
                    data: JSON.stringify({ empcode: $('#lbUserId').text() }),
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                            textStatus + "\n\nError: " + errorThrown);
                    },
                    success: function (result) {
                        try {
                            var strdata = '';
                            $.each(result, function (k, Val) {
                                if (Val.MsgErr == '') {
                                    $.each(Val.LstMemoData, function (key, Value) {
                                        strdata += '<option id="' + Value.MemoID + '">' + Value.MemoID + '</option>';
                                    });
                                    $('#RefDocID').empty();
                                    $('#RefDocID').append(strdata);
                                    $('#RefDocID').trigger("chosen:updated");
                                }
                                else {
                                    bootbox.alert('<h4><center>' + Val.MsgErr + '</center></h4>');
                                }
                            });

                        } catch (e) {

                        }
                    }
                });
            }
            else if (Permission.CLevel) {

                var optUser;
                var optUserSearch = '<option value="All"> -- All -- </option>';
                for (var i = 0; i < AllUser.length; i++) {
                    optUser += '<option value="' + AllUser[i].ID + '">' + AllUser[i].ID + ' : ' + AllUser[i].FName + ' ' + AllUser[i].LName + '</option>';
                }
                optUserSearch += optUser;

                var optUser = "";
                for (var i = 0; i < AllUser.length; i++) {
                    if (Permission.UserDetails[0].EmpCode == AllUser[i].ID) {
                        optUser += '<option value="' + AllUser[i].ID + '" selected="selected">' + AllUser[i].ID + ' : ' + AllUser[i].FName + ' ' + AllUser[i].LName + '</option>';
                    }
                    else {
                        optUser += '<option value="' + AllUser[i].ID + '">' + AllUser[i].ID + ' : ' + AllUser[i].FName + ' ' + AllUser[i].LName + '</option>';
                    }
                }

                var optUserApprove = '<option value=""> </option>';
                for (var i = 0; i < CLevelUser.length; i++) {
                    if (Permission.UserDetails[0].LeaderID == CLevelUser[i].ID) {
                        optUserApprove += '<option value="' + CLevelUser[i].ID + '" selected="selected">' + CLevelUser[i].ID + ' : ' + CLevelUser[i].FName + ' ' + CLevelUser[i].LName + '</option>';
                    }
                    else {
                        optUserApprove += '<option value="' + CLevelUser[i].ID + '">' + CLevelUser[i].ID + ' : ' + CLevelUser[i].FName + ' ' + CLevelUser[i].LName + '</option>';
                    }
                }

                $('#classApprove').addClass('disabledbutton');
            }
            else if (Permission.ETC) {
                var optUser;
                var optUserSearch = '<option value="All"> -- All -- </option>';
                for (var i = 0; i < AllUser.length; i++) {
                    optUser += '<option value="' + AllUser[i].ID + '">' + AllUser[i].ID + ' : ' + AllUser[i].FName + ' ' + AllUser[i].LName + '</option>';
                }
                optUserSearch += optUser;

                var optUser = "";
                for (var i = 0; i < AllUser.length; i++) {
                    if (Permission.UserDetails[0].EmpCode == AllUser[i].ID) {
                        optUser += '<option value="' + AllUser[i].ID + '" selected="selected">' + AllUser[i].ID + ' : ' + AllUser[i].FName + ' ' + AllUser[i].LName + '</option>';
                    }
                    else {
                        optUser += '<option value="' + AllUser[i].ID + '">' + AllUser[i].ID + ' : ' + AllUser[i].FName + ' ' + AllUser[i].LName + '</option>';
                    }
                }

                var optUserApprove = '<option value=""> </option>';
                for (var i = 0; i < HeadOfUser.length; i++) {
                    if (Permission.UserDetails[0].LeaderID == HeadOfUser[i].ID) {
                        optUserApprove += '<option value="' + HeadOfUser[i].ID + '" selected="selected">' + HeadOfUser[i].ID + ' : ' + HeadOfUser[i].FName + ' ' + HeadOfUser[i].LName + '</option>';
                    }
                    else {
                        optUserApprove += '<option value="' + HeadOfUser[i].ID + '">' + HeadOfUser[i].ID + ' : ' + HeadOfUser[i].FName + ' ' + HeadOfUser[i].LName + '</option>';
                    }
                }
            }
            else if (Permission.HR) {
                var optUser;
                var optUserSearch = '<option value="All"> -- All -- </option>';
                for (var i = 0; i < AllUser.length; i++) {
                    optUser += '<option value="' + AllUser[i].ID + '">' + AllUser[i].ID + ' : ' + AllUser[i].FName + ' ' + AllUser[i].LName + '</option>';
                }
                optUserSearch += optUser;

                var optUser = "";
                for (var i = 0; i < AllUser.length; i++) {
                    if (Permission.UserDetails[0].EmpCode == AllUser[i].ID) {
                        optUser += '<option value="' + AllUser[i].ID + '" selected="selected">' + AllUser[i].ID + ' : ' + AllUser[i].FName + ' ' + AllUser[i].LName + '</option>';
                    }
                    else {
                        optUser += '<option value="' + AllUser[i].ID + '">' + AllUser[i].ID + ' : ' + AllUser[i].FName + ' ' + AllUser[i].LName + '</option>';
                    }
                }

                var optUserApprove = '<option value=""> </option>';
                for (var i = 0; i < CLevelUser.length; i++) {
                    if (Permission.UserDetails[0].LeaderID == CLevelUser[i].ID) {
                        optUserApprove += '<option value="' + CLevelUser[i].ID + '" selected="selected">' + CLevelUser[i].ID + ' : ' + CLevelUser[i].FName + ' ' + CLevelUser[i].LName + '</option>';
                    }
                    else {
                        optUserApprove += '<option value="' + CLevelUser[i].ID + '">' + CLevelUser[i].ID + ' : ' + CLevelUser[i].FName + ' ' + CLevelUser[i].LName + '</option>';
                    }
                }
            }
            else if (Permission.HeadOf) {
                var optUser;
                var optUserSearch = '<option value="All"> -- All -- </option>';
                for (var i = 0; i < AllUser.length; i++) {
                    optUser += '<option value="' + AllUser[i].ID + '">' + AllUser[i].ID + ' : ' + AllUser[i].FName + ' ' + AllUser[i].LName + '</option>';
                }
                optUserSearch += optUser;

                var optUser = "";
                for (var i = 0; i < AllUser.length; i++) {
                    if (Permission.UserDetails[0].EmpCode == AllUser[i].ID) {
                        optUser += '<option value="' + AllUser[i].ID + '" selected="selected">' + AllUser[i].ID + ' : ' + AllUser[i].FName + ' ' + AllUser[i].LName + '</option>';
                    }
                    else {
                        optUser += '<option value="' + AllUser[i].ID + '">' + AllUser[i].ID + ' : ' + AllUser[i].FName + ' ' + AllUser[i].LName + '</option>';
                    }
                }

                $('#classApprove').addClass('disabledbutton');

            }
            else if (Permission.LCM) {
                var optUser;
                var optUserSearch = '<option value="All"> -- All -- </option>';
                for (var i = 0; i < AllUser.length; i++) {
                    optUser += '<option value="' + AllUser[i].ID + '">' + AllUser[i].ID + ' : ' + AllUser[i].FName + ' ' + AllUser[i].LName + '</option>';
                }
                optUserSearch += optUser;

                var optUser = "";
                for (var i = 0; i < AllUser.length; i++) {
                    if (Permission.UserDetails[0].EmpCode == AllUser[i].ID) {
                        optUser += '<option value="' + AllUser[i].ID + '" selected="selected">' + AllUser[i].ID + ' : ' + AllUser[i].FName + ' ' + AllUser[i].LName + '</option>';
                    }
                    else {
                        optUser += '<option value="' + AllUser[i].ID + '">' + AllUser[i].ID + ' : ' + AllUser[i].FName + ' ' + AllUser[i].LName + '</option>';
                    }
                }

                var optUserApprove = '<option value=""> </option>';
                for (var i = 0; i < HeadOfUser.length; i++) {
                    if (Permission.UserDetails[0].LeaderID == HeadOfUser[i].ID) {
                        optUserApprove += '<option value="' + HeadOfUser[i].ID + '" selected="selected">' + HeadOfUser[i].ID + ' : ' + HeadOfUser[i].FName + ' ' + HeadOfUser[i].LName + '</option>';
                    }
                    else {
                        optUserApprove += '<option value="' + HeadOfUser[i].ID + '">' + HeadOfUser[i].ID + ' : ' + HeadOfUser[i].FName + ' ' + HeadOfUser[i].LName + '</option>';
                    }
                }

                $('#classCostCenter').addClass('disabledbutton');
                $('#classGLNo').addClass('disabledbutton');

            }
            else if (Permission.MKT) {
                var optUser;
                var optUserSearch = '<option value="All"> -- All -- </option>';
                for (var i = 0; i < AllUser.length; i++) {
                    optUser += '<option value="' + AllUser[i].ID + '">' + AllUser[i].ID + ' : ' + AllUser[i].FName + ' ' + AllUser[i].LName + '</option>';
                }
                optUserSearch += optUser;

                var optUser = "";
                for (var i = 0; i < AllUser.length; i++) {
                    if (Permission.UserDetails[0].EmpCode == AllUser[i].ID) {
                        optUser += '<option value="' + AllUser[i].ID + '" selected="selected">' + AllUser[i].ID + ' : ' + AllUser[i].FName + ' ' + AllUser[i].LName + '</option>';
                    }
                    else {
                        optUser += '<option value="' + AllUser[i].ID + '">' + AllUser[i].ID + ' : ' + AllUser[i].FName + ' ' + AllUser[i].LName + '</option>';
                    }
                }

                $.ajax({
                    type: "POST",
                    url: "RequisitionRequest.aspx/GetMemoData",
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    async: false,
                    data: JSON.stringify({ empcode: $('#lbUserId').text() }),
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                            textStatus + "\n\nError: " + errorThrown);
                    },
                    success: function (result) {
                        try {
                            var strdata = '';
                            $.each(result, function (k, Val) {
                                if (Val.MsgErr == '') {
                                    $.each(Val.LstMemoData, function (key, Value) {
                                        strdata += '<option id="' + Value.MemoID + '">' + Value.MemoID + '</option>';
                                    });
                                    $('#RefDocID').empty();
                                    $('#RefDocID').append(strdata);
                                    $('#RefDocID').trigger("chosen:updated");
                                }
                                else {
                                    bootbox.alert('<h4><center>' + Val.MsgErr + '</center></h4>');
                                }
                            });

                        } catch (e) {
                        }
                    }
                });
                $('#classApprove').addClass('disabledbutton');
            }


            var p_Projects = '<option value="All"> </option>';
            p_Projects += '<option value="999111"> -- ไม่ระบุโครงการ -- </option>';
            for (var i = 0; i < Projects.length; i++) {
                p_Projects += '<option value="' + Projects[i].ProjectID + '">' + Projects[i].ProjectID + ' : ' + Projects[i].ProjectName + '</option>';
            }

            var f_Projects = '<option value=""> </option>';
            for (var i = 0; i < Projects.length; i++) {
                f_Projects += '<option value="' + Projects[i].ProjectID + '">' + Projects[i].ProjectID + ' : ' + Projects[i].ProjectName + '</option>';
            }

            var p_Items = '<option value="All"> </option>';
            for (var i = 0; i < PromotionItems.length; i++) {
                p_Items += '<option value="' + PromotionItems[i].ItemNO + '">' + PromotionItems[i].ItemNO + ' : ' + PromotionItems[i].ItemName + '</option>';
            }


            var optCostCenter;
            var optCCSearch = '<option value="All">-- All -- </option>';

            for (var i = 0; i < CostCenter.length; i++) {

                //if (Permission.UserDetails[0].CostCenterCode == CostCenter[i].CostCenter) {
                //    optCCAdd += '<option value="' + CostCenter[i].CostCenter + '" selected="selected">' + CostCenter[i].CostCenter + ' : ' + CostCenter[i].CostCenterName + '</option>';
                //}
                //else {
                //    optCCAdd += '<option value="' + CostCenter[i].CostCenter + '">' + CostCenter[i].CostCenter + ' : ' + CostCenter[i].CostCenterName + '</option>';
                //}

                optCostCenter += '<option value="' + CostCenter[i].CostCenter + '">' + CostCenter[i].CostCenter + ' : ' + CostCenter[i].CostCenterName + '</option>';
            }

            var optCCAdd;
            optCCAdd = '<option value="" selected="selected">-- กรุณาเลือก หน่วยงาน --</option>';

            for (var i = 0; i < CostCenterForNewReq.length; i++) {

                if (Permission.UserDetails[0].CostCenterCode == CostCenter[i].CostCenter) {
                    optCCAdd += '<option value="' + CostCenterForNewReq[i].CostCenter + '" selected="selected">' + CostCenterForNewReq[i].CostCenter + ' : ' + CostCenterForNewReq[i].CostCenterName + '</option>';
                }
                else {
                    optCCAdd += '<option value="' + CostCenterForNewReq[i].CostCenter + '">' + CostCenterForNewReq[i].CostCenter + ' : ' + CostCenterForNewReq[i].CostCenterName + '</option>';
                }
            }

            optCCSearch += optCostCenter;
            //optCCAdd += optCostCenter;

            var optGL = "";
            var otpGLSearch = '<option value="All"> -- All -- </option>';
            var otpGLAdd = '';
            for (var i = 0; i < GLData.length; i++) {
                if (i == 0 && Permission.MKT) {
                    optGL += '<option value="' + GLData[i].GLID + '" selected>' + GLData[i].GLID + ' : ' + GLData[i].GLDESC + '</option>';
                } else {
                    optGL += '<option value="' + GLData[i].GLID + '">' + GLData[i].GLID + ' : ' + GLData[i].GLDESC + '</option>';
                }
            }

            if (!Permission.MKT) {
                optGL = '<option value=""> </option>' + optGL;
            }

            otpGLSearch += optGL;
            otpGLAdd += optGL;

            var optPromo = '<option value="001">001 : Marketing Campaign</option>' +
                '<option value="002">002 : Event</option>' +
                '<option value="003">003 : อื่นๆ</option>';
            var optPromoSearch = '<option value="All">-- All -- </option>' + optPromo;
            var optPromoAdd = '<option value=""> </option>' + optPromo;

            var optObjectiveData;
            var optObjectiveAdd = '<option value=""> </option>'
            var optObjectiveSearch = '<option value="All">-- All -- </option>'

            for (var i = 0; i < Objective.length; i++) {
                optObjectiveData += '<option value="' + Objective[i].ID + '">' + Objective[i].Titles + '</option>';
            }

            optObjectiveSearch += optObjectiveData;
            optObjectiveAdd += optObjectiveData;


            $('#cbxAddProject').empty();
            $('#cbxForProject').empty();
            $('#cbxAddPromoType').empty();

            $('#cbxAddCostCenter').empty();
            $('#cbxCostCenter').empty();
            $('#cbxAddPromotionType').empty();
            $('#cbxPromotionType').empty();
            $('#cbxAddApprover').empty();
            $('#cbxAddUser').empty();
            $('#cbxUser').empty();
            $('#cbxAddObjective').empty();
            $('#cbxObjective').empty();
            $('#cbxAddGLNo').empty();
            $('#cbxGLNo').empty();

            $('#a_cbxAddUser').empty();
            $('#a_cbxAddApprover').empty();
            $('#a_cbxAddPromotionType').empty();
            $('#a_cbxAddCostCenter').empty();
            $('#a_cbxAddGLNo').empty();
            $('#a_cbxAddObjective').empty();

            $('#cbxAddProject').append(p_Projects);

            if (MKTUser != "") {
                //$('#cbxForProject').append(optCCSearch);
                $('#cbxForProject').append(optCCAdd);

            } else {
                $('#cbxForProject').append(f_Projects);
            }

            $('#cbxAddPromoType').append(p_Items);
            $('#cbxAddCostCenter').append(optCCAdd);
            $('#cbxCostCenter').append(optCCSearch);
            $('#cbxAddPromotionType').append(optPromoAdd);
            $('#cbxPromotionType').append(optPromoSearch);

            $('#cbxAddApprover').append(optUserApprove);
            $('#cbxAddApprover').trigger("chosen:updated");

            $('#cbxAddUser').append(optUser);
            $('#cbxUser').append(optUserSearch);
            $('#cbxAddObjective').append(optObjectiveAdd);
            $('#cbxObjective').append(optObjectiveSearch);
            $('#cbxAddGLNo').append(otpGLAdd);
            $('#cbxGLNo').append(otpGLSearch);

            $('#a_cbxAddUser').append(optUser);
            $('#a_cbxAddApprover').append(optUserApprove);
            $('#a_cbxAddPromotionType').append(optPromoAdd);
            $('#a_cbxAddCostCenter').append(optCCAdd);
            $('#a_cbxAddGLNo').append(otpGLAdd);
            $('#a_cbxAddObjective').append(optObjectiveAdd);

            $('#cbxAddProject').trigger("chosen:updated");
            $('#cbxForProject').trigger("chosen:updated");
            $('#cbxAddPromoType').trigger("chosen:updated");
            $('#cbxAddObjective').trigger("chosen:updated");
            $('#cbxAddGLNo').trigger("chosen:updated");
            $('#cbxObjective').trigger("chosen:updated");
            $('#cbxGLNo').trigger("chosen:updated");
            $('#cbxAddUser').trigger("chosen:updated");
            $('#cbxUser').trigger("chosen:updated");
            $('#cbxAddPromotionType').trigger("chosen:updated");
            $('#cbxPromotionType').trigger("chosen:updated");
            $('#cbxAddCostCenter').trigger("chosen:updated");
            $('#cbxCostCenter').trigger("chosen:updated");
            //$('#RefDocID').trigger("chosen:updated");

            $('#a_cbxAddUser').trigger("chosen:updated");
            $('#a_cbxAddApprover').trigger("chosen:updated");
            $('#a_cbxAddPromotionType').trigger("chosen:updated");
            $('#a_cbxAddCostCenter').trigger("chosen:updated");
            $('#a_cbxAddGLNo').trigger("chosen:updated");
            $('#a_cbxAddObjective').trigger("chosen:updated");
        }

        function ClearData(dataTableMode) {
            localStorage.clear();
            if ($.fn.dataTable.isDataTable('' + dataTableMode + '')) {
                table = $('' + dataTableMode + '').DataTable({
                    retrieve: true,
                    paging: false
                });
                table.destroy();
            }
            $('#iBody').empty();
        }

        function ValidateSave() {
            if ($('#cbxAddPromoType').val() == "") {
                return false;
            } else if ($('#cbxAddProject').val() == "") {
                return false;
            } else if ($('#txtQuantity').val() == "") {
                return false;
            } else if ($('#setStdDatePicker').val() == "") {
                return false;
            } else if ($('#setEndDatePicker').val() == "") {
                return false;
            } else if ($('#reminDay').val() == "") {
                return false;
            } else {
                return true;
            }
        }

        function ManageDataBeforeSave(status) {
            switch (status) {
                case 0:
                    if (Permission.MKT || (Permission.Admin && ($('#RefDocID').val() != null && $('#RefDocID').val()[0] != ""))) {

                        var dataDetails = [];
                        var refDetails = [];
                        var table = $('#mktTable').DataTable().data();

                        for (var i = 0; i < table.length; i++) {

                            var projn = (table[i][2].split(':').length == 2 ?
                                table[i][2].split(':')[1].trim() :
                                table[i][2].trim());

                            var propn = (table[i][3].split(':').length == 2 ?
                                table[i][3].split(':')[1].trim() :
                                table[i][3].trim());

                            dataDetails.push({
                                "DRNOBYITEM": table[i][3].trim(),
                                "PROJID": table[i][1].split(':')[0].trim(),
                                "PROJN": projn,
                                "PROPID": table[i][4].trim(),
                                "PROPN": table[i][5].trim(),
                                "RMDB": '',
                                "STDDT": '',
                                "ENDDT": '',
                                "QUANT": table.$('input')[i].value,
                                "ITEMS": (i + 1)
                            });
                        }
                        var lstRef = $('#RefDocID').val();
                        if (lstRef == null) {
                            refDetails.push({
                                "DRNO": ''
                            });
                        }
                        else {
                            for (var i = 0; i < lstRef.length; i++) {
                                refDetails.push({
                                    "DRNO": lstRef[i]
                                });
                            }
                        }

                        var gl = ($('#cbxAddGLNo option:selected').text().split(':').length == 2 ?
                            $('#cbxAddGLNo option:selected').text().split(':')[1].trim() :
                            $('#cbxAddGLNo option:selected').text().trim());

                        var appr = ($('#cbxAddApprover option:selected').text().split(':').length == 2 ?
                            $('#cbxAddApprover option:selected').text().split(':')[1].trim() :
                            $('#cbxAddApprover option:selected').text().trim());
                        var appvid = $('#cbxAddApprover').val();

                        var data = [];
                        data.push({
                            "RQRNO": $('#newDocID').val(),
                            "DRNO": refDetails,
                            "RCVD": $('#recieveDocDatePick').val(),
                            "CURD": $('#bookDocDatePick').val(),
                            "REFREQID": (Permission.Admin == true ? Permission.UserDetails[0].EmpCode : ""),
                            "REQID": $('#cbxAddUser').val(),
                            "REQN": $('#cbxAddUser option:selected').text().split(':')[1].trim(),
                            "PROMOTID": $('#cbxAddPromotionType').val(),
                            "PROMOTN": $('#cbxAddPromotionType option:selected').text().split(':')[1].trim(),
                            "DEPTID": $('#cbxAddCostCenter').val(),
                            "DEPTN": $('#cbxAddCostCenter option:selected').text().split(':')[1].trim(),
                            "GLID": $('#cbxAddGLNo').val(),
                            "GLN": gl,
                            "OBJID": $('#cbxAddObjective').val(),
                            "OBJN": $('#cbxAddObjective option:selected').text().trim(),
                            "REMK": $('#txtRemark').val(),
                            "APPVID": appvid,
                            "APPVN": appr,
                            "STATUS": 2,
                            "dataDetails": dataDetails,
                        });

                        return data;
                    }
                    else if (!Permission.MKT) {

                        var dataDetails = [];
                        var refDetails = [];


                        var status;
                        if (Permission.CLevel || Permission.HeadOf || Permission.Admin || Permission.MKT) {
                            status = 2;
                        } else if (true) {
                            status = 1;
                        }

                        var table = $('#table-product').DataTable().data();

                        for (var i = 0; i < table.length; i++) {

                            var PROJID = table[i][0];
                            var PROJN = (table[i][1].split(':').length == 2 ? table[i][1].split(':')[1].trim() : table[i][1].trim());
                            var PROPID = table[i][2];
                            var PROPN = (table[i][3].split(':').length == 2 ? table[i][3].split(':')[1].trim() : table[i][1].trim());

                            dataDetails.push({
                                "PROJID": PROJID,
                                "PROJN": PROJN,
                                "PROPID": PROPID,
                                "PROPN": PROPN,
                                "QUANT": table[i][4],
                                "STDDT": table[i][5],
                                "ENDDT": table[i][6],
                                "RMDB": table[i][7],
                                "ITEMS": (i + 1)
                            });
                        }
                        var lstRef = $('#RefDocID').val();
                        if (lstRef == null) {
                            refDetails.push({
                                "DRNO": ''
                            });
                        }
                        else {
                            for (var i = 0; i < lstRef.length; i++) {
                                refDetails.push({
                                    "DRNO": lstRef[i]
                                });
                            }
                        }
                        var gl = ($('#cbxAddGLNo option:selected').text().split(':').length == 2 ?
                            $('#cbxAddGLNo option:selected').text().split(':')[1].trim() :
                            $('#cbxAddGLNo option:selected').text().trim());

                        var appr = ($('#cbxAddApprover option:selected').text().split(':').length == 2 ?
                            $('#cbxAddApprover option:selected').text().split(':')[1].trim() :
                            $('#cbxAddApprover option:selected').text().trim());

                        var appvid = $('#cbxAddApprover').val();
                        if (Permission.Admin || Permission.CLevel || Permission.HeadOf) {
                            appvid = '';
                            appr = '';
                        }

                        var data = [];
                        data.push({
                            "RQRNO": $('#newDocID').val(),
                            "DRNO": refDetails,
                            "RCVD": $('#recieveDocDatePick').val(),
                            "CURD": $('#bookDocDatePick').val(),
                            "REFREQID": (Permission.Admin == true ? Permission.UserDetails[0].EmpCode : ""),
                            "REQID": $('#cbxAddUser').val(),
                            "REQN": $('#cbxAddUser option:selected').text().split(':')[1].trim(),
                            "PROMOTID": $('#cbxAddPromotionType').val(),
                            "PROMOTN": $('#cbxAddPromotionType option:selected').text().split(':')[1].trim(),
                            "DEPTID": $('#cbxAddCostCenter').val(),
                            "DEPTN": $('#cbxAddCostCenter option:selected').text().split(':')[1].trim(),
                            "GLID": $('#cbxAddGLNo').val(),
                            "GLN": gl,
                            "OBJID": $('#cbxAddObjective').val(),
                            "OBJN": $('#cbxAddObjective option:selected').text().trim(),
                            "REMK": $('#txtRemark').val(),

                            "APPVID": appvid,
                            "APPVN": appr,

                            "STATUS": status,
                            "dataDetails": dataDetails,
                        });
                    }
                    return data;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 6:
                    if (Permission.MKT || (Permission.Admin && ($('#RefDocID').val() != null && $('#RefDocID').val()[0] != ""))) {
                        var dataDetails = [];
                        var refDetails = [];
                        var table = $('#mktTable').DataTable().data();

                        for (var i = 0; i < table.length; i++) {

                            var projn = (table[i][2].split(':').length == 2 ?
                                table[i][2].split(':')[1].trim() :
                                table[i][2].trim());

                            var propn = (table[i][3].split(':').length == 2 ?
                                table[i][3].split(':')[1].trim() :
                                table[i][3].trim());

                            dataDetails.push({
                                "DRNOBYITEM": table[i][3].trim(),
                                "PROJID": table[i][1].split(':')[0].trim(),
                                "PROJN": projn,
                                "PROPID": table[i][4].trim(),
                                "PROPN": table[i][5].trim(),
                                "RMDB": '',
                                "STDDT": '',
                                "ENDDT": '',
                                "QUANT": table.$('input')[i].value,
                                "ITEMS": (i + 1)
                            });
                        }
                        var lstRef = $('#RefDocID').val();
                        if (lstRef == null) {
                            refDetails.push({
                                "DRNO": ''
                            });
                        }
                        else {
                            for (var i = 0; i < lstRef.length; i++) {
                                refDetails.push({
                                    "DRNO": lstRef[i]
                                });
                            }
                        }

                        var gl = ($('#cbxAddGLNo option:selected').text().split(':').length == 2 ?
                            $('#cbxAddGLNo option:selected').text().split(':')[1].trim() :
                            $('#cbxAddGLNo option:selected').text().trim());

                        var appr = ($('#cbxAddApprover option:selected').text().split(':').length == 2 ?
                            $('#cbxAddApprover option:selected').text().split(':')[1].trim() :
                            $('#cbxAddApprover option:selected').text().trim());
                        var appvid = $('#cbxAddApprover').val();

                        var data = [];
                        data.push({
                            "RQRNO": $('#newDocID').val(),
                            "DRNO": refDetails,
                            "RCVD": $('#recieveDocDatePick').val(),
                            "CURD": $('#bookDocDatePick').val(),
                            "REFREQID": (Permission.Admin == true ? Permission.UserDetails[0].EmpCode : ""),
                            "REQID": $('#cbxAddUser').val(),
                            "REQN": $('#cbxAddUser option:selected').text().split(':')[1].trim(),
                            "PROMOTID": $('#cbxAddPromotionType').val(),
                            "PROMOTN": $('#cbxAddPromotionType option:selected').text().split(':')[1].trim(),
                            "DEPTID": $('#cbxAddCostCenter').val(),
                            "DEPTN": $('#cbxAddCostCenter option:selected').text().split(':')[1].trim(),
                            "GLID": $('#cbxAddGLNo').val(),
                            "GLN": gl,
                            "OBJID": $('#cbxAddObjective').val(),
                            "OBJN": $('#cbxAddObjective option:selected').text().trim(),
                            "REMK": $('#txtRemark').val(),
                            "APPVID": appvid,
                            "APPVN": appr,
                            "STATUS": 0,
                            "dataDetails": dataDetails,
                        });
                        return data;
                    }
                    else if (!Permission.MKT) {
                        var dataDetails = [];
                        var refDetails = [];
                        var table = $('#table-product').DataTable().data();

                        for (var i = 0; i < table.length; i++) {


                            var projn = (table[i][1].split(':').length == 2 ?
                                table[i][1].split(':')[1].trim() :
                                table[i][1].trim());

                            var propn = (table[i][3].split(':').length == 2 ?
                                table[i][3].split(':')[1].trim() :
                                table[i][3].trim());


                            dataDetails.push({
                                "PROJID": table[i][0].trim(),
                                "PROJN": projn,
                                "PROPID": table[i][2].trim(),
                                "PROPN": propn,
                                "QUANT": table[i][4].trim(),
                                "STDDT": table[i][5].trim(),
                                "ENDDT": table[i][6].trim(),
                                "RMDB": table[i][7].trim(),
                                "ITEMS": (i + 1)
                            });
                        }
                        var lstRef = $('#RefDocID').val();
                        if (lstRef == null) {
                            refDetails.push({
                                "DRNO": ''
                            });
                        }
                        else {
                            for (var i = 0; i < lstRef.length; i++) {
                                refDetails.push({
                                    "DRNO": lstRef[i]
                                });
                            }
                        }

                        var gl = ($('#cbxAddGLNo option:selected').text().split(':').length == 2 ?
                            $('#cbxAddGLNo option:selected').text().split(':')[1].trim() :
                            $('#cbxAddGLNo option:selected').text().trim());

                        var appr = ($('#cbxAddApprover option:selected').text().split(':').length == 2 ?
                            $('#cbxAddApprover option:selected').text().split(':')[1].trim() :
                            $('#cbxAddApprover option:selected').text().trim());
                        var appvid = $('#cbxAddApprover').val();
                        //if (Permission.Admin || Permission.CLevel || Permission.HeadOf) {
                        //    appvid = '';
                        //    appr = '';
                        //}

                        var data = [];
                        data.push({
                            "RQRNO": $('#newDocID').val(),
                            "DRNO": refDetails,
                            "RCVD": $('#recieveDocDatePick').val(),
                            "CURD": $('#bookDocDatePick').val(),
                            "REFREQID": (Permission.Admin == true ? Permission.UserDetails[0].EmpCode : ""),
                            "REQID": $('#cbxAddUser').val(),
                            "REQN": $('#cbxAddUser option:selected').text().split(':')[1].trim(),
                            "PROMOTID": $('#cbxAddPromotionType').val(),
                            "PROMOTN": $('#cbxAddPromotionType option:selected').text().split(':')[1].trim(),
                            "DEPTID": $('#cbxAddCostCenter').val(),
                            "DEPTN": $('#cbxAddCostCenter option:selected').text().split(':')[1].trim(),
                            "GLID": $('#cbxAddGLNo').val(),
                            "GLN": gl,
                            "OBJID": $('#cbxAddObjective').val(),
                            "OBJN": $('#cbxAddObjective option:selected').text().trim(),
                            "REMK": $('#txtRemark').val(),
                            "APPVID": appvid,
                            "APPVN": appr,
                            "STATUS": 0,
                            "dataDetails": dataDetails,
                        });
                        return data;
                    }
            }
        }

        function Recall(status, rqrno) {
            if (status == 0) {
                var status = JSON.stringify({ rqrno: rqrno });
                $.ajax({
                    type: "POST",
                    url: "RequisitionRequest.aspx/SetOpenRecallRequisition",
                    data: status,
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                            textStatus + "\n\nError: " + errorThrown);
                    },
                    success: function (result) {
                        ClearData('#dynamic-table');
                        try {

                            $.each(result, function (k, Val) {

                                if (Val.MsgErr != '') {
                                    bootbox.alert('<h4><center>' + Val.MsgErr + '</center></h4>');
                                }

                                $.each(Val.lstrr, function (key, Value) {
                                    var data = RenderDatatable(Value);
                                    $('#iBody').append(data);
                                });
                            });

                        } catch (e) {

                        }
                        CallDatatable();
                    }
                });
            } else if (status == 1) {
                var status = JSON.stringify({ rqrno: rqrno });
                $.ajax({
                    type: "POST",
                    url: "RequisitionRequest.aspx/SetConfirmRecallRequisition",
                    data: status,
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                            textStatus + "\n\nError: " + errorThrown);
                    },
                    success: function (result) {
                        ClearData('#dynamic-table');
                        try {

                            $.each(result, function (k, Val) {

                                if (Val.MsgErr != '') {
                                    bootbox.alert('<h4><center>' + Val.MsgErr + '</center></h4>');
                                }

                                $.each(Val.lstrr, function (key, Value) {
                                    var data = RenderDatatable(Value);
                                    $('#iBody').append(data);
                                });
                            });

                        } catch (e) {

                        }
                        CallDatatable();
                    }
                });
            }

        }

        function SaveData(status) {

            if (status == 7) {
                if (Permission.CLevel || Permission.Admin || Permission.HeadOf || Permission.MKT) {
                    status = 2
                }
            }

            switch (status) {
                case 0:
                    var data = ManageDataBeforeSave(status);
                    var ss = JSON.stringify({ data: data });

                    $.ajax({
                        type: "POST",
                        url: "RequisitionRequest.aspx/SaveData",
                        data: JSON.stringify({ data: data }),
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        async: false,
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                                textStatus + "\n\nError: " + errorThrown);
                        },
                        success: function (result) {
                            ClearData('#dynamic-table');
                            try {

                                $.each(result, function (k, Val) {

                                    if (Val.MsgErr != '') {
                                        bootbox.alert('<h4><center>' + Val.MsgErr + '</center></h4>');
                                    }

                                    $.each(Val.lstrr, function (key, Value) {
                                        var data = RenderDatatable(Value);
                                        $('#iBody').append(data);
                                    });
                                });

                            } catch (e) {

                            }
                            CallDatatable();
                        }
                    });

                    break;
                case 1:
                    /*

                    */ // Edit

                    var data = ManageDataBeforeSave(6);
                    var ss = JSON.stringify({ data: data });

                    $.ajax({
                        type: "POST",
                        url: "RequisitionRequest.aspx/UpdateData",
                        data: JSON.stringify({ data: data }),
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                                textStatus + "\n\nError: " + errorThrown);
                        },
                        success: function (result) {
                            ClearData('#dynamic-table');
                            try {

                                $.each(result, function (k, Val) {

                                    if (Val.MsgErr != '') {
                                        bootbox.alert('<h4><center>' + Val.MsgErr + '</center></h4>');
                                    }

                                    $.each(Val.lstrr, function (key, Value) {
                                        var data = RenderDatatable(Value);
                                        $('#iBody').append(data);
                                    });
                                });

                            } catch (e) {

                            }
                            CallDatatable();
                        }
                    });

                    break;
                case 2:
                    /*

                    */ // Approve = Change Status => 2

                    var docno = ($('#a_DocID').val() == '' ? $('#newDocID').val() : $('#a_DocID').val());

                    var status = JSON.stringify({ rqrno: docno });
                    $.ajax({
                        type: "POST",
                        url: "RequisitionRequest.aspx/SetApproveRequisition",
                        data: status,
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                                textStatus + "\n\nError: " + errorThrown);
                        },
                        success: function (result) {
                            ClearData('#dynamic-table');
                            try {

                                $.each(result, function (k, Val) {

                                    if (Val.MsgErr != '') {
                                        bootbox.alert('<h4><center>' + Val.MsgErr + '</center></h4>');
                                    }

                                    $.each(Val.lstrr, function (key, Value) {
                                        var data = RenderDatatable(Value);
                                        $('#iBody').append(data);
                                    });
                                });

                            } catch (e) {

                            }
                            CallDatatable();
                        }
                    });
                    break;
                case 3:
                    /*

                    */ // Approve = Change Status => 3

                    var status = JSON.stringify({ rqrno: $('#a_DocID').val() });
                    $.ajax({
                        type: "POST",
                        url: "RequisitionRequest.aspx/SetAcceptRequisition",
                        data: status,
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                                textStatus + "\n\nError: " + errorThrown);
                        },
                        success: function (result) {
                            ClearData('#dynamic-table');
                            try {

                                $.each(result, function (k, Val) {

                                    if (Val.MsgErr != '') {
                                        bootbox.alert('<h4><center>' + Val.MsgErr + '</center></h4>');
                                    }

                                    $.each(Val.lstrr, function (key, Value) {
                                        var data = RenderDatatable(Value);
                                        $('#iBody').append(data);
                                    });
                                });

                            } catch (e) {

                            }
                            CallDatatable();
                        }
                    });
                    break;
                case 4:

                    //------------ recall

                    var status = JSON.stringify({ rqrno: $('#newDocID').val() });
                    $.ajax({
                        type: "POST",
                        url: "RequisitionRequest.aspx/SetAcceptRequisition",
                        data: status,
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                                textStatus + "\n\nError: " + errorThrown);
                        },
                        success: function (result) {
                            ClearData('#dynamic-table');
                            try {

                                $.each(result, function (k, Val) {

                                    if (Val.MsgErr != '') {
                                        bootbox.alert('<h4><center>' + Val.MsgErr + '</center></h4>');
                                    }

                                    $.each(Val.lstrr, function (key, Value) {
                                        var data = RenderDatatable(Value);
                                        $('#iBody').append(data);
                                    });
                                });

                            } catch (e) {

                            }
                            CallDatatable();
                        }
                    });



                    break;
                case 5:

                    break;
                case 6:
                    var data = ManageDataBeforeSave(status);
                    var ss = JSON.stringify({ data: data });

                    $.ajax({
                        type: "POST",
                        url: "RequisitionRequest.aspx/SaveData",
                        data: JSON.stringify({ data: data }),
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                                textStatus + "\n\nError: " + errorThrown);
                        },
                        success: function (result) {
                            ClearData('#dynamic-table');
                            try {

                                $.each(result, function (k, Val) {

                                    if (Val.MsgErr != '') {
                                        bootbox.alert('<h4><center>' + Val.MsgErr + '</center></h4>');
                                    }

                                    $.each(Val.lstrr, function (key, Value) {
                                        var data = RenderDatatable(Value);
                                        $('#iBody').append(data);
                                    });
                                });

                            } catch (e) {

                            }
                            CallDatatable();
                        }
                    });
                    break;
                case 7:

                    var status = JSON.stringify({ rqrno: $('#newDocID').val() });
                    $.ajax({
                        type: "POST",
                        url: "RequisitionRequest.aspx/SetSentRequisition",
                        data: status,
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                                textStatus + "\n\nError: " + errorThrown);
                        },
                        success: function (result) {
                            ClearData('#dynamic-table');
                            try {

                                $.each(result, function (k, Val) {

                                    if (Val.MsgErr != '') {
                                        bootbox.alert('<h4><center>' + Val.MsgErr + '</center></h4>');
                                    }

                                    $.each(Val.lstrr, function (key, Value) {
                                        var data = RenderDatatable(Value);
                                        $('#iBody').append(data);
                                    });
                                });

                            } catch (e) {

                            }
                            CallDatatable();
                        }
                    });
                    break;
            }
        }

        function RenderDatatable(str) {
            var _status;
            var _icons;
            if (str.STATUS == 1 && (!Permission.Admin && !Permission.CLevel && !Permission.HeadOf)) {
                _status = '<span class="label label-yellow" style="border-radius: 25px" id="status_' + str.RQRNO.trim() + '">' +
                    '<i class="ace-icon fa fa-bell-o bigger-120"></i> ' +
                    'รออนุมัติ' +
                    '</span>';
                _icons = '<div class="hidden-sm hidden-xs action-buttons">' +
                    '<a href="#myModal" class="btn btn-sm btn-white btn-default btn-round open-myModal" id="' + str.RQRNO.trim() + '" data-toggle="modal" data-target="#myModal" data-id=\'' + str.RQRNO.trim() + '\'>' +
                    'แสดง' +
                    '</a>' +
                    '<a href="#" class="btn btn-sm btn-white btn-default btn-round" id="' + str.RQRNO.trim() + '" onclick="recallDataRequisition(\'' + str.RQRNO.trim() + '\');return false;">' +
                    'เรียกคืน' +
                    '</a>' +
                    '</div>';

            } else if (str.STATUS == 3) {
                _status = '<span class="label label-primary" style="border-radius: 25px" id="status_' + str.RQRNO.trim() + '">' +
                    '<i class="ace-icon glyphicon glyphicon-ok bigger-120"></i> ' +
                    'รอจ่าย' +
                    '</span>';
                if (Permission.Admin) {
                    _icons = '<div class="hidden-sm hidden-xs action-buttons">' +
                        '<a href="#myModal" class="btn btn-sm btn-white btn-default btn-round open-myModal" id="' + str.RQRNO.trim() + '" data-toggle="modal" data-target="#myModal" data-id=\'' + str.RQRNO.trim() + '\'>' +
                        'แสดง' +
                        '</a>' +
                        //'<a class="green" href="#" id="Approve'+str.RQRNO.trim()+'" onclick="viewDataRequisition(\''+str.RQRNO.trim()+'\');return false;"><i class="ace-icon fa fa-search bigger-130"></i></a>' +
                        '</div>';
                } else {
                    _icons = '<div class="hidden-sm hidden-xs action-buttons">' +
                        '<a href="#myModal" class="btn btn-sm btn-white btn-default btn-round open-myModal" id="' + str.RQRNO.trim() + '" data-toggle="modal" data-target="#myModal" data-id=\'' + str.RQRNO.trim() + '\'>' +
                        'แสดง' +
                        '</a>' +
                        //'<a class="green" href="#" id="Approve'+str.RQRNO.trim()+'" onclick="viewDataRequisition(\''+str.RQRNO.trim()+'\');return false;"><i class="ace-icon fa fa-search bigger-130"></i></a>' +
                        '</div>';
                }
            } else if (str.STATUS == 1 && (Permission.Admin || Permission.CLevel || Permission.HeadOf)) {
                if (Permission.Admin) {
                    if (str.REQID == Permission.UserDetails[0].EmpCode) {
                        _status = '<span class="label label-yellow" style="border-radius: 25px" id="status_' + str.RQRNO.trim() + '">' +
                            '<i class="ace-icon fa fa-exclamation-circle bigger-120"></i> ' +
                            'รออนุมัติ' +
                            '</span>';
                        _icons = '<div class="hidden-sm hidden-xs action-buttons">' +
                            '<a href="#myModal" class="btn btn-sm btn-white btn-default btn-round open-myModal" id="' + str.RQRNO.trim() + '" data-toggle="modal" data-target="#myModal" data-id=\'' + str.RQRNO.trim() + '\'>' +
                            'อนุมัติ' +
                            '</a>' +
                            //'<a class="green" href="#" id="Approve'+str.RQRNO.trim()+'" onclick="viewDataRequisition(\''+str.RQRNO.trim()+'\');return false;"><i class="ace-icon fa fa-search bigger-130"></i></a>' +
                            '</div>';
                    } else if (str.REFREQID == Permission.UserDetails[0].EmpCode) {
                        _status = '<span class="label label-yellow" style="border-radius: 25px" id="status_' + str.RQRNO.trim() + '">' +
                            '<i class="ace-icon fa fa-bell-o bigger-120"></i> ' +
                            'รออนุมัติ' +
                            '</span>';
                        _icons = '<div class="hidden-sm hidden-xs action-buttons">' +
                            '<a href="#myModal" class="btn btn-sm btn-white btn-default btn-round open-myModal" id="' + str.RQRNO.trim() + '" data-toggle="modal" data-target="#myModal" data-id=\'' + str.RQRNO.trim() + '\'>' +
                            'อนุมัติ' +
                            '</a>' +

                            '</div>';
                    } else {
                        _status = '<span class="label label-yellow" style="border-radius: 25px" id="status_' + str.RQRNO.trim() + '">' +
                            '<i class="ace-icon fa fa-bell-o bigger-120"></i> ' +
                            'รออนุมัติ' +
                            '</span>';
                        _icons = '<div class="hidden-sm hidden-xs action-buttons">' +
                            '<a href="#myModal" class="btn btn-sm btn-white btn-default btn-round open-myModal" id="' + str.RQRNO.trim() + '" data-toggle="modal" data-target="#myModal" data-id=\'' + str.RQRNO.trim() + '\'>' +
                            'แสดง' +
                            '</a>' +

                            '</div>';
                    }

                } else if (Permission.CLevel || Permission.HeadOf) {
                    if (str.REQID == Permission.UserDetails[0].EmpCode) {
                        _status = '<span class="label label-yellow" style="border-radius: 25px" id="status_' + str.RQRNO.trim() + '">' +
                            '<i class="ace-icon fa fa-bell-o bigger-120"></i> ' +
                            'รออนุมัติ' +
                            '</span>';
                        _icons = '<div class="hidden-sm hidden-xs action-buttons">' +
                            '<a href="#myModal" class="btn btn-sm btn-white btn-default btn-round open-myModal" id="' + str.RQRNO.trim() + '" data-toggle="modal" data-target="#myModal" data-id=\'' + str.RQRNO.trim() + '\'>' +
                            'อนุมัติ' +
                            '</a>' +
                            '</div>';
                    } else {
                        _status = '<span class="label label-yellow" style="border-radius: 25px" id="status_' + str.RQRNO.trim() + '">' +
                            '<i class="ace-icon fa fa-exclamation-circle bigger-120"></i> ' +
                            'รออนุมัติ' +
                            '</span>';
                        _icons = '<div class="hidden-sm hidden-xs action-buttons">' +
                            '<a href="#myModal" class="btn btn-sm btn-white btn-default btn-round open-myModal" id="' + str.RQRNO.trim() + '" data-toggle="modal" data-target="#myModal" data-id=\'' + str.RQRNO.trim() + '\'>' +
                            'อนุมัติ' +
                            '</a>' +
                            '</div>';
                    }
                }
            } else if (str.STATUS == 2) {
                _status = '<span class="label label-success" style="border-radius: 25px" id="status_' + str.RQRNO.trim() + '">' +
                    '<i class="ace-icon fa fa-check-square-o bigger-120"></i> ' +
                    'อนุมัติ' +
                    '</span>';
                if (Permission.Admin) {
                    _icons = '<div class="hidden-sm hidden-xs action-buttons">' +
                        '<a href="#myModal" class="btn btn-sm btn-white btn-default btn-round open-myModal" id="' + str.RQRNO.trim() + '" data-toggle="modal" data-target="#myModal" data-id=\'' + str.RQRNO.trim() + '\'>' +
                        'รับงาน' +
                        '</a>' +
                        '</div>';
                } else if (Permission.CLevel || Permission.HeadOf) {
                    _icons = '<div class="hidden-sm hidden-xs action-buttons">' +
                        '<a href="#myModal" class="btn btn-sm btn-white btn-default btn-round open-myModal" id="' + str.RQRNO.trim() + '" data-toggle="modal" data-target="#myModal" data-id=\'' + str.RQRNO.trim() + '\'>' +
                        'แสดง' +
                        '</a>' +
                        '</div>';
                } else {
                    _icons = '<div class="hidden-sm hidden-xs action-buttons">' +
                        '<a href="#myModal" class="btn btn-sm btn-white btn-default btn-round open-myModal" id="' + str.RQRNO.trim() + '" data-toggle="modal" data-target="#myModal" data-id=\'' + str.RQRNO.trim() + '\'>' +
                        'แสดง' +
                        '</a>' +
                        '</div>';
                }
            } else if (str.STATUS == 4) {

                _status = '<span class="label label-inverse" style="border-radius: 25px" id="status_' + str.RQRNO.trim() + '">' +
                    '<i class="ace-icon fa fa-exchange bigger-120"></i> ' +
                    'เรียกคืน' +
                    '</span>';

                if (Permission.Admin) {
                    _icons = '<div class="hidden-sm hidden-xs action-buttons">' +
                        '<a href="#myModal" class="btn btn-sm btn-white btn-default btn-round open-myModal" id="' + str.RQRNO.trim() + '" data-toggle="modal" data-target="#myModal" data-id=\'' + str.RQRNO.trim() + '\'>' +
                        'แสดง' +
                        '</a>' +
                        '<a class="btn btn-sm btn-white btn-default btn-round" href="#" id="Recall' + str.RQRNO.trim() + '" onclick="recallDataRequisition(\'' + str.RQRNO.trim() + '\');return false;">' +
                        'รอเรียกคืน' +
                        '</a>' +
                        '</div>';
                } else if (Permission.CLevel || Permission.HeadOf) {
                    _icons = '<div class="hidden-sm hidden-xs action-buttons">' +
                        '<a href="#myModal" class="btn btn-sm btn-white btn-default btn-round open-myModal" id="' + str.RQRNO.trim() + '" data-toggle="modal" data-target="#myModal" data-id=\'' + str.RQRNO.trim() + '\'>' +
                        'แสดง' +
                        '</a>' +
                        '</div>';
                } else {
                    _icons = '<div class="hidden-sm hidden-xs action-buttons">' +
                        '<a href="#myModal" class="btn btn-sm btn-white btn-default btn-round open-myModal" id="' + str.RQRNO.trim() + '" data-toggle="modal" data-target="#myModal" data-id=\'' + str.RQRNO.trim() + '\'>' +
                        'แสดง' +
                        '</a>' +
                        '<a class="btn btn-sm btn-white btn-default btn-round" href="#" id="Recall' + str.RQRNO.trim() + '" onclick="recallDataRequisition(\'' + str.RQRNO.trim() + '\');return false;">' +
                        'รอเรียกคืน' +
                        '</a>' +
                        '</div>';
                }

            } else if (str.STATUS == 0) {
                if ((str.REQID == Permission.UserDetails[0].EmpCode) && Permission.Admin) {
                    _status = '<span class="label label-custom" style="border-radius: 25px" id="status_' + str.RQRNO.trim() + '">' +
                        '<i class="ace-icon fa fa-folder-open-o bigger-120"></i> ' +
                        'เตรียมส่ง' +
                        '</span>';
                    _icons = '<div class="hidden-sm hidden-xs action-buttons">' +
                        '<a class="btn btn-sm btn-white btn-default btn-round" href="#" id="Edit' + str.RQRNO.trim() + '" onclick="editDataRequisition(\'' + str.RQRNO.trim() + '\');return false;">' +
                        'อนุมัติ' +
                        '</a>' +
                        '<a href="#" class="btn btn-sm btn-white btn-default btn-round" id="' + str.RQRNO.trim() + '" onclick="deleteDataRequisition(\'' + str.RQRNO.trim() + '\');return false;">' +
                        'ลบ' +
                        '</a>' +
                        '</div>';
                } else if (str.REQID == Permission.UserDetails[0].EmpCode) {
                    _status = '<span class="label label-custom" style="border-radius: 25px" id="status_' + str.RQRNO.trim() + '">' +
                        '<i class="ace-icon fa fa-folder-open-o bigger-120"></i> ' +
                        'เตรียมส่ง' +
                        '</span>';
                    _icons = '<div class="hidden-sm hidden-xs action-buttons">' +
                        '<a class="btn btn-sm btn-white btn-default btn-round" href="#" id="Edit' + str.RQRNO.trim() + '" onclick="editDataRequisition(\'' + str.RQRNO.trim() + '\');return false;">' +
                        'อนุมัติ' +
                        '</a>' +
                        '<a href="#" class="btn btn-sm btn-white btn-default btn-round" id="' + str.RQRNO.trim() + '" onclick="deleteDataRequisition(\'' + str.RQRNO.trim() + '\');return false;">' +
                        'ลบ' +
                        '</a>' +
                        '</div>';
                } else if ((str.REQID != Permission.UserDetails[0].EmpCode) && Permission.Admin) {
                    _status = '<span class="label label-custom" style="border-radius: 25px" id="status_' + str.RQRNO.trim() + '">' +
                        '<i class="ace-icon fa fa-folder-open-o bigger-120"></i> ' +
                        'เตรียมส่ง' +
                        '</span>';
                    _icons = '<div class="hidden-sm hidden-xs action-buttons">' +
                        '<a href="#myModal" class="btn btn-sm btn-white btn-default btn-round open-myModal" id="' + str.RQRNO.trim() + '" data-toggle="modal" data-target="#myModal" data-id=\'' + str.RQRNO.trim() + '\'>' +
                        'แสดง' +
                        '</a>' +
                        '</div>';
                } else {
                    return;
                }


            } else if (str.STATUS == 5) {
                _status = '<span class="label label-danger" style="border-radius: 25px" id="status_' + str.RQRNO.trim() + '">' +
                    '<i class="ace-icon fa fa-ban bigger-120"></i> ' +
                    'ปฏิเสธ' +
                    '</span>';
                _icons = '<div class="hidden-sm hidden-xs action-buttons">' +
                    '<a href="#myModal" class="btn btn-sm btn-white btn-default btn-round open-myModal" id="' + str.RQRNO.trim() + '" data-toggle="modal" data-target="#myModal" data-id=\'' + str.RQRNO.trim() + '\'>' +
                    'แสดง' +
                    '</a>' +

                    '</div>';
            } else {
                return;
            }

            var res = '<tr>' +
                '<td style="text-align: center">' + str.CURD + '</td>' +
                '<td style="text-align: center">' + str.RQRNO + '</td>' +
                '<td style="text-align: center">' + str.DRNO + '</td>' +
                '<td style="text-align: right">' + str.REQN + '</td>' +
                '<td style="text-align: center">' +
                _status +
                '</td>' +
                '<td>' +
                _icons +
                '</td>' +
                '</tr>';

            return res;
        }

        function deleteUserGroup(id) {
            var table = $('#table-product').DataTable();

            $('#table-product tbody').on('click', 'i.ace-icon', function () {

                table
                    .row($(this).parents('tr'))
                    .remove()
                    .draw();
            });
        }

        function initialMarketingForm() {

        }
        function initialHRSecretaryForm() {

        }
        function initialETCForm() {

        }
        function initialApproverForm() {

        }
        function initialAdminForm() {

        }

        function validate(index) {

            if (index == 1) {
                if ($('#RefDocID').val() == "" || $('#RefDocID').val() == null) {
                    messagefn.infomation2('<h4><center>กรุณาระบุ<br /><br />เลขที่เอกสารอ้างอิง</center></h4>', 'warning', 'OK', 'btn-sm btn-warning');
                    return false;
                }
                if ($('#recieveDocDatePick').val() == "") {
                    messagefn.infomation2('<h4><center>กรุณาระบุ<br /><br />วันที่รับเอกสารขอเบิก</center></h4>', 'warning', 'OK', 'btn-sm btn-warning');
                    return false;
                }
                if ($('#bookDocDatePick').val() == "") {
                    messagefn.infomation2('<h4><center>กรุณาระบุ<br /><br />วันที่บันทึกใบขอเบิก</center></h4>', 'warning', 'OK', 'btn-sm btn-warning');
                    return false;
                }
                if ($('#cbxAddUser').val() == "") {
                    messagefn.infomation2('<h4><center>กรุณาระบุ<br /><br />วันที่รับเอกสารขอเบิก</center></h4>', 'warning', 'OK', 'btn-sm btn-warning');
                    return false;
                }
                if ($('#cbxAddPromotionType').val() == "") {
                    messagefn.infomation2('<h4><center>กรุณาระบุ<br /><br />โปรโมชั่น</center></h4>', 'warning', 'OK', 'btn-sm btn-warning');
                    return false;
                }
                if ($('#cbxAddCostCenter').val() == "") {
                    messagefn.infomation2('<h4><center>กรุณาระบุ<br /><br />หน่วยงาน</center></h4>', 'warning', 'OK', 'btn-sm btn-warning');
                    return false;
                }
                if ($('#cbxAddGLNo').val() == "") {
                    messagefn.infomation2('<h4><center>กรุณาระบุ<br /><br />วัตถุประสงค์</center></h4>', 'warning', 'OK', 'btn-sm btn-warning');
                    return false;
                }
                if ($('#cbxAddObjective').val() == "") {
                    messagefn.infomation2('<h4><center>กรุณาระบุ<br /><br />วิธีการแจก</center></h4>', 'warning', 'OK', 'btn-sm btn-warning');
                    return false;
                }
                return true;
            } else if (index == 2) {
                if ($('#table-product').DataTable().data().length == 0) {
                    if ($('#RefDocID').val().length > 0 && Permission.Admin) {
                        var isVerify = false;
                        $('#table-product').DataTable().destroy();
                        if ($('#mktTable').DataTable().data().length > 0) {
                            var rows = $('#mktTable').DataTable().data().length;
                            for (var i = 0; i < rows; i++) {
                                var table = $('#mktTable').dataTable();
                                if (table.$('input')[i].value == '0' || table.$('input')[i].value == '') {
                                    bootbox.alert('<h4><center>กรุณาระบุ จำนวนขอเบิก!</center></h4>')
                                    return false;
                                }
                            }
                            isVerify = true;
                        }
                        else {
                            bootbox.alert('<h4><center>ไม่มีรายการขอเบิก!</center></h4>')
                            return false;
                        }
                        if (!isVerify && $('#cbxAddProject option:selected').val() == 'All' || $('#cbxAddProject option:selected').val() == '') {
                            bootbox.alert('<h4><center>กรุณาเลือก โครงการ!</center></h4>')
                            return false;
                        }
                    }
                    else if (Permission.MKT) {
                        var isVerify = false;
                        $('#table-product').DataTable().destroy();
                        if ($('#mktTable').DataTable().data().length > 0) {
                            var rows = $('#mktTable').DataTable().data().length;
                            for (var i = 0; i < rows; i++) {
                                var table = $('#mktTable').dataTable();
                                if (table.$('input')[i].value == '0' || table.$('input')[i].value == '') {
                                    bootbox.alert('<h4><center>กรุณาระบุ จำนวนขอเบิก!</center></h4>')
                                    return false;
                                }
                            }
                            isVerify = true;
                        }
                        else {
                            bootbox.alert('<h4><center>ไม่มีรายการขอเบิก!</center></h4>')
                            return false;
                        }
                        if (!isVerify && $('#cbxAddProject option:selected').val() == 'All' || $('#cbxAddProject option:selected').val() == '') {
                            bootbox.alert('<h4><center>กรุณาเลือก โครงการ!</center></h4>')
                            return false;
                        }

                    }
                    else {
                        return false;
                    }
                }
                return true;
            } else if (index == 3) {

                return true;
            } else if (index == 4) {
                if ($('#cbxAddProject').val() == "") {
                    bootbox.alert('<h4><center>กรุณาระบุ โครงการ!</center></h4>')
                    return false;
                } else if ($('#cbxAddPromoType').val() == "") {
                    bootbox.alert('<h4><center>กรุณาระบุ สินค้าโปรโมชั่น!</center></h4>')
                    return false;
                } else if ($('#setStdDatePicker').val() == "") {
                    bootbox.alert('<h4><center>กรุณาระบุ วันใช้งานเริ่มต้น!</center></h4>')
                    return false;
                } else if ($('#setEndDatePicker').val() == "") {
                    bootbox.alert('<h4><center>กรุณาระบุ วันใช้งานสุดท้าย!</center></h4>')
                    return false;
                } else if ($('#reminDay').val() == "") {
                    bootbox.alert('<h4><center>กรุณาระบุ วันที่แจ้งเตือน!</center></h4>')
                    return false;
                } else if ($('#txtQuantity').val() == "") {
                    bootbox.alert('<h4><center>กรุณาระบุ จำนวนสินค้า!</center></h4>')
                    return false;
                }
                return true;
            }
        }

        function onTop(x) {
            $('html, body').animate({ scrollTop: 0 }, 'fast');

            var wizard = $('#fuelux-wizard-container').data('fu.wizard');
            if (wizard.currentStep != 2 && wizard.currentStep != 3) {
                $('.formkt').removeClass('hide').addClass('hide');
                $('.cls-promo').removeClass('hide').addClass('hide');

                $('#table-product').addClass('hide');
                $('#mktTable').addClass('hide');
                $('#a_t_product').addClass('hide');
                $('#a_mktTable').addClass('hide');
                if ($.fn.dataTable.isDataTable('#table-product')) {
                    table = $('#table-product').DataTable({
                        retrieve: true,
                        paging: false
                    });
                    table.destroy();
                }
                if ($.fn.dataTable.isDataTable('#a_t_product')) {
                    table = $('#a_t_product').DataTable({
                        retrieve: true,
                        paging: false
                    });
                    table.destroy();
                }
                if ($.fn.dataTable.isDataTable('#a_mktTable')) {
                    table = $('#a_mktTable').DataTable({
                        retrieve: true,
                        paging: false
                    });
                    table.destroy();
                }
                if ($.fn.dataTable.isDataTable('#mktTable') || $('#mktTable').DataTable().data().length > 0) {
                    table = $('#mktTable').DataTable({
                        retrieve: true,
                        paging: false
                    });

                    var isChange = false;
                    if ($('#RefDocID').val() != null && $('#RefDocID').val() != '') {
                        for (var i = 0; i < table.data().length; i++) {
                            for (var ii = 0; ii < $('#RefDocID').val().length; ii++) {
                                if (!($('#RefDocID').val()[ii].toString() == table.data()[i][3].toString())) {
                                    isChange = true;
                                }
                            }
                        }
                        if (isChange == true) {
                            table.destroy();
                            $('#mktTable').dataTable().fnClearTable();
                        }
                    }
                }
            }
            if (x == 1) {
                $('#save').text('อนุมัติ');
                if (validate(wizard.currentStep)) {

                    if (wizard.currentStep == 2 && m_add) {
                        $('#open').removeClass('hide');
                        $('#lblTextQ').text('คุณต้องการ บันทึก หรืออนุมัติ หรือไม่ ?');
                    } else if (wizard.currentStep == 2 && m_edit) {
                        $('#reject').removeClass('hide').addClass('hide');
                        $('#edit').removeClass('hide');
                        $('#sent').removeClass('hide');
                        $('#save').removeClass('hide').addClass('hide');
                        $('#accept').removeClass('hide').addClass('hide');
                        $('#approve').removeClass('hide').addClass('hide');
                        $('#lblTextQ').text('คุณต้องการ บันทึก หรืออนุมัติ หรือไม่ ?');
                    } else if (wizard.currentStep == 2 && m_view) {
                        $('#reject').removeClass('hide').addClass('hide');
                        $('#edit').removeClass('hide').addClass('hide');
                        $('#save').removeClass('hide').addClass('hide');
                        $('#accept').removeClass('hide').addClass('hide');
                        $('#approve').removeClass('hide').addClass('hide');
                        $('#sent').removeClass('hide').addClass('hide');

                    } else if (wizard.currentStep == 2 && m_approve) {
                        if (m_reject)
                            $('#reject').removeClass('hide');
                        $('#edit').removeClass('hide').addClass('hide');
                        $('#save').removeClass('hide').addClass('hide');
                        $('#accept').removeClass('hide').addClass('hide');
                        $('#approve').removeClass('hide');
                        $('#sent').removeClass('hide').addClass('hide');

                        $('#lblTextQ').text('คุณต้องการ อนุมัติ หรือปฎิเสธ เอกสารนี้หรือไม่ ?');

                    } else if (wizard.currentStep == 2 && m_accept) {
                        $('#reject').removeClass('hide').addClass('hide');
                        $('#edit').removeClass('hide').addClass('hide');
                        $('#save').removeClass('hide').addClass('hide');
                        $('#accept').removeClass('hide');
                        $('#approve').removeClass('hide').addClass('hide');

                        $('#lblTextQ').text('คุณต้องการที่จะ รับงานนี้หรือไม่ ?');
                        $('#sent').removeClass('hide').addClass('hide');
                    }
                    else if (wizard.currentStep == 1) {
                        if (Permission.Admin && ($('#RefDocID').val() != null && $('#RefDocID').val()[0] != "")) {
                            console.log("First");
                            if ($.fn.dataTable.isDataTable('#table-product')) {
                                table = $('#table-product').DataTable({
                                    retrieve: true,
                                    paging: false
                                });
                                table.destroy();
                            }

                            $('#table-product').addClass('hide'); $('.cls-promo').addClass('hide'); $('.cls-srh').addClass('hide');
                            $('#useperiod').addClass('hide'); $('#cbxAddProject').addClass('hide'); $('.a_project').addClass('hide');
                            $('#mktTable').removeClass('hide');
                            $('.formkt').removeClass('hide');

                            if ($.fn.dataTable.isDataTable('#mktTable')) {
                                table = $('#mktTable').DataTable({
                                    retrieve: true,
                                    paging: false
                                });
                                table.destroy();
                            }

                            var table1 = $('#mktTable').dataTable({

                                "bInfo": false,
                                "bFilter": false,
                                "bDeferRender": false,

                                "iDisplayLength": -1,
                                "aoColumns": [
                                    { 'visible': false },
                                    { 'visible': false },
                                    { "sClass": "center setwidthproject-insidetable", 'bSortable': false },
                                    { "sClass": "center setwidth-insidetable", 'bSortable': false },
                                    { 'visible': false },
                                    { "sClass": "align-right setwidthstock-insidetable", 'bSortable': false },
                                    { "sClass": "align-right setwidth-insidetable", 'bSortable': false },
                                    { "sClass": "center", 'bSortable': false },
                                    { "sClass": "center", 'bSortable': false },
                                    { "sClass": "center", 'bSortable': false }
                                ],
                                'paging': false,
                                "fnCreatedRow": function (nRow, aData, iDataIndex) {
                                    $(nRow).attr('id', aData[0]);
                                    var txtBox = $(nRow).find("input[type=text]");
                                    txtBox.attr('id', 'text-' + aData[0]);
                                },
                                "columnDefs": [{
                                    "targets": -1,
                                    "data": null,
                                    "defaultContent": '<a href="" class="btn btn-sm btn-white btn-danger btn-round btnMemoDataDel" onclick="return false;">' +
                                        '<i class="ace-icon fa fa-trash bigger-130"></i></a>'
                                }]
                            });
                            var textbox = '<input type="text" class="setwidth-textbox">';
                            var checkbox = '<input type="checkbox">';


                            $('#cbxForMemo').empty().trigger('chosen:updated');

                            var cbxMemoData = '<option id="" value="">-- กรุณาเลือก ใบบันทึกขอเบิก --</option>';
                            //var docno = '';
                            var lstRef = $('#RefDocID').val();
                            if (lstRef != null) {
                                $.ajax({
                                    type: "POST",
                                    url: "RequisitionRequest.aspx/CheckListRef",
                                    data: JSON.stringify({ lstRef: lstRef }),
                                    contentType: 'application/json; charset=utf-8',
                                    dataType: 'json',
                                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                                        alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                                            textStatus + "\n\nError: " + errorThrown);
                                    },
                                    success: function (result) {
                                        try {
                                            if (result.d.Success) {
                                                for (var i = 0; i < lstRef.length; i++) {
                                                    docno += lstRef[i];
                                                    cbxMemoData += '<option id="' + lstRef[i] + '">' + lstRef[i] + '</option>';
                                                }
                                                $('#cbxForMemo').append(cbxMemoData).trigger('chosen:updated');
                                            }
                                            else {
                                                bootbox.alert('<h4><center>' + result.d.Message + '</center></h4>', function () {
                                                    $(result.d.InvalidRefNo).each(function (k, v) {
                                                        var j = lstRef.indexOf(v);
                                                        if (j != -1) {

                                                            lstRef.splice(j, 1);
                                                        }
                                                    });
                                                    for (var i = 0; i < lstRef.length; i++) {
                                                        docno += lstRef[i];
                                                        cbxMemoData += '<option id="' + lstRef[i] + '">' + lstRef[i] + '</option>';
                                                    }
                                                    $('#cbxForMemo').append(cbxMemoData).trigger('chosen:updated');
                                                });
                                            }
                                        } catch (e) {

                                        }
                                    }
                                });
                            }
                            if (m_add) {
                                $('#mktTable').dataTable().fnClearTable();
                            }
                        } else if (Permission.MKT) {
                            console.log("Second");
                            if ($.fn.dataTable.isDataTable('#table-product')) {
                                table = $('#table-product').DataTable({
                                    retrieve: true,
                                    paging: false
                                });
                                table.destroy();
                            }

                            $('#table-product').addClass('hide'); $('.cls-promo').addClass('hide'); $('.cls-srh').addClass('hide');
                            $('#useperiod').addClass('hide'); $('#cbxAddProject').addClass('hide'); $('.a_project').addClass('hide');
                            $('#mktTable').removeClass('hide');
                            $('.formkt').removeClass('hide');

                            if ($.fn.dataTable.isDataTable('#mktTable') || $('#mktTable').DataTable().data().length > 0) {
                                table = $('#mktTable').DataTable({
                                    retrieve: true,
                                    paging: false
                                });
                                var isChange = false;
                                if ($('#RefDocID').val() != null && $('#RefDocID').val() != '') {
                                    for (var i = 0; i < table.data().length; i++) {
                                        for (var ii = 0; ii < $('#RefDocID').val().length; ii++) {
                                            if (!($('#RefDocID').val()[ii].toString() == table.data()[i][3].toString())) {
                                                isChange = true;
                                            }
                                        }
                                    }
                                    if (isChange == true) {
                                        table.destroy();
                                        $('#mktTable').dataTable().fnClearTable();
                                        var table1 = $('#mktTable').dataTable({

                                            "bInfo": false,
                                            "bFilter": false,
                                            "bDeferRender": false,

                                            "iDisplayLength": -1,
                                            "aoColumns": [
                                                { 'visible': false },
                                                { 'visible': false },
                                                { "sClass": "center setwidthproject-insidetable", 'bSortable': false },
                                                { "sClass": "center setwidth-insidetable", 'bSortable': false },
                                                { 'visible': false },
                                                { "sClass": "align-right setwidthstock-insidetable", 'bSortable': false },
                                                { "sClass": "align-right setwidth-insidetable", 'bSortable': false },
                                                { "sClass": "center", 'bSortable': false },
                                                { "sClass": "center", 'bSortable': false },
                                                { "sClass": "center", 'bSortable': false }
                                            ],
                                            'paging': false,
                                            "fnCreatedRow": function (nRow, aData, iDataIndex) {
                                                $(nRow).attr('id', aData[0]);
                                                var txtBox = $(nRow).find("input[type=text]");
                                                txtBox.attr('id', 'text-' + aData[0]);
                                            },
                                            "columnDefs": [{
                                                "targets": -1,
                                                "data": null,
                                                "defaultContent": '<a href="" class="btn btn-sm btn-white btn-danger btn-round btnMemoDataDel" onclick="return false;">' +
                                                    '<i class="ace-icon fa fa-trash bigger-130"></i></a>'
                                            }]
                                        });
                                    }
                                }
                            }


                            var textbox = '<input type="text" class="setwidth-textbox">';
                            var checkbox = '<input type="checkbox">';


                            $('#cbxForMemo').empty().trigger('chosen:updated');

                            var cbxMemoData = '<option id="" value="">-- กรุณาเลือก ใบบันทึกขอเบิก --</option>';
                            var docno = '';
                            var lstRef = $('#RefDocID').val();
                            if (lstRef != null) {
                                for (var i = 0; i < lstRef.length; i++) {
                                    docno += lstRef[i];
                                    cbxMemoData += '<option id="' + lstRef[i] + '">' + lstRef[i] + '</option>';
                                }
                            }

                            $('#cbxForMemo').append(cbxMemoData).trigger('chosen:updated');
                            //if (m_add) {
                            //    $('#mktTable').dataTable().fnClearTable();
                            //}


                        }
                        else {

                            var t = $('#table-product').DataTable({
                                "aoColumns": [
                                    { 'visible': false },
                                    null,
                                    { 'visible': false },
                                    null,
                                    null,
                                    null,
                                    null,
                                    null,
                                    null
                                ]
                            });
                            $('#table-product').removeClass('hide'); $('.cls-promo').removeClass('hide'); $('.cls-srh').removeClass('hide');
                        }
                    }

                } else {

                    if (wizard.currentStep == 2) {
                        wizard.currentStep -= 1;
                    } else {
                        wizard.currentStep = 0;
                    }

                    wizard.setState();
                    return;
                }
            } else if (x == 2) {
                messagefn.confirm2('คุณต้องการ บันทึก เอกสารนี้หรือไม่ ?', 6);
            } else if (x == 8) {
                $('#edit').removeClass('hide');
                $('#save').addClass('hide');
                setDefaultDialog(false);
            } else if (x == 4) {
                messagefn.confirm2('คุณต้องการ อนุมัติ เอกสารนี้หรือไม่ ?', 2);
            } else if (x == 6) {
                messagefn.confirm2('คุณต้องการ รับงาน เอกสารนี้หรือไม่ ?', 3);
            } else if (x == 5) {

                bootbox.prompt({
                    title: '<div class="center">กรุณาระบุเหตุผล</div>',
                    inputType: 'textarea',
                    buttons: {
                        confirm: {
                            label: 'Reject',
                            className: 'btn-danger'
                        },
                        cancel: {
                            label: 'No',
                            className: 'btn-info'
                        }
                    },
                    callback: function (result) {
                        if (result != null) {
                            rejectDataRequisition($('#a_DocID').val(), result);
                        }
                    }
                });

                SaveData(x);
            } else if (x == 0) {
                $('#accept').removeClass('hide').addClass('hide');
                $('#edit').removeClass('hide').addClass('hide');
                $('#sent').removeClass('hide').addClass('hide');
                $('#save').removeClass('hide');
                return;
            } else if (x == 7) {
                messagefn.confirm2('คุณต้องการ อนุมัติ เอกสารนี้หรือไม่ ?', 7);
            } else if (x == 3) {
                messagefn.confirm2('คุณต้องการ บันทึก เอกสารนี้หรือไม่ ?', 1);
            } else {
                setDefaultDialog(false);
            }
        }

        var messagefn = {
            confirm1: function (text) {
                bootbox.confirm('<span style="text-align: center; color: red;"><h2>' + text + '</h2></span>', function (res) {
                    if (res) {
                        return res;
                    }
                });
            },
            confirm2: function (text, func) {
                bootbox.confirm('<span style="text-align: center; color: red;"><h2>' + text + '</h2></span>', function (res) {
                    if (res) {
                        SaveData(func);
                        setDefaultDialog();
                        $('#myModal').modal('hide');
                    }
                });
            },
            confirm3: function (text, func, msg) {
                bootbox.confirm('<span style="text-align: center; color: red;"><h2>' + text + '</h2></span>', function (res) {
                    if (res) {
                        SaveData(func);
                        setDefaultDialog();

                        bootbox.dialog({
                            message: msg,
                            buttons: {
                                "information": {
                                    "label": "OK",
                                    "className": "btn-sm btn-info"
                                }
                            }
                        });
                    }
                });
            },
            infomation1: function (str) {
                bootbox.dialog({
                    message: str,
                    buttons: {
                        "warning": {
                            "label": "OK",
                            "className": "btn-sm btn-warning"
                        }
                    }
                });
            },
            infomation2: function (str, type, label, className) {
                bootbox.dialog({
                    message: str,
                    buttons: {
                        type: {
                            "label": label,
                            "className": className
                        }
                    }
                });
            },

            openRecall: function (text, rqrno) {
                bootbox.confirm('<span style="text-align: center; color: red;"><h2>' + text + '</h2></span>', function (res) {
                    if (res) {
                        Recall(0, rqrno);
                    }
                });
            },
            confirmRecall: function (text, rqrno) {
                bootbox.confirm('<span style="text-align: center; color: red;"><h2>' + text + '</h2></span>', function (res) {
                    if (res) {
                        Recall(1, rqrno);
                    }
                });
            },
        };

        function setDefaultDialog(isClose) {


            $('#reject').removeClass('hide').addClass('hide');
            $('#accept').removeClass('hide').addClass('hide');
            $('#approve').removeClass('hide').addClass('hide');
            $('#open').removeClass('hide').addClass('hide');
            $('#edit').removeClass('hide').addClass('hide');
            $('#sent').removeClass('hide').addClass('hide');
            $('#save').removeClass('hide');



            var wizard = $('#fuelux-wizard-container').data('fu.wizard')
            wizard.currentStep = 1;
            wizard.setState();
            //typeof isClose === 'undefined'
            //isClose = (typeof isClose === 'undefined' ? false : true);
            if ((typeof isClose === 'undefined' ? true : false)) {
                $('#dialog-message').closest('.ui-dialog-content').dialog('close');
            }

        }

        function CallDatatable() {
            var oTable1 = $("#dynamic-table").dataTable({
                sdom: 'Bfrtip',
                select: true,
                bAutoWidth: false,
                "aoColumns": [
                    null, null, null, null, null,
                    { "bSortable": false }
                ],
                "aaSorting": []
            });


            var colvis = new $.fn.dataTable.ColVis(oTable1, {
                "buttonText": "<i class='fa fa-search'></i>",
                "aiExclude": [0, 3],
                "bShowAll": true,
                "sAlign": "right",
                "fnLabel": function (i, title, th) {
                    return $(th).text();
                }
            });

            $(colvis.button()).addClass('btn-group').find('button').addClass('btn btn-white btn-info btn-bold')

            $(colvis.button())
                .prependTo('.tableTools-container .btn-group')
                .attr('title', 'Show/hide columns').tooltip({ container: 'body' });

            $(colvis.dom.collection)
                .addClass('dropdown-menu dropdown-light dropdown-caret dropdown-caret-right')
                .find('li').wrapInner('<a href="javascript:void(0)" />')
                .find('input[type=checkbox]').addClass('ace').next().addClass('lbl padding-8');

            $('th input[type=checkbox], td input[type=checkbox]').prop('checked', false);

            $('#dynamic-table > thead > tr > th input[type=checkbox]').eq(0).on('click', function () {
                var th_checked = this.checked;

                $(this).closest('table').find('tbody > tr').each(function () {
                    var row = this;
                    if (th_checked) tableTools_obj.fnSelect(row);
                    else tableTools_obj.fnDeselect(row);
                });
            });

            $('#dynamic-table').on('click', 'td input[type=checkbox]', function () {
                var row = $(this).closest('tr').get(0);
                if (!this.checked) tableTools_obj.fnSelect(row);
                else tableTools_obj.fnDeselect($(this).closest('tr').get(0));
            });

            $(document).on('click', '#dynamic-table .dropdown-toggle', function (e) {
                e.stopImmediatePropagation();
                e.stopPropagation();
                e.preventDefault();
            });
        }

        var aoCol = [
            { "bSortable": false, "visible": false }, null,
            { "bSortable": false, "visible": false },
            null, null, null, null, null,
            { "bSortable": false }
        ];

        function CallTableProduct() {
            var oTable1 = $("#table-product").dataTable({
                bAutoWidth: false,
                "aoColumns": aoCol,
                "aaSorting": [],
            });

            //ColVis extension
            var colvis = new $.fn.dataTable.ColVis(oTable1, {
                "buttonText": "<i class='fa fa-search'></i>",
                "aiExclude": [0, 3],
                "bShowAll": true,
                "sAlign": "right",
                "fnLabel": function (i, title, th) {
                    return $(th).text();
                }
            });

            $(colvis.button()).addClass('btn-group').find('button').addClass('btn btn-white btn-info btn-bold')

            $(colvis.button())
                .prependTo('.tableTools-container .btn-group')
                .attr('title', 'Show/hide columns').tooltip({ container: 'body' });

            $(colvis.dom.collection)
                .addClass('dropdown-menu dropdown-light dropdown-caret dropdown-caret-right')
                .find('li').wrapInner('<a href="javascript:void(0)" />')
                .find('input[type=checkbox]').addClass('ace').next().addClass('lbl padding-8');

            $('th input[type=checkbox], td input[type=checkbox]').prop('checked', false);

            $('#table-product > thead > tr > th input[type=checkbox]').eq(0).on('click', function () {
                var th_checked = this.checked;

                $(this).closest('table').find('tbody > tr').each(function () {
                    var row = this;
                    if (th_checked) tableTools_obj.fnSelect(row);
                    else tableTools_obj.fnDeselect(row);
                });
            });

            $('#table-product').on('click', 'td input[type=checkbox]', function () {
                var row = $(this).closest('tr').get(0);
                if (!this.checked) tableTools_obj.fnSelect(row);
                else tableTools_obj.fnDeselect($(this).closest('tr').get(0));
            });

            $(document).on('click', '#table-product .dropdown-toggle', function (e) {
                e.stopImmediatePropagation();
                e.stopPropagation();
                e.preventDefault();
            });

        }

        function CallATProduct() {
            var oTable1 = $("#a_t_product").dataTable({
                bAutoWidth: false,
                "aoColumns": aoCol,
                "aaSorting": [],
            });

            //ColVis extension
            var colvis = new $.fn.dataTable.ColVis(oTable1, {
                "buttonText": "<i class='fa fa-search'></i>",
                "aiExclude": [0, 3],
                "bShowAll": true,
                "sAlign": "right",
                "fnLabel": function (i, title, th) {
                    return $(th).text();
                }
            });

            $(colvis.button()).addClass('btn-group').find('button').addClass('btn btn-white btn-info btn-bold')

            $(colvis.button())
                .prependTo('.tableTools-container .btn-group')
                .attr('title', 'Show/hide columns').tooltip({ container: 'body' });

            $(colvis.dom.collection)
                .addClass('dropdown-menu dropdown-light dropdown-caret dropdown-caret-right')
                .find('li').wrapInner('<a href="javascript:void(0)" />')
                .find('input[type=checkbox]').addClass('ace').next().addClass('lbl padding-8');

            $('th input[type=checkbox], td input[type=checkbox]').prop('checked', false);

            $('#a_t_product > thead > tr > th input[type=checkbox]').eq(0).on('click', function () {
                var th_checked = this.checked;

                $(this).closest('table').find('tbody > tr').each(function () {
                    var row = this;
                    if (th_checked) tableTools_obj.fnSelect(row);
                    else tableTools_obj.fnDeselect(row);
                });
            });

            $('#a_t_product').on('click', 'td input[type=checkbox]', function () {
                var row = $(this).closest('tr').get(0);
                if (!this.checked) tableTools_obj.fnSelect(row);
                else tableTools_obj.fnDeselect($(this).closest('tr').get(0));
            });

            $(document).on('click', '#a_t_product .dropdown-toggle', function (e) {
                e.stopImmediatePropagation();
                e.stopPropagation();
                e.preventDefault();
            });

        }

        function rejectDataRequisition(rqrno, reason) {

            $.ajax({
                type: "POST",
                url: "RequisitionRequest.aspx/RejectDataRequisition",
                data: JSON.stringify({ rqrno: rqrno, reason: reason }),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                        textStatus + "\n\nError: " + errorThrown);
                },
                success: function (result) {
                    ClearData('#dynamic-table');
                    try {

                        $.each(result, function (k, Val) {

                            if (Val.MsgErr != '') {
                                bootbox.alert('<h4><center>' + Val.MsgErr + '</center></h4>');
                            }

                            $.each(Val.lstrr, function (key, Value) {
                                var data = RenderDatatable(Value);
                                $('#iBody').append(data);
                            });
                        });

                    } catch (e) {

                    }
                    CallDatatable();
                }
            });
            setDefaultDialog();
        }

        function deleteDataRequisition(rqrno) {
            bootbox.confirm('<span style="text-align: center;"><h2>คุณต้องการ ลบเอกสารนี้หรือไม่ ?</h2></span>', function (res) {
                if (res) {
                    $.ajax({
                        type: "POST",
                        url: "RequisitionRequest.aspx/DeleteDataRequisition",
                        data: JSON.stringify({ rqrno: rqrno }),
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                                textStatus + "\n\nError: " + errorThrown);
                        },
                        success: function (result) {
                            ClearData('#dynamic-table');
                            try {

                                $.each(result, function (k, Val) {

                                    if (Val.MsgErr != '') {
                                        bootbox.alert('<h4><center>' + Val.MsgErr + '</center></h4>');
                                    }

                                    $.each(Val.lstrr, function (key, Value) {
                                        var data = RenderDatatable(Value);
                                        $('#iBody').append(data);
                                    });
                                });

                            } catch (e) {

                            }
                            CallDatatable();
                        }
                    });
                }
            });
        }

        function SetButton(status, reqid) {
            if (Permission.Admin) {
                if (status == 0) {
                    $('#a_btnClose').removeClass('hide');
                    $('#a_Reject').removeClass('hide').addClass('hide');
                    $('#a_Approve').removeClass('hide').addClass('hide');
                    $('#a_Accept').removeClass('hide').addClass('hide');
                    return (reqid == Permission.UserDetails[0].EmpCode ? false : true);
                } else if (status == 1) {
                    $('#a_btnClose').removeClass('hide');
                    $('#a_Reject').removeClass('hide').addClass('hide');
                    $('#a_Approve').removeClass('hide').addClass('hide');
                    $('#a_Accept').removeClass('hide').addClass('hide');
                    return true;
                } else if (status == 2) {
                    $('#a_btnClose').removeClass('hide');
                    $('#a_Reject').removeClass('hide');
                    $('#a_Approve').removeClass('hide').addClass('hide');
                    $('#a_Accept').removeClass('hide');
                    return true;
                } else if (status == 3) {
                    $('#a_btnClose').removeClass('hide');
                    $('#a_Reject').removeClass('hide').addClass('hide');
                    $('#a_Approve').removeClass('hide').addClass('hide');
                    $('#a_Accept').removeClass('hide').addClass('hide');
                    return true;
                } else if (status == 4) {
                    $('#a_btnClose').removeClass('hide');
                    $('#a_Reject').removeClass('hide').addClass('hide');
                    $('#a_Approve').removeClass('hide').addClass('hide');
                    $('#a_Accept').removeClass('hide').addClass('hide');
                    return true;
                } else if (status == 5) {
                    $('#a_btnClose').removeClass('hide');
                    $('#a_Reject').removeClass('hide').addClass('hide');
                    $('#a_Approve').removeClass('hide').addClass('hide');
                    $('#a_Accept').removeClass('hide').addClass('hide');
                    return true;
                }
            } else if (Permission.CLevel || Permission.HeadOf) {
                if (status == 0) {
                    $('#a_btnClose').removeClass('hide');
                    $('#a_Reject').removeClass('hide').addClass('hide');
                    $('#a_Approve').removeClass('hide').addClass('hide');
                    $('#a_Accept').removeClass('hide').addClass('hide');
                    return (reqid == Permission.UserDetails[0].EmpCode ? false : true);
                } else if (status == 1) {
                    $('#a_btnClose').removeClass('hide');
                    $('#a_Reject').removeClass('hide');
                    $('#a_Approve').removeClass('hide');
                    $('#a_Accept').removeClass('hide').addClass('hide');
                    return true;
                } else if (status == 2) {
                    $('#a_btnClose').removeClass('hide');
                    $('#a_Reject').removeClass('hide').addClass('hide');
                    $('#a_Approve').removeClass('hide').addClass('hide');
                    $('#a_Accept').removeClass('hide').addClass('hide');
                    return true;
                } else if (status == 3) {
                    $('#a_btnClose').removeClass('hide');
                    $('#a_Reject').removeClass('hide').addClass('hide');
                    $('#a_Approve').removeClass('hide').addClass('hide');
                    $('#a_Accept').removeClass('hide').addClass('hide');
                    return true;
                } else if (status == 4) {
                    $('#a_btnClose').removeClass('hide');
                    $('#a_Reject').removeClass('hide').addClass('hide');
                    $('#a_Approve').removeClass('hide').addClass('hide');
                    $('#a_Accept').removeClass('hide').addClass('hide');
                    return true;
                } else if (status == 5) {
                    $('#a_btnClose').removeClass('hide');
                    $('#a_Reject').removeClass('hide').addClass('hide');
                    $('#a_Approve').removeClass('hide').addClass('hide');
                    $('#a_Accept').removeClass('hide').addClass('hide');
                    return true;
                }
            } else if (Permission.LCM) {
                if (status == 0) {
                    $('#a_btnClose').removeClass('hide');
                    $('#a_Reject').removeClass('hide').addClass('hide');
                    $('#a_Approve').removeClass('hide').addClass('hide');
                    $('#a_Accept').removeClass('hide').addClass('hide');
                    return (reqid == Permission.UserDetails[0].EmpCode ? false : true);
                } else if (status == 1) {
                    $('#a_btnClose').removeClass('hide');
                    $('#a_Reject').removeClass('hide').addClass('hide');
                    $('#a_Approve').removeClass('hide').addClass('hide');
                    $('#a_Accept').removeClass('hide').addClass('hide');
                    return true;
                } else if (status == 2) {
                    $('#a_btnClose').removeClass('hide');
                    $('#a_Reject').removeClass('hide').addClass('hide');
                    $('#a_Approve').removeClass('hide').addClass('hide');
                    $('#a_Accept').removeClass('hide').addClass('hide');
                    return true;
                } else if (status == 3) {
                    $('#a_btnClose').removeClass('hide');
                    $('#a_Reject').removeClass('hide').addClass('hide');
                    $('#a_Approve').removeClass('hide').addClass('hide');
                    $('#a_Accept').removeClass('hide').addClass('hide');
                    return true;
                } else if (status == 4) {
                    $('#a_btnClose').removeClass('hide');
                    $('#a_Reject').removeClass('hide').addClass('hide');
                    $('#a_Approve').removeClass('hide').addClass('hide');
                    $('#a_Accept').removeClass('hide').addClass('hide');
                    return true;
                } else if (status == 5) {
                    $('#a_btnClose').removeClass('hide');
                    $('#a_Reject').removeClass('hide').addClass('hide');
                    $('#a_Approve').removeClass('hide').addClass('hide');
                    $('#a_Accept').removeClass('hide').addClass('hide');
                    return true;
                }
            } else if (Permission.HR) {
                if (status == 0) {
                    $('#a_btnClose').removeClass('hide');
                    $('#a_Reject').removeClass('hide').addClass('hide');
                    $('#a_Approve').removeClass('hide').addClass('hide');
                    $('#a_Accept').removeClass('hide').addClass('hide');
                    return (reqid == Permission.UserDetails[0].EmpCode ? false : true);
                } else if (status == 1) {
                    $('#a_btnClose').removeClass('hide');
                    $('#a_Reject').removeClass('hide').addClass('hide');
                    $('#a_Approve').removeClass('hide').addClass('hide');
                    $('#a_Accept').removeClass('hide').addClass('hide');
                    return true;
                } else if (status == 2) {
                    $('#a_btnClose').removeClass('hide');
                    $('#a_Reject').removeClass('hide').addClass('hide');
                    $('#a_Approve').removeClass('hide').addClass('hide');
                    $('#a_Accept').removeClass('hide').addClass('hide');
                    return true;
                } else if (status == 3) {
                    $('#a_btnClose').removeClass('hide');
                    $('#a_Reject').removeClass('hide').addClass('hide');
                    $('#a_Approve').removeClass('hide').addClass('hide');
                    $('#a_Accept').removeClass('hide').addClass('hide');
                    return true;
                } else if (status == 4) {
                    $('#a_btnClose').removeClass('hide');
                    $('#a_Reject').removeClass('hide').addClass('hide');
                    $('#a_Approve').removeClass('hide').addClass('hide');
                    $('#a_Accept').removeClass('hide').addClass('hide');
                    return true;
                } else if (status == 5) {
                    $('#a_btnClose').removeClass('hide');
                    $('#a_Reject').removeClass('hide').addClass('hide');
                    $('#a_Approve').removeClass('hide').addClass('hide');
                    $('#a_Accept').removeClass('hide').addClass('hide');
                    return true;
                }
            } else if (Permission.ETC) {
                if (status == 0) {
                    $('#a_btnClose').removeClass('hide');
                    $('#a_Reject').removeClass('hide').addClass('hide');
                    $('#a_Approve').removeClass('hide').addClass('hide');
                    $('#a_Accept').removeClass('hide').addClass('hide');
                    return (reqid == Permission.UserDetails[0].EmpCode ? false : true);
                } else if (status == 1) {
                    $('#a_btnClose').removeClass('hide');
                    $('#a_Reject').removeClass('hide').addClass('hide');
                    $('#a_Approve').removeClass('hide').addClass('hide');
                    $('#a_Accept').removeClass('hide').addClass('hide');
                    return true;
                } else if (status == 2) {
                    $('#a_btnClose').removeClass('hide');
                    $('#a_Reject').removeClass('hide').addClass('hide');
                    $('#a_Approve').removeClass('hide').addClass('hide');
                    $('#a_Accept').removeClass('hide').addClass('hide');
                    return true;
                } else if (status == 3) {
                    $('#a_btnClose').removeClass('hide');
                    $('#a_Reject').removeClass('hide').addClass('hide');
                    $('#a_Approve').removeClass('hide').addClass('hide');
                    $('#a_Accept').removeClass('hide').addClass('hide');
                    return true;
                } else if (status == 4) {
                    $('#a_btnClose').removeClass('hide');
                    $('#a_Reject').removeClass('hide').addClass('hide');
                    $('#a_Approve').removeClass('hide').addClass('hide');
                    $('#a_Accept').removeClass('hide').addClass('hide');
                    return true;
                } else if (status == 5) {
                    $('#a_btnClose').removeClass('hide');
                    $('#a_Reject').removeClass('hide').addClass('hide');
                    $('#a_Approve').removeClass('hide').addClass('hide');
                    $('#a_Accept').removeClass('hide').addClass('hide');
                    return true;
                }
            }
            else if (Permission.MKT) {
                if (status == 0) {

                } else if (status == 1) {

                } else if (status == 2) {
                    $('#a_btnClose').removeClass('hide');
                    $('#a_Reject').removeClass('hide').addClass('hide');
                    $('#a_Approve').removeClass('hide').addClass('hide');
                    $('#a_Accept').removeClass('hide').addClass('hide');
                    return true;
                } else if (status == 3) {
                    $('#a_btnClose').removeClass('hide');
                    $('#a_Reject').removeClass('hide').addClass('hide');
                    $('#a_Approve').removeClass('hide').addClass('hide');
                    $('#a_Accept').removeClass('hide').addClass('hide');
                    return true;
                } else if (status == 4) {
                    $('#a_btnClose').removeClass('hide');
                    $('#a_Reject').removeClass('hide').addClass('hide');
                    $('#a_Approve').removeClass('hide').addClass('hide');
                    $('#a_Accept').removeClass('hide').addClass('hide');
                    return true;
                } else if (status == 5) {
                    $('#a_btnClose').removeClass('hide');
                    $('#a_Reject').removeClass('hide').addClass('hide');
                    $('#a_Approve').removeClass('hide').addClass('hide');
                    $('#a_Accept').removeClass('hide').addClass('hide');
                    return true;
                }
                return false;
            }
        }

        function editDataRequisition(rqrno) {
            if ($('#status_' + rqrno + '').text().trim() === "เตรียมส่ง") {
                m_edit = true;
                m_add = false;
                m_view = false;
                m_approve = false;
                m_accept = false;
                m_reject = false;

                $('#sent').addClass('hide');
            }
            else if ($('#status_' + rqrno + '').text().trim() === "รออนุมัติ") {
                m_edit = false;
                m_add = false;
                m_view = false;
                m_approve = true;
                m_accept = false;
                m_reject = true;

                $('#a_btnClose').removeClass('hide');
                $('#a_Reject').removeClass('hide');
                $('#a_Approve').removeClass('hide');
                $('#a_Accept').removeClass('hide').addClass('hide');
            }
            else if ($('#status_' + rqrno + '').text().trim() === "อนุมัติ") {
                m_edit = false;
                m_add = false;
                m_view = false;
                m_approve = false;
                m_accept = true;
                m_reject = false;

                $('#a_btnClose').removeClass('hide');
                $('#a_Reject').removeClass('hide').addClass('hide');
                $('#a_Approve').removeClass('hide').addClass('hide');
                $('#a_Accept').removeClass('hide').addClass('hide');
            }

            SetComboBox();

            $.ajax({
                type: "POST",
                url: "RequisitionRequest.aspx/EditDataRequisition",
                data: JSON.stringify({ rqrno: rqrno }),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                async: false,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                        textStatus + "\n\nError: " + errorThrown);
                },
                success: function (result) {
                    try {

                        $.each(result, function (k, Val) {
                            $.each(Val, function (key, Value) {

                                var isShow = SetButton(Value.STATUS, Value.REQID);

                                if (isShow) {
                                    $('#a_DocID').val(Value.RQRNO);
                                    $('#RefDocID').empty().trigger('chosen:updated');
                                    $('#a_RefDocID').empty().trigger('chosen:updated');
                                    $.each(Value.DRNO, function (key, data) {
                                        var splitData = data.DRNO.split(',');
                                        for (var i = 0; i < splitData.length; i++) {
                                            var select = $('#a_RefDocID');
                                            var option = $("<option>").val(splitData[i]).text(splitData[i]);
                                            select.prepend(option);
                                            select.find(option).prop('selected', true);
                                            select.trigger("chosen:updated");
                                        }
                                    });

                                    $('#a_recieveDocDatePick').val(Value.RCVD);
                                    $('#a_bookDocDatePick').val(Value.CURD);

                                    $('#a_cbxAddUser').val(Value.REQID);
                                    $('#a_cbxAddUser option:selected').text(Value.REQID + ' : ' + Value.REQN);
                                    $('#a_cbxAddUser').trigger("chosen:updated");

                                    $('#a_cbxAddApprover').val(Value.APPVID);
                                    $('#a_cbxAddApprover option:selected').text(Value.APPVID + ' : ' + Value.APPVN);
                                    $('#a_cbxAddApprover').trigger("chosen:updated");

                                    if ((Permission.Admin || Permission.CLevel || Permission.HO) && !m_edit) {
                                        $('#id-StepOne').addClass('disabledClass');
                                        $('#id-StepTwo').addClass('disabledClass');
                                    }
                                    else {
                                        $('#id-StepOne').removeClass('disabledClass');
                                        $('#id-StepTwo').removeClass('disabledClass');
                                    }

                                    var ss = '00' + Value.PROMOTID.toString();
                                    //$('#a_cbxAddPromotionType').addClass('disabledClass');
                                    $('#a_cbxAddPromotionType').val('');
                                    $('#a_cbxAddPromotionType').trigger("chosen:updated");
                                    $('#a_cbxAddPromotionType').val(ss);
                                    $('#a_cbxAddPromotionType option:selected').text(ss + ' : ' + Value.PROMOTN);
                                    $('#a_cbxAddPromotionType').trigger("chosen:updated");

                                    $('#a_cbxAddCostCenter').val(Value.DEPTID);
                                    $('#a_cbxAddCostCenter option:selected').text(Value.DEPTID + ' : ' + Value.DEPTN);
                                    $('#a_cbxAddCostCenter').trigger("chosen:updated");

                                    $('#a_cbxAddGLNo').val('');
                                    $('#a_cbxAddGLNo').trigger("chosen:updated");
                                    $('#a_cbxAddGLNo').val(Value.GLID);
                                    $('#a_cbxAddGLNo option:selected').text(Value.GLN);
                                    $('#a_cbxAddGLNo').trigger("chosen:updated");

                                    $('#a_cbxAddObjective').val('');
                                    $('#a_cbxAddObjective').trigger("chosen:updated");
                                    $('#a_cbxAddObjective').val(Value.OBJID);
                                    $('#a_cbxAddObjective option:selected').text(Value.OBJN);
                                    $('#a_cbxAddObjective').trigger("chosen:updated");

                                    $('#a_Remark').val(Value.REMK);
                                    if (Permission.Admin && (Value.DRNO[0].DRNO != "")) {
                                        $('#a_mktTable').removeClass('hide');
                                        if ($.fn.dataTable.isDataTable('#a_mktTable')) {
                                            table = $('#a_mktTable').DataTable({
                                                retrieve: true,
                                                paging: false
                                            });
                                            table.destroy();
                                        }

                                        var table1 = $('#a_mktTable').dataTable({

                                            "bInfo": false,
                                            "bFilter": false,
                                            "bDeferRender": false,

                                            "iDisplayLength": -1,
                                            "aoColumns": [
                                                { 'visible': false },
                                                { 'visible': false },
                                                { "sClass": "center setwidthproject-insidetable", 'bSortable': false },
                                                { "sClass": "center setwidth-insidetable", 'bSortable': false },
                                                { 'visible': false },
                                                { "sClass": "align-right setwidthstock-insidetable", 'bSortable': false },
                                                { "sClass": "align-right setwidth-insidetable", 'bSortable': false },
                                                { "sClass": "center", 'bSortable': false },
                                                { "sClass": "center", 'bSortable': false },
                                                { "sClass": "center", 'bSortable': false }
                                            ],
                                            'paging': false,
                                            "fnCreatedRow": function (nRow, aData, iDataIndex) {
                                                $(nRow).attr('id', aData[0]);
                                                var txtBox = $(nRow).find("input[type=text]");
                                                txtBox.attr('id', 'text-' + aData[0]);
                                            },
                                            "columnDefs": [{
                                                "targets": -1,
                                                "data": null,
                                                "defaultContent": '<a href="" class="btn btn-sm btn-white btn-danger btn-round btnMemoDataDel" onclick="return false;">' +
                                                    '<i class="ace-icon fa fa-trash bigger-130"></i></a>'
                                            }]
                                        });
                                        $('#a_t_product').addClass('hide');
                                        if ($.fn.dataTable.isDataTable('#a_t_product')) {
                                            table = $('#a_t_product').DataTable({
                                                retrieve: true,
                                                paging: false
                                            });
                                            table.destroy();
                                        }
                                        $('#table-product').addClass('hide'); $('.cls-promo').addClass('hide'); $('.cls-srh').addClass('hide');

                                        $('#a_mktTable').dataTable().fnClearTable();
                                        var a_table1 = $('#a_mktTable').dataTable();
                                        $.each(Value.dataDetails, function (key, data) {

                                            var textbox = '<input type="text" value="' + data.QUANT + '">'

                                            a_table1.fnAddData([
                                                data.ITEMS,
                                                data.PROJID,
                                                data.PROJN,
                                                data.DRNOBYITEM,
                                                data.PROPID,
                                                data.PROPN,
                                                data.MEMOBALANCE,
                                                textbox,
                                                data.TYPE,
                                                '',
                                                ''
                                            ]);

                                        });
                                        $('#mktTable').dataTable();
                                    }
                                    else //if (Permission.MKT) {
                                    {
                                        $('#a_mktTable').removeClass('hide');
                                        if ($.fn.dataTable.isDataTable('#a_mktTable')) {
                                            table = $('#a_mktTable').DataTable({
                                                retrieve: true,
                                                paging: false
                                            });
                                            table.destroy();
                                        }

                                        var table1 = $('#a_mktTable').dataTable({

                                            "bInfo": false,
                                            "bFilter": false,
                                            "bDeferRender": false,

                                            "iDisplayLength": -1,
                                            "aoColumns": [
                                                { 'visible': false },
                                                { 'visible': false },
                                                { "sClass": "center setwidthproject-insidetable", 'bSortable': false },
                                                { "sClass": "center setwidth-insidetable", 'bSortable': false },
                                                { 'visible': false },
                                                { "sClass": "align-right setwidthstock-insidetable", 'bSortable': false },
                                                { "sClass": "align-right setwidth-insidetable", 'bSortable': false },
                                                { "sClass": "center", 'bSortable': false },
                                                { "sClass": "center", 'bSortable': false },
                                                { "sClass": "center", 'bSortable': false }
                                            ],
                                            'paging': false,
                                            "fnCreatedRow": function (nRow, aData, iDataIndex) {
                                                $(nRow).attr('id', aData[0]);
                                                var txtBox = $(nRow).find("input[type=text]");
                                                txtBox.attr('id', 'text-' + aData[0]);
                                            },
                                            "columnDefs": [{
                                                "targets": -1,
                                                "data": null,
                                                "defaultContent": '<a href="" class="btn btn-sm btn-white btn-danger btn-round btnMemoDataDel" onclick="return false;">' +
                                                    '<i class="ace-icon fa fa-trash bigger-130"></i></a>'
                                            }]
                                        });
                                        $('#a_t_product').addClass('hide');
                                        if ($.fn.dataTable.isDataTable('#a_t_product')) {
                                            table = $('#a_t_product').DataTable({
                                                retrieve: true,
                                                paging: false
                                            });
                                            table.destroy();
                                        }
                                        $('#table-product').addClass('hide'); $('.cls-promo').addClass('hide'); $('.cls-srh').addClass('hide');

                                        $('#a_mktTable').dataTable().fnClearTable();
                                        var a_table1 = $('#a_mktTable').dataTable();
                                        $.each(Value.dataDetails, function (key, data) {

                                            var textbox = '<input type="text" value="' + data.QUANT + '">'

                                            a_table1.fnAddData([
                                                data.ITEMS,
                                                data.PROJID,
                                                data.PROJN,
                                                data.DRNOBYITEM,
                                                data.PROPID,
                                                data.PROPN,
                                                data.MEMOBALANCE,
                                                textbox,
                                                data.TYPE,
                                                '',
                                                ''
                                            ]);

                                        });
                                        $('#mktTable').dataTable();
                                    }

                                }
                                else {

                                    $('#newDocID').val(Value.RQRNO);
                                    $('#RefDocID').empty().trigger('chosen:updated');
                                    $('#a_RefDocID').empty().trigger('chosen:updated');
                                    $.each(Value.DRNO, function (key, data) {
                                        var splitData = data.DRNO.split(',');
                                        for (var i = 0; i < splitData.length; i++) {
                                            var select = $('#RefDocID');
                                            var option = $("<option>").val(splitData[i]).text(splitData[i]);
                                            select.prepend(option);
                                            select.find(option).prop('selected', true);
                                            select.trigger("chosen:updated");
                                        }
                                    });

                                    $('#RefDocID').trigger("chosen:updated");

                                    $('#recieveDocDatePick').val(Value.RCVD);
                                    $('#bookDocDatePick').val(Value.CURD);

                                    $('#cbxAddUser').val(Value.REQID);
                                    $('#cbxAddUser option:selected').text(Value.REQID + ' : ' + Value.REQN);
                                    $('#cbxAddUser').trigger("chosen:updated");

                                    $('#cbxAddPromotionType').addClass('disabledClass');

                                    if ((Permission.Admin || Permission.CLevel || Permission.HO) && !m_edit) {
                                        $('#id-StepOne').addClass('disabledClass');
                                        $('#id-StepTwo').addClass('disabledClass');
                                    }
                                    else {
                                        $('#id-StepOne').removeClass('disabledClass');
                                        $('#id-StepTwo').removeClass('disabledClass');
                                    }

                                    $('#cbxAddPromotionType').val('');
                                    $('#cbxAddPromotionType').trigger("chosen:updated");
                                    var ss = '00' + Value.PROMOTID.toString();
                                    $('#cbxAddPromotionType').val(ss);
                                    $('#cbxAddPromotionType option:selected').text(ss + ' : ' + Value.PROMOTN);
                                    $('#cbxAddPromotionType').trigger("chosen:updated");

                                    $('#cbxAddCostCenter').val(Value.DEPTID);
                                    $('#cbxAddCostCenter option:selected').text(Value.DEPTID + ' : ' + Value.DEPTN);
                                    $('#cbxAddCostCenter').trigger("chosen:updated");

                                    $('#cbxAddGLNo').val('');
                                    $('#cbxAddGLNo').trigger("chosen:updated");
                                    $('#cbxAddGLNo').val(Value.GLID);
                                    $('#cbxAddGLNo option:selected').text(Value.GLID + ' : ' + Value.GLN);
                                    $('#cbxAddGLNo').trigger("chosen:updated");

                                    $('#cbxAddObjective').val('');
                                    $('#cbxAddObjective').trigger("chosen:updated");
                                    $('#cbxAddObjective').val(Value.OBJID);
                                    $('#cbxAddObjective option:selected').text(Value.OBJN);
                                    $('#cbxAddObjective').trigger("chosen:updated");

                                    $('#txtRemark').val(Value.REMK);

                                    $('#cbxAddApprover').val(Value.APPVID);
                                    $('#cbxAddApprover option:selected').text(Value.APPVID + ' : ' + Value.APPVN);
                                    $('#cbxAddApprover').trigger("chosen:updated");

                                    $.each(Value.DRNO, function (key, data) {
                                        var splitData = data.DRNO.split(',');
                                        for (var i = 0; i < splitData.length; i++) {
                                            var select = $('#a_RefDocID');
                                            var option = $("<option>").val(splitData[i]).text(splitData[i]);
                                            select.prepend(option);
                                            select.find(option).prop('selected', true);
                                            select.trigger("chosen:updated");
                                        }
                                    });

                                    $('#a_RefDocID').trigger("chosen:updated");

                                    $('#recieveDocDatePick').val(Value.RCVD);
                                    $('#bookDocDatePick').val(Value.CURD);

                                    $('#a_cbxAddUser').val(Value.REQID);
                                    $('#a_cbxAddUser option:selected').text(Value.REQID + ' : ' + Value.REQN);
                                    $('#a_cbxAddUser').trigger("chosen:updated");

                                    $('#a_cbxAddPromotionType').addClass('disabledClass');

                                    if ((Permission.Admin || Permission.CLevel || Permission.HO) && !m_edit) {
                                        $('#id-StepOne').addClass('disabledClass');
                                        $('#id-StepTwo').addClass('disabledClass');
                                    }
                                    else {
                                        $('#id-StepOne').removeClass('disabledClass');
                                        $('#id-StepTwo').removeClass('disabledClass');
                                    }

                                    $('#a_cbxAddPromotionType').val('');
                                    $('#a_cbxAddPromotionType').trigger("chosen:updated");
                                    var ss = '00' + Value.PROMOTID.toString();
                                    $('#a_cbxAddPromotionType').val(ss);
                                    $('#a_cbxAddPromotionType option:selected').text(ss + ' : ' + Value.PROMOTN);
                                    $('#a_cbxAddPromotionType').trigger("chosen:updated");

                                    $('#a_cbxAddCostCenter').val(Value.DEPTID);
                                    $('#a_cbxAddCostCenter option:selected').text(Value.DEPTID + ' : ' + Value.DEPTN);
                                    $('#a_cbxAddCostCenter').trigger("chosen:updated");

                                    $('#a_cbxAddGLNo').val('');
                                    $('#a_cbxAddGLNo').trigger("chosen:updated");
                                    $('#a_cbxAddGLNo').val(Value.GLID);
                                    $('#a_cbxAddGLNo option:selected').text(Value.GLN);
                                    $('#a_cbxAddGLNo').trigger("chosen:updated");

                                    $('#a_cbxAddObjective').val('');
                                    $('#a_cbxAddObjective').trigger("chosen:updated");
                                    $('#a_cbxAddObjective').val(Value.OBJID);
                                    $('#a_cbxAddObjective option:selected').text(Value.OBJN);
                                    $('#a_cbxAddObjective').trigger("chosen:updated");

                                    $('#a_txtRemark').val(Value.REMK);

                                    $('#a_cbxAddApprover').val(Value.APPVID);
                                    $('#a_cbxAddApprover option:selected').text(Value.APPVID + ' : ' + Value.APPVN);
                                    $('#a_cbxAddApprover').trigger("chosen:updated");


                                    if (Permission.MKT) {
                                        $('#a_t_product').addClass('hide');
                                        $('#a_mktTable').removeClass('hide');
                                        $('#mktTable').removeClass('hide');
                                        if ($.fn.dataTable.isDataTable('#mktTable')) {
                                            table = $('#mktTable').DataTable({
                                                retrieve: true,
                                                paging: false
                                            });
                                            table.destroy();
                                        }
                                        if ($.fn.dataTable.isDataTable('#a_mktTable')) {
                                            table = $('#a_mktTable').DataTable({
                                                retrieve: true,
                                                paging: false
                                            });
                                            table.destroy();
                                        }
                                        var table1 = $('#mktTable').dataTable({

                                            "bInfo": false,
                                            "bFilter": false,
                                            "bDeferRender": false,

                                            "iDisplayLength": -1,
                                            "aoColumns": [
                                                { 'visible': false },
                                                { "sClass": "center setwidthproject-insidetable", 'bSortable': false },
                                                { "sClass": "center setwidthproject-insidetable", 'bSortable': false },
                                                { "sClass": "center setwidth-insidetable", 'bSortable': false },
                                                { 'visible': false },
                                                { "sClass": "align-right setwidthstock-insidetable", 'bSortable': false },
                                                { "sClass": "align-right setwidth-insidetable", 'bSortable': false },
                                                { "sClass": "center", 'bSortable': false },
                                                { "sClass": "center", 'bSortable': false },
                                                { "sClass": "center", 'bSortable': false }
                                            ],
                                            'paging': false,
                                            "fnCreatedRow": function (nRow, aData, iDataIndex) {
                                                $(nRow).attr('id', aData[0]);
                                                var txtBox = $(nRow).find("input[type=text]");
                                                txtBox.attr('id', 'text-' + aData[0]);
                                            },
                                            "columnDefs": [{
                                                "targets": -1,
                                                "data": null,
                                                "defaultContent": '<a href="" class="btn btn-sm btn-white btn-danger btn-round btnMemoDataDel" onclick="return false;">' +
                                                    '<i class="ace-icon fa fa-trash bigger-130"></i></a>'
                                            }]
                                        });
                                        $('#table-product').addClass('hide'); $('.cls-promo').addClass('hide'); $('.cls-srh').addClass('hide');

                                        $('#mktTable').dataTable().fnClearTable();
                                        var a_table1 = $('#mktTable').dataTable();
                                        $.each(Value.dataDetails, function (key, data) {

                                            var textbox = '<input type="text" class="text-limit" value="' + data.QUANT + '" disabled>'

                                            a_table1.fnAddData([
                                                data.ITEMS,
                                                data.PROJID,
                                                data.PROJN,
                                                data.DRNOBYITEM,
                                                data.PROPID,
                                                data.PROPN,
                                                data.MEMOBALANCE,
                                                textbox,
                                                data.TYPE,
                                                '',
                                                ''
                                            ]);

                                        });
                                        $('#mktTable').dataTable();

                                        var table1 = $('#a_mktTable').dataTable({

                                            "bInfo": false,
                                            "bFilter": false,
                                            "bDeferRender": false,

                                            "iDisplayLength": -1,
                                            "aoColumns": [
                                                { 'visible': false },
                                                { 'visible': false },
                                                { "sClass": "center setwidthproject-insidetable", 'bSortable': false },
                                                { "sClass": "center setwidth-insidetable", 'bSortable': false },
                                                { 'visible': false },
                                                { "sClass": "align-right setwidthstock-insidetable", 'bSortable': false },
                                                { "sClass": "align-right setwidth-insidetable", 'bSortable': false },
                                                { "sClass": "center", 'bSortable': false },
                                                { "sClass": "center", 'bSortable': false },
                                                { "sClass": "center", 'bSortable': false }
                                            ],
                                            'paging': false,
                                            "fnCreatedRow": function (nRow, aData, iDataIndex) {
                                                $(nRow).attr('id', aData[0]);
                                                var txtBox = $(nRow).find("input[type=text]");
                                                txtBox.attr('id', 'text-' + aData[0]);
                                            },
                                            "columnDefs": [{
                                                "targets": -1,
                                                "data": null,
                                                "defaultContent": '<a href="" class="btn btn-sm btn-white btn-danger btn-round btnMemoDataDel" onclick="return false;">' +
                                                    '<i class="ace-icon fa fa-trash bigger-130"></i></a>'
                                            }]
                                        });
                                        $('#table-product').addClass('hide'); $('.cls-promo').addClass('hide'); $('.cls-srh').addClass('hide');

                                        $('#a_mktTable').dataTable().fnClearTable();
                                        var a_table1 = $('#a_mktTable').dataTable();
                                        $.each(Value.dataDetails, function (key, data) {

                                            var textbox = '<input type="text" value="' + data.QUANT + '">'

                                            a_table1.fnAddData([
                                                data.ITEMS,
                                                data.PROJID,
                                                data.PROJN,
                                                data.DRNOBYITEM,
                                                data.PROPID,
                                                data.PROPN,
                                                data.MEMOBALANCE,
                                                textbox,
                                                data.TYPE,
                                                '',
                                                ''
                                            ]);

                                        });
                                        $('#a_mktTable').dataTable();
                                    }
                                    else {
                                        if ($.fn.dataTable.isDataTable('#table-product')) {
                                            table = $('#table-product').DataTable({
                                                retrieve: true,
                                                paging: false
                                            });
                                            table.destroy();
                                        }
                                        $('#table-product').removeClass('hide'); $('.cls-promo').removeClass('hide'); $('.cls-srh').removeClass('hide');
                                        $('#iProduct').empty();
                                        CallTableProduct();
                                        var t = $('#table-product').DataTable();
                                        $.each(Value.dataDetails, function (key, data) {
                                            t.row.add([
                                                data.PROJID,
                                                data.PROJN,
                                                data.PROPID,
                                                data.PROPN,
                                                data.QUANT,
                                                data.STDDT,
                                                data.ENDDT,
                                                data.RMDB,
                                                '<td>' +
                                                '<div class="action-buttons ' + ((Permission.CLevel || Permission.HeadOf || Permission.Admin) && !m_edit ? 'disabledbutton' : '') + ' ">' +
                                                '<a class="red" href="#" id="' + data.ITEMS + '" onclick="deleteUserGroup(' + data.ITEMS + ');"><i class="ace-icon fa fa-trash bigger-130"></i></a>' +
                                                '</div>' +
                                                '</td>'
                                            ]).draw(false);
                                        });
                                    }

                                    if ($('#status_' + rqrno + '').text().trim() === 'เตรียมส่ง') {
                                        var dialog = $("#dialog-message").removeClass('hide').dialog({
                                            width: screen.width / 100 * 80,
                                            modal: true,
                                            title: "<div class='widget-header widget-header-small'> " +
                                                "<h4 class='smaller'><i class='ace-icon glyphicon glyphicon-plus'></i> New Requisition Request</h4></div>",
                                            title_html: true,

                                        });

                                        if (!dialog.hasClass('hide')) {
                                            $('#newDocID').blur();
                                        }
                                    }

                                }
                            });
                        });

                        $('html, body').animate({ scrollTop: 0 }, 'fast');
                        $("#reminDay").focusin();

                    } catch (e) {

                        bootbox.alert('<h4><center>' + e.message + '</center></h4>');
                    }
                    SetFocus();
                }
            });

            var wizard = $('#fuelux-wizard-container').data('fu.wizard');
            wizard.currentStep = 1;
            wizard.setState();
            $('#reject').removeClass('hide').addClass('hide');
            $('#accept').removeClass('hide').addClass('hide');
            $('#approve').removeClass('hide').addClass('hide');
            $('#open').removeClass('hide').addClass('hide');
            $('#edit').removeClass('hide').addClass('hide');
            $('#save').removeClass('hide');
        }

        function viewDataRequisition(rqrno) {
            m_edit = false;
            m_add = false;
            m_view = true;

            if ($('#status_' + rqrno + '').text().trim() === "ปฏิเสธ") {
                $('#lblTextQ').text('คำขอถูกปฏิเสธ');
            } else if ($('#status_' + rqrno + '').text().trim() === "Accepted") {
                $('#lblTextQ').text('This request was successfully');
            } else {
                $('#lblTextQ').text('Improgress...');
            }

            $.ajax({
                type: "POST",
                url: "RequisitionRequest.aspx/EditDataRequisition",
                data: JSON.stringify({ rqrno: rqrno }),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                        textStatus + "\n\nError: " + errorThrown);
                },
                success: function (result) {
                    try {

                        $.each(result, function (k, Val) {
                            $.each(Val, function (key, Value) {

                                $('#a_DocID').val(Value.RQRNO);

                                $('#a_RefDocID').val('').trigger('chosen:updated');
                                $.each(Value.DRNO, function (key, data) {
                                    var splitData = data.DRNO.split(',');
                                    for (var i = 0; i < splitData.length; i++) {
                                        var select = $('#a_RefDocID');
                                        var option = $("<option>").val(splitData[i]).text(splitData[i]);
                                        select.prepend(option);
                                        select.find(option).prop('selected', true);
                                        select.trigger("chosen:updated");
                                    }
                                });

                                $('#a_recieveDocDatePick').val(Value.RCVD);
                                $('#a_bookDocDatePick').val(Value.CURD);

                                $('#a_cbxAddUser').val(Value.REQID);
                                $('#a_cbxAddUser option:selected').text(Value.REQID + ' : ' + Value.REQN);
                                $('#a_cbxAddUser').trigger("chosen:updated");

                                $('#a_cbxAddApprover').val(Value.APPVID);
                                $('#a_cbxAddApprover option:selected').text(Value.APPVID + ' : ' + Value.APPVN);
                                $('#a_cbxAddApprover').trigger("chosen:updated");

                                if ((Permission.Admin || Permission.CLevel || Permission.HO) && !m_edit) {
                                    $('#id-StepOne').addClass('disabledClass');
                                    $('#id-StepTwo').addClass('disabledClass');
                                }
                                else {
                                    $('#id-StepOne').removeClass('disabledClass');
                                    $('#id-StepTwo').removeClass('disabledClass');
                                }

                                var ss = '00' + Value.PROMOTID.toString();
                                //$('#a_cbxAddPromotionType').addClass('disabledClass');
                                $('#a_cbxAddPromotionType').val('');
                                $('#a_cbxAddPromotionType').trigger("chosen:updated");
                                $('#a_cbxAddPromotionType').val(ss);
                                $('#a_cbxAddPromotionType option:selected').text(ss + ' : ' + Value.PROMOTN);
                                $('#a_cbxAddPromotionType').trigger("chosen:updated");

                                $('#a_cbxAddCostCenter').val(Value.DEPTID);
                                $('#a_cbxAddCostCenter option:selected').text(Value.DEPTID + ' : ' + Value.DEPTN);
                                $('#a_cbxAddCostCenter').trigger("chosen:updated");

                                $('#a_cbxAddGLNo').val('');
                                $('#a_cbxAddGLNo').trigger("chosen:updated");
                                $('#a_cbxAddGLNo').val(Value.GLID);
                                $('#a_cbxAddGLNo option:selected').text(Value.GLN);
                                $('#a_cbxAddGLNo').trigger("chosen:updated");

                                $('#a_cbxAddObjective').val('');
                                $('#a_cbxAddObjective').trigger("chosen:updated");
                                $('#a_cbxAddObjective').val(Value.OBJID);
                                $('#a_cbxAddObjective option:selected').text(Value.OBJN);
                                $('#a_cbxAddObjective').trigger("chosen:updated");

                                $('#a_Remark').val(Value.REMK);

                                if ($.fn.dataTable.isDataTable('#a_t_product')) {
                                    table = $('#a_t_product').DataTable({
                                        retrieve: true,
                                        paging: false
                                    });
                                    table.destroy();
                                }
                                $('#a_Product').empty();
                                CallATProduct();
                                var t = $('#a_t_product').DataTable();
                                $.each(Value.dataDetails, function (key, data) {
                                    t.row.add([
                                        '<td class="hide">' + data.PROJID + '</td>',
                                        data.PROJN,
                                        '<td class="hide">' + data.PROPID + '</td>',
                                        data.PROPN,
                                        data.QUANT,
                                        data.STDDT,
                                        data.ENDDT,
                                        data.RMDB,
                                        '<td></td>'
                                    ]).draw(false);

                                });

                            });
                        });

                        $('html, body').animate({ scrollTop: 0 }, 'fast');
                        $("#reminDay").focusin();
                        var dialog = $("#dialog-message").removeClass('hide').dialog({
                            width: screen.width / 100 * 80,
                            modal: true,
                            title: "<div class='widget-header widget-header-small'> " +
                                "<h4 class='smaller'><i class='ace-icon glyphicon glyphicon-plus'></i> View Requisition Request</h4></div>",
                            title_html: true,
                        });

                    } catch (e) {

                    }

                    SetFocus();
                }
            });
            var wizard = $('#fuelux-wizard-container').data('fu.wizard');
            wizard.currentStep = 1;
            wizard.setState();
            $('#reject').removeClass('hide').addClass('hide');
            $('#accept').removeClass('hide').addClass('hide');
            $('#approve').removeClass('hide').addClass('hide');
            $('#open').removeClass('hide').addClass('hide');
            $('#edit').removeClass('hide').addClass('hide');
            $('#save').removeClass('hide');


        }

        function recallDataRequisition(rqrno) {
            if (Permission.Admin && ($('#status_' + rqrno + '').text().trim() == 'เรียกคืน')) {
                messagefn.confirmRecall("คุณต้องการ เรียกคืน เอกสารเลขที่ " + rqrno + " นี้หรือไม่ ?", rqrno);
                return;
            }
            messagefn.openRecall("คุณต้องการ เรียกคืน เอกสารเลขที่ " + rqrno + " นี้หรือไม่ ?", rqrno);
        }

        function SetFocus() {

            $('#focusText').focus();
        }

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row">
        <div class="col-sm-12">
            <div id="accordion" class="accordion-style1 panel-group">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h4 class="panel-title">
                            <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion" href="#collapseOne">
                                <i class="ace-icon fa fa-angle-down bigger-110" data-icon-hide="ace-icon fa fa-angle-down" data-icon-show="ace-icon fa fa-angle-right"></i>
                                เอกสารตั้งเบิก
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
                                            <label class="col-sm-5 control-label move-right" for="docSearchID">เลขที่เอกสารใบเบิก</label>
                                            <div class="col-sm-7">
                                                <input id="docSearchID" type="text" class="max-width" name="txtBox" value="" placeholder="เลขที่เอกสารใบเบิก" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-sm-5">
                                        <div class="form-group">
                                            <label class="col-sm-5 control-label move-right" for="tagSearchID">เลขที่เอกสารอ้างอิง</label>
                                            <div class="col-sm-7">
                                                <input id="tagSearchID" type="text" class="max-width" name="txtBox" value="" placeholder="เลขที่เอกสารอ้างอิง" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-sm-1"></div>
                                </div>
                                <div class="col-sm-12 margin-bottom-menu">
                                    <div class="col-sm-1"></div>

                                    <div class="col-sm-5">
                                        <div class="form-group">
                                            <label class="col-sm-5 control-label move-right" for="recvDate">วันที่รับเอกสารขอเบิก</label>
                                            <div class="col-sm-7">
                                                <span class="block input-icon input-icon-right" id="recvDate">
                                                    <input type="text" id="RecieveDatePicker1" class="max-width" placeholder="วันที่รับเอกสารขอเบิก" />
                                                    <i class="ace-icon fa fa-calendar"></i>
                                                </span>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-sm-5">
                                        <div class="form-group">
                                            <label class="col-sm-5 control-label move-right" for="bookDate">วันที่บันทึกใบขอเบิก</label>
                                            <div class="col-sm-7">
                                                <span class="block input-icon input-icon-right" id="bookDate">
                                                    <input type="text" id="BookDatePicker1" class="max-width" placeholder="วันที่บันทึกใบขอเบิก" />
                                                    <i class="ace-icon fa fa-calendar"></i>
                                                </span>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-sm-1"></div>
                                </div>
                                <div class="col-sm-12 margin-bottom-menu">
                                    <div class="col-sm-1"></div>

                                    <div class="col-sm-5">
                                        <div class="form-group">
                                            <label class="col-sm-3 control-label move-right" for="cbxUser">ผู้ขอเบิก</label>
                                            <div class="col-sm-9">
                                                <select class="chosen-select form-control max-width" id="cbxUser" data-placeholder="--- ผู้ขอเบิก ---">
                                                </select>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-sm-5">
                                        <div class="form-group">
                                            <label class="col-sm-3 control-label move-right" for="cbxPromotionType">โปรโมชั่น</label>
                                            <div class="col-sm-9">
                                                <select class="chosen-select form-control" id="cbxPromotionType" data-placeholder="--- โปรโมชั่น ---">
                                                </select>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-sm-1"></div>
                                </div>
                                <div class="col-sm-12 margin-bottom-menu">
                                    <div class="col-sm-1"></div>
                                    <div class="col-sm-5">
                                        <div class="form-group">
                                            <label class="col-sm-3 control-label move-right" for="cbxCostCenter">หน่วยงาน</label>
                                            <div class="col-sm-9">
                                                <select class="chosen-select form-control" id="cbxCostCenter" data-placeholder="--- หน่วยงาน ---">
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-5">
                                        <div class="form-group">
                                            <label class="col-sm-3 control-label move-right" for="cbxObjective">วิธีการแจก</label>
                                            <div class="col-sm-9">
                                                <select class="chosen-select form-control" id="cbxObjective" data-placeholder="--- วิธีการแจก ---">
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-1"></div>
                                </div>
                                <div class="col-sm-12 margin-bottom-menu">
                                    <div class="col-sm-1"></div>
                                    <div class="col-sm-5">
                                        <div class="form-group">
                                            <label class="col-sm-3 control-label move-right" for="cbxGLNo">วัตถุประสงค์</label>
                                            <div class="col-sm-9">
                                                <select class="chosen-select form-control" id="cbxGLNo" data-placeholder="--- วัตถุประสงค์ ---">
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-5"></div>
                                    <div class="col-sm-1"></div>
                                </div>
                                <div class="row" style="margin-top: 5px;">
                                    <div class="col-sm-12">
                                        <div class="col-sm-6 move-right">
                                            <button type="button" id="form-search" class="btn btn-sm btn-white btn-primary width-30 .form-search">
                                                <i class="ace-icon glyphicon glyphicon-search"></i>ค้นหา
                                            </button>
                                        </div>
                                        <div class="col-sm-6 move-left">
                                            <button type="button" id="clear-search" class="btn btn-sm btn-white btn-info">
                                                <i class="ace-icon glyphicon glyphicon-refresh"></i>Clear
                                            </button>
                                        </div>
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
        <div class="col-sm-12">
            <div class="widget-box" style="border: none">
                <div class="widget-body">
                    <div class="row">
                        <div class="col-sm-12 pull-left">
                            <button type="button" id="btnAdd" class="btn btn-white btn-info">
                                <i class="ace-icon glyphicon glyphicon-plus"></i>สร้างใหม่
                            </button>

                            <%--Delete Objective dialogbox--%>
                            <div id="dialog-delete" class="hide">
                                <div class="row">
                                    <div class="col-sm-12">
                                        <h3>คุณต้องการ ลบเหตุผลนี้หรือไม่?</h3>
                                    </div>
                                </div>
                            </div>

                            <%--Warning dialogbox--%>
                            <div id="dialog-warning" class="hide">
                                <div class="row">
                                    <div class="col-sm-12" style="text-align: center; color: orange">
                                        <h3><span id="textWarning"></span></h3>
                                    </div>
                                </div>
                            </div>
                            <%--------------------------------%>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12">
                            <table id="dynamic-table" class="table table-striped table-bordered table-hover no-margin-bottom no-border-top">
                                <thead>
                                    <tr>
                                        <th style="width: 100px">วันที่บันทึกใบขอเบิก</th>
                                        <th>เลขที่เอกสารใบเบิก</th>
                                        <th>เลขที่เอกสารอ้างอิง</th>
                                        <th>ผู้ขอเบิก</th>
                                        <th>สถานะ</th>
                                        <th style="width: 160px"></th>
                                    </tr>
                                </thead>

                                <tbody id="iBody">
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="dialog-message" class="hide">
        <div class="row">
            <div class="col-xs-12">
                <div class="widget-box">
                    <div class="widget-body">
                        <div class="widget-main">
                            <div id="fuelux-wizard-container">
                                <input type="text" class="hide" value="" id="focusText" />
                                <ul class="steps">
                                    <li data-step="1" class="active">
                                        <span class="step">1</span>
                                        <span class="title">รายละเอียดการตั้งเบิก</span>
                                    </li>

                                    <li data-step="2">
                                        <span class="step">2</span>
                                        <span class="title">รายละเอียดสินค้า</span>
                                    </li>

                                    <li data-step="3">
                                        <span class="step">3</span>
                                        <span class="title">การจัดการ</span>
                                    </li>
                                </ul>
                                <hr />
                                <div class="step-content pos-rel">
                                    <div class="step-pane active" data-step="1">
                                        <div class="row" id="id-StepOne">
                                            <div class="col-sm-12 margin-bottom-menu">
                                                <div class="col-sm-1">
                                                </div>
                                                <div class="col-sm-5">
                                                    <div class="form-group has-info">
                                                        <label class="col-sm-5 control-label move-right" for="lblNewDocID">เลขที่เอกสารใบเบิก</label>
                                                        <div class="col-sm-7">
                                                            <span class="block input-icon input-icon-right" id="lblNewDocID">
                                                                <input type="text" id="newDocID" class="max-width" name="txtBox" placeholder="เลขที่เอกสารใบเบิก" readonly="readonly" />
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-sm-5">
                                                    <div class="form-group has-warning" id="classRefDocID">
                                                        <label class="col-sm-5 control-label move-right" for="lblRefID">เลขที่เอกสารอ้างอิง</label>
                                                        <div class="col-sm-7">
                                                            <span class="block input-icon input-icon-right" id="lblRefID">
                                                                <select multiple="" class="chosen-select form-control" id="RefDocID" data-placeholder="เลขที่เอกสารอ้างอิง">
                                                                </select>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-sm-1"></div>
                                            </div>
                                            <div class="col-sm-12 margin-bottom-menu" id="classrecieveDocDatePick">
                                                <div class="col-sm-1"></div>
                                                <div class="col-sm-5">
                                                    <div class="form-group has-warning">
                                                        <label class="col-sm-5 control-label move-right" for="lblRvDocDate">วันที่รับเอกสารขอเบิก</label>
                                                        <div class="col-sm-7">
                                                            <span class="block input-icon input-icon-right" id="lblRvDocDate">
                                                                <input type="text" id="recieveDocDatePick" class="max-width" placeholder="วันที่รับเอกสารขอเบิก" />
                                                                <i class="ace-icon fa fa-calendar"></i>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-sm-5">
                                                    <div class="form-group has-warning" id="classbookDocDatePick">
                                                        <label class="col-sm-5 control-label move-right" for="lblBkDocDate">วันที่บันทึกใบขอเบิก</label>
                                                        <div class="col-sm-7">
                                                            <span class="block input-icon input-icon-right" id="lblBkDocDate">
                                                                <input type="text" id="bookDocDatePick" class="max-width" placeholder="วันที่บันทึกใบขอเบิก" />
                                                                <i class="ace-icon fa fa-calendar"></i>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-sm-1"></div>
                                            </div>
                                            <div class="col-sm-12 margin-bottom-menu">
                                                <div class="col-sm-1"></div>
                                                <div class="col-sm-5">
                                                    <div class="form-group has-warning" id="classcbxAddUser">
                                                        <label class="col-sm-3 control-label move-right" for="lblAddUser">ผู้ขอเบิก</label>
                                                        <div class="col-sm-9">
                                                            <span class="block input-icon input-icon-right" id="lblAddUser">
                                                                <select class="chosen-select max-width" id="cbxAddUser" data-placeholder="--- กรุณาเลือก ผู้ขอเบิก ---">
                                                                </select>
                                                                <i class="ace-icon fa fa-asterisk"></i>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-sm-5" id="rqApprover">
                                                    <div class="form-group has-warning" id="classApprove">
                                                        <label class="col-sm-3 control-label move-right" for="lblAddType">ผู้อนุมัติ</label>
                                                        <div class="col-sm-9">
                                                            <span class="block input-icon input-icon-right" id="lblAddType">
                                                                <select class="chosen-select form-control" id="cbxAddApprover" data-placeholder="--- กรุณาเลือก ผู้อนุมัติ ---">
                                                                </select>
                                                                <i class="ace-icon fa fa-asterisk"></i>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-sm-1"></div>
                                            </div>
                                            <div class="col-sm-12 margin-bottom-menu">
                                                <div class="col-sm-1"></div>
                                                <div class="col-sm-5">
                                                    <div class="form-group has-warning" id="classcbxAddPromotionType">
                                                        <label class="col-sm-3 control-label move-right" for="lblAddPromo">โปรโมชั่น</label>
                                                        <div class="col-sm-9">
                                                            <span class="block input-icon input-icon-right" id="lblAddPromo">
                                                                <select class="chosen-select form-control" id="cbxAddPromotionType" data-placeholder="--- กรุณาเลือก โปรโมชั่น ---">
                                                                </select>
                                                                <i class="ace-icon fa fa-asterisk"></i>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-sm-5">
                                                    <div class="form-group has-warning" id="classCostCenter">
                                                        <label class="col-sm-3 control-label move-right" for="lblAddCostCenter">หน่วยงาน</label>
                                                        <div class="col-sm-9">
                                                            <span class="block input-icon input-icon-right" id="lblAddCostCenter">
                                                                <select class="chosen-select form-control" id="cbxAddCostCenter" data-placeholder="--- กรุณาเลือก หน่วยงาน ---">
                                                                </select>
                                                                <i class="ace-icon fa fa-asterisk"></i>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-sm-1"></div>
                                            </div>
                                            <div class="col-sm-12 margin-bottom-menu">
                                                <div class="col-sm-1"></div>
                                                <div class="col-sm-5">
                                                    <div class="form-group has-warning" id="classGLNo">
                                                        <label class="col-sm-3 control-label move-right" for="lblAddGL">วัตถุประสงค์</label>
                                                        <div class="col-sm-9">
                                                            <span class="block input-icon input-icon-right" id="lblAddGL">
                                                                <select class="chosen-select form-control" id="cbxAddGLNo" data-placeholder="--- กรุณาเลือก วัตถุประสงค์ ---">
                                                                </select>
                                                                <i class="ace-icon fa fa-asterisk"></i>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-sm-5">
                                                    <div class="form-group has-info" id="classcbxAddObjective">
                                                        <label class="col-sm-3 control-label move-right" for="lblAddObj">วิธีการแจก</label>
                                                        <div class="col-sm-9">
                                                            <span class="block input-icon input-icon-right" id="lblAddObj">
                                                                <select class="chosen-select form-control" id="cbxAddObjective" data-placeholder="--- กรุณาเลือก วิธีการแจก ---">
                                                                </select>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-sm-1"></div>
                                            </div>
                                            <div class="col-sm-12 margin-bottom-menu">
                                                <div class="col-sm-1"></div>
                                                <div class="col-sm-10">
                                                    <div class="form-group has-warning">
                                                        <label id="txtDetail" class="control-label" for="txtDetail">หมายเหตุ</label>
                                                        <label id="lbDetail2" class="control-label" for="txtDetail2"></label>
                                                        <textarea id="txtRemark" class="form-control max-width" placeholder="หมายเหตุ"></textarea>
                                                    </div>
                                                </div>
                                                <div class="col-sm-1"></div>
                                            </div>
                                            <div class="col-sm-12 margin-bottom-menu">
                                                <div class="col-sm-1"></div>
                                                <div class="col-sm-10">
                                                    <div class="form-group">
                                                        <label id="txtDetailRemark" class="control-label hide" for="lblNewDocID" style="color: #DC143C;">
                                                            “สินค้าที่มีมูลค่า 1,000 บาทขึ้นไป ต้องหักภาษี ณ ที่จ่าย 5% และต้องขอชื่อ-นามสกุลของลูกค้า , E-mail , เบอร์โทรศัพท์มือถือ/เบอร์ติดต่อลูกค้า ระบุมาที่แบบฟอร์มรับของด้วย”
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="col-sm-1"></div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="step-pane" data-step="2">
                                        <div class="row" id="id-StepTwo">
                                            <div class="col-sm-12 margin-bottom-menu">

                                                <div class="hide formkt">

                                                    <form class="form-inline">
                                                        <div class="col-sm-5 form-group has-warning">
                                                            <label class="col-sm-4" for="lblAddProject">อ้างอิงเอกสาร Memo เลขที่</label>
                                                            <span class="col-sm-8" id="lblForMemo">
                                                                <select class="chosen-select form-control hide" id="cbxForMemo" data-placeholder="--- กรุณาเลือก โครงการ ---">
                                                                </select>
                                                            </span>
                                                        </div>
                                                        <div class="col-sm-5 form-group has-warning">
                                                            <asp:Label ID="lbCostCenter" runat="server" Text="เพื่อโครงการ" class="col-sm-4" for="lblAddProject"></asp:Label>
                                                            <%--<label class="col-sm-4" for="lblAddProject">เพื่อโครงการ</label>--%>
                                                            <span class="col-sm-8" id="lblForProject">
                                                                <select class="chosen-select form-control" id="cbxForProject" data-placeholder="--- กรุณาเลือก โครงการ ---">
                                                                </select>
                                                            </span>
                                                        </div>

                                                        <div class="col-sm-2 form-group">
                                                            <a href="#" class="btn btn-white btn-default btn-round btn-selectmemo" onclick="return false;">เลือก</a>
                                                        </div>
                                                    </form>
                                                </div>

                                                <div class="col-sm-5 a_project">
                                                    <div class="form-group has-warning">
                                                        <label class="col-sm-3 control-label move-right" for="lblAddProject">โครงการ/หน่วยงาน</label>
                                                        <div class="col-sm-9">
                                                            <span class="block input-icon input-icon-right" id="lblAddProject">
                                                                <select class="chosen-select form-control" id="cbxAddProject" data-placeholder="--- กรุณาเลือก โครงการ ---">
                                                                </select>
                                                                <i class="ace-icon fa fa-asterisk"></i>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-sm-7">
                                                    <div class="form-group has-warning" id="useperiod">
                                                        <label class="col-sm-3 control-label move-right" for="lblsetStdDate">ระยะเวลาใช้งาน</label>
                                                        <div class="col-sm-4">
                                                            <span class="block input-icon input-icon-right" id="lblsetStdDate">
                                                                <input type="text" id="setStdDatePicker" class="max-width" placeholder="" />
                                                                <i class="ace-icon fa fa-calendar"></i>
                                                            </span>
                                                        </div>
                                                        <label class="col-sm-1 control-label" for="lblsetEndDate">~</label>
                                                        <div class="col-sm-4">
                                                            <span class="block input-icon input-icon-right" id="lblsetEndDate">
                                                                <input type="text" id="setEndDatePicker" class="max-width" placeholder="" />
                                                                <i class="ace-icon fa fa-calendar"></i>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-sm-12 margin-bottom-menu cls-promo">
                                                <div class="col-sm-6">
                                                    <div class="form-group has-warning">
                                                        <label class="col-sm-3 control-label move-right" for="lblAddPromoType">สินค้าโปรโมชั่น</label>
                                                        <div class="col-sm-9">
                                                            <span class="block input-icon input-icon-right" id="lblAddPromoType">
                                                                <select class="chosen-select form-control" id="cbxAddPromoType" data-placeholder="--- กรุณาเลือก สินค้าโปรโมชั่น ---">
                                                                </select>
                                                                <i class="ace-icon fa fa-asterisk"></i>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-sm-3">
                                                    <div class="form-group has-warning">
                                                        <label class="col-sm-4 control-label move-right" for="lblreminDay">เตือนก่อน</label>
                                                        <div class="col-sm-6">
                                                            <span class="block input-icon input-icon-right" id="lblreminDay">
                                                                <input type="text" id="reminDay" class="max-width" name="txtBox" placeholder="เตือนก่อน" />
                                                                <i class="ace-icon fa fa-asterisk"></i>
                                                            </span>
                                                        </div>
                                                        <label class="col-sm-2 control-label move-left" for="lblreminDay">วัน</label>
                                                    </div>
                                                </div>
                                                <div class="col-sm-3">
                                                    <div class="form-group has-warning">
                                                        <label class="col-sm-3 control-label move-right" for="lblItems">จำนวน</label>
                                                        <div class="col-sm-7">
                                                            <span class="block input-icon input-icon-right" id="lblItems">
                                                                <input type="text" id="txtQuantity" class="max-width" name="txtBox" placeholder="จำนวน" />
                                                                <i class="ace-icon fa fa-asterisk"></i>
                                                            </span>
                                                        </div>
                                                        <label class="col-sm-2 control-label move-left" for="lblItems">ชิ้น</label>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-sm-12 cls-srh">
                                                <div class="col-sm-7">
                                                </div>
                                                <div class="col-sm-2">
                                                    <button type="button" id="btnClearDetail" class="btn btn-sm btn-white btn-info max-width">
                                                        <i class="ace-icon glyphicon glyphicon-refresh"></i>เคลียร์
                                                    </button>
                                                </div>
                                                <div class="col-sm-3">
                                                    <button type="button" id="btnAddDetail" class="btn btn-sm btn-white btn-primary max-width">
                                                        <i class="ace-icon glyphicon glyphicon-plus"></i>เพิ่ม
                                                    </button>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="hr hr-18 dotted hr-double"></div>
                                        <div class="row" style="margin-top: 5px;">
                                            <div class="col-sm-12">
                                                <table id="table-product" class="table table-bordered table-hover" style="overflow: auto;">
                                                    <thead>
                                                        <tr>
                                                            <th class="hide" id="projectID">111</th>
                                                            <th id="">โครงการ</th>
                                                            <th class="hide" id="promoItemNo">222</th>
                                                            <th id="">สินค้าโปรโมชั่น</th>
                                                            <th id="" style="width: 75px">จำนวน</th>
                                                            <th id="" style="width: 75px">วันเริ่มต้น</th>
                                                            <th id="" style="width: 75px">วันสิ้นสุด</th>
                                                            <th id="" style="width: 105px">เตือนก่อน(วัน)</th>
                                                            <th style="width: 35px"></th>
                                                        </tr>
                                                    </thead>

                                                    <tbody id="iProduct">
                                                    </tbody>
                                                </table>

                                                <table id="mktTable" class="table table-hover hide" cellspacing="0" width="100%">
                                                    <thead>
                                                        <tr>
                                                            <th class="center"></th>
                                                            <th class="center">เลขที่โครงการ</th>
                                                            <th class="center">โครงการ</th>
                                                            <th class="center">Memo</th>
                                                            <th class="center">เลขที่รายการ</th>
                                                            <th class="center" style="width: 30%">รายการขอเบิก</th>
                                                            <th class="center">สิทธิ์การเบิก</th>
                                                            <th class="center" style="width: 70px">จำนวน</th>
                                                            <th class="center" style="width: 25px">หน่วย</th>
                                                            <th class="center" style="width: 35px">#</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody id="mktDetails">
                                                    </tbody>
                                                </table>

                                            </div>
                                        </div>
                                    </div>


                                    <div class="step-pane" data-step="3">
                                        <div class="center">
                                            <h3 class="blue lighter" id="lblTextQ">คุณต้องการ เปิด หรือ ส่งเอกสารนี้หรือไม่ ?</h3>

                                            <div class="hide" id="confirm-reject">
                                                <div class="col-sm-12">
                                                    <div class="col-sm-1"></div>
                                                    <div class="col-sm-10">

                                                        <div class="col-sm-12">
                                                            <div style="clear: both;"></div>
                                                            <div class="on-focus clearfix  width-100" style="position: relative; padding: 0px; margin: 10px auto; display: table; float: left">
                                                                <input type="text" style="width: 100%;" value="" name="try" id="rejectText" placeholder="ระบุเหตุผล" />
                                                                <div class="tool-tip  slideIn">Input here !!</div>
                                                            </div>

                                                        </div>
                                                        <div class="col-sm-12" style="margin-top: 5px;">
                                                            <button class="btn btn-primary" id="cancelReject">
                                                                ยกเลิก
                                                            </button>
                                                            <button class="btn btn-danger" id="submitReject">
                                                                ไม่อนุมัติ
                                                            </button>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-1"></div>
                                                </div>
                                            </div>

                                        </div>

                                        <div class="row">
                                        </div>

                                    </div>
                                </div>
                            </div>

                            <hr />

                            <div class="wizard-actions">

                                <%--<button id="btnClear" class="btn btn-white btn-info pull-left">
                                    <i class="ace-icon glyphicon glyphicon-refresh"></i>
                                    Clear
                                </button>--%>

                                <button class="btn btn-prev" onclick="onTop(0);">
                                    <i class="ace-icon fa fa-arrow-left"></i>
                                    ก่อนหน้า
                                </button>

                                <button class="btn btn-danger hide" data-last="Reject" id="reject" onclick="onTop(5)">
                                    ไม่อนุมัติ
									<i class="ace-icon fa fa-ban icon-on-right"></i>
                                </button>

                                <button class="btn btn-success hide" data-last="Open" id="open" onclick="onTop(2)">
                                    บันทึก
									<i class="ace-icon fa fa-floppy-o icon-on-right"></i>
                                </button>

                                <button class="btn btn-success hide" data-last="Edit" id="edit" onclick="onTop(3)">
                                    บันทึก
									<i class="ace-icon fa fa-floppy-o icon-on-right"></i>
                                </button>

                                <button class="btn btn-success hide" data-last="Sent" id="sent" onclick="onTop(7)">
                                    อนุมัติ
									<i class="ace-icon fa fa-share icon-on-right"></i>
                                </button>

                                <button class="btn btn-success hide" data-last="Approve" id="approve" onclick="onTop(4)">
                                    อนุมัติ
									<i class="ace-icon fa fa-check icon-on-right"></i>
                                </button>

                                <button class="btn btn-success hide" data-last="Accept" id="accept" onclick="onTop(6)">
                                    รับงาน
									<i class="ace-icon fa fa-check icon-on-right"></i>
                                </button>

                                <button class="btn btn-success btn-next" data-last="อนุมัติ" id="save" onclick="onTop(1)">
                                    ถัดไป
									<i class="ace-icon fa fa-arrow-right icon-on-right"></i>
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="myModalLabel">แสดงรายละเอียด</h4>
                </div>
                <div class="modal-body">
                    <div class="panel panel-info disabledClass">
                        <div class="panel-heading">
                            <h3 class="panel-title center">ข้อมูลทั่วไป</h3>
                        </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-sm-12 margin-bottom-menu">
                                    <div class="col-sm-1">
                                    </div>
                                    <div class="col-sm-5">
                                        <div class="form-group has-info">
                                            <label class="col-sm-5 control-label move-right" for="lblNewDocID2">เลขที่เอกสารใบเบิก</label>
                                            <div class="col-sm-7">
                                                <span class="block input-icon input-icon-right" id="lblNewDocID2">
                                                    <input type="text" id="a_DocID" class="max-width" name="txtBox" placeholder="เลขที่เอกสารใบเบิก" readonly="readonly" />
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-5">
                                        <div class="form-group " id="classRefDocID2">
                                            <label class="col-sm-5 control-label move-right" for="lblRefID2">เลขที่เอกสารอ้างอิง</label>
                                            <div class="col-sm-7">
                                                <span class="block input-icon input-icon-right" id="lblRefID2">
                                                    <select multiple="" class="chosen-select form-control" id="a_RefDocID" data-placeholder="เลขที่เอกสารอ้างอิง">
                                                    </select>

                                                </span>
                                            </div>
                                        </div>

                                    </div>
                                    <div class="col-sm-1"></div>
                                </div>
                                <div class="col-sm-12 margin-bottom-menu" id="classrecieveDocDatePick2">
                                    <div class="col-sm-1"></div>
                                    <div class="col-sm-5">
                                        <div class="form-group ">
                                            <label class="col-sm-5 control-label move-right" for="lblRvDocDate2">วันที่รับเอกสารขอเบิก</label>
                                            <div class="col-sm-7">
                                                <span class="block input-icon input-icon-right" id="lblRvDocDate2">
                                                    <input type="text" id="a_recieveDocDatePick" class="max-width" placeholder="วันที่รับเอกสารขอเบิก" />
                                                    <i class="ace-icon fa fa-calendar"></i>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-5">
                                        <div class="form-group " id="classbookDocDatePick2">
                                            <label class="col-sm-5 control-label move-right" for="lblBkDocDate2">วันที่บันทึกใบขอเบิก</label>
                                            <div class="col-sm-7">
                                                <span class="block input-icon input-icon-right" id="lblBkDocDate2">
                                                    <input type="text" id="a_bookDocDatePick" class="max-width" placeholder="วันที่บันทึกใบขอเบิก" />
                                                    <i class="ace-icon fa fa-calendar"></i>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-1"></div>
                                </div>
                                <div class="col-sm-12 margin-bottom-menu">
                                    <div class="col-sm-1"></div>
                                    <div class="col-sm-5">
                                        <div class="form-group " id="classcbxAddUser2">
                                            <label class="col-sm-3 control-label move-right" for="lblAddUser2">ผู้ขอเบิก</label>
                                            <div class="col-sm-9">
                                                <span class="block input-icon input-icon-right" id="lblAddUser2">
                                                    <select class="chosen-select max-width" id="a_cbxAddUser" data-placeholder="--- กรุณาเลือก ผู้ขอเบิก ---">
                                                    </select>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-5" id="rqApprover2">
                                        <div class="form-group " id="classApprove2">
                                            <label class="col-sm-3 control-label move-right" for="lblAddType2">ผู้อนุมัติ</label>
                                            <div class="col-sm-9">
                                                <span class="block input-icon input-icon-right" id="lblAddType2">
                                                    <select class="chosen-select form-control" id="a_cbxAddApprover" data-placeholder="--- กรุณาเลือก ผู้อนุมัติ ---">
                                                    </select>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-1"></div>
                                </div>
                                <div class="col-sm-12 margin-bottom-menu">
                                    <div class="col-sm-1"></div>
                                    <div class="col-sm-5">
                                        <div class="form-group " id="classcbxAddPromotionType2">
                                            <label class="col-sm-3 control-label move-right" for="lblAddPromo2">โปรโมชั่น</label>
                                            <div class="col-sm-9">
                                                <span class="block input-icon input-icon-right" id="lblAddPromo2">
                                                    <select class="chosen-select form-control" id="a_cbxAddPromotionType" data-placeholder="--- กรุณาเลือก โปรโมชั่น ---">
                                                    </select>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-5">
                                        <div class="form-group " id="classCostCenter2">
                                            <label class="col-sm-3 control-label move-right" for="lblAddCostCenter2">หน่วยงาน</label>
                                            <div class="col-sm-9">
                                                <span class="block input-icon input-icon-right" id="lblAddCostCenter2">
                                                    <select class="chosen-select form-control" id="a_cbxAddCostCenter" data-placeholder="--- กรุณาเลือก หน่วยงาน ---">
                                                    </select>

                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-1"></div>
                                </div>
                                <div class="col-sm-12 margin-bottom-menu">
                                    <div class="col-sm-1"></div>
                                    <div class="col-sm-5">
                                        <div class="form-group " id="classGLNo2">
                                            <label class="col-sm-3 control-label move-right" for="lblAddGL2">วัตถุประสงค์</label>
                                            <div class="col-sm-9">
                                                <span class="block input-icon input-icon-right" id="lblAddGL2">
                                                    <select class="chosen-select form-control" id="a_cbxAddGLNo" data-placeholder="--- กรุณาเลือก วัตถุประสงค์ ---">
                                                    </select>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-5">
                                        <div class="form-group has-info" id="classcbxAddObjective2">
                                            <label class="col-sm-3 control-label move-right" for="lblAddObj2">วิธีการแจก</label>
                                            <div class="col-sm-9">
                                                <span class="block input-icon input-icon-right" id="lblAddObj2">
                                                    <select class="chosen-select form-control" id="a_cbxAddObjective" data-placeholder="--- วิธีการแจก ---">
                                                    </select>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-1"></div>
                                </div>
                                <div class="col-sm-12 margin-bottom-menu">
                                    <div class="col-sm-1"></div>
                                    <div class="col-sm-10">
                                        <div class="form-group ">
                                            <label class="control-label" for="txtDetail">หมายเหตุ</label>
                                            <textarea id="a_Remark" class="form-control max-width" placeholder="หมายเหตุ"></textarea>
                                        </div>
                                    </div>
                                    <div class="col-sm-1"></div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-info disabledClass">
                        <div class="panel-heading">
                            <h3 class="panel-title center">รายละเอียดรายการ</h3>
                        </div>
                        <div class="panel-body">
                            <div class="row">
                                <table id="a_t_product" class="table table-striped table-bordered table-hover no-margin-bottom no-border-top" style="overflow: auto;">
                                    <thead>
                                        <tr>
                                            <th class="hide" id="a_projectID"></th>
                                            <th id="">โครงการ</th>
                                            <th class="hide" id="a_promoItemNo"></th>
                                            <th id="">สินค้าโปรโมชั่น</th>
                                            <th id="" style="width: 75px">จำนวน</th>
                                            <th id="" style="width: 75px">วันเริ่มต้น</th>
                                            <th id="" style="width: 75px">วันสิ้นสุด</th>
                                            <th id="" style="width: 105px">เตือนก่อน(วัน)</th>
                                            <th style="width: 35px"></th>
                                        </tr>
                                    </thead>

                                    <tbody id="a_Product">
                                    </tbody>
                                </table>

                                <table id="a_mktTable" class="table table-hover hide" cellspacing="0" width="100%">
                                    <thead>
                                        <tr>
                                            <th class="center">รายการที่</th>
                                            <th class="center">เลขที่โครงการ</th>
                                            <th class="center">โครงการ</th>
                                            <th class="center">Memo เลขที่</th>
                                            <th class="center">เลขที่รายการขอเบิก</th>
                                            <th class="center">รายการขอเบิก</th>
                                            <th class="center">สิทธิ์การเบิก</th>
                                            <th class="center" style="width: 70px">จำนวนขอเบิก</th>
                                            <th class="center" style="width: 25px">หน่วย</th>
                                            <th class="center" style="width: 35px">#</th>
                                        </tr>
                                    </thead>
                                    <tbody id="mktDetails">
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" id="a_btnClose" class="btn btn-default" data-dismiss="modal">ปิด</button>
                    <button type="button" id="a_Reject" class="btn btn-danger" onclick="onTop(5)">ปฎิเสธ <i class="ace-icon fa fa-ban icon-on-right"></i></button>
                    <button type="button" id="a_Approve" class="btn btn-success" onclick="onTop(4)">อนุมัติ <i class="ace-icon fa fa-check icon-on-right"></i></button>
                    <button type="button" id="a_Accept" class="btn btn-success" onclick="onTop(6)">รับงาน <i class="ace-icon fa fa-check icon-on-right"></i></button>

                </div>
            </div>
        </div>
    </div>


    <!-- Modal -->
    <div class="modal fade" id="memo-modal" tabindex="-1" role="dialog" aria-labelledby="memo-label">
        <div class="modal-dialog modal-md" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="memo-label">การจัดการรายการตั้งเบิก</h4>
                </div>
                <div class="modal-body">

                    <table id="memoTable" class="table table-hover">
                        <thead>
                            <tr>
                                <th class="center" style="width: 30px"></th>
                                <th class="center">เลขที่โครงการ</th>
                                <th class="center">โครงการ</th>
                                <th class="center" style="width: 150px">Memo เลขที่</th>
                                <th class="center" style="width: 180px">รหัสสินค้า</th>
                                <th class="center" id="">รายการขอเบิก</th>
                            </tr>
                        </thead>

                        <tbody id="memoBody">
                        </tbody>
                    </table>
                </div>
                <div class="modal-footer">
                    <button type="button" id="m_btnClose" class="btn btn-danger" data-dismiss="modal">ปิด</button>
                    <button type="button" id="b_add" class="btn btn-success" onclick="return false;">เพิ่ม</button>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
