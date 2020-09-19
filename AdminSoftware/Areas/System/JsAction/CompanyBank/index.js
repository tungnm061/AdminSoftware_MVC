var record = 0;
$(document).ready(function () {

    $("#FromDateSearch,#ToDateSearch").kendoDatePicker({
        dateInput: true,
        format: "dd/MM/yyyy"
    });

    $('#ExpenseIdSearch').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: expenseTypes,
        optionLabel: "Tất cả"
    });

    $("#btnSearchDate").click(function () {
        var fromDate = $("#FromDateSearch").data("kendoDatePicker").value();
        var toDate = $("#ToDateSearch").data("kendoDatePicker").value();
        var expenseIdSearch = $("#ExpenseIdSearch").data("kendoDropDownList").value();

        //if (fromDate == null) {
        //    $.msgBox({
        //        title: "Hệ thống",
        //        type: "error",
        //        content: "Bạn phải chọn ngày bắt đầu tìm kiếm!",
        //        buttons: [{ value: "Đồng ý" }],
        //        success: function () {
        //        }
        //    });
        //    return;
        //}
        //if (toDate == null) {
        //    $.msgBox({
        //        title: "Hệ thống",
        //        type: "error",
        //        content: "Bạn phải chọn ngày kết thúc tìm kiếm!",
        //        buttons: [{ value: "Đồng ý" }],
        //        success: function () {
        //        }
        //    });
        //    return;
        //}
        if (fromDate != null && toDate !=null && toDate < fromDate) {
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
                                expenseId: expenseIdSearch
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
                        TradingDate: { type: 'date' }
                    }
                }
            },
            aggregate: [
                { field: "MoneyNumberVND", aggregate: "sum" },
                { field: "MoneyNumberUSD", aggregate: "sum" }
            ],
            pageSize: 9999,
            serverPaging: false,
            serverFiltering: false
        });
        var grid = $("#grdMain").data("kendoGrid");
        grid.setDataSource(newDataSource);
    });

    $('#btnDelete').bind("click", function () {
        var id = GetGridRowSelectedKeyValue('#grdMain');
        if (id == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn dữ liệu trước khi sửa!",
                buttons: [{ value: "Đồng ý" }]
            });
            return false;
        }
        $.msgBox({
            title: "Hệ thống",
            type: "confirm",
            content: "Bạn có chắc chắn muốn xóa dữ liệu không?",
            buttons: [{ value: "Đồng ý" }, { value: "Hủy bỏ" }],
            success: function (result) {
                if (result === "Đồng ý") {
                    $('#processing').show();
                    $.ajax({
                        type: 'POST',
                        url: '/system/CompanyBank/Delete',
                        data: JSON.stringify({ id: id }),
                        contentType: 'application/json;charset=utf-8',
                        success: function (response) {
                            $('#processing').hide();
                            if (response.Status === 0) {
                                $.msgBox({
                                    title: "Hệ thống",
                                    type: "error",
                                    content: response.Message,
                                    buttons: [{ value: "Đồng ý" }],
                                    success: function () {
                                    }
                                });
                            } else {
                                $.msgBox({
                                    title: "Hệ thống",
                                    type: "info",
                                    content: response.Message,
                                    buttons: [{ value: "Đồng ý" }],
                                    success: function () {
                                        GridCallback('#grdMain');
                                    }
                                });
                            }
                        }
                    });
                }
            }
        });
        return true;
    });

    $('#btnCreate').bind("click", function () {
        InitWindowModal('/system/CompanyBank/CompanyBank?id=0', false, 600, 370, "Thêm mới giao dịch", false);

    });

    $('#btnEdit').bind("click", function () {
        var id = GetGridRowSelectedKeyValue('#grdMain');
        if (id == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn dữ liệu trước khi sửa!",
                buttons: [{ value: "Đồng ý" }]
            });
            return false;
        }
        InitWindowModal('/system/CompanyBank/CompanyBank?id=' + id, false, 600, 370, "Cập nhật giao dịch", false);
        return true;
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
                                expenseId: $("#ExpenseIdSearch").data("kendoDropDownList").value()
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
                    }
                }
            },
            pageSize: 100,
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
                width: 60
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
                width: 150,
                format: "{0:dd/MM/yyyy}"
            },
            {
                field: "MoneyNumberVND",
                title: "VND",
                format: "{0:n2}",
                width: 150
            },
            {
                field: "MoneyNumberUSD",
                title: "USD",
                format: "{0:n2}",
                width: 150
            },
            {
                field: "ExpenseId",
                title: "Loại giao dịch",
                width: 150,
                values: expenseTypes
            },
            {
                field: "ExpenseText",
                title: "Mô tả",
                width: 150
            },
            //{
            //    field: "Description",
            //    title: "Ghi chú",
            //    minwidth: 200
            //},
            {
                field: "CreateDate",
                title: "Ngày tạo",
                width: 150,
                format: "{0:dd/MM/yyyy}"
            }
        ],
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    });
});