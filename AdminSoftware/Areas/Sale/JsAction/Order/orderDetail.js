$(document).ready(function () {
    console.log(products);
    $('#ProductId').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: products,
        optionLabel: "Chọn sản phẩm",
        filter: 'contains'
    });

    $('#Size').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: sizes,
        optionLabel: "Chọn kích thước",
        filter: 'contains'
    });

    $('#Color').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: colors,
        optionLabel: "Chọn màu sắc",
        filter: 'contains'
    });

    $("#Quantity").kendoNumericTextBox({
        format: "{0:###,###}",
        min: 1,
        step: 1
    });

    $('#btnSaveAdd').click(function () {
        var model = {
            ProductId: $('#ProductId').val(),
            OrderId: orderId,
            OrderDetailId: orderDetailId,
            ProductCode: $('#ProductCode').val(),
            ProductName: $('#ProductName').val(),
            Size: $('#Size').data('kendoDropDownList').value(),
            Color: $('#Color').data('kendoDropDownList').value(),
            Quantity: $("#Quantity").data('kendoNumericTextBox').value()
        };
        if (model.ProductId === null || model.ProductId.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn sản phẩm!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }


        if (model.Size === null || model.Size.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn kích thước!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }

        if (model.Color === null || model.Color.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn màu sắc!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }

        if (model.Quantity === null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn số lượng!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }



        $('#processing').show();
        $.ajax({
            type: "POST",
            url: '/sale/Order/SaveDetail',
            data: JSON.stringify({ model: model }),
            contentType: 'application/json; charset=utf-8',
            success: function (response) {
                $('#processing').hide();
                var type;
                if (response.Status === 1)
                    type = 'info';
                else
                    type = 'error';
                $.msgBox({
                    title: "Hệ thống",
                    type: type,
                    content: response.Message,
                    buttons: [{ value: "Đồng ý" }],
                    success: function (result) {
                        if (result == 'Đồng ý' && response.Status === 1) {
                            CloseChildWindowModal();
                            GridCallback('#grdOrderDetail');
                        }
                    } //
                });
            },
            error: function (response) {
                $('#processing').hide();
                $.msgBox({
                    title: "Hệ thống",
                    type: "error",
                    content: response.Msg,
                    buttons: [{ value: "Đồng ý" }]
                });
            }
        });
    });
});