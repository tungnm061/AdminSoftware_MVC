var record = 0;
function ChangeStatus() {
    GridCallback('#grdMain');
}
$(document).ready(function () {
    $("#grdMain").kendoGrid({
        dataSource: {
            transport: {
                read: function (options) {
                    $.ajax(
                    {
                        type: 'POST',
                        url: '/system/Account/Accounts',
                        dataType: "json",
                        data: JSON.stringify({
                            isActive: $('input[name="status"]:checked').val()
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
                        CreateDate: { type: 'date' },
                        IsActive:{type:'boolean'}
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
                width: 60,
                locked : true
            },
            {
                field: "UserName",
                title: "Tên đăng nhập",
                width: 170,
                locked: true
            },
            {
                field: "RoleId",
                title: "Quyền",
                width: 150,
                values: roles,
                locked: true
            },
            {
                field: "UserGroupId",
                title: "Nhóm",
                width: 200,
                values: userGroups
            },
            {
                field: "EmployeeId",
                title: "Mã nhân viên",
                width: 170,
                values: employees
            },
            {
                field: "FullName",
                title: "Họ tên",
                width: 200
            },
            {
                field: "Email",
                title: "Email",
                width: 170
            },
            //{
            //    field: "PhoneNumber",
            //    title: "Điện thoại",
            //    width: 170
            //},
            //{
            //    field: "Description",
            //    title: "Ghi chú",
            //    width:250
            //},
            {
                field: "CreateDate",
                title: "Ngày tạo",
                format: "{0:dd/MM/yyyy}",
                width: 170
            },
            {
                field: "IsActive",
                title: "Kich hoạt",
                width: 120,
                template: '<div class="text-center"><i class="#= IsActive ? "glyphicon glyphicon-ok" : "glyphicon glyphicon-ban-circle" #"></i></div>'
            }
        ],
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    });
    $('#btnCreate').click(function () {
        InitWindowModal('/system/Account/Account', false, 800, 245, 'Thêm mới tài khoản', false);
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
        InitWindowModal('/system/Account/Account?id=' + id, false, 800, 220, 'Cập nhật tài khoản', false);
    });
    $('#btnSetFunction').click(function () {
        var row = GetGridRowSelected('#grdMain');
        if (row == null || row == typeof undefined) {
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
        if (row.RoleId === 1) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn không thể phân quyền cho tài khoản quản trị hệ thống!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        InitWindowModal('/system/Account/UserRight?userId=' + row.UserId, false, 800, 500, 'Phân quyền tài khoản ' + row.UserName, false);
    });

    $('#btnReset').click(function () {
        var id = GetGridRowSelectedKeyValue('#grdMain');
        if (id == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn phải chọn tài khoản cần reset mật khẩu!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        $.msgBox({
            title: "Hệ thống",
            type: "confirm",
            content: "Bạn có chắc chắn muốn reset mật khẩu không?",
            buttons: [{ value: "Đồng ý" }, { value: "Hủy bỏ" }],
            success: function (result) {
                if (result === "Đồng ý") {
                    $('#processing').show();
                    $.ajax({
                        type: 'POST',
                        url: '/system/Account/ResetPassWord',
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