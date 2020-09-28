$(document).ready(function () {
    function widgetChange() {
        $('#processing').show();
        if ($('#EmployeeId').data('kendoDropDownList').value() == null || $('#EmployeeId').data('kendoDropDownList').value().trim() === "") {
            $('#FullName').val('');
            $('#Email').val(''),
            $('#PhoneNumber').val('');
            $('#processing').hide();
        } else {
            $.ajax({
                type: "POST",
                url: '/system/Account/Employee',
                data: JSON.stringify({ id: $('#EmployeeId').data('kendoDropDownList').value() }),
                contentType: 'application/json; charset=utf-8',
                success: function (response) {
                    $('#FullName').val(response.FullName);
                    $('#Email').val(response.Email),
                    $('#PhoneNumber').val(response.PhoneNumber);
                    $('#processing').hide();
                }
            });
        }
    };
    //$("#ModuleGroupId").kendoDropDownList({
    //    dataTextField: "text",
    //    dataValueField: "value",
    //    dataSource: moduleGroups,
    //    optionLabel: "Chọn phân hệ"
    //});
    $("#EmployeeId").kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: employees,
        optionLabel: "Chọn nhân viên",
        change: widgetChange,
        filter: 'startswith'
    });
    $("#RoleId").kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: roles
    });
    $("#UserGroupId").kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: userGroups
    });

    $('#btnSave').click(function () {
        var model = {
            UserId: userId,
            RoleId: $('#RoleId').data('kendoDropDownList').value(),
            UserGroupId: $('#UserGroupId').data('kendoDropDownList').value(),
            EmployeeId: $('#EmployeeId').data('kendoDropDownList').value(),
            //ModuleGroupId: $('#ModuleGroupId').data('kendoDropDownList').value(),
            UserName: $('#UserName').val(),
            FullName: $('#FullName').val(),
            Description: $('#Description').val(),
            Email: $('#Email').val(),
            //PhoneNumber: $('#PhoneNumber').val(),
            IsActive: $('#IsActive').is(":checked"),
            Password: parseInt(userId) <= 0 ? $('#Password').val() : "1"
        };
        if (model.UserName == null || model.UserName.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa nhập tên đăng nhập!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.FullName == null || model.FullName.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa nhập họ tên!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.RoleId == null || model.RoleId.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa chọn quyền!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        //if (parseInt(model.RoleId) !== 1 && (model.ModuleGroupId == null || model.ModuleGroupId.trim() === "")) {
        //    $.msgBox({
        //        title: "Hệ thống",
        //        type: "error",
        //        content: "Bạn chưa chọn phân hệ!",
        //        buttons: [{ value: "Đồng ý" }],
        //        success: function () {
        //        }
        //    });
        //    return;
        //}
        if (parseInt(userId) <= 0) {
            if (model.Password == null || model.Password.trim() === "" || model.Password.length < 6) {
                $.msgBox({
                    title: "Hệ thống",
                    type: "error",
                    content: "Mật khẩu không được để trống và phải lớn hơn 6 ký tự!",
                    buttons: [{ value: "Đồng ý" }],
                    success: function () {
                    }
                });
                return;
            }
            if (model.Password !== $('#ConfirmPassword').val()) {
                $.msgBox({
                    title: "Hệ thống",
                    type: "error",
                    content: "Mật khẩu xác nhận không đúng!",
                    buttons: [{ value: "Đồng ý" }],
                    success: function () {
                    }
                });
                return;
            }
        }
        $('#processing').show();
        $.ajax({
            type: "POST",
            url: '/system/Account/Save',
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