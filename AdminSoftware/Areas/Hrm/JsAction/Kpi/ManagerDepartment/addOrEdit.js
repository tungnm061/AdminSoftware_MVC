
$(document).ready(function () {
    $("#FromDate,#ToDate").kendoDatePicker({
        dateInput: true,
        format: "dd/MM/yyyy"

    });

    $("#grdWorkPlanDetail").kendoGrid({
        dataSource: {
            data: workPlanDetails,
            schema: {
                model: {
                    id: "WorkPlanDetailId",
                    fields: {
                        FromDate: { type: 'date' },
                        ToDate: { type: 'date' }
                    }
                }
            },
            pageSize: 10000,
            serverPaging: false,
            serverFiltering: false
        },
        height: 370,
        filterable: false,
        sortable: true,
        pageable: false,
        selectable: 'row',
        columns: [
            {
                title: " Công việc",
                width: 200,
                template: '#= TaskCode +" - " + TaskName #'


            },
            {
                field: "UsefulHourTask",
                title: "Giờ hữu ích QĐ",
                width: 120
            },
            {
                field: "FromDate",
                title: "Ngày bắt đầu",
                format: "{0:dd/MM/yyyy}",
                width :140

            },
            {
                field: "ToDate",
                title: "Ngày hoàn thành",
                format: "{0:dd/MM/yyyy}",
                width: 140
            },
            {
                field: "Description",
                title: "Mô tả công việc",
                width: 300
            }

        ]
    });



    $('#btnApproved').click(function () {
        $('#processing').show();
        $.ajax({
            type: 'POST',
            url: '/hrm/ManagerDepartment/Save',
            dataType: "json",
            data: JSON.stringify({
                id: window.workPlanId,
                action : 1
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
                            GridCallback('#grdMain');
                            CloseWindowModal();
                        }
                    });
                }
            }
        });
    });
    $('#btnCancel').click(function () {
        $('#processing').show();
        $.ajax({
            type: 'POST',
            url: '/hrm/ManagerDepartment/Save',
            dataType: "json",
            data: JSON.stringify({
                id: window.workPlanId,
                action: 2
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
                            GridCallback('#grdMain');
                            CloseWindowModal();
                        }
                    });
                }
            }
        });
    });


});