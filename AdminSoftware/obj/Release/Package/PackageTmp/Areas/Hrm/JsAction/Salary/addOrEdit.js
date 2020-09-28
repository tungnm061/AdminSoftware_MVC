$(document).ready(function () {
    $('#ApplyDate').kendoDatePicker({
        start: "year",
        depth: "year",
        dateInput: true,
        format: "MM/yyyy"
    });
    $('#BasicSalary').kendoNumericTextBox({
        min: 0,
        format:'{0:###,###}'
    });
    $('#BasicCoefficient,#ProfessionalCoefficient,#ResponsibilityCoefficient').kendoNumericTextBox({
        min: 0,
        decimals:2
    });
    $('#btnSave').click(function () {
        var model = {
            SalaryId: salaryId,
            EmployeeId: $("#EmployeeIdPost").val(),
            BasicSalary: $('#BasicSalary').data('kendoNumericTextBox').value(),
            BasicCoefficient: $('#BasicCoefficient').data('kendoNumericTextBox').value(),
            ProfessionalCoefficient: $('#ProfessionalCoefficient').data('kendoNumericTextBox').value(),
            ResponsibilityCoefficient: $('#ResponsibilityCoefficient').data('kendoNumericTextBox').value(),
            PercentProfessional: $('#PercentProfessional').val(),
            ApplyDate: $('#ApplyDate').data('kendoDatePicker').value()
        }
        if (model.ApplyDate == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn thời gian áp dụng!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (model.BasicSalary == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn phải nhập lương cơ bản!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (model.BasicCoefficient == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn phải nhập hệ số lương cơ bản!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (model.ProfessionalCoefficient == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn phải nhập hệ số xếp hạng chuyên môn lành nghề!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (model.PercentProfessional == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn phải nhập % hưởng lương theo trình độ chuyên môn!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (model.ResponsibilityCoefficient == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn phải nhập % hưởng lương theo trình độ chuyên môn!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        $('#processing').show();
        $.ajax({
            type: "POST",
            url: '/hrm/Employee/SaveSalary',
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
                            CloseChildWindowModal();
                            GridCallback('#grdSalaries');
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