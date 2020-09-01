var record = 0;
$(document).ready(function () {
    $("#grdMain").kendoGrid({
        toolbar: ["excel"],
        excel: {
            fileName: "SanPhamKinhDoanh.xlsx",
            filterable: true,
            allPages: true
        },
        dataSource: {
            transport: {
                read: '/sale/Product/Products',
                dataType: "json"
            },
            schema: {
                model: {
                    id: "ProductId",
                    fields: {
                        CreateDate: { type: 'date' },
                        IsActive: { type: 'boolean' },
                        Price: { type: 'number' },
                        Quantity: { type: 'number' }

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
                  field: "ProductCode",
                  title: "Mã sản phẩm",
                  width: 140
              },
            {
                field: "ProductName",
                title: "Tên sản phẩm",
                width: 250
            },
            {
                field: "Price",
                title: "Đơn giá (VND)",
                width: 200,
                format: "{0:n0}"
            },
            {
                field: "Quantity",
                title: "Số lượng",
                width: 200,
                format: "{0:n0}"
            },
            {
                field: "CreateDate",
                title: "Ngày tạo",
                width: 120,
                format: "{0:dd/MM/yyyy}"
            },
         {
             field: "CreateBy",
             title: "Người tạo",
             width: 160,
             values: employees
         },
            {
                field: "Description",
                title: "Ghi Chú",
                width: 200
            },
            {
                field: "IsActive",
                title: "Trạng Thái",
                width: 120,
                template: '<div class="text-center"><i class="#= IsActive ? "glyphicon glyphicon-ok" : "glyphicon glyphicon-ban-circle" #"></i></div>'

            }


        ],
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    });

    $('#btnCreate').click(function () {
        InitWindowModal('/sale/Product/Product', false, 600, 425, 'Thêm mới sản phẩm', false);
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
        InitWindowModal('/sale/Product/Product?id=' + id, false, 600, 425, 'Cập nhật sản phẩm', false);
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
                        url: '/sale/Product/Delete',
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