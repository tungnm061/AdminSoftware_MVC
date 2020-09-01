
$(document).ready(function () {
    $('#SubmitDate').kendoDatePicker({
        dateInput: true,
        format: "dd/MM/yyyy"
    });
    $('#Amount').kendoNumericTextBox({
        format:'{0:###,###}'
    });
    $('#btnSave').click(function() {
        var model = {
            IncurredSalaryId: incurredSalaryId,
            EmployeeId: $('#EmployeeIdPost').val(),
            Amount: $('#Amount').data('kendoNumericTextBox').value(),
            Title: $('#Title').val(),
            SubmitDate: $('#SubmitDate').data('kendoDatePicker').value(),
            Description: $('#Description').val()
        };
        if (model.Title == null || model.Title.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn phải nhập lý do!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
     
        if (model.Amount == null || model.Amount === 0) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn phải nhập số tiền!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.SubmitDate == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn phải chọn kỳ hoàn trả!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        $('#processing').show();
        $.ajax({
            type: 'POST',
            url: '/hrm/Employee/SaveIncurredSalary',
            data: JSON.stringify({ model: model }),
            contentType: 'application/json;charset=utf-8',
            success: function(response) {
                $('#processing').hide();
                if (response.Status === 0) {
                    $.msgBox({
                        title: "Hệ thống",
                        type: "error",
                        content: response.Message,
                        buttons: [{ value: "Đồng ý" }]
                    });
                } else {
                    $.msgBox({
                        title: "Hệ thống",
                        type: "info",
                        content: response.Message,
                        buttons: [{ value: "Đồng ý" }],
                        success:function() {
                            CloseChildWindowModal();
                            GridCallback('#grdMainIncurredSalary');
                        }
                    });
                }
            }
        });
    });
});