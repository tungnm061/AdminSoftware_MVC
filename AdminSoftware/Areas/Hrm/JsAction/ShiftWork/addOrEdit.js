function ValidateTime(time) {
    var regex = new RegExp(/((?!00)[0-1][0-9]|2[1-4]):[0-5][0-9]/g);
    return regex.test(time);
}
$(document).ready(function () {
    $('#StartTime,#EndTime,#RelaxStartTime,#RelaxEndTime').kendoMaskedTextBox({
        mask: "00:00"
    });
    $('#btnSave').click(function () {
        var model = {
            ShiftWorkCode: $('#ShiftWorkCode').val(),
            Description: $('#Description').val(),
            ShiftWorkId: shiftWorkId,
            StartTime: $('#StartTime').data('kendoMaskedTextBox').value(),
            EndTime: $('#EndTime').data('kendoMaskedTextBox').value(),
            RelaxStartTime: $('#RelaxStartTime').data('kendoMaskedTextBox').value(),
            RelaxEndTime: $('#RelaxEndTime').data('kendoMaskedTextBox').value()
        }
        if (model.ShiftWorkCode == null || model.ShiftWorkCode.trim() === "") {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn chưa nhập mã ca làm việc!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (model.StartTime == null || model.StartTime.trim() === "") {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn chưa nhập thời gian bắt đầu làm việc!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (!ValidateTime(model.StartTime)) {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Thời gian bắt đầu làm việc không hợp lệ!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (model.EndTime == null || model.EndTime.trim() === "") {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn chưa nhập thời gian kết thúc làm việc!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (!ValidateTime(model.EndTime)) {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Thời gian kết thúc làm việc không hợp lệ!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (model.RelaxStartTime == null || model.RelaxStartTime.trim() === "") {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn chưa nhập thời gian bắt đầu nghỉ!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (!ValidateTime(model.RelaxStartTime)) {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Thời gian bắt đầu nghỉ không hợp lệ!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (model.RelaxEndTime == null || model.RelaxEndTime.trim() === "") {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn chưa nhập thời gian kết thúc nghỉ!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (!ValidateTime(model.RelaxEndTime)) {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Thời gian kết thúc nghỉ không hợp lệ!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        $('#processing').show();
        $.ajax({
            type: "POST",
            url: '/hrm/ShiftWork/Save',
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
                            CloseWindowModal();
                            GridCallback('#grdMain');
                        }
                    } //
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