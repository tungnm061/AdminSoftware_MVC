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
                        url: '/hrm/Praise/Delete',
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
        InitWindowModal('/hrm/Praise/Praise', false, 800, 520, "Thêm mới khen thưởng", false);
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
        InitWindowModal('/hrm/Praise/Praise?id=' + id, false, 800, 520, "Cập nhật khen thưởng", false);
    });
    $("#grdMain").kendoGrid({
        dataSource: {
            transport: {
                read: '/hrm/Praise/Praises',
                dataType: "json"
            },
            schema: {
                model: {
                    id: "PraiseDisciplineId",
                    fields: {
                        CreateDate: { type: 'date' },
                        PraiseDisciplineDate: { type: 'date' }
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
                field: "PraiseDisciplineCode",
                title: "Mã KT",
                width: 150
            },
            {
                field: "Title",
                title: "Quyết định",
                width: 200
            },
            {
                field: "DecisionNumber",
                title: "Số quyết định",
                width: 150
            },
            {
                field: "PraiseDisciplineDate",
                title: "Ngày quyết định",
                format: "{0:dd/MM/yyyy}",
                width: 180
            },
            {
                field: "CreateBy",
                title: "Người tạo",
                width: 150,
                values: users
            },
            {
                field: "CreateDate",
                title: "Ngày tạo",
                format: "{0:dd/MM/yyyy hh:mm tt}",
                width: 180
            }
        ],
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    });
});