$(document).ready(function () {
    $('#ModalTask').click(function () {
        InitChildOfChildWindowModal('/hrm/WorkPlan/TaskSearch', 800, 520, 'Danh sách công việc hệ thống ', false);
    }); 
    $('#ModalTaskMission').click(function () {
        InitChildOfChildWindowModal('/hrm/WorkPlan/TaskMission', 800, 520, 'Danh sách công việc nhiệm vụ ', false);
    });
    $("#FromDate2,#ToDate2").kendoDatePicker({
        dateInput: true,
        format: "{0:dd/MM/yyyy}"
    });
    $("#Quantity").kendoNumericTextBox({
        format:"{0:###,###}",
        min: 1,
        step: 1
    });
    $('#btnSaveAdd').click(function () {
        var model = {
            TaskId: $('#TaskId').val(),
            WorkPlanId: workPlanId,
            WorkPlanDetailId: workPlanDetailId,
            TaskCode: $('#TaskCode').val(),
            TaskName: $('#TaskName').val(),
            FromDate: $('#FromDate2').data('kendoDatePicker').value(),
            ToDate: $('#ToDate2').data('kendoDatePicker').value(),
            Description: $('#Description2').val(),
            Quantity : $("#Quantity").data('kendoNumericTextBox').value(),
            Status: status
        };
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
        if (model.Quantity === null ) {
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
                content: "Ngày bắt đầu không nhỏ hơn ngày bắt đầu kế hoạch!",
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
                content: "Ngày hoàn thành không lớn hơn ngày hoàn thành kế hoạch!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        $('#processing').show();
        $.ajax({
            type: "POST",
            url: '/hrm/WorkPlan/SaveDetail',
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
                            GridCallback('#grdWorkPlanDetail');
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