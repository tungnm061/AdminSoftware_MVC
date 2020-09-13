
$(document).ready(function () {
    $('#FromDepartmentName').val($("#DepartmentNamePost").val());
    $('#FromPositionName').val($("#PostitonNamePost").val());
    $('#ToDepartmentId').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: departments,
        optionLabel: " "
    });
    $('#ToPositionId').kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: positions,
        optionLabel: " "
    });
    $('#btnUploadJobChangeFile').click(function () {
        $('#UploadJobChangeFile').click();
    });
    $('#UploadJobChangeFile').change(function () {
        var data = new FormData();
        var files = $("#UploadJobChangeFile").get(0).files;
        if (files.length > 0) {
            data.append("JobChangeFile", files[0]);
        }
        $('#processing').show();
        $.ajax({
            url: '/hrm/Employee/UploadJobChangeFile',
            type: "POST",
            processData: false,
            contentType: false,
            data: data,
            success: function (response) {
                $('#processing').hide();
                if (response.Status === 1) {
                    $.msgBox({
                        title: "Hệ thống",
                        type: "info",
                        content: response.Message,
                        buttons: [{ value: "Đồng ý" }],
                        success: function () {
                            $('#JobChangeFile').attr('src', response.Url);
                        }
                    });
                } else {
                    $.msgBox({
                        title: "Hệ thống",
                        type: "error",
                        content: response.Message,
                        buttons: [{ value: "Đồng ý" }],
                        success: function () {
                        }
                    });
                }
            },
            error: function (er) {
                $('#processing').hide();
                $.msgBox({
                    title: "Hệ thống",
                    type: "error",
                    content: er,
                    buttons: [{ value: "Đồng ý" }],
                });
            }
        });
    });
    $('#btnRemoveJobChangeFile').click(function () {
        var filePath = $('#JobChangeFile').attr('src');
        $('#processing').show();
        $.ajax({
            type: 'POST',
            url: '/hrm/Employee/DeleteJobChangeFile',
            data: JSON.stringify({ filePath: filePath }),
            contentType: 'application/json;charset=utf-8',
            success: function (response) {
                $('#processing').hide();
                if (response.Status === 1) {
                    $.msgBox({
                        title: "Hệ thống",
                        type: "info",
                        content: response.Message,
                        buttons: [{ value: "Đồng ý" }],
                        success: function () {
                            $('#JobChangeFile').attr('src', response.Url);
                        }
                    });
                } else {
                    $.msgBox({
                        title: "Hệ thống",
                        type: "error",
                        content: response.Message,
                        buttons: [{ value: "Đồng ý" }],
                        success: function () {
                        }
                    });
                }
            }
        });
    });
    $('#btnSave').click(function () {
        var model = {
            JobChangeId: jobChangeId,
            JobChangeCode: $('#JobChangeCode').val(),
            EmployeeId: $('#EmployeeIdPost').val(),
            FromDepartmentId: $('#DepartmentIdPost').val(),
            ToDepartmentId: $('#ToDepartmentId').data('kendoDropDownList').value(),
            FromPositionId: $('#PostitonIdPost').val(),
            ToPositionId: $('#ToPositionId').data('kendoDropDownList').value(),
            JobChangeNumber: $('#JobChangeNumber').val(),
            JobChangeFile:$('#JobChangeFile').val(),
            Description: $('#Description').val(),
            Reason: $('#Reason').val()
        }
        if (model.JobChangeCode == null || model.JobChangeCode.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Chưa có số phiếu!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (model.EmployeeId == null || model.EmployeeId.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn phải chọn nhân viên!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (model.FromDepartmentId == null || model.FromDepartmentId.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Không có thông tin phòng cũ!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (model.ToDepartmentId == null || model.ToDepartmentId.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn phải chọn phòng mới!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (model.Reason == null || model.Reason.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn phải nhập lý do!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        $('#processing').show();
        $.ajax({
            type: "POST",
            url: '/hrm/Employee/SaveJobChange',
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
                            CloseChildWindowModal();
                            GridCallback('#grdMainJobChange');
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