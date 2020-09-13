$(document).ready(function () {
    $("#PercentSalary").kendoNumericTextBox({
        format: "p0",
        min: 0,
        max: 1,
        step: 0.01
    });
    $('#btnSaveHolidayReason').click(function () {
        var model = {
            ReasonName: $('#ReasonNameHoliday').val(),
            ReasonCode: $('#ReasonCode').val(),
            Description: $('#Description').val(),
            IsActive: $('#IsActive').is(":checked"),
            HolidayReasonId: holidayReasonId,
            PercentSalary: $('#PercentSalary').data('kendoNumericTextBox').value()
        }
        if (model.ReasonCode == null || model.ReasonCode.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa nhập mã lý do nghỉ!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (model.PercentSalary == null ) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa nhập % nhận lương!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (model.ReasonName == null || model.ReasonName.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa nhập lý do nghỉ!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        $('#processing').show();
        $.ajax({
            type: "POST",
            url: '/hrm/EmployeeHoliday/SaveHolidayReason',
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
                            GridCallback('#grdMainHolidayReason');
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