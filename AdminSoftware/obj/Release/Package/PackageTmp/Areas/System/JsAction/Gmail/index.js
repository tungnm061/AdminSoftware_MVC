var record = 0;
$(document).ready(function () {

    $('#btnSkuView').on("click", function () {
        var id = GetGridRowSelectedKeyValue('#grdMain');
        if (id == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn dữ liệu !",
                buttons: [{ value: "Đồng ý" }]
            });
            return false;
        }
        InitWindowModal('/system/Gmail/SkuView?gmailId=' + id, true, "", "", 'Quản lý SKU', false);
    });

    $('#btnDelete').on("click", function () {
        var id = GetGridRowSelectedKeyValue('#grdMain');
        if (id == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn dữ liệu trước khi sửa!",
                buttons: [{ value: "Đồng ý" }]
            });
            return false;
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
                        url: '/system/Gmail/Delete',
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
        return true;
    });

    $('#btnCreate').on("click", function () {
        InitWindowModal('/system/Gmail/Gmail?id=0', false, 500, 385, "Thêm mới gmail", false);

    });

    $('#btnEdit').on("click", function () {
        var id = GetGridRowSelectedKeyValue('#grdMain');
        if (id == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn dữ liệu trước khi sửa!",
                buttons: [{ value: "Đồng ý" }]
            });
            return false;
        }
        InitWindowModal('/system/Gmail/Gmail?id=' + id, false, 500, 385, "Cập nhật gmail", false);
        return true;
    });

    $("#grdMain").kendoGrid({
        dataSource: {
            transport: {
                read: '/system/Gmail/Gmails',
                dataType: "json"
            },
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        CreateDate: { type: 'date' },
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
                field: "FullName",
                title: "Tài khoản",
                width: 200
            },
            {
                field: "LinkUrl",
                title: "Đường dẫn",
                width: 160,
                template: "<a href=\"#=LinkUrl#\" target=\"_blank\">#:LinkUrl#</a>"

            },
            {
                field: "CreateUser",
                title: "NV tạo",
                width: 120,
                values: users
            },
            {
                field: "RemoveUser",
                title: "NV gỡ",
                width: 120,
                values: users
            },
            {
                field: "ListtingUser",
                title: "NV Listting",
                width: 120,
                values: users
            },
            {
                field: "OrderUser",
                title: "NV Order",
                width: 120,
                values: users
            },
            {
                field: "LinkUrl",
                title: "Đường dẫn",
                width: 180
            },
            {
                field: "Description",
                title: "Ghi chú",
                width: 180
            },
            {
                field: "CreateDate",
                title: "Ngày tạo",
                width: 150,
                format: "{0:dd/MM/yyyy}"
            }
        ],
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    });
});