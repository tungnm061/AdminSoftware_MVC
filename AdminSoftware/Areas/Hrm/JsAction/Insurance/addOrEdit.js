$(document).ready(function() {
    $('#SubscriptionDate').kendoDatePicker({
        format: "dd/MM/yyyy",
        dateInput: true,
        max:new Date()
    });
    $('#CityId').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: cities,
        optionLabel: 'Chọn nơi đăng ký',
        filter: 'startswith'
    });
    $('#MonthBefore').kendoNumericTextBox({
        min: 0,
        format:'{0:###,###}'
    });
    $('#btnSave').click(function() {
        var model = {
            InsuranceId: insuranceId,
            EmployeeId: employeeId,
            InsuranceNumber: $('#InsuranceNumber').val(),
            SubscriptionDate: $('#SubscriptionDate').data('kendoDatePicker').value(),
            CityId: $('#CityId').data('kendoDropDownList').value(),
            Description: $('#Description').val(),
            IsActive: $('#IsActive').is(":checked"),
            MonthBefore: $('#MonthBefore').data('kendoNumericTextBox').value()
        };
        if (model.EmployeeId == null || model.EmployeeId.trim() === "") {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn chưa chọn nhân viên!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.InsuranceNumber == null || model.InsuranceNumber.trim() === "") {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn phải nhập số BHXH!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.SubscriptionDate == null) {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn phải chọn ngày đăng ký!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.CityId == null || model.CityId.trim() === "") {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn phải chọn nơi cấp!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        $('#processing').show();
        $.ajax({
            type: "POST",
            url: '/hrm/Employee/SaveInsurance',
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
                    title: "Hệ thống ERP",
                    type: type,
                    content: response.Message,
                    buttons: [{ value: "Đồng ý" }],
                    success: function (result) {
                        if (result === 'Đồng ý' && response.Status === 1) {
                            CloseChildOfChildWindowModal();
                            GridCallback('#grdMain');
                        }
                    }
                });
            },
            error: function (response) {
                $('#processing').hide();
                $.msgBox({
                    title: "Hệ thống ERP",
                    type: "error",
                    content: response.Msg,
                    buttons: [{ value: "Đồng ý" }]
                });
            }
        });
    });
});