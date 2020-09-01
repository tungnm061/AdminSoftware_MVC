$(document).ready(function () {
    $('#PraiseDisciplineDate').kendoDatePicker({
        format: "dd/MM/yyyy",
        dateInput: true
    });

    $('#btnSave').click(function() {
        var model = {
            PraiseDisciplineId: praiseDisciplineId,
            PraiseDisciplineCode:$('#PraiseDisciplineCode').val(),
            Title:$('#Title').val(),
            DecisionNumber:$('#DecisionNumber').val(),
            PraiseDisciplineDate:$('#PraiseDisciplineDate').data('kendoDatePicker').value(),
            Formality:$('#Formality').val(),
            Reason:$('#Reason').val(),
            Description:$('#Description').val()
        }
        if (model.Title == null || model.Title.trim() === "") {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn phải nhập quyết định!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.PraiseDisciplineDate == null) {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn phải nhập ngày quyết định!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.Formality == null || model.Formality.trim() ==="") {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn phải nhập hình thức!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        if (model.Reason == null || model.Reason.trim() === "") {
            $.msgBox({
                title: "Hệ thống ERP",
                type: "error",
                content: "Bạn phải nhập lý do!",
                buttons: [{ value: "Đồng ý" }],
                success: function () {
                }
            });
            return;
        }
        //if ($('#grdPraiseDisciplineDetail').data("kendoGrid").dataSource.total() <= 0) {
        //    $.msgBox({
        //        title: "Hệ thống ERP",
        //        type: "error",
        //        content: "Bạn phải chọn nhân viên!",
        //        buttons: [{ value: "Đồng ý" }],
        //        success: function () {
        //        }
        //    });
        //    return;
        //}
        $('#processing').show();
        $.ajax({
            type: 'POST',
            url: '/hrm/Employee/SaveDiscipline',
            dataType: "json",
            data: JSON.stringify({
                model: model,
                employeeId : $('#EmployeeIdPost').val()
            }),
            contentType: 'application/json;charset=utf-8',
            success: function (response) {
                $('#processing').hide();
                if (response.Status === 0) {
                    $.msgBox({
                        title: "Hệ thống ERP",
                        type: "error",
                        content: response.Message,
                        buttons: [{ value: "Đồng ý" }],
                        success: function () {
                        }
                    });
                } else {
                    $.msgBox({
                        title: "Hệ thống ERP",
                        type: "info",
                        content: response.Message,
                        buttons: [{ value: "Đồng ý" }],
                        success: function () {
                            GridCallback('#grdMainDiscipline');
                            CloseChildWindowModal();
                        }
                    });
                }
            }
        });
    });
});