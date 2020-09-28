var checkedIds = {};
function selectRow() {
    var checked = this.checked,
        row = $(this).closest("tr"),
        grid = $("#grdMain").data("kendoGrid"),
        dataItem = grid.dataItem(row);

    checkedIds[dataItem.id] = checked;

    if (checked) {
        //-select the row
        row.addClass("k-state-selected");

        var checkHeader = true;

        $.each(grid.items(), function (index, item) {
            if (!($(item).hasClass("k-state-selected"))) {
                checkHeader = false;
            }
        });

        $("#header-chb")[0].checked = checkHeader;
    } else {
        //-remove selection
        row.removeClass("k-state-selected");
        $("#header-chb")[0].checked = false;
    }
}

function resetSelect() {
    checkedIds = {};
    $("#header-chb")[0].checked = false;
}

function fnSave(status) {
    var checked = [];
    for (var i in checkedIds) {
        if (checkedIds[i]) {
            checked.push(i);
        }
    }

    var textStatus = "xác nhận";
    if (status === 2) {
        textStatus = "trả về";
    }

    $.msgBox({
        title: "Hệ thống",
        type: "confirm",
        content: "Bạn có chắc chắn muốn " + textStatus + " các bản ghi đã chọn ?",
        buttons: [{ value: "Đồng ý" }, { value: "Hủy bỏ" }],
        success: function (result) {
            if (result === "Đồng ý") {
                $('#processing').show();
                $.ajax({
                    type: "POST",
                    url: '/sale/ConfirmPoPayment/Save',
                    data: JSON.stringify({ listId: checked, status: status }),
                    contentType: 'application/json; charset=utf-8',
                    success: function (response) {
                        $('#processing').hide();
                        var type;
                        if (response.Status === 1)
                            type = 'info';
                        else
                            type = 'error';
                        $.msgBox({
                            title: "Hệ thống",
                            type: type,
                            content: response.Message,
                            buttons: [{ value: "Đồng ý" }],
                            success: function (result) {
                                if (result === 'Đồng ý' && response.Status === 1) {
                                    GridCallback('#grdMain');
                                    resetSelect();
                                }
                            } //
                        });
                    },
                    error: function (response) {
                        $('#processing').hide();
                        $.msgBox({
                            title: "Hệ thống",
                            type: "error",
                            content: response.Msg,
                            buttons: [{ value: "Đồng ý" }]
                        });
                    }
                });
            }
        }
    });



}
var record = 0;
$(document).ready(function () {

    $("#FromDateSearch,#ToDateSearch").kendoDatePicker({
        dateInput: true,
        format: "dd/MM/yyyy"
    });

    $('#StatusSearch').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: statusPo,
        optionLabel: "Trạng thái"
    });

    //$("#btnConfirm").prop("disabled", true);
    //$("#btnCancel").prop("disabled", true);

    $("#grdMain[event-dbclick='1']").on("dblclick", "tr.k-state-selected", function () {
        var id = GetGridRowSelectedKeyValue('#grdMain');
        if (id == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn dữ liệu trước khi cập nhật!",
                buttons: [{ value: "Đồng ý" }]
            });
            return false;
        }
        InitWindowModal('/sale/ConfirmPoPayment/PoPayment?id=' + id, false, 650, 265, "Xác nhận giao dịch", false);
        return true;
    });

    $("#btnSearchDate").click(function () {
        var fromDate = $("#FromDateSearch").data("kendoDatePicker").value();
        var toDate = $("#ToDateSearch").data("kendoDatePicker").value();
        var status = $("#StatusSearch").data("kendoDropDownList").value();

        if (fromDate != null && toDate != null && toDate < fromDate) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Ngày bắt đầu không được lớn hơn ngày kết thúc!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        var newDataSource = new kendo.data.DataSource({
            transport: {
                read: function (options) {
                    $.ajax(
                        {
                            type: 'POST',
                            url: '/sale/ConfirmPoPayment/PoPayments',
                            dataType: "json",
                            data: JSON.stringify({
                                fromDate: fromDate,
                                toDate: toDate,
                                status: status
                            }),
                            contentType: 'application/json;charset=utf-8',
                            success: function (response) {
                                options.success(response);
                            }
                        });
                }
            },
            schema: {
                model: {
                    id: "PoPaymentId",
                    fields: {
                        ConfirmDate: { type: 'date' },
                        TradingDate: { type: 'date' },
                        MoneyNumber: { type: 'number' }
                    }
                }
            },
            pageSize: 20,
            serverPaging: false,
            serverFiltering: false
        });
        var grid = $("#grdMain").data("kendoGrid");
        grid.setDataSource(newDataSource);

        var value = status;
        if (value == null || value == undefined) {
            grid.hideColumn(1);
            $("#btnConfirm").prop("disabled", true);
            $("#btnCancel").prop("disabled", true);
        }
        else if (value == 1) {
            grid.showColumn(1);
            $("#btnConfirm").prop("disabled", false);
            $("#btnCancel").prop("disabled", false);

        } else if (value == 2) {
            grid.showColumn(1);
            $("#btnConfirm").prop("disabled", false);
            $("#btnCancel").prop("disabled", true);
        }
        else if (value == 3) {
            grid.showColumn(1);
            $("#btnConfirm").prop("disabled", true);
            $("#btnCancel").prop("disabled", false);
        }
    });

    $('#btnConfirm').on("click", function () {
        fnSave(3);
    });

    $('#btnCancel').on("click", function () {
        fnSave(2);
    });

    $("#grdMain").kendoGrid({
        dataSource: {
            transport: {
                read: function(options) {
                    $.ajax(
                        {
                            type: 'POST',
                            url: '/sale/ConfirmPoPayment/PoPayments',
                            dataType: "json",
                            data: JSON.stringify({
                                fromDate: $("#FromDateSearch").data("kendoDatePicker").value(),
                                toDate: $("#ToDateSearch").data("kendoDatePicker").value(),
                                status: $("#StatusSearch").data("kendoDropDownList").value()
                            }),
                            contentType: 'application/json;charset=utf-8',
                            success: function(response) {
                                options.success(response);
                            }
                        });
                }
            },
            schema: {
                model: {
                    id: "PoPaymentId",
                    fields: {
                        ConfirmDate: { type: 'date' },
                        TradingDate: { type: 'date' },
                        MoneyNumber: { type: 'number' }
                    }
                }
            },
            pageSize: 20,
            serverPaging: false,
            serverFiltering: false
        },
        height: gridHeight,
        filterable: true,
        sortable: true,
        pageable: {
            refresh: true
        },
        selectable: 'row',
        columns: [
            {
                title: "STT",
                template: "#= ++record #",
                width: 50
            },
            {
                title: 'Select All',
                headerTemplate: "<input type='checkbox' id='header-chb' class='k-checkbox header-checkbox'><label class='k-checkbox-label' for='header-chb'></label>",
                template: function (dataItem) {
                    return "<input type='checkbox' id='" + dataItem.PoPaymentId + "' class='k-checkbox row-checkbox'><label class='k-checkbox-label' for='" + dataItem.PoPaymentId + "'></label>";
                },
                width: 40
                //hidden: true
            },
            {
                field: "TradingBy",
                title: "Người giao dịch",
                width: 160,
                values: employees
            },
            {
                field: "TradingDate",
                title: "Ngày giao dịch",
                width: 140,
                format: "{0:dd/MM/yyyy}"
            },
            {
                field: "MoneyNumber",
                title: "Số tiền (USD)",
                width: 200,
                format: "{0:n2}"
            },
            {
                field: "RateMoney",
                title: "Tỷ lệ",
                width: 120,
                format: "{0:n0}"
            },
            {
                field: "Status",
                title: "Trạng thái",
                values: statusPo
            },
            {
                field: "ConfirmDate",
                title: "Ngày xác nhận",
                width: 140,
                format: "{0:dd/MM/yyyy}"
            },
            {
                field: "ConfirmBy",
                title: "Người xác nhận",
                width: 160,
                values: employees
            }

        ],
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        },
        page: function (e) {
            resetSelect();
        }
    });

    $("#grdMain").data("kendoGrid").table.on("click", ".row-checkbox", selectRow);

    $('#header-chb').change(function (ev) {
        var checked = ev.target.checked;
        $('.row-checkbox').each(function (idx, item) {
            if (checked) {
                if (!($(item).closest('tr').is('.k-state-selected'))) {
                    $(item).click();
                }
            } else {
                if ($(item).closest('tr').is('.k-state-selected')) {
                    $(item).click();
                }
            }
        });
    });



});