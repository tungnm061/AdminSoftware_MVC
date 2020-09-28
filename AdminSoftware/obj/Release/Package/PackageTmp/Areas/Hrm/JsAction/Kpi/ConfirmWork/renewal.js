$(document).ready(function () {
    $("#ToDate").kendoDatePicker({
        dateInput: true,
        format: "dd/MM/yyyy"
    });
    $("#btnSaveRenewal").click(function () {
        if ($("#ToDate").data("kendoDatePicker").value() === null)  {
            $.msgBox({
                title: "Hệ thống",
                type: "error",
                content: "Bạn phải chọn ngày gia hạn!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        $('#processing').show();
        $.ajax({
            type: 'POST',
            url: '/hrm/ConfirmWork/SaveRenewal',
            data: JSON.stringify({
                toDate: $("#ToDate").data("kendoDatePicker").value(),
                id: $("#WorkDetailId").val(),
                workType : $("#WorkType").val()
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
                            CloseWindowModal();
                            GridCallback('#grdMain');
                        }
                    });
                }
            }
        });



    });

});