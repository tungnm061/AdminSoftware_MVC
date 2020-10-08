var record = 0;
$(document).ready(function () {

    $('#btnDelete').bind("click", function () {
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
                        url: '/system/GmailRemove/Delete',
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

    $('#btnCreate').bind("click", function () {
        InitWindowModal('/system/GmailRemove/GmailRemove?id=0', false, 600, 435, "Thêm mới tài khoản gỡ Amazon", false);

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
            return false;
        }
        InitWindowModal('/system/GmailRemove/GmailRemove?id=' + id, false, 600, 435, "Cập nhật tài khoản gỡ Amazon", false);
        return true;
    });

    $("#grdMain").kendoGrid({
        toolbar: ["excel"],
        excel: {
            fileName: "RemoveTKAmazon.xlsx",
            filterable: true,
            allPages: true
        },
        dataSource: {
            transport: {
                read: '/system/GmailRemove/GmailRemoves',
                dataType: "json"
            },
            schema: {
                model: {
                    id: "GmailRemoveId",
                    fields: {
                        CreateDate: { type: 'date' },
                        UpdateDate: { type: 'date' },
                    }
                }
            },
            pageSize: 20,
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
                title: "TK Gmail gỡ Amazon",
                width: 500,
                columns: [
                    {
                        field: "GmailId",
                        title: "Tên tài khoản",
                        width: 200,
                        values: gmails
                    },
                    {
                        field: "Password",
                        title: "Mật khẩu",
                        width: 100
                    },
                    {
                        field: "GmailRestoreId",
                        title: "Mail khôi phục",
                        width: 200,
                        values: gmails
                    },
                ],
                headerAttributes: {
                    style: "text-align: center"
                }
            },
            {
                title: "TK Gmail thay",
                width: 500,
                columns: [
                    {
                        field: "GmailChangeId",
                        title: "Tên tài khoản",
                        width: 200,
                        values: gmails
                    },
                    {
                        field: "PasswordGmailChange",
                        title: "Mật khẩu",
                        width: 100
                    },
                    {
                        field: "GmailRestoreChangeId",
                        title: "Mail khôi phục",
                        width: 200,
                        values: gmails
                    },
                ],
                headerAttributes: {
                    style: "text-align: center"
                }
            },
            {
                field: "Description",
                title: "Ghi chú",
                minwidth: 200
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