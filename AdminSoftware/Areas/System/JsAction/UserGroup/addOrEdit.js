$(document).ready(function () {
    $('#btnSave').click(function () {
        var model = {
            GroupName: $('#GroupName').val(),
            GroupCode: $('#GroupCode').val(),
            Description: $('#Description').val(),
            UserGroupId: userGroupId
        }
        if (model.GroupCode == null || model.GroupCode.trim() === "") {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn chưa nhập mã nhóm!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        if (model.GroupName == null || model.GroupName.trim() === "") {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn chưa nhập tên nhóm!",
                buttons: [{ value: "Đồng ý" }]
            });
            return;
        }
        $('#processing').show();
        $.ajax({
            type: "POST",
            url: '/system/UserGroup/Save',
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