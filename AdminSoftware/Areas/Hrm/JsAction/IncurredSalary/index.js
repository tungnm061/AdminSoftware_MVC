var record = 0;
$(document).ready(function () {
    $('#FromDate,#ToDate').kendoDatePicker({
        max: new Date(),
        dateInput: true,
        format:'{0:dd/MM/yyyy}'
    });
    $("#grdMain").kendoGrid({
        dataSource: {
            transport: {
                read: function (options) {
                    $.ajax(
                    {
                        type: 'POST',
                        url: '/hrm/IncurredSalary/IncurredSalaries',
                        dataType: "json",
                        data: JSON.stringify({
                            fromDate: $('#FromDate').data('kendoDatePicker').value(),
                            toDate: $('#ToDate').data('kendoDatePicker').value()
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
                    id: "IncurredSalaryId",
                    fields: {
                        CreateDate:{type:'date'},
                        SubmitDate: { type: 'date'},
                        Amount: { type: 'number' }
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
                field: "Title",
                title: "Lý do",
                width: 200
            },
            {
                field: "EmployeeCode",
                title: "Mã nhân viên",
                width:150
            },
            {
                field: "FullName",
                title: "Tên nhân viên",
                width: 200
            },
            {
                field: "Amount",
                title: "Số tiền",
                width: 150,
                format: '{0:###,###}'
            },
            {
                field: "SubmitDate",
                title: "Kỳ trả",
                width: 150,
                format: '{0:dd/MM/yyyy}'
            },
            {
                field: "CreateDate",
                title: "Ngày tạo",
                width: 150,
                format:'{0:dd/MM/yyyy}'
            },
            {
                field: "CreateBy",
                title: "Người tạo",
                width: 150,
                values:users
            }
        ],
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    });
    $('#btnSearch').click(function () {
        var fromDate = $('#FromDate').data('kendoDatePicker').value();
        var toDate = $('#ToDate').data('kendoDatePicker').value();
        if (fromDate == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn phải chọn ngày bắt đầu!",
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
                content: "Bạn phải chọn ngày kết thúc!",
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
                        url: '/hrm/IncurredSalary/IncurredSalaries',
                        dataType: "json",
                        data: JSON.stringify({
                            fromDate: $('#FromDate').data('kendoDatePicker').value(),
                            toDate: $('#ToDate').data('kendoDatePicker').value()
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
                    id: "IncurredSalaryId",
                    fields: {
                        CreateDate: { type: 'date' },
                        SubmitDate: { type: 'date' },
                        Amount: { type: 'number' }
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
        InitWindowModal('/hrm/IncurredSalary/IncurredSalary?id=' + id, false, 600, 350, 'Cập nhật lương phát sinh', false);
    });
    $('#btnCreate').click(function () {
        InitWindowModal('/hrm/IncurredSalary/IncurredSalary', false, 600, 350, 'Thêm mới lương phát sinh', false);
    });
    $('#btnDelete').bind("click", function () {
        var id = GetGridRowSelectedKeyValue('#grdMain');
        if (id == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "info",
                content: "Bạn phải chọn dữ liệu trước khi xóa!",
                buttons: [{ value: "Đồng ý" }]
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
                        url: '/hrm/IncurredSalary/Delete',
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