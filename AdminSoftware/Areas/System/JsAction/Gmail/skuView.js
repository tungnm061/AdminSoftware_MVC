var recordSku = 0;
$(document).ready(function () {
    $('#btnDeleteSku').bind("click", function () {
        var id = GetGridRowSelectedKeyValue('#grdMainSku');
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
                        url: '/system/Gmail/DeleteSku',
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
                                        GridCallback('#grdMainSku');
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

    $('#btnAddSku').bind("click", function () {
        InitChildWindowModal('/system/Gmail/Sku?id=0', 500, 280, "Thêm mới SKU");

    });

    $('#btnEditSku').bind("click", function () {
        var id = GetGridRowSelectedKeyValue('#grdMainSku');
        if (id == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn dữ liệu trước khi sửa!",
                buttons: [{ value: "Đồng ý" }]
            });
            return false;
        }
        InitChildWindowModal('/system/Gmail/Sku?id=' + id, 500, 280, "Cập nhật SKU");
        return true;
    });

    $("#grdMainSku").kendoGrid({
        dataSource: {
            transport: {
                read: '/system/Gmail/Skus?gmailId=' + gmailId,
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
                template: "#= ++recordSku #",
                width: 60
            },
            {
                field: "Code",
                title: "Mã SKU",
                width: 200
            },
            {
                field: "Description",
                title: "Ghi chú",
                minwidth: 200
            },
            {
                field: "CreateDate",
                title: "Ngày tạo",
                width: 120,
                format: "{0:dd/MM/yyyy}"
            }
        ],
        dataBinding: function () {
            recordSku = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    });
});