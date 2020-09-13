var record = 0;
$(document).ready(function () {
    $('#btnCreate').bind("click", function () {
        InitWindowModal('/hrm/Maternity/Maternity', false, 550, 420, "Thêm mới chế độ thai sản", false);
    });
    $('#btnEdit').bind("click", function () {
        var id = GetGridRowSelectedKeyValue('#grdMain');
        if (id == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn dữ liệu trước khi sửa!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        InitWindowModal('/hrm/Maternity/Maternity?id=' + id, false, 550, 420, "Cập nhật chế độ thai sản", false);
    });
    $("#grdMain").kendoGrid({
        dataSource: {
            transport: {
                read: '/hrm/Maternity/Maternitys',
                dataType: "json"
            },
            schema: {
                model: {
                    id: "MaternityId",
                    fields: {
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
                field: "EmployeeCode",
                title: "Mã Nhân viên",
                width:150
            },
             {
                 field: "FullName",
                 title: "Tên Nhân viên",
                 width: 200
             },
            {
                field: "",
                title: "Thời gian làm việc",
                width: 150,
                template:'<span>#=StartTime# - #=EndTime#</span>'
            },
            {
                field: "",
                title: "Thời gian nghỉ",
                width: 150,
                template: '<span>#=RelaxStartTime# - #=RelaxEndTime#</span>'
            },
            {
                field: "FromDate",
                title: "Từ ngày",
                width: 150,
                format:"{0:dd/MM/yyyy}"
            },
            {
                field: "ToDate",
                title: "Đến ngày",
                width: 150,
                format: "{0:dd/MM/yyyy}"
            },
            {
                field: "Description",
                title: "Ghi chú",
                minwidth:250
            }
        ],
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
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
                        url: '/hrm/Maternity/Delete',
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