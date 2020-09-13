$(document).ready(function () {
    $('#ParentId').kendoDropDownTree({
        dataSource: {
            type: 'json',
            data: departmentTrees,
            schema: {
                model: {
                    id: "value",
                    children: "ChildModels"
                }
            }
        },
        dataTextField: "text",
        dataValueField: "value",
        clearButton: true,
        placeholder: "Chọn nhóm cha",
        height: "300px"
});
    $('#btnSave').click(function () {
        var model = {
            DepartmentId: departmentId,
            ParentId: $('#ParentId').data('kendoDropDownTree').value() === "0" ? null : $('#ParentId').data('kendoDropDownTree').value(),
            DepartmentName: $('#DepartmentName').val(),
            DepartmentCode: $('#DepartmentCode').val(),
            Description: $('#Description').val(),
            IsActive: $('#IsActive').is(':checked')
        };
        if (model.DepartmentCode == null || model.DepartmentCode.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa nhập mã phòng ban!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.DepartmentName == null || model.DepartmentName.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa nhập tên phòng ban!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        $('#processing').show();
        $.ajax({
            type: "POST",
            url: '/hrm/Department/Save',
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
                            TreeListCallback('#treeMain');
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