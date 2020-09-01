var record = 0;
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
                title: "Hệ thống ERP",
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
                title: "Hệ thống ERP",
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
                title: "Hệ thống ERP",
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
                        url: '/hrm/ViewAssignWork/AssignWorks',
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
                    id: "AssignWorkId",
                    fields: {
                        CreateDate: { type: 'date' },
                        Status: { type: 'number' },
                        FromDate: { type: 'date' },
                        ToDate: { type: 'date' }
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
        dataSource: {
            transport: {
                read: function (options) {
                    $.ajax(
                    {
                        type: 'POST',
                        url: '/hrm/ViewAssignWork/AssignWorks',
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
                    id: "AssignWorkId",
                    fields: {
                        CreateDate: { type: 'date' },
                        Status: { type: 'number' },
                        FromDate: { type: 'date' },
                        ToDate: { type: 'date' }
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
                  field: "TaskCode",
                  title: "Mã công việc",
                  width: 140
              },
              {
                  field: "TaskName",
                  title: "Tên công việc",
                  width: 250
              },
                {
                    field: "Quantity",
                    title: "Số lượng",
                    width: 100
                },
            {
                field: "FromDate",
                title: "Thời gian bắt đầu",
                width: 180,
                format: "{0:dd/MM/yyyy}"

            },
            {
                field: "ToDate",
                title: "Thời gian hoàn thành",
                width: 180,
                format: "{0:dd/MM/yyyy}"

            },
            {
                field: "CreateDate",
                title: "Ngày tạo",
                width: 120,
                format: "{0:dd/MM/yyyy}"
            },
            {
                field: "Description",
                title: "Ghi Chú",
                width: 350
            },
            {
                field: "Status",
                title: "Trạng Thái",
                width: 120,
                values: statusAssignWorks
            }
        ],
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
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
        InitWindowModal('/hrm/ViewAssignWork/AssignWork?id=' + id, false, 680, 360, 'Chi tiết công việc được giao');
    });
});