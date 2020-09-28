$(document).ready(function () {
    $("#FromDate,#ToDate").kendoDatePicker({
        dateInput: true,
        format: "dd/MM/yyyy"
    });
    $("#ModalTaskId").click(function () {
        InitChildWindowModal('/hrm/SuggesWork/TaskSearch', 800, 520, "Danh sách công việc");
    });
    $("#Quantity").kendoNumericTextBox({
        format: "{0:###,###}",
        min: 1,
        step: 1
    });
    $('#btnSave').click(function () {
        var model = {
            SuggesWorkId: suggesWorkId,
            TaskId: $('#TaskId').val(),
            Status: status,
            FromDate: $('#FromDate').data('kendoDatePicker').value(),
            ToDate: $('#ToDate').data('kendoDatePicker').value(),
            CreateBy: createBy,
            CreateDate: createDate,
            Quantity: $("#Quantity").data('kendoNumericTextBox').value(),
            Description: $('#Description').val()
        };
        if (model.Quantity === 0 || model.Quantity=== null) {
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
        if (model.TaskId == null || model.TaskId.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn phải chọn công việc !",
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
                content: "Ngày hoàn thành phải lớn hơn bằng ngày bắt đầu",
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
                content: "Bạn chưa nhập ngày bắt đầu",
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
                content: "Bạn chưa nhập ngày hoàn thành",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        $('#processing').show();
        $.ajax({
            type: "POST",
            url: '/hrm/SuggesWork/Save',
            data: JSON.stringify({
                model: model
            }),
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