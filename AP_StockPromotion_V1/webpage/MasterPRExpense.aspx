<%@ Page Title="Master PR Expense" Language="C#" MasterPageFile="~/master/MasterPage.Master"
    AutoEventWireup="true" CodeBehind="MasterPRExpense.aspx.cs" Inherits="AP_StockPromotion_V1.webpage.MasterPRExpense" %>


<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        .table > tbody > tr > td {
            vertical-align: middle !important;
        }

        .table > thead {
            background-color: #ffd1d1 !important;
            box-shadow: 1px 2px 5px #888888;
            padding-bottom: 10px !important;
        }

        .dataTable > thead > tr > th.sorting_desc,
        .dataTable > thead > tr > th.sorting_asc {
            color: aqua !important;
            background-repeat: repeat-x;
            background-image: linear-gradient(to bottom, #ffb4b4 0%, #ffb4b4 100%) !important;
        }
    </style>

    <script type="text/javascript">

        $(document).ready(function () {
            InitControl();
            $('#divNavx').html('Management >> Master PR Expense');
            $(document)
                .on('click', '.open-myModal', function (e) {
                    InitControl();
                })
                .on('click', '#add-type', function (e) {
                    if (ValidateBeforeAddType()) {
                        var iRow = $('#type-mapping tr').length;
                        $('#body-type').append(
                            '<tr>' +
                            '<td class="center" id="' + $('#apv-type option:selected').val() + '">' + $('#apv-type option:selected').text() + '</td>' +
                            '<td class="center" id="' + $('#auth-type option:selected').val() + '">' + $('#auth-type option:selected').text() + '</td>' +
                            '<td class="center">' +
                            '<a href="#" id="' + iRow + '" class="btn btn-white btn-danger btn-round on-delete">' +
                            'Delete' +
                            '</a>' +
                            '</td>' +
                            '</tr>'
                        );
                    }
                    else {
                        bootbox.alert("Please select Approve Type and Authority Type");
                    }
                })
                .on('click', '.on-delete', function (e) {
                    var row = e.currentTarget.parentNode.parentNode;
                    row.parentNode.removeChild(row);
                })
                .on('click', '#add-type', function (e) {

                })
                .on('change', '#expense-desc', function (e) {
                    $('#min-price').val(e.target.value.split(':')[1]);
                    $('#max-price').val((e.target.value.split(':')[2] == "0" ? "ขึ้นไป" : e.target.value.split(':')[2]));
                })
                .on('change', '#apv-type', function (e) {
                    if (e.target.value != '') {
                        e.preventDefault();
                        $('#auth-type').prop("disabled", false);
                    }
                    else {
                        e.preventDefault();
                        $('#auth-type').val('');
                        $('#auth-type').prop("disabled", true);
                    }
                })
                .on('click', '#btnSave', function (e) {
                    if (ValidateBeforeSave()) {
                        SaveType();
                    }
                })
                .on('click', '.btnEdit', function (e) {
                    alert(e.currentTarget.id);
                })
                .on('click', '.btnDeletePRE', function (e) {
                    var data = $(this).attr('data-pk');
                    var id = e.currentTarget.id;
                    bootbox.dialog({
                        message: '<h4 style="text-align: center">Do you want to delete you data ?</h4>',
                        buttons: {
                            yes: {
                                label: "Yes",
                                className: "btn-sm btn-primary",
                                callback: function () {
                                    DeleteType(id, data);
                                }
                            },
                            no: {
                                label: "NO",
                                className: "btn-sm btn-danger"
                            }

                        }
                    })

                })
                .on('change', '#costcenter-asc', function (e) {

                });

        });

        function DeleteType(id, CCID) {
            $.ajax({
                type: "POST",
                url: "MasterPRExpense.aspx/DeleteData",
                data: JSON.stringify({ id: id, CCID: CCID }),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                        textStatus + "\n\nError: " + errorThrown);
                },
                success: function (result) {
                    try {
                        if (result.d.Success) {
                            GetTierData();
                        } else {
                            bootbox.alert(result.d.Message, function () {
                                GetTierData();
                            });
                        }
                    } catch (e) {

                    }
                }
            });
        }

        function SaveType() {
            var listDoc = [];
            var listType = [];
            var expenseID, expenseMin, expenseMax, approveTypeID, approveType, authoriryTypeID, authoriryType;
            expenseID = $('#expense-desc option:selected').val().split(':')[0];
            expenseMin = $('#expense-desc option:selected').val().split(':')[1];
            expenseMax = $('#expense-desc option:selected').val().split(':')[2];
            ccID = $('#costcenter-asc').val();

            $('#body-type').find('tr').each(function (i, el) {
                var $tds = $(this).find('td'),
                    approveTypeID = $tds.eq(0)[0].id,
                    approveType = $tds.eq(0).text(),
                    authoriryTypeID = $tds.eq(1)[0].id,
                    authoriryType = $tds.eq(1).text();

                listType.push({
                    'ApvTypeID': approveTypeID,
                    'ApvType': approveType,
                    'AuthTypeID': authoriryTypeID,
                    'AuthType': authoriryType,
                });

            });

            listDoc.push({
                'ExpID': expenseID,
                'ExpMin': expenseMin,
                'ExpMax': expenseMax,
                'ListType': listType,
                'CCID': ccID
            });

            $.ajax({
                type: "POST",
                url: "MasterPRExpense.aspx/SaveData",
                data: JSON.stringify({ data: listDoc }),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                        textStatus + "\n\nError: " + errorThrown);
                },
                success: function (result) {
                    try {
                        if (result.d.Success) {
                            GetTierData();
                        } else {
                            bootbox.alert(result.d.Message, function () {
                                GetTierData();
                            });
                        }
                        SetDefaultPRMemo();
                    } catch (e) {

                    }

                    $('#expense-desc').val('');
                    $('#min-price').val('');
                    $('#max-price').val('');
                    $('#apv-type').val('');
                    $('#auth-type').val('');


                    $('#body-type').empty();
                }
            });
        }

        function numberWithCommas(x) {
            return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        }

        function SetDataTierExpense(res) {
            var data = '<tr><td class="center">';
            data += res.CCID;
            data += '</td>';
            data += '' +
                '<td class="center">' + res.EXPTEXT + '</td>' +
                '<td class="center">' + numberWithCommas(res.EXPMINVALUE) + '</td>' +
                '<td class="center">' + (res.EXPMAXVALUE == "0" ? "ขึ้นไป" : numberWithCommas(res.EXPMAXVALUE)) + '</td>';

            var dataApv = '';
            var dataAuth = '';

            var lenNo = res.DetailsType.length - 1;
            $.each(res.DetailsType, function (k, Val) {
                dataApv += '<p>' + Val.APVTYPENAME + '</p>';
                dataAuth += '<p>' + Val.AUTRTTEXT + '</p>';
                if (lenNo != k) {
                    dataApv += '<h4 class="header blue"></h4>';
                    dataAuth += '<h4 class="header blue"></h4>';
                }
            });

            data += '<td class="center">';
            data += dataApv;
            data += '</td>';

            data += '<td class="center">';
            data += dataAuth;
            data += '</td>';
            data += '<td class="center">' +
                '<a href="#" data-pk="' + res.CCVALUE + '" class="btn btn-white btn-danger btn-round btnDeletePRE" id="' + res.PRID + '">DELETE' +
                '</a>' +
                '</td>' +
                '</tr>';

            return data;
        }

        function ValidateBeforeSave() {
            var resBool = false;
            if ($('#expense-desc option:selected').val() != ""
                && $('#body-type tr').length > 0) {
                resBool = true;
            }
            return resBool;
        }

        function ValidateBeforeAddType() {
            var resBool = false;
            if ($('#apv-type option:selected').val() != ""
                && $('#auth-type option:selected').val() != "") resBool = true;
            return resBool;
        }

        function InitControl() {
            var expData, apvType, authType;
            $('#costcenter-asc').empty();
            $('#expense-desc').empty();
            $('#apv-type').empty();
            $('#auth-type').empty();

            $.ajax({
                type: "POST",
                url: "MasterPRExpense.aspx/GetListOfData",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                        textStatus + "\n\nError: " + errorThrown);
                },
                success: function (result) {
                    try {
                        var expdesc, auth, apvtype;
                        expdesc = '<option value="">-- Expense Desc --</option>';
                        auth = '<option value="">-- Authority --</option>';
                        apvtype = '<option value="">-- Approve Type --</option>';
                        $.each(result, function (k, Val) {

                            for (var i = 0; i < Val.ExpDesc.length; i++) {
                                expdesc += '<option value="' + Val.ExpDesc[i].ID + ':' + Val.ExpDesc[i].EXPMINVALUE + ':' + Val.ExpDesc[i].EXPMAXVALUE + '">' + Val.ExpDesc[i].EXPTEXT + ': ' + numberWithCommas(Val.ExpDesc[i].EXPMINVALUE) + ' - ' + (Val.ExpDesc[i].EXPMAXVALUE == "0" ? "ขึ้นไป" : numberWithCommas(Val.ExpDesc[i].EXPMAXVALUE)) + '</option>';
                            }

                            for (var i = 0; i < Val.ApvType.length; i++) {
                                apvtype += '<option value="' + Val.ApvType[i].ID + '">' + Val.ApvType[i].APVTYPENAME + '</option>';
                            }

                            for (var i = 0; i < Val.Auth.length; i++) {
                                auth += '<option value="' + Val.Auth[i].ID + '">' + Val.Auth[i].AUTRTTEXT + '</option>';
                            }

                        });
                        $('#expense-desc').empty();
                        $('#apv-type').empty();
                        $('#auth-type').empty();
                        $('#expense-desc').append(expdesc);
                        $('#apv-type').append(apvtype);
                        $('#auth-type').append(auth);

                    } catch (e) {

                    }
                }
            });
            GetCostCenter();
            GetTierData();
        }

        function GetCostCenter() {
            if ($('#costcenter-asc').has('option').length > 0) {
                return false;
            }
            $('#costcenter-asc').empty();
            $.ajax({
                type: "POST",
                url: "MasterPRExpense.aspx/GetListOfCostCenter",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                        textStatus + "\n\nError: " + errorThrown);
                },
                success: function (result) {
                    var res = result.d;
                    try {
                        if (res.Success) {
                            var option = '<option value="999777|-- ไม่ระบุโครงการ --"> -- ไม่ระบุโครงการ -- </option>';
                            $("#costcenter-asc").append(option);
                            $(res.Data.ListCostCenter).each(function () {
                                option = "<option value='" + this.CostCenterID + "|" + this.CostCenterName + "'>" + this.CostCenterID + " - " + this.CostCenterName + "</option>";
                                $("#costcenter-asc").append(option);
                            });
                            $('#costcenter-asc').select2();
                        } else {
                            alert("Error :: " + res.Message);
                        }
                    } catch (e) {

                    }
                }
            });
        }

        function GetTierData() {
            $('#authorityBody').empty();
            $.ajax({
                type: "POST",
                url: "MasterPRExpense.aspx/GetTierData",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                        textStatus + "\n\nError: " + errorThrown);
                },
                success: function (result) {
                    try {
                        $.each(result, function (k, Val) {
                            $.each(Val, function (l, res) {
                                var data = SetDataTierExpense(res);
                                $('#authorityBody').append(data);
                            });
                        });
                    } catch (e) {

                    }
                }
            });
        }

        function GetDataBeforeSave() {
            var lstData = [];
            var lstAuth = [];

            lstAuth.push({
                'AuthID': '',
                'ApvtID': '',
            });

            lstData.push({
                'Expt': '',
                'Min': '',
                'Max': '',
                'LstAuth': lstAuth,
            });

            return "";
        }

        function SetDefaultPRMemo() {

        }

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <div>
                        <a href="#" class="btn btn-sm btn-white btn-default btn-round open-myModal" id="" data-toggle="modal"
                            data-target="#iAuthority" data-id="">New Authority Type
                        </a>
                    </div>
                </div>
                <div class="panel-body">
                    <div class="row">
                        <table id="authority-table" class="table display" cellspacing="0" width="100%">
                            <thead>
                                <tr>

                                    <th colspan="3">Tier Expense</th>
                                    <th colspan="3">Authority</th>
                                    <th rowspan="2" style="width: 180px">#</th>
                                </tr>
                                <tr>
                                    <th>Cost Center</th>
                                    <th>Teir Description</th>
                                    <th>Min Price</th>
                                    <th>Max Price</th>
                                    <th>Approve Type</th>
                                    <th>Authority Type</th>

                                </tr>
                            </thead>
                            <tbody id="authorityBody">
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal -->
    <div id="iAuthority" class="modal fade" role="dialog" style="z-index: 999!important;">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">New Authority Type</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <form class="form-horizontal">
                            <div class="form-group">
                                <label for="costcenter-asc" class="col-sm-4 control-label text-right">Cost Center :</label>
                                <div class="col-sm-8">
                                    <select class="form-control hide" id="costcenter-asc" style="width: 100%;">
                                    </select>

                                </div>
                            </div>
                            <br />
                            <div class="form-group">
                                <label for="expense-desc" class="col-sm-4 control-label text-right">Expense Desc :</label>
                                <div class="col-sm-8">
                                    <select class="form-control" id="expense-desc">
                                    </select>

                                </div>
                            </div>
                            <br />
                            <br />
                            <div class="form-group">
                                <label for="" class="col-sm-4 control-label text-right">Tier :</label>
                                <div class="col-sm-3">
                                    <input type="text" class="form-control" id="min-price" placeholder="Min Price" readonly="readonly" />
                                </div>
                                <div class="col-sm-1 center">
                                    ~
                               
                                </div>
                                <div class="col-sm-3">
                                    <input type="text" class="form-control" id="max-price" placeholder="Max Price" readonly="readonly" />
                                </div>
                                <div class="col-sm-1">
                                </div>
                            </div>
                            <br />
                            <h4 class="header blue"></h4>

                            <div class="row">
                                <div class="form-group">
                                    <label for="apv-type" class="col-sm-4 control-label text-right">Approve Type :</label>
                                    <div class="col-sm-6">
                                        <select class="form-control" id="apv-type">
                                        </select>
                                    </div>
                                    <div class="col-sm-2">
                                    </div>
                                </div>
                                <br />
                                <br />
                                <div class="form-group">
                                    <label for="auth-type" class="col-sm-4 control-label text-right">Authority Type :</label>
                                    <div class="col-sm-6">
                                        <select class="form-control" id="auth-type" disabled="disabled">
                                        </select>
                                    </div>
                                    <div class="col-sm-2">
                                        <a href="#" class="btn btn-sm btn-white btn-default btn-round" id="add-type">เพิ่ม
                                        </a>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <br />
                            <table class="table" id="type-mapping">
                                <thead>
                                    <tr>
                                        <th id="apvType">Approve Type</th>
                                        <th id="authType">Authority Type</th>
                                        <th>#</th>
                                    </tr>
                                </thead>
                                <tbody id="body-type">
                                </tbody>
                            </table>
                        </form>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" id="btnClose" class="btn btn-danger" data-dismiss="modal">Close</button>
                    <button type="button" id="btnSave" class="btn btn-default" data-dismiss="modal">Save</button>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
