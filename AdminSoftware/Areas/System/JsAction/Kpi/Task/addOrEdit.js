$(document).ready(function () {
    $("#UsefulHours").kendoNumericTextBox({
        format: "{0:###,###.##}",
        min : 0
    });
    $("#CategoryKpiId").kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: categoryKpis,
        optionLabel: "Chọn nhóm Kpi",
        filter: 'startswith'
    });
    $("#WorkPointConfigId").kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: workPointConfigs,
        optionLabel: "Chọn điểm công việc"
    }); 
    $("#CalcType").kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: calcType1
    });
    $('#btnSave').click(function () {
        var model = {
            TaskId: taskId,
            CategoryKpiId: $('#CategoryKpiId').data('kendoDropDownList').value(),
            TaskName: $('#TaskName').val(),
            TaskCode: $('#TaskCode').val(),
            CalcType: $('#CalcType').data('kendoDropDownList').value(),
            UsefulHours: $('#UsefulHours').data('kendoNumericTextBox').value(),
            WorkPointConfigId: $('#WorkPointConfigId').data('kendoDropDownList').value(),
            IsSystem: isSystem,
            CreateBy: createBy,
            CreateDate : createDate,
            Description: $('#Description').val(),
            Frequent: $('#Frequent').is(":checked"),
            GroupName: $('#GroupName').val()
        };
        if (model.GroupName == null || model.GroupName.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa nhập nhóm công việc!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.TaskName == null || model.TaskName.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa nhập tên công việc!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.CategoryKpiId == null || model.CategoryKpiId.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn nhóm Kpi!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.TaskCode == null || model.TaskCode.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa nhập mã công việc",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.CalcType == null || model.CalcType.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn thể thức tính",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.UsefulHours == null) {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa nhập giờ hữu ích",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.WorkPointConfigId == null || model.WorkPointConfigId.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn điểm CV",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        $('#processing').show();
        $.ajax({
            type: "POST",
            url: '/system/Task/Save',
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