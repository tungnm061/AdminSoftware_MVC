$(document).ready(function () {
    $('#btnDepartment').bind("click", function () {

        InitWindowModal('/Dashboard/DepartmentIndex', true, '', '', "Cơ cấu tổ chức", false);
    });
    //$("#FromDateSearch,#ToDateSearch").kendoDatePicker({
    //    dateInput: true,
    //    format: "dd/MM/yyyy"
    //});

    //$("#EmployeeId").kendoDropDownList({
    //    dataTextField: "text",
    //    dataValueField: "value",
    //    dataSource: employees,
    //    optionLabel: "Chọn nhân viên",
    //    filter: 'contains',
    //    height: 420
    //});
    //$("#btnSearchDate").click(function () {
    //    var employeeId = $("#EmployeeId").data("kendoDropDownList").value();
    //    var fromDate = $("#FromDateSearch").data("kendoDatePicker").value();
    //    var toDate = $("#ToDateSearch").data("kendoDatePicker").value();
    //    if (fromDate == null) {
    //        $.msgBox({
    //            title: "Hệ thống ERP",
    //            type: "error",
    //            content: "Bạn phải chọn ngày bắt đầu tìm kiếm!",
    //            buttons: [{ value: "Đồng ý" }],
    //            success: function () {
    //            }
    //        });
    //        return;
    //    }
    //    if (employeeId == null || employeeId === "") {
    //        $.msgBox({
    //            title: "Hệ thống ERP",
    //            type: "error",
    //            content: "Bạn chưa chọn nhân viên!",
    //            buttons: [{ value: "Đồng ý" }],
    //            success: function () {
    //            }
    //        });
    //        return;
    //    }
    //    if (toDate == null) {
    //        $.msgBox({
    //            title: "Hệ thống ERP",
    //            type: "error",
    //            content: "Bạn phải chọn ngày kết thúc tìm kiếm!",
    //            buttons: [{ value: "Đồng ý" }],
    //            success: function () {
    //            }
    //        });
    //        return;
    //    }
    //    if (toDate < fromDate) {
    //        $.msgBox({
    //            title: "Hệ thống ERP",
    //            type: "error",
    //            content: "Ngày bắt đầu không được lớn hơn ngày kết thúc!",
    //            buttons: [{ value: "Đồng ý" }],
    //            success: function () {
    //            }
    //        });
    //        return;
    //    }

    //    var newDataSource = new kendo.data.DataSource({
    //        transport: {
    //            read: function (options) {
    //                $.ajax(
    //                {
    //                    type: 'POST',
    //                    url: '/Dashboard/EmployeeKpi',
    //                    dataType: "json",
    //                    data: JSON.stringify({
    //                        fromDate: fromDate,
    //                        toDate: toDate,
    //                        employeeId: employeeId
    //                    }),
    //                    contentType: 'application/json;charset=utf-8',
    //                    success: function (response) {
    //                        options.success(response);
    //                    }
    //                });
    //            }
    //        },
    //        schema: {
    //            model: {
    //                id: "EmployeeId",
    //                fields: {
    //                    FactorPoint: { type: "number" },
    //                    ToTalUsefulHours: { type: "number" }
    //                }
    //            }
    //        },
    //        pageSize: 100,
    //        serverPaging: false,
    //        serverFiltering: false

    //    });
    //    var grid = $("#grdMain").data("kendoGrid");
    //    grid.setDataSource(newDataSource);
    //});

    //$("#grdMain").kendoGrid({
    //    height: 150,
    //    filterable: false,
    //    sortable: true,
    //    pageable: false,
    //    selectable: 'row',
    //    columns: [
    //        {
    //            field: "EmployeeCode",
    //            title: "Mã Nhân viên",
    //            width: 120
    //        },
    //        {
    //            field: "FullName",
    //            title: "Họ và tên",
    //            width: 120
    //        },
    //        {
    //            field: "DepartmentName",
    //            title: "Phòng Ban",
    //            width: 200
    //        },
    //        {
    //            field: "TotalUsefulHours",
    //            title: "Thời gian làm việc hữu ích",
    //            width: 160

    //        },
    //        {
    //            field: "FactorPoint",
    //            title: "Hệ số năng suất lao động tháng",
    //            width: 160
    //        }
    //    ],
    //    dataBinding: function () {
    //    }
    //});
});