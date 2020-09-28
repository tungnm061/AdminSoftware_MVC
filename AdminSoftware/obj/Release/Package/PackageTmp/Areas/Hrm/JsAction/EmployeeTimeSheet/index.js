var record = 0;
function BuildDateString(date) {
    return (date.getMonth() + 1) + "/" + date.getDate() + "/" + date.getFullYear();
}
$(document).ready(function () {
    $("#btnSearchDate").click(function () {
        var fromDate = $("#FromDateSearch").data("kendoDatePicker").value();
        var toDate = $("#ToDateSearch").data("kendoDatePicker").value();
        window.location.href = '/hrm/EmployeeTimeSheet/Index?fromDate=' + BuildDateString(fromDate) + '&toDate=' + BuildDateString(toDate);
    });
    $("#FromDateSearch,#ToDateSearch").kendoDatePicker({
        dateInput: true,
        format: "dd/MM/yyyy"
    });
    $("#grdMain").kendoGrid({
        toolbar: ["excel"],
        excel: {
            fileName: "bangtonghopcong.xlsx",
            filterable: true,
            allPages: true
        },
        dataSource: {
            transport: {
                read: function (options) {
                    $.ajax(
                    {
                        type: 'POST',
                        url: '/hrm/EmployeeTimeSheet/EmployeeTimeSheets',
                        dataType: "json",
                        data: JSON.stringify({
                             fromDate :  $("#FromDateSearch").data("kendoDatePicker").value(),
                             toDate :    $("#ToDateSearch").data("kendoDatePicker").value()
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
                    id: "EmployeeId",
                    fields: {
                        CheckIn: { type: 'date' },
                        CheckOut: { type: 'date' },
                        TimeSheetDate: { type: 'date' }
                    }
                }
            },
            pageSize: 100,
            serverPaging: false,
            serverFiltering: false,
            group: { field: "DepartmentId" }
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
            e.workbook.fileName = "bangtonghopcong-" + $("#FromDateSearch").val()+ "-" + $("#ToDateSearch").val() + ".xlsx";
        }
    });


   


});