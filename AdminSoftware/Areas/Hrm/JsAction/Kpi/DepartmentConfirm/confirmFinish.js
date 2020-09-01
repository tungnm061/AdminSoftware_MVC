
$(document).ready(function () {
    $("#UsefulHours,#UsefulHoursTask").kendoNumericTextBox({
        format: "{0:###,###.##}"
    });
    //$("#WorkPointType").kendoDropDownList({
    //    dataTextField: "text",
    //    dataValueField: "value",
    //    dataSource: workPointType,
    //    optionLabel: "Chọn tiêu chí"
    //});
    $("#grdMainWorkingNote").kendoGrid({
        dataSource: {
            data: window.workingNotes,
            schema: {
                model: {
                    id: "WorkingNoteId",
                    fields: {
                        CreateDate: { type: 'date' }
                    }
                }
            },
            pageSize: 100,
            serverPaging: false,
            serverFiltering: false
        },
        height: 225,
        filterable: false,
        sortable: true,
        pageable: false,
        selectable: 'row',
        columns: [
            {
                field: "CreateDate",
                title: "Thời gian",
                format: "{0:dd/MM/yyyy hh:mm:ss tt}",
                width: 140
            },
            {
                field: "TextNote",
                title: "Công việc thực hiện",
                width: 500
            }

        ]
    });
    $("#btnSaveCancel").click(function () {
        $('#processing').show();
        $.ajax({
            type: 'POST',
            url: '/hrm/DepartmentConfirm/Save',
            data: JSON.stringify({
                id: window.workDetailId,
                action: 2,
                workType: window.workType,
                workPointType: $("#WorkPointType").data("kendoDropDownList").value()
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
    $("#btnSaveAdd").click(function () {

        $('#processing').show();
        $.ajax({
            type: 'POST',
            url: '/hrm/DepartmentConfirm/Save',
            data: JSON.stringify({
                id: window.workDetailId,
                action: 1,
                workType: window.workType,
                workPointType: "A"
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