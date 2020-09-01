$(document).ready(function () {
    $("#Price").kendoNumericTextBox({
        format: "{0:n0}"
    });
    $("#Quantity").kendoNumericTextBox({
        format: "{0:n0}"
    });

    $('#btnSave').click(function () {
        var model = {
            ProductId: productId,
            ProductName: $('#ProductName').val(),
            ProductCode: $('#ProductCode').val(),
            Price: $('#Price').data('kendoNumericTextBox').value(),
            Quantity: $('#Quantity').data('kendoNumericTextBox').value(),
            CreateDate: createDate,
            CreateBy : createBy,
            Description: $('#Description').val(),
            IsActive: $('#IsActive').is(":checked")
        };
        if (model.ProductName == null || model.ProductName.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa nhập tên sản phẩm!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }

        if (model.ProductCode == null || model.ProductCode.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa nhập mã sản phẩm",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }

        if (model.Price == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa nhập đơn giá",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.Quantity == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa nhập số lượng sản phẩm",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        $('#processing').show();
        $.ajax({
            type: "POST",
            url: '/sale/Product/Save',
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
                            CloseWindowModal();
                            GridCallback('#grdMain');
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