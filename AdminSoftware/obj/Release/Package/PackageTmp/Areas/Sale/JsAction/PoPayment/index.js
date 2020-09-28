
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
        optionLabel: "Tất cả"
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
                            url: '/sale/PoPayment/PoPayments',
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
                        CreateDate: { type: 'date' },
                        TradingDate: { type: 'date' },
                        MoneyNumber: { type: 'number' },
                        MoneyNumberVND: { type: 'number' }
                    }
                }
            },
            aggregate: [
                { field: "MoneyNumber", aggregate: "sum" },
                { field: "MoneyNumberVND", aggregate: "sum" }

            ],
            group: {
                field: "TradingMonth", aggregates: [
                    { field: "MoneyNumber", aggregate: "sum" },
                    { field: "MoneyNumberVND", aggregate: "sum" }

                ]
            },
            pageSize: 9999,
            serverPaging: false,
            serverFiltering: false
        });
        var grid = $("#grdMain").data("kendoGrid");
        grid.setDataSource(newDataSource);
    });

    $("#grdMain").kendoGrid({
        toolbar: ["excel"],
        excel: {
            fileName: "QuanLyTienPO.xlsx",
            filterable: true,
            allPages: true
        },
        excelExport: function (e) {
            var sheet = e.workbook.sheets[0];
            for (var rowIndex = 1; rowIndex < sheet.rows.length; rowIndex++) {
                var row = sheet.rows[rowIndex];


                if (row.type == "footer" || row.type == "group-footer") {
                    for (var ci = 0; ci < row.cells.length; ci++) {
                        var cell = row.cells[ci];
                        if (cell.value) {
                            cell.value = kendo.parseFloat(cell.value);
                            cell.format = "###,###,###";
                        }
                    }
                } else {
                    if (row.cells.length > 1) {
                        row.cells[3].format = "#,##0.00";
                        row.cells[4].format = "#,##0.00";
                        row.cells[5].format = "###,###,###";

                        row.cells[1].format = "dd/MM/yyyy";
                        //row.cells[5].format = "dd/MM/yyyy";
                    }
                }
            }
            e.workbook.fileName = "QuanLyTienPO-" + $("#FromDateSearch").val() + "-" + $("#ToDateSearch").val() + ".xlsx";
        },
        dataSource: {
            transport: {
                read: function(options) {
                    $.ajax(
                        {
                            type: 'POST',
                            url: '/sale/PoPayment/PoPayments',
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
                        CreateDate: { type: 'date' },
                        TradingDate: { type: 'date' },
                        MoneyNumber: { type: 'number' },
                        MoneyNumberVND: { type: 'number' }
                    }
                }
            },
            aggregate: [
                { field: "MoneyNumber", aggregate: "sum" },
                { field: "MoneyNumberVND", aggregate: "sum" }

            ],
            group: {
                field: "TradingMonth", aggregates: [
                    { field: "MoneyNumber", aggregate: "sum" },
                    { field: "MoneyNumberVND", aggregate: "sum" }

                ]
            },
            pageSize: 9999,
            serverPaging: false,
            serverFiltering: false
        },
        height: 460,
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
                field: "TradingMonth",
                title: "",
                width: 140,
                hidden: true,
                groupHeaderTemplate: "Tháng : #=value#"

            },
            {
                field: "TradingDate",
                title: "Ngày giao dịch",
                width: 140,
                format: "{0:dd/MM/yyyy}",
                footerTemplate : "Tổng tiền"
            },
            {
                field: "TradingBy",
                title: "Người giao dịch",
                width: 160,
                values: employees
            },
            {
                field: "MoneyNumber",
                title: "Số tiền (USD)",
                width: 200,
                format: "{0:n2}",
                groupFooterTemplate: "#=  kendo.toString(sum , \"n2\") #" + " $",
                footerTemplate: "#:sum ? kendo.toString(sum, \"n2\") : 0 #" + " $"
            },
            {
                field: "RateMoney",
                title: "Tỷ lệ",
                width: 120,
                format: "{0:n0}"
            },
            {
                field: "MoneyNumberVND",
                title: "Số tiền (VND)",
                width: 200,
                format: "{0:n2}",
                groupFooterTemplate: "#=  kendo.toString(sum , \"n2\") #" + " $",
                footerTemplate: "#:sum ? kendo.toString(sum, \"n2\") : 0 #" + " $"
            },
            {
                field: "Status",
                title: "Trạng thái",
                width: 120,
                values: statusPo
            },
            //{
            //    field: "CreateDate",
            //    title: "Ngày tạo",
            //    width: 120,
            //    format: "{0:dd/MM/yyyy}"
            //},
            //{
            //    field: "CreateBy",
            //    title: "Người tạo",
            //    width: 160,
            //    values: employees
            //}

        ],
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    });

    $('#btnCreate').click(function () {
        InitWindowModal('/sale/PoPayment/PoPayment', false, 650, 280, 'Thêm mới PO', false);
    });

    $('#btnEdit').click(function () {
        var id = GetGridRowSelectedKeyValue('#grdMain');
        if (id == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn phải chọn dữ liệu trước khi cập nhật!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        InitWindowModal('/sale/PoPayment/PoPayment?id=' + id, false, 650, 280, 'Cập nhật PO', false);
    });

    $('#btnDelete').click(function () {
        var id = GetGridRowSelectedKeyValue('#grdMain');
        if (id == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn phải chọn dữ liệu cần xóa!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
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
                        url: '/sale/PoPayment/Delete',
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
    });
});