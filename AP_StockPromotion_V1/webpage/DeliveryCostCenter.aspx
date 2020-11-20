<%@ Page Title="เคลียร์เอกสารส่งมอบ(Cost Center)" Language="C#" MasterPageFile="~/master/MasterPage.Master"
    AutoEventWireup="true" CodeBehind="DeliveryCostCenter.aspx.cs" Inherits="AP_StockPromotion_V1.webpage.DeliveryCostCenter" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="server">
    <style>
        .select2-dropdown {
            z-index: 5002 !important;
        }

        .div-nopadding {
            padding: 2px 4px 1px 2px !important;
        }

        .col-setheight {
            height: 25px !important;
        }

        .fix-pageheader {
            margin: 0 0 2px !important;
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
        var totalsQuantity = 0;
        var tblSetting = [
            { "bSortable": false, "sClass": "center" },
            { "bSortable": false, "sClass": "center" },
            { "bSortable": true, "sClass": "center" },
            { "bSortable": true, "sClass": "center", "sType": "date-uk" },
            { "bSortable": true, "sClass": "left" },
            { "bSortable": true, "sClass": "center" },
            { "bSortable": true, "sClass": "center" },
            { "bSortable": true, "sClass": "center" },
            { "bSortable": true, "sClass": "center" }
        ];



        (function (a) {
            a.createModal = function (b) {
                defaults = {
                    title: "Memo Report", message: "", closeButton: true, scrollable: false
                };
                var b = a.extend({}, defaults, b);
                var c = (b.scrollable === true) ? 'style="max-height: 450px;overflow-y: auto;"' : "";
                html = '<div class="modal fade" id="myModal" style="z-index: 5001 !important;">';
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

                    $('#divNavx').html('เคลียร์เอกสารส่งมอบ(CostCenter)');
                    $(".js-example-basic-single").select2({ width: '100%' });
                    $('.js-example-basic-single-ws').select2({ minimumResultsForSearch: Infinity, width: '100%' });

                    SetDatePicker();
                    InitialCbxCostCenterPromotion();
                    InitialCbxItemPromotion();
                    $('#accordion-style').on('click', function (ev) {
                        var target = $('input', ev.target);
                        var which = parseInt(target.val());
                        if (which == 2) $('#accordion').addClass('accordion-style2');
                        else $('#accordion').removeClass('accordion-style2');
                    });

                    $('#btnSearch').click();

                    $.unblockUI();
                }, 1000);
        });

        $(document).ready(function () {
            GenerateDatatable();
        });

        $(document)
            .on('keydown', 'txtAmount', function (e) { })
            .on('click', '#btnClear', function (e) {
                $('#txtDeliveryNo').val('');
                $('#cbxCC').val('0').trigger("change");
                $('#cbxItems').val('0').trigger("change");
                $('#StdDate').val('');
                $('#EndDate').val('');
                $('#cbxDelvStatus').val('0').trigger("change");
            })

            .on('click', '#btnSearch', function (e) {
                var docno = $('#txtDeliveryNo').val();
                var costcenter = $('#cbxCC').val();
                var itemno = $('#cbxItems').val();
                var startdate = $('#StdDate').val();
                var enddate = $('#EndDate').val();
                var DelvStatus = $('#cbxDelvStatus').val();
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
                        InitialDeliveryDetailTable(docno, costcenter, itemno, startdate, enddate, DelvStatus);
                        $.unblockUI();
                    }, 1000);
            })

            .on('click', '.delivery', function (e) {
                var e;
                var strID = [];
                var _dt = $('.delivery-detail').dataTable();
                $("input:checked", _dt.fnGetNodes()).each(function () {
                    e = $(this);
                    strID.push({
                        "ID": $(this).val()
                    });
                });
                if (strID.length > 0) {
                    var ret = LoadDataToDeliveryCostCenter(e, strID);
                    if (ret == false) {
                        bootbox.alert("<center><h4>Something wrong !!<h4></center>");
                    }
                    //return ret;
                }
                else {
                    bootbox.alert("<center><h4>กรุณาเลือกรายการ<h4></center>");
                    //return false;
                }

                $('#deliveryCostCenter').modal('toggle');
            })

            .on('click', '.details', function (e) {

                var index = $(this).attr('data-index');

                var GLID = $("input:hidden[name='GLID_" + index + "']").val();
                var GLN = $("input:hidden[name='GLN_" + index + "']").val();

                $('.GL-ID').val(GLID);
                $('.GL-text').text(GLN);

                $('#txtAmount')[0].value = '';
                LoadDataToDeliveryCostCenter(e, e.currentTarget.childNodes[1].id);
                GetCostCenterData(e.currentTarget.parentElement.parentElement.cells[3].innerText);
                GetGLData(GLID);
                $('#deliveryCostCenter').modal('toggle');

            })
            .on('dblclick', '.addItem', function (e) {
                e.preventDefault();
                $('.addItem').prop('disabled', false);
            })
            .on('click', '.addItem', function (e) {
                var ddd = $(this);
                var e = e;
                ddd.prop('disabled', true);
                bootbox.confirm({
                    title: "",
                    message: "คุณต้องการเคลียร์สินค้าหรือไม่?",
                    buttons: {
                        cancel: {
                            label: '<i class="fa fa-times"></i> Cancel'
                        },
                        confirm: {
                            label: '<i class="fa fa-check"></i> Confirm'
                        }
                    },
                    callback: function (result) {
                        if (result == true) {
                            if (!ValidateBeforeAdd()) return;

                            var amount = 0;
                            var nodes = ddd[0].closest('div.dataheader').children[0].children[0].children;
                            $.each(nodes, function (i, res) {
                                switch (res.classList[0]) {
                                    case 'des_amount':
                                        amount = res.children["0"].childNodes[1].value;
                                        break;
                                }
                            });

                            var newdocno = '';
                            $.each(nodes, function (i, res) {
                                if (res.classList[0] == 'des_quantity') {
                                    if (amount != '') {
                                        $('.detailData').empty();
                                        newdocno = DeliveryAddDetail($('#data_id').val(), amount, $('#data_docHandleDate').val(), nodes, res);

                                        if (newdocno != '') {
                                        }
                                        ddd.prop('disabled', false);
                                    }
                                    else if (parseInt(res.innerText) == 0) {
                                        nodes[4].childNodes[1].childNodes[1].value = '';
                                        bootbox.alert('สินค้าไม่มีใน Stock');
                                        ddd.prop('disabled', false);
                                    }
                                }
                            });
                        }
                    }
                });
            })

            .on('click', '.delItem', function (e) {
                try {
                    var quantity = parseInt($('.dataheader').find('div.des_quantity')[0].innerText);
                    var amount = parseInt(e.currentTarget.parentElement.parentElement.parentElement.childNodes[3].innerText);

                    $('.dataheader').find('div.des_quantity')[0].innerText = quantity + amount;

                } catch (e) {
                    alert(e.message);
                }
                e.currentTarget.closest('div.start').remove()
                DeliveryDelDetail(e.currentTarget.closest('div.delItem').id);
            })

            .on('click', '.delivery-report', function printReport(e) {
                var strID = [];
                var CostCenter = '';
                var isSameCostCenter = true;
                var _dt = $('table.delivery-detail').dataTable();
                $("input:checked", _dt.fnGetNodes()).each(function (e) {

                    if (CostCenter == '' || CostCenter == $(this)[0].parentNode.parentNode.cells[3].innerText.substring(0, 1)) {
                        CostCenter = $(this)[0].parentNode.parentNode.cells[3].innerText.substring(0, 1);
                        isSameCostCenter = true;
                    } else {
                        CostCenter = $(this)[0].parentNode.parentNode.cells[3].innerText.substring(0, 1);
                        isSameCostCenter = false;
                        return;
                    }

                    strID.push({
                        "DelvID": $(this).val()
                    });
                });
                if (!isSameCostCenter) {
                    bootbox.alert('กรุณาเลือก หน่วยงาน/โครงการ เดียวกัน.');
                    return;
                } else {
                    if (strID.length > 0) {
                        GetReportDelivery(strID, CostCenter);
                    }
                }
            })
            .on('click', '.config-costcenter', function (e) {

                $('#costcenter-select').val($('.costcenter-text').text().split(' : ')[0]).trigger('change');

                if ($('.costcenter-data').hasClass('hide')) {
                    $('.confirm-costcenter').removeClass('hide');
                    $('.cancel-costcenter').removeClass('hide');

                    $('.costcenter-data').removeClass('hide');

                    $('.costcenter-text').addClass('hide');
                    $('.config-costcenter').addClass('hide');
                } else {
                    $('.confirm-costcenter').removeClass('hide').addClass('hide');
                    $('.cancel-costcenter').removeClass('hide').addClass('hide');
                    $('.costcenter-data').removeClass('hide').addClass('hide');

                    $('.costcenter-text').removeClass('hide');
                    $('.config-costcenter').removeClass('hide');
                }
            })
            .on('click', '.confirm-costcenter', function (e) {
                bootbox.confirm({
                    message: "คุณต้องการเปลี่ยน Cost Center นี่หรือไม่?",
                    buttons: {
                        confirm: {
                            label: 'Yes',
                            className: 'btn-success'
                        },
                        cancel: {
                            label: 'No',
                            className: 'btn-danger'
                        }
                    },
                    callback: function (result) {
                        if (result == true) {

                            docId = $('#data_id').val();
                            currentCostcenterId = $('.costcenter-data').text().split(' : ')[0];

                            $.ajax({
                                type: "POST",
                                url: "DeliveryCostCenter.aspx/EditCostCenter",
                                contentType: 'application/json; charset=utf-8',
                                data: JSON.stringify({ 'currentCostcenterId': currentCostcenterId, 'docId': docId }),
                                dataType: 'json',
                                error: function (XMLHttpRequest, textStatus, errorThrown) {
                                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                                        textStatus + "\n\nError: " + errorThrown);
                                },
                                success: function (result) {
                                    var ret = result.d;
                                    if (ret[0] == '') {
                                        bootbox.alert('แก้ไข Coscenter/IO สำเร็จ');
                                        $('.costcenter-text').text($('.costcenter-data').text());
                                        if ($('.costcenter-data').hasClass('hide')) {
                                            $('.confirm-costcenter').removeClass('hide');
                                            $('.cancel-costcenter').removeClass('hide');

                                            $('.costcenter-data').removeClass('hide');

                                            $('.costcenter-text').addClass('hide');
                                            $('.config-costcenter').addClass('hide');
                                        } else {
                                            $('.confirm-costcenter').removeClass('hide').addClass('hide');
                                            $('.cancel-costcenter').removeClass('hide').addClass('hide');
                                            $('.costcenter-data').removeClass('hide').addClass('hide');

                                            $('.costcenter-text').removeClass('hide');
                                            $('.config-costcenter').removeClass('hide');
                                        }
                                        $('#btnSearch').click();
                                    }
                                    else {
                                        bootbox.alert(ret[0]);
                                        $('.confirm-costcenter').removeClass('hide').addClass('hide');
                                        $('.cancel-costcenter').removeClass('hide').addClass('hide');
                                        $('.costcenter-data').removeClass('hide').addClass('hide');

                                        $('.costcenter-text').removeClass('hide');
                                        $('.config-costcenter').removeClass('hide');
                                    }
                                }
                            });
                        }
                    }
                });
            })
            .on('click', '.cancel-costcenter', function (e) {
                if ($('.costcenter-data').hasClass('hide')) {
                    $('.confirm-costcenter').removeClass('hide');
                    $('.cancel-costcenter').removeClass('hide');

                    $('.costcenter-data').removeClass('hide');

                    $('.costcenter-text').addClass('hide');
                    $('.config-costcenter').addClass('hide');
                } else {
                    $('.confirm-costcenter').removeClass('hide').addClass('hide');
                    $('.cancel-costcenter').removeClass('hide').addClass('hide');
                    $('.costcenter-data').removeClass('hide').addClass('hide');

                    $('.costcenter-text').removeClass('hide');
                    $('.config-costcenter').removeClass('hide');
                }
            })

            //OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO

            .on('click', '.config-GL', function (e) {

                $('#GL-select').val($('.GL-ID').val());

                if ($('.GL-data').hasClass('hide')) {

                    $('.confirm-GL').removeClass('hide');
                    $('.cancel-GL').removeClass('hide');
                    $('.GL-data').removeClass('hide');
                    $('.GL-text').addClass('hide');
                    $('.config-GL').addClass('hide');
                } else {

                    $('.confirm-GL').removeClass('hide').addClass('hide');
                    $('.cancel-GL').removeClass('hide').addClass('hide');
                    $('.GL-data').removeClass('hide').addClass('hide');
                    $('.GL-text').removeClass('hide');
                    $('.config-GL').removeClass('hide');
                }
            })

            .on('click', '.confirm-GL', function (e) {
                bootbox.confirm({
                    message: "คุณต้องการเปลี่ยน GL นี่หรือไม่?",
                    buttons: {
                        confirm: {
                            label: 'Yes',
                            className: 'btn-success'
                        },
                        cancel: {
                            label: 'No',
                            className: 'btn-danger'
                        }
                    },
                    callback: function (result) {
                        if (result == true) {

                            docId = $('#data_id').val();
                            currentGLId = $('.GL-data').text().split(' : ')[0];

                            $.ajax({
                                type: "POST",
                                url: "DeliveryCostCenter.aspx/EditGL",
                                contentType: 'application/json; charset=utf-8',
                                data: JSON.stringify({ 'currentGLId': currentGLId, 'docId': docId }),
                                dataType: 'json',
                                error: function (XMLHttpRequest, textStatus, errorThrown) {
                                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                                        textStatus + "\n\nError: " + errorThrown);
                                },
                                success: function (result) {
                                    var ret = result.d;
                                    if (ret[0] == '') {
                                        bootbox.alert('แก่ไข GL สำเร็จ');
                                        $('.GL-text').text($('.GL-data').text());
                                        if ($('.GL-data').hasClass('hide')) {
                                            $('.confirm-GL').removeClass('hide');
                                            $('.cancel-GL').removeClass('hide');

                                            $('.GL-data').removeClass('hide');

                                            $('.GL-text').addClass('hide');
                                            $('.config-GL').addClass('hide');
                                        } else {
                                            $('.confirm-GL').removeClass('hide').addClass('hide');
                                            $('.cancel-GL').removeClass('hide').addClass('hide');
                                            $('.GL-data').removeClass('hide').addClass('hide');

                                            $('.GL-text').removeClass('hide');
                                            $('.config-GL').removeClass('hide');
                                        }
                                        $('#btnSearch').click();
                                    }
                                    else {
                                        bootbox.alert(ret[0]);
                                        $('.confirm-GL').removeClass('hide').addClass('hide');
                                        $('.cancel-GL').removeClass('hide').addClass('hide');
                                        $('.GL-data').removeClass('hide').addClass('hide');

                                        $('.GL-text').removeClass('hide');
                                        $('.config-GL').removeClass('hide');
                                    }
                                }
                            });
                        }
                    }
                });
            })

            .on('click', '.cancel-GL', function (e) {
                if ($('.GL-data').hasClass('hide')) {
                    $('.confirm-GL').removeClass('hide');
                    $('.cancel-GL').removeClass('hide');

                    $('.GL-data').removeClass('hide');

                    $('.GL-text').addClass('hide');
                    $('.config-GL').addClass('hide');
                } else {
                    $('.confirm-GL').removeClass('hide').addClass('hide');
                    $('.cancel-GL').removeClass('hide').addClass('hide');
                    $('.GL-data').removeClass('hide').addClass('hide');

                    $('.GL-text').removeClass('hide');
                    $('.config-GL').removeClass('hide');
                }
            })

            //OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO


            .on('click', '#deliveryCostCenter .close', function (e) {
                $('#txtAmount').text('');

                $('.confirm-costcenter').removeClass('hide').addClass('hide');
                $('.cancel-costcenter').removeClass('hide').addClass('hide');
                $('.costcenter-data').removeClass('hide').addClass('hide');

                $('.costcenter-text').removeClass('hide');
                $('.config-costcenter').removeClass('hide');
            });

        function GenerateDatatable() {
            $('.delivery-detail').DataTable({
                sdom: 'Bfrtip',
                select: true,
                bAutoWidth: false,
                "aoColumns": tblSetting,
                "aaSorting": [],
                "bDestroy": true
            });

            $('th input[type=checkbox], td input[type=checkbox]').prop('checked', false);

            $('.delivery-detail > thead > tr > th input[type=checkbox]').eq(0).on('click', function () {
                var th_checked = this.checked;

                $(this).closest('table').find('tbody > tr').each(function () {
                    var row = this;
                    if (th_checked) tableTools_obj.fnSelect(row);
                    else tableTools_obj.fnDeselect(row);
                });
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

        }
        function SetDatePicker() {
            $('#StdDate').datepicker({ dateFormat: "dd/mm/yy" });
            $('#EndDate').datepicker({ dateFormat: "dd/mm/yy" });
            $('#data_docHandleDate').datepicker({ dateFormat: "dd/mm/yy" });
        }
        function InitialCbxCostCenterPromotion() {
            $.ajax({
                type: "POST",
                url: "DeliveryCostCenter.aspx/InitialCbxCostCenter",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                        textStatus + "\n\nError: " + errorThrown);
                },
                success: function (result) {
                    var res = result.d;
                    if (res[0] == '') {
                        var data = '<option value="0">-- ทั้งหมด --</option>';
                        $.each(res[1], function (k, Val) {
                            data += '<option value="' + Val.CCID + '">' + Val.CCID + ' : ' + Val.CCNAME + '</option>'
                        });
                        $('#cbxCC').append(data);
                        $('#cbxCC').select2({
                            //allowClear: true
                        });
                    }
                }
            });
        }
        function InitialCbxItemPromotion() {
            $.ajax({
                type: "POST",
                url: "DeliveryCostCenter.aspx/InitialCbxItemPromotion",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                        textStatus + "\n\nError: " + errorThrown);
                },
                success: function (result) {
                    var res = result.d;
                    if (res[0] == '') {
                        var data = '<option value="0">-- ทั้งหมด --</option>';
                        $.each(res[1], function (k, Val) {
                            data += '<option value="' + Val.ITEMID + '">' + Val.ITEMID + ' : ' + Val.ITEMNAME + '</option>'
                        });
                        $('#cbxItems').append(data);
                        $('#cbxItems').select2({
                            //allowClear: true
                        });
                    }
                }
            });
        }
        function InitialDeliveryDetailTable(docno, costcenter, itemno, startdate, enddate, DelvStatus) {
            $.ajax({
                type: "POST",
                url: "DeliveryCostCenter.aspx/OnCommandSearch",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ docno, costcenter, itemno, startdate, enddate, DelvStatus }),
                dataType: 'json',
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                        textStatus + "\n\nError: " + errorThrown);
                },
                success: function (result) {
                    var res = result.d;
                    if (res[0] == '') {

                        if ($.fn.dataTable.isDataTable('.delivery-detail')) {
                            table = $('table.delivery-detail').DataTable({
                                retrieve: true,
                                paging: false
                            });
                            table.destroy();
                            $('#delvdetailbody').empty();
                        }

                        var i = 1;
                        $.each(res[1], function (k, Val) {
                            $('table.delivery-detail').dataTable().fnAddData([
                                '<td style="text-align: center">' +
                                '    <div class="action-buttons details" data-index=' + i + '>' +
                                '        <a class="red" href="#" id="' + Val.ID + '">' +
                                '            <i class="ace-icon fa fa-pencil-square-o bigger-130"></i>' +
                                '        </a>' +
                                '    </div>' +
                                '   <input type="hidden" name="GLID_' + i.toString() + '" value="' + Val.GLID + '">' +
                                '   <input type="hidden" name="GLN_' + i.toString() + '" value="' + Val.GLN + '">' +
                                '</td>',
                                '<td style="text-align: center">' +
                                '    <input type="checkbox" class="' + (Val.StatusId == 1 || Val.StatusId == 3 ? 'hide' : '') + '" name="name" value="' + Val.ID + '" />' +
                                '</td>',
                                '<td style="text-align: center">' + Val.DocNo + '</td>',
                                '<td style="text-align: center">' + Val.CostCenterCode + '</td>',
                                '<td>' + Val.CostCenterName + '</td>',
                                '<td style="text-align: center">' + Val.ItemNo + '</td>',
                                '<td>' + Val.ItemName + '</td>',
                                '<td style="text-align: center">' + Val.Quantity + '</td>',
                                '<td style="text-align: center" ><span class="hide" id="' + Val.StatusId + '"></span>' + Val.Status + '</td>'
                            ]);
                            i++;
                        });

                        GenerateDatatable();
                    }
                    else {
                        bootbox.alert(res[0]);
                    }
                }
            });
        }

        function LoadDataToDeliveryCostCenter(e, id) {
            $('.detailData').empty();
            GetDeliveryCostCenterDetail(id);

            $('#data_id').val(id);
            $('#data_docId').val(e.currentTarget.parentElement.parentElement.childNodes[2].innerText);
            $('#data_docStatus').val(e.currentTarget.parentElement.parentElement.childNodes[8].innerText);
            $('#data_docHandleBy').val($('#lbUserId').text());
            var _date = new Date();
            $('#data_docHandleDate').val(moment(_date).format('DD/MM/YYYY'));

            $('div.des_docno')[0].innerText = e.currentTarget.parentElement.parentElement.childNodes[2].innerText;
            $('div.des_itemno')[0].innerText = e.currentTarget.parentElement.parentElement.childNodes[5].innerText

            var item = '';
            var itemname = e.currentTarget.parentElement.parentElement.childNodes[6].innerText.split('-');
            if (itemname.length > 2) {
                item = itemname[1] + ' - ' + itemname[2];
            } else if (itemname.length > 1) {
                item = itemname[1];
            } else {
                item = itemname[0];
            }
            $('div.des_itemname')[0].innerText = item;

            var quantity = fnGetRecQuantity(id);
            $('div.des_quantity')[0].innerText = quantity;
            $('.costcenter-text').text(e.currentTarget.parentElement.parentElement.cells[3].innerText + ' : ' + e.currentTarget.parentElement.parentElement.cells[4].innerText);
        }
        function fnGetRecQuantity(id) {
            var quantity = '';
            $.ajax({
                type: "POST",
                url: "DeliveryCostCenter.aspx/GetRecQuantity",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ 'id': id }),
                dataType: 'json',
                async: false,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                        textStatus + "\n\nError: " + errorThrown);
                },
                success: function (result) {
                    var ret = result.d;
                    if (ret[0] == '') {
                        quantity = ret[1];
                    }
                }
            });
            return quantity;
        }
        function DeliveryAddDetail(id, amount, pstingdate, nodes, results) {
            $('#deliveryCostCenter').attr("disabled", true);
            $('#deliveryCostCenter').fadeTo("fast", 0.33);

            var newdocno = '';
            var empcode = $('#lbUserId').text();

            $('#txtAmount').text('');
            $.blockUI({
                css: {
                    border: 'none',
                    baseZ: 19900,
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
                    $.ajax({
                        type: "POST",
                        url: "DeliveryCostCenter.aspx/DeliveryAddDetail",
                        contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify({ 'id': id, 'amount': amount, 'pstingdate': pstingdate, 'empcode': empcode }),
                        dataType: 'json',
                        async: false,
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                                textStatus + "\n\nError: " + errorThrown);
                        },
                        success: function (result) {
                            var ret = result.d;
                            if (ret[0] == '') {
                                $.each(nodes, function (i, res) {
                                    switch (res.classList[0]) {
                                        case 'des_docno':
                                            newdocno = ret[1];
                                            break;
                                        case 'des_amount':
                                            res.children["0"].childNodes[1].value = '';
                                            break;
                                    }
                                });

                                if (ret[2] == 'SERIAL') {
                                    results.innerText = parseInt(results.innerText) - 1;
                                } else {
                                    results.innerText = parseInt(results.innerText) - amount;
                                }
                            }
                            else {
                                newdocno = 'NULL';
                                bootbox.alert(ret[0], function () {
                                    $('#txtAmount').text('');
                                });
                            }

                            GetDeliveryCostCenterDetail(id);
                            $('#deliveryCostCenter').attr("disabled", false);
                            $('#deliveryCostCenter').fadeTo("fast", 1);
                            $('#btnSearch').click();
                            return newdocno;
                        }
                    });

                    $.unblockUI();
                }, 1000);
        }
        function GetDeliveryCostCenterDetail(id) {
            $('#deliveryCostCenter').attr("disabled", true);
            $('#deliveryCostCenter').fadeTo("fast", 0.33);
            //alert(id);
            $.blockUI({
                css: {
                    border: 'none',
                    baseZ: 19900,
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
                    $.ajax({
                        type: "POST",
                        url: "DeliveryCostCenter.aspx/GetDeliveryCostCenterDetail",
                        contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify({ 'id': id }),
                        dataType: 'json',
                        async: false,
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                                textStatus + "\n\nError: " + errorThrown);
                        },
                        success: function (result) {
                            var ret = result.d;
                            if (ret[0] == '') {

                                $.each(ret[1], function (i, res) {
                                    var str = '<div class="start col-sm-12 col-setheight" style="border-bottom: 1px dotted #e2e2e2;">'
                                        + '<div class="col-sm-1"></div>'
                                        + '<div class="col-sm-11">'
                                        + '<form class="form-inline">';
                                    str += '<div class="col-sm-2">' + res.DOCNO + '</div>';
                                    str += '<div class="col-sm-1">' + res.ITEMNO + '</div>';
                                    str += '<div class="col-sm-5">' + res.ITEMNAME + '</div>';
                                    str += '<div class="col-sm-1 center">' + res.QUANTITY + '</div>';
                                    str += '<div class="col-sm-2 center">' + res.STATUS + '</div>';
                                    str += '<label class="inline">'
                                        + '<div class="col-sm-1 center">'
                                        + '<div class="action-buttons ' + (res.STATUS == 'เสร็จสิ้น' ? 'hide' : '') + ' delItem" id="' + res.ID + '">'
                                        + '<a class="red" href="#" id="">'
                                        + '<i class="ace-icon fa fa-minus bigger-130"></i>'
                                        + '</a>'
                                        + '</div>'
                                        + '</div>'
                                        + '</label>'
                                        + '</form>'
                                        + '</div>'
                                        + '</div>';
                                    $('.detailData').append(str);
                                });

                                $('#deliveryCostCenter').attr("disabled", false);
                                $('#deliveryCostCenter').fadeTo("fast", 1);

                            }
                            else {
                                $('#deliveryCostCenter').fadeTo("fast", 1);
                                bootbox.alert(ret[0]);
                            }
                            $('#btnSearch').click();
                        }
                    });

                    $.unblockUI();
                }, 1000);
        }
        function DeliveryDelDetail(id) {
            $('#deliveryCostCenter').attr("disabled", true);
            $('#deliveryCostCenter').fadeTo("fast", 0.33);

            var empcode = $('#lbUserId').text();
            $.blockUI({
                css: {
                    border: 'none',
                    baseZ: 19900,
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
                    $.ajax({
                        type: "POST",
                        url: "DeliveryCostCenter.aspx/DeliveryDelDetail",
                        contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify({ 'id': id }),
                        dataType: 'json',
                        async: false,
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                                textStatus + "\n\nError: " + errorThrown);
                        },
                        success: function (result) {
                            var ret = result.d;
                            if (ret[0] == '' && ret[1] == true) {
                                $('#deliveryCostCenter').attr("disabled", false);
                                $('#deliveryCostCenter').fadeTo("fast", 1);
                            }
                            else {
                                $('#deliveryCostCenter').fadeTo("fast", 1);
                                bootbox.alert(ret[0]);
                            }
                            $('#btnSearch').click();
                        }
                    });

                    $.unblockUI();
                }, 1000);
        }
        function ValidateBeforeAdd() {
            if ($('#data_docHandleDate').val() == '') {
                bootbox.alert('กรุณาระบุ Posting Date');
                return false;
            }
            return true;
        }
        function GetReportDelivery(ListOfID, CostCenter) {
            $.blockUI({
                css: {
                    border: 'none',
                    baseZ: 19900,
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
                    $.ajax({
                        type: "POST",
                        url: "DeliveryCostCenter.aspx/DeliveryGetReport",
                        contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify({ 'ListOfID': ListOfID, 'CostCenter': CostCenter }),
                        dataType: 'json',
                        async: false,
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                                textStatus + "\n\nError: " + errorThrown);
                        },
                        success: function (result) {
                            var ret = result.d;
                            if (ret[0] == '' && ret[1] != '') {
                                pdf_link = ret[1];
                                var iframe = '<div class="iframe-container"><iframe src="' + pdf_link + '"></iframe></div>'
                                var pdf_link_length = pdf_link.split('/').length - 1;
                                $.createModal({
                                    title: pdf_link.split('/')[pdf_link_length],
                                    message: iframe,
                                    closeButton: true,
                                    scrollable: false
                                });
                            }
                            else {
                                bootbox.alert(ret[0]);
                            }
                        }
                    });
                    $.unblockUI();
                }, 1000);
        }
        function GetCostCenterData(currentCostcenterId) {
            $.ajax({
                type: "POST",
                url: "DeliveryCostCenter.aspx/GetCostCenterData",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                        textStatus + "\n\nError: " + errorThrown);
                },
                success: function (result) {
                    var ret = result.d;
                    if (ret[0] == '') {
                        var result = '';
                        $.each(ret[1], function (k, Val) {
                            result += '<option value="' + Val.CostCenterID + '">' + Val.CostCenterID + ' : ' + Val.CostCenterName + '</option>';
                        });
                        $('#costcenter-select').append(result);

                        $('#costcenter-select').val(currentCostcenterId);

                        $('#costcenter-select').select2({
                            containerCssClass: 'costcenter-data hide',
                        });
                    }
                    else {
                        bootbox.alert(ret[0]);
                    }
                    $('#btnSearch').click();
                }
            });
        }

        function GetGLData(currentGLId) {
            $.ajax({
                type: "POST",
                url: "DeliveryCostCenter.aspx/GetGLData",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " +
                        textStatus + "\n\nError: " + errorThrown);
                },
                success: function (result) {
                    var ret = result.d;
                    if (ret[0] == '') {
                        result = '';
                        $.each(ret[1], function (k, Val) {
                            result += '<option value="' + Val.GLID + '">' + Val.GLID + ' : ' + Val.GLN + '</option>';
                        });
                        $('#GL-select').append(result);

                        $('#GL-select').val(currentGLId);

                        $('#GL-select').select2({
                            containerCssClass: 'GL-data hide',
                        });
                    }
                    else {
                        bootbox.alert(ret[0]);
                    }
                    $('#btnSearch').click();
                }
            });
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row">
        <div id="accordion" class="accordion-style1 panel-group">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion" href="#collapseOne">
                            <i class="ace-icon fa fa-angle-down bigger-110" data-icon-hide="ace-icon fa fa-angle-down" data-icon-show="ace-icon fa fa-angle-right"></i>
                            &nbsp;รายการส่งมอบสินค้าโปรโมชั่น(CostCenter)
                        </a>
                    </h4>
                </div>

                <div class="panel-collapse collapse in" id="collapseOne">
                    <div class="panel-body">
                        <div class="col-sm-12" style="padding-bottom: 5px">
                            <div class="col-sm-2 div-caption">เลขที่เอกสารส่งมอบ</div>
                            <div class="col-sm-3">
                                <input type="text" id="txtDeliveryNo" placeholder="เลขที่เอกสาร" />
                            </div>
                            <div class="col-sm-2 div-caption">โครงการ</div>
                            <div class="col-sm-3">
                                <select id="cbxCC" class="hide" style="width: 100%">
                                </select>
                            </div>
                            <div class="col-sm-2 div-caption"></div>
                        </div>
                        <div class="col-sm-12" style="padding-bottom: 5px">
                            <div class="col-sm-2 div-caption">สินค้าโปรโมชั่น</div>
                            <div class="col-sm-3">
                                <select id="cbxItems" class="hide" style="width: 100%">
                                </select>
                            </div>
                            <div class="col-sm-2 div-caption">วันที่ส่งมอบ</div>
                            <div class="col-sm-3">
                                <div class="col-sm-5" style="padding-left: 0; padding-right: 0;">
                                    <input type="text" id="StdDate" value="" style="width: 100%" placeholder="Start Date" />
                                </div>
                                <div class="col-sm-2" style="text-align: center;">-</div>
                                <div class="col-sm-5" style="padding-left: 0; padding-right: 0;">
                                    <input type="text" id="EndDate" value="" style="width: 100%" placeholder="End Date" />
                                </div>
                            </div>
                            <div class="col-sm-2 div-caption"></div>
                        </div>

                        <div class="col-sm-12" style="padding-bottom: 5px">
                            <div class="col-sm-2 div-caption">สถานะ</div>
                            <div class="col-sm-2">
                                <select id="cbxDelvStatus" style="width: 100%">
                                    <option value="0" selected="selected">-- ทั้งหมด --</option>
                                    <option value="1">รายการใหม่</option>
                                    <option value="2">รอบันทึกบัญชี</option>
                                    <option value="3">เสร็จสิ้น</option>
                                </select>
                            </div>
                            <div class="col-sm-8"></div>
                        </div>

                        <div class="col-sm-12" style="text-align: center;">
                            <a href="#" class="btn btn-white btn-info btn-sm" id="btnSearch" onclick="return false;">Search</a>
                            &nbsp;
                           
                            <a href="#" class="btn btn-white btn-info btn-sm" id="btnClear" onclick="return false;">Clear</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-sm-3" style="text-align: left">
            <%--<a href="#" class="btn btn-sm btn-white btn-info delivery" onclick="return false;">ส่งมอบสินค้า(CostCenter)</a>--%>
        </div>
        <div class="col-sm-offset-6 col-sm-3" style="text-align: right">
            <a href="#" class="btn btn-sm btn-white btn-info delivery-report" onclick="return false;">แสดงรายงาน</a>
        </div>
    </div>

    <div class="row">
        <table id="" class="delivery-detail table table-hover" cellspacing="0" width="100%">
            <thead>
                <tr>
                    <th style="width: 20px"></th>
                    <th style="width: 20px">
                        <label class="pos-rel">
                            <input type="checkbox" class="ace" />
                        </label>
                    </th>
                    <th style="width: 100px">เลขที่เอกสาร</th>
                    <th style="width: 100px">โครงการ/หน่วยงาน</th>
                    <th style="width: 200px">ชื่อโครงการ/หน่วยงาน</th>
                    <th style="width: 100px">เลขที่สินค้า</th>
                    <th>ชื่อสินค้า</th>
                    <th style="width: 75px; text-align: center">จำนวน</th>
                    <th style="width: 150px; text-align: center">สถานะ</th>
                </tr>
            </thead>
            <tbody id="delvdetailbody">
            </tbody>
        </table>
    </div>

    <div class="modal fade" id="deliveryCostCenter" role="dialog" aria-labelledby="deliveryCostCenter-label">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="deliveryCostCenter-label">เคลียร์เอกสาร (Cost Center)</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-sm-12">
                            <span id="data_id" class="hide"></span>
                            <div class="col-sm-1"></div>
                            <label id="" class="col-sm-2" style="text-align: right">เลขที่เอกสาร</label>
                            <input type="text" class="col-sm-2" name="docId" id="data_docId" value="" disabled />
                            <label id="" class="col-sm-2" style="text-align: right">สถานะ</label>
                            <input type="text" class="col-sm-2" name="docStatus" id="data_docStatus" value="" disabled />

                        </div>
                        <div class="col-sm-12" style="margin-top: 3px;">
                            <div class="col-sm-1"></div>
                            <label id="" class="col-sm-2" style="text-align: right">ผู้ดำเนินการ</label>
                            <input type="text" class="col-sm-2" name="docHandleBy" id="data_docHandleBy" value="" disabled />
                            <label id="" class="col-sm-2" style="text-align: right">วันที่</label>
                            <input type="text" class="col-sm-2" name="docHandleDate" id="data_docHandleDate" value="" placeholder="Posting Date" />
                        </div>

                        <div class="col-sm-12" style="margin-top: 3px;">
                            <div class="col-sm-1"></div>
                            <label class="col-sm-2" style="text-align: right">เบิกให้โครงการ</label>
                            <div style="text-align: left">
                                <input type="hidden" name="GL_ID" class="GL-ID" value="" />

                                <%--<input type="text" class="GL-text" name="GL_ID" value="" disabled />--%>

                                <span class="GL-text">XXX</span>

                                <label class="inline">
                                    <select id="GL-select" class="hide" style="width: 100%">
                                    </select>
                                </label>

                                <label class="inline">
                                    <div class="action-buttons">
                                        <a class="config-GL" style="width: 90%" href="#">
                                            <i class="ace-icon fa fa-pencil bigger-130"></i>
                                        </a>
                                    </div>
                                </label>
                                <label class="inline">
                                    <div class="action-buttons">
                                        <a class="green confirm-GL hide" href="#">
                                            <i class="ace-icon fa fa-check bigger-130"></i>
                                        </a>
                                    </div>
                                </label>
                                <label class="inline">
                                    <div class="action-buttons">
                                        <a class="red cancel-GL hide" href="#">
                                            <i class="ace-icon fa fa-times bigger-130"></i>
                                        </a>
                                    </div>
                                </label>
                            </div>
                        </div>
                    </div>
                </div>

                <h4 class="page-header fix-pageheader">รายการรอการเคลียร์เอกสาร</h4>
                <br />
                <div class="row col-setheight">
                    <div class="col-sm-12">
                        <form class="form-inline">
                            <div class="col-sm-1 center"><b>Doc No</b></div>
                            <div class="col-sm-2 center"><b>Item No</b></div>
                            <div class="col-sm-2 center"><b>Item Name</b></div>
                            <div class="col-sm-4 center"><b>Cost Center/IO</b></div>
                            <div class="col-sm-1 center"><b>Quantity</b></div>
                            <div class="col-sm-1 center"><b>Amount/Serial</b></div>
                            <div class="col-sm-1"></div>
                        </form>
                    </div>
                </div>
                <h4 style="border-bottom: 1px dotted #e2e2e2;"></h4>
                <div class="row descData" style="margin-top: 2px">
                    <div class="dataheader col-sm-12 col-setheight">
                        <div class="col-sm-12">
                            <form class="form-inline">
                                <div class="des_docno col-sm-1 center"></div>
                                <div class="des_itemno col-sm-2"></div>
                                <div class="des_itemname col-sm-2"></div>

                                <div class="des_costcenter col-sm-4 center">
                                    <span class="costcenter-text" style="width: 90%">XXX</span>
                                    <label class="inline">
                                        <select id="costcenter-select" class="hide" style="width: 100%; min-width: 170px;">
                                        </select>
                                    </label>

                                    <label class="inline">
                                        <div class="action-buttons">
                                            <a class="config-costcenter" style="width: 90%" href="#">
                                                <i class="ace-icon fa fa-pencil bigger-130"></i>
                                            </a>
                                        </div>
                                    </label>
                                    <label class="inline">
                                        <div class="action-buttons">
                                            <a class="green confirm-costcenter hide" href="#">
                                                <i class="ace-icon fa fa-check bigger-130"></i>
                                            </a>
                                        </div>
                                    </label>
                                    <label class="inline">
                                        <div class="action-buttons">
                                            <a class="red cancel-costcenter hide" href="#">
                                                <i class="ace-icon fa fa-times bigger-130"></i>
                                            </a>
                                        </div>
                                    </label>
                                </div>

                                <div class="des_quantity col-sm-1 center"></div>
                                <label class="des_amount inline">
                                    <div class="input col-sm-1 center">
                                        <input type="text" name="txtAmount" value="" id="txtAmount" style="width: 100px; height: 22px; text-align: center;"
                                            onkeydown="return (event.keyCode!=13);" />
                                    </div>
                                </label>
                                <label class="inline">
                                    <div class="col-sm-1 center">
                                        <div class="action-buttons addItem">
                                            <a class="blue" href="#" id="">
                                                <i class="ace-icon fa fa-plus bigger-130"></i>
                                            </a>
                                        </div>
                                    </div>
                                </label>
                            </form>
                        </div>
                    </div>
                </div>
                <h4 style="border-bottom: 1px dotted #e2e2e2;"></h4>
                <br />
                <h4 class="page-header fix-pageheader">รายการรอบันทึกบัญชี</h4>
                <div class="row">
                    <div class="start col-sm-12 col-setheight" id="del1" style="border-bottom: 1px dotted #e2e2e2;">
                        <div class="col-sm-1"></div>
                        <div class="col-sm-11">
                            <form class="form-inline">
                                <div class="col-sm-2"><b>Doc No</b></div>
                                <div class="col-sm-1"><b>Item No</b></div>
                                <div class="col-sm-5"><b>Item Name</b></div>
                                <div class="col-sm-1 center"><b>Quantity</b></div>
                                <div class="col-sm-2 center"><b>Status</b></div>
                                <div class="col-sm-1 center"></div>
                            </form>
                        </div>
                    </div>
                </div>
                <div class="row detailData">
                </div>
                <br />
            </div>
            <div class="modal-footer">
            </div>
        </div>
    </div>
    </div>

</asp:Content>
