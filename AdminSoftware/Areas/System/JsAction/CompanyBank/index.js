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
        optionLabel: "Loại giao dịch"
    });

    $('#SystemSearch').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: departments,
        optionLabel: "Hệ thống"
    });

    $('#StatusSearch').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: statusSearch,
        optionLabel: "Trạng thái"
    });

    $("#btnSearchDate").click(function () {
        var fromDate = $("#FromDateSearch").data("kendoDatePicker").value();
        var toDate = $("#ToDateSearch").data("kendoDatePicker").value();
        var expenseIdSearch = $("#ExpenseIdSearch").data("kendoDropDownList").value();
        var statusSearch = $("#StatusSearch").data("kendoDropDownList").value();
        var systemSearch = $("#SystemSearch").data("kendoDropDownList").value();

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
                                statusSearch: statusSearch,
                                systemSearch: systemSearch
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
            aggregate: [
                { field: "MoneyNumberVND", aggregate: "sum" },
                { field: "MoneyNumberUSD", aggregate: "sum" }
            ],
            group: {
                field: "TradingDate", aggregates: [
                    { field: "MoneyNumberVND", aggregate: "sum" },
                    { field: "MoneyNumberUSD", aggregate: "sum" }
                ]
            },
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
        InitWindowModal('/system/CompanyBank/CompanyBank?id=0', false, 800, 590, "Thêm mới giao dịch", false);

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
        InitWindowModal('/system/CompanyBank/CompanyBank?id=' + id, false, 800, 590, "Cập nhật giao dịch", false);
        return true;
    });

    $("#grdMain").kendoGrid({
        toolbar: ["excel"],
        excel: {
            fileName: "QuanLyThuChi.xlsx",
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
                        row.cells[3].format = "###,###,###";
                        row.cells[2].format = "###,###,###";
                        row.cells[6].format = "dd/MM/yyyy";

                    }
                }
                //if (row.type == "group-header") {
                //    for (var ci = 0; ci < row.cells.length; ci++) {
                //        var cell = row.cells[ci];
                //        if (cell.value) {
                //            cell.value = $(cell.value).text();
                //        }
                //    }
                //}
            }
            e.workbook.fileName = "QuanLyThuChi-" + $("#FromDateSearch").val() + "-" + $("#ToDateSearch").val() + ".xlsx";
        },
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
                                statusSearch : $("#StatusSearch").data("kendoDropDownList").value(),
                                systemSearch : $("#SystemSearch").data("kendoDropDownList").value()
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
            aggregate: [
                { field: "MoneyNumberVND", aggregate: "sum" },
                { field: "MoneyNumberUSD", aggregate: "sum" }
            ],
            group: {
                field: "TradingDate", aggregates: [
                    { field: "MoneyNumberVND", aggregate: "sum" },
                    { field: "MoneyNumberUSD", aggregate: "sum" }
                ] },
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
                format: "{0:dd/MM/yyyy}",
                hidden: true,
                groupHeaderTemplate: "#=kendo.toString(value,\"dd/MM/yyyy\")#"
                //groupHeaderTemplate:"#=kendo.toString(value,\"dd/MM/yyyy\")#</td><td> #=kendo.toString(aggregates.MoneyNumberVND.sum, \"n0\")#</td><td>#=kendo.toString(aggregates.MoneyNumberUSD.sum, \"n2\")#"
                //groupHeaderTemplate: "#=  kendo.toString(value,\"dd/MM/yyyy\") # VND : #=kendo.toString(aggregates.MoneyNumberVND.sum, \"n0\")# USD : #=kendo.toString(aggregates.MoneyNumberUSD.sum, \"n2\")#" 
                //groupFooterTemplate: function (e) {
                //    return "Total: " + e.MoneyNumberVND.sum;
                //}
            },
            {
                field: "MoneyNumberVND",
                title: "VND",
                format: "{0:n0}",
                width: 150,
                footerTemplate: "#:sum ? kendo.toString(sum, \"n0\") : 0 #" + " đ",
                groupFooterTemplate: "#=  kendo.toString(sum , \"n0\") #"


            },
            {
                field: "MoneyNumberUSD",
                title: "USD",
                format: "{0:n2}",
                width: 150,
                footerTemplate: "#:sum ? kendo.toString(sum, \"n2\") : 0 #" + " $",
                groupFooterTemplate: "#=  kendo.toString(sum , \"n2\") #"

            },
            {
                field: "ExpenseId",
                title: "Loại giao dịch",
                width: 150,
                values: expenseTypes
            },
            //{
            //    field: "ExpenseText",
            //    title: "Mô tả",
            //    width: 150
            //},
            {
                field: "CreateDate",
                title: "Ngày tạo",
                width: 150,
                format: "{0:dd/MM/yyyy}"
            },
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
            }
        ],
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
        //,dataBound: function (e) {
        //    var firstCell = e.sender.element.find(".k-grouping-row td:first-child");
        //    firstCell.attr("colspan",3);
        //    var lastCell = e.sender.element.find(".k-grouping-row td:last-child");
        //    lastCell.attr("colspan", 4);

        //}
    });
});