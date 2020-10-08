var record = 0;
//on click of the checkbox:
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
    //$('.row-checkbox').each(function(idx, item) {
    //    if (($(item).closest('tr').is('.k-state-selected'))) {
    //        $(item).click();
    //    }
    //});
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
                    url: '/system/ConfirmCompanyBank/Save',
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

$(document).ready(function () {

    $("#FromDateSearch,#ToDateSearch").kendoDatePicker({
        dateInput: true,
        format: "dd/MM/yyyy"
    });

    $('#ExpenseIdSearch').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: expenseTypes,
        optionLabel: "Loại giao dịch",
        filter: 'contains'

    });


    $('#StatusSearch').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: statusSearch,
        optionLabel: "Trạng thái",
        //change: function (e) {
        //    var value = this.value();
        //}
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
        InitWindowModal('/system/ConfirmCompanyBank/CompanyBank?id=' + id, false, 800, 590, "Xác nhận giao dịch", false);
        return true;
    });

    $("#btnSearchDate").click(function () {
        var fromDate = $("#FromDateSearch").data("kendoDatePicker").value();
        var toDate = $("#ToDateSearch").data("kendoDatePicker").value();
        var expenseIdSearch = $("#ExpenseIdSearch").data("kendoDropDownList").value();
        var statusSearch = $("#StatusSearch").data("kendoDropDownList").value();

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
                            url: '/system/CompanyBank/CompanyBanks',
                            dataType: "json",
                            data: JSON.stringify({
                                fromDate: fromDate,
                                toDate: toDate,
                                expenseId: expenseIdSearch,
                                statusSearch: statusSearch
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
                    id: "CompanyBankId",
                    fields: {
                        CreateDate: { type: 'date' },
                        TradingDate: { type: 'date' },
                        MoneyNumberVND: { type: 'number' },
                        MoneyNumberUSD: { type: 'number' },
                        ConfirmDate : { type: 'date' }
                    }
                }
            },
            pageSize: 20,
            serverPaging: false,
            serverFiltering: false
        });
        var grid = $("#grdMain").data("kendoGrid");
        grid.setDataSource(newDataSource);

        var value = statusSearch;
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
                read: function (options) {
                    $.ajax(
                        {
                            type: 'POST',
                            url: '/system/CompanyBank/CompanyBanks',
                            dataType: "json",
                            data: JSON.stringify({
                                fromDate: $("#FromDateSearch").data("kendoDatePicker").value(),
                                toDate: $("#ToDateSearch").data("kendoDatePicker").value(),
                                expenseId: $("#ExpenseIdSearch").data("kendoDropDownList").value(),
                                statusSearch : $("#StatusSearch").data("kendoDropDownList").value()
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
                    id: "CompanyBankId",
                    fields: {
                        CreateDate: { type: 'date' },
                        TradingDate: { type: 'date' },
                        MoneyNumberVND: { type: 'number' },
                        MoneyNumberUSD: { type: 'number' },
                        ConfirmDate: { type: 'date' }
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
                    return "<input type='checkbox' id='" + dataItem.CompanyBankId + "' class='k-checkbox row-checkbox'><label class='k-checkbox-label' for='" + dataItem.CompanyBankId + "'></label>";
                },
                width: 40
            },
            {
                field: "TradingBy",
                title: "Người giao dịch",
                width: 150,
                values: users
            },
            {
                field: "TradingDate",
                title: "Ngày giao dịch",
                width: 130,
                format: "{0:dd/MM/yyyy}"
            },
            {
                field: "MoneyNumberVND",
                title: "VND",
                format: "{0:n0}",
                width: 120
            },
            {
                field: "MoneyNumberUSD",
                title: "USD",
                format: "{0:n2}",
                width: 100
            },
            {
                field: "ExpenseId",
                title: "Loại giao dịch",
                values: expenseTypes
            },
            //{
            //    field: "ExpenseText",
            //    title: "Mô tả",
            //    width: 150
            //},
            {
                field: "Status",
                title: "Trạng thái",
                width: 130,
                values: statusSearch
            },
            {
                field: "ConfirmBy",
                title: "Người xác nhận",
                width: 150,
                values: users
            },
            {
                field: "ConfirmDate",
                title: "Ngày xác nhận",
                width: 130,
                format: "{0:dd/MM/yyyy}"
            },
            {
                field: "FilePath",
                title: "File xác nhận",
                width: 160,
                template: "#if(FilePath != null){#" + "<a href='#=FilePath#' target='_blank' style='text-align:center'><i class='glyphicon glyphicon-download'></i></a>" + "#}#",
                filterable: false,
                sortable: false
            }
        ],
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        },
        dataBound: function(e) {
    //    var view = this.dataSource.view();
    //    for(var i = 0; i<view.length; i++) {
    //    if (checkedIds[view[i].id]) {
    //        this.tbody.find("tr[data-uid='" + view[i].uid + "']")
    //            .addClass("k-state-selected")
    //            .find(".k-checkbox")
    //            .attr("checked", "checked");
    //    }
    //}
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