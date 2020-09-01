$(document).ready(function () {
    $("#ModalEmployee").click(function () {
        InitChildWindowModal('/hrm/TimeSheetOt/Employees', 720, 445, "Tìm kiếm nhân viên");
    });
    $("#Hours").kendoNumericTextBox({
        format: "{0:###,###.##}",
        min : 0
    });
    $("#CoefficientPoint").kendoNumericTextBox({
        format: "p0",
        min: 0,
        max: 10,
        step: 0.1
    });
    $("#DayDate").kendoDatePicker({
        dateInput: true,
        format: "dd/MM/yyyy"
    });
    $('#btnSave').click(function () {
        var model = {
            TimeSheetOtId: timeSheetOtId,
            DayDate: $('#DayDate').data("kendoDatePicker").value(),
            Hours: $('#Hours').data("kendoNumericTextBox").value(),
            EmployeeId: $('#EmployeeId').val(),
            CoefficientPoint: $('#CoefficientPoint').data('kendoNumericTextBox').value(),
            CreateDate : createDate,
            Description: $('#Description').val()
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

        if (model.Hours == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa nhập số giờ làm!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.CoefficientPoint == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa nhập hệ số tính công!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.DayDate == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn ngày!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        $('#processing').show();
        $.ajax({
            type: "POST",
            url: '/hrm/TimeSheetOt/Save',
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