var dataSource = [
    { text: "Giao việc cho nhân viên", value: "3" }
];
$(document).ready(function () {
    $("#Quantity").kendoNumericTextBox({
        format: "{0:###,###}",
        min: 1,
        step: 1
    });
    //$("#TypeAssignWork").kendoDropDownList(
    //{
    //    dataTextField: "text",
    //    dataValueField: "value",
    //    dataSource: dataSource
    //});

    $("#FromDate,#ToDate").kendoDatePicker({
        dateInput: true,
        format: "dd/MM/yyyy"
    });
    $("#ModalTaskId").click(function () {
        InitChildWindowModal('/hrm/Home/TaskSearch', 800, 520, "Danh sách công việc");
    });
    $("#ModalEmployeeId").click(function () {
        var action = '3';
        //if (action === "") {
        //    $.msgBox({
        //        title: "Hệ thống",
        //        type: "error",
        //        content: "Bạn chưa chọn giao việc cho ai!",
        //        buttons: [{ value: "Đồng ý" }],
        //        success: function () {
        //        }
        //    });
        //    return;
        //}
        InitChildWindowModal('/hrm/Home/Employees?id=' + action, 800, 550, "Tìm kiếm nhân viên");
    });
    $('#btnSave').click(function () {
        var model = {
            AssignWorkId: window.assignWorkId,
            TaskId: $('#TaskId').val(),
            FromDate: $('#FromDate').data('kendoDatePicker').value(),
            ToDate: $('#ToDate').data('kendoDatePicker').value(),
            AssignBy: $('#UserId').val(),
            Quantity: $('#Quantity').data('kendoNumericTextBox').value(),
            DirectorFollowBy: null,
            DepartmentFollowBy: null,
            Status: statusAssignWork,
            Description: $('#Description').val()
        };
        if (model.TaskId == null || model.TaskId.trim() === "") {
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
        if (model.ToDate < model.FromDate) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Ngày hoàn thành phải lớn hơn bằng ngày bắt đầu!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.AssignBy === '0') {
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
        $('#processing').show();
        $.ajax({
            type: "POST",
            url: '/hrm/Home/SaveAssignWork',
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
                            GridCallback('#grdMainGiaoViec');
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
