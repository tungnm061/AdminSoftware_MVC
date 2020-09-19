var record = 0;
var totalRow = 0;
$(document).ready(function () {
    $("#FromDateSearch,#ToDateSearch").kendoDatePicker({
        dateInput: true,
        format: "dd/MM/yyyy"
    });

    $('#StatusSearch').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: statusOrder,
        optionLabel: "Tất cả"
    });

    $('#KeySearch').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: keySearchs,
        optionLabel: "Tất cả"
    });

    $("#btnSearchDate").click(function () {
        var fromDate = $("#FromDateSearch").data("kendoDatePicker").value();
        var toDate = $("#ToDateSearch").data("kendoDatePicker").value();
        var keySearch = $("#KeySearch").data("kendoDropDownList").value();
        var statusSearch = $("#StatusSearch").data("kendoDropDownList").value();

        if (fromDate == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn phải chọn ngày bắt đầu tìm kiếm!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (toDate == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn phải chọn ngày kết thúc tìm kiếm!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (toDate < fromDate) {
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
                            url: '/sale/Order/Orders',
                            dataType: "json",
                            data: JSON.stringify({
                                fromDate: fromDate,
                                toDate: toDate,
                                keySearch: keySearch,
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
                    id: "OrderId",
                    fields: {
                        CreateDate: { type: 'date' },
                        FinishDate: { type: 'date' },
                        TotalPrince: { type: 'number' },
                        TotalPriceVND: { type: 'number' }
                    }
                }
            },
            aggregate: [
                { field: "TotalPrince", aggregate: "sum" },
                { field: "TotalPriceVND", aggregate: "sum" }
            ],
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
            fileName: "QuanLyDonHang.xlsx",
            filterable: true,
            allPages: true
        },
        excelExport: function (e) {
            var sheet = e.workbook.sheets[0];
            for (var rowIndex = 1; rowIndex < sheet.rows.length; rowIndex++) {
                var row = sheet.rows[rowIndex];
                row.cells[3].format = "dd/MM/yyyy";
                row.cells[4].format = "dd/MM/yyyy";
                row.cells[6].format = "dd/MM/yyyy";
                row.cells[1].format = "###,###,###";
                if (row.type == "footer") {
                    for (var ci = 0; ci < row.cells.length; ci++) {
                        var cell = row.cells[ci];
                        if (cell.value) {
                            cell.value = kendo.parseFloat(cell.value);
                        }
                    }
                }
            }
            e.workbook.fileName = "QuanLyDonHang-" + $("#FromDateSearch").val() + "-" + $("#ToDateSearch").val() + ".xlsx";
        },
        dataSource: {
            transport: {
                read: function (options) {
                    $.ajax(
                        {
                            type: 'POST',
                            url: '/sale/Order/Orders',
                            dataType: "json",
                            data: JSON.stringify({
                                fromDate: $("#FromDateSearch").data("kendoDatePicker").value(),
                                toDate: $("#ToDateSearch").data("kendoDatePicker").value(),
                                keySearch : $("#KeySearch").data("kendoDropDownList").value(),
                                statusSearch : $("#StatusSearch").data("kendoDropDownList").value()
                            }),
                            contentType: 'application/json;charset=utf-8',
                            success: function (response) {
                                options.success(response);
                                totalRow = response.length;
                            }
                        });
                }
            },
            schema: {
                model: {
                    id: "OrderId",
                    fields: {
                        CreateDate: { type: 'date' },
                        FinishDate: { type: 'date' },
                        TotalPrince: { type: 'number' },
                        StartDate: { type: 'date' }
                    }
                }
            },
            aggregate: [
                { field: "TotalPrince", aggregate: "sum" },
                { field: "OrderCode", aggregate: "count" }

            ],
            pageSize: 9999,
            serverPaging: false,
            serverFiltering: false
        },
        height: 480,
        filterable: true,
        sortable: true,
        //pageable: {
        //    refresh: true
        //},
        selectable: 'row',
        columns: [
            {
                title: "STT",
                template: "#= ++record #",
                width: 80,
                footerTemplate: "Tổng Đơn" 
            },
            {
                field: "OrderCode",
                title: "Mã đơn hàng",
                width: 140,
                footerTemplate: "#=count#"
            },
            {
                field: "TotalPrince",
                title: "Tổng tiền",
                width: 90,
                format: '{0:n2}',
                footerTemplate: "#:sum ? kendo.toString(sum, \"n2\") : 0 #" + " $"
            },
            {
                field: "GmailId",
                title: "Gmail",
                width: 150,
                values : gmails
            },
            {
                field: "StartDate",
                title: "Ngày bắt đầu",
                width: 150,
                format: "{0:dd/MM/yyyy}"
            },
            {
                field: "FinishDate",
                title: "Ngày hoàn thành",
                width: 150,
                format: "{0:dd/MM/yyyy}"
            },
            {
                field: "Description",
                title: "Ghi chú",
                width: 180
            },
            {
                field: "CreateDate",
                title: "Ngày tạo",
                width: 120,
                format: "{0:dd/MM/yyyy}"
            },
            {
                field: "Status",
                title: "Trạng thái",
                width: 120,
                values: statusOrder

            },

        ],
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    });
    $('#btnCreate').click(function () {
        InitWindowModal('/sale/Order/Order', true, "", "", 'Thêm mới đơn hàng', false);
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
        InitWindowModal('/sale/Order/Order?id=' + id, true, "", "", 'Cập nhật đơn hàng', false);
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
                        url: '/sale/Order/Delete',
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