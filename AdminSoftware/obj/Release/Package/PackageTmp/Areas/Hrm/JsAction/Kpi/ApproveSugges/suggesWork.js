
$(document).ready(function () {
    $("#btnApproved").click(function () {
        $('#processing').show();
        $.ajax({
            type: 'POST',
            url: '/hrm/ApproveSugges/Save',
            data: JSON.stringify({
                id: window.workDetailId,
                type: workType,
                action: 7
            }),
            contentType: 'application/json;charset=utf-8',
            success: function (response) {
                $('#processing').hide();
                if (response.Status === 0) {
                    $.msgBox({
                        title: "Hệ thống",
                        type: "error",
                        content: response.Message,
                        buttons: [{ value: "Đồng ý" }],
                        success: function () {
                        }
                    });
                } else {
                    $.msgBox({
                        title: "Hệ thống",
                        type: "info",
                        content: response.Message,
                        buttons: [{ value: "Đồng ý" }],
                        success: function () {
                            GridCallback('#grdMain');
                            CloseWindowModal();
                        }
                    });
                }
            }
        });
    });
    $("#btnCancel").click(function () {
        $('#processing').show();
        $.ajax({
            type: 'POST',
            url: '/hrm/ApproveSugges/Save',
            data: JSON.stringify({
                id: window.workDetailId,
                type : workType,
                action: 8
            }),
            contentType: 'application/json;charset=utf-8',
            success: function (response) {
                $('#processing').hide();
                if (response.Status === 0) {
                    $.msgBox({
                        title: "Hệ thống",
                        type: "error",
                        content: response.Message,
                        buttons: [{ value: "Đồng ý" }],
                        success: function () {
                        }
                    });
                } else {
                    $.msgBox({
                        title: "Hệ thống",
                        type: "info",
                        content: response.Message,
                        buttons: [{ value: "Đồng ý" }],
                        success: function () {
                            GridCallback('#grdMain');      
                            CloseWindowModal();
                        }
                    });
                }
            }
        });
    });
});