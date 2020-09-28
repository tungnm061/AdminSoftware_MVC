﻿var record = 0;
function OpenDetail(id) {
    $("#UserId").val(id);
    InitWindowModal('/hrm/StatisticalComplainDepartment/ComplainDetail', false, 1000, 500, "Danh sách phản ánh", false);
}
$(document).ready(function () {
    $("#FromDateSearch,#ToDateSearch").kendoDatePicker({
        dateInput: true,
        format: "dd/MM/yyyy",
        max: new Date()
    });
    $("#btnSearchDate").click(function () {
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
                        url: '/hrm/StatisticalComplainDepartment/StatisticalComplains',
                        dataType: "json",
                        data: JSON.stringify({
                            fromDate: fromDate,
                            toDate: toDate
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
                    fields: {
                        TotalQuantity: { type: "number" },
                        TotalFinish: { type: "number" },
                        ToTalUsefulHours: { type: "number" }
                    }
                }
            },
            pageSize: 100,
            serverPaging: false,
            serverFiltering: false

        });
        var grid = $("#grdMain").data("kendoGrid");
        grid.setDataSource(newDataSource);
    });




    $("#grdMain").kendoGrid({
        toolbar: ["excel"],
        excel: {
            fileName: "ThongKePhanAnh.xlsx",
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
                 field: "TotalQuantity",
                 title: "Tổng số phản ánh",
                 width: 140

             },
              {
                  field: "TotalFinish",
                  title: "Phản ánh đúng",
                  width: 140
              },
              {
                  field: "RatingPoint",
                  title: "Điểm",
                  width: 100

              },
               {
                   field: "Rating",
                   title: "Xếp loại",
                   width: 140

               }

        ],
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    });

});