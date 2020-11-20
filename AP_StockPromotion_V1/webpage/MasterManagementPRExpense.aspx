<%@ Page Title="Management PR Expense" Language="C#" MasterPageFile="~/master/MasterPage.Master"
    AutoEventWireup="true" CodeBehind="MasterManagementPRExpense.aspx.cs" Inherits="AP_StockPromotion_V1.webpage.MasterManagementPRExpense" %>



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

        .right {
            text-align: right;
        }

        .left {
            text-align: left;
        }
    </style>

    <script type="text/javascript">

        $(document).ready(function () {
            GetListOfCostCenter();
            InitialTable();
            InitControl();
            $('#divNavx').html('Management >> Managment PR');
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
            .on('change', '#expense', function (e) {
                e.preventDefault();
                if (e.target.value != '') {
                    $('#apvName').prop("disabled", false);
                    $('#signName').prop("disabled", false);

                    var PRID = $('#expense').val().split(':')[0];
                    var CCID = $('#costcenter-asc').val();
                    InitAppvAndSign(PRID, CCID);
                } else {
                    $('#apvName').prop("disabled", true);
                    $('#signName').prop("disabled", true);
                }
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
                } else {
                    bootbox.alert('Please select all field');
                }
            })
            .on('click', '.btnEdit', function (e) {
                alert(e.currentTarget.id);
            })
            .on('click', '.btnDelete', function (e) {
                DeleteType(e.currentTarget.id, $(this).attr('data-pk'))
                InitialTable();
                InitControl();
            })
            .on('change', '#costcenter-asc', function (e) {
                GetTierDataByCostCenter($('#costcenter-asc').val());
            })
            ;

        });

        function InitialTable() {
            $.ajax({
                type: "POST",
                url: "MasterManagementPRExpense.aspx/GetPRMemoData",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                            textStatus + "\n\nError: " + errorThrown);
                },
                success: function (result) {
                    try {
                        $('#authorityBody').empty();
                        $.each(result, function (k, Val) {
                            $.each(Val, function (l, res) {
                                var data = SetDataTable(res);
                                $('#authorityBody').append(data);
                            });
                        });
                    } catch (e) {

                    }
                }
            });
        }

        function numberWithCommas(x) {
            return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        }

        function InitAppvAndSign(PRID, CCID) {

            $.ajax({
                type: "POST",
                url: "MasterManagementPRExpense.aspx/GetListMemoData",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ PRID: PRID, CCID: CCID }),
                dataType: 'json',
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                            textStatus + "\n\nError: " + errorThrown);
                },
                success: function (result) {
                    try {

                        var apvName, signName;
                        auth = '<option value="">-- Authority --</option>';
                        apvtype = '<option value="">-- Approver --</option>';
                        $.each(result, function (k, Val) {

                            for (var i = 0; i < Val.ListApv.length; i++) {
                                apvtype += '<option value="' + Val.ListApv[i].UserID + '">' + Val.ListApv[i].FullName + '</option>';
                            }

                            for (var i = 0; i < Val.ListSign.length; i++) {
                                auth += '<option value="' + Val.ListSign[i].UserID + '">' + Val.ListSign[i].FullName + '</option>';
                            }
                        });
                        $('#apvName').empty();
                        $('#signName').empty();
                        $('#apvName').append(apvtype);
                        $('#signName').append(auth);

                    } catch (e) {

                    }
                }
            });
        }

        function DeleteType(id, CCID) {
            $.ajax({
                type: "POST",
                url: "MasterManagementPRExpense.aspx/DeleteData",
                data: JSON.stringify({ id: id, CCID: CCID }),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                            textStatus + "\n\nError: " + errorThrown);
                },
                success: function (result) {
                    try {
                        if (result) {
                            GetTierData();
                        }
                    } catch (e) {

                    }
                }
            });
        }

        function SaveType() {

            $.ajax({
                type: "POST",
                url: "MasterManagementPRExpense.aspx/SaveData",
                data: JSON.stringify({
                    expense: $('#expense').val(),
                    apvID: $('#apvName').val(),
                    signID: ($('#signName').val() == '' ? 0 : $('#signName').val()),
                    CCID: $('#costcenter-asc').val()
                }),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                            textStatus + "\n\nError: " + errorThrown);
                },
                success: function (result) {
                    try {
                        if (result.d.Success) {
                            InitialTable();
                        } else {
                            bootbox.alert(result.d.Message, function () {
                                InitialTable();
                            });
                        }
                    } catch (e) {

                    }
                }
            });
        }

        function SetDataTable(res) {
            var data = '<tr>' +
                            '<td class="center">' + res.EXPTEXT + '</td>' +
                            '<td class="right">' + numberWithCommas(res.EXPMINVALUE) + '</td>' +
                            '<td class="right">' + (res.EXPMAXVALUE == "0" ? "ขึ้นไป" : numberWithCommas(res.EXPMAXVALUE)) + '</td>' +
                            '<td class="left">' + res.MainSign + '</td>' +
                            '<td class="left">' + res.SubSign + '</td>' +
                            '<td class="left">' + res.CCTEXT + '</td>' +
                            '<td class="center">' +
                            '<a href="#" id="' + res.PREXPID + '" data-pk="' + res.CCID + '" class="btn btn-white btn-danger btn-round btnDelete">Delete</a>' +
                            '</td>';

            return data;
        }

        function ValidateBeforeSave() {
            var resBool = false;
            if ($('#expense').val() != '' && $('#apvName').val() != '') {

                resBool = true;
            }else {
                return false;
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
            $('#expense').empty();
            $('#apvName').empty();
            $('#signName').empty();

            $.ajax({
                type: "POST",
                url: "MasterManagementPRExpense.aspx/GetExpenseDesc",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                            textStatus + "\n\nError: " + errorThrown);
                },
                success: function (result) {
                    try {

                        var expense, apvName, signName;
                        expdesc = '<option value="">-- Expense Desc --</option>';
                        auth = '<option value="">-- Authority --</option>';
                        apvtype = '<option value="">-- Approve Type --</option>';
                        $.each(result, function (k, Val) {

                            for (var i = 0; i < Val.length; i++) {
                                expdesc += '<option value="' + Val[i].ID + ':' + Val[i].EXPMINVALUE + ':' + Val[i].EXPMAXVALUE + '">' + Val[i].EXPTEXT + ': ' + numberWithCommas(Val[i].EXPMINVALUE) + ' - ' + (Val[i].EXPMAXVALUE == "0" ? "ขึ้นไป" : numberWithCommas(Val[i].EXPMAXVALUE)) + '</option>';
                            }

                        });
                        $('#expense').empty();
                        $('#apvName').empty();
                        $('#signName').empty();
                        $('#expense').append(expdesc);
                        $('#apvName').append(apvtype);
                        $('#signName').append(auth);

                    } catch (e) {

                    }
                }
            });

        }

        function GetTierData() {
            $('#authorityBody').empty();

            InitialTable();
            InitControl();

        }

        function GetListOfCostCenter() {
            if ($('#costcenter-asc').has('option').length > 0) {
                return false;
            }
            $.ajax({
                type: "POST",
                url: "MasterManagementPRExpense.aspx/GetListOfCostCenter",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                            textStatus + "\n\nError: " + errorThrown);
                },
                success: function (result) {
                    try {
                        var res = result.d;
                        if (res.Success) {
                            $(res.Data).each(function () {
                                var option = "<option value='" + this.CostCenterID + "|" + this.CostCenterName + "'>" + this.CostCenterID + " - " + this.CostCenterName + "</option>";
                                $("#costcenter-asc").append(option);
                            });
                            $("#costcenter-asc").select2({
                                placeholder: {
                                    id: '-1',
                                    text: "--Select Cost Center--"
                                },
                                allowClear: true
                            });
                            $("#costcenter-asc").select2("val", "-1");
                        } else {
                            alert(res.Message);
                        }
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

        function GetApprover() {
            var CostCenter = $("#costcenter-asc").val();
            var Tier = $("#expense").val();
            $.ajax({
                type: "POST",
                url: "MasterManagementPRExpense.aspx/GetApprover",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ CostCenter: CostCenter, Tier: Tier }),
                dataType: 'json',
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                            textStatus + "\n\nError: " + errorThrown);
                },
                success: function (result) {
                    try {
                        var res = result.d;
                        if (res.Success) {
                            $(res.Data).each(function () {
                                //var option = "<option value='" + this.ID + "'>" + this.EXPTEXT + "  " + this.EXPMINVALUE + " - " + this.EXPMAXVALUE + "</option>";
                                //$("#expense").append(option);
                            });
                            $("#apvName").select2({
                                placeholder: {
                                    id: '-1',
                                    text: "--Select Approver--"
                                },
                                allowClear: true
                            });
                            $("#apvName").select2("val", "-1");
                        } else {
                            alert(res.Message);
                        }
                    } catch (e) {

                    }
                }
            });
        }

        function GetTierDataByCostCenter(val) {
            var CostCenter = $("#costcenter-asc").val();
            $("#expense").empty();
            $.ajax({
                type: "POST",
                url: "MasterManagementPRExpense.aspx/GetTierDataByCostCenter",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ CostCenter: CostCenter }),
                dataType: 'json',
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                            textStatus + "\n\nError: " + errorThrown);
                },
                success: function (result) {
                    try {
                        var res = result.d;
                        if (res.Success) {
                            $(res.Data).each(function (i) {
                                console.log(res.Data[i].EXPMAXVALUE);
                                var option = "<option value='" + res.Data[i].ID + "'>" + res.Data[i].EXPTEXT + "  " + numberWithCommas(res.Data[i].EXPMINVALUE) + " - " + (res.Data[i].EXPMAXVALUE == "0" || res.Data[i].EXPMAXVALUE == null ? "ขึ้นไป" : numberWithCommas(res.Data[i].EXPMAXVALUE)) + "</option>";
                                $("#expense").append(option);
                            });
                            $("#expense").select2({
                                placeholder: {
                                    id: '-1',
                                    text: "--Select Tier--"
                                },
                                allowClear: true
                            });
                            $("#expense").select2("val", "-1");
                        } else {
                            alert(res.Message);
                        }
                    } catch (e) {

                    }
                }
            });
        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <div style="width:100%">
                        <a href="#" class="btn btn-sm btn-white btn-default btn-round open-myModal" id="" data-toggle="modal"
                            data-target="#iAuthority" data-id="">New Authority Type
                        </a>
                        <asp:Button ID="btnGetPRFromSAP" runat="server" Text="ดึงข้อมูล PR จาก SAP"  
                            class="btn btn-sm btn-white btn-default btn-round" style="float:right" OnClick="btnGetPRFromSAP_Click"/>

                        <%--<asp:HyperLink ID="HyperLink1" runat="server" 
                            class="btn btn-sm btn-white btn-default btn-round" style="float:right">ดึงข้อมูล PR จาก SAP
                        </asp:HyperLink>--%>
                    </div>
    
                </div>
                <div class="panel-body">
                    <div class="row">
                        <table id="authority-table" class="table display" cellspacing="0" width="100%">
                            <thead>
                                    <tr>
                                        <th colspan="6">Tier Expense</th>
                                        <th rowspan="3">#</th>
                                    </tr>
                                    <tr>
                                        <th>Teir Description</th>
                                        <th>Min Price</th>
                                        <th>Max Price</th>
                                        <th>Main Approve Name</th>
                                        <th>Sub Approve Name</th>
                                        <th>Cost Center</th>
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
    <div id="iAuthority" class="modal fade" role="dialog" style="z-index: 999 !important;">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">New Authority Management</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <form class="form-horizontal">
                            <div class="form-group">
                                <input type="hidden" id="PRID" value="0" />
                                <label for="costcenter-asc" class="col-sm-4 control-label text-right">Cost Center</label>
                                <div class="col-sm-8">
                                    <select class="form-control hide" id="costcenter-asc" data-placeholder="-- Select Cost Center --" style="width: 100% !important;">
                                    </select>
                                </div>
                            </div>
                            <br />
                            <div class="form-group">
                                <label for="expense-desc" class="col-sm-4 control-label text-right">วงเงิน</label>
                                <div class="col-sm-6">
                                    <select class="form-control hide" id="expense" style="width: 100% !important;">
                                    </select>
                                </div>
                                <div class="col-sm-2"></div>
                            </div>
                            <br />
                            <br />

                            <h4 class="header blue"></h4>

                            <div class="row">
                                <div class="form-group">
                                    <label for="apv-type" class="col-sm-4 control-label text-right">ผู้อนุมัติ</label>
                                    <div class="col-sm-6">
                                        <select class="form-control" id="apvName" disabled="disabled">
                                        </select>
                                    </div>
                                    <div class="col-sm-2"></div>
                                </div>
                                <br />
                                <br />
                                <div class="form-group">
                                    <label for="auth-type" class="col-sm-4 control-label text-right">ลงนามร่วม</label>
                                    <div class="col-sm-6">
                                        <select class="form-control" id="signName" disabled="disabled">
                                        </select>
                                    </div>
                                    <div class="col-sm-2"></div>
                                </div>
                            </div>
                            <br />
                            <br />
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
