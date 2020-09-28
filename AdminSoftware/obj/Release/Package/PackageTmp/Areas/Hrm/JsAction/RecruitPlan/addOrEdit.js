$(document).ready(function () {
    $("#ChanelIds").kendoMultiSelect({
        placeholder: "Chọn kênh tuyển dụng",
        dataTextField: "text",
        dataValueField: "value",
        dataSource: recruitChanels
    });
    $('#FromDate,#ToDate').kendoDatePicker({
        format:'{0:dd/MM/yyyy}'
    });
    $('#Quantity').kendoNumericTextBox({
        min: 1,
        format:'{0:###,###}'
    });
    $('#DepartmentId').kendoDropDownList({
        dataTextField: 'text',
        dataValueField: 'value',
        dataSource: departments,
        filter: "startswith"
    });
    $('#PositionId').kendoDropDownList({
        dataTextField: 'text',
        dataValueField: 'value',
        dataSource: positions,
        filter: "startswith"
    });
    $('#btnSave').click(function () {
        var model = {
            RecruitPlanId: recruitPlanId,
            RecruitPlanCode: $('#RecruitPlanCode').val(),
            Title: $('#Title').val(),
            DepartmentId: $('#DepartmentId').data('kendoDropDownList').value(),
            PositionId: $('#PositionId').data('kendoDropDownList').value(),
            Quantity: $('#Quantity').data('kendoNumericTextBox').value(),
            FromDate: $('#FromDate').data('kendoDatePicker').value(),
            ToDate: $('#ToDate').data('kendoDatePicker').value(),
            Requirements: $('#Requirements').val(),
            ChanelIds: $('#ChanelIds').data('kendoMultiSelect').value().toString(),
            Description:$('#Description').val(),
            IsActive: $('#IsActive').is(":checked")
        }
        if (model.RecruitPlanCode == null || model.RecruitPlanCode.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Không có mã kế hoạch!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (model.Title == null || model.Title.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa nhập kế hoạch!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (model.DepartmentId == null || model.DepartmentId.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn phòng ban!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (model.PositionId == null || model.PositionId.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn vị trí!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (model.Quantity == null || model.Quantity<= 0) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Số lượng tuyển dụng không hợp lệ!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (model.FromDate == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn ngày bắt đầu!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (model.ToDate == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn ngày kết thúc!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (model.ToDate< model.FromDate) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Ngày kết thúc không được nhỏ hơn ngày bắt đầu!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (model.ChanelIds == null || model.ChanelIds.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn kênh tuyển dụng!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        $('#processing').show();
        $.ajax({
            type: "POST",
            url: '/hrm/RecruitPlan/Save',
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