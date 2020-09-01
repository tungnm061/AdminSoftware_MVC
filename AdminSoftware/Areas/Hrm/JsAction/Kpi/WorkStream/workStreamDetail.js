$(document).ready(function () {
    $('#ModalTaskDetail').click(function () {
        $("#CheckTaskSearch").val("2");
        InitChildOfChildWindowModal('/hrm/WorkStream/TaskSearch', 800, 520, 'Danh sách công việc ', false);
    });
    $("#FromDate2,#ToDate2").kendoDatePicker({
        dateInput: true,
        format: "{0:dd/MM/yyyy}"
    });
    $("#Quantity").kendoNumericTextBox({
        format: "{0:###,###}",
        min: 1,
        step: 1
    });
    $('#btnSaveAdd').click(function () {
        var model = {
            TaskId: $('#TaskId2').val(),
            WorkStreamId: $('#WorkStreamId').val(),
            WorkStreamDetailId: workStreamDetailId,
            FromDate: $('#FromDate2').data('kendoDatePicker').value(),
            ToDate: $('#ToDate2').data('kendoDatePicker').value(),
            Description: $('#Description2').val(),
            Status: $('#Status').val(),
            CreateBy: createby,
            CreateDate: createdate,
            IsDefault: isDefault,
            TaskName: $('#TaskNameModel').val(),
            Quantity: $("#Quantity").data('kendoNumericTextBox').value(),
            TaskCode: $('#TaskCodeModel').val()
        };
        if (model.Quantity === 0 || model.Quantity === null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn số lượng!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.TaskId === 0 || model.TaskId.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn công việc!",
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
                content: "Bạn chưa nhập ngày kết thúc!",
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
                content: "Bạn chưa nhập ngày bắt đầu!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.FromDate > model.ToDate) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Ngày bắt đầu phải nhỏ hơn ngày hoàn thành!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.FromDate < $('#FromDate').data('kendoDatePicker').value()) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Ngày bắt đầu không nhỏ hơn ngày bắt đầu công việc nhóm!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.ToDate > $('#ToDate').data('kendoDatePicker').value()) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Ngày hoàn thành không lớn hơn ngày hoàn thành công việc nhóm!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        $('#processing').show();
        $.ajax({
            type: "POST",
            url: '/hrm/WorkStream/SaveDetail',
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
                            CloseChildWindowModal();
                            GridCallback('#grdWorkStreamDetail');
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
    //

});