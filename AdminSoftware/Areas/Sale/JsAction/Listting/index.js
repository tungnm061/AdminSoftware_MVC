var record = 0;
$(document).ready(function () {
    $("#grdMain").kendoGrid({
        toolbar: [],
        //excel: {
        //    fileName: "SanPhamKinhDoanh.xlsx",
        //    filterable: true,
        //    allPages: true
        //},
        dataSource: {
            transport: {
                read: '/sale/Listting/Listtings',
                dataType: "json"
            },
            schema: {
                model: {
                    id: "ListtingId",
                    fields: {
                        CreateDate: { type: 'date' },
                        IsActive: { type: 'boolean' }
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
                  field: "GmailId",
                  title: "Gmail",
                  width: 180,
                  values : gmails
              },
            {
                field: "ThreeNumberPayOnner",
                title: "Ba số PayOnner",
                width: 150
            },
            {
                field: "PayOnner",
                title: "PayOnner",
                width: 150,
            },
           {
             field: "ListProduct",
             title: "Listting",
             width: 160,
            },
            {
                field: "Balance",
                title: "Balance",
                width: 160,
            },
            {
                field: "Description",
                title: "Ghi Chú",
                width: 200
            }


        ],
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    });

    $('#btnCreate').click(function () {
        InitWindowModal('/sale/Listting/Listting', false, 600, 395, 'Thêm mới listting', false);
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
        InitWindowModal('/sale/Listting/Listting?id=' + id, false, 600, 395, 'Cập nhật listting', false);
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
                        url: '/sale/Listting/Delete',
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