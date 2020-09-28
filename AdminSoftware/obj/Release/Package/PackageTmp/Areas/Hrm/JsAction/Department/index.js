$(document).ready(function () {
    $('#btnCreate').click(function () {
        InitWindowModal('/hrm/Department/Department', false, 600, 330, "Thêm mới phòng ban", false);
    });
    $('#btnEdit').click(function () {
        var id = GetTreeListSelectedKeyValue('#treeMain');
        if (id == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn phải chọn dữ liệu trước khi cập nhật!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
        } else {
            InitWindowModal('/hrm/Department/Department?id=' + id, false, 600, 330, "Cập nhật phòng ban", false);
        }
    });

    $('#btnDelete').bind("click", function () {
        var id = GetTreeListSelectedKeyValue('#treeMain');
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
                        url: '/hrm/Department/Delete',
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
                                        TreeListCallback('#treeMain');
                                    }
                                });
                            }
                        }
                    });
                }
            }
        });
    });
    $("#treeMain").kendoTreeList({
        dataSource: {
            transport: {
                read: {
                    url: '/hrm/Department/Departments',
                    dataType: "json"
                }
            },
            schema: {
                model: {
                    id: "DepartmentId",
                    parentId: "ParentId",
                    fields: {
                        IsActive: { field: "IsActive", type: "boolean" },
                        ParentId: { field: "ParentId", nullable: true },
                        DepartmentId: { field: "DepartmentId", nullable: false }
                    },
                    expanded: true
                }
            }
        },
        height: gridHeight,
        filterable: true,
        sortable: true,
        selectable: 'row',
        columns: [
            {
                field: "DepartmentCode",
                title: "Mã phòng ban",
                width: 150
            },
            {
                field: "DepartmentName",
                title: "Tên phòng ban",
                width: 200
            },
              {
                  field: "EmployeeOns",
                  title: "Số NV đang làm",
                  width: 150
              },
                {
                    field: "EmployeeOffs",
                    title: "Số NV nghỉ",
                    width: 150
                },
            {
                field: "Description",
                title: "Ghi chú",
                minwidth:350
            },
            {
                field: "IsActive",
                title: "Kích hoạt",
                template: '<div class="text-center"><i class="#= IsActive ? "glyphicon glyphicon-ok" : "glyphicon glyphicon-ban-circle" #"></i></div>',
                width: 150
            }
        ]
    });
});