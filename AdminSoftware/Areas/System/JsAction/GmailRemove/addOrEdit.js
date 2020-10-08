﻿$(document).ready(function () {
    $('#GmailId').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: gmails,
        optionLabel: "Chọn tài khoản",
        filter: 'contains'
    });

    $('#GmailRestoreId').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: gmails,
        optionLabel: "Chọn tài khoản",
        filter: 'contains'
    });

    $('#GmailChangeId').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: gmails,
        optionLabel: "Chọn tài khoản",
        filter: 'contains'
    });

    $('#GmailRestoreChangeId').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: gmails,
        optionLabel: "Chọn tài khoản",
        filter: 'contains'
    });

    $('#btnSave').click(function () {
        var model = {
            GmailId: $("#GmailId").data("kendoDropDownList").value(),
            GmailRestoreId: $("#GmailRestoreId").data("kendoDropDownList").value(),
            GmailChangeId: $("#GmailId").data("kendoDropDownList").value(),
            GmailRestoreChangeId: $("#GmailRestoreId").data("kendoDropDownList").value(),
            Password: $('#Password').val(),
            PasswordGmailChange: $('#PasswordGmailChange').val(),
            Description: $('#Description').val(),
            GmailRemoveId: id
        }

        $('#processing').show();
        $.ajax({
            type: "POST",
            url: '/system/GmailRemove/Save',
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