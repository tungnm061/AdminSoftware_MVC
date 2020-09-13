function ValidateTime(time) {
    var regex = new RegExp(/((?!00)[0-1][0-9]|2[1-4]):[0-5][0-9]/g);
    return regex.test(time);
}
$("#ModalEmployee").click(function () {
    InitChildWindowModal('/hrm/Maternity/Employees', 720, 445, "Tìm kiếm nhân viên");
});
$(document).ready(function () {
    $('#StartTime,#EndTime,#RelaxStartTime,#RelaxEndTime').kendoMaskedTextBox({
        mask: "00:00"
    });
    $("#FromDate,#ToDate").kendoDatePicker({
        dateInput: true,
        format: "dd/MM/yyyy"
    });
    $('#btnSave').click(function () {
        var model = {
            EmployeeId: $('#EmployeeId').val(),
            Description: $('#Description').val(),
            MaternityId: maternityId,
            StartTime: $('#StartTime').data('kendoMaskedTextBox').value(),
            EndTime: $('#EndTime').data('kendoMaskedTextBox').value(),
            FromDate: $('#FromDate').data('kendoDatePicker').value(),
            ToDate: $('#ToDate').data('kendoDatePicker').value(),
            RelaxStartTime: $('#RelaxStartTime').data('kendoMaskedTextBox').value(),
            RelaxEndTime: $('#RelaxEndTime').data('kendoMaskedTextBox').value(),
            CreateDate: createDate,
            CreateBy: createBy

        }
        if (model.FromDate == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn ngày bắt đầu!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.ToDate == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn ngày kết thúc!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.ToDate <= model.FromDate) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Ngày kết thúc phải lớn hơn ngày bắt đầu",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.StartTime == null || model.StartTime.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa nhập thời gian bắt đầu làm việc!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (!ValidateTime(model.StartTime)) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Thời gian bắt đầu làm việc không hợp lệ!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (model.EndTime == null || model.EndTime.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa nhập thời gian kết thúc làm việc!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (!ValidateTime(model.EndTime)) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Thời gian kết thúc làm việc không hợp lệ!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (model.RelaxStartTime == null || model.RelaxStartTime.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa nhập thời gian bắt đầu nghỉ!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (!ValidateTime(model.RelaxStartTime)) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Thời gian bắt đầu nghỉ không hợp lệ!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (model.RelaxEndTime == null || model.RelaxEndTime.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa nhập thời gian kết thúc nghỉ!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (!ValidateTime(model.RelaxEndTime)) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Thời gian kết thúc nghỉ không hợp lệ!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        $('#processing').show();
        $.ajax({
            type: "POST",
            url: '/hrm/Maternity/Save',
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