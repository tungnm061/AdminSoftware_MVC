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
        InitWindowModal('/system/Gmail/Gmail?id=0', false, 500, 280, "Thêm mới gmail", false);

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
        InitWindowModal('/system/Gmail/Gmail?id=' + id, false, 500, 280, "Cập nhật gmail", false);
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
                width: 60
            },
            {
                field: "FullName",
                title: "Tài khoản",
                width: 250
            },
            {
                field: "UserId",
                title: "Nhân viên",
                width: 120,
                values: users
            },
            {
                field: "Description",
                title: "Ghi chú",
                minwidth: 250
            },
            {
                field: "CreateDate",
                title: "Ngày tạo",
                width: 150,
                format: "{0:dd/MM/yyyy}"
            }
        ],
        dataoning: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    });
});