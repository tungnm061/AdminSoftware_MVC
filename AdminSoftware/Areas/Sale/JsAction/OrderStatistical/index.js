var record = 0;
function BuildDateString(date) {
    return (date.getMonth() + 1) + "/" + date.getDate() + "/" + date.getFullYear();
}
$(document).ready(function () {
    $("#btnSearchDate").click(function () {
        var fromDate = $("#FromDateSearch").data("kendoDatePicker").value();
        var toDate = $("#ToDateSearch").data("kendoDatePicker").value();
        window.location.href = '/sale/OrderStatistical/Index?fromDate=' + BuildDateString(fromDate) + '&toDate=' + BuildDateString(toDate);
    });
    $("#FromDateSearch,#ToDateSearch").kendoDatePicker({
        dateInput: true,
        format: "dd/MM/yyyy"
    });
    $("#grdMain").kendoGrid({
        toolbar: ["excel"],
        excel: {
            fileName: "BangThongKeDonHang.xlsx",
            filterable: true,
            allPages: true
        },
        dataSource: {
            transport: {
                read: function (options) {
                    $.ajax(
                        {
                            type: 'POST',
                            url: '/sale/OrderStatistical/Orders',
                            dataType: "json",
                            data: JSON.stringify({
                                fromDate: $("#FromDateSearch").data("kendoDatePicker").value(),
                                toDate: $("#ToDateSearch").data("kendoDatePicker").value()
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
                    id: "GmailId",
                    fields: {
                        DateDay: { type: 'date' },
                        TotalOrderPrice: { type: 'number' },
                        FinishOrderPrice: { type: 'number' },
                        TotalOrder: { type: 'number' },
                        TotalFinish: { type: 'number' },
                        TotalCancel: { type: 'number' }
                    }
                }
            },
            aggregate: aggregates,
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
        columns: columns,
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        },
        excelExport: function (e) {
            var sheet = e.workbook.sheets[0];
            for (var rowIndex = 1; rowIndex < sheet.rows.length; rowIndex++) {
                var row = sheet.rows[rowIndex];
                row.cells[4].format = "###,###,###";
                row.cells[5].format = "###,###,###";
                if (row.type == "footer") {
                    for (var ci = 0; ci < row.cells.length; ci++) {
                        var cell = row.cells[ci];
                        if (cell.value) {
                            cell.value = kendo.parseFloat(cell.value);
                        }
                    }
                }
            }
            e.workbook.fileName = "BangThongKeDonHang-" + $("#FromDateSearch").val() + "-" + $("#ToDateSearch").val() + ".xlsx";
        }
    });





});