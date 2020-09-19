$(document).ready(function () {
    $('#ExpenseId').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: expenseTypes,
        optionLabel: "Chọn loại giao dịch",
        filter: 'contains'
    });

    $('#TypeMonney').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: typeMoneys,
        filter: 'contains'
    });

    $('#TradingBy').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: users,
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


    $('#btnSave').click(function () {
        var model = {
            CompanyBankId: id,
            ExpenseId: $('#ExpenseId').data("kendoDropDownList").value(),
            //Description: $('#Description').val(),
            TypeMonney: $('#TypeMonney').data("kendoDropDownList").value(),
            MoneyNumber: $('#MoneyNumber').data("kendoNumericTextBox").value(),
            TradingDate: $('#TradingDate').data("kendoDatePicker").value(),
            TradingBy: $('#TradingBy').data("kendoDropDownList").value(),
            ExpenseText: $('#ExpenseText').val()

        }

        $('#processing').show();
        $.ajax({
            type: "POST",
            url: '/system/CompanyBank/Save',
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