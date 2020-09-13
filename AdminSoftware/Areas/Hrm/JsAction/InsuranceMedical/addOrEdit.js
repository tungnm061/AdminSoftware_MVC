$(document).ready(function() {
    $('#StartDate,#ExpiredDate').kendoDatePicker({
        format: "dd/MM/yyyy",
        dateInput: true
    });
    $('#CityId').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: cities,
        optionLabel: 'Chọn nơi đăng ký',
        filter: 'startswith'
    });
    $('#MedicalId').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: medicals,
        optionLabel: 'Chọn nơi KCB',
        filter: 'startswith'
    });
    $('#Amount').kendoNumericTextBox({
        min: 0,
        format:'{0:###,###}'
    });
    $('#btnSave').click(function() {
        var model = {
            InsuranceMedicalId: insuranceMedicalId,
            EmployeeId: $('#EmployeeIdPost').val(),
            InsuranceMedicalNumber: $('#InsuranceMedicalNumber').val(),
            StartDate: $('#StartDate').data('kendoDatePicker').value(),
            ExpiredDate: $('#ExpiredDate').data('kendoDatePicker').value(),
            CityId: $('#CityId').data('kendoDropDownList').value(),
            MedicalId: $('#MedicalId').data('kendoDropDownList').value(),
            Description: $('#Description').val(),
            Amount: $('#Amount').data('kendoNumericTextBox').value()
        };
        if (model.EmployeeId == null || model.EmployeeId.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn nhân viên!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.InsuranceMedicalNumber == null || model.InsuranceMedicalNumber.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn phải nhập số BHYT!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.StartDate == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn phải chọn ngày bắt đầu!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.ExpiredDate == null || model.ExpiredDate < model.StartDate) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Ngày hết hạn không hợp lệ!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.CityId == null || model.CityId.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn phải chọn nơi cấp!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.MedicalId == null || model.MedicalId.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn phải chọn nơi KCB!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.Amount == null || model.Amount <= 0) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn phải nhập mức đóng BHYT!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        $('#processing').show();
        $.ajax({
            type: "POST",
            url: '/hrm/Employee/SaveInsuranceMedical',
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
                            GridCallback('#grdMainMedical');
                        }
                    }
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