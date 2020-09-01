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
                        url: '/system/UserGroup/Delete',
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
        InitWindowModal('/system/UserGroup/UserGroup', false, 550, 280, "Thêm mới nhóm người dùng", false);
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
        InitWindowModal('/system/UserGroup/UserGroup?id=' + id, false, 550, 280, "Cập nhật nhóm người dùng", false);
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
        InitWindowModal('/system/UserGroup/UserGroupRight?id=' + row.UserGroupId, false, 800, 500, 'Phân quyền nhóm tài khoản ' + row.GroupName, false);
    });
    $("#grdMain").kendoGrid({
        dataSource: {
            transport: {
                read: '/system/UserGroup/UserGroups',
                dataType: "json"
            },
            schema: {
                model: {
                    id: "UserGroupId",
                    fields: {
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
                field: "GroupCode",
                title: "Mã nhóm",
                width: 200
            },
            {
                field: "GroupName",
                title: "Tên nhóm",
                width: 250
            },
            {
                field: "Description",
                title: "Ghi chú",
                minwidth: 350
            }
        ],
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    });
});