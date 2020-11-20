<%@ Page Title="Master Access Right" Language="C#" MasterPageFile="~/master/MasterPage.Master"
    AutoEventWireup="true" CodeBehind="MasterData.aspx.cs" Inherits="AP_StockPromotion_V1.webpage.MasterData" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        textarea {
            width: 90%;
            height: 100px;
        }

        input[type="text"] {
            width: 50%;
        }

        .ui-datepicker {
            z-index: 1150 !important;
        }

        .ui-dialog {
            top: 50px !important;
        }

        .max-width {
            width: 100% !important;
        }
    </style>

    <script type="text/javascript">

        var tableSetting, titleText, source, dialogMode, dataTableMode, dataBody, optUser, optGroup, optVal, optGroupForEditing;
        var saveDataMapping = [];

        jQuery(function ($) {
            //bootbox.dialog({
            //    message: txtMessage,
            //    buttons: {
            //        btnType: {
            //            "label": lblText,
            //            "className": clsText
            //        }
            //    }
            //});

            $("#description").keyup(function (e) {
                if (this.value.length > 250) {
                    alert("Description is not over than 250 character");
                    this.value = "";
                    this.focus();
                }
            });

            $("#StartDatePicker").datepicker({
                showOtherMonths: true,
                selectOtherMonths: false,
                dateFormat: 'dd/mm/yy'
            });

            $("#EndDatePicker").datepicker({
                showOtherMonths: true,
                selectOtherMonths: false,
                dateFormat: 'dd/mm/yy'
            });

            $("#startDate").datepicker({
                showOtherMonths: true,
                selectOtherMonths: false,
                dateFormat: 'dd/mm/yy',
            });



            $("#endDate").datepicker({
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

            $("#id-btn-dialog1").on('click', function (e) {
                $("#startDate").val(moment().format('DD/MM/YYYY'));
                e.preventDefault();

                $("#txt1").attr('readonly', false);

                localStorage.setItem("Mode", 1);
                var dialog = $(dialogMode).removeClass('hide').dialog({
                    width: screen.width / 100 * 80,
                    height: (source == 'right' ? 500 : 300),
                    modal: true,
                    title: titleText,
                    title_html: true,
                    buttons: [
                        {
                            text: "Cancel",
                            "class": "btn btn-minier",
                            click: function () {
                                $(this).dialog("close");
                            }
                        },
                        {
                            text: (source == 'right' ? "Save" : "Add"),
                            "class": "btn btn-primary btn-minier",
                            click: function () {

                                if (source == 'right') {

                                    bootbox.dialog({
                                        message: '<h4 style="text-align: center">Do you want to save ?</h4>',
                                        buttons: {
                                            success: {
                                                label: "Yes",
                                                className: "btn-sm btn-primary",
                                                callback: function () {
                                                    if (saveDataMapping == "") return;

                                                    $.ajax({
                                                        type: "POST",
                                                        url: "MasterData.aspx/OnCommandSaveMapping",
                                                        data: JSON.stringify({ saveDataMapping: saveDataMapping }),
                                                        contentType: 'application/json; charset=utf-8',
                                                        dataType: 'json',
                                                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                                                            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                                                                    textStatus + "\n\nError: " + errorThrown); S
                                                        },
                                                        success: function (result) {
                                                            ClearData();
                                                            try {
                                                                $.each(result, function (k, Val) {
                                                                    $.each(Val, function (key, Value) {
                                                                        localStorage.setItem(key, JSON.stringify(Value));
                                                                        var data = RenderDatatable(key, Value);
                                                                        $(dataBody).append(data);
                                                                    });
                                                                });
                                                            } catch (e) {

                                                            }
                                                            CallDatatable();

                                                            saveDataMapping = [];

                                                            var table = $('#tUserGroup').DataTable();
                                                            table.row().remove().draw();
                                                        }
                                                    });

                                                }
                                            },
                                            danger: {
                                                label: "No",
                                                className: "btn-sm btn-danger",
                                                callback: function () {
                                                    return;
                                                }
                                            }
                                        }

                                    });

                                }
                                else {
                                    var startDate = $("#startDate").val();
                                    var endDate = $("#endDate").val();
                                    var Reason = $('input[name="reasonText"]').val();

                                    if (Reason === "" || Reason === null) {
                                        $("#textDesc").text("กรุณากรอกข้อมูลให้ครบทุกช่อง");
                                        var dialog = $("#dialog-desc").removeClass('hide').dialog({
                                            width: screen.width / 100 * 30,
                                            modal: true,
                                            title: "<div class='widget-header widget-header-small' style='color: Orange'> " +
                                                   "<h4 class='smaller'><i class='ace-icon glyphicon glyphicon-flag'></i> Warning</h4></div>",
                                            title_html: true,
                                            buttons: [
                                                {
                                                    text: "OK",
                                                    "class": "btn btn-info btn-minier",
                                                    click: function () {
                                                        $(this).dialog("close");
                                                    }
                                                }
                                            ]
                                        });
                                        return false;
                                    }
                                    if (ValidateData(localStorage.getItem("Mode"))) {
                                        var dataValue = "{'title':'" + $("#txt1").val() +
                                                        "', 'description':'" + $("#txtarea").val() +
                                                        "', 'startDate':'" + $("#startDate").val() +
                                                        "', 'endDate':'" + $("#endDate").val() +
                                                        "', 'mode':'1'}";
                                        $.ajax({
                                            type: "POST",
                                            url: "MasterData.aspx/OnCommandAdd",
                                            data: dataValue,
                                            contentType: 'application/json; charset=utf-8',
                                            dataType: 'json',
                                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                                alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                                                        textStatus + "\n\nError: " + errorThrown);
                                            },
                                            success: function (result) {
                                                if (result.d.Success) {
                                                    ClearData();
                                                    $.each(result.d.Data, function (key, Value) {
                                                        localStorage.setItem(key, JSON.stringify(Value));
                                                        var data = RenderDatatable(key, Value);
                                                        $(dataBody).append(data);
                                                    });

                                                    CallDatatable();
                                                } else {
                                                    bootbox.alert(result.d.Message);
                                                }
                                            }
                                        });
                                    }
                                    else {
                                        return;
                                    }
                                    $("#StartDatePicker").val(startDate);
                                    $("#EndDatePicker").val(endDate);
                                    $(this).dialog("close");

                                }
                            }
                        }
                    ]
                });
                $(this).dialog("close");
            });

            $('' + dialogMode + '').on('dialogclose', function (e) {
                $("#txt1").val("");
                $("#txtarea").val("");
                $("#startDate").val("");
                $("#endDate").val("");

                $('#cbxAddUser').val('');
                $('#cbxAddUser').trigger("chosen:updated");
                $('#cbxAddGroup').val('');
                $('#cbxAddGroup').trigger("chosen:updated");
                saveDataMapping = [];
                var table = $('#tUserGroup').DataTable();
                table.row().remove().draw();
            });

            $('' + dialogMode + '').on('dialogopen', function (e) {
                $("#description").focus();
            });

            var dataValue = "{'mode':'" + 0 +
                            "', 'startDate':'', 'endDate':''}";
            $.ajax({
                type: "POST",
                url: "MasterData.aspx/OnCommandLoad",
                contentType: 'application/json; charset=utf-8',
                data: dataValue,
                dataType: 'json',
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                            textStatus + "\n\nError: " + errorThrown);
                },
                success: function (result) {
                    ClearData();
                    try {
                        $.each(result, function (k, Val) {
                            $.each(Val, function (key, Value) {
                                localStorage.setItem(key, JSON.stringify(Value));
                                var data = RenderDatatable(key, Value);
                                $(dataBody).append(data);
                            });
                        });
                    } catch (e) {

                    }

                    CallDatatable();
                }
            });

            $("#clear").on('click', function (e) {
                $("#StartDatePicker").val("");
                $("#EndDatePicker").val("");
            });

            $("#search").on('click', function (e) {

                var mode = 3;
                if ($("#StartDatePicker").val() != "" && $("#EndDatePicker").val() != "") {
                    mode = 4
                }

                var StartDatePicker = $("#StartDatePicker").val();
                var EndDatePicker = $("#EndDatePicker").val();

                if (StartDatePicker != "" && EndDatePicker != "") {
                    var start = $("#StartDatePicker").val().split("/");
                    var first = new Date(start[2], start[1] - 1, start[0]);
                    var end = $("#EndDatePicker").val().split("/");
                    var last = new Date(end[2], end[1] - 1, end[0]);
                    if (first > last) {
                        bootbox.alert('Start Date ต้องน้อยกว่า End Date');
                        return false;
                    }
                }

                var dataValue = "{'mode':'" + mode +
                                "', 'startDate':'" + $("#StartDatePicker").val() +
                                "', 'endDate':'" + $("#EndDatePicker").val() + "'}";
                $.ajax({
                    type: "POST",
                    url: "MasterData.aspx/OnCommandLoad",
                    contentType: 'application/json; charset=utf-8',
                    data: dataValue,
                    dataType: 'json',
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                                textStatus + "\n\nError: " + errorThrown);
                    },
                    success: function (result) {
                        ClearData();
                        try {
                            $.each(result, function (k, Val) {
                                $.each(Val, function (key, Value) {
                                    localStorage.setItem(key, JSON.stringify(Value));
                                    var data = RenderDatatable(key, Value);
                                    $(dataBody).append(data);
                                });
                            });
                        } catch (e) {
                        }

                        CallDatatable();
                    }
                });
            });

            if (!ace.vars['touch']) {
                $('.chosen-select').chosen({ allow_single_deselect: true });
                //resize the chosen on window resize

                $(window)
                .off('resize.chosen')
                .on('resize.chosen', function () {
                    $('.chosen-select').each(function () {
                        var $this = $(this);
                        $this.next().css({ 'width': 100 + '%' }); //$this.parent().width()
                    })
                }).trigger('resize.chosen');
                //resize chosen on sidebar collapse/expand
                $(document).on('settings.ace.chosen', function (e, event_name, event_val) {
                    if (event_name != 'sidebar_collapsed') return;
                    $('.chosen-select').each(function () {
                        var $this = $(this);
                        $this.next().css({ 'width': 100 + '%' }); //$this.parent().width()
                    })
                });
            }

            $("#btnAddToTb").on('click', function (e) {

                var userID = $('#cbxAddUser').val().split(':');
                var userName = $('#cbxAddUser option:selected').text().split(':')[1];

                var groupID = $('#cbxAddGroup').val();
                var groupName = $('#cbxAddGroup option:selected').text();


                var t = $('#tUserGroup').DataTable();

                if (userID[0] == "" || groupID == "") {
                    return;
                }

                for (var i = 0; i < saveDataMapping.length; i++) {
                    if (saveDataMapping[i].userID == userID[0] && saveDataMapping[i].masterGroupID == groupID) {
                        return;
                    }
                }

                saveDataMapping.push({
                    "userID": userID[0],
                    "costCenter": userID[1],
                    "masterGroupID": groupID
                });

                t.row.add([
                    userID[0],
                    userName,
                    userID[1],
                    groupID,
                    groupName,
                    '<td>' +
                    '<div class="hidden-sm hidden-xs action-buttons">' +
                    '<a class="red" href="#" id="' + userID[0] + '" onclick="deleteUserGroup(' + userID[0] + ')"><i class="ace-icon fa fa-trash bigger-130"></i></a>' +
                    '</div>' +
                    '<div class="hidden-md hidden-lg">' +
                    '<div class="inline pos-rel">' +
                    '<button class="btn btn-minier btn-yellow dropdown-toggle" data-toggle="dropdown" data-position="auto"><i class="ace-icon fa fa-caret-down icon-only bigger-120"></i></button>' +
                    '<ul class="dropdown-menu dropdown-only-icon dropdown-yellow dropdown-menu-right dropdown-caret dropdown-close">' +
                    '<li><a href="#" class="tooltip-error" data-rel="tooltip" title="Delete"><span class="red"><i class="ace-icon fa fa-trash-o bigger-120"></i></span></a></li>' +
                    '</ul>' +
                    '</div>' +
                    '</div>' +
                    '</td>'
                ]).draw(false);

            });
        });

        function initAccessRight(result) {
            try {
                $.each(result, function (k, Val) {
                    optUser = '<option value=""> </option>';
                    for (var i = 0; i < Val.User.length; i++) {
                        optUser += '<option value="' + Val.User[i].ID + ':' + Val.User[i].CostCenter + '">' + Val.User[i].ID + ' : ' + Val.User[i].FName + ' ' + Val.User[i].LName + '</option>';
                    }
                    optGroup = '<option value=""> </option>';
                    for (var i = 0; i < Val.Group.length; i++) {
                        optGroup += '<option value="' + Val.Group[i].RIGHT + '">' + Val.Group[i].DESC + '</option>';
                    }
                });
                $('#cbxAddUser').append(optUser);
                $('#cbxAddUser').trigger("chosen:updated");
                $('#cbxAddGroup').append(optGroup);
                $('#cbxAddGroup').trigger("chosen:updated");
            } catch (e) {

            }
        }

        function deleteUserGroup(id) {

            var table = $('#tUserGroup').DataTable();

            $('#tUserGroup tbody').on('click', 'i.ace-icon', function () {

                var userID = table.row($(this).parents('tr')).selector.rows[0].childNodes[0].innerText;
                var costCenter = table.row($(this).parents('tr')).selector.rows[0].childNodes[2].innerText;
                var groupID = table.row($(this).parents('tr')).selector.rows[0].childNodes[3].innerText;

                for (var i = 0; i < saveDataMapping.length; i++) {
                    if (saveDataMapping[i].userID == userID && saveDataMapping[i].costCenter == costCenter && saveDataMapping[i].masterGroupID == groupID) {
                        saveDataMapping.splice(i, 1);
                    }
                }
                table
                    .row($(this).parents('tr'))
                    .remove()
                    .draw();
            });
        }

        function ClearData() {
            localStorage.clear();
            if ($.fn.dataTable.isDataTable('' + dataTableMode + '')) {
                table = $('' + dataTableMode + '').DataTable({
                    retrieve: true,
                    paging: false
                });
                table.destroy();
            }
            $(dataBody).empty();
        }

        function ClearSaveData() {
            //localStorage.clear();
            if ($.fn.dataTable.isDataTable('#tUserGroup')) {
                table = $('#tUserGroup').DataTable({
                    retrieve: true,
                    paging: false
                });
                table.destroy();
            }
            $(dataBody).empty();



        }

        function ChangeUrl(page, url) {
            if (typeof (history.pushState) != "undefined") {
                var obj = { Page: page, Url: url };
                history.pushState(obj, obj.Page, obj.Url);
            } else {
                alert("Browser does not support HTML5.");
            }
        }

        function initFormObjective(str) {
            dialogMode = "#dialog-message";
            dataTableMode = "#dynamic-table";
            dataBody = "#iBody";

            $('#divNavx').html('Management >> Master Objective');
            $('#set-header').html('<i class="ace-icon glyphicon glyphicon-list"></i> Objective');
            tableSetting = [{ "bSortable": false, "visible": false },
                            { "visible": false },
                            null, null,
                            { "visible": false }, { "visible": false },
                            { "visible": false }, { "visible": false },
                            { "visible": false }, null,
                            { "bSortable": false }];

            titleText = "<div class='widget-header widget-color-blue widget-header-flat'> " +
                           "<h4 class='smaller'><i class='ace-icon glyphicon glyphicon-plus'></i> New Objective</h4></div>"

            $("#widget-header").addClass("widget-color-green");

            $("#lbl1").text('Description:');
            $("#lbl2").text('Reason');

            $("#iHeader").html('');
            $("#iHeader").append(setHeader(str));



            source = str;
            $("#Source").val(source);

            ChangeUrl("MasterData.aspx?source=" + source, "MasterData.aspx");

        }

        function initFormGL(str) {
            dialogMode = "#dialog-message";
            dataTableMode = "#dynamic-table";
            dataBody = "#iBody";

            $('#divNavx').html('Management >> Master GL');
            $('#set-header').html('<i class="ace-icon glyphicon glyphicon-list"></i> GL');
            tableSetting = [{ "bSortable": false, "visible": false },
                            { "visible": false },
                            { "visible": false }, { "visible": false },
                            null, null,
                            { "visible": false }, { "visible": false },
                            { "visible": false }, null,
                            { "bSortable": false }];

            titleText = "<div class='widget-header widget-header-small widget-color-orange widget-header-flat'> " +
                           "<h4 class='smaller'><i class='ace-icon glyphicon glyphicon-plus'></i> New GL</h4></div>"

            $("#widget-header").addClass("widget-color-orange");

            $("#lbl1").text('GL Account Number');
            $("#lbl2").text('Description');

            $("#iHeader").html('');
            $("#iHeader").append(setHeader(str));

            source = str;
            $("#Source").val(source);

            ChangeUrl("MasterData.aspx?source=" + source, "MasterData.aspx");
        }

        function initFormINO(str) {
            dialogMode = "#dialog-message";
            dataTableMode = "#dynamic-table";
            dataBody = "#iBody";

            $('#divNavx').html('Management >> Master Internal Order');
            $('#set-header').html('<i class="ace-icon glyphicon glyphicon-list"></i> Internal Order');
            tableSetting = [{ "bSortable": false, "visible": false },
                            { "visible": false },
                            { "visible": false }, { "visible": false },
                            { "visible": false }, { "visible": false },
                            null, null,
                            { "visible": false }, { "visible": false },
                            { "bSortable": false }];

            titleText = "<div class='widget-header widget-header-small widget-color-black widget-header-flat'> " +
                           "<h4 class='smaller'><i class='ace-icon glyphicon glyphicon-plus'></i> New Internal Order</h4></div>"

            $("#widget-header").addClass("widget-color-pink");

            $("#lbl1").text('Internal Order');
            $("#lbl2").text('Description');

            $("#rowData").addClass('hide');

            $("#iHeader").html('');
            $("#iHeader").append(setHeader(str));

            source = str;

            $("#Source").val(source);

            $(".box-start").hide();
            $(".box-end").hide();
            $(".box-search").hide();
            $(".box-clear").hide();

            ChangeUrl("MasterData.aspx?source=" + source, "MasterData.aspx");
        }

        function initFormAccessRight(str) {
            dialogMode = "#dialog-access-right";
            dataTableMode = "#access-table";
            dataBody = "#accessBody";

            $('#dynamic-table').addClass('hide');
            $('#access-table').removeClass('hide');

            $('#divNavx').html('Management >> Master Access Right');
            $('#set-header').html('<i class="ace-icon glyphicon glyphicon-list"></i> Access Right');
            tableSetting = [null, null, null, { "bSortable": false }];

            titleText = "<div class='widget-header widget-header-small widget-color-black widget-header-flat'> " +
                               "<h4 class='smaller'><i class='ace-icon glyphicon glyphicon-plus'></i> Access Right</h4></div>"

            $("#widget-header").addClass("widget-color-blue");

            $.ajax({
                type: "POST",
                url: "MasterData.aspx/OnCommandInitialControl",
                contentType: 'application/json; charset=utf-8',
                //data: dataValue,
                dataType: 'json',
                //async: false,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                            textStatus + "\n\nError: " + errorThrown);
                },
                success: function (result) {
                    optVal = result;
                    initAccessRight(result);
                }
            });
            source = str;
            $("#Source").val(source);
            ChangeUrl("MasterData.aspx?source=" + source, "MasterData.aspx");

        }

        function initGroupForEditing(IDs) {
            optGroupForEditing = "";
            try {

                $('#' + IDs + '').removeClass('hide');

                var grpID = IDs.split('-')[1];
                $.each(optVal, function (k, Val) {
                    for (var i = 0; i < Val.Group.length; i++) {
                        if (grpID == Val.Group[i].RIGHT) {
                            optGroupForEditing += '<option value="' + Val.Group[i].RIGHT + '" selected="selected">' + Val.Group[i].DESC + '</option>';
                        } else {
                            optGroupForEditing += '<option value="' + Val.Group[i].RIGHT + '">' + Val.Group[i].DESC + '</option>';
                        }
                    }
                });
                $('#select' + IDs.split('-')[0] + IDs.split('-')[1] + '').append(optGroupForEditing);
                $('#select' + IDs.split('-')[0] + IDs.split('-')[1] + '').trigger("chosen:updated");

                $('#' + IDs.split('-')[0] + '_' + IDs.split('-')[1] + '').addClass('hide');
                $('#Edit' + IDs.split('-')[0] + '').addClass('hide');

                $('#confirm' + IDs.split('-')[0] + '').removeClass('hide');
                $('#cancel' + IDs.split('-')[0] + '').removeClass('hide');

            } catch (e) {
            }
        }

        function cancelEditing(id, spanID, divID, selectID) {
            optGroupForEditing = "";


            $('#' + spanID + '').removeClass('hide');
            $('#Edit' + id + '').removeClass('hide');


            $('#' + divID + '').addClass('hide');
            $('#confirm' + id + '').addClass('hide');
            $('#cancel' + id + '').addClass('hide');

            $('#' + selectID + '').html(optGroupForEditing);
            $('#' + selectID + '').trigger("chosen:updated");
        }

        function setHeader(str) {

            return strHeader = '<th class=""><label class="pos-rel"><input type="checkbox" class="ace" /><span class="lbl"></span></label></th>'
                             + '<th class="" id="USID">Record ID</th>'
                             + '<th class="" id="USDES" style="width: 200px;">Description</th>'
                             + '<th class="" id="LNGRES">Reason</th>'
                             + '<th class="" id="GLACC" style="width: 200px;">GL Account Number</th>'
                             + '<th class="" id="GLDES">Description</th>'
                             + '<th class="" id="InternalOrder" style="width: 200px;">Internal Order</th>'
                             + '<th class="" id="Description">Description</th>'
                             + '<th class="" id="VALIDSTD">Valid Start Date</th>'
                             + '<th class="" id="VALIDEND" style="width: 115px;">Valid End Date</th>'
                             + '<th style="width: 55px;"></th>';
        }

        function DeleteData(idx) {
            var res = JSON.parse(localStorage.getItem(idx))
            var dialog = $("#dialog-delete").removeClass('hide').dialog({
                width: screen.width / 100 * 30,
                modal: true,
                title: "<div class='widget-header widget-header-small widget-header-flat' style='color: red'> " +
                       "<h4 class='smaller'><i class='ace-icon glyphicon glyphicon-trash'></i> Delete Objective</h4></div>",
                title_html: true,
                buttons: [
                    {
                        text: "Cancel",
                        "class": "btn btn-minier",
                        click: function () {
                            $(this).dialog("close");
                        }
                    },
                    {
                        text: "Delete",
                        "class": "btn btn-danger btn-minier",
                        click: function () {
                            if (ValidateData(4)) {

                                var dataValue = "{'id':'" + (res.USID != "" ? res.USID : 0)
                                              + "', 'title':'" + (res.GLACC == "" ? res.InternalOrder : res.GLACC) + "'}";
                                $.ajax({
                                    type: "POST",
                                    url: "MasterData.aspx/OnCommandDelete",
                                    data: dataValue,
                                    contentType: 'application/json; charset=utf-8',
                                    dataType: 'json',
                                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                                        alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                                                textStatus + "\n\nError: " + errorThrown);
                                    },
                                    success: function (result) {
                                        ClearData();

                                        $.each(result, function (k, Val) {
                                            $.each(Val, function (key, Value) {
                                                localStorage.setItem(key, JSON.stringify(Value));
                                                var data = RenderDatatable(key, Value);
                                                $(dataBody).append(data);
                                            });
                                        });
                                        bootbox.alert('ลบข้อมูลสำเร็จ');
                                        CallDatatable();
                                    }
                                });
                            }
                            else {
                            }
                            $(this).dialog("close");
                        }
                    }
                ]
            });
        }

        function GetRowData(idx) {
            localStorage.setItem("Mode", 2);
            var res = JSON.parse(localStorage.getItem(idx))

            if (res.USDES != "") {
                $("#txt1").val(res.USDES);
                $("#txtarea").val(res.LNGRES);
            }
            else if (res.GLACC != "") {
                $("#txt1").val(res.GLACC);
                $("#txt1").attr('readonly', true);
                $("#txtarea").val(res.GLDES);
            }
            else if (res.InternalOrder != "") {
                $("#txt1").val(res.InternalOrder);
                $("#txt1").attr('readonly', true);
                $("#txtarea").val(res.Description);
            }

            $("#startDate").val(res.VALIDSTD);
            $("#endDate").val(res.VALIDEND);

            var dialog = $("#dialog-message").removeClass('hide').dialog({
                width: screen.width / 100 * 80,
                modal: true,
                title: "<div class='widget-header widget-header-small'> " +
                       "<h4 class='smaller'><i class='ace-icon glyphicon glyphicon-plus'></i> Edit Objective</h4></div>",
                title_html: true,
                buttons: [
                    {
                        text: "Cancel",
                        "class": "btn btn-minier",
                        click: function () {
                            $(this).dialog("close");
                        }
                    },
                    {
                        text: "Edit",
                        "class": "btn btn-primary btn-minier",
                        click: function () {
                            if (ValidateData(localStorage.getItem("Mode"))) {

                                var dataValue = "{'id':'" + (res.USID != '' ? res.USID : 0) +
                                                "', 'title':'" + $("#txt1").val() +
                                                "', 'description':'" + $("#txtarea").val() +
                                                "', 'startDate':'" + $("#startDate").val() +
                                                "', 'endDate':'" + $("#endDate").val() +
                                                "', 'mode':'2'}";
                                $.ajax({
                                    type: "POST",
                                    url: "MasterData.aspx/OnCommandEdit",
                                    data: dataValue,
                                    contentType: 'application/json; charset=utf-8',
                                    dataType: 'json',
                                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                                        alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                                                textStatus + "\n\nError: " + errorThrown);
                                    },
                                    success: function (result) {
                                        if (result.d.Success) {
                                            ClearData();
                                            result = result.d.Data;
                                            console.log(result);
                                            $.each(result, function (key, Value) {
                                                localStorage.setItem(key, JSON.stringify(Value));
                                                var data = RenderDatatable(key, Value);
                                                $(dataBody).append(data);

                                            });
                                            CallDatatable();
                                            bootbox.alert("แก้ไขข้อมูลเรียบร้อยแล้ว");
                                        } else {
                                            bootbox.alert(result.d.Message);
                                        }
                                    }
                                });
                            }
                            else {
                                return;
                            }
                            $(this).dialog("close");
                        }
                    }
                ]
            });

            $("#defaultFocus").focus();
        }

        function addEditing(id, right) {

            bootbox.dialog({
                message: '<h4 style="text-align: center">Do you want to change you data ?</h4>',
                buttons: {
                    yes: {
                        label: "Yes",
                        className: "btn-sm btn-primary",
                        callback: function () {
                            var _right = $('#select' + id + right + '').val();
                            var dataValue = "{'ID':'" + id + "', 'RIGHT':'" + _right + "'}";

                            $.ajax({
                                type: "POST",
                                url: "MasterData.aspx/OnCommandEditGroupMapping",
                                contentType: 'application/json; charset=utf-8',
                                data: dataValue,
                                dataType: 'json',
                                error: function (XMLHttpRequest, textStatus, errorThrown) {
                                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                                            textStatus + "\n\nError: " + errorThrown);
                                },
                                success: function (result) {

                                    optGroupForEditing = "";

                                    var groupName = $('#select' + id + right + ' option:selected').text();
                                    $('#' + id + '_' + right + '').text(groupName);

                                    $('#' + id + '_' + right + '').removeClass('hide');
                                    $('#Edit' + id + '').removeClass('hide');

                                    $('#' + id + '-' + right + '').addClass('hide');
                                    $('#confirm' + id + '').addClass('hide');
                                    $('#cancel' + id + '').addClass('hide');

                                    $('#select' + id + right + '').html(optGroupForEditing);
                                    $('#select' + id + right + '').trigger("chosen:updated");

                                    return;

                                }
                            });
                        }
                    },
                    no: {
                        label: "NO",
                        className: "btn-sm btn-danger"
                    }
                }
            });
        }

        function activeEditing(id) {
            bootbox.dialog({
                message: '<h4 style="text-align: center">Do you want to change you data ?</h4>',
                buttons: {
                    yes: {
                        label: "Yes",
                        className: "btn-sm btn-primary",
                        callback: function () {
                            var dataValue = "{'ID':'" + id + "'}";

                            $.ajax({
                                type: "POST",
                                url: "MasterData.aspx/OnCommandEditingActive",
                                contentType: 'application/json; charset=utf-8',
                                data: dataValue,
                                dataType: 'json',
                                error: function (XMLHttpRequest, textStatus, errorThrown) {
                                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                                            textStatus + "\n\nError: " + errorThrown);
                                },
                                success: function (result) {
                                    try {

                                        $.each(result, function (k, Val) {
                                            $.each(Val, function (key, Value) {
                                                if (Value.Status == "INACTIVE") {
                                                    $('#atv' + id + '').removeClass('btn-info-two');
                                                    $('#atv' + id + '').addClass('btn-danger');

                                                    $('#atv' + id + '').text(Value.Status);
                                                } else if (Value.Status == "ACTIVE") {
                                                    $('#atv' + id + '').removeClass('btn-danger');
                                                    $('#atv' + id + '').addClass('btn-info-two');

                                                    $('#atv' + id + '').text(Value.Status);
                                                }
                                            });
                                        });

                                    } catch (e) {

                                    }

                                    return;

                                }
                            });
                        }
                    },
                    no: {
                        label: "NO",
                        className: "btn-sm btn-danger"
                    }
                }
            });
        }

        function RenderDatatable(key, Value) {
            if (source == 'right') {
                var res = '<tr>'
                        + '<td style="text-align: center;">'
                        + (Value.Status == 'ACTIVE' ? '<button class="btn btn-xs btn-white btn-info-two btn-bold" id="atv' + Value.ID + '" onclick="activeEditing(\'' + Value.ID + '\');return false;">' : '<button class="btn btn-xs btn-white btn-danger btn-bold" id="atv' + Value.ID + '" onclick="activeEditing(\'' + Value.ID + '\');return false;">')
                        + Value.Status
                        + '</button></td>'
                        + '<td>'
                        + Value.UserName
                        + '</td>'
                        + '<td>'
                        + '<span class="" id="' + Value.ID + '_' + Value.GroupID + '">' + Value.Group + '</span>'
                        + '<div class="form-group has-edit hide" id="' + Value.ID + '-' + Value.GroupID + '">'
                        + '<div class="col-sm-12">'
                        + '<select class="chosen-select" id="select' + Value.ID + Value.GroupID + '" style="height: 28px;">'
                        + '</select>'
                        + '</div>'
                        + '</div>'
                        + '</td>'
                        + '<td>'

                        + '<button class="btn btn-xs btn-white btn-default btn-round hide" id="confirm' + Value.ID + '" onclick="addEditing(\'' + Value.ID + '\', \'' + Value.GroupID + '\');return false;">'
						+ '<i class="ace-icon fa fa-check bigger-130 blue" style="margin-top: 5px; margin-left: 2px"></i>'
						+ '</button> '
                        + '<button class="btn btn-xs btn-white btn-default btn-round hide" id="cancel' + Value.ID + '" onclick="cancelEditing(\'' + Value.ID + '\', \'' + Value.ID + '_' + Value.GroupID + '\', \'' + Value.ID + '-' + Value.GroupID + '\', \'' + Value.ID + Value.GroupID + '\');return false;">'
						+ '<i class="ace-icon fa fa-ban bigger-130 red2" style="margin-top: 5px; margin-left: 2px"></i>'
						+ '</button>'

                        + '<button class="btn btn-xs btn-white btn-default btn-round" id="Edit' + Value.ID + '" onclick="initGroupForEditing(\'' + Value.ID + '-' + Value.GroupID + '\');return false;">'
						+ '<i class="ace-icon fa fa-pencil bigger-130 green" style="margin-top: 5px; margin-left: 2px"></i>'
						+ '</button>'

                        + '</td></tr>';

            } else {
                var res = "<tr>"
                    + "<td class='center'>"
                    + "<label class='pos-rel'><input type='checkbox' class='ace' /><span class='lbl'></span></label>"
                    + "</td>"
                    + "<td class=''>" + Value.USID + "</td>"
                    + "<td class=''>" + Value.USDES + "</td>"
                    + "<td class=''>" + Value.LNGRES + "</td>"
                    + "<td class=''>" + Value.GLACC + "</td>"
                    + "<td class=''>" + Value.GLDES + "</td>"
                    + "<td class=''>" + Value.InternalOrder + "</td>"
                    + "<td class=''>" + Value.Description + "</td>"
                    + "<td class=''>" + Value.VALIDSTD + "</td>"
                    + "<td>" + Value.VALIDEND + "</td>"
                    + "<td>"
                    + "<div class='hidden-sm hidden-xs action-buttons'>"
                    + "<a class='green' href='#' id='onEdit" + key + "' onclick='return GetRowData(" + key + ");'><i class='ace-icon fa fa-pencil bigger-130'></i></a>"
                    + "<a class='red' href='#' id='onDelete" + key + "' onclick='return DeleteData(" + key + ");'><i class='ace-icon fa fa-trash-o bigger-130'></i></a>"
                    + "</div>"
                    + "<div class='hidden-md hidden-lg'><div class='inline pos-rel'>"
                    + "<button class='btn btn-minier btn-yellow dropdown-toggle' data-toggle='dropdown' data-position='auto'>"
                    + "<i class='ace-icon fa fa-caret-down icon-only bigger-120'></i></button>"
                    + "<ul class='dropdown-menu dropdown-only-icon dropdown-yellow dropdown-menu-right dropdown-caret dropdown-close'>"
                    + "<li><a href='#' class='tooltip-success' data-rel='tooltip' title='Edit' id='onEdit'" + key + ">"
                    + "<span class='green'><i class='ace-icon fa fa-pencil-square-o bigger-120'></i></span>"
                    + "</a></li>"
                    + "<li>"
                    + "<a href='#' class='tooltip-error' data-rel='tooltip' title='Delete'>"
                    + "<span class='red'><i class='ace-icon fa fa-trash-o bigger-120'></i></span>"
                    + "</a></li></ul></div></div>"
                    + "</td>"
                    + "</tr>";
            }

            return res;
        }

        function ValidateData(e) {
            // -----------------
            // 1 = Add, 2 = Edit
            // -----------------

            if (e == 1 || e == 2) {

                var StartDatePicker = $("#startDate").val();
                var EndDatePicker = $("#endDate").val();

                if ($('#txtarea').val().trim() == "") {
                    bootbox.alert("กรุณาระบุ " + $("#lbl2").html());
                    return false;
                }

                if ($('#txt1').val().trim() == "") {
                    bootbox.alert("กรุณาระบุ " + $("#lbl1").html());
                    return false;
                }

                if (StartDatePicker != "" && EndDatePicker != "") {
                    var start = $("#startDate").val().split("/");
                    var first = new Date(start[2], start[1] - 1, start[0]);
                    var end = $("#endDate").val().split("/");
                    var last = new Date(end[2], end[1] - 1, end[0]);
                    if (first > last) {
                        bootbox.alert('Start Date ต้องน้อยกว่า End Date');
                        return false;
                    }
                }

                if ($("#description").val() == "") {
                    $("#textDesc").text("Please fill Description");
                    var dialog = $("#dialog-desc").removeClass('hide').dialog({
                        width: screen.width / 100 * 30,
                        modal: true,
                        title: "<div class='widget-header widget-header-small' style='color: Orange'> " +
                               "<h4 class='smaller'><i class='ace-icon glyphicon glyphicon-flag'></i> Warning</h4></div>",
                        title_html: true,
                        buttons: [
                            {
                                text: "OK",
                                "class": "btn btn-info btn-minier",
                                click: function () {
                                    $(this).dialog("close");
                                }
                            }
                        ]
                    });
                    return false;
                }
                else if ($("#reason").val() == "") {
                    $("#textDesc").text("Please fill Reason");
                    var dialog = $("#dialog-desc").removeClass('hide').dialog({
                        width: screen.width / 100 * 30,
                        modal: true,
                        title: "<div class='widget-header widget-header-small' style='color: Orange'> " +
                               "<h4 class='smaller'><i class='ace-icon glyphicon glyphicon-flag'></i> Warning</h4></div>",
                        title_html: true,
                        buttons: [
                            {
                                text: "OK",
                                "class": "btn btn-info btn-minier",
                                click: function () {
                                    $(this).dialog("close");
                                }
                            }
                        ]
                    });
                    return false;
                }
                else if ($("#startDate").val() == "") {
                    if (source == 'ino') return true;
                    $("#textDesc").text("กรุณาระบุ Start Date");
                    var dialog = $("#dialog-desc").removeClass('hide').dialog({
                        width: screen.width / 100 * 30,
                        modal: true,
                        title: "<div class='widget-header widget-header-small' style='color: Orange'> " +
                               "<h4 class='smaller'><i class='ace-icon glyphicon glyphicon-flag'></i> Warning</h4></div>",
                        title_html: true,
                        buttons: [
                            {
                                text: "OK",
                                "class": "btn btn-info btn-minier",
                                click: function () {
                                    $(this).dialog("close");
                                }
                            }
                        ]
                    });
                    return false;
                }
                else if ($("#endDate").val() == "") {
                    if (source == 'ino') return true;
                    $("#textDesc").text("กรุณาระบุ End Date");
                    var dialog = $("#dialog-desc").removeClass('hide').dialog({
                        width: screen.width / 100 * 30,
                        modal: true,
                        title: "<div class='widget-header widget-header-small' style='color: Orange'> " +
                               "<h4 class='smaller'><i class='ace-icon glyphicon glyphicon-flag'></i> Warning</h4></div>",
                        title_html: true,
                        buttons: [
                            {
                                text: "OK",
                                "class": "btn btn-info btn-minier",
                                click: function () {
                                    $(this).dialog("close");
                                }
                            }
                        ]
                    });
                    return false;
                }
                //else if (($("#startDate").val().length == 9 ? '0' + $("#startDate").val() : $("#startDate").val()) > $("#endDate").val()) {
                //    $("#textDesc").text("Start date cannot over than End date");
                //    var dialog = $("#dialog-desc").removeClass('hide').dialog({
                //        width: screen.width / 100 * 30,
                //        modal: true,
                //        title: "<div class='widget-header widget-header-small' style='color: Orange'> " +
                //               "<h4 class='smaller'><i class='ace-icon glyphicon glyphicon-flag'></i> Warning</h4></div>",
                //        title_html: true,
                //        buttons: [
                //            {
                //                text: "OK",
                //                "class": "btn btn-info btn-minier",
                //                click: function () {
                //                    $(this).dialog("close");
                //                }
                //            }
                //        ]
                //    });
                //    return false;
                //}

                return true;
            }
            else if (e == 4) {
                return true;
            }
            else {

                return -9;
            }
        }

        function CallDatatable() {
            var oTable1 = $(dataTableMode).dataTable({
                //sdom: 'Bfrtip',
                select: true,
                bAutoWidth: false,
                "aoColumns": tableSetting,
                "aaSorting": []
            });

            //ColVis extension
            var colvis = new $.fn.dataTable.ColVis(oTable1, {
                "buttonText": "<i class='fa fa-search'></i>",
                "aiExclude": [0, 3],
                "bShowAll": true,
                //"bRestore": true,
                "sAlign": "right",
                "fnLabel": function (i, title, th) {
                    return $(th).text();//remove icons, etc
                }
            });

            //style it
            $(colvis.button()).addClass('btn-group').find('button').addClass('btn btn-white btn-info btn-bold')

            //and append it to our table tools btn-group, also add tooltip
            $(colvis.button())
            .prependTo('.tableTools-container .btn-group')
            .attr('title', 'Show/hide columns').tooltip({ container: 'body' });

            //and make the list, buttons and checkboxed Ace-like
            $(colvis.dom.collection)
            .addClass('dropdown-menu dropdown-light dropdown-caret dropdown-caret-right')
            .find('li').wrapInner('<a href="javascript:void(0)" />') //'A' tag is required for better styling
            .find('input[type=checkbox]').addClass('ace').next().addClass('lbl padding-8');

            $('th input[type=checkbox], td input[type=checkbox]').prop('checked', false);

            $('' + dataTableMode + ' > thead > tr > th input[type=checkbox]').eq(0).on('click', function () {
                var th_checked = this.checked;//checkbox inside "TH" table header

                $(this).closest('table').find('tbody > tr').each(function () {
                    var row = this;
                    if (th_checked) tableTools_obj.fnSelect(row);
                    else tableTools_obj.fnDeselect(row);
                });
            });

            $('' + dataTableMode + '').on('click', 'td input[type=checkbox]', function () {
                var row = $(this).closest('tr').get(0);
                if (!this.checked) tableTools_obj.fnSelect(row);
                else tableTools_obj.fnDeselect($(this).closest('tr').get(0));
            });

            $(document).on('click', '' + dataTableMode + ' .dropdown-toggle', function (e) {
                e.stopImmediatePropagation();
                e.stopPropagation();
                e.preventDefault();
            });
        }

        function Callback_Function(result, context) {
            alert('WebMethod was called');
        }

        function AlertError(strerr) {
            bootbox.alert(strerr);
        }

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <input type="hidden" id="Source" />
    <div class="col-sm-12">
        <div class="row">
            <div id="widget-header" class="widget-box">
                <div class="widget-header widget-header-flat">
                    <h4 class="smaller" id="set-header"></h4>
                </div>
                <div class="widget-body">
                    <div class="widget-main" style="margin: 0 5px 0 5px">
                        <div class="row" style="text-align: center;">
                            <div class="col-sm-2" style="text-align: center;">
                                <button type="button" id="id-btn-dialog1" class="btn btn-white btn-info">
                                    <i class="ace-icon glyphicon glyphicon-plus"></i>Add new
                                </button>

                                <%--    Access Dialog   --%>
                                <div id="dialog-access-right" class="hide">
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <div id="accordion" class="accordion-style1 panel-group">
                                                <div class="panel panel-default">
                                                    <div class="panel-heading">
                                                        <h4 class="panel-title">
                                                            <a class="accordion-toggle"
                                                                data-toggle="collapse"
                                                                data-parent="#accordion"
                                                                href="#collapseRight">
                                                                <i class="ace-icon fa fa-angle-down bigger-110"
                                                                    data-icon-hide="ace-icon fa fa-angle-down"
                                                                    data-icon-show="ace-icon fa fa-angle-right"></i>
                                                                Access Right
                                                            </a>
                                                        </h4>
                                                    </div>
                                                    <div class="panel-collapse collapse in" id="collapseRight">
                                                        <div class="panel-body">
                                                            <div class="row">
                                                                <div class="col-sm-12">
                                                                    <div class="col-md-5">
                                                                        <div class="form-group has-warning">
                                                                            <label class="col-sm-3 control-label move-right" for="lblAddUser">ชื่อผู้ใช้งาน</label>
                                                                            <div class="col-sm-9">
                                                                                <span class="block input-icon input-icon-right" id="lblAddUser">
                                                                                    <select class="chosen-select max-width" id="cbxAddUser" data-placeholder="--- กรุณาเลือก ชื่อผู้ใช้งาน ---">
                                                                                    </select>
                                                                                    <i class="ace-icon fa fa-asterisk"></i>
                                                                                </span>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-5">
                                                                        <div class="form-group has-warning">
                                                                            <label class="col-sm-3 control-label move-right" for="lblAddGrp">สิทธ์การใช้งาน</label>
                                                                            <div class="col-sm-9">
                                                                                <span class="block input-icon input-icon-right" id="lblAddGrp">
                                                                                    <select class="chosen-select form-control" id="cbxAddGroup" data-placeholder="--- กรุณาเลือก สิทธ์การใช้งาน ---">
                                                                                    </select>
                                                                                    <i class="ace-icon fa fa-asterisk"></i>
                                                                                </span>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-2">
                                                                        <button type="button" id="btnAddToTb" class="btn btn-white btn-info">
                                                                            <i class="ace-icon glyphicon glyphicon-plus"></i>Add
                                                                        </button>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-12">
                                            <table class="table table-bordered table-hover" id="tUserGroup">
                                                <thead>
                                                    <tr id="">
                                                        <th style="width: 150px;">UserID</th>
                                                        <th>Username</th>
                                                        <th style="width: 150px;">CostCenter</th>
                                                        <th style="width: 100px;">GroupID</th>
                                                        <th style="width: 120px;">Group</th>
                                                        <th style="width: 35px;"></th>
                                                    </tr>
                                                </thead>

                                                <tbody id="iUserGroup">
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>


                                <%--Add new Objective dialogbox--%>
                                <div id="dialog-message" class="hide">
                                    <div class="row">
                                        <div class="col-sm-2">
                                            <span class="pull-right" id="lbl1">Description:</span>
                                        </div>
                                        <div class="col-sm-10">
                                            <input type="text" name="reasonText" id="txt1" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-2">
                                            <span class="pull-right" id="lbl2">Reason:</span>
                                        </div>
                                        <div class="col-sm-10">
                                            <textarea name="reasonText" id="txtarea"></textarea>
                                        </div>
                                    </div>
                                    <div class="row" id="rowData" style="margin-top: 3px;">
                                        <div class="col-sm-2">
                                            <span class="pull-right" id="defaultFocus">Select Date:</span>
                                        </div>
                                        <div class="col-sm-3">
                                            <div class="input-group input-group-sm">
                                                <span class="input-group-addon">
                                                    <i class="ace-icon fa fa-calendar"></i>
                                                    Start Date
                                                </span>
                                                <input type="text" id="startDate" class="form-control" readonly="readonly" />
                                            </div>
                                        </div>
                                        <div class="col-sm-3">
                                            <div class="input-group input-group-sm">
                                                <span class="input-group-addon">
                                                    <i class="ace-icon fa fa-calendar"></i>
                                                    End Date
                                                </span>
                                                <input type="text" id="endDate" class="form-control" readonly="readonly" />
                                            </div>
                                        </div>

                                        <div class="col-sm-4">
                                        </div>
                                    </div>
                                </div>

                                <%--Delete Objective dialogbox--%>
                                <div id="dialog-delete" class="hide">
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <h3>Do you want to delete this objective ?</h3>
                                        </div>
                                    </div>
                                </div>

                                <%--Warning dialogbox--%>
                                <div id="dialog-desc" class="hide">
                                    <div class="row">
                                        <div class="col-sm-12" style="text-align: center; color: orange">
                                            <h3><span id="textDesc"></span></h3>
                                        </div>
                                    </div>
                                </div>
                                <%--------------------------------%>
                            </div>

                            <%-- Make a space --%>
                            <div class="col-sm-2"></div>
                            <%-------------------------------%>

                            <%-- Create Date Picker --%>
                            <div class="col-sm-3 box-start" style="text-align: center;">
                                <div class="input-group input-group-sm">
                                    <span class="input-group-addon">
                                        <i class="ace-icon fa fa-calendar"></i>
                                        Start Date
                                    </span>
                                    <input type="text" id="StartDatePicker" class="form-control" readonly="readonly" />
                                </div>
                            </div>
                            <div class="col-sm-3 box-end" style="text-align: center;">
                                <div class="input-group input-group-sm">
                                    <span class="input-group-addon">
                                        <i class="ace-icon fa fa-calendar"></i>
                                        End Date
                                    </span>
                                    <input type="text" id="EndDatePicker" class="form-control" readonly="readonly" />
                                </div>
                            </div>
                            <%-------------------------------%>

                            <%-- Create Button Search and Clear --%>
                            <div class="col-sm-1 box-search" style="text-align: center;">
                                <button type="button" id="search" class="btn btn-white btn-primary">
                                    <i class="ace-icon glyphicon glyphicon-search"></i>Search
                                </button>
                            </div>
                            <div class="col-sm-1 box-clear" style="text-align: center;">
                                <button type="button" id="clear" class="btn btn-white btn-warning">
                                    <i class="ace-icon glyphicon glyphicon-trash"></i>Clear
                                </button>
                            </div>
                            <%-------------------------------%>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <%-- Create Datatable By ACE(Bootstrap) --%>

    <div class="col-sm-12">
        <div class="row">
            <table id="dynamic-table" class="table table-bordered table-hover">
                <thead>
                    <tr id="iHeader">
                    </tr>
                </thead>

                <tbody id="iBody">
                </tbody>
            </table>

            <table id="access-table" class="table table-bordered table-hover hide">
                <thead>
                    <tr>
                        <th style="width: 100px; text-align: center;">Status</th>
                        <th>User Name</th>
                        <th style="width: 150px; text-align: center;">Group</th>
                        <th style="width: 80px;"></th>
                    </tr>
                </thead>

                <tbody id="accessBody">
                </tbody>
            </table>
        </div>
    </div>

    <%-------------------------------%>
</asp:Content>
