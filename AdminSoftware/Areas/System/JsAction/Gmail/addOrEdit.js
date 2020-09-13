$(document).ready(function () {

    $('#UserId').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: users,
        optionLabel: "Chọn nhân viên",
        filter: 'contains'
    });

    $('#btnSave').click(function () {
        var model = {
            FullName: $('#FullName').val(),
            Description: $('#Description').val(),
            UserId: $('#UserId').data('kendoDropDownList').value(),
            Id: id
        }
        if (model.FullName == null || model.FullName.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa nhập tài khoản!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        $('#processing').show();
        $.ajax({
            type: "POST",
            url: '/system/Gmail/Save',
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
                        if (result === 'Đồng ý' && response.Status === 1) {
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