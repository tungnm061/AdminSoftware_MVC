﻿$(document).ready(function () {
    $('#btnSave').click(function () {
        var model = {
            TypeName: $('#TypeName').val(),
            Description: $('#Description').val(),
            IsActive: $('#IsActive').is(":checked"),
            ContractTypeId: contractTypeId
        }
        if (model.TypeName == null || model.TypeName.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa nhập loại hợp đồng!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        $('#processing').show();
        $.ajax({
            type: "POST",
            url: '/hrm/Employee/SaveContractType',
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
                            CloseChildOfChildWindowModal();
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