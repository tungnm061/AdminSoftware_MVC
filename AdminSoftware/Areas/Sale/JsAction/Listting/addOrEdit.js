$(document).ready(function () {

    $('#GmailId').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: gmails,
        height: 350,
        optionLabel: "Chọn Gmail",
        filter: 'contains'
    });

    $("#ListProduct").kendoNumericTextBox({
        format: "{0:n0}",
        min: 0,
        step: 1
    });

    $("#Balance").kendoNumericTextBox({
        format: "{0:n0}",
        min: 0,
        step: 1
    });
    
    $('#btnSave').click(function () {
        var model = {
            ListtingId: listtingId,
            GmailId: $('#GmailId').data('kendoDropDownList').value(),
            ThreeNumberPayOnner: $('#ThreeNumberPayOnner').val(),
            PayOnner: $('#PayOnner').val(),
            ListProduct: $('#ListProduct').data('kendoNumericTextBox').value(),
            Balance: $('#Balance').data('kendoNumericTextBox').value(),
            Description: $('#Description').val(),
            IsActive: $('#IsActive').is(":checked")
        };

        $('#processing').show();
        $.ajax({
            type: "POST",
            url: '/sale/Listting/Save',
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