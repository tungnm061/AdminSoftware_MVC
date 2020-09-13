$(document).ready(function () {
    //$('#InsuranceId').kendoDropDownList({
    //    dataTextField: "InsuranceNumber",
    //    dataValueField: "InsuranceId",
    //    dataSource: insurances,
    //    filter: 'startswith',
    //    headerTemplate: '<div class="dropdown-header k-widget k-header">' +
    //                           '<span>Mã BHXH</span>' +
    //                           '<span>Mã nhân viên</span>' +
    //                           '<span>Tên nhân viên</span>' +
    //                       '</div>',
    //    template: '<span class="k-state-default">#: data.InsuranceNumber #</span>' +
    //                              '<span class="k-state-default">#: data.EmployeeCode #</span><span class="k-state-default">#: data.FullName #</span>'
    //});
    $('#FromDate,#ToDate').kendoDatePicker({
        format: "dd/MM/yyyy",
        dateInput: true
    });
    $('#Amount').kendoNumericTextBox({
        min: 0,
        format:'{0:###,###}'
    });
    $('#btnSave').click(function() {
        var model = {
            InsuranceProcessId: insuranceProcessId,
            InsuranceId: $('#InsuranceId').val(),
            FromDate: $('#FromDate').data('kendoDatePicker').value(),
            ToDate: $('#ToDate').data('kendoDatePicker').value(),
            Description: $('#Description').val(),
            Amount: $('#Amount').data('kendoNumericTextBox').value()
        };
        if (model.InsuranceId == null || model.InsuranceId.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa có mã BHXH!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.FromDate == null) {
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
        if (model.ToDate != null && model.ToDate < model.FromDate) {
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
            url: '/hrm/Employee/SaveProcess',
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
                            GridCallback('#grdMainProcess');
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