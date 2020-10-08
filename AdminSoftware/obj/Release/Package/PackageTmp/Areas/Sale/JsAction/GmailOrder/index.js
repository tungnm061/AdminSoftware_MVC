var record = 0;
function BuildDateString(date) {
    return (date.getMonth() + 1) + "/" + 1 + "/" + date.getFullYear();
}
$(document).ready(function () {
    $("#FromDateSearch").kendoDatePicker({
        start: "year",
        depth: "year",
        format: "MM yyyy",
        dateInput: true

    });
    $("#ToDateSearch").kendoDatePicker({
        start: "year",
        depth: "year",
        format: "MM yyyy",
        dateInput: true

    });

    $('#GmailSearch').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: gmails,
        optionLabel: "Chọn gmail"
    });

    $("#btnSearchDate").click(function () {
        var fromDate = $("#FromDateSearch").data("kendoDatePicker").value();
        var gmailId = $("#GmailSearch").data("kendoDropDownList").value();

        window.location.href = '/sale/GmailOrder/Index?fromDate=' + BuildDateString(fromDate) + '&gmailId=' + gmailId;
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
                            url: '/sale/GmailOrder/GmailOrders',
                            dataType: "json",
                            data: JSON.stringify({
                                fromDate: $("#FromDateSearch").data("kendoDatePicker").value(),
                                gmailId: $("#GmailSearch").data("kendoDropDownList").value()
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
                        CancelOrder: { type: 'number' },
                        TotalOrder: { type: 'number' },
                        RefundOrder: { type: 'number' },
                    }
                }
            },
            aggregate: aggregates,
            pageSize: 9999,
            serverPaging: false,
            serverFiltering: false
        },
        height: gridHeight,
        //filterable: true,
        sortable: true,
        //pageable: {
        //    refresh: true
        //},
        selectable: 'row',
        columns: columns,
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        },
        excelExport: function (e) {
            var sheet = e.workbook.sheets[0];
            for (var rowIndex = 1; rowIndex < sheet.rows.length; rowIndex++) {
                var row = sheet.rows[rowIndex];
                row.cells[4].format = "#,##0.00";
                row.cells[5].format = "#,##0.00";
                if (row.type == "footer") {
                    for (var ci = 0; ci < row.cells.length; ci++) {
                        var cell = row.cells[ci];
                        if (cell.value) {
                            cell.value = kendo.parseFloat(cell.value);
                        }
                    }
                }
            }
            e.workbook.fileName = "BangThongKeDonHang-" + $("#FromDateSearch").val() + ".xlsx";
        }
    });

    $('#btnExcel').click(function () {
        InitWindowModal('/sale/GmailOrder/ViewExcel', false, 550, 240, 'Import file Excel', false);
    });

});