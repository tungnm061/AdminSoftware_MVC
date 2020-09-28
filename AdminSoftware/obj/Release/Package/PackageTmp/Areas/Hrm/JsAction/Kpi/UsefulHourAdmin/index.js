var record = 0;
function OpenDetail(id) {
    $("#UserId").val(id);
    InitWindowModal('/hrm/UsefulHourAdmin/Employee',false, 1000, 500, "Công việc",false);
}
$(document).ready(function () {
    $("#FromDateSearch,#ToDateSearch").kendoDatePicker({
        dateInput: true,
        format: "dd/MM/yyyy"
    });

    $("#DepartmentId").kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: department,
        optionLabel: "Tất cả",
        filter: 'startswith'
    });
    $("#btnSearchDate").click(function () {
        var departmentId = $("#DepartmentId").data("kendoDropDownList").value();
        var fromDate = $("#FromDateSearch").data("kendoDatePicker").value();
        var toDate = $("#ToDateSearch").data("kendoDatePicker").value();
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
                        url: '/hrm/UsefulHourAdmin/EmployeeUsefulHours',
                        dataType: "json",
                        data: JSON.stringify({
                            fromDate: fromDate,
                            toDate: toDate,
                            departmentId: departmentId
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
                    id: "UserId",
                    fields : {
                        TotalQuantity: { type: "number" },
                        TotalFinish: { type: "number" },
                        ToTalUsefulHours: { type: "number" }
                    }
                }
            },
            pageSize: 100,
            serverPaging: false,
            serverFiltering: false,
            group: { field: 'DepartmentName' }

        });
        var grid = $("#grdMain").data("kendoGrid");
        grid.setDataSource(newDataSource);
    });


    

    $("#grdMain").kendoGrid({
        toolbar: ["excel"],
        excel: {
            fileName: "ThongKeGioLamViec.xlsx",
            filterable: true,
            allPages: true
        },
        height: gridHeight,
        filterable: false,
        sortable: true,
        pageable: false,
        selectable: 'row',
        columns: [
             {
                 field: "DepartmentName",
                 title: "Phòng Ban",
                 width: 200,
                 hidden : true
             },
             {
                 field: "EmployeeCode",
                 title: "Mã Nhân viên",
                 width: 120,
                 template: '<a href="javascript:void(0)" onclick="OpenDetail(\'#=UserId#\')">#=EmployeeCode#</a>'
             },
             {
                 field: "FullName",
                 title: "Họ và tên",
                 width: 120
             },
             {
                 field: "UserName",
                 title: "Tài khoản",
                 width: 120
             }, 
             
             {
                 field: "TotalQuantity",
                 title: "Tổng công việc",
                 width: 120

             },
              {
                  field: "TotalFinish",
                  title: "Công việc hoàn thành",
                  width: 140
              },
               {
                   field: "ToTalUsefulHoursReal",
                   title: "Số giờ hữu ích thực tế",
                   width: 160

               },
                {
                    field: "ToTalUsefulHoursTask",
                    title: "Số giờ hữu ích quy định",
                    width: 160

                }

        ],
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    });

});