$(document).ready(function () {
    $('#Status').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: statusPo,
        filter: 'contains'
    });

    $("#TradingDate").kendoDatePicker({
        dateInput: true,
        format: "dd/MM/yyyy"
    });

    $("#MoneyNumber").kendoNumericTextBox({
        format: "{0:n2}",
        step: 1
    });
    $("#RateMoney").kendoNumericTextBox({
        format: "{0:n2}",
        step: 1
    });
    $('#btnSave').click(function () {
        var model = {
            PoPaymentId: poPaymentId,
            MoneyNumber: $('#MoneyNumber').data("kendoNumericTextBox").value(),
            RateMoney: $('#RateMoney').data("kendoNumericTextBox").value(),
            TradingDate: $('#TradingDate').data("kendoDatePicker").value(),
            Status: $('#Status').data("kendoDropDownList").value()
        };

        $('#processing').show();
        $.ajax({
            type: "POST",
            url: '/sale/PoPayment/Save',
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