$(document).ready(function () {

    $("#ModalAccusedBy").click(function () {
        InitChildWindowModal('/hrm/Complain/Employee', 800, 535, "Tìm kiếm nhân viên");
    });
    $('#btnSave').click(function () {
        var model = {
            ComplainId: window.complainId,
            Status: window.statusComplain,
            AccusedBy: $('#AccusedBy').val(),
            CreateBy: createBy,
            CreateDate: createDate,
            Description: $('#Description').val()
        };
        if (model.AccusedBy == null || model.AccusedBy.trim() === "") {
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
        if (model.Description == null || model.Description.trim() === "") {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn chưa nhập hành vi!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        $('#processing').show();
        $.ajax({
            type: "POST",
            url: '/hrm/Complain/Save',
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
