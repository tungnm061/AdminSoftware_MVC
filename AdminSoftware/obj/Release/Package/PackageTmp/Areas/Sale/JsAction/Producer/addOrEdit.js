$(document).ready(function () {
    $('#btnSave').click(function () {
        var model = {
            ProducerId: ProducerId,
            ProducerName: $('#ProducerName').val(),
            ProducerCode: $('#ProducerCode').val(),
            Description: $('#Description').val(),
            IsActive: $('#IsActive').is(":checked")
        };

        if (model.ProducerName == null || model.ProducerName.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa nhập tên nhà sản xuất!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }

        if (model.ProducerCode == null || model.ProducerCode.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa nhập mã nhà sản xuất",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }

        $('#processing').show();
        $.ajax({
            type: "POST",
            url: '/sale/Producer/Save',
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