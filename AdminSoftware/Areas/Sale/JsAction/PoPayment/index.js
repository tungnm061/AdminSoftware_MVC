
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
                read: '/sale/PoPayment/PoPayments',
                dataType: "json"
            },
            schema: {
                model: {
                    id: "PoPaymentId",
                    fields: {
                        CreateDate: { type: 'date' },
                        TradingDate: { type: 'date' },
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
                field: "TradingDate",
                title: "Ngày giao dịch",
                width: 140,
                format: "{0:dd/MM/yyyy}"
            },
            {
                field: "MoneyNumber",
                title: "Số tiền (USD)",
                width: 200
            },
            {
                field: "RateMoney",
                title: "Tỷ lệ",
                width: 120
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
            }



        ],
        dataBinding: function () {
            record = (this.dataSource.page() - 1) * this.dataSource.pageSize();
        }
    });

    $('#btnCreate').click(function () {
        InitWindowModal('/sale/PoPayment/PoPayment', false, 600, 305, 'Thêm mới PO', false);
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
        InitWindowModal('/sale/PoPayment/PoPayment?id=' + id, false, 600, 305, 'Cập nhật PO', false);
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
                        url: '/sale/PoPayment/Delete',
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