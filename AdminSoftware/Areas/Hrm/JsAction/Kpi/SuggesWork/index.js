var record = 0;
$(document).ready(function() {
    $("#FromDateSearch,#ToDateSearch").kendoDatePicker({
        dateInput: true,
        format: "dd/MM/yyyy"
    });
    $(document).on("change", "input[type='radio'][name=status]", function() {

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
        GridCallback("#grdMain");

    });

    $("#grdMain").kendoGrid({
        dataSource: {
            transport: {
                read: function(options) {
                    $.ajax(
                    {
                        type: 'POST',
                        url: '/hrm/SuggesWork/SuggesWorks',
                        dataType: "json",
                        data: JSON.stringify({
                            action: $("#Action").val(),
                            fromDate: $("#FromDateSearch").data("kendoDatePicker").value(),
                            toDate: $("#ToDateSearch").data("kendoDatePicker").value()
                        }),
                        contentType: 'application/json;charset=utf-8',
                        success: function(response) {
                            options.success(response);
                        }
                    });
                }
            },
            schema: {
                model: {
                    id: "SuggesWorkId",
                    fields: {
                        CreateDate: { type: 'date' },
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
                width: 50
            },
            {
                field: "TaskCode",
                title: "Mã Công Việc",
                width: 160
            },
            {
                field: "TaskName",
                title: "Tên công việc",
                width: 300
            },
            {
                field: "Quantity",
                title: "Số lượng",
                width: 100
            },
            {
                field: "FromDate",
                title: "Ngày bắt đầu",
                width: 150,
                format: "{0:dd/MM/yyyy}"

            },
            {
                field: "ToDate",
                title: "Ngày hoàn thành",
                width: 160,
                format: "{0:dd/MM/yyyy}"

            },
            {
                field: "Status",
                title: "Trạng thái",
                width: 120,
                values: statusSugges
            },
            {
                field: "Description",
                title: "Ghi chú",
                width: 260
            },
            {
                field: "CreateDate",
                title: "Ngày tạo",
                format: "{0:dd/MM/yyyy}",
                width: 120
            }
        ],
        dataBinding: function() {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    });

    $('#btnCreate').click(function() {
        InitWindowModal('/hrm/SuggesWork/SuggesWork', false, 550, 335, 'Thêm mới công việc phát sinh', false);
    });
    $('#btnSearchDate').click(function()
    {
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
        InitWindowModal('/hrm/SuggesWork/SuggesWork?id=' + id, false, 550, 365, 'Cập nhật công việc phát sinh', false);
    });

    $('#btnDelete').click(function () {
        var id = GetGridRowSelectedKeyValue('#grdMain');
        if (id == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn phải chọn dữ liệu cần xóa!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
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
                        url: '/hrm/SuggesWork/Delete',
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
    });
});