
function fnSaveDetail(status) {
    var textStatus = "xác nhận";
    if (status === 2) {
        textStatus = "trả về";
    }

    $.msgBox({
        title: "Hệ thống",
        type: "confirm",
        content: "Bạn có chắc chắn muốn " + textStatus + " giao dịch này ?",
        buttons: [{ value: "Đồng ý" }, { value: "Hủy bỏ" }],
        success: function (result) {
            if (result === "Đồng ý") {
                $('#processing').show();
                $.ajax({
                    type: "POST",
                    url: '/sale/ConfirmPoPayment/SaveDetail',
                    data: JSON.stringify({ poPaymentId: poPaymentId, status: status }),
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
                                    GridCallback('#grdMain');
                                    CloseWindowModal();
                                    resetSelect();
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
            }
        }
    });
}
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

    $('#TradingBy').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: employees,
        filter: 'contains'
    });

    $("#MoneyNumber").kendoNumericTextBox({
        format: "{0:n2}",
        step: 1
    });
    $("#RateMoney").kendoNumericTextBox({
        format: "{0:n2}",
        step: 1
    });

    $('#btnConfirmDetail').click(function () {
        fnSaveDetail(3);
    });

    $('#btnCancelDetail').click(function () {
        fnSaveDetail(2);
    });

});