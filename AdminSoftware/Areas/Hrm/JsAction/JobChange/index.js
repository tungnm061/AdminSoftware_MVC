var record = 0;
$(document).ready(function () {
    $('#btnDelete').bind("click", function () {
        var id = GetGridRowSelectedKeyValue('#grdMain');
        if (id == null) {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn chưa chọn dữ liệu trước khi sửa!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        $.msgBox({
            title: "Hệ thống ERP",
            type: "confirm",
            content: "Bạn có chắc chắn muốn xóa dữ liệu không?",
            buttons: [{ value: "Đồng ý" }, { value: "Hủy bỏ" }],
            success: function (result) {
                if (result === "Đồng ý") {
                    $('#processing').show();
                    $.ajax({
                        type: 'POST',
                        url: '/hrm/JobChange/Delete',
                        data: JSON.stringify({ id: id }),
                        contentType: 'application/json;charset=utf-8',
                        success: function (response) {
                            $('#processing').hide();
                            if (response.Status === 0) {
                                $.msgBox({
                                    title: "Hệ thống ERP",
                                    type: "error",
                                    content: response.Message,
                                    buttons: [{ value: "Đồng ý" }],
                                    success: function () {
                                    }
                                });
                            } else {
                                $.msgBox({
                                    title: "Hệ thống ERP",
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
    $('#btnCreate').bind("click", function () {
        InitWindowModal('/hrm/JobChange/JobChange', false, 900, 380, "Thêm mới thuyên chuyển công tác", false);
    });
    $('#btnEdit').bind("click", function () {
        var id = GetGridRowSelectedKeyValue('#grdMain');
        if (id == null) {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn chưa chọn dữ liệu trước khi sửa!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        InitWindowModal('/hrm/JobChange/JobChange?id=' + id, false, 900, 380, "Cập nhật thuyên chuyển công tác", false);
    });
    $("#grdMain").kendoGrid({
        dataSource: {
            transport: {
                read: '/hrm/JobChange/JobChanges',
                dataType: "json"
            },
            schema: {
                model: {
                    id: "JobChangeId",
                    fields: {
                        CreateDate: { type: 'date' }
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
                field: "JobChangeCode",
                title: "Số phiếu",
                width: 150
            },
            {
                field: "EmployeeId",
                title: "Nhân viên",
                width: 250,
                values: employees
            },
            {
                field: "JobChangeNumber",
                title: "Số quyết định",
                width: 150
            },
            {
                field: "FromDepartmentId",
                title: "Phòng cũ",
                width: 170,
                values: departments
            },
            {
                field: "ToDepartmentId",
                title: "Phòng mới",
                width: 170,
                values: departments
            },
            {
                field: "FromPositionId",
                title: "Chức vụ cũ",
                width: 170,
                values:positions
            },
            {
                field: "ToPositionId",
                title: "Chức vụ mới",
                width: 170,
                values: positions
            },
            {
                field: "CreateBy",
                title: "Người tạo",
                width: 150,
                values:users
            },
            {
                field: "CreateDate",
                title: "Ngày tạo",
                format: "{0:dd/MM/yyyy hh:mm tt}",
                width: 170
            },
            {
                field: "Reason",
                title: "Lý do",
                width: 250
            }
        ],
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    });
});