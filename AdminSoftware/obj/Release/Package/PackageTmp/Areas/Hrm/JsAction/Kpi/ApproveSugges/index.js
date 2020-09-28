var record = 0;
$(document).ready(function () {
    $("#FromDateSearch,#ToDateSearch").kendoDatePicker({
        dateInput: true,
        format: "dd/MM/yyyy",
        max: new Date()
    });
    $("#FromDateSearch").data("kendoDatePicker").enable(false);
    $("#ToDateSearch").data("kendoDatePicker").enable(false);
    $("#btnSearchDate").prop("disabled", true);
    $(document).on("change", "input[type='radio'][name=status]", function () {
        $("#Action").val(this.value);
        var fromDate = $("#FromDateSearch").data("kendoDatePicker").value();
        var toDate = $("#ToDateSearch").data("kendoDatePicker").value();
        var action = this.value;
        var grid = $("#grdMain").data("kendoGrid");
        if (action === '7') {
            $("#FromDateSearch").data("kendoDatePicker").enable(false);
            $("#ToDateSearch").data("kendoDatePicker").enable(false);
            $("#btnSearchDate").prop("disabled", true);
            grid.hideColumn("ConfirmedByName");
            grid.hideColumn("ConfirmedDate");
        }
        if (action === '8') {
            $("#FromDateSearch").data("kendoDatePicker").enable(true);
            $("#ToDateSearch").data("kendoDatePicker").enable(true);
            $("#btnSearchDate").prop("disabled", false);
            grid.showColumn("ConfirmedByName");
            grid.showColumn("ConfirmedDate");
        }
        var newDataSource = new kendo.data.DataSource({
            transport: {
                read: function (options) {
                    $.ajax(
                    {
                        type: 'POST',
                        url: '/hrm/ApproveSugges/SuggesWorks',
                        dataType: "json",
                        data: JSON.stringify({
                            action: action,
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
                    id: "WorkDetailId",
                    fields: {
                        FromDate: { type: 'date' },
                        ToDate: { type: 'date' },
                        Status: { type: 'number' },
                        WorkType: { type: 'number' }
                    }
                }
            },
            pageSize: 100,
            serverPaging: false,
            serverFiltering: false
        });
        grid = $("#grdMain").data("kendoGrid");
        grid.setDataSource(newDataSource);
    });
    $("#btnSearchDate").click(function () {
        var fromDate = $("#FromDateSearch").data("kendoDatePicker").value();
        var toDate = $("#ToDateSearch").data("kendoDatePicker").value();
        var action = $("#Action").val();
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
                        url: '/hrm/ApproveSugges/SuggesWorks',
                        dataType: "json",
                        data: JSON.stringify({
                            action: action,
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
                    id: "WorkDetailId",
                    fields: {
                        FromDate: { type: 'date' },
                        ToDate: { type: 'date' },
                        Status: { type: 'number' },
                        WorkType: { type: 'number' }
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
                        url: '/hrm/ApproveSugges/SuggesWorks',
                        dataType: "json",
                        data: JSON.stringify({
                            action: 7,
                            fromDate: null,
                            toDate: null
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
                    id: "WorkDetailId",
                    fields: {
                        FromDate: { type: 'date' },
                        ToDate: { type: 'date' },
                        Status: { type: 'number' },
                        WorkType: { type: 'number' }
                    }
                }
            },
            pageSize: 100,
            serverPaging: false,
            serverFiltering: false
        },
        height: gridHeight,
        filterable: false,
        sortable: true,
        pageable: false,
        selectable: 'row',
        columns: [
              {
                  field: "EmployeeCode",
                  title: "Mã nhân viên",
                  width: 120
              },
             {
                 field: "CreateByName",
                 title: "Tên nhân viên",
                 width: 120
             },
            {
                field: "WorkType",
                title: "Loại Công Việc",
                width: 160,
                values: window.statusWorkDetail
            },
            {
                field: "TaskCode",
                title: "Mã Công việc",
                width: 120
            },
           {
               field: "TaskName",
               title: "Tên Công việc",
               width: 200
           },
            {
                field: "UsefulHoursTask",
                title: "Giờ hữu ích QĐ",
                width: 120
            },
            {
                field: "FromDate",
                title: "Ngày bắt đầu",
                format: "{0:dd/MM/yyyy}",
                width: 140

            },
            {
                field: "ToDate",
                title: "Ngày hoàn thành",
                format: "{0:dd/MM/yyyy}",
                width: 140
            },
            {
                field: "Description",
                title: "Mô tả công việc",
                width: 300
            },
             {
                 field: "Status",
                 title: "Trạng Thái",
                 width: 170,
                 values: workDetailStatusEnum
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
        InitWindowModal('/hrm/ApproveSugges/SuggesWork?id=' + id, false, 600, 340, 'Cập nhật công việc đề xuât', false);
    });
});