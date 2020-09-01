var record = 0;
$(document).ready(function () {
    $("#FromDateSearch,#ToDateSearch").kendoDatePicker({
        dateInput: true,
        format: "dd/MM/yyyy"
    });
    $("#FromDateSearch").data("kendoDatePicker").enable(false);
    $("#ToDateSearch").data("kendoDatePicker").enable(false);
    $("#btnSearchDate").prop("disabled", true);
    $("#btnRenewal").prop("disabled", true);
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
        GridCallback("#grdMain");

    });
    $(document).on("change", "input[type='radio'][name=status]", function () {
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
        var action = this.value;
        $("#Action").val(action);
        if (action === '2') {
            $("#btnRenewal").prop("disabled", false);
        }
        if (action === '1' || action === '3') {
            $("#btnRenewal").prop("disabled", true);
        }
        if (action === '1') {
            $("#FromDateSearch").data("kendoDatePicker").enable(false);
            $("#ToDateSearch").data("kendoDatePicker").enable(false);
            $("#btnSearchDate").prop("disabled", true);
        }
        if (action === '3' || action === '2') {
            $("#FromDateSearch").data("kendoDatePicker").enable(true);
            $("#ToDateSearch").data("kendoDatePicker").enable(true);
            $("#btnSearchDate").prop("disabled", false);
        }
        GridCallback("#grdMain");
    });

    $("#grdMain").kendoGrid({
        dataSource: {
            transport: {
                read: function (options) {
                    $.ajax(
                    {
                        type: 'POST',
                        url: '/hrm/ConfirmWork/WorkDetails',
                        dataType: "json",
                        data: JSON.stringify({
                            action: $("#Action").val(),
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
                    id: "WorkDetailId",
                    fields: {
                        FromDate: { type: 'date' },
                        ToDate: { type: 'date' },
                        Status: { type: 'number' },
                        WorkType: { type: 'number' },
                        UsefulHoursTask: { type: 'number' }
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
                field: "WorkType",
                title: "Loại Công Việc",
                width:  120,
                values: window.statusWorkDetail
            },
            {
                field: "TaskCode",
                title: "Mã Công việc",
                width: 130
            },
           {
               field: "TaskName",
               title: "Tên Công việc",
               width: 280
           },
            {
                field: "Quantity",
                title: "Số lượng",
                width: 80
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
                width: 120

            },
            {
                field: "ToDate",
                title: "Ngày hoàn thành",
                format: "{0:dd/MM/yyyy}",
                width: 130
            },
            {
                field: "Description",
                title: "Mô tả công việc",
                width: 280
            },
             {
                 field: "Status",
                 title: "Trạng Thái",
                 width: 170,
                 values: window.statusWorkPlanDetail
             }

        ],
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    });
    
    $('#btnEdit').click(function () {
        var grid = $("#grdMain").data("kendoGrid");
        var selectedItem = grid.dataItem(grid.select());
        var id = selectedItem.WorkDetailId;
        var workType = selectedItem.WorkType;
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
        InitWindowModal('/hrm/ConfirmWork/WorkDetail?id=' + id + '&workType=' + workType, false, 800, 550, 'Thực hiện công việc', false);
    });
});